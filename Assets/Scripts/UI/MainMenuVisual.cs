using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuVisual : MonoBehaviour
{
    [SerializeField] MenuVisual menu;
    [SerializeField] Button playButton;
    [SerializeField] Button quitButton;
    // Start is called before the first frame update
    void Start()
    {
        playButton.onClick.AddListener(menu.ShowLanguageMenu);
        quitButton.onClick.AddListener(Application.Quit);
    }
}
