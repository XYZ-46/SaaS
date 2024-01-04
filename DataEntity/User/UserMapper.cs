namespace DataEntity.User
{
    public static class UserMapper
    {

        public static UserLoginModel MapToUserLogin(this UserRegisterRequest userRegisterRequest)
        {
            return new()
            {
                Username = userRegisterRequest.Username,
                PasswordHash = userRegisterRequest.Password,
            };
        }

        public static UserProfileModel MapToUserProfile(this UserRegisterRequest userRegisterRequest)
        {
            return new()
            {
                Fullname = userRegisterRequest.Fullname,
                Email = userRegisterRequest.Email,
            };
        }
    }
}
