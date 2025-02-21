// 🟠 Fungsi untuk memperbarui District Dropdown berdasarkan State
function updateDistrictDropdown(url) {
    $('#StateDropdown').change(function () {
        var stateId = $(this).val();
        $.getJSON(url, { stateId: stateId }, function (data) {
            var items = '<option>Select District</option>';
            $.each(data, function (i, product) {
                items += `<option value="${product.Value}">${product.Text}</option>`;
            });
            $('#DistrictDropdown').html(items);
        });
    });
}

/* 🟠 Fungsi untuk memperbarui Major Dropdown berdasarkan Faculty*/
function updateMajorDropdown(url) {
    $('#FacultyDropdown').change(function () {
        var facultyId = $(this).val();
        $.getJSON(url, { facultyId: facultyId }, function (data) {
            var items = '<option>Select Major</option>';
            $.each(data, function (i, product) {
                items += `<option value="${product.Value}">${product.Text}</option>`;
            });
            $('#MajorDropdown').html(items);
        });
    });
}

