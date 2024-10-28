// Please see documentation at https://learn.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

function registration() {
    debugger

    var data = {
        FirstName: $("#FirstName").val(),
        LastName: $("#LastName").val(),
        PhoneNumber: $("#PhoneNumber").val(),
        Address: $("#Address").val(),
        Email: $("#Email").val(),
        Password: $("#Password").val(),
        ConfirmPassword: $("#ConfirmPassword").val()     

    };

    if (data.FirstName == "" && data.FirstName != undefined ) {
        errorAlert("Firstname is required");
        return;
    }

    if (data.LastName == "" && data.LastName != undefined) {
        errorAlert("LastName is required");
        return;
    }
    if (data.PhoneNumber == "") {
        errorAlert("PhoneNumber is required");
        return;
    }
   
    if (data.Address == "") {
        errorAlert("Address is required");
        return;
    }
    if (data.Email == "") {
        errorAlert("Email is required");
        return;
    }
    if (data.Password == "") {
        errorAlert("Password is required");
        return;
    }
    if (data.Password != data.ConfirmPassword && data.ConfirmPassword == "") {
        errorAlert("ConfirmPassword does not match password");
        return;
    }


    $.ajax({
        type: 'POST',
        url: '/Account/Register',
        data: JSON.stringify(data),
        contentType: 'application/json; charset=utf-8',
        dataType: 'json',
        success: function (result) {
            debugger;
            if (result.isError) {
                errorAlert(result.msg)
            }
            else {
                var url = '/Account/Login';
                successAlertWithRedirect(result.msg, url);
            }
        },
        error: function (ex) {
            $('#validationSummary').text("Error occurred. Please try again.");
        }
    });
}


function showSidebar() {
    document.getElementById("toggleablesidebar").classList.toggle("hidden");
}
