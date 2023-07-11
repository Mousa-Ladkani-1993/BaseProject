
$("#ImageUploadDZ").dropzone({
    maxFilesize: MaxUploadedFileSizeMB,
    autoProcessQueue: false,
    acceptedFiles: 'image/*',
    addRemoveLinks: true,
    maxFiles: 1,
    accept: function (file, done) {
        done();
    },
    init: function () {
        this.on("addedfile", function () {

            if (this.files[0].size > MaxUploadedFileSizeB)
                return false;

            _UploadFile(this.files[0], 'ImageUploadDZ', 'SubmitDiv'
                , 'ImageUploadDZ_fileProgressDiv', 'ImageUploadDZ_fileProgress',
                'ImageUploadDZ_fileProgresstxt', ImageUploadDZCallback);

        });

        this.on("removedfile", function () {

            if (ImageUploadDZId > 0) {
                _RemoveFile(ImageUploadDZId);
                ImageUploadDZId = 0;
                $('#ImageUrl').val('');
            }

        });

        this.on("maxfilesexceeded", function (file) {
            $('#ImageUrl').val('');
        });
    }
});


function ImageUploadDZCallback(url, id) {
    ImageUploadDZId = id;
    $('#ImageUrl').val(url);
}
 

jQuery(document).ready(function () {




    if (MobileCustomMenuId == null || MobileCustomMenuId == 0) {
        $('#ImageUploadDZ').show();

        $('#ImageEdit').hide();
        $('#cancelImageId').hide();
        $('#cancelImageDiv').hide();  
    }

    if (MobileCustomMenuId != null && MobileCustomMenuId.trim() != '') {
        _getMediaId();
    }

});



function _getMediaId() {



    const Localhost = DevelopmentLocalhost == null ? '' : DevelopmentLocalhost;

    let ApplicationUrl = window.location.href.toLowerCase().split('/menus')[0];

    let ApplicationUrlCut = window.location.href.toLowerCase().split('/menus')[0];

    if (ApplicationUrlCut.replace(Localhost, '').length < 2)
        ApplicationUrl = '';

    if ($('#ImageSrcI').val() != undefined && $('#ImageSrcI').val().length > 0) {

        document.getElementById("ImageUploadDZ").style.display = "none";
        document.getElementById("ImageEdit").style.display = "";

        const ImgSRC = $('#ImageSrcI').val();
        $("#ImageSrc").attr("src", ApplicationUrl + ImgSRC);
    }
      
     
}




function cancelAddImage(e) {
    e.preventDefault();

    document.getElementById("ImageUploadDZ").style.display = "none";
    document.getElementById("ImageEdit").style.display = "";

    document.getElementById("changeImageId").style.display = "";
    document.getElementById("cancelImageId").style.display = "none";
    document.getElementById("cancelImageDiv").style.display = "none";
    //Dropzone.forElement('#ImageUploadDZ').removeAllFiles(true);
    UploadedImage = null;

    return false;
}


function changeImage(e) {
    e.preventDefault();

    document.getElementById("ImageUploadDZ").style.display = "";
    document.getElementById("ImageEdit").style.display = "none";

    document.getElementById("changeImageId").style.display = "none";
    document.getElementById("cancelImageId").style.display = "";
    document.getElementById("cancelImageDiv").style.display = "";

    //Dropzone.forElement('#ImageUploadDZ').removeAllFiles(true);
    UploadedImage = null;

    return false;
}
