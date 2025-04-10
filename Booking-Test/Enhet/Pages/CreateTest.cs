using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Moq;
using Projektarbete_Bokningssystem.Data;
using Projektarbete_Bokningssystem.Pages.Bookings;

namespace Booking_Test.Enhet.Pages
{
    public class CreateTest
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

            var testUser = new IdentityUser { Email = "test@example.com" };

            var mockUserManager = GetMockUserManager<IdentityUser>();
            mockUserManager.Setup(um => um.GetUserAsync(It.IsAny<ClaimsPrincipal>()))
                           .ReturnsAsync(testUser);

            var userManager = GetMockUserManager<IdentityUser>();
            var createModel = new CreateModel(context, userManager.Object);


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
