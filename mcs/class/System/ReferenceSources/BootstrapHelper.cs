//
// BootstrapHelper.cs
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
using System.Threading;
using System.Threading.Tasks;
using System.Security.Authentication;
using System.Security.Authentication.ExtendedProtection;
using System.Security.Cryptography.X509Certificates;

//
// This file is only used during the bootstrap build, to bring a few
// APIs that will be used from the referencesource during the full
// build step.
//

namespace System.Net.Security
{
#if MONO_FEATURE_NEW_TLS && !SECURITY_DEP
	[Flags]
	public enum SslPolicyErrors
	{
		None = 0x0,
		RemoteCertificateNotAvailable = 0x1,
		RemoteCertificateNameMismatch = 0x2,
		RemoteCertificateChainErrors = 0x4
	}

	public delegate bool RemoteCertificateValidationCallback (object sender,X509Certificate certificate,X509Chain chain,SslPolicyErrors sslPolicyErrors);
	public delegate X509Certificate LocalCertificateSelectionCallback (object sender,string targetHost,X509CertificateCollection localCertificates,X509Certificate remoteCertificate,string[] acceptableIssuers);
#endif

#if !MONO_FEATURE_NEW_TLS || !SECURITY_DEP
	public enum EncryptionPolicy
	{
		// Prohibit null ciphers (current system defaults)
		RequireEncryption = 0,

		// Add null ciphers to current system defaults
		AllowNoEncryption,

		// Request null ciphers only
		NoEncryption
	}

	internal delegate bool RemoteCertValidationCallback(string host, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors);
	internal delegate X509Certificate LocalCertSelectionCallback(string targetHost, X509CertificateCollection localCertificates, X509Certificate remoteCertificate, string[] acceptableIssuers);
#endif

#if !SECURITY_DEP
	public partial class SslStream: AuthenticatedStream
	{
		public SslStream (Stream innerStream)
			: this (innerStream, false, null, null)
		{
		}

		public SslStream (Stream innerStream, bool leaveInnerStreamOpen)
			: this (innerStream, leaveInnerStreamOpen, null, null, EncryptionPolicy.RequireEncryption)
		{
		}

		public SslStream (Stream innerStream, bool leaveInnerStreamOpen, RemoteCertificateValidationCallback userCertificateValidationCallback)
			: this (innerStream, leaveInnerStreamOpen, userCertificateValidationCallback, null, EncryptionPolicy.RequireEncryption)
		{
		}

		public SslStream (Stream innerStream, bool leaveInnerStreamOpen, RemoteCertificateValidationCallback userCertificateValidationCallback, 
			LocalCertificateSelectionCallback userCertificateSelectionCallback)
			: this (innerStream, leaveInnerStreamOpen, userCertificateValidationCallback, userCertificateSelectionCallback, EncryptionPolicy.RequireEncryption)
		{
		}

		public SslStream (Stream innerStream, bool leaveInnerStreamOpen, RemoteCertificateValidationCallback userCertificateValidationCallback, 
			LocalCertificateSelectionCallback userCertificateSelectionCallback, EncryptionPolicy encryptionPolicy)
			: base (innerStream, leaveInnerStreamOpen)
		{
			throw new NotSupportedException ();
		}

		public virtual void AuthenticateAsClient (string targetHost)
		{
			throw new NotSupportedException ();
		}
		public virtual void AuthenticateAsClient (string targetHost, X509CertificateCollection clientCertificates, SslProtocols enabledSslProtocols, bool checkCertificateRevocation)
		{
			throw new NotSupportedException ();
		}
		public virtual IAsyncResult BeginAuthenticateAsClient (string targetHost, AsyncCallback asyncCallback, object asyncState)
		{
			throw new NotSupportedException ();
		}
		public virtual IAsyncResult BeginAuthenticateAsClient (
			string targetHost, X509CertificateCollection clientCertificates,
			SslProtocols enabledSslProtocols, bool checkCertificateRevocation,
			AsyncCallback asyncCallback, object asyncState)
		{
			throw new NotSupportedException ();
		}
		public virtual void EndAuthenticateAsClient (IAsyncResult asyncResult)
		{
			throw new NotSupportedException ();
		}
		public virtual void AuthenticateAsServer (X509Certificate serverCertificate)
		{
			throw new NotSupportedException ();
		}
		public virtual void AuthenticateAsServer (
			X509Certificate serverCertificate, bool clientCertificateRequired,
			SslProtocols enabledSslProtocols, bool checkCertificateRevocation)
		{
			throw new NotSupportedException ();
		}
		public virtual IAsyncResult BeginAuthenticateAsServer (X509Certificate serverCertificate, AsyncCallback asyncCallback, object asyncState)
		{
			throw new NotSupportedException ();
		}
		public virtual IAsyncResult BeginAuthenticateAsServer (
			X509Certificate serverCertificate, bool clientCertificateRequired,
			SslProtocols enabledSslProtocols, bool checkCertificateRevocation,
			AsyncCallback asyncCallback, object asyncState)
		{
			throw new NotSupportedException ();
		}
		public virtual void EndAuthenticateAsServer (IAsyncResult asyncResult)
		{
			throw new NotSupportedException ();
		}
		public TransportContext TransportContext {
			get {
				throw new NotSupportedException ();
			}
		}
		internal ChannelBinding GetChannelBinding(ChannelBindingKind kind)
		{
			throw new NotSupportedException ();
		}
		public virtual Task AuthenticateAsClientAsync (string targetHost)
		{
			throw new NotSupportedException ();
		}
		public virtual Task AuthenticateAsClientAsync (string targetHost, X509CertificateCollection clientCertificates, SslProtocols enabledSslProtocols, bool checkCertificateRevocation)
		{
			throw new NotSupportedException ();
		}
		public virtual Task AuthenticateAsServerAsync (X509Certificate serverCertificate)
		{
			throw new NotSupportedException ();
		}
		public virtual Task AuthenticateAsServerAsync (X509Certificate serverCertificate, bool clientCertificateRequired, SslProtocols enabledSslProtocols, bool checkCertificateRevocation)
		{
			throw new NotSupportedException ();
		}

