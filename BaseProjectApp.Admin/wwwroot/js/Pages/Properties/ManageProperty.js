

function redrawDT(table) {
    

    if (table == 'MediaFiles') {

        if (PropertyId > 0) {

            if (MediaFileDT != null) {
                MediaFileDT.destroy()
            }

            $('#divNotDefinedPropertyImage').hide();
            $('#filesTabcontent').show();
            MediaFileDataTable.init();
        }
        else {
            $('#filesTabcontent').hide();
            $('#divNotDefinedPropertyImage').show();
        }
    }
}

function FeaturedchkChange() {
    if ($('#Featuredchk').is(":checked")) {
        $("#Premiumchk").prop("checked", false);
    }
}

function PremiumchkChange() {
    if ($('#Premiumchk').is(":checked")) {
        $("#Featuredchk").prop("checked", false);
    }
}

jQuery(document).ready(function () {

    


    InitClientSelection(); 
    InitLocationsSelection('Country', 'City');

    $('.DecimalNumber').each(function (index) {

        let value = $(this).val();

        if (value != '' && value != null) {
            $(this).val(toFixedWithoutZeros(parseFloat(value), 2));
        }

    });


    if (PropertyId > 0) {


        if ($('#CityIdVal').val() != '') {
            $('#City').append(new Option($('#CityNameVal').val(), $('#CityIdVal').val(), true, true)).trigger('change');
        }

        if ($('#CountryIdVal').val() != '') {
            $('#Country').append(new Option($('#CountryNameVal').val(), $('#CountryIdVal').val(), true, true)).trigger('change');
        }

        if ($('#AreaIdVal').val() != '') {
            $('#Area').append(new Option($('#AreaNameVal').val(), $('#AreaIdVal').val(), true, true)).trigger('change');
        }


        TypeChange();
        CountryChange();
        CityChange();
         
        TerraceSizeVal = $('#TerraceSizetxt').val();
        GardenSizeVal = $('#GardenSizetxt').val();
        AreaVal = $('#Areatxt').val();
        AreaUnitIdVal = $('#AreaMeasurement').val();
        SelectedCurrencyId = $('#CurrencyId').val();
        PriceVal = $('#Pricetxt').val();
        ExpectedMonthlyFeeTxtVal = $('#ExpectedMonthlyFeeTxt').val();


        let data_ClientName_val = $('#data_ClientName').val();
        let data_ClientId_val = $('#data_ClientId').val();

        if (data_ClientName_val != null && data_ClientId_val != null) {
            let newOption = new Option(data_ClientName_val, data_ClientId_val, true, true);
            $('#ClientId').append(newOption).trigger('change');
        }

    }


    var Amenities = $('#AmenitiesList').val();
    loadAmenitiesDropDownsList(Amenities);

    var Keywords = $('#KeywordsList').val();
    loadKeywordsDropDownsList(Keywords);


    $('.datePicker').datepicker({
        minDate: 0,
        format: 'mm/dd/yyyy',
        rtl: KTUtil.isRTL(),
        todayBtn: "linked",
        clearBtn: true,
        todayHighlight: true
    });

});


async function CurrencyIdChange() {

    

    if ($("#CurrencyId").val() != '0') {
        $('#Pricetxt').attr('placeholder', $("#CurrencyId option:selected").text());
        $('#ExpectedMonthlyFeeTxt').attr('placeholder', $("#CurrencyId option:selected").text());
    }
    else if ($("#CurrencyId").val() == '0') {
        $('#Pricetxt').attr('placeholder', '');
        $('#ExpectedMonthlyFeeTxt').attr('placeholder', '');
    }


    //if (SelectedCurrencyId > 0 && (PriceVal > 0 || ExpectedMonthlyFeeTxtVal > 0)) {

    if (SelectedCurrencyId > 0 && ($('#Pricetxt').val() > 0 || $('#ExpectedMonthlyFeeTxt').val() > 0)) {

        PriceVal = $('#Pricetxt').val();
        ExpectedMonthlyFeeTxtVal = $('#ExpectedMonthlyFeeTxt').val();

        let res = await swalConfirm('Do you want to change (Price,Expected Monthly Fee) value relating to the new currency ?')

        if (res == true) {
            ChangeCurrencyValues()
        }

    }
    else {
        SelectedCurrencyId = $('#CurrencyId').val();
    }

}

