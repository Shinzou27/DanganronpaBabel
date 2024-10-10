using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrialGround : MonoBehaviour
{
    public bool gameStarted;
    public float animationDuration = 5.5f;
    public float textSpeed = 1.5f;
    public float damageMultiplier = 1f;
    public float healMultiplier = 1f;
    public float slowMotionMultiplier = 0.33f;
    public float fastForwardMultiplier = 2f;
    public float bulletSpeed = 1f;
    public int fakeCounterableArgumentsChance = 20;
    public bool slowMotionEnabled = false;
    [Serializable]
    public class CounterArgument {
        public int argumentIndex;
        public BulletSO bullet;
        public bool shot;
    }
    [SerializeField] public List<CounterArgument> counterArguments;
    [SerializeField] public List<TrialCharacterVisual> characters;
    public CounterArgument lastArgumentWon;
    public DialogueSO trialDialogue;
    public int randomLanguageIndex;
    private void OnTriggerEnter(Collider other)
    {
        Singleton.Instance.onTrial = true;
        Singleton.Instance.OnTrialEnter?.Invoke(this, EventArgs.Empty);
        GetComponent<BuffObjectHandler>().EvaluateBuff(GameFlow.Instance.holdingBuffObject,
            out textSpeed,
            out damageMultiplier,
            out healMultiplier,
            out bulletSpeed,
            out fakeCounterableArgumentsChance,
            out slowMotionEnabled);
    }
    public void SetFullBodySprite(DialogueSO.DialogueLine line) {
        foreach (TrialCharacterVisual characterVisual in characters) {
            if (characterVisual.GetCharacter().characterIndex == line.character.characterIndex) {
                characterVisual.SetSprite(line.fullBodyExpression);
            } else {
                characterVisual.SetSprite(characterVisual.GetCharacter().fullBodySprite);
            }
        }
    }
    public int SetAndGetRandomLanguageIndex() {
        randomLanguageIndex = RandomTextPicker.GetRandomLanguage();
        return randomLanguageIndex;
    }
    public int GetRandomLanguageIndex() {
        return randomLanguageIndex;
    }
    public void StartGame() {
        gameStarted = true;
    }

}
