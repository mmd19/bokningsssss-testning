using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Projektarbete_Bokningssystem.Data;
using Projektarbete_Bokningssystem.Models;
using Projektarbete_Bokningssystem.Pages.Bookings;

namespace Booking_Test.Enhet.Models
{
    public class BookingTest
    {
        [Fact]

        //Kollar så att booking startas med rätt status
        public void bookingStatus()
        {
           var booking = new Booking();
            Assert.Equal(BookingStatus.Confirmed, booking.Status);


        }

        [Fact]

        //Kollar så att en bokning skapas och läggs till
        public void createBookings() 
        {
            var booking = new Booking { Id = 1 };
            var studyRoom = new StudyRoom();
            
            studyRoom.Bookings = new List<Booking>() { booking};

            Assert.Contains(booking, studyRoom.Bookings);


        }


    }
}
