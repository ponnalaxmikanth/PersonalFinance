var sipstable = '';
var currentView = -1;
var portfolioId = -1;
var investmentDetails = '';
var individualInvests = [];
var investPerfDetails = '';
var sectorData = [];

$(function () {
    InitializeControls();
    GetDashboardData(-1);
    GetDashboardChartData();
    GetLastProcessedDetails();
});

function InitializeControls() {
    $('.datepicker').datepicker();

    var myDate = new Date();
    var todayDate = (myDate.getMonth() + 1) + '/' + myDate.getDate() + '/' + myDate.getFullYear();
    $('[data-select-graph="to-date"]').val(moment(new Date(todayDate)).format("MM/DD/YYYY"));

    $('[data-select-graph="from-date"]').val(moment(new Date(new Date().getFullYear() - 3, new Date().getMonth() + 1, 1)).format("MM/DD/YYYY"));
};

function GetDashboardData(portfolioId) {
    var myDate = new Date();
    var todayDate = (myDate.getMonth() + 1) + '/' + myDate.getDate() + '/' + myDate.getFullYear();

    var request = {
        request: {
            PortfolioId: portfolioId,
            FromDate: '01/01/2008',
            ToDate: todayDate
        }
    };

    $.ajax({
        url: '/MFDashboard/GetDashboardData',
        type: 'post',
        dataType: 'json',
        data: JSON.stringify(request),
        contentType: "application/json; charset=utf-8",
        success: GetDashboardDataSuccess,
        error: GetDashboardDataError
    });
};

function GetDashboardDataSuccess(response) {
    if (response !== null && response !== undefined) {
        GenerateSips(response.SipDetails);
    }

    if (response !== null && response !== undefined) {
        SetInvesetments(response);
    }
};

function GetDashboardDataError(response) {
    console.log(JSON.stringify(response));
};

$(document).on('click', '[data-select-graph="get-data"]', function () {
    GetDashboardChartData();
});

function GetDashboardChartData() {
    var myDate = new Date();
    var todayDate = (myDate.getMonth() + 1) + '/' + myDate.getDate() + '/' + myDate.getFullYear();

    var request = {
        request: {
            PortfolioId: $('[data-select="graph-portfolios"]').find(':selected').attr('id'),
            FromDate: $('[data-select-graph="from-date"]').val(),
            ToDate: $('[data-select-graph="to-date"]').val()
        }
    };

    console.log('getting chart data: ' + JSON.stringify(request));

    $.ajax({
        url: '/MFDashboard/GetDashboardChartData',
        type: 'post',
        dataType: 'json',
        data: JSON.stringify(request),
        contentType: "application/json; charset=utf-8",
        success: GetDashboardChartDataSuccess,
        error: GetDashboardChartDataError
    });
};

function GetLastProcessedDetails() {
    $.ajax({
        url: '/MutualFunds/GetLastProcessedDetails',
        type: 'GET',
        dataType: 'json',
        //data: JSON.stringify(request),
        //contentType: "application/json; charset=utf-8",
        success: GetLastProcessedDetailsSuccess,
        error: GetLastProcessedDetailsError
    });
};

function GetLastProcessedDetailsSuccess(data) {
    try {
        if (data != null && data != undefined && data != '') {
            console.log(JSON.stringify(data));
            $('#lastProcessed').text('Last Processed: ' + data);
        }
    }
    catch (ex) {

    }
};

function GetLastProcessedDetailsError(data) {
    try {
        console.log(JSON.stringify(data));
    }
    catch (ex) {

    }
};

function GetDashboardChartDataSuccess(response) {
    if (response != null && response != undefined) {
        GenerateGraph(response);
    }
};

function GetDashboardChartDataError(response) {
    console.log(JSON.stringify(response));
};

