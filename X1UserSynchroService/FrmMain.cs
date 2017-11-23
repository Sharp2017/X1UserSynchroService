using RightManagerDll.ClientAgency;
using RightManagerDll.RightManagerWS;
using SynchroCommon;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using X1UserSynchroService.Classes;

namespace X1UserSynchroService
{
    public partial class FrmMain : Form
    {
        bool isNew = false;
        bool isStart;
        public bool IsStart
        {
            get
            {
                return isStart;
            }

            set
            {
                isStart = value;
                if (isStart)
                {
                    this.btnStart.Image = global::X1UserSynchroService.Properties.Resources.stop;

                    this.btnStart.Text = "停止";
                }
                else
                {
                    this.btnStart.Image = global::X1UserSynchroService.Properties.Resources.Start;

                    this.btnStart.Text = "启动";
                }
            }
        }

        Thread ExcThread;

        public FrmMain()
        {
            InitializeComponent();

            ExcThread = new Thread(new ThreadStart(this.Task));

            ExcThread.IsBackground = true;
            ExcThread.Start();
            IsStart = true;
        }


        /// <summary>
        /// 日志输出
        /// </summary>
        /// <param name="pContent"></param>
        private void AppendMessageLine(string pContent)
        {

            this.BeginInvoke(new Action(() =>
            {
                if (txtInfo.Text.Length > txtInfo.MaxLength)
                    txtInfo.Text = "";
                this.txtInfo.AppendText(DateTime.Now + ":  " + pContent + "\n");
            }));
        }
        #region //任务执行

        private void TaskFunc()
        {
            try
            {
                this.Invoke(new Tasks(this.Task));
            }
            catch (Exception ex)
            {
                AppendMessageLine("程序错误，方法：TaskFunc 错误信息：" + ex.Message);
                LogService.WriteErr("程序错误，方法：TaskFunc 错误信息：" + ex.Message);
            }
        }

        private delegate void Tasks();

