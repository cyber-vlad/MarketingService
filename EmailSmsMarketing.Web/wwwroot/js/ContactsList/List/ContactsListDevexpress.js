var userIdGrid;
$('#contactsListTable').dxDataGrid({                                 
    dataSource: {
        load: function () {
            return $.getJSON("/ContactList/ContactsList")
                .then(function (data) {
                    return data.Records;
                });
        }
    },
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
    columnAutoWidth: false,
    columnMinWidth: 125,  
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
        text: 'Loading...',
        showIndicator: true,
      
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
            caption: 'Title',
            dataType: 'string',
        },
        {
            dataField: 'Email',
            caption: 'Emails',
            dataType: 'string',
        },
        {
            dataField: 'Phone',
            caption: 'Phone Numbers',
            dataType: 'string',
        },
        {
            dataField: 'CreateDate',
            caption: 'Create Date',
            dataField: 'DateTime',
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
            caption: 'Title',
            dataField: 'Name',
            dataType: 'string',
        }, {
            name: 'Email',
            caption: 'Emails',
            dataField: 'Email',
            dataType: 'string',
        }, {
            name: 'Phone',
            caption: 'Phone Numbers',
            dataField: 'Phone',
            dataType: 'string',
        }, {
            name: 'CreateDate',
            caption: 'Create Date',
            dataField: 'CreateDate',
            dataType: 'date',
            format: 'dd-MM-yyyy HH:mm',
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
                text: 'Create list',
                icon: 'add',
                stylingMode: 'contained',
                type: 'success',
                onClick(e) {
                    addContactListButtonClicked(e);
                },
            }
        },
        {
            location: 'before',
            locateInMenu: 'auto',
            widget: 'dxButton',
            options: {
                icon: 'download',
                hint: 'Download example of Excel-file',
                onClick(e) {
                    downloadExcelFileButtonClicked(e);
                },
            },
        },
        {
            location: 'after',
            locateInMenu: 'auto',
            widget: 'dxButton',
            options: {
                icon: 'refresh',
                hint: 'Refresh table',
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

function addContactListButtonClicked() {
    ISESM.DrawPartialModal(ISESM.BuildActionUrl("ContactList", "CreateContactList"), "lgModalBody");

}

function downloadExcelFileButtonClicked(e) {

    var workbook = new ExcelJS.Workbook();
    var worksheet = workbook.addWorksheet('ContactList');

    worksheet.columns = [
        { header: 'ID', key: 'id', width: 10 },
        { header: 'Name', key: 'name', width: 20 }, 
        { header: 'Email', key: 'email', width: 30 },
        { header: 'Phone', key: 'phone', width: 20 }
    ];

    worksheet.addRow({ id: 1, name: 'Walter White', email: 'heisenberg@gmail.com', phone: '069444222' });
    worksheet.addRow({ id: 2, name: 'Jesse Pinkman', email: 'pinkman@gmail.com', phone: '069222444' });

    workbook.xlsx.writeBuffer().then(function (buffer) {
        saveAs(new Blob([buffer], { type: 'application/octet-stream' }), 'Contact_List_Example.xlsx');
    });
}

function getList() {
    let element = document.getElementById("contactsListTable");
    return DevExpress.ui.dxDataGrid.getInstance(element);
}


function editButtonClicked(e) {
    e.component.selectRowsByIndexes(e.row.rowIndex);
    const data = e.row.data;

    if (data) {
        var id = data.ID;
        ISESM.renderView(ISESM.BuildActionUrl("ContactList", "List", id));
    }
}

function deleteButtonClicked(e) {
    e.component.selectRowsByIndexes(e.row.rowIndex);
    const data = e.row.data;

    if (data) {
        var id = data.ID;
        ISESM.DrawPartialModal(ISESM.BuildActionUrl("ContactList", "DeleteContactList", id), 'lgModalBody');
    }
}

