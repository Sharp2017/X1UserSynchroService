using System;
using System.Collections.Generic;
using System.Text;

namespace SynchroCommon
{
  public  class TableSynchroTask
    {
        public TableSynchroTask()
        { }
        #region Model
        private Guid _tablesynchrotaskid;
        private string _tablename;
        private string _tablerowid;
        private int _operationtype;
        private int _taskstate = 0;
        private DateTime? _adddate = DateTime.Now;
        /// <summary>
        /// 
        /// </summary>
        public Guid TableSynchroTaskID
        {
            set { _tablesynchrotaskid = value; }
            get { return _tablesynchrotaskid; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string TableName
        {
            set { _tablename = value; }
            get { return _tablename; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string TableRowID
        {
            set { _tablerowid = value; }
            get { return _tablerowid; }
        }
        /// <summary>
        /// 0:表示插入或者更新  1:表示删除
        /// </summary>
        public int OperationType
        {
            set { _operationtype = value; }
            get { return _operationtype; }
        }
        /// <summary>
        /// 0:表示新任务  -1:表示执行失败的任务
        /// </summary>
        public int TaskState
        {
            set { _taskstate = value; }
            get { return _taskstate; }
        }
        /// <summary>
        /// 
        /// </summary>
        public DateTime? AddDate
        {
            set { _adddate = value; }
            get { return _adddate; }
        }
        #endregion Model
    }
}
