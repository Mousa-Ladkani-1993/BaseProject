
$(".InputDecimal18_10").on('keypress', function (e) {

    let CurrentValue = $(this).val();
    let value = String.fromCharCode(e.which);

    if (value == '.' && !$(this).val().includes('.'))
        return;

    if (!$.isNumeric(value))
        event.preventDefault();


    if (CurrentValue.includes('.')) {

        let DigitsNumber = CurrentValue.split('.')[1].length;

        if (DigitsNumber == 10)
            event.preventDefault();
    }
    else {
        if (CurrentValue.length == 8)
            event.preventDefault();
    }

});

$(".InputDecimal18_10").bind("paste", function (e) {



    let CurrentValue = $(this).val();
    let Addedvalue = e.originalEvent.clipboardData.getData('text');

    let Decimal18_10Reg = /^(?![0.]*$)\d{0,8}(?:\.\d{0,10})?$/;

    if (!Decimal18_10Reg.test(CurrentValue.trim() + Addedvalue.trim()))
        event.preventDefault();


    //if (value.includes('.') && CurrentValue.includes('.'))
    //    event.preventDefault();


    //if (!$.isNumeric(value) && value != '.')
    //    event.preventDefault();


    //if (CurrentValue.includes('.')) {

    //    let DigitsNumber = CurrentValue.split('.')[1].length;
    //    let AddedDigitsNumber = value.length;

    //    if (DigitsNumber + AddedDigitsNumber > 10)
    //        event.preventDefault();
    //}
    //else {
    //    let AddedDigitsNumber = value.length;
    //    if (CurrentValue.length + AddedDigitsNumber > 8)
    //        event.preventDefault();
    //} 

});

$(".InputDecimal18_2").on('keypress', function (e) {

    let CurrentValue = $(this).val();
    let value = String.fromCharCode(e.which);

    if (value == '.' && !$(this).val().includes('.'))
        return;

    if (!$.isNumeric(value))
        event.preventDefault();


    if (CurrentValue.includes('.')) {

        let DigitsNumber = CurrentValue.split('.')[1].length;

        if (DigitsNumber == 2)
            event.preventDefault();
    }
    else {
        if (CurrentValue.length == 16)
            event.preventDefault();
    }

});

$(".InputDecimal18_2").bind("paste", function (e) {



    let CurrentValue = $(this).val();
    let Addedvalue = e.originalEvent.clipboardData.getData('text');

    let Decimal18_10Reg = /^(?![0.]*$)\d{0,16}(?:\.\d{0,2})?$/;

    if (!Decimal18_10Reg.test(CurrentValue.trim() + Addedvalue.trim()))
        event.preventDefault();


    //let CurrentValue = $(this).val();
    //let value = e.originalEvent.clipboardData.getData('text');

    //if (value.includes('.') && CurrentValue.includes('.'))
    //    event.preventDefault();


    //if (!$.isNumeric(value) && value != '.')
    //    event.preventDefault();


    //if (CurrentValue.includes('.')) {

    //    let DigitsNumber = CurrentValue.split('.')[1].length;
    //    let AddedDigitsNumber = value.length;

    //    if (DigitsNumber + AddedDigitsNumber > 2)
    //        event.preventDefault();
    //}
    //else {
    //    let AddedDigitsNumber = value.length;
    //    if (CurrentValue.length + AddedDigitsNumber > 16)
    //        event.preventDefault();
    //}

});


$(".InputDecimal8_6").on('keypress', function (e) {


    let CurrentValue = $(this).val();
    let value = String.fromCharCode(e.which);

    if (value == '.' && !$(this).val().includes('.'))
        return;

    if (!$.isNumeric(value))
        event.preventDefault();


    if (CurrentValue.includes('.')) {

        let DigitsNumber = CurrentValue.split('.')[1].length;

        if (DigitsNumber == 6)
            event.preventDefault();
    }
    else {
        if (CurrentValue.length == 2)
            event.preventDefault();
    }

});

$(".InputDecimal8_6").bind("paste", function (e) {


    let CurrentValue = $(this).val();
    let Addedvalue = e.originalEvent.clipboardData.getData('text');

    let Decimal8_6Reg = /^(?![0.]*$)\d{0,2}(?:\.\d{0,6})?$/;

    if (!Decimal8_6Reg.test(CurrentValue.trim() + Addedvalue.trim()))
        event.preventDefault();


});



