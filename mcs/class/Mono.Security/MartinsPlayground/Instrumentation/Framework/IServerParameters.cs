using System;

namespace Mono.Security.Instrumentation.Framework
{
	public interface IServerParameters : IConnectionParameters
	{
		ServerCertificate ServerCertificate {
			get; set;
		}

		bool AskForClientCertificate {
			get; set;
		}

		bool RequireClientCertificate {
			get; set;
		}
	}
}

