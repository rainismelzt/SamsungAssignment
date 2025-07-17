using Microsoft.EntityFrameworkCore;
using UserService.Core.Dto;
using UserService.Core.Interface;
using UserService.Database;
using UserService.Database.BusinessEntity;
using System.Linq.Dynamic.Core;

namespace UserService.Infrastructure.Repository
{
    public class UserDataRepository : IUserDataRepository
    {
        private readonly UserDbContext _dbContext;

        public UserDataRepository(UserDbContext dbContext) {
            _dbContext = dbContext;
        }

        public async Task CreateUser(UserData user)
        {
            _dbContext.Users.Add(user);
            await _dbContext.SaveChangesAsync();
        }

        public async Task UpdateUser(UserData user)
        {
            var userToBeUpdate = await _dbContext.Users.AsNoTracking().FirstOrDefaultAsync(u => u.Id.Equals(user.Id)) ?? throw new Exception("Record not found.");
            _dbContext.Users.Update(user);
            await _dbContext.SaveChangesAsync();
        }

        public async Task DeleteUser(long id)
        {
            var userToBeRemove = await _dbContext.Users.FirstOrDefaultAsync(u => u.Id.Equals(id)) ?? throw new Exception("Record not found.");
            _dbContext.Remove(userToBeRemove);
            await _dbContext.SaveChangesAsync();
        }

        public async Task<(List<UserData>, int)> GetUserList(GetUserListRequestDto request)
        {
            var queryable = _dbContext.Users.AsQueryable();
            // Apply filtering if any
            if (!string.IsNullOrWhiteSpace(request.FirstName))
                queryable = queryable.Where(u => u.FirstName.Contains(request.FirstName));
            if (!string.IsNullOrWhiteSpace(request.LastName))
                queryable = queryable.Where(u => u.LastName.Contains(request.LastName));
            if (!string.IsNullOrWhiteSpace(request.Email))
                queryable = queryable.Where(u => u.Email.Contains(request.Email));
            if (!string.IsNullOrWhiteSpace(request.PhoneNumber))
                queryable = queryable.Where(u => !string.IsNullOrWhiteSpace(u.PhoneNumber) && u.PhoneNumber.Contains(request.PhoneNumber));
            if (!string.IsNullOrWhiteSpace(request.ZipCode))
                queryable = queryable.Where(u => !string.IsNullOrWhiteSpace(u.ZipCode) && u.ZipCode.Contains(request.ZipCode));

            // Apply sorting if any
            if (!string.IsNullOrWhiteSpace(request.SortColumn) && !string.IsNullOrWhiteSpace(request.SortOrder))
            {
                queryable = queryable.OrderBy($"{request.SortColumn} {request.SortOrder}");
            }

            // Get total count
            int totalCount = queryable.Count();

            // Apply pagination
            queryable = queryable.Skip((request.PageCount - 1) * request.PageSize).Take(request.PageSize);

            return (await queryable.ToListAsync(), totalCount);
        }

        public async Task<UserData> GetUserDetails(long id)
        {
            var user = await _dbContext.Users.FirstOrDefaultAsync(u => u.Id.Equals(id)) ?? throw new Exception("Record not found.");
            return user;
        }
    }
}
