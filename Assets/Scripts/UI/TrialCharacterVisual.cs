using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR.Interaction.Toolkit.UI;

public class TrialCharacterVisual : MonoBehaviour
{
    [SerializeField] private CharacterSO character;
    [SerializeField] private Image image;
    [SerializeField] private Button characterClick;
    [SerializeField] private Animator animator;
    private bool canSelect;
    // Start is called before the first frame update
    void Start()
    {
        GetComponent<Canvas>().worldCamera = Camera.main;
        characterClick.onClick.AddListener(CulpritSelect);
        image.sprite = character.fullBodySprite;
        Singleton.Instance.OnCulpritSelection += EnableCulpritSelect;
    }


    private void Update() {
        GetComponent<TrackedDeviceGraphicRaycaster>().enabled = Singleton.Instance.onTrial && canSelect;
    }
    public CharacterSO GetCharacter() {
        return character;
    }
    public Transform GetImageTransform() {
        return image.gameObject.transform;
    }
    private void EnableCulpritSelect(object sender, EventArgs e)
    {
        canSelect = true;
    }
    private void CulpritSelect() {
        GameFlow.Instance.SelectCulprit(character);
        Singleton.Instance.OnCulpritSelect?.Invoke(this, new Singleton.CharacterEventArgs {
            character = character
        });
        animator.SetTrigger("CulpritSelect");
        AudioManager.Instance.PlayCharacterSelect();
    }
    public void SetSprite(Sprite sprite) {
        image.sprite = sprite;
    }
}
