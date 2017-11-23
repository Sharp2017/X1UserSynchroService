using System;
using System.Collections.Generic;
using System.Text;

namespace SynchroCommon
{
    [Serializable]
    public   class UserGroup2User
    {
        public UserGroup2User()
        { }
        #region Model
        private Guid _id;
        private Guid _usergroupid;
        private Guid _userid;
        /// <summary>
        /// 
        /// </summary>
        public Guid ID
        {
            set { _id = value; }
            get { return _id; }
        }
        /// <summary>
        /// 
        /// </summary>
        public Guid UserGroupID
        {
            set { _usergroupid = value; }
            get { return _usergroupid; }
        }
        /// <summary>
        /// 
        /// </summary>
        public Guid UserID
        {
            set { _userid = value; }
            get { return _userid; }
        }
        #endregion Model

    }
}
