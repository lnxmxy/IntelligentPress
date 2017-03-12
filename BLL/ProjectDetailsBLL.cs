using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.OleDb;

namespace BLL
{
    public class ProjectDetailsBLL:BaseBll<Models.ProjectDetails>
    {
        public ProjectDetailsBLL() {
            this.Context = new DAL.ProjectDetailsDal();
        }
        public override List<ValidataError> Validata(Models.ProjectDetails Model)
        {
            List<ValidataError> list = new List<ValidataError>();
            if (Utils.IsEmptyOrNull(Model.CementContent))
            {
                list.Add(new ValidataError("CementContent", "每立方米水泥用量必须填写"));
            }
            if (Utils.IsEmptyOrNull(Model.DetailsId))
            {
                list.Add(new ValidataError("DetailsId", "工程详情编号必须填写"));
            }
            if (Utils.IsEmptyOrNull(Model.DetailsTilte))
            {
                list.Add(new ValidataError("DetailsTilte", "工程详情名称必须填写"));
            }
            if (Utils.IsEmptyOrNull(Model.Intensity))
            {
                list.Add(new ValidataError("Intensity", "设计强度必须填写"));
            }
            if (Utils.IsEmptyOrNull(Model.Mileage))
            {
                list.Add(new ValidataError("Mileage", "采样里程必须填写"));
            }
            if (Utils.IsEmptyOrNull(Model.MixDesign))
            {
                list.Add(new ValidataError("MixDesign", "设计混合比必须填写"));
            }
            if (Utils.IsEmptyOrNull(Model.ProjectId))
            {
                list.Add(new ValidataError("ProjectId", "项目名称必须填写"));
            }
            //if (Utils.IsEmptyOrNull(Model.SamplingDate))
            //{
            //    list.Add(new ValidataError("SamplingDate", "采样日期必须填写"));
            //}
            if (Utils.IsEmptyOrNull(Model.SerialNumber))
            {
                list.Add(new ValidataError("SerialNumber", "工程项目编号必须填写"));
            }
            return list;
        }
        public DataTable GetDetailsByProjectId(string ProjectId) {
            return Context.GetData(" and ProjectId = @ProjectId ", new System.Data.OleDb.OleDbParameter("ProjectId", ProjectId));
        }
        void CreatProject(Models.Project p) {

            ProjectBLL projectBll = new ProjectBLL();
            Models.Project project1 = projectBll.GetModel(p.ProjectId);
            if (project1 == null)
            {
                projectBll.Create(p);
            }
        }
        public void SaveList(List<Models.ProjectDetails> list) {
            DateTime dt = DateTime.Now;
            Common.MessageHelper.ShowMessage("建立一级菜单");
            List<Models.Project> p1 = list.Select(p => p.Project.ParentProject.ParentProject).Distinct().ToList();
            p1.ForEach(p => { 
                CreatProject(p);
            });
            Common.MessageHelper.ShowMessage("一级菜单建立完成");
            Common.MessageHelper.ShowMessage("建立二级菜单");
            p1 = list.Select(p => p.Project.ParentProject).Distinct().ToList();
            p1.ForEach(p => CreatProject(p));
            Common.MessageHelper.ShowMessage("二级菜单建立完成");
            Common.MessageHelper.ShowMessage("建立三级菜单");
            p1 = list.Select(p => p.Project).Distinct().ToList();
            p1.ForEach(p => CreatProject(p));
            Common.MessageHelper.ShowMessage("三级菜单建立完成");


            Common.MessageHelper.ShowMessage("开始建立四级菜单");
            list.ForEach(p => {
                Common.MessageHelper.ShowMessage(string.Format("正在处理四级菜单,当前处理第{0}个,共{1}个",list.IndexOf(p)+1,list.Count)); 
                Models.ProjectDetails details = this.GetModel(p.DetailsId);
                if (details == null) { 
                   FunctionResult result  = this.Create(p);
                    if(!result.ExcuteState){
                        Common.MessageHelper.ShowMessage("新建四级菜单中发生错误"+result.Message); 
                    }
                }
            });
            DateTime dt2 = DateTime.Now;

            Common.MessageHelper.ShowMessage(string.Format("处理完成，共耗时{0}秒",(dt2-dt).Milliseconds/(float)1000)); 
        }

        public DataTable GetDataByProjectId(string p1Id, string p2Id, string p3Id)
        {
            //DetailsId	DetailsTilte	ProjectId	DisplayOrder	SerialNumber	Mileage	Intensity	CementContent	MixDesign	Instar	SamplingDate	DetailState	p3Id	p3Name	p2Id	p2Name	p1Id	p1Name
            string sql = @" select DetailsId,DetailsTilte,ProjectId,DisplayOrder,SerialNumber,Mileage,Intensity,CementContent,MixDesign from ProjectDetails_Project ";
            string where ="  where 1=1 ";
            List<OleDbParameter> list = new List<OleDbParameter> ();
            if(!Utils.IsEmptyOrNull(p3Id)){
                where += " and p3Id=@p3Id";
                list.Add(new OleDbParameter("@p3Id",p3Id));
            }
            
            if(!Utils.IsEmptyOrNull(p2Id)){
                where += " and p2Id=@p2Id";
                list.Add(new OleDbParameter("@p2Id",p2Id));
            }
            
            if(!Utils.IsEmptyOrNull(p1Id)){
                where += " and p1Id=@p1Id";
                list.Add(new OleDbParameter("@p1Id",p1Id));
            }
            
            where +=" order by displayOrder";
            return Context.Query(sql+where,list.ToArray());
        }
    }
}
