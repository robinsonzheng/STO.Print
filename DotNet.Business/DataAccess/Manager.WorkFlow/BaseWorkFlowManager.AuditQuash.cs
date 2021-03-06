﻿//-----------------------------------------------------------------
// All Rights Reserved , Copyright (C) 2016 , Hairihan TECH, Ltd.  
//-----------------------------------------------------------------

using System;

namespace DotNet.Business
{
    using DotNet.IService;
    using DotNet.Model;
    using DotNet.Utilities;

    /// <summary>
    /// BaseWorkFlowCurrentManager
    /// 流程管理.
    /// 
    /// 修改记录
    ///		
    ///		2012.04.04 版本：1.0 JiRiGaLa	脱离。
    /// 
    /// <author>
    ///		<name>JiRiGaLa</name>
    ///		<date>2012.04.04</date>
    /// </author>
    /// </summary>
    public partial class BaseWorkFlowCurrentManager : BaseManager, IBaseManager
    {
        public int AuditQuash(IWorkFlowManager workFlowManager, string categoryCode, string objectId, string auditIdea)
        {
            string id = GetCurrentId(categoryCode, objectId);
            return this.AuditQuash(workFlowManager, id, auditIdea);
        }

        #region public int AuditQuash(string categoryCode, string[] objectIds, string auditIdea)
        /// <summary>
        /// 撤消审批流程中的单据(撤销的没必要发送提醒信息)
        /// </summary>
        /// <param name="categoryCode">分类编号</param>
        /// <param name="objectId">实体主键</param>
        /// <param name="auditIdea">批示</param>
        /// <returns>影响行数</returns>
        public int AuditQuash(IWorkFlowManager workFlowManager, string categoryCode, string[] objectIds, string auditIdea)
        {
            int result = 0;
            string id = string.Empty;
            for (int i = 0; i < objectIds.Length; i++)
            {
                id = GetCurrentId(categoryCode, objectIds[i]);
                result += this.AuditQuash(workFlowManager, id, auditIdea);
            }
            return result;
        }
        #endregion

        #region public int AuditQuash(string categoryCode, string objectId, string auditIdea)
        /// <summary>
        /// 撤消审批流程中的单据(撤销的没必要发送提醒信息)
        /// </summary>
        /// <param name="categoryCode">分类编号</param>
        /// <param name="objectId">实体主键</param>
        /// <param name="auditIdea">批示</param>
        /// <returns>影响行数</returns>
        public int AuditQuash(string categoryCode, string objectId, string auditIdea)
        {
            string id = GetCurrentId(categoryCode, objectId);
            // 退回时，应该给创建者一个提醒消息
            return this.AuditQuash(id, auditIdea);
        }
        #endregion

        public int AuditQuash(string currentId, string auditIdea)
        {
            IWorkFlowManager workFlowManager = this.GetWorkFlowManager(currentId);
            workFlowManager.SetUserInfo(this.UserInfo);
            return AuditQuash(workFlowManager, currentId, auditIdea);
        }

        public int AuditQuash(string[] currentIds, string auditIdea)
        {
            int result = 0;
            for (int i = 0; i < currentIds.Length; i++)
            {
                result += AuditQuash(currentIds[i], auditIdea);
            }
            return result;
        }

        #region public int AuditQuash(string currentId, string auditIdea)
        /// <summary>
        /// 撤消审批流程中的单据(撤销的没必要发送提醒信息)
        /// </summary>
        /// <param name="currentId">当前主键</param>
        /// <param name="auditIdea">批示</param>
        /// <returns>影响行数</returns>
        public int AuditQuash(IWorkFlowManager workFlowManager, string currentId, string auditIdea)
        {
            int result = 0;
            if (!string.IsNullOrEmpty(currentId))
            {
                // using (TransactionScope transactionScope = new TransactionScope())
                // {
                try
                {
                    BaseWorkFlowCurrentEntity workFlowCurrentEntity = this.GetObject(currentId);
                    // 只有待审核状态的，才可以撤销，被退回的也可以废弃
                    if (workFlowCurrentEntity.AuditStatus.Equals(AuditStatus.AuditComplete.ToString())
                        || workFlowCurrentEntity.AuditStatus.Equals(AuditStatus.AuditQuash.ToString()))
                    {
                        return result;
                    }
                    // 是超级管理员，或者创建者自己才可以撤销
                    if (!UserInfo.IsAdministrator && !workFlowCurrentEntity.CreateUserId.Equals(UserInfo.Id))
                    {
                        return result;
                    }
                    // 是自己的的单子需要被审核
                    // 最后修改人是自己，表明是自己发出的
                    // if ((workFlowCurrentEntity.ModifiedUserId == UserInfo.Id) || (workFlowCurrentEntity.CreateUserId == UserInfo.Id))
                    //{
                    // 进行更新操作
                    workFlowCurrentEntity = this.StepAuditQuash(currentId, auditIdea);
                    if (!string.IsNullOrEmpty(workFlowCurrentEntity.Id))
                    {
                        // 发送提醒信息
                        if (workFlowManager != null)
                        {
                            if (workFlowManager.OnAuditQuash(workFlowCurrentEntity))
                            {
                                result++;
                                workFlowManager.SendRemindMessage(workFlowCurrentEntity, AuditStatus.AuditQuash, new string[] { workFlowCurrentEntity.CreateUserId }, null, null);
                            }
                        }
                    }
                    else
                    {
                        // 数据可能被删除
                        this.StatusCode = Status.ErrorDeleted.ToString();
                    }
                    // }
                    this.StatusMessage = this.GetStateMessage(this.StatusCode);
                }
                catch (System.Exception ex)
                {
                    // 在本地记录异常
                    FileUtil.WriteException(UserInfo, ex);
                }
                finally
                {
                }
                // transactionScope.Complete();
                // }
            }
            return result;
        }
        #endregion

        #region private BaseWorkFlowCurrentEntity StepAuditQuash(string currentId, string auditIdea)
        /// <summary>
        /// 撤消审批流程中的单据
        /// </summary>
        /// <param name="id">当前主键</param>
        /// <returns>影响行数</returns>
        private BaseWorkFlowCurrentEntity StepAuditQuash(string currentId, string auditIdea)
        {
            BaseWorkFlowCurrentEntity workFlowCurrentEntity = this.GetObject(currentId);
            // 1.记录审核时间、审核人
            workFlowCurrentEntity.ToUserId = this.UserInfo.Id;
            workFlowCurrentEntity.ToUserRealName = this.UserInfo.RealName;
            workFlowCurrentEntity.AuditDate = DateTime.Now;
            workFlowCurrentEntity.AuditIdea = auditIdea;
            workFlowCurrentEntity.AuditStatus = AuditStatus.AuditQuash.ToString();
            workFlowCurrentEntity.AuditStatusName = AuditStatus.AuditQuash.ToDescription();
            workFlowCurrentEntity.ActivityId = null;
            workFlowCurrentEntity.Enabled = 1;
            workFlowCurrentEntity.DeletionStateCode = 0;
            // 4.生成审核结束的记录
            this.AddHistory(workFlowCurrentEntity);
            // 废弃的,就变成删除状态了
            this.UpdateObject(workFlowCurrentEntity);
            // 5.把历史都设置为删除标志
            return workFlowCurrentEntity;
        }
        #endregion
    }
}