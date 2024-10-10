using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR;

public class DormRoomReplacerVisual : MonoBehaviour
{
    [SerializeField] private Image roulette;
    [SerializeField] private Image selectedImage;
    [SerializeField] private List<CharacterSO> characters;
    [SerializeField] private CharacterSO selectedCharacter;
    [SerializeField] private CharacterSO currentCharacter;
    [SerializeField] private CharacterSO characterToReplace;
    [SerializeField] private TextMeshProUGUI selectedName;
    [SerializeField] private Button confirmButton;
    [SerializeField] private XRNode inputSource;
    private Vector2 inputAxis;
    private readonly int rotateSpeed = 150;
    private int rouletteIndex;
    public enum GuestToReplace {
        Fuyuhiko,
        Hajime
    }
    private GuestToReplace guestToReplace;
    // Start is called before the first frame update
    void Start()
    {
        confirmButton.onClick.AddListener(ChangeRooms);
        rouletteIndex = GetWheelIndex(rouletteIndex);
        characterToReplace = characters[rouletteIndex];
        selectedImage.sprite = characterToReplace.pixelSprite;
        selectedName.text = characterToReplace.characterName;
    }


    // Update is called once per frame
    void Update()
    {
        InputDevice device = InputDevices.GetDeviceAtXRNode(inputSource);
        device.TryGetFeatureValue(CommonUsages.primary2DAxis, out inputAxis);
        if (Mathf.Abs(inputAxis.x) > 0.5f) {
            roulette.transform.Rotate(new(0, 0, rotateSpeed * inputAxis.x * Time.deltaTime));
            UpdateCharacter();
        }
    }
    private void UpdateCharacter() {
        int newIndex = GetWheelIndex(roulette.transform.localEulerAngles.z);
        if (newIndex != rouletteIndex) {
            AudioManager.Instance.PlayRouletteChangeCharacter();
        }
        rouletteIndex = newIndex;
        characterToReplace = characters[rouletteIndex];
        selectedImage.sprite = characterToReplace.pixelSprite;
        selectedName.text = characterToReplace.characterName;
    }
    public int GetWheelIndex(float angle)
    {
        // Normalize the angle to be within [0, 360)
        angle = -angle;
        angle %= 360;
        if (angle < 0)
        {
            angle += 360;
        }

        // Define the size of each segment in degrees
        float segmentSize = 22.5f;

        // Calculate the index by dividing the angle by the segment size
        int index = Mathf.FloorToInt((angle + segmentSize / 2) / segmentSize);
        // Ensure the index is within the valid range [0, 15]
        if (index >= 16)
        {
            index = 0;
        }
        return index;
    }

    private void ChangeRooms()
    {
        AudioManager.Instance.PlayRouletteConfirm();
        Singleton.Instance.OnDormRoomReplace?.Invoke(this, new Singleton.ReplaceRoomEventArgs {
            toReplace = characterToReplace,
            current = currentCharacter
        });
        if (guestToReplace == GuestToReplace.Hajime) {
            Singleton.Instance.replacedHajimeRoom = true;
        }
        GetComponentInParent<HandbookVisual>().ToggleButtonView();
        Singleton.Instance.onWheelActive = false;
    }
    public void SetGuestToReplace(GuestToReplace guest) {
        if (guest == GuestToReplace.Fuyuhiko) {
            currentCharacter = characters[12];
        } else if (guest == GuestToReplace.Hajime) {
            currentCharacter = characters[8];
        }
    }
}
