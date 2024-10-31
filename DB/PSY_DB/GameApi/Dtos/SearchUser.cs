namespace GameApi.Dtos;

public class ReqDtoSearchUser
{
    public string? UserName { get; set; }
    public string? Password { get; set; }
}

public class ResDtoSearchUser
{
    public string? UserName { get; set; }
    public DateTime RegisterDate { get; set; }
    public DateTime UpdateDate { get; set; }
    public DateTime? DeletedDate { get; set; }
}