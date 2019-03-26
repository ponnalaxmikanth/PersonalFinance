var MutualFundData = "";
var FundTransactionsTable = '';
var editor;
var fundId = '';
var table = '';

$(function () {
    MutualFundData = "";

    $('[data-select="FromDate"]').datepicker();
    $('[data-select="ToDate"]').datepicker();

    $('[data-select="FromDate"]').val('01/01/2008');
    var myDate = new Date();
    var todayDate = (myDate.getMonth() + 1) + '/' + myDate.getDate() + '/' + myDate.getFullYear();
    $('[data-select="ToDate"]').val(todayDate);

    $.ajax({
        url: '/MutualFunds/GetMutualFundData',
        type: 'GET',
        success: LoadMutualFundData,
        error: LoadMutualFundDataError
    });

    

    $('.datepicker').datepicker();

    function LoadMutualPortfolios(data) {
        var s = $('[data-select="Portfolios"]');

        s.empty();
        //$('<option />', { value: "Select...", text: "Select...", }).attr("selected", "selected").attr("disabled", "disabled").appendTo(s);
        for (var i = 0; i < data.length; i++) {
            $('<option />', { value: data[i].PortfolioId, text: data[i].Portfolio }).appendTo(s);
        }
        //$('[data-select="Portfolios"]').multiselect(
        //    {
        //        header: true,
        //        selectedList: 1,
        //        //selectedText: function (numChecked, numTotal, checkedItems) {
        //        //    return numChecked + ' of ' + numTotal + ' checked';
        //        //}
        //    });
        //$('[data-select="Portfolios"]').multiselect("uncheckAll");
    };

    function LoadFolios(data) {
        var s = $('[data-transaction="folios"]');
        s.empty();
        $('<option />', { value: 'Select', text: 'Select' }).appendTo(s);

        for (var i = 0; i < data.length; i++) {
            //$('<option />', { value: data[i].FolioId, text: data[i].Description }).appendTo(s);
            var ele = $('<option />', { value: data[i].FolioId, text: data[i].FolioNumber });
            $(ele).attr("data-fundhouseid", data[i].FundHouseId);
            $(ele).appendTo(s);
        }
    };

    function LoadMutualFundData(data) {
        try {
            console.log(JSON.stringify(data));

            MutualFundData = data;
            LoadMutualPortfolios(data.LstPortfolios);
            LoadFolios(data.LstFolios);
            LoadFunds(data.LstFunds, 'data-select="mf-funds"');
        }
        catch (ex) {

        }
    };

    function LoadMutualFundDataError(response)
    {
        console.log(JSON.stringify(response));
    };

    $('[data-button="MFView"]').click(function () {
        RefreshData();
    });

    $('[data-button="MFSave"]').click(function () {
        //$('#myModal').modal('hide');
        $("[data-dismiss=modal]").trigger({ type: "click" });
    });

    $('[data-button="MFAdd"]').click(function () {
        console.log('MFAdd clicked');

        //$('<option />', { value: data[i].PortfolioId, text: data[i].Portfolio }).appendTo(s);
        var row = $('<tr />');
        row.append($('<td/>').append($('<input type="text" data-select="purchase-date" data-transaciton-id="-1"/>').addClass("datepicker w-100-fixed")));//purchase date
        row.append($('<td/>').append($('<input type="text" data-select="amount"/>').addClass("w-100")));//amount
        row.append($('<td/>').append($('<input type="text" data-select="purchase-nav"/>').addClass("w-100")));//purchase nav
        row.append($('<td/>').append($('<input type="text" data-select="total-units"/>').addClass("w-100")));//total units
        row.append($('<td/>').append($('<input type="text" data-select="dividend"/>').addClass("w-100")));//dividend
        row.append($('<td/>').append($('<input type="text" data-select="actual-nav"/>').addClass("w-100")));//actual nav
        row.append($('<td/>').append($('<input type="text" data-select="sell-date"/>').addClass("datepicker w-100-fixed")));//redeem date
        row.append($('<td/>').append($('<input type="text" data-select="sell-units"/>').addClass("w-100")));//redeem units
        row.append($('<td/>').append($('<input type="text" data-select="sell-nav"/>').addClass("w-100")));//redeem nav
        row.append($('<td/>').append($('<input type="text" data-select="STT"/>').addClass("w-100")));//STT
        row.append($('<td/>').html('&nbsp;'));//profit
        row.append($('<td/>').html('&nbsp;'));//profit %
        row.append($('<td/>').append($('<input type="button" data-button="add-transaction" value="Add"/>').addClass("button")));//save button
       
        //$('#FundTransactions > tbody:last-child').append('<tr><td>abc</td></tr>');
        $('#FundTransactions > tbody:last-child').append(row);

        $('.datepicker').datepicker();
    });

});

