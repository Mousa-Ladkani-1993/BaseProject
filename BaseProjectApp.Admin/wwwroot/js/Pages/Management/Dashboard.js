
var AdsPerCountry_DashboardBarChartsData = [];
var UsersPerCountry_DashboardBarChartsData = [];
var PackagesTotalDiv_DashboardChartsData = [];
var PackagesTotalPerYear_DashboardBarChartsData = [];

jQuery(document).ready(function () {

    AdsPerCountry_DashboardBarChartsInitData(0);
    UsersPerCountry_DashboardBarChartsInitData(0);
    PackagesTotalDiv_DashboardChartsInitData(0);
    PackagesTotalPerYear_DashboardBarChartsInitData();
    ClientPackagesDataTable.init();

});


function AdsPerCountrySelectChange() {
    AdsPerCountry_DashboardBarChartsInitData($('#AdsPerCountrySelect').val());
}

function UsersPerCountrySelectChange() {

    UsersPerCountry_DashboardBarChartsInitData($('#UsersPerCountrySelect').val());
}

function PackagesTotalSelectChange() {
    PackagesTotalDiv_DashboardChartsInitData($('#PackagesTotalSelect').val());
}


function AdsPerCountry_DashboardBarChartsInitData(FilterId) {


    $.ajax({
        type: "GET",
        url: APIUrl + "Api/Dashboard/AdsPerCountry?FilterId=" + FilterId,
        dataType: "json",
        success: function (data) {

            $('#AdsPerCountryDiv_Container').empty();
            $('#AdsPerCountryDiv_Container').append(' <div id="AdsPerCountryDiv"></div>');

            if (data != null && data.length > 0) {

                AdsPerCountry_DashboardBarChartsData = [];

                data.forEach(async (item) => {
                    AdsPerCountry_DashboardBarChartsData.push({ 'y': item.Country, 'a': item.Count });
                });
            }
            else {

                AdsPerCountry_DashboardBarChartsData = [];
            }

            AdsPerCountry_DashboardBarCharts.init();

        },
        error: function (request, status, error) {

            AdsPerCountry_DashboardBarChartsData = [];

            $('#AdsPerCountryDiv_Container').empty();
            $('#AdsPerCountryDiv_Container').append(' <div id="AdsPerCountryDiv"></div>');

            AdsPerCountry_DashboardBarCharts.init()
        }
    });

}

function UsersPerCountry_DashboardBarChartsInitData(FilterId) {

    $.ajax({
        type: "GET",
        url: APIUrl + "Api/Dashboard/UsersPerCountry?FilterId=" + FilterId,
        dataType: "json",
        success: function (data) {

            $('#UsersPerCountryDiv_Container').empty();
            $('#UsersPerCountryDiv_Container').append(' <div id="UsersPerCountryDiv"></div>');


            if (data != null && data.length > 0) {

                UsersPerCountry_DashboardBarChartsData = [];

                data.forEach(async (item) => {
                    UsersPerCountry_DashboardBarChartsData.push({ 'y': item.Country, 'a': item.Count });
                });
            }
            else {

                UsersPerCountry_DashboardBarChartsData = [];
            }

            UsersPerCountry_DashboardBarCharts.init();

        },
        error: function (request, status, error) {

            UsersPerCountry_DashboardBarChartsData = [];

            $('#UsersPerCountryDiv_Container').empty();
            $('#UsersPerCountryDiv_Container').append(' <div id="UsersPerCountryDiv"></div>');



            UsersPerCountry_DashboardBarCharts.init();
        }
    });

}

function PackagesTotalDiv_DashboardChartsInitData(FilterId) {


    $.ajax({
        type: "GET",
        url: APIUrl + "Api/Dashboard/PackagesPerType?FilterId=" + FilterId,
        dataType: "json",
        success: function (data) {

            $('#PackagesTotalDiv_Container').empty();
            $('#PackagesTotalDiv_Container').append('<div id="PackagesTotalDiv" style="height: 430px;" ></div>');


            if (data != null && data.length > 0) {

                PackagesTotalDiv_DashboardChartsData = [];

                data.forEach(async (item) => {
                    PackagesTotalDiv_DashboardChartsData.push({ 'label': item.label, 'data': item.data, 'color': item.color });
                });
            }
            else {

                PackagesTotalDiv_DashboardChartsData = [];
            }

            PackagesTotalDiv_DashboardCharts.init();
        },
        error: function (request, status, error) {

            PackagesTotalDiv_DashboardChartsData = [];

            $('#PackagesTotalDiv_Container').empty();
            $('#PackagesTotalDiv_Container').append('<div id="PackagesTotalDiv" style="height: 430px;" ></div>');

            PackagesTotalDiv_DashboardCharts.init();

        }
    });

}

function PackagesTotalPerYear_DashboardBarChartsInitData() {

    $.ajax({
        type: "GET",
        url: APIUrl + "Api/Dashboard/PackagesPerMonth",
        dataType: "json",
        success: function (data) {

            $('#PackagesTotalPerYearDiv_Container').empty();
            $('#PackagesTotalPerYearDiv_Container').append('<div id="PackagesTotalPerYearDiv"></div>');


            if (data != null && data.length > 0) {

                PackagesTotalPerYear_DashboardBarChartsData = [];

                data.forEach(async (item) => {
                    PackagesTotalPerYear_DashboardBarChartsData.push({ 'y': item.Month, 'a': item.Count, });
                });
            }
            else {

                PackagesTotalPerYear_DashboardBarChartsData = [];
            }

            PackagesTotalPerYear_DashboardBarCharts.init();
        },
        error: function (request, status, error) {

            PackagesTotalPerYear_DashboardBarChartsData = [];

            $('#PackagesTotalPerYearDiv_Container').empty();
            $('#PackagesTotalPerYearDiv_Container').append('<div id="PackagesTotalPerYearDiv"></div>');

            PackagesTotalPerYear_DashboardBarCharts.init();

        }
    });

}


