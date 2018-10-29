namespace Galery.Common.DTO.User
{
    public class UserInfoDTO : UserDTO
    {
        public int SubscribersCount { get; set; }
        public bool IsInSubscribes { get; set; }
    }
}
