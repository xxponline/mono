extern alias MonoSecurity;
using MonoSecurity::Mono.Security.Protocol.NewTls;
using MonoSecurity::Mono.Security.Protocol.NewTls.Cipher;

namespace Mono.Security.Instrumentation.Framework
{
	public class ClientParameters : ConnectionParameters, IClientParameters
	{
		public CipherSuiteCollection ClientCiphers {
			get; set;
		}

		public CertificateAndKeyAsPFX ClientCertificate {
			get; set;
		}
	}
}

