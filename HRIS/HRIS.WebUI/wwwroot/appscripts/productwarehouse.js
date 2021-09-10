
$(function () {

    $('#actionLoader1').hide();
    $('#actionLoader2').hide();
    var baseUrl = $('#ApiBaseUrl').val();
    var token = $('#jwtToken').val();
    var bearerToken = 'Bearer ' + token;
   // var branchId = parseInt($('#branchId').val());


    $('#btn-ResetWarehouse').click(function () {
        LoadProductsInWarehouse();
    });

    $('#btn-FilterWarehouse').click(function () {
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
            FilterProductWarehouse();
        }

    });


    LoadProductsInWarehouse();

    function LoadProductsInWarehouse() {
        var remoteDataLoader = window.DevExpress.data.AspNet.createStore({
            key: "productWareHouseId",
            loadUrl: baseUrl + "v1.0/ProductWarehouse/get-products",
            onBeforeSend: function (operation,
                ajaxSettings) {
                ajaxSettings.beforeSend = function (xhr) {
                    xhr.setRequestHeader('Authorization', bearerToken);
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
                        dataField: "productName",
                        caption: "Product Name",
                        sortIndex: 0,
                        sortOrder: 'asc',
                        //fixed: true,
                        cssClass: 'font-bold'

                    },
                    ,
                    {
                        dataField: "allocatedQuantity",
                        caption: "Quantity",
                        sortIndex: 0,
                        sortOrder: 'asc',
                        //fixed: true,
                        cssClass: 'font-bold'

                    },
                    {
                        dataField: "balanceBefore",
                        caption: "Balance Brought Forward",
                        sortIndex: 0,
                        sortOrder: 'asc',
                        //fixed: true,
                        cssClass: 'font-bold'

                    },
                    {
                        dataField: "balanceAfter",
                        caption: "Balance After",
                        sortIndex: 0,
                        sortOrder: 'asc',
                        //fixed: true,
                        cssClass: 'font-bold'

                    },
                    {
                        dataField: "transactionType",
                        caption: "Transaction Type",
                        sortIndex: 0,
                        sortOrder: 'asc',
                        //fixed: true,
                        cssClass: 'font-bold'

                    },
                    {
                        dataField: "branchName",
                        caption: "Branch",
                        sortIndex: 0,
                        sortOrder: 'asc',
                        //fixed: true,
                        cssClass: 'font-bold'

                    },

                    {
                        dataField: "createdOn",
                        caption: "Transaction Date",
                        sortIndex: 0,
                        sortOrder: 'asc',
                        //fixed: true,
                        cssClass: 'font-bold'

                    },
                    {
                        dataField: "createdBy",
                        caption: "Acted Upon By",
                        sortIndex: 0,
                        sortOrder: 'asc',
                        //fixed: true,
                        cssClass: 'font-bold'

                    }
                ],

            };

        dataGrid = window.$("#ProductWarehouseContainer").dxDataGrid(gridOptions).dxDataGrid("instance");
    }


    function FilterProductWarehouse() {
        var remoteDataLoader = window.DevExpress.data.AspNet.createStore({
            key: "productWareHouseId",
            loadUrl: baseUrl + "v1.0/ProductWarehouse/get-products",
            onBeforeSend: function (operation,
                ajaxSettings) {
                ajaxSettings.beforeSend = function (xhr) {
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
                        dataField: "productName",
                        caption: "Product Name",
                        sortIndex: 0,
                        sortOrder: 'asc',
                        //fixed: true,
                        cssClass: 'font-bold'

                    },
                    ,
                    {
                        dataField: "allocatedQuantity",
                        caption: "Quantity",
                        sortIndex: 0,
                        sortOrder: 'asc',
                        //fixed: true,
                        cssClass: 'font-bold'

                    },
                    {
                        dataField: "balanceBefore",
                        caption: "Balance Brought Forward",
                        sortIndex: 0,
                        sortOrder: 'asc',
                        //fixed: true,
                        cssClass: 'font-bold'

                    },
                    {
                        dataField: "balanceAfter",
                        caption: "Balance After",
                        sortIndex: 0,
                        sortOrder: 'asc',
                        //fixed: true,
                        cssClass: 'font-bold'

                    },
                    {
                        dataField: "transactionType",
                        caption: "Transaction Type",
                        sortIndex: 0,
                        sortOrder: 'asc',
                        //fixed: true,
                        cssClass: 'font-bold'

                    },
                    {
                        dataField: "branchName",
                        caption: "Branch",
                        sortIndex: 0,
                        sortOrder: 'asc',
                        //fixed: true,
                        cssClass: 'font-bold'

                    },
                    {
                        dataField: "createdOn",
                        caption: "Transaction Date",
                        sortIndex: 0,
                        sortOrder: 'asc',
                        //fixed: true,
                        cssClass: 'font-bold'

                    },
                    {
                        dataField: "createdBy",
                        caption: "Acted Upon By",
                        sortIndex: 0,
                        sortOrder: 'asc',
                        //fixed: true,
                        cssClass: 'font-bold'

                    }
                ],

            };

        dataGrid = window.$("#ProductWarehouseContainer").dxDataGrid(gridOptions).dxDataGrid("instance");
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