function GetSelectedValues(element) {
    //var array_of_checked_values = $('[data-select=' + element + ']').multiselect("getChecked").map(function () {
    //    return this.value;
    //}).get().toString();

    //return array_of_checked_values;

    return $('[data-select="'+ element +'"] option:selected').val();
};

function OpenMFDetails($this) {
    try {

        $('#myModal').modal('show');
        $('.modal-title').html($($this).attr('data-fundname'));

        fundId = $($this).attr('data-fundid');//


        var fromDate = $('[data-select="FromDate"]').val();
        var ToDate = $('[data-select="ToDate"]').val();
        var portfolios = GetSelectedValues('Portfolios');

        var request = {
            getMFTransactions: {
                PortfolioId: portfolios,
                FundId: fundId,
                FromDate: fromDate,
                ToDate: ToDate
            }
        };

        console.log('Get Fund Transactions:' + JSON.stringify(request));
        $.ajax({
            url: '/MutualFunds/GetFundTransactions',
            type: 'post',
            dataType: 'json',
            data: JSON.stringify(request),
            //data: request,
            contentType: "application/json; charset=utf-8",
            success: ShowFundTransactions,
            error: ShowFundTransactionsError
        });
    }
    catch (ex) {
    }
};

function ShowFundTransactionsError(response) {
    console.log(JSON.stringify(response));
}

function ShowTransactions(data) {
    if (table !== '')
        table.destroy();

    console.log(data);
    console.log(JSON.stringify(data));
    $('[data-select="consolidate-div"]').empty();

    table = $('#MFTransactions').DataTable(
        {
            "headers": ["Total (not column total)", ],
            "lengthMenu": [[25, 50, 100, 150, 200, -1], [25, 50, 100, 150, 200, "All"]],
            "iDisplayLength": -1,
            scrollY: "400px",
            scrollCollapse: true,
            colReorder: true,
            "dom": '<"top"iflp<"clear">>rt<"datatable-scroll"t><"bottom"iflp<"clear">>',
            "language": {
                "lengthMenu": "Show _MENU_ records",
                "info": "Showing _START_ to _END_ of _TOTAL_ records",
            },
            //"order": [[0, "desc"]],
            data: data,
            columns: [
                        {
                            "data": "FundName", title: "Fund", width: "500px", "fnCreatedCell": function (nTd, sData, oData, iRow, iCol) {
                                $(nTd).html("<a href='javascript:;' onclick='javascript:OpenMFDetails(this);' data-fundname='" + oData.FundName + "' data-fundid='" + oData.FundId + "'>" + oData.FundName + "</a>");
                            }
                        },
                        { "data": "Amount", title: "Amount", mRender: function (data, type, full) { return parseFloat(data).toFixed(3) } },
                        { "data": "Dividend", title: "Dividend", mRender: function (data, type, full) { return parseFloat(data).toFixed(3) } },
                        {
                            "data": null, title: "Value",
                            mRender: function (data, type, full) { return parseFloat(((full.Units - ((full.SellUnits == null || full.SellUnits == undefined || full.SellUnits.length <= 0) ? 0 : full.SellUnits))) * full.CurrentNAV).toFixed(3); }
                        },
                        {
                            "data": null, title: "Profit",
                            mRender: function (data, type, full) { return parseFloat(((full.Units - ((full.SellUnits == null || full.SellUnits == undefined || full.SellUnits.length <= 0) ? 0 : full.SellUnits))) * full.CurrentNAV - full.Amount).toFixed(3); }
                        },
                        {
                            "data": null, title: "Profit (%)",
                            mRender: function (data, type, full) { return parseFloat((((full.Units - ((full.SellUnits == null || full.SellUnits == undefined || full.SellUnits.length <= 0) ? 0 : full.SellUnits))) * full.CurrentNAV - full.Amount) * 100 / full.Amount).toFixed(3); }
                        },
                        { "data": "Units", title: "Total Units", mRender: function (data, type, full) { return parseFloat(data).toFixed(3) } },
                        { "data": "PurchaseNAV", title: "NAV (Avg)", mRender: function (data, type, full) { return parseFloat(data).toFixed(3) } },
                        { "data": "CurrentNAV", title: "NAV", mRender: function (data, type, full) { return parseFloat(data).toFixed(3) } },
                        {
                            "data": "LatestNAVDate", title: "Latest NAV Date", type: "date",
                            mRender: function (data, type, full) { return moment(new Date(data)).format("DD-MMM-YYYY"); }
                        },
            ]
        });
};

