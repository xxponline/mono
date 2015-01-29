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
using System;
using System.IO;
using System.Net;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using Mono.Security.Protocol.Tls;
using Mono.Net.Security;

namespace Mono.Security.Interface
{
	class MonoDefaultTlsProvider : MonoTlsProvider
	{
		public override bool SupportsHttps {
			get { return true; }
		}

		public override bool SupportsSslStream {
			get { return true; }
		}

		public override bool IsHttpsStream (Stream stream)
		{
			return stream is IMonoHttpsStream;
		}

		public override IMonoHttpsStream GetHttpsStream (Stream stream)
		{
			return (IMonoHttpsStream)stream;
		}

		public override IMonoHttpsStream CreateHttpsClientStream (
			Stream innerStream, X509CertificateCollection clientCertificates, HttpWebRequest request, byte[] buffer,
			CertificateValidationCallback2	validationCallback)
		{
			SslClientStream sslStream;
			sslStream = new HttpsClientStream (innerStream, request.ClientCertificates, request, buffer);

			if (validationCallback != null)
				sslStream.ServerCertValidation2 += validationCallback;

			return (IMonoHttpsStream)sslStream;
		}

		public override MonoSslStream CreateSslStream (
			Stream innerStream, bool leaveInnerStreamOpen,
			RemoteCertificateValidationCallback userCertificateValidationCallback,
			LocalCertificateSelectionCallback userCertificateSelectionCallback)
		{
			MonoSslStream sslStream;
			#if MOBILE
			sslStream = (MonoSslStream)(object)new MonoSslStreamImpl ();
			#else
			var obj = Activator.CreateInstance (Consts.AssemblySystem, "Mono.Net.Security.MonoSslStreamImpl");
			sslStream = (MonoSslStream)obj.Unwrap ();
			#endif

			sslStream.Initialize (innerStream, leaveInnerStreamOpen, userCertificateValidationCallback, userCertificateSelectionCallback);
			return sslStream;
		}
	}
}

