using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [SerializeField] private AudioSource audioSourceBGM;
    [SerializeField] private AudioSource audioSourceSFX;
    [SerializeField] private AudioClip menuBGM;
    [SerializeField] private AudioClip investigationBGM;
    [SerializeField] private AudioClip investigationSuspenseBGM;
    [SerializeField] private AudioClip freeTimeBGM;
    [SerializeField] private AudioClip trialBGM;
    [SerializeField] private AudioClip monokumaLessonBGM;

    [SerializeField] private AudioClip freeTimeSFX;
    [SerializeField] private AudioClip bulletObtainedSFX;
    [SerializeField] private AudioClip buttonShortSFX;
    [SerializeField] private AudioClip buttonLongSFX;
    [SerializeField] private AudioClip characterSelectSFX;
    [SerializeField] private AudioClip earthquakeSFX;
    [SerializeField] private AudioClip doorbellSFX;
    [SerializeField] private AudioClip loadBulletSFX;
    [SerializeField] private AudioClip shootBulletSFX;
    [SerializeField] private AudioClip replaceButtonSFX;
    [SerializeField] private AudioClip rouletteCharacterChangeSFX;
    [SerializeField] private AudioClip rouletteConfirmSFX;
    [SerializeField] private AudioClip doorOpenSFX;
    [SerializeField] private AudioClip doorCloseSFX;

    public static AudioManager Instance { get; private set; }

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
        Singleton.Instance.OnStartGame += PlayInvestigationBGM;
        Singleton.Instance.OnFreeTimeStarts += PlayFreeTimeBGM;
        Singleton.Instance.OnTrialEnter += PlayTrialBGM;
        Singleton.Instance.OnInvestigationSecondPhase += PlayInvestigationSecondPhaseBGM;
        Singleton.Instance.OnGameOver += PlayMonokumaBGM;
        audioSourceBGM.clip = menuBGM;
        audioSourceBGM.Play();
    }


    private void PlayInvestigationBGM(object sender, EventArgs e)
    {
        audioSourceBGM.clip = investigationBGM;
        audioSourceBGM.Play();
    }

    private void PlayInvestigationSecondPhaseBGM(object sender, EventArgs e)
    {
        audioSourceBGM.clip = investigationSuspenseBGM;
        audioSourceBGM.Play();
    }

    private void PlayFreeTimeBGM(object sender, EventArgs e)
    {
        audioSourceBGM.clip = freeTimeBGM;
        audioSourceBGM.Play();
    }

    private void PlayTrialBGM(object sender, EventArgs e)
    {
        audioSourceBGM.clip = trialBGM;
        audioSourceBGM.Play();
    }
    private void PlayMonokumaBGM(object sender, EventArgs e)
    {
        audioSourceBGM.clip = monokumaLessonBGM;
        audioSourceBGM.Play();
    }
    public void PlayBulletObtainedAudio() {
        audioSourceSFX.volume = 0.3f;
        audioSourceSFX.PlayOneShot(bulletObtainedSFX);
    }
    public void PlayFreeTimeStartsAudio() {
        audioSourceSFX.volume = 1f;
        audioSourceSFX.PlayOneShot(freeTimeSFX);
    }
    public void PlayButtonLong() {
        audioSourceSFX.volume = 1f;
        audioSourceSFX.PlayOneShot(buttonLongSFX);
    }
    public void PlayButtonShort() {
        audioSourceSFX.volume = 1f;
        audioSourceSFX.PlayOneShot(buttonShortSFX);
    }
    public void PlayCharacterSelect() {
        audioSourceSFX.volume = 1f;
        audioSourceSFX.PlayOneShot(characterSelectSFX);
    }
    public void PlayEarthquake() {
        audioSourceSFX.volume = 1f;
        audioSourceSFX.PlayOneShot(earthquakeSFX);
    }
    public void PlayDoorbell() {
        audioSourceSFX.volume = 0.6f;
        audioSourceSFX.PlayOneShot(doorbellSFX);
    }
    public void PlayLoadBullet() {
        audioSourceSFX.volume = 1f;
        audioSourceSFX.PlayOneShot(loadBulletSFX);
    }
    public void PlayShootBullet() {
        audioSourceSFX.volume = 1f;
        audioSourceSFX.PlayOneShot(shootBulletSFX);
    }
    public void PlayReplaceButtonClick() {
        audioSourceSFX.volume = 1f;
        audioSourceSFX.PlayOneShot(replaceButtonSFX);
    }
    public void PlayRouletteChangeCharacter() {
        audioSourceSFX.volume = 1f;
        audioSourceSFX.PlayOneShot(rouletteCharacterChangeSFX);
    }
    public void PlayRouletteConfirm() {
        audioSourceSFX.volume = 1f;
        audioSourceSFX.PlayOneShot(rouletteConfirmSFX);
    }
    public void PlayDoorOpen() {
        audioSourceSFX.volume = 1f;
        audioSourceSFX.PlayOneShot(doorOpenSFX);
    }
    public void PlayDoorClose() {
        audioSourceSFX.volume = 1f;
        audioSourceSFX.PlayOneShot(doorCloseSFX);
    }
}
