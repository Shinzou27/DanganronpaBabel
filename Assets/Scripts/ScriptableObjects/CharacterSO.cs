using UnityEngine;

[CreateAssetMenu(fileName = "CharacterSO", menuName = "CharacterSO", order = 0)]
public class CharacterSO : ScriptableObject {
    public string characterName;
    public int characterIndex;
    public Sprite pixelSprite;
    public Sprite fullBodySprite;
    public GameObject buffObject;
    public DialogueSO freeTimeDialogue;
}