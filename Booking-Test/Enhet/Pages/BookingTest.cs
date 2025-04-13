using Microsoft.AspNetCore.Identity;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Moq;
using Projektarbete_Bokningssystem.Data;
using Projektarbete_Bokningssystem.Pages.Bookings;

namespace Booking_Test.Enhet.Pages
{
    public class BookingTest
    {

        [Fact]
        public async void createBooking()
        {
            //Databasen
            var connection = new SqliteConnection("DataSource=:memory:");
            connection.Open();

            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseSqlite(connection)
                .Options;

            using var context = new ApplicationDbContext(options);
            context.Database.EnsureCreated(); // required for SQLite

            //Mocka UserManager
            var mockUserManager = GetMockUserManager<IdentityUser>();
            var createModel = new CreateModel(context, mockUserManager.Object);
            var testUser = new IdentityUser { Email = "test@example.com" };

            mockUserManager.Setup(x => x.GetUserAsync(createModel.User))
                          .ReturnsAsync(testUser);

            //Kollar så att databasen är tom från början
            Assert.Empty(context.Bookings);

            //Skapa användare
            await createModel.OnPostAsync();



        }

        //Mock
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