$(document).on('click', '[data-select-portfolio]', function () {
    var id = $(this).attr('data-select-portfolio');
    $('[data-invest-filter]').addClass('hide');
    $('[data-select-portfolio]').removeClass('selected');
    $(this).addClass('selected');
    GetDashboardView(id);
    currentView = id;
    if (currentView == -1 || (currentView != -1 && id == -1)) {
        //currentView = id;
        //GetDashboardView(id);
    }
    //else if (id != -1) {
    //    GetDashboardData(id);
    //    GetIndividualInvestments(id);
    //    GetPerfOfMoreThanYear(id);
    //    GetSectorBreakup(id);
    //}
    //else {
    //    GetDashboardData(id);
    //}
    portfolioId = id;
    $("input[name=Invest][value='All']").prop("checked", true);
});

function GetDashboardView(id) {

    var myDate = new Date();
    var todayDate = (myDate.getMonth() + 1) + '/' + myDate.getDate() + '/' + myDate.getFullYear();

    var request = {
        request: {
            PortfolioId: id,
            FromDate: '01/01/2008',
            ToDate: todayDate
        }
    };

    $.ajax({
        url: '/MFDashboard/GetView',
        type: 'POST',
        data: JSON.stringify(request),
        contentType: "application/json; charset=utf-8",
        success: GetDashboardViewSuccess,
        error: GetDashboardViewError
    });
};

function GetDashboardViewSuccess(response) {

    $('[data-container="view"]').html(response);

    InitializeControls();
    GetDashboardData(portfolioId);

    if (portfolioId == -1) {
        GetDashboardChartData();
    }
    else {
        GetIndividualInvestments(portfolioId);
        GetSectorBreakup(portfolioId);
        GetPerfOfMoreThanYear(portfolioId);
    }
        if(portfolioId == 1)
            GetULIPValue();
};

function GetDashboardViewError(response) {
    console.log('getview error');
};

function GetULIPValue()
{
    $.ajax({
        url: '/MFDashboard/GetULIPValue',
        type: 'POST',
        data: '',
        contentType: "application/json; charset=utf-8",
        success: GetULIPValueSuccess,
        error: GetULIPValueError
    });
};

function GetULIPValueSuccess(response)
{
    try{
        if(response != undefined || response != null)
        {
            var profit = ((parseFloat(response.CurrentValue) - parseFloat(response.Invest)) * 100)/parseFloat(response.Invest);
            $('[data-select="ULIP"]').html(response.Invest.toLocaleString() + ' (' + profit.toLocaleString() + ' %)');
        }
    }
    catch(ex)
    {

    }
};

function GetULIPValueError(response)
{
};

function SetInvesetments(investments) {
    try {
        $('[data-select="investment"]').html(investments.Investments.Investment.toLocaleString());

        $('[data-select="current-value"]').html(investments.Investments.CurrentValue.toLocaleString());
        $('[data-select="profit"]').html(investments.Investments.Profit.toLocaleString() + ' (' + investments.Investments.ProfitPer + '%)');
        //$('[data-select="profitper"]').html(' (' + investments.Investments.ProfitPer + '%)');

        $('[data-select="YTD"]').html(investments.YTD.Amount.toLocaleString() + ' (' + investments.YTD.ProfitPer + '%)');
        $('[data-select="QTD"]').html(investments.QTD.Amount.toLocaleString() + ' (' + investments.QTD.ProfitPer + '%)');
        $('[data-select="MTD"]').html(investments.MTD.Amount.toLocaleString() + ' (' + investments.MTD.ProfitPer + '%)');
        //data-select="YTD"
    }
    catch (ex) {

    }
};

