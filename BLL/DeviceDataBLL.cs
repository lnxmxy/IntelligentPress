using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.OleDb;

namespace BLL
{
    public class DeviceDataBLL:BaseBll<Models.DeviceData>
    {
        public DeviceDataBLL() {
            this.Context = new DAL.DeviceDataDal();
        }

        public override List<ValidataError> Validata(Models.DeviceData Model)
        {
            List<ValidataError> list = new List<ValidataError>();
           
            if (Utils.IsEmptyOrNull(Model.DataId)) {
                list.Add(new ValidataError("DataId", "数据编号必须填写"));
            }
            if (Utils.IsEmptyOrNull(Model.DeviceValue))
            {
                list.Add(new ValidataError("DeviceValue", "数据值必须填写"));
            }
            if (Utils.IsEmptyOrNull(Model.GroupName))
            {
                list.Add(new ValidataError("GroupName", "组别必须填写"));
            }
            if (Utils.IsEmptyOrNull(Model.ProjectDetailId))
            {
                list.Add(new ValidataError("ProjectDetailId", "工程项目编号必须填写"));
            }
            return list;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="p1Id">一级项目编号</param>
        /// <param name="p2Id">二级项目编号</param>
        /// <param name="p3Id">三级项目编号</param>
        /// <param name="projectDetailsId">四i项目编号</param>
        /// <param name="beginDate">查询开始日期</param>
        /// <param name="endDate">开始结束日期</param>
        /// <param name="deviceValue">RFID值</param>
        /// <param name="updateState">上传状态0：全部 1：已上传 2：未上传</param>
        /// <returns></returns>
        public DataTable Query(string p1Id, string p2Id, string p3Id, string projectDetailsId, string beginDate, string endDate, string deviceValue,int updateState=0)
        {
            // t2.DetailsTilte, t2.ProjectId, t2.DisplayOrder, t2.SerialNumber, t2.Mileage, t2.Intensity, t2.CementContent, t2.MixDesign, t2.Instar, t2.SamplingDate, t2.DetailState, t2.p3Id, t2.p3Name, t2.p2Id, t2.p2Name, t2.p1Id, t2.p1Name
            string sql = "select  p1Name as 单位工程, p2Name as 分部工程,p3Name as 子分部工程,DetailsTilte as 项目工程,GroupName as 试验分组,DeviceValue as RFID卡号,Instar as 龄期 ,YangSheng as 养生类型,SamplingDate as 取样时间,SerialNumber as 分项工程编号,Mileage as 取样里程,Intensity as 设计强度,CementContent as 立方水泥用量,MixDesign as 设计混合比 ";
            if(updateState == 2){
                sql += " ,NotUploadReason as 未上传原因 ";
            }
            sql += "from DeviceDatatotal where 1=1 ";
            string where = "";
            string order = "";
            List<OleDbParameter> list = new List<OleDbParameter>();
            if (!Utils.IsEmptyOrNull(projectDetailsId))
            {
                where += " and ProjectDetailId=@ProjectDetailId ";
                list.Add(new OleDbParameter("@ProjectDetailId", projectDetailsId));
            }

            if (!Utils.IsEmptyOrNull(p3Id))
            {
                where += " and p3Id=@p3Id ";
                list.Add(new OleDbParameter("@p3Id", p3Id));
            }
            if (!Utils.IsEmptyOrNull(p2Id))
            {
                where += " and p2Id=@p2Id ";
                list.Add(new OleDbParameter("@p2Id", p2Id));
            }
            if (!Utils.IsEmptyOrNull(p1Id))
            {
                where += " and p1Id=@p1Id ";
                list.Add(new OleDbParameter("@p1Id", p1Id));
            }
            if (updateState != 2)
            {
                if (!Utils.IsEmptyOrNull(beginDate))
                {
                    where += " and SamplingDate>=@beginDate ";
                    list.Add(new OleDbParameter("@beginDate", beginDate));
                }
                if (!Utils.IsEmptyOrNull(endDate))
                {
                    where += " and SamplingDate<=@endDate ";
                    list.Add(new OleDbParameter("@endDate", endDate));
                }
            }
            if (updateState == 2) {
                where += " and IfUpload = 0 "; 
            }
            else if (updateState == 1)
            {
                where += " and IfUpload = 1"; 
            }
            if (!Utils.IsEmptyOrNull(deviceValue))
            {
                where += " and DeviceValue like @deviceValue ";
                list.Add(new OleDbParameter("@deviceValue","%"+deviceValue+"%" ));
            }
            

            sql = sql + where + order;
            return this.Context.Query(sql,list.ToArray());
        }
    }
}
