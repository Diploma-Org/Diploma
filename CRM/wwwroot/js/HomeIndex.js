document.addEventListener('DOMContentLoaded', function() {
    const checkboxes = document.querySelectorAll('.time-slot-input');
    let selectedMasterId = null;
    let firstSelectedRow = null;
    let lastSelectedRow = null;
    let bookingForm = null;

    function updateSelectedTime() {
        if (firstSelectedRow === null || lastSelectedRow === null) return;
        
        // Get first selected checkbox
        const firstCheckbox = document.querySelector(
            `.time-slot-input[data-row-index="${firstSelectedRow}"][data-master-id="${selectedMasterId}"]`
        );
        selectedMasterId = firstCheckbox.dataset.masterId;
        
        // Get last selected checkbox
        const lastCheckbox = document.querySelector(
            `.time-slot-input[data-row-index="${lastSelectedRow}"][data-master-id="${selectedMasterId}"]`
        );
        
        if (!firstCheckbox || !lastCheckbox) return;
        
        // Calculate start time (from first selected slot)
        const startHour = parseInt(firstCheckbox.dataset.time);
        const startMinute = parseInt(firstCheckbox.dataset.minute);
        const startTime = new Date(0, 0, 0, startHour, startMinute, 0);
        
        // Calculate end time (last selected slot + 15 minutes)
        const endHour = parseInt(lastCheckbox.dataset.time);
        const endMinute = parseInt(lastCheckbox.dataset.minute) + 15;
        const endTime = new Date(0, 0, 0, endHour, endMinute, 0);
        
        // Format time strings
        const startTimeStr = `${startTime.getHours().toString().padStart(2, '0')}:${startTime.getMinutes().toString().padStart(2, '0')}`;
        const endTimeStr = `${endTime.getHours().toString().padStart(2, '0')}:${endTime.getMinutes().toString().padStart(2, '0')}`;
        
        return { startTime: startTimeStr, endTime: endTimeStr };
    }

    function updateBookingForm() {
        if (!bookingForm) return;
        
        const times = updateSelectedTime();
        if (!times) return;
        
        // Update time display in form
        const timeDisplay = bookingForm.querySelector('p');
        if (timeDisplay) {
            timeDisplay.textContent = `Chosen time: ${times.startTime} - ${times.endTime}`;
        }
        
        // Update hidden inputs that will be submitted
        const form = bookingForm.querySelector('#appointmentForm');
        form.querySelector('input[name="StartTime"]').value = times.startTime;
        form.querySelector('input[name="EndTime"]').value = times.endTime;
        positionBookingForm();
    }

    function createBookingForm(appointmentData = null) {
        const appData = document.getElementById('app-data');
        const masters = JSON.parse(appData.dataset.masters);
        const services = JSON.parse(appData.dataset.services);
    
        // Determine selected time
        let times;
        if (appointmentData) {
            const start = new Date(appointmentData.startTime);
            const end = new Date(appointmentData.endTime);
            times = {
                startTime: `${start.getHours().toString().padStart(2, '0')}:${start.getMinutes().toString().padStart(2, '0')}`,
                endTime: `${end.getHours().toString().padStart(2, '0')}:${end.getMinutes().toString().padStart(2, '0')}`
            };
        } else {
            times = updateSelectedTime();
        }
    
        console.log("Times for booking form:", times);
    
        const selectedDate = appointmentData?.SelectedDate || appData.dataset.selectedDate;
    
        // If form exists and no time is available, remove it
        if (bookingForm && times === null) {
            bookingForm.remove();
            if (scrollHandler) {
                window.removeEventListener('scroll', scrollHandler);
            }
        }
    
        bookingForm = document.createElement('div');
        bookingForm.className = 'booking-form-container';
    
        const clientName = appointmentData?.visitorName || "";
        const clientPhone = appointmentData?.visitorPhone || "";
        const comment = appointmentData?.Comment || "";
        const selectedServiceId = appointmentData?.idProvidedService || "";
        selectedMasterId = appointmentData?.idMaster || selectedMasterId;
    
        const servicesOptions = services.map(service =>
            `<option value="${service.id}" ${service.id == selectedServiceId ? 'selected' : ''}>${service.serviceName}</option>`
        ).join('');
    
        const actionUrl = appointmentData ? `/Home/EditAppointment` : `/Home/BookAnAppointment`;
    
        bookingForm.innerHTML = `
            <h4>${appointmentData ? "Edit" : "Booking"} Form</h4>
            <p>Chosen time: ${times.startTime} - ${times.endTime}</p>
    
            <form id="appointmentForm" method="post" action="${actionUrl}">
                <input type="hidden" name="date" value="${selectedDate}">
                <input type="hidden" name="StartTime" value="${times.startTime}">
                <input type="hidden" name="EndTime" value="${times.endTime}">
                ${appointmentData ? `<input type="hidden" name="Id" value="${appointmentData.id}">` : ''}
    
                <div class="form-group">
                    <label>Master: ${masters.find(master => master.id == selectedMasterId)?.name}</label>
                    <input type="hidden" name="MasterId" value="${selectedMasterId}">
                </div>
    
                <div class="form-group">
                    <label>Service:</label>
                    <select name="ServiceId" required>${servicesOptions}</select>
                </div>
    
                <div class="form-group">
                    <label>Client name:</label>
                    <input type="text" name="clientName" required value="${clientName}">
                </div>
    
                <div class="form-group">
                    <label>Phone number:</label>
                    <input type="tel" name="clientPhone" required value="${clientPhone}">
                </div>
    
                <div class="form-group">
                    <label>Additional comments:</label>
                    <textarea name="comment">${comment}</textarea>
                </div>
                <div>
                    <div class="form-check">
                        <input class="form-check-input" type="checkbox" name="IsPaid" value="${appointmentData?.IsPaid}"}>
                        <label class="form-check-label">Paid</label>
                    </div>
                </div>
    
                <div style="display:flex; justify-content: space-between;">
                    <button type="button" id="cancelBookingBtn" class="btn btn-secondary">Cancel</button>
                    <button type="submit" class="btn btn-primary">${appointmentData ? "Save Changes" : "Book"}</button>
                </div>
            </form>
        `;
    
        document.querySelector('.appointments-table-container').appendChild(bookingForm);
        positionBookingForm();
    
        scrollHandler = () => positionBookingForm();
        window.addEventListener('scroll', scrollHandler, { passive: true });
    
        bookingForm.querySelector('#cancelBookingBtn').addEventListener('click', function (e) {
            e.preventDefault();
            if (bookingForm){
                bookingForm.remove(); 
                checkboxes.forEach(cb => cb.checked = false);
                selectedMasterId = null;
                firstSelectedRow = null;
                lastSelectedRow = null;
            }
            bookingForm = null;
            if (scrollHandler) {
                window.removeEventListener('scroll', scrollHandler);
                scrollHandler = null;
            }
        });
    }
    
    
    
    function positionBookingForm() {
        if (!bookingForm) return;
    
        const tableContainer = document.querySelector('.appointments-table-container');
        const containerRect = tableContainer.getBoundingClientRect();
        const formWidth = Math.min(400, window.innerWidth * 0.9);
        const formHeight = 400; // Approximate height
    
        // Case: edit mode (no selected rows)
        if (firstSelectedRow === null || selectedMasterId === null) {
            bookingForm.style.position = 'absolute';
            bookingForm.style.top = `${(containerRect.height - formHeight) / 2}px`;
            bookingForm.style.left = `${(containerRect.width - formWidth) / 2}px`;
            bookingForm.style.width = `${formWidth}px`;
            return;
        }
    
        // Case: normal selection mode
        const firstCheckbox = document.querySelector(
            `.time-slot-input[data-row-index="${firstSelectedRow}"][data-master-id="${selectedMasterId}"]`
        );
        if (!firstCheckbox) return;
    
        const cell = firstCheckbox.closest('td');
        const cellRect = cell.getBoundingClientRect();
    
        // Calculate position relative to container
        let leftPosition = cellRect.right - containerRect.left + 10;
        let topPosition = cellRect.top - containerRect.top;
    
        // Boundary checks
        // Right boundary
        if (leftPosition + formWidth > containerRect.width) {
            leftPosition = cellRect.left - containerRect.left - formWidth - 10;
        }
    
        // Left boundary
        if (leftPosition < 0) {
            leftPosition = 10;
        }
    
        // Bottom boundary
        if (topPosition + formHeight > containerRect.height) {
            topPosition = containerRect.height - formHeight - 10;
        }
    
        // Top boundary
        if (topPosition < 0) {
            topPosition = 10;
        }
    
        // Apply positioning
        bookingForm.style.position = 'absolute';
        bookingForm.style.left = `${leftPosition}px`;
        bookingForm.style.top = `${topPosition}px`;
        bookingForm.style.width = `${formWidth}px`;
    }
    
    

    function handleCheckboxSelection(currentMasterId, currentRow) {
        // If no selection yet or clicked on another master
        if (selectedMasterId === null || currentMasterId !== selectedMasterId) {
            // Clear all selections
            checkboxes.forEach(cb => cb.checked = false);
            
            // Start new selection
            selectedMasterId = currentMasterId;
            firstSelectedRow = currentRow;
            lastSelectedRow = currentRow;
            return true;
        }

        // If clicking on already selected checkbox - clear selection
        if (document.querySelector(`.time-slot-input[data-row-index="${currentRow}"][data-master-id="${currentMasterId}"]`).checked && 
            firstSelectedRow === currentRow && lastSelectedRow === currentRow) {
            return false;
        }

        // Check if new selection is adjacent to existing selection
        const isAdjacent = (currentRow === firstSelectedRow - 1) || 
                            (currentRow === lastSelectedRow + 1);

        if (!isAdjacent) {
            // If not adjacent, start new selection
            checkboxes.forEach(cb => cb.checked = false);
            selectedMasterId = currentMasterId;
            firstSelectedRow = currentRow;
            lastSelectedRow = currentRow;
            return true;
        }

        // Update selection range
        if (currentRow < firstSelectedRow) {
            firstSelectedRow = currentRow;
        } else if (currentRow > lastSelectedRow) {
            lastSelectedRow = currentRow;
        }

        return true;
    }

    function updateCheckboxesSelection() {
        // Select all checkboxes in the new range
        document.querySelectorAll(`.time-slot-input[data-master-id="${selectedMasterId}"]`).forEach(cb => {
            cb.checked = false;
        });

        for (let row = firstSelectedRow; row <= lastSelectedRow; row++) {
            const cb = document.querySelector(`.time-slot-input[data-row-index="${row}"][data-master-id="${selectedMasterId}"]`);
            if (cb) cb.checked = true;
        }
    }

    checkboxes.forEach(checkbox => {
        checkbox.addEventListener('click', function(e) {
            const currentMasterId = this.dataset.masterId;
            const currentRow = parseInt(this.dataset.rowIndex);

            const shouldContinue = handleCheckboxSelection(currentMasterId, currentRow);
            if (!shouldContinue) {
                // Clear selection
                checkboxes.forEach(cb => cb.checked = false);
                selectedMasterId = null;
                firstSelectedRow = null;
                lastSelectedRow = null;
                if (bookingForm) {
                    bookingForm.remove();
                    bookingForm = null;
                }
                return;
            }

            updateCheckboxesSelection();

            if (!bookingForm) {
                createBookingForm();
            } else {
                updateBookingForm();
            }
        });

        // Disable mouseover selection to enforce strict rules
        checkbox.addEventListener('mouseover', function(e) {
            e.preventDefault();
        });
    });

    // Prevent text selection when clicking
    document.addEventListener('selectstart', function(e) {
        e.preventDefault();
    });
    document.addEventListener('click', function (e) {
        if (e.target.classList.contains('edit-appointment')) {
            e.preventDefault();
            const appointmentData = JSON.parse(e.target.dataset.appointment);
            console.log("Parsed appointment data:", appointmentData); // 👈 Check this
            createBookingForm(appointmentData);
        }
    });
    
});