function GetTransactionsError(response) {
    console.log(JSON.stringify(response));
};

function ShowFundTransactions(data) {
    if (FundTransactionsTable !== '')
        FundTransactionsTable.destroy();

    console.log(JSON.stringify(data));

    FundTransactionsTable = $('#FundTransactions').DataTable(
        {
            "headers": ["Total (not column total)", ],
            "lengthMenu": [[5, 10, 25, 50, 100, 150, 200, -1], [5, 10, 25, 50, 100, 150, 200, "All"]],
            "iDisplayLength": 10,
            "autoWidth": true,
            colReorder: true,
            "dom": '<"top"iflp<"clear">>rt<"datatable-scroll"t><"bottom"iflp<"clear">>',
            "language": {
                "lengthMenu": "Show _MENU_ records",
                "info": "Showing _START_ to _END_ of _TOTAL_ records",
            },
            "order": [[0, "desc"]],
            data: data,
            columns: [
                        {
                            data: "PurchaseDate", title: "Purchase Date", //type: "datetime", format: 'MM-DD-YYYY',
                            //"render": function (data) {
                            //    var date = new Date(data);
                            //    var month = date.getMonth() + 1;
                            //    return (month.length > 1 ? month : "0" + month) + "/" + date.getDate() + "/" + date.getFullYear();
                            //}
                            //"type": "date",
                            //"dateFormat": "yy-mm-dd",
                            mRender: function (data, type, full) { return moment(new Date(data)).format("YYYY-MM-DD"); }
                            //mRender: function (data, type, full) {
                            //    //var dtStart = moment(new Date(data)).format("DD-MMM-YYYY");
                            //    //return '<input type="text" name="textbox' + '" data-provide="datepicker' + '" class="datepicker w-100-fixed' + '" data-transaciton-id=' + full.TransactionId + ' data-select="purchase-date" value=' + dtStart + '>';
                            //    //return new Date(data);
                            //}
                            //def: function () { return new Date(data); },
                        },
                        {
                            data: "Amount", title: "Amount", className: "w-100-fixed", sType: 'numeric',
                            //mRender: function (data, type, full) {
                            //    return '<input type="text" name="textbox' + '" class="w-100"' + 'data-select="amount" value=' + data + '>';
                            //}
                        },
                        {
                            data: "Dividend", title: "Dividend", sType: 'numeric',
                            //mRender: function (data, type, full) {
                            //    return '<input type="text" name="textbox' + '" class="w-100"' + ' data-select="dividend" value=' + data + '>';
                            //}
                        },
                        {
                            data: null, title: "Current Value", className: "w-70-fixed", sType: 'numeric',
                            mRender: function (data, type, full) { return (full.LatestNAV * full.PurchaseUnits).toFixed(3); }
                        },
                        {
                            data: null, title: "Profit", sType: 'numeric',
                            mRender: function (data, type, full) {
                                //if (full.SellDate == undefined || full.SellDate.length <= 0) return "";
                                //else {
                                    //return ((parseFloat(full.SellUnits) * parseFloat(full.SellNAV)) - parseFloat(full.STT) - (parseFloat(full.PurchaseNAV) * parseFloat(full.PurchaseUnits))).toFixed(3);
                                    return (parseFloat(parseFloat(full.LatestNAV * full.PurchaseUnits) - parseFloat(full.Amount))).toFixed(3);
                                //}
                            }
                        },
                        {
                            data: null, title: "Profit (%)", className: "w-100-fixed", sType: 'numeric',
                            mRender: function (data, type, full) {
                                //if (full.SellDate == undefined || full.SellDate.length <= 0) return "";
                                //else {
                                //    return (((parseFloat(full.SellUnits) * parseFloat(full.SellNAV)) - parseFloat(full.STT)
                                //        - (parseFloat(full.PurchaseNAV) * parseFloat(full.PurchaseUnits))) * 100 / (parseFloat(full.PurchaseNAV) * parseFloat(full.PurchaseUnits))).toFixed(2) + ' %';
                                //}

                                return parseFloat(parseFloat(parseFloat(parseFloat(full.LatestNAV * full.PurchaseUnits) - parseFloat(full.Amount)) / (parseFloat(parseFloat(full.Amount)))) * 100).toFixed(3);
                            }
                        },
                        {
                            data: null, title: "% Age", className: "w-70-fixed", sType: 'numeric',
                            mRender: function (data, type, full) {
                                var todaysdate = moment(new Date());
                                var purchaseDate = moment(new Date(full.PurchaseDate));
                                var daysDiff = todaysdate.diff(purchaseDate, 'days');
                                var x = 365 / daysDiff;
                                var a = (full.LatestNAV * full.PurchaseUnits).toFixed(3) / full.Amount;

                                var val = Math.pow(a, x) - 1;
                                return parseFloat(val * 100).toFixed(2);
                            }
                        },
                        {
                            data: "PurchaseNAV", title: "Invest NAV", sType: 'numeric',
                            //mRender: function (data, type, full) {
                            //    return '<input type="text" name="textbox' + '" class="w-100"' + ' data-select="purchase-nav" value=' + data + '>';
                            //}
                        },
                        {
                            data: "ActualNAV", title: "Actual NAV", sType: 'numeric',
                            //mRender: function (data, type, full) {
                            //    return '<input type="text" name="textbox' + '" class="w-100"' + ' data-select="actual-nav" value=' + data + '>';
                            //}
                        },
                        { data: "LatestNAV", title: "Current NAV", sType: 'numeric' },
                        {
                            data: "PurchaseUnits", title: "Units", className: "w-70-fixed", sType: 'numeric',
                            //mRender: function (data, type, full) {
                            //    return '<input type="text" name="textbox' + '" class="w-100"' + ' data-select="total-units" value=' + data + '>';
                            //}
                        },
                        //{
                        //    data: "SellDate", title: "Redeemed Date",
                        //    mRender: function (data, type, full) {
                        //        if (data == undefined || data.length <= 0) {
                        //            return '<input type="text" name="textbox' + '" class="datepicker w-100-fixed"' + ' data-select="sell-date" value="">';
                        //        }
                        //        else {
                        //            dtStart = moment(new Date(data)).format("DD-MMM-YYYY");
                        //            return '<input type="text" name="textbox' + '" class="datepicker  w-100-fixed"' + ' data-select="sell-date" value=' + dtStart + '>';
                        //        }
                        //    }
                        //},
                        //{
                        //    data: "SellUnits", title: "Redeemed Units",
                        //    mRender: function (data, type, full) {
                        //        if (data == undefined || data.length <= 0) {
                        //            return '<input type="text" name="textbox' + '" class="w-100"' + ' data-select="sell-units" value="">';
                        //        }
                        //        else
                        //            return '<input type="text" name="textbox' + '" class="w-100"' + ' data-select="sell-units" value=' + data + '>';
                        //    }
                        //},
                        //{
                        //    data: "SellNAV", title: "Redeemed NAV",
                        //    mRender: function (data, type, full) {
                        //        if (data == undefined || data.length <= 0) {
                        //            return '<input type="text" name="textbox' + '" class="w-100"' + ' data-select="sell-nav" value="">';
                        //        }
                        //        else
                        //            return '<input type="text" name="textbox' + '" class="w-100"' + ' data-select="sell-nav" value=' + data + '>';
                        //    }
                        //},
                        //{
                        //    data: "STT", title: "STT",
                        //    mRender: function (data, type, full) {
                        //        if (data == undefined || data.length <= 0) {
                        //            return '<input type="text" name="textbox' + '" class="w-100"' + ' data-select="STT" value="">';
                        //        }
                        //        else
                        //            return '<input type="text" name="textbox' + '" class="w-100"' + ' data-select="STT" value=' + data + '>';
                        //    }
                        //},
                        {
                            data: null, title: "Save",
                            mRender: function (data, type, full) {
                                //return '<input type="button" name="textbox' + '" class="' + '" value="Update" data-button="update-transaction">';
                                return '<a class="' + '" value="Update" data-button="update-transaction">Update</a>';
                            }
                        },
            ],
            //buttons: [
            //{ extend: "create", editor: editor },
            //{ extend: "edit", editor: editor },
            //{ extend: "remove", editor: editor }
            //]
        });
    $('.datepicker').datepicker();
    $('#FundTransactions').css('width', '');
};

