$(function () {
    var baseUrl = $('#ApiBaseUrl').val();
    var token = $('#jwtToken').val();
    var siteLocation = $('#siteLocation').val();
    var bearerToken = 'Bearer ' + token;
    //alert(bearerToken);

    $('#btn-AddStore').click(function () {
        AddStore();
    });
    $('#btn-EditStore').click(function () {
        EditStore();
    });
    GetStoreCount();
    loadStore();

    function loadStore() {
        var remoteDataLoader = window.DevExpress.data.AspNet.createStore({
            key: "storeSetupId",
            loadUrl: baseUrl + "v1.0/StoreSetup/GetAllStores",
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
                        dataField: "storeName",
                        caption: "Store Name",
                        sortIndex: 0,
                        sortOrder: 'asc',
                        //fixed: true,
                        cssClass: 'font-bold'

                    },
                    {
                        dataField: "storeEmail",
                        caption: "Store Email",
                        sortIndex: 0,
                        sortOrder: 'asc',
                        //fixed: true,
                        cssClass: 'font-bold'

                    },
                    {
                        dataField: "storePhone",
                        caption: "Store Phone Number",
                        sortIndex: 0,
                        sortOrder: 'asc',
                        //fixed: true,
                        cssClass: 'font-bold'

                    },
                    {
                        dataField: "storeAddress",
                        caption: "Store Address",
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
                                        $('#storeSetupId').val(options.data.storeSetupId);
                                        $('#storeName').val(options.data.storeName);
                                        $('#storeAddress').val(options.data.storeAddress);
                                        $('#storeEmail').val(options.data.storeEmail);
                                        $('#storePhone').val(options.data.storePhone);
                                        $('#Mod').modal('show');
                                    }
                                }
                            ).appendTo(container);

                            if ($('#Role').val() === "GlobalAdmin") {
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
                                                    ResendEmail(options.data.storeEmail);

                                                } else {

                                                }
                                            });

                                        }
                                    }
                                ).appendTo(container);
                            }


                        },
                        visible: true
                    }
                ],

            };

        dataGrid = window.$("#storeContainer").dxDataGrid(gridOptions).dxDataGrid("instance");
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
    //function DeleteCategory(id) {
    //    var deleteCategoryRequest = {};
    //    deleteCategoryRequest.CategoryId = id;
    //    var token = $('#jwtToken').val();
    //    var bearerToken = 'Bearer ' + token;
    //    alert(token);
    //    var applicationUrl = baseUrl + 'v1.0/Category/DeleteCategory';
    //    $.ajax({
    //        headers: {
    //            'Authorization': bearerToken
    //        },
    //        url: applicationUrl,
    //        type: 'POST',
    //        contentType: 'application/json',
    //        data: JSON.stringify(deleteCategoryRequest),
    //        success: function (data) {
    //            alert('houraay!!!! successful')
    //            loadCategory();
    //        },
    //        error: function (xhr) {
    //            alert('Woow something went wrong');
    //        }
    //    });
    //}

    function EditStore() {
        var EditStoreReques = {
            storeSetupId: parseInt($('#storeSetupId').val()),
            storeName: $('#storeName').val(),
            storeAddress: $('#storeAddress').val(),
            storePhone: $('#storePhone').val(),
            storeEmail: $('#storeEmail').val()
        };

        var token = $('#jwtToken').val();
        var bearerToken = 'Bearer ' + token;

        var applicationUrl = baseUrl + 'v1.0/StoreSetup/UpdateStore';
        $.ajax({
            headers: {
                'Authorization': bearerToken
            },
            url: applicationUrl,
            type: 'POST',
            contentType: 'application/json',
            data: JSON.stringify(EditStoreReques),
            success: function (data) {
                if (data.responseCode == "00") {
                    swal({
                        title: "Successful!",
                        text: "Store udpated successfully",
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
                $('#Mod').modal('hide');
                loadStore();
            },
            error: function (xhr) {
                // alert('Woow something went wrong');
                //alert(applicationUrl);
            }
        });
    }

    function AddStore() {
        var EditStoreRequest = {
            storeName: $('#storeNameAdd').val(),
            storeAddress: $('#storeAddressAdd').val(),
            storePhone: $('#storePhoneAdd').val(),
            storeEmail: $('#storeEmailAdd').val(),
            websiteLocation: siteLocation
        };

        var token = $('#jwtToken').val();
        var bearerToken = 'Bearer ' + token;

        var applicationUrl = baseUrl + 'v1.0/StoreSetup';
        $.ajax({
            headers: {
                'Authorization': bearerToken
            },
            url: applicationUrl,
            type: 'POST',
            contentType: 'application/json',
            data: JSON.stringify(EditStoreRequest),
            success: function (data) {
                if (data.responseCode == "00") {
                    swal({
                        title: "Successful!",
                        text: "Store created successfully",
                        icon: "success",
                        closeOnClickOutside: false,
                        closeOnEsc: false
                    })
                        .then(() => {
                            $('#AddStore').hide();
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

                $('#myModal').modal('hide');
                loadStore();
            },
            error: function (xhr) {
                //alert('Woow something went wrong');
            }
        });
    }

    function GetStoreCount() {

        var applicationUrl = baseUrl + 'v1.0/MenuItem/GetStoreName';
        $.ajax({
            url: applicationUrl,
            type: 'GET',
            contentType: 'application/json',
            success: function (data) {
                if (data.data === "" || data.data == null) {
                    $('#AddStore').show();
                }
                else {

                    $('#AddStore').hide();
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