function ChangeCurrencyValues() { 

    let CurrentCurrencyId = $('#CurrencyId').val();

    $.ajax({
        type: "GET",
        url: APIUrl + `Api/CurrenciesRates/Calculations?FeeValue=${ExpectedMonthlyFeeTxtVal}&PriceValue=${PriceVal}&FromCurrencyId=${SelectedCurrencyId}&ToCurrencyId=${CurrentCurrencyId}`,
        dataType: "json",
        success: function (res) {

            SelectedCurrencyId = CurrentCurrencyId;

            PriceVal = res.Price;
            $('#Pricetxt').val(PriceVal);

            ExpectedMonthlyFeeTxtVal = res.ExpectedMonthlyFee;
            $('#ExpectedMonthlyFeeTxt').val(ExpectedMonthlyFeeTxtVal);

        },
        error: function (request, status, error) {

        }
    });
}

function PricetxtKeyUp() {
    PriceVal = $('#Pricetxt').val();
}

async function AreaMeasurementChange() {

    let val = $('#AreaMeasurement').val();

    if (val == Square_Meter) {
        $('#Areatxt').attr('placeholder', 'm²');
        $('#TerraceSizetxt').attr('placeholder', 'm²');
        $('#GardenSizetxt').attr('placeholder', 'm²');
        $('#BuildingArea').attr('placeholder', 'm²');
    }
    else if (val == Square_Feet) {
        $('#Areatxt').attr('placeholder', 'sqft');
        $('#TerraceSizetxt').attr('placeholder', 'sqft');
        $('#GardenSizetxt').attr('placeholder', 'sqft');
        $('#BuildingArea').attr('placeholder', 'sqft');

    }
    else if (val == '') {
        $('#Areatxt').attr('placeholder', 'm²/sqft');
        $('#TerraceSizetxt').attr('placeholder', 'm²/sqft');
        $('#GardenSizetxt').attr('placeholder', 'm²/sqft');
        $('#BuildingArea').attr('placeholder', 'm²/sqft');
    }

    if ((AreaVal > 0 || TerraceSizeVal > 0 || GardenSizeVal > 0) && val != '') {

        let res = await swalConfirm('Do you want to change (Area,Terrace Size , Garden Size) values relating to the new unit (' + $('#AreaMeasurement option:selected').text() + ')?')

        if (res == true) {
            ChangeAreaValues(val);
        }
        else if (val != '' && val != '0') {

            AreaVal = $('#Areatxt').val() == '' ? 0 : $('#Areatxt').val();
            TerraceSizeVal = $('#TerraceSizetxt').val() == '' ? 0 : $('#TerraceSizetxt').val();
            GardenSizeVal = $('#GardenSizetxt').val() == '' ? 0 : $('#GardenSizetxt').val();

            AreaUnitIdVal = val;
        }
    }
}

function ChangeAreaValues(val) {


    if ((AreaVal > 0 || TerraceSizeVal > 0 || GardenSizeVal > 0) && val != '') {

        let newAreaVal = 0;
        let newTerraceSizeVal = 0;
        let newGardenSizeVal = 0;

        if (AreaUnitIdVal != '0' && AreaUnitIdVal != val && val == Square_Meter) {

            newAreaVal = AreaVal * Feet2_To_Meter2;
            newAreaVal = Math.ceil(newAreaVal);

            newTerraceSizeVal = TerraceSizeVal * Feet2_To_Meter2;
            newTerraceSizeVal = Math.ceil(newTerraceSizeVal);

            newGardenSizeVal = GardenSizeVal * Feet2_To_Meter2;
            newGardenSizeVal = Math.ceil(newGardenSizeVal);

        }
        else if (AreaUnitIdVal != '0' && AreaUnitIdVal != val && val == Square_Feet) {

            newAreaVal = AreaVal * Meter2_To_Feet2;
            newAreaVal = Math.ceil(newAreaVal);


            newTerraceSizeVal = TerraceSizeVal * Meter2_To_Feet2;
            newTerraceSizeVal = Math.ceil(newTerraceSizeVal);

            newGardenSizeVal = GardenSizeVal * Meter2_To_Feet2;
            newGardenSizeVal = Math.ceil(newGardenSizeVal);
        }

        $('#Areatxt').val(newAreaVal);
        AreaVal = newAreaVal;

        $('#TerraceSizetxt').val(newTerraceSizeVal);
        TerraceSizeVal = newTerraceSizeVal;

        $('#GardenSizetxt').val(newGardenSizeVal);
        GardenSizeVal = newGardenSizeVal;

        AreaUnitIdVal = val;
    }
    else if (val != '' && val != '0') {
        AreaVal = $('#Areatxt').val() == '' ? 0 : $('#Areatxt').val();
        TerraceSizeVal = $('#TerraceSizetxt').val() == '' ? 0 : $('#TerraceSizetxt').val();
        GardenSizeVal = $('#GardenSizetxt').val() == '' ? 0 : $('#GardenSizetxt').val();

        AreaUnitIdVal = val;
    }

}

