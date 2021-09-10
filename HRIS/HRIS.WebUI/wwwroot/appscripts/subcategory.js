$(function () {
   
    $('#actionLoader1').hide();
    $('#actionLoader2').hide();
    var baseUrl = $('#ApiBaseUrl').val();
    var token = $('#jwtToken').val();
    var bearerToken = 'Bearer ' + token;
    var branchId = parseInt($('#branchId').val());

    $('#btn-AddSubCategory').click(function () {
        if ($('#Description').val() === "") {
            $('#Description').css('border-color', 'Red');
            ($('#Description').focus());
            return false;
        }
        else {
            //$('#actionLoader1').show();
            //$('#actionLoader2').show();
           // $('#ibox1').children('.modal fade').toggleClass('sk-loading');
            AddSubCategory();
        }
       
    });
    $('#btn-SubmitEditCategory').click(function () {
        EditSubCategory();
    });

    loadSubCategory();

    function loadSubCategory() {
        var remoteDataLoader = window.DevExpress.data.AspNet.createStore({
            key: "subCategoryId",
            loadUrl: baseUrl + "v1.0/SubCategory/GetAllSubCategory",
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
                        dataField: "subCategoryName",
                        caption: "Sub Category Name",
                        sortIndex: 0,
                        sortOrder: 'asc',
                        //fixed: true,
                        cssClass: 'font-bold'

                    },
                    {
                        dataField: "categoryName",
                        caption: "Category Name",
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
                                        $('#SubCategoryId').val(options.data.subCategoryId);
                                        $('#editSubCategory_CategoryId').val(options.data.categoryId).trigger("chosen:updated");
                                        $('#editSubCategory_Description').val(options.data.subCategoryName);

                                        $('#editSubCategoryModal').modal('show');
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
                                                DeleteSubCategory(options.data.subCategoryId)

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

        dataGrid = window.$("#subCategoryContainer").dxDataGrid(gridOptions).dxDataGrid("instance");
    }

    function DeleteSubCategory(id) {
        var deleteSubCategoryRequest = {};
        deleteSubCategoryRequest.SubCategoryId = id;
        var token = $('#jwtToken').val();
        var bearerToken = 'Bearer ' + token;
        var applicationUrl = baseUrl + 'v1.0/SubCategory/DeleteSubCategory';
        $.ajax({
            headers: {
                'Authorization': bearerToken
            },
            url: applicationUrl,
            type: 'POST',
            contentType: 'application/json',
            data: JSON.stringify(deleteSubCategoryRequest),
            success: function (data) {
                swal({
                    title: "Successful!",
                    text: "Subcategory deleted successfully",
                    icon: "success",
                    closeOnClickOutside: false,
                    closeOnEsc: false
                })
                    .then(() => {
                        loadSubCategory();
                    });
               
            },
            error: function (xhr) {
                alert('Woow something went wrong');
            }
        });
    }

    function EditSubCategory() {
        var EditSubCategoryRequest = {
            SubCategoryId: parseInt($('#SubCategoryId').val()),
            CategoryId: parseInt($('#editSubCategory_CategoryId').val()),
            Description: $('#editSubCategory_Description').val()
        };

        var token = $('#jwtToken').val();
        var bearerToken = 'Bearer ' + token;

        var applicationUrl = baseUrl + 'v1.0/SubCategory/UpdateSubCategory';
        $.ajax({
            headers: {
                'Authorization': bearerToken
            },
            url: applicationUrl,
            type: 'POST',
            contentType: 'application/json',
            data: JSON.stringify(EditSubCategoryRequest),
            success: function (data) {
                if (data.succeeded) {
                    if (data.responseCode == "00") {
                        swal({
                            title: "Successful!",
                            text: "subcategory updated successfully",
                            icon: "success",
                            closeOnClickOutside: false,
                            closeOnEsc: false
                        })
                            .then(() => {
                                $('#editSubCategoryModal').modal('hide');
                                loadSubCategory();
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

    function AddSubCategory() {
        var AddSubCategoryRequest = {
            Description: $('#Description').val(),
            CategoryId: parseInt($('#CategoryId').val()),
        };

        var token = $('#jwtToken').val();
        var bearerToken = 'Bearer ' + token;

        var applicationUrl = baseUrl + 'v1.0/SubCategory/CreateSubCategory';
        $.ajax({
            headers: {
                'Authorization': bearerToken
            },
            url: applicationUrl,
            type: 'POST',
            contentType: 'application/json',
            data: JSON.stringify(AddSubCategoryRequest),
            success: function (data) {
                if (data.responseCode == "00") {
                    swal({
                        title: "Successful!",
                        text: "subcategory created successfully",
                        icon: "success",
                        closeOnClickOutside: false,
                        closeOnEsc: false
                    })
                        .then(() => {
                            $('#myModal').modal('hide');
                            loadSubCategory();
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