using Microsoft.AspNetCore.Http.HttpResults;
using StoreServer.Entities;
using Microsoft.Extensions.DependencyInjection;
using StoreServer.Models;
using Microsoft.EntityFrameworkCore;
using StoreServer.Models.EFCoreEntitiesCopies;

namespace StoreServer.Services
{
    /// <summary>
    /// Provides functions for managing all user related 
    /// </summary>
    public class UsersService
    {
        StoreContext db;
        JWTTokenConstructor tokenConstructor;
        public UsersService(StoreContext db, JWTTokenConstructor tokenConstructor)
        {
            this.db = db;
            this.tokenConstructor = tokenConstructor;
        }
        /// <summary>
        /// Registers user in system
        /// </summary>
        /// <param name="user"></param>
        /// <returns>Token instance</returns>
        public async Task<Token> RegisterAsync(User user)
        {
            bool userExists = db.Users.Any(x => x.UserLogin == user.UserLogin);
            if (userExists) return null;

            UserEntity newUser = new UserEntity()
            {
                UserName = user.UserName,
                UserLogin = user.UserLogin,
                Password = user.Password
            };

            await db.Users.AddAsync(newUser);
            await db.SaveChangesAsync();

            UserEntity addedUser = await db.Users.FirstOrDefaultAsync(u => u.UserLogin == user.UserLogin);

            user.Id = addedUser.Id;

            return tokenConstructor.GetToken(user);
        }
        /// <summary>
        /// Logins user in system
        /// </summary>
        /// <param name="userEntity"></param>
        /// <returns>Token instance</returns>
        public async Task<Token> LoginAsync(User user)
        {
            string? login = user.UserLogin;
            string? password = user.Password;

            UserEntity userEntity = await db.Users.FirstOrDefaultAsync(u => u.UserLogin == login);
            if (userEntity is null) return null;
            if (password != db.Users.FirstOrDefault(x => x.UserLogin == login).Password) return null;

            user.Id = userEntity.Id;

            return tokenConstructor.GetToken(user);
        }
        /// <summary>
        /// Adds new user to the system
        /// </summary>
        /// <param name="user"></param>
        /// <returns>User instance</returns>
        public async Task<User> AddUserAsync(User user)
        {
            UserEntity newUser = new UserEntity()
            {
                UserName = user.UserName,
                UserLogin = user.UserLogin,
                Password = user.Password
            };
            await db.Users.AddAsync(newUser);
            await db.SaveChangesAsync();

            return user;
        }
        /// <summary>
        /// Gets specific user
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public async Task<User> GetUserAsync(int userId)
        {
            UserEntity user = await db.Users.FirstAsync(x => x.Id == userId);
            if (user is null) return null;

            User foundUser = new User()
            {
                UserName = user.UserName,
                UserLogin = user.UserLogin,
                Password = user.Password
            };

            return foundUser;
        }
        /// <summary>
        /// Gets all users
        /// </summary>
        /// <returns></returns>
        public async Task<List<UserEntity>> GetUsersAsync()
        {
            return await db.Users.ToListAsync();
        }
        /// <summary>
        /// Deletes user
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public async Task<User> DeleteUserAsync(int userId)
        {
            UserEntity user = await db.Users.FirstAsync(x => x.Id == userId);
            if (user == null) return null;
            if (user.UserLogin == "admin") return null;

            db.Users.Remove(user);
            await db.SaveChangesAsync();

            User deletedUser = new User()
            {
                UserName = user.UserName,
                UserLogin = user.UserLogin,
                Password = user.Password
            };
            return deletedUser;
        }
        /// <summary>
        /// Changes existing user
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public async Task<User> ChangeUserAsync(User user)
        {
            var updatedUser = await db.Users.FirstOrDefaultAsync(x => x.UserLogin == user.UserLogin);
            if (user is null) return null;

            updatedUser.UserName = user.UserName;
            updatedUser.UserLogin = user.UserLogin;
            await db.SaveChangesAsync();

            return user;
        }
    }
}
