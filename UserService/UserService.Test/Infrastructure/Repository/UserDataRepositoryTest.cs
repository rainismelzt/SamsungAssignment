using Microsoft.EntityFrameworkCore;
using UserService.Core.Dto;
using UserService.Database;
using UserService.Database.BusinessEntity;
using UserService.Infrastructure.Repository;

namespace UserService.Test.Infrastructure.Repository
{
    public class UserDataRepositoryTest
    {
        private UserDbContext GetDbContext(string dbName)
        {
            var options = new DbContextOptionsBuilder<UserDbContext>()
                .UseInMemoryDatabase(databaseName: dbName)
                .Options;
            return new UserDbContext(options);
        }

        [Fact]
        public async Task CreateUser_AddsUserToDb()
        {
            var dbContext = GetDbContext(nameof(CreateUser_AddsUserToDb));
            var repo = new UserDataRepository(dbContext);
            var user = new UserData { FirstName = "A", LastName = "B", Email = "a@b.com" };

            await repo.CreateUser(user);

            Assert.Single(dbContext.Users);
            Assert.Equal("A", dbContext.Users.First().FirstName);
        }

        [Fact]
        public async Task UpdateUser_UpdatesUserInDb()
        {
            var dbContext = GetDbContext(nameof(UpdateUser_UpdatesUserInDb));
            var user = new UserData { Id = 1, FirstName = "A", LastName = "B", Email = "a@b.com" };
            dbContext.Users.Add(user);
            dbContext.SaveChanges();

            // Detach the entity so the context is not tracking it
            dbContext.Entry(user).State = EntityState.Detached;

            var repo = new UserDataRepository(dbContext);

            var updated = new UserData { Id = 1, FirstName = "X", LastName = "Y", Email = "x@y.com" };
            await repo.UpdateUser(updated);

            var dbUser = dbContext.Users.First(u => u.Id == 1);
            Assert.Equal("X", dbUser.FirstName);
            Assert.Equal("Y", dbUser.LastName);
            Assert.Equal("x@y.com", dbUser.Email);
        }

        [Fact]
        public async Task UpdateUser_Throws_WhenNotFound()
        {
            var dbContext = GetDbContext(nameof(UpdateUser_Throws_WhenNotFound));
            var repo = new UserDataRepository(dbContext);
            var user = new UserData { Id = 99, FirstName = "A", LastName = "B", Email = "a@b.com" };

            var ex = await Assert.ThrowsAsync<Exception>(() => repo.UpdateUser(user));
            Assert.Contains("Record not found", ex.Message);
        }

        [Fact]
        public async Task DeleteUser_RemovesUserFromDb()
        {
            var dbContext = GetDbContext(nameof(DeleteUser_RemovesUserFromDb));
            var user = new UserData { Id = 1, FirstName = "A", LastName = "B", Email = "a@b.com" };
            dbContext.Users.Add(user);
            dbContext.SaveChanges();
            var repo = new UserDataRepository(dbContext);

            await repo.DeleteUser(1);

            Assert.Empty(dbContext.Users);
        }

        [Fact]
        public async Task DeleteUser_Throws_WhenNotFound()
        {
            var dbContext = GetDbContext(nameof(DeleteUser_Throws_WhenNotFound));
            var repo = new UserDataRepository(dbContext);

            var ex = await Assert.ThrowsAsync<Exception>(() => repo.DeleteUser(99));
            Assert.Contains("Record not found", ex.Message);
        }

        [Fact]
        public async Task GetUserList_ReturnsFilteredSortedPagedList()
        {
            var dbContext = GetDbContext(nameof(GetUserList_ReturnsFilteredSortedPagedList));
            dbContext.Users.AddRange(
                new UserData { Id = 1, FirstName = "A", LastName = "B", Email = "a@b.com", PhoneNumber = "123", ZipCode = "11111" },
                new UserData { Id = 2, FirstName = "C", LastName = "D", Email = "c@d.com", PhoneNumber = "456", ZipCode = "22222" }
            );
            dbContext.SaveChanges();
            var repo = new UserDataRepository(dbContext);

            var request = new GetUserListRequestDto
            {
                FirstName = "A",
                PageSize = 1,
                PageCount = 1,
                SortColumn = "Id",
                SortOrder = "desc"
            };

            var (users, totalCount) = await repo.GetUserList(request);

            Assert.Single(users);
            Assert.Equal(1, totalCount);
            Assert.Equal("A", users[0].FirstName);
        }

        [Fact]
        public async Task GetUserList_ReturnsAll_WhenNoFilter()
        {
            var dbContext = GetDbContext(nameof(GetUserList_ReturnsAll_WhenNoFilter));
            dbContext.Users.AddRange(
                new UserData { Id = 1, FirstName = "A", LastName = "B", Email = "a@b.com" },
                new UserData { Id = 2, FirstName = "C", LastName = "D", Email = "c@d.com" }
            );
            dbContext.SaveChanges();
            var repo = new UserDataRepository(dbContext);

            var request = new GetUserListRequestDto
            {
                PageSize = 10,
                PageCount = 1
            };

            var (users, totalCount) = await repo.GetUserList(request);

            Assert.Equal(2, users.Count);
            Assert.Equal(2, totalCount);
        }

        [Fact]
        public async Task GetUserDetails_ReturnsUser_WhenExists()
        {
            var dbContext = GetDbContext(nameof(GetUserDetails_ReturnsUser_WhenExists));
            dbContext.Users.Add(new UserData { Id = 1, FirstName = "A", LastName = "B", Email = "a@b.com" });
            dbContext.SaveChanges();
            var repo = new UserDataRepository(dbContext);

            var user = await repo.GetUserDetails(1);

            Assert.NotNull(user);
            Assert.Equal(1, user.Id);
        }

        [Fact]
        public async Task GetUserDetails_Throws_WhenNotFound()
        {
            var dbContext = GetDbContext(nameof(GetUserDetails_Throws_WhenNotFound));
            var repo = new UserDataRepository(dbContext);

            var ex = await Assert.ThrowsAsync<Exception>(() => repo.GetUserDetails(99));
            Assert.Contains("Record not found", ex.Message);
        }
    }
}