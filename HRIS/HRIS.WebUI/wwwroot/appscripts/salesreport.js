
$(function () {

    $('#actionLoader1').hide();
    $('#actionLoader2').hide();
    var baseUrl = $('#ApiBaseUrl').val();
    var brnchId = $('#brnchId').val();
    var token = $('#jwtToken').val();
    var webUrl = $('#webUrl').val();
    var bearerToken = 'Bearer ' + token;
    // var branchId = parseInt($('#branchId').val());


    $('#btn-ResetSales').click(function () {
        LoadSales();
    });

    $('#btn-FilterSales').click(function () {
        let from = document.getElementById("from");
        let to = document.getElementById("to");

        let IsValidFromDate = from.checkValidity();
        let IsValidToDate = to.checkValidity();

        let isValid = true;

        if (IsValidFromDate == false || IsValidToDate == false) {
            isValid = false;
        }

        if (isValid == false) {
            swal({
                title: "Failed!",
                text: "All filter options are required",
                icon: "warning",
                closeOnClickOutside: false,
                closeOnEsc: false
            })
                .then(() => {
                    return false
                });
            return false;
        }
        else {
            FilterSales();
        }

    });


    LoadSales();

    function LoadSales() {
        var remoteDataLoader = window.DevExpress.data.AspNet.createStore({
            key: "billNumber",
            loadUrl: baseUrl + "v1.0/Sales/get-all",
            onBeforeSend: function (operation,
                ajaxSettings) {
                ajaxSettings.beforeSend = function (xhr) {
                    xhr.setRequestHeader('Authorization', bearerToken);
                    if (brnchId === null || brnchId === "") {

                    }
                    else {
                        xhr.setRequestHeader('brnchId', brnchId);
                    }
                },
                    ajaxSettings.global = false;
            }
        });




        var dataGrid,
            gridOptions = {
                dataSource: remoteDataLoader,
                columnHidingEnabled: true,
                showBorders: true,
                remoteOperations: {
                    paging: true,
                    filtering: true,
                    sorting: true,
                    grouping: true,
                    summary: true,
                    groupPaging: true
                },
                searchPanel: {
                    visible: true,
                    placeholder: "Search...",
                    width: 250
                },
                paging: {
                    pageSize: 10
                },
                pager: {
                    showNavigationButtons: true,
                    showPageSizeSelector: true,
                    allowedPageSizes: [10, 20, 100, 250],
                    showInfo: true
                },
                selection: {
                    mode: "single",
                    mode: "multiple",
                    selectAllMode: 'page',
                    showCheckBoxesMode: 'no'
                },
                "export": {
                    enabled: false,
                    fileName: ""
                },
                hoverStateEnabled: true,
                showRowLines: true,
                rowAlternationEnabled: true,
                columnAutoWidth: true,
                columns: [
                    {
                        caption: 'S/N',
                        width: "auto",
                        allowSorting: false,
                        allowFiltering: false,
                        allowReordering: false,
                        allowHeaderFiltering: false,
                        allowGrouping: false,
                        cellTemplate: function (container, options) {
                            container.text(dataGrid.pageIndex() * dataGrid.pageSize() + (options.rowIndex + 1));

                        }
                    },
                    {
                        dataField: "billNumber",
                        caption: "Bill Number",
                        sortIndex: 0,
                        sortOrder: 'asc',
                        //fixed: true,
                        cssClass: 'font-bold'

                    },
                    {
                        dataField: "totalAmount",
                        caption: "Total Amount",
                        sortIndex: 0,
                        sortOrder: 'asc',
                        //fixed: true,
                        cssClass: 'font-bold'

                    },
                    {
                        dataField: "totalDiscount",
                        caption: "Total Discount",
                        sortIndex: 0,
                        sortOrder: 'asc',
                        //fixed: true,
                        cssClass: 'font-bold'

                    },
                    {
                        dataField: "totalVat",
                        caption: "Total Vat",
                        sortIndex: 0,
                        sortOrder: 'asc',
                        //fixed: true,
                        cssClass: 'font-bold'

                    },
                    {
                        dataField: "modeOfPayment",
                        caption: "Payment Mode",
                        sortIndex: 0,
                        sortOrder: 'asc',
                        //fixed: true,
                        cssClass: 'font-bold'

                    },
                    {
                        dataField: "fullname",
                        caption: "Customer Name",
                        sortIndex: 0,
                        sortOrder: 'asc',
                        //fixed: true,
                        cssClass: 'font-bold'

                    },
                    {
                        dataField: "datePaid",
                        caption: "Date Paid",
                        sortIndex: 0,
                        sortOrder: 'asc',
                        //fixed: true,
                        cssClass: 'font-bold'

                    },
                    {
                        caption: "Action",
                        alignment: "center",
                        cellTemplate: function (container, options) {

                            $('<div />').dxButton(
                                {
                                    icon: 'fa fa-eye',
                                    text: 'View Details',
                                    type: 'success',
                                    title: 'View payments',
                                    cssClass: 'btn btn-primary btn-sm',
                                    alignment: "center",
                                    onClick: function (e) {
                                        GotoDetails(options.data.billNumber);
                                    }
                                }
                            ).appendTo(container);

                        },
                        visible: true
                    }
                ],

            };

        dataGrid = window.$("#salesReportContainer").dxDataGrid(gridOptions).dxDataGrid("instance");
    }
    function FilterSales() {
        var remoteDataLoader = window.DevExpress.data.AspNet.createStore({
            key: "billNumber",
            loadUrl: baseUrl + "v1.0/Sales/get-all",
            onBeforeSend: function (operation,
                ajaxSettings) {
                ajaxSettings.beforeSend = function (xhr) {
                    xhr.setRequestHeader('Authorization', bearerToken);
                    if (brnchId === null || brnchId === "") {

                    }
                    else {
                        xhr.setRequestHeader('brnchId', brnchId);
                    }
                    xhr.setRequestHeader('Authorization', bearerToken);
                    xhr.setRequestHeader('from', $('#from').val());
                    xhr.setRequestHeader('to', $('#to').val());
                    xhr.setRequestHeader('ProductName', $('#prodId').val());
                    xhr.setRequestHeader('transactionType', $('#transactionType').val());
                },
                    ajaxSettings.global = false;
            }
        });




        var dataGrid,
            gridOptions = {
                dataSource: remoteDataLoader,
                columnHidingEnabled: true,
                showBorders: true,
                remoteOperations: {
                    paging: true,
                    filtering: true,
                    sorting: true,
                    grouping: true,
                    summary: true,
                    groupPaging: true
                },
                searchPanel: {
                    visible: true,
                    placeholder: "Search...",
                    width: 250
                },
                paging: {
                    pageSize: 10
                },
                pager: {
                    showNavigationButtons: true,
                    showPageSizeSelector: true,
                    allowedPageSizes: [10, 20, 100, 250],
                    showInfo: true
                },
                selection: {
                    mode: "single",
                    mode: "multiple",
                    selectAllMode: 'page',
                    showCheckBoxesMode: 'no'
                },
                "export": {
                    enabled: false,
                    fileName: ""
                },
                hoverStateEnabled: true,
                showRowLines: true,
                rowAlternationEnabled: true,
                columnAutoWidth: true,
                columns: [
                    {
                        caption: 'S/N',
                        width: "auto",
                        allowSorting: false,
                        allowFiltering: false,
                        allowReordering: false,
                        allowHeaderFiltering: false,
                        allowGrouping: false,
                        cellTemplate: function (container, options) {
                            container.text(dataGrid.pageIndex() * dataGrid.pageSize() + (options.rowIndex + 1));

                        }
                    },
                    {
                        dataField: "billNumber",
                        caption: "Bill Number",
                        sortIndex: 0,
                        sortOrder: 'asc',
                        //fixed: true,
                        cssClass: 'font-bold'

                    },
                    {
                        dataField: "totalAmount",
                        caption: "Total Amount",
                        sortIndex: 0,
                        sortOrder: 'asc',
                        //fixed: true,
                        cssClass: 'font-bold'

                    },
                    {
                        dataField: "totalDiscount",
                        caption: "Total Discount",
                        sortIndex: 0,
                        sortOrder: 'asc',
                        //fixed: true,
                        cssClass: 'font-bold'

                    },
                    {
                        dataField: "totalVat",
                        caption: "Total Vat",
                        sortIndex: 0,
                        sortOrder: 'asc',
                        //fixed: true,
                        cssClass: 'font-bold'

                    },
                    {
                        dataField: "modeOfPayment",
                        caption: "Payment Mode",
                        sortIndex: 0,
                        sortOrder: 'asc',
                        //fixed: true,
                        cssClass: 'font-bold'

                    },
                    {
                        dataField: "fullname",
                        caption: "Customer Name",
                        sortIndex: 0,
                        sortOrder: 'asc',
                        //fixed: true,
                        cssClass: 'font-bold'

                    },
                    {
                        dataField: "datePaid",
                        caption: "Date Paid",
                        sortIndex: 0,
                        sortOrder: 'asc',
                        //fixed: true,
                        cssClass: 'font-bold'

                    },
                    {
                        caption: "Action",
                        alignment: "center",
                        cellTemplate: function (container, options) {

                            $('<div />').dxButton(
                                {
                                    icon: 'fa fa-eye',
                                    text: 'View Details',
                                    type: 'success',
                                    title: 'View payments',
                                    cssClass: 'btn btn-primary btn-sm',
                                    alignment: "center",
                                    onClick: function (e) {
                                        GotoDetails(options.data.billNumber);
                                    }
                                }
                            ).appendTo(container);

                        },
                        visible: true
                    }
                ],

            };

        dataGrid = window.$("#salesReportContainer").dxDataGrid(gridOptions).dxDataGrid("instance");
    }


    function GotoDetails(bill) {
        let url = webUrl + "/Reports/SaleDetails/?billNumber=" + bill;
        window.location.href = url;
    }

});
//$(document).ready(function () {
//    var date = new Date();

//    var day = date.getDate();
//    var month = date.getMonth() + 1;
//    var year = date.getFullYear();

//    if (month < 10) month = "0" + month;
//    if (day < 10) day = "0" + day;

//    var today = year + "-" + month + "-" + day + "T00:00";
//    $("#ExpenditureDate").attr("value", today);
//});
$(document).on({
    ajaxStart: function () {
        $('#cover-spin').show(0);
    },
    ajaxStop: function () {
        $('#cover-spin').hide();
    }
});