bnmrkPerfTable = '';
$(function () {
    $('[data-select="FromDate"]').datepicker();
    $('[data-select="ToDate"]').datepicker();

    
    var myDate = new Date();
    var todayDate = (myDate.getMonth() + 1) + '/' + myDate.getDate() + '/' + myDate.getFullYear();

    var fromDate = moment(new Date()).subtract(365, 'days').format('L');

    $('[data-select="FromDate"]').val(fromDate);
    $('[data-select="ToDate"]').val(todayDate);

    $('.datepicker').datepicker();

   

    GetBenchmarkHistoryData();


    

    function GetbenchMarkHistorydData(response) {
        console.log(response);

        if (bnmrkPerfTable !== '')
            bnmrkPerfTable.destroy();

        bnmrkPerfTable = $('#BenchmarkPerformance').DataTable(
        {
            "headers": ["Total (not column total)", ],
            "lengthMenu": [[25, 50, 100, 150, 200, -1], [25, 50, 100, 150, 200, "All"]],
            "iDisplayLength": -1,
            colReorder: true,
            scrollY: '68vh',
            scrollCollapse: true,
            paging: false,
            "dom": '<"top"iflp<"clear">>rt<"datatable-scroll"t><"bottom"iflp<"clear">>',
            "language": {
                "lengthMenu": "Show _MENU_ records",
                "info": "Showing _START_ to _END_ of _TOTAL_ records",
            },
            data: response,
            columns: [
                    { data: "Benchmark", title: "Benchmark" },
                    { data: "Latest.Date", title: "Latest Date", mRender: function (data, type, full) { return moment(new Date(data)).format("DD-MMM-YYYY"); } },
                    { data: "Latest.Close", title: "Latest Close", mRender: function (data, type, full) { return parseFloat(data).toFixed(3); } },
                    {
                        data: null, title: "Latest/High %",
                        mRender: function (data, type, full) {
                            return parseFloat((parseFloat(full.Latest.Close) - parseFloat(full.High.Close)) * 100 / parseFloat(full.High.Close)).toFixed(3) + ' %';
                        }
                    },
                    {
                        data: null, title: "Latest/Low %",
                        mRender: function (data, type, full) {
                            return parseFloat((parseFloat(full.Latest.Close) - parseFloat(full.Low.Close)) * 100 / parseFloat(full.Low.Close)).toFixed(3) + ' %';
                        }
                    },
                    { data: "High.Date", title: "High Date", mRender: function (data, type, full) { return moment(new Date(data)).format("DD-MMM-YYYY"); } },
                    { data: "High.Close", title: "High Close", mRender: function (data, type, full) { return parseFloat(data).toFixed(3); } },
                    { data: "Low.Date", title: "Low Date", mRender: function (data, type, full) { return moment(new Date(data)).format("DD-MMM-YYYY"); } },
                    { data: "Low.Close", title: "Low Close", mRender: function (data, type, full) { return parseFloat(data).toFixed(3); } },
                ]
        });
    };

    function GetbenchMarkHistoryDataError(response) {
        console.log(response);
    };

    function GetBenchmarkHistoryData(response) {

        var request = {
            FromDate: $('[data-select="FromDate"]').val(),
            ToDate: $('[data-select="ToDate"]').val()
        };

        $.ajax({
            url: '/MFDashboard/GetBenchMarks',
            type: 'post',
            dataType: 'json',
            data: JSON.stringify(request),
            contentType: "application/json; charset=utf-8",
            success: GetbenchMarkHistorydData,
            error: GetbenchMarkHistoryDataError
        });
    };

    $(document).on('click', '[data-button="View"]', function () {
        GetBenchmarkHistoryData();
    }); 

    

});