//
// IMonoTlsProvider.cs
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
using XRemoteCertificateValidationCallback = System.Net.Security.RemoteCertificateValidationCallback;
using XLocalCertificateSelectionCallback = System.Net.Security.LocalCertificateSelectionCallback;

using Mono.Security.Protocol.Tls;
using Mono.Security.Interface;
#else
extern alias PrebuiltSystem;
extern alias MonoSecurity;

using XRemoteCertificateValidationCallback = PrebuiltSystem::System.Net.Security.RemoteCertificateValidationCallback;
using XLocalCertificateSelectionCallback = PrebuiltSystem::System.Net.Security.LocalCertificateSelectionCallback;

using MonoSecurity::Mono.Security.Protocol.Tls;
using MonoSecurity::Mono.Security.Interface;
#endif
#endif

using System;
using System.IO;
using System.Net;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;

namespace Mono.Net.Security
{
	interface IMonoTlsProvider
	{
		bool IsHttpsStream (Stream stream);

#if SECURITY_DEP
		IMonoHttpsStream GetHttpsStream (Stream stream);

		IMonoHttpsStream CreateHttpsClientStream (
			Stream innerStream, X509CertificateCollection clientCertificates,
			HttpWebRequest request, byte[] buffer,
			CertificateValidationCallback2 certValidationCallback);

		IMonoSslStream CreateSslStream (
			Stream innerStream, bool leaveInnerStreamOpen,
			XRemoteCertificateValidationCallback userCertificateValidationCallback,
			XLocalCertificateSelectionCallback userCertificateSelectionCallback);
#endif
	}
}
