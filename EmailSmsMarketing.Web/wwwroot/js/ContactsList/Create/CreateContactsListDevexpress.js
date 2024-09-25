function onSubmitClick(e)
{
    e.event.preventDefault();

    var gridId = "contactsListTable";
    var $form = $('form[name="importContacts"]');
    var url = $form.attr('action');
    var modal = '#lgModalBody';

    ISESM.ajaxPOST(url, $form, modal, gridId);
}
function onFileInput(e)
{
    var file = e.value[0];
    var reader = new FileReader();
    reader.onloadend = function () {

        $("#" + e.element[0].id + "String").dxTextBox("instance").option("value", reader.result);
    }
    reader.readAsDataURL(file);
}


function onDeleteClick(e)
{
    e.event.preventDefault();
    var gridId = "contactsListTable";
    var $form = $('form[name="deleteContacts"]');
    var url = $form.attr('action');
    var modal = '#lgModalBody';
    ISESM.ajaxPOST(url, $form, modal, gridId);
}