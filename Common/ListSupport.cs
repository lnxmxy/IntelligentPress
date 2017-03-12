using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Reflection;
/// <summary>
///ListSupport 的摘要说明
/// </summary>
public static class ListSupport
{
  

//将泛型类转换成DataTable 
        public static DataTable Fill<T>(IList<T> objlist)
        {
            if (objlist == null || objlist.Count <= 0)
            {
                return null;
            }
            DataTable dt = new DataTable(typeof(T).Name);
            DataColumn column;
            DataRow row;
 
            System.Reflection.PropertyInfo[] myPropertyInfo = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);
 
            foreach (T t in objlist)
            {
                if (t == null)
                {
                    continue;
                }
 
                row = dt.NewRow();
 
                for (int i = 0, j = myPropertyInfo.Length; i < j; i++)
                {
                    System.Reflection.PropertyInfo pi = myPropertyInfo[i];
 
                    string name = pi.Name;
 
                    if (dt.Columns[name] == null)
                    {
                        column = new DataColumn(name, pi.PropertyType);
                        dt.Columns.Add(column);
                    }
 
                    row[name] = pi.GetValue(t, null);
                }
 
                dt.Rows.Add(row);
            }
            return dt;
        }
        public static List<TResult> ToList<TResult>(this DataTable dt) where TResult : class,new()
        {
            //创建一个属性的列表
            List<PropertyInfo> prlist = new List<PropertyInfo>();
            //获取TResult的类型实例  反射的入口
            Type t = typeof(TResult);
            //获得TResult 的所有的Public 属性 并找出TResult属性和DataTable的列名称相同的属性(PropertyInfo) 并加入到属性列表 
            Array.ForEach<PropertyInfo>(t.GetProperties(), p => { if (dt.Columns.IndexOf(p.Name) != -1) prlist.Add(p); });
            //创建返回的集合
            List<TResult> oblist = new List<TResult>();

            foreach (DataRow row in dt.Rows)
            {
                //创建TResult的实例
                TResult ob = new TResult();
                //找到对应的数据  并赋值
                prlist.ForEach(p => { if (row[p.Name] != DBNull.Value) p.SetValue(ob, row[p.Name], null); });
                //放入到返回的集合中.
                oblist.Add(ob);
            }
            return oblist;
        }
        public static string ToJson<T>(IList<T> list) {
            return Newtonsoft.Json.JsonConvert.SerializeObject(list); 
        }
        public static string ToJson<T>(T obj) {
            return Newtonsoft.Json.JsonConvert.SerializeObject(obj); 
        }
}