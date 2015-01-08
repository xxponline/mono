extern alias MonoSecurity;
using MonoSecurity::Mono.Security.Protocol.NewTls;
using MonoSecurity::Mono.Security.Protocol.NewTls.Instrumentation;

namespace Mono.Security.Instrumentation.Framework
{
	using Framework;

	public class MonoClientParameters : ClientParameters, IMonoClientParameters
	{
		public ClientCertificateParameters ClientCertificateParameters {
			get; set;
		}

		public InstrumentCollection ClientInstrumentation {
			get; set;
		}
	}
}

