
$(function () {

    $('#actionLoader1').hide();
    $('#actionLoader2').hide();
    var baseUrl = $('#ApiBaseUrl').val();
    var token = $('#jwtToken').val();
    var bearerToken = 'Bearer ' + token;
    var branchId = parseInt($('#branchId').val());

    $('#isPack').change(function () {

        let id = $('#isPack').val();

        if (id === "Yes") {
            $("#CartonDiv").show();
            $("#totalQty").hide();
            $("#totalQty").val('');
        }
        else {
            $("#totalQty").show();
            $("#CartonDiv").hide();
            $("#CartonDiv").val('');
        }
    });
    $('#fullyPaidDropDown').change(function () {

        let id = $('#fullyPaidDropDown').val();

        if (id === "Yes") {

            $("#amountPaidDiv").hide();
            $("#amountPaidDiv").val('');
        }
        else {
            $("#amountPaidDiv").show();
        }
    });






    //getBranchProducts();
    //function getBranchProducts() {
    //    let productUrl = baseUrl + "v1.0/StoreProduct/get-all-store-products";
    //    let bearerToken = 'Bearer ' + token;
    //    var productDataLoader = DevExpress.data.AspNet.createStore({
    //        key: "storeProductId",
    //        loadUrl: productUrl,
    //        onBeforeSend: function (operation,
    //            ajaxSettings) {
    //            ajaxSettings.beforeSend = function (xhr) {
    //                xhr.setRequestHeader('Authorization', bearerToken);
    //            },
    //                ajaxSettings.global = false;
    //        }
    //    });
    //    var productDataSource = new DevExpress.data.DataSource({
    //        pageSize: 10,
    //        paginate: true,
    //        store: productDataLoader
    //    });

    //    $("#productIdDevExVendor").dxSelectBox({
    //        dataSource: productDataSource,
    //        valueExpr: "storeProductId",
    //        displayExpr: "productName",
    //        searchEnabled: true,
    //        hoverStateEnabled: true,
    //        searchExpr: ["productName", "storeProductId"],
    //        placeholder: "Enter products's name to search",
    //        showClearButton: true,
    //        onValueChanged: function (data) {
    //            window.localStorage.removeItem('selectedProductIdVendor');
    //            window.localStorage.setItem('selectedProductIdVendor', data.value);
    //        }
    //    }).dxSelectBox("instance");
    //}













    $('#btn-Warehouse').click(function () {
        let IsValid = true;
        if ($('#stProductId').val() === "") {
            $('#stProductId').css('border-color', 'Red');
            ($('#stProductId').focus());
            IsValid = false;
        }
        if ($('#VendorId').val() === "") {
            $('#VendorId').css('border-color', 'Red');
            ($('#VendorId').focus());
            IsValid = false;
        }
        if ($('#TotalAmount').val() === "") {
            $('#TotalAmount').css('border-color', 'Red');
            ($('#TotalAmount').focus());
            IsValid = false;
        }
        if ($('#DelieveryDate').val() === "") {
            $('#DelieveryDate').css('border-color', 'Red');
            ($('#DelieveryDate').focus());
            IsValid = false;
        }
       

        if (IsValid == false) {
            return false
        }
        else {
            //$('#actionLoader1').show();
            //$('#actionLoader2').show();
            //$('#ibox1').children('.modal fade').toggleClass('sk-loading');
            AddToWarehouse();
        }

    });

    
    $('#btn-Reset').click(function () {
        LoadVendorPayment();
    });
    $('#btn-Filter').click(function () {
        let from = document.getElementById("from");
        let to = document.getElementById("to");
        let supplierFilter = document.getElementById("SupplierId");
        
        let IsValidFromDate = from.checkValidity();
        let IsValidToDate = to.checkValidity();
        let IsValidSupplierIdFilter = supplierFilter.checkValidity();
       
        let isValid = true;

        if (IsValidFromDate == false || IsValidToDate == false || IsValidSupplierIdFilter == false) {
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
            Filter();
        }

    });
    LoadVendorPayment();

    function LoadVendorPayment() {
        var remoteDataLoader = window.DevExpress.data.AspNet.createStore({
            key: "vendorPaymentMasterId",
            loadUrl: baseUrl + "v1.0/ProductWarehouse/get-all-vendor-payment",
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
                        dataField: "billNo",
                        caption: "Bill No",
                        sortIndex: 0,
                        sortOrder: 'asc',
                        //fixed: true,
                        cssClass: 'font-bold'

                    },
                    ,
                    {
                        dataField: "vendorName",
                        caption: "Vendor",
                        sortIndex: 0,
                        sortOrder: 'asc',
                        //fixed: true,
                        cssClass: 'font-bold'

                    },
                    {
                        dataField: "productName",
                        caption: "Product",
                        sortIndex: 0,
                        sortOrder: 'asc',
                        //fixed: true,
                        cssClass: 'font-bold'

                    },
                    {
                        dataField: "cartons",
                        caption: "Cartons / Pack",
                        sortIndex: 0,
                        sortOrder: 'asc',
                        //fixed: true,
                        cssClass: 'font-bold'

                    },
                    {
                        dataField: "totalItemsPerCarton",
                        caption: "Items Per Carton",
                        sortIndex: 0,
                        sortOrder: 'asc',
                        //fixed: true,
                        cssClass: 'font-bold'

                    },
                    {
                        dataField: "totalQuantity",
                        caption: "Total Quantity",
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
                        dataField: "totalPaid",
                        caption: "Total Paid",
                        sortIndex: 0,
                        sortOrder: 'asc',
                        //fixed: true,
                        cssClass: 'font-bold'

                    },
                    {
                        dataField: "balance",
                        caption: "Balance",
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
                                    text: 'View',
                                    type: 'success',
                                    title: 'View payments',
                                    cssClass: 'btn btn-primary btn-sm',
                                    alignment: "center",
                                    onClick: function (e) {
                                        
                                    }
                                }
                            ).appendTo(container);
                           
                        },
                        visible: true
                    }
               
                ],

            };

        dataGrid = window.$("#VendorPaymentContainer").dxDataGrid(gridOptions).dxDataGrid("instance");
    }

    function Filter() {
        var remoteDataLoader = window.DevExpress.data.AspNet.createStore({
            key: "vendorPaymentMasterId",
            loadUrl: baseUrl + "v1.0/ProductWarehouse/get-all-vendor-payment",
            onBeforeSend: function (operation,
                ajaxSettings) {
                ajaxSettings.beforeSend = function (xhr) {
                    xhr.setRequestHeader('Authorization', bearerToken);
                    xhr.setRequestHeader('from', $('#from').val());
                    xhr.setRequestHeader('to', $('#to').val());
                    xhr.setRequestHeader('SupplierId', $('#SupplierId').val());
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
                        dataField: "billNo",
                        caption: "Bill No",
                        sortIndex: 0,
                        sortOrder: 'asc',
                        //fixed: true,
                        cssClass: 'font-bold'

                    },
                    ,
                    {
                        dataField: "vendorName",
                        caption: "Vendor",
                        sortIndex: 0,
                        sortOrder: 'asc',
                        //fixed: true,
                        cssClass: 'font-bold'

                    },
                    {
                        dataField: "productName",
                        caption: "Product",
                        sortIndex: 0,
                        sortOrder: 'asc',
                        //fixed: true,
                        cssClass: 'font-bold'

                    },
                    {
                        dataField: "cartons",
                        caption: "Cartons / Pack",
                        sortIndex: 0,
                        sortOrder: 'asc',
                        //fixed: true,
                        cssClass: 'font-bold'

                    },
                    {
                        dataField: "totalItemsPerCarton",
                        caption: "Items Per Carton",
                        sortIndex: 0,
                        sortOrder: 'asc',
                        //fixed: true,
                        cssClass: 'font-bold'

                    },
                    {
                        dataField: "totalQuantity",
                        caption: "Total Quantity",
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
                        dataField: "totalPaid",
                        caption: "Total Paid",
                        sortIndex: 0,
                        sortOrder: 'asc',
                        //fixed: true,
                        cssClass: 'font-bold'

                    },
                    {
                        dataField: "balance",
                        caption: "Balance",
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
                                    text: 'View',
                                    type: 'success',
                                    title: 'View payments',
                                    cssClass: 'btn btn-primary btn-sm',
                                    alignment: "center",
                                    onClick: function (e) {

                                    }
                                }
                            ).appendTo(container);

                        },
                        visible: true
                    }

                ],

            };

        dataGrid = window.$("#VendorPaymentContainer").dxDataGrid(gridOptions).dxDataGrid("instance");
    }

    function AddToWarehouse() {

        var AddToWarehouseRequest = {
            VendorId: parseInt($('#VendorId').val()),
            StoreProductId: parseInt($('#stProductId').val()),
            TotalAmount: parseFloat($('#TotalAmount').val().replace(/,/g, '')),
            DeliveryDate: $('#DelieveryDate').val(),
        };
        //var AddToWarehouseRequest = {
        //    VendorId: parseInt($('#VendorId').val()),
        //    StoreProductId: parseInt(window.localStorage.getItem('selectedProductIdVendor')),
        //    TotalAmount: parseFloat($('#TotalAmount').val().replace(/,/g, '')),
        //    DeliveryDate: $('#DelieveryDate').val(),
        //};

        if ($('#fullyPaidDropDown').val() == "Yes") {
            AddToWarehouseRequest.Balance = 0;
            AddToWarehouseRequest.TotalPaid = parseFloat($('#TotalAmount').val().replace(/,/g, ''));
        }
        else {
            AddToWarehouseRequest.Balance = (parseFloat($('#TotalAmount').val().replace(/,/g, '')) - parseFloat($('#AmountPaid').val().replace(/,/g, '')));
            AddToWarehouseRequest.TotalPaid = parseFloat($('#AmountPaid').val().replace(/,/g, ''));
        }

        if ($('#isPack').val() == "Yes") {
            let tQty = (parseInt($('#TotalCarton').val()) * parseInt($('#TotalItemPerPack').val()));
            AddToWarehouseRequest.TotalItemsPerCarton = parseInt($('#TotalItemPerPack').val());
            AddToWarehouseRequest.Cartons = parseInt($('#TotalCarton').val());
            AddToWarehouseRequest.TotalQuantity = tQty ;
        }
        else {
            AddToWarehouseRequest.TotalItemsPerCarton = 0;
            AddToWarehouseRequest.Cartons = 0;
            AddToWarehouseRequest.TotalQuantity = parseInt($('#TotalQuantity').val());
        }


        var token = $('#jwtToken').val();
        var bearerToken = 'Bearer ' + token;

        var applicationUrl = baseUrl + 'v1.0/ProductWarehouse/add-product';
        $.ajax({
            headers: {
                'Authorization': bearerToken
            },
            url: applicationUrl,
            type: 'POST',
            contentType: 'application/json',
            data: JSON.stringify(AddToWarehouseRequest),
            success: function (data) {
                if (data.responseCode == "00") {
                    swal({
                        title: "Successful!",
                        text: data.message,
                        icon: "success",
                        closeOnClickOutside: false,
                        closeOnEsc: false
                    })
                        .then(() => {
                            $('#myModal').modal('hide');
                            LoadVendorPayment();
                        });
                }
                else if (data.responseCode === "-1") {
                    swal({
                        title: "Failed!",
                        text: data.message,
                        icon: "warning",
                        closeOnClickOutside: false,
                        closeOnEsc: false
                    })
                        .then(() => {

                        });
                }
                else {
                    swal({
                        title: "Failed!",
                        text: data.message,
                        icon: "danger",
                        closeOnClickOutside: false,
                        closeOnEsc: false
                    })
                        .then(() => {

                        });
                }

            },
            error: function (xhr) {
                // alert('Woow something went wrong');
            }
        });
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

