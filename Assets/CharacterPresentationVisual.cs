using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterPresentationVisual : MonoBehaviour
{
    [SerializeField] private Sprite[] images;
    [SerializeField] private Image active;
    [SerializeField] private Button next;
    [SerializeField] private Button previous;
    private int imageIndex;
    // Start is called before the first frame update
    void Start()
    {
        next.onClick.AddListener(GoToNext);
        previous.onClick.AddListener(GoToPrevious);
        active.sprite = images[imageIndex];
        SetSprite();
    }
    private void GoToPrevious()
    {
        if (imageIndex > 0) {
            imageIndex--;
            SetSprite();
        }
    }

    private void GoToNext()
    {
        if (imageIndex < images.Length - 1) {
            imageIndex++;
            SetSprite();
        }
    }
    private void SetSprite() {
            active.sprite = images[imageIndex];
    }
}
