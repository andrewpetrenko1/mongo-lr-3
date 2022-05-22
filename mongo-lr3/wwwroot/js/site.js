let formName = $('#form input[name="name"]');
let formAge = $('#form input[name="age"]');
let formId = $('#form input[name="id"]');

$('.operation-edit').on('click', (e) => {
    let id = $(e.target).attr('id');
    $.ajax({
        url: `/Member/Get/${id}`,
        method: 'GET'
    }).done(response => {
        $(`#form`).attr('action', `/Member/Edit/`);
        formName.val(response.name);
        formAge.val(response.age);
        formId.val(id);
    });
});

$('#form :input').on('change', (e) => {
    if (formName.val() === "" || formAge.val() === "")
        $('#form input[type="submit"]').attr("disabled", "true")
    else
        $('#form input[type="submit"]').removeAttr("disabled")
});

$('#nameFilter, #ageFilter').on('change', (e) => {
    window.location.search = `name=${$('#nameFilter').val()}&age=${$('#ageFilter').val()}`;
});