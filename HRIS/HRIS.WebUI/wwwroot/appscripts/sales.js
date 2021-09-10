
$(document).ready(function () {
    var baseUrl = $('#ApiBaseUrl').val();
    var token = $('#jwtToken').val();
    var siteLocation = $('#siteLocation').val();
    var bearerToken = 'Bearer ' + token;
    var productDetailList = [];
    var productDetailList2 = [];
    var serialNumber = 0;
    var serialNumber2 = 0;
    var branchId = parseInt($('#branchId').val());

    //getBranchProducts();
    //function getBranchProducts() {
    //    let productUrl = baseUrl + "v1.0/Products/get-allocated-branchproducts";
    //    var productDataLoader = DevExpress.data.AspNet.createStore({
    //        key: "branchProductId",
    //        loadUrl: productUrl,
    //        onBeforeSend: function (operation,
    //            ajaxSettings) {
    //            ajaxSettings.beforeSend = function (xhr) {
    //                xhr.setRequestHeader('branchId', branchId);
    //            },
    //                ajaxSettings.global = false;
    //        }
    //    });
    //    var productDataSource = new DevExpress.data.DataSource({
    //        pageSize: 10,
    //        paginate: true,
    //        store: productDataLoader
    //    });

    //    $("#productIdDevEx").dxSelectBox({
    //        dataSource: productDataSource,
    //        valueExpr: "branchProductId",
    //        displayExpr: "productName",
    //        searchEnabled: true,
    //        hoverStateEnabled: true,
    //        searchExpr: ["productName", "branchProductId"],
    //        placeholder: "Enter products's name to search",
    //        showClearButton: true,
    //        onValueChanged: function (data) {
    //            LoadProductDetails();
    //            window.localStorage.removeItem('selectedProductId');
    //            window.localStorage.setItem('selectedProductId', data.value);
    //        }
    //    }).dxSelectBox("instance");
    //}
    


    function LoadProductDetails() {
        let productid = window.localStorage.getItem('selectedProductId');
        if (productid == null || productid == "" || parseInt(productid) <= 0) {

        }
        else {
            GetProductDetails(productid);
        }
    }


    $(document).on('click', '#myBtn', function () {
        var ctl = this;
        var id = $(this).attr("data-id") 
        var row = $("#productTable button[data-id='" + id + "']")
            .parents("tr")[0];

        var productRemoved = $.grep(productDetailList, function (e) { return e.serNum == parseInt(id); });
        //let productRemoved = productDetailList.find(x => x.serNum === parseInt(id));

        let vatp = productRemoved[0].vatValue;
        let discnt = productRemoved[0].discount;
        let tamount = productRemoved[0].subTotal;
        let tmt = (parseInt(tamount) - (parseInt(vatp) + parseInt(discnt)))
        DeleteVatFromTotalVatPayable(vatp);
        DeleteDiscountFromTotalDiscount(discnt);
        DeleteSubtotalFromTotalAmount(tmt)
        productDetailList.splice(productDetailList.findIndex(function (i) {
            
            return i.serNum === id;
        }), 1);
        productDetailList2.splice(productDetailList2.findIndex(function (i) {
            return i.serNum === id;
        }), 1);
       
        // Remove original product
        $(row).remove();


    });
    //On change of product drop down
    $('#productId').change(function () {
        let productid = "#productId";
        let productidd = $(productid).val()
        if (productidd == null || productidd <= 0) {

        }
        else {
            GetProductDetails(productidd);
        }

    });
    ////Add product to list button
    $('#btn_AddProduct').click(function () {
        let prodIt = document.getElementById('productId').value;
        let salQty = document.getElementById('SaleQuantity').value;
        if (prodIt === "" || prodIt === null) {
            swal({
                title: "warning!",
                text: "Please select a product",
                icon: "warning",
                closeOnClickOutside: true,
                closeOnEsc: true
            }).then(() => {

            });
        }
        else {
            if ((salQty === "0")) {
                swal({
                    title: "warning!",
                    text: "Please attach a sale quantity",
                    icon: "warning",
                    closeOnClickOutside: true,
                    closeOnEsc: true
                }).then(() => {

                });
            }
            else if (parseFloat(salQty) > parseFloat($('#Quantity').val().replace(/,/g, ''))) {
                swal({
                    title: "warning!",
                    text: "Sale quantity cannot be greater than available quantity",
                    icon: "warning",
                    closeOnClickOutside: true,
                    closeOnEsc: true
                }).then(() => {

                });

            }
            //else if ($('#Quantity').val() === "") {
            //    swal({
            //        title: "warning!",
            //        text: "The selected product does ",
            //        icon: "warning",
            //        closeOnClickOutside: true,
            //        closeOnEsc: true
            //    }).then(() => {

            //    });

            //}
            else {
                AddProductToList();
            }
        }


    });



    //Add product to list button
    //$('#btn_AddProduct').click(function () {
    //    let prodIt = window.localStorage.getItem('selectedProductId');
    //    let salQty = document.getElementById('SaleQuantity').value;
    //    if (prodIt === "" || prodIt === null) {
    //        swal({
    //            title: "warning!",
    //            text: "Please select a product",
    //            icon: "warning",
    //            closeOnClickOutside: true,
    //            closeOnEsc: true
    //        }).then(() => {

    //        });
    //    }
    //    else {
    //        if ((salQty === "0")) {
    //            swal({
    //                title: "warning!",
    //                text: "Please attach a sale quantity",
    //                icon: "warning",
    //                closeOnClickOutside: true,
    //                closeOnEsc: true
    //            }).then(() => {

    //            });
    //        }
    //        else if (parseFloat(salQty) > parseFloat($('#Quantity').val().replace(/,/g, ''))) {
    //            swal({
    //                title: "warning!",
    //                text: "Sale quantity cannot be greater than available quantity",
    //                icon: "warning",
    //                closeOnClickOutside: true,
    //                closeOnEsc: true
    //            }).then(() => {

    //            });

    //        }
    //        //else if ($('#Quantity').val() === "") {
    //        //    swal({
    //        //        title: "warning!",
    //        //        text: "The selected product does ",
    //        //        icon: "warning",
    //        //        closeOnClickOutside: true,
    //        //        closeOnEsc: true
    //        //    }).then(() => {

    //        //    });

    //        //}
    //        else {
    //            AddProductToList();
    //        }
    //    }


    //});


    //Find customer on key up
    $("#CustomerPhone").keyup(function () {
        let phone = document.getElementById('CustomerPhone').value;
        getCustomerNameByPhone(phone);
    });

    //Find voucher on key up
    $("#DiscountVoucher").keyup(function () {
        let voucher = document.getElementById('DiscountVoucher').value;
        let phoneNo = document.getElementById('CustomerPhone').value;
        getVoucherByVoucherNo(voucher, phoneNo);
    });

    $("#btn_ProcessBill").click(function () {
        //if (productDetailList.length <= 0) {
        //    swal({
        //        title: "warning!",
        //        text: "Please add atleast one product",
        //        icon: "warning",
        //        closeOnClickOutside: true,
        //        closeOnEsc: true
        //    }).then(() => {

        //    });
        //}
        //else {

        //}
        ProcessBill();
        $('#btn_saveEntry').show();
    });
    $("#btn_saveEntry").click(function () {

        //if (productDetailList.length <= 0) {
        //    swal({
        //        title: "warning!",
        //        text: "Please add atleast one product",
        //        icon: "warning",
        //        closeOnClickOutside: true,
        //        closeOnEsc: true
        //    }).then(() => {

        //    });
        //}
        //else {

        //}
        ProcessSaleNow();
    });

    function ProcessBill() {
        let voucherNo = document.getElementById('DiscountVoucher').value;
        let voucherAmount = 0
        if (voucherNo === "") {

        }
        else {
            let vocherBalance = document.getElementById('VoucherBalance').value;
            voucherAmount = vocherBalance
        }
        let tAmount = document.getElementById('totalAmount').value;
        let tVat = document.getElementById('totalVatPaid').value;
        let tDiscount = document.getElementById('totalDiscount').value;
        let tAmtPaid = parseFloat($('#AmountPaid').val().replace(/,/g, ''))
        let tVoucherBal = document.getElementById('VoucherBalance').value;

        //let totalAmountToPay = CalculateTotalAmountToPay(parseInt(tAmount), parseInt(tVat), voucherAmount, parseInt(tDiscount));

        document.getElementById('totalAmountToPay').value = (parseInt(tAmount));
        document.getElementById('totalAmountToPayShown').value = numberWithCommas(tAmount);
        document.getElementById('totalVatToPay').value = tVat;
        document.getElementById('totalVatToPayShown').value = numberWithCommas(tVat);
        document.getElementById('totalDiscountEarned').value = tDiscount;
        document.getElementById('totalDiscountEarnedShown').value = numberWithCommas(tDiscount);
        if (tVoucherBal === "" || tVoucherBal === 0) {
            document.getElementById('totalAmountPaid').value = tAmtPaid;
            document.getElementById('totalAmountPaidShown').value = numberWithCommas(tAmtPaid);
            document.getElementById('Balance').value = (tAmount - parseInt(tAmtPaid));
            document.getElementById('BalanceShown').value = numberWithCommas((tAmount - parseInt(tAmtPaid)));
        }
        else {
            if (parseInt(tVoucherBal) >= parseInt(tAmount)) {
                let balAfter = (parseInt(tVoucherBal) - parseInt(tAmount));
                document.getElementById('totalAmountPaid').value = tAmount;
                document.getElementById('totalAmountPaidShown').value = numberWithCommas(tAmount);
                document.getElementById('BalanceAfterVoucher').value = balAfter;
                document.getElementById('Balance').value = 0;
                $('#voucherBalDiv').show();
            }
            else {
                let totalPaid = (parseInt(tVoucherBal) + parseInt(tAmtPaid));
                $('#totalAmountPaid').val(totalPaid);
                $('#totalAmountPaidShown').val(numberWithCommas(totalPaid));
                $('#BalanceAfterVoucher').val(0);
                document.getElementById('Balance').value = (parseInt(tAmount) - parseInt(totalPaid));
                $('#voucherBalDiv').show();
            }
        }



    }

    function CalculateTotalAmountToPay(totalAmt, vattotal, voucherAmt, discnt) {
        let total = (totalAmt - (vattotal + vattotal + voucherAmt, + discnt));
        return total;
    }
    function numberWithCommas(x) {
        return x.toString().replace(/\B(?=(\d{3})+(?!\d))/g, ",");
    }
    function GetProductDetails(id) {

        let getProductDetailsUrl = baseUrl + 'v1.0/Products/GetById/' + id;
        $.ajax({
            headers: {
                'Authorization': bearerToken
            },
            url: getProductDetailsUrl,
            type: 'GET',
            contentType: 'application/json',
            traditional: true,
            data: id,
            success: function (response) {
                document.getElementById("Vat").value = response.data.vatPercent;
                document.getElementById("Quantity").value = response.data.quantityRemaning;
                document.getElementById("Discount").value = response.data.discount;
                document.getElementById("UnitPrice").value = response.data.sellingPrice;
                document.getElementById("UnitPrice11").value = numberWithCommas(response.data.sellingPrice);
                document.getElementById("ProductName").value = response.data.productName;
                document.getElementById("productId").value = response.data.branchProductId;
            },
            error: function (err, result) {
                // alert("Error occurred" + err.responseText);
            }
        });
    }


    function AddProductToList() {
        let productVat = parseFloat($('#Vat').val());
        let productId = $('#productId').val();
        let discount = parseFloat($('#Discount').val());
        let saleQuantity = parseFloat($('#SaleQuantity').val());
        let unitPrice = parseFloat($('#UnitPrice').val().replace(/,/g, ''));
        let productAmount = (saleQuantity * unitPrice).toString();

        discount = (discount / 100 * productAmount);

        let vatPaid = CalculateVatOnProduct(productVat, productAmount);
        var productName = $('#ProductName').val();
       
        var productObj = {
            serNum: serialNumber + 1,
            productName: productName,
            vatValue: vatPaid,
            sellingPrice: unitPrice,
            discount: discount.toString(),
            vatPercent: productVat,
            quantity: saleQuantity,
            productId: productId,
            subTotal: productAmount,
            BranchId: branchId
        }
        var productObj2 = {
            vatValue: vatPaid.toString(),
            sellingPrice: unitPrice,
            discount: discount,
            vatPercent: productVat,
            quantity: saleQuantity,
            productId: productId,
            subTotal: productAmount,
            BranchId: branchId
        }

        productDetailList.push(productObj);
        productDetailList2.push(productObj2);
      
        computeTotalVatPayable(vatPaid); //increment total vat shown below
        computeTotalDiscount(discount); //increment total  discount
        computeTotalAmount(productAmount, discount, vatPaid);
        //increment total  amount
        LoadProductDetailList();

        serialNumber++;
        $('#productForm')[0].reset();
        $('#productId').val("").trigger("chosen:updated");
    }



    ////DevExpress
    //function AddProductToList() {
    //    let productVat = parseFloat($('#Vat').val());
    //    let productId = window.localStorage.getItem('selectedProductId');
    //    let discount = parseFloat($('#Discount').val());
    //    let saleQuantity = parseFloat($('#SaleQuantity').val());
    //    let unitPrice = parseFloat($('#UnitPrice').val().replace(/,/g, ''));
    //    let productAmount = (saleQuantity * unitPrice).toString();

    //    discount = (discount / 100 * productAmount);

    //    let vatPaid = CalculateVatOnProduct(productVat, productAmount);
    //    var productName = $('#ProductName').val();

    //    var productObj = {
    //        serNum: serialNumber + 1,
    //        productName: productName,
    //        vatValue: vatPaid,
    //        sellingPrice: unitPrice,
    //        discount: discount.toString(),
    //        vatPercent: productVat,
    //        quantity: saleQuantity,
    //        productId: productId,
    //        subTotal: productAmount,
    //        BranchId: branchId
    //    }
    //    var productObj2 = {
    //        vatValue: vatPaid.toString(),
    //        sellingPrice: unitPrice,
    //        discount: discount,
    //        vatPercent: productVat,
    //        quantity: saleQuantity,
    //        productId: productId,
    //        subTotal: productAmount,
    //        BranchId: branchId
    //    }

    //    productDetailList.push(productObj);
    //    productDetailList2.push(productObj2);

    //    computeTotalVatPayable(vatPaid); //increment total vat shown below
    //    computeTotalDiscount(discount); //increment total  discount
    //    computeTotalAmount(productAmount, discount, vatPaid);
    //    //increment total  amount
    //    LoadProductDetailList();

    //    serialNumber++;
    //    $('#productForm')[0].reset();
    //   // $('#productId').val("").trigger("chosen:updated");
    //}

    function getVoucherDetails(voucherNo, phone) {
        let getCustomerByPhoneUrl = baseUrl + 'v1.0/Voucher/GetVoucherByNo/' + voucherNo + '/' + phone;
        $.ajax({
            headers: {
                'Authorization': bearerToken,
                'branchId': branchId
            },
            url: getCustomerByPhoneUrl,
            type: 'GET',
            contentType: 'application/json',
            traditional: true,
            data: phone,
            success: function (response) {

                return response.data.balance;

            },
            error: function (err, result) {
                // alert("Error occurred" + err.responseText);
            }
        });
    }
    function getVoucherByVoucherNo(voucherNo, phone) {
        let getCustomerByVoucherUrl = baseUrl + 'Voucher/GetVoucherByNo/' + voucherNo + '/' + phone;
        $.ajax({
            headers: {
                'Authorization': bearerToken,
                'branchId': branchId
            },
            url: getCustomerByVoucherUrl,
            type: 'GET',
            contentType: 'application/json',
            traditional: true,
            data: phone,
            success: function (data) {

                if (data.responseCode === "00") {
                    swal({
                        title: "Successful!",
                        text: "Voucher is valid and active",
                        icon: "success",
                        closeOnClickOutside: true,
                        closeOnEsc: true
                    }).then(() => {
                        document.getElementById('VoucherBalance').value = data.data.balance;
                        document.getElementById('vouchId').value = data.data.voucherId;
                    });
                }
                else {
                    $('#DiscountVoucher').val('');
                }

            },
            error: function (err, result) {
                // alert("Error occurred" + err.responseText);
            }
        });
    }

    function computeTotalVatPayable(amount) {
        let v = document.getElementById('totalVatPaid').value;
        v = (parseInt(v) + (parseInt(amount)));
        document.getElementById('totalVatPaid').value = v;
    }
    function DeleteVatFromTotalVatPayable(amount) {
        let v = document.getElementById('totalVatPaid').value;
        v = (parseInt(v) - (parseInt(amount)));
        document.getElementById('totalVatPaid').value = v;
    }
    function computeTotalDiscount(amount) {
        let v = document.getElementById('totalDiscount').value;
        v = (parseInt(v) + (parseInt(amount)));
        document.getElementById('totalDiscount').value = v;
    }
    function DeleteDiscountFromTotalDiscount(amount) {
        let v = document.getElementById('totalDiscount').value;
        v = (parseInt(v) - (parseInt(amount)));
        document.getElementById('totalDiscount').value = v;
    }
    function computeTotalAmount(amount, discount, vat) {
        let v = document.getElementById('totalAmount').value;
        let vDis = document.getElementById('totalDiscount').value;
        v = ((parseInt(v) + ((parseInt(amount) - parseInt(discount)) + vat)));
        document.getElementById('totalAmount').value = v;
        document.getElementById('totalAmountShown').value = numberWithCommas(v);
    }
    function DeleteSubtotalFromTotalAmount(amount) {
        let v = document.getElementById('totalAmount').value;
        v = (parseInt(v) - parseInt(amount));
        document.getElementById('totalAmount').value = v;
        document.getElementById('totalAmountShown').value = numberWithCommas(v);
    }


    function CalculateVatOnProduct(vat, amount) {
        let vatPercent = 100.0;
        vatPaid = (vat / vatPercent) * amount;
        return vatPaid;
    }
    //function LoadProductDetailList() {
    //    var rowIndex = 1;
    //    $('#table_product_details_body').empty();
    //    $.each(productDetailList, function (index, item) {

    //        var eachrow = "<tr>"
    //            + "<td>" + (rowIndex++) + "</td>"
    //            + "<td>" + item.productName + "</td>"
    //            + "<td>" + item.sellingPrice.toString().replace(/\B(?=(\d{3})+(?!\d))/g, ",") + "</td>"
    //            + "<td>" + item.quantity + "</td>"
    //            + "<td>" + item.vatValue.toString().replace(/\B(?=(\d{3})+(?!\d))/g, ",") + "</td>"
    //            + "<td>" + item.discount.toString().replace(/\B(?=(\d{3})+(?!\d))/g, ",") + "</td>"
    //            + "<td>" + item.subTotal.toString().replace(/\B(?=(\d{3})+(?!\d))/g, ",") + "</td>"
    //            + "<td>" + "<button type='button' onclick='removeProduct(" + item.productId + ")' class='btn btn-danger' > " +
    //            "<span class='glyphicon glyphicon-remove' />" +
    //            "</button>" +
    //            "</td>"
    //            + "</tr>";
    //        $('#table_product_details_body').append(eachrow);
    //    });
    //}
    function LoadProductDetailList() {
        var rowIndex = 1;
        $('#productTable tbody').empty();
        $.each(productDetailList, function (index, item) {

            var eachrow = "<tr>"
                + "<td>" + (rowIndex++) + "</td>"
                + "<td>" + item.productName + "</td>"
                + "<td>" + item.sellingPrice.toString().replace(/\B(?=(\d{3})+(?!\d))/g, ",") + "</td>"
                + "<td>" + item.quantity + "</td>"
                + "<td>" + item.vatValue.toString().replace(/\B(?=(\d{3})+(?!\d))/g, ",") + "</td>"
                + "<td>" + item.discount.toString().replace(/\B(?=(\d{3})+(?!\d))/g, ",") + "</td>"
                + "<td>" + item.subTotal.toString().replace(/\B(?=(\d{3})+(?!\d))/g, ",") + "</td>"
                + "<td>" +
                "<button type='button' class='btn btn-danger' id='myBtn' data-id='" + item.serNum + "' > " +
                "<span class='glyphicon glyphicon-remove' />" +
                "</button>" +
                "</td>"
                + "</tr>";
            if ($("#productTable tbody").length == 0) {
                $("#productTable").append("<tbody></tbody>");
            }
            $("#productTable tbody").append(eachrow);
            // $('#table_product_details_body').append(eachrow);
        });
    }




    function removeProduct(id) {
        var lol = [];

        // Find Product in <table>
        var row = $("#productTable button[data-id='" + id + "']")
            .parents("tr")[0];

        // Remove original product
        $(row).remove();
        //for (var key in productDetailList) {

        //    console.log(key);
        //    //if (productDetailList[key].data == id) {

        //    //} else {
        //    //    let pol = {
        //    //        productName: productName,
        //    //        vatValue: vatPaid,
        //    //        sellingPrice: unitPrice,
        //    //        discount: discount,
        //    //        vatPercent: productVat,
        //    //        quantity: saleQuantity,
        //    //        productId: productId,
        //    //        subTotal: productAmount
        //    //    }
        //    //}
        //}
        ////let productId = id;
        ////let index = productDetailList.findIndex(
        ////    i => i.productId === productId);
        ////productDetailList.splice(index, 1)
    }
    //function PostSales() {

    //}

    function getCustomerNameByPhone(phone) {
        removeProduct();
        let getCustomerByPhoneUrl = baseUrl + 'v1.0/Customer/GetCustomerByPhone/' + phone;
        $.ajax({
            headers: {
                'Authorization': bearerToken,
                'branchId': branchId
            },
            url: getCustomerByPhoneUrl,
            type: 'GET',
            contentType: 'application/json',
            traditional: true,
            data: phone,
            success: function (response) {
                if (response.responseCode === "00") {
                    swal({
                        title: "Successful!",
                        text: "We found the customer",
                        icon: "success",
                        closeOnClickOutside: true,
                        closeOnEsc: true
                    }).then(() => {
                        document.getElementById("CustomerFullName").value = response.data.fullName;
                    });

                }

            },
            error: function (err, result) {
                // alert("Error occurred" + err.responseText);
            }
        });
    }

    function ProcessSaleNow() {

        let modeOfPayment = document.getElementById('ModeOfPayment').value;
        let saleDetail = productDetailList2;

        let saleMaster = new Object()
        saleMaster.CustomerName = document.getElementById('CustomerFullName').value;
        saleMaster.CustomerPhone = document.getElementById('CustomerPhone').value;
        saleMaster.TotalVat = document.getElementById('totalVatToPay').value;
        saleMaster.TotalPaid = document.getElementById('totalAmountPaid').value;
        saleMaster.TotalDiscount = document.getElementById('totalDiscountEarned').value;
        saleMaster.ModeOfPayment = document.getElementById('ModeOfPayment').value;
        saleMaster.DatePaid = new Date();
        saleMaster.branchId = branchId;

        if (modeOfPayment === "Credit") {
            saleMaster.isPaid = false;
        }
        else {
            saleMaster.isPaid = true;
        }

        let tVoucher = document.getElementById('VoucherBalance').value;
        if (tVoucher === "" || tVoucher === null) {

        }
        else {
            saleMaster.isVoucherUsed = true;
            saleMaster.CustomerVoucherId = document.getElementById('vouchId').value;
        }


        let modelDTO = {
            detail: saleDetail,
            sales: saleMaster
        }

        PostSale(modelDTO);

    }

    function PostSale(modelDTO) {

        var token = $('#jwtToken').val();
        var bearerToken = 'Bearer ' + token;

        var applicationUrl = baseUrl + 'v1.0/Sales/CreateSales';
        $.ajax({
            headers: {
                'Authorization': bearerToken
            },
            url: applicationUrl,
            type: 'POST',
            contentType: 'application/json',
            data: JSON.stringify(modelDTO),
            success: function (data) {

                //alert(JSON.stringify(data));
                if (data.responseCode == "00") {
                    swal({
                        title: "Successful!",
                        text: data.message,
                        icon: "success",
                        closeOnClickOutside: false,
                        closeOnEsc: false,
                        showCancelButton: false,
                        confirmButtonColor: "#b21db8",
                        confirmButtonText: "Print Receipt!",
                        closeOnConfirm: false,
                        closeOnCancel: false
                    })
                        .then(() => {
                            window.location.href = siteLocation + "/Index/Receipts?billNo=" + data.data.billNumber;
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
                //alert(xhr.responseText); 
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