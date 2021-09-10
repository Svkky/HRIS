
$(document).ready(function () {
    $('#actionLoader').hide();
    var that = this;
    //domainName = document.querySelector("#domainNameId").value;

    $('#btnLogin').on('click', function () {
        $('#btnLogin').attr('disabled', 'disabled');
        var isValid = true;
        //if ($('#txtLoginEmail').val().trim() === "") {
        //    $('#txtLoginEmail').css('border-color', 'Red');
        //    isValid = false;
        //}
        //else {
        //    $('#txtLoginEmail').css('border-color', 'lightgrey');
        //}
        //if ($('#txtLoginPassword').val().trim() === "") {
        //    $('#txtLoginPassword').css('border-color', 'Red');
        //    isValid = false;
        //}
        //else {
        //    $('#txtLoginPassword').css('border-color', 'lightgrey');
        //}

        const isValidEmail = txtLoginEmail.checkValidity();
        const isValidPassword = txtLoginPassword.checkValidity();

        if (isValidEmail == false) {
            $('#txtLoginEmail').css('border-color', 'Red');
            $('#txtLoginEmail').focus();
            $('#btnLogin').removeAttr('disabled');
            ShowMessagePopup("Oops!", "Please ensure you fill the email field correctly", "warning");

            return false;
        }
        else if (isValidPassword == false) {
            $('#txtLoginPassword').css('border-color', 'Red');
            ($('#txtLoginPassword').focus());
            $('#btnLogin').removeAttr('disabled');
            ShowMessagePopup("Oops!", "Please ensure you fill the password field correctly", "warning");


            return false;
        }
        else {
            $('#ibox1').children('.ibox-content').toggleClass('sk-loading');
            document.forms["SubmitLoginForm"].submit();
        }
       
    })




});

