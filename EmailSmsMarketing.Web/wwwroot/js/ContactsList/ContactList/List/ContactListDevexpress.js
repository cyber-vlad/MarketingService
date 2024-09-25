$('#contactListTable').dxDataGrid({
    dataSource: ISESMDevextreme.ajaxCustomStoreGET(ISESM.BuildActionUrl("ContactList", "ContactList", ISESM.DivideActionUrl())),
    showBorders: true,
    showRowLines: true,
    filterPanel: {
        visible: true,
    },
    filterRow: {
        visible: true,
    },
    headerFilter: {
        visible: true,
    },
    columnChooser: {
        enabled: true,
        visible: true,
    },
    columnMinWidth: 30,
    columnAutoWidth: true,
    hoverStateEnabled: true,
    rowAlternationEnabled: true,
    scrolling: {
        rowRenderingMode: 'standard',
    },
    selection: {
        mode: 'single',
    },
    grouping: {
        autoExpandAll: true,
    },
    groupPanel: {
        visible: true,
    },
    allowColumnResizing: true,
    allowColumnReordering: true,
    searchPanel: {
        visible: true,
        width: 240,
        placeholder: 'Search',
    },
    loadPanel: {
        enabled: true,
    },
    paging: {
        pageSize: 15,
    },
    pager: {
        visible: true,
        showInfo: true,
        displayMode: 'adaptive',
        showPageSizeSelector: true,
        showNavigationButtons: true,
        allowedPageSizes: [15, 25, 50, 'all'],
    },
    filterBuilder: {
        fileds: [{
            dataField: 'ID',
            caption: 'ID',
            dataType: 'number',
            visible: false,
        },
        {
            dataField: 'Name',
            caption: 'Name',
            dataType: 'string',
        },
        {
            dataField: 'Email',
            caption: 'Email',
            dataType: 'string',
        },
        {
            dataField: 'Phone',
            caption: 'Phone',
            dataType: 'string',
        }],
        allowHierarchicalFields: true,
    },
    filterBuilderPopup: {
        position: {
            at: ['Center', 'Top'],
            my: ['Center', 'Top'],
            offset: [0, 10]
        },
    },
    columns: [
        {
            name: 'ID',
            dataField: 'ID',
            alignment: 'left',
            visible: false,
        }, {
            name: 'Name',
            caption: 'Name',
            dataField: 'Name',
            width: 250,
            dataType: 'string',
        }, {
            name: 'Email',
            caption: 'Email',
            dataField: 'Email',
            dataType: 'string',
        }, {
            name: 'Phone',
            caption: 'Phone',
            dataField: 'Phone',
            dataType: 'string',
        }, {
            name: 'contextBtns',
            type: 'buttons',
            buttons: [{
                icon: 'edit',
                onClick(e) {
                    editButtonClicked(e);
                },
            }, {
                icon: 'trash',
                onClick(e) {
                    deleteButtonClicked(e);
                },
            }]
        }],
    toolbar: {
        items: [{
            location: 'before',
            locateInMenu: 'auto',
            widget: 'dxButton',
            options: {
                text: 'Create Contact',
                icon: 'add',
                stylingMode: 'contained',
                type: 'success',
                onClick(e) {
                    addContactListButtonClicked(e);
                },
            }
        },
        {
            location: 'after',
            locateInMenu: 'auto',
            widget: 'dxButton',
            options: {
                icon: 'refresh',
                onClick() {
                    getList().refresh(true);
                },
            },
        },
        {
            location: 'after',
            locateInMenu: 'auto',
            name: 'searchPanel'
        },
        {
            location: 'before',
            locateInMenu: 'auto',
            name: 'groupPanel'
        },
        {
            location: 'before',
            locateInMenu: 'auto',
            name: 'grouping'
        }]
    },
}).dxDataGrid('instance');

function addContactListButtonClicked(e) {
    var parrentId = ISESM.DivideActionUrl();
    ISESM.DrawPartialModal("/List" + ISESM.BuildActionUrl("ContactList", "UpsertContact", 0, parrentId), "lgModalBody");
}

function getList() {
    let element = document.getElementById("contactListTable");
    return DevExpress.ui.dxDataGrid.getInstance(element);
}

function editButtonClicked(e) {
    e.component.selectRowsByIndexes(e.row.rowIndex);
    var parrentId = ISESM.DivideActionUrl();
    const data = e.row.data;

    if (data) {
        var id = data.ID;
        ISESM.DrawPartialModal("/List" + ISESM.BuildActionUrl("ContactList", "UpsertContact", id, parrentId), 'lgModalBody');
    }
}

function deleteButtonClicked(e) {
    e.component.selectRowsByIndexes(e.row.rowIndex);
    var parrentId = ISESM.DivideActionUrl();
    const data = e.row.data;

    if (data) {
        var id = data.ID;
        ISESM.DrawPartialModal("/List" + ISESM.BuildActionUrl("ContactList", "DeleteContact", id, parrentId), 'lgModalBody');
    }
}


