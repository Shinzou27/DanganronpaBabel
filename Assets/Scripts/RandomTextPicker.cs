using System;
using System.Collections.Generic;

public class RandomTextPicker {
    public static int GetRandomLanguage() {
        Random random = new();
        GameLanguage native = Singleton.Instance.nativeLanguage;
        List<GameLanguage> languages = Singleton.Instance.selectedLanguages;
        int nativeLanguageChance = 75;
        if (random.Next(100) < nativeLanguageChance) { // 75% de chance de vir o idioma escolhido como nativo
            return (int) native;
        }
        if (!languages.Contains(native)) {
            languages.Add(native);
        }
        for (int i = languages.Count - 1; i > 0; i--)
        {
            int j = random.Next(i + 1);
            (languages[j], languages[i]) = (languages[i], languages[j]);
        }
        return (int)languages[0];
    }
}