function GenerateSips(sipDetails) {
    if (sipstable !== '')
        sipstable.destroy();

    sipstable = $('#upcoming-sips').DataTable(
        {
            //"headers": ["Total (not column total)", ],
            //"lengthMenu": [[25, 50, 100, 150, 200, -1], [25, 50, 100, 150, 200, "All"]],
            //"iDisplayLength": -1,
            colReorder: true,
            //"dom": '<"top"iflp<"clear">>rt<"datatable-scroll"t><"bottom"iflp<"clear">>',
            //"language": {
            //    "lengthMenu": "Show _MENU_ records",
            //    "info": "Showing _START_ to _END_ of _TOTAL_ records",
            //},
            searching: false,
            data: sipDetails,
            columns: [
                { "data": "NextSipDate", title: "Next Sip Date", mRender: function (data, type, full) { return moment(new Date(data)).format("YYYY-MM-DD"); } },
                { "data": "PortfolioName", title: "Port Folio" },
                { "data": "SipStartDate", title: "Start Date", mRender: function (data, type, full) { return moment(new Date(data)).format("YYYY-MM-DD"); } },
                { "data": "SipEndDate", title: "End Date", mRender: function (data, type, full) { return moment(new Date(data)).format("YYYY-MM-DD"); } },
                { "data": "FundDetails.FundName", title: "Fund Name" },
                { "data": "Investment.SIPAmount", title: "SIP Amount", mRender: function (data, type, full) { return data.toLocaleString(); } },
                { "data": "Investment.Investment", title: "Investment", mRender: function (data, type, full) { return data.toLocaleString(); } },
                {
                    "data": null, title: "Profit", mRender: function (data, type, full) {
                        return parseFloat((parseFloat(full.Investment.Units) * parseFloat(full.FundDetails.NAV)) - (parseFloat(full.Investment.Units) * parseFloat(full.Investment.AvgNAV))).toLocaleString();
                    }
                },
                {
                    "data": null, title: "Profit (%)", mRender: function (data, type, full) {
                        return (parseFloat(parseFloat((parseFloat(full.Investment.Units) * parseFloat(full.FundDetails.NAV)) - (parseFloat(full.Investment.Units) * parseFloat(full.Investment.AvgNAV)))
                            / (parseFloat(full.Investment.Units) * parseFloat(full.Investment.AvgNAV))) * 100).toFixed(3);
                    }
                },
                { "data": "FundDetails.NAV", title: "NAV" },
                { "data": "Investment.AvgNAV", title: "Avg NAV", mRender: function (data, type, full) { return data.toLocaleString(); } },
                { "data": "Investment.Dividend", title: "Dividend", mRender: function (data, type, full) { return data.toLocaleString(); } },
                //{ "data": "Investment.Units", title: "Units" },
            ]
        });

    $('#upcoming-sips_length').addClass('hide');
    $('#upcoming-sips_paginate').addClass('hide');

    $('.dataTables_length').addClass('hide');
    $('.dataTables_paginate paging_simple_numbers').addClass('hide');
};

function GenerateGraph(response) {
    try {
        var res = [];

        for (i = 0; i < response.length; i++) {
            res.push(
                {
                    "category": moment(new Date(response[i].Date)).format("MM/YY"),
                    "Investment": response[i].Investments.Investment,
                    "CurrentValue": response[i].Investments.CurrentValue,
                    "CurrentProfit": response[i].Investments.CurrentProfit,
                    "Dividend": response[i].Investments.Dividend,
                    "RedeemInvest": response[i].Investments.RedeemInvest,
                    "RedeemValue": response[i].Investments.RedeemValue,
                    "Profit": response[i].Investments.Profit,
                    "ProfitPer": response[i].Investments.ProfitPer,
                });
        }
        GenerateChartNew(res);
    }
    catch (ex) {

    }
};

