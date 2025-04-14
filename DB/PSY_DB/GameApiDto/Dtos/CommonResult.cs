namespace WebApi.Models.Dto
{
    public class CommonResult<T>
    {
        public bool IsSuccess { get; set; } = false;
        public string Message { get; set; } = string.Empty;
        public EStatusCode StatusCode { get; set; } = EStatusCode.OK;
        public T Data { get; set; }
    }

    public enum EStatusCode
    { 
        OK,
        UnknownError,           // 서버에서 결과를 못줬을때.
        MissionAlreadyExists,   // 이미 있는 미션
        NotFoundEntity,
        ServerException,
        NameAlreadyExists,
        RequestError,
        ChangedRowsIsZero, // 변경된 DB 줄 수 0
        EnergyInsufficient,
        NotConnectionUser,
        GoogleAccountAlreadyExists
    }


    public class CommonException : Exception
    {
        protected EStatusCode _statusCode = EStatusCode.OK;
        public int StatusCode => (int)_statusCode;
        
        protected object _data;
        public object Data => _data;

        public CommonException(EStatusCode statusCode, string message, object data = null) : base(message) 
        {
            _statusCode = statusCode;
            _data = data;
        }

        public override string ToString()
        {
            return $"[{StatusCode}] {this.Message}";
        }
    }
}
