
var MenusDT;
var MenusDataTable = function () {
    var MenusDataTableObj = function () {
        MenusDT = $('#MenusDiv').KTDatatable({
            data: {
                type: 'remote',
                source: {
                    read: {
                        url: APIUrl + `Api/Menus/All`,
                        method: 'Get',
                    }
                }
            },
            layout: {
                scroll: false,
                footer: false
            },
            sortable: true,
            saveState: false,
            search: {
                input: $('#SearchTerm')
            },
            pagination: true,
            columns: [
                {
                    field: 'Label',
                    title: 'Label',
                    width: 100,
                    textAlign: 'center',
                },
                {
                    field: 'Name',
                    title: 'English Name',
                    width: 100,
                    textAlign: 'center',
                },
                {
                    field: 'NameAr',
                    title: 'Arabic Name',
                    width: 100,
                    textAlign: 'center',
                },
                {
                    field: 'Active',
                    title: 'Status',
                    width: 60,
                    textAlign: 'center',
                    template: function (row) {

                        if (row.Active == true) { return '<span class="kt-badge kt-badge--inline kt-badge--success font-weight-bold">Active</span>' }
                        else { return `<span class="kt-badge kt-badge--inline kt-badge--danger font-weight-bold">Inactive</span>` }
                    }
                },
                {
                    field: 'Priority',
                    title: 'Priority',
                    width: 50,
                    textAlign: 'center',
                },

                {
                    field: 'Actions',
                    title: 'Actions',
                    sortable: false,
                    width: 200,
                    autoHide: false,
                    overflow: 'visible',
                    template: function (row) {

                        let htmlStr = '';

                        if (canEdit == 1) { 
                            htmlStr += `
                                <a href="ManageMenu?id=${row.Id}" class="btn btn-sm btn-clean btn-icon btn-icon-md " title="Edit details">
                                    <i class="la la-edit"></i>
                                </a>`;


                            htmlStr += `
                                <a data-row-id="${row.Id}" class="btn btn-sm btn-clean btn-icon btn-icon-md data-up" title="Up">
                                    <i class="la la-arrow-circle-up"></i>
                                </a>`;

                            htmlStr += `
                                <a data-row-id="${row.Id}" class="btn btn-sm btn-clean btn-icon btn-icon-md data-down" title="down">
                                    <i class="la la-arrow-circle-down"></i>
                                </a>`;


                            if (row.Active == true) {
                                htmlStr += `
                                <a data-row-id="${row.Id}" class="btn btn-sm btn-clean btn-icon btn-icon-md data-UnActive" title="Deactivate">
                                    <i class="font-weight-bold la la-times-circle-o"></i>
                                </a>`;

                            }
                            else {
                                htmlStr += `
                                <a data-row-id="${row.Id}" class="btn btn-sm btn-clean btn-icon btn-icon-md data-Active" title="Activate">
                                    <i class="font-weight-bold la la-check-circle-o"></i>
                                </a>`;

                            }  
                        }

                        if (canEdit == 0 && canView == 1) {
                            htmlStr += `
                                <a href="ManageMenu?id=${row.Id}" class="btn btn-sm btn-clean btn-icon btn-icon-md " title="Show details">
                                    <i class="la la-eye"></i>
                                </a>`;
                        }

                        if (canDelete == 1) {
                            htmlStr += `
                                <a data-row-id="${row.Id}" class="btn btn-sm btn-clean btn-icon btn-icon-md show-delete-data" title="Delete">
                                    <i class="la la-trash"></i>
                                </a>`;
                        }

                        return htmlStr;

                    },
                }],
        });
    };

    return {
        init: function () {
            MenusDataTableObj();
        }
    };
}();

jQuery(document).ready(function () {
    MenusDataTable.init();
  
    $("#MenusDiv").on("click", ".data-up", function (e) { 
        MenuUp($(this).data("row-id")); 
    })

    $("#MenusDiv").on("click", ".data-down", function (e) { 
        MenuDown($(this).data("row-id"));
    })

    $("#MenusDiv").on("click", ".data-Active", function (e) {
        ActiveMenu($(this).data("row-id"));
    })

    $("#MenusDiv").on("click", ".data-UnActive", function (e) {
        unActiveMenu($(this).data("row-id"));
    })


    $("#MenusDiv").on("click", ".show-delete-data", function (e) {
        Swal.fire({
            title: 'Alert',
            text: 'Are you sure you want to delete ?',
            type: 'warning',
            showCancelButton: true,
            confirmButtonColor: '#3085d6',
            cancelButtonColor: '#d33',
            confirmButtonText: 'Yes',
            cancelButtonText: 'No',

        }).then((result) => {
            if (result.value) {
                DeleteRow(APIUrl + "Api/Menus?id=" + $(this).data("row-id"), MenusDT);
            }
            else if (result.dismiss === Swal.DismissReason.cancel) {
                result = false;
            }
        })
    })

});



  function MenuUp(id) {

    

    let url = APIUrl + 'api/Menus/Up?Id=' + id;

    $.ajax({
        method: 'PUT',
        url: url,
        success: (res) => {

            
            MenusDT.reload();
        },
        error: (err) => {

        },
    });
}

  function MenuDown(id) {

    let url = APIUrl + 'api/Menus/Down?Id=' + id;

    $.ajax({
        method: 'PUT',
        url: url,
        success: (res) => {
            MenusDT.reload();
        },
        error: (err) => {

        },
    });
}


async function ActiveMenu(id) {
    if (await swalConfirm('Are you sure you want to activate this menu ?') == false) {
        return;
    }

    let url = APIUrl + 'api/Menus/ActiveMenus?Active=true&Id=' + id;

    $.ajax({
        method: 'PUT',
        url: url,
        success: (res) => {
            AlertSwalSucceeded('Menu activated successfully');
            MenusDT.reload();
        },
        error: (err) => {
            
            console.log(err);
            AlertSwalError('failed to activated menu try again later');
        },
    });
}

async function unActiveMenu(id) {
    if (await swalConfirm('Are you sure you want to deactivate this menu ?') == false) {
        return;
    }

    let url = APIUrl + 'api/Menus/ActiveMenus?Active=false&Id=' + id;

    $.ajax({
        method: 'PUT',
        url: url,
        success: (res) => {
            AlertSwalSucceeded('Menu deactivated successfully');
            MenusDT.reload();
        },
        error: (err) => {
            console.log(err); 
            AlertSwalError('failed to deactivate menu try again later');
        },
    });
}

