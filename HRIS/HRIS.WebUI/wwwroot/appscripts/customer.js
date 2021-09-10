
$(function () {

    $('#actionLoader1').hide();
    $('#actionLoader2').hide();
    var baseUrl = $('#ApiBaseUrl').val();
    var token = $('#jwtToken').val();
    var bearerToken = 'Bearer ' + token;
    var branchId = parseInt($('#branchId').val());

    $('#btn-AddCustomer').click(function () {
        var name = document.getElementById('FullName');
        var email = document.getElementById('Email');
        var phone = document.getElementById('PhoneNumber');
        var address = document.getElementById('Address');

        var isEmailValid = email.checkValidity();
        var isNameValid = name.checkValidity();
        var isAddressValid = address.checkValidity();
        var isPhoneValid = phone.checkValidity();

        if (!isEmailValid) {
            $('#Email').css('border-color', 'Red');
            ($('#Email').focus());
            return false;
        }
        if (!isNameValid) {
            $('#FullName').css('border-color', 'Red');
            ($('#FullName').focus());
            return false;
        }
        if (!isPhoneValid) {
            $('#PhoneNumber').css('border-color', 'Red');
            ($('#PhoneNumber').focus());
            return false;
        }


        AddCustomer();
    });
    $('#btn-EditCustomer').click(function () {
        EditCustomer();
    });

    loadCustomer();

    function loadCustomer() {
        var remoteDataLoader = window.DevExpress.data.AspNet.createStore({
            key: "customerId",
            loadUrl: baseUrl + "v1.0/Customer/GetAllCustomer",
            onBeforeSend: function (operation,
                ajaxSettings) {
                ajaxSettings.beforeSend = function (xhr) {
                    console.log($('#jwtToken').val());
                    xhr.setRequestHeader('branchId', branchId);
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
                        dataField: "fullName",
                        caption: "Customer Name",
                        sortIndex: 0,
                        sortOrder: 'asc',
                        //fixed: true,
                        cssClass: 'font-bold'

                    },
                    {
                        dataField: "phone",
                        caption: "Customer Phone",
                        sortIndex: 0,
                        sortOrder: 'asc',
                        //fixed: true,
                        cssClass: 'font-bold'

                    },
                    {
                        dataField: "email",
                        caption: "Customer Email",
                        sortIndex: 0,
                        sortOrder: 'asc',
                        //fixed: true,
                        cssClass: 'font-bold'

                    },
                    {
                        dataField: "address",
                        caption: "Customer Address",
                        sortIndex: 0,
                        sortOrder: 'asc',
                        //fixed: true,
                        cssClass: 'font-bold'

                    },
                    //<button class="btn btn-success btn-sm" type="button" data-toggle="modal" data-target="#Mod"><i class="fa fa-edit"></i>&nbsp;Edit</button>
                    //                                    <a class="btn btn-danger btn-sm" type="button" href="#"><i class="fa fa-list-ul"></i>&nbsp;Disable</a>
                    //{
                    //    "render": function (data, type, row) { return "<a data-toggle='modal' data-target='#Mod' type='button' class='btn btn-success btn-sm'  onclick=ViewMembers(''); ><i class='fa fa - edit'</a>"; }
                    //},
                    {
                        caption: "Action",
                        width: 340,
                        alignment: "center",
                        cellTemplate: function (container, options) {

                            $('<div />').dxButton(
                                {
                                    icon: 'fa fa-edit',
                                    text: 'Edit',
                                    type: 'success',
                                    cssClass: 'btn btn-primary btn-sm',
                                    alignment: "center",
                                    onClick: function (e) {
                                        $('#CustomerId').val(options.data.customerId);
                                        $('#editCustomer_Name').val(options.data.fullName);
                                        $('#editCustomer_Email').val(options.data.email);
                                        $('#editCustomer_Phone').val(options.data.phone);
                                        $('#editCustomer_Address').val(options.data.address);

                                        $('#editCustomerModal').modal('show');
                                    }
                                }
                            ).appendTo(container);
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
                                                DeleteCustomer(options.data.customerId)

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

        dataGrid = window.$("#customerContainer").dxDataGrid(gridOptions).dxDataGrid("instance");
    }

    function DeleteCustomer(id) {
        var deleteCustomerRequest = {};
        deleteCustomerRequest.CustomerId = id;
        var token = $('#jwtToken').val();
        var bearerToken = 'Bearer ' + token;
        var applicationUrl = baseUrl + 'v1.0/Customer/DeleteCustomer';
        $.ajax({
            headers: {
                'Authorization': bearerToken
            },
            url: applicationUrl,
            type: 'POST',
            contentType: 'application/json',
            data: JSON.stringify(deleteCustomerRequest),
            success: function (data) {
                if (data.responseCode == "00") {
                    swal({
                        title: "Successful!",
                        text: "Customer deleted successfully",
                        icon: "success",
                        closeOnClickOutside: false,
                        closeOnEsc: false
                    })
                        .then(() => {
                            loadCustomer();
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

    function EditCustomer() {
        var EditCustomerRequest = {
            CustomerId: parseInt($('#CustomerId').val()),
            FullName: $('#editCustomer_Name').val(),
            Phone: $('#editCustomer_Phone').val(),
            Email: $('#editCustomer_Email').val(),
            Address: $('#editCustomer_Address').val(),
        };

        var token = $('#jwtToken').val();
        var bearerToken = 'Bearer ' + token;

        var applicationUrl = baseUrl + 'v1.0/Customer/UpdateCustomer';
        $.ajax({
            headers: {
                'Authorization': bearerToken
            },
            url: applicationUrl,
            type: 'POST',
            contentType: 'application/json',
            data: JSON.stringify(EditCustomerRequest),
            success: function (data) {
                if (data.succeeded) {
                    if (data.responseCode == "00") {
                        swal({
                            title: "Successful!",
                            text: "Customer updated successfully",
                            icon: "success",
                            closeOnClickOutside: false,
                            closeOnEsc: false
                        })
                            .then(() => {
                                $('#editCustomerModal').modal('hide');
                                loadCustomer();
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

                }
            },
            error: function (xhr) {
                alert('Woow something went wrong');
            }
        });
    }

    function AddCustomer() {
        var AddCustomerRequest = {
            FullName: $('#FullName').val(),
            Phone: $('#PhoneNumber').val(),
            Email: $('#Email').val(),
            Address: $('#Address').val(),
            BranchId: branchId
        };
         
        var token = $('#jwtToken').val();
        var bearerToken = 'Bearer ' + token;

        var applicationUrl = baseUrl + 'v1.0/Customer/CreateCustomer';
        if (AddCustomerRequest.FullName == '' || AddCustomerRequest.Phone == '') {
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
                data: JSON.stringify(AddCustomerRequest),
                success: function (data) {
                    if (data.responseCode == "00") {
                        swal({
                            title: "Successful!",
                            text: "Customer added successfully",
                            icon: "success",
                            closeOnClickOutside: false,
                            closeOnEsc: false
                        })
                            .then(() => {
                                $('#myModal').modal('hide');
                                loadCustomer();
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
                                $('#myModal').modal('hide');
                                loadCustomer();
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