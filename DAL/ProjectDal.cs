using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Models;
using System.Data;
using System.Data.OleDb;

namespace DAL
{
    public class ProjectDal : BaseDal<Project>
    {
        public ProjectDal()
            : base()
        {
            this.SelectSql = " select * from Project where 1=1";
            this.CreateSql = " insert into Project values (@ProjectId,@ProjectName,@ProjectLevel,@DisplayOrder,@ParentProjectId) ";
            this.EditSql = " update Project set ProjectName=@ProjectName,ProjectLevel=@ProjectLevel,DisplayOrder=@DisplayOrder, ParentProjectId=@ParentProjectId where ProjectId=@ProjectId ";
            this.DeleteSql = "Delete from Project where 1=1 ";
        }
        public override Project GetModel(string id)
        {
            DataTable table = new DataTable();
            string sql = SelectSql + " and ProjectId=@ProjectId ";
            table = db.Query(sql, new OleDbParameter("@ProjectId", id)).Tables[0];
            if (table.Rows.Count > 0)
            {
                Project project = GetModelByRow(table.Rows[0]);
                if (!Utils.IsEmptyOrNull(project.ParentProjectId)) {
                    project.ParentProject = this.GetModel(project.ParentProjectId);
                }

                return project;
            }
            else
            {
                return null;
            }
        }

        public override int Create(Project t)
        {
            OleDbParameter[] para = { 
                new OleDbParameter("@ProjectId",t.ProjectId),
                new OleDbParameter("@ProjectName",t.ProjectName),
                new OleDbParameter("@ProjectLevel",t.ProjectLevel),
                new OleDbParameter("@DisplayOrder",t.DisplayOrder),
                new OleDbParameter("@ParentProjectId",t.ParentProjectId)
                                    };
            int n = db.ExecuteSql(CreateSql, para);
            return n;
        }

        public override int Edit(Project t)
        {
            OleDbParameter[] para = { 
                new OleDbParameter("@ProjectName",t.ProjectName),
                new OleDbParameter("@ProjectLevel",t.ProjectLevel),
                new OleDbParameter("@DisplayOrder",t.DisplayOrder),
                new OleDbParameter("@ParentProjectId",t.ParentProjectId),
                new OleDbParameter("@ProjectId",t.ProjectId)
                                    };
            int n = db.ExecuteSql(EditSql, para);
            return n;
        }

        public override int Delete(Project t)
        {
            string sql = DeleteSql + " and ProjectId=@ProjectId ";
            int n = db.ExecuteSql(sql, new OleDbParameter("@ProjectId", t.ProjectId));
            return n;
        }

        public override Project GetModelByRow(DataRow row)
        {
            try
            {
                Project project = new Project()
                {
                    DisplayOrder = int.Parse(row["DisplayOrder"].ToString()),
                    ParentProjectId = row["ParentProjectId"].ToString(),
                    ProjectId = row["ProjectId"].ToString(),
                    ProjectName = row["ProjectName"].ToString(),
                    ProjectLevel = int.Parse(row["ProjectLevel"].ToString())
                };

                return project;
            }
            catch {
                return null;
            }
        }
    }
}
