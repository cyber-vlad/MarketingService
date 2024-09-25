
    function onSubmitClick(e) {
        e.event.preventDefault();
    var gridId = "contactListTable";
    var $form = $('form[name="changeTitle"]');
    var url = $form.attr('action');
    var modal = '#lgModalBody';
    ISESM.ajaxPOST(url, $form, modal, gridId);
}
