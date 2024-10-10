using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CharacterStandingVisual : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private Button dialogueButton;
    [SerializeField] private Image dialogueBox;
    [SerializeField] private Image characterStandingImage;
    [SerializeField] private Animator characterStandingAnimator;
    private Sprite defaultSprite;
    private bool onWrongDoorDialogue;

    private void Start() {
        defaultSprite = characterStandingImage.sprite;
        dialogueButton.onClick.AddListener(ShowDialogue);
        SetActive(dialogueButton.gameObject, false);
        SetActive(dialogueBox.gameObject, false);
    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        if (!(dialogueBox.gameObject.activeSelf || onWrongDoorDialogue)) {
            DormRoom dormRoom = GetComponentInParent<DormRoom>();
            OutdoorCharacterHandler handler = GetComponentInParent<OutdoorCharacterHandler>();
            if (dormRoom != null && (dormRoom.GetCharacter() == GameFlow.Instance.GetCurrentDialogue().lines[0].character || GameFlow.Instance.atFreeTime)) {
                SetActive(dialogueButton.gameObject, true);
            } else if (handler != null) {
                if (handler.gameObject.activeSelf) {
                    SetActive(dialogueButton.gameObject, true);
                }
            }
        }
    }
    private void Update() {
        // DormRoom dormRoom = GetComponentInParent<DormRoom>();
        // if (dormRoom != null && dormRoom.GetCharacter().characterIndex == 10) {
        //     transform.position = GameFlow.Instance.mikanAnalysisPosition;
        // }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (!(dialogueBox.gameObject.activeSelf || onWrongDoorDialogue)) {
            SetActive(dialogueButton.gameObject, false);
        }
    }
    public void SetActive(GameObject gameObject, bool state) {
        gameObject.SetActive(state);
    }
    public void ShowDialogue() {
        dialogueBox.gameObject.GetComponent<CharacterDialogue>().ToggleActiveState();
        SetActive(dialogueButton.gameObject, false);
        characterStandingAnimator.SetTrigger("CharacterBounce");
        AudioManager.Instance.PlayCharacterSelect();
        EventSystem.current.SetSelectedGameObject(null);
        Singleton.Instance.onDialogue = true;
    }
    public void SetOnWrongDoorDialogue(bool state) {
        onWrongDoorDialogue = state;
    }
    public void SetSprite(Sprite sprite) {
        if (sprite != null) {
            characterStandingImage.sprite = sprite;
        } else {
            characterStandingImage.sprite = defaultSprite;
        }
    }
}
