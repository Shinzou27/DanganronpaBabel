using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HandbookVisual : MonoBehaviour
{
    [SerializeField] List<Image> pixelSprites;
    [SerializeField] Canvas canvas;
    [SerializeField] Button freeTimeDismiss;
    [SerializeField] Button goToMapButton;
    [SerializeField] Button goToBulletsButton;
    [SerializeField] Button showGoalTab;
    [SerializeField] Button hideGoalTab;
    [SerializeField] TextMeshProUGUI goalDescription;
    [SerializeField] GameObject mapWrapper;
    [SerializeField] GameObject bulletsWrapper;
    [SerializeField] GameObject freeTimeModalWrapper;
    [SerializeField] GameObject goalTabWrapper;
    [SerializeField] Sprite greenBullet;
    [SerializeField] Sprite redBullet;
    [SerializeField] Sprite questionMark;
    [SerializeField] GameObject playerDot;
    [SerializeField] GameObject fuyuhikoRoomReplaceButton;
    [SerializeField] GameObject hajimeRoomReplaceButton;
    [SerializeField] DormRoomReplacerVisual dormRoomReplacer;
    private List<DormRoom> rooms;
    // [SerializeField] private List<BulletCardVisual> bulletCards;

    private enum Page {
        Map,
        Bullets,
        FreeTime
    }
    private Page currentPage = Page.Bullets;
    // Start is called before the first frame update
    void Start()
    {
        canvas.worldCamera = Camera.main;
        ToggleButtonView();
        if (GameFlow.Instance.atFreeTime) {
            SetFreeTimeModalVisibility();
        }
        goToMapButton.onClick.AddListener(ChangePage);
        goToBulletsButton.onClick.AddListener(ChangePage);
        freeTimeDismiss.onClick.AddListener(ChangePage);
        fuyuhikoRoomReplaceButton.GetComponent<Button>().onClick.AddListener(() => ReplaceGuest(DormRoomReplacerVisual.GuestToReplace.Fuyuhiko));
        hajimeRoomReplaceButton.GetComponent<Button>().onClick.AddListener(() => ReplaceGuest(DormRoomReplacerVisual.GuestToReplace.Hajime));
        Singleton.Instance.OnFreeTimeStarts += SetFreeTimeModalVisibility;
        Singleton.Instance.OnFreeTimeEnds += ActivateGoalTab;
        Singleton.Instance.OnTrialEnter += DisableMap;
        showGoalTab.onClick.AddListener(ShowGoal);
        hideGoalTab.onClick.AddListener(HideGoal);
        // Singleton.Instance.OnDormRoomEntered += SetSprites;
        hideGoalTab.gameObject.SetActive(false);
    }

    private void ChangePage()
    {
        if (currentPage == Page.Map) {
            currentPage = Page.Bullets;
        } else if (currentPage == Page.Bullets) {
            currentPage = Page.Map;
        }
        ToggleButtonView();
    }
    private void OnDestroy() {
        Singleton.Instance.OnFreeTimeStarts -= SetFreeTimeModalVisibility;
        Singleton.Instance.OnFreeTimeEnds -= ActivateGoalTab;
        Singleton.Instance.OnTrialEnter -= DisableMap;

        // Singleton.Instance.OnDormRoomEntered -= SetSprites;
    }
    // Update is called once per frame
    void Update()
    {
        if (rooms != null) {
            SetSprites();
            playerDot.transform.localPosition = PlayerPositionVector.Instance.CalculateMapPosition();
            // fuyuhikoRoomReplaceButton.SetActive(!GameFlow.Instance.inHouse && GameFlow.Instance.Visited(4) && !GameFlow.Instance.atFreeTime);
            fuyuhikoRoomReplaceButton.SetActive(false);
            hajimeRoomReplaceButton.SetActive(!(GameFlow.Instance.inHouse || Singleton.Instance.replacedHajimeRoom || GameFlow.Instance.atFreeTime));
        } else {
            rooms = Singleton.Instance.dormRooms;
        }
    }
    void SetSprites(object sender = null, Singleton.DormRoomEventArgs e = null) {
        int index = 0;
        foreach (DormRoom room in Singleton.Instance.dormRooms) {
            if (
                GameFlow.Instance.Visited(room.GetCharacter()) ||
                room.GetCharacter().characterIndex == 5 ||
                // room.GetCharacter().characterIndex == 7 || //Retirar depois
                GameFlow.Instance.atFreeTime) {
                pixelSprites[index].sprite = room.GetCharacter().pixelSprite;
                if (room.GetCharacter().characterIndex == 5) {
                    hajimeRoomReplaceButton.transform.localPosition = new(0, pixelSprites[index].gameObject.transform.localPosition.y, pixelSprites[index].gameObject.transform.localPosition.z);
                }
                if (room.GetCharacter().characterIndex == 3 && GameFlow.Instance.Visited(room.GetCharacter())) {
                    fuyuhikoRoomReplaceButton.transform.localPosition = new(pixelSprites[index].gameObject.transform.localPosition.x + 0.02f, pixelSprites[index].gameObject.transform.localPosition.y, pixelSprites[index].gameObject.transform.localPosition.z);
                }
            } else {
                pixelSprites[index].sprite = questionMark;
            }
            index++;
        }
    }
    public void ToggleButtonView() {
        if (currentPage == Page.Map) {
            goToMapButton.gameObject.SetActive(false);
            goToBulletsButton.gameObject.SetActive(true);
            freeTimeModalWrapper.SetActive(false);
            dormRoomReplacer.gameObject.SetActive(false);
            mapWrapper.SetActive(false);
            bulletsWrapper.SetActive(true);
            Singleton.Instance.onWheelActive = false;
        } else if (currentPage == Page.Bullets) {
            freeTimeModalWrapper.SetActive(false);
            dormRoomReplacer.gameObject.SetActive(false);
            goToBulletsButton.gameObject.SetActive(false);
            goToMapButton.gameObject.SetActive(true);
            mapWrapper.SetActive(true);
            bulletsWrapper.SetActive(false);
        } else {
            currentPage = Page.Bullets;
            ToggleButtonView();
        }
    }
    private void SetFreeTimeModalVisibility(object sender = null, EventArgs e = null) {
        currentPage = Page.FreeTime;
        if (!goToMapButton.IsDestroyed()) {
            goToMapButton.gameObject.SetActive(false);
            goToBulletsButton.gameObject.SetActive(false);
            goalTabWrapper.SetActive(false);
            freeTimeModalWrapper.SetActive(true);
            mapWrapper.SetActive(false);
            bulletsWrapper.SetActive(false);
        }
    }
    private void ActivateGoalTab(object sender = null, EventArgs e = null) {
        goalTabWrapper.SetActive(true);
    }
    private void ReplaceGuest(DormRoomReplacerVisual.GuestToReplace guest) {
        AudioManager.Instance.PlayReplaceButtonClick();
        dormRoomReplacer.SetGuestToReplace(guest);
        mapWrapper.SetActive(false);
        bulletsWrapper.SetActive(false);
        dormRoomReplacer.gameObject.SetActive(true);
        Singleton.Instance.onWheelActive = true;
    }
    private void ShowGoal() {
        showGoalTab.gameObject.SetActive(false);
        hideGoalTab.gameObject.SetActive(true);
        goalDescription.text = GameFlow.Instance.GetCurrentGoal();
        AudioManager.Instance.PlayReplaceButtonClick();
    }
    private void HideGoal() {
        hideGoalTab.gameObject.SetActive(false);
        showGoalTab.gameObject.SetActive(true);
        AudioManager.Instance.PlayReplaceButtonClick();
    }
    private void DisableMap(object sender, EventArgs e)
    {
        goToMapButton.gameObject.SetActive(false);
        goToBulletsButton.gameObject.SetActive(false);
        freeTimeModalWrapper.SetActive(false);
        dormRoomReplacer.gameObject.SetActive(false);
        mapWrapper.SetActive(false);
        bulletsWrapper.SetActive(true);
    }
}
