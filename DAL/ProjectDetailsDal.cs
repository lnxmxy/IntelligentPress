using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.OleDb;
using Models;

namespace DAL
{
    public class ProjectDetailsDal : BaseDal<Models.ProjectDetails>
    {
        public ProjectDetailsDal()
            : base()
        {
            this.SelectSql = " select * from ProjectDetails where 1=1";
            this.CreateSql = " insert into ProjectDetails values (@DetailsId,@DetailsTilte,@ProjectId,@DisplayOrder,@SerialNumber,@Mileage,@Intensity,@CementContent,@MixDesign,@DetailState) ";
            this.EditSql = "  update ProjectDetails set DetailsTilte=@DetailsTilte,ProjectId=@ProjectId,DisplayOrder=@DisplayOrder, SerialNumber=@SerialNumber, "
                           + " Mileage=@Mileage,Intensity=@Intensity,CementContent=@CementContent, MixDesign=@MixDesign, DetailState=@DetailState "
                           + " where DetailsId=@DetailsId ";
            this.DeleteSql = "Delete from ProjectDetails where 1=1 ";
        }
        public override Models.ProjectDetails GetModel(string id)
        {
            DataTable table = new DataTable();
            string sql = SelectSql + " and DetailsId=@DetailsId ";
            table = db.Query(sql, new OleDbParameter("@DetailsId", id)).Tables[0];
            if (table.Rows.Count > 0)
            {
                ProjectDetails ProjectDetails = GetModelByRow(table.Rows[0]);
                ProjectDetails.Project = new ProjectDal().GetModel(ProjectDetails.ProjectId);
                //ProjectDetails.DeviceDataList =ListSupport.ToList<Models.DeviceData>( new DeviceDataDal().GetData(" and ProjectDetailId='" + ProjectDetails.DetailsId + "'"));
                return ProjectDetails;
            }
            else
            {
                return null;
            }
        }

        public override int Create(Models.ProjectDetails t)
        { 
            OleDbParameter[] para = { 
                new OleDbParameter("@DetailsId",t.DetailsId), 
                new OleDbParameter("@DetailsTilte",t.DetailsTilte), 
                new OleDbParameter("@ProjectId",t.ProjectId), 
                new OleDbParameter("@DisplayOrder",t.DisplayOrder), 
                new OleDbParameter("@SerialNumber",t.SerialNumber), 
                new OleDbParameter("@Mileage",t.Mileage), 
                new OleDbParameter("@Intensity",t.Intensity), 
                new OleDbParameter("@CementContent",t.CementContent), 
                new OleDbParameter("@MixDesign",t.MixDesign), 
               //new OleDbParameter("@Instar",t.Instar), 
                //new OleDbParameter("@SamplingDate",t.SamplingDate) ,
                new OleDbParameter("@DetailState",t.DetailState) 
                                    };
            int n = db.ExecuteSql(CreateSql, para);
            return n;
        }

        public override int Edit(Models.ProjectDetails t)
        {

            OleDbParameter[] para = { 
                new OleDbParameter("@DetailsTilte",t.DetailsTilte), 
                new OleDbParameter("@ProjectId",t.ProjectId), 
                new OleDbParameter("@DisplayOrder",t.DisplayOrder), 
                new OleDbParameter("@SerialNumber",t.SerialNumber), 
                new OleDbParameter("@Mileage",t.Mileage), 
                new OleDbParameter("@Intensity",t.Intensity), 
                new OleDbParameter("@CementContent",t.CementContent), 
                new OleDbParameter("@MixDesign",t.MixDesign), 
              //  new OleDbParameter("@Instar",t.Instar), 
               // new OleDbParameter("@SamplingDate",t.SamplingDate),
                new OleDbParameter("@DetailState",t.DetailState)  ,
                new OleDbParameter("@DetailsId",t.DetailsId)
                                    };
            int n = db.ExecuteSql(EditSql, para);
            return n;
        }

        public override int Delete(Models.ProjectDetails t)
        {
            string sql = DeleteSql + " and DetailsId=@DetailsId ";
            int n = db.ExecuteSql(sql, new OleDbParameter("@DetailsId", t.DetailsId));
            return n;
        }

        public override ProjectDetails GetModelByRow(DataRow row)
        {
            try
            {
                ProjectDetails ProjectDetails = new ProjectDetails()
                {
                    CementContent = row["CementContent"].ToString(),
                    DetailsId = row["DetailsId"].ToString(),
                    DetailsTilte = row["DetailsTilte"].ToString(),
                    DisplayOrder = int.Parse(row["DisplayOrder"].ToString()),
                   // Instar = int.Parse(row["Instar"].ToString()),
                    Intensity = row["Intensity"].ToString(),
                    Mileage = row["Mileage"].ToString(),
                    MixDesign = row["MixDesign"].ToString(),
                    ProjectId = row["ProjectId"].ToString(),
                   // SamplingDate = row["SamplingDate"].ToString(),
                    SerialNumber = row["SerialNumber"].ToString(),
                    DetailState = int.Parse(row["DetailState"].ToString())
                };
              //  ProjectDetails.DeviceDataList = ListSupport.ToList<Models.DeviceData>(new DeviceDataDal().GetData(" ProjectDetailId=@ProjectDetailId ", new System.Data.OleDb.OleDbParameter("@ProjectDetailId", ProjectDetails.DetailsId)));

                return ProjectDetails;
            }
            catch {
                return null;
            }
        }
    }
}
