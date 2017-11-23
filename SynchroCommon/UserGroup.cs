using System;
using System.Collections.Generic;
using System.Text;

namespace SynchroCommon
{
    [Serializable]
    public  class UserGroup
    {
        public UserGroup()
        { }
        #region Model
        private Guid _id;
        private Guid _systemid;
        private string _name;
        private string _description;
        private int? _category;
        private string _categoryname;
        private int? _sysrelnumber;
        private string _createusername;
        private Guid _createuserid;
        private DateTime? _createdatetime;
        private string _modifyusername;
        private Guid _modifyuserid;
        private DateTime? _modifydatetime;
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
        public Guid SystemID
        {
            set { _systemid = value; }
            get { return _systemid; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string Name
        {
            set { _name = value; }
            get { return _name; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string Description
        {
            set { _description = value; }
            get { return _description; }
        }
        /// <summary>
        /// 
        /// </summary>
        public int? Category
        {
            set { _category = value; }
            get { return _category; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string CategoryName
        {
            set { _categoryname = value; }
            get { return _categoryname; }
        }
        /// <summary>
        /// 
        /// </summary>
        public int? SysRelNumber
        {
            set { _sysrelnumber = value; }
            get { return _sysrelnumber; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string CreateUserName
        {
            set { _createusername = value; }
            get { return _createusername; }
        }
        /// <summary>
        /// 
        /// </summary>
        public Guid CreateUserID
        {
            set { _createuserid = value; }
            get { return _createuserid; }
        }
        /// <summary>
        /// 
        /// </summary>
        public DateTime? CreateDateTime
        {
            set { _createdatetime = value; }
            get { return _createdatetime; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string ModifyUserName
        {
            set { _modifyusername = value; }
            get { return _modifyusername; }
        }
        /// <summary>
        /// 
        /// </summary>
        public Guid ModifyUserID
        {
            set { _modifyuserid = value; }
            get { return _modifyuserid; }
        }
        /// <summary>
        /// 
        /// </summary>
        public DateTime? ModifyDateTime
        {
            set { _modifydatetime = value; }
            get { return _modifydatetime; }
        }
        #endregion Model

    }
}
