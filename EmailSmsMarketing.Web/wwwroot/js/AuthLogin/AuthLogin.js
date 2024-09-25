
function changePasswordMode() {

    var passwordEditor = DevExpress.ui.dxTextBox.getInstance(document.getElementById("Password"));
    var buttonEditor = DevExpress.ui.dxButton.getInstance(document.getElementById("PasswordButton"));

    passwordEditor.option("mode", passwordEditor.option("mode") === "text" ? "password" : "text");
    buttonEditor.option("icon", buttonEditor.option("icon") === "fa fa-eye-slash" ? "fa fa-eye" : "fa fa-eye-slash");

}