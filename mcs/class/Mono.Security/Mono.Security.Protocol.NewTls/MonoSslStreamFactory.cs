#if NEW_MONO_API
extern alias MonoSecurity;
using System;
using System.IO;
using System.Net.Security;
using System.Security.Authentication;
using System.Security.Cryptography.X509Certificates;
using TlsSettings = MonoSecurity.Mono.Security.Protocol.NewTls.TlsSettings;

namespace Mono.Security.NewMonoSource
{
    public static class MonoSslStreamFactory
    {
        public static MonoSslStream CreateServer(Stream innerStream, bool leaveOpen, RemoteCertificateValidationCallback certValidationCallback, 
            LocalCertificateSelectionCallback certSelectionCallback, EncryptionPolicy encryptionPolicy, TlsSettings settings,
            X509Certificate serverCertificate, bool clientCertificateRequired, SslProtocols enabledSslProtocols, bool checkCertificateRevocation)
        {
            var stream = new MonoSslStream(innerStream, leaveOpen, certValidationCallback, certSelectionCallback, encryptionPolicy, settings);
            try
            {
                stream.AuthenticateAsServer(serverCertificate, clientCertificateRequired, enabledSslProtocols, checkCertificateRevocation);
            }
            catch (Exception ex)
            {
                var tlsEx = stream.LastError;
                if (tlsEx != null)
                    throw new AggregateException(ex, tlsEx);
                throw;
            }
            return stream;
        }

        public static MonoSslStream CreateClient(Stream innerStream, bool leaveOpen, RemoteCertificateValidationCallback certValidationCallback, 
            LocalCertificateSelectionCallback certSelectionCallback, EncryptionPolicy encryptionPolicy, TlsSettings settings,
            string targetHost, X509CertificateCollection clientCertificates, SslProtocols enabledSslProtocols, bool checkCertificateRevocation)
        {
            var stream = new MonoSslStream(innerStream, leaveOpen, certValidationCallback, certSelectionCallback, encryptionPolicy, settings);
            try
            {
                stream.AuthenticateAsClient(targetHost, clientCertificates, enabledSslProtocols, checkCertificateRevocation);
            }
            catch (Exception ex)
            {
                var tlsEx = stream.LastError;
                if (tlsEx != null)
                    throw new AggregateException(ex, tlsEx);
                throw;
            }
            return stream;
        }
    }
}
#endif
