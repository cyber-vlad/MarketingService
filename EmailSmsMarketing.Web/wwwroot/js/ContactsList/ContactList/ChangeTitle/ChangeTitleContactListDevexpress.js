$(document).ready(function () {
    var contactId = $('#editTitleButton').data('contact-id');

    $('#editTitleButton').dxButton({
        icon: 'edit',
        stylingMode: 'contained',
        type: 'normal',
        onClick: function () {
            changeTitleContactList(contactId);
        }
    }).dxButton('instance');
});

function changeTitleContactList(id) {

        ISESM.DrawPartialModal(ISESM.BuildActionUrl("ContactList", "ChangeTitleContactList", id),'lgModalBody');
}
