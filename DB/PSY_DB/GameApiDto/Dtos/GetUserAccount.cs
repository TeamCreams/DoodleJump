﻿namespace GameApi.Dtos
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
        public int UserAccountId { get; set; }

        public DateTime RegisterDate { get; set; }
        public DateTime UpdateDate { get; set; }
        public DateTime? DeletedDate { get; set; }
        public int HighScore { get; set; }
        public int LatelyScore { get; set; }
        public int Gold {  get; set; }
        public int TotalScore { get; set; }
        // 디자인
        public int HairStyle { get; set; }
        public int EyebrowStyle { get; set; }
        public int EyesStyle { get; set; }
        // 업데이트 스택
        public int Evolution { get; set; }
    }
}