        private void UserSynchroTask()
        {
            try
            {
                Application.DoEvents();

                if (!isStart)
                {
                    if (isNew)
                    {
                        AppendMessageLine("任务已经停止！");

                        isNew = false;
                    }

                    return;
                }
                isNew = true;
                AppendMessageLine("开始获取任务！");
                List<TableSynchroTask> TableSynchroTaskLisy = SynchroDBManager.GetTableSynchroTaskList();
                if (TableSynchroTaskLisy == null || TableSynchroTaskLisy.Count <= 0)
                {
                    AppendMessageLine("未获取到新任务！");

                }
                else
                {

                    #region //同步Xstudio系统
                    foreach (TableSynchroTask item in TableSynchroTaskLisy)
                    {
                        switch (item.TableName)
                        {
                            case "Users":
                                if (item.OperationType == 0)
                                {
                                    #region //Update
                                    X1UserSynchroService.UserSynchroWS.User _User = SynchroDBManager.GetUserByID(item.TableRowID.ToString());
                                    if (_User != null)
                                    {
                                        #region //同步Xstudio系统
                                        bool XstudioOK = false;
                                        if (Globals.UserSynchroWebService.UpdateUser(_User))
                                        {
                                            AppendMessageLine("同步更新成功，表名：Users ID：" + item.TableRowID.ToString());
                                            LogService.Write("同步更新成功，表名：Users ID：" + item.TableRowID.ToString());
                                            XstudioOK = true;
                                        }
                                        else
                                        {

                                            AppendMessageLine("同步更新失败，表名：Users ID：" + item.TableRowID.ToString());
                                            LogService.Write("同步更新失败，表名：Users ID：" + item.TableRowID.ToString());
                                            XstudioOK = false;
                                        }
                                        #endregion

                                        #region //同步频率库
                                        try
                                        {
                                            RightManagerClient.SetUrl(ConfigurationManager.AppSettings["RightManagerURL"]);
                                            RightManagerDS.SystemsDataTable sysDT = RightManagerClient.System.GetSystems("XStudio");
                                            int countOK = 0;
                                            foreach (RightManagerDS.SystemsRow sr in sysDT)
                                            {
                                                try
                                                {
                                                    if (sr.Keys.Equals("XStudio", StringComparison.CurrentCultureIgnoreCase))
                                                    {
                                                        countOK++;
                                                        continue;
                                                    }

                                                    if (RightManagerClient.User.UpdatePassword(_User.LoginName, _User.Password, _User.ModifyUserID, _User.ModifyUserName, sr.Keys))
                                                    {
                                                        AppendMessageLine("同步：" + sr.Name + "(" + sr.Keys + ")频率" + _User.LoginName + " 用户信息成功！");
                                                        LogService.Write("同步：" + sr.Name + "(" + sr.Keys + ")频率" + _User.LoginName + " 用户信息成功！");
                                                        countOK++;
                                                    }

                                                }
                                                catch (Exception ex)
                                                {
                                                    AppendMessageLine("同步：" + sr.Name + "(" + sr.Keys + ")" + " 频率，异常，错误信息：" + ex.Message);
                                                    LogService.Write("同步：" + sr.Name + "(" + sr.Keys + ")" + " 频率，异常，错误信息：" + ex.Message);
                                                    continue;
                                                }

                                            }
                                            if (sysDT.Count == countOK && XstudioOK)
                                            {
                                                if (SynchroDBManager.DelTableSynchroTaskByID(item.TableSynchroTaskID.ToString()))
                                                {

                                                    LogService.Write("删除任务成功！");
                                                }
                                                else
                                                {

                                                    LogService.Write("删除任务失败！ 任务ID：" + item.TableSynchroTaskID.ToString() + "");
                                                }
                                            }
                                            else
                                            {

                                                LogService.Write("同步更新失败，表名：Users ID：" + item.TableRowID.ToString());
                                                if (SynchroDBManager.UpdateTableSynchroTaskStateByID(item.TableSynchroTaskID.ToString(), -1))
                                                {
                                                    LogService.Write("更新任务状态成功！");
                                                }
                                                else
                                                {
                                                    LogService.Write("更新任务状态失败！ 任务ID：" + item.TableSynchroTaskID.ToString() + "");
                                                }

                                            }
                                        }
                                        catch (Exception ex)
                                        {
                                            AppendMessageLine("同步频率库异常，错误信息：" + ex.Message);
                                            LogService.Write("同步频率库异常，错误信息：" + ex.Message);

                                        }
                                        #endregion
                                    }
                                    else
                                    {
                                        //AppendMessageLine("Users 未找到 ID为：" + item.TableRowID.ToString() + "的数据");
                                        LogService.Write("Users 未找到 ID为：" + item.TableRowID.ToString() + "的数据");

                                        if (SynchroDBManager.DelTableSynchroTaskByID(item.TableSynchroTaskID.ToString()))
                                        {
                                            //AppendMessageLine("删除任务成功！");
                                            LogService.Write("删除任务成功！");
                                        }
                                        else
                                        {
                                            //AppendMessageLine("删除任务失败！ 任务ID：" + item.TableSynchroTaskID.ToString() + "");
                                            LogService.Write("删除任务失败！ 任务ID：" + item.TableSynchroTaskID.ToString() + "");
                                        }
                                    }

                                    #endregion
                                }
                                else if (item.OperationType == 1)
                                {
                                    #region //Del

                                    if (Globals.UserSynchroWebService.DeleteUserByID(item.TableRowID))
                                    {
                                        AppendMessageLine("同步删除成功，表名：Users ID：" + item.TableRowID.ToString());
                                        LogService.Write("同步删除成功，表名：Users ID：" + item.TableRowID.ToString());
                                        if (SynchroDBManager.DelTableSynchroTaskByID(item.TableSynchroTaskID.ToString()))
                                        {
                                            /// AppendMessageLine("删除任务成功！");
                                            LogService.Write("删除任务成功！");
                                        }
                                        else
                                        {
                                            // AppendMessageLine("删除任务失败！ 任务ID：" + item.TableSynchroTaskID.ToString() + "");
                                            LogService.Write("删除任务失败！ 任务ID：" + item.TableSynchroTaskID.ToString() + "");
                                        }
                                    }
                                    else
                                    {

                                        AppendMessageLine("同步删除失败，表名：Users ID：" + item.TableRowID.ToString());
                                        LogService.Write("同步删除失败，表名：Users ID：" + item.TableRowID.ToString());
                                        if (SynchroDBManager.UpdateTableSynchroTaskStateByID(item.TableSynchroTaskID.ToString(), -1))
                                        {
                                            //AppendMessageLine("更新任务状态成功！");
                                            LogService.Write("更新任务状态成功！");
                                        }
                                        else
                                        {
                                            //AppendMessageLine("更新任务状态失败！ 任务ID：" + item.TableSynchroTaskID.ToString() + "");
                                            LogService.Write("更新任务状态失败！ 任务ID：" + item.TableSynchroTaskID.ToString() + "");
                                        }
                                    }
                                    #endregion
                                }
                                break;
                            case "UserFingerInfo":
                                if (item.OperationType == 0)
                                {
                                    #region //Update

                                    X1UserSynchroService.UserSynchroWS.UserFingerInfo _UserFingerInfo = SynchroDBManager.GetUserFingerInfoByID(item.TableRowID.ToString());
                                    X1UserSynchroService.UserSynchroWS.User _User = SynchroDBManager.GetUserByID(item.TableRowID.ToString());
                                    if (_UserFingerInfo != null && _User != null)
                                    {
                                        bool XstudioOK = false;
                                        #region//Xstudio系统
                                        if (Globals.UserSynchroWebService.UpdateUserFingerInfo(_UserFingerInfo))
                                        {
                                            AppendMessageLine("同步更新成功，表名：UserFingerInfo ID：" + item.TableRowID.ToString());
                                            LogService.Write("同步更新成功，表名：UserFingerInfo ID：" + item.TableRowID.ToString());
                                            XstudioOK = true;
                                        }
                                        else
                                        {

                                            AppendMessageLine("同步更新失败，表名：UserFingerInfo ID：" + item.TableRowID.ToString());
                                            LogService.Write("同步更新失败，表名：UserFingerInfo ID：" + item.TableRowID.ToString());
                                            XstudioOK = false;
                                        }
                                        #endregion


                                        #region //同步频率库
                                        try
                                        {
                                            RightManagerClient.SetUrl(ConfigurationManager.AppSettings["RightManagerURL"]);
                                            RightManagerDS.SystemsDataTable sysDT = RightManagerClient.System.GetSystems("XStudio");
                                            int countOK = 0;
                                            foreach (RightManagerDS.SystemsRow sr in sysDT)
                                            {
                                                try
                                                {
                                                    if (sr.Keys.Equals("XStudio", StringComparison.CurrentCultureIgnoreCase))
                                                    {
                                                        countOK++;
                                                        continue;
                                                    }

                                                    if (RightManagerClient.UserFingerInfo.UpdateUserFingerInfoByLoginName(_User.LoginName, _UserFingerInfo.FingerInfoStr1, _UserFingerInfo.FingerInfoStr2, _UserFingerInfo.FingerInfoStr3, sr.Keys))
                                                    {
                                                        countOK++;
                                                        AppendMessageLine("同步：" + sr.Name + "(" + sr.Keys + ")频率" + _UserFingerInfo.UserID + " 用户指纹信息成功！");
                                                        LogService.Write("同步：" + sr.Name + "(" + sr.Keys + ")频率" + _UserFingerInfo.UserID + " 用户指纹信息成功！");
                                                    }


                                                }
                                                catch (Exception ex)
                                                {
                                                    AppendMessageLine("同步：" + sr.Name + "(" + sr.Keys + ")" + " 频率，异常，错误信息：" + ex.Message);
                                                    LogService.Write("同步：" + sr.Name + "(" + sr.Keys + ")" + " 频率，异常，错误信息：" + ex.Message);
                                                    continue;
                                                }

                                            }

                                            if (sysDT.Count == countOK && XstudioOK)
                                            {
                                                if (SynchroDBManager.DelTableSynchroTaskByID(item.TableSynchroTaskID.ToString()))
                                                {

                                                    LogService.Write("删除任务成功！");
                                                }
                                                else
                                                {

                                                    LogService.Write("删除任务失败！ 任务ID：" + item.TableSynchroTaskID.ToString() + "");
                                                }
                                            }
                                            else
                                            {

                                                LogService.Write("同步更新失败，表名：UserFingerInfo ID：" + item.TableRowID.ToString());
                                                if (SynchroDBManager.UpdateTableSynchroTaskStateByID(item.TableSynchroTaskID.ToString(), -1))
                                                {
                                                    LogService.Write("更新任务状态成功！");
                                                }
                                                else
                                                {
                                                    LogService.Write("更新任务状态失败！ 任务ID：" + item.TableSynchroTaskID.ToString() + "");
                                                }

                                            }
                                        }
                                        catch (Exception ex)
                                        {
                                            AppendMessageLine("同步频率库异常，错误信息：" + ex.Message);
                                            LogService.Write("同步频率库异常，错误信息：" + ex.Message);

                                        }
                                        #endregion

                                    }
                                    else
                                    {
                                        // AppendMessageLine("UserFingerInfo表 未找到 ID为：" + item.TableRowID.ToString() + "的数据");
                                        LogService.Write("UserFingerInfo表 未找到 ID为：" + item.TableRowID.ToString() + "的数据");

                                        if (SynchroDBManager.DelTableSynchroTaskByID(item.TableSynchroTaskID.ToString()))
                                        {
                                            //AppendMessageLine("删除任务成功！");
                                            LogService.Write("删除任务成功！");
                                        }
                                        else
                                        {
                                            //AppendMessageLine("删除任务失败！ 任务ID：" + item.TableSynchroTaskID.ToString() + "");
                                            LogService.Write("删除任务失败！ 任务ID：" + item.TableSynchroTaskID.ToString() + "");
                                        }
                                    }

                                    #endregion
                                }
                                else if (item.OperationType == 1)
                                {
                                    #region //Del

                                    #region //Xstudio系统
                                    bool XstudioOK = false;
                                    if (Globals.UserSynchroWebService.DeleteUserFingerInfoByID(item.TableRowID))
                                    {
                                        AppendMessageLine("同步删除成功，表名：UserFingerInfo ID：" + item.TableRowID.ToString());
                                        LogService.Write("同步删除成功，表名：UserFingerInfo ID：" + item.TableRowID.ToString());
                                        XstudioOK = true;
                                    }
                                    else
                                    {

                                        AppendMessageLine("同步删除UserFingerInfo表失败！");
                                        XstudioOK = false;
                                    }
                                    #endregion

                                    #region //同步频率库
                                    try
                                    {
                                        RightManagerClient.SetUrl(ConfigurationManager.AppSettings["RightManagerURL"]);
                                        RightManagerDS.SystemsDataTable sysDT = RightManagerClient.System.GetSystems("XStudio");
                                        int countOK = 0;
                                        foreach (RightManagerDS.SystemsRow sr in sysDT)
                                        {
                                            try
                                            {
                                                if (sr.Keys.Equals("XStudio", StringComparison.CurrentCultureIgnoreCase))
                                                {
                                                    countOK++; continue;
                                                }

                                                if (RightManagerClient.UserFingerInfo.DeleteUserFingerInfoByLoginName(SynchroDBManager.GetUserByID(item.TableRowID.ToString()).LoginName, sr.Keys))
                                                {
                                                    countOK++;
                                                    AppendMessageLine("同步删除：" + sr.Name + "(" + sr.Keys + ")频率" + item.TableRowID + " 用户指纹信息成功！");
                                                    LogService.Write("同步删除：" + sr.Name + "(" + sr.Keys + ")频率" + item.TableRowID + " 用户指纹信息成功！");
                                                }

                                            }
                                            catch (Exception ex)
                                            {
                                                AppendMessageLine("同步删除：" + sr.Name + "(" + sr.Keys + ")" + item.TableRowID + " 频率，异常，错误信息：" + ex.Message);
                                                LogService.Write("同步删除：" + sr.Name + "(" + sr.Keys + ")" + item.TableRowID + " 频率，异常，错误信息：" + ex.Message);
                                                continue;
                                            }

                                        }

                                        if (sysDT.Count == countOK && XstudioOK)
                                        {
                                            if (SynchroDBManager.DelTableSynchroTaskByID(item.TableSynchroTaskID.ToString()))
                                            {

                                                LogService.Write("删除任务成功！");
                                            }
                                            else
                                            {

                                                LogService.Write("删除任务失败！ 任务ID：" + item.TableSynchroTaskID.ToString() + "");
                                            }
                                        }
                                        else
                                        {

                                            LogService.Write("同步更新失败，表名：Users ID：" + item.TableRowID.ToString());
                                            if (SynchroDBManager.UpdateTableSynchroTaskStateByID(item.TableSynchroTaskID.ToString(), -1))
                                            {
                                                LogService.Write("更新任务状态成功！");
                                            }
                                            else
                                            {
                                                LogService.Write("更新任务状态失败！ 任务ID：" + item.TableSynchroTaskID.ToString() + "");
                                            }

                                        }
                                    }
                                    catch (Exception ex)
                                    {
                                        AppendMessageLine("同步频率库异常，错误信息：" + ex.Message);
                                        LogService.Write("同步频率库异常，错误信息：" + ex.Message);

                                    }
                                    #endregion

                                    #endregion
                                }
                                break;
                            case "UserGroups":
                                if (item.OperationType == 0)
                                {
                                    #region //Update

                                    X1UserSynchroService.UserSynchroWS.UserGroup _UserGroup = SynchroDBManager.GetUserGroupByID(item.TableRowID.ToString());
                                    if (_UserGroup != null)
                                    {

                                        if (Globals.UserSynchroWebService.UpdateUserGroups(_UserGroup))
                                        {
                                            AppendMessageLine("同步更新成功，表名：UserGroups ID：" + item.TableRowID.ToString());
                                            LogService.Write("同步更新成功，表名：UserGroups ID：" + item.TableRowID.ToString());
                                            if (SynchroDBManager.DelTableSynchroTaskByID(item.TableSynchroTaskID.ToString()))
                                            {
                                                //AppendMessageLine("删除任务成功！");
                                                LogService.Write("删除任务成功！");
                                            }
                                            else
                                            {
                                                //AppendMessageLine("删除任务失败！ 任务ID：" + item.TableSynchroTaskID.ToString() + "");
                                                LogService.Write("删除任务失败！ 任务ID：" + item.TableSynchroTaskID.ToString() + "");
                                            }
                                        }
                                        else
                                        {

                                            AppendMessageLine("同步更新失败，表名：UserGroups ID：" + item.TableRowID.ToString());
                                            LogService.Write("同步更新失败，表名：UserGroups ID：" + item.TableRowID.ToString());
                                            if (SynchroDBManager.UpdateTableSynchroTaskStateByID(item.TableSynchroTaskID.ToString(), -1))
                                            {
                                                //AppendMessageLine("更新任务状态成功！");
                                                LogService.Write("更新任务状态成功！");
                                            }
                                            else
                                            {
                                                //AppendMessageLine("更新任务状态失败！ 任务ID：" + item.TableSynchroTaskID.ToString() + "");
                                                LogService.Write("更新任务状态失败！ 任务ID：" + item.TableSynchroTaskID.ToString() + "");
                                            }
                                        }
                                    }
                                    else
                                    {
                                        // AppendMessageLine("UserGroups 未找到 ID为：" + item.TableRowID.ToString() + "的数据");
                                        LogService.Write("UserGroups 未找到 ID为：" + item.TableRowID.ToString() + "的数据");

                                        if (SynchroDBManager.DelTableSynchroTaskByID(item.TableSynchroTaskID.ToString()))
                                        {
                                            //AppendMessageLine("删除任务成功！");
                                            LogService.Write("删除任务成功！");
                                        }
                                        else
                                        {
                                            // AppendMessageLine("删除任务失败！ 任务ID：" + item.TableSynchroTaskID.ToString() + "");
                                            LogService.Write("删除任务失败！ 任务ID：" + item.TableSynchroTaskID.ToString() + "");
                                        }
                                    }
                                    #endregion
                                }
                                else if (item.OperationType == 1)
                                {
                                    #region //Del

                                    if (Globals.UserSynchroWebService.DeleteUserGroupByID(item.TableRowID))
                                    {
                                        AppendMessageLine("同步删除成功，表名：UserGroups ID：" + item.TableRowID.ToString());
                                        LogService.Write("同步删除成功，表名：UserGroups ID：" + item.TableRowID.ToString());
                                        if (SynchroDBManager.DelTableSynchroTaskByID(item.TableSynchroTaskID.ToString()))
                                        {
                                            //AppendMessageLine("删除任务成功！");
                                            LogService.Write("删除任务成功！");
                                        }
                                        else
                                        {
                                            //AppendMessageLine("删除任务失败！ 任务ID：" + item.TableSynchroTaskID.ToString() + "");
                                            LogService.Write("删除任务失败！ 任务ID：" + item.TableSynchroTaskID.ToString() + "");
                                        }
                                    }
                                    else
                                    {

                                        AppendMessageLine("同步删除失败，表名：UserGroups ID：" + item.TableRowID.ToString());
                                        LogService.Write("同步删除失败，表名：UserGroups ID：" + item.TableRowID.ToString());
                                        if (SynchroDBManager.UpdateTableSynchroTaskStateByID(item.TableSynchroTaskID.ToString(), -1))
                                        {
                                            // AppendMessageLine("更新任务状态成功！");
                                            LogService.Write("更新任务状态成功！");
                                        }
                                        else
                                        {
                                            // AppendMessageLine("更新任务状态失败！ 任务ID：" + item.TableSynchroTaskID.ToString() + "");
                                            LogService.Write("更新任务状态失败！ 任务ID：" + item.TableSynchroTaskID.ToString() + "");
                                        }
                                    }
                                    #endregion
                                }

                                break;
                            case "UserGroup2User":
                                if (item.OperationType == 0)
                                {
                                    #region //Update
                                    X1UserSynchroService.UserSynchroWS.UserGroup2User _UserGroup2User = SynchroDBManager.GetUserGroup2UserByID(item.TableRowID.ToString());
                                    if (_UserGroup2User != null)
                                    { 

                                        if (Globals.UserSynchroWebService.UpdateUserGroup2User(_UserGroup2User))
                                        {
                                            AppendMessageLine("同步更新成功，表名：UserGroup2User ID：" + item.TableRowID.ToString());
                                            LogService.Write("同步更新成功，表名：UserGroup2User ID：" + item.TableRowID.ToString());
                                            if (SynchroDBManager.DelTableSynchroTaskByID(item.TableSynchroTaskID.ToString()))
                                            {
                                                //AppendMessageLine("删除任务成功！");
                                                LogService.Write("删除任务成功！");
                                            }
                                            else
                                            {
                                                //AppendMessageLine("删除任务失败！ 任务ID：" + item.TableSynchroTaskID.ToString() + "");
                                                LogService.Write("删除任务失败！ 任务ID：" + item.TableSynchroTaskID.ToString() + "");
                                            }
                                        }
                                        else
                                        {

                                            AppendMessageLine("同步更新失败，表名：UserGroup2User ID：" + item.TableRowID.ToString());
                                            LogService.Write("同步更新失败，表名：UserGroup2User ID：" + item.TableRowID.ToString());
                                            if (SynchroDBManager.UpdateTableSynchroTaskStateByID(item.TableSynchroTaskID.ToString(), -1))
                                            {
                                                //AppendMessageLine("更新任务状态成功！");
                                                LogService.Write("更新任务状态成功！");
                                            }
                                            else
                                            {
                                                //AppendMessageLine("更新任务状态失败！ 任务ID：" + item.TableSynchroTaskID.ToString() + "");
                                                LogService.Write("更新任务状态失败！ 任务ID：" + item.TableSynchroTaskID.ToString() + "");
                                            }
                                        }
                                    }
                                    else
                                    {
                                        //AppendMessageLine("UserGroup2User 未找到 ID为：" + item.TableRowID.ToString() + "的数据");
                                        LogService.Write("UserGroup2User 未找到 ID为：" + item.TableRowID.ToString() + "的数据");

                                        if (SynchroDBManager.DelTableSynchroTaskByID(item.TableSynchroTaskID.ToString()))
                                        {
                                            //AppendMessageLine("删除任务成功！");
                                            LogService.Write("删除任务成功！");
                                        }
                                        else
                                        {
                                            //AppendMessageLine("删除任务失败！ 任务ID：" + item.TableSynchroTaskID.ToString() + "");
                                            LogService.Write("删除任务失败！ 任务ID：" + item.TableSynchroTaskID.ToString() + "");
                                        }
                                    }
                                    #endregion
                                }
                                else if (item.OperationType == 1)
                                {
                                    #region //Del

                                    if (Globals.UserSynchroWebService.DeleteUserGroup2UserByID(item.TableRowID))
                                    {
                                        AppendMessageLine("同步删除成功，表名：UserGroup2User ID：" + item.TableRowID.ToString());
                                        LogService.Write("同步删除成功，表名：UserGroup2User ID：" + item.TableRowID.ToString());
                                        if (SynchroDBManager.DelTableSynchroTaskByID(item.TableSynchroTaskID.ToString()))
                                        {
                                            //AppendMessageLine("删除任务成功！");
                                            LogService.Write("删除任务成功！");
                                        }
                                        else
                                        {
                                            //AppendMessageLine("删除任务失败！ 任务ID：" + item.TableSynchroTaskID.ToString() + "");
                                            LogService.Write("删除任务失败！ 任务ID：" + item.TableSynchroTaskID.ToString() + "");
                                        }
                                    }
                                    else
                                    {

                                        AppendMessageLine("同步删除失败，表名：UserGroup2User ID：" + item.TableRowID.ToString());
                                        LogService.Write("同步删除失败，表名：UserGroup2User ID：" + item.TableRowID.ToString());
                                        if (SynchroDBManager.UpdateTableSynchroTaskStateByID(item.TableSynchroTaskID.ToString(), -1))
                                        {
                                            //AppendMessageLine("更新任务状态成功！");
                                            LogService.Write("更新任务状态成功！");
                                        }
                                        else
                                        {
                                            //AppendMessageLine("更新任务状态失败！ 任务ID：" + item.TableSynchroTaskID.ToString() + "");
                                            LogService.Write("更新任务状态失败！ 任务ID：" + item.TableSynchroTaskID.ToString() + "");
                                        }
                                    }
                                    #endregion
                                }

                                break;
                            case "UserGroup2UserGroup":
                                if (item.OperationType == 0)
                                {

                                    #region //Update
                                    X1UserSynchroService.UserSynchroWS.UserGroup2UserGroup _UserGroup2UserGroup = SynchroDBManager.GetUserGroup2UserGroupByID(item.TableRowID.ToString());
                                    if (_UserGroup2UserGroup != null)
                                    {
                                        if (Globals.UserSynchroWebService.UpdateUserGroup2UserGroup(_UserGroup2UserGroup))
                                        {
                                            AppendMessageLine("同步更新成功，表名：UserGroup2UserGroup ID：" + item.TableRowID.ToString());
                                            LogService.Write("同步更新成功，表名：UserGroup2UserGroup ID：" + item.TableRowID.ToString());
                                            if (SynchroDBManager.DelTableSynchroTaskByID(item.TableSynchroTaskID.ToString()))
                                            {
                                                //AppendMessageLine("删除任务成功！");
                                                LogService.Write("删除任务成功！");
                                            }
                                            else
                                            {
                                                //AppendMessageLine("删除任务失败！ 任务ID：" + item.TableSynchroTaskID.ToString() + "");
                                                LogService.Write("删除任务失败！ 任务ID：" + item.TableSynchroTaskID.ToString() + "");
                                            }
                                        }
                                        else
                                        {

                                            AppendMessageLine("同步更新失败，表名：UserGroup2UserGroup ID：" + item.TableRowID.ToString());
                                            LogService.Write("同步更新失败，表名：UserGroup2UserGroup ID：" + item.TableRowID.ToString());
                                            if (SynchroDBManager.UpdateTableSynchroTaskStateByID(item.TableSynchroTaskID.ToString(), -1))
                                            {
                                                //AppendMessageLine("更新任务状态成功！");
                                                LogService.Write("更新任务状态成功！");
                                            }
                                            else
                                            {
                                                //AppendMessageLine("更新任务状态失败！ 任务ID：" + item.TableSynchroTaskID.ToString() + "");
                                                LogService.Write("更新任务状态失败！ 任务ID：" + item.TableSynchroTaskID.ToString() + "");
                                            }
                                        }
                                    }
                                    else
                                    {
                                        //AppendMessageLine("UserGroup2UserGroup 未找到 ID为：" + item.TableRowID.ToString() + "的数据");
                                        LogService.Write("UserGroup2UserGroup 未找到 ID为：" + item.TableRowID.ToString() + "的数据");

                                        if (SynchroDBManager.DelTableSynchroTaskByID(item.TableSynchroTaskID.ToString()))
                                        {
                                            //AppendMessageLine("删除任务成功！");
                                            LogService.Write("删除任务成功！");
                                        }
                                        else
                                        {
                                            //AppendMessageLine("删除任务失败！ 任务ID：" + item.TableSynchroTaskID.ToString() + "");
                                            LogService.Write("删除任务失败！ 任务ID：" + item.TableSynchroTaskID.ToString() + "");
                                        }
                                    }
                                    #endregion
                                }
                                else if (item.OperationType == 1)
                                {
                                    #region //Del

                                    if (Globals.UserSynchroWebService.DeleteUserGroup2UserGroupByID(item.TableRowID))
                                    {
                                        AppendMessageLine("同步删除成功，表名：UserGroup2UserGroup ID：" + item.TableRowID.ToString());
                                        LogService.Write("同步删除成功，表名：UserGroup2UserGroup ID：" + item.TableRowID.ToString());
                                        if (SynchroDBManager.DelTableSynchroTaskByID(item.TableSynchroTaskID.ToString()))
                                        {
                                            //AppendMessageLine("删除任务成功！");
                                            LogService.Write("删除任务成功！");
                                        }
                                        else
                                        {
                                            //AppendMessageLine("删除任务失败！ 任务ID：" + item.TableSynchroTaskID.ToString() + "");
                                            LogService.Write("删除任务失败！ 任务ID：" + item.TableSynchroTaskID.ToString() + "");
                                        }
                                    }
                                    else
                                    {

                                        AppendMessageLine("同步删除失败，表名：UserGroup2UserGroup ID：" + item.TableRowID.ToString());
                                        LogService.Write("同步删除失败，表名：UserGroup2UserGroup ID：" + item.TableRowID.ToString());
                                        if (SynchroDBManager.UpdateTableSynchroTaskStateByID(item.TableSynchroTaskID.ToString(), -1))
                                        {
                                            //AppendMessageLine("更新任务状态成功！");
                                            LogService.Write("更新任务状态成功！");
                                        }
                                        else
                                        {
                                            //AppendMessageLine("更新任务状态失败！ 任务ID：" + item.TableSynchroTaskID.ToString() + "");
                                            LogService.Write("更新任务状态失败！ 任务ID：" + item.TableSynchroTaskID.ToString() + "");
                                        }
                                    }
                                    #endregion
                                }

                                break;
                            case "UserLogin":
                                if (item.OperationType == 0)
                                {

                                    #region //Update
                                    X1UserSynchroService.UserSynchroWS.UserLogin _UserLogin = SynchroDBManager.GetUserLoginByUserID(item.TableRowID.ToString());
                                    if (_UserLogin != null)
                                    {
                                        if (Globals.UserSynchroWebService.UpdateUserLogin(_UserLogin))
                                        {
                                            AppendMessageLine("同步更新成功，表名：UserLogin ID：" + item.TableRowID.ToString());
                                            LogService.Write("同步更新成功，表名：UserLogin ID：" + item.TableRowID.ToString());
                                            if (SynchroDBManager.DelTableSynchroTaskByID(item.TableSynchroTaskID.ToString()))
                                            {
                                                //AppendMessageLine("删除任务成功！");
                                                LogService.Write("删除任务成功！");
                                            }
                                            else
                                            {
                                                //AppendMessageLine("删除任务失败！ 任务ID：" + item.TableSynchroTaskID.ToString() + "");
                                                LogService.Write("删除任务失败！ 任务ID：" + item.TableSynchroTaskID.ToString() + "");
                                            }
                                        }
                                        else
                                        {

                                            AppendMessageLine("同步更新失败，表名：UserLogin ID：" + item.TableRowID.ToString());
                                            LogService.Write("同步更新失败，表名：UserLogin ID：" + item.TableRowID.ToString());
                                            if (SynchroDBManager.UpdateTableSynchroTaskStateByID(item.TableSynchroTaskID.ToString(), -1))
                                            {
                                                //AppendMessageLine("更新任务状态成功！");
                                                LogService.Write("更新任务状态成功！");
                                            }
                                            else
                                            {
                                                //AppendMessageLine("更新任务状态失败！ 任务ID：" + item.TableSynchroTaskID.ToString() + "");
                                                LogService.Write("更新任务状态失败！ 任务ID：" + item.TableSynchroTaskID.ToString() + "");
                                            }
                                        }
                                    }
                                    else
                                    {
                                        //AppendMessageLine("UserLogin 未找到 UserID为：" + item.TableRowID.ToString() + "的数据");
                                        LogService.Write("UserLogin 未找到 UserID为：" + item.TableRowID.ToString() + "的数据");

                                        if (SynchroDBManager.DelTableSynchroTaskByID(item.TableSynchroTaskID.ToString()))
                                        {
                                            //AppendMessageLine("删除任务成功！");
                                            LogService.Write("删除任务成功！");
                                        }
                                        else
                                        {
                                            //AppendMessageLine("删除任务失败！ 任务ID：" + item.TableSynchroTaskID.ToString() + "");
                                            LogService.Write("删除任务失败！ 任务ID：" + item.TableSynchroTaskID.ToString() + "");
                                        }
                                    }
                                    #endregion
                                }
                                else if (item.OperationType == 1)
                                {
                                    #region //Del

                                    if (Globals.UserSynchroWebService.DeleteUserLoginByID(item.TableRowID))
                                    {
                                        AppendMessageLine("同步删除成功，表名：UserLogin ID：" + item.TableRowID.ToString());
                                        LogService.Write("同步删除成功，表名：UserLogin ID：" + item.TableRowID.ToString());
                                        if (SynchroDBManager.DelTableSynchroTaskByID(item.TableSynchroTaskID.ToString()))
                                        {
                                            //AppendMessageLine("删除任务成功！");
                                            LogService.Write("删除任务成功！");
                                        }
                                        else
                                        {
                                            //AppendMessageLine("删除任务失败！ 任务ID：" + item.TableSynchroTaskID.ToString() + "");
                                            LogService.Write("删除任务失败！ 任务ID：" + item.TableSynchroTaskID.ToString() + "");
                                        }
                                    }
                                    else
                                    {

                                        AppendMessageLine("同步删除失败，表名：UserLogin UserID：" + item.TableRowID.ToString());
                                        LogService.Write("同步删除失败，表名：UserLogin UserID：" + item.TableRowID.ToString());
                                        if (SynchroDBManager.UpdateTableSynchroTaskStateByID(item.TableSynchroTaskID.ToString(), -1))
                                        {
                                            //AppendMessageLine("更新任务状态成功！");
                                            LogService.Write("更新任务状态成功！");
                                        }
                                        else
                                        {
                                            //AppendMessageLine("更新任务状态失败！ 任务ID：" + item.TableSynchroTaskID.ToString() + "");
                                            LogService.Write("更新任务状态失败！ 任务ID：" + item.TableSynchroTaskID.ToString() + "");
                                        }
                                    }
                                    #endregion
                                }
                                break;
                            case "UserLoginEx":


                                if (item.OperationType == 0)
                                {

                                    #region //Update
                                    X1UserSynchroService.UserSynchroWS.UserLoginEx[] _UserLoginExList = SynchroDBManager.GetUserLoginListExByUserName(item.TableRowID.ToString());
                                    if (_UserLoginExList != null)
                                    {
                                        if (Globals.UserSynchroWebService.UpdateUserLoginExList(_UserLoginExList))
                                        {
                                            AppendMessageLine("同步更新成功，表名：UserLoginEx ID：" + item.TableRowID.ToString());
                                            LogService.Write("同步更新成功，表名：UserLoginEx ID：" + item.TableRowID.ToString());
                                            if (SynchroDBManager.DelTableSynchroTaskByTableInfo("UserLoginEx", item.TableRowID))
                                            {
                                                //AppendMessageLine("删除任务成功！");
                                                LogService.Write("删除任务成功！");
                                            }
                                            else
                                            {
                                                //AppendMessageLine("删除任务失败！ 任务ID：" + item.TableSynchroTaskID.ToString() + "");
                                                LogService.Write("删除任务失败！ 任务ID：" + item.TableSynchroTaskID.ToString() + "");
                                            }
                                        }
                                        else
                                        {

                                            AppendMessageLine("同步更新失败，表名：UserLoginEx UserLoginName：" + item.TableRowID.ToString());
                                            LogService.Write("同步更新失败，表名：UserLoginEx UserLoginName：" + item.TableRowID.ToString());
                                            if (SynchroDBManager.UpdateTableSynchroTaskStateByByTableInfo("UserLoginEx", item.TableRowID, -1))
                                            {
                                                //AppendMessageLine("更新任务状态成功！");
                                                LogService.Write("更新任务状态成功！");
                                            }
                                            else
                                            {
                                                //AppendMessageLine("更新任务状态失败！ 任务ID：" + item.TableSynchroTaskID.ToString() + "");
                                                LogService.Write("更新任务状态失败！ 任务ID：" + item.TableSynchroTaskID.ToString() + "");
                                            }
                                        }
                                    }
                                    else
                                    {
                                        //AppendMessageLine("UserLoginEx 未找到 UserName为：" + item.TableRowID.ToString() + "的数据");
                                        LogService.Write("UserLoginEx 未找到 UserName为：" + item.TableRowID.ToString() + "的数据");

                                        if (SynchroDBManager.DelTableSynchroTaskByTableInfo("UserLoginEx", item.TableRowID))
                                        {
                                            //AppendMessageLine("删除任务成功！");
                                            LogService.Write("删除任务成功！");
                                        }
                                        else
                                        {
                                            //AppendMessageLine("删除任务失败！ 任务ID：" + item.TableSynchroTaskID.ToString() + "");
                                            LogService.Write("删除任务失败！ 任务ID：" + item.TableSynchroTaskID.ToString() + "");
                                        }
                                    }
                                    #endregion
                                }
                                else if (item.OperationType == 1)
                                {
                                    #region //Del

                                    if (Globals.UserSynchroWebService.DeleteUserLoginExByUserName(item.TableRowID))
                                    {
                                        AppendMessageLine("同步删除成功，表名：UserLoginEx ID：" + item.TableRowID.ToString());
                                        LogService.Write("同步删除成功，表名：UserLoginEx ID：" + item.TableRowID.ToString());
                                        if (SynchroDBManager.DelTableSynchroTaskByTableInfo("UserLoginEx", item.TableRowID))
                                        {
                                            //AppendMessageLine("删除任务成功！");
                                            LogService.Write("删除任务成功！");
                                        }
                                        else
                                        {
                                            //AppendMessageLine("删除任务失败！ 任务ID：" + item.TableSynchroTaskID.ToString() + "");
                                            LogService.Write("删除任务失败！ 任务ID：" + item.TableSynchroTaskID.ToString() + "");
                                        }
                                    }
                                    else
                                    {

                                        AppendMessageLine("同步删除失败，表名：UserLoginEx UserName：" + item.TableRowID.ToString());
                                        LogService.Write("同步删除失败，表名：UserLoginEx UserName：" + item.TableRowID.ToString());
                                        if (SynchroDBManager.UpdateTableSynchroTaskStateByByTableInfo("UserLoginEx", item.TableRowID, -1))
                                        {
                                            //AppendMessageLine("更新任务状态成功！");
                                            LogService.Write("更新任务状态成功！");
                                        }
                                        else
                                        {
                                            //AppendMessageLine("更新任务状态失败！ 任务ID：" + item.TableSynchroTaskID.ToString() + "");
                                            LogService.Write("更新任务状态失败！ 任务ID：" + item.TableSynchroTaskID.ToString() + "");
                                        }
                                    }
                                    #endregion
                                }

                                break;
                            default:
                                break;
                        }
                    }
                    #endregion
                }



            }
            catch (Exception ex)
            {
                AppendMessageLine("程序错误，方法：Task 错误信息：" + ex.Message);
                LogService.WriteErr("程序错误，方法：Task 错误信息：" + ex.Message);
            }
        }

