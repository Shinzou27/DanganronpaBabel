using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PekoMotivationVisual : MonoBehaviour
{
    [SerializeField] private Image image;
    // Start is called before the first frame update
    void Start()
    {
       image.gameObject.SetActive(false);
       Singleton.Instance.OnGameOver += HandleImageVisibility;
    }

    private void HandleImageVisibility(object sender, EventArgs e)
    {
        if (GameFlow.Instance.culprit.characterIndex == 13) {
            image.gameObject.SetActive(true);
        } else {
            image.gameObject.SetActive(false);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
