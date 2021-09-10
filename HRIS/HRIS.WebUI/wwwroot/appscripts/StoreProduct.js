
$(function () {

    $('#actionLoader1').hide();
    $('#actionLoader2').hide();
    var baseUrl = $('#ApiBaseUrl').val();
    var token = $('#jwtToken').val();
    var bearerToken = 'Bearer ' + token;
    var branchId = parseInt($('#branchId').val());

    $('#btn-AddStoreProduct').click(function () {
        let IsValid = false;
        if ($('#ProductName').val() === "") {
            $('#ProductName').css('border-color', 'Red');
            ($('#ProductName').focus());
            IsValid = false;
        }
        else {
            //$('#actionLoader1').show();
            //$('#actionLoader2').show();
            //$('#ibox1').children('.modal fade').toggleClass('sk-loading');
            AddStoreProduct();
        }

    });
    $('#btn-SubmitEditStoreProduct').click(function () {
        EditStoreProduct();
    });

    $('#editStoreProduct_CategoryId').change(function () {

        let id = $('#editStoreProduct_CategoryId').val();

        $('#editStoreProduct_SubCategoryId').empty();

        if (id != null) {

            $.ajax({
                headers: {
                    'Authorization': bearerToken,
                    'BranchId': branchId
                },
                url: baseUrl + 'v1.0/SubCategory/GetSubCategoryById/' + id,
                type: 'GET',
                datatype: 'json',
                success: function (lgaList) {
                    $.each(lgaList.data, function (index, item) {

                        $('#editStoreProduct_SubCategoryId').append(
                            $("<option></option>")
                                .text(item.description)
                                .val(item.subCategoryId)
                        ).trigger("chosen:updated");
                    });
                }
            });
        }
    });
    $('#CategoryId').change(function () {

        let id = $('#CategoryId').val();

        $('#SubCategoryId').empty();

        if (id != null) {

            $.ajax({
                headers: {
                    'Authorization': bearerToken,
                    'BranchId': branchId
                },
                url: baseUrl + 'v1.0/SubCategory/GetSubCategoryById/' + id,
                type: 'GET',
                datatype: 'json',
                success: function (lgaList) {
                    $.each(lgaList.data, function (index, item) {

                        $('#SubCategoryId').append(
                            $("<option></option>")
                                .text(item.description)
                                .val(item.subCategoryId)
                        ).trigger("chosen:updated");
                    });
                }
            });
        }
    });
    loadStoreProduct();

    function loadStoreProduct() {
        var remoteDataLoader = window.DevExpress.data.AspNet.createStore({
            key: "storeProductId",
            loadUrl: baseUrl + "v1.0/StoreProduct/GetAllStoreProduct",
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
                        dataField: "categoryName",
                        caption: "Category Name",
                        sortIndex: 0,
                        sortOrder: 'asc',
                        //fixed: true,
                        cssClass: 'font-bold'

                    },
                    ,
                    {
                        dataField: "subCategoryNames",
                        caption: "Sub Category Name",
                        sortIndex: 0,
                        sortOrder: 'asc',
                        //fixed: true,
                        cssClass: 'font-bold'

                    },
                    {
                        dataField: "productName",
                        caption: "Product Name",
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
                                        $('#StoreProductId').val(options.data.storeProductId);
                                        $('#editStoreProduct_SubCategoryId').val(options.data.subCategoryId).trigger("chosen:updated");
                                        $('#editStoreProduct_CategoryId').val(options.data.categoryId).trigger("chosen:updated");
                                        $('#editStoreProduct_ProductName').val(options.data.productName);

                                        $('#EditStoreProductModal').modal('show');
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
                                                DeleteStoreProduct(options.data.storeProductId)

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

        dataGrid = window.$("#StoreProductContainer").dxDataGrid(gridOptions).dxDataGrid("instance");
    }

    function DeleteStoreProduct(id) {
        var DeleteStoreProductRequest = {};
        DeleteStoreProductRequest.StoreProductId = id;
        var token = $('#jwtToken').val();
        var bearerToken = 'Bearer ' + token;
        var applicationUrl = baseUrl + 'v1.0/StoreProduct/RemoveStoreProduct';
        $.ajax({
            headers: {
                'Authorization': bearerToken
            },
            url: applicationUrl,
            type: 'POST',
            contentType: 'application/json',
            data: JSON.stringify(DeleteStoreProductRequest),
            success: function (data) {
                swal({
                    title: "Successful!",
                    text: "product deleted successfully",
                    icon: "success",
                    closeOnClickOutside: false,
                    closeOnEsc: false
                })
                    .then(() => {
                        loadStoreProduct();
                    });

            },
            error: function (xhr) {
                //alert('Woow something went wrong');
            }
        });
    }

    function EditStoreProduct() {
        var EditStoreProductRequest = {
            SubCategoryId: $('#editStoreProduct_SubCategoryId').val(),
            StoreProductId: parseInt($('#StoreProductId').val()),
            CategoryId: parseInt($('#editStoreProduct_CategoryId').val()),
            ProductName: $('#editStoreProduct_ProductName').val()
        };

        var token = $('#jwtToken').val();
        var bearerToken = 'Bearer ' + token;

        var applicationUrl = baseUrl + 'v1.0/StoreProduct/UpdateStoreProduct';
        $.ajax({
            headers: {
                'Authorization': bearerToken
            },
            url: applicationUrl,
            type: 'POST',
            contentType: 'application/json',
            data: JSON.stringify(EditStoreProductRequest),
            success: function (data) {
                if (data.succeeded) {
                    if (data.responseCode == "00") {
                        swal({
                            title: "Successful!",
                            text: "product updated successfully",
                            icon: "success",
                            closeOnClickOutside: false,
                            closeOnEsc: false
                        })
                            .then(() => {
                                $('#EditStoreProductModal').modal('hide');
                                loadStoreProduct();
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
                }
            },
            error: function (xhr) {
               // alert('Woow something went wrong'); 
            }
        });
    }

    function AddStoreProduct() {
        var AddStoreProductRequest = {
            ProductName: $('#ProductName').val(),
            CategoryId: parseInt($('#CategoryId').val()),
            SubCategoryId: $('#SubCategoryId').val(),
        };

        var token = $('#jwtToken').val();
        var bearerToken = 'Bearer ' + token;

        var applicationUrl = baseUrl + 'v1.0/StoreProduct/CreateStoreProduct';
        $.ajax({
            headers: {
                'Authorization': bearerToken
            },
            url: applicationUrl,
            type: 'POST',
            contentType: 'application/json',
            data: JSON.stringify(AddStoreProductRequest),
            success: function (data) {
                if (data.responseCode == "00") {
                    swal({
                        title: "Successful!",
                        text: "product added successfully",
                        icon: "success",
                        closeOnClickOutside: false,
                        closeOnEsc: false
                    })
                        .then(() => {
                            $('#myModal').modal('hide');
                            loadStoreProduct();
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
$(document).on({
    ajaxStart: function () {
        $('#cover-spin').show(0);
    },
    ajaxStop: function () {
        $('#cover-spin').hide();
    }
});