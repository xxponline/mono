#if NEW_MONO_API
extern alias MonoSecurity;
using System;
using System.IO;
using System.Net.Security;
using System.Threading.Tasks;
using MonoSecurity::Mono.Security.Interface;
using MonoSecurity::Mono.Security.Protocol.NewTls;

namespace Mono.Security.NewMonoSource
{
    public class MonoSslStream : SslStream
    {
        internal MonoSslStream(Stream innerStream, TlsSettings settings)
            : this(innerStream, false, null, null, settings)
        {
        }

        internal MonoSslStream(Stream innerStream, bool leaveOpen, TlsSettings settings)
            : this(innerStream, leaveOpen, null, null, EncryptionPolicy.RequireEncryption, settings)
        {
        }

        internal MonoSslStream(Stream innerStream, bool leaveOpen, RemoteCertificateValidationCallback certValidationCallback, MonoTlsSettings settings)
            : this(innerStream, leaveOpen, certValidationCallback, null, EncryptionPolicy.RequireEncryption, settings)
        {
        }

        internal MonoSslStream(Stream innerStream, bool leaveOpen, RemoteCertificateValidationCallback certValidationCallback, 
                        LocalCertificateSelectionCallback certSelectionCallback, MonoTlsSettings settings)
            : this(innerStream, leaveOpen, certValidationCallback, certSelectionCallback, EncryptionPolicy.RequireEncryption, settings)
        {
        }

        internal MonoSslStream(Stream innerStream, bool leaveOpen, RemoteCertificateValidationCallback certValidationCallback, 
                        LocalCertificateSelectionCallback certSelectionCallback, EncryptionPolicy encryptionPolicy, MonoTlsSettings settings)
            : base(innerStream, leaveOpen, certValidationCallback, certSelectionCallback, encryptionPolicy, settings)
        {
        }

        public bool IsClosed
        {
            get { return base.IsClosed; }
        }

        public TlsException LastError
        {
            get { return (TlsException)base.LastError; }
        }

        public Task Shutdown(bool waitForReply)
        {
            return Task.Factory.FromAsync((state,result) => BeginShutdown (waitForReply, state, result), EndShutdown, null);
        }
    }
}
#endif

