//
// TlsContextWrapper.cs
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
using Mono.Security.Interface;
using Mono.Security.Protocol.NewTls;

namespace Mono.Security.Providers.NewTls
{
	class TlsContextWrapper : SecretParameters, IMonoTlsContext
	{
		TlsConfiguration config;
		TlsContext context;

		public TlsContextWrapper (TlsConfiguration config)
		{
			this.config = config;
		}

		public bool IsValid {
			get { return context != null && context.IsValid; }
		}

		public void Initialize (bool serverMode)
		{
			if (context != null)
				throw new InvalidOperationException ();
			context = new TlsContext (config, serverMode);
		}

		protected override void Clear ()
		{
			if (context != null) {
				context.Clear ();
				context = null;
			}
		}

		public void SetCertificate (Mono.Security.X509.X509Certificate certificate, System.Security.Cryptography.AsymmetricAlgorithm privateKey)
		{
			throw new NotImplementedException ();
		}

		public int GenerateNextToken (IBufferOffsetSize incoming, out IMultiBufferOffsetSize outgoing)
		{
			throw new NotImplementedException ();
		}

		public int EncryptMessage (ref IBufferOffsetSize incoming)
		{
			throw new NotImplementedException ();
		}

		public int DecryptMessage (ref IBufferOffsetSize incoming)
		{
			throw new NotImplementedException ();
		}

		public byte[] CreateCloseNotify ()
		{
			throw new NotImplementedException ();
		}

		public System.Security.Cryptography.X509Certificates.X509Certificate2 GetRemoteCertificate (out Mono.Security.X509.X509CertificateCollection remoteCertificateStore)
		{
			throw new NotImplementedException ();
		}

		public bool VerifyRemoteCertificate ()
		{
			throw new NotImplementedException ();
		}

		public bool HasCredentials {
			get {
				throw new NotImplementedException ();
			}
		}

		public Exception LastError {
			get {
				throw new NotImplementedException ();
			}
		}

		public bool ReceivedCloseNotify {
			get {
				throw new NotImplementedException ();
			}
		}

		public void Dispose ()
		{
			throw new NotImplementedException ();
		}
	}
}

