using System;
using System.Collections;
using UnityEngine;
using static Define;

public class ErrorManager
{
    public NoticeInfo GetError(EErrorCode searchType)
    {
        foreach (var item in Managers.Data.ErrorDataDic)
        {
            if (searchType == item.Value.Type)
            {
                switch (Managers.Language.ELanguageInfo)
                {
                    case ELanguage.Kr:
                        return new NoticeInfo(item.Value.TitleKr, item.Value.NoticeKr);

                    case ELanguage.En:
                        return new NoticeInfo(item.Value.TitleEn, item.Value.NoticeEn);
                }
            }
        }
        return new NoticeInfo("", "");
    }
}