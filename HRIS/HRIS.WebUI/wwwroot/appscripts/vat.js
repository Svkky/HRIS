//const { parseFloat } = require("core-js/core/number");

$(function () {
    var baseUrl = $('#ApiBaseUrl').val();
    var token = $('#jwtToken').val();
    var branchId = $('#branchId').val();
    var bearerToken = 'Bearer ' + token;
    

    $('#btn-AddVat').click(function () {
        var name = document.getElementById("NameAdd");
        var percentAdd = document.getElementById("percentAdd");
        var isNameValid = name.checkValidity();
        var IsPercentValid = percentAdd.checkValidity();
        //if (isNameValid == false) {
        //    $('#NameAdd').css('border-color', 'Red');
        //    $('#NameAdd').focus();
        //    $('#btn-AddVat').attr('disabled', true);
        //    ShowMessagePopup("Oops!", "Please ensure you fill the vat description field correctly", "warning");
        //    return false;
        //}
        //if (IsPercentValid == false) {
        //    $('#percentAdd').css('border-color', 'Red');
        //    $('#percentAdd').focus();
        //    $('#btn-AddVat').attr('disabled', true);
        //    ShowMessagePopup("Oops!", "Please ensure you fill the vat amount field correctly", "warning");
        //    return false;
        //}

        AddVat();
    });
    $('#btn-EditVat').click(function () {
        var name = document.getElementById("NameEdit");
        var percentAdd = document.getElementById("percentEdit");
        var isNameValid = name.checkValidity();
        var IsPercentValid = percentAdd.checkValidity();
        if (isNameValid == false) {
           
            $('#btn-EditVat').attr('disabled', true);
            ShowMessagePopup("Oops!", "Please ensure you fill the vat description field correctly", "warning");
            return false;
        }
        if (IsPercentValid == false) {
            $('#percentEdit').css('border-color', 'Red');
            $('#percentEdit').focus();
            $('#btn-EditVat').attr('disabled', true);
            ShowMessagePopup("Oops!", "Please ensure you fill the vat amount field correctly", "warning");
            return false;
        }

        EditVat();
    });

    loadVat();

    function loadVat() {
        var remoteDataLoader = window.DevExpress.data.AspNet.createStore({
            key: "vatId",
            loadUrl: baseUrl + "Vat/GetAllVat",
            onBeforeSend: function (operation,
                ajaxSettings) {
                ajaxSettings.beforeSend = function (xhr) {
                    console.log($('#jwtToken').val());

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
                        dataField: "name",
                        caption: "Name",
                        sortIndex: 0,
                        sortOrder: 'asc',
                        //fixed: true,
                        cssClass: 'font-bold'

                    },
                    {
                        dataField: "percentage",
                        caption: "Vat Amount"

                    },
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
                                    cssClass: 'btn btn-primary btn-xs',
                                    alignment: "center",
                                    onClick: function (e) {
                                        $('#VatId').val(options.data.vatId);
                                        $('#NameEdit').val(options.data.name);
                                        $('#percentEdit').val(options.data.percentage);
                                        $('#EditVatModal').modal('show');
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
                                                DeleteVat(options.data.VatId)

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

        dataGrid = window.$("#VatContainer").dxDataGrid(gridOptions).dxDataGrid("instance");
    }

    function DeleteVat(id) {
        var deleteVatRequest = {
            VatId:id
        };
        
        var token = $('#jwtToken').val();
        var bearerToken = 'Bearer ' + token;
       
        var applicationUrl = baseUrl + 'Vat/RemoveVat';
        $.ajax({
            headers: {
                'Authorization': bearerToken
            },
            url: applicationUrl,
            type: 'POST',
            contentType: 'application/json',
            data: JSON.stringify(deleteVatRequest),
            success: function (data) {
                if (data.responseCode == "00") {
                    swal({
                        title: "Successful!",
                        text: "Vat deleted successfully",
                        icon: "success",
                        closeOnClickOutside: false,
                        closeOnEsc: false
                    })
                        .then(() => {

                            loadCategory();
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

    function EditVat() {
        var EditVatRequest = {
            Name: $('#NameEdit').val(),
            vatId: parseInt($('#VatId').val()),
            percentage: parseFloat($('#percentEdit').val())
        };

        var token = $('#jwtToken').val();
        var bearerToken = 'Bearer ' + token;

        var applicationUrl = baseUrl + 'Vat/UpdateVat';
        $.ajax({
           headers: {
                'Authorization': bearerToken
            },
            url: applicationUrl,
            type: 'POST',
            contentType: 'application/json',
            data: JSON.stringify(EditVatRequest),
            success: function (data) {
                if (data.responseCode === "00") {
                    swal({
                        title: "Successful!",
                        text: "Vat updated successfully",
                        icon: "success",
                        closeOnClickOutside: false,
                        closeOnEsc: false
                    })
                        .then(() => {
                            $('#EditVatModal').modal('hide');
                            //$('#editCustomerModal').modal('hide');
                            $('#btn-EditVat').removeAttr('disabled');
                            loadVat();
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
                            $('#btn-EditVat').removeAttr('disabled');
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
                            $('#btn-EditVat').removeAttr('disabled');
                        });
                }
            },
            error: function (xhr) {
                alert('Woow something went wrong');
            }
        });
    }

    function AddVat() {
        var AddVatRequest = {
            Name: $('#NameAdd').val(),
            percentage: parseFloat($('#percentAdd').val()),
            branchId: parseInt(branchId)
        };

        var token = $('#jwtToken').val();
        var bearerToken = 'Bearer ' + token;

        var applicationUrl = baseUrl + 'Vat/CreateVat';
        $.ajax({
            headers: {
                'Authorization': bearerToken
            },
            url: applicationUrl,
            type: 'POST',
            contentType: 'application/json',
            data: JSON.stringify(AddVatRequest),
            success: function (data) {
                if (data.responseCode === "00") {
                    swal({
                        title: "Successful!",
                        text: "Vat added successfully",
                        icon: "success",
                        closeOnClickOutside: false,
                        closeOnEsc: false
                    })
                        .then(() => {
                            $('#VatModal').modal('hide');
                            $('#btn-AddVat').removeAttr('disabled');
                            loadVat();
                        });
                }
                else if (data.responseCode === "-1") {
                    swal({
                        title: "VAT Already Exists!",
                        text: data.message,
                        icon: "warning",
                        closeOnClickOutside: false,
                        closeOnEsc: false
                    })
                        .then(() => {
                            $('#btn-AddVat').removeAttr('disabled');
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
                            $('#btn-AddVat').removeAttr('disabled');
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