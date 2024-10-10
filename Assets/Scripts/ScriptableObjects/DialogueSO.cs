using UnityEngine;
using System.Collections.Generic;
using System;

[CreateAssetMenu(fileName = "DialogueSO", menuName = "DialogueSO", order = 0)]
public class DialogueSO : ScriptableObject {
    [Serializable]
    public class DialogueText {
        public string text;
        public GameLanguage language;
    }

    [Serializable]
    public struct DialogueLine {
        public CharacterSO character;
        public List<DialogueText> texts;
        public Sprite halfBodyExpression;
        public Sprite fullBodyExpression;
    }
    public List<DialogueLine> lines;
    
    public string ExportToJson()
    {
        return JsonUtility.ToJson(this, true);
    }
    public void ImportFromJson(string json)
    {
        JsonUtility.FromJsonOverwrite(json, this);
    }
}