
$(function () {

    $('#actionLoader1').hide();
    $('#actionLoader2').hide();
    var baseUrl = $('#ApiBaseUrl').val();
    var token = $('#jwtToken').val();
    var bearerToken = 'Bearer ' + token;
    var branchId = parseInt($('#branchId').val());
    $('#btn-AddExpenditure').click(function () {
        let IsValid = true;
        if ($('#Description').val() === "") {
            $('#Description').css('border-color', 'Red');
            ($('#Description').focus());
            IsValid = false;
        }
        if ($('#Amount').val() === "") {
            $('#Amount').css('border-color', 'Red');
            ($('#Amount').focus());
            IsValid = false;
        }
        if ($('#ExpenditureDate').val() === "") {
            $('#ExpenditureDate').css('border-color', 'Red');
            ($('#ExpenditureDate').focus());
            IsValid = false;
        }

        if (IsValid == false) {
            return false
        }
        else {
            //$('#actionLoader1').show();
            //$('#actionLoader2').show();
            //$('#ibox1').children('.modal fade').toggleClass('sk-loading');
            AddExpenditure();
        }

    });
    //$('#btn-EditCustomer').click(function () {
    //    EditCustomer();
    //});

    LoadExpenditure();

    function LoadExpenditure() {
        var remoteDataLoader = window.DevExpress.data.AspNet.createStore({
            key: "expenditureID",
            loadUrl: baseUrl + "v1.0/Expenditure/GetAllExpendture",
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
                        dataField: "description",
                        caption: "Description",
                        sortIndex: 0,
                        sortOrder: 'asc',
                        //fixed: true,
                        cssClass: 'font-bold'

                    },
                    ,
                    {
                        dataField: "amount",
                        caption: "Amount",
                        sortIndex: 0,
                        sortOrder: 'asc',
                        //fixed: true,
                        cssClass: 'font-bold'

                    },
                    {
                        dataField: "expenditureDatee",
                        caption: "Date",
                        sortIndex: 0,
                        sortOrder: 'asc',
                        //fixed: true,
                        cssClass: 'font-bold'

                    },
                    {
                        dataField: "comment",
                        caption: "Comment",
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
                ],

            };

        dataGrid = window.$("#ExpenditureContainer").dxDataGrid(gridOptions).dxDataGrid("instance");
    }



    function AddExpenditure() {
        var AddExpenditureRequest = {
            Description: $('#Description').val(),
            ExpenditureDate: $('#ExpenditureDate').val(),
            Amount: parseFloat($('#Amount').val().replace(/,/g, '')),
            Comment: $('#Comment').val(),
        };

        var token = $('#jwtToken').val();
        var bearerToken = 'Bearer ' + token;

        var applicationUrl = baseUrl + 'v1.0/Expenditure/CreateExpenditure';
        $.ajax({
            headers: {
                'Authorization': bearerToken
            },
            url: applicationUrl,
            type: 'POST',
            contentType: 'application/json',
            data: JSON.stringify(AddExpenditureRequest),
            success: function (data) {
                if (data.responseCode == "00") {
                    swal({
                        title: "Successful!",
                        text: data.message,
                        icon: "success",
                        closeOnClickOutside: false,
                        closeOnEsc: false
                    })
                        .then(() => {
                            $('#myModal').modal('hide');
                            LoadExpenditure();
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
$(document).ready(function () {
    var date = new Date();

    var day = date.getDate();
    var month = date.getMonth() + 1;
    var year = date.getFullYear();

    if (month < 10) month = "0" + month;
    if (day < 10) day = "0" + day;

    var today = year + "-" + month + "-" + day + "T00:00";
    $("#ExpenditureDate").attr("value", today);
});
$(document).on({
    ajaxStart: function () {
        $('#cover-spin').show(0);
    },
    ajaxStop: function () {
        $('#cover-spin').hide();
    }
});

