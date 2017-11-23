using SynchroCommon;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Text;
using System.Web;
using System.Web.Services;

namespace UserSynchroWebService
{
    /// <summary>
    /// UserSynchroWebService 的摘要说明
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]

    public class UserSynchroWebService : System.Web.Services.WebService
    {
        private const string LOG_DIR = "C:\\Infomedia\\Logs\\UserSynchroLogs";
        public UserSynchroWebService()
        {

            //Uncomment the following line if using designed components 
            //InitializeComponent(); 
        }
        [WebMethod]
        public string HelloWorld()
        {
            return "Hello World";
        }

        //[WebMethod]
        //public void WriteErrorTest()
        //{
        //    Exception ex = new Exception();
        //    WriteError(ex);
        //}

        /// <summary>
        /// 获取根服务器连接字符串
        /// </summary>
        /// <returns></returns>
        private string getTmpAppConnectionString()
        {
            string connStr = "";
            try
            {
                connStr = System.Configuration.ConfigurationManager.AppSettings.Get("primaryConnection").ToString();
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

        protected void WriteError(Exception ex)
        {
            try
            {
                if (!Directory.Exists(LOG_DIR))
                    Directory.CreateDirectory(LOG_DIR);

                StreamWriter sw = new StreamWriter(LOG_DIR + "\\Err[" + DateTime.Now.ToString("yyyy - MM - dd") + "].log", true);
                sw.WriteLine("\r\n");
                sw.WriteLine("\r\n-------------------------------------------------------------------------------");
                sw.WriteLine("\r\n" + DateTime.Now.ToString());
                sw.WriteLine("\r\n当前异常的消息： " + ex.Message);
                sw.WriteLine("\r\n错误对象的名称： " + ex.Source);
                sw.WriteLine("\r\n当前异常发生时调用堆栈：" + ex.StackTrace);
                sw.WriteLine("\r\n-------------------------------------------------------------------------------");
                sw.Flush();
                sw.Close();
            }
            catch { }
        }


        #region //UserTable

        [WebMethod]
        public bool UpdateUser(User ur)
        {
            using (SqlConnection conn = new SqlConnection(this.getTmpAppConnectionString()))
            {

                SqlTransaction tran = null;
                SqlCommand cmd = new SqlCommand();
                try
                {
                    conn.Open();
                    tran = conn.BeginTransaction(IsolationLevel.ReadUncommitted);
                    StringBuilder sql = new StringBuilder();
                    sql.Append(" IF EXISTS (SELECT ID FROM [USERS] WHERE [LoginName] = @LoginName)\r\n");
                    sql.Append("    BEGIN\r\n");
                    sql.Append("        UPDATE [USERS]\r\n");
                    sql.Append("        SET\r\n");
                    sql.Append("            Name = @Name\r\n");
                    sql.Append("           ,Password = @Password\r\n");
                    sql.Append("           ,Code = @Code\r\n");
                    sql.Append("           ,Gender = @Gender\r\n");
                    sql.Append("           ,IDNumber = @IDNumber\r\n");
                    sql.Append("           ,Certificate = @Certificate\r\n");
                    sql.Append("           ,ForceCard = @ForceCard\r\n");
                    sql.Append("           ,JobStatus = @JobStatus\r\n");
                    sql.Append("           ,ICNumber1 = @ICNumber1\r\n");
                    sql.Append("           ,ICNumber2 = @ICNumber2\r\n");
                    sql.Append("           ,ICNumber3 = @ICNumber3\r\n");
                    sql.Append("           ,JobNumber = @JobNumber\r\n");
                    sql.Append("           ,Fingerprint = @Fingerprint\r\n");
                    sql.Append("           ,Email1 = @Email1\r\n");
                    sql.Append("           ,Email2 = @Email2\r\n");
                    sql.Append("           ,Telephone1 = @Telephone1\r\n");
                    sql.Append("           ,Telephone2 = @Telephone2\r\n");
                    sql.Append("           ,Tag1 = @Tag1\r\n");
                    sql.Append("           ,Tag2 = @Tag2\r\n");
                    sql.Append("           ,ModifyUserName = @ModifyUserName\r\n");
                    sql.Append("           ,ModifyUserID = @ModifyUserID\r\n");
                    sql.Append("           ,ModifyDateTime = @ModifyDateTime\r\n");
                    sql.Append("        WHERE\r\n");
                    sql.Append("            [LoginName] = @LoginName\r\n");
                    sql.Append("    END\r\n");
                    sql.Append(" ELSE\r\n");
                    sql.Append("    BEGIN\r\n");
                    sql.Append("    INSERT INTO [USERS]\r\n");
                    sql.Append("        ([ID]\r\n");
                    sql.Append("        ,[Name]\r\n");
                    sql.Append("        ,[Code]\r\n");
                    sql.Append("        ,[Gender]\r\n");
                    sql.Append("        ,[LoginName]\r\n");
                    sql.Append("        ,[Password]\r\n");
                    sql.Append("        ,[Tag1]\r\n");
                    sql.Append("        ,[Tag2]\r\n");
                    sql.Append("        ,[Tag3]\r\n");
                    sql.Append("        ,[Certificate]\r\n");
                    sql.Append("        ,[ForceCard]\r\n");
                    sql.Append("        ,[JobStatus]\r\n");
                    sql.Append("        ,[ICNumber1]\r\n");
                    sql.Append("        ,[ICNumber2]\r\n");
                    sql.Append("        ,[ICNumber3]\r\n");
                    sql.Append("        ,[JobNumber]\r\n");
                    sql.Append("        ,[IDNumber]\r\n");
                    sql.Append("        ,[Fingerprint]\r\n");
                    sql.Append("        ,[Email1]\r\n");
                    sql.Append("        ,[Email2]\r\n");
                    sql.Append("        ,[Telephone1]\r\n");
                    sql.Append("        ,[Telephone2]\r\n");
                    sql.Append("        ,[CompanyAddr]\r\n");
                    sql.Append("        ,[HomeAddr]\r\n");
                    sql.Append("        ,[LastLoginDateTime]\r\n");
                    sql.Append("        ,[CreateUserName]\r\n");
                    sql.Append("        ,[CreateUserID]\r\n");
                    sql.Append("        ,[CreateDateTime]\r\n");
                    sql.Append("        ,[ModifyUserName]\r\n");
                    sql.Append("        ,[ModifyUserID]\r\n");
                    sql.Append("        ,[ModifyDateTime])\r\n");
                    sql.Append("    VALUES\r\n");
                    sql.Append("        (@ID\r\n");
                    sql.Append("        ,@Name\r\n");
                    sql.Append("        ,@Code\r\n");
                    sql.Append("        ,@Gender\r\n");
                    sql.Append("        ,@LoginName\r\n");
                    sql.Append("        ,@Password\r\n");
                    sql.Append("        ,@Tag1\r\n");
                    sql.Append("        ,@Tag2\r\n");
                    sql.Append("        ,@Tag3\r\n");
                    sql.Append("        ,@Certificate\r\n");
                    sql.Append("        ,@ForceCard\r\n");
                    sql.Append("        ,@JobStatus\r\n");
                    sql.Append("        ,@ICNumber1\r\n");
                    sql.Append("        ,@ICNumber2\r\n");
                    sql.Append("        ,@ICNumber3\r\n");
                    sql.Append("        ,@JobNumber\r\n");
                    sql.Append("        ,@IDNumber\r\n");
                    sql.Append("        ,@Fingerprint\r\n");
                    sql.Append("        ,@Email1\r\n");
                    sql.Append("        ,@Email2\r\n");
                    sql.Append("        ,@Telephone1\r\n");
                    sql.Append("        ,@Telephone2\r\n");
                    sql.Append("        ,@CompanyAddr\r\n");
                    sql.Append("        ,@HomeAddr\r\n");
                    sql.Append("        ,@LastLoginDateTime\r\n");
                    sql.Append("        ,@CreateUserName\r\n");
                    sql.Append("        ,@CreateUserID\r\n");
                    sql.Append("        ,@CreateDateTime\r\n");
                    sql.Append("        ,@ModifyUserName\r\n");
                    sql.Append("        ,@ModifyUserID\r\n");
                    sql.Append("        ,@ModifyDateTime)\r\n");
                    sql.Append("    END\r\n");
                    cmd.CommandText = sql.ToString();
                    cmd.CommandType = CommandType.Text;
                    cmd.Connection = conn;
                    cmd.Transaction = tran;
                    cmd.Parameters.Clear();
                    cmd.Parameters.AddWithValue("@ID", ur.ID);
                    cmd.Parameters.AddWithValue("@Name", ur.Name);
                    cmd.Parameters.AddWithValue("@Code", ur.Code);
                    cmd.Parameters.AddWithValue("@Gender", ur.Gender);
                    cmd.Parameters.AddWithValue("@LoginName", ur.LoginName);
                    cmd.Parameters.AddWithValue("@Password", ur.Password);
                    cmd.Parameters.AddWithValue("@Tag1", ur.Tag1);
                    cmd.Parameters.AddWithValue("@Tag2", ur.Tag2);
                    cmd.Parameters.AddWithValue("@Tag3", ur.Tag3);
                    cmd.Parameters.AddWithValue("@Certificate", ur.Certificate);
                    cmd.Parameters.AddWithValue("@ForceCard", ur.ForceCard);
                    cmd.Parameters.AddWithValue("@JobStatus", ur.JobStatus);
                    cmd.Parameters.AddWithValue("@ICNumber1", ur.ICNumber1);
                    cmd.Parameters.AddWithValue("@ICNumber2", ur.ICNumber2);
                    cmd.Parameters.AddWithValue("@ICNumber3", ur.ICNumber3);
                    cmd.Parameters.AddWithValue("@JobNumber", ur.JobNumber);
                    cmd.Parameters.AddWithValue("@IDNumber", ur.IDNumber);
                    cmd.Parameters.AddWithValue("@Fingerprint", ur.Fingerprint);
                    cmd.Parameters.AddWithValue("@Email1", ur.Email1);
                    cmd.Parameters.AddWithValue("@Email2", ur.Email2);
                    cmd.Parameters.AddWithValue("@Telephone1", ur.Telephone1);
                    cmd.Parameters.AddWithValue("@Telephone2", ur.Telephone2);
                    cmd.Parameters.AddWithValue("@CompanyAddr", ur.CompanyAddr);
                    cmd.Parameters.AddWithValue("@HomeAddr", ur.HomeAddr);
                    cmd.Parameters.AddWithValue("@LastLoginDateTime", ur.LastLoginDateTime);
                    cmd.Parameters.AddWithValue("@CreateUserName", ur.CreateUserName);
                    cmd.Parameters.AddWithValue("@CreateUserID", ur.CreateUserID);
                    cmd.Parameters.AddWithValue("@CreateDateTime", ur.CreateDateTime);
                    cmd.Parameters.AddWithValue("@ModifyUserName", ur.ModifyUserName);
                    cmd.Parameters.AddWithValue("@ModifyUserID", ur.ModifyUserID);
                    cmd.Parameters.AddWithValue("@ModifyDateTime", ur.ModifyDateTime);
                    cmd.ExecuteNonQuery();

                    tran.Commit();

                    return true;
                }
                catch (Exception ex)
                {
                    tran.Rollback();
                    WriteError(ex);
                    return false;
                }
                finally
                {
                    if (conn.State == ConnectionState.Open)
                    {
                        conn.Close();
                    }

                }
            }
        }

        [WebMethod]
        public bool DeleteUserByID(string userID)
        {
            using (SqlConnection conn = new SqlConnection(this.getTmpAppConnectionString()))
            {

                SqlTransaction tran = null;
                try
                {
                    // 删除 Users 表
                    SqlCommand cmd = new SqlCommand();
                    cmd.Connection = conn;
                    conn.Open();
                    tran = conn.BeginTransaction(IsolationLevel.ReadUncommitted);
                    cmd.Transaction = tran;
                    cmd.CommandText = "DELETE FROM [Users] WHERE [ID] = @Original_ID";
                    cmd.CommandType = CommandType.Text;
                    cmd.Parameters.Clear();
                    cmd.Parameters.AddWithValue("@Original_ID", userID);
                    cmd.ExecuteNonQuery();

                    // 删除 UserGroup2User 表
                    cmd = new SqlCommand();
                    cmd.Connection = conn;
                    cmd.Transaction = tran;
                    cmd.CommandText = "DELETE FROM [UserGroup2User] WHERE UserID = @Original_UserID";
                    cmd.CommandType = CommandType.Text;
                    cmd.Parameters.Clear();
                    cmd.Parameters.AddWithValue("@Original_UserID", userID);
                    cmd.ExecuteNonQuery();

                    // 删除 Resource2User 表
                    cmd = new SqlCommand();
                    cmd.Connection = conn;
                    cmd.Transaction = tran;
                    cmd.CommandText = "DELETE FROM [Resource2User] WHERE UserID = @Original_UserID";
                    cmd.CommandType = CommandType.Text;
                    cmd.Parameters.Clear();
                    cmd.Parameters.AddWithValue("@Original_UserID", userID);
                    cmd.ExecuteNonQuery();

                    // 删除 Role2User 表
                    cmd = new SqlCommand();
                    cmd.Connection = conn;
                    cmd.Transaction = tran;
                    cmd.CommandText = "DELETE FROM [Role2User] WHERE UserID = @Original_UserID";
                    cmd.CommandType = CommandType.Text;
                    cmd.Parameters.Clear();
                    cmd.Parameters.AddWithValue("@Original_UserID", userID);
                    cmd.ExecuteNonQuery();

                    // 删除 Right2User 表
                    cmd = new SqlCommand();
                    cmd.Connection = conn;
                    cmd.Transaction = tran;
                    cmd.CommandText = "DELETE FROM [Right2User] WHERE UserID = @Original_UserID";
                    cmd.CommandType = CommandType.Text;
                    cmd.Parameters.Clear();
                    cmd.Parameters.AddWithValue("@Original_UserID", userID);
                    cmd.ExecuteNonQuery();

                    // 删除 Authorizations 表
                    cmd = new SqlCommand();
                    cmd.Connection = conn;
                    cmd.Transaction = tran;
                    cmd.CommandText = "DELETE FROM [Authorizations] WHERE UserID = @Original_UserID AND IsUserGroup = 'false'";
                    cmd.CommandType = CommandType.Text;
                    cmd.Parameters.Clear();
                    cmd.Parameters.AddWithValue("@Original_UserID", userID);
                    cmd.ExecuteNonQuery();

                    // 提交
                    tran.Commit();
                    return true;
                }
                catch (Exception ex)
                {
                    tran.Rollback();
                    WriteError(ex);
                    return false;
                }
                finally
                {

                    if (conn.State == ConnectionState.Open)
                        conn.Close();
                }
            }
        }
        #endregion


        #region //UserFingerInfoTable

        [WebMethod]
        public bool UpdateUserFingerInfo(UserFingerInfo ur)
        {
            using (SqlConnection conn = new SqlConnection(this.getTmpAppConnectionString()))
            {

                SqlTransaction tran = null;
                SqlCommand cmd = new SqlCommand();
                try
                {
                    conn.Open();
                    tran = conn.BeginTransaction(IsolationLevel.ReadUncommitted);
                    StringBuilder sql = new StringBuilder();
                    sql.Append(" IF EXISTS (SELECT UserID FROM [UserFingerInfo] WHERE [UserID] = @UserID)\r\n");
                    sql.Append("    BEGIN\r\n");
                    sql.Append("        UPDATE [UserFingerInfo]\r\n");
                    sql.Append("        SET\r\n");
                    sql.Append("            UserID = @UserID\r\n");
                    sql.Append("           ,FingerInfoStr1 = @FingerInfoStr1\r\n");
                    sql.Append("           ,FingerInfoStr2 = @FingerInfoStr2\r\n");
                    sql.Append("           ,FingerInfoStr3 = @FingerInfoStr3\r\n");
                    sql.Append("        WHERE\r\n");
                    sql.Append("            [UserID] = @UserID\r\n");
                    sql.Append("    END\r\n");
                    sql.Append(" ELSE\r\n");
                    sql.Append("    BEGIN\r\n");
                    sql.Append("    INSERT INTO [UserFingerInfo]\r\n");
                    sql.Append("        ([UserID]\r\n");
                    sql.Append("        ,[FingerInfoStr1]\r\n");
                    sql.Append("        ,[FingerInfoStr2]\r\n");
                    sql.Append("        ,[FingerInfoStr3])\r\n");
                    sql.Append("    VALUES\r\n");
                    sql.Append("        (@UserID\r\n");
                    sql.Append("        ,@FingerInfoStr1\r\n");
                    sql.Append("        ,@FingerInfoStr2\r\n");
                    sql.Append("        ,@FingerInfoStr3)\r\n");
                    sql.Append("    END\r\n");
                    cmd.CommandText = sql.ToString();
                    cmd.CommandType = CommandType.Text;
                    cmd.Connection = conn;
                    cmd.Transaction = tran;
                    cmd.Parameters.Clear();
                    cmd.Parameters.AddWithValue("@UserID", ur.UserID);
                    cmd.Parameters.AddWithValue("@FingerInfoStr1", ur.FingerInfoStr1);
                    cmd.Parameters.AddWithValue("@FingerInfoStr2", ur.FingerInfoStr2);
                    cmd.Parameters.AddWithValue("@FingerInfoStr3", ur.FingerInfoStr3);
                    cmd.ExecuteNonQuery();

                    tran.Commit();

                    return true;
                }
                catch (Exception ex)
                {
                    tran.Rollback();
                    WriteError(ex);
                    return false;
                }
                finally
                {
                    if (conn.State == ConnectionState.Open)
                    {
                        conn.Close();
                    }

                }
            }
        }

        [WebMethod]
        public bool DeleteUserFingerInfoByID(string userID)
        {
            using (SqlConnection conn = new SqlConnection(this.getTmpAppConnectionString()))
            {

                SqlTransaction tran = null;
                try
                {
                    // 删除 UserFingerInfo 表
                    SqlCommand cmd = new SqlCommand();
                    cmd.Connection = conn;
                    conn.Open();
                    tran = conn.BeginTransaction(IsolationLevel.ReadUncommitted);
                    cmd.Transaction = tran;
                    cmd.CommandText = "DELETE FROM [UserFingerInfo] WHERE [UserID] = @Original_ID";
                    cmd.CommandType = CommandType.Text;
                    cmd.Parameters.Clear();
                    cmd.Parameters.AddWithValue("@Original_ID", userID);
                    cmd.ExecuteNonQuery();

                    // 提交
                    tran.Commit();
                    return true;
                }
                catch (Exception ex)
                {
                    tran.Rollback();
                    WriteError(ex);
                    return false;
                }
                finally
                {

                    if (conn.State == ConnectionState.Open)
                        conn.Close();
                }
            }
        }
        #endregion

        #region //UserGroups
        [WebMethod]
        public bool UpdateUserGroups(UserGroup ur)
        {
            using (SqlConnection conn = new SqlConnection(this.getTmpAppConnectionString()))
            {

                SqlTransaction tran = null;
                SqlCommand cmd = new SqlCommand();
                try
                {
                    conn.Open();
                    tran = conn.BeginTransaction(IsolationLevel.ReadUncommitted);
                    StringBuilder sql = new StringBuilder();
                    sql.Append(" IF EXISTS (SELECT ID FROM [UserGroups] WHERE [ID] = @ID)\r\n");
                    sql.Append("    BEGIN\r\n");
                    sql.Append(" UPDATE [UserGroups]\r\n");
                    sql.Append(" SET\r\n");
                    sql.Append("     Name = @Name,\r\n");
                    sql.Append("     SysRelNumber = @SysRelNumber,\r\n");
                    sql.Append("     Description = @Description,\r\n");
                    sql.Append("     ModifyUserName = @ModifyUserName,\r\n");
                    sql.Append("     ModifyUserID = @ModifyUserID,\r\n");
                    sql.Append("     ModifyDateTime = @ModifyDateTime\r\n");
                    sql.Append(" WHERE\r\n");
                    sql.Append("     ID = @ID\r\n");
                    sql.Append("    END\r\n");
                    sql.Append(" ELSE\r\n");
                    sql.Append("    BEGIN\r\n");
                    sql.Append("  INSERT INTO [UserGroups]\r\n");
                    sql.Append("     ([ID]\r\n");
                    sql.Append("     ,[SystemID]\r\n");
                    sql.Append("     ,[Name]\r\n");
                    sql.Append("     ,[Description]\r\n");
                    sql.Append("     ,[Category]\r\n");
                    sql.Append("     ,[CategoryName]\r\n");
                    sql.Append("     ,[SysRelNumber]\r\n");
                    sql.Append("     ,[CreateUserName]\r\n");
                    sql.Append("     ,[CreateUserID]\r\n");
                    sql.Append("     ,[CreateDateTime]\r\n");
                    sql.Append("     ,[ModifyUserName]\r\n");
                    sql.Append("     ,[ModifyUserID]\r\n");
                    sql.Append("     ,[ModifyDateTime])\r\n");
                    sql.Append("  VALUES\r\n");
                    sql.Append("      (@ID\r\n");
                    sql.Append("      ,@SystemID\r\n");
                    sql.Append("      ,@Name\r\n");
                    sql.Append("      ,@Description\r\n");
                    sql.Append("      ,@Category\r\n");
                    sql.Append("      ,@CategoryName\r\n");
                    sql.Append("      ,@SysRelNumber\r\n");
                    sql.Append("      ,@CreateUserName\r\n");
                    sql.Append("      ,@CreateUserID\r\n");
                    sql.Append("      ,@CreateDateTime\r\n");
                    sql.Append("      ,@ModifyUserName\r\n");
                    sql.Append("      ,@ModifyUserID\r\n");
                    sql.Append("      ,@ModifyDateTime)\r\n");
                    sql.Append("    END\r\n");
                    cmd.CommandText = sql.ToString();
                    cmd.CommandType = CommandType.Text;
                    cmd.Connection = conn;
                    cmd.Transaction = tran;
                    cmd.Parameters.Clear();
                    cmd.Parameters.AddWithValue("@ID", ur.ID);
                    cmd.Parameters.AddWithValue("@SystemID", ur.SystemID);
                    cmd.Parameters.AddWithValue("@Name", ur.Name);
                    cmd.Parameters.AddWithValue("@Description", ur.Description);
                    cmd.Parameters.AddWithValue("@Category", ur.Category);
                    cmd.Parameters.AddWithValue("@CategoryName", ur.CategoryName);
                    cmd.Parameters.AddWithValue("@SysRelNumber", ur.SysRelNumber);
                    cmd.Parameters.AddWithValue("@CreateUserName", ur.CreateUserName);
                    cmd.Parameters.AddWithValue("@CreateUserID", ur.CreateUserID);
                    cmd.Parameters.AddWithValue("@CreateDateTime", ur.CreateDateTime);
                    cmd.Parameters.AddWithValue("@ModifyUserName", ur.CreateUserName);
                    cmd.Parameters.AddWithValue("@ModifyUserID", ur.CreateUserID);
                    cmd.Parameters.AddWithValue("@ModifyDateTime", ur.ModifyDateTime);
                    cmd.ExecuteNonQuery();

                    tran.Commit();

                    return true;
                }
                catch (Exception ex)
                {
                    tran.Rollback();
                    WriteError(ex);
                    return false;
                }
                finally
                {
                    if (conn.State == ConnectionState.Open)
                    {
                        conn.Close();
                    }

                }
            }
        }
        [WebMethod]
        public bool DeleteUserGroup(string userGroupID)
        {
            using (SqlConnection conn = new SqlConnection(this.getTmpAppConnectionString()))
            {
                SqlTransaction tran = null;
                try
                {
                    // 删除 UserGroups 表
                    SqlCommand cmd = new SqlCommand();
                    conn.Open();
                    tran = conn.BeginTransaction(IsolationLevel.ReadUncommitted);
                    cmd.Connection = conn;
                    cmd.Transaction = tran;
                    cmd.CommandText = "DELETE FROM [UserGroups] WHERE [ID] = @Original_ID";
                    cmd.CommandType = CommandType.Text;
                    cmd.Parameters.Clear();
                    cmd.Parameters.AddWithValue("@Original_ID", userGroupID);
                    cmd.ExecuteNonQuery();

                    // 删除 UserGroups2UserGroup 表
                    cmd = new SqlCommand();
                    cmd.Connection = conn;
                    cmd.Transaction = tran;
                    cmd.CommandText = "DELETE FROM [UserGroup2UserGroup] WHERE [LowerID] = @Original_ID";
                    cmd.CommandType = CommandType.Text;
                    cmd.Parameters.Clear();
                    cmd.Parameters.AddWithValue("@Original_ID", userGroupID);
                    cmd.ExecuteNonQuery();

                    // 删除 UserGroups2User 表
                    cmd = new SqlCommand();
                    cmd.Connection = conn;
                    cmd.Transaction = tran;
                    cmd.CommandText = "DELETE FROM [UserGroup2User] WHERE [UserGroupID] = @Original_ID";
                    cmd.CommandType = CommandType.Text;
                    cmd.Parameters.Clear();
                    cmd.Parameters.AddWithValue("@Original_ID", userGroupID);
                    cmd.ExecuteNonQuery();

                    // 删除 Resources 表
                    cmd = new SqlCommand();
                    cmd.Connection = conn;
                    cmd.Transaction = tran;
                    cmd.CommandText = "DELETE FROM [Resources] WHERE [ID] = @ID";
                    cmd.CommandType = CommandType.Text;
                    cmd.Parameters.Clear();
                    cmd.Parameters.AddWithValue("@ID", userGroupID);
                    cmd.ExecuteNonQuery();

                    // 删除 Resource2Right 表
                    cmd = new SqlCommand();
                    cmd.Connection = conn;
                    cmd.Transaction = tran;
                    cmd.CommandText = "DELETE FROM [Resource2Right] WHERE [ResourceID] = @ResourceID";
                    cmd.CommandType = CommandType.Text;
                    cmd.Parameters.Clear();
                    cmd.Parameters.AddWithValue("@ResourceID", userGroupID);
                    cmd.ExecuteNonQuery();

                    // 删除 Resource2User 表
                    cmd = new SqlCommand();
                    cmd.Connection = conn;
                    cmd.Transaction = tran;
                    cmd.CommandText = "DELETE FROM [Resource2User] WHERE [ResourceID] = @ResourceID";
                    cmd.CommandType = CommandType.Text;
                    cmd.Parameters.Clear();
                    cmd.Parameters.AddWithValue("@ResourceID", userGroupID);
                    cmd.ExecuteNonQuery();

                    // 删除 Resource2Role 表
                    cmd = new SqlCommand();
                    cmd.Connection = conn;
                    cmd.Transaction = tran;
                    cmd.CommandText = "DELETE FROM [Resource2Role] WHERE [ResourceID] = @ResourceID";
                    cmd.CommandType = CommandType.Text;
                    cmd.Parameters.Clear();
                    cmd.Parameters.AddWithValue("@ResourceID", userGroupID);
                    cmd.ExecuteNonQuery();

                    // 删除 Authorizations 表
                    cmd = new SqlCommand();
                    cmd.Connection = conn;
                    cmd.Transaction = tran;
                    cmd.CommandText = "DELETE FROM [Authorizations] WHERE [UserID] = @Original_ID and IsUserGroup = 'true'";
                    cmd.CommandType = CommandType.Text;
                    cmd.Parameters.Clear();
                    cmd.Parameters.AddWithValue("@Original_ID", userGroupID);
                    cmd.ExecuteNonQuery();
                    cmd = new SqlCommand();
                    cmd.Connection = conn;
                    cmd.Transaction = tran;
                    cmd.CommandText = "DELETE FROM [Authorizations] WHERE [ResourceID] = @ResourceID";
                    cmd.CommandType = CommandType.Text;
                    cmd.Parameters.Clear();
                    cmd.Parameters.AddWithValue("@ResourceID", userGroupID);
                    cmd.ExecuteNonQuery();

                    tran.Commit();
                    return true;
                }
                catch (Exception ex)
                {
                    tran.Rollback();
                    WriteError(ex);
                    return false;
                }
                finally
                {
                    if (conn.State == ConnectionState.Open)
                        conn.Close();
                }
            }
        }
        #endregion

        #region //UserGroup2User
        [WebMethod]
        public bool UpdateUserGroup2User(UserGroup2User ur)
        {
            using (SqlConnection conn = new SqlConnection(this.getTmpAppConnectionString()))
            {
                SqlTransaction tran = null;
                SqlCommand cmd = new SqlCommand();
                try
                {
                    conn.Open();
                    tran = conn.BeginTransaction(IsolationLevel.ReadUncommitted);
                    StringBuilder sql = new StringBuilder();
                    sql.Append(" IF EXISTS (SELECT ID FROM [UserGroup2User] WHERE ID = @ID )\r\n");
                    sql.Append("    BEGIN\r\n");
                    sql.Append(" UPDATE [UserGroup2User]\r\n");
                    sql.Append(" SET\r\n");
                    sql.Append("     UserGroupID = @UserGroupID ,\r\n");
                    sql.Append("     UserID = @UserID\r\n");
                    sql.Append(" WHERE\r\n");
                    sql.Append("     ID = @ID\r\n");
                    sql.Append("    END\r\n");
                    sql.Append(" ELSE\r\n");
                    sql.Append("    BEGIN\r\n");
                    sql.Append("        INSERT INTO [UserGroup2User]\r\n");
                    sql.Append("                    ([ID]\r\n");
                    sql.Append("                    ,[UserGroupID]\r\n");
                    sql.Append("                    ,[UserID])\r\n");
                    sql.Append("                VALUES\r\n");
                    sql.Append("                    (\r\n");
                    sql.Append("                     @ID,\r\n");
                    sql.Append("                     @UserGroupID,\r\n");
                    sql.Append("                     @UserID\r\n");
                    sql.Append("                    )\r\n");
                    sql.Append("    END\r\n");
                    cmd.CommandText = sql.ToString();
                    cmd.CommandType = CommandType.Text;
                    cmd.Connection = conn;
                    cmd.Transaction = tran;
                    cmd.Parameters.Clear();
                    cmd.Parameters.AddWithValue("@ID", ur.ID);
                    cmd.Parameters.AddWithValue("@UserGroupID", ur.UserGroupID);
                    cmd.Parameters.AddWithValue("@UserID", ur.UserID);
                    cmd.ExecuteNonQuery();

                    tran.Commit();

                    return true;
                }
                catch (Exception ex)
                {
                    tran.Rollback();
                    WriteError(ex);
                    return false;
                }
                finally
                {
                    if (conn.State == ConnectionState.Open)
                    {
                        conn.Close();
                    }

                }
            }
        }
        [WebMethod]
        public bool DeleteUserGroup2User(string userGroup2UserID)
        {
            using (SqlConnection conn = new SqlConnection(this.getTmpAppConnectionString()))
            {
                SqlTransaction tran = null;
                try
                {
                    // 清表
                    SqlCommand cmd = new SqlCommand();
                    conn.Open();
                    tran = conn.BeginTransaction(IsolationLevel.ReadUncommitted);
                    cmd.Connection = conn;
                    cmd.CommandText = "DELETE FROM [UserGroup2User] WHERE  ID = @Original_ID";
                    cmd.Parameters.Clear();
                    cmd.Parameters.AddWithValue("@Original_ID", userGroup2UserID);
                    cmd.Transaction = tran;
                    cmd.ExecuteNonQuery();
                    // 提交
                    tran.Commit();
                    return true;
                }
                catch (Exception ex)
                {
                    tran.Rollback();
                    WriteError(ex);
                    return false;
                }
                finally
                {
                    if (conn.State == ConnectionState.Open)
                        conn.Close();
                }
            }
        }
        #endregion

        #region //UserGroup2UserGroup
        [WebMethod]
        public bool UpdateUserGroup2UserGroup(UserGroup2UserGroup ur)
        {
            using (SqlConnection conn = new SqlConnection(this.getTmpAppConnectionString()))
            {
                SqlTransaction tran = null;
                SqlCommand cmd = new SqlCommand();
                try
                {
                    conn.Open();
                    tran = conn.BeginTransaction(IsolationLevel.ReadUncommitted);
                    StringBuilder sql = new StringBuilder();
                    sql.Append(" IF EXISTS (SELECT ID FROM [UserGroup2UserGroup] WHERE ID = @ID )\r\n");
                    sql.Append("    BEGIN\r\n");
                    sql.Append(" UPDATE [UserGroup2UserGroup]\r\n");
                    sql.Append(" SET\r\n");
                    sql.Append("     SuperiorID = @SuperiorID ,\r\n");
                    sql.Append("     LowerID = @LowerID\r\n");
                    sql.Append(" WHERE\r\n");
                    sql.Append("     ID = @ID\r\n");
                    sql.Append("    END\r\n");
                    sql.Append(" ELSE\r\n");
                    sql.Append("    BEGIN\r\n");
                    sql.Append("        INSERT INTO [UserGroup2UserGroup]\r\n");
                    sql.Append("                    ([ID]\r\n");
                    sql.Append("                    ,[SuperiorID]\r\n");
                    sql.Append("                    ,[LowerID])\r\n");
                    sql.Append("                VALUES\r\n");
                    sql.Append("                    (\r\n");
                    sql.Append("                     @ID,\r\n");
                    sql.Append("                     @SuperiorID,\r\n");
                    sql.Append("                     @LowerID\r\n");
                    sql.Append("                    )\r\n");
                    sql.Append("    END\r\n");
                    cmd.CommandText = sql.ToString();
                    cmd.CommandType = CommandType.Text;
                    cmd.Connection = conn;
                    cmd.Transaction = tran;
                    cmd.Parameters.Clear();
                    cmd.Parameters.AddWithValue("@ID", ur.ID);
                    cmd.Parameters.AddWithValue("@SuperiorID", ur.SuperiorID);
                    cmd.Parameters.AddWithValue("@LowerID", ur.LowerID);
                    cmd.ExecuteNonQuery();

                    tran.Commit();

                    return true;
                }
                catch (Exception ex)
                {
                    tran.Rollback();
                    WriteError(ex);
                    return false;
                }
                finally
                {
                    if (conn.State == ConnectionState.Open)
                    {
                        conn.Close();
                    }

                }
            }
        }
        [WebMethod]
        public bool DeleteUserGroup2UserGroup(string UserGroup2UserGroupID)
        {
            using (SqlConnection conn = new SqlConnection(this.getTmpAppConnectionString()))
            {
                SqlTransaction tran = null;
                try
                {
                    // 清表
                    SqlCommand cmd = new SqlCommand();
                    conn.Open();
                    tran = conn.BeginTransaction(IsolationLevel.ReadUncommitted);
                    cmd.Connection = conn;
                    cmd.CommandText = "DELETE FROM [UserGroup2UserGroup] WHERE ID = @ID";
                    cmd.Parameters.Clear();
                    cmd.Parameters.AddWithValue("@ID", UserGroup2UserGroupID);
                    cmd.Transaction = tran;
                    cmd.ExecuteNonQuery();
                    // 提交
                    tran.Commit();
                    return true;
                }
                catch (Exception ex)
                {
                    tran.Rollback();
                    WriteError(ex);
                    return false;
                }
                finally
                {
                    if (conn.State == ConnectionState.Open)
                        conn.Close();
                }
            }
        }
        #endregion


        #region UserLogin
        [WebMethod]
        public bool UpdateUserLogin(UserLogin ur)
        {
            using (SqlConnection conn = new SqlConnection(this.getTmpAppConnectionString()))
            {

                SqlTransaction tran = null;
                SqlCommand cmd = new SqlCommand();
                try
                {
                    conn.Open();
                    tran = conn.BeginTransaction(IsolationLevel.ReadUncommitted);
                    StringBuilder sql = new StringBuilder();
                    sql.Append(" IF EXISTS (SELECT ID FROM [UserLogin] WHERE [UserID] = @UserID)\r\n");
                    sql.Append("    BEGIN\r\n");
                    sql.Append(" UPDATE [UserLogin]\r\n");
                    sql.Append(" SET\r\n");
                    sql.Append("     UserID = @UserID,\r\n");
                    sql.Append("     LoginTime = @LoginTime,\r\n");
                    sql.Append("     ComputerName = @ComputerName,\r\n");
                    sql.Append("     IPAddress = @IPAddress\r\n");
                    sql.Append(" WHERE\r\n");
                    sql.Append("     UserID = @UserID\r\n");
                    sql.Append("    END\r\n");
                    sql.Append(" ELSE\r\n");
                    sql.Append("    BEGIN\r\n");
                    sql.Append("  INSERT INTO [UserLogin]\r\n");
                    sql.Append("     ([UserID]\r\n");
                    sql.Append("     ,[LoginTime]\r\n");
                    sql.Append("     ,[ComputerName]\r\n");
                    sql.Append("     ,[IPAddress])\r\n");
                    sql.Append("  VALUES\r\n");
                    sql.Append("      (@UserID\r\n");
                    sql.Append("      ,@LoginTime\r\n");
                    sql.Append("      ,@ComputerName\r\n");
                    sql.Append("      ,@IPAddress)\r\n");
                    sql.Append("    END\r\n");
                    cmd.CommandText = sql.ToString();
                    cmd.CommandType = CommandType.Text;
                    cmd.Connection = conn;
                    cmd.Transaction = tran;
                    cmd.Parameters.Clear();
                    cmd.Parameters.AddWithValue("@UserID", ur.UserID);
                    cmd.Parameters.AddWithValue("@LoginTime", ur.LoginTime);
                    cmd.Parameters.AddWithValue("@ComputerName", ur.ComputerName);
                    cmd.Parameters.AddWithValue("@IPAddress", ur.IPAddress);

                    cmd.ExecuteNonQuery();

                    tran.Commit();

                    return true;
                }
                catch (Exception ex)
                {
                    tran.Rollback();
                    WriteError(ex);
                    return false;
                }
                finally
                {
                    if (conn.State == ConnectionState.Open)
                    {
                        conn.Close();
                    }

                }
            }
        }
        [WebMethod]
        public bool DeleteUserLogin(string userID)
        {
            using (SqlConnection conn = new SqlConnection(this.getTmpAppConnectionString()))
            {
                SqlTransaction tran = null;
                try
                {
                    // 删除 UserLogin 表
                    SqlCommand cmd = new SqlCommand();
                    conn.Open();
                    tran = conn.BeginTransaction(IsolationLevel.ReadUncommitted);
                    cmd.Connection = conn;
                    cmd.Transaction = tran;
                    cmd.CommandText = "DELETE FROM [UserLogin] WHERE [UserID] = @UserID";
                    cmd.CommandType = CommandType.Text;
                    cmd.Parameters.Clear();
                    cmd.Parameters.AddWithValue("@UserID", userID);
                    cmd.ExecuteNonQuery();


                    tran.Commit();
                    return true;
                }
                catch (Exception ex)
                {
                    tran.Rollback();
                    WriteError(ex);
                    return false;
                }
                finally
                {
                    if (conn.State == ConnectionState.Open)
                        conn.Close();
                }
            }
        }
        #endregion

        #region UserLoginEx
        [WebMethod]
        public bool UpdateUserLoginEx(UserLoginEx ur)
        {
            using (SqlConnection conn = new SqlConnection(this.getTmpAppConnectionString()))
            {

                SqlTransaction tran = null;
                SqlCommand cmd = new SqlCommand();
                try
                {
                    conn.Open();
                    tran = conn.BeginTransaction(IsolationLevel.ReadUncommitted);
                    StringBuilder sql = new StringBuilder();
                    sql.Append(" IF EXISTS (SELECT UserLoginName FROM [UserLoginEx] WHERE  [UserLoginName] = @UserLoginName AND [UserLoginEx1]=@UserLoginEx1 AND [UserLoginEx2]=@UserLoginEx2 AND [UserLoginEx3]=@UserLoginEx3 AND [UserLoginEx4]=@UserLoginEx4 AND [UserLoginEx5]=@UserLoginEx5) \r\n");
                    sql.Append("    BEGIN\r\n");
                    sql.Append(" UPDATE [UserLoginEx]\r\n");
                    sql.Append(" SET\r\n");
                    sql.Append("     UserLoginName = @UserLoginName,\r\n");
                    sql.Append("     UserLoginEx1 = @UserLoginEx1,\r\n");
                    sql.Append("     UserLoginEx2 = @UserLoginEx2,\r\n");
                    sql.Append("     UserLoginEx3 = @UserLoginEx3,\r\n");
                    sql.Append("     UserLoginEx4 = @UserLoginEx4,\r\n");
                    sql.Append("     UserLoginEx5 = @UserLoginEx5\r\n");
                    sql.Append(" WHERE\r\n");
                    sql.Append("     [UserLoginName] = @UserLoginName AND [UserLoginEx1]=@UserLoginEx1 AND [UserLoginEx2]=@UserLoginEx2 AND [UserLoginEx3]=@UserLoginEx3 AND [UserLoginEx4]=@UserLoginEx4 AND [UserLoginEx5]=@UserLoginEx5 \r\n");
                    sql.Append("    END\r\n");
                    sql.Append(" ELSE\r\n");
                    sql.Append("    BEGIN\r\n");
                    sql.Append("  INSERT INTO [UserLoginEx]\r\n");
                    sql.Append("     ([UserLoginName]\r\n");
                    sql.Append("     ,[UserLoginEx1]\r\n");
                    sql.Append("     ,[UserLoginEx2]\r\n");
                    sql.Append("     ,[UserLoginEx3]\r\n");
                    sql.Append("     ,[UserLoginEx4]\r\n");
                    sql.Append("     ,[UserLoginEx5])\r\n");
                    sql.Append("  VALUES\r\n");
                    sql.Append("      (@UserLoginName\r\n");
                    sql.Append("      ,@UserLoginEx1\r\n");
                    sql.Append("      ,@UserLoginEx2\r\n");
                    sql.Append("      ,@UserLoginEx3\r\n");
                    sql.Append("      ,@UserLoginEx4\r\n");
                    sql.Append("      ,@UserLoginEx5)\r\n");
                    sql.Append("    END\r\n");
                    cmd.CommandText = sql.ToString();
                    cmd.CommandType = CommandType.Text;
                    cmd.Connection = conn;
                    cmd.Transaction = tran;
                    cmd.Parameters.Clear();
                    cmd.Parameters.AddWithValue("@UserLoginName", ur.UserLoginName);
                    cmd.Parameters.AddWithValue("@UserLoginEx1", ur.UserLoginEx1);
                    cmd.Parameters.AddWithValue("@UserLoginEx2", ur.UserLoginEx2);
                    cmd.Parameters.AddWithValue("@UserLoginEx3", ur.UserLoginEx3);
                    cmd.Parameters.AddWithValue("@UserLoginEx4", ur.UserLoginEx4);
                    cmd.Parameters.AddWithValue("@UserLoginEx5", ur.UserLoginEx5);
                    cmd.ExecuteNonQuery();

                    tran.Commit();

                    return true;
                }
                catch (Exception ex)
                {
                    tran.Rollback();
                    WriteError(ex);
                    return false;
                }
                finally
                {
                    if (conn.State == ConnectionState.Open)
                    {
                        conn.Close();
                    }

                }
            }
        }
        [WebMethod]
        public bool UpdateUserLoginExList(UserLoginEx[] pUserLoginExList)
        {
            using (SqlConnection conn = new SqlConnection(this.getTmpAppConnectionString()))
            {

                SqlTransaction tran = null;
                SqlCommand cmd = new SqlCommand();
                try
                {
                    conn.Open();
                    tran = conn.BeginTransaction(IsolationLevel.ReadUncommitted);
                    foreach (UserLoginEx ur in pUserLoginExList)
                    { 

                        StringBuilder sql = new StringBuilder();
                        sql.Append(" IF EXISTS ( SELECT UserLoginName FROM [UserLoginEx] WHERE [UserLoginName] = @UserLoginName AND [UserLoginEx1]=@UserLoginEx1 AND [UserLoginEx2]=@UserLoginEx2 AND [UserLoginEx3]=@UserLoginEx3 AND [UserLoginEx4]=@UserLoginEx4 AND [UserLoginEx5]=@UserLoginEx5)\r\n");
                        sql.Append("    BEGIN\r\n");
                        sql.Append(" UPDATE [UserLoginEx]\r\n");
                        sql.Append(" SET\r\n");
                        sql.Append("     UserLoginName = @UserLoginName,\r\n");
                        sql.Append("     UserLoginEx1 = @UserLoginEx1,\r\n");
                        sql.Append("     UserLoginEx2 = @UserLoginEx2,\r\n");
                        sql.Append("     UserLoginEx3 = @UserLoginEx3,\r\n");
                        sql.Append("     UserLoginEx4 = @UserLoginEx4,\r\n");
                        sql.Append("     UserLoginEx5 = @UserLoginEx5\r\n");
                        sql.Append(" WHERE\r\n");
                        sql.Append("      [UserLoginName] = @UserLoginName AND [UserLoginEx1]=@UserLoginEx1 AND [UserLoginEx2]=@UserLoginEx2 AND [UserLoginEx3]=@UserLoginEx3 AND [UserLoginEx4]=@UserLoginEx4 AND [UserLoginEx5]=@UserLoginEx5\r\n");
                        sql.Append("    END\r\n");
                        sql.Append(" ELSE\r\n");
                        sql.Append("    BEGIN\r\n");
                        sql.Append("  INSERT INTO [UserLoginEx]\r\n");
                        sql.Append("     ([UserLoginName]\r\n");
                        sql.Append("     ,[UserLoginEx1]\r\n");
                        sql.Append("     ,[UserLoginEx2]\r\n");
                        sql.Append("     ,[UserLoginEx3]\r\n");
                        sql.Append("     ,[UserLoginEx4]\r\n");
                        sql.Append("     ,[UserLoginEx5])\r\n");
                        sql.Append("  VALUES\r\n");
                        sql.Append("      (@UserLoginName\r\n");
                        sql.Append("      ,@UserLoginEx1\r\n");
                        sql.Append("      ,@UserLoginEx2\r\n");
                        sql.Append("      ,@UserLoginEx3\r\n");
                        sql.Append("      ,@UserLoginEx4\r\n");
                        sql.Append("      ,@UserLoginEx5)\r\n");
                        sql.Append("    END\r\n");
                        cmd.CommandText = sql.ToString();
                        cmd.CommandType = CommandType.Text;
                        cmd.Connection = conn;
                        cmd.Transaction = tran;
                        cmd.Parameters.Clear();
                        cmd.Parameters.AddWithValue("@UserLoginName", ur.UserLoginName);
                        cmd.Parameters.AddWithValue("@UserLoginEx1", ur.UserLoginEx1);
                        cmd.Parameters.AddWithValue("@UserLoginEx2", ur.UserLoginEx2);
                        cmd.Parameters.AddWithValue("@UserLoginEx3", ur.UserLoginEx3);
                        cmd.Parameters.AddWithValue("@UserLoginEx4", ur.UserLoginEx4);
                        cmd.Parameters.AddWithValue("@UserLoginEx5", ur.UserLoginEx5);
                        cmd.ExecuteNonQuery();
                    }
                    tran.Commit();

                    return true;
                }
                catch (Exception ex)
                {
                    tran.Rollback();
                    WriteError(ex);
                    return false;
                }
                finally
                {
                    if (conn.State == ConnectionState.Open)
                    {
                        conn.Close();
                    }

                }
            }
        }
        [WebMethod]
        public bool DeleteUserLoginEx(string userLoginName)
        {
            using (SqlConnection conn = new SqlConnection(this.getTmpAppConnectionString()))
            {
                SqlTransaction tran = null;
                try
                {
                    // 删除 UserLoginEx 表
                    SqlCommand cmd = new SqlCommand();
                    conn.Open();
                    tran = conn.BeginTransaction(IsolationLevel.ReadUncommitted);
                    cmd.Connection = conn;
                    cmd.Transaction = tran;
                    cmd.CommandText = "DELETE FROM [UserLoginEx] WHERE [UserLoginName] = @UserLoginName";
                    cmd.CommandType = CommandType.Text;
                    cmd.Parameters.Clear();
                    cmd.Parameters.AddWithValue("@UserLoginName", userLoginName);
                    cmd.ExecuteNonQuery();


                    tran.Commit();
                    return true;
                }
                catch (Exception ex)
                {
                    tran.Rollback();
                    WriteError(ex);
                    return false;
                }
                finally
                {
                    if (conn.State == ConnectionState.Open)
                        conn.Close();
                }
            }
        }
        #endregion

    }

}