$(".InputDecimal9_6").on('keypress', function (e) {


    let CurrentValue = $(this).val();
    let value = String.fromCharCode(e.which);

    if (value == '.' && !$(this).val().includes('.'))
        return;

    if (!$.isNumeric(value))
        event.preventDefault();


    if (CurrentValue.includes('.')) {

        let DigitsNumber = CurrentValue.split('.')[1].length;

        if (DigitsNumber == 6)
            event.preventDefault();
    }
    else {
        if (CurrentValue.length == 3)
            event.preventDefault();
    }

});

$(".InputDecimal9_6").bind("paste", function (e) {


    let CurrentValue = $(this).val();
    let Addedvalue = e.originalEvent.clipboardData.getData('text');

    let Decimal19_6Reg = /^(?![0.]*$)\d{0,3}(?:\.\d{0,6})?$/;

    if (!Decimal19_6Reg.test(CurrentValue.trim() + Addedvalue.trim()))
        event.preventDefault();


});


$(".InputDecimal18_6").on('keypress', function (e) {



    let CurrentValue = $(this).val();
    let value = String.fromCharCode(e.which);

    if (value == '.' && !$(this).val().includes('.'))
        return;

    if (!$.isNumeric(value))
        event.preventDefault();


    if (CurrentValue.includes('.')) {

        let DigitsNumber = CurrentValue.split('.')[1].length;

        if (DigitsNumber == 6)
            event.preventDefault();
    }
    else {
        if (CurrentValue.length == 12)
            event.preventDefault();
    }

});

$(".InputDecimal18_6").bind("paste", function (e) {



    let CurrentValue = $(this).val();
    let Addedvalue = e.originalEvent.clipboardData.getData('text');

    let Decimal18_10Reg = /^(?![0.]*$)\d{0,12}(?:\.\d{0,6})?$/;

    if (!Decimal18_10Reg.test(CurrentValue.trim() + Addedvalue.trim()))
        event.preventDefault();


});


$(".InputDecimal16_9").on('keypress', function (e) {


    let CurrentValue = $(this).val();
    let value = String.fromCharCode(e.which);

    if (value == '.' && !$(this).val().includes('.'))
        return;

    if (!$.isNumeric(value))
        event.preventDefault();


    if (CurrentValue.includes('.')) {

        let DigitsNumber = CurrentValue.split('.')[1].length;

        if (DigitsNumber == 9)
            event.preventDefault();
    }
    else {
        if (CurrentValue.length == 7)
            event.preventDefault();
    }

});

$(".InputDecimal16_9").bind("paste", function (e) {



    let CurrentValue = $(this).val();
    let Addedvalue = e.originalEvent.clipboardData.getData('text');

    let Decimal16_9Reg = /^(?![0.]*$)\d{0,7}(?:\.\d{0,9})?$/;

    if (!Decimal16_9Reg.test(CurrentValue.trim() + Addedvalue.trim()))
        event.preventDefault();


});


$(".InputNum").on('keypress', function (e) {

    let value = String.fromCharCode(e.which);

    if (!$.isNumeric(value))
        event.preventDefault();
});

$(".InputNum").bind("paste", function (e) {

    let value = e.originalEvent.clipboardData.getData('text');

    if (!$.isNumeric(value) || value.includes('.'))
        event.preventDefault();
});

$(".YearInput").on('keypress', function (e) {

    let value = String.fromCharCode(e.which);
    let CurrentValue = $(this).val();
    if (!$.isNumeric(value))
        event.preventDefault();

    if (CurrentValue.length == 0) {
        if (value > 2 || value < 1)
            event.preventDefault();
    }

    if (CurrentValue.length == 1) {

        if (CurrentValue == 1 && value != 9)
            event.preventDefault();
        else if (CurrentValue == 2 && value != 0)
            event.preventDefault();
    }

});

$(".YearInput").bind("paste", function (e) {

    let pastedData = e.originalEvent.clipboardData.getData('text');

    if (!$.isNumeric(pastedData))
        event.preventDefault();

    let value = pastedData;
    let CurrentValue = $(this).val();
    if (!$.isNumeric(value))
        event.preventDefault();

    if (CurrentValue.length == 0) {
        if (value > 2100 || value < 1900)
            event.preventDefault();
    }

    if (CurrentValue.length == 1) {

        if (CurrentValue == 1 && value[0] != 9)
            event.preventDefault();
        else if (CurrentValue == 2 && value[0] != 0)
            event.preventDefault();
    }

});


 


