using System;
using System.Collections.Generic;
using UnityEngine;

public class Singleton : MonoBehaviour
{
    public class DormRoomEventArgs {
        public DormRoom dormRoom;
    }
    public class CharacterEventArgs {
        public CharacterSO character;
    }
    public class ReplaceRoomEventArgs {
        public CharacterSO current;
        public CharacterSO toReplace;
    }

    public EventHandler<DormRoomEventArgs> OnDormRoomEntered;
    public EventHandler<DormRoomEventArgs> OnDormRoomExited;
    public EventHandler<DormRoomEventArgs> OnWrongDormRoom;
    public EventHandler<DormRoomEventArgs> OnWrongDormRoomExited;
    public EventHandler<ReplaceRoomEventArgs> OnDormRoomReplace;
    public EventHandler<CharacterEventArgs> OnCulpritSelect;
    public EventHandler OnStartGame;
    public EventHandler OnCharacterVisited;
    public EventHandler OnGameOver;
    public EventHandler OnNekomaruDialogue;
    public EventHandler OnNagitoDialogue;
    public EventHandler OnInvestigationSecondPhase;
    public EventHandler OnBloodAnalysis;
    public EventHandler OnFreeTimeStarts;
    public EventHandler OnFreeTimeAlertShow;
    public EventHandler OnFreeTimeEnds;
    public EventHandler OnLeavingFreeTime;
    public EventHandler OnTrialEnter;
    public EventHandler OnTrialStarts;
    public EventHandler OnBulletShot;
    public EventHandler OnCulpritSelection;
    public static Singleton Instance { get; private set; }
    public bool onDialogue;
    public bool onTrial;
    public bool updatingList;
    public bool replacedHajimeRoom;
    public bool onWheelActive;
    public GameLanguage nativeLanguage;
    public List<GameLanguage> selectedLanguages;
    public List<DormRoom> dormRooms;
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
}