$(document).on('click', '[data-button="add-transaction"]', function () {
    console.log('add-transaction clicked');
    $this = this;
    $('[data-button-newtrans="Save"]').removeAttr("disabled");

    var row = $($this).closest('tr');

    AddorUpdateTransaction(row);

});

$(document).on('click', '[data-button-new="Transaction"]', function () {
    $('[data-button-newtrans="Save"]').removeAttr("disabled");
    $('#NewTransaction').modal('show');
    $('.modal-title').html('Add Transaction');

    ResetAddTransactionFields();

});

$(document).on('click', '[data-button-new="Dividend"]', function () {
    $('#NewDividend').modal('show');
    $('[data-button-new-dividend="Save"]').removeAttr("disabled");
    ResetAddDividendFields();
});

$(document).on('click', '[data-button-new-dividend="Save"]', function () {
    $this = this;
    if (!ValidateAddDividend()) {
        return false;
    }
    $('[data-button-new-dividend="Save"]').attr("disabled", "disabled");
    var date = $('[data-dividend="date"]').val();
    var fundId = $('[data-dividend="fund"]').val();
    var nav = $('[data-dividend="nav"]').val();
    var dividend = $('[data-dividend="dividend"]').val();
    var units = $('[data-dividend="units"]').val();
    var portfolio = GetSelectedValues('Portfolios');

    var request = {
        _mfDividendRequest: {
            FundId: fundId,
            DividendDate: date,
            NAV: nav,
            Dividend: dividend
        }
    };
    console.log(JSON.stringify(request));

    $.ajax({
        url: '/MutualFunds/AddDividend',
        type: 'post',
        dataType: 'json',
        data: JSON.stringify(request),
        //data: request,
        contentType: "application/json; charset=utf-8",
        success: RefreshData
    });
});

