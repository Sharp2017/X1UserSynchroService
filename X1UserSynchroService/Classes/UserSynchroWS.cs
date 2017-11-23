using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using X1UserSynchroService.UserSynchroWS;

namespace X1UserSynchroService.Classes
{
   public class UserSynchroWS
    {
        UserSynchroWebService service = null;
        ArrayList arr = new ArrayList();
        public UserSynchroWS()
        {
            service = new UserSynchroWebService();
        }
        private void checkUrl()
        {
            Random ran = new Random();
            int tmpRan = ran.Next();
            System.Collections.IEnumerator ie = this.arr.GetEnumerator();
            int count = 0;
            while (ie.MoveNext())
            {
                if (ie.Current.ToString() == Globals.UserSynchroWebServiceUrl)
                {
                    count++;
                }
            }

            if (count >= 3)
            {
                this.service.Url = Globals.UserSynchroWebServiceUrl1;
                this.service.Timeout = 50000;
                return;
            }

            ie = this.arr.GetEnumerator();
            count = 0;
            while (ie.MoveNext())
            {
                if (ie.Current.ToString() == Globals.UserSynchroWebServiceUrl1)
                {
                    count++;
                }
            }

            if (count >= 3)
            {
                this.service.Url = Globals.UserSynchroWebServiceUrl;
                this.service.Timeout = 50000;
                return;
            }
            if (tmpRan % 2 == 0)
            {

                try
                {
                    service.Url = Globals.UserSynchroWebServiceUrl;
                    service.HelloWorld(); 

                }
                catch (System.Exception ex)
                {
                    arr.Add(Globals.UserSynchroWebServiceUrl);
                    service.Url = Globals.UserSynchroWebServiceUrl1;
                }
            }
            else
            {

                try
                {
                    service.Url = Globals.UserSynchroWebServiceUrl1;
                    service.HelloWorld();

                }
                catch (System.Exception ex)
                {
                    arr.Add(Globals.UserSynchroWebServiceUrl1);
                    service.Url = Globals.UserSynchroWebServiceUrl;
                }
            }

            this.service.Timeout = 50000;
        }

        #region //UserTable
        public bool UpdateUser(User ur)
        {
            this.checkUrl();
            try
            {
                return this.service.UpdateUser(ur);
            }
            catch
            {
                try
                {
                    return this.service.UpdateUser(ur);
                }
                catch (System.Exception ex)
                {
                    LogService.WriteErr("UserSynchroWebService，地址是：" + this.service.Url + " 方法：UpdateUser 原因：" + ex.Message);
                    return false;
                }
            }
        }
        public bool DeleteUserByID(string id)
        {
            this.checkUrl();
            try
            {
                return this.service.DeleteUserByID(id);
            }
            catch
            {
                try
                {
                    return this.service.DeleteUserByID(id);
                }
                catch (System.Exception ex)
                {
                    LogService.WriteErr("UserSynchroWebService，地址是：" + this.service.Url + " 方法：DeleteUserByID 原因：" + ex.Message);
                    return false;
                }
            }
        }
        #endregion
        #region //UserFingerInfoTable
        public bool UpdateUserFingerInfo(UserFingerInfo ur)
        {
            this.checkUrl();
            try
            {
                return this.service.UpdateUserFingerInfo(ur);
            }
            catch
            {
                try
                {
                    return this.service.UpdateUserFingerInfo(ur);
                }
                catch (System.Exception ex)
                {
                    LogService.WriteErr("UserSynchroWebService，地址是：" + this.service.Url + " 方法：UpdateUserFingerInfo 原因：" + ex.Message);
                    return false;
                }
            }
        }
        public bool DeleteUserFingerInfoByID(string id)
        {
            this.checkUrl();
            try
            {
                return this.service.DeleteUserFingerInfoByID(id);
            }
            catch
            {
                try
                {
                    return this.service.DeleteUserFingerInfoByID(id);
                }
                catch (System.Exception ex)
                {
                    LogService.WriteErr("UserSynchroWebService，地址是：" + this.service.Url + " 方法：DeleteUserFingerInfoByID 原因：" + ex.Message);
                    return false;
                }
            }
        }
        #endregion

        #region //UserGroupsTable
        public bool UpdateUserGroups(UserGroup ur)
        {
            this.checkUrl();
            try
            {
                return this.service.UpdateUserGroups(ur);
            }
            catch
            {
                try
                {
                    return this.service.UpdateUserGroups(ur);
                }
                catch (System.Exception ex)
                {
                    LogService.WriteErr("UserSynchroWebService，地址是：" + this.service.Url + " 方法：UpdateUserGroups  原因：" + ex.Message);
                    return false;
                }
            }
        }
        public bool DeleteUserGroupByID(string id)
        {
            this.checkUrl();
            try
            {
                return this.service.DeleteUserGroup(id);
            }
            catch
            {
                try
                {
                    return this.service.DeleteUserGroup(id);
                }
                catch (System.Exception ex)
                {
                    LogService.WriteErr("UserSynchroWebService，地址是：" + this.service.Url + "  方法：DeleteUserGroupByID 原因：" + ex.Message);
                    return false;
                }
            }
        }
        #endregion

