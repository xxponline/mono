//
// SSPIWrapper.cs
//
// Author:
//       Martin Baulig <martin.baulig@xamarin.com>
//
// Copyright (c) 2015 Xamarin Inc. (http://www.xamarin.com)
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

//
// When building System.dll, `MONO_INSIDE_SYSTEM' is defined.  We directly
// include the X509 sources, but pull TLS from the `MonoSecurity' extern alias.
//
// When building the tests, `MONO_SECURITY_ALIAS' is defined.  In this case,
// Mono.Security.dll contains the X509 sources, so we pull everything from the
// extern alias.
//
// MX = Mono.Security.X509.
// MSCX = System.Security.Cryptography.X509Certificates from Mono.Security.dll
// (if appropriate, otherwise from System.dll).
// SSCX = System.Security.Cryptography.X509Certificates from System.dll
//

#if MONO_INSIDE_SYSTEM || MONO_SECURITY_ALIAS
extern alias MonoSecurity;
using MX = MonoSecurity::Mono.Security.X509;

#if MONO_SECURITY_ALIAS
using MSCX = MonoSecurity::System.Security.Cryptography.X509Certificates;

#else
using MSCX = System.Security.Cryptography.X509Certificates;
#endif
#if MONO_FEATURE_NEW_TLS
using MonoSecurity::Mono.Security.Protocol.NewTls;
#endif
#else
using MX = Mono.Security.X509;

#if MONO_FEATURE_NEW_TLS
using Mono.Security.Protocol.NewTls;
#endif
#endif

using System.Runtime.InteropServices;
using System.Security.Authentication.ExtendedProtection;
using SSCX = System.Security.Cryptography.X509Certificates;

namespace System.Net.Security
{
    internal class SSPIInterface
    {
        #if MONO_FEATURE_NEW_TLS
        public TlsConfiguration Configuration
        {
            get;
            private set;
        }

        public SSPIInterface(TlsConfiguration config)
        {
            Configuration = config;
        }
        #endif
    }

    internal static class GlobalSSPI
    {
        internal static SSPIInterface Create(string hostname, bool serverMode, SchProtocols protocolFlags, SSCX.X509Certificate serverCertificate, SSCX.X509CertificateCollection clientCertificates,
                                        bool remoteCertRequired, bool checkCertName, bool checkCertRevocationStatus, EncryptionPolicy encryptionPolicy,
                                        LocalCertSelectionCallback certSelectionDelegate, RemoteCertValidationCallback remoteValidationCallback, SSPIConfiguration userConfig)
        {
            #if MONO_FEATURE_NEW_TLS
            TlsConfiguration config;
            if (serverMode)
            {
                var cert = (MSCX.X509Certificate2)serverCertificate;
                var monoCert = new MX.X509Certificate(cert.RawData);
                config = new TlsConfiguration((TlsProtocols)protocolFlags, userConfig != null ? userConfig.Settings : null, monoCert, cert.PrivateKey);
            }
            else
            {
                config = new TlsConfiguration((TlsProtocols)protocolFlags, userConfig != null ? userConfig.Settings : null, hostname);
                #if FIXME
				if (certSelectionDelegate != null)
					config.Client.LocalCertSelectionCallback = (t, l, r, a) => certSelectionDelegate(t, l, r, a);
                #endif
                if (remoteValidationCallback != null)
                    config.RemoteCertValidationCallback = (h, c, ch, p) =>
                    {
                        var ssc = new SSCX.X509Certificate(c.RawData);
                        return remoteValidationCallback(h, ssc, null, (SslPolicyErrors)p);
                    };
            }
            return new SSPIInterface(config);
            #else
			throw new NotImplementedException ();
            #endif
        }
    }

