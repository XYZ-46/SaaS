﻿using DataEntity.Model;
using InterfaceProject.Repository;
using InterfaceProject.Service;
using Microsoft.EntityFrameworkCore;
using Repository.Database;

namespace Repository
{
    public class UserProfileRepository(AzureDB azureDB, IRedisService cacheHandler)
        : BaseCrudRepository<UserProfileModel>(azureDB), IUserProfileRepository
    {
        private readonly IRedisService _cacheHandler = cacheHandler;

        public async Task<UserProfileModel?> FindByEmailAsync(string email)
        {
            var userProfile = await _azureDB.UserProfileModel
                .Include(x => x.UserLogin)
                .SingleOrDefaultAsync(x => x.Email == email && !x.IsDelete);
            return userProfile;
        }

        public async Task<UserProfileModel?> FindByFullNameAsync(string fullname) => await _azureDB.UserProfileModel.SingleOrDefaultAsync(x => x.Fullname == fullname && !x.IsDelete);

        public async Task<UserProfileModel?> FindByUserLoginUsernameAsync(string username)
        {
            var keyCache = $"UserProfileModel:{username}";
            var cacheData = await _cacheHandler.GetDataAsync<UserProfileModel>(keyCache);
            if (cacheData != null) return cacheData;

            var userProfile = await _azureDB.UserProfileModel
                                            .Include(x => x.UserLogin)
                                            .SingleOrDefaultAsync(x => x.UserLogin.Username == username && !x.IsDelete && !x.UserLogin.IsDelete);
            if (userProfile != null)
            {
                // update cache
                var expirationTime = DateTime.Now.AddHours(24).TimeOfDay;
                await _cacheHandler.SetDataAsync(keyCache, userProfile, expirationTime);
            }

            return userProfile;
        }

        public async Task<UserProfileModel?> FindByUserLoginIdAsync(int userLoginId)
        {
            var userProfile = await _azureDB.UserProfileModel.SingleOrDefaultAsync(x => x.UserLoginId == userLoginId && !x.IsDelete);
            return userProfile;
        }

        public IQueryable<UserProfileModel> Paging() => _azureDB.UserProfileModel.AsQueryable();

    }
}
