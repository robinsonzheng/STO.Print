﻿//-----------------------------------------------------------------
// All Rights Reserved , Copyright (C) 2015 , Hairihan TECH, Ltd. 
//-----------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Caching;

namespace DotNet.Business
{
    using DotNet.Utilities;
    using DotNet.Model;

    /// <remarks>
    /// BaseStaffManager
    /// 职员管理
    /// 
    /// 修改纪录
    /// 
    ///	版本：1.0 2012.12.17    JiRiGaLa    选项管理从缓存读取，通过编号显示名称的函数完善。
    ///	
    /// <author>  
    ///		<name>JiRiGaLa</name>
    ///		<date>2012.12.17</date>
    /// </author> 
    /// </remarks>
    public partial class BaseStaffManager
    {
        // 当前的锁
        private static object locker = new Object();

        #region public static void ClearCache() 清除缓存
        /// <summary>
        /// 清除缓存
        /// </summary>
        public static void ClearCache()
        {
            lock (BaseSystemInfo.UserLock)
            {
                if (HttpContext.Current.Cache[BaseStaffEntity.TableName] != null)
                {
                    HttpContext.Current.Cache.Remove(BaseStaffEntity.TableName);
                }
            }
        }
        #endregion

        #region public static List<BaseStaffEntity> GetEntities() 获取职员表，从缓存读取
        /// <summary>
        /// 获取职员表，从缓存读取
        /// </summary>
        public static List<BaseStaffEntity> GetEntities()
        {
            if (HttpContext.Current.Session == null || HttpContext.Current.Cache[BaseStaffEntity.TableName] == null)
            {
                lock (BaseSystemInfo.UserLock)
                {
                    if (HttpContext.Current.Session == null || HttpContext.Current.Cache[BaseStaffEntity.TableName] == null)
                    {
                        // 读取目标表中的数据
                        List<BaseStaffEntity> entityList = null;
                        BaseStaffManager manager = new DotNet.Business.BaseStaffManager(BaseStaffEntity.TableName);
                        entityList = manager.GetList<BaseStaffEntity>();
                        // 这个是没写过期时间的方法
                        // HttpContext.Current.Cache[tableName] = entityList;
                        // 设置过期时间为8个小时，第2天若有不正常的自动就可以正常了
                        HttpContext.Current.Cache.Add(BaseStaffEntity.TableName, entityList, null, DateTime.Now.AddMinutes(10), TimeSpan.Zero, CacheItemPriority.Normal, null);
                    }
                }
            }
            return HttpContext.Current.Cache[BaseStaffEntity.TableName] as List<BaseStaffEntity>;
        }
        #endregion

        #region public static string GetRealName(string id) 通过编号获取选项的显示内容
        /// <summary>
        /// 通过编号获取选项的显示内容
        /// 这里是进行了内存缓存处理，减少数据库的I/O处理，提高程序的运行性能，
        /// 若有数据修改过，重新启动一下程序就可以了，这些基础数据也不是天天修改来修改去的，
        /// 所以没必要过度担忧，当然有需要时也可以写个刷新缓存的程序
        /// </summary>
        /// <param name="id">主键</param>
        /// <returns>显示值</returns>
        public static string GetRealName(string id)
        {
            string result = id;
            if (!string.IsNullOrEmpty(id))
            {
                List<BaseStaffEntity> entityList = GetEntities();
                BaseStaffEntity staffEntity = entityList.FirstOrDefault(entity => entity.Id.HasValue && entity.Id.ToString().Equals(id));
                if (staffEntity != null)
                {
                    result = staffEntity.RealName;
                }
            }
            return result;
        }
        #endregion

        public static BaseStaffEntity GetObjectByCodeByCache(string code)
        {
            BaseStaffEntity result = null;
            System.Web.Caching.Cache cache = HttpRuntime.Cache;
            string cacheObject = "StaffByCode" + code;
            if (cache != null && cache[cacheObject] == null)
            {
                lock (locker)
                {
                    if (cache != null && cache[cacheObject] == null)
                    {
                        BaseStaffManager staffManager = new BaseStaffManager();
                        result = staffManager.GetObjectByCode(code);
                        cache.Add(cacheObject, result, null, DateTime.Now.AddMinutes(10), TimeSpan.Zero, CacheItemPriority.Normal, null);
                        System.Console.WriteLine(System.DateTime.Now.ToString(BaseSystemInfo.DateTimeFormat) + " cache Staff");
                    }
                }
            }
            result = cache[cacheObject] as BaseStaffEntity;
            return result;
        }
    }
}