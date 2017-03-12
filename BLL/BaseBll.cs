using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace BLL
{
    public class ValidataError{
        public string ProprtyName { get; set; }
        public string ErrorMessage{get;set;}
        public ValidataError() { }
        public ValidataError(string ProprtyName, string ErrorMessage) { 
            this.ErrorMessage=ErrorMessage;
            this.ProprtyName = ProprtyName;
        }
    }
    public class FunctionResult {
        public bool ExcuteState { get; set; }
        public string Message { get; set; }
        public List<ValidataError> ErrorList { get; set; } 
    }
    public delegate int ExcuteDeleGate<T>(T Model);
    public abstract class BaseBll<T>
    {
        protected DAL.BaseDal<T> Context { get; set; }
        public abstract List<ValidataError> Validata(T Model);
        private FunctionResult Excute(T Model, ExcuteDeleGate<T> excute,bool ifNeedVlatete=true) {
            FunctionResult result = new FunctionResult();
            if (ifNeedVlatete)
            {
                result.ErrorList = this.Validata(Model);
                if (result.ErrorList.Where(p => p.ErrorMessage.Contains("已存在")).Count() > 0) {
                    result.ExcuteState = true;
                    result.Message = "数据已存在";
                    return result;
                }
                if (result.ErrorList.Count > 0)
                {
                    result.ExcuteState = false;
                    result.Message = "验证未通过，请检查";
                    return result;
                }
            }
            int n = excute(Model);
            if (n > 0)
            {
                result.ExcuteState = true;
                result.Message = "保存成功";
            }
            else
            {
                result.ExcuteState = false;
                result.Message = "保存数据失败";
            }
            return result;
        }
        public FunctionResult Create(T Model) {
            return Excute(Model, Context.Create); 
        }
        public FunctionResult Edit(T Model) {
            return Excute(Model, Context.Edit); 
        }
        public FunctionResult Delete(T Model) {
            return Excute(Model, Context.Delete, false);
        }
        public T GetModel(string id) {
            return Context.GetModel(id);
        }
        public T GetModelByWhere(string where) {
            return Context.GetModelByWhere(where);
        }
        public T GetModelByWhere(string where, params System.Data.OleDb.OleDbParameter[] para) {
            return Context.GetModelByWhere(where, para);
        }
        public T GetModelByRow(DataRow row) {
            return Context.GetModelByRow(row);
        }
        public int Excute(string sql)
        {
            return Context.ExcuteSql(sql);
        }
        public int Excute(string sql, params System.Data.OleDb.OleDbParameter[] para) {
            return Context.ExcuteSql(sql, para);
        }
        public DataTable Query(string sql) {
            return Context.Query(sql);
        }
        public DataTable Query(string sql, params System.Data.OleDb.OleDbParameter[] para) {
            return Context.Query(sql, para);
        }
        public DataTable GetData(string where="") {
           return   Context.GetData(where);
        }
        public DataTable GetData(string where, params System.Data.OleDb.OleDbParameter[] para) {
            return Context.GetData(where, para);
        }
    }
}
