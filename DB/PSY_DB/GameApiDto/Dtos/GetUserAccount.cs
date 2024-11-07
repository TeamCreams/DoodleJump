namespace GameApi.Dtos
{

    public class ReqDtoGetUserAccount
    {
        public string? UserName { get; set; }
    }

    public class ResDtoGetUserAccount
    {
        public string? UserName { get; set; }
        public string? Password { get; set; }
        public string? Nickname { get; set; }

        public DateTime RegisterDate { get; set; }
        public DateTime UpdateDate { get; set; }
        public DateTime? DeletedDate { get; set; }
        public int HighScore { get; set; }
        public int LatelyScore { get; set; }
        public int Gold {  get; set; }
    }
}