using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LanguageCard : MonoBehaviour
{
    [SerializeField] private GameLanguage language;
    [SerializeField] private Button languageButton;
    [SerializeField] private Image frame;
    [SerializeField] private Sprite unselectedFrame;
    [SerializeField] private Sprite selectedFrame;
    [SerializeField] private string languageName;
    // Start is called before the first frame update
    void Start()
    {
        languageButton.onClick.AddListener(HandleClick);
    }

    // Update is called once per frame
    void Update()
    {
        if (GetComponentInParent<LanguageMenuVisual>().onNativeLanguageSelection) {
            languageButton.transition = Selectable.Transition.SpriteSwap;
        } else {
            languageButton.transition = Selectable.Transition.None;
        }
        
    }
    public string GetLanguageName() {
        return languageName;
    }
    public void HandleClick() {
        AudioManager.Instance.PlayButtonShort();
        if (GetComponentInParent<LanguageMenuVisual>().onNativeLanguageSelection) {
            Singleton.Instance.nativeLanguage = language;
        } else {
            if (Singleton.Instance.selectedLanguages.Contains(language)) {
                Singleton.Instance.selectedLanguages.Remove(language);
                frame.sprite = unselectedFrame;
            } else {
                Singleton.Instance.selectedLanguages.Add(language);
                frame.sprite = selectedFrame;
            }
        }
        GetComponentInParent<LanguageMenuVisual>().SetLastSelectedLanguage(languageName.ToUpper());
    }
}