    /*
     * SSPIWrapper _is a _class that provides a managed implementation of the equivalent
     * _class _in Microsofts .NET Framework.   
     * 
     * The SSPIWrapper class is used by the TLS/SSL stack to implement both the 
     * protocol handshake as well as the encryption and decryption of messages.
     * 
     * Microsoft's implementation of this class is merely a P/Invoke wrapper
     * around the native SSPI APIs on Windows.   This implementation instead, 
     * provides a managed implementation that uses the cross platform Mono.Security 
     * to provide the equivalent functionality.
     * 
     * Ideally, this should be abstracted with a different name, and decouple
     * the naming from the SSPIWrapper name, but this allows Mono to reuse
     * the .NET code with minimal changes.
     * 
     * The "internal" methods here are the API that is consumed by the class
     * libraries.
     */
    internal static class SSPIWrapper
    {
        static void SetCredentials(SSPIInterface secModule, SafeFreeCredentials credentials)
        {
            #if MONO_FEATURE_NEW_TLS
            if (credentials != null && !credentials.IsInvalid)
            {
                if (!secModule.Configuration.HasCredentials && credentials.Certificate != null)
                {
                    var cert = new MX.X509Certificate(credentials.Certificate.RawData);
                    secModule.Configuration.SetCertificate(cert, credentials.Certificate.PrivateKey);
                }
                bool success = true;
                credentials.DangerousAddRef(ref success);
            }
            #else
			throw new NotImplementedException ();
            #endif
        }

        /*
         * @safecontext is null on the first use, but it will become non-null for invocations 
         * where the connection is being re-negotiated.
         * 
         */
        internal static int AcceptSecurityContext(SSPIInterface secModule, ref SafeFreeCredentials credentials, ref SafeDeleteContext safeContext, ContextFlags inFlags, Endianness endianness, SecurityBuffer inputBuffer, SecurityBuffer outputBuffer, ref ContextFlags outFlags)
        {
            #if MONO_FEATURE_NEW_TLS
            if (endianness != Endianness.Native)
                throw new NotSupportedException();

            if (safeContext == null)
            {
                if (credentials == null || credentials.IsInvalid)
                    return (int)SecurityStatus.CredentialsNeeded;

                safeContext = new SafeDeleteContext(new TlsContext(secModule.Configuration, true));
            }

            SetCredentials(secModule, credentials);

            var incoming = GetInputBuffer(inputBuffer);
            var outgoing = new TlsMultiBuffer();

            var retval = (int)safeContext.Context.GenerateNextToken(incoming, outgoing);
            UpdateOutput(outgoing, outputBuffer);
            return retval;
            #else
			throw new NotImplementedException ();
            #endif
        }

        internal static int InitializeSecurityContext(SSPIInterface secModule, ref SafeFreeCredentials credentials, ref SafeDeleteContext safeContext, string targetName, ContextFlags inFlags, Endianness endianness, SecurityBuffer inputBuffer, SecurityBuffer outputBuffer, ref ContextFlags outFlags)
        {
            #if MONO_FEATURE_NEW_TLS
            if (inputBuffer != null)
                throw new InvalidOperationException();

            if (safeContext == null)
                safeContext = new SafeDeleteContext(new TlsContext(secModule.Configuration, false));

            return InitializeSecurityContext(secModule, credentials, ref safeContext, targetName, inFlags, endianness, null, outputBuffer, ref outFlags);
            #else
			throw new NotImplementedException ();
            #endif
        }

        internal static int InitializeSecurityContext(SSPIInterface secModule, SafeFreeCredentials credentials, ref SafeDeleteContext safeContext, string targetName, ContextFlags inFlags, Endianness endianness, SecurityBuffer[] inputBuffers, SecurityBuffer outputBuffer, ref ContextFlags outFlags)
        {
            #if MONO_FEATURE_NEW_TLS
            if (endianness != Endianness.Native)
                throw new NotSupportedException();

            SetCredentials(secModule, credentials);

            SecurityBuffer inputBuffer = null;
            if (inputBuffers != null)
            {
                if (inputBuffers.Length != 2 || inputBuffers[1].type != BufferType.Empty)
                    throw new NotSupportedException();
                inputBuffer = inputBuffers[0];
            }

            var incoming = GetInputBuffer(inputBuffer);
            var outgoing = new TlsMultiBuffer();

            var retval = (int)safeContext.Context.GenerateNextToken(incoming, outgoing);
            UpdateOutput(outgoing, outputBuffer);
            return retval;
            #else
			throw new NotImplementedException ();
            #endif
        }

        internal static int EncryptMessage(SSPIInterface secModule, SafeDeleteContext safeContext, SecurityBuffer securityBuffer, uint sequenceNumber)
        {
            #if MONO_FEATURE_NEW_TLS
            var incoming = GetInputBuffer(securityBuffer);
            var retval = (int)safeContext.Context.EncryptMessage(ref incoming);
            UpdateOutput(incoming, securityBuffer);
            return retval;
            #else
			throw new NotImplementedException ();
            #endif
        }

