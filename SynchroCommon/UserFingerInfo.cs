using System;
using System.Collections.Generic;
using System.Text;

namespace SynchroCommon
{
    /// <summary>
	/// UserFingerInfo:实体类(属性说明自动提取数据库字段的描述信息)
	/// </summary>
	[Serializable]
    public partial class UserFingerInfo
    {
        public UserFingerInfo()
        { }
        #region Model
        private Guid _userid;
        private string _fingerinfostr1;
        private string _fingerinfostr2;
        private string _fingerinfostr3;
        /// <summary>
        /// 
        /// </summary>
        public Guid UserID
        {
            set { _userid = value; }
            get { return _userid; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string FingerInfoStr1
        {
            set { _fingerinfostr1 = value; }
            get { return _fingerinfostr1; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string FingerInfoStr2
        {
            set { _fingerinfostr2 = value; }
            get { return _fingerinfostr2; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string FingerInfoStr3
        {
            set { _fingerinfostr3 = value; }
            get { return _fingerinfostr3; }
        }
        #endregion Model

    }
}
