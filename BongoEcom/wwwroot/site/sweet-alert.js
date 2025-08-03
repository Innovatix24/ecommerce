window.toaster = {
    // Success Toast
    showSuccess: function (message) {
        Swal.fire({
            toast: true,
            position: 'top-end',
            showConfirmButton: false,
            timer: 3000,
            title: message,
            icon: 'success'
        });
    },

    // Error Toast
    showError: function (message) {
        Swal.fire({
            toast: true,
            position: 'top-end',
            showConfirmButton: false,
            timer: 3000,
            title: message,
            icon: 'error'
        });
    },

    // Warning Toast
    showWarning: function (message) {
        Swal.fire({
            toast: true,
            position: 'top-end',
            showConfirmButton: false,
            timer: 3000,
            title: message,
            icon: 'warning'
        });
    },

    // Info Toast
    showInfo: function (message) {
        Swal.fire({
            toast: true,
            position: 'top-end',
            showConfirmButton: false,
            timer: 3000,
            title: message,
            icon: 'info'
        });
    },

    // Question Toast
    showQuestion: function (message) {
        Swal.fire({
            toast: true,
            position: 'top-end',
            showConfirmButton: false,
            timer: 3000,
            title: message,
            icon: 'question'
        });
    },

    // Custom Toast with different position and color
    showCustom: function (message, icon = 'success', position = 'center') {
        Swal.fire({
            toast: true,
            position: position,
            showConfirmButton: false,
            timer: 3000,
            title: message,
            icon: icon,
            background: '#4a4a4a',
            color: '#ffffff'
        });
    }
};

window.sweet = {
    confirm: function (title, text, confirmText = "Confirm") {
        return Swal.fire({
            title: title,
            text: text,
            icon: 'info',
            showCancelButton: true,
            confirmButtonColor: '#3085d6',
            cancelButtonColor: '#d33',
            confirmButtonText: confirmText,
            allowOutsideClick: false
        }).then((result) => {
            return result.isConfirmed;
        });
    }
}

window.sweet.confirmDanger = (action) => {
    return Swal.fire({
        title: `Are you sure?`,
        html: `You're about to <strong>${action}</strong>. This cannot be undone!`,
        icon: 'error',
        showCancelButton: true,
        confirmButtonColor: '#dc3545',
        cancelButtonColor: '#6c757d',
        confirmButtonText: 'Yes, proceed',
        cancelButtonText: 'Cancel',
        focusCancel: true,
        reverseButtons: true
    }).then((result) => result.isConfirmed);
};


// Example of using these functions:
// window.toaster.showSuccess('Operation completed successfully!');
// window.toaster.showError('Something went wrong!');
// window.toaster.showWarning('This action cannot be undone');
// window.toaster.showInfo('New update available');
// window.toaster.showQuestion('Are you sure?');
// window.toaster.showCustom('Custom message', 'info', 'bottom-start');