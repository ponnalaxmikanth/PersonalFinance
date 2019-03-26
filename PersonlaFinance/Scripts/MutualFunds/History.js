$(function () {
    $('.datepicker').datepicker();
   

    var myDate = new Date();
    var todayDate = (myDate.getMonth() + 1) + '/' + myDate.getDate() + '/' + myDate.getFullYear();
    $('[data-new-chart="TODATE"]').val(moment(new Date(todayDate)).format("MM/DD/YYYY"));
    $('[data-new-chart="FROMDATE" ]').val(moment(new Date(new Date().getFullYear(), new Date().getMonth() - 1, myDate.getDate())).format("MM/DD/YYYY"));
    //var result = getElementById("FROMDATE").val;
   
    GetGraphHistory();

  //  GetBenchmarkHistoryData();
});
function onclickbutton()
{
    GetGraphHistory();
}

function GetGraphHistory(fromdate,todate)
{
    var myDate = new Date();
    var todayDate = (myDate.getMonth() + 1) + '/' + myDate.getDate() + '/' + myDate.getFullYear();
    

    var request = {
        fromDate: $('[data-new-chart="FROMDATE"]').val(),
    toDate: $('[data-new-chart="TODATE"]').val()
    };

    $.ajax({
        url: '/MFDashboard/GetGraphHistory',
        type: 'post',
        dataType: 'json',
        data: JSON.stringify(request),
        contentType: "application/json; charset=utf-8",
        success: GetGraphHistorysucess,
        error: GetGraphHistoryerror,
    });

};

function GetGraphHistorysucess(dataresponse)
{

    try {

        console.log("GetGraphHistorysucess....." + JSON.stringify(dataresponse));

        var dres=[];
        for (var i = 0; i < dataresponse.length; i++)
        {


            dres.push(
            {
                "Date": moment(new Date(dataresponse[i].Date)).format("DD/MM/YYYY"),
                "SchemaName": dataresponse[i].SchemaName,
                "SchemaCode": dataresponse[i].SchemaCode,
                "NAV": dataresponse[i].NAV,
            });
                       
          };

        generatechart(dres);
    }


    catch (ex)
    {
        console.log("GetGraphHistorysucess......." + ex);
           
    }
};

function GetGraphHistoryerror(dataresponse)
{
    try
    {
        console.log("GetGraphHistoryerror....."+JSON.stringify(dataresponse));
    }

    catch (exc)
    {

        console.log("GetGraphHistoryerror........." + exc);
    }
};

function generatechart(dataresponce)

{
    try {

        AmCharts.makeChart("chartdiv",
            {
                "type": "serial",
                "theme": "light",
                "legend": { "maxColumns": 10, "position": "top", "useGraphSettings": true, "markerSize": 10, "divId": "legenddiv" },//"useGraphSettings": true, "position": "top"
                "dataProvider": dataresponce,
                "valueAxes": [
                                { "id": "v1", "title": "Amount", "position": "left", "autoGridCount": false, "axisThickness": 2, "axisAlpha": 1, },
                                { "id": "v2", "title": "", "position": "right", "autoGridCount": false, "gridAlpha": 0, },
                                { "id": "v3", "title": "Redeem", "position": "left", "autoGridCount": false, "axisThickness": 2, "axisAlpha": 1, "offset": 50, },
                ],
                "categoryField": "Date",
                //"dataDateFormat": [{"period":"YYYY","format":"YYYY"}],
                "chartCursor": { "pan": false, "valueLineEnabled": true, "valueLineBalloonEnabled": true, "cursorAlpha": 0, "valueLineAlpha": 0.2 },
               
               
                "categoryAxis": { "gridPosition": "start" },
                "valueScrollbar": { "enabled": true },
                "chartScrollbar": { "enabled": true },
                "trendLines": [],
                "graphs": [
                    {
               
                        "balloonText": "<b>[[title]]</b><br><span style='font-size:14px;color:#84b761;'><b>[[value]]</b></span>",
                        "fillAlphas": 1,
                        "legendAlpha": 1,
                        "id": "AmGraph-1",
                        "title": "Invest",
                        "type": "column",
                        "markerType": "circle",
                        "valueField": "Investment",
                        "valueAxis": "v1",
                        "lineColor": "#84b761",
                        "fillColors": "#84b761",
                        "fillAlphas": 1,
                        "clustered": false,
                        "columnWidth": 0.8,
                    },
                    {
                        "id": "AmGraph-5",
                        "legendAlpha": 1,
                        "valueAxis": "v1",
                        "bullet": "round",
                        "bulletBorderAlpha": 1,
                        "bulletColor": "#FFFFFF",
                        "bulletSize": 5,
                        "hideBulletsCount": 50,
                        "lineThickness": 2,
               
                        "lineColor": "#f442c5",
               
                        "type": "smoothedLine",
                        "title": "Current Value",
                        "useLineColorForBulletBorder": true,
                        "valueField": "NAV",
               
                        "balloonText": "<b>[[title]]</b><br><span style='font-size:14px'><b>[[value]]</b></span>",
                    },
                    {
                        "id": "AmGraph-6",
                        "valueAxis": "v2",
                        "bullet": "round",
                        "bulletBorderAlpha": 1,
                        "bulletColor": "#2f4074",
                        "bulletSize": 5,
                        "hideBulletsCount": 50,
                        "lineThickness": 2,
                        "lineColor": "#2f4074",
                        "type": "smoothedLine",
                        "title": "Current Profit(%)",
                        "useLineColorForBulletBorder": true,
                        "valueField": "CurrentProfit",
               
                        "balloonText": "<b>[[title]]</b><br><span style='font-size:14px'><b>[[value]]</b></span>",
                    },
                    {
               
                        "balloonText": "<b>[[title]]</b><br><span style='font-size:14px'><b>[[value]]</b></span>",
                        "id": "AmGraph-2",
                        "title": "Redeem",
                        "type": "column",
                        "markerType": "circle",
                        "valueField": "RedeemInvest",
                        "valueAxis": "v1",
                        "lineColor": "#cc4748",
                        "fillColors": "#cc4748",
                        "fillAlphas": 1,
                        "clustered": false,
                        "columnWidth": 0.5,
                    },
                    {
               
                        "balloonText": "<b>[[title]]</b><br><span style='font-size:14px'><b>[[value]]</b></span>",
                        "fillAlphas": 1,
                        "id": "AmGraph-3",
                        "title": "Profit",
                        "markerType": "circle",
                        "minDistance": 0,
                        "legendAlpha": 1,
                        "type": "column",
                        "valueField": "Profit",
                        "valueAxis": "v1",
                        "lineColor": "#424bf4",
                        "fillColors": "#424bf4",
                        "fillAlphas": 1,
               
                        "columnWidth": 0.3,
                    },
                    {
                        "id": "AmGraph-4",
                        "valueAxis": "v2",
                        "bullet": "round",
                        "bulletBorderAlpha": 1,
                        "bulletColor": "#FFFFFF",
                        "bulletSize": 5,
                        "hideBulletsCount": 50,
                        "lineThickness": 2,
                        "lineColor": "#20acd4",
                        "type": "smoothedLine",
                        "title": "Profit (%)",
                        "useLineColorForBulletBorder": true,
                        "valueField": "ProfitPer",
               
                        "balloonText": "<b>[[title]]</b><br><span style='font-size:14px'><b>[[value]]</b></span>",
                    }
                ],
                "guides": [],
                "allLabels": [],
               
            }
        );

    }
    catch (exc)
    {
        console.log("generatechart... " + exc);
    }
}


