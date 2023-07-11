 
function initPagination(ActionsDivId, apiUrl, TotalRecordsVar, PageNumberVar, PageSizeVar, searchFunctionName) {


    $(`#${ActionsDivId}`).find('a').remove().end();
    $(`#${ActionsDivId}`).find('select').remove().end();
    $(`#${ActionsDivId}`).find('p').remove().end();

    $.ajax({
        type: "Get",
        url: apiUrl, 
        dataType: "text",
        success: function (Count) {
            
            let total = Count;
            TotalRecords = total;
            let count = 1;

            let ShowingFrom = (window[PageNumberVar] - 1) * window[PageSizeVar];
            let ShowingTo = ShowingFrom + window[PageSizeVar];
            ShowingFrom = ShowingFrom + 1;
            let FirstPage = 1;
            let PrevPage = window[PageNumberVar] - 1;
            let NextPage = window[PageNumberVar] + 1;
            let LastPage = Math.ceil(total / window[PageSizeVar]);


            if (total <= 0)
                return false;

            if (PrevPage > 0) {

                $(`#${ActionsDivId}`).append(`<a class="PagingAnkor PagingAnkor-NavBtn kt-datatable__pager-link--first" href="#${FirstPage}" onclick="getPage(${FirstPage},'${PageNumberVar}', '${searchFunctionName}');"><i class="flaticon2-fast-back"></i></a>`);
                $(`#${ActionsDivId}`).append(`<a class="PagingAnkor PagingAnkor-NavBtn kt-datatable__pager-link--prev" href="#${PrevPage}" onclick="getPage(${PrevPage},'${PageNumberVar}', '${searchFunctionName}');"><i class="flaticon2-back"></i></a>`);
            }
            else {

                $(`#${ActionsDivId}`).append(`<a class="PagingAnkor PagingAnkor-NavBtn-Disabled kt-datatable__pager-link--first"><i class="flaticon2-fast-back"></i></a>`);
                $(`#${ActionsDivId}`).append(`<a class="PagingAnkor PagingAnkor-NavBtn-Disabled kt-datatable__pager-link--prev"><i class="flaticon2-back"></i></a>`);
            }



            if (total > window[PageSizeVar] * 10) {

                if (window[PageNumberVar] >= 5) {

                    count = window[PageNumberVar] - 4;

                    for (let x = 0; x < window[PageSizeVar] * 10; x += window[PageSizeVar]) {

                        $(`#${ActionsDivId}`).append(`<a class="PagingAnkor ${count == window[PageNumberVar] ? 'active btn-brand' : ''} href="#${count}" onclick="getPage(${count}, '${PageNumberVar}', '${searchFunctionName}');">${count}</a>`);

                        

                        if (count == LastPage) {
                            break;
                        }

                        count++;

                    }
                }

                else {

                    for (let x = 0; x < window[PageSizeVar] * 10; x += window[PageSizeVar]) {
                        $(`#${ActionsDivId}`).append(`<a class="PagingAnkor ${count == window[PageNumberVar] ? 'active btn-brand' : ''} href="#${count}" onclick="getPage(${count}, '${PageNumberVar}', '${searchFunctionName}');">${count}</a>`);
                        count++;
                    }

                }

            }
            else {

                for (let x = 0; x < total; x += window[PageSizeVar]) {
                    $(`#${ActionsDivId}`).append(`<a class="PagingAnkor ${count == window[PageNumberVar] ? 'active btn-brand' : ''} href="#${count}" onclick="getPage(${count}, '${PageNumberVar}', '${searchFunctionName}');">${count}</a>`);
                    count++;
                }
            }

            if (window[PageNumberVar] >= LastPage) {
                $(`#${ActionsDivId}`).append(`<a class="PagingAnkor-NavBtn-Disabled  PagingAnkor-NavBtn kt-datatable__pager-link--next" href="#${count}"><i class="flaticon2-next"></i></a>`);
                $(`#${ActionsDivId}`).append(`<a class="PagingAnkor-NavBtn-Disabled  PagingAnkor-NavBtn kt-datatable__pager-link--last" href="#${count}"><i class="flaticon2-fast-next"></i></a>`);
            }
            else {
                $(`#${ActionsDivId}`).append(`<a class="PagingAnkor  PagingAnkor-NavBtn kt-datatable__pager-link--next" href="#${NextPage}" onclick="getPage(${NextPage}, '${PageNumberVar}', '${searchFunctionName}');"><i class="flaticon2-next"></i></a>`);
                $(`#${ActionsDivId}`).append(`<a class="PagingAnkor  PagingAnkor-NavBtn kt-datatable__pager-link--last" href="#${LastPage}" onclick="getPage(${LastPage}, '${PageNumberVar}', '${searchFunctionName}');"><i class="flaticon2-fast-next"></i></a>`);
            }

            $(`#${ActionsDivId}`).append(`<select onchange="ChangePageSize(this, '${PageSizeVar}', '${PageNumberVar}','${searchFunctionName}')" class="PagingSelect ml-auto form-control">
                             <option ${(window[PageSizeVar] == 10 ? 'selected="selected"' : '')} value="10">10</option>
                             <option ${(window[PageSizeVar] == 30 ? 'selected="selected"' : '')} value="30">30</option>
                             <option ${(window[PageSizeVar] == 50 ? 'selected="selected"' : '')} value="50">50</option>
                             </select>`);


            $(`#${ActionsDivId}`).append(`<p class="mt-3 text-right mb-0">Showing ${ShowingFrom} - ${LastPage == window[PageNumberVar] ? total : ShowingTo} of ${total}</p>`);


        },
        error: function (request, status, error) {

        }
    });
}
  
function ChangePageSize(element, PageSizeVar, PageNumberVar, searchFunctionName) {
    
     
    if (PageNumberVar != '' && window[PageNumberVar] != undefined) {
        window[PageNumberVar] = 1;
    }

    if (PageSizeVar != '' && window[PageSizeVar] != undefined) {
        window[PageSizeVar] = +element.value; 
    }

    window[searchFunctionName]();
}
 
function getPage(pageNum, PageNumberVar, searchFunctionName) {
    
    if (PageNumberVar != '' && window[PageNumberVar] != undefined) {
        window[PageNumberVar] = pageNum;
    }

    window[searchFunctionName]();
}

function SearchData(PageNumberVar, searchFunctionName, ExportBtnId = '') {

    

    if (ExportBtnId != '' && document.getElementById(ExportBtnId) != null) {
        $(`#${ExportBtnId}`).show();
    }

    if (PageNumberVar != '' && window[PageNumberVar] != undefined) {
        window[PageNumberVar] = 1;
    }

    

    window[searchFunctionName]();
}

function DeleteRow(apiUrl,objDT) {
      
    $.ajax({ 
        type: "Delete",
        url: apiUrl,
        dataType: "text",
        success: function (data) {
            AlertSwalSucceeded('Deleted Successfully');
            objDT.reload();
        },
        error: function (request, NumberofBedrooms, error) {
            AlertSwalError("Error", request.responseText);
        }
    });


}
