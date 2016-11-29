﻿//-----------------------------------------------------------------
// All Rights Reserved , Copyright (C) 2016 , Hairihan TECH, Ltd.  
//-----------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;

namespace DotNet.Business
{
    using DotNet.Model;
    using DotNet.Utilities;

    /// <summary>
    /// BaseAreaProvinceMarkManager 
    /// 地区表(省、市、县)
    ///
    /// 修改记录
    ///
    ///		2015.07.03 版本：1.0 JiRiGaLa  修改记录独立化。
    ///
    /// </summary>
    /// <author>
    ///		<name>JiRiGaLa</name>
    ///		<date>2015.07.03</date>
    /// </author>
    /// </summary>
    public partial class BaseAreaProvinceMarkManager : BaseManager
    {
        public int Update(BaseAreaProvinceMarkEntity entity)
        {
            // 获取原始实体信息
            var entityOld = this.GetObject(entity.Id);
            // 保存修改记录
            this.UpdateEntityLog(entity, entityOld);

            return this.UpdateObject(entity);
        }

        #region public void UpdateEntityLog(BaseAreaProvinceMarkEntity newEntity, BaseAreaProvinceMarkEntity oldEntity)
        /// <summary>
        /// 实体修改记录
        /// </summary>
        /// <param name="newEntity">修改前的实体对象</param>
        /// <param name="oldEntity">修改后的实体对象</param>
        /// <param name="tableName">表名称</param>
        public void UpdateEntityLog(BaseAreaProvinceMarkEntity newEntity, BaseAreaProvinceMarkEntity oldEntity, string tableName = null)
        {
            if (string.IsNullOrEmpty(tableName))
            {
                tableName = this.CurrentTableName + "_LOG";
            }
            BaseModifyRecordManager manager = new BaseModifyRecordManager(this.UserInfo, tableName);
            foreach (var property in typeof(BaseAreaProvinceMarkEntity).GetProperties())
            {
                var oldValue = Convert.ToString(property.GetValue(oldEntity, null));
                var newValue = Convert.ToString(property.GetValue(newEntity, null));
                //不记录创建人、修改人、没有修改的记录
                var fieldDescription = property.GetCustomAttributes(typeof(FieldDescription), false).FirstOrDefault() as FieldDescription;
                if (!fieldDescription.NeedLog || oldValue == newValue)
                {
                    continue;
                }
                var record = new BaseModifyRecordEntity();
                record.ColumnCode = property.Name.ToUpper();
                record.ColumnDescription = fieldDescription.Text;
                record.NewValue = newValue;
                record.OldValue = oldValue;
                record.TableCode = this.CurrentTableName.ToUpper();
                record.TableDescription = FieldExtensions.ToDescription(typeof(BaseAreaProvinceMarkEntity), "TableName");
                record.RecordKey = oldEntity.Id.ToString();
                record.IPAddress = Utilities.GetIPAddress(true);
                manager.Add(record, true, false);
            }
        }
        #endregion
    }
}