$('.kt-datatable').on('keypress', '.InputDecimal18_6', function (e) {



    let CurrentValue = $(this).val();
    let value = String.fromCharCode(e.which);

    if (value == '.' && !$(this).val().includes('.'))
        return;

    if (!$.isNumeric(value))
        event.preventDefault();


    if (CurrentValue.includes('.')) {

        let DigitsNumber = CurrentValue.split('.')[1].length;

        if (DigitsNumber == 6)
            event.preventDefault();
    }
    else {
        if (CurrentValue.length == 12)
            event.preventDefault();
    }
});

$(".kt-datatable").on("paste", '.InputDecimal18_6', function (e) {


    let CurrentValue = $(this).val();
    let Addedvalue = e.originalEvent.clipboardData.getData('text');

    let Decimal18_10Reg = /^(?![0.]*$)\d{0,12}(?:\.\d{0,6})?$/;

    if (!Decimal18_10Reg.test(CurrentValue.trim() + Addedvalue.trim()))
        event.preventDefault();



});

$('.kt-datatable').on('keypress', '.InputNum', function (e) {

    let value = String.fromCharCode(e.which);

    if (!$.isNumeric(value))
        event.preventDefault();

});

$(".kt-datatable").on("paste", '.InputNum', function (e) {


    let value = e.originalEvent.clipboardData.getData('text');

    if (!$.isNumeric(value) || value.includes('.'))
        event.preventDefault();
});

$(".kt-datatable").on("keypress", '.YearInput', function (e) {



    let value = String.fromCharCode(e.which);
    let CurrentValue = $(this).val();
    if (!$.isNumeric(value))
        event.preventDefault();

    if (CurrentValue.length == 0) {
        if (value > 2 || value < 1)
            event.preventDefault();
    }

    if (CurrentValue.length == 1) {

        if (CurrentValue == 1 && value != 9)
            event.preventDefault();
        else if (CurrentValue == 2 && value != 0)
            event.preventDefault();
    }

});

$(".kt-datatable").on("paste", '.YearInput', function (e) {

    let pastedData = e.originalEvent.clipboardData.getData('text');

    if (!$.isNumeric(pastedData))
        event.preventDefault();

    let value = pastedData;
    let CurrentValue = $(this).val();
    if (!$.isNumeric(value) || value.includes('.'))
        event.preventDefault();

    if (CurrentValue.length == 0) {
        if (value > 2100 || value < 1900)
            event.preventDefault();
    }

    if (CurrentValue.length == 1) {

        if (CurrentValue == 1 && value[0] != 9)
            event.preventDefault();
        else if (CurrentValue == 2 && value[0] != 0)
            event.preventDefault();
    }

});






$("input").on('keypress', function (e) {
    let value = String.fromCharCode(e.which);
    let CurrentValue = $(this).val();



    if (CurrentValue == '') {
        const FirstChar = value[0];

        if (IsArabicWord(FirstChar))
            $(this).css('direction', 'rtl');
        else
            $(this).css('direction', 'ltr');
    }

});

$("input").bind("paste", function (e) {
    let pastedData = e.originalEvent.clipboardData.getData('text');
    let CurrentValue = $(this).val();

    if (CurrentValue == '') {
        const FirstChar = pastedData[0];

        if (IsArabicWord(FirstChar))
            $(this).css('direction', 'rtl');
        else
            $(this).css('direction', 'ltr');
    }


});


$("textarea").on('keypress', function (e) {
    let value = String.fromCharCode(e.which);
    let CurrentValue = $(this).val();

    if (CurrentValue == '') {
        const FirstChar = value[0];

        if (IsArabicWord(FirstChar))
            $(this).css('direction', 'rtl');
        else
            $(this).css('direction', 'ltr');
    }


});

$("textarea").bind("paste", function (e) {

    let pastedData = e.originalEvent.clipboardData.getData('text');
    let CurrentValue = $(this).val();

    if (CurrentValue == '') {
        const FirstChar = value[0];

        if (IsArabicWord(FirstChar))
            $(this).css('direction', 'rtl');
        else
            $(this).css('direction', 'ltr');
    }


});