$(document).on('click', '[data-button="update-transaction"]', function () {
    console.log('update-transaction clicked');
    $('[data-button-new-dividend="Save"]').attr("disabled", "disabled");
    $this = this;

    var row = $($this).closest('tr');

    AddorUpdateTransaction(row);
});

$(document).on('click', '[data-button-newtrans="Save"]', function () {
    console.log('add transaction clicked');
    $this = this;

    if (!ValidateAddTransaction()) {
        return false;
    }

    $('[data-button-newtrans="Save"]').attr("disabled", "disabled");

    var purchasedate = $('[data-transaction="purchase-date"]').val();
    var amount = $('[data-transaction="amount"]').val();
    var purchasenav = $('[data-transaction="nav"]').val();

    var folios = $('[data-transaction="folios"]').val();
    var fundId = $('[data-transaction="funds"]').val();

    var sellunits = $('[data-transaction="units"]').val();
    var STT = $('[data-transaction="STT"]').val();

    var isSip = $('[data-transaction="sip"]').is(':checked');

    var PortfolioId = GetSelectedValues('Portfolios');

    var request = {
        _mfTransactionRequest: {
            TransactionType: $('[data-transaction="type"]').val(),
            TransactionId: -1,
            FromDate: $('[data-select="FromDate"]').val(),
            ToDate: $('[data-select="ToDate"]').val(),
            PortfolioId: PortfolioId,
            FolioId: folios,
            FundId: fundId,
            PurchaseDate: purchasedate,
            Amount: amount,
            PurchaseNAV: purchasenav,
            SellDate: purchasedate,
            SellUnits: sellunits,
            STT: STT,
            IsSIP: isSip
        }
    };
    console.log(JSON.stringify(request));

    $.ajax({
        url: '/MutualFunds/AddFundTransaction',
        type: 'post',
        dataType: 'json',
        data: JSON.stringify(request),
        contentType: "application/json; charset=utf-8",
        success: RefreshData,
        error: addMFTransFailed
    });
});

