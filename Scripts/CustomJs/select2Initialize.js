function initializeSelect2(selector, placeholder, parent) {
    $(selector).select2({
        placeholder: placeholder,
        allowClear: true,
        dropdownParent: $(parent)
    });
}