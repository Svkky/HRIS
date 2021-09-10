$(function () {

    $('#actionLoader1').hide();
    $('#actionLoader2').hide();
    var baseUrl = $('#ApiBaseUrl').val();
    var token = $('#jwtToken').val();
    var branchId = $('#branchId').val();
    var branchIdCreate = $('#branchIdCreate').val();
    var siteLocation = $('#siteLocation').val();
    var bearerToken = 'Bearer ' + token;

    $('#btn-AddBranchAdmin').click(function () {
        var fname = document.getElementById('FirstName');
        var lname = document.getElementById('LastName');
        var email = document.getElementById('Email');
        var phone = document.getElementById('PhoneNumber');

        var isEmailValid = email.checkValidity();
        var isFistNameValid = fname.checkValidity();
        var isLastNameValid = lname.checkValidity();
        var isPhoneValid = phone.checkValidity();

        if (!isEmailValid) {
            $('#Email').css('border-color', 'Red');
            ($('#Email').focus());
            return false;
        }
        if (!isFistNameValid) {
            $('#FirstName').css('border-color', 'Red');
            ($('#FirstName').focus());
            return false;
        }
        if (!isLastNameValid) {
            $('#LastName').css('border-color', 'Red');
            ($('#LastName').focus());
            return false;
        }
        if (!isPhoneValid) {
            $('#PhoneNumber').css('border-color', 'Red');
            ($('#PhoneNumber').focus());
            return false;
        }


        AddBranchAdmin();
    });
    $('#btn-EditBranchAdmin').click(function () {
        EditBranchAdmin();
    });

    loadBranchAdmin();

    function loadBranchAdmin() {
        var apiUrl = baseUrl + 'v1.0/BranchAdmin/GetAllBranchAdmin';
        var remoteDataLoader = window.DevExpress.data.AspNet.createStore({
            key: "branchAdminId",
            loadUrl: apiUrl,
            onBeforeSend: function (operation,
                ajaxSettings) {
                ajaxSettings.beforeSend = function (xhr) {
                    console.log($('#jwtToken').val());

                    xhr.setRequestHeader('Authorization', bearerToken);
                    xhr.setRequestHeader('branchId', branchIdCreate);
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
                        dataField: "firstName",
                        caption: "First Name",
                        sortIndex: 0,
                        sortOrder: 'asc',
                        //fixed: true,
                        cssClass: 'font-bold'

                    },
                    {
                        dataField: "lastName",
                        caption: "Last Name",
                        sortIndex: 0,
                        sortOrder: 'asc',
                        //fixed: true,
                        cssClass: 'font-bold'

                    },
                    {
                        dataField: "phone",
                        caption: "Phone",
                        sortIndex: 0,
                        sortOrder: 'asc',
                        //fixed: true,
                        cssClass: 'font-bold'

                    },
                    {
                        dataField: "email",
                        caption: "Email",
                        sortIndex: 0,
                        sortOrder: 'asc',
                        //fixed: true,
                        cssClass: 'font-bold'

                    },

                    {
                        dataField: "isActive",
                        caption: "Status",
                        sortIndex: 0,
                        sortOrder: 'asc',
                        //fixed: true,
                        cssClass: 'font-bold'

                    },
                   
                    {
                        caption: "Action",
                        width: 500,
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
                                        $('#BranchAdminId').val(options.data.branchAdminId);
                                        $('#editBranchAdmin_FirstName').val(options.data.firstName);
                                        $('#editBranchAdmin_LastName').val(options.data.lastName);
                                        $('#editBranchAdmin_Email').val(options.data.email);
                                        $('#editBranchAdmin_Phone').val(options.data.phone);

                                        $('#editBranchAdminModal').modal('show');
                                    }
                                }
                            ).appendTo(container);

                            if (options.data.isActive === false) {
                                $('<div />').dxButton(
                                    {
                                        icon: 'fa fa-list-ul',
                                        text: 'Enable',
                                        cssClass: 'btn btn-danger btn-sm',
                                        type: 'primary',
                                        alignment: "center",
                                        onClick: function (args) {
                                            swal({
                                                title: "Enable this Admin?",
                                                text: "Please press 'Yes' to confirm.",
                                                type: "warning",
                                                showCancelButton: true,
                                                confirmButtonColor: "#DD6B55",
                                                cancelButtonColor: "#008000",
                                                confirmButtonText: "Yes, Enable",
                                                cancelButtonText: "No, cancel please!",
                                                closeOnConfirm: false,
                                                closeOnCancel: false
                                            }).then(function (event) {
                                                if (event == true) {
                                                    EnableUser(options.data.branchAdminId)

                                                } else {

                                                }
                                            });

                                        }
                                    }
                                ).appendTo(container);
                            }
                            else {
                                $('<div />').dxButton(
                                    {
                                        icon: 'fa fa-list-ul',
                                        text: 'Disable',
                                        cssClass: 'btn btn-danger btn-sm',
                                        type: 'primary',
                                        alignment: "center",
                                        onClick: function (args) {
                                            swal({
                                                title: "Disable this Admin?",
                                                text: "Please press 'Yes' to confirm.",
                                                type: "warning",
                                                showCancelButton: true,
                                                confirmButtonColor: "#DD6B55",
                                                cancelButtonColor: "#008000",
                                                confirmButtonText: "Yes, Disable",
                                                cancelButtonText: "No, cancel please!",
                                                closeOnConfirm: false,
                                                closeOnCancel: false
                                            }).then(function (event) {
                                                if (event == true) {
                                                    DisableUser(options.data.branchAdminId)

                                                } else {

                                                }
                                            });

                                        }
                                    }
                                ).appendTo(container);
                                
                            }
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
                                                DeleteBranchAdmin(options.data.branchAdminId)

                                            } else {

                                            }
                                        });

                                    }
                                }
                            ).appendTo(container);
                            $('<div />').dxButton(
                                {
                                    icon: 'fa fa-lock',
                                    text: 'Resend Email',
                                    type: 'secondary',
                                    cssClass: 'btn btn-warning btn-sm',
                                    alignment: "center",
                                    onClick: function (e) {
                                        swal({
                                            title: "Are you sure?",
                                            text: "Please confirm you want to resend activation email to this user.",
                                            type: "warning",
                                            showCancelButton: true,
                                            confirmButtonColor: "#DD6B55",
                                            cancelButtonColor: "#008000",
                                            confirmButtonText: "Yes, send it!",
                                            cancelButtonText: "No, cancel please!",
                                            closeOnConfirm: false,
                                            closeOnCancel: false
                                        }).then(function (event) {
                                            if (event == true) {
                                                ResendEmail(options.data.email);

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

        dataGrid = window.$("#BranchAdminContainer").dxDataGrid(gridOptions).dxDataGrid("instance");
    }

    function DeleteBranchAdmin(id) {
        var deleteBranchAdminRequest = {};
        deleteBranchAdminRequest.BranchAdminId = id;
        var token = $('#jwtToken').val();
        var bearerToken = 'Bearer ' + token;
        var applicationUrl = baseUrl + 'v1.0/BranchAdmin/DeleteBranchAdmin';
        $.ajax({
            headers: {
                'Authorization': bearerToken
            },
            url: applicationUrl,
            type: 'POST',
            contentType: 'application/json',
            data: JSON.stringify(deleteBranchAdminRequest),
            success: function (data) {
                if (data.responseCode == "00") {
                    swal({ 
                        title: "Successful!",
                        text: "BranchAdmin deleted successfully",
                        icon: "success",
                        closeOnClickOutside: false,
                        closeOnEsc: false
                    })
                        .then(() => {
                            loadBranchAdmin();
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
                
            }
        });
    }

    function EnableUser(id) {
        var enableBranchAdminRequest = {};
        enableBranchAdminRequest.BranchAdminId = id;
        var token = $('#jwtToken').val();
        var bearerToken = 'Bearer ' + token;
        var applicationUrl = baseUrl + 'v1.0/BranchAdmin/Enable';
        $.ajax({
            headers: {
                'Authorization': bearerToken
            },
            url: applicationUrl,
            type: 'POST',
            contentType: 'application/json',
            data: JSON.stringify(enableBranchAdminRequest),
            success: function (data) {
                if (data.responseCode == "00") {
                    swal({ 
                        title: "Successful!",
                        text: "BranchAdmin enabled successfully",
                        icon: "success",
                        closeOnClickOutside: false,
                        closeOnEsc: false
                    })
                        .then(() => {
                            loadBranchAdmin();
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
                
            }
        });
    }


    function DisableUser(id) {
        var disableBranchAdminRequest = {};
        disableBranchAdminRequest.BranchAdminId = id;
        var token = $('#jwtToken').val();
        var bearerToken = 'Bearer ' + token;
        var applicationUrl = baseUrl + 'v1.0/BranchAdmin/Disable';
        $.ajax({
            headers: {
                'Authorization': bearerToken
            },
            url: applicationUrl,
            type: 'POST',
            contentType: 'application/json',
            data: JSON.stringify(disableBranchAdminRequest),
            success: function (data) {
                if (data.responseCode == "00") {
                    swal({
                        title: "Successful!",
                        text: "BranchAdmin disabled successfully",
                        icon: "success",
                        closeOnClickOutside: false,
                        closeOnEsc: false
                    })
                        .then(() => {
                            loadBranchAdmin();
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

            }
        });
    }

    function EditBranchAdmin() {
        var EditBranchAdminRequest = {
            BranchAdminId: parseInt($('#BranchAdminId').val()),
            FirstName: $('#editBranchAdmin_FirstName').val(),
            LastName: $('#editBranchAdmin_LastName').val(),
            Phone: $('#editBranchAdmin_Phone').val(),
            Email: $('#editBranchAdmin_Email').val(),
        };

        var token = $('#jwtToken').val();
        var bearerToken = 'Bearer ' + token;

        var applicationUrl = baseUrl + 'v1.0/BranchAdmin/UpdateBranchAdmin';
        $.ajax({
            headers: {
                'Authorization': bearerToken
            },
            url: applicationUrl,
            type: 'POST',
            contentType: 'application/json',
            data: JSON.stringify(EditBranchAdminRequest),
            success: function (data) {
                if (data.succeeded) {
                    if (data.responseCode == "00") {
                        swal({
                            title: "Successful!",
                            text: "BranchAdmin updated successfully",
                            icon: "success",
                            closeOnClickOutside: false,
                            closeOnEsc: false
                        })
                            .then(() => {
                                $('#editBranchAdminModal').modal('hide');
                                loadBranchAdmin();
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
                //alert('Woow something went wrong');
            }
        });
    }

    function AddBranchAdmin() {
        var AddBranchAdminRequest = {
            FirstName: $('#FirstName').val(),
            LastName: $('#LastName').val(),
            Phone: $('#PhoneNumber').val(),
            Email: $('#Email').val(),
            BranchId: branchIdCreate,
            websiteLocation: siteLocation
        };

        var token = $('#jwtToken').val();
        var bearerToken = 'Bearer ' + token;

        var applicationUrl = baseUrl + 'v1.0/BranchAdmin/CreateBranchAdmin';
        $.ajax({
            headers: {
                'Authorization': bearerToken
            },
            url: applicationUrl,
            type: 'POST',
            contentType: 'application/json',
            data: JSON.stringify(AddBranchAdminRequest),
            success: function (data) {
                if (data.responseCode == "00") {
                    swal({
                        title: "Successful!",
                        text: "BranchAdmin added successfully",
                        icon: "success",
                        closeOnClickOutside: false,
                        closeOnEsc: false
                    })
                        .then(() => {
                            $('#myModal').modal('hide');
                            loadBranchAdmin();
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
                            loadBranchAdmin();
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
               
            }
        });
    }

    function ResendEmail(id) {

        var token = $('#jwtToken').val();
        var bearerToken = 'Bearer ' + token;
        var applicationUrl = baseUrl + 'Account/ResendEmail/' + id;
        $.ajax({
            headers: {
                'Authorization': bearerToken,
                'webUrl': siteLocation
            },
            url: applicationUrl,
            type: 'GET',
            contentType: 'application/json',
            data: id,
            success: function (data) {
                if (data == 1) {
                    swal({
                        title: "Successful!",
                        text: "Verifcation email re-sent to the user successfully",
                        icon: "success",
                        closeOnClickOutside: false,
                        closeOnEsc: false
                    })
                        .then(() => {
                            loadCustomer();
                        });
                }
                else if (data == -2) {
                    swal({
                        title: "Account Confirmed!",
                        text: "Users email verification is complete",
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
                        text: "Failed to send email verification please confirm that you have an active internet connection",
                        icon: "warning",
                        closeOnClickOutside: false,
                        closeOnEsc: false
                    })
                        .then(() => {

                        });
                }

            },
            error: function (xhr) {
                //alert('Woow something went wrong');
            }
        });
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
