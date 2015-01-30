//
// Mono-specific additions to Microsoft's SslStream.cs
//
namespace System.Net.Security {
    using System.Net.Sockets;
    using System.IO;

    partial class SslStream
    {
        SSPIConfiguration _Configuration;

        internal SslStream(Stream innerStream, bool leaveInnerStreamOpen, RemoteCertificateValidationCallback userCertificateValidationCallback, 
            LocalCertificateSelectionCallback userCertificateSelectionCallback, EncryptionPolicy encryptionPolicy, SSPIConfiguration config)
            : base(innerStream, leaveInnerStreamOpen)
        {
            if (encryptionPolicy != EncryptionPolicy.RequireEncryption && encryptionPolicy != EncryptionPolicy.AllowNoEncryption && encryptionPolicy != EncryptionPolicy.NoEncryption) 
                throw new ArgumentException(SR.GetString(SR.net_invalid_enum, "EncryptionPolicy"), "encryptionPolicy");

            _userCertificateValidationCallback = userCertificateValidationCallback;
            _userCertificateSelectionCallback  = userCertificateSelectionCallback;
            RemoteCertValidationCallback _userCertValidationCallbackWrapper = new RemoteCertValidationCallback(userCertValidationCallbackWrapper);
            LocalCertSelectionCallback   _userCertSelectionCallbackWrapper  = userCertificateSelectionCallback==null  ? null : new LocalCertSelectionCallback(userCertSelectionCallbackWrapper);
            _SslState = new SslState(innerStream, _userCertValidationCallbackWrapper, _userCertSelectionCallbackWrapper, encryptionPolicy, config);
        }

        internal bool IsClosed
        {
            get { return _SslState.IsClosed; }
        }

        internal Exception LastError
        {
            get { return _SslState.LastError; }
        }

        internal IAsyncResult BeginShutdown(bool waitForReply, AsyncCallback asyncCallback, object asyncState)
        {
            return _SslState.BeginShutdown(waitForReply, asyncCallback, asyncState);
        }

        internal void EndShutdown(IAsyncResult asyncResult)
        {
            _SslState.EndShutdown(asyncResult);
        }
    }
}