function GenerateChartNew(data) {
    AmCharts.makeChart("chartdiv",
            {
                "type": "serial",
                "theme": "light",
                "legend": { "maxColumns": 10, "position": "top", "useGraphSettings": true, "markerSize": 10, "divId": "legenddiv" },//"useGraphSettings": true, "position": "top"
                "dataProvider": data,
                "valueAxes": [
                                { "id": "v1", "title": "Amount", "position": "left", "autoGridCount": false, "axisThickness": 2, "axisAlpha": 1, },
                                { "id": "v2", "title": "", "position": "right", "autoGridCount": false, "gridAlpha": 0, },
                                { "id": "v3", "title": "Redeem", "position": "left", "autoGridCount": false, "axisThickness": 2, "axisAlpha": 1, "offset": 50, },
                ],
                //"panels": [{
                //    "title": "Volume",
                //    "percentHeight": 30,
                //    "stockGraphs": [{
                //        "valueField": "volume",
                //        "type": "column",
                //        "showBalloon": false,
                //        "fillAlphas": 1
                //    }],
                //    "stockLegend": {
                //        "periodValueTextRegular": "[[value]]%"
                //    }
                //}],
                "categoryField": "category",
                "chartCursor": { "pan": false, "valueLineEnabled": true, "valueLineBalloonEnabled": true, "cursorAlpha": 0, "valueLineAlpha": 0.2 },
                //"startDuration": 1, "position": "top", "usePrefixes": true,
                //"categoryAxis": { "gridPosition": "start", "labelRotation": 45 },
                "categoryAxis": { "gridPosition": "start" },
                "valueScrollbar": { "enabled": true },
                "chartScrollbar": { "enabled": true },
                "trendLines": [],
                "graphs": [
                    {
                        //"balloonText": "[[title]] of [[category]]:[[value]]",
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
                        ////"lineColor": "#913167",
                        //"lineColor": "#f442c5",
                        ////"lineColor": "#00cc99",
                        "lineColor": "#84b761",
                        "type": "smoothedLine",
                        "title": "Current Value",
                        "useLineColorForBulletBorder": true,
                        "valueField": "CurrentValue",
                        //"balloonText": "[[title]] of [[category]]:[[value]]",
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
                        "negativeLineColor": "#d1655d",
                        "type": "smoothedLine",
                        "title": "Current Profit(%)",
                        "useLineColorForBulletBorder": true,
                        "valueField": "CurrentProfit",
                        "label": "CurrentProfit",
                        //"balloonText": "[[title]] of [[category]]:[[value]]",
                        "balloonText": "<b>[[title]]</b><br><span style='font-size:14px'><b>[[value]]</b></span>",
                    },
                    {
                        //"balloonText": "[[title]] of [[category]]:[[value]]",
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
                        //"balloonText": "[[title]] of [[category]]:[[value]]",
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
                        //"clustered": false,
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
                        //"balloonText": "[[title]] of [[category]]:[[value]]",
                        "balloonText": "<b>[[title]]</b><br><span style='font-size:14px'><b>[[value]]</b></span>",
                    }
                ],
                "guides": [],
                "allLabels": [],
                //"balloon": {
                //    "borderThickness": 1,
                //    "shadowAlpha": 0
                //},
            }
        );
};