function TypeChange() {

    //Sub
    //To Do make sure Type Change Sub Types 

    let Value = $('#TypeId').val();

    if (Value == '3' || $("#TypeId option:selected").text().toLowerCase() == 'land') {
        $('.BuildingAreaclass').show();
    }
    else {
        $('.BuildingAreaclass').hide();
    }

    if (Value == '7') {
        $('.BuildingsDiv').show();
    }
    else {
        $('.BuildingsDiv').hide();
    }

    if (Value == '5') {
        $('.RoomsDiv').show();
    }
    else {
        $('.RoomsDiv').hide();
    }
     
    TypeChanged(Value);
}

function TypeChanged(Value) {


    let Selected = $('#SubTypeId').val();

    let SubTypes = TypesObj.filter(s => s.ParentId == Value);

    $('#SubTypeId').empty();
    $('#SubTypeId').append($('<option>').val('0').text('---'));

    if (SubTypes != null && SubTypes.length > 0) {

        for (let i = 0; i < SubTypes.length; i++) {
            let item = SubTypes[i];
            $('#SubTypeId').append($('<option>').val(item.Id).text(item.Name));
        }

    }

    if (Selected != '0' && Selected != '' && Selected != null) {
        $('#SubTypeId').val(Selected);
    }
}

function CountryChange() {


    let Selected = $('#City').val();
    let Value = $('#Country').val();

    let Cities = GetLocations(Value);

    $('#City').empty();
    $('#City').append($('<option>').val('0').text('---'));

    if (Cities != null && Cities.length > 0) {

        for (let i = 0; i < Cities.length; i++) {
            let item = Cities[i];
            $('#City').append($('<option>').val(item.Id).text(item.Value));
        }

    }

    if (Selected != '0' && Selected != '' && Selected != null) {
        $('#City').val(Selected);
    }
}

