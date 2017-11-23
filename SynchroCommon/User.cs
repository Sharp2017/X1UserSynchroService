using System;
using System.Collections.Generic;
using System.Text;

namespace SynchroCommon
{ 
        [Serializable]
        public   class User
        {
            public User()
            { }
            #region Model
            private Guid _id;
            private string _name;
            private string _code;
            private bool _gender;
            private string _loginname;
            private string _password;
            private string _tag1;
            private string _tag2;
            private string _tag3;
            private string _certificate = "";
            private bool _forcecard;
            private int? _jobstatus;
            private string _icnumber1;
            private string _icnumber2;
            private string _icnumber3;
            private string _jobnumber;
            private string _idnumber;
            private string _fingerprint;
            private string _email1;
            private string _email2;
            private string _telephone1;
            private string _telephone2;
            private string _companyaddr;
            private string _homeaddr;
            private DateTime? _lastlogindatetime;
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
            public string Name
            {
                set { _name = value; }
                get { return _name; }
            }
            /// <summary>
            /// 
            /// </summary>
            public string Code
            {
                set { _code = value; }
                get { return _code; }
            }
            /// <summary>
            /// 
            /// </summary>
            public bool Gender
            {
                set { _gender = value; }
                get { return _gender; }
            }
            /// <summary>
            /// 
            /// </summary>
            public string LoginName
            {
                set { _loginname = value; }
                get { return _loginname; }
            }
            /// <summary>
            /// 
            /// </summary>
            public string Password
            {
                set { _password = value; }
                get { return _password; }
            }
            /// <summary>
            /// 
            /// </summary>
            public string Tag1
            {
                set { _tag1 = value; }
                get { return _tag1; }
            }
            /// <summary>
            /// 
            /// </summary>
            public string Tag2
            {
                set { _tag2 = value; }
                get { return _tag2; }
            }
            /// <summary>
            /// 
            /// </summary>
            public string Tag3
            {
                set { _tag3 = value; }
                get { return _tag3; }
            }
            /// <summary>
            /// 
            /// </summary>
            public string Certificate
            {
                set { _certificate = value; }
                get { return _certificate; }
            }
            /// <summary>
            /// 
            /// </summary>
            public bool ForceCard
            {
                set { _forcecard = value; }
                get { return _forcecard; }
            }
            /// <summary>
            /// 
            /// </summary>
            public int? JobStatus
            {
                set { _jobstatus = value; }
                get { return _jobstatus; }
            }
            /// <summary>
            /// 
            /// </summary>
            public string ICNumber1
            {
                set { _icnumber1 = value; }
                get { return _icnumber1; }
            }
            /// <summary>
            /// 
            /// </summary>
            public string ICNumber2
            {
                set { _icnumber2 = value; }
                get { return _icnumber2; }
            }
            /// <summary>
            /// 
            /// </summary>
            public string ICNumber3
            {
                set { _icnumber3 = value; }
                get { return _icnumber3; }
            }
            /// <summary>
            /// 
            /// </summary>
            public string JobNumber
            {
                set { _jobnumber = value; }
                get { return _jobnumber; }
            }
            /// <summary>
            /// 
            /// </summary>
            public string IDNumber
            {
                set { _idnumber = value; }
                get { return _idnumber; }
            }
            /// <summary>
            /// 
            /// </summary>
            public string Fingerprint
            {
                set { _fingerprint = value; }
                get { return _fingerprint; }
            }
            /// <summary>
            /// 
            /// </summary>
            public string Email1
            {
                set { _email1 = value; }
                get { return _email1; }
            }
            /// <summary>
            /// 
            /// </summary>
            public string Email2
            {
                set { _email2 = value; }
                get { return _email2; }
            }
            /// <summary>
            /// 
            /// </summary>
            public string Telephone1
            {
                set { _telephone1 = value; }
                get { return _telephone1; }
            }
            /// <summary>
            /// 
            /// </summary>
            public string Telephone2
            {
                set { _telephone2 = value; }
                get { return _telephone2; }
            }
            /// <summary>
            /// 
            /// </summary>
            public string CompanyAddr
            {
                set { _companyaddr = value; }
                get { return _companyaddr; }
            }
            /// <summary>
            /// 
            /// </summary>
            public string HomeAddr
            {
                set { _homeaddr = value; }
                get { return _homeaddr; }
            }
            /// <summary>
            /// 
            /// </summary>
            public DateTime? LastLoginDateTime
            {
                set { _lastlogindatetime = value; }
                get { return _lastlogindatetime; }
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

 
