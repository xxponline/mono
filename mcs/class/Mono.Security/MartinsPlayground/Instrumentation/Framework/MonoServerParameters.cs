extern alias MonoSecurity;
using MonoSecurity::Mono.Security.Protocol.NewTls;
using MonoSecurity::Mono.Security.Protocol.NewTls.Instrumentation;

namespace Mono.Security.Instrumentation.Framework
{
	using Framework;

	public class MonoServerParameters : ServerParameters, IMonoServerParameters
	{
		public InstrumentCollection ServerInstrumentation {
			get; set;
		}
	}
}

