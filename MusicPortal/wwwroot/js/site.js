
var tooltipTriggerList = [].slice.call(document.querySelectorAll('[data-bs-toggle="tooltip"]'))
var tooltipList = tooltipTriggerList.map(function (tooltipTriggerEl) {
    return new bootstrap.Tooltip(tooltipTriggerEl)
})

document.addEventListener('DOMContentLoaded', function () {
    const fileInput = document.getElementById('fileInput');
    const fileName = document.getElementById('fileName');

    if (fileInput && fileName) {
        fileInput.addEventListener('change', function () {
            if (this.files && this.files[0]) {
                fileName.textContent = this.files[0].name;
            }
        });
    }
});

function confirmDelete(message) {
    return confirm(message || 'Ви впевнені, що хочете видалити цей елемент?');
}

function setLanguage(lang) {
    fetch('/Home/SetLanguage', {
        method: 'POST',
        headers: {
            'Content-Type': 'application/x-www-form-urlencoded',
        },
        body: 'culture=' + lang + '&returnUrl=' + encodeURIComponent(window.location.href)
    }).then(() => {
        window.location.reload();
    });
}