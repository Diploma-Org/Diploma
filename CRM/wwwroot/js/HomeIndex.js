document.addEventListener('DOMContentLoaded', function() {
    const checkboxes = document.querySelectorAll('.time-slot-input');
    let selectedMasterId = null;
    let firstSelectedRow = null;
    let lastSelectedRow = null;
    let bookingForm = null;

    function updateSelectedTime() {
        if (firstSelectedRow === null || lastSelectedRow === null) return;
        
        const firstCheckbox = document.querySelector(
            `.time-slot-input[data-row-index="${firstSelectedRow}"][data-master-id="${selectedMasterId}"]`
        );
        selectedMasterId = firstCheckbox.dataset.masterId;
        
        const lastCheckbox = document.querySelector(
            `.time-slot-input[data-row-index="${lastSelectedRow}"][data-master-id="${selectedMasterId}"]`
        );
        
        if (!firstCheckbox || !lastCheckbox) return;
        
        const startHour = parseInt(firstCheckbox.dataset.time);
        const startMinute = parseInt(firstCheckbox.dataset.minute);
        const startTime = new Date(0, 0, 0, startHour, startMinute, 0);
        
        const endHour = parseInt(lastCheckbox.dataset.time);
        const endMinute = parseInt(lastCheckbox.dataset.minute) + 15;
        const endTime = new Date(0, 0, 0, endHour, endMinute, 0);
        
        const startTimeStr = `${startTime.getHours().toString().padStart(2, '0')}:${startTime.getMinutes().toString().padStart(2, '0')}`;
        const endTimeStr = `${endTime.getHours().toString().padStart(2, '0')}:${endTime.getMinutes().toString().padStart(2, '0')}`;
        
        return { startTime: startTimeStr, endTime: endTimeStr };
    }

    function updateBookingForm() {
        if (!bookingForm) return;
        
        const times = updateSelectedTime();
        if (!times) return;
        
        const timeDisplay = bookingForm.querySelector('p');
        if (timeDisplay) {
            timeDisplay.textContent = `Chosen time: ${times.startTime} - ${times.endTime}`;
        }
        
        const form = bookingForm.querySelector('#appointmentForm');
        form.querySelector('input[name="StartTime"]').value = times.startTime;
        form.querySelector('input[name="EndTime"]').value = times.endTime;
        positionBookingForm();
    }

    function createBookingForm(appointmentData = null) {
        const appData = document.getElementById('app-data');
        const masters = JSON.parse(appData.dataset.masters);
        const services = JSON.parse(appData.dataset.services);
    
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
        const selectedServiceId = appointmentData?.idProvidedService || "";
        selectedMasterId = appointmentData?.idMaster || selectedMasterId;
    
        const masterServices = JSON.parse(appData.dataset.masterservices);
        const mastersServices = masterServices.filter(ms => ms.idMaster == selectedMasterId);
        const servicesOptions = mastersServices.map(ms => {
            const service = services.find(s => s.id == ms.idProvidedService);
                return service
                    ? `<option value="${ms.idProvidedService}" ${ms.idProvidedService == selectedServiceId ? 'selected' : ''}>${service.serviceName} ${service.price} грн</option>`
                    : '';
            }).join('');
        
        const actionUrl = appointmentData ? `/Home/EditAppointment` : `/Home/BookAnAppointment`;
    
        bookingForm.innerHTML = `
            <h4>${appointmentData ? "Редагування" : "Бронювання"}</h4>
            <p>Обраний час: ${times.startTime} - ${times.endTime}</p>

            <form id="appointmentForm" method="post" action="${actionUrl}">
                <input type="hidden" name="date" value="${selectedDate}">
                <input type="hidden" name="StartTime" value="${times.startTime}"> 
                <input type="hidden" name="EndTime" value="${times.endTime}">
                ${appointmentData ? `<input type="hidden" name="Id" value="${appointmentData.id}">` : ''}

                <div class="form-group">
                    <label>Майстер: ${masters.find(master => master.id == selectedMasterId)?.name}</label>
                    <input type="hidden" name="MasterId" value="${selectedMasterId}">
                </div>

                <div class="form-group">
                    <label>Послуга:</label>
                    <select name="ServiceId" required>${servicesOptions}</select>
                </div>

                <div class="form-group position-relative">
                    <label>Знайти клієнта (ім'я, прізвище, or телефон):</label>
                    <input type="text" id="clientSearch" class="form-control" placeholder="шукати клієнтів...">
                    <div id="clientSuggestions" class="list-group position-absolute w-100 z-3"></div>
                </div>

                <div class="form-group">
                    <label>Ім'я клієнта:</label>
                    <input type="text" id="clientName" name="clientName" class="form-control" required value="${clientName}">
                </div>

                <div class="form-group">
                    <label>Номер телефону:</label>
                    <input type="tel" id="clientPhone" name="clientPhone" class="form-control" required value="${clientPhone}">
                </div>

                <div class="form-check mb-2">
                    <input class="form-check-input" type="checkbox" name="IsPaid" value="true" ${appointmentData?.isPaid ? 'checked' : ''}>
                    <input type="hidden" name="IsPaidCopy" value="${appointmentData?.isPaid ? 'true' : 'false'}">
                    <label class="form-check-label">Оплачено</label>
                </div>

                <div class="d-flex justify-content-between">
                    <button type="button" id="cancelBookingBtn" class="btn btn-secondary">Відмінити</button>
                    <button type="submit" class="btn btn-primary">${appointmentData ? "Зберегти зміни" : "Забронювати"}</button>
                </div>
            </form>
        `;

        // Після вставки HTML — викликаємо автозаповнення
        

    
        document.querySelector('.appointments-table-container').appendChild(bookingForm);
        positionBookingForm();
        setupClientAutocomplete();
    
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
        const formHeight = 400;
    
        if (firstSelectedRow === null || selectedMasterId === null) {
            bookingForm.style.position = 'absolute';
            bookingForm.style.top = `${(containerRect.height - formHeight) / 2}px`;
            bookingForm.style.left = `${(containerRect.width - formWidth) / 2}px`;
            bookingForm.style.width = `${formWidth}px`;
            return;
        }
    
        const firstCheckbox = document.querySelector(
            `.time-slot-input[data-row-index="${firstSelectedRow}"][data-master-id="${selectedMasterId}"]`
        );
        if (!firstCheckbox) return;
    
        const cell = firstCheckbox.closest('td');
        const cellRect = cell.getBoundingClientRect();
    
        let leftPosition = cellRect.right - containerRect.left + 10;
        let topPosition = cellRect.top - containerRect.top;
    

        if (leftPosition + formWidth > containerRect.width) {
            leftPosition = cellRect.left - containerRect.left - formWidth - 10;
        }
    
        if (leftPosition < 0) {
            leftPosition = 10;
        }
    
        if (topPosition + formHeight > containerRect.height) {
            topPosition = containerRect.height - formHeight - 10;
        }
    
        if (topPosition < 0) {
            topPosition = 10;
        }
    
        bookingForm.style.position = 'absolute';
        bookingForm.style.left = `${leftPosition}px`;
        bookingForm.style.top = `${topPosition}px`;
        bookingForm.style.width = `${formWidth}px`;
    }
    
    

    function handleCheckboxSelection(currentMasterId, currentRow) {
        if (selectedMasterId === null || currentMasterId !== selectedMasterId) {
            checkboxes.forEach(cb => cb.checked = false);
            
            selectedMasterId = currentMasterId;
            firstSelectedRow = currentRow;
            lastSelectedRow = currentRow;
            return true;
        }

        if (document.querySelector(`.time-slot-input[data-row-index="${currentRow}"][data-master-id="${currentMasterId}"]`).checked && 
            firstSelectedRow === currentRow && lastSelectedRow === currentRow) {
            return false;
        }

        const isAdjacent = (currentRow === firstSelectedRow - 1) || 
                            (currentRow === lastSelectedRow + 1);

        if (!isAdjacent) {
            checkboxes.forEach(cb => cb.checked = false);
            selectedMasterId = currentMasterId;
            firstSelectedRow = currentRow;
            lastSelectedRow = currentRow;
            return true;
        }

        if (currentRow < firstSelectedRow) {
            firstSelectedRow = currentRow;
        } else if (currentRow > lastSelectedRow) {
            lastSelectedRow = currentRow;
        }

        return true;
    }

    function updateCheckboxesSelection() {
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

        checkbox.addEventListener('mouseover', function(e) {
            e.preventDefault();
        });
    });

    document.addEventListener('selectstart', function(e) {
        e.preventDefault();
    });
    document.addEventListener('click', function (e) {
        if (e.target.classList.contains('edit-appointment')) {
            e.preventDefault();
            const appointmentData = JSON.parse(e.target.dataset.appointment);
            console.log("Parsed appointment data:", appointmentData);
            createBookingForm(appointmentData);
        }
    });


    function setupClientAutocomplete() {
        const searchInput = document.getElementById("clientSearch");
        const nameInput = document.getElementById("clientName");
        const phoneInput = document.getElementById("clientPhone");
        const suggestions = document.getElementById("clientSuggestions");

        // Перевіряємо, чи елементи існують
        if (!searchInput || !nameInput || !phoneInput) {
            console.error("Autocomplete elements not found");
            return;
        }

        searchInput.addEventListener("input", async () => {
            const query = searchInput.value.trim();
            if (query.length < 2) {
                suggestions.innerHTML = "";
                return;
            }

            const response = await fetch(`/Home/SearchClients?query=${encodeURIComponent(query)}`);
            const clients = await response.json();

            suggestions.innerHTML = "";
            clients.forEach(client => {
                const item = document.createElement("button");
                item.type = "button";
                item.className = "list-group-item list-group-item-action";
                item.textContent = `${client.name} ${client.surname} (${client.phone})`;

                item.addEventListener("click", () => {
                    nameInput.value = `${client.name} ${client.surname}`;
                    phoneInput.value = client.phone;
                    searchInput.value = "";
                    suggestions.innerHTML = "";
                });

                suggestions.appendChild(item);
            });
        });
    }

    
});