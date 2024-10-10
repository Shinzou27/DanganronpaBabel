using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuVisual : MonoBehaviour
{
    [SerializeField] GameObject mainWrapper;
    [SerializeField] GameObject languageSelectorWrapper;
    // Start is called before the first frame update
    void Start()
    {
        ShowMainMenu();
        Singleton.Instance.OnStartGame += HideRoom;
    }

    private void HideRoom(object sender, EventArgs e)
    {
        gameObject.SetActive(false);
    }
    public void ShowLanguageMenu() {
        AudioManager.Instance.PlayButtonLong();
        mainWrapper.SetActive(false);
        languageSelectorWrapper.SetActive(true);
    }
    public void ShowMainMenu() {
        mainWrapper.SetActive(true);
        languageSelectorWrapper.SetActive(false);
    }
}