function GenerateChart(data) {
    AmCharts.makeChart("chartdiv",
            {
                "type": "serial",
                "categoryField": "category",
                "angle": 30,
                "depth3D": 30,
                "colors": [
                    "#84b761",
                    "#67b7dc",
                    "#cc4748",
                    "#fdd400",
                    "#cd82ad",
                    "#2f4074",
                    "#448e4d",
                    "#b7b83f",
                    "#b9783f",
                    "#b93e3d",
                    "#913167"
                ],
                "startDuration": 1,
                "theme": "light",
                "categoryAxis": {
                    "gridPosition": "start",
                    "labelRotation": 45
                },
                "valueScrollbar": {
                    "enabled": true
                },
                "chartScrollbar": {
                    "enabled": true
                },
                "trendLines": [],
                "graphs": [
                    {
                        "balloonText": "[[title]] of [[category]]:[[value]]",
                        "fillAlphas": 1,
                        "id": "AmGraph-1",
                        "title": "Investment",
                        "type": "column",
                        "valueField": "Investment"
                    },
                    {
                        "balloonText": "[[title]] of [[category]]:[[value]]",
                        "fillAlphas": 1,
                        "id": "AmGraph-2",
                        "title": "Dividend",
                        "type": "column",
                        "valueField": "Dividend"
                    },
                    {
                        "balloonText": "[[title]] of [[category]]:[[value]]",
                        "fillAlphas": 1,
                        "id": "AmGraph-3",
                        "title": "Redeem",
                        "type": "column",
                        "valueField": "Redeem"
                    }
                ],
                "guides": [],
                //"valueAxes": [
                //    {
                //        "id": "ValueAxis-1",
                //        "title": "Axis title"
                //    }
                //],
                "allLabels": [],
                "balloon": {},
                "legend": {
                    "enabled": true,
                    "useGraphSettings": true
                },
                //"titles": [
                //    {
                //        "id": "Title-1",
                //        "size": 15,
                //        "text": "Chart Title"
                //    }
                //],
                "dataProvider": data
                //"dataProvider": [
                //    {
                //        "category": "Jan-17",
                //        "Investment": 8,
                //        "Dividend": 5,
                //        "Redeem": 10
                //    },
                //    {
                //        "category": "Feb-17",
                //        "Investment": 6,
                //        "Dividend": 7,
                //        "Redeem": 0
                //    },
                //    {
                //        "category": "Mar-17",
                //        "Investment": 2,
                //        "Dividend": 3,
                //        "Redeem": 5
                //    }
                //]
            }
        );
};

function InvestChanged($this, val)
{
    try {
        //console.log($('input[name=Invest]:checked').attr("data-select"));
        if (val == 'A') {
            GenerateInvestmentDetails(individualInvests);
        }
        else {
            var filterResult = individualInvests.filter(function (invest) {
                return invest.Type == val;
            });
            GenerateInvestmentDetails(filterResult);
        }
    }
    catch (ex)
    {

    }
};

function GetIndividualInvestments(portfolioId) {
    var myDate = new Date();
    var todayDate = (myDate.getMonth() + 1) + '/' + myDate.getDate() + '/' + myDate.getFullYear();

    var request = {
        request: {
            PortfolioId: portfolioId,
            FromDate: '01/01/2008',
            ToDate: todayDate,
            Type: $('input[name=Invest]:checked').val()
        }
    };

    $.ajax({
        url: '/MFDashboard/GetIndividualInvestments',
        type: 'post',
        dataType: 'json',
        data: JSON.stringify(request),
        contentType: "application/json; charset=utf-8",
        success: GetIndividualInvestmentsSuccess,
        error: GetIndividualInvestmentsError
    });
};

function GetIndividualInvestmentsSuccess(response) {
    try {
        individualInvests = response;
        GenerateInvestmentDetails(response);
        $("input[name=Invest][value='Invest']").prop("checked", true);
        InvestChanged(this, 'I');
        $('[data-invest-filter]').removeClass('hide');
    }
    catch (ex) {
    }
};

