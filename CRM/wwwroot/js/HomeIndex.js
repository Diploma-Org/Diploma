document.addEventListener('DOMContentLoaded', function() {
    // Day selection handler
    document.querySelectorAll('.day').forEach(day => {
        day.addEventListener('click', function() {
            const date = this.getAttribute('data-date');
            const urlParams = new URLSearchParams(window.location.search);
            urlParams.set('date', date);
            window.location.search = urlParams.toString();
        });
    });

    // Checkbox selection handler with strict rules
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
        form.querySelector('select[name="MasterId"]').value = selectedMasterId;
        console.log("selectedMasterId", selectedMasterId);
    }

    function createBookingForm() {
        const times = updateSelectedTime();
        if (!times) return;
        
        // Get data from hidden div
        const appData = document.getElementById('app-data');
        const masters = JSON.parse(appData.dataset.masters);
        const services = JSON.parse(appData.dataset.services);
        const selectedDate = appData.dataset.selectedDate;
    
        // Remove existing form if any
        if (bookingForm) {
            bookingForm.remove();
        }
    
        // Create form element
        bookingForm = document.createElement('div');
        bookingForm.className = 'booking-form-container';
        bookingForm.style.position = 'fixed';
        bookingForm.style.zIndex = '1000';
        
    
        // Generate masters options
        const mastersOptions = masters.map(master => 
            `<option value="${master.id}">${master.name}</option>`
        ).join('');
    
        // Generate services options
        const servicesOptions = services.map(service => 
            `<option value="${service.id}">${service.serviceName}</option>`
        ).join('');
    
        // Form content
        bookingForm.innerHTML = `
            <h4>Booking Form</h4>
            <p>Chosen time: ${times.startTime} - ${times.endTime}</p>
            
            <form id="appointmentForm" method="post" action="/Home/BookAnAppointment">
                <input type="hidden" name="date" value="${selectedDate}">
                <input type="hidden" name="StartTime" value="${times.startTime}">
                <input type="hidden" name="EndTime" value="${times.endTime}">
                
                <div class="form-group" style="margin-bottom: 15px;">
                    <label>Master:</label>
                    <select name="MasterId" class="form-control" value="${selectedMasterId}" required style="width: 100%; padding: 8px;">
                        ${mastersOptions}
                    </select>
                </div>
                
                <div class="form-group" style="margin-bottom: 15px;">
                    <label>Service:</label>
                    <select name="ServiceId" class="form-control" required style="width: 100%; padding: 8px;">
                        ${servicesOptions}
                    </select>
                </div>
                
                <div class="form-group" style="margin-bottom: 15px;">
                    <label>Client name:</label>
                    <input type="text" name="clientName" class="form-control" required style="width: 100%; padding: 8px;">
                </div>
                
                <div class="form-group" style="margin-bottom: 15px;">
                    <label>Phone number:</label>
                    <input type="tel" name="clientPhone" class="form-control" required style="width: 100%; padding: 8px;">
                </div>
                
                <div class="form-group" style="margin-bottom: 15px;">
                    <label>Additional comments:</label>
                    <textarea name="comment" class="form-control" style="width: 100%; padding: 8px;"></textarea>
                </div>
                
                <div style="display: flex; justify-content: space-between;">
                    <button type="button" id="cancelBookingBtn" class="btn btn-secondary" style="padding: 8px 15px;">Cancel</button>
                    <button type="submit" class="btn btn-primary" style="padding: 8px 15px;">Book</button>
                </div>
            </form>
        `;
        const form = bookingForm.querySelector('#appointmentForm');
        form.querySelector('select[name="MasterId"]').value = selectedMasterId;
    
        // Add form to body
        document.body.appendChild(bookingForm);
    
        // Add cancel button handler - direct event listener
        const cancelBtn = bookingForm.querySelector('#cancelBookingBtn');
        cancelBtn.addEventListener('click', function(e) {
            e.preventDefault();
            e.stopPropagation();
            
            if (bookingForm) {
                bookingForm.remove();
                bookingForm = null;
            }
            
            // Clear all selections
            checkboxes.forEach(cb => cb.checked = false);
            selectedMasterId = null;
            firstSelectedRow = null;
            lastSelectedRow = null;
        });
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
});