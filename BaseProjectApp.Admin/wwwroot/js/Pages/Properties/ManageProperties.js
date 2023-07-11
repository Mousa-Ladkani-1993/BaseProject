
var PropertiesDT;
var PropertiesDataTable = function () {
    var PropertiesDataTableObj = function () {
        PropertiesDT = $('#PropertiesDiv').KTDatatable({
            data: {
                type: 'remote',
                source: {
                    read: {
                        url: APIUrl + 'Api/Properties/All' +
                            "?PageNumber=" + PageNumber + "&PageSize=" + PageSize
                            + `&SearchTerm=${encodeURIComponent(SearchTerm)}` +
                            `&PriceFrom=${PriceFrom}` +
                            `&PriceTo=${PriceTo}` +
                            `&AreaFrom=${AreaFrom}` +
                            `&AreaTo=${AreaTo}` +
                            `&MonthlyFee=${encodeURIComponent(MonthlyFee)}` +
                            `&BuildingArea=${encodeURIComponent(BuildingArea)}` +
                            `&Floor=${encodeURIComponent(Floor)}` +
                            `&YearBuilt=${encodeURIComponent(YearBuilt)}` +
                            `&Featured=${Featured}` +
                            `&Premium=${Premium}` +
                            `&Published=${Published}` +
                            `&PublishDateStart=${PublishDateStart}` +
                            `&PublishDateEnd=${PublishDateEnd}` +
                            `&Type=${Type}` +
                            `&BusinessType=${BusinessType}` +
                            `&PaymentType=${PaymentType}` +
                            `&Currency=${Currency}` +
                            `&Ownership=${Ownership}` +
                            `&Furnished=${Furnished}` +
                            `&NumberofBedrooms=${NumberofBedrooms}` +
                            `&NumberofBathrooms=${NumberofBathrooms}` +
                            `&NumberofParkingSpaces=${NumberofParkingSpaces}` +
                            `&SelectedAreaUnitId=${SelectedAreaUnitId}` +
                            `&AreaId=${AreaId}` +
                            `&CountryId=${CountryId}` +
                            `&CityId=${CityId}` +
                            `&Status=${Status}
                             `,
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
                input: $('#PropertiesSearch')
            },
            columns: [
                {
                    field: 'Id',
                    width: 30,
                    title: 'Id',
                },
                {
                    textAlign: 'center',
                    field: 'Name',
                    width: 70,
                    title: 'Name',
                },
                {
                    textAlign: 'center',
                    field: '_Type',
                    width: 70,
                    title: 'Type',
                },

                {
                    textAlign: 'center',
                    field: 'Country',
                    width: 70,
                    title: 'Country',
                },
                {
                    textAlign: 'center',
                    field: 'Arealbl',
                    width: 70,
                    title: 'Area',
                },
                {
                    textAlign: 'center',
                    field: 'Pricelbl',
                    width: 70,
                    title: 'Price',
                },
                {
                    textAlign: 'center',
                    width: 55,
                    field: 'Published',
                    title: 'Published',
                    template: function (row) {

                        if (row.Published == true) { return '<i style="font-size:1.5rem;" class="fa fa-check-circle text-success"></i>'; }
                        else { return '<i style="font-size:1.5rem;" class="fa fa-times-circle text-danger"></i>'; }
                    }
                },

                {
                    textAlign: 'center',
                    width: 55,
                    field: 'Premium',
                    title: 'Premium',
                    template: function (row) {
                        if (row.Premium == true) { return '<i style="font-size:1.5rem;" class="fa fa-check-circle text-success"></i>'; }
                        else { return '<i style="font-size:1.5rem;" class="fa fa-times-circle text-danger"></i>'; }
                    }
                },
                {
                    textAlign: 'center',
                    width: 55,
                    field: 'Featured',
                    title: 'Featured',
                    template: function (row) {
                        if (row.Featured == true) { return '<i style="font-size:1.5rem;" class="fa fa-check-circle text-success"></i>'; }
                        else { return '<i style="font-size:1.5rem;" class="fa fa-times-circle text-danger"></i>'; }
                    }
                },

                {
                    textAlign: 'center',
                    width: 55,
                    field: 'Approved',
                    title: 'Approved',
                    template: function (row) {
                        if (row.Approved == true) { return '<i style="font-size:1.5rem;" class="fa fa-check-circle text-success"></i>'; }
                        else { return '<i style="font-size:1.5rem;" class="fa fa-times-circle text-danger"></i>'; }
                    }
                },
                {
                    textAlign: 'center',
                    field: 'Actions',
                    title: 'Actions',
                    sortable: false,
                    width: 120,
                    autoHide: false,
                    overflow: 'visible',
                    template: function (row) {

                        let htmlStr = '';

                        //if (canEdit == '1') {

                        //    if (row.Approved == true) {
                        //        htmlStr += `
                        //    <a  data-row-id="${row.Id}" class="btn btn-sm btn-clean btn-icon btn-icon-md Reject" title="Reject">
                        //        <i class="font-weight-bold la la-times-circle-o"></i>
                        //    </a>`;
                        //    }
                        //    else {
                        //        htmlStr += `
                        //    <a  data-row-id="${row.Id}" class="btn btn-sm btn-clean btn-icon btn-icon-md Approve" title="Approve">
                        //        <i class="font-weight-bold la la-check-circle-o"></i>
                        //    </a>`;
                        //    }
                        //}

                        if (canEdit == '1') { 
                         htmlStr += `
                        <a href="ManageProperty?id=${row.Id}" class="btn btn-sm btn-clean btn-icon btn-icon-md " title="Edit details">
                            <i class="la la-edit"></i>
                        </a>`; 

                        } 
                        

                        if (canEdit == '0' && canView == '1') {
                            htmlStr += `
                            <a href="ManageProperty?id=${row.Id}" class="btn btn-sm btn-clean btn-icon btn-icon-md " title="Show details">
                                <i class="la la-eye"></i>
                            </a>`;  
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
            PropertiesDataTableObj();

            initPagination('page-numbers', GetTotalSearchAPIurl(APIUrl), 'TotalRecords', 'PageNumber', 'PageSize', 'SearchProperties');

            $("#PropertiesDiv").on("click", ".show-delete-data", function (e) {
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
                        DeleteRow(APIUrl + "Api/Properties?id=" + $(this).data("row-id"), PropertiesDT); 
                    }
                    else if (result.dismiss === Swal.DismissReason.cancel) {
                        result = false;
                    }
                })

            })



            $("#PropertiesDiv").on("click", ".Approve", function (e) {
                Swal.fire({
                    title: 'Alert',
                    text: 'Are you sure you want to approve ad ?',
                    type: 'warning',
                    showCancelButton: true,
                    confirmButtonColor: '#3085d6',
                    cancelButtonColor: '#d33',
                    confirmButtonText: 'Yes',
                    cancelButtonText: 'No',

                }).then((result) => {
                    if (result.value) { 
                        ApprovalCallBack($(this).data("row-id"),true)
                    }
                    else if (result.dismiss === Swal.DismissReason.cancel) {
                        result = false;
                    }
                })

            })



            $("#PropertiesDiv").on("click", ".Reject", function (e) {
                Swal.fire({
                    title: 'Alert',
                    text: 'Are you sure you want to reject ad ?',
                    type: 'warning',
                    showCancelButton: true,
                    confirmButtonColor: '#3085d6',
                    cancelButtonColor: '#d33',
                    confirmButtonText: 'Yes',
                    cancelButtonText: 'No',

                }).then((result) => {
                    if (result.value) { 
                        ApprovalCallBack($(this).data("row-id"), false)
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


    InitLocationsSelection('CountryId', 'CityId');

    PublishDateDatepicker.init();

    Dropzone.instances.forEach(dz => dz.destroy());

    $("#ExcelUpload").dropzone({
        maxFilesize: MaxUploadedFileSizeMB,
        autoProcessQueue: false,
        acceptedFiles: '.xls,.xlsx',
        addRemoveLinks: true,
        maxFiles: 1,
        accept: function (file, done) {
            done();
        },
        init: function () {
            this.on("addedfile", function () {

                if (this.files[0].size > MaxUploadedFileSizeB)
                    return false;

                if (toUploadFiles == null)
                    toUploadFiles = []

                if (toUploadFiles.length == 0)
                    toUploadFiles.push(this.files[0]);


                const Extention = this.files[0].upload.filename.split('.').pop();
                if (Extention == 'xlsx' || Extention == 'xls') {
                    $('#UploadAll').prop('disabled', false);
                }

            });

            this.on("removedfile", function () {

                toUploadFiles = this.files;
                if (toUploadFiles != null && toUploadFiles.length == 0)
                    toUploadFiles = null;

                $('#UploadAll').prop('disabled', true);

            });

            this.on("maxfilesexceeded", function (file) {

                this.removeAllFiles();
                this.addFile(file);
                $('#UploadAll').prop('disabled', true);

            });
        }
    });

    PropertiesDataTable.init();


});

$("body").on("click", "#UploadAll", function () {

    var formData = new FormData();
    formData.append("fileName", "ExcelFile");
    formData.append("file", toUploadFiles[0]);
    $("#lblMessageSuccess").hide();


    $.ajax({
        //url: APIUrl + "Api/Properties/Attrs/Import",
        url: APIUrl + "Api/Properties/Import",
        type: 'POST',
        data: formData,
        cache: false,
        contentType: false,
        processData: false,
        success: function (res) {

            $("#fileProgressDiv").hide();
            $('#fileProgressDivPro').hide();

            if (res.Success) {
                $("#lblMessageSuccess").show();
                if (res.RowsErorrsCount > 0) {
                    $("#lblMessage").text(res.RowsErorrs);

                    if (res.RowsErorrsCount > 8) {
                        ExcelRowsErrorMessage = res.RowsErorrs;
                        $('#loggerBtn').css('display', '');
                    }
                }

            }

            else {

                $("#lblMessage").text(res.MajorErorrs);
                $("#lblMessage").text(res.RowsErorrs + res.MajorErorrs);

                if (res.MajorErorrsCount + res.RowsErorrsCount > 8) {
                    ExcelRowsErrorMessage = res.RowsErorrs;
                    ExcelMajorErrorMessage = res.MajorErorrs;
                    $('#loggerBtn').css('display', '');
                }

            }

            $("#lblMessageDiv").show();

        },
        xhr: function () {

            var fileXhr = $.ajaxSettings.xhr();
            if (fileXhr.upload) {
                $("#fileProgressDiv").show();
                fileXhr.upload.addEventListener("progress", function (e) {
                    if (e.lengthComputable) {

                        $("#fileProgress").css('width', e.loaded);

                        $('#fileProgressDivPro').show();


                    }
                }, false);
            }
            return fileXhr;
        }
        ,
        error: function (request, NumberofBedrooms, error) {
            $("#fileProgressDiv").hide();
            $('#fileProgressDivPro').hide();
            AlertSwalError("Error", request.responseJSON.Message)
        }
    });
});

function CurrencyChange() {
    $('.Currencylbl').html($("#Currency option:selected").text());
}

function SelectedAreaUnitIdChange() {
    $('.SelectedAreaUnitIdlbl').html($("#SelectedAreaUnitId option:selected").text());
}

function DownloadLogger() {
    if (ExcelRowsErrorMessage == '' && ExcelMajorErrorMessage == '')
        return;

    download(ExcelRowsErrorMessage, ExcelMajorErrorMessage)
}

function download(textRows, textMajor) {

    let element = document.createElement('a');
    element.setAttribute('href', 'data:text/plain;charset=utf-8,' + encodeURIComponent(textRows));
    element.setAttribute('download', "LogFile.txt");

    element.style.display = 'none';
    document.body.appendChild(element);

    element.click();

    document.body.removeChild(element);


    if (textMajor != '') {
        let elementM = document.createElement('a');
        elementM.setAttribute('href', 'data:text/plain;charset=utf-8,' + encodeURIComponent(textMajor));
        elementM.setAttribute('download', "Major-LogFile.txt");

        elementM.style.display = 'none';
        elementM.body.appendChild(elementM);

        elementM.click();

        document.body.removeChild(elementM);
    }
}

function CountryIdChange() {
    

    let value = $('#CountryId').val();

    if (value == '' || value == 0)
        return false;


    let Cities = GetLocations(value);


    if (Cities != '' && Cities != null && Cities != undefined && Cities.length > 0) {

        for (let i = 0; i < Cities.length; i++) {
            $('#CityId').append(new Option(Cities[i].Value, Cities[i].Id, false, false)).trigger('change');

        }
    }






}

function CityIdChange() {
    let value = $('#CityId').val();

    if (value == '' || value == 0)
        return false;

    let Areas = GetLocations(value);


    $('#AreaId').empty();
    $('#AreaId').append(`<option value="0" selected="selected">---</option>`);

    if (Areas != '' && Areas != null && Areas != undefined && Areas.length > 0) {

        for (let i = 0; i < Areas.length; i++) {
            $('#AreaId').append(`<option value="${Areas[i].Id}">${Areas[i].Value}</option>`);
        }
    }

}

function GetLocations(Id) {
    let values = [];

    jQuery.ajax({
        url: APIUrl + `Api/Locations/RelatedLocations?Id=${Id}`,
        success: function (result) {

            values = result;

        },
        async: false
    });

    return values;

}

function SearchProperties() {

    SelectedAreaUnitId = $('#SelectedAreaUnitId').val();
    SearchTerm = $('#SearchTerm').val();
    PriceFrom = $('#PriceFrom').val() == '' ? 0 : parseFloat($('#PriceFrom').val());
    PriceTo = $('#PriceTo').val() == '' ? 0 : parseFloat($('#PriceTo').val());
    AreaFrom = $('#AreaFrom').val() == '' ? 0 : parseFloat($('#AreaFrom').val());
    AreaTo = $('#AreaTo').val() == '' ? 0 : parseFloat($('#AreaTo').val());;
    MonthlyFee = $('#MonthlyFee').val();
    BuildingArea = $('#BuildingArea').val();
    Floor = $('#Floor').val();
    YearBuilt = $('#YearBuilt').val();

    PublishDateStart = $('#PublishDateStart').val();
    PublishDateEnd = $('#PublishDateEnd').val();

    if ($('#Featured').val() == '-1') { Featured = ""; }
    else { Featured = $('#Featured').val() == "1"; }

    if ($('#Published').val() == '-1') { Published = ""; }
    else { Published = $('#Published').val() == "1"; }

    if ($('#Premium').val() == '-1') { Premium = ""; }
    else { Premium = $('#Premium').val() == "1"; }

    CountryId = $('#CountryId').val();
    CityId = $('#CityId').val();
    AreaId = $('#AreaId').val();

    Type = $('#Type').val();
    BusinessType = $('#BusinessType').val();
    PaymentType = $('#PaymentType').val();
    Currency = $('#Currency').val();
    Ownership = $('#Ownership').val();
    Furnished = $('#Furnished').val();
    NumberofBedrooms = $('#NumberofBedrooms').val();
    NumberofBathrooms = $('#NumberofBathrooms').val();
    NumberofParkingSpaces = $('#NumberofParkingSpaces').val();
    Status = $('#Status').val();

    if (PropertiesDT != null)
        PropertiesDT.destroy();

    PropertiesDataTable.init();

}

function ClearProperties() {

    SelectedAreaUnitId = 0;
    $('#SelectedAreaUnitId').val('0');

    SearchTerm = '';
    $('#SearchTerm').val('');

    PriceFrom = 0;
    $('#PriceFrom').val('');
    PriceTo = 0;
    $('#PriceFrom').val('');

    AreaFrom = 0;
    $('#AreaFrom').val('');
    AreaTo = 0;
    $('#AreaFrom').val('');


    MonthlyFee = '';
    $('#MonthlyFee').val('');
    BuildingArea = '';
    $('#BuildingArea').val('');
    Floor = '';
    $('#Floor').val('');
    YearBuilt = '';
    $('#YearBuilt').val('');

    PublishDateStart = '';
    $('#PublishDateStart').val('');
    PublishDateEnd = '';
    $('#PublishDateEnd').val('');

    Featured = null;
    $('#Featured').val('-1');
    Published = null;
    $('#Published').val('-1');
    Premium = null;
    $('#Premium').val('-1');


    CountryId = '';
    $('#CountryId').val('0');
    CityId = '';
    $('#CityId').val('0');
    AreaId = '';
    $('#AreaId').val('0');

    Type = '';
    $('#Type').val('0');
    BusinessType = '';
    $('#BusinessType').val('0');
    PaymentType = '';
    $('#PaymentType').val('0');
    Currency = '';
    $('#Currency').val('0');
    Ownership = '';
    $('#Ownership').val('0');
    Furnished = '';
    $('#Furnished').val('0');
    NumberofBedrooms = '';
    $('#NumberofBedrooms').val('0');
    NumberofBathrooms = '';
    $('#NumberofBathrooms').val('0');
    NumberofParkingSpaces = '';
    $('#NumberofParkingSpaces').val('0');
    Status = '';
    $('#Status').val('0');

    if (PropertiesDT != null)
        PropertiesDT.destroy();

    PropertiesDataTable.init();
}


$("body").on("click", "#ExportBtn", function () {

    $('#ExportBtnIc').hide();
    $('#ExportBtnSp').show();

    $.ajax({
        url: APIUrl + 'Api/Properties/All'
            + "PageNumber=" + 1 + "&PageSize=" + (+TotalRecords) + '&Export=true', 
        data: $(this).serialize(),
        dataType: 'binary',
        xhrFields: {
            'responseType': 'blob'
        },
        success: function (data, NumberofBedrooms, xhr) {

            let today = new Date();
            let link = document.createElement('a');
            let filename = 'Properties-' + today.toLocaleDateString("en-US") + '.xlsx';

            link.href = URL.createObjectURL(data);
            link.download = filename;
            link.click();

            $('#ExportBtnIc').show();
            $('#ExportBtnSp').hide();

        }
    });

});

 
function GetTotalSearchAPIurl(baseUrl) {

    return baseUrl + 'Api/Properties/AllTotal'
        + `?SearchTerm=${encodeURIComponent(SearchTerm)}` +
        `&PriceFrom=${encodeURIComponent(PriceFrom)}` +
        `&PriceTo=${encodeURIComponent(PriceTo)}` +
        `&AreaFrom=${encodeURIComponent(AreaFrom)}` +
        `&AreaTo=${encodeURIComponent(AreaTo)}` +
        `&MonthlyFee=${encodeURIComponent(MonthlyFee)}` +
        `&BuildingArea=${encodeURIComponent(BuildingArea)}` +
        `&Floor=${encodeURIComponent(Floor)}` +
        `&YearBuilt=${encodeURIComponent(YearBuilt)}` +
        `&Featured=${Featured}` +
        `&Premium=${Premium}` +
        `&Published=${Published}` +
        `&PublishDateStart=${PublishDateStart}` +
        `&PublishDateEnd=${PublishDateEnd}` +
        `&Type=${Type}` +
        `&BusinessType=${BusinessType}` +
        `&PaymentType=${PaymentType}` +
        `&Currency=${Currency}` +
        `&Ownership=${Ownership}` +
        `&AreaId=${AreaId}` +
        `&CountryId=${CountryId}` +
        `&CityId=${CityId}` +
        `&Furnished=${Furnished}` +
        `&NumberofBedrooms=${NumberofBedrooms}` +
        `&NumberofBathrooms=${NumberofBathrooms}` +
        `&NumberofParkingSpaces=${NumberofParkingSpaces}` +
        `&SelectedAreaUnitId=${SelectedAreaUnitId}` +
        `&Status=${Status}`

}
 
function ApprovalCallBack(id, Approve) {

    let url = `${APIUrl}Api/Properties/Approvals/${id}?Approve=${Approve}`;
     
    $.ajax({
        type: "Put",
        url: url, 
        contentType: "application/json",
        success: function (msg) {

            if (PropertiesDT != null)
                PropertiesDT.destroy();

            PropertiesDataTable.init();
        },
        error: function (request, status, error) {
            AlertSwalError("Error", error)
        }
    }); 
}