$(function () {

    $('#actionLoader1').hide();
    $('#actionLoader2').hide();
    var baseUrl = $('#ApiBaseUrl').val();
    var token = $('#jwtToken').val();
    var bearerToken = 'Bearer ' + token;
    var branchId = parseInt($('#branchId').val());

    $('#btn-AddVendor').click(function () {
        //var name = document.getElementById('Name');
        //var email = document.getElementById('Email');
        //var phone = document.getElementById('PhoneNumber');
        //var address = document.getElementById('Address');

        //var isEmailValid = email.checkValidity();
        //var isNameValid = name.checkValidity();
        //var isAddressValid = address.checkValidity();
        //var isPhoneValid = phone.checkValidity();

        //if (!isEmailValid) {
        //    $('#Email').css('border-color', 'Red');
        //    ($('#Email').focus());
        //    return false;
        //}
        //if (!isNameValid) {
        //    $('#Name').css('border-color', 'Red');
        //    ($('#Name').focus());
        //    return false;
        //}
        //if (!isPhoneValid) {
        //    $('#PhoneNumber').css('border-color', 'Red');
        //    ($('#PhoneNumber').focus());
        //    return false;
        //}
     

        AddVendor();
    });
    $('#btn-EditVendor').click(function () {
        EditVendor();
    });

    loadVendor();

    function loadVendor() {
        var remoteDataLoader = window.DevExpress.data.AspNet.createStore({
            key: "supplierId",
            loadUrl: baseUrl + "v1.0/Supplier/GetAllSupplier",
            onBeforeSend: function (operation,
                ajaxSettings) {
                ajaxSettings.beforeSend = function (xhr) {
                    console.log($('#jwtToken').val()); 
                    xhr.setRequestHeader('BranchId', branchId);
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
                        dataField: "name",
                        caption: "Supplier Name",
                        sortIndex: 0,
                        sortOrder: 'asc',
                        //fixed: true,
                        cssClass: 'font-bold'

                    },
                    {
                        dataField: "phone",
                        caption: "Supplier Phone",
                        sortIndex: 0,
                        sortOrder: 'asc',
                        //fixed: true,
                        cssClass: 'font-bold'

                    },
                    {
                        dataField: "email",
                        caption: "Supplier Email",
                        sortIndex: 0,
                        sortOrder: 'asc',
                        //fixed: true,
                        cssClass: 'font-bold'

                    },
                    {
                        dataField: "address",
                        caption: "Supplier Address",
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
                                        $('#SupplierId').val(options.data.supplierId);
                                        $('#editVendor_Name').val(options.data.name);
                                        $('#editVendor_Email').val(options.data.email);
                                        $('#editVendor_Phone').val(options.data.phone);
                                        $('#editVendor_Address').val(options.data.address);

                                        $('#editVendorModal').modal('show');
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
                                                DeleteVendor(options.data.supplierId)

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

        dataGrid = window.$("#vendorContainer").dxDataGrid(gridOptions).dxDataGrid("instance");
    }

    function DeleteVendor(id) {
        var deleteVendorRequest = {};
        deleteVendorRequest.SupplierId = id;
        var token = $('#jwtToken').val();
        var bearerToken = 'Bearer ' + token;
        var applicationUrl = baseUrl + 'v1.0/Supplier/DeleteSupplier';
        $.ajax({
            headers: {
                'Authorization': bearerToken
            },
            url: applicationUrl,
            type: 'POST',
            contentType: 'application/json',
            data: JSON.stringify(deleteVendorRequest),
            success: function (data) {
                if (data.responseCode === "00") {
                    swal({
                        title: "Successful!",
                        text: "Supplier deleted successfully",
                        icon: "success",
                        closeOnClickOutside: false,
                        closeOnEsc: false
                    })
                        .then(() => {
                            loadVendor();
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

    function EditVendor() {
        var EditVendorRequest = {
            SupplierId: parseInt($('#SupplierId').val()),
            Name: $('#editVendor_Name').val(),
            Phone: $('#editVendor_Phone').val(),
            Email: $('#editVendor_Email').val(),
            Address: $('#editVendor_Address').val(),
        };

        var token = $('#jwtToken').val();
        var bearerToken = 'Bearer ' + token;

        var applicationUrl = baseUrl + 'v1.0/Supplier/UpdateSupplier';
        $.ajax({
            headers: {
                'Authorization': bearerToken
            },
            url: applicationUrl,
            type: 'POST',
            contentType: 'application/json',
            data: JSON.stringify(EditVendorRequest),
            success: function (data) {
                if (data.responseCode === "00") {
                    swal({
                        title: "Successful!",
                        text: "Supplier updated successfully",
                        icon: "success",
                        closeOnClickOutside: false,
                        closeOnEsc: false
                    })
                        .then(() => {
                            $('#editVendorModal').modal('hide');
                            $('#editCustomerModal').modal('hide');
                            loadVendor();
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

    function AddVendor() {
        var AddVendorRequest = {
            Name: $('#Name').val(),
            Phone: $('#PhoneNumber').val(),
            Email: $('#Email').val(),
            Address: $('#Address').val(),
            BranchId: 0
        };

        var token = $('#jwtToken').val();
        var bearerToken = 'Bearer ' + token;

        var applicationUrl = baseUrl + 'v1.0/Supplier/CreateSupplier';
        $.ajax({
            headers: {
                'Authorization': bearerToken
            },
            url: applicationUrl,
            type: 'POST',
            contentType: 'application/json',
            data: JSON.stringify(AddVendorRequest),
            success: function (data) {
                if (data.responseCode === "00") {
                    swal({
                        title: "Successful!",
                        text: "Supplier added successfully",
                        icon: "success",
                        closeOnClickOutside: false,
                        closeOnEsc: false
                    })
                        .then(() => {
                            $('#myModal').modal('hide');
                            loadVendor();
                        });
                }
                else if (data.responseCode === "-1") {
                    swal({
                        title: "Vendor Already Exists!",
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
$(document).on({
    ajaxStart: function () {
        $('#cover-spin').show(0);
    },
    ajaxStop: function () {
        $('#cover-spin').hide();
    }
});