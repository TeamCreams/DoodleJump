namespace GameApi.Dtos
{
    public class ReqDtoInsertUserAccount
        // 기존 회원 가입
        // 구글 회원 가입
        // 기존 계정에 구글 계정 통합
    {
        public string? UserName { get; set; }
        public string? Password { get; set; }
        public string? NickName { get; set; }
    }

    public class ResDtoInsertUserAccount
    {
    }
}