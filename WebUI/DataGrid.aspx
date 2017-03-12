<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="DataGrid.aspx.cs" Inherits="WebUI.DataGrid" %>

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
            min-width: 1000px;
        }
        a
        {
            color: #666;
            text-decoration: none;
        }
        .head label
        {
            display: inline-block;
            min-width: 80px;
            text-align: right;
            padding: 0px 5px;
            line-height: 19px;
        }
        .easyui_form > div
        {
            padding: 5px;
            float: left;
        }
        .easyui_form .normal
        {
            width: 120px;
        }
        .easyui_form .small
        {
            width: 50px;
        }
        .head
        {
            width: 100%;
            height: 200px;
        }
        .chart
        {
            width: 90%; height: 300px; margin:auto;
        }
        .head .button
        {
            display: inline-block;
            padding: 4px 15px;
            background-color: #78ce07;
            color: #fff;
            border-radius: 4px;
        }
        .head .button:hover
        {
            background-color: #485b79;
        }
        fieldset
        {
            border: 1px solid #95B8E7;
        }
        .textbox, .easyui-combobox
        {
            height: 23px;
            line-height: 23px;
            width: 190px;
        }
        
        .easyui-datebox
        {
            width: 82px;
        }
        .chart
        {
            padding: 10px;
        }
        
    </style>
