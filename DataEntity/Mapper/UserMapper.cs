using DataEntity.Model;
using DataEntity.Request;

namespace DataEntity.Mapper
{
    public static class UserMapper
    {
        public static UserLoginModel MapToUserLogin(this UserRegisterRequest userRegisterRequest)
        {
            return new UserLoginModel()
            {
                Username = userRegisterRequest.Username,
                PasswordHash = userRegisterRequest.Password
            };
        }

        public static UserProfileModel MapToUserProfile(this UserRegisterRequest userRegisterRequest)
        {
            return new UserProfileModel()
            {
                Fullname = userRegisterRequest.Fullname,
                Email = userRegisterRequest.Email
            };
        }
    }
}
