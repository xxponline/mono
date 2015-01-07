#if MONO_FEATURE_NEW_TLS
#if MONO_INSIDE_SYSTEM || MONO_SECURITY_ALIAS
extern alias MonoSecurity;
using TlsContext = MonoSecurity::Mono.Security.Protocol.NewTls.TlsContext;
#if MONO_SECURITY_ALIAS
using X509Certificate2 = MonoSecurity::System.Security.Cryptography.X509Certificates.X509Certificate2;
#else
using X509Certificate2 = System.Security.Cryptography.X509Certificates.X509Certificate2;
#endif
#else
using TlsContext = Mono.Security.Protocol.NewTls.TlsContext;
using X509Certificate2 = System.Security.Cryptography.X509Certificates.X509Certificate2;
#endif
#else
using X509Certificate2 = System.Security.Cryptography.X509Certificates.X509Certificate2;
#endif

using System.Runtime.InteropServices;

namespace System.Net.Security
{
	class DummySafeHandle : SafeHandle
	{
		protected DummySafeHandle () : base ((IntPtr)(-1), true)
		{
		}

		protected override bool ReleaseHandle ()
		{
			return true;
		}

		public override bool IsInvalid {
			get { return handle == (IntPtr)(-1); }
		}
	}

	class SafeFreeCertContext : DummySafeHandle
	{
	}

	class SafeFreeCredentials : DummySafeHandle
	{
		SecureCredential credential;

		public X509Certificate2 Certificate {
			get {
				if (IsInvalid)
					throw new ObjectDisposedException ("Certificate");
				return credential.certificate;
			}
		}

		public SafeFreeCredentials (SecureCredential credential)
		{
			this.credential = credential;
			bool success = true;
			DangerousAddRef (ref success);
		}

		public override bool IsInvalid {
			get {
				return credential.certificate == null;
			}
		}

		protected override bool ReleaseHandle ()
		{
			credential.Clear ();
			return base.ReleaseHandle ();
		}
	}

#if MONO_FEATURE_NEW_TLS
	class SafeDeleteContext : DummySafeHandle
	{
		TlsContext context;

		public TlsContext Context {
			get {
				if (IsInvalid)
					throw new ObjectDisposedException ("Context");
				return context;
			}
		}

		public SafeDeleteContext (TlsContext context)
		{
			this.context = context;
		}

		public override bool IsInvalid {
			get {
				return context == null || !context.IsValid;
			}
		}

		protected override bool ReleaseHandle ()
		{
			context.Clear ();
			context = null;
			return base.ReleaseHandle ();
		}
	}
#endif

	struct SecureCredential
	{
		public const int CurrentVersion = 0x4;

		[Flags]
		public enum Flags
		{
			Zero = 0,
			NoSystemMapper = 0x02,
			NoNameCheck = 0x04,
			ValidateManual = 0x08,
			NoDefaultCred = 0x10,
			ValidateAuto = 0x20
		}

		int version;
		internal X509Certificate2 certificate;
		SchProtocols protocols;
		EncryptionPolicy policy;

		public SecureCredential (int version, X509Certificate2 certificate, SecureCredential.Flags flags, SchProtocols protocols, EncryptionPolicy policy)
		{
			this.version = version;
			this.certificate = certificate;
			this.protocols = protocols;
			this.policy = policy;
		}

		public void Clear ()
		{
			certificate = null;
		}
	}

	internal class SafeCredentialReference : DummySafeHandle
	{
		//
		// Static cache will return the target handle if found the reference in the table.
		//
		internal SafeFreeCredentials _Target;

		//
		//
		internal static SafeCredentialReference CreateReference (SafeFreeCredentials target)
		{
			SafeCredentialReference result = new SafeCredentialReference (target);
			if (result.IsInvalid)
				return null;

			return result;
		}

		private SafeCredentialReference (SafeFreeCredentials target) : base ()
		{
			// Bumps up the refcount on Target to signify that target handle is statically cached so
			// its dispose should be postponed
			bool b = false;
			try {
				target.DangerousAddRef (ref b);
			} catch {
				if (b) {
					target.DangerousRelease ();
					b = false;
				}
			} finally {
				if (b) {
					_Target = target;
					SetHandle (new IntPtr (0));   // make this handle valid
				}
			}
		}

		override protected bool ReleaseHandle ()
		{
			SafeFreeCredentials target = _Target;
			if (target != null)
				target.DangerousRelease ();
			_Target = null;
			return true;
		}
	}

}
