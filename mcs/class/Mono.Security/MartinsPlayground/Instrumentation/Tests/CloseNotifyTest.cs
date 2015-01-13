//
// CloseNotifyTest.cs
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
using System.Threading;
using System.Threading.Tasks;
using Mono.Security.Protocol.NewTls;
using Mono.Security.Protocol.NewTls.Instrumentation;
using Mono.Security.Protocol.NewTls.Negotiation;
using Mono.Security.Protocol.NewTls.Handshake;
using NUnit.Framework;

namespace Mono.Security.Instrumentation.Tests
{
	using Framework;
	using Resources;

	[Explicit]
	[Category ("NotWorking")]
	[ConnectionFactoryParameters (ConnectionType.OpenSslClient | ConnectionType.OpenSslServer)]
	class CloseNotifyTest : ConnectionTest
	{
		public CloseNotifyTest (TestConfiguration config, ClientAndServerFactory factory)
			: base (config, factory)
		{
		}

		ClientAndServerParameters GetDefaultParameters ()
		{
			return new ClientAndServerParameters {
				VerifyPeerCertificate = false
			};
		}

		[Test]
		[Category ("Martin")]
		public async void Simple ()
		{
			await Run (MyFlags.None);
		}

		async Task Run (MyFlags flags, ClientAndServerParameters parameters = null, Action<ClientAndServer> action = null)
		{
			if (parameters == null)
				parameters = GetDefaultParameters ();

			try {
				if (Configuration.EnableDebugging)
					parameters.EnableDebugging = true;
				using (var connection = (ClientAndServer)await Factory.Start (parameters)) {
					if (action != null)
						action (connection);
					var handler = new MyConnectionHandler (connection, flags);
					await handler.Run ();
				}
			} catch (Exception ex) {
				DebugHelper.WriteLine ("ERROR: {0} {1}", ex.GetType (), ex);
				throw;
			}
		}

		[Flags]
		enum MyFlags {
			None = 0
		}

		class MyConnectionHandler : ClientAndServerHandler
		{
			public readonly MyFlags Flags;

			public MyConnectionHandler (IConnection connection, MyFlags flags)
				: base ((ClientAndServer)connection)
			{
				Flags = flags;
			}

			protected override async Task MainLoop (ILineBasedStream serverStream, ILineBasedStream clientStream)
			{
				await serverStream.WriteLineAsync ("SERVER OK");
				var line = await clientStream.ReadLineAsync ();
				if (!line.Equals ("SERVER OK"))
					throw new ConnectionException ("Got unexpected output from server: '{0}'", line);
				await clientStream.WriteLineAsync ("CLIENT OK");
				line = await serverStream.ReadLineAsync ();
				if (!line.Equals ("CLIENT OK"))
					throw new ConnectionException ("Got unexpected output from client: '{0}'", line);
				await Connection.Shutdown (true);
				Connection.Dispose ();
			}
		}

	}
}

