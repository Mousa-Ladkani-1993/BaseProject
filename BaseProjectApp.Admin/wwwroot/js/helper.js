var StatusCodes =
{
    badRequest: 400,
    unAuthorized: 403,
    notFound: 404,
    serverError: 500
};

var AutoCompleteDelayTime = 500;


// fixed format dd-mm-yyyy
function dateString2Date(dateString) {
    const dt = dateString.split(/\-|\s/);
    return new Date(dt.slice(0, 3).reverse().join('-') + ' ' + dt[3]);
}

// multiple formats (e.g. yyyy/mm/dd (ymd) or mm-dd-yyyy (mdy) etc.)
function tryParseDateFromString(dateStringCandidateValue, format = "ymd") {
    const candidate = (dateStringCandidateValue || ``)
        .split(/[ :\-\/]/g).map(Number).filter(v => !isNaN(v));
    const toDate = () => {
        format = [...format].reduce((acc, val, i) => ({ ...acc, [val]: i }), {});
        const parts =
            [candidate[format.y], candidate[format.m] - 1, candidate[format.d]]
                .concat(candidate.length > 3 ? candidate.slice(3) : []);
        const checkDate = d => d.getDate &&
            ![d.getFullYear(), d.getMonth(), d.getDate()]
                .find((v, i) => v !== parts[i]) && d || undefined;

        return checkDate(new Date(Date.UTC(...parts)));
    };

    return candidate.length < 3 ? undefined : toDate();
}


function isExistImgURL(url) {
    let img = new Image();
    img.src = url;
    let res = false;

    if (img.complete) {
        res = true;
    }
    else {
        img.onload = () => {
            res = true;
        };
        img.onerror = () => {
            res = false;
        };
    }

    return res;
}

function GetRate(number) {
    //let value = Number.parseFloat(number).toFixed(9);
    //let length = 0;
    return (Number.parseFloat(number).toFixed(9)).toString();
}

const toFixedWithoutZeros = (num, precision) =>
    num.toFixed(precision).replace(/\.0+$/, '');

$(document).on('click', ".Modal-small-img", function () {

    let modalImg = document.getElementById("ImgModal-img");
    let captionText = document.getElementById("ImgModal-caption");

    modalImg.src = $(this).attr('src');
    captionText.innerHTML = $(this).attr('alt');

    $('#ImgModal').modal('toggle');
});


function _UploadFile(file, ToolId, SubmitBtnClass, fileProgressDiv, fileProgressBar, fileProgressBarTxt, AfterUploadCallback) {

    

    const data = new FormData();
    data.append('file', file);
    let Rate = 0;

    var formData = new FormData();

    formData.append("fileName", "File");
    formData.append("file", file);

    $.ajax({
        url: APIUrl + 'api/MediaFile/_SaveFile',
        type: 'POST',
        data: formData,
        cache: false,
        contentType: false,
        processData: false,
        success: function (obj) {

            $("#" + fileProgressBar).css('width', '0');
            $("#" + fileProgressDiv).hide();

            $($($('#' + ToolId).children()[1]).children()[6]).show();
            $("." + SubmitBtnClass).show();

            AfterUploadCallback(obj.url, obj.id);
        },
        xhr: function () {
            let fileXhr = $.ajaxSettings.xhr();
            if (fileXhr.upload) {
                $("#" + fileProgressDiv).show();

                $("." + SubmitBtnClass).hide();
                $($($('#' + ToolId).children()[1]).children()[6]).hide();

                fileXhr.upload.addEventListener("progress", function (e) {
                    if (e.lengthComputable) {
                        Rate = parseInt((e.loaded / e.total) * 100);
                        $("#" + fileProgressBar).css('width', Rate + '%');
                        $("#" + fileProgressBarTxt).html(Rate + '%');
                    }
                }, false);
            }
            return fileXhr;
        }
        ,
        error: function (request, status, error) {
            $("#" + fileProgressBar).css('width', '0');
            $("#" + fileProgressDiv).hide();
        }
    });


}

function _RemoveFile(id) {

    $.ajax({
        type: "Delete",
        url: APIUrl + `api/MediaFile?id=${id}`,
        dataType: "text",
        success: function (result) {
            AlertSwalSucceeded('Removed Successfully');
        },
    });

}

function DataTableCustomAlertSwalError(statusCode, Message) {

    let _message = 'Table failed to load';
    let title = 'Erorr';

    if (statusCode == 0 || statusCode == null) {
        AlertSwalError(title, _message);
        return false;
    }

  

    switch (statusCode) {


        case StatusCodes.badRequest:
            _message = GetText(Message, 'Something is wrong with the sent request..');
            title = 'Bad Request';
            break;

        case StatusCodes.unAuthorized:

            _message = GetText(Message, 'You are unauthorized to view these records..');
            title = 'Access Denied';

            break;

        case StatusCodes.notFound:

            _message = GetText(Message, 'There are no data to display..');
            title = 'Not Found';

            break;

        case StatusCodes.serverError:

            _message = GetText(Message, 'Operation failed..');
            title = 'Internal Server Error';

            break;
    }


    //AlertSwalError(title, _message);

}

function GetText(text, staticText) {

    return (text == '' || text == undefined || text == null ? staticText : text);

}



