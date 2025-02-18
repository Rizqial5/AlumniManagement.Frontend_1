
//function untuk mengatur validasi
function reinitializeValidation() {
    var form = $('form');
    if (form.data('validator')) {
        form.removeData('validator');
        form.removeData('unobtrusiveValidation');
    }
    $.validator.unobtrusive.parse(form);
}

function initializeForm() {
    reinitializeValidation();
}

