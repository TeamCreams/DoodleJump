using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using static Define;

public class LanguageDataMamager
{
    public string LocalizedString(ELocalizableTerms eLocalizableTerm)
    {
        int stringId = 0;

        foreach (var gameLanguageData in Managers.Data.GameLanguageDataDic)
        {
            if(gameLanguageData.Value.LocalizableTerm == eLocalizableTerm)
            {
                stringId = gameLanguageData.Value.Id;
                break;
            }
        }

        var content = Managers.Data.GameLanguageDataDic[stringId];

        switch (Managers.Game.ELanguageInfo)
        {
            case ELanguage.Kr:
                return content.KrText;

            case ELanguage.En:
                return content.EnText;
        }

        return "";
    }
}