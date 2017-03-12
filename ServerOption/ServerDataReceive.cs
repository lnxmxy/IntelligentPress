using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace ServerOption
{
    public class ServerDataReceive
    {
        BLL.ProjectDetailsBLL detailsBll = new BLL.ProjectDetailsBLL();
        BLL.ProjectBLL projectBll = new BLL.ProjectBLL();
        BLL.DeviceDataBLL dataBll = new BLL.DeviceDataBLL();
        public bool DataReceive(string str)
        {
            try
            {
                Models.DeviceData details = Newtonsoft.Json.JsonConvert.DeserializeObject<Models.DeviceData>(str);
                Thread.Sleep(200);
            }
            catch (Exception ex)
            {
                throw new Exception("无效的参数");
            }
            return true;
        }
        private bool SaveDeviceDate(Models.DeviceData data) {
            Models.ProjectDetails ProjectDetail = data.ProjectDetail;
            if (SaveProjectDetails(ProjectDetail)) { 
                /*
                 读卡数据处理逻辑
                 */
                return dataBll.Create(data).ExcuteState;
            }
            return false;
        }
        private bool SaveProjectDetails(Models.ProjectDetails details) {
            Models.ProjectDetails temp = detailsBll.GetModel(details.DetailsId);
            if (temp != null) {//已存在的数据不再插入
                return true;
            }
            Models.Project project = details.Project;
            if (SaveProject(project)) { 
                /*
                 四级菜单处理逻辑
                 */
                return detailsBll.Create(details).ExcuteState;
            }
            return false;
        }
        private bool SaveProject(Models.Project project) {
            Models.Project temp = projectBll.GetModel(project.ProjectId);
            if (temp != null)
                return true;
            if (project.ParentProject != null) {
                bool b = SaveProject(project.ParentProject);
                if (!b) {//保存上级工程失败
                    return false;
                }
            }
            return projectBll.Create(project).ExcuteState;
            
        }
    }
}
