#if MOBILE
namespace System.Diagnostics
{
	//
	// Dummy TraceSource class, allows us to bring the stubbed-out
	// (MONO_FEATURE_LOGGING not set) version of Logging.
	//
	class TraceSource
	{
	}
}
#endif
