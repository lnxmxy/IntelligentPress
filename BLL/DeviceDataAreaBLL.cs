using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace BLL
{
    public class DeviceDataAreaBLL:BaseBll<Models.DeviceDataArea>
    {
        public DeviceDataAreaBLL() {
            this.Context = new DAL.DeviceDataAreaDal();
        }
        public override List<ValidataError> Validata(Models.DeviceDataArea Model)
        {
            return new List<ValidataError>();
        }
        public bool IfInArea(double value) {
            DataTable table = this.GetData();
            if (table.Rows.Count > 0) {
                Models.DeviceDataArea area = this.Context.GetModelByRow(table.Rows[0]);
                return value >= area.MinValue && value <= area.MaxValue;
            }
            return false;
           
        }
    }
}
