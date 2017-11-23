using System;
using System.Collections.Generic;
using System.Configuration;
using System.Windows.Forms;
using X1UserSynchroService.Classes;

namespace X1UserSynchroService
{
    static class Program
    {
        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            if (Common.IsAppRunning("X1UserSynchroService"))
            {
                MessageBox.Show("程序正在运行，请先退出！", "提示！", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                Globals.SynchroType = Convert.ToInt32(ConfigurationManager.AppSettings["SynchroType"]);
            }
            catch (Exception)
            {

                Globals.SynchroType = 0;
            }
            try
            {
                Globals.RequestInterval = Convert.ToInt32(ConfigurationManager.AppSettings["RequestInterval"]) * 1000;
            }
            catch (Exception)
            {

                Globals.RequestInterval = 300 * 1000;
            }
            Globals.UserSynchroWebServiceUrl = ConfigurationManager.AppSettings["UserSynchroWebServiceUrl"];
            Globals.UserSynchroWebServiceUrl1 = ConfigurationManager.AppSettings["UserSynchroWebServiceUrl1"];
            Application.Run(new FrmMain());
        }
    }
}
