﻿using FineUIPro;
using Infobasis.Data.DataEntity;
using Infobasis.Web.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Infobasis.Web.Pages.Admin
{
    public partial class UserRole_AddNew : PageBase
    {
        #region Page_Load

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadData();
            }
        }

        private void LoadData()
        {
            btnClose.OnClientClick = ActiveWindow.GetHideReference();

            int id = GetQueryIntValue("id");
            PermissionRole current = DB.PermissionRoles.Find(id);
            if (current == null)
            {
                // 参数错误，首先弹出Alert对话框然后关闭弹出窗口
                Alert.Show("参数错误！", String.Empty, ActiveWindow.GetHideReference());
                return;
            }

            // 每页记录数
            Grid1.PageSize = UserInfo.Current.DefaultPageSize;
            ddlGridPageSize.SelectedValue = UserInfo.Current.DefaultPageSize.ToString();


            BindGrid();
        }


        private void BindGrid()
        {
            IQueryable<Infobasis.Data.DataEntity.User> q = DB.Users;

            // 在名称中搜索
            string searchText = ttbSearchMessage.Text.Trim();
            if (!String.IsNullOrEmpty(searchText))
            {
                q = q.Where(u => u.Name.Contains(searchText) || u.ChineseName.Contains(searchText) || u.EnglishName.Contains(searchText));
            }

            // 排除已经属于本角色的用户
            int roleID = GetQueryIntValue("id");
            q = q.Where(u => u.UserPermissionRoles.All(r => r.PermissionRoleID != roleID));

            // 在查询添加之后，排序和分页之前获取总记录数
            Grid1.RecordCount = q.Count();

            // 排列和分页
            q = SortAndPage<Infobasis.Data.DataEntity.User>(q, Grid1);

            Grid1.DataSource = q;
            Grid1.DataBind();
        }

        #endregion

        #region Events

        protected void btnSaveClose_Click(object sender, EventArgs e)
        {
            int roleID = GetQueryIntValue("id");

            int[] ids = DropDownBox1.Values.Select(r => Convert.ToInt32(r)).ToArray();

            foreach (int userID in ids)
            {
                DB.UserPermissionRoles.Add(new UserPermissionRole()
                {
                    UserID = userID,
                    PermissionRoleID = roleID,
                    CreateDatetime = DateTime.Now
                });
            }
            DB.SaveChanges();

            PageContext.RegisterStartupScript(ActiveWindow.GetHidePostBackReference());
        }


        protected void ttbSearchMessage_Trigger2Click(object sender, EventArgs e)
        {
            ttbSearchMessage.ShowTrigger1 = true;
            BindGrid();
        }

        protected void ttbSearchMessage_Trigger1Click(object sender, EventArgs e)
        {
            ttbSearchMessage.Text = String.Empty;
            ttbSearchMessage.ShowTrigger1 = false;
            BindGrid();
        }

        protected void Grid1_Sort(object sender, GridSortEventArgs e)
        {
            Grid1.SortDirection = e.SortDirection;
            Grid1.SortField = e.SortField;
            BindGrid();
        }

        protected void Grid1_PageIndexChange(object sender, GridPageEventArgs e)
        {
            Grid1.PageIndex = e.NewPageIndex;
            BindGrid();
        }


        protected void ddlGridPageSize_SelectedIndexChanged(object sender, EventArgs e)
        {
            Grid1.PageSize = Convert.ToInt32(ddlGridPageSize.SelectedValue);

            BindGrid();
        }

        #endregion

    }
}