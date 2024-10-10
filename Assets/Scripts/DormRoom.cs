using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DormRoom : MonoBehaviour
{
    [SerializeField] private Transform buffObjectTransformParent;
    [SerializeField] private Canvas canvas;
    [SerializeField] private GameObject dialogueBox;
    [SerializeField] private GameObject characterStanding;
    [SerializeField] private Animator doorAnimator;
    private CharacterSO character;
    private bool locked = true;
    // private bool setInitialLockState = false;
    // Start is called before the first frame update
    void Start()
    {
        canvas.worldCamera = Camera.main;
        UpdateLockedState();
        Singleton.Instance.OnCharacterVisited += UpdateLockedState;
        Singleton.Instance.OnWrongDormRoom += ReplacePosition;
        Singleton.Instance.OnFreeTimeAlertShow += SpawnBuffObject;
    }

    private void ReplacePosition(object sender, Singleton.DormRoomEventArgs e)
    {
        float onWrongPosition = -280;
        float initialPosition = 280;
        if (e.dormRoom == this) {
            dialogueBox.transform.localPosition = new(dialogueBox.transform.localPosition.x, dialogueBox.transform.localPosition.y, onWrongPosition);
            characterStanding.transform.localPosition = new(characterStanding.transform.localPosition.x, characterStanding.transform.localPosition.y, onWrongPosition);
            UpdateOnWrongDoorState(true);
            characterStanding.GetComponent<CharacterStandingVisual>().ShowDialogue();
            dialogueBox.GetComponent<CharacterDialogue>().SetWrongDoorWarningText();
        } else {
            dialogueBox.transform.localPosition = new(dialogueBox.transform.localPosition.x, dialogueBox.transform.localPosition.y, initialPosition);
            characterStanding.transform.localPosition = new(characterStanding.transform.localPosition.x, characterStanding.transform.localPosition.y, initialPosition);
            UpdateOnWrongDoorState(false);
        }
    }
    // Update is called once per frame
    void Update()
    {
        // if (!setInitialLockState) {
            UpdateLockedState();
        // }
    }
    public void SetCharacter(CharacterSO _character) {
        character = _character;
    }
    public void SetLocked(bool _locked) {
        locked = _locked;
    }
    public bool GetLockedState() {
        return locked;
    }
    private void UpdateLockedState() {
        if (character != null) {
            if (GameFlow.Instance.atFreeTime ||
                GameFlow.Instance.CheckCharacterMatch(character) ||
                character.characterIndex == 5 ||
                character.characterIndex == 3 ||
                GameFlow.Instance.IsInHouse()
                ) {
                locked = false;
            } else {
                locked = true;
            }
            canvas.gameObject.SetActive(!(character.characterIndex == 5 || character.characterIndex == 3));
        }
    }
    private void UpdateLockedState(object sender, EventArgs e)
    {
        UpdateLockedState();
    }
    public CharacterSO GetCharacter() {
        return character;
    }
    public void SpawnBuffObject(object sender, EventArgs e) {
        for (int i = 0; i < buffObjectTransformParent.childCount; i++) {
            Destroy(buffObjectTransformParent.GetChild(i).gameObject);
        }
        if (GameFlow.Instance.atFreeTime || character.characterIndex == 5) {
            GameObject newBuffObject = Instantiate(character.buffObject, buffObjectTransformParent);
        }
    }
    public void SpawnBuffObject() {
        for (int i = 0; i < buffObjectTransformParent.childCount; i++) {
            Destroy(buffObjectTransformParent.GetChild(i).gameObject);
        }
        if (character.characterIndex == 5 || GameFlow.Instance.atFreeTime) {
            GameObject newBuffObject = Instantiate(character.buffObject, buffObjectTransformParent);
        }
    }
    public void UpdateOnWrongDoorState(bool state) {
        dialogueBox.GetComponent<CharacterDialogue>().SetOnWrongDoorDialogue(state);
        characterStanding.GetComponent<CharacterStandingVisual>().SetOnWrongDoorDialogue(state);
        // doorAnimator.SetTrigger("EnteredRoom");
    }
}
