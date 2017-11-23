using System;
using System.Collections.Generic;
using System.Text;

namespace X1UserSynchroService.Classes
{
    public class Common
    {
        public static bool IsAppRunning(string appName)
        {
            System.Diagnostics.Process[] myProcesses = System.Diagnostics.Process.GetProcesses();

            int count = 0;
            foreach (System.Diagnostics.Process myProcess in myProcesses)
            {
                if (myProcess.ProcessName.ToLower() == (appName.ToLower()))
                {
                    count++;
                }
            }
            if (count > 1)
            {

                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
