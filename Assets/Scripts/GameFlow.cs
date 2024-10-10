using System;
using System.Collections;
using System.Collections.Generic;
using Unity.XR.CoreUtils;
using UnityEngine;
using UnityEngine.TextCore.Text;

public class GameFlow : MonoBehaviour
{
    public static GameFlow Instance { get; private set; }
    public List<CharacterSO> characterVisitOrder;
    public List<bool> visitedCharacters;
    public List<DialogueSO> dialogueReadingOrder;
    public List<string> goals;
    public List<BulletSO> bullets;
    public int characterToVisitIndex;
    public bool atFreeTime;
    public bool inHouse;
    public int HP = 100;
    public DormRoom lastEnteredRoom;
    public BuffObject holdingBuffObject;
    public CharacterSO culprit;
    public enum GameState {
        Menu,
        Investigation,
        FreeTime,
        Trial
    }
    public GameState currentState;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else if (Instance != this)
        {
            Destroy(this);
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        ClearBulletsState();
        currentState = GameState.Menu;
        visitedCharacters = new()
        {
            false,
            false,
            false,
            false,
            false,
            false,
            false,
            false,
            false,
            false,
            false,
            false,
            false,
            false,
            false,
            false
        };
        ResetVisitedCharacters();
    }

    public void ToggleInHouse() {
        inHouse = !inHouse;
    }
    public bool CheckCharacterMatch(CharacterSO character) {
        return characterVisitOrder[characterToVisitIndex] == character;
    }
    public void UpdateCharacterVisitIndex() {
        Debug.Log("Passei pro próximo diálogo!");
        int characterVisitOrderCount = characterVisitOrder.Count;
        CheckNeedToTriggerBullet();
        if (characterToVisitIndex < characterVisitOrderCount - 1) {
            characterToVisitIndex++;
            ResetVisitedCharacters();
            if (characterToVisitIndex == characterVisitOrder.Count - 3) {
                Singleton.Instance.OnInvestigationSecondPhase?.Invoke(this, EventArgs.Empty);
            }
            if (characterToVisitIndex == characterVisitOrder.Count - 2) {
                Singleton.Instance.OnBloodAnalysis?.Invoke(this, EventArgs.Empty);
            }
            if (characterToVisitIndex == characterVisitOrder.Count - 1) {
                atFreeTime = true;
                Singleton.Instance.OnFreeTimeStarts?.Invoke(this, EventArgs.Empty);
            }
        } else {
                Singleton.Instance.OnTrialStarts?.Invoke(this, EventArgs.Empty);
        }
    }

    private void CheckNeedToTriggerBullet()
    {
        foreach (BulletSO bullet in bullets) {
            if (!bullet.found && bullet.triggerDialogue == GetCurrentDialogue()) {
                ChangeBulletState(bullet);
                AudioManager.Instance.PlayBulletObtainedAudio();
            }
        }
    }

    public DialogueSO GetCurrentDialogue() {
        return dialogueReadingOrder[characterToVisitIndex];
    }
    public void ChangeBulletState(BulletSO bullet) {
        bullet.found = true;
    }
    public void ClearBulletsState() {
        foreach (BulletSO bullet in bullets) {
            bullet.found = false;
        }
    }
    public bool IsInHouse() {
        return inHouse;
    }
    public void AddToVisit(CharacterSO character) {
        Debug.Log("Acabei de visitar " + character.characterName);
        visitedCharacters[character.characterIndex] = true;
    }
    public bool Visited(CharacterSO character) {
        return visitedCharacters[character.characterIndex];
    }
    public bool Visited(int characterIndex) {
        return visitedCharacters[characterIndex];
    }
    public void Damage(int value) {
        HP = Mathf.Clamp(HP - value, 0, 100);
        if (HP <= 0) {
            Singleton.Instance.OnGameOver?.Invoke(this, EventArgs.Empty);
        }
    }
    public void Heal(int value) {
        HP = Mathf.Clamp(HP + value, 0, 100);
    }
    public void SetHoldingObject(BuffObject buffObject) {
        holdingBuffObject = buffObject;
    }
    public void ReleaseObject() {
        holdingBuffObject = BuffObject.NONE;
    }
    public void SelectCulprit(CharacterSO character) {
        culprit = character;
    }
    public string GetCurrentGoal() {
        return goals[characterToVisitIndex];
    }
    public List<bool> GetVisitedCharacters() {
        return visitedCharacters;
    }
    private void ResetVisitedCharacters() {
        for (int i = 0; i < visitedCharacters.Count; i++)
        {
            visitedCharacters[i] = false;
        }
    }
}