function GetBenchmarkHistoryData() {
    var myDate = new Date();
    var todayDate = (myDate.getMonth() + 1) + '/' + myDate.getDate() + '/' + myDate.getFullYear();

    var request = {
        fromDate: '01/01/2018',
        toDate: todayDate
        };

    $.ajax({
        url: '/MFDashboard/GetBenchmarkHistoryValues',
        type: 'post',
        dataType: 'json',
        data: JSON.stringify(request),
        contentType: "application/json; charset=utf-8",
        success: GetBenchmarkHistoryDataSuccess,
        error: GetBenchmarkHistoryDataError
    });
};

function GetBenchmarkHistoryDataSuccess(response)
{
    try {
        console.log(JSON.stringify(response));
        AmCharts.makeChart("chartdiv",
                    {
                        "type": "serial",
                        "theme": "light",
                        "legend": { "maxColumns": 10, "position": "top", "useGraphSettings": true, "markerSize": 10, "divId": "legenddiv" },
                        "dataProvider": response,
                        "valueAxes": [
                                        { "id": "v1", "title": "HistoryDetails.Date", "position": "left", "autoGridCount": false, "axisThickness": 2, "axisAlpha": 1, }
                        ],
                        //"categoryField": "category",
                        //"chartCursor": { "pan": false, "valueLineEnabled": true, "valueLineBalloonEnabled": true, "cursorAlpha": 0, "valueLineAlpha": 0.2 },
                        "categoryAxis": { "gridPosition": "start" },
                        "valueScrollbar": { "enabled": true },
                        "chartScrollbar": { "enabled": true },
                        "trendLines": [],
                        "graphs": [
                            {
                                "id": "AmGraph-1",
                                "valueAxis": "v2",
                                "bullet": "round",
                                "bulletBorderAlpha": 1,
                                "bulletColor": "#2f4074",
                                "bulletSize": 5,
                                "hideBulletsCount": 50,
                                "lineThickness": 2,
                                "lineColor": "#2f4074",
                                "type": "smoothedLine",
                                "title": "Current Profit(%)",
                                "useLineColorForBulletBorder": true,
                                "valueField": "HistoryDetails.Close",
                                "balloonText": "<b>[[title]]</b><br><span style='font-size:14px'><b>[[value]]</b></span>",
                            }
                        ],
                        "guides": [],
                        "allLabels": [],
                    }
                );
    }
    catch (ex)
    {
        console.log("GetBenchmarkHistoryDataSuccess :: " + ex);
    }
};

function GetBenchmarkHistoryDataError(response) {

};
