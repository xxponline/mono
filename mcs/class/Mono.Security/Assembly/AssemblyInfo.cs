//
// AssemblyInfo.cs
//
// Authors:
//	Andreas Nahr (ClassDevelopment@A-SoftTech.com)
//	Sebastien Pouliot  (sebastien@ximian.com)
//
// (C) 2003 Ximian, Inc.  http://www.ximian.com
// (C) 2004 Novell (http://www.novell.com)
//

//
// Permission is hereby granted, free of charge, to any person obtaining
// a copy of this software and associated documentation files (the
// "Software"), to deal in the Software without restriction, including
// without limitation the rights to use, copy, modify, merge, publish,
// distribute, sublicense, and/or sell copies of the Software, and to
// permit persons to whom the Software is furnished to do so, subject to
// the following conditions:
// 
// The above copyright notice and this permission notice shall be
// included in all copies or substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND,
// EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF
// MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
// NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE
// LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION
// OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION
// WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
//

using System;
using System.Reflection;
using System.Resources;
using System.Security;
using System.Security.Permissions;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

// General Information about the system assembly

[assembly: AssemblyVersion (Consts.FxVersion)]

[assembly: AssemblyCompany ("MONO development team")]
[assembly: AssemblyCopyright ("(c) 2003-2004 Various Authors")]
[assembly: AssemblyDescription ("Mono.Security.dll")]
[assembly: AssemblyProduct ("MONO CLI")]
[assembly: AssemblyTitle ("Mono.Security.dll")]
[assembly: CLSCompliant (true)]
[assembly: ComVisible (false)]
[assembly: NeutralResourcesLanguage ("en-US")]


// BigInteger use unsafe code
// incomplete - prevent testing on Windows
//[assembly:SecurityPermission (SecurityAction.RequestOptional, UnmanagedCode=true)]


[assembly: AssemblyDelaySign (true)]
[assembly: AssemblyKeyFile ("../mono.pub")]

#if MOBILE
[assembly: InternalsVisibleTo ("System, PublicKey=00240000048000009400000006020000002400005253413100040000010001008D56C76F9E8649383049F383C44BE0EC204181822A6C31CF5EB7EF486944D032188EA1D3920763712CCB12D75FB77E9811149E6148E5D32FBAAB37611C1878DDC19E20EF135D0CB2CFF2BFEC3D115810C3D9069638FE4BE215DBF795861920E5AB6F7DB2E2CEEF136AC23D5DD2BF031700AEC232F6C6B1C785B4305C123B37AB")]
#endif

[assembly: InternalsVisibleTo ("Mono.Security.NewTls, PublicKey=00240000048000009400000006020000002400005253413100040000010001008D56C76F9E8649383049F383C44BE0EC204181822A6C31CF5EB7EF486944D032188EA1D3920763712CCB12D75FB77E9811149E6148E5D32FBAAB37611C1878DDC19E20EF135D0CB2CFF2BFEC3D115810C3D9069638FE4BE215DBF795861920E5AB6F7DB2E2CEEF136AC23D5DD2BF031700AEC232F6C6B1C785B4305C123B37AB")]

#if INSTRUMENTATION
[assembly: InternalsVisibleTo ("Mono.Security.Instrumentation.Framework, PublicKey=0024000004800000940000000602000000240000525341310004000011000000990dad24771188a27bb12112dff736fc75d80d42b0ad009366b859ec62a4b628d65e99bfae957c3907c4e728ba933316727b16ca62ea951b9ce6050ecdc8daf04613befedbc99007f1210fee0f22e8b822a05cd889241bb12324a9907962adf7e2e976bca92702eddee917b440aff54af6f8511f4863379fac442cf72b01e2a8")]
[assembly: InternalsVisibleTo ("Mono.Security.Instrumentation.Tests, PublicKey=0024000004800000940000000602000000240000525341310004000011000000990dad24771188a27bb12112dff736fc75d80d42b0ad009366b859ec62a4b628d65e99bfae957c3907c4e728ba933316727b16ca62ea951b9ce6050ecdc8daf04613befedbc99007f1210fee0f22e8b822a05cd889241bb12324a9907962adf7e2e976bca92702eddee917b440aff54af6f8511f4863379fac442cf72b01e2a8")]
[assembly: InternalsVisibleTo ("Mono.Security.Instrumentation.Console, PublicKey=0024000004800000940000000602000000240000525341310004000011000000990dad24771188a27bb12112dff736fc75d80d42b0ad009366b859ec62a4b628d65e99bfae957c3907c4e728ba933316727b16ca62ea951b9ce6050ecdc8daf04613befedbc99007f1210fee0f22e8b822a05cd889241bb12324a9907962adf7e2e976bca92702eddee917b440aff54af6f8511f4863379fac442cf72b01e2a8")]
#endif
