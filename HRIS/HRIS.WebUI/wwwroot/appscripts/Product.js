$(function () {

    var baseUrl = $('#ApiBaseUrl').val();
    var token = $('#jwtToken').val();
    var branchId = $('#branchId').val();
    var bearerToken = 'Bearer ' + token;
    var categoryId = 0;
    var branchId = $('#branchId').val();

    $('#Expire').hide();
    $('#ProductType').hide();
    


    $('#IsVat').change(function () {
        let v = $('#IsVat').val();
        if (v === "Yes") {
            
            $.ajax({
                    headers: {
                    'Authorization': bearerToken,
                    'BranchId': branchId
                    },
                    url: baseUrl +'Vat/GetFirstVat',
                    type: 'GET',
                    contentType: 'application/json',
                    success: function (data) {
                    let output = JSON.stringify(data);

                        $('#Vat').val(output);  
                        $('#vatDiv').show();
                    },
                    error: function (xhr) {
                        //alert('Woow something went wrong');
                    }
                });
        }
        else {
            $('#Vat').val(0);
            $('#vatDiv').hide();
        }
    });

    $('#CanExpire').change(function () {
        let v = $('#CanExpire').val();
        if (v === "1") {
            $('#expire').show();
        } else {
            $('#expire').hide();
        }
    });

    $('#IsProductType').change(function () {
        let v = $('#IsProductType').val();
        if (v === "Yes") {
            $('#ProductType').show();
        } else {
            $('#ProductType').hide();
        }
    });

    //$('#isVendor').change(function () {
    //    let v = $('#isVendor').val();
    //    if (v === "Yes") {
    //        $('#vendor').show();
    //    } else {
    //        $('#vendor').hide();
    //    }
    //});

    $('#IsDiscount').change(function () {
        let v = $('#IsDiscount').val();
        if (v === "Yes") {
            $('#discountDiv').show();
        } else {
            $('#discountDiv').hide();
        }
    });

    $('#ProductAllocationUseId').change(function () {

        let id = $('#ProductAllocationUseId').val();


        if (id != null) {

            $.ajax({
                headers: {
                    'Authorization': bearerToken,
                    'BranchId': branchId
                },
                url: baseUrl + 'v1.0/Products/GetProductQuantityRemaining/' + id,
                type: 'GET',
                datatype: 'json',
                success: function (response) {
                    $('#QuantityRemaining').val(response.data);
                }
            });
        }
    });

    //$('#Quantity').keyup(function () {

    //    let price = parseFloat($('#SellPrice').val());
    //    let quantity = parseInt($('#Quantity').val());

    //    let amount = price * quantity;

    //    $('#amount').val(amount);

    //});


    $('#CriticalLevel').keyup(function () {

        let quantity = parseInt($('#QuantityRemaining').val());
        let criticallevel = parseInt($('#CriticalLevel').val());


        if (quantity <= criticallevel) {
            swal({
                title: "failed!",
                text:"You can't enter value less than the Quantity",
                icon: "danger",
                closeonclickoutside: false,
                closeonesc: false
               })
                .then(() => {
                    $('#CriticalLevel').val(0);
                });
        }
    });


    $('#btn_EditPrduct').click(function () {
        let  isvalid = true;
        if ($('#SellPrice').val() === "0") {
            isvalid = false;
            $('#SellPriceSpan').html("Amount should be greater than 0");
        }
        if ($('#CanExpire').val() === "1") {
            if ($('#ManufactureDate').val() == "0001-01-01") {
                isvalid = false;
                $('#manufactureDateSpan').html("please input a valid date")
            }
            if ($('#ExpiryDate').val() == "0001-01-01") {
                isvalid = false;
                $('#expiryDateSpan').html("please input a valid date")
            }
        }
        if ($('#IsDiscount').val() === "Yes") {
            if ($('#Discount').val() == "0") {
                isvalid = false;
                $('#discountSpan').html("please input a value greater than 0")
            }
           
        }
        if ($('#IsProductType').val() === "Yes") {
            if ($('#VariantNameId').val() == "") {
                isvalid = false;
                $('#productVarSpan').html("please select a valid value")
            }
            if ($('#VariantQty').val() === "0") {
                isvalid = false;
                $('#variantQtySpan').html("please enter a value greater than 0")
            }
           
        }
        if ($('#CriticalLevel').val() === "0") {
            isvalid = false;
            $('#CriticalLevelSpan').html("Amount should be greater than 0")
        }

        if (isvalid === false) {
            

            return false;
        }
        else {
            EditProduct();
        }




    });



    //$('#btn-EditProduct').click(function () {
    //    EditProduct();
    //});


    loadProduct();
   // categorySelect();

    function loadProduct() {
        var remoteDataLoader = window.DevExpress.data.AspNet.createStore({
            key: "branchProductId",
            loadUrl: baseUrl + "v1.0/Products/get-allocated-branch-products",
            onBeforeSend: function (operation,
                ajaxSettings) {
                ajaxSettings.beforeSend = function (xhr) {
                    xhr.setRequestHeader('Authorization', bearerToken);
                    xhr.setRequestHeader('BranchId', branchId);
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
                        caption: "Product Name",
                        sortIndex: 0,
                        sortOrder: 'asc',
                        //fixed: true,
                        cssClass: 'font-bold'

                    },
                    {
                        dataField: "quantityRemaning",
                        caption: "Quantity",
                        sortIndex: 0,
                        sortOrder: 'asc',
                        //fixed: true,
                        cssClass: 'font-bold'

                    },
                    {
                        dataField: "sellingPrice",
                        caption: "Selling Price",
                        sortIndex: 0,
                        sortOrder: 'asc',
                        //fixed: true,
                        cssClass: 'font-bold'

                    },
                    {
                        dataField: "criticalLevel",
                        caption: "Critical Level",
                        sortIndex: 0,
                        sortOrder: 'asc',
                        //fixed: true,
                        cssClass: 'font-bold'

                    },
                    {
                        dataField: "createdOn",
                        caption: "Date Allocated",
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
                                        if (options.data.vatPercent == 0) {
                                            $('#IsVat').val("No").trigger("chosen:updated");
                                            $('#Vat').val(0);
                                            $('#vatDiv').hide();
                                        }
                                        else {
                                            $('#IsVat').val("Yes").trigger("chosen:updated");
                                            $('#Vat').val(options.data.vatPercent);
                                            $('#vatDiv').show();
                                        }

                                        if (options.data.discount == 0) {
                                            $('#IsDiscount').val("No").trigger("chosen:updated");;
                                            $('#Discount').val(0);
                                            $('#discountDiv').hide();
                                        }
                                        else {
                                            $('#IsDiscount').val("Yes").trigger("chosen:updated");;
                                            $('#Discount').val(options.data.discount);
                                            $('#discountDiv').show();
                                        }
                                        if (options.data.canExpire == false) {
                                            $('#CanExpire').val("0").trigger("chosen:updated");;
                                            $('#expire').hide();
                                        }
                                        else {
                                            $('#CanExpire').val("1").trigger("chosen:updated");;
                                            $('#expire').show();
                                            $('#ManufactureDate').val(options.data.manufactureDate);
                                            $('#ExpiryDate').val(options.data.expiryDate);

                                        }
                                        if (options.data.productTypeVariationId == null) {
                                            $('#IsProductType').val("No").trigger("chosen:updated");;
                                            $('#ProductType').hide();
                                        }
                                        else {
                                            $('#IsProductType').val("Yes").trigger("chosen:updated");;
                                            $('#VariantNameId').val(options.data.productTypeVariationId).trigger("chosen:updated");
                                            $('#VariantQty').val(options.data.variationQuantity);
                                            $('#ProductType').show();

                                        }

                                        $('#CriticalLevel').val(options.data.criticalLevel);
                                        $('#ProductName').val(options.data.productName);
                                        $('#SellPrice').val(options.data.sellingPrice);
                                        $('#BranchProductId').val(options.data.branchProductId);


                                        $('#productEditModal').modal('show');
                                    }
                                }
                            ).appendTo(container);

                        },
                        visible: true
                    }
                ],

            };

        dataGrid = window.$("#ProductContainer").dxDataGrid(gridOptions).dxDataGrid("instance");
    }

   
    function formatDate(date) {
        var d = new Date(date),
            month = '' + (d.getMonth() + 1),
            day = '' + d.getDate(),
            year = d.getFullYear();

        if (month.length < 2)
            month = '0' + month;
        if (day.length < 2)
            day = '0' + day;

        return [year, month, day].join('-');
    }

    function EditProduct() {

        let d = formatDate(new Date().toString());
       
        var AddProductRequest = {
            sellingPrice: parseFloat($('#SellPrice').val().replace(/,/g, '')),
            canExpire: $('#CanExpire').val() == '1' ? true:false,
            manufactureDate: $('#ManufactureDate').val(),
            ExpiryDate: $('#ExpiryDate').val(),
            Discount: $('#Discount').val(),
            ProductTypeVariationId: $('#VariantNameId').val() == "" ? null : $('#VariantNameId').val(),
            BranchProductId: $('#BranchProductId').val(),
            variationQuantity: $('#VariantQty').val(),
            CriticalLevel: $('#CriticalLevel').val(),
            VatPercent: $('#Vat').val(),
            BranchId: branchId
        };


        console.log(JSON.stringify(AddProductRequest));
        

        
         //var bearertoken = 'Bearer ' + token;

        var applicationurl = baseUrl + 'v1.0/products/UpdateProduct';
        $.ajax({
            headers: {
                'Authorization': bearerToken
            },
            url: applicationurl,
            type: 'POST',
            contentType: 'application/json',
            data: JSON.stringify(AddProductRequest),
            success: function (data) {
                if (data.statusCode = 200) {
                    swal({
                        title: "successful!",
                        text: "Successfully updated a product",
                        icon: "success",
                        closeonclickoutside: false,
                        closeonesc: false
                    }).then(() => {
                        $('#productEditModal').modal('hide');
                         loadProduct();
                       });
                }
            },
            error: function (xhr) {
                swal({
                        title: "failed!",
                        text: data.message,
                        icon: "danger",
                        closeonclickoutside: false,
                        closeonesc: false
                    })
                        .then(() => {

                   });
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