//
// MonoDefaultTlsProvider.cs
//
// Author:
//       Martin Baulig <martin.baulig@xamarin.com>
//
// Copyright (c) 2015 Xamarin, Inc.
//
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
//
// The above copyright notice and this permission notice shall be included in
// all copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
// THE SOFTWARE.
#if SECURITY_DEP

#if MONOTOUCH || MONODROID
using Mono.Security.Protocol.Tls;
#else
extern alias MonoSecurity;
using MonoSecurity::Mono.Security.Protocol.Tls;
using System.Reflection;
#endif

using System;
using System.IO;
using System.Net;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;

namespace Mono.Net.Security
{
	class MonoDefaultTlsProvider : IMonoTlsProvider
	{
		readonly Type implType;

		public MonoDefaultTlsProvider ()
		{
#if NET_2_1 && SECURITY_DEP
			implType = typeof (HttpsClientStream);
#else
			// HttpsClientStream is an internal glue class in Mono.Security.dll
			implType = Type.GetType ("Mono.Security.Protocol.Tls.HttpsClientStream, " + Consts.AssemblyMono_Security, false);

			if (implType == null) {
				string msg = "Missing Mono.Security.dll assembly. Support for SSL/TLS is unavailable.";
				throw new NotSupportedException (msg);
			}
#endif
		}

		public bool IsTlsStream (Stream stream)
		{
			throw new NotImplementedException ();
		}

		public IMonoHttpsStream CreateHttpsClientStream (Stream innerStream, X509CertificateCollection clientCertificates, HttpWebRequest request, byte[] buffer)
		{
			SslClientStream sslStream;
#if SECURITY_DEP
#if MONOTOUCH || MONODROID
			sslStream = new HttpsClientStream (innerStream, request.ClientCertificates, request, buffer);
#else
			object[] args = new object [4] {
				innerStream,
				request.ClientCertificates,
				request, buffer};
			sslStream = (SslClientStream) Activator.CreateInstance (implType, args);
#endif
#endif
			return new MonoHttpsStreamWrapper (implType, sslStream);
		}

		class MonoHttpsStreamWrapper : IMonoHttpsStream
		{
			SslClientStream sslStream;
			CertificateValidationCallback2 validationCallback;
#if !MONOTOUCH && !MONODROID
			PropertyInfo piTrustFailure;
#endif

			public MonoHttpsStreamWrapper (Type implType, SslClientStream sslStream)
			{
				this.sslStream = sslStream;

				sslStream.ServerCertValidation2 += (collection) => {
					if (validationCallback == null)
						return null;
					return validationCallback (collection);
				};

#if !MONOTOUCH && !MONODROID
				piTrustFailure = implType.GetProperty ("TrustFailure");
#endif
			}

			public Stream Stream {
				get { return sslStream; }
			}

			public CertificateValidationCallback2 ServerCertValidationCallback2 {
				get { return validationCallback; }
				set { validationCallback = value; }
			}

			public X509Certificate SelectedClientCertificate {
				get { return sslStream.SelectedClientCertificate; }
			}

			public X509Certificate ServerCertificate {
				get { return sslStream.ServerCertificate; }
			}

			public bool TrustFailure {
				get {
#if MONOTOUCH || MONODROID
					return sslStream.TrustFailure;
#else
					return (bool)piTrustFailure.GetValue (sslStream, null);
#endif
				}
			}
		}
	}
}
#endif

