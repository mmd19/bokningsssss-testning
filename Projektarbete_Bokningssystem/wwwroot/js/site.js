// Please see documentation at https://learn.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.


// Funktion för att initiera bokningskalendern
function initBookingCalendar(bookingEvents) {
    document.addEventListener('DOMContentLoaded', function () {
        var calendarEl = document.getElementById('calendar');
        if (!calendarEl) return; // Avsluta om kalenderelementet inte finns på sidan

        // Säkerställ att bookingEvents finns
        bookingEvents = bookingEvents || []; // Sätts till tom array om bokning ej hittas

        var calendar = new FullCalendar.Calendar(calendarEl, {
            initialView: 'dayGridMonth',
            locale: 'sv',
            headerToolbar: {
                left: 'prev,next today',
                center: 'title',
                right: 'dayGridMonth'
            },
            weekends: false, // visar inte helgdagar
            height: 'auto',
            events: bookingEvents,
            eventColor: '#dc3545', // Röd färg för bokade tider

            // När man klickar på ett datum
            dateClick: function (info) {
                // Hitta datumfältet via attribut 
                var dateField = document.querySelector('input[name="Booking.BookingDate"]');
                if (dateField) {
                    dateField.value = info.dateStr;
                } else {
                    console.error('Kunde inte hitta datumfältet');
                }
            }
        });

        calendar.render();

        // Sätt upp filtrering baserat på rumsval
        setupRoomFiltering(calendar, bookingEvents);
    });
}

// Funktion för att filtrera synliga bokningar i kalendern mellan rummen
function setupRoomFiltering(calendar, bookingEvents) {
    // Hitta selected studierum med hjälp av QuerySelector
    var roomSelect = document.querySelector('select[name="Booking.StudyRoomId"]');
    if (!roomSelect) {
        console.error('Kunde inte hitta valt rum');
        return;
    }

    roomSelect.addEventListener('change', function () {
        var selectedRoomId = this.value;

        // Ta bort alla befintliga events
        calendar.getEvents().forEach(function (event) {
            event.remove();
        });

        // Om inget rum är valt, visa alla bokningar
        if (!selectedRoomId) {
            bookingEvents.forEach(function (event) {
                calendar.addEvent(event);
            });
        } else {
            // Annars, filtrera och visa endast bokningar för valt rum
            bookingEvents.filter(function (event) {
                return event.roomId == selectedRoomId;
            }).forEach(function (event) {
                calendar.addEvent(event);
            });
        }
    });
}
