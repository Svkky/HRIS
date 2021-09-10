$(function () {
    var baseUrl = $('#ApiBaseUrl').val();
    var token = $('#jwtToken').val();
    var bearerToken = 'Bearer ' + token;
    var branchId = $('#branchId').val();

    $('#btn-AddRecord').click(function () {
        AddCustomerVoucher();
    });
   

    loadCustomerVoucher();

    function loadCustomerVoucher() {
        var remoteDataLoader = window.DevExpress.data.AspNet.createStore({
            key: "customerVoucherId",
            loadUrl: baseUrl + "v1.0/CustomerVoucher/GetAllCustomerVoucher",
            onBeforeSend: function (operation,
                ajaxSettings) {
                ajaxSettings.beforeSend = function (xhr) {
                    xhr.setRequestHeader('Authorization', bearerToken);
                    xhr.setRequestHeader('branchId', branchId);
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
                        dataField: "customer",
                        caption: "Name",
                        sortIndex: 0,
                        sortOrder: 'asc',
                        //fixed: true,
                        cssClass: 'font-bold'

                    },
                    {
                        dataField: "voucher",
                        caption: "Voucher Name."

                    },
                    {
                        dataField: "voucherNo",
                        caption: "Voucher No."

                    },
                    {
                        dataField: "amount",
                        caption: "Amount."

                    },
                    {
                        dataField: "balance",
                        caption: "Balance."

                    },

                    {
                        caption: "Action",
                        width: 340,
                        alignment: "center",
                        cellTemplate: function (container, options) {

                           
                            //$('<div />').dxButton(
                            //    {
                            //        icon: 'fa fa-list-ul',
                            //        text: 'Disable',
                            //        cssClass: 'btn btn-danger btn-sm',
                            //        type: 'danger',
                            //        alignment: "center",
                            //        onClick: function (args) {
                            //            DeleteCustomerVoucher(options.data.customerVoucherId);
                            //        }
                            //    }
                            //).appendTo(container);
                            $('<div />').dxButton(
                                {
                                    icon: 'fa fa-list-ul',
                                    text: 'Delete',
                                    cssClass: 'btn btn-danger btn-sm',
                                    type: 'danger',
                                    alignment: "center",
                                    onClick: function (args) {
                                        swal({
                                            title: "Are you sure?",
                                            text: "This action cannot be undone.",
                                            type: "warning",
                                            showCancelButton: true,
                                            confirmButtonColor: "#DD6B55",
                                            cancelButtonColor: "#008000",
                                            confirmButtonText: "Yes, Delete it!",
                                            cancelButtonText: "No, cancel please!",
                                            closeOnConfirm: false,
                                            closeOnCancel: false
                                        }).then(function (event) {
                                            if (event == true) {
                                                DeleteCustomerVoucher(options.data.customerVoucherId)

                                            } else {

                                            }
                                        });

                                    }
                                }
                            ).appendTo(container);
                        },
                        visible: true
                    }
                ],

            };

        dataGrid = window.$("#CustomerVoucherContainer").dxDataGrid(gridOptions).dxDataGrid("instance");
    }

    function DeleteCustomerVoucher(id) {
        var deleteVoucherRequest = {
            customerVoucherId: id
        };

        var token = $('#jwtToken').val();
        var bearerToken = 'Bearer ' + token;

        var applicationUrl = baseUrl + 'v1.0/CustomerVoucher/RemoveCustomerVoucher';
        $.ajax({
            headers: {
                'Authorization': bearerToken
            },
            url: applicationUrl,
            type: 'POST',
            contentType: 'application/json',
            data: JSON.stringify(deleteVoucherRequest),
            success: function (data) {
                if (data.responseCode === "00") {
                    swal({
                        title: "Successful!",
                        text: "Customer Voucher deleted successfully",
                        icon: "success",
                        closeOnClickOutside: false,
                        closeOnEsc: false
                    })
                        .then(() => {
                            loadCustomerVoucher();
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
                alert('Woow something went wrong');
            }
        });
    }

 
    function AddCustomerVoucher() {
        var AddCustomerVoucherRequest = {
            customerId: parseInt($('#CustomerId').val()),
            voucherId: parseInt($('#VoucherId').val()),
            BranchId: parseInt(branchId)
        };

        var token = $('#jwtToken').val();
        var bearerToken = 'Bearer ' + token;

        var applicationUrl = baseUrl + 'v1.0/CustomerVoucher/CreateCustomerVoucher';

        if (AddCustomerVoucherRequest.customerId == '' || AddCustomerVoucherRequest.voucherId == '' || AddCustomerVoucher.BranchId == '') {
            swal({
                title: 'Fields Empty!!',
                text: 'please check the missing field',
                icon: 'warning',
                confirmButtonText: 'Ok'
            });
        } else {
            $.ajax({
                headers: {
                    'Authorization': bearerToken
                },
                url: applicationUrl,
                type: 'POST',
                contentType: 'application/json',
                data: JSON.stringify(AddCustomerVoucherRequest),
                success: function (data) {
                    if (data.responseCode === "00") {
                        swal({
                            title: "Successful!",
                            text: "Customer Voucher added successfully",
                            icon: "success",
                            closeOnClickOutside: false,
                            closeOnEsc: false
                        })
                            .then(() => {
                                $('#CustomerId').val(''),
                                 $('#VoucherId').val('');
                                $('#myModal').modal('hide');
                                $('#CustomerVoucherModal').modal('hide');
                                loadCustomerVoucher();
                            });
                    }
                    else if (data.responseCode === "-1") {
                        swal({
                            title: "Already Exists!",
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
                    swal({
                        title: "Failed!",
                        text: 'Wooo Something went wrong',
                        icon: "danger",
                        closeOnClickOutside: false,
                        closeOnEsc: false
                    })
                        .then(() => {

                        });
                }
            });
        }
        
    }

});
$(document).on({
    ajaxStart: function () {
        $('#cover-spin').show(0);
    },
    ajaxStop: function () {
        $('#cover-spin').hide();
    }
});