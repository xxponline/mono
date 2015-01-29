//
// MonoTlsProviderImpl.cs
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
#if MOBILE
using MSI = Mono.Security.Interface;
using TLS = Mono.Security.Protocol.Tls;
using XHttpWebRequest = System.Net.HttpWebRequest;
using XX509CertificateCollection = System.Security.Cryptography.X509Certificates.X509CertificateCollection;
#else
extern alias PrebuiltSystem;
extern alias MonoSecurity;
using MSI = MonoSecurity::Mono.Security.Interface;
using TLS = MonoSecurity::Mono.Security.Protocol.Tls;

using XRemoteCertificateValidationCallback = PrebuiltSystem::System.Net.Security.RemoteCertificateValidationCallback;
using XLocalCertificateSelectionCallback = PrebuiltSystem::System.Net.Security.LocalCertificateSelectionCallback;

using XHttpWebRequest = PrebuiltSystem::System.Net.HttpWebRequest;
using XX509CertificateCollection = PrebuiltSystem::System.Security.Cryptography.X509Certificates.X509CertificateCollection;
#endif

using System;
using System.IO;
using System.Net;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;

namespace Mono.Net.Security
{
	class MonoTlsProviderImpl : IMonoTlsProvider
	{
		MSI.MonoTlsProvider provider;

		public MonoTlsProviderImpl (MSI.MonoTlsProvider provider)
		{
			this.provider = provider;
		}

		public bool IsHttpsStream (Stream stream)
		{
			return provider.IsHttpsStream (stream);
		}

		public MSI.IMonoHttpsStream GetHttpsStream (Stream stream)
		{
			return provider.GetHttpsStream (stream);
		}

		public MSI.IMonoHttpsStream CreateHttpsClientStream (
			Stream innerStream, X509CertificateCollection clientCertificates,
			HttpWebRequest request, byte[] buffer,
			TLS.CertificateValidationCallback2 certValidationCallback)
		{
			var clientCertificates2 = (XX509CertificateCollection)(object)clientCertificates;
			var request2 = (XHttpWebRequest)(object)request;
			return provider.CreateHttpsClientStream (
				innerStream, clientCertificates2, request2, buffer, certValidationCallback);
		}

		public IMonoSslStream CreateSslStream (
			Stream innerStream, bool leaveInnerStreamOpen,
			XRemoteCertificateValidationCallback userCertificateValidationCallback,
			XLocalCertificateSelectionCallback userCertificateSelectionCallback)
		{
			var sslStream = (MonoSslStreamImpl)provider.CreateSslStream (
				innerStream, leaveInnerStreamOpen,
				userCertificateValidationCallback, userCertificateSelectionCallback);
			return sslStream.Impl;
		}
	}
}

#endif
