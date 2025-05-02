document.addEventListener('DOMContentLoaded', function() {
    document.querySelectorAll('.day').forEach(day => {
        day.addEventListener('click', function() {
            const date = this.getAttribute('data-date');
            const urlParams = new URLSearchParams(window.location.search);
            urlParams.set('date', date);
            window.location.search = urlParams.toString();
            console.log('Date clicked:', date);
        });
    });
    });
