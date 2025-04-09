using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.Blazor;
using Projektarbete_Bokningssystem.Models;

namespace Booking_Test.Enhet.Models
{
    public class StudyRoomTest
    {
        [Fact]

        //Kollar så att det finns ett bokat rum
        public void studyRoomStatus()
        {
            var studyRoom = new StudyRoom();
            studyRoom.Bookings = new List<Booking>();
            studyRoom.Bookings.Add(new Booking());
            Assert.Equal(studyRoom.Bookings.Count, 1);
            
        }

        [Fact]

        //Tar bort en specifik bokning
        public void cancelStudyRoom_RemoveSpecificBooking()
        {
            var studyRoom = new StudyRoom();
            var booking1 = new Booking { Id = 1 };
            var booking2 = new Booking { Id = 2 };
            studyRoom.Bookings = new List<Booking>() { booking1, booking2};


            studyRoom.Bookings.Remove(booking1);
            Assert.Contains(booking2, studyRoom.Bookings);
        }
    }
}
