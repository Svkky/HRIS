$(function () {
    var baseUrl = $('#ApiBaseUrl').val();
    var token = $('#jwtToken').val();
    var bearerToken = 'Bearer ' + token;
    var branchId = $('#branchId').val();

    $('#btn-AddCategory').click(function () {
        var description = document.getElementById('Description');
        var isDescriptionValid = description.checkValidity();
        if (!isDescriptionValid) {
            $('#Description').css('border-color', 'Red');
            ($('#Description').focus());
            return false;
        }
        AddCategory();
    });
   
    $('#btn-EditCategory').click(function () {
        var editDescription = document.getElementById('edit_CategoryDescription');
        var isEditDescriptionValid = editDescription.checkValidity();
        if (!isEditDescriptionValid) {
            $('#edit_CategoryDescription').css('border-color', 'Red');
            ($('#edit_CategoryDescription').focus());
            return false;
        }
        EditCategory();
    });

    loadCategory();

    function loadCategory() {

        
        var remoteDataLoader = window.DevExpress.data.AspNet.createStore({
            key: "categoryId",
            loadUrl: baseUrl + "v1.0/Category/GetAllCategory",
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
                        dataField: "description",
                        caption: "Category Name",
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
                                        $('#CategoryId').val(options.data.categoryId);
                                        $('#edit_CategoryDescription').val(options.data.description);

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
                              -                  DeleteCategory(options.data.categoryId)

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

        dataGrid = window.$("#categoryContainer").dxDataGrid(gridOptions).dxDataGrid("instance");
    }

    function DeleteCategory(id) {
        var deleteCategoryRequest = {};
        deleteCategoryRequest.CategoryId = id;
        var token = $('#jwtToken').val();
        var bearerToken = 'Bearer ' + token;
        var applicationUrl = baseUrl + 'v1.0/Category/DeleteCategory';
        $.ajax({
            headers: {
                'Authorization': bearerToken
            },
            url: applicationUrl,
            type: 'POST',
            contentType: 'application/json',
            data: JSON.stringify(deleteCategoryRequest),
            success: function (data) {
                if (data.responseCode == "00") {
                    swal({
                        title: "Successful!",
                        text: "category deleted successfully",
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

                loadCategory();
            },
            error: function (xhr) {
                alert('Woow something went wrong');
            }
        });
    }
    
    function EditCategory() {
        var EditCategoryRequest = {
            CategoryId: parseInt($('#CategoryId').val()),
            Description: $('#edit_CategoryDescription').val()
        };

        var token = $('#jwtToken').val();
        var bearerToken = 'Bearer ' + token;
       
        var applicationUrl = baseUrl + 'v1.0/Category/UpdateCategory';

        if (EditCategoryRequest.CategoryId == '' || EditCategoryRequest.Description == '') {
            swal({
                title: 'Wrong format!!',
                text: 'Invalid Input field',
                icon: 'warning',
                confirmButtonText: 'Ok'
            });
        } else {
            $.ajax({
                headers: {
                    'Authorization': bearerToken
                },
                url: applicationUrl,
                type: 'POST',
                contentType: 'application/json',
                data: JSON.stringify(EditCategoryRequest),
                success: function (data) {

                    //alert(JSON.stringify(data));
                    if (data.responseCode == "00") {
                        swal({
                            title: "Successful!",
                            text: "category updated successfully",
                            icon: "success",
                            closeOnClickOutside: false,
                            closeOnEsc: false
                        })
                            .then(() => {
                                $('#Mod').modal('hide');
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
                },
                error: function (xhr) {
                    alert(xhr.responseText);
                }
            });
        }
       
    }

    function AddCategory() {
        var AddCategoryRequest = {
            Description: $('#Description').val(),
            BranchId: parseInt(branchId)
        };

        var token = $('#jwtToken').val();
        var bearerToken = 'Bearer ' + token;

        var applicationUrl = baseUrl + 'v1.0/Category/CreateCategory';
        if (AddCategoryRequest.Description == '' || AddCategoryRequest.BranchId == '') {
            swal({
                title: 'Fields Empty!!',
                text: 'please check the missing field',
                icon: 'warning',
                confirmButtonText: 'Ok'
            });

        } else {
            $.ajax({
                headers: {
                    'Authorization': bearerToken
                },
                url: applicationUrl,
                type: 'POST',
                contentType: 'application/json',
                data: JSON.stringify(AddCategoryRequest),
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
                                $('#Description').val('');
                                $('#myModal').modal('hide');



                                loadCategory();
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
