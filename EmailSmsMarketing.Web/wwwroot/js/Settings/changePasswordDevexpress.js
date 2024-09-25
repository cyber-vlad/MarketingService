function changeOldPasswordMode() {
    var passwordEditor = DevExpress.ui.dxTextBox.getInstance(document.getElementById("OldPassword"));
    var buttonEditor = DevExpress.ui.dxButton.getInstance(document.getElementById("oldPasswordButton"));

    passwordEditor.option("mode", passwordEditor.option("mode") === "text" ? "password" : "text");
    buttonEditor.option("icon", buttonEditor.option("icon") === "fa fa-eye-slash" ? "fa fa-eye" : "fa fa-eye-slash");
}


function changeNewPasswordMode() {
    var passwordEditor = DevExpress.ui.dxTextBox.getInstance(document.getElementById("NewPassword"));
    var buttonEditor = DevExpress.ui.dxButton.getInstance(document.getElementById("newPasswordButton"));

    passwordEditor.option("mode", passwordEditor.option("mode") === "text" ? "password" : "text");
    buttonEditor.option("icon", buttonEditor.option("icon") === "fa fa-eye-slash" ? "fa fa-eye" : "fa fa-eye-slash");
}

function changeConfirmPasswordMode() {
    var passwordEditor = DevExpress.ui.dxTextBox.getInstance(document.getElementById("ConfirmPassword"));
    var buttonEditor = DevExpress.ui.dxButton.getInstance(document.getElementById("confirmPasswordButton"));

    passwordEditor.option("mode", passwordEditor.option("mode") === "text" ? "password" : "text");
    buttonEditor.option("icon", buttonEditor.option("icon") === "fa fa-eye-slash" ? "fa fa-eye" : "fa fa-eye-slash");
} 