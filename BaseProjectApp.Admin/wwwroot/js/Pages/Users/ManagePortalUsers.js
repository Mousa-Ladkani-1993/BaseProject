
var PortalUsersDT;

var PortalUsersDataTable = function () {
    var PortalUsersDataTableObj = function () {
        PortalUsersDT = $('#PortalUsersDiv').KTDatatable({
            data: {
                type: 'remote',
                source: {
                    read: {
                        url: APIUrl + 'Api/Users/PortalUsers/All' +
                            "?PageNumber=" + PageNumber + "&PageSize=" + PageSize + 
                                `&Name=${encodeURIComponent(Name)}` +
                                `&Email=${encodeURIComponent(Email)}` +
                                `&Address=${encodeURIComponent(Address)}` +
                                `&Phone=${encodeURIComponent(Phone)}` +
                                `&Verified=${Verified}` +
                                `&CountryId=${CountryId}` +
                                `&PublishDateStart=${PublishDateStart}` +
                                `&PublishDateEnd=${PublishDateEnd}`,
                        method: 'Get'
                    }
                }
            },
            layout: {
                scroll: false,
                footer: false
            },
            sortable: true,
            pagination: false,
            saveState: false,
            search: {
                input: $('#PortalUsersSearch')
            },
            columns: [
                {
                    field: 'FullName',
                    title: 'Name',
                    width: 100,
                },

                { 
                    field: 'Verified',
                    title: 'Status',
                    width: 80,
                    template: function (row) {

                        let Label = {
                            1: { 'title': 'Verified', 'class': 'kt-badge--success' },
                            2: { 'title': 'Not Verified', 'class': ' kt-badge--danger' }
                        };

                        if (row.Verified == true) {
                            return '<span class="kt-badge ' + Label[1].class + ' kt-badge--inline kt-badge--pill">' + Label[1].title + '</span>';
                        }
                        else {
                            return '<span class="kt-badge ' + Label[2].class + ' kt-badge--inline kt-badge--pill">' + Label[2].title + '</span>';
                        }

                    },
                },

                {
                    field: 'Email', 
                    title: 'Email',
                    width: 180,
                },
                {
                    field: 'Country',
                    title: 'Country',
                },
                {
                    field: 'Phone', 
                    title: 'Phone',
                },   
                {
                    field: 'Actions',
                    title: 'Actions',
                    sortable: false,
                    width: 100,
                    autoHide: false,
                    overflow: 'visible',
                    template: function (row) {
                        let htmlStr = '';

                        if (canEdit == '1') {

                            htmlStr += `
                <a href="ManagePortalUser?id=${row.Id}" class="btn btn-sm btn-clean btn-icon btn-icon-md " title="Edit Details">
                    <i class="la la-edit"></i>
                </a>`;

                        }

                        if (canEdit == '0' && canView == '1') {
                            htmlStr += `
                        <a href="ManagePortalUser?id=${row.Id}" class="btn btn-sm btn-clean btn-icon btn-icon-md " title="Show details">
                            <i class="la la-eye"></i>
                        </a>`;
                        }

                        if (canDelete == '1') {
                            htmlStr += `
                            <a data-row-id="${row.UserId}" class="btn btn-sm btn-clean btn-icon btn-icon-md show-delete-data" title="Delete">
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
            PortalUsersDataTableObj();

            initPagination('page-numbers', GetTotalSearchAPIurl(APIUrl), 'TotalRecords', 'PageNumber', 'PageSize', 'SearchPortalUsers');

            $("#PortalUsersDiv").on("click", ".show-delete-data", function (e) {
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
                        
                        DeleteRow(APIUrl + "Api/Users?id=" + $(this).data("row-id"), PortalUsersDT);
                    }
                    else if (result.dismiss === Swal.DismissReason.cancel) {
                        result = false;
                    }
                })

            })
        }
    };
}();

var PublishDateDatepicker = function () {

    var arrows;
    if (KTUtil.isRTL()) {
        arrows = {
            leftArrow: '<i class="la la-angle-right"></i>',
            rightArrow: '<i class="la la-angle-left"></i>'
        }
    } else {
        arrows = {
            leftArrow: '<i class="la la-angle-left"></i>',
            rightArrow: '<i class="la la-angle-right"></i>'
        }
    }

    var PublishDateDatepickerObj = function () {
        $('#PublishDateDatepickerDiv').datepicker({
            format: 'dd/mm/yyyy',
            rtl: KTUtil.isRTL(),
            todayHighlight: true,
            templates: arrows
        });
    }

    return {
        init: function () {
            PublishDateDatepickerObj();
        }
    };
}();

jQuery(document).ready(function () {

    PublishDateDatepicker.init();

    PortalUsersDataTable.init();

});

function SearchPortalUsers() {

    Name = $('#Name').val();
    Email = $('#Email').val();
    Address = $('#Address').val();
    Phone = $('#Phone').val();
    Verified = $('#Verified').val();
    CountryId = $('#CountryId').val();
    PublishDateStart = $('#PublishDateStart').val();
    PublishDateEnd = $('#PublishDateEnd').val();


    if (PortalUsersDT != null)
        PortalUsersDT.destroy();

    PortalUsersDataTable.init();

}

function ClearPortalUsers() {

    Name = '';
    $('#Name').val('');

    Email = '';
    $('#Email').val('');

    Address = '';
    $('#Address').val('');

    Phone = '';
    $('#Phone').val('');

    CountryId = '';
    $('#CountryId').val('0');

    Verified = -1;
    $('#Verified').val('-1');

    PublishDateStart = '';
    $('#PublishDateStart').val('');
    PublishDateEnd = '';
    $('#PublishDateEnd').val('');

    if (PortalUsersDT != null)
        PortalUsersDT.destroy();

    PortalUsersDataTable.init();
}

function GetTotalSearchAPIurl(baseUrl) {

    return baseUrl + 'Api/Users/PortalUsers/AllTotal' +
        `?Name=${encodeURIComponent(Name)}` +
        `&Email=${encodeURIComponent(Email)}` +
        `&Address=${encodeURIComponent(Address)}` +
        `&Phone=${encodeURIComponent(Phone)}` +
        `&CountryId=${CountryId}` +
        `&Verified=${Verified}` +
        `&PublishDateStart=${PublishDateStart}` +
        `&PublishDateEnd=${PublishDateEnd}`;

}