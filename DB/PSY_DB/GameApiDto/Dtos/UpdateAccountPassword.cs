namespace GameApi.Dtos
{
    public class ReqDtoUpdateUserAccountPassword
    {
        public string? UserName { get; set; }
        public string? Password { get; set; }
        public string? UpdatePassword { get; set; }
    }

    public class ResDtoUpdateUserAccountPassword
    {
    }
}