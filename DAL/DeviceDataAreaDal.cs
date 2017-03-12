using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using Models;
using System.Data.OleDb;

namespace DAL
{
    public class DeviceDataAreaDal : BaseDal<Models.DeviceDataArea>
    {
        public DeviceDataAreaDal()
            : base()
        {
            this.SelectSql = " select * from DeviceDataArea where 1=1";
            this.CreateSql = " insert into DeviceDataArea values (@AreaId,@MaxValue,@MinValue) ";
            this.EditSql = " update DeviceDataArea set MaxValue=@MaxValue,MinValue=@MinValue  where AreaId=@AreaId ";
            this.DeleteSql = "Delete from DeviceDataArea where 1=1 ";
        }
        public override Models.DeviceDataArea GetModel(string id)
        {
            DataTable table = new DataTable();
            string sql = SelectSql + " and AreaId=@AreaId ";
            table = db.Query(sql, new OleDbParameter("@AreaId", id)).Tables[0];
            if (table.Rows.Count > 0)
            {
                DeviceDataArea DeviceData = GetModelByRow(table.Rows[0]);

                return DeviceData;
            }
            else
            {
                return null;
            }
        }

        public override int Create(Models.DeviceDataArea t)
        {
            OleDbParameter[] para = { 
                new OleDbParameter("@AreaId",t.AreaId),
                new OleDbParameter("@MaxValue",t.MaxValue),
                new OleDbParameter("@MinValue",t.MinValue) 
                                    };
            int n = db.ExecuteSql(CreateSql, para);
            return n;
        }

        public override int Edit(Models.DeviceDataArea t)
        {
            OleDbParameter[] para = { 
                new OleDbParameter("@MaxValue",t.MaxValue),
                new OleDbParameter("@MinValue",t.MinValue) ,
                new OleDbParameter("@AreaId",t.AreaId)
                                    };
            int n = db.ExecuteSql(EditSql, para);
            return n;
        }

        public override int Delete(Models.DeviceDataArea t)
        {
            string sql = DeleteSql + " and AreaId=@AreaId ";
            int n = db.ExecuteSql(sql, new OleDbParameter("@AreaId", t.AreaId));
            return n;
        }

        public override Models.DeviceDataArea GetModelByRow(System.Data.DataRow row)
        {
            try
            {
                DeviceDataArea DeviceDataArea = new DeviceDataArea()
                    {
                        AreaId = row["AreaId"].ToString(),
                        MaxValue = double.Parse(row["MaxValue"].ToString()),
                        MinValue = double.Parse(row["MinValue"].ToString())
                    };
                return DeviceDataArea;
            }
            catch
            {
                return null;
            }
        }
    }
}
