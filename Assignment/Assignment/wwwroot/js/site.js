var searchInput = '';
function AddTask() {
    $('#showmodal').load('/Home/AddTask/', () => {

        const modal = new bootstrap.Modal(document.getElementById('AddTask'));
        modal.show();
    });
}

function EditTask(id) {
    $('#showmodal').load('/Home/EditTask/?id=' + id, () => {

        const modal = new bootstrap.Modal(document.getElementById('EditTask'));
        modal.show();
    });
}

$(document).ready(function () {
    $.ajax({
        url: '/Home/TableView/',
        method: 'POST',
        data: {
            searchInput: searchInput
        },
        success: function (response) {
            $('#TableData').html(response);
        },
        error: function (error) {
            console.error('Error in AJAX', error);
        }
    })
});

$('#Search').on("keyup", function () {
    searchInput = $(this).val().toLowerCase();

    $.ajax({
        url: '/Home/TableView/',
        method: 'POST',
        data: {
            searchInput: searchInput
        },
        success: function (response) {

        },
        error: function (error) {
            console.error('Error in AJAX', error);
        }
    });
});