function GenerateInvestmentDetails(response) {
    if (investmentDetails !== '')
        investmentDetails.destroy();

    investmentDetails = $('#investments').DataTable(
        {
            scrollY: "280px",
            scrollCollapse: true,
            paging: false,
            colReorder: true,
            data: response,
            order: [[0, "desc"]],
            info: true,
            searching: true,
            //dom: '<"top"iflp<"clear">>rt<"bottom"iflp<"clear">>',
            dom: '<"top"<"clear">>rt<"bottom"iflp<"clear">>',
            //dom: '<lfip><"clear">',
            iDisplayLength : 10,
            columns: [
                { "data": "Date", title: "Date", mRender: function (data, type, full) { return moment(new Date(data)).format("YYYY-MM-DD"); } },
                { "data": "FundName", title: "Fund Name", class:"text-nowrap" },
                { "data": "Type", title: "Type" },
                { "data": "Investment", title: "Invest", mRender: function (data, type, full) { return data.toLocaleString(); } },
                { "data": "CurrentValue", title: "Current", mRender: function (data, type, full) { return data.toLocaleString(); } },
                { "data": "Profit", title: "Profit(%)", mRender: function (data, type, full) { return data.toLocaleString() + ' %'; } },
                { "data": "AgePer", title: "Age(%)", mRender: function (data, type, full) { return data.toLocaleString() + ' %'; } },
                //{
                //    "data": null, title: "Profit (%)", mRender: function (data, type, full) {
                //        if (full.Type == "S") return "0 %";
                //        else if (full.Type == "I" || full.Type == "R") {
                //            return parseFloat(((parseInt(full.CurrentValue) - parseInt(full.Investment)) * 100) / parseInt(full.Investment)).toLocaleString() + " %";
                //        }
                //}},
                //{
                //    "data": "Investment", title: "Investment", mRender: function (data, type, full) {
                //        if (full.Investment == "0")
                //            return full.Investment.toLocaleString();
                //        else
                //            return full.Investment.toLocaleString() + ' (' + full.Profit.toLocaleString() + '%)';
                //    }
                //},
                //{
                //    "data": "RedeemInvest", title: "Redeem", mRender: function (data, type, full) {
                //        //return data.toLocaleString();
                //        if (full.Investment != "0")
                //            return full.RedeemInvest.toLocaleString();
                //        else
                //            return full.RedeemInvest.toLocaleString() + ' (' + full.Profit.toLocaleString() + '%)';
                //    }
                //},
                //{ "data": "AgePer", title: "% Age", mRender: function (data, type, full) { return data.toLocaleString() + '%'; } },
            ]
        });

    //$('#investments_length').addClass('hide');
    //$('#investments_paginate').addClass('hide');

    //$('.dataTables_length').addClass('hide');
    //$('.dataTables_paginate paging_simple_numbers').addClass('hide');
};

function GetIndividualInvestmentsError(response) {
    try {
        console.log('GetIndividualInvestmentsError');
    }
    catch (ex) {
    }
};

function GetPerfOfMoreThanYear(portfolioId) {
    var myDate = new Date();
    var todayDate = (myDate.getMonth() + 1) + '/' + myDate.getDate() + '/' + myDate.getFullYear();

    var request = {
        request: {
            PortfolioId: portfolioId,
            FromDate: '01/01/2008',
            ToDate: todayDate,
            Type: $('input[name=Invest]:checked').val()
        }
    };

    $.ajax({
        url: '/MFDashboard/GetPerfOfMoreThanYear',
        type: 'post',
        dataType: 'json',
        data: JSON.stringify(request),
        contentType: "application/json; charset=utf-8",
        success: GetPerfOfMoreThanYearSuccess,
        error: GetPerfOfMoreThanYearError
    });
};

function GetPerfOfMoreThanYearSuccess(response) {
    try {
        GetPerfOfMoreThanYearDetails(response);
    }
    catch (ex) {
    }
};

function GetPerfOfMoreThanYearError(response) {
    try {
        console.log('GetPerfOfMoreThanYearError');
    }
    catch (ex) {
    }
};

