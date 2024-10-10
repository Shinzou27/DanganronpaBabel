using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DormRoomVisual : MonoBehaviour
{
    [SerializeField] SpriteRenderer characterPlateSprite;
    [SerializeField] Image characterStandingImage;
    [SerializeField] Sprite questionMark;
    private CharacterSO character;
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if (GameFlow.Instance.Visited(character) || character.characterIndex == 5 || GameFlow.Instance.atFreeTime) {
            characterPlateSprite.sprite = character.pixelSprite;
        } else {
            characterPlateSprite.sprite = questionMark;
        }
    }

    public void SetCharacter(CharacterSO _character) {
        character = _character;
        characterStandingImage.sprite = character.fullBodySprite;
    }


}
