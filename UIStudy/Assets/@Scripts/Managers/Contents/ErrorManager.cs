using System;
using System.Collections;
using UnityEngine;
using static Define;

public class ErrorMamager
{
    static (string title, string notice) GetError(NoticeType searchType)
    {
        foreach (var item in Managers.Data.ErrorDataDic)
        {
            if (searchType == item.Type)
            {
                switch (Managers.Language.ELanguageInfo)
                {
                    case ELanguage.Kr:
                        return (item.TitleKr, item.NoticeKr);

                    case ELanguage.En:
                        return (item.TitleEn, item.NoticeEn);
                }
            }
        }
        return ("", "");
    }
}