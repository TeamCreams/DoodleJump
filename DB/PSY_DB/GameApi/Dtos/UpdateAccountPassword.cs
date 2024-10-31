namespace GameApi.Dtos;

public class ReqDtoUpdateAccountPassword
{
    public string? UserName { get; set; }
    public string? Password { get; set; }
    public string? UpdatePassword { get; set; }
}


public class ResDtoUpdateAccountPassword
{
}
