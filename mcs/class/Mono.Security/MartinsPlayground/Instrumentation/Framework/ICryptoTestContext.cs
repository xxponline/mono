extern alias MonoSecurity;
using System;
using MonoSecurity::Mono.Security.Interface;
using MonoSecurity::Mono.Security.Protocol.NewTls;
using MonoSecurity::Mono.Security.Protocol.NewTls.Cipher;
using MonoSecurity::Mono.Security.Protocol.NewTls.Handshake;

namespace Mono.Security.Instrumentation.Framework
{
	public interface ICryptoTestContext : IDisposable
	{
		bool EnableDebugging {
			get; set;
		}

		byte ExtraPaddingBlocks {
			get; set;
		}

		void InitializeCBC (CipherSuiteCode cipher, byte[] key, byte[] mac, byte[] iv);

		void InitializeGCM (CipherSuiteCode cipher, byte[] key, byte[] implNonce, byte[] explNonce);

		void EncryptRecord (ContentType contentType, IBufferOffsetSize input, TlsStream output);

		IBufferOffsetSize Encrypt (IBufferOffsetSize input);

		int Encrypt (IBufferOffsetSize input, IBufferOffsetSize output);

		IBufferOffsetSize Decrypt (IBufferOffsetSize input);

		int Decrypt (IBufferOffsetSize input, IBufferOffsetSize output);

		int BlockSize {
			get;
		}

		int GetEncryptedSize (int size);

		int MinExtraEncryptedBytes {
			get;
		}

		int MaxExtraEncryptedBytes {
			get;
		}
	}
}