        internal static int DecryptMessage(SSPIInterface secModule, SafeDeleteContext safeContext, SecurityBuffer securityBuffer, uint sequenceNumber)
        {
            #if MONO_FEATURE_NEW_TLS
            var incoming = GetInputBuffer(securityBuffer);
            var retval = (int)safeContext.Context.DecryptMessage(ref incoming);
            UpdateOutput(incoming, securityBuffer);
            return retval;
            #else
			throw new NotImplementedException ();
            #endif
        }

        internal static byte[] CreateShutdownMessage(SSPIInterface secModule, SafeDeleteContext safeContext)
        {
            #if MONO_FEATURE_NEW_TLS
            return safeContext.Context.CreateAlert(new Alert(AlertLevel.Warning, AlertDescription.CloseNotify));
            #else
			throw new NotImplementedException ();
            #endif
        }

        internal static bool IsClosed(SSPIInterface secModule, SafeDeleteContext safeContext)
        {
            #if MONO_FEATURE_NEW_TLS
            return safeContext.Context.ReceivedCloseNotify;
            #else
			throw new NotImplementedException ();
            #endif
        }

        internal static Exception GetLastError(SSPIInterface secModule, SafeDeleteContext safeContext)
        {
            #if MONO_FEATURE_NEW_TLS
            return safeContext.LastError;
            #else
			return null;
            #endif
        }

        internal static SafeFreeCredentials AcquireCredentialsHandle(SSPIInterface SecModule, string package, CredentialUse intent, SecureCredential scc)
        {
            return new SafeFreeCredentials(scc);
        }

        public static ChannelBinding QueryContextChannelBinding(SSPIInterface SecModule, SafeDeleteContext securityContext, ContextAttribute contextAttribute)
        {
            return null;
        }

        internal static MSCX.X509Certificate2 GetRemoteCertificate(SafeDeleteContext safeContext, out MSCX.X509Certificate2Collection remoteCertificateStore)
        {
            #if MONO_FEATURE_NEW_TLS
            MX.X509CertificateCollection monoCollection;
            if (safeContext == null || safeContext.IsInvalid)
            {
                remoteCertificateStore = null;
                return null;
            }
            var monoCert = safeContext.Context.GetRemoteCertificate(out monoCollection);
            if (monoCert == null)
            {
                remoteCertificateStore = null;
                return null;
            }

            remoteCertificateStore = new MSCX.X509Certificate2Collection();
            foreach (var cert in monoCollection)
            {
                remoteCertificateStore.Add(new SSCX.X509Certificate2(cert.RawData));
            }
            return new MSCX.X509Certificate2(monoCert.RawData);
            #else
			throw new NotImplementedException ();
            #endif
        }

        internal static bool CheckRemoteCertificate(SafeDeleteContext safeContext)
        {
            #if MONO_FEATURE_NEW_TLS
            return safeContext.Context.VerifyRemoteCertificate();
            #else
			throw new NotImplementedException ();
            #endif
        }

        #if MONO_FEATURE_NEW_TLS
        static TlsBuffer GetInputBuffer(SecurityBuffer incoming)
        {
            return incoming != null ? new TlsBuffer(incoming.token, incoming.offset, incoming.size) : null;
        }

        static void UpdateOutput(TlsMultiBuffer outgoing, SecurityBuffer outputBuffer)
        {
            if (outgoing.IsEmpty)
                return;
            var buffer = outgoing.StealBuffer();
            outputBuffer.token = buffer.Buffer;
            outputBuffer.offset = buffer.Offset;
            outputBuffer.size = buffer.Size;
            outputBuffer.type = BufferType.Token;
        }

        static void UpdateOutput(TlsBuffer buffer, SecurityBuffer outputBuffer)
        {
            if (buffer != null)
            {
                outputBuffer.token = buffer.Buffer;
                outputBuffer.offset = buffer.Offset;
                outputBuffer.size = buffer.Size;
                outputBuffer.type = BufferType.Token;
            }
            else
            {
                outputBuffer.token = null;
                outputBuffer.size = outputBuffer.offset = 0;
                outputBuffer.type = BufferType.Empty;
            }
        }
        #endif
    }
}
