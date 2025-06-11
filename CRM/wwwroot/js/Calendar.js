document.addEventListener('DOMContentLoaded', function() {
    const urlParams = new URLSearchParams(window.location.search);
    let monthOffset = parseInt(urlParams.get('monthOffset') || "0");

    document.getElementById('prev-month')?.addEventListener('click', () => {
        urlParams.set('monthOffset', monthOffset - 3);
        window.location.search = urlParams.toString();
    });

    document.getElementById('next-month')?.addEventListener('click', () => {
        urlParams.set('monthOffset', monthOffset + 3);
        window.location.search = urlParams.toString();
    });

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
