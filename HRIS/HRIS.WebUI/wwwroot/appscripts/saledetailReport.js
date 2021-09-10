
$(function () {

    $('#actionLoader1').hide();
    $('#actionLoader2').hide();
    var baseUrl = $('#ApiBaseUrl').val();
    var token = $('#jwtToken').val();
    var billNumber = $('#billNumber').val();
    var bearerToken = 'Bearer ' + token;
    // var branchId = parseInt($('#branchId').val());




    LoadSaleDetails();

    function LoadSaleDetails() {
        var remoteDataLoader = window.DevExpress.data.AspNet.createStore({
            key: "saleDetailId",
            loadUrl: baseUrl + "v1.0/Sales/get-details",
            onBeforeSend: function (operation,
                ajaxSettings) {
                ajaxSettings.beforeSend = function (xhr) {
                    xhr.setRequestHeader('Authorization', bearerToken);
                    xhr.setRequestHeader('billNumber', billNumber);
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
                        dataField: "name",
                        caption: "Product Name",
                        sortIndex: 0,
                        sortOrder: 'asc',
                        //fixed: true,
                        cssClass: 'font-bold'

                    },
                    {
                        dataField: "sellingPrice",
                        caption: "Selling Price",
                        sortIndex: 0,
                        sortOrder: 'asc',
                        //fixed: true,
                        cssClass: 'font-bold'

                    },
                    {
                        dataField: "quantity",
                        caption: "Quantity",
                        sortIndex: 0,
                        sortOrder: 'asc',
                        //fixed: true,
                        cssClass: 'font-bold'

                    },
                    {
                        dataField: "discount",
                        caption: "Discount",
                        sortIndex: 0,
                        sortOrder: 'asc',
                        //fixed: true,
                        cssClass: 'font-bold'

                    },
                    {
                        dataField: "vatPercent",
                        caption: "Vat Percent",
                        sortIndex: 0,
                        sortOrder: 'asc',
                        //fixed: true,
                        cssClass: 'font-bold'

                    },
                    {
                        dataField: "vatValue",
                        caption: "Vat Value",
                        sortIndex: 0,
                        sortOrder: 'asc',
                        //fixed: true,
                        cssClass: 'font-bold'

                    },
                    {
                        dataField: "subTotal",
                        caption: "SubTotal",
                        sortIndex: 0,
                        sortOrder: 'asc',
                        //fixed: true,
                        cssClass: 'font-bold'

                    }
                ],

            };

        dataGrid = window.$("#salesDetailsReportContainer").dxDataGrid(gridOptions).dxDataGrid("instance");
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