$(document).on('change', '[data-transaction="type"]', function () {
    console.log('transaction type changed');
    $this = this;
    
    if ($('[data-transaction="type"]').val() == "Purchase") {
        //$('[data-transaction="sell"]').addClass('hide');
        $('[data-transaction="purchase"]').removeClass('hide');
    }
    if ($('[data-transaction="type"]').val() == "Redeem") {
        //$('[data-transaction="sell"]').removeClass('hide');
        $('[data-transaction="purchase"]').addClass('hide');
    }

});

$(document).on('change', '[data-transaction="folios"]', function () {
    console.log('folios changed');
    $this = this;
    //var fundhouseid = $this.Attr("data-fundhouseid");
    if ($('[data-transaction="folios"]').val() != "Select") {
        var fundhouseid = $('[data-transaction="folios"] option:selected').attr('data-fundhouseid');
        //var funds = data.LstFunds;
        fundhouseid = $(this).find(':selected').data('fundhouseid');
        var funds = $.grep(MutualFundData.LstFunds, function (v) {
            return v.FundHouseId === fundhouseid;
        });

        LoadFunds(funds, 'data-transaction="funds"');
    }
    else {
        LoadFunds(MutualFundData.LstFunds);
    }
});

$(document).on('click', '[data-button-new="Performance"]', function () {
    console.log('fund performances');
    $this = this;
        GetFundPerformances();
});

$(document).on('click', '[data-button-load="History"]', function () {
    console.log('load fund history');
    $this = this;
    LoadFundHistory();
});

function ValidateAddDividend() {
    try {
        var message = '';
        if ($('[data-dividend="fund"]').val() == "Select") {
            message = 'Select fund\n';
        }
        if ($('[data-dividend="nav"]').val() == '') {
            message += 'Invalid Nav\n';
        }
        if ($('[data-dividend="dividend"]').val() == '') {
            message += 'Invalid dividend\n';
        }
        if (message.length > 0) {
            alert(message);
            return false;
        }
        return true;
    }
    catch (ex) {
        console.log('Exception: ValidateAddDividend - ' + ex);
    }
};

