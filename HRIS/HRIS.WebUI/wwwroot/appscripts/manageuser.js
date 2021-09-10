$(function () {
    var role = document.getElementById('roleId');
    var user = document.getElementById('userId');
    var token = $('#jwtToken').val();
    var bearerToken = 'Bearer ' + token;
    //$('#table_pages_by_userId').dataTable();
    //$('#table_pages_by_roleId').dataTable();
    //var roleId = $('#roleId').val();
    //var userId = $('#userId').val();
    var baseUrl = $('#ApiBaseUrl').val();
    if (role.value != "") {
        getPagesByRoleId(role.value);
        getPagesByUserId(user.value);
    }


    function getPagesByRoleId(id) {

        var token = $('#jwtToken').val();
        var bearerToken = 'Bearer ' + token;

        var applicationUrl = baseUrl + 'v1.0/MenuItem/GetPagesByRoleId/' + id;
        $.ajax({
            headers: {
                'Authorization': bearerToken
            },
            url: applicationUrl,
            type: 'GET',
            dataType: 'json',
            success: function (response) {
                var rowIndex = 1;
                var data = response.data;
                $("#table_pages_by_roleId tr").remove();
                //var firstRow = "<tr>"
                //    + "<td>" + "" + "</td>"
                //    + "<td>" + "" + "</td>"
                //    + "<td>" + '<input type="checkbox' + '" id="' + "rowPagesCheckAll" + '" class="rolePagesOne' + '" name="rolePagesOne' + '">' + "</td>"
                //    + "</tr>";
                var firstRow = "<tr>"
                    + "<td>" + "" + "</td>"
                    + "<td>" + "" + "</td>"
                    + "<td>" + "<input type='checkbox' id='rowPagesCheckAll' class='rolePagesOne'name='rolePagesOne'</td>"
                    + "</tr>";
                $('#table_pages_by_roleId_body').append(firstRow);
                $.each(data, function (index, item) {
                    var eachrow = "<tr>"
                        + "<td>" + (rowIndex++) + "</td>"
                        + "<td>" + item.menuName + "</td>"
                        + "<td>" + '<input type="checkbox' + '" value="' + item.menuId + '" class="rowPagesClass' + '" name="rowPages' + '">' + "</td>"
                        + "</tr>";
                    $('#table_pages_by_roleId_body').append(eachrow);
                });
            }
        });
    }
    function getPagesByUserId(id) {

        var token = $('#jwtToken').val();
        var bearerToken = 'Bearer ' + token;

        var applicationUrl = baseUrl + 'v1.0/MenuItem/GetPagesByUserId/' + id;
        $.ajax({
            headers: {
                'Authorization': bearerToken
            },
            url: applicationUrl,
            type: 'GET',
            dataType: 'json',
            success: function (response) {
                var rowIndex = 1;
                var data = response.data;
                $("#table_pages_by_userId tr").remove();
                var firstRow = "<tr>"
                    + "<td>" + "" + "</td>"
                    + "<td>" + "" + "</td>"
                    + "<td>" + '<input type="checkbox' + '" id="' + "userPagesCheckAll" + '" class="userPagesOne' + '" name="userPagesOne' + '">' + "</td>"
                    + "</tr>";
                $('#table_pages_by_userId_body').append(firstRow);
                $.each(data, function (index, item) {
                   
                    var eachrow = "<tr>"
                        + "<td>" + (rowIndex++) + "</td>"
                        + "<td>" + item.menuName + "</td>"
                        + "<td>" + '<input type="checkbox' + '" value="' + item.menuId + '" class="userPages' + '" name="userPages' + '">' + "</td>"
                        + "</tr>";
                    $('#table_pages_by_userId_body').append(eachrow);
                });
            }
        });
    }

    $("#rowPagesCheckAll").change(function () {
        if (this.checked) {
            $(".rowPagesClass").each(function () {
                this.checked = true;
            });
        } else {
            $(".rowPagesClass").each(function () {
                this.checked = false;
            });
        }
    });
    $("#userPagesCheckAll").change(function () {
        if (this.checked) {
            $(".userPages").each(function () {
                this.checked = true;
            });
        } else {
            $(".userPages").each(function () {
                this.checked = false;
            });
        }
    });

    $('#btn-assignRowPages').click(function () {
        AssignPagestoUser(user.value);
    });
    $('#btn-removeAssignedPages').click(function () {
        RemoveUserAssignedPages(user.value);
    });

    function AssignPagestoUser(id) {
        var arr = [];
        $('input.rowPagesClass:checkbox:checked').each(function () {
            arr.push($(this).val());
        });
        var AssignPagesToUserRequest = {
            "roleId": role.value,
            "userId": user.value,
            menus: arr
        };

        var applicationUrl = baseUrl + 'ProcessMenuToUsers/AssignMenu';
        $.ajax({
            headers: {
                'Authorization': bearerToken
            },
            url: applicationUrl,
            type: 'POST',
            contentType: 'application/json',
            traditional: true,
            data: JSON.stringify(AssignPagesToUserRequest),
            success: function (data) {
                if (data.responseCode == "00") {
                    swal({
                        title: "Successful!",
                        text: "Page(s) has been succesfully mapped to the user!!",
                        icon: "success",
                        closeOnClickOutside: false,
                        closeOnEsc: false
                    }).then(() => {
                        getPagesByRoleId(role.value);
                        getPagesByUserId(user.value);
                    });
                }
                else if (data.responseCode === "-1") {
                    swal({
                        title: "An error occured!",
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
            error: function (err, result) {
                alert("Error occurred" + err.responseText);
            }
        });

    }
    function RemoveUserAssignedPages(id) {
        var arr = [];
        $('input.userPages:checkbox:checked').each(function () {
            arr.push($(this).val());
        });
        var RemoveUserAssignedPagesRequest = {
            "roleId": role.value,
            "userId": user.value,
            menus: arr
        };

        var applicationUrl = baseUrl + 'ProcessMenuToUsers/RemoveAssignMenu';
        $.ajax({
            headers: {
                'Authorization': bearerToken
            },
            url: applicationUrl,
            type: 'POST',
            contentType: 'application/json',
            traditional: true,
            data: JSON.stringify(RemoveUserAssignedPagesRequest),
            success: function (data) {
                if (data.responseCode == "00") {
                    swal({
                        title: "Successful!",
                        text: "Assigned Page(s) has been removed succesfully",
                        icon: "success",
                        closeOnClickOutside: false,
                        closeOnEsc: false
                    }).then(() => {
                        getPagesByRoleId(role.value);
                        getPagesByUserId(user.value);
                    });
                }
                else if (data.responseCode === "-1") {
                    swal({
                        title: "An error occured!",
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
            error: function (err, result) {
                alert("Error occurred" + err.responseText);
            }
        });

    }

    function arrayToList(array) {
        let list = null;
        for (let i = array.length - 1; i >= 0; i--) {
            list = { value: array[i], rest: list };
        }
        return list;
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