function InitClientSelection() {

    


    $(".ClientSearch").select2({
        placeholder: "Type To Filter",
        language: {
            errorLoading: function () {
                return "No clients found";
            },
            loadingMore: function () {
                return "Loading more clients";
            },
            noResults: function () {
                return "No clients found";
            },
            searching: function () {
                return "Searching...";
            },
        },
        width: '100%',
        minimumInputLength: 3,
        tags: [],
        createTag: function (tag) {
            return false;
        },
        ajax: {
            delay: AutoCompleteDelayTime,
            url: APIUrl + `api/Web/Accounts/Clients`,
            dataType: 'json',
            type: "GET",
            quietMillis: 50,
            data: function (params) {
                return {
                    SearchTerm: params.term// search term
                };
            },
            processResults: function (data) { 
                return {
                    results: $.map(data, function (item) {
                        return {
                            text: item.FullName,
                            id: item.Id,
                        }
                    })
                };
            }

        }
    });
}
 
function InitPackageSelection() {

    $(".PackageSearch").select2({
        placeholder: "Type To Filter",
        language: {
            errorLoading: function () {
                return "No Packages found";
            },
            loadingMore: function () {
                return "Loading more Packages";
            },
            noResults: function () {
                return "No Packages found";
            },
            searching: function () {
                return "Searching...";
            },
        },
        width: '100%',
        minimumInputLength: 3,
        tags: [],
        createTag: function (tag) {
            return false;
        },
        ajax: {
            delay: AutoCompleteDelayTime,
            url: APIUrl + `api/Packages/Data`,
            dataType: 'json',
            type: "GET",
            quietMillis: 50,
            data: function (params) {
                return {
                    SearchTerm: params.term// search term
                };
            },
            processResults: function (data) {

                return {
                    results: $.map(data, function (item) {
                        return {
                            text: item.Value,
                            id: item.Id,
                        }
                    })
                };
            }

        }
    });
}
 
function InitDatePicker(className = 'datePicker') {
  
    $(`.${className}`).datepicker({
        format: 'dd/mm/yyyy',
        rtl: KTUtil.isRTL(),
        todayBtn: "linked",
        clearBtn: true,
        todayHighlight: true
    });

}
 
function InitLocationsSelection(CountriesSearchId, CitiesSearchId) {

    

    $(".CountriesSearch").select2({
        placeholder: "Type To Filter",
        language: {
            errorLoading: function () {
                return "No countries found";
            },
            loadingMore: function () {
                return "Loading more countries";
            },
            noResults: function () {
                return "No countries found";
            },
            searching: function () {
                return "Searching...";
            },
        },
        width: '100%',
        minimumInputLength: 0,
        tags: [],
        createTag: function (tag) {
            return false;
        },
        ajax: {
            delay: AutoCompleteDelayTime,
            url: APIUrl + `api/Locations/Countries`,
            dataType: 'json',
            type: "GET",
            quietMillis: 50,
            data: function (params) {
                return {
                    ParentId: 0,
                    SearchTerm: params.term// search term
                };
            },
            processResults: function (data) {
                return {
                    results: $.map(data, function (item) {
                        return {
                            text: item.Name,
                            id: item.Id,
                        }
                    })
                };
            }

        }
    });


    $(".CitiesSearch").select2({
        placeholder: "Type To Filter",
        language: {
            errorLoading: function () {
                return "No cities found";
            },
            loadingMore: function () {
                return "Loading more cities";
            },
            noResults: function () {
                return "No cities found";
            },
            searching: function () {
                return "Searching...";
            },
        },
        width: '100%',
        minimumInputLength: 0,
        tags: [''],
        createTag: function (tag) {
            return false;
        }, 
        ajax: { 
            delay: AutoCompleteDelayTime,
            url: APIUrl + `api/Locations/Cities`,
            dataType: 'json',
            type: "GET",
            quietMillis: 50,
            data: function (params) {
                return {
                    ParentId: ($(`#${CountriesSearchId}`).val() != null && $(`#${CountriesSearchId}`).val() != undefined &&
                               $(`#${CountriesSearchId}`).val() > 0 ? $(`#${CountriesSearchId}`).val() : 0),
                    SearchTerm: params.term// search term
                };
            },
            processResults: function (data) {
                return {
                    results: $.map(data, function (item) {
                        return {
                            text: item.Name,
                            id: item.Id,
                        }
                    })
                };
            }

        }
    });




    $(".AreasSearch").select2({
        placeholder: "Type To Filter",
        language: {
            errorLoading: function () {
                return "No areas found";
            },
            loadingMore: function () {
                return "Loading more areas";
            },
            noResults: function () {
                return "No areas found";
            },
            searching: function () {
                return "Searching...";
            },
        },
        width: '100%',
        minimumInputLength: 0,
        tags: [],
        createTag: function (tag) {
            return false;
        },
        ajax: {
            delay: AutoCompleteDelayTime,
            url: APIUrl + `api/Locations/Areas`,
            dataType: 'json',
            type: "GET",
            quietMillis: 50,
            data: function (params) {
                return {
                    ParentId: ($(`#${CitiesSearchId}`).val() != null && $(`#${CitiesSearchId}`).val() != undefined &&
                        $(`#${CitiesSearchId}`).val() > 0 ? $(`#${CitiesSearchId}`).val() : 0),
                    SearchTerm: params.term// search term
                };
            },
            processResults: function (data) {
                return {
                    results: $.map(data, function (item) {
                        return {
                            text: item.Name,
                            id: item.Id,
                        }
                    })
                };
            }

        }
    });

}