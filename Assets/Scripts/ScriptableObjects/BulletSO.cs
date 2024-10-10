using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "BulletSO", menuName = "BulletSO", order = 0)]
public class BulletSO : ScriptableObject {
    public string title;
    public List<BulletText> texts;
    public bool found;
    [Serializable]
    public class BulletText {
        public string text;
        public GameLanguage language;
    }
    public DialogueSO triggerDialogue;
}