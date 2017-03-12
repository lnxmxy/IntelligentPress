<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Details.aspx.cs" Inherits="WebUI.Details" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link href="Scripts/jquery-easyui-1.5.1/themes/default/easyui.css" rel="stylesheet"
        type="text/css" />
    <link href="Scripts/jquery-easyui-1.5.1/themes/icon.css" rel="stylesheet" type="text/css" />
    <link href="Scripts/jquery-easyui-1.5.1/themes/color.css" rel="stylesheet" type="text/css" />
    <script src="Scripts/jquery-easyui-1.5.1/jquery.min.js" type="text/javascript"></script>
    <script src="Scripts/jquery-easyui-1.5.1/jquery.easyui.min.js" type="text/javascript"></script>
    <script src="Scripts/echarts.min.js" type="text/javascript"></script>
    <style>
    body
        {
            padding: 0;
            margin: 0;
            font-family: 微软雅黑;
            font-size: 12px;
            color: #666;
            width: 580px;
        }
        td
        {
             vertical-align:top; 
            }
            #details .panel-header,#details .panel-body
            {
                border-left:none;
                border-right:none;
                }
                #tableGrid .datagrid-row
                {
                     height:270px;
                    }
                     #tableGrid td{ border:none;}
    </style>
</head>
<body>
    <form id="form1" runat="server">
   <div>
        <div class="easyui-panel" title="当前XXXXX详情"  data-options="iconCls:'icon-save'" style=" width:100%">
        <table border='0' width='100%' cellpadding=2 cellspacing=2>
            <tr>
            <td>序号：1</td>
            <td>标段工程：梨沟大桥</td>
            <td>施工单位：单位1</td>
            <td>工程名称：基础及下部构造</td>
            </tr>
            
            <tr>
            <td>工程部位：右桥1号桥</td>
            <td>取样日期：2017-02-24</td>
            <td>实验日期：2017-02-24</td>
            <td>龄期：28天</td>
            </tr>
            
            <tr>
            <td>样品规格：11</td>
            <td>设计强度(Mpa:C25</td>
            <td>实验结果:330</td>
            <td>换算强的(MPA)：330</td>
            </tr>
            
            <tr>
            <td>达到设计强度百分比(%)：80</td>
            <td>合格判定:合格</td>
            <td></td>
            <td></td>
            </tr>
            
        </table>
        </div>
   </div>
   <div id="details" class="easyui-panel" title="当前查看对应的工程是：xxxx"  data-options="iconCls:'icon-save'" style=" width:100%">
   <div style=" width:160px; float:left; ">
        <table class="easyui-datagrid"data-options="height:305, rownumbers:true"  >
    <thead>
		<tr>
			<th data-options="field:'code'">Code</th>
			<th data-options="field:'name'">Name</th>
			<th data-options="field:'price'">Price</th>
		</tr>
    </thead>
    <tbody>
		<tr>
			<td>001</td><td>name1</td><td>2323</td>
		</tr>
		<tr>
			<td>002</td><td>name2</td><td>4612</td>
		</tr>
		<tr>
			<td>002</td><td>name2</td><td>4612</td>
		</tr>
	</tbody>
</table>
   </div>
   <div  style=" width:415px; float:left;" id="tableGrid"> 
    <table class="easyui-datagrid"data-options="height:305,onLoadSuccess:function(){init();}" id="myGrid"  >
     <thead>
		<tr>
			<th data-options="field:'code',width:'100%',formatter: function(value,row,index){return aa();}">数据图形</th> 
		</tr>
    </thead>
    <tbody>
		<tr>
			<td  >
           
            </td>
		</tr>
        </tbody>
    </table>
   </div>
   </div>
    
            <script>
                function aa() { 
                    return '<div style="width:90%; height:200px; margin:auto;" id="tableChart"> </div><div><center>试件1的图形</center></div>';
                }
                function init() {
                    option = {
                        title: {
                            text: "对数轴示例",
                            x: "center"
                        },
                        tooltip: {
                            trigger: "item",
                            formatter: "{a} <br/>{b} : {c}"
                        },
                        legend: {
                            x: 'left',
                            data: ["2的指数"]
                        },
                        xAxis: [
       {
           type: "category",
           name: "x",
           splitLine: { show: false },
           data: ["一", "二", "三", "四", "五", "六", "七", "八", "九"]
       }
   ],
                        yAxis: [
       {
           type: "log",
           name: "y"
       }
   ],
                        toolbox: {
                            show: true,
                            feature: {
                                mark: {
                                    show: true
                                },
                                dataView: {
                                    show: true,
                                    readOnly: true
                                },
                                restore: {
                                    show: true
                                },
                                saveAsImage: {
                                    show: true
                                }
                            }
                        },
                        calculable: true,
                        series: [
       {
           name: "2的指数",
           type: "line",
           data: [1, 2, 4, 8, 16, 32, 64, 128, 256]

       }
   ]
                    };
                    var myChart = echarts.init(document.getElementById("tableChart"));
                    myChart.setOption(option, true);
                }
   </script>
    </form>
</body>
</html>
