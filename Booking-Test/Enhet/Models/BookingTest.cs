using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Moq;
using Projektarbete_Bokningssystem.Data;
using Projektarbete_Bokningssystem.Models;
using Projektarbete_Bokningssystem.Pages.Bookings;

namespace Booking_Test.Enhet.Models
{
    public class BookingTest
    {
        [Fact]
        public async void createBooking()
        {
            var connection = new SqliteConnection("DataSource=:memory:");
            connection.Open();

            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseSqlite(connection)
                .Options;

            using var context = new ApplicationDbContext(options);
            context.Database.EnsureCreated(); // required for SQLite

          
            var userManager = GetMockUserManager<IdentityUser>();
            var createModel = new CreateModel (context, userManager.Object);
            var testUser = new IdentityUser { Email = "test@example.com" };

            
            var mockUserManager = GetMockUserManager<IdentityUser>();
            mockUserManager.Setup(x => x.GetUserAsync(createModel.User))
                           .ReturnsAsync(testUser);

            Assert.Empty(context.Bookings);

            await createModel.OnPostAsync();

            Assert.Empty(context.Bookings);


        }

        public static Mock<UserManager<TUser>> GetMockUserManager<TUser>() where TUser : class
        {
            var store = new Mock<IUserStore<TUser>>();
            var mgr = new Mock<UserManager<TUser>>(
                store.Object,
                null, null, null, null, null, null, null, null
            );
            return mgr;
        }
    }
}
