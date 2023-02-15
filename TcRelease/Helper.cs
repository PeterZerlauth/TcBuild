    
using System;
using System.Security.Principal;

namespace TcRelease
{
    class Helper
    {

        public bool Administrator()
        {
            WindowsIdentity identity = WindowsIdentity.GetCurrent();
            WindowsPrincipal principal = new WindowsPrincipal(identity);
            return principal.IsInRole(WindowsBuiltInRole.Administrator);
        }
   }
}
