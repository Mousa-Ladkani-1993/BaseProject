var formatDate = function (date, mask) {
    if (date == "0001/01/01")
        date = new Date(0);
    var d = new Date(date),
        month = '' + (d.getMonth() + 1),
        day = '' + d.getDate(),
        year = d.getFullYear();

    if (month.length < 2) month = '0' + month;
    if (day.length < 2) day = '0' + day;
    var date;

    mask = (mask == null || mask == '') ? 'dd/mm/yyyy' : mask;
    mask = mask.toLowerCase();
    if (mask == 'yyyy/mm/dd')
        date = year + '/' + month + '/' + day;
    else if (mask == 'dd/mm/yyyy')
        date = day + '/' + month + '/' + year;
    else if (mask == 'mm/dd/yyyy')
        date = month + '/' + day + '/' + year;

    return date;
}