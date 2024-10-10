using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.XR.CoreUtils;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class CharacterDialogue : MonoBehaviour
{
    private Button dialogueBoxButton;
    [SerializeField] private TextMeshProUGUI dialogueText;
    [SerializeField] private Text legacyText;
    [SerializeField] private DialogueSO wrongDoorDialogue;
    [SerializeField] private CharacterStandingVisual characterVisual;
    private DialogueSO dialogue;
    private int lineIndex;
    private bool onWrongDoorDialogue;
    private bool wrongDoorDialogueStart = true;

    private void Start() {
        dialogueBoxButton = GetComponent<Button>();
        dialogueBoxButton.onClick.AddListener(UpdateLine);
        dialogue = GameFlow.Instance.GetCurrentDialogue();
        UpdateLine();
    }
    public void ToggleActiveState() {
        if (!gameObject.activeSelf && lineIndex == 0) {
            dialogue = GameFlow.Instance.GetCurrentDialogue();
        }
        gameObject.SetActive(!gameObject.activeSelf);
    }
    public void UpdateLine() {
        DormRoom dormRoom = GetComponentInParent<DormRoom>();
        if (onWrongDoorDialogue) {
            if (!wrongDoorDialogueStart) {
                Singleton.Instance.OnWrongDormRoomExited.Invoke(this, new Singleton.DormRoomEventArgs {
                    dormRoom = dormRoom
                });
            }
            FinishDialogueLine();
            Debug.Log("update line");
        } else if (GameFlow.Instance.atFreeTime) {
            if (dormRoom != null) {
                dialogue = dormRoom.GetCharacter().freeTimeDialogue;
            }
            if (lineIndex < dialogue.lines.Count) {
                SetLine(lineIndex);
            } else {
                lineIndex = 0;
                ToggleActiveState();
                Singleton.Instance.onDialogue = false;
                GameFlow.Instance.atFreeTime = false;
                Debug.Log("Encerrei o Free Time");
                Singleton.Instance.OnFreeTimeEnds?.Invoke(this, EventArgs.Empty);
            }
        } else {
            if (lineIndex < dialogue.lines.Count) {
                SetLine(lineIndex);
            } else {
                lineIndex = 0;
                FinishDialogueLine();
            }
        }
    }
    public void SetLine(int index) {
        List<DialogueSO.DialogueText> texts = dialogue.lines[index].texts;
        int randomLanguageIndex = RandomTextPicker.GetRandomLanguage();
        characterVisual.SetSprite(dialogue.lines[index].fullBodyExpression);
        SetText(texts[randomLanguageIndex]);
        lineIndex++;
    }
    public void FinishDialogueLine() {
        if (onWrongDoorDialogue && wrongDoorDialogueStart) {
            wrongDoorDialogueStart = false;
            return;
        }
        if (!onWrongDoorDialogue){
            GameFlow.Instance.UpdateCharacterVisitIndex();
            Singleton.Instance.OnCharacterVisited?.Invoke(this, EventArgs.Empty);
        }
        ToggleActiveState();
        Singleton.Instance.onDialogue = false;
        DormRoom dormRoom = GetComponentInParent<DormRoom>();
        if (dormRoom != null) {
            dormRoom.UpdateOnWrongDoorState(false);
        }
    }
    public void SetWrongDoorWarningText() {
        List<DialogueSO.DialogueText> texts = wrongDoorDialogue.lines[0].texts;
        int randomLanguageIndex = RandomTextPicker.GetRandomLanguage();
        SetText(texts[randomLanguageIndex]);
    }
    public void SetOnWrongDoorDialogue(bool state) {
        onWrongDoorDialogue = state;
    }
    private void SetText(DialogueSO.DialogueText text) {
        if (text.language == GameLanguage.Japanese) {
            dialogueText.gameObject.SetActive(false);
            legacyText.gameObject.SetActive(true);
            legacyText.text = text.text;
        } else {
            dialogueText.gameObject.SetActive(true);
            legacyText.gameObject.SetActive(false);
            dialogueText.text = text.text;
        }
    }
}
