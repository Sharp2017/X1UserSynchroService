using X1UserSynchroService.UserSynchroWS;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace X1UserSynchroService.Classes
{
    public class SynchroDBManager
    {
        private static string getTmpAppConnectionString()
        {
            string connStr = "";
            try
            {
                connStr = ConfigurationManager.AppSettings.Get("primaryConnection").ToString();
                if (connStr == null)
                {
                    return "";
                }
                else
                {
                    return connStr;
                }
            }
            catch
            {
                return connStr;
            }

        }

        public static List<SynchroCommon.TableSynchroTask> GetTableSynchroTaskList()
        {
            try
            {
                List<SynchroCommon.TableSynchroTask> _TableSynchroTaskList = new List<SynchroCommon.TableSynchroTask>();
                DataTable taskDataTable = new DataTable();
                using (SqlConnection conn = new SqlConnection(getTmpAppConnectionString()))
                {
                    string sql = " select top 10* from TableSynchroTask where  TaskState=0  and TableRowID is Not NULL  order by AddDate";
                    using (SqlDataAdapter sd = new SqlDataAdapter(sql, conn))
                    {
                        sd.Fill(taskDataTable);
                    }
                    if (taskDataTable.Rows.Count > 0)
                    {
                        for (int i = 0; i < taskDataTable.Rows.Count; i++)
                        {
                            DataRow row = taskDataTable.Rows[i];
                            SynchroCommon.TableSynchroTask item = new SynchroCommon.TableSynchroTask();
                            item = DataRowToTableSynchroTask(taskDataTable.Rows[i]);
                            //UserLoginEx 特殊处理
                            if (item.TableName == "UserLoginEx" && _TableSynchroTaskList.Where(s => s.TableName == "UserLoginEx" && s.TableRowID == item.TableRowID).ToList().Count > 0)
                            {
                                continue;
                            }

                            _TableSynchroTaskList.Add(item); 

                        }
                    }
                    else
                    {
                        _TableSynchroTaskList = null;
                    }

                }
                return _TableSynchroTaskList;
            }
            catch (Exception ex)
            {
                LogService.WriteErr("程序错误，方法：GetTableSynchroTaskList 错误信息：" + ex.Message);
                return null;


            }
        }



        /// <summary>
        /// 根据任务ID删除任务
        /// </summary>
        /// <param name="pTaskID"></param>
        /// <returns></returns>
        public static bool DelTableSynchroTaskByID(string pTaskID)
        {
            using (SqlConnection conn = new SqlConnection(getTmpAppConnectionString()))
            {
                SqlCommand cmd = new SqlCommand();
                try
                {
                    conn.Open();
                    cmd.Connection = conn;
                    cmd.Transaction = conn.BeginTransaction();


                    cmd.CommandText = "delete TableSynchroTask  where  TableSynchroTaskID='" + pTaskID + "' ";
                    cmd.ExecuteNonQuery();

                    cmd.Transaction.Commit();
                    return true;
                }
                catch (System.Exception ex)
                {
                    cmd.Transaction.Rollback();
                    LogService.WriteErr("程序错误，方法：DelTableSynchroTaskByID 错误信息：" + ex.Message);
                    return false;
                }

            }
        }

        /// <summary>
        /// 根据任务ID删除任务
        /// </summary>
        /// <param name="pTaskID"></param>
        /// <returns></returns>
        public static bool UpdateTableSynchroTaskStateByID(string pTaskID, int pState)
        {
            using (SqlConnection conn = new SqlConnection(getTmpAppConnectionString()))
            {
                SqlCommand cmd = new SqlCommand();
                try
                {
                    conn.Open();
                    cmd.Connection = conn;
                    cmd.Transaction = conn.BeginTransaction();


                    cmd.CommandText = "Update TableSynchroTask   set TaskState=" + pState + " where  TableSynchroTaskID='" + pTaskID + "' ";
                    cmd.ExecuteNonQuery();

                    cmd.Transaction.Commit();
                    return true;
                }
                catch (System.Exception ex)
                {
                    cmd.Transaction.Rollback();
                    LogService.WriteErr("程序错误，方法：UpdateTableSynchroTaskStateByID 错误信息：" + ex.Message);
                    return false;
                }

            }
        }


        /// <summary>
        /// 根据t同步信息删除任务
        /// </summary>
        /// <param name="pTaskID"></param>
        /// <returns></returns>
        public static bool DelTableSynchroTaskByTableInfo(string pTableName, string pRowID)
        {
            using (SqlConnection conn = new SqlConnection(getTmpAppConnectionString()))
            {
                SqlCommand cmd = new SqlCommand();
                try
                {
                    conn.Open();
                    cmd.Connection = conn;
                    cmd.Transaction = conn.BeginTransaction();


                    cmd.CommandText = "delete TableSynchroTask  where TableName='" + pTableName + "' and TableRowID='" + pRowID + "' ";
                    cmd.ExecuteNonQuery();

                    cmd.Transaction.Commit();
                    return true;
                }
                catch (System.Exception ex)
                {
                    cmd.Transaction.Rollback();
                    LogService.WriteErr("程序错误，方法：DelTableSynchroTaskByID 错误信息：" + ex.Message);
                    return false;
                }

            }
        }

        /// <summary>
        /// 根据任务ID删除任务
        /// </summary>
        /// <param name="pTaskID"></param>
        /// <returns></returns>
        public static bool UpdateTableSynchroTaskStateByByTableInfo(string pTableName, string pRowID, int pState)
        {
            using (SqlConnection conn = new SqlConnection(getTmpAppConnectionString()))
            {
                SqlCommand cmd = new SqlCommand();
                try
                {
                    conn.Open();
                    cmd.Connection = conn;
                    cmd.Transaction = conn.BeginTransaction();


                    cmd.CommandText = "Update TableSynchroTask   set TaskState=" + pState + "  where TableName='" + pTableName + "' and TableRowID='" + pRowID + "' ";
                    cmd.ExecuteNonQuery();

                    cmd.Transaction.Commit();
                    return true;
                }
                catch (System.Exception ex)
                {
                    cmd.Transaction.Rollback();
                    LogService.WriteErr("程序错误，方法：UpdateTableSynchroTaskStateByID 错误信息：" + ex.Message);
                    return false;
                }

            }
        }


        /// <summary>
        /// 得到一个对象实体
        /// </summary>
        public static SynchroCommon.TableSynchroTask DataRowToTableSynchroTask(DataRow row)
        {
            SynchroCommon.TableSynchroTask model = new SynchroCommon.TableSynchroTask();
            if (row != null)
            {
                if (row["TableSynchroTaskID"] != null && row["TableSynchroTaskID"].ToString() != "")
                {
                    model.TableSynchroTaskID = new Guid(row["TableSynchroTaskID"].ToString());
                }
                if (row["TableName"] != null)
                {
                    model.TableName = row["TableName"].ToString();
                }
                if (row["TableRowID"] != null && row["TableRowID"].ToString() != "")
                {
                    model.TableRowID = row["TableRowID"].ToString();
                }
                if (row["OperationType"] != null && row["OperationType"].ToString() != "")
                {
                    model.OperationType = int.Parse(row["OperationType"].ToString());
                }
                if (row["TaskState"] != null && row["TaskState"].ToString() != "")
                {
                    model.TaskState = int.Parse(row["TaskState"].ToString());
                }
                if (row["AddDate"] != null && row["AddDate"].ToString() != "")
                {
                    model.AddDate = DateTime.Parse(row["AddDate"].ToString());
                }
            }
            return model;
        }

        #region //UserTable
        public static User GetUserByID(string pID)
        {
            try
            {
                User _User = new User();
                DataTable taskDataTable = new DataTable();
                using (SqlConnection conn = new SqlConnection(getTmpAppConnectionString()))
                {
                    string sql = " select * from Users where ID='" + pID + "' ";
                    using (SqlDataAdapter sd = new SqlDataAdapter(sql, conn))
                    {
                        sd.Fill(taskDataTable);
                    }
                    if (taskDataTable.Rows.Count > 0)
                    {
                        _User = DataRowToUser(taskDataTable.Rows[0]);
                    }
                    else
                    {
                        _User = null;
                    }
                }
                return _User;
            }
            catch (Exception ex)
            {
                LogService.WriteErr("程序错误，方法：GetUserByID 错误信息：" + ex.Message);
                return null;


            }
        }

        /// <summary>
        /// 得到一个对象实体
        /// </summary>
        public static User DataRowToUser(DataRow row)
        {
            User model = new User();
            if (row != null)
            {
                if (row["ID"] != null && row["ID"].ToString() != "")
                {
                    model.ID = new Guid(row["ID"].ToString());
                }
                if (row["Name"] != null)
                {
                    model.Name = row["Name"].ToString();
                }
                if (row["Code"] != null)
                {
                    model.Code = row["Code"].ToString();
                }
                if (row["Gender"] != null && row["Gender"].ToString() != "")
                {
                    if ((row["Gender"].ToString() == "1") || (row["Gender"].ToString().ToLower() == "true"))
                    {
                        model.Gender = true;
                    }
                    else
                    {
                        model.Gender = false;
                    }
                }
                if (row["LoginName"] != null)
                {
                    model.LoginName = row["LoginName"].ToString();
                }
                if (row["Password"] != null)
                {
                    model.Password = row["Password"].ToString();
                }
                if (row["Tag1"] != null)
                {
                    model.Tag1 = row["Tag1"].ToString();
                }
                if (row["Tag2"] != null)
                {
                    model.Tag2 = row["Tag2"].ToString();
                }
                if (row["Tag3"] != null)
                {
                    model.Tag3 = row["Tag3"].ToString();
                }
                if (row["Certificate"] != null)
                {
                    model.Certificate = row["Certificate"].ToString();
                }
                if (row["ForceCard"] != null && row["ForceCard"].ToString() != "")
                {
                    if ((row["ForceCard"].ToString() == "1") || (row["ForceCard"].ToString().ToLower() == "true"))
                    {
                        model.ForceCard = true;
                    }
                    else
                    {
                        model.ForceCard = false;
                    }
                }
                if (row["JobStatus"] != null && row["JobStatus"].ToString() != "")
                {
                    model.JobStatus = int.Parse(row["JobStatus"].ToString());
                }
                if (row["ICNumber1"] != null)
                {
                    model.ICNumber1 = row["ICNumber1"].ToString();
                }
                if (row["ICNumber2"] != null)
                {
                    model.ICNumber2 = row["ICNumber2"].ToString();
                }
                if (row["ICNumber3"] != null)
                {
                    model.ICNumber3 = row["ICNumber3"].ToString();
                }
                if (row["JobNumber"] != null)
                {
                    model.JobNumber = row["JobNumber"].ToString();
                }
                if (row["IDNumber"] != null)
                {
                    model.IDNumber = row["IDNumber"].ToString();
                }
                if (row["Fingerprint"] != null)
                {
                    model.Fingerprint = row["Fingerprint"].ToString();
                }
                if (row["Email1"] != null)
                {
                    model.Email1 = row["Email1"].ToString();
                }
                if (row["Email2"] != null)
                {
                    model.Email2 = row["Email2"].ToString();
                }
                if (row["Telephone1"] != null)
                {
                    model.Telephone1 = row["Telephone1"].ToString();
                }
                if (row["Telephone2"] != null)
                {
                    model.Telephone2 = row["Telephone2"].ToString();
                }
                if (row["CompanyAddr"] != null)
                {
                    model.CompanyAddr = row["CompanyAddr"].ToString();
                }
                if (row["HomeAddr"] != null)
                {
                    model.HomeAddr = row["HomeAddr"].ToString();
                }
                if (row["LastLoginDateTime"] != null && row["LastLoginDateTime"].ToString() != "")
                {
                    model.LastLoginDateTime = DateTime.Parse(row["LastLoginDateTime"].ToString());
                }
                if (row["CreateUserName"] != null)
                {
                    model.CreateUserName = row["CreateUserName"].ToString();
                }
                if (row["CreateUserID"] != null && row["CreateUserID"].ToString() != "")
                {
                    model.CreateUserID = new Guid(row["CreateUserID"].ToString());
                }
                if (row["CreateDateTime"] != null && row["CreateDateTime"].ToString() != "")
                {
                    model.CreateDateTime = DateTime.Parse(row["CreateDateTime"].ToString());
                }
                if (row["ModifyUserName"] != null)
                {
                    model.ModifyUserName = row["ModifyUserName"].ToString();
                }
                if (row["ModifyUserID"] != null && row["ModifyUserID"].ToString() != "")
                {
                    model.ModifyUserID = new Guid(row["ModifyUserID"].ToString());
                }
                if (row["ModifyDateTime"] != null && row["ModifyDateTime"].ToString() != "")
                {
                    model.ModifyDateTime = DateTime.Parse(row["ModifyDateTime"].ToString());
                }
            }
            return model;
        }

        #endregion

        #region //UserFingerInfoTable
        public static UserFingerInfo GetUserFingerInfoByID(string pID)
        {
            try
            {
                UserFingerInfo _UserFingerInfo = new UserFingerInfo();
                DataTable taskDataTable = new DataTable();
                using (SqlConnection conn = new SqlConnection(getTmpAppConnectionString()))
                {
                    string sql = " select * from UserFingerInfo where UserID='" + pID + "' ";
                    using (SqlDataAdapter sd = new SqlDataAdapter(sql, conn))
                    {
                        sd.Fill(taskDataTable);
                    }
                    if (taskDataTable.Rows.Count > 0)
                    {
                        _UserFingerInfo = DataRowToUserFingerInfo(taskDataTable.Rows[0]);
                    }
                    else
                    {
                        _UserFingerInfo = null;
                    }
                }
                return _UserFingerInfo;
            }
            catch (Exception ex)
            {
                LogService.WriteErr("程序错误，方法：GetUserFingerInfoByID 错误信息：" + ex.Message);
                return null;


            }
        }

        /// 得到一个对象实体
        /// </summary>
        public static UserFingerInfo DataRowToUserFingerInfo(DataRow row)
        {
            UserFingerInfo model = new UserFingerInfo();
            if (row != null)
            {
                if (row["UserID"] != null && row["UserID"].ToString() != "")
                {
                    model.UserID = new Guid(row["UserID"].ToString());
                }
                if (row["FingerInfoStr1"] != null)
                {
                    model.FingerInfoStr1 = row["FingerInfoStr1"].ToString();
                }
                if (row["FingerInfoStr2"] != null)
                {
                    model.FingerInfoStr2 = row["FingerInfoStr2"].ToString();
                }
                if (row["FingerInfoStr3"] != null)
                {
                    model.FingerInfoStr3 = row["FingerInfoStr3"].ToString();
                }
            }
            return model;
        }

        #endregion

        #region //UserGroups
        public static UserGroup GetUserGroupByID(string pID)
        {
            try
            {
                UserGroup _UserGroup = new UserGroup();
                DataTable taskDataTable = new DataTable();
                using (SqlConnection conn = new SqlConnection(getTmpAppConnectionString()))
                {
                    string sql = " select * from UserGroups where ID='" + pID + "' ";
                    using (SqlDataAdapter sd = new SqlDataAdapter(sql, conn))
                    {
                        sd.Fill(taskDataTable);
                    }
                    if (taskDataTable.Rows.Count > 0)
                    {
                        _UserGroup = DataRowToUserGroup(taskDataTable.Rows[0]);
                    }
                    else
                    {
                        _UserGroup = null;
                    }
                }
                return _UserGroup;
            }
            catch (Exception ex)
            {

                LogService.WriteErr("程序错误，方法：GetUserGroupByID 错误信息：" + ex.Message);
                return null;

            }
        }

        /// <summary>
        /// 得到一个对象实体
        /// </summary>
        public static UserGroup DataRowToUserGroup(DataRow row)
        {
            UserGroup model = new UserGroup();
            if (row != null)
            {
                if (row["ID"] != null && row["ID"].ToString() != "")
                {
                    model.ID = new Guid(row["ID"].ToString());
                }
                if (row["SystemID"] != null && row["SystemID"].ToString() != "")
                {
                    model.SystemID = new Guid(row["SystemID"].ToString());
                }
                if (row["Name"] != null)
                {
                    model.Name = row["Name"].ToString();
                }
                if (row["Description"] != null)
                {
                    model.Description = row["Description"].ToString();
                }
                if (row["Category"] != null && row["Category"].ToString() != "")
                {
                    model.Category = int.Parse(row["Category"].ToString());
                }
                if (row["CategoryName"] != null)
                {
                    model.CategoryName = row["CategoryName"].ToString();
                }
                if (row["SysRelNumber"] != null && row["SysRelNumber"].ToString() != "")
                {
                    model.SysRelNumber = int.Parse(row["SysRelNumber"].ToString());
                }
                if (row["CreateUserName"] != null)
                {
                    model.CreateUserName = row["CreateUserName"].ToString();
                }
                if (row["CreateUserID"] != null && row["CreateUserID"].ToString() != "")
                {
                    model.CreateUserID = new Guid(row["CreateUserID"].ToString());
                }
                if (row["CreateDateTime"] != null && row["CreateDateTime"].ToString() != "")
                {
                    model.CreateDateTime = DateTime.Parse(row["CreateDateTime"].ToString());
                }
                if (row["ModifyUserName"] != null)
                {
                    model.ModifyUserName = row["ModifyUserName"].ToString();
                }
                if (row["ModifyUserID"] != null && row["ModifyUserID"].ToString() != "")
                {
                    model.ModifyUserID = new Guid(row["ModifyUserID"].ToString());
                }
                if (row["ModifyDateTime"] != null && row["ModifyDateTime"].ToString() != "")
                {
                    model.ModifyDateTime = DateTime.Parse(row["ModifyDateTime"].ToString());
                }
            }
            return model;
        }

        #endregion

        #region //UserGroup2User
        public static UserGroup2User GetUserGroup2UserByID(string pID)
        {
            try
            {
                UserGroup2User _UserGroup2User = new UserGroup2User();
                DataTable taskDataTable = new DataTable();
                using (SqlConnection conn = new SqlConnection(getTmpAppConnectionString()))
                {
                    string sql = " select * from UserGroup2User where ID='" + pID + "' ";
                    using (SqlDataAdapter sd = new SqlDataAdapter(sql, conn))
                    {
                        sd.Fill(taskDataTable);
                    }
                    if (taskDataTable.Rows.Count > 0)
                    {
                        _UserGroup2User = DataRowToUserGroup2User(taskDataTable.Rows[0]);
                    }
                    else
                    {
                        _UserGroup2User = null;
                    }
                }
                return _UserGroup2User;
            }
            catch (Exception ex)
            {
                LogService.WriteErr("程序错误，方法：GetUserGroup2UserByID 错误信息：" + ex.Message);
                return null;


            }
        }


        /// <summary>
        /// 得到一个对象实体
        /// </summary>
        public static UserGroup2User DataRowToUserGroup2User(DataRow row)
        {
            UserGroup2User model = new UserGroup2User();
            if (row != null)
            {
                if (row["ID"] != null && row["ID"].ToString() != "")
                {
                    model.ID = new Guid(row["ID"].ToString());
                }
                if (row["UserGroupID"] != null && row["UserGroupID"].ToString() != "")
                {
                    model.UserGroupID = new Guid(row["UserGroupID"].ToString());
                }
                if (row["UserID"] != null && row["UserID"].ToString() != "")
                {
                    model.UserID = new Guid(row["UserID"].ToString());
                }
            }
            return model;
        }

        #endregion

        #region //UserGroup2UserGroup
        public static UserGroup2UserGroup GetUserGroup2UserGroupByID(string pID)
        {
            try
            {
                UserGroup2UserGroup _UserGroup2UserGroup = new UserGroup2UserGroup();
                DataTable taskDataTable = new DataTable();
                using (SqlConnection conn = new SqlConnection(getTmpAppConnectionString()))
                {
                    string sql = " select * from UserGroup2UserGroup where ID='" + pID + "' ";
                    using (SqlDataAdapter sd = new SqlDataAdapter(sql, conn))
                    {
                        sd.Fill(taskDataTable);
                    }
                    if (taskDataTable.Rows.Count > 0)
                    {
                        _UserGroup2UserGroup = DataRowToUserGroup2UserGroup(taskDataTable.Rows[0]);
                    }
                    else
                    {
                        _UserGroup2UserGroup = null;
                    }
                }
                return _UserGroup2UserGroup;
            }
            catch (Exception ex)
            {
                LogService.WriteErr("程序错误，方法：GetUserGroup2UserGroupByID 错误信息：" + ex.Message);
                return null;

            }
        }

        /// <summary>
        /// 得到一个对象实体
        /// </summary>
        public static UserGroup2UserGroup DataRowToUserGroup2UserGroup(DataRow row)
        {
            UserGroup2UserGroup model = new UserGroup2UserGroup();
            if (row != null)
            {
                if (row["ID"] != null && row["ID"].ToString() != "")
                {
                    model.ID = new Guid(row["ID"].ToString());
                }
                if (row["SuperiorID"] != null && row["SuperiorID"].ToString() != "")
                {
                    model.SuperiorID = new Guid(row["SuperiorID"].ToString());
                }
                if (row["LowerID"] != null && row["LowerID"].ToString() != "")
                {
                    model.LowerID = new Guid(row["LowerID"].ToString());
                }
            }
            return model;
        }
        #endregion

        #region //UserLogin
        public static UserLogin GetUserLoginByUserID(string pUserID)
        {
            try
            {
                UserLogin _UserLogin = new UserLogin();
                DataTable taskDataTable = new DataTable();
                using (SqlConnection conn = new SqlConnection(getTmpAppConnectionString()))
                {
                    string sql = " select * from UserLogin where UserID='" + pUserID + "' ";
                    using (SqlDataAdapter sd = new SqlDataAdapter(sql, conn))
                    {
                        sd.Fill(taskDataTable);
                    }
                    if (taskDataTable.Rows.Count > 0)
                    {
                        _UserLogin = DataRowUserLogin(taskDataTable.Rows[0]);
                    }
                    else
                    {
                        _UserLogin = null;
                    }
                }
                return _UserLogin;
            }
            catch (Exception ex)
            {
                LogService.WriteErr("程序错误，方法：GetUserLoginByUserID 错误信息：" + ex.Message);
                return null;

            }
        }

        /// <summary>
        /// 得到一个对象实体
        /// </summary>
        public static UserLogin DataRowUserLogin(DataRow row)
        {
            UserLogin model = new UserLogin();
            if (row != null)
            {
                if (row["UserID"] != null && row["UserID"].ToString() != "")
                {
                    model.UserID = new Guid(row["UserID"].ToString());
                }
                if (row["LoginTime"] != null && row["LoginTime"].ToString() != "")
                {
                    model.LoginTime = DateTime.Parse(row["LoginTime"].ToString());
                }
                if (row["ComputerName"] != null && row["ComputerName"].ToString() != "")
                {
                    model.ComputerName = row["ComputerName"].ToString();
                }
                if (row["IPAddress"] != null && row["IPAddress"].ToString() != "")
                {
                    model.IPAddress = row["IPAddress"].ToString();
                }
            }
            return model;
        }
        #endregion


        #region //UserLoginEx
        public static UserLoginEx GetUserLoginExByUserName(string pUserLoginName)
        {
            try
            {
                UserLoginEx _UserLoginEx = new UserLoginEx();
                DataTable taskDataTable = new DataTable();
                using (SqlConnection conn = new SqlConnection(getTmpAppConnectionString()))
                {
                    string sql = " select * from UserLoginEx where UserLoginName='" + pUserLoginName + "' ";
                    using (SqlDataAdapter sd = new SqlDataAdapter(sql, conn))
                    {
                        sd.Fill(taskDataTable);
                    }
                    if (taskDataTable.Rows.Count > 0)
                    {
                        _UserLoginEx = DataRowUserLoginEx(taskDataTable.Rows[0]);
                    }
                    else
                    {
                        _UserLoginEx = null;
                    }
                }
                return _UserLoginEx;
            }
            catch (Exception ex)
            {
                LogService.WriteErr("程序错误，方法：GetUserLoginExByUserName 错误信息：" + ex.Message);
                return null;

            }
        }
        public static UserLoginEx[] GetUserLoginListExByUserName(string pUserLoginName)
        {
            try
            {
                UserLoginEx[] _UserLoginExList = null;
                DataTable taskDataTable = new DataTable();
                using (SqlConnection conn = new SqlConnection(getTmpAppConnectionString()))
                {
                    string sql = " select * from UserLoginEx where UserLoginName='" + pUserLoginName + "' ";
                    using (SqlDataAdapter sd = new SqlDataAdapter(sql, conn))
                    {
                        sd.Fill(taskDataTable);
                    }
                    if (taskDataTable.Rows.Count > 0)
                    {
                        _UserLoginExList = new UserLoginEx[taskDataTable.Rows.Count];
                        for (int i = 0; i < taskDataTable.Rows.Count; i++)
                        {
                            _UserLoginExList[i] = DataRowUserLoginEx(taskDataTable.Rows[i]);
                        }

                    }
                    else
                    {
                        _UserLoginExList = null;
                    }
                }
                return _UserLoginExList;
            }
            catch (Exception ex)
            {
                LogService.WriteErr("程序错误，方法：GetUserLoginListExByUserName 错误信息：" + ex.Message);
                return null;

            }
        }

        /// <summary>
        /// 得到一个对象实体
        /// </summary>
        public static UserLoginEx DataRowUserLoginEx(DataRow row)
        {
            UserLoginEx model = new UserLoginEx();
            if (row != null)
            {
                if (row["UserLoginName"] != null && row["UserLoginName"].ToString() != "")
                {
                    model.UserLoginName = row["UserLoginName"].ToString();
                }

                if (row["UserLoginEx1"] != null && row["UserLoginEx1"].ToString() != "")
                {
                    model.UserLoginEx1 = row["UserLoginEx1"].ToString();
                }
                else
                {
                    model.UserLoginEx1 = "";
                }
                if (row["UserLoginEx2"] != null && row["UserLoginEx2"].ToString() != "")
                {
                    model.UserLoginEx2 = row["UserLoginEx2"].ToString();
                }
                else
                {
                    model.UserLoginEx2 = "";
                }
                if (row["UserLoginEx3"] != null && row["UserLoginEx3"].ToString() != "")
                {
                    model.UserLoginEx3 = row["UserLoginEx3"].ToString();
                }
                else
                {
                    model.UserLoginEx3 = "";
                }
                if (row["UserLoginEx4"] != null && row["UserLoginEx4"].ToString() != "")
                {
                    model.UserLoginEx4 = row["UserLoginEx4"].ToString();
                }
                else
                {
                    model.UserLoginEx4 = "";
                }
                if (row["UserLoginEx5"] != null && row["UserLoginEx5"].ToString() != "")
                {
                    model.UserLoginEx5 = row["UserLoginEx5"].ToString();
                }
                else
                {
                    model.UserLoginEx5 = "";
                }
            }
            return model;
        }
        #endregion

    }
}
