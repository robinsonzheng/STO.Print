﻿//-----------------------------------------------------------------
// All Rights Reserved , Copyright (C) 2016 , Hairihan TECH, Ltd. 
//-----------------------------------------------------------------

using System.Collections.Generic;
using System.Data;
using System.Reflection;
using System.ServiceModel;

namespace DotNet.Business
{
    using DotNet.IService;
    using DotNet.Model;
    using DotNet.Utilities;

    /// <summary>
    /// ServicesLicenseService
    /// 参数服务
    /// 
    /// 修改记录
    /// 
    ///		2015.12.26 版本：1.0 JiRiGaLa 创建。
    ///	
    /// <author>
    ///		<name>JiRiGaLa</name>
    ///		<date>2015.12.26</date>
    /// </author> 
    /// </summary>
    [ServiceBehavior(IncludeExceptionDetailInFaults = true)]
    [System.ServiceModel.Activation.AspNetCompatibilityRequirements(RequirementsMode = System.ServiceModel.Activation.AspNetCompatibilityRequirementsMode.Allowed)]
    public class ServicesLicenseService : IServicesLicenseService
    {
        #region public DataTable GetDataTableByUser(BaseUserInfo userInfo, string userId) 获取列表
        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="userInfo">用户</param>
        /// <param name="userId">用户主键</param>
        /// <returns>数据表</returns>
        public DataTable GetDataTableByUser(BaseUserInfo userInfo, string userId)
        {
            var result = new DataTable(BaseServicesLicenseEntity.TableName);

            var parameter = ServiceInfo.Create(userInfo, MethodBase.GetCurrentMethod());
            ServiceUtil.ProcessUserCenterReadDb(userInfo, parameter, (dbHelper) =>
            {
                var manager = new BaseServicesLicenseManager(dbHelper, userInfo);
                List<KeyValuePair<string, object>> parameters = new List<KeyValuePair<string, object>>();
                parameters.Add(new KeyValuePair<string, object>(BaseServicesLicenseEntity.FieldUserId, userId));
                parameters.Add(new KeyValuePair<string, object>(BaseServicesLicenseEntity.FieldDeletionStateCode, 0));
                parameters.Add(new KeyValuePair<string, object>(BaseServicesLicenseEntity.FieldEnabled, 1));
                result = manager.GetDataTable(parameters);
                result.TableName = BaseServicesLicenseEntity.TableName;
            });

            return result;
        }
        #endregion

        /// <summary>
        /// 添加实体
        /// </summary>
        /// <param name="userInfo">用户</param>
        /// <param name="entity">实体</param>
        /// <returns>主键</returns>
        public string Add(BaseUserInfo userInfo, BaseServicesLicenseEntity entity)
        {
            string result = string.Empty;

            var parameter = ServiceInfo.Create(userInfo, MethodBase.GetCurrentMethod());
            ServiceUtil.ProcessUserCenterWriteDbWithTransaction(userInfo, parameter, (dbHelper) =>
            {
                var manager = new BaseServicesLicenseManager(dbHelper, userInfo);
                result = manager.AddObject(entity);
            });

            return result;
        }

        #region public BaseServicesLicenseEntity GetObject(BaseUserInfo userInfo, string id)
        /// <summary>
        /// 获取实体
        /// </summary>
        /// <param name="userInfo">用户</param>
        /// <param name="id">主键</param>
        /// <returns>实体</returns>
        public BaseServicesLicenseEntity GetObject(BaseUserInfo userInfo, string id)
        {
            BaseServicesLicenseEntity entity = null;

            var parameter = ServiceInfo.Create(userInfo, MethodBase.GetCurrentMethod()); 
            ServiceUtil.ProcessUserCenterReadDb(userInfo, parameter, (dbHelper) =>
            {
                var manager = new BaseServicesLicenseManager(dbHelper, userInfo);
                entity = manager.GetObject(id);
            });

            return entity;
        }
        #endregion

        #region public int Update(BaseUserInfo userInfo, string tableName, BaseParameterEntity entity)
        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="userInfo">用户</param>
        /// <param name="tableName">表名</param>
        /// <param name="entity">实体</param>
        /// <returns>影响行数</returns>
        public int Update(BaseUserInfo userInfo, string tableName, BaseServicesLicenseEntity entity)
        {
            int result = 0;

            string returnCode = string.Empty;
            string returnMessage = string.Empty;

            var parameter = ServiceInfo.Create(userInfo, MethodBase.GetCurrentMethod());
            ServiceUtil.ProcessUserCenterWriteDb(userInfo, parameter, (dbHelper) =>
            {
                var manager = new BaseServicesLicenseManager(dbHelper, userInfo, tableName);
                result = manager.UpdateObject(entity);
            });

            return result;
        }
        #endregion

        #region public int SetDeleted(BaseUserInfo userInfo, string tableName, string[] ids)
        /// <summary>
        /// 批量打删除标志
        /// </summary>
        /// <param name="userInfo">用户</param>
        /// <param name="tableName">表名</param>
        /// <param name="ids">主键数组</param>
        /// <returns>影响行数</returns>
        public int SetDeleted(BaseUserInfo userInfo, string tableName, string[] ids)
        {
            int result = 0;

            var parameter = ServiceInfo.Create(userInfo, MethodBase.GetCurrentMethod());
            ServiceUtil.ProcessUserCenterWriteDb(userInfo, parameter, (dbHelper) =>
            {
                var manager = new BaseServicesLicenseManager(dbHelper, userInfo, tableName);
                for (int i = 0; i < ids.Length; i++)
                {
                    // 设置为删除状态
                    result += manager.SetDeleted(ids[i], true, true);
                }
            });

            return result;
        }
        #endregion
    }
}