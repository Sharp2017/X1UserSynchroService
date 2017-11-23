using System;
using System.Collections.Generic;
using System.Text;

namespace X1UserSynchroService.Classes
{
    public class Globals
    {
        /// <summary>
        /// 间隔时间 毫秒
        /// </summary>
        public static int RequestInterval = 300;

        /// <summary>
        /// 0：默认只同步登陆的密码 1:河南台同步 
        /// </summary>
        public static int SynchroType = 0;
        /// <summary>
        /// 检查停止时间
        /// </summary>
        public static DateTime ExecuteStopDate;

        public static string UserSynchroWebServiceUrl = "";
        public static string UserSynchroWebServiceUrl1 = "";
        public static UserSynchroWS UserSynchroWebService = new UserSynchroWS();
        public static SynchroDBManager SynchroDBManager = new SynchroDBManager();
    }
}
