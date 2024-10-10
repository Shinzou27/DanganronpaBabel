using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LanguageMenuVisual : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI descriptionLabel;
    [SerializeField] TextMeshProUGUI lastSelectedLanguageName;
    [SerializeField] TextMeshProUGUI proceedButtonLabel;
    [SerializeField] Button proceedButton;

    public bool onNativeLanguageSelection = true;

    // Start is called before the first frame update
    void Start()
    {
        proceedButton.onClick.AddListener(HandlePage);
        descriptionLabel.text = "SELECIONE SEU IDIOMA NATIVO.";
        proceedButtonLabel.text = "CONTINUAR";
    }

    // Update is called once per frame
    void Update()
    {
        if (onNativeLanguageSelection) {
            proceedButton.enabled = (int) Singleton.Instance.nativeLanguage >= 0;
        } else {
            proceedButton.enabled = Singleton.Instance.selectedLanguages.Count > 0;
        }
        
    }
    private void HandlePage() {
        AudioManager.Instance.PlayButtonLong();
        if (onNativeLanguageSelection) {
            descriptionLabel.text = "SELECIONE OS IDIOMAS QUE DESEJA APRENDER.";
            proceedButtonLabel.text = "INICIAR";
            SetLastSelectedLanguage("");
            onNativeLanguageSelection = false;
        } else {
            Singleton.Instance.OnStartGame?.Invoke(this, EventArgs.Empty);
        }
    }
    public void SetLastSelectedLanguage(string language){
        lastSelectedLanguageName.text = language;
    }
}
