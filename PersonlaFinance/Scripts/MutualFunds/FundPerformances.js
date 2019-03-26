var FundPerformance = "";


$(function () {

    $.ajax({
        url: '/MutualFunds/GetFundPerformances',
        type: 'GET',
        success: LoadFundPerformances
    });

    function LoadFundPerformances(data) {
        console.log(JSON.stringify(data));
    };

});