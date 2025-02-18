namespace GameApi.Dtos
{
    public class ReqDtoInsertUserAccount
    {
        public string? UserName { get; set; }
        public string? Password { get; set; }
        public string? NickName { get; set; }
    }

    public class ResDtoInsertUserAccount
    {
    }
}