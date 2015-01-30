//
// MonoTlsProvider.cs
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

namespace Mono.Security.Interface
{
#if INSIDE_SYSTEM
	internal
#else
	public
#endif
	delegate bool MonoRemoteCertificateValidationCallback (
		string host, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors);

#if INSIDE_SYSTEM
	internal
#else
	public
#endif
	delegate X509Certificate MonoLocalCertificateSelectionCallback (
		string targetHost, X509CertificateCollection localCertificates, X509Certificate remoteCertificate,
		string[] acceptableIssuers);

#if INSIDE_SYSTEM
	internal
#else
	public
#endif
	abstract class MonoTlsProvider
	{
		public abstract bool SupportsHttps {
			get;
		}

		public abstract bool SupportsSslStream {
			get;
		}

		public abstract bool SupportsMonoExtensions {
			get;
		}

		public abstract bool SupportsTlsContext {
			get;
		}

		public abstract bool IsHttpsStream (Stream stream);

#pragma warning disable 618

		public abstract IMonoHttpsStream GetHttpsStream (Stream stream);

		public abstract IMonoHttpsStream CreateHttpsClientStream (
			Stream innerStream, X509CertificateCollection clientCertificates, HttpWebRequest request, byte[] buffer,
			CertificateValidationCallback2	validationCallback);

#pragma warning restore

		public abstract MonoSslStream CreateSslStream (
			Stream innerStream, bool leaveInnerStreamOpen,
			RemoteCertificateValidationCallback userCertificateValidationCallback,
			LocalCertificateSelectionCallback userCertificateSelectionCallback);

		// Only available if @SupportsMonoExtensions is set.
		public abstract MonoSslStream CreateSslStream (
			Stream innerStream, bool leaveInnerStreamOpen,
			RemoteCertificateValidationCallback userCertificateValidationCallback,
			LocalCertificateSelectionCallback userCertificateSelectionCallback,
			MonoTlsSettings settings);

		public abstract IMonoTlsContext CreateTlsContext (
			string hostname, bool serverMode, TlsProtocols protocolFlags,
			X509Certificate serverCertificate, X509CertificateCollection clientCertificates,
			bool remoteCertRequired, bool checkCertName, bool checkCertRevocationStatus,
			EncryptionPolicy encryptionPolicy,
			MonoLocalCertificateSelectionCallback certSelectionDelegate,
			MonoRemoteCertificateValidationCallback remoteValidationCallback,
			MonoTlsSettings settings);
	}
}
