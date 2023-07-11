
var AlertSwal = function(msg) {
    swal.fire(msg);
}

var AlertSwalError = function (title, msg) {
    Swal.fire({
        type: 'error',
        title: title,
        text: msg,
        //footer: '<a href>Why do I have this issue?</a>'
    })
}

var AlertSwalSucceeded = function(msg) {
        Swal.fire({
            type: 'success',
            title: msg,
            showConfirmButton: false,
            timer: 1500
        })
    }

var AlertSwalConfirm = function (title, msg, confirmButtonText, cancelButtonText) {
        return Swal.fire({
            title: title,
            text: msg,
            type: 'warning',
            showCancelButton: true,
            confirmButtonColor: '#3085d6',
            cancelButtonColor: '#d33',
            confirmButtonText: confirmButtonText,
            cancelButtonText: cancelButtonText,

        }).then((result) => {
            if (result.value) {
                result = false;
            }
            else if (result.dismiss === Swal.DismissReason.cancel) {
                result = false;
            }
        })

    }
