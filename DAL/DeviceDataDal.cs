using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.OleDb;
using Models;

namespace DAL
{
    public class DeviceDataDal :BaseDal<Models.DeviceData>
    {
        public DeviceDataDal() : base() {
            this.SelectSql = " select * from DeviceData where 1=1";
            this.CreateSql = " insert into DeviceData values (@DataId,@GroupName,@DeviceValue,@ProjectDetailId,@Instar,@YangSheng,@SamplingDate,@IfUpload,@NotUploadReason,@CreateDate) ";
            this.EditSql = " update DeviceData set GroupName=@GroupName,DeviceValue=@DeviceValue,ProjectDetailId=@ProjectDetailId,Instar=@Instar,YangSheng=@YangSheng,SamplingDate=@SamplingDate,IfUpload = @IfUpload,NotUploadReason=@NotUploadReason,CreateDate=@CreateDate  where DataId=@DataId ";
            this.DeleteSql = "Delete from DeviceData where 1=1 ";
        }
        public override Models.DeviceData GetModel(string id)
        {
            DataTable table = new DataTable();
            string sql = SelectSql + " and DataId=@DataId ";
            table = db.Query(sql, new OleDbParameter("@ProjectId", id)).Tables[0];
            if (table.Rows.Count > 0)
            {
                DeviceData DeviceData = GetModelByRow(table.Rows[0]);

                return DeviceData;
            }
            else
            {
                return null;
            }
        }

        public override int Create(Models.DeviceData t)
        { 
            OleDbParameter[] para = { 
                new OleDbParameter("@DataId",t.DataId),
                new OleDbParameter("@GroupName",t.GroupName),
                new OleDbParameter("@DeviceValue",t.DeviceValue),
                new OleDbParameter("@ProjectDetailId",t.ProjectDetailId),
                new OleDbParameter("@Instar",t.Instar),
                new OleDbParameter("@YangSheng",t.YangSheng),
                new OleDbParameter("@SamplingDate",t.SamplingDate),
                new OleDbParameter("@IfUpload",t.IfUpload),
                new OleDbParameter("@NotUploadReason",t.NotUploadReason),
                new OleDbParameter("@CreateDate",t.CreateDate)
                                    };
            int n = db.ExecuteSql(CreateSql, para);
            return n;
        }

        public override int Edit(Models.DeviceData t)
        {
            OleDbParameter[] para = { 
                new OleDbParameter("@GroupName",t.GroupName),
                new OleDbParameter("@DeviceValue",t.DeviceValue),
                new OleDbParameter("@ProjectDetailId",t.ProjectDetailId),
                new OleDbParameter("@Instar",t.Instar),
                new OleDbParameter("@YangSheng",t.YangSheng),
                new OleDbParameter("@SamplingDate",t.SamplingDate),
                new OleDbParameter("@IfUpload",t.IfUpload),
                new OleDbParameter("@NotUploadReason",t.NotUploadReason),
                new OleDbParameter("@CreateDate",t.CreateDate),
                new OleDbParameter("@DataId",t.DataId)
                                    };
            int n = db.ExecuteSql(EditSql, para);
            return n;
        }

        public override int Delete(Models.DeviceData t)
        { 
            string sql = DeleteSql + " and DataId=@DataId ";
            int n = db.ExecuteSql(sql, new OleDbParameter("@DataId", t.DataId));
            return n;
        } 
        public override DeviceData GetModelByRow(DataRow row)
        {
            try
            {
                DeviceData DeviceData = new DeviceData()
                {
                    DataId = row["DataId"].ToString(),
                    DeviceValue = row["DeviceValue"].ToString(),
                    GroupName = row["GroupName"].ToString(),
                    ProjectDetailId = row["ProjectDetailId"].ToString(),
                    IfUpload = int.Parse(row["IfUpload"].ToString()),
                    NotUploadReason =  row["NotUploadReason"].ToString(),
                    Instar = row["Instar"].ToString(),
                    YangSheng = row["YangSheng"].ToString(),
                    SamplingDate = row["SamplingDate"].ToString(),
                    CreateDate = row["CreateDate"].ToString(),
                    ProjectDetail = new ProjectDetailsDal().GetModel( row["ProjectDetailId"].ToString())
                };
                return DeviceData;
            }catch{
                return null;
            }
        } 
    }
}
