function onSubmitEditProfileClick(e) {
	e.event.preventDefault();
	var $form = $('form[name="editProfileForm"]');
	var url = $form.attr('action');
	var firstName = $form.find('input[name="FirstName"]');
	var lastName = $form.find('input[name="LastName"]');
	var modal = "#editProfile"

	$.ajax({
		url: url,
		cache: false,
		type: "POST",
		dataType: "html",
		data: $form.serialize(),
		success: function (result) {
			if (EmailSmsMarketing.IsJSON(result))
			{
				result = EmailSmsMarketing.ParseJSON(result);

				if (result.Result == 1) { // OK

					if (result.ShowToast) {
						ShowToast('success', result.Message);
					}
					$("#userFullName").text(firstName.val() + " " + lastName.val());

				}
				else if (result.Result == 2) {// KO
					if (result.ShowToast) {
						ShowToast('warning', result.Message);
					}
				}
				else if (result.Result == 3) {// ERROR
					if (result.ShowToast) {
						ShowToast('info', result.Message);
					}
				} else if (result.Result == 4) {// NOTVALID
					if (result.ShowToast) {
						ShowToast('warning', result.Message);
					}
				} else if (result.Result == 5) {// EXCEPTION
					if (result.ShowToast) {
						ShowToast('warning', result.Message);
					}
				} else if (result.Result == 6) {// LOGOUT
					if (result.ShowToast) {
						ShowToast('danger', result.Message);
					}
					window.location.href = '/Account/TokenLogout/';
				}
			}
			else if (!EmailSmsMarketing.IsJSON(result)) {
				if (modal != null) {
					$(modal).html(result);

				}
			}
		}
	});
}

function onSubmitChangePasswordClick(e) {
	e.event.preventDefault();
	var $form = $('form[name="changePasswordForm"]');
	var url = $form.attr('action');
	var modal = "#changePassword"

	EmailSmsMarketing.ajaxPOST(url, $form, modal);
}