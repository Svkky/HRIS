$(function () {
    var baseUrl = $('#ApiBaseUrl').val();
    var token = $('#jwtToken').val();
    var bearerToken = 'Bearer ' + token;
    var branchId = parseInt($('#branchId').val());

    $('#btn-AddProductTypeVariation').click(function () {
        AddProductTypeVariation();
    });
    $('#btn_EditProductTypeVariation').click(function () {
        var editDescription = document.getElementById('edit_ProductTypeVariation');
        //var isEditDescriptionValid = editDescription.checkValidity();
        //if (!isEditDescriptionValid) {
        //    $('#edit_ProductTypeVariation').css('border-color', 'Red');
        //    ($('#edit_ProductTypeVariation').focus());
        //    return false;
        //}
        EditProductTypeVariation();
    });

    loadProductTypeVariation();

    function loadProductTypeVariation() {
        var remoteDataLoader = window.DevExpress.data.AspNet.createStore({
            key: "productTypeVariationId",
            loadUrl: baseUrl + "v1.0/ProductTypeVariation/GetAllProductTypeVariation",
            onBeforeSend: function (operation,
                ajaxSettings) {
                ajaxSettings.beforeSend = function (xhr) {
                    //console.log($('#jwtToken').val());
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
                        dataField: "description",
                        caption: "Product Type Variation Name",
                        sortIndex: 0,
                        sortOrder: 'asc',
                        //fixed: true,
                        cssClass: 'font-bold'

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
                                    cssClass: 'btn btn-primary btn-sm',
                                    alignment: "center",
                                    onClick: function (e) {
                                        $('#productTypeVariationId').val(options.data.productTypeVariationId);
                                        $('#description').val(options.data.description);
                                         $('#Mod').modal('show');

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
                                                DeleteproductTypeVariation(options.data.productTypeVariationId)
                                              

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

        dataGrid = window.$("#producttypevariationContainer").dxDataGrid(gridOptions).dxDataGrid("instance");
    }

    function DeleteproductTypeVariation(id) {
        var deleteCategoryRequest = {};
        deleteCategoryRequest.productTypeVariationId = id;
        var token = $('#jwtToken').val();
        var bearerToken = 'Bearer ' + token;
        var applicationUrl = baseUrl + 'v1.0/ProductTypeVariation/DeleteProductTypeVariation';
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
                        text: "Product Type Variation deleted successfully",
                        icon: "success",
                        closeOnClickOutside: false,
                        closeOnEsc: false
                    })
                        .then(() => {

                            loadProductTypeVariation();
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

    function EditProductTypeVariation() {
        var EditStoreReques = {
            productTypeVariationId: parseInt($('#productTypeVariationId').val()),
            description: $('#description').val()
        };

        var token = $('#jwtToken').val();
        var bearerToken = 'Bearer ' + token;

        var applicationUrl = baseUrl + 'v1.0/ProductTypeVariation/UpdateProductTypeVariation';
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
                        text: "Product Type updated successfully",
                        icon: "success",
                        closeOnClickOutside: false,
                        closeOnEsc: false
                    })
                        .then(() => {
                            $('#Mod').modal('hide');
                            //$('#editCustomerModal').modal('hide');
                            loadProductTypeVariation();
                        });
                }
                else if (data.responseCode === "-1") {
                    swal({
                        title: "Product Type Variation Already Exists!",
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

    function AddProductTypeVariation() {
        var EditStoreRequest = {
            description: $('#descriptionAdd').val(),
            branchId: branchId
        };

        var token = $('#jwtToken').val();
        var bearerToken = 'Bearer ' + token;

        var applicationUrl = baseUrl + 'v1.0/ProductTypeVariation';
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
                        text: "Product Type Variation added successfully",
                        icon: "success",
                        closeOnClickOutside: false,
                        closeOnEsc: false
                    })
                        .then(() => {
                            $('#myModal').modal('hide');
                            loadProductTypeVariation();
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
                //alert('Woow something went wrong');
            }
        });
    }

});
//$(document).on({
//    ajaxStart: function () {
//        $('#cover-spin').show(0);
//    },
//    ajaxStop: function () {
//        $('#cover-spin').hide();
//    }
//});