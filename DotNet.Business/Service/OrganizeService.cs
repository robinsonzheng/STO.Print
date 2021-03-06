﻿//-----------------------------------------------------------------
// All Rights Reserved , Copyright (C) 2016 , Hairihan TECH, Ltd. 
//-----------------------------------------------------------------

using System;
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
    /// OrganizeService
    /// 组织机构服务
    /// 
    /// 修改记录
    /// 
    ///		2014.01.30 版本：3.0 JiRiGaLa 增加判断重复的Exists代码。
    ///		2013.06.06 版本：3.0 张祈璟重构
	///		2007.08.15 版本：2.2 JiRiGaLa 改进运行速度采用 WebService 变量定义 方式处理数据。
    ///		2007.08.14 版本：2.1 JiRiGaLa 改进运行速度采用 Instance 方式处理数据。
    ///     2007.06.11 版本：1.3 JiRiGaLa 加入调试信息#if (DEBUG)。
    ///     2007.06.04 版本：1.2 JiRiGaLa 加入WebService。
    ///     2007.05.29 版本：1.1 JiRiGaLa 排版，修改，完善。
    ///		2007.05.11 版本：1.0 JiRiGaLa 窗体与数据库连接的分离。
    ///	
    /// <author>
    ///		<name>JiRiGaLa</name>
    ///		<date>2007.08.15</date>
    /// </author> 
    /// </summary>
    [ServiceBehavior(IncludeExceptionDetailInFaults = true)]
    [System.ServiceModel.Activation.AspNetCompatibilityRequirements(RequirementsMode = System.ServiceModel.Activation.AspNetCompatibilityRequirementsMode.Allowed)]
    public partial class OrganizeService : IOrganizeService
    {
        #region public bool Exists(BaseUserInfo userInfo, List<KeyValuePair<string, object>> parameters, string id)
        /// <summary>
        /// 判断字段是否重复
        /// </summary>
        /// <param name="userInfo">用户</param>
        /// <param name="parameters">字段名,字段值</param>
        /// <param name="id">主键</param>
        /// <returns>已存在</returns>
        public bool Exists(BaseUserInfo userInfo, List<KeyValuePair<string, object>> parameters, string id)
        {
            bool result = false;

            var parameter = ServiceInfo.Create(userInfo, MethodBase.GetCurrentMethod());
            ServiceUtil.ProcessUserCenterReadDb(userInfo, parameter, (dbHelper) =>
            {
                var manager = new BaseManager(dbHelper, userInfo, BaseOrganizeEntity.TableName);
                result = manager.Exists(parameters, id);
            });
            return result;
        }
        #endregion

        #region public string Add(BaseUserInfo userInfo, BaseOrganizeEntity entity, out string statusCode, out string statusMessage)
        /// <summary>
        /// 添加实体
        /// </summary>
        /// <param name="userInfo">用户</param>
        /// <param name="entity">实体</param>
        /// <param name="statusCode">状态码</param>
        /// <param name="statusMessage">状态信息</param>
        /// <returns>主键</returns>
        public string Add(BaseUserInfo userInfo, BaseOrganizeEntity entity, out string statusCode, out string statusMessage)
        {
            string result = string.Empty;
            string returnCode = string.Empty;
			string returnMessage = string.Empty;
			
            var parameter = ServiceInfo.Create(userInfo, MethodBase.GetCurrentMethod());
            ServiceUtil.ProcessUserCenterWriteDb(userInfo, parameter, (dbHelper) =>
			{
				var manager = new BaseOrganizeManager(dbHelper, userInfo);
				result = manager.Add(entity);
				returnMessage = manager.GetStateMessage(returnCode);
				if(returnCode.Equals(Status.OKAdd.ToString()))
				{
					entity.Id = result;
					var folderManager = new BaseFolderManager(dbHelper, userInfo);
					folderManager.FolderCheck(entity.Id.ToString(), entity.FullName);
				}
			});
			statusCode = returnCode;
			statusMessage = returnMessage;
			return result;
        }
        #endregion

        #region public string AddByDetail(BaseUserInfo userInfo, string parentId, string code, string fullName, string categoryId, string outerPhone, string innerPhone, string fax, bool enabled, out string statusCode, out string statusMessage)
        /// <summary>
        /// 按详细情况添加实体
        /// </summary>
        /// <param name="userInfo">用户</param>
        /// <param name="parentId">父主键</param>
        /// <param name="code">编号</param>
        /// <param name="fullName">全称</param>
        /// <param name="categoryId">分类</param>
        /// <param name="outerPhone">外线</param>
        /// <param name="innerPhone">内线</param>
        /// <param name="fax">传真</param>
        /// <param name="enabled">有效</param>
        /// <param name="statusCode">状态码</param>
        /// <param name="statusMessage">状态信息</param>
        /// <returns>主键</returns>
        public string AddByDetail(BaseUserInfo userInfo, string parentId, string code, string fullName, string categoryId, string outerPhone, string innerPhone, string fax, bool enabled, out string statusCode, out string statusMessage)
        {
            string result = string.Empty;
            
            string returnCode = string.Empty;
			string returnMessage = string.Empty;
			
            var parameter = ServiceInfo.Create(userInfo, MethodBase.GetCurrentMethod());
            ServiceUtil.ProcessUserCenterWriteDb(userInfo, parameter, (dbHelper) =>
			{
				var manager = new BaseOrganizeManager(dbHelper, userInfo);
				result = manager.AddByDetail(parentId, code, fullName, categoryId, outerPhone, innerPhone, fax, enabled);
				returnMessage = manager.GetStateMessage(returnCode);
			});
			statusCode = returnCode;
			statusMessage = returnMessage;
			return result;
        }
        #endregion

        #region public BaseOrganizeEntity GetObject(BaseUserInfo userInfo, string id)
        /// <summary>
        /// 获取实体
        /// </summary>
        /// <param name="userInfo">用户</param>
        /// <param name="id">主键</param>
        /// <returns>实体</returns>
        public BaseOrganizeEntity GetObject(BaseUserInfo userInfo, string id)
        {
			BaseOrganizeEntity entity = null;

            var parameter = ServiceInfo.Create(userInfo, MethodBase.GetCurrentMethod());
            ServiceUtil.ProcessUserCenterReadDb(userInfo, parameter, (dbHelper) =>
			{
				var manager = new BaseOrganizeManager(dbHelper, userInfo);
				entity = manager.GetObject(id);
			});
			return entity;
        }
        #endregion

        #region public BaseOrganizeEntity GetObjectByCode(BaseUserInfo userInfo, string code)
        /// <summary>
        /// 按编号获取实体
        /// </summary>
        /// <param name="userInfo">用户</param>
        /// <param name="code">编号</param>
        /// <returns>实体</returns>
        public BaseOrganizeEntity GetObjectByCode(BaseUserInfo userInfo, string code)
        {
            BaseOrganizeEntity entity = null;

            var parameter = ServiceInfo.Create(userInfo, MethodBase.GetCurrentMethod());
            ServiceUtil.ProcessUserCenterReadDb(userInfo, parameter, (dbHelper) =>
            {
                var manager = new BaseOrganizeManager(dbHelper, userInfo);
                entity = manager.GetObjectByCode(code);
            });
            return entity;
        }
        #endregion

        #region public BaseOrganizeEntity GetObjectByName(BaseUserInfo userInfo, string fullName)
        /// <summary>
        /// 按名称获取实体
        /// </summary>
        /// <param name="userInfo">用户</param>
        /// <param name="fullName">名称</param>
        /// <returns>实体</returns>
        public BaseOrganizeEntity GetObjectByName(BaseUserInfo userInfo, string fullName)
        {
            BaseOrganizeEntity entity = null;

            var parameter = ServiceInfo.Create(userInfo, MethodBase.GetCurrentMethod());
            ServiceUtil.ProcessUserCenterReadDb(userInfo, parameter, (dbHelper) =>
            {
                var manager = new BaseOrganizeManager(dbHelper, userInfo);
                entity = manager.GetObjectByName(fullName);
            });
            return entity;
        }
        #endregion

        #region public DataTable GetDataTable(BaseUserInfo userInfo)
        /// <summary>
        /// 获取部门列表
        /// </summary>
        /// <param name="userInfo">用户</param>
        /// <returns>数据表</returns>
        public DataTable GetDataTable(BaseUserInfo userInfo)
        {
			var dt = new DataTable(BaseOrganizeEntity.TableName);

            var parameter = ServiceInfo.Create(userInfo, MethodBase.GetCurrentMethod());
            ServiceUtil.ProcessUserCenterReadDb(userInfo, parameter, (dbHelper) =>
			{
				// 获得组织机构列表
                string commandText = string.Empty;
                if (BaseSystemInfo.OrganizeDynamicLoading)
                {
                    commandText = "    SELECT * "
                                + "      FROM " + BaseOrganizeEntity.TableName
                                + "     WHERE " + BaseOrganizeEntity.FieldDeletionStateCode + " = 0 "
                                + "           AND " + BaseOrganizeEntity.FieldIsInnerOrganize + " = 1 "
                                + "           AND " + BaseOrganizeEntity.FieldEnabled + " = 1 "
                                + "           AND (" + BaseOrganizeEntity.FieldParentId + " IS NULL "
                                + "                OR " + BaseOrganizeEntity.FieldParentId + " IN (SELECT " + BaseOrganizeEntity.FieldId + " FROM " + BaseOrganizeEntity.TableName + " WHERE " + BaseOrganizeEntity.FieldDeletionStateCode + " = 0 AND " + BaseOrganizeEntity.FieldIsInnerOrganize + " = 1 AND " + BaseOrganizeEntity.FieldEnabled + " = 1 AND " + BaseOrganizeEntity.FieldParentId + " IS NULL)) "
                                + "  ORDER BY " + BaseOrganizeEntity.FieldSortCode;
                }
                else
                {
                    commandText = "    SELECT * "
                                + "      FROM " + BaseOrganizeEntity.TableName
                                + "     WHERE " + BaseOrganizeEntity.FieldDeletionStateCode + " = 0 "
                                + "           AND " + BaseOrganizeEntity.FieldIsInnerOrganize + " = 1 "
                                + "           AND " + BaseOrganizeEntity.FieldEnabled + " = 1 "
                                + "  ORDER BY " + BaseOrganizeEntity.FieldSortCode;
                }


				// var manager = new BaseOrganizeManager(dbHelper, result);
				// result = manager.GetDataTable(new KeyValuePair<string, object>(BaseOrganizeEntity.FieldDeletionStateCode, 0), BaseOrganizeEntity.FieldSortCode);
                dt = dbHelper.Fill(commandText);
                dt.DefaultView.Sort = BaseOrganizeEntity.FieldSortCode;
				dt.TableName = BaseOrganizeEntity.TableName;
			});
			return dt;
        }
        #endregion

        #region public DataTable GetDataTableByIds(BaseUserInfo userInfo, string[] ids)
        /// <summary>
        /// 按主键数组获取列表
        /// </summary>
        /// <param name="userInfo">用户</param>
        /// <param name="ids">组织机构主键</param>
        /// <returns>数据表</returns>
        public DataTable GetDataTableByIds(BaseUserInfo userInfo, string[] ids)
        {
			var dt = new DataTable(BaseOrganizeEntity.TableName);

            var parameter = ServiceInfo.Create(userInfo, MethodBase.GetCurrentMethod());
            ServiceUtil.ProcessUserCenterReadDb(userInfo, parameter, (dbHelper) =>
			{
				var manager = new BaseOrganizeManager(dbHelper, userInfo);
				dt = manager.GetDataTable(BaseOrganizeEntity.FieldId, ids, BaseOrganizeEntity.FieldSortCode);
				dt.TableName = BaseOrganizeEntity.TableName;
			});
			return dt;
        }
        #endregion

        #region public DataTable GetDataTable(BaseUserInfo userInfo, List<KeyValuePair<string, object>> parameters)
        /// <summary>
        /// 获取部门列表
        /// </summary>
        /// <param name="userInfo">用户</param>
        /// <param name="parameters">参数</param>
        /// <returns>数据表</returns>
        public DataTable GetDataTable(BaseUserInfo userInfo, List<KeyValuePair<string, object>> parameters)
        {
			var dt = new DataTable(BaseOrganizeEntity.TableName);

            var parameter = ServiceInfo.Create(userInfo, MethodBase.GetCurrentMethod());
            ServiceUtil.ProcessUserCenterReadDb(userInfo, parameter, (dbHelper) =>
			{
				// 获得组织机构列表
				var manager = new BaseOrganizeManager(dbHelper, userInfo);
				dt = manager.GetDataTable(parameters, BaseOrganizeEntity.FieldSortCode);
				dt.DefaultView.Sort = BaseOrganizeEntity.FieldSortCode;
				dt.TableName = BaseOrganizeEntity.TableName;
			});
			return dt;
        }
        #endregion

        /// <summary>
        /// 获得列表
        /// </summary>
        /// <param name="userInfo">用户</param>
        /// <param name="parentId">父亲节点主键</param>
        /// <returns>数据表</returns>
        public DataTable GetErrorDataTable(BaseUserInfo userInfo, string parentId)
        {
            var dt = new DataTable(BaseOrganizeEntity.TableName);

            var parameter = ServiceInfo.Create(userInfo, MethodBase.GetCurrentMethod());
            ServiceUtil.ProcessUserCenterReadDb(userInfo, parameter, (dbHelper) =>
            {
                // 这里可以缓存起来，提高效率
                var manager = new BaseOrganizeManager(dbHelper, userInfo);
                dt = manager.GetErrorDataTable(parentId);
                dt.DefaultView.Sort = BaseOrganizeEntity.FieldSortCode;
                dt.TableName = BaseOrganizeEntity.TableName;
            });
            return dt;
        }

        #region public DataTable GetDataTableByParent(BaseUserInfo userInfo, string parentId)
        /// <summary>
        /// 按父节点获取列表
        /// </summary>
        /// <param name="userInfo">用户</param>
        /// <param name="parentId">父节点</param>
        /// <returns>数据表</returns>
        public DataTable GetDataTableByParent(BaseUserInfo userInfo, string parentId)
        {
			var dt = new DataTable(BaseOrganizeEntity.TableName);

            var parameter = ServiceInfo.Create(userInfo, MethodBase.GetCurrentMethod());
            ServiceUtil.ProcessUserCenterReadDb(userInfo, parameter, (dbHelper) =>
			{
                // 这里可以缓存起来，提高效率
                var manager = new BaseOrganizeManager(dbHelper, userInfo);
                // 这里是条件字段
				List<KeyValuePair<string, object>> parameters = new List<KeyValuePair<string, object>>();
				parameters.Add(new KeyValuePair<string, object>(BaseOrganizeEntity.FieldParentId, parentId));
				parameters.Add(new KeyValuePair<string, object>(BaseOrganizeEntity.FieldEnabled, 1));
				parameters.Add(new KeyValuePair<string, object>(BaseOrganizeEntity.FieldDeletionStateCode, 0));
                // 获取列表，指定排序字段
				dt = manager.GetDataTable(parameters, BaseOrganizeEntity.FieldSortCode);
                dt.DefaultView.Sort = BaseOrganizeEntity.FieldSortCode;
				dt.TableName = BaseOrganizeEntity.TableName;
			});
			return dt;
        }
        #endregion

        /// <summary>
        /// 获得列表
        /// </summary>
        /// <param name="userInfo">用户</param>
        /// <param name="provinceId">省主键</param>
        /// <returns>数据表</returns>
        public DataTable GetDataTableByProvinceId(BaseUserInfo userInfo, string provinceId)
        {
            var result = new DataTable(BaseOrganizeEntity.TableName);

            var parameter = ServiceInfo.Create(userInfo, MethodBase.GetCurrentMethod()); 
            ServiceUtil.ProcessUserCenterReadDb(userInfo, parameter, (dbHelper) =>
            {
                // 这里可以缓存起来，提高效率
                var manager = new BaseOrganizeManager(dbHelper, userInfo);
                // 这里是条件字段
                List<KeyValuePair<string, object>> parameters = new List<KeyValuePair<string, object>>();
                parameters.Add(new KeyValuePair<string, object>(BaseOrganizeEntity.FieldProvinceId, provinceId));
                parameters.Add(new KeyValuePair<string, object>(BaseOrganizeEntity.FieldEnabled, 1));
                parameters.Add(new KeyValuePair<string, object>(BaseOrganizeEntity.FieldDeletionStateCode, 0));
                // 获取列表，指定排序字段
                result = manager.GetDataTable(parameters, BaseOrganizeEntity.FieldSortCode);
                result.DefaultView.Sort = BaseOrganizeEntity.FieldSortCode;
                result.TableName = BaseOrganizeEntity.TableName;
            });

            return result;
        }

        /// <summary>
        /// 获得列表
        /// </summary>
        /// <param name="userInfo">用户</param>
        /// <param name="cityId">市主键</param>
        /// <returns>数据表</returns>
        public DataTable GetDataTableByCityId(BaseUserInfo userInfo, string cityId)
        {
            var result = new DataTable(BaseOrganizeEntity.TableName);

            var parameter = ServiceInfo.Create(userInfo, MethodBase.GetCurrentMethod());
            ServiceUtil.ProcessUserCenterReadDb(userInfo, parameter, (dbHelper) =>
            {
                // 这里可以缓存起来，提高效率
                var manager = new BaseOrganizeManager(dbHelper, userInfo);
                // 这里是条件字段
                List<KeyValuePair<string, object>> parameters = new List<KeyValuePair<string, object>>();
                parameters.Add(new KeyValuePair<string, object>(BaseOrganizeEntity.FieldCityId, cityId));
                parameters.Add(new KeyValuePair<string, object>(BaseOrganizeEntity.FieldEnabled, 1));
                parameters.Add(new KeyValuePair<string, object>(BaseOrganizeEntity.FieldDeletionStateCode, 0));
                // 获取列表，指定排序字段
                result = manager.GetDataTable(parameters, BaseOrganizeEntity.FieldSortCode);
                result.DefaultView.Sort = BaseOrganizeEntity.FieldSortCode;
                result.TableName = BaseOrganizeEntity.TableName;
            });

            return result;
        }

        /// <summary>
        /// 获得列表
        /// </summary>
        /// <param name="userInfo">用户</param>
        /// <param name="districtId">区县主键</param>
        /// <returns>数据表</returns>
        public DataTable GetDataTableByDistrictId(BaseUserInfo userInfo, string districtId)
        {
            var result = new DataTable(BaseOrganizeEntity.TableName);

            var parameter = ServiceInfo.Create(userInfo, MethodBase.GetCurrentMethod());
            ServiceUtil.ProcessUserCenterReadDb(userInfo, parameter, (dbHelper) =>
            {
                // 这里可以缓存起来，提高效率
                var manager = new BaseOrganizeManager(dbHelper, userInfo);
                // 这里是条件字段
                List<KeyValuePair<string, object>> parameters = new List<KeyValuePair<string, object>>();
                parameters.Add(new KeyValuePair<string, object>(BaseOrganizeEntity.FieldDistrictId, districtId));
                parameters.Add(new KeyValuePair<string, object>(BaseOrganizeEntity.FieldEnabled, 1));
                parameters.Add(new KeyValuePair<string, object>(BaseOrganizeEntity.FieldDeletionStateCode, 0));
                // 获取列表，指定排序字段
                result = manager.GetDataTable(parameters, BaseOrganizeEntity.FieldSortCode);
                result.DefaultView.Sort = BaseOrganizeEntity.FieldSortCode;
                result.TableName = BaseOrganizeEntity.TableName;
            });

            return result;
        }

        /// <summary>
        /// 获得列表
        /// </summary>
        /// <param name="userInfo">用户</param>
        /// <param name="streetId">街道主键</param>
        /// <returns>数据表</returns>
        public DataTable GetDataTableByStreetId(BaseUserInfo userInfo, string streetId)
        {
            var result = new DataTable(BaseOrganizeEntity.TableName);

            var parameter = ServiceInfo.Create(userInfo, MethodBase.GetCurrentMethod());
            ServiceUtil.ProcessUserCenterReadDb(userInfo, parameter, (dbHelper) =>
            {
                // 这里可以缓存起来，提高效率
                var manager = new BaseOrganizeManager(dbHelper, userInfo);
                // 这里是条件字段
                List<KeyValuePair<string, object>> parameters = new List<KeyValuePair<string, object>>();
                parameters.Add(new KeyValuePair<string, object>(BaseOrganizeEntity.FieldStreetId, streetId));
                parameters.Add(new KeyValuePair<string, object>(BaseOrganizeEntity.FieldEnabled, 1));
                parameters.Add(new KeyValuePair<string, object>(BaseOrganizeEntity.FieldDeletionStateCode, 0));
                // 获取列表，指定排序字段
                result = manager.GetDataTable(parameters, BaseOrganizeEntity.FieldSortCode);
                result.DefaultView.Sort = BaseOrganizeEntity.FieldSortCode;
                result.TableName = BaseOrganizeEntity.TableName;
            });

            return result;
        }

        #region public DataTable GetInnerOrganizeDT(BaseUserInfo userInfo, string organizeId)
        /// <summary>
        /// 获取内部组织机构
        /// </summary>
        /// <param name="userInfo">用户</param>
        /// <param name="organizeId">组织机构</param>
        /// <returns>数据表</returns>
        public DataTable GetInnerOrganizeDT(BaseUserInfo userInfo, string organizeId)
        {
			var dt = new DataTable(BaseOrganizeEntity.TableName);

            var parameter = ServiceInfo.Create(userInfo, MethodBase.GetCurrentMethod());
            ServiceUtil.ProcessUserCenterReadDb(userInfo, parameter, (dbHelper) =>
			{
				// 获得组织机构列表
				var manager = new BaseOrganizeManager(dbHelper, userInfo);
				dt = manager.GetInnerOrganize(organizeId);
				dt.DefaultView.Sort = BaseOrganizeEntity.FieldSortCode;
				dt.TableName = BaseOrganizeEntity.TableName;
			});
			return dt;
        }
        #endregion

        #region public DataTable GetCompanyDT(BaseUserInfo userInfo, string organizeId)
        /// <summary>
        /// 获取公司列表
        /// </summary>
        /// <param name="userInfo">用户</param>
        /// <param name="organizeId">组织机构主键</param>
        /// <returns>数据表</returns>
        public DataTable GetCompanyDT(BaseUserInfo userInfo, string organizeId)
        {
			var dt = new DataTable(BaseOrganizeEntity.TableName);

            var parameter = ServiceInfo.Create(userInfo, MethodBase.GetCurrentMethod());
            ServiceUtil.ProcessUserCenterReadDb(userInfo, parameter, (dbHelper) =>
			{
				// 获得组织机构列表
				var manager = new BaseOrganizeManager(dbHelper, userInfo);
				dt = manager.GetCompanyDT(organizeId);
				dt.DefaultView.Sort = BaseOrganizeEntity.FieldSortCode;
				dt.TableName = BaseOrganizeEntity.TableName;
			});
			return dt;
        }
        #endregion

        #region public DataTable GetDepartmentDT(BaseUserInfo userInfo, string organizeId)
        /// <summary>
        /// 获取部门列表
        /// </summary>
        /// <param name="userInfo">用户</param>
        /// <param name="organizeId">组织机构</param>
        /// <returns>数据表</returns>
        public DataTable GetDepartmentDT(BaseUserInfo userInfo, string organizeId)
        {
			var dt = new DataTable(BaseOrganizeEntity.TableName);

            var parameter = ServiceInfo.Create(userInfo, MethodBase.GetCurrentMethod());
            ServiceUtil.ProcessUserCenterReadDb(userInfo, parameter, (dbHelper) =>
			{
				// 获得组织机构列表
				var manager = new BaseOrganizeManager(dbHelper, userInfo);
				dt = manager.GetDepartmentDT(organizeId);
				dt.DefaultView.Sort = BaseOrganizeEntity.FieldSortCode;
				dt.TableName = BaseOrganizeEntity.TableName;
			});
			return dt;
        }
        #endregion

        #region public List<BaseOrganizeEntity> GetListByIds(BaseUserInfo userInfo, string[] ids) 获取用户列表
        /// <summary>
        /// 按主键获取列表
        /// </summary>
        /// <param name="userInfo">用户</param>
        /// <param name="ids">主键数组</param>
        /// <returns>数据表</returns>
        public List<BaseOrganizeEntity> GetListByIds(BaseUserInfo userInfo, string[] ids)
        {
            List<BaseOrganizeEntity> result = new List<BaseOrganizeEntity>();

            var parameter = ServiceInfo.Create(userInfo, MethodBase.GetCurrentMethod());
            ServiceUtil.ProcessUserCenterReadDb(userInfo, parameter, (dbHelper) =>
            {
                var userManager = new BaseOrganizeManager(dbHelper, userInfo);
                result = userManager.GetList<BaseOrganizeEntity>(BaseOrganizeEntity.FieldId, ids, BaseOrganizeEntity.FieldSortCode);
            });

            return result;
        }
        #endregion

        #region public DataTable Search(BaseUserInfo userInfo, string organizeId, string searchValue)
        /// <summary>
        /// 查询组织机构
        /// </summary>
        /// <param name="userInfo">用户</param>
        /// <param name="organizeId">组织机构</param>
        /// <param name="searchValue">查询</param>
        /// <returns>数据表</returns>
        public DataTable Search(BaseUserInfo userInfo, string organizeId, string searchValue)
        {
			var dt = new DataTable(BaseOrganizeEntity.TableName);
            
            var parameter = ServiceInfo.Create(userInfo, MethodBase.GetCurrentMethod());
            ServiceUtil.ProcessUserCenterReadDb(userInfo, parameter, (dbHelper) =>
			{
				// 获得组织机构列表
				var manager = new BaseOrganizeManager(dbHelper, userInfo);
                dt = manager.Search(searchValue, organizeId);
				dt.DefaultView.Sort = BaseOrganizeEntity.FieldSortCode;
				dt.TableName = BaseOrganizeEntity.TableName;
			});
			return dt;
        }
        #endregion

        #region public int Update(BaseUserInfo userInfo, BaseOrganizeEntity entity, out string statusCode, out string statusMessage)
        /// <summary>
        /// 更新组织机构
        /// </summary>
        /// <param name="userInfo">用户</param>
        /// <param name="entity">实体</param>
        /// <param name="statusCode">状态码</param>
        /// <param name="statusMessage">状态信息</param>
        /// <returns>影响行数</returns>
        public int Update(BaseUserInfo userInfo, BaseOrganizeEntity entity, out string statusCode, out string statusMessage)
        {
            int result = 0;

            string returnCode = string.Empty;
            string returnMessage = string.Empty;

            var parameter = ServiceInfo.Create(userInfo, MethodBase.GetCurrentMethod()); 
            ServiceUtil.ProcessUserCenterWriteDb(userInfo, parameter, (dbHelper) =>
			{
                // 2015-12-19 吉日嘎拉 网络不稳定，数据获取不完整时，异常时，会引起重大隐患
				var manager = new BaseOrganizeManager(dbHelper, userInfo);
                // result = manager.Update(entity);
                if (manager.StatusCode.Equals(Status.OKUpdate.ToString()))
				{
					// var folderManager = new BaseFolderManager(dbHelper, userInfo);
					// result = folderManager.SetProperty(entity.Id.ToString(), new KeyValuePair<string, object>(BaseFolderEntity.FieldFolderName, entity.FullName));
				}
                returnCode = manager.StatusCode;
				returnMessage = manager.StatusMessage;
            });

			statusCode = returnCode;
			statusMessage = returnMessage;

			return result;
        }
        #endregion

        #region public int Synchronous(BaseUserInfo userInfo, bool all = false)
        /// <summary>
        /// 同步数据
        /// </summary>
        /// <param name="userInfo">用户</param>
        /// <param name="all">同步所有数据</param>
        /// <returns>影响行数</returns>
        public int Synchronous(BaseUserInfo userInfo, bool all = false)
        {
            int result = 0;

            var parameter = ServiceInfo.Create(userInfo, MethodBase.GetCurrentMethod()); 
            ServiceUtil.ProcessUserCenterWriteDb(userInfo, parameter, (dbHelper) =>
            {
                var manager = new BaseOrganizeManager(dbHelper, userInfo);
                // result = manager.Synchronous(all);
            });

            return result;
        }
        #endregion

        #region public int Delete(BaseUserInfo userInfo, string id)
        /// <summary>
        /// 删除数据
        /// </summary>
        /// <param name="userInfo">用户</param>
        /// <param name="id">主键</param>
        /// <returns>影响行数</returns>
        public int Delete(BaseUserInfo userInfo, string id)
        {
			int result = 0;

            var parameter = ServiceInfo.Create(userInfo, MethodBase.GetCurrentMethod());
            ServiceUtil.ProcessUserCenterWriteDb(userInfo, parameter, (dbHelper) =>
			{
				var manager = new BaseOrganizeManager(dbHelper, userInfo);
				result = manager.Delete(id);
				// 把公司文件夹也删除了
				var folderManager = new BaseFolderManager(dbHelper, userInfo);
				result = folderManager.Delete(id);
			});
			return result;
        }
        #endregion

        #region public int BatchDelete(BaseUserInfo userInfo, string[] ids)
        /// <summary>
        /// 批量删除数据
        /// </summary>
        /// <param name="userInfo">用户</param>
        /// <param name="ids">主键数组</param>
        /// <returns>影响行数</returns>
        public int BatchDelete(BaseUserInfo userInfo, string[] ids)
        {
			int result = 0;

            var parameter = ServiceInfo.Create(userInfo, MethodBase.GetCurrentMethod());
            ServiceUtil.ProcessUserCenterWriteDb(userInfo, parameter, (dbHelper) =>
			{
				var manager = new BaseOrganizeManager(dbHelper, userInfo);
				result = manager.Delete(ids);
				// 把公司文件夹也删除了
				BaseFolderManager folderManager = new BaseFolderManager(dbHelper, userInfo);
				result = folderManager.Delete(ids);
			});
			return result;
        }
        #endregion

        #region public int SetDeleted(BaseUserInfo userInfo, string[] ids)
        /// <summary>
        /// 批量打删除标志
        /// </summary>
        /// <param name="userInfo">用户</param>
        /// <param name="ids">主键数组</param>
        /// <returns>影响行数</returns>
        public int SetDeleted(BaseUserInfo userInfo, string[] ids)
        {
			int result = 0;

            var parameter = ServiceInfo.Create(userInfo, MethodBase.GetCurrentMethod());
            ServiceUtil.ProcessUserCenterWriteDb(userInfo, parameter, (dbHelper) =>
			{
				var manager = new BaseOrganizeManager(dbHelper, userInfo);
				for(int i = 0; i < ids.Length; i++)
				{
					// 设置部门为删除状态
					result += manager.SetDeleted(ids[i]);
					// 相应的用户也需要处理
					var userManager = new BaseUserManager(dbHelper, userInfo);
					List<KeyValuePair<string, object>> parameters = new List<KeyValuePair<string, object>>();
					parameters.Add(new KeyValuePair<string, object>(BaseUserEntity.FieldCompanyId, null));
					parameters.Add(new KeyValuePair<string, object>(BaseUserEntity.FieldCompanyName, null));
					userManager.SetProperty(new KeyValuePair<string, object>(BaseUserEntity.FieldCompanyId, ids[i]), parameters);
					parameters = new List<KeyValuePair<string, object>>();
					parameters.Add(new KeyValuePair<string, object>(BaseUserEntity.FieldSubCompanyId, null));
					parameters.Add(new KeyValuePair<string, object>(BaseUserEntity.FieldSubCompanyName, null));
					userManager.SetProperty(new KeyValuePair<string, object>(BaseUserEntity.FieldSubCompanyId, ids[i]), parameters);
					parameters = new List<KeyValuePair<string, object>>();
					parameters.Add(new KeyValuePair<string, object>(BaseUserEntity.FieldDepartmentId, null));
					parameters.Add(new KeyValuePair<string, object>(BaseUserEntity.FieldDepartmentName, null));
					userManager.SetProperty(new KeyValuePair<string, object>(BaseUserEntity.FieldDepartmentId, ids[i]), parameters);
					parameters = new List<KeyValuePair<string, object>>();
					parameters.Add(new KeyValuePair<string, object>(BaseUserEntity.FieldWorkgroupId, null));
					parameters.Add(new KeyValuePair<string, object>(BaseUserEntity.FieldWorkgroupName, null));
					userManager.SetProperty(new KeyValuePair<string, object>(BaseUserEntity.FieldWorkgroupId, ids[i]), parameters);
					// 相应的员工也需要处理
					var staffManager = new BaseStaffManager(dbHelper, userInfo);
					staffManager.SetProperty(new KeyValuePair<string, object>(BaseStaffEntity.FieldCompanyId, ids[i]), new KeyValuePair<string, object>(BaseStaffEntity.FieldCompanyId, null));
					staffManager.SetProperty(new KeyValuePair<string, object>(BaseStaffEntity.FieldSubCompanyId, ids[i]), new KeyValuePair<string, object>(BaseStaffEntity.FieldSubCompanyId, null));
					staffManager.SetProperty(new KeyValuePair<string, object>(BaseStaffEntity.FieldDepartmentId, ids[i]), new KeyValuePair<string, object>(BaseStaffEntity.FieldDepartmentId, null));
					staffManager.SetProperty(new KeyValuePair<string, object>(BaseStaffEntity.FieldWorkgroupId, ids[i]), new KeyValuePair<string, object>(BaseStaffEntity.FieldWorkgroupId, null));
				}
				var folderManager = new BaseFolderManager(dbHelper, userInfo);
				folderManager.SetDeleted(ids);
			});
			return result;
        }
        #endregion

        #region public int BatchSave(BaseUserInfo userInfo, DataTable result)
        /// <summary>
        /// 批量保存数据
        /// </summary>
        /// <param name="userInfo">用户</param>
        /// <param name="result">数据表</param>
        /// <returns>影响行数</returns>
        public int BatchSave(BaseUserInfo userInfo, DataTable dt)
        {
			int result = 0;

            var parameter = ServiceInfo.Create(userInfo, MethodBase.GetCurrentMethod());
            ServiceUtil.ProcessUserCenterWriteDb(userInfo, parameter, (dbHelper) =>
			{
				var manager = new BaseOrganizeManager(dbHelper, userInfo);
				result = manager.BatchSave(dt);
			});
			return result;
        }
        #endregion

        #region public int MoveTo(BaseUserInfo userInfo, string id, string parentId)
        /// <summary>
        /// 移动数据
        /// </summary>
        /// <param name="userInfo">用户</param>
        /// <param name="id">主键</param>
        /// <param name="parentId">父主键</param>
        /// <returns>影响行数</returns>
        public int MoveTo(BaseUserInfo userInfo, string id, string parentId)
        {
			int result = 0;

            var parameter = ServiceInfo.Create(userInfo, MethodBase.GetCurrentMethod());
            ServiceUtil.ProcessUserCenterWriteDb(userInfo, parameter, (dbHelper) =>
			{
				var manager = new BaseOrganizeManager(dbHelper, userInfo);
				result = manager.MoveTo(id, parentId);
			});
			return result;
        }
        #endregion

        #region public int BatchMoveTo(BaseUserInfo userInfo, string[] ids, string parentId)
        /// <summary>
        /// 批量移动数据
        /// </summary>
        /// <param name="userInfo">用户</param>
        /// <param name="ids">主键数组</param>
        /// <param name="parentId">父节点主键</param>
        /// <returns>影响行数</returns>
        public int BatchMoveTo(BaseUserInfo userInfo, string[] organizeIds, string parentId)
        {
			int result = 0;

            var parameter = ServiceInfo.Create(userInfo, MethodBase.GetCurrentMethod());
            ServiceUtil.ProcessUserCenterWriteDb(userInfo, parameter, (dbHelper) =>
			{
				var manager = new BaseOrganizeManager(dbHelper, userInfo);
				for(int i = 0; i < organizeIds.Length; i++)
				{
					result += manager.MoveTo(organizeIds[i], parentId);
				}
			});
			return result;
        }
        #endregion

        #region public int BatchSetCode(BaseUserInfo userInfo, string[] ids, string[] codes)
        /// <summary>
        /// 批量重新生成编号
        /// </summary>
        /// <param name="userInfo">用户</param>
        /// <param name="ids">主键</param>
        /// <param name="codes">编号</param>
        /// <returns>影响行数</returns>
        public int BatchSetCode(BaseUserInfo userInfo, string[] ids, string[] codes)
        {
			int result = 0;

            var parameter = ServiceInfo.Create(userInfo, MethodBase.GetCurrentMethod());
            ServiceUtil.ProcessUserCenterWriteDb(userInfo, parameter, (dbHelper) =>
			{
				var manager = new BaseOrganizeManager(dbHelper, userInfo);
				result = manager.BatchSetCode(ids, codes);
			});
			return result;
        }
        #endregion

        #region public int BatchSetSortCode(BaseUserInfo userInfo, string[] ids)
        /// <summary>
        /// 批量重新生成排序码
        /// </summary>
        /// <param name="userInfo">用户</param>
        /// <param name="ids">主键数组</param>
        /// <returns>影响行数</returns>
        public int BatchSetSortCode(BaseUserInfo userInfo, string[] ids)
        {
			int result = 0;

            var parameter = ServiceInfo.Create(userInfo, MethodBase.GetCurrentMethod());
            ServiceUtil.ProcessUserCenterWriteDb(userInfo, parameter, (dbHelper) =>
			{
				var manager = new BaseOrganizeManager(dbHelper, userInfo);
				result = manager.BatchSetSortCode(ids);
			});
			return result;
        }
        #endregion

        #region public DataTable GetDataTableByPage(BaseUserInfo userInfo, out int recordCount, int pageIndex, int pageSize, string whereClause, List<KeyValuePair<string, object>> dbParameters, string order = null)
        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="userInfo">用户</param>
        /// <param name="recordCount">记录数</param>
        /// <param name="pageIndex">当前页</param>
        /// <param name="pageSize">每页显示</param>
        /// <param name="whereClause">条件</param>
        /// <param name="dbParameters">参数</param>
        /// <param name="order">排序</param>
        /// <returns>数据表</returns>
        public DataTable GetDataTableByPage(BaseUserInfo userInfo, out int recordCount, int pageIndex, int pageSize, string whereClause, List<KeyValuePair<string, object>> dbParameters, string order = null)
        {
            int myRecordCount = 0;
            var dt = new DataTable(BaseOrganizeEntity.TableName);

            var parameter = ServiceInfo.Create(userInfo, MethodBase.GetCurrentMethod());
            ServiceUtil.ProcessUserCenterReadDb(userInfo, parameter, (dbHelper) =>
            {
                if (SecretUtil.IsSqlSafe(whereClause))
                {
                    var organizeManager = new BaseOrganizeManager(dbHelper, userInfo);
                    dt = organizeManager.GetDataTableByPage(out myRecordCount, pageIndex, pageSize, whereClause, dbHelper.MakeParameters(dbParameters), order);
                    dt.TableName = BaseOrganizeEntity.TableName;
                }
                else
                {
                    if (System.Web.HttpContext.Current != null)
                    {
                        // 记录注入日志
                        FileUtil.WriteMessage("userInfo:" + userInfo.Serialize() + " " + whereClause, System.Web.HttpContext.Current.Server.MapPath("~/Log/") + "SqlSafe" + DateTime.Now.ToString(BaseSystemInfo.DateFormat) + ".txt");
                    }
                }
            });
            recordCount = myRecordCount;
            return dt;
        }
        #endregion

        /// <summary>
        /// 刷新列表
        /// 2015-12-11 吉日嘎拉 刷新缓存功能优化
        /// </summary>
        /// <param name="userInfo">用户</param>
        /// <returns>数据表</returns>
        public void CachePreheating(BaseUserInfo userInfo)
        {
            BaseOrganizeManager.CachePreheating();
        }
    }
}