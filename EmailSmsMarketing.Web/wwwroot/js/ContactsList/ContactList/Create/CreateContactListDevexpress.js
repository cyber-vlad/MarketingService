function onSubmitClick(e) {
    e.event.preventDefault();
    var gridId = "contactListTable";
    var $form = $('form[name="upsertContact"]');
    var url = $form.attr('action');
    var modal = '#lgModalBody';
    ISESM.ajaxPOST(url, $form, modal, gridId);
}

function onDeleteClick(e) {
    e.event.preventDefault();
    var gridId = "contactListTable";
    var $form = $('form[name="deleteContact"]');
    var url = $form.attr('action');
    var modal = '#lgModalBody';
    ISESM.ajaxPOST(url, $form, modal, gridId);
}