var AdsPerCountry_DashboardBarCharts = function () {

    // Private functions

    var AdsPerCountry = function () {
        // BAR CHART
        new Morris.Bar({
            element: 'AdsPerCountryDiv',
            data: AdsPerCountry_DashboardBarChartsData,
            xkey: 'y',
            ykeys: ['a'],
            labels: ['Ads'],
            barColors: ['#5D78FF']
        });
    }


    return {
        // public functions
        init: function () {
            AdsPerCountry();
        }
    };
}();

var UsersPerCountry_DashboardBarCharts = function () {

    var UsersPerCountry = function () {
        // BAR CHART
        new Morris.Bar({
            element: 'UsersPerCountryDiv',
            data: UsersPerCountry_DashboardBarChartsData,
            xkey: 'y',
            ykeys: ['a'],
            labels: ['Users'],
            barColors: ['#5D78FF']
        });
    }



    return {
        // public functions
        init: function () {
            UsersPerCountry();
        }
    };
}();

var PackagesTotalDiv_DashboardCharts = function () {

    // Private functions 
    var PackagesTotal = function () {

        $.plot($("#PackagesTotalDiv"), PackagesTotalDiv_DashboardChartsData, {
            series: {
                pie: {
                    show: true
                }
            },
            legend: {
                show: false
            }
        });
    }


    return {
        // public functions
        init: function () {
            PackagesTotal();
        }
    };
}();

var PackagesTotalPerYear_DashboardBarCharts = function () {

    // Private functions

    var PackagesTotalPerYear = function () {
        // LINE CHART
        new Morris.Line({
            parseTime: false,
            // ID of the element in which to draw the chart.
            element: 'PackagesTotalPerYearDiv',
            // Chart data records -- each entry in this array corresponds to a point on
            // the chart.
            data: PackagesTotalPerYear_DashboardBarChartsData,
            // The name of the data record attribute that contains x-values.
            xkey: 'y',
            // A list of names of data record attributes that contain y-values.
            ykeys: ['a'],
            // Labels for the ykeys -- will be displayed when you hover over the
            // chart.
            labels: ['Packages'],
            lineColors: ['#5D78FF']
        });

    }


    return {
        // public functions
        init: function () {
            PackagesTotalPerYear();
        }
    };
}();





/////////////////////


var ClientPackagesDT;
var ClientPackagesDataTable = function () {
    var ClientPackagesDataTableObj = function () {
        ClientPackagesDT = $('#ClientPackagesDiv').KTDatatable({
            data: {
                type: 'remote',
                source: {
                    read: {
                        url: APIUrl + 'Api/Dashboard/NearExpiry' +
                            "?PageNumber=" + PageNumber + "&PageSize=" + PageSize,
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
                input: $('#ClientPackagesSearch')
            },
            columns: [
                {
                    field: 'Client',
                    width: 80,
                    title: 'Client',
                },
                //{
                //    field: 'RegistrationDate',
                //    width: 80,
                //    title: 'Registration Date',
                //},

                {
                    field: 'Package',
                    width: 80,
                    title: 'Package',

                    template: function (row) {
                        if (row.Package == null || row.Package == '') { return ''; }
                        else {
                            return `<span   
                                        title="${row.Text}"
                                        data-original-title="Test">${row.Package}</span>`; }
                    }
                },
                //{
                //    field: 'Price',
                //    width: 80,
                //    title: 'Price',
                //},
                {
                    field: 'ExpiryDate',
                    width: 80,
                    title: 'Expiry Date',
                }, 
                {
                    textAlign: 'center',
                    field: 'Paid',
                    width: 80,
                    title: 'Paid',
                    template: function (row) {
                        if (row.Paid == true) { return '<i style="font-size:1.5rem;" class="fa fa-check-circle text-success"></i>'; }
                        else { return '<i style="font-size:1.5rem;" class="fa fa-times-circle text-danger"></i>'; }
                    }
                }
                //,
                //{
                //    field: 'PaymentDate',
                //    width: 80,
                //    title: 'Payment Date',
                //}
                //,
                //{
                //    textAlign: 'center',
                //    field: 'StatusValue',
                //    width: 80,
                //    title: 'Status',
                //    template: function (row) {
                //        if (row.StatusValue == null || row.StatusValue == '') { return ''; }
                //        else if (row.StatusValue == 'Active') { return '<span class="kt-badge kt-badge--inline text-white kt-badge--success font-weight-bold">Active</span>'; }
                //        else if (row.StatusValue == 'Expired') { return `<span class="kt-badge kt-badge--inline text-white kt-badge--warning font-weight-bold">Expired</span>`; }
                //        else if (row.StatusValue == 'Cancelled') { return `<span class="kt-badge kt-badge--inline text-white kt-badge--danger font-weight-bold">Cancelled</span>`; }
                //        else { return `<span class="kt-badge kt-badge--inline kt-badge--warning font-weight-bold">Cancelled</span>`; }
                //    }
                //},

            ],
        });
    };

    return {
        init: function () {
            ClientPackagesDataTableObj();

            initPagination('page-numbers', GetTotalSearchAPIurl(APIUrl), 'TotalRecords', 'PageNumber', 'PageSize', 'SearchClientPackages');


        }
    };
}();




function SearchClientPackages() {


    if (ClientPackagesDT != null)
        ClientPackagesDT.destroy();

    ClientPackagesDataTable.init();

}

function ClearClientPackages() {

}


function GetTotalSearchAPIurl(baseUrl) {

    return baseUrl + 'Api/Dashboard/NearExpiryTotal';

}

