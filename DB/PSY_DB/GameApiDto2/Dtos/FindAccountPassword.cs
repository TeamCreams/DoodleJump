namespace GameApi.Dtos
{
    public class ReqDtoGetUserAccountPassword
    {
        public string UserName { get; set; }
    }

    public class ResDtoGetUserAccountPassword
    {
        public string Password { get; set; }
    }
}