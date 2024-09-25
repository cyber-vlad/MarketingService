function changePasswordMode() {
	//var passwordEditor = $("#Password").dxTextBox("instance");
	//var buttonEditor = $("#passwordButton").dxButton("instance");

	var passwordEditor = DevExpress.ui.dxTextBox.getInstance(document.getElementById("Password"));
	var buttonEditor = DevExpress.ui.dxButton.getInstance(document.getElementById("passwordButton"));

	passwordEditor.option("mode", passwordEditor.option("mode") === "text" ? "password" : "text");
	buttonEditor.option("icon", buttonEditor.option("icon") === "fa fa-eye-slash" ? "fa fa-eye" : "fa fa-eye-slash");
}
$(document).ready(function () {
	$("#Password").dxTextBox({})
		.dxValidator({
			name: "Password",
			validationRules: [{
				type: "required",
				message: formatMessage('LockScreenPasswordValidation'),
			}]
		});
});