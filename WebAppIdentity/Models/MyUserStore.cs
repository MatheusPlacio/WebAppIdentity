﻿using Dapper;
using Microsoft.AspNetCore.Identity;
using System.Data.Common;
using System.Data.SqlClient;

namespace WebAppIdentity.Models
{
    public class MyUserStore : IUserStore<MyUser>, IUserPasswordStore<MyUser>
    {
        public async Task<IdentityResult> CreateAsync(MyUser user, CancellationToken cancellationToken)
        {
            using (var connection = GetOpenConnection())
            {
                await connection.ExecuteAsync("insert into Users" +
                    " Values(@id, @userName, @normalizedUserName, @passwordHash)",
                         new
                         {
                             id = Guid.NewGuid().ToString(),
                             userName = user.UserName,
                             normalizedUserName = user.NormalizedUserName,
                             passwordHash = user.PasswordHash
                         });
            }

            return IdentityResult.Success;
        }

        public async Task<IdentityResult> DeleteAsync(MyUser user, CancellationToken cancellationToken)
        {
            using (var connection = GetOpenConnection())
            {
                await connection.ExecuteAsync("delete from Users where Id = @id",
                         new
                         {
                             id = user.Id,
                         });
            }

            return IdentityResult.Success;
        }

        public void Dispose()
        {
            
        }

        public static DbConnection GetOpenConnection()
        {
            var connection = new SqlConnection("Server=DESKTOP-MH03KG6\\SQLEXPRESS;Database=Identificacao;Trusted_Connection=True;TrustServerCertificate=True;");

            connection.Open();

            return connection;
        }

        public async Task<MyUser?> FindByIdAsync(string userId, CancellationToken cancellationToken)
        {
            using(var connection = GetOpenConnection())
            {
                return await connection.QueryFirstOrDefaultAsync<MyUser>("select * from Users where Id = @id",
                    new { id = userId});
            }
        }

        public async Task<MyUser?> FindByNameAsync(string normalizedUserName, CancellationToken cancellationToken)
        {
            using (var connection = GetOpenConnection())
            {
                return await connection.QueryFirstOrDefaultAsync<MyUser>("select * from Users where normalizedUserName = @name",
                    new { name = normalizedUserName });
            }
        }

        public Task<string?> GetNormalizedUserNameAsync(MyUser user, CancellationToken cancellationToken)
        {
            return Task.FromResult(user.NormalizedUserName);
        }

        public Task<string> GetUserIdAsync(MyUser user, CancellationToken cancellationToken)
        {
            return Task.FromResult(user.Id);
        }

        public Task<string?> GetUserNameAsync(MyUser user, CancellationToken cancellationToken)
        {
            return Task.FromResult(user.UserName);
        }

        public Task SetNormalizedUserNameAsync(MyUser user, string? normalizedName, CancellationToken cancellationToken)
        {
            user.NormalizedUserName = normalizedName;
            return Task.CompletedTask;
        }

        public Task SetUserNameAsync(MyUser user, string? userName, CancellationToken cancellationToken)
        {
            user.UserName = userName;
            return Task.CompletedTask;
        }

        public async Task<IdentityResult> UpdateAsync(MyUser user, CancellationToken cancellationToken)
        {
            using (var connection = GetOpenConnection())
            {
                await connection.ExecuteAsync(
                    "update users " +
                    "set Id = @id," +
                    "UserName = @userName, " +
                    "NormalizedUserName = @normalizedUserName, " +
                    "PasswordHash = @passwordHash " +
                    "where Id = @id",
                         new 
                        {
                            id = user.Id,
                            userName = user.UserName,
                            NormalizedUserName = user.NormalizedUserName,
                            passwordHash = user.PasswordHash
                         });
            }

            return IdentityResult.Success;
        }

        public Task SetPasswordHashAsync(MyUser user, string? passwordHash, CancellationToken cancellationToken)
        {
            user.PasswordHash = passwordHash;
            return Task.CompletedTask;
        }

        public Task<string?> GetPasswordHashAsync(MyUser user, CancellationToken cancellationToken)
        {
            return Task.FromResult(user.PasswordHash);
        }

        public Task<bool> HasPasswordAsync(MyUser user, CancellationToken cancellationToken)
        {
            return Task.FromResult(user.PasswordHash != null);
        }
    }
}
