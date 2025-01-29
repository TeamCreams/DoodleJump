using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using static Define;

public class LanguageDataManager
{
    private ELanguage _eLanguageInfo = ELanguage.Kr;
    public ELanguage ELanguageInfo
    {
        get { return _eLanguageInfo; }
        set { _eLanguageInfo = value; }
    }

    public string LocalizedString(int id)
    {
        var content = Managers.Data.GameLanguageDataDic[id];

        if(content == null)
        {
            return String.Empty;
        }
        switch (this.ELanguageInfo)
        {
            case ELanguage.Kr:
                return content.KrText;

            case ELanguage.En:
                return content.EnText;
        }

        return String.Empty;
    }
}