extern alias MonoSecurity;
using MonoSecurity::Mono.Security.Protocol.NewTls;
using MonoSecurity::Mono.Security.Protocol.NewTls.Cipher;

namespace Mono.Security.Instrumentation.Framework
{
	public interface IClientParameters : IConnectionParameters
	{
		CipherSuiteCollection ClientCiphers {
			get; set;
		}

		CertificateAndKeyAsPFX ClientCertificate {
			get; set;
		}
	}
}

