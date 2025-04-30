$(document).ready(function () {

    // Function for individual delete button
    $('.delete-btn').click(function (e) {
        e.preventDefault(); // Prevent form submission

        const form = $(this).closest('form');
        const authorName = form.data('name'); // Get the author's name

        // Show SweetAlert for confirmation
        Swal.fire({
            title: 'Are you sure?',
            text: `You are about to delete ${authorName}`,
            icon: 'warning',
            showCancelButton: true,
            confirmButtonColor: '#3085d6',
            cancelButtonColor: '#d33',
            confirmButtonText: 'Yes, delete it!',
            cancelButtonText: 'Cancel'
        }).then((result) => {
            if (result.isConfirmed) {
                form.submit(); // Submit the form if confirmed
            } else {
                toastr.info(`${authorName} deletion cancelled.`);
            }
        });
    });

    // Function for delete all button
    $('#delete-all-btn').click(function (e) {
        e.preventDefault(); // Prevent form submission

        // Show SweetAlert for confirmation
        Swal.fire({
            title: 'Are you sure?',
            text: 'You are about to delete all authors',
            icon: 'warning',
            showCancelButton: true,
            confirmButtonColor: '#3085d6',
            cancelButtonColor: '#d33',
            confirmButtonText: 'Yes, delete all!',
            cancelButtonText: 'Cancel'
        }).then((result) => {
            if (result.isConfirmed) {
                $('#delete-all-form').submit(); // Submit the form if confirmed
            } else {
                toastr.info("Deletion cancelled.");
            }
        });
    });
});

var tooltipTriggerList = [].slice.call(document.querySelectorAll('[data-bs-toggle="tooltip"]'));
var tooltipList = tooltipTriggerList.map(function (tooltipTriggerEl) {
    return new bootstrap.Tooltip(tooltipTriggerEl);
});
