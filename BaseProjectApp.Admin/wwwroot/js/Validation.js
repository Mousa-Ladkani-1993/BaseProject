
 
$(".SubmitForm").submit(function (event) {

    ClearValiditors();
    return checkValidity();
});

function ClearValiditors() {
    $('.ValidatorSpan').each(function (index) {
        $(this).html('');
    });
}

function checkValidity() {

    let validForm = true;
    let continueScrolling = true;

    $('.ConstraintHiddenInput').each(function (index) {

        if ($(this).hasClass('ImgUploader') && $('#ImgUploaderChecker').val() != undefined && $('#ImgUploaderChecker').val() == '1')
            return;

        const currentInputId = $(this).attr('id');
        const currentInputlbl = $(this).attr('title');

        if (currentInputId != undefined && currentInputId != '') {

            const targetSpanClass = currentInputId + 'Rule';
            if ($('#' + currentInputId).val() == '') {
                $('.' + targetSpanClass).each(function (index) {
                    $(this).html(currentInputlbl == undefined || currentInputlbl == '' ? currentInputId : currentInputlbl + ' is required');
                    validForm = false;
                });
            }
        }
    });

    $('.ConstraintHiddenSelect').each(function (index) {

        const currentSelectId = $(this).attr('id');
        const currentSelectlbl = $(this).attr('title');
        const HasConstraintHiddenSelectIgnoreClass = $(this).hasClass('ConstraintHiddenSelectIgnore');
        let ignoreThis = false;

        if (currentSelectId != undefined && currentSelectId != '' && HasConstraintHiddenSelectIgnoreClass) {
            if ($('#' + currentSelectId + 'Div').css('display') == 'none') {
                ignoreThis = true;
            }

        }



        if (currentSelectId != undefined && currentSelectId != '' && !ignoreThis) {

            const targetSpanClass = currentSelectId + 'Rule';
            if ($('#' + currentSelectId).val() == '0') {
                $('.' + targetSpanClass).each(function (index) {
                    $(this).html('Please select ' + (currentSelectlbl == undefined || currentSelectlbl == '' ? currentInputId : currentSelectlbl));
                    validForm = false;
                });
            }
        }
    });

    $('.ConstraintInput').each(function (index) {

        if ($(this).hasClass('ImgUploader') && $('#ImgUploaderChecker').val() != undefined && $('#ImgUploaderChecker').val() == '1')
            return;

        const currentInputId = $(this).attr('id');
        const currentInputlbl = $(this).attr('title');

        if (currentInputId != undefined && currentInputId != '') {


            if ($('#' + currentInputId).val() == '') {
                $('#' + currentInputId).addClass('border-danger');
                validForm = false;

                if (continueScrolling == true && !$(this).hasClass('NoScrolling')) {
                    continueScrolling = false;
                    $([document.documentElement, document.body]).animate({
                        scrollTop: $(this).offset().top - 300
                    }, 2000);
                }

            }
            else {

                $('#' + currentInputId).removeClass('border-danger')
            }
        }
    });

    $('.ConstraintSelect').each(function (index) {

        const currentSelectId = $(this).attr('id');
        const currentSelectlbl = $(this).attr('title');
        const HasConstraintHiddenSelectIgnoreClass = $(this).hasClass('ConstraintHiddenSelectIgnore');
        let ignoreThis = false;

        if (currentSelectId != undefined && currentSelectId != '' && HasConstraintHiddenSelectIgnoreClass) {
            if ($('#' + currentSelectId + 'Div').css('display') == 'none') {
                ignoreThis = true;
            }

        }

        if (currentSelectId != undefined && currentSelectId != '' && !ignoreThis) {

            if ($('#' + currentSelectId).val() == '0' || $('#' + currentSelectId).val() == '-1') {

                $('#' + currentSelectId).addClass('border-danger');

                validForm = false;

                

                if (continueScrolling == true && !$(this).hasClass('NoScrolling')) {

                    continueScrolling = false;
                    $([document.documentElement, document.body]).animate({
                        scrollTop: $(this).offset().top - 300
                    }, 2000);
                }

            }
            else {

                $('#' + currentSelectId).removeClass('border-danger')
            }
        }
    });

     
    $('.ConstraintSelect2').each(function (index) {
         
        const currentSelectId = $(this).attr('id');
        const currentSelectlbl = $(this).attr('title');
        const HasConstraintHiddenSelectIgnoreClass = $(this).hasClass('ConstraintHiddenSelectIgnore');
        let ignoreThis = false;

        if (currentSelectId != undefined && currentSelectId != '' && HasConstraintHiddenSelectIgnoreClass) {
            if ($('#' + currentSelectId + 'Div').css('display') == 'none') {
                ignoreThis = true;
            }

        }
         

        if (currentSelectId != undefined && currentSelectId != '' && !ignoreThis) {

            

            if ($('#' + currentSelectId).val() == '0' || $('#' + currentSelectId).val() == '-1' || $('#' + currentSelectId).val() == null) {

                $('#select2-' + currentSelectId + '-container').parent().addClass('border-danger'); 

                validForm = false;

                

                if (continueScrolling == true && !$(this).hasClass('NoScrolling')) {

                    continueScrolling = false;
                    $([document.documentElement, document.body]).animate({
                        scrollTop: $(this).offset().top - 300
                    }, 2000);
                }

            }
            else {
                $('#select2-' + currentSelectId + '-container').parent().removeClass('border-danger'); 
            }
        }
    });



    return validForm;
}