		public override bool IsAuthenticated {
			get {
				throw new NotSupportedException ();
			}
		}
		public override bool IsMutuallyAuthenticated {
			get {
				throw new NotSupportedException ();
			}
		}
		public override bool IsEncrypted {
			get {
				throw new NotSupportedException ();
			}
		}
		public override bool IsSigned {
			get {
				throw new NotSupportedException ();
			}
		}
		public override bool IsServer {
			get {
				throw new NotSupportedException ();
			}
		}
		public virtual SslProtocols SslProtocol {
			get {
				throw new NotSupportedException ();
			}
		}
		public virtual bool CheckCertRevocationStatus {
			get {
				throw new NotSupportedException ();
			}
		}
		public virtual X509Certificate LocalCertificate {
			get {
				throw new NotSupportedException ();
			}
		}
		public virtual X509Certificate RemoteCertificate {
			get {
				throw new NotSupportedException ();
			}
		}
		public virtual CipherAlgorithmType CipherAlgorithm {
			get {
				throw new NotSupportedException ();
			}
		}
		public virtual int CipherStrength {
			get {
				throw new NotSupportedException ();
			}
		}
		public virtual HashAlgorithmType HashAlgorithm {
			get {
				throw new NotSupportedException ();
			}
		}
		public virtual int HashStrength {
			get {
				throw new NotSupportedException ();
			}
		}
		public virtual ExchangeAlgorithmType KeyExchangeAlgorithm {
			get {
				throw new NotSupportedException ();
			}
		}
		public virtual int KeyExchangeStrength {
			get {
				throw new NotSupportedException ();
			}
		}
		public override bool CanSeek {
			get {
				throw new NotSupportedException ();
			}
		}
		public override bool CanRead {
			get {
				throw new NotSupportedException ();
			}
		}
		public override bool CanTimeout {
			get {
				throw new NotSupportedException ();
			}
		}
		public override bool CanWrite {
			get {
				throw new NotSupportedException ();
			}
		}
		public override int ReadTimeout {
			get {
				throw new NotSupportedException ();
			}
			set {
				throw new NotSupportedException ();
			}
		}
		public override int WriteTimeout {
			get {
				throw new NotSupportedException ();
			}
			set {
				throw new NotSupportedException ();
			}
		}
		public override long Length {
			get {
				throw new NotSupportedException ();
			}
		}
		public override long Position {
			get {
				throw new NotSupportedException ();
			}
			set {
				throw new NotSupportedException ();
			}
		}
		public override void SetLength (long value)
		{
			throw new NotSupportedException ();
		}
		public override long Seek (long offset, SeekOrigin origin)
		{
			throw new NotSupportedException ();
		}
		public override void Flush ()
		{
			throw new NotSupportedException ();
		}
		public override int Read (byte[] buffer, int offset, int count)
		{
			throw new NotSupportedException ();
		}
		public void Write (byte[] buffer)
		{
			throw new NotSupportedException ();
		}
		public override void Write (byte[] buffer, int offset, int count)
		{
			throw new NotSupportedException ();
		}
		public override IAsyncResult BeginRead (byte[] buffer, int offset, int count, AsyncCallback asyncCallback, object asyncState)
		{
			throw new NotSupportedException ();
		}
		public override int EndRead (IAsyncResult asyncResult)
		{
			throw new NotSupportedException ();
		}
		public override IAsyncResult BeginWrite (byte[] buffer, int offset, int count, AsyncCallback asyncCallback, object asyncState)
		{
			throw new NotSupportedException ();
		}
		public override void EndWrite (IAsyncResult asyncResult)
		{
			throw new NotSupportedException ();
		}
	}
#endif
}

