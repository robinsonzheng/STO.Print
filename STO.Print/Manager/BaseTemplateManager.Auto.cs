﻿//-----------------------------------------------------------------
// All Rights Reserved , Copyright (C) 2015 , STO TECH, Ltd. 
//-----------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;

namespace STO.Print.Manager
{
    using DotNet.Business;
    using DotNet.Model;
    using DotNet.Utilities;
    using STO.Print.Model;

    /// <summary>
    /// BaseTemplateManager
    /// 
    /// 
    /// 修改纪录
    /// 
    /// 2015-08-24 版本：1.0 YangHengLian 创建文件。
    /// 
    /// <author>
    ///     <name>YangHengLian</name>
    ///     <date>2015-08-24</date>
    /// </author>
    /// </summary>
    public partial class BaseTemplateManager : BaseManager, IBaseManager
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public BaseTemplateManager()
        {
            if (base.dbHelper == null)
            {
                base.dbHelper = DbHelperFactory.GetHelper(BaseSystemInfo.BusinessDbType, BaseSystemInfo.BusinessDbConnection);
            }
            if (string.IsNullOrEmpty(base.CurrentTableName))
            {
                base.CurrentTableName = BaseTemplateEntity.TableName;
            }
            base.PrimaryKey = "Id";
        }

        /// <summary>
        /// 构造函数
        /// <param name="tableName">指定表名</param>
        /// </summary>
        public BaseTemplateManager(string tableName)
        {
            base.CurrentTableName = tableName;
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="dbHelper">数据库连接</param>
        public BaseTemplateManager(IDbHelper dbHelper): this()
        {
            DbHelper = dbHelper;
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="userInfo">用户信息</param>
        public BaseTemplateManager(BaseUserInfo userInfo) : this()
        {
            UserInfo = userInfo;
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="userInfo">用户信息</param>
        /// <param name="tableName">指定表名</param>
        public BaseTemplateManager(BaseUserInfo userInfo, string tableName) : this(userInfo)
        {
            base.CurrentTableName = tableName;
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="dbHelper">数据库连接</param>
        /// <param name="userInfo">用户信息</param>
        public BaseTemplateManager(IDbHelper dbHelper, BaseUserInfo userInfo) : this(dbHelper)
        {
            UserInfo = userInfo;
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="dbHelper">数据库连接</param>
        /// <param name="userInfo">用户信息</param>
        /// <param name="tableName">指定表名</param>
        public BaseTemplateManager(IDbHelper dbHelper, BaseUserInfo userInfo, string tableName) : this(dbHelper, userInfo)
        {
            base.CurrentTableName = tableName;
        }

        /// <summary>
        /// 添加, 这里可以人工干预，提高程序的性能
        /// </summary>
        /// <param name="entity">实体</param>
        /// <param name="identity">自增量方式，表主键是否采用自增的策略</param>
        /// <param name="returnId">返回主键，不返回程序允许速度会快，主要是为了主细表批量插入数据优化用的</param>
        /// <returns>主键</returns>
        public string Add(BaseTemplateEntity entity, bool identity = false, bool returnId = false)
        {
            this.Identity = identity;
            this.ReturnId = returnId;
            return this.AddObject(entity);
        }

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="entity">实体</param>
        public int Update(BaseTemplateEntity entity)
        {
            return this.UpdateObject(entity);
        }

        /// <summary>
        /// 获取实体
        /// </summary>
        /// <param name="id">主键</param>
        public BaseTemplateEntity GetObject(string id)
        {
            return BaseEntity.Create<BaseTemplateEntity>(this.ExecuteReader(new KeyValuePair<string, object>(PrimaryKey, id)));
        }

        /// <summary>
        /// 添加实体
        /// </summary>
        /// <param name="entity">实体</param>
        public string AddObject(BaseTemplateEntity entity)
        {
            string key = string.Empty;
            if (entity.Id != null)
            {
                key = entity.Id.ToString();
            }
            SQLBuilder sqlBuilder = new SQLBuilder(DbHelper, this.Identity, this.ReturnId);
            sqlBuilder.BeginInsert(this.CurrentTableName, this.PrimaryKey);
            if (!this.Identity) 
            {
                // 这里已经是指定了主键了，所以不需要返回主键了
                sqlBuilder.ReturnId = false;
                sqlBuilder.SetValue(this.PrimaryKey, entity.Id);
            }
            else
            {
                if (!this.ReturnId && (DbHelper.CurrentDbType == CurrentDbType.Oracle || DbHelper.CurrentDbType == CurrentDbType.DB2))
                {
                    if (DbHelper.CurrentDbType == CurrentDbType.Oracle)
                    {
                        sqlBuilder.SetFormula(this.PrimaryKey, "SEQ_" + this.CurrentTableName.ToUpper() + ".NEXTVAL ");
                    }
                    if (DbHelper.CurrentDbType == CurrentDbType.DB2)
                    {
                        sqlBuilder.SetFormula(this.PrimaryKey, "NEXT VALUE FOR SEQ_" + this.CurrentTableName.ToUpper());
                    }
                }
                else
                {
                    if (this.Identity && (DbHelper.CurrentDbType == CurrentDbType.Oracle || DbHelper.CurrentDbType == CurrentDbType.DB2))
                    {
                        BaseSequenceManager sequenceManager = new BaseSequenceManager(DbHelper);
                        //entity.Id = sequenceManager.Increment(this.CurrentTableName);
                        sqlBuilder.SetValue(this.PrimaryKey, entity.Id);
                    }
                }
            }
            this.SetObject(sqlBuilder, entity);
            sqlBuilder.SetDBNow(BaseTemplateEntity.FieldCreateOn);
            sqlBuilder.SetDBNow(BaseTemplateEntity.FieldModifiedOn);
            if (this.Identity && (DbHelper.CurrentDbType == CurrentDbType.SqlServer || DbHelper.CurrentDbType == CurrentDbType.Access))
            {
                key = sqlBuilder.EndInsert().ToString();
            }
            else
            {
                sqlBuilder.EndInsert();
            }
            if (this.Identity && (DbHelper.CurrentDbType == CurrentDbType.Oracle || DbHelper.CurrentDbType == CurrentDbType.DB2))
            {
                return entity.Id.ToString();
            }
            return key;
        }

        /// <summary>
        /// 更新实体
        /// </summary>
        /// <param name="entity">实体</param>
        public int UpdateObject(BaseTemplateEntity entity)
        {
            SQLBuilder sqlBuilder = new SQLBuilder(DbHelper);
            sqlBuilder.BeginUpdate(this.CurrentTableName);
            this.SetObject(sqlBuilder, entity);
            sqlBuilder.SetDBNow(BaseTemplateEntity.FieldModifiedOn);
            sqlBuilder.SetWhere(this.PrimaryKey, entity.Id);
            return sqlBuilder.EndUpdate();
        }

        // 这个是声明扩展方法
        partial void SetObjectExpand(SQLBuilder sqlBuilder, BaseTemplateEntity entity);
        
        /// <summary>
        /// 设置实体
        /// </summary>
        /// <param name="entity">实体</param>
        private void SetObject(SQLBuilder sqlBuilder, BaseTemplateEntity entity)
        {
            SetObjectExpand(sqlBuilder, entity);
            sqlBuilder.SetValue(BaseTemplateEntity.FieldExpressId, entity.ExpressId);
            sqlBuilder.SetValue(BaseTemplateEntity.FieldCreateUserName, entity.CreateUserName);
            sqlBuilder.SetValue(BaseTemplateEntity.FieldModifiedUserName, entity.ModifiedUserName);
            sqlBuilder.SetValue(BaseTemplateEntity.FieldLength, entity.Length);
            sqlBuilder.SetValue(BaseTemplateEntity.FieldRemark, entity.Remark);
            sqlBuilder.SetValue(BaseTemplateEntity.FieldName, entity.Name);
            sqlBuilder.SetValue(BaseTemplateEntity.FieldBackgroundImagePath, entity.BackgroundImagePath);
            sqlBuilder.SetValue(BaseTemplateEntity.FieldWidth, entity.Width);
            sqlBuilder.SetValue(BaseTemplateEntity.FieldLayer, entity.Layer);
            sqlBuilder.SetValue(BaseTemplateEntity.FieldFilePath, entity.FilePath);
        }

        /// <summary>
        /// 删除实体
        /// </summary>
        /// <param name="id">主键</param>
        /// <returns>影响行数</returns>
        public int Delete(string id)
        {
            return this.Delete(new KeyValuePair<string, object>(this.PrimaryKey, id));
        }
        /// <summary>
        /// 更新修改记录
        /// </summary>
        /// <param name="oldShow"> </param>
        /// <param name="newShow"> </param>

        public void UpdateModifyRecord(BaseTemplateEntity oldShow, BaseTemplateEntity newShow,string tableName = null)
        {
            if (string.IsNullOrEmpty(tableName))
            {
                tableName = this.CurrentTableName + "_LOG";
            }
            BaseModifyRecordManager manager =new BaseModifyRecordManager(DbHelper,this.UserInfo, tableName);
            foreach (var property in typeof(BaseTemplateEntity).GetProperties())
            {
                  var fieldDescription = property.GetCustomAttributes(typeof(FieldDescription), false).FirstOrDefault() as FieldDescription;
                  var oldValue = Convert.ToString(property.GetValue(oldShow, null));
                  var newValue = Convert.ToString(property.GetValue(newShow, null));

                  if (!fieldDescription.NeedLog || oldValue == newValue)
                  {
                       continue;
                  }
                   var record = new BaseModifyRecordEntity();
                  record.ColumnCode = property.Name.ToUpper();
                  record.ColumnDescription = fieldDescription.Text;
                  record.NewValue = newValue;
                  record.OldValue = oldValue;
                  record.TableCode = BaseTemplateEntity.TableName.ToUpper();
                  record.TableDescription = FieldExtensions.ToDescription(typeof(BaseTemplateEntity), "TableName");
                  record.RecordKey = oldShow.Id.ToString();
                  record.IPAddress = DotNet.Business.Utilities.GetIPAddress(true);
                  record.CreateBy = UserInfo.RealName;
                  record.CreateOn = DateTime.Now;
                  manager.Add(record, true, false);
              }
        }
    }
}
