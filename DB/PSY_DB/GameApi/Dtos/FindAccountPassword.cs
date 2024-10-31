namespace GameApi.Dtos;

public class ReqDtoFindAccountPassword
{
    public string? UserName { get; set; }
}

public class ResDtoFindAccountPassword
{
    public string? Password { get; set; }
}

