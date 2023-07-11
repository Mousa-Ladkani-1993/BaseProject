
var UserDT;
var UserDataTable = function () {

    var UserDataTableObj = function () {
        UserDT = $('#UserDiv').KTDatatable({
            data: {
                saveState: 'false',
                type: 'remote',
                source: {
                    read: {
                        url: APIUrl + 'api/users/All' +
                            "?PageNumber=" + PageNumber + "&PageSize=" + PageSize,
                        method: 'Get',
                    }
                }
            },
            layout: {
                scroll: false,
                footer: false
            },
            sortable: true,
            pagination: false,
            search: {
                input: $('#UsersSearch')
            },
            columns: [
                {
                    field: 'Id',
                    title: 'Id',
                    sortable: false,
                    width: 20,
                    type: 'number',
                    textAlign: 'center',
                    responsive: { hidden: 'xl' },
                },
                {
                    field: 'UserName',
                    title: 'User Name',
                    width: 150,
                },
                {
                    field: 'Email',
                    title: 'Email',
                    width: 150,
                },
                {
                    field: 'LockoutEnd',
                    title: 'Locked',

                    width: 50,
                    template: function (row) {
                        if (row.LockoutEnd != null && new Date(row.LockoutEnd) > Date.now())
                            return 'Yes';
                        else
                            return 'No';
                    }
                },
                //{
                //    field: 'PortalUser',
                //    title: 'Type',
                //    width: 70,
                //    template: function (row) {
                //        if (row.PortalUser == true)
                //            return 'Portal User';
                //        else
                //            return 'Client';
                //    }
                //},
                {
                    field: 'Actions',
                    title: 'Actions',
                    sortable: false,
                    width: 150,
                    autoHide: false,
                    overflow: 'visible',
                    template: function (row) {

                        let htmlStr = "";

                        if (row.PortalUser == true) {
                            htmlStr += `
                                     <a href="ManageUser?id=${row.Id}&mode=Edit" class="btn btn-sm btn-clean btn-icon btn-icon-md " title="Edit Details">\
                                        <i class="la la-edit"></i>
                                        </a> 
                                `;
                        }


                        htmlStr += '\
                                    <a href="ManageUser?id='+ row.Id + '&mode=Reset" class="btn btn-sm btn-clean btn-icon btn-icon-md " title="Resset Password">\
                                        <i class="flaticon-list kt-font-brand   "></i>\
                                    </a>\
                                    <a data-row-id='+ row.Id + ' class="btn btn-sm btn-clean btn-icon btn-icon-md show-delete-data" title="Delete">\
                                        <i class="la la-trash"></i>\
                                    </a>\
                                ';



                        if (row.LockoutEnd != null && new Date(row.LockoutEnd) > Date.now()) {
                            htmlStr += `
                                    <a onclick="unlockAccount('${row.Id}')" class="btn btn-sm btn-clean btn-icon btn-icon-md title="unlock">
                                        <i class="la la-key"></i>
                                    </a>
                                `;
                        }
                        else {
                            htmlStr += `
                                    <a onclick="lockAccount('${row.Id}')" class="btn btn-sm btn-clean btn-icon btn-icon-md title="lock">
                                        <i class="la la-lock"></i>
                                    </a>
                                `;
                        }

                        return htmlStr;
                    },
                }],
        });
    };

    return {
        init: function () {
            UserDataTableObj(); 
            initPagination('page-numbers', (APIUrl + 'Api/Users/AllTotal'), 'TotalRecords', 'PageNumber', 'PageSize', 'SearchUsers');

            $("#UserDiv").on("click", ".show-delete-data", function (e) {  
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
                         
                        DeleteRow(APIUrl + "Api/users?id=" + $(this).data("row-id"), UserDT); 
                    }
                    else if (result.dismiss === Swal.DismissReason.cancel) {
                        result = false;
                    }
                });

            });

        }
    };
}();

jQuery(document).ready(function () {

    UserDataTable.init(); 

});
 
function SearchUsers() {
    
    if (UserDT != null)
        UserDT.destroy();

    UserDataTable.init(); 
}
  
async function lockAccount(id) {
    if (await swalConfirm('are you sure you want to lock this account ?') == false) {
        return;
    }

    let url = APIUrl + 'api/users/LockAccount?Id=' + id;

    $.ajax({
        method: 'Patch',
        url: url,
        success: (res) => {
            AlertSwalSucceeded('account locked successfully');
            UserDT.reload();
        },
        error: (err) => {
            console.log(err);
            AlertSwalError('failed to lock account try again later');
        },
    });
}

async function unlockAccount(id) {
    if (await swalConfirm('are you sure you want to unlock this account ?') == false) {
        return;
    }

    let url = APIUrl + 'api/users/UnlockAccount?Id=' + id;

    $.ajax({
        method: 'Patch',
        url: url,
        success: (res) => {
            AlertSwalSucceeded('account unlocked successfully');
            UserDT.reload();
        },
        error: (err) => {
            console.log(err);
            AlertSwalError('failed to unlock account try again later');
        },
    });
}


 