document.querySelectorAll(".calendar-day").forEach(day => {
    day.addEventListener("click", function() {
        var selectedDate = this.getAttribute("data-date");
        window.location.href = "?date=" + selectedDate;
    });
});