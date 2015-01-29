using System;

namespace Mono.Security.Protocol.NewTls
{
	public interface IBufferOffsetSize
	{
		byte[] Buffer {
			get;
		}

		int Offset {
			get;
		}

		int Size {
			get;
		}
	}
}

