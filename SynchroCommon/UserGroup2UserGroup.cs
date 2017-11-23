using System;
using System.Collections.Generic;
using System.Text;

namespace SynchroCommon
{
     
	[Serializable]
    public   class UserGroup2UserGroup
    {
        public UserGroup2UserGroup()
        { }
        #region Model
        private Guid _id;
        private Guid _superiorid;
        private Guid _lowerid;
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
        public Guid SuperiorID
        {
            set { _superiorid = value; }
            get { return _superiorid; }
        }
        /// <summary>
        /// 
        /// </summary>
        public Guid LowerID
        {
            set { _lowerid = value; }
            get { return _lowerid; }
        }
        #endregion Model

    }
}