function ResetAddDividendFields() {
    try {
        console.log('ResetAddDividendFields called');
        var myDate = new Date();
        var todayDate = (myDate.getMonth() + 1) + '/' + myDate.getDate() + '/' + myDate.getFullYear();
        $('[data-dividend="date"]').val(todayDate);

        $('[data-dividend="fund"]').val('Select');
        $('[data-dividend="nav"]').val('');
        $('[data-dividend="dividend"]').val('');
    }
    catch (ex) {
        console.log('Exception: ResetAddDividendFields - ' + ex);
    }
};

function ValidateAddTransaction() {
    try {
        var message = '';
        if ($('[data-transaction="type"]').val() == "Select")
        {
            message = 'Select valid transaction type\n';
        }
        if ($('[data-transaction="folios"]').val() == "Select")
        {
            message += 'Select valid Folio\n';
        }
        if ($('[data-transaction="funds"]').val() == "Select") //data-transaction="funds"
        {
            message += 'Select valid Fund\n';
        }
        if ($('[data-transaction="nav"]').val() == '') {
            message += 'Invalid Nav\n';
        }
        if ($('[data-transaction="type"]').val() == "Purchase" && $('[data-transaction="amount"]').val() == '')
        {
            message += 'Invalid Amount\n';
        }    
        if ($('[data-transaction="type"]').val() == "Redeem" && $('[data-transaction="units"]').val() == '')
        {
            message += 'Invalid Units\n';
        }
        if ($('[data-transaction="type"]').val() == "Redeem" && $('[data-transaction="STT"]').val() == '')
        {
            message += 'Invalid STT\n';
        }
        if (message.length > 0) {
            alert(message);
            return false;
        }
        return true;
    }
    catch (ex)
    {
        console.log('Exception: ValidateAddTransaction - ' + ex);
    }
};

function ResetAddTransactionFields() {
    try {
        console.log('ResetAddTransactionFields called');
        var myDate = new Date();
        var todayDate = (myDate.getMonth() + 1) + '/' + myDate.getDate() + '/' + myDate.getFullYear();
        $('[data-transaction="purchase-date"]').val(todayDate);

        $('[data-transaction="type"]').val('Select');
        $('[data-transaction="folios"]').val('Select');
        $('[data-transaction="funds"]').val('Select');
        $('[data-transaction="amount"]').val('');
        $('[data-transaction="nav"]').val('');
        $('[data-transaction="units"]').val('');
        $('[data-transaction="STT"]').val('');
        $('[data-transaction="sip"]').attr('checked', false);
    }
    catch (ex)
    {
        console.log('Exception: ResetAddTransactionFields - ' + ex);
    }
};

function addMFTransFailed(response)
{
    console.log(JSON.stringify(response));
};

function RefreshData(response)
{
    $('#NewTransaction').modal('hide');
    $('#NewDividend').modal('hide');

    var fromDate = $('[data-select="FromDate"]').val();
    var ToDate = $('[data-select="ToDate"]').val();
    var portfolios = GetSelectedValues('Portfolios');

    var request = {
        getMFTransactions: {
            PortfolioId: portfolios,
            FromDate: fromDate,
            ToDate: ToDate
        }
    };

    $.ajax({
        url: '/MutualFunds/GetTransactions',
        type: 'post',
        dataType: 'json',
        data: JSON.stringify(request),
        //data: request,
        contentType: "application/json; charset=utf-8",
        success: ShowTransactions,
        error: GetTransactionsError
    });
};