function GetPerfOfMoreThanYearDetails(response) {
    if (investPerfDetails !== '')
        investPerfDetails.destroy();
    try {
        var searching = false;
        if (response != null && response != undefined && response.length > 10)
            searching = true;
        investPerfDetails = $('[data-select="invest-perf-table"]').DataTable(
            {
                "scrollY": "200px",
                "scrollCollapse": true,
                "paging": false,
                colReorder: true,
                data: response,
                "order": [[6, "asc"]],
                fixedHeader: { header: true, footer: false },
                info: false,
                //"dom": '<"top"iflp<"clear">>rt<"bottom"iflp<"clear">>',
                searching: searching,
                //"fnFooterCallback": function( nFoot, aData, iStart, iEnd, aiDisplay ) {
                //    //nFoot.getElementsByTagName('th')[0].innerHTML = "Starting index is "+iStart;
                //},
                //"oLanguage": { "sSearch": "Search all columns:" },
                //"fnInitComplete": function (oSettings, json) {
                //    $("tfoot th").each(function (i) {
                //        this.innerHTML = fnCreateSelect(oTable.fnGetColumnData(i));
                //        $('select', this).change(function () { oTable.fnFilter($(this).val(), i); });
                //    });
                //},
                columns: [
                    { "data": "Date", title: "Date", class: "", mRender: function (data, type, full) { return moment(new Date(data)).format("YYYY-MM-DD"); } },
                    { "data": "FundName", title: "Fund Name", class: "text-nowrap" },
                    { "data": "Investment", title: "Investment", class: "text-center text-right", mRender: function (data, type, full) { return data.toLocaleString(); } },
                    { "data": "CurrentValue", title: "Current Value", class: "text-center text-right", mRender: function (data, type, full) { return data.toLocaleString(); } },
                    { "data": null, title: "Profit", class: "text-center text-right", mRender: function (data, type, full) { return parseInt(parseInt(full.CurrentValue) - parseInt(full.Investment)).toLocaleString(); } },
                    { "data": "Profit", title: "Profit (%)", class: "text-center text-right", mRender: function (data, type, full) { return data.toLocaleString() + ' %'; } },
                    { "data": "AgePer", title: "Age (%)", class: "text-center text-right", mRender: function (data, type, full) { return data.toLocaleString() + '%'; } },
                    {
                        "data": null, title: "Age (Days)", class: "text-center text-right", mRender: function (data, type, full) {
                            var myDate = new Date();
                            var todayDate = (myDate.getMonth() + 1) + '/' + myDate.getDate() + '/' + myDate.getFullYear();
                            return moment(todayDate).diff(moment(new Date(full.Date)), 'days');
                        }
                    },
                ]
            });

        //$('#investperftable').dataTable().fnDestroy();-
        //$("#investperftable").append($('<tfoot/>').append($("#investperftable thead tr").clone()));
        //var table = $('#investperftable').DataTable();

        //$('#investperftable tfoot th').each(function () {
        //    var title = $(this).text();
        //    $(this).html('<input type="text" placeholder="Search ' + title + '" />');
        //});

        //table.columns().every(function () {
        //    var that = this;
        //    $('input', this.footer()).on('keyup change', function () {
        //        if (that.search() !== this.value) {
        //            that.search(this.value).draw();
        //            //console.log(this.value);
        //        }
        //    });
        //});

        //$('#investperftable thead th').each(function () {
        //    var title = $('#investperftable tfoot th').eq($(this).index()).text();
        //    $(this).html('&amp;lt;input type=&amp;quot;text&amp;quot; placeholder=&amp;quot;Search ' + title + '&amp;quot; /&amp;gt;');
        //});
        //investPerfDetails.columns().flatten().each(function (colIdx) {
        //    // Create the select list and search operation
        //    var select = $('<select />').appendTo(investPerfDetails.column(colIdx).footer())
        //        .on('change', function () { investPerfDetails.column(colIdx).search($(this).val()).draw(); });

        //    // Get the search data for the first column and add to the select list
        //    investPerfDetails.column(colIdx).cache('search').sort().unique().each(function (d) {
        //            select.append($('<option value="' + d + '">' + d + '</option>'));
        //        });
        //});
    }
    catch (ex) {
    }
};

$(document).on('change', '[data-portfolio="sector"]', function () {
    GeneratePieChart();
});

function GetSectorBreakup(portfolioId) {
    var myDate = new Date();
    var todayDate = (myDate.getMonth() + 1) + '/' + myDate.getDate() + '/' + myDate.getFullYear();

    var request = {
        request: {
            PortfolioId: portfolioId,
            FromDate: '01/01/2008',
            ToDate: todayDate
        }
    };

    $.ajax({
        url: '/MFDashboard/GetSectorBreakup',
        type: 'post',
        dataType: 'json',
        data: JSON.stringify(request),
        contentType: "application/json; charset=utf-8",
        success: GetSectorBreakupSuccess,
        error: GetSectorBreakupError
    });
};

