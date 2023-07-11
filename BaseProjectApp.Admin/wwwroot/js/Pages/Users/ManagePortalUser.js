
jQuery(document).ready(function () {
    _getMediaId();
    //Fav_PropertiesDataTable.init();
    //Ads_PropertiesDataTable.init();
    InitDatePicker();
    GetClientPackageCount(clientId)
   
});

function reBuild(val) {
     

    if (val == 'fav') {

        if (Fav_PropertiesDT != null) {
            Fav_PropertiesDT.destroy();
        }

        Fav_PropertiesDataTable.init();
    }
    else if (val == 'ads') {

        if (Ads_PropertiesDT != null) {
            Ads_PropertiesDT.destroy();
        }

        Ads_PropertiesDataTable.init();
    }


}

function UserPackagesInit() {

    if (Packages_PropertiesDT != null) { Packages_PropertiesDT.destroy(); }

    Packages_PropertiesDataTable.init();
}

async function LockUser() {

    if (UserId == null || UserId == '') {

        return false;
    }

    let res = await swalConfirm('Are you sure you want to lock this account ?');

    if (res == false) {
        return;
    }

    let url = APIUrl + 'api/users/LockAccount?Id=' + UserId;

    $.ajax({
        method: 'Put',
        url: url,
        success: (res) => {
            AlertSwalSucceeded('Account locked successfully');

            $('#LockUserbtn').hide();
            $('#UnlockUserbtn').show();

        },
        error: (err) => {
            console.log(err);
            AlertSwalError('Failed to lock account try again later');
        },
    });

}

async function UnlockUser() {

    if (UserId == null || UserId == '') {
        return false;
    }


    if (await swalConfirm('Are you sure you want to unlock this account ?') == false) {
        return;
    }

    let url = APIUrl + 'api/users/UnlockAccount?Id=' + UserId;

    $.ajax({
        method: 'Put',
        url: url,
        success: (res) => {
            AlertSwalSucceeded('Account unlocked successfully');

            $('#LockUserbtn').show();
            $('#UnlockUserbtn').hide();


        },
        error: (err) => {
            console.log(err);
            AlertSwalError('Failed to unlock account try again later');
        },
    });

}

async function VerifyUser() {

    if (clientId == null || clientId == 0) {

        return false;
    }

    if (await swalConfirm('Are you sure you want to unlock this account ?') == false) {
        return;
    }

    let url = APIUrl + 'api/users/VerifyUser?Id=' + clientId;

    $.ajax({
        method: 'Put',
        url: url,
        success: (res) => {
            AlertSwalSucceeded('Account verified successfully');


            $('#Verifiedlbl').show();
            $('#NotVerifiedlbl').hide();

            $('#VerifyUser').hide();
            $('#VerifyUserD').show();


        },
        error: (err) => {
            console.log(err);
            AlertSwalError('Failed to verified account ,,try again later');
        },
    });

}

async function ResetPassword() {

    if (UserId == null || UserId == '') {

        return false;
    }


    if (await swalConfirm('Are you sure you want to reset account password ?') == false) {
        return;
    }

    let url = APIUrl + 'api/users/ResetPassword?Id=' + UserId;

    $.ajax({
        method: 'Put',
        url: url,
        success: (res) => {
            AlertSwalSucceeded('Account password updated successfully');

            if (res != '') {
                $('#ResetPasswordDivSpan').html(res);
                $('#ResetPasswordDiv').show();
            }

        },
        error: (err) => {
            console.log(err);
            AlertSwalError('Failed to update account password ,,try again later');
        },
    });


}

function _getMediaId() {


    const Localhost = DevelopmentLocalhost == null ? '' : DevelopmentLocalhost;

    let ApplicationUrl = window.location.href.toLowerCase().split('/users')[0];

    let ApplicationUrlCut = window.location.href.toLowerCase().split('/users')[0];

    if (ApplicationUrlCut.replace(Localhost, '').length < 2)
        ApplicationUrl = '';

    if ($('#imgInput').val() != undefined && $('#imgInput').val().length > 0) {

        const ImgSRC = $('#imgInput').val();
        $("#imgSRC").attr("src", ApplicationUrl + ImgSRC);
    }
    else {

        $("#imgSRC").attr("src", "../no-results.png")
        $("#imgSRC").attr("title", "Image Not Found..")
    }

}



////////////////////////////////////////