        private void UserTableSynchroFromXstudio()
        {
            try
            {
                Application.DoEvents();

                if (!isStart)
                {
                    if (isNew)
                    {
                        AppendMessageLine("任务已经停止！");

                        isNew = false;
                    }

                    return;
                }
                isNew = true;
                AppendMessageLine("开始获取任务！");
                List<TableSynchroTask> TableSynchroTaskLisy = SynchroDBManager.GetTableSynchroTaskList();
                if (TableSynchroTaskLisy == null || TableSynchroTaskLisy.Count <= 0)
                {
                    AppendMessageLine("未获取到新任务！");

                }
                else
                {
                    #region //同步Xstudio系统


                    foreach (TableSynchroTask item in TableSynchroTaskLisy)
                    {
                        if (item.TableName == "Users")
                        {
                            if (item.OperationType == 10)
                            {
                                #region //Update
                                X1UserSynchroService.UserSynchroWS.User _User = SynchroDBManager.GetUserByID(item.TableRowID.ToString());
                                if (_User != null)
                                {
                                    #region //同步频率库
                                    try
                                    {
                                        RightManagerClient.SetUrl(ConfigurationManager.AppSettings["RightManagerURL"]);
                                        RightManagerDS.SystemsDataTable sysDT = RightManagerClient.System.GetSystems("XStudio");
                                        int countOK = 0;

                                        foreach (RightManagerDS.SystemsRow sr in sysDT)
                                        {

                                            try
                                            {
                                                if (sr.Keys.Equals("XStudio", StringComparison.CurrentCultureIgnoreCase))
                                                {
                                                    countOK++;
                                                    continue;
                                                }

                                                if (RightManagerClient.User.UpdatePassword(_User.LoginName, _User.Password, _User.ModifyUserID, _User.ModifyUserName, sr.Keys))
                                                {
                                                    countOK++;
                                                    AppendMessageLine("同步：" + sr.Name + "(" + sr.Keys + ")频率" + _User.LoginName + " 用户信息成功！");
                                                    LogService.Write("同步：" + sr.Name + "(" + sr.Keys + ")频率" + _User.LoginName + " 用户信息成功！");
                                                   
                                                }
                                                else
                                                {
                                                    AppendMessageLine("同步：" + sr.Name + "(" + sr.Keys + ")频率" + _User.LoginName + " 用户信息失败！");
                                                    LogService.Write("同步：" + sr.Name + "(" + sr.Keys + ")频率" + _User.LoginName + " 用户信息失败！");
                                                }
                                            }
                                            catch (Exception ex)
                                            {
                                                AppendMessageLine("同步：" + sr.Name + "(" + sr.Keys + ")" + " 频率，异常，错误信息：" + ex.Message);
                                                LogService.Write("同步：" + sr.Name + "(" + sr.Keys + ")" + " 频率，异常，错误信息：" + ex.Message);
                                                continue;
                                            }

                                        }
                                        if (sysDT.Count == countOK)
                                        {
                                            if (SynchroDBManager.DelTableSynchroTaskByID(item.TableSynchroTaskID.ToString()))
                                            {

                                                LogService.Write("删除任务成功！");
                                            }
                                            else
                                            {

                                                LogService.Write("删除任务失败！ 任务ID：" + item.TableSynchroTaskID.ToString() + "");
                                            }
                                        }
                                        else
                                        {

                                            LogService.Write("同步更新失败，表名：Users ID：" + item.TableRowID.ToString());
                                            if (SynchroDBManager.UpdateTableSynchroTaskStateByID(item.TableSynchroTaskID.ToString(), -1))
                                            {
                                                LogService.Write("更新任务状态成功！");
                                            }
                                            else
                                            {
                                                LogService.Write("更新任务状态失败！ 任务ID：" + item.TableSynchroTaskID.ToString() + "");
                                            }

                                        }
                                    }
                                    catch (Exception ex)
                                    {
                                        AppendMessageLine("同步频率库异常，错误信息：" + ex.Message);
                                        LogService.Write("同步频率库异常，错误信息：" + ex.Message);

                                    }
                                    #endregion
                                }
                                else
                                {
                                    LogService.Write("Users 未找到 ID为：" + item.TableRowID.ToString() + "的数据");

                                    if (SynchroDBManager.DelTableSynchroTaskByID(item.TableSynchroTaskID.ToString()))
                                    {

                                        LogService.Write("删除任务成功！");
                                    }
                                    else
                                    {
                                        LogService.Write("删除任务失败！ 任务ID：" + item.TableSynchroTaskID.ToString() + "");
                                    }
                                }

                                #endregion
                            }
                            else if (item.OperationType == 1)
                            {
                                #region //Del

                                //if (Globals.UserSynchroWebService.DeleteUserByID(item.TableRowID))
                                //{
                                //    AppendMessageLine("同步删除成功，表名：Users ID：" + item.TableRowID.ToString());
                                //    LogService.Write("同步删除成功，表名：Users ID：" + item.TableRowID.ToString());
                                //    if (SynchroDBManager.DelTableSynchroTaskByID(item.TableSynchroTaskID.ToString()))
                                //    {
                                //        LogService.Write("删除任务成功！");
                                //    }
                                //    else
                                //    {
                                //        LogService.Write("删除任务失败！ 任务ID：" + item.TableSynchroTaskID.ToString() + "");
                                //    }
                                //}
                                //else
                                //{

                                //    AppendMessageLine("同步删除失败，表名：Users ID：" + item.TableRowID.ToString());
                                //    LogService.Write("同步删除失败，表名：Users ID：" + item.TableRowID.ToString());
                                //    if (SynchroDBManager.UpdateTableSynchroTaskStateByID(item.TableSynchroTaskID.ToString(), -1))
                                //    {
                                //        LogService.Write("更新任务状态成功！");
                                //    }
                                //    else
                                //    {
                                //        LogService.Write("更新任务状态失败！ 任务ID：" + item.TableSynchroTaskID.ToString() + "");
                                //    }
                                //}

                                X1UserSynchroService.UserSynchroWS.User _User = SynchroDBManager.GetUserByID(item.TableRowID.ToString());
                                if (_User != null)
                                {
                                    #region //同步频率库
                                    try
                                    {
                                        RightManagerClient.SetUrl(ConfigurationManager.AppSettings["RightManagerURL"]);
                                        RightManagerDS.SystemsDataTable sysDT = RightManagerClient.System.GetSystems("XStudio");
                                        int countOK = 0;
                                        foreach (RightManagerDS.SystemsRow sr in sysDT)
                                        {
                                            try
                                            {
                                                if (sr.Keys.Equals("XStudio", StringComparison.CurrentCultureIgnoreCase))
                                                {
                                                    countOK++;
                                                    continue;
                                                }

                                                if (RightManagerClient.User.DeleteUserByLoginName(_User.LoginName, sr.Keys))
                                                {
                                                    countOK++;
                                                    AppendMessageLine("同步删除：" + sr.Name + "(" + sr.Keys + ")频率" + _User.LoginName + " 用户信息成功！");
                                                    LogService.Write("同步删除：" + sr.Name + "(" + sr.Keys + ")频率" + _User.LoginName + " 用户信息成功！");
                                                    
                                                }
                                            }
                                            catch (Exception ex)
                                            {
                                                AppendMessageLine("同步删除：" + sr.Name + "(" + sr.Keys + ")" + " 频率，异常，错误信息：" + ex.Message);
                                                LogService.Write("同步删除：" + sr.Name + "(" + sr.Keys + ")" + " 频率，异常，错误信息：" + ex.Message);
                                                continue;
                                            }

                                        }
                                        if (sysDT.Count == countOK)
                                        {
                                            if (SynchroDBManager.DelTableSynchroTaskByID(item.TableSynchroTaskID.ToString()))
                                            {

                                                LogService.Write("删除任务成功！");
                                            }
                                            else
                                            {

                                                LogService.Write("删除任务失败！ 任务ID：" + item.TableSynchroTaskID.ToString() + "");
                                            }
                                        }
                                        else
                                        {

                                            LogService.Write("同步更新失败，表名：Users ID：" + item.TableRowID.ToString());
                                            if (SynchroDBManager.UpdateTableSynchroTaskStateByID(item.TableSynchroTaskID.ToString(), -1))
                                            {
                                                LogService.Write("更新任务状态成功！");
                                            }
                                            else
                                            {
                                                LogService.Write("更新任务状态失败！ 任务ID：" + item.TableSynchroTaskID.ToString() + "");
                                            }

                                        }
                                    }
                                    catch (Exception ex)
                                    {
                                        AppendMessageLine("同步频率库异常，错误信息：" + ex.Message);
                                        LogService.Write("同步频率库异常，错误信息：" + ex.Message);

                                    }
                                    #endregion
                                }
                                else
                                {
                                    LogService.Write("Users 未找到 ID为：" + item.TableRowID.ToString() + "的数据");

                                    if (SynchroDBManager.DelTableSynchroTaskByID(item.TableSynchroTaskID.ToString()))
                                    {

                                        LogService.Write("删除任务成功！");
                                    }
                                    else
                                    {
                                        LogService.Write("删除任务失败！ 任务ID：" + item.TableSynchroTaskID.ToString() + "");
                                    }
                                }
                                #endregion
                            }
                        }

                    }
                    #endregion

                }

            }
            catch (Exception ex)
            {
                AppendMessageLine("程序错误，方法：UserSynchroTask 错误信息：" + ex.Message);
                LogService.WriteErr("程序错误，方法：UserSynchroTask 错误信息：" + ex.Message);
            }
        }


