//
// Mono-specific additions to Microsoft's _SslState.cs
//
namespace System.Net.Security {
    using System.Net.Sockets;

    partial class SslState
    {
        internal IAsyncResult BeginShutdown (AsyncCallback asyncCallback, object asyncState)
        {
            LazyAsyncResult lazyResult = new LazyAsyncResult(this, asyncState, asyncCallback);
            AsyncProtocolRequest asyncRequest = new AsyncProtocolRequest(lazyResult);
            ProtocolToken message = Context.CreateShutdownMessage();
            StartWriteShutdown(message.Payload, asyncRequest);
            return lazyResult;
        }

        internal void EndShutdown(IAsyncResult asyncResult)
        {

        }

        void StartWriteShutdown(byte[] buffer, AsyncProtocolRequest asyncRequest)
        {
            // request a write IO slot
            if (CheckEnqueueWrite(asyncRequest))
            {
                // operation is async and has been queued, return.
                return;
            }

            byte[] lastHandshakePayload = null;
            if (LastPayload != null)
                throw new NotImplementedException();

            if (asyncRequest != null)
            {
                // prepare for the next request
                IAsyncResult ar = ((NetworkStream)InnerStream).BeginWrite(buffer, 0, buffer.Length, ShutdownCallback, asyncRequest);
                if (!ar.CompletedSynchronously)
                    return;

                ((NetworkStream)InnerStream).EndWrite(ar);
            }
            else
            {
                ((NetworkStream)InnerStream).Write(buffer, 0, buffer.Length);
            }

            // release write IO slot
            FinishWrite();

            if (asyncRequest != null)
                asyncRequest.CompleteUser();
        }

        static void ShutdownCallback(IAsyncResult transportResult)
        {
            if (transportResult.CompletedSynchronously)
                return;

            AsyncProtocolRequest asyncRequest = (AsyncProtocolRequest)transportResult.AsyncState;

            SslState sslState = (SslState)asyncRequest.AsyncObject;
            try {
                ((NetworkStream)(sslState.InnerStream)).EndWrite(transportResult);
                sslState.FinishWrite();
                sslState.WaitForShutdown(asyncRequest);
            } catch (Exception e) {
                if (asyncRequest.IsUserCompleted) {
                    // This will throw on a worker thread.
                    throw;
                }
                sslState.FinishWrite();
                asyncRequest.CompleteWithError(e);
            }
        }

        static void ShutdownReadCallback(IAsyncResult transportResult)
        {
            AsyncProtocolRequest asyncRequest = (AsyncProtocolRequest)transportResult.AsyncState;

            var buffer = new byte [8];
            SslState sslState = (SslState)asyncRequest.AsyncObject;
            try {
                var ret = sslState.SecureStream.EndRead(transportResult);
                sslState.FinishRead(null);
                asyncRequest.CompleteUser();
            } catch (Exception e) {
                if (asyncRequest.IsUserCompleted) {
                    // This will throw on a worker thread.
                    throw;
                }
                sslState.FinishRead(null);
                asyncRequest.CompleteWithError(e);
            }
        }

        void WaitForShutdown(AsyncProtocolRequest asyncRequest)
        {
            var buffer = new byte[16];
            IAsyncResult ar = SecureStream.BeginRead(buffer, 0, buffer.Length, ShutdownReadCallback, asyncRequest);
            if (ar.CompletedSynchronously)
                ShutdownReadCallback(ar);
        }
    }
}
