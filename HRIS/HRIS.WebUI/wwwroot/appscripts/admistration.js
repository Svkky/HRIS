$(function () {
    $('#actionLoader1').hide();
    $('#actionLoader2').hide();
    var baseUrl = $('#ApiBaseUrl').val();
    var token = $('#jwtToken').val();
    var branchId = $('#branchId').val();
    var siteLocation = $('#siteLocation').val();
    var bearerToken = 'Bearer ' + token;

    $('#btn-AddUser').click(function () {
        var fname = document.getElementById('FirstName');
        var lname = document.getElementById('LastName');
        var email = document.getElementById('Email');
        var phone = document.getElementById('PhoneNumber');

        var isEmailValid = email.checkValidity();
        var isFirstNameValid = fname.checkValidity();
        var isLastNameValid = lname.checkValidity();
        var isPhoneValid = phone.checkValidity();
        var isValid = true;
        if (!isEmailValid) {
            $('#Email').css('border-color', 'Red');
            ($('#Email').focus());
            isValid = false;
        }
        if (isLastNameValid == false) {
            $('#LastName').css('border-color', 'Red');
            ($('#LastName').focus());
            isValid = false;
        }
        if (isFirstNameValid == false) {
            $('#FirstName').css('border-color', 'Red');
            ($('#FirstName').focus());
            isValid = false;
        }
        if (isPhoneValid == false) {
            $('#PhoneNumber').css('border-color', 'Red');
            ($('#PhoneNumber').focus());
            isValid = false;
        }

        if (isValid == false) {
            return false;
        }

        AddUser();
    });
    $('#btn-EditUser').click(function () {
        EditUser();
    });

    loadUser();

    function loadUser() {
        var remoteDataLoader = window.DevExpress.data.AspNet.createStore({
            key: "userId",
            loadUrl: baseUrl + "Account/get-users",
            onBeforeSend: function (operation,
                ajaxSettings) {
                ajaxSettings.beforeSend = function (xhr) {
                    console.log($('#jwtToken').val());

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
                        dataField: "lastName",
                        caption: "User Name",
                        sortIndex: 0,
                        sortOrder: 'asc',
                        //fixed: true,
                        cssClass: 'font-bold'

                    },
                    {
                        dataField: "firstName",
                        caption: "User Name",
                        sortIndex: 0,
                        sortOrder: 'asc',
                        //fixed: true,
                        cssClass: 'font-bold'

                    },
                    {
                        dataField: "role",
                        caption: "Role",
                        sortIndex: 0,
                        sortOrder: 'asc',
                        //fixed: true,
                        cssClass: 'font-bold'

                    },
                    {
                        dataField: "email",
                        caption: "User Email",
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
                        width: 550,
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
                                        $('#UserId').val(options.data.UserId);
                                        $('#editUser_FirstName').val(options.data.firstName);
                                        $('#editUser_LastName').val(options.data.lastName);
                                        $('#editUser_Email').val(options.data.email);
                                        $('#editUser_Phone').val(options.data.phone);
                                        $('#editUser_RoleId').val(options.data.roleId).trigger("chosen:updated");

                                        $('#editUserModal').modal('show');
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
                                                    EnableUser(options.data.userId)

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
                                        icon: 'fa fa-user',
                                        text: 'Manage Users',
                                        type: 'warning',
                                        cssClass: 'btn btn-primary btn-sm',
                                        alignment: "center",
                                        onClick: function (e) {
                                            GoToManageUserPriviledgePage(options.data.userId, options.data.roleId);
                                        }
                                    }
                                ).appendTo(container);
                                $('<div />').dxButton(
                                    {
                                        icon: 'fa fa-list-ul',
                                        text: 'Disable',
                                        cssClass: 'btn btn-warning btn-sm',
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
                                                    DisableUser(options.data.userId)

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
                                                DeleteUser(options.data.userId)

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

        dataGrid = window.$("#UserContainer").dxDataGrid(gridOptions).dxDataGrid("instance");
    }

    function DeleteUser(id) {
        var deleteUserRequest = {};
        deleteUserRequest.UserId = id;
        var token = $('#jwtToken').val();
        var bearerToken = 'Bearer ' + token;
        var applicationUrl = baseUrl + 'Account/DeleteUser/' + id;
        $.ajax({
            headers: {
                'Authorization': bearerToken
            },
            url: applicationUrl,
            type: 'POST',
            contentType: 'application/json',
            data: JSON.stringify(deleteUserRequest),
            success: function (data) {
                if (data.succeeded === true) {
                    swal({
                        title: "Successful!",
                        text: "User deleted successfully",
                        icon: "success",
                        closeOnClickOutside: false,
                        closeOnEsc: false
                    })
                        .then(() => {
                            loadUser();
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

    function EditUser() {
        var EditUserRequest = {
            UserId: parseInt($('#UserId').val()),
            FullName: $('#editUser_Name').val(),
            Phone: $('#editUser_Phone').val(),
            Email: $('#editUser_Email').val(),
        };

        var token = $('#jwtToken').val();
        var bearerToken = 'Bearer ' + token;

        var applicationUrl = baseUrl + 'Account/UpdateUser';
        $.ajax({
            headers: {
                'Authorization': bearerToken
            },
            url: applicationUrl,
            type: 'POST',
            contentType: 'application/json',
            data: JSON.stringify(EditUserRequest),
            success: function (data) {
                if (data.succeeded) {
                    if (data.responseCode == "00") {
                        swal({
                            title: "Successful!",
                            text: "User updated successfully",
                            icon: "success",
                            closeOnClickOutside: false,
                            closeOnEsc: false
                        })
                            .then(() => {
                                $('#editUserModal').modal('hide');
                                loadUser();
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

    function AddUser() {
        var AddUserRequest = {
            FirstName: $('#FirstName').val(),
            LastName: $('#LastName').val(),
            Phone: $('#PhoneNumber').val(),
            RoleId: $('#RoleId').val(),
            Email: $('#Email').val(),
            UserName: $('#Email').val(),      
            branchId: branchId,
            webUrl: $('#webUrl').val()
        };

        var token = $('#jwtToken').val();
        var bearerToken = 'Bearer ' + token;

        var applicationUrl = baseUrl + 'Account/register';
        $.ajax({
            //headers: {
            //    'Authorization': bearerToken
            //},
            url: applicationUrl,
            type: 'POST',
            contentType: 'application/json',
            data: JSON.stringify(AddUserRequest),
            success: function (data) {
                if (data.responseCode == "00") {
                    swal({
                        title: "Successful!",
                        text: "User added successfully",
                        icon: "success",
                        closeOnClickOutside: false,
                        closeOnEsc: false
                    })
                        .then(() => {
                            $('#myModal').modal('hide');
                            loadUser();
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
                            loadUser();
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

    function GoToManageUserPriviledgePage(userId, roleId) {
        var gotoUrl = siteLocation + "/Administration/ManageUserPriviledge" + "?userId=" + userId + "&roleId=" + roleId;
        window.location.href = gotoUrl;
    }
  
    function EnableUser(id) {
        //var enableUserRequest = {};
        //enableUserRequest.userId = id;
        var token = $('#jwtToken').val();
        var bearerToken = 'Bearer ' + token;
        var applicationUrl = baseUrl + 'account/Enable/' + id;
        $.ajax({
            headers: {
                'Authorization': bearerToken
            },
            url: applicationUrl,
            type: 'POST',
            contentType: 'application/json',
            data: JSON.stringify(id),
            success: function (data) {
                if (data.responseCode == "00") {
                    swal({
                        title: "Successful!",
                        text: "user enabled successfully",
                        icon: "success",
                        closeOnClickOutside: false,
                        closeOnEsc: false
                    })
                        .then(() => {
                            loadUser();
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
        var token = $('#jwtToken').val();
        var bearerToken = 'Bearer ' + token;
        var applicationUrl = baseUrl + 'account/Disable/'+ id;
        $.ajax({
            headers: {
                'Authorization': bearerToken
            },
            url: applicationUrl,
            type: 'POST',
            contentType: 'application/json',
            data: JSON.stringify(id),
            success: function (data) {
                if (data.responseCode == "00") {
                    swal({
                        title: "Successful!",
                        text: "user disabled successfully",
                        icon: "success",
                        closeOnClickOutside: false,
                        closeOnEsc: false
                    })
                        .then(() => {
                            loadUser();
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

});

$(document).on({
    ajaxStart: function () {
        $('#cover-spin').show(0);
    },
    ajaxStop: function () {
        $('#cover-spin').hide();
    }
});