        private void Task()
        {
            while (true)
            {
                switch (Globals.SynchroType)
                {
                    case 0://同步修改密码
                        UserTableSynchroFromXstudio();
                        break;
                    case 1://河南台
                        UserSynchroTask();
                        break;
                    default:
                        UserTableSynchroFromXstudio();
                        break;
                }
                
               
                Thread.Sleep(Globals.RequestInterval);
            }

        }
        #endregion
        #region//窗体事件
        private void FrmMain_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (IsStart)
            {
                MessageBox.Show("监测运行中，请先停止！", "提示！", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                e.Cancel = true;
            }
            else if (isNew)
            {
                MessageBox.Show("监测运行中，请先停止！", "提示！", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                e.Cancel = true;
            }



        }
        private void notifyIcon_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            //判断是否已经最小化于托盘 
            if (WindowState == FormWindowState.Minimized)
            {
                //还原窗体显示 
                WindowState = FormWindowState.Normal;
                //激活窗体并给予它焦点 
                this.Activate();
                //任务栏区显示图标 
                this.ShowInTaskbar = true;
                //托盘区图标隐藏 
                notifyIcon.Visible = false;
            }
        }
        private void FrmMain_SizeChanged(object sender, EventArgs e)
        {
            //判断是否选择的是最小化按钮 
            if (WindowState == FormWindowState.Minimized)
            {
                //托盘显示图标等于托盘图标对象 
                //注意notifyIcon1是控件的名字而不是对象的名字 

                //隐藏任务栏区图标 
                this.ShowInTaskbar = false;
                //图标显示在托盘区 
                notifyIcon.Visible = true;
            }
        }

        private void btnShowForm_Click(object sender, EventArgs e)
        {
            notifyIcon_MouseDoubleClick(sender, null);
        }
        private void btnStart_Click(object sender, EventArgs e)
        {
            IsStart = !IsStart;

        }

        private void btnSetting_Click(object sender, EventArgs e)
        {


            if (IsStart)
            {
                MessageBox.Show("监测运行中，请先停止！", "提示！", MessageBoxButtons.OK, MessageBoxIcon.Warning);

            }
            else if (isNew)
            {
                MessageBox.Show("监测运行中，请先停止！", "提示！", MessageBoxButtons.OK, MessageBoxIcon.Warning);

            }
        }
        private void btnCloseForm_Click(object sender, EventArgs e)
        {
            if (IsStart)
            {
                notifyIcon_MouseDoubleClick(sender, null);
                MessageBox.Show("监测运行中，请先停止！", "提示！", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else if (isNew)
            {
                notifyIcon_MouseDoubleClick(sender, null);
                MessageBox.Show("监测运行中，请先停止！", "提示！", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else
            {
                Close();
            }
        }
        #endregion


    }
}
