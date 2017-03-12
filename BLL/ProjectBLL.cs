using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace BLL
{
    public class ProjectBLL : BaseBll<Models.Project>
    {
        public ProjectBLL()
        {
            this.Context = new DAL.ProjectDal();
        }
        public override List<ValidataError> Validata(Models.Project Model)
        {
            List<ValidataError> list = new List<ValidataError>();
            if (Utils.IsEmptyOrNull(Model.ProjectId))
            {
                list.Add(new ValidataError() { ProprtyName = "ProjectId", ErrorMessage = "项目Id不可为空" });
            }
            if (Utils.IsEmptyOrNull(Model.ProjectName))
            {
                list.Add(new ValidataError() { ProprtyName = "ProjectName", ErrorMessage = "项目名称不可为空" });
            }
            if (Model.ProjectLevel < 0 || Model.ProjectLevel > 3)
            {
                list.Add(new ValidataError() { ProprtyName = "ProjectLevel", ErrorMessage = "项目项目级别应在1-3之间" });
            }
            return list;
        }
        public DataTable GetProjectByParentProjectId(string ParentProjectId)
        {
            DataTable table = Context.GetData("ParentProjectId=@ParentProjectId", new System.Data.OleDb.OleDbParameter("@ParentProjectId", ParentProjectId));
            return table;
        }
        public Models.Project GetProjectByProjecTNameByParentId_Name_Level(string ParentProjectId, string ProjectName, int Level, int order)
        {
            System.Data.OleDb.OleDbParameter[] para = {
                                                     new System.Data.OleDb.OleDbParameter("@ParentProjectId",ParentProjectId),
                                                     new System.Data.OleDb.OleDbParameter("@ProjectName",ProjectName),
                                                     new System.Data.OleDb.OleDbParameter("@Level",Level),
                                                     };
            DataTable table = Context.GetData(" ParentProjectId = @ParentProjectId and ProjectName=@ProjectName and ProjectLevel=@ProjectLevel", para);
            if (table.Rows.Count > 0)
            {
                Models.Project project = Context.GetModelByRow(table.Rows[0]);
                return project;
            }
            else
            {
                Models.Project project = new Models.Project()
                {
                    ProjectId = Utils.getGUID(),
                    ParentProjectId = ParentProjectId,
                    ProjectLevel = Level,
                    ProjectName = ProjectName,
                    DisplayOrder = order
                };
                FunctionResult result = this.Create(project);
                if (result.ExcuteState)
                {
                    return project;
                }
                else {
                    throw new ArgumentException("创建项目失败，请检查");
                }
            }
        }
    }
}
