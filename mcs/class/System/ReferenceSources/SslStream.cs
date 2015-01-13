//
// Mono-specific additions to Microsoft's SslStream.cs
//
namespace System.Net.Security {
    using System.Net.Sockets;

    partial class SslStream
    {
        internal IAsyncResult BeginShutdown(AsyncCallback asyncCallback, object asyncState)
        {
            return _SslState.BeginShutdown(asyncCallback, asyncState);
        }

        internal void EndShutdown(IAsyncResult asyncResult)
        {
            _SslState.EndShutdown(asyncResult);
        }
    }
}