</head>
<body >
    <form id="form1" runat="server">
    <div class="head">
        <fieldset>
            <legend>查询条件</legend>
            <div style="float: left;">
                <div style="padding: 5px">
                    <span>
                        <label>
                            单位工程：</label>
                        <select id="Select3" class="easyui-combobox" name="dept">
                            <option value="aa">aitem1</option>
                            <option>bitem2</option>
                            <option>bitem3</option>
                            <option>ditem4</option>
                            <option>eitem5</option>
                        </select>
                    </span><span>
                        <label>
                            分部工程：</label>
                        <select id="Select4" class="easyui-combobox" name="dept">
                            <option value="aa">aitem1</option>
                            <option>bitem2</option>
                            <option>bitem3</option>
                            <option>ditem4</option>
                            <option>eitem5</option>
                        </select>
                    </span><span>
                        <label>
                            子分部工程：</label>
                        <select id="Select5" class="easyui-combobox" name="dept">
                            <option value="aa">aitem1</option>
                            <option>bitem2</option>
                            <option>bitem3</option>
                            <option>ditem4</option>
                            <option>eitem5</option>
                        </select>
                    </span>
                </div>
                <div style="padding: 5px">
                    <span>
                        <label>
                            分项工程：</label>
                        <select id="Select6" class="easyui-combobox" name="dept">
                            <option value="aa">aitem1</option>
                            <option>bitem2</option>
                            <option>bitem3</option>
                            <option>ditem4</option>
                            <option>eitem5</option>
                        </select>
                    </span><span>
                        <label>
                            龄期：</label>
                        <select id="Select2" class="easyui-combobox" name="dept">
                            <option value="aa">aitem1</option>
                            <option>bitem2</option>
                            <option>bitem3</option>
                            <option>ditem4</option>
                            <option>eitem5</option>
                        </select>
                    </span><span>
                        <label>
                            样品规格：</label>
                        <select id="Select1" class="easyui-combobox" name="dept">
                            <option value="aa">aitem1</option>
                            <option>bitem2</option>
                            <option>bitem3</option>
                            <option>ditem4</option>
                            <option>eitem5</option>
                        </select>
                    </span>
                </div>
                <div style="padding: 5px">
                    <span>
                        <label>
                            设计强度：</label>
                        <input id="Text6" class=" textbox easyui-validatebox" />
                    </span><span>
                        <label>
                            取样日期：</label>
                        <input id="Text1" class=" textbox easyui-datebox" />
                        至
                        <input id="Text2" class=" textbox easyui-datebox" />
                    </span><span>
                        <label>
                            实验日期：</label>
                        <input id="Text9" class=" textbox easyui-datebox" />
                        至
                        <input id="Text10" class=" textbox easyui-datebox" />
                    </span>
                </div>
            </div>
            <div style="float: left">
                <div style="padding: 5px">
                    <span style="display: inline-block; padding-right: 20px;"><a href="javascript:void"
                        class="button">查询</a> </span>
                </div>
            </div>
        </fieldset>
    </div>
    <div class="chart">
        <div id="ChartContainer" style="width: 100%; height: 300px;">
        </div>
    </div>
    <div class="grid" style="padding:10px;">
        <table id="dg">
        </table>
    </div>
    <div id="dd">Dialog Content.</div>
    <script>
        $(document).ready(function () {
            var option = {
                title: {
                    text: '未来一周气温变化',
                    subtext: '纯属虚构'
                },
                tooltip: {
                    trigger: 'axis'
                },
                legend: {
                    data: ['最高气温', '最低气温']
                },
                toolbox: {
                    show: true,
                    feature: {
                        mark: { show: true },
                        dataView: { show: true, readOnly: false },
                        magicType: { show: true, type: ['line', 'bar'] },
                        restore: { show: true },
                        saveAsImage: { show: true }
                    }
                },
                calculable: true,
                xAxis: [
                    {
                        type: 'category',
                        boundaryGap: false,
                        data: ['周一', '周二', '周三', '周四', '周五', '周六', '周日']
                    }
                ],
                yAxis: [
                    {
                        type: 'value',
                        axisLabel: {
                            formatter: '{value} °C'
                        }
                    }
                ],
                series: [
                    {
                        name: '最高气温',
                        type: 'line',
                        data: [11, 11, 15, 13, 12, 13, 10],
                        markPoint: {
                            data: [
                                { type: 'max', name: '最大值' },
                                { type: 'min', name: '最小值' }
                            ]
                        },
                        markLine: {
                            data: [
                                { type: 'average', name: '平均值' }
                            ]
                        }
                    },
                    {
                        name: '最低气温',
                        type: 'line',
                        data: [1, -2, 2, 5, 3, 2, 0],
                        markPoint: {
                            data: [
                                { name: '周最低', value: -2, xAxis: 1, yAxis: -1.5 }
                            ]
                        },
                        markLine: {
                            data: [
                                { type: 'average', name: '平均值' }
                            ]
                        }
                    }
                ]
            };
            var myChart = echarts.init(document.getElementById("ChartContainer"));
            myChart.setOption(option, true);
            $('#dg').datagrid({
                url: 'datagrid_data.json',
                method: 'get',
                striped: true,
                nowrap: false,
                pagination: true,
                height: 400,
                pageNumber: 1,
                pageSize: 15,
                pageList: [5, 10, 15, 20, 25],
                wdith: 900,
                columns: [[
		            { field: 'code', title: '序号' },
		            { field: 'prject', title: '标段工程' },
		            { field: 'unit', title: '施工单位' },
		            { field: 'name', title: '工程名称' },
		            { field: 'part', title: '工程部位' },
		            { field: 'qdate', title: '取样日期' },
		            { field: 'sdate', title: '实验日期' },
		            { field: 'lq', title: '龄期' },
		            { field: 'gg', title: '样品规格' },
		            { field: 'sqd', title: '设计强度(Mpa)' },
		            { field: 'result', title: '实验结果' },
		            { field: 'hs', title: '换算强度(Mpa)' },
		            { field: 'bfb', title: '达到设计强度百分比(%)', width: '80' },
		            { field: 'he', title: '合格判定' },
		            { field: 'xq', title: '查看详情',
		                formatter: function (value, row, index) {
		                    return '<a href="javascript:showDetails(\'' + row.code + '\')">详情</a>';
		                }
		            }
                ]]
            });
        });

		    function showDetails(code) {
		        $('#dd').dialog({
		            title: '详细信息',
		            width: 600,
		            height: 500,
		            closed: false,
		            cache: false,
		            content: "<iframe src='Details.aspx' width='580px' height='460px' />",
		            modal: true
		        });
		        //$('#dd').dialog('refresh', 'new_content.php');
		    }
    </script>
    </form>
</body>
</html>