function AddorUpdateTransaction(row) {
    var purchasedate = $(row).find('[data-select="purchase-date"]').val();
    var amount = $(row).find('[data-select="amount"]').val();
    var purchasenav = $(row).find('[data-select="purchase-nav"]').val();
    var totalunits = $(row).find('[data-select="total-units"]').val();
    var dividend = $(row).find('[data-select="dividend"]').val();
    var actualnav = $(row).find('[data-select="actual-nav"]').val();
    var selldate = $(row).find('[data-select="sell-date"]').val();
    var sellunits = $(row).find('[data-select="sell-units"]').val();
    var sellnav = $(row).find('[data-select="sell-nav"]').val();
    var STT = $(row).find('[data-select="STT"]').val();
    var isSIP = $(row).find('[data-transaction="sip"]').is(':checked');

    var portfolios = GetSelectedValues('Portfolios');

    var request = {
        _mfTransactionRequest: {
            TransactionId: $(row).find('[data-transaciton-id]').attr('data-transaciton-id'),
            FromDate: $('[data-select="FromDate"]').val(),
            ToDate: $('[data-select="ToDate"]').val(),
            PortfolioId: GetSelectedValues('Portfolios'),
            FundId: fundId,
            PurchaseDate: purchasedate,
            Amount: amount,
            PurchaseNAV: purchasenav,
            Units: totalunits,
            Dividend: dividend,
            ActualNAV: actualnav,
            SellDate: selldate,
            SellUnits: sellunits,
            SellNAV: sellnav,
            STT: STT,

            IsSIP: isSIP
        }
    };
    console.log(JSON.stringify(request));

    $.ajax({
        url: '/MutualFunds/AddFundTransaction',
        type: 'post',
        dataType: 'json',
        data: JSON.stringify(request),
        //data: request,
        contentType: "application/json; charset=utf-8",
        success: ShowFundTransactions
    });
};

function LoadFunds(data, element) {
    //var funds = $('[data-select="mf-funds"]');
    var funds = $('[' + element + ']');
    funds.empty();
    $('<option />', { value: 'Select', text: 'Select' }).appendTo(funds);

    for (var i = 0; i < data.length; i++) {
        //$('<option />', { value: data[i].FundId, text: data[i].FundName }).appendTo(funds);
        var ele = $('<option />', { value: data[i].FundId, text: data[i].FundName });
        $(ele).attr("data-fundhouseid", data[i].FundHouseId);
        $(ele).appendTo(funds);
    }
};

function GetFundPerformances() {
    $.ajax({
        url: '/MutualFunds/GetFundPerformances',
        type: 'GET',
        success: ShowFundPerformance,
        error: ShowFundPerformanceError
    });
};

function ShowFundPerformance(data)
{
    table = $('#FundPerformance').DataTable(
        {
            "headers": ["Total (not column total)", ],
            "lengthMenu": [[25, 50, 100, 150, 200, -1], [25, 50, 100, 150, 200, "All"]],
            "iDisplayLength": -1,
            colReorder: true,
            "dom": '<"top"iflp<"clear">>rt<"datatable-scroll"t><"bottom"iflp<"clear">>',
            "language": {
                "lengthMenu": "Show _MENU_ records",
                "info": "Showing _START_ to _END_ of _TOTAL_ records",
            },
            data: data,
            columns: [
                        { data: "SchemaName", title: "Fund Name", className: "w-100-fixed", },
                        //{ data: "HistoryNAV", title: "1 Month", className: "w-100-fixed", },
                        { data: "LatestNAV", title: "Latest NAV", className: "w-100-fixed", },
            ]
        });

    $('#mfPerformance').modal('show');
};

function ShowFundPerformanceError(data) {
};


function LoadFundHistory() {
    var fromDate = $('[data-select="FromDate"]').val();
    var ToDate = $('[data-select="ToDate"]').val();

    $.ajax({
        url: '/MutualFunds/DownloadFundNavHistory',
        type: 'post',
        dataType: 'json',
        data: JSON.stringify({ fromDate: fromDate, toDate: ToDate }),
        //data: request,
        contentType: "application/json; charset=utf-8",
        success: FundHistorySuccess
    });
};

function FundHistorySuccess(response) {
};