        #region //UserGroup2User
        public bool UpdateUserGroup2User(UserGroup2User ur)
        {
            this.checkUrl();
            try
            {
                return this.service.UpdateUserGroup2User(ur);
            }
            catch
            {
                try
                {
                    return this.service.UpdateUserGroup2User(ur);
                }
                catch (System.Exception ex)
                {
                    LogService.WriteErr("UserSynchroWebService，地址是：" + this.service.Url + "方法： UpdateUserGroup2User 原因：" + ex.Message);
                    return false;
                }
            }
        }
        public bool DeleteUserGroup2UserByID(string id)
        {
            this.checkUrl();
            try
            {
                return this.service.DeleteUserGroup2User(id);
            }
            catch
            {
                try
                {
                    return this.service.DeleteUserGroup2User(id);
                }
                catch (System.Exception ex)
                {
                    LogService.WriteErr("UserSynchroWebService，地址是：" + this.service.Url + "方法：DeleteUserGroup2UserByID  原因：" + ex.Message);
                    return false;
                }
            }
        }
        #endregion

        #region //UserGroup2UserGroup
        public bool UpdateUserGroup2UserGroup(UserGroup2UserGroup ur)
        {
            this.checkUrl();
            try
            {
                return this.service.UpdateUserGroup2UserGroup(ur);
            }
            catch
            {
                try
                {
                    return this.service.UpdateUserGroup2UserGroup(ur);
                }
                catch (System.Exception ex)
                {
                    LogService.WriteErr("UserSynchroWebService，地址是：" + this.service.Url + "方法：UpdateUserGroup2UserGroup 原因：" + ex.Message);
                    return false;
                }
            }
        }
        public bool DeleteUserGroup2UserGroupByID(string id)
        {
            this.checkUrl();
            try
            {
                return this.service.DeleteUserGroup2UserGroup(id);
            }
            catch
            {
                try
                {
                    return this.service.DeleteUserGroup2UserGroup(id);
                }
                catch (System.Exception ex)
                {
                    LogService.WriteErr("UserSynchroWebService，地址是：" + this.service.Url + "方法：DeleteUserGroup2UserGroupByID 原因：" + ex.Message);
                    return false;
                }
            }
        }
        #endregion

        #region //UserLogin
        public bool UpdateUserLogin(UserLogin ur)
        {
            this.checkUrl();
            try
            {
                return this.service.UpdateUserLogin(ur);
            }
            catch
            {
                try
                {
                    return this.service.UpdateUserLogin(ur);
                }
                catch (System.Exception ex)
                {
                    LogService.WriteErr("UserSynchroWebService，地址是：" + this.service.Url + "方法：UpdateUserLogin 原因：" + ex.Message);
                    return false;
                }
            }
        }
        public bool DeleteUserLoginByID(string id)
        {
            this.checkUrl();
            try
            {
                return this.service.DeleteUserLogin(id);
            }
            catch
            {
                try
                {
                    return this.service.DeleteUserLogin(id);
                }
                catch (System.Exception ex)
                {
                    LogService.WriteErr("UserSynchroWebService，地址是：" + this.service.Url + "方法：DeleteUserLoginByID 原因：" + ex.Message);
                    return false;
                }
            }
        }
        #endregion

        #region //UserLoginEx
        public bool UpdateUserLoginEx(UserLoginEx ur)
        {
            this.checkUrl();
            try
            {
                return this.service.UpdateUserLoginEx(ur);
            }
            catch
            {
                try
                {
                    return this.service.UpdateUserLoginEx(ur);
                }
                catch (System.Exception ex)
                {
                    LogService.WriteErr("UserSynchroWebService，地址是：" + this.service.Url + "方法：UpdateUserLoginEx 原因：" + ex.Message);
                    return false;
                }
            }
        }
        public bool UpdateUserLoginExList(UserLoginEx[] ur)
        {
            this.checkUrl();
            try
            {
                return this.service.UpdateUserLoginExList(ur);
            }
            catch
            {
                try
                {
                    return this.service.UpdateUserLoginExList(ur);
                }
                catch (System.Exception ex)
                {
                    LogService.WriteErr("UserSynchroWebService，地址是：" + this.service.Url + "方法：UpdateUserLoginExList 原因：" + ex.Message);
                    return false;
                }
            }
        }
        public bool DeleteUserLoginExByUserName(string pUserName)
        {
            this.checkUrl();
            try
            {
                return this.service.DeleteUserLoginEx(pUserName);
            }
            catch
            {
                try
                {
                    return this.service.DeleteUserLoginEx(pUserName);
                }
                catch (System.Exception ex)
                {
                    LogService.WriteErr("UserSynchroWebService，地址是：" + this.service.Url + "方法：DeleteUserLoginExByUserName 原因：" + ex.Message);
                    return false;
                }
            }
        }
        #endregion
    }
}