var Fav_PropertiesDT;
var Fav_PropertiesDataTable = function () {
    var Fav_PropertiesDataTableObj = function () {
        Fav_PropertiesDT = $('#Fav_PropertiesDiv').KTDatatable({
            data: {
                type: 'remote',
                source: {
                    read: {
                        url: APIUrl + 'Api/Properties/Favorites' +
                            "?PageNumber=" + Fav_PageNumber + "&PageSize=" + Fav_PageSize
                            + `&ClienId=${encodeURIComponent(clientId)}` 
                            + `&SearchTerm=${encodeURIComponent(Fav_SearchTerm)}`,
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
                input: $('#Fav_PropertiesSearch')
            },
            columns: [
                {
                    field: 'Id',
                    //width: 30,
                    title: 'Id',
                    width: 50,
                    //textAlign: 'center'
                },
                {
                    field: 'Name',
                    //width: 360,
                    title: 'Name',
                    //textAlign: 'center'
                },
                {
                    field: 'Actions',
                    title: 'Actions',
                     width: 50,
                    //textAlign: 'center',
                    template: function (row) {
                        let htmlStr = '';

                        htmlStr += `
                            <a href="${AppName}/Properties/ManageProperty?id=${row.Id}" class="btn btn-sm btn-clean btn-icon btn-icon-md " target="_blank" title="Show details">
                                <i class="la la-eye"></i>
                            </a>`;

                        return htmlStr;

                    },
                }],
        });
    };

    return {
        init: function () {
            Fav_PropertiesDataTableObj();

            initPagination('Fav_page-numbers', Fav_GetTotalSearchAPIurl(APIUrl), 'Fav_TotalRecords', 'Fav_PageNumber', 'Fav_PageSize', 'SearchFav_Properties');

        }
    };
}();
 
function SearchFav_Properties() {

    Fav_SearchTerm = $('#Fav_SearchTerm').val();

    if (Fav_PropertiesDT != null)
        Fav_PropertiesDT.destroy();

    Fav_PropertiesDataTable.init();

}

function ClearFav_Properties() {

    Fav_SearchTerm = '';
    $('#Fav_SearchTerm').val('');

    if (Fav_PropertiesDT != null)
        Fav_PropertiesDT.destroy();

    Fav_PropertiesDataTable.init();
}


////////////////////////////////////////

var Ads_PropertiesDT;
var Ads_PropertiesDataTable = function () {
    var Ads_PropertiesDataTableObj = function () {
        Ads_PropertiesDT = $('#Ads_PropertiesDiv').KTDatatable({
            data: {
                type: 'remote',
                source: {
                    read: {
                        url: APIUrl + 'Api/Properties/Ads' +
                            "?PageNumber=" + Ads_PageNumber + "&PageSize=" + Ads_PageSize
                            + `&ClienId=${encodeURIComponent(clientId)}`
                            + `&UserId=${encodeURIComponent(UserId)}`
                            + `&SearchTerm=${encodeURIComponent(Ads_SearchTerm)}`,
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
                input: $('#Ads_PropertiesSearch')
            },
            columns: [

                {
                    field: 'Id',
                    //width: 30,
                    title: 'Id',
                    width: 50,
                    //textAlign: 'center'
                },
                {
                    field: 'Name',
                    //width: 360,
                    title: 'Name',
                    //textAlign: 'center'
                },
                {
                    field: 'Actions',
                    title: 'Actions',
                    width: 50,
                    //textAlign: 'center',
                    template: function (row) {
                        let htmlStr = '';

                        htmlStr += `
                            <a href="${AppName}/Properties/ManageProperty?id=${row.Id}" class="btn btn-sm btn-clean btn-icon btn-icon-md "  target="_blank" title="Show details">
                                <i class="la la-eye"></i>
                            </a>`;

                        return htmlStr;

                    },
                }],
        });
    };

    return {
        init: function () {
            Ads_PropertiesDataTableObj();

            initPagination('Ads_page-numbers', Ads_GetTotalSearchAPIurl(APIUrl), 'Ads_TotalRecords', 'Ads_PageNumber', 'Ads_PageSize', 'SearchAds_Properties');

        }
    };
}();
 
function SearchAds_Properties() {

    Ads_SearchTerm = $('#Ads_SearchTerm').val();

    if (Ads_PropertiesDT != null)
        Ads_PropertiesDT.destroy();

    Ads_PropertiesDataTable.init();

}

function ClearAds_Properties() {

    Ads_SearchTerm = '';
    $('#Ads_SearchTerm').val('');

    if (Ads_PropertiesDT != null)
        Ads_PropertiesDT.destroy();

    Ads_PropertiesDataTable.init();
}


////////////////////////////////////////


var Packages_PropertiesDT;
var Packages_PropertiesDataTable = function () {
    var Packages_PropertiesDataTableObj = function () {
        Packages_PropertiesDT = $('#Packages_PropertiesDiv').KTDatatable({
            data: {
                type: 'remote',
                source: {
                    read: {
                        url: APIUrl + `Api/ClientPackages/Clients/${encodeURIComponent(clientId)}/All` +
                            "?PageNumber=" + Packages_PageNumber + "&PageSize=" + Packages_PageSize 
                            + `&SearchTerm=${encodeURIComponent(Packages_SearchTerm)}`,
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
                input: $('#Packages_PropertiesSearch')
            },
            columns: [
                {
                    textAlign: 'center',
                    field: 'Package',
                    width: 100,
                    title: 'Package',
                },
                {
                    textAlign: 'center',
                    field: 'StartDateVal',
                    width: 80,
                    title: 'Start Date',
                },
                {
                    textAlign: 'center',
                    field: 'ExpiryDateVal',
                    width: 80,
                    title: 'Expiry Date',
                },
                {
                    textAlign: 'center',
                    field: 'StatusValue',
                    width: 70,
                    title: 'Status',
                    template: function (row) {
                        if (row.StatusValue == null || row.StatusValue == '') { return ''; }
                        else if (row.StatusValue == 'Active') { return '<span class="kt-badge kt-badge--inline text-white kt-badge--success font-weight-bold">Active</span>'; }
                        else if (row.StatusValue == 'Expired') { return `<span class="kt-badge kt-badge--inline text-white kt-badge--warning font-weight-bold">Expired</span>`; }
                        else if (row.StatusValue == 'Cancelled') { return `<span class="kt-badge kt-badge--inline text-white kt-badge--danger font-weight-bold">Cancelled</span>`; }
                        else { return `<span class="kt-badge kt-badge--inline kt-badge--warning font-weight-bold">Cancelled</span>`; }
                    }
                },
                {
                    textAlign: 'center',
                    field: 'Paid',
                    width: 70,
                    title: 'Paid',
                    template: function (row) {
                        if (row.Paid == true) { return '<i style="font-size:1.5rem;" class="fa fa-check-circle text-success"></i>'; }
                        else { return '<i style="font-size:1.5rem;" class="fa fa-times-circle text-danger"></i>'; }
                    }
                },
                {
                    field: 'Actions',
                    title: '',
                    width: 110,
                    template: function (row) {
                        let htmlStr = '';

                        if (canEdit == '1') {

                            htmlStr +=
                                `<a  href="${AppName}/Packages/ManageClientPackage?Id=${row.Id}" class="btn btn-sm btn-clean btn-icon btn-icon-md"  target="_blank"  title="Edit Details">
                                            <i class="la la-edit"></i>
                                        </a>`;

                        }

                        if (canEdit == '0' && canView == '1') {
                            htmlStr += `
                                        <a     href="${AppName}/Packages/ManageClientPackage?Id=${row.Id}" class="btn btn-sm btn-clean btn-icon btn-icon-md" target="_blank" title="View Details">
                                            <i class="la la-eye"></i>
                                        </a>`;
                        }

                        if (canEdit == '1') {
                             
                            if (row.StatusValue == 'Cancelled') {

                                htmlStr += `
                                <a data-row-id="${row.Id}" class="btn btn-sm btn-clean btn-icon btn-icon-md show-activate-data" title="Activate">
                                   <i class="la la-check-circle" style="font-size: 1.5rem;"></i>
                                </a>`;
                            }
                            else {
                                htmlStr += `
                                <a data-row-id="${row.Id}" class="btn btn-sm btn-clean btn-icon btn-icon-md show-cancel-data" title="Cancel">
                                   <i class="la la-times-circle" style="font-size: 1.5rem;"></i>
                                </a>`;
                            }
                   
                        }
                         
                        if (canDelete == '1') {
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
            Packages_PropertiesDataTableObj();

            initPagination('Packages_page-numbers', Packages_GetTotalSearchAPIurl(APIUrl), 'Packages_TotalRecords', 'Packages_PageNumber', 'Packages_PageSize', 'SearchPackages_Properties');

            GetClientPackageCount(clientId)

            $("#Packages_PropertiesDiv").on("click", ".show-row-data", function (e) {
                LoadClientPackage($(this).data("row-id"))
            });


            $("#Packages_PropertiesDiv").on("click", ".show-cancel-data", function (e) {

                Swal.fire({
                    title: 'Alert',
                    text: 'Are you sure you want to cancel ?',
                    type: 'warning',
                    showCancelButton: true,
                    confirmButtonColor: '#3085d6',
                    cancelButtonColor: '#d33',
                    confirmButtonText: 'Yes',
                    cancelButtonText: 'No',

                }).then((result) => {
                    if (result.value) {
                        CancelClientPackage($(this).data("row-id"));
                    }
                    else if (result.dismiss === Swal.DismissReason.cancel) {
                        result = false;
                    }
                })


            })

            $("#Packages_PropertiesDiv").on("click", ".show-activate-data", function (e) {

                Swal.fire({
                    title: 'Alert',
                    text: 'Are you sure you want to activate ?',
                    type: 'warning',
                    showCancelButton: true,
                    confirmButtonColor: '#3085d6',
                    cancelButtonColor: '#d33',
                    confirmButtonText: 'Yes',
                    cancelButtonText: 'No',

                }).then((result) => {
                    if (result.value) {
                        ActivateClientPackage($(this).data("row-id"));
                    }
                    else if (result.dismiss === Swal.DismissReason.cancel) {
                        result = false;
                    }
                })


            })

            

            $("#Packages_PropertiesDiv").on("click", ".show-delete-data", function (e) {

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
                        DeleteRow(APIUrl + "Api/ClientPackages?id=" + $(this).data("row-id"), ClientPackagesDT);
                    }
                    else if (result.dismiss === Swal.DismissReason.cancel) {
                        result = false;
                    }
                });

            })
        }
    };
}();


function SearchPackages_Properties() {

    Packages_SearchTerm = $('#Packages_SearchTerm').val();

    if (Packages_PropertiesDT != null)
        Packages_PropertiesDT.destroy();

    Packages_PropertiesDataTable.init();

}

function ClearPackages_Properties() {

    Packages_SearchTerm = '';
    $('#Packages_SearchTerm').val('');

    if (Packages_PropertiesDT != null)
        Packages_PropertiesDT.destroy();

    Packages_PropertiesDataTable.init();
}


////////////////////////////////////////


function Fav_GetTotalSearchAPIurl(baseUrl) {

    return baseUrl + 'Api/Properties/FavoritesTotal'
        + `?SearchTerm=${encodeURIComponent(Fav_SearchTerm)}`
        + `&ClienId=${encodeURIComponent(clientId)}`;

}
 
function Ads_GetTotalSearchAPIurl(baseUrl) {

    return baseUrl + 'Api/Properties/AdsTotal'
        + `?SearchTerm=${encodeURIComponent(Ads_SearchTerm)}`
        + `&ClienId=${encodeURIComponent(clientId)}`
        + `&UserId=${encodeURIComponent(UserId)}`;

}

function Packages_GetTotalSearchAPIurl(baseUrl) {

    return baseUrl + `Api/ClientPackages/Clients/${encodeURIComponent(clientId)}/AllTotal` + `?SearchTerm=${encodeURIComponent(Packages_SearchTerm)}`; 

}


//////////////////Packages/////////////////////


function Ads_SearchTermonkeyup(obj) {

    
    Ads_SearchTerm = obj.value;

    if (Ads_PropertiesDT != null) {
        Ads_PropertiesDT.destroy();
    }

    Ads_PropertiesDataTable.init();
}

function Fav_SearchTermonkeyup(obj) {

    
    Fav_SearchTerm = obj.value;

    if (Fav_PropertiesDT != null) {
        Fav_PropertiesDT.destroy();
    }

    Fav_PropertiesDataTable.init();
}

function Packages_SearchTermonkeyup(obj) {

    
    Packages_SearchTerm = obj.value;

    if (Packages_PropertiesDT != null) {
        Packages_PropertiesDT.destroy();
    }

    Packages_PropertiesDataTable.init();
}


//////////////////Packages/////////////////////////


function onsubmitForm() {

    let valid = true;

    let $modal = $('#editor-modal')

    let Id;

    if (document.getElementById('txtId').value == "")
        Id = 0;
    else
        Id = document.getElementById('txtId').value;


    let url = APIUrl + 'Api/ClientPackages';

    let obj = new Object();

    obj.Id = parseInt(Id);
    obj.ExpiryDate = $('#ExpiryDate').val();
    obj.PurchaseDate = $('#PurchaseDate').val();
    obj.StartDate = $('#StartDate').val();
    obj.PaymentDate = $('#PaymentDate').val();
    obj.RemainingFreeAds = $('#RemainingFreeAds').val();
    obj.RemainingFeaturedAds = $('#RemainingFeaturedAds').val();
    obj.RemainingPremiumAds = $('#RemainingPremiumAds').val();
    obj.Paid = $('#Modal_Paid').prop('checked');

    if (StartDate == '') {
        alert('Start Date is required');
        valid = false;
    }

    if (!valid)
        return false;

    $.ajax({
        type: "Put",
        url: url,
        data: JSON.stringify(obj),
        contentType: "application/json",
        success: function (msg) {
            SearchPackages_Properties();
            $modal.modal('hide');
        },
        error: function (request, status, error) {
            AlertSwalError("Error", error)
        }
    });

    return false;
}


function CancelClientPackage(Id) {

    $.ajax({
        type: "Put",
        url: APIUrl + `Api/ClientPackages?id=${Id}&CancelPackage=true`,
        dataType: "text",
        success: function () {
            SearchPackages_Properties();
        },
        error: function (request, status, error) {
            //alert(request.responseText);
        }
    });

}


function ActivateClientPackage(Id) {

    $.ajax({
        type: "Put",
        url: APIUrl + `Api/ClientPackages?id=${Id}&CancelPackage=false`,
        dataType: "text",
        success: function () {
            SearchPackages_Properties();
        },
        error: function (request, status, error) {
            //alert(request.responseText);
        }
    });

}


function GetClientPackageCount(Id) {

    $.ajax({
        type: "Get",
        url: APIUrl + `Api/ClientPackages/Clients/${Id}/Ads`,
        dataType: "json",
        success: function (result) {

            //let result = $.parseJSON(data);
            //console.log(result);

            

            $('#AviallabeAdslbl').html(result.AvailableAdsCount);
            $('#FreeAdslbl').html(result.FreeAds);
            $('#FeaturedAdslbl').html(result.FeaturedAds);
            $('#PremiumAdslbl').html(result.PremiumAds);

        },
        error: function (request, status, error) {
             
        }
    });

}


function LoadClientPackage(Id) {

    clearFields();

    $.ajax({
        type: "GET",
        url: APIUrl + "Api/ClientPackages?id=" + Id,
        dataType: "text",
        success: function (data) {

            let result = $.parseJSON(data);

            txtId
            $('#txtId').val(result.Id);

            $('#Modal_Clientid').val(result.ClientId);
            $('#Modal_Clienttxt').val(result.Client);

            $('#Modal_Packageid').val(result.PackageId);
            $('#Modal_Packagetxt').val(result.Package);

            $('#ExpiryDate').val(result.ExpiryDateVal);
            $('#PurchaseDate').val(result.PurchaseDateVal);
            $('#StartDate').val(result.StartDateVal);
            $('#PaymentDate').val(result.PaymentDateVal);

            $('#RemainingFreeAds').val(result.RemainingFreeAds);
            $('#RemainingFeaturedAds').val(result.RemainingFeaturedAds);
            $('#RemainingPremiumAds').val(result.RemainingPremiumAds);

            let htmlElem = '';

            if (result.StatusValue == null || result.StatusValue == '') { htmlElem = ''; }
            else if (result.StatusValue == 'Active') { htmlElem = '<span class="kt-badge kt-badge--inline text-white kt-badge--success font-weight-bold">Active</span>'; }
            else if (result.StatusValue == 'Expired') { htmlElem = `<span class="kt-badge kt-badge--inline text-white kt-badge--warning font-weight-bold">Expired</span>`; }
            else if (result.StatusValue == 'Cancelled') { htmlElem = `<span class="kt-badge kt-badge--inline text-white kt-badge--danger font-weight-bold">Cancelled</span>`; }

            $('#Model_StatusDiv').empty();
            $('#Model_StatusDiv').append(htmlElem);

            $('#Modal_Paid').prop('checked', result.Paid == true);

        },
        error: function (request, status, error) {
            //alert(request.responseText);
        }
    });

}


function clearFields() {


    $('#Modal_Clientid').val('');
    $('#Modal_Clienttxt').val('');
    $('#Modal_Packageid').val('');
    $('#Modal_Packagetxt').val('');

    $('#ExpiryDate').val('');
    $('#PurchaseDate').val('');
    $('#StartDate').val('');
    $('#PaymentDate').val('');

    $('#RemainingFreeAds').val('');
    $('#RemainingFeaturedAds').val('');
    $('#RemainingPremiumAds').val('');

    $('#Model_StatusDiv').empty();

    $('#Modal_Paid').prop('checked', false);



}


//////////////////Packages/////////////////////////
