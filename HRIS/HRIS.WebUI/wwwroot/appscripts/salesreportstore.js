
$(function () {

    $('#actionLoader1').hide();
    $('#actionLoader2').hide();
    var baseUrl = $('#ApiBaseUrl').val();
    var webUrl = $('#webUrl').val();
    var token = $('#jwtToken').val();
    var bearerToken = 'Bearer ' + token;


    LoadSales();

    function LoadSales() {
        var remoteDataLoader = window.DevExpress.data.AspNet.createStore({
            key: "branchId",
            loadUrl: baseUrl + "v1.0/Sales/get-branch-sales",
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
                        dataField: "branchName",
                        caption: "Branch Name",
                        sortIndex: 0,
                        sortOrder: 'asc',
                        //fixed: true,
                        cssClass: 'font-bold'

                    },
                    {
                        dataField: "totalOrders",
                        caption: "Total Orders",
                        sortIndex: 0,
                        sortOrder: 'asc',
                        //fixed: true,
                        cssClass: 'font-bold'

                    },
                    ,
                    {
                        dataField: "totalAmount",
                        caption: "Total Amount",
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
                                    title: 'View Sales',
                                    cssClass: 'btn btn-primary btn-sm',
                                    alignment: "center",
                                    onClick: function (e) {
                                        GotoDetails(options.data.branchId);
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

    function GotoDetails(branchId) {
        let url = webUrl + "/Reports/Sales/?brnchId=" + branchId;
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