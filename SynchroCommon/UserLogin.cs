using System;
using System.Collections.Generic;
using System.Text;

namespace SynchroCommon
{
    public class UserLogin
    {

        /// <summary>
        /// UserID
        /// </summary>		
        private Guid _userid;
        public Guid UserID
        {
            get { return _userid; }
            set { _userid = value; }
        }
        /// <summary>
        /// LoginTime
        /// </summary>		
        private DateTime _logintime;
        public DateTime LoginTime
        {
            get { return _logintime; }
            set { _logintime = value; }
        }
        /// <summary>
        /// ComputerName
        /// </summary>		
        private string _computername;
        public string ComputerName
        {
            get { return _computername; }
            set { _computername = value; }
        }
        /// <summary>
        /// IPAddress
        /// </summary>		
        private string _ipaddress;
        public string IPAddress
        {
            get { return _ipaddress; }
            set { _ipaddress = value; }
        }

    }

}