function CityChange() {



    let Selected = $('#Area').val();
    let Value = $('#City').val();
    let Areas = GetLocations(Value);

    $('#Area').empty();
    $('#Area').append($('<option>').val('0').text('---'));

    if (Areas != null && Areas.length > 0) {

        for (let i = 0; i < Areas.length; i++) {
            let item = Areas[i];
            $('#Area').append($('<option>').val(item.Id).text(item.Value));
        }

    }

    

    if (Selected != '0' && Selected != '' && Selected != null) {
        $('#Area').val(Selected);
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



function loadKeywordsDropDownsList(Keywords) {

    $.ajax({
        type: "GET",
        url: "./ManageProperty?handler=LoadKeywordsList",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        beforeSend: function (xhr) {
            xhr.setRequestHeader("XSRF-TOKEN",
                $('input:hidden[name="__RequestVerificationToken"]').val());
        },
        data: "{}",
        success: function (result) {

            let Data = [
                {
                    id: -1,
                    title: 'Select All',
                    subs: result
                }
            ];

            KeywordsComboTree = $('#KeywordsList').comboTree({
                source: Data,
                isMultiple: true,
                cascadeSelect: true,
                collapse: false
            });

            if (Keywords != "") {
                let arr = Keywords.split(' ');
                KeywordsComboTree.setSelection(arr);
            }
        },
        error: function (result) {
            alert('error when get drop downs list');
        }
    });

}


function loadAmenitiesDropDownsList(Amenities) {

    $.ajax({
        type: "GET",
        url: "./ManageProperty?handler=LoadAmenitiesList",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        beforeSend: function (xhr) {
            xhr.setRequestHeader("XSRF-TOKEN",
                $('input:hidden[name="__RequestVerificationToken"]').val());
        },
        data: "{}",
        success: function (result) {

            let Data = [
                {
                    id: -1,
                    title: 'Select All',
                    subs: result
                }
            ];

            AmenitiesComboTree = $('#AmenitiesList').comboTree({
                source: Data,
                isMultiple: true,
                cascadeSelect: true,
                collapse: false
            });

            //Load Countries
            if (Amenities != "") {
                let arr = Amenities.split(' ');
                AmenitiesComboTree.setSelection(arr);
            }
        },
        error: function (result) {
            alert('error when get drop downs list');
        }
    });

}

$(document).ready(() => {
    rxjs.fromEvent(window, 'media-file-p1_save-done').subscribe((res) => {
        if (res.detail.success) {
            MediaFileDT.reload();
        }
    });
});


var MediaFileDT;
var MediaFileDataTable = function () {
    var MediaFileObj = function () {
        MediaFileDT = $('#dt').KTDatatable({
            data: {
                type: 'remote',
                source: {
                    read: {
                        url: APIUrl + 'Api/MediaFile/GetPropertyFiles?PropertyId=' + PropertyId,
                        method: 'Get',
                    }
                }
            },
            layout: {
                scroll: false,
                footer: false
            },
            sortable: true,
            pagination: true,
            search: {
                input: $('#MediaFilesSearch')
            },
            columns: [
                {
                    field: 'Id',
                    title: 'Id',
                    type: 'number',
                    textAlign: 'left', 
                },
                {
                    field: 'TypeId',
                    title: 'Type',
                    width: 60,
                    textAlign: 'left',
                    template: function (row) {
                        if (row.TypeId == '3')
                            return 'File';
                        else if (row.TypeId == '2')
                            return 'Video';
                        else
                            return 'Image';
                    }
                },
                {
                    field: 'CaptionEn',
                    title: 'Caption',
                    textAlign: 'left',
                },
                {
                    field: 'YouTubePath',
                    title: 'Video Link',
                    textAlign: 'left',
                    template: function (row) {
                        if (row.YouTubePath != "" && row.YouTubePath != null)
                            return '<a href="#" data-row-id="' + row.YouTubePath + '" class="btn btn-outline-brand btn-icon"><i class="fa fa-eye"></i></a>';
                        else
                            return '';
                    },
                },
                {
                    field: 'DisplayOrder',
                    title: 'Display Order',
                    textAlign: 'left',
                },
                {
                    field: 'FilePath',
                    title: 'File / Image',
                    textAlign: 'left',
                    template: function (row) {
                        if (row.TypeId == '3')
                            return '<a href="#" data-row-id="' + row.FilePath + '" class="btn btn-outline-brand btn-icon"><i class="fa fa-eye"></i></a>';
                        else
                            return '<a href="#" data-row-id="' + row.FilePath + '" class="btn btn-outline-brand btn-icon"><img src="' + row.FilePath + '" width="60" height="60"/></a > ';
                    },
                },
                {
                    field: 'Actions',
                    title: 'Actions',
                    sortable: false,
                    autoHide: false,
                    overflow: 'visible',
                    template: function (row) {

                        let htmlStr = '';

                        if (canEdit == '1') {
                            htmlStr += `<a data-toggle="modal" data-target="#editor-modal" data-row-id='` + row.Id + `'
                                class="btn btn-sm btn-clean btn-icon btn-icon-md show-row-data disabledClassToHide" title="Edit details">
                                <i class="la la-edit"></i></a><a  data-row-id='` + row.Id + `' class="btn btn-sm btn-clean btn-icon btn-icon-md show-delete-data disabledClassToHide"
                                title="Delete"><i class="la la-trash"></i></a>`;
                        }

                        return htmlStr;
                    },
                }],
        });
    };

    return {
        // public functions
        init: function () {
            MediaFileObj();

            $("#dt").on("click", ".show-row-data", function (e) {
                loadMediafilesModal('media-file-p1', PropertyId, 'PropertyFile', $(this).data("row-id"));
                // EditFile($(this).data("row-id"))
            })
            $("#dt").on("click", ".show-delete-data", function (e) {
                DeleteFile($(this).data("row-id"))
            })
            $("#dt").on("click", ".btn-outline-brand", function (e) {
                var url = $(this).data("row-id");
                var win = window.open(url, '_blank');
                win.focus();
            })
        }
    };
}();

function onClickAddMediaFile() {
    loadMediafilesModal('media-file-p1', PropertyId, 'PropertyFile', 0);
}

function DeleteFile(id) {
    rxjs.fromEvent(window, `${id}_file-delete-done`)
        .pipe(rxjs.operators.take(1))
        .subscribe((res) => {
            if (res.detail.success) MediaFileDT.reload();
        });

    deleteMediaFile(id);
}














/////////////////////////////////////Toastar 



var SelectedClientToast = function () {

    // Private functions

    // basic SelectedClientToastObj
    var SelectedClientToastObj = function () {
        // init bootstrap switch
        $('[data-switch=true]').bootstrapSwitch();


        function NotifyToast() {
            content.message = 'New order has been placed';
            content.title = 'Notification Title';
            content.icon = 'icon la la-warning';

            $.notify(content, {
                type: Toasttype,
                //'success', //danger
                allow_dismiss: false,
                newest_on_top: true,
                mouse_over: false,
                showProgressbar: false,
                spacing: 10,
                timer: 2000,
                placement: {
                    from: 'Top',
                    align: 'right'
                },
                offset: {
                    x: 30,
                    y: 30
                },
                delay: 1000,
                z_index: 10000,
                animate: {
                    enter: 'animated ' + Toastenter,
                    //+ 'pulse', //shake
                    exit: 'animated pulse'
                }
            });
        }


    }

    return {
        // public functions
        init: function () {
            SelectedClientToastObj();
        }
    };
}();

//jQuery(document).ready(function () {
//    SelectedClientToast.init();
//});

//////////////////////////////////////Toast