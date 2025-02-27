using UnityEngine;

public static class SecurePlayerPrefs
{
    public static void SetString(string key, string value)
    {
        PlayerPrefs.SetString(key, value);
    }

    public static string GetString(string key, string defaultValue)
    {
        return PlayerPrefs.GetString(key, defaultValue);
    }
}


//저장할떄 암호화
// 암호화 기술
//   단방향 암호화 SHA-256 (서버에서 패스워드 확인)
//    [String] => [암호화된 String]

//   양방향 암호화 AES-256 (유저 데이터)
//    [String] <=> [암호화된 String]
//    
//   저장할때, String이랑 Key 를 같이 입력받습니다.
//   해독할때 필요한 열쇠
//   클라에 하드코딩형태로 저장합니다.


// 
//312433c28349f63c4f387953ff337046e794bea0f9b9ebfcb08e90046ded9c76
//1000