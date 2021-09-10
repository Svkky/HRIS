$(function () {

    $('#actionLoader1').hide();
    $('#actionLoader2').hide();
    var baseUrl = $('#ApiBaseUrl').val();
    var token = $('#jwtToken').val();
    var bearerToken = 'Bearer ' + token;
   // var branchId = parseInt($('#branchId').val());






    //getBranchProducts();
    //function getBranchProducts() {
    //    let productUrl = baseUrl + "v1.0/StoreProduct/get-all-store-products";
    //    let bearerToken = 'Bearer ' + token;
    //    var productDataLoader = DevExpress.data.AspNet.createStore({
    //        key: "storeProductId",
    //        loadUrl: productUrl,
    //        onBeforeSend: function (operation,
    //            ajaxSettings) {
    //            ajaxSettings.beforeSend = function (xhr) {
    //                xhr.setRequestHeader('Authorization', bearerToken);
    //            },
    //                ajaxSettings.global = false;
    //        }
    //    });
    //    var productDataSource = new DevExpress.data.DataSource({
    //        pageSize: 10,
    //        paginate: true,
    //        store: productDataLoader
    //    });

    //    $("#productIdDevExAllocate").dxSelectBox({
    //        dataSource: productDataSource,
    //        valueExpr: "storeProductId",
    //        displayExpr: "productName",
    //        searchEnabled: true,
    //        hoverStateEnabled: true,
    //        searchExpr: ["productName", "storeProductId"],
    //        placeholder: "Enter products's name to search",
    //        showClearButton: true,
    //        onValueChanged: function (data) {
    //            loadproductDetails();
    //            window.localStorage.removeItem('selectedProductIdAllocate');
    //            window.localStorage.setItem('selectedProductIdAllocate', data.value);
    //        }
    //    }).dxSelectBox("instance");
    //}
















    $('#btn_AddProductAllocation').click(function () {
        let isvalid = true;
        if ($('#ProductId').val() === "") {
            $('#ProductId').css('border-color', 'Red');
            ($('#ProductId').focus());
            isvalid = false;
        }
        if ($('#AllocationQuantity').val() === "") {
            $('#AllocationQuantity').css('border-color', 'Red');
            ($('#AllocationQuantity').focus());
            isvalid = false;
        }
        if ((parseInt($('#AllocationQuantity').val()) > parseInt($('#qtyRemaining').val()))) {
            swal({
                title: "Warning!",
                text: "Quantity to be allocated must not be greater than the Qty remaining",
                icon: "warning",
                closeOnClickOutside: false,
                closeOnEsc: false
            })
                .then(() => {
                    isvalid = false;
                });
            isvalid = false;
        }
        if ((parseInt($('#AllocationQuantity').val()) == 0)) {
            swal({
                title: "Warning!",
                text: "Quantity to be allocated must be greater than 0",
                icon: "warning",
                closeOnClickOutside: false,
                closeOnEsc: false
            })
                .then(() => {
                    isvalid = false;
                });
            isvalid = false;
        }
        if ($('#BranchId').val() === "") {
            $('#BranchId').css('border-color', 'Red');
            ($('#BranchId').focus());
            isvalid = false;
        }
        if (isvalid == false) {
            return false;
        }
        else {
          
            AllocateProduct();
        }

    });

    //Dev express
    //$('#btn_AddProductAllocation').click(function () {
    //    let isvalid = true;
    //    let pid = window.localStorage.getItem('selectedProductIdAllocate');
    //    if (pid === "" || pid === null) {
    //        $('#productIdDevExAllocate').css('border-color', 'Red');
    //        ($('#productIdDevExAllocate').focus());
    //        isvalid = false;
    //    }
    //    if ($('#AllocationQuantity').val() === "") {
    //        $('#AllocationQuantity').css('border-color', 'Red');
    //        ($('#AllocationQuantity').focus());
    //        isvalid = false;
    //    }
    //    if ((parseInt($('#AllocationQuantity').val()) > parseInt($('#qtyRemaining').val()))) {
    //        swal({
    //            title: "Warning!",
    //            text: "Quantity to be allocated must not be greater than the Qty remaining",
    //            icon: "warning",
    //            closeOnClickOutside: false,
    //            closeOnEsc: false
    //        })
    //            .then(() => {
    //                isvalid = false;
    //            });
    //        isvalid = false;
    //    }
    //    if ((parseInt($('#AllocationQuantity').val()) == 0)) {
    //        swal({
    //            title: "Warning!",
    //            text: "Quantity to be allocated must be greater than 0",
    //            icon: "warning",
    //            closeOnClickOutside: false,
    //            closeOnEsc: false
    //        })
    //            .then(() => {
    //                isvalid = false;
    //            });
    //        isvalid = false;
    //    }
    //    if ($('#BranchId').val() === "") {
    //        $('#BranchId').css('border-color', 'Red');
    //        ($('#BranchId').focus());
    //        isvalid = false;
    //    }
    //    if (isvalid == false) {
    //        return false;
    //    }
    //    else {
          
    //        AllocateProduct();
    //    }

    //});
   
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
                        );
                    });
                }
            });
        }
    });
    $('#ProductId').change(function () {

        let id = $('#ProductId').val();


        if (id != null) {

            $.ajax({
                headers: {
                    'Authorization': bearerToken,
                },
                url: baseUrl + 'v1.0/ProductWarehouse/get-product-count/' + id,
                type: 'GET',
                datatype: 'json',
                success: function (response) {
                    document.getElementById("qtyRemaining").value = response.data;
                }
            });
        }
    });

    function loadproductDetails() {
        let id = window.localStorage.getItem('selectedProductIdAllocate');;


        if (id != null || id != "") {

            $.ajax({
                headers: {
                    'Authorization': bearerToken,
                },
                url: baseUrl + 'v1.0/ProductWarehouse/get-product-count/' + id,
                type: 'GET',
                datatype: 'json',
                success: function (response) {
                    document.getElementById("qtyRemaining").value = response.data;
                }
            });
        }
    }

    LoadProductAllocation();

    function LoadProductAllocation() {
        var remoteDataLoader = window.DevExpress.data.AspNet.createStore({
            key: "productAllocationId",
            loadUrl: baseUrl + "v1.0/ProductAllocation/get-allocated-product",
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
                        dataField: "productName",
                        caption: "Product",
                        sortIndex: 0,
                        sortOrder: 'asc',
                        //fixed: true,
                        cssClass: 'font-bold'

                    },
                    {
                        dataField: "allocationQuantity",
                        caption: "Qty Allocated",
                        sortIndex: 0,
                        sortOrder: 'asc',
                        //fixed: true,
                        cssClass: 'font-bold'

                    },
                    {
                        dataField: "branchName",
                        caption: "Branch",
                        sortIndex: 0,
                        sortOrder: 'asc',
                        //fixed: true,
                        cssClass: 'font-bold'

                    },
                    {
                        dataField: "dateCreated",
                        caption: "Allocated On",
                        sortIndex: 0,
                        sortOrder: 'asc',
                        //fixed: true,
                        cssClass: 'font-bold'

                    }
                    //<button class="btn btn-success btn-sm" type="button" data-toggle="modal" data-target="#Mod"><i class="fa fa-edit"></i>&nbsp;Edit</button>
                    //                                    <a class="btn btn-danger btn-sm" type="button" href="#"><i class="fa fa-list-ul"></i>&nbsp;Disable</a>
                    //{
                    //    "render": function (data, type, row) { return "<a data-toggle='modal' data-target='#Mod' type='button' class='btn btn-success btn-sm'  onclick=ViewMembers(''); ><i class='fa fa - edit'</a>"; }
                    //},
                    //{
                    //    caption: "Action",
                    //    width: 340,
                    //    alignment: "center",
                    //    cellTemplate: function (container, options) {

                    //        $('<div />').dxButton(
                    //            {
                    //                icon: 'fa fa-edit',
                    //                text: 'Edit',
                    //                type: 'success',
                    //                cssClass: 'btn btn-primary btn-sm',
                    //                alignment: "center",
                    //                onClick: function (e) {
                    //                    $('#SubCategoryId').val(options.data.subCategoryId);
                    //                    $('#editSubCategory_CategoryId').val(options.data.categoryId);
                    //                    $('#editSubCategory_Description').val(options.data.subCategoryName);

                    //                    $('#editSubCategoryModal').modal('show');
                    //                }
                    //            }
                    //        ).appendTo(container);
                    //        $('<div />').dxButton(
                    //            {
                    //                icon: 'fa fa-list-ul',
                    //                text: 'Delete',
                    //                cssClass: 'btn btn-danger btn-sm',
                    //                type: 'danger',
                    //                alignment: "center",
                    //                onClick: function (args) {
                    //                    swal({
                    //                        title: "Are you sure?",
                    //                        text: "This action cannot be undone.",
                    //                        type: "warning",
                    //                        showCancelButton: true,
                    //                        confirmButtonColor: "#DD6B55",
                    //                        cancelButtonColor: "#008000",
                    //                        confirmButtonText: "Yes, Delete it!",
                    //                        cancelButtonText: "No, cancel please!",
                    //                        closeOnConfirm: false,
                    //                        closeOnCancel: false
                    //                    }).then(function (event) {
                    //                        if (event == true) {
                    //                            DeleteSubCategory(options.data.subCategoryId)

                    //                        } else {

                    //                        }
                    //                    });

                    //                }
                    //            }
                    //        ).appendTo(container);
                    //    },
                    //    visible: true
                    //}
                ],

            };

        dataGrid = window.$("#AllocationContainer").dxDataGrid(gridOptions).dxDataGrid("instance");
    }

    function AllocateProduct() {
        //var AllocateProductRequest = {
        //    AllocationQuantity: parseInt($('#AllocationQuantity').val()),
        //    ProductId: parseInt(window.localStorage.getItem('selectedProductIdAllocate')),
        //    BranchId: parseInt($('#BranchId').val())
        //};
        var AllocateProductRequest = {
            AllocationQuantity: parseInt($('#AllocationQuantity').val()),
            ProductId: parseInt($('#ProductId').val()),
            BranchId: parseInt($('#BranchId').val())
        };

        var token = $('#jwtToken').val();
        var bearerToken = 'Bearer ' + token;

        var applicationUrl = baseUrl + 'v1.0/ProductAllocation/allocate';
        $.ajax({
            headers: {
                'Authorization': bearerToken
            },
            url: applicationUrl,
            type: 'POST',
            contentType: 'application/json',
            data: JSON.stringify(AllocateProductRequest),
            success: function (data) {
                if (data.responseCode == "00") {
                    swal({
                        title: "Successful!",
                        text: "product allocated successfully",
                        icon: "success",
                        closeOnClickOutside: false,
                        closeOnEsc: false
                    })
                        .then(() => {
                            $('#myModal').modal('hide');
                            LoadProductAllocation();
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