function GetSectorBreakupSuccess(response) {
    //portfolio-sector

    var res = [];

    //for (i = 0; i < response.length; i++) {
    //    res.push(
    //        {
    //            "Sector": response[i].Sector, "Amount": response[i].Amount, "CurrentValue": response[i].CurrentValue,
    //            //"InvestPer": response[i].InvestPer, "CurrentPer": response[i].CurrentPer,
    //        });
    //}

    sectorData = response;
    GeneratePieChart();
};

function GetSectorBreakupError(response) {

};

function GeneratePieChart() {

    //AmCharts.makeChart("portfolio-sector", {
    //    "type": "pie",
    //    "theme": "light",
    //    "dataProvider": data,
    //    "labelText": "[[title]]: [[value]]",
    //    "balloonText": "[[title]]: [[value]]",
    //    "titleField": "Sector",
    //    "valueField": "InvestPer",
    //    "outlineColor": "#FFFFFF",
    //    "outlineAlpha": 0.8,
    //    "outlineThickness": 2,
    //});

    //AmCharts.makeChart("portfoliosector", {
    //    "type": "pie", "theme": "light", "startDuration": 0, "addClassNames": true,
    //    "legend": { "position": "right", "marginRight": 100, "autoMargins": false },
    //    "innerRadius": "30%",
    //    "defs": {
    //        "filter": [{
    //            "id": "shadow", "width": "200%", "height": "200%",
    //            "feOffset": { "result": "offOut", "in": "SourceAlpha", "dx": 0, "dy": 0 },
    //            "feGaussianBlur": { "result": "blurOut", "in": "offOut", "stdDeviation": 5 },
    //            "feBlend": { "in": "SourceGraphic", "in2": "blurOut", "mode": "normal" }
    //        }]
    //    },
    //    "dataProvider": data, "valueField": "InvestPer", "titleField": "Sector", "balloon": { "fixedPosition": true },
    //"export": { "enabled": true }
    //});

    AmCharts.makeChart("portfoliosector", {
        "type": "pie", "theme": "light", labelsEnabled: true, autoMargins: true,
        "labelText": "[[percents]]%", //"labelRadius": -35, //"balloonText": "[[title]]: [[value]]",
        marginTop: 20, marginBottom: 20, marginLeft: 0, marginRight: 0, pullOutRadius: 5,// "startDuration": 0, "addClassNames": true,
        "legend": { "position": "bottom", "marginRight": 10, "autoMargins": true },
        "balloon": { "fixedPosition": true },
        "listeners": [{
            "event": "init",
            "method": function(event) {
                var chart = event.chart;
                if (chart.labelColorField === undefined)
                    chart.labelColorField = "labelColor";
                for(var i = 0; i < chart.chartData.length; i++) { chart.dataProvider[i][chart.labelColorField] = chart.chartData[i].color; }
                chart.validateData();
                chart.animateAgain();
            }
        }],
        "innerRadius": "0%",
        //"defs": {
        //    "filter": [{
        //        "id": "shadow", "width": "200%", "height": "200%",
        //        //"feOffset": { "result": "offOut", "in": "SourceAlpha", "dx": 0, "dy": 0 },
        //        //"feGaussianBlur": { "result": "blurOut", "in": "offOut", "stdDeviation": 5 },
        //        //"feBlend": { "in": "SourceGraphic", "in2": "blurOut", "mode": "normal" }
        //    }]
        //},
        "dataProvider": sectorData, "valueField": $('[data-portfolio="sector"] option:selected').val(), "titleField": "Sector"//, "balloon": { "fixedPosition": true },
    });
};
