$(function () {
    var baseUrl = $('#ApiBaseUrl').val();
    var token = $('#jwtToken').val();
    var siteLocation = $('#siteLocation').val();
    var bearerToken = 'Bearer ' + token;

    $('#btn-Addbranch').click(function () {
        Addbranch();
    });
    $('#btn-AddAdminbranch').click(function () {
        AddBranchAdmin();
    });
    $('#addBranchAdmin').click(function () {
        //$('#ModBranchAdmin').hide();
        $('#ModBranchAdmin').modal({ show: false })
    });
    $('#btn-Editbranch').click(function () {
        Editbranch();
       
    });
    document.getElementById("btn-Addbranch").disabled = true;
    //document.getElementById("btn-Editbranch").disabled = true;

   

    loadbranch();

    function loadbranch() {
        var baseUrl = $('#ApiBaseUrl').val();

        var remoteDataLoader = window.DevExpress.data.AspNet.createStore({
            key: "branchID",
            loadUrl: baseUrl + "v1.0/Branch/GetAllBranch",
            onBeforeSend: function (operation,
                ajaxSettings) {
                ajaxSettings.beforeSend = function (xhr) {
                    //console.log($('#jwtToken').val());
                   
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
                        dataField: "phoneNumber",
                        caption: "Branch Phone no",
                        sortIndex: 0,
                        sortOrder: 'asc',
                        //fixed: true,
                        cssClass: 'font-bold'

                    },
                    {
                        dataField: "location",
                        caption: "Branch Location",
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
                                        $('#branchID').val(options.data.branchID);
                                        $('#branchNameEdit').val(options.data.branchName);
                                        $('#phoneNumberEdit').val(options.data.phoneNumber);
                                        $('#locationEdit').val(options.data.location);
                                        $('#Mod').modal('show');
                                    }
                                }
                            ).appendTo(container);
                            $('<div />').dxButton(
                                {
                                    icon: 'fa fa-edit',
                                    text: 'View Admin',
                                    type: 'info',
                                    cssClass: 'btn btn-info btn-sm',
                                    alignment: "center",
                                    onClick: function (e) {
                                        window.location.href = siteLocation + '/Setup/BranchAdmin/?branchId=' + options.data.branchID
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
                                                
                                                DeleteBranch(options.data.branchID)
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

        dataGrid = window.$("#branchContainer").dxDataGrid(gridOptions).dxDataGrid("instance");
    }

    function LoadBranchAdmin(id) {
        var baseUrl = $('#ApiBaseUrl').val();

        var remoteDataLoader = window.DevExpress.data.AspNet.createStore({
            key: "branchID",
            loadUrl: baseUrl + "v1.0/Branch/GetAllBranchAdmin",
            onBeforeSend: function (operation,
                ajaxSettings) {
                ajaxSettings.beforeSend = function (xhr) {
                    //console.log($('#jwtToken').val());

                    xhr.setRequestHeader('Authorization', bearerToken);
                    xhr.setRequestHeader('id',id);
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
                    allowedPageSizes: [20, 40, 200, 500],
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
                        dataField: "userId",
                        caption: "ID",
                        sortIndex: 0,
                        sortOrder: 'asc',                        
                        //fixed: true,
                        cssClass: 'font-bold'

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
                        dataField: "email",
                        caption: "Email",
                        sortIndex: 0,
                        sortOrder: 'asc',
                        //fixed: true,
                        cssClass: 'font-bold'

                    },
                    {
                        dataField: "phone",
                        caption: "Phone Number",
                        sortIndex: 0,
                        sortOrder: 'asc',
                        //fixed: true,
                        cssClass: 'font-bold'

                    },
                    {
                        caption: "Action",
                        width: 700,
                        alignment: "center",
                        cellTemplate: function (container, options) {
                           
                            $('<div />').dxButton(
                                {
                                    icon: 'fa fa-list-ul',
                                    text: 'Remove',
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
                                                DeleteBranchAdmin(options.data.userId)
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

        dataGrid = window.$("#branchAdminContainer").dxDataGrid(gridOptions).dxDataGrid("instance");
    }

    function DeleteBranch(id) {
       
        var deleteCategoryRequest = {};
        deleteCategoryRequest.userId = id;
        var token = $('#jwtToken').val();
        var bearerToken = 'Bearer ' + token;
        var applicationUrl = baseUrl + 'v1.0/Branch/DeleteBranch';
        $.ajax({
            headers: {
                'Authorization': bearerToken
            },
            url: applicationUrl,
            type: 'POST',
            contentType: 'application/json',
            data: JSON.stringify(deleteCategoryRequest),
            //success: function (data) {
            //    alert('houraay!!!! successful')
            //    loadProductTypeVariation();
            //},
            success: function (data) {
                if (data.responseCode == "00") {
                    swal({
                        title: "Successful!",
                        text: "Branch deleted successfully",
                        icon: "success",
                        closeOnClickOutside: false,
                        closeOnEsc: false
                    })
                        .then(() => {

                            loadbranch();
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

                loadVat();
            },
            error: function (xhr) {
                alert('Woow something went wrong');

            }
        });
    }

    function DeleteBranchAdmin(id) {
        var deleteCategoryRequest = {};
        deleteCategoryRequest.userId = id;
        alert(id);
        var token = $('#jwtToken').val();
        var bearerToken = 'Bearer ' + token;
        var applicationUrl = baseUrl + 'v1.0/Branch/RemoveUserFromBranch/' + id;
        $.ajax({
            headers: {
                'Authorization': bearerToken
            },
            url: applicationUrl,
            type: 'POST',
            contentType: 'application/json',
            data: JSON.stringify(deleteCategoryRequest),           
            success: function (data) {
                if (data.responseCode == "00") {
                    swal({
                        title: "Successful!",
                        text: "User Removed successfully",
                        icon: "success",
                        closeOnClickOutside: false,
                        closeOnEsc: false
                    })
                        .then(() => {
                            $('#ModBranchAdmin').modal('hide');
                            loadbranch();
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

                $('#ModBranchAdmin').modal('hide');
                loadbranch();            },
            error: function (xhr) {
                alert('Woow something went wrong');

            }
        });
    }

    function Editbranch() {
        var EditStoreReques = {
            branchID: parseInt($('#branchID').val()),
            branchName: $('#branchNameEdit').val(),
            location: $('#locationEdit').val(),
            phoneNumber: $('#phoneNumberEdit').val(),
        };
        //alert $('#branchId');
        //alert $('#branchName');
        //alert $('#location');
        //alert $('#phoneNumber');

        var baseUrl = $('#ApiBaseUrl').val();
        var token = $('#jwtToken').val();
        var bearerToken = 'Bearer ' + token;
       
        var applicationUrl = baseUrl + 'v1.0/Branch/UpdateBranch';
        $.ajax({
            headers: {
                'Authorization': bearerToken
            },
            url: applicationUrl,
            type: 'POST',
            contentType: 'application/json',
            data: JSON.stringify(EditStoreReques),

            success: function (data) {
                if (data.responseCode === "00") {
                    swal({
                        title: "Successful!",
                        text: "Branch updated successfully",
                        icon: "success",
                        closeOnClickOutside: false,
                        closeOnEsc: false
                    })
                        .then(() => {
                            $('#Mod').modal('hide');
                            //$('#editCustomerModal').modal('hide');
                            loadbranch();
                        });
                }
                else if (data.responseCode === "-1") {
                    swal({
                        title: "Branch Already Exists!",
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
                //alert(applicationUrl);
            }
        });
    }

    function Addbranch() {
        var EditStoreRequest = {
            branchName: $('#branchNameAdd').val(),
            location: $('#branchlocationAdd').val(),
            phoneNumber: $('#branchPhoneAdd').val()
        };

        var token = $('#jwtToken').val();
        var bearerToken = 'Bearer ' + token;

        var applicationUrl = baseUrl + 'v1.0/Branch';
        $.ajax({
            headers: {
                'Authorization': bearerToken
            },
            url: applicationUrl,
            type: 'POST',
            contentType: 'application/json',
            data: JSON.stringify(EditStoreRequest),

            success: function (data) {
                if (data.responseCode === "00") {
                    swal({
                        title: "Successful!",
                        text: "Branch added successfully",
                        icon: "success",
                        closeOnClickOutside: false,
                        closeOnEsc: false
                    })
                        .then(() => {
                            $('#myModal').modal('hide');
                            loadbranch();
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
                alert('Woow something went wrong');
            }
        });
    }

    function AddBranchAdmin() {
        var EditStoreRequest = {
            UserID: $('#UserID').val(),
            BranchID: parseInt($('#BranchID').val())

        };

        var token = $('#jwtToken').val();
        var bearerToken = 'Bearer ' + token;

        var applicationUrl = baseUrl + 'v1.0/Branch/AssignUserToBranch';
        $.ajax({
            headers: {
                'Authorization': bearerToken
            },
            url: applicationUrl,
            type: 'POST',
            contentType: 'application/json',
            data: JSON.stringify(EditStoreRequest),

            success: function (data) {
                if (data.responseCode === "00") {
                    swal({
                        title: "Successful!",
                        text: "Branch added successfully",
                        icon: "success",
                        closeOnClickOutside: false,
                        closeOnEsc: false
                    })
                        .then(() => {
                            $('#AddBranchAdmin').modal('hide');
                            $('#ModBranchAdmin').modal('hide');
                            loadbranch();
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
                alert('Woow something went wrong');
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
