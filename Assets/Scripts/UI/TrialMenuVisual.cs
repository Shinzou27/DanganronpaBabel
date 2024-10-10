using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit.UI;

public class TrialMenuVisual : MonoBehaviour
{
    [SerializeField] private List<BulletSO> bullets;
    private int loadedBulletIndex;
    [SerializeField] private DialogueSO trial;

    [SerializeField] private GameObject hpWrapper;
    [SerializeField] private Image hpBar;
    [SerializeField] private TextMeshProUGUI hpLabel;

    [SerializeField] private Image barrel;
    [SerializeField] private Image speakingCharacter;
    [SerializeField] private TextMeshProUGUI speakingCharacterName;
    [SerializeField] private TextMeshProUGUI speakingCharacterDialogueText;
    [SerializeField] private Text speakingCharacterDialogueTextLegacy;

    [SerializeField] private TextMeshProUGUI upperBullet;
    [SerializeField] private TextMeshProUGUI centerBullet;
    [SerializeField] private TextMeshProUGUI bottomBullet;
    [SerializeField] private XRNode inputSource;

    [SerializeField] private GameObject bulletSelection;
    [SerializeField] private GameObject speakingCharacterWrapper;
    [SerializeField] private GameObject prepareScreen;
    [SerializeField] private GameObject prepareConfirmLabel;
    [SerializeField] private Button prepareButton;
    [SerializeField] private Button nextTutorialButton;
    [SerializeField] private Image prepareTutorial;
    [SerializeField] private TextMeshProUGUI prepareCountdown;
    [SerializeField] private GameObject frailArgumentWarning;
    [SerializeField] private Image frailArgumentWarningBar;
    [SerializeField] private GameObject hajimeCounter;
    [SerializeField] private GameObject pauseScreen;
    [SerializeField] private Button pauseButton;
    [SerializeField] private Button resumeButton;
    [SerializeField] private GameObject culpritSelection;
    [SerializeField] private GameObject culpritSelectionAlert;
    [SerializeField] private GameObject culpritSelectionView;
    [SerializeField] private TextMeshProUGUI culpritName;
    [SerializeField] private Button culpritSelectionConfirmButton;
    [SerializeField] private TrialGround trialGround;
    [SerializeField] private Sprite[] tutorialImages;
    private int tutorialImagesIndex;

    private float hajimeCounterTimerMax = 2f;
    private float hajimeCounterTimer;
    private Vector2 inputAxis;
    private Animator animator;
    private float loadCooldown = 0.5f;
    private float loadTimer;
    private bool loadTouched;
    private float shotCooldown = 1f;
    private float shotTimer;
    private bool shotTouched;
    private float barrelAnimationDuration = 0.5f;
    private float frailArgumentWarningTimer;
    private float frailArgumentWarningTimerDuration;
    private bool onCountdown;
    private float countdownTimer;
    private float countdownTimerMax = 5f;
    private bool onCulpritSelect;
    public bool pause;

    // Start is called before the first frame update
    void Start()
    {
        GetComponent<Canvas>().worldCamera = Camera.main;
        animator = GetComponent<Animator>();
        loadedBulletIndex = 0;
        centerBullet.text = bullets[loadedBulletIndex].title;
        upperBullet.text = bullets[^1].title;
        bottomBullet.text = bullets[loadedBulletIndex+1].title;
        SetInitialState();
        Singleton.Instance.OnTrialEnter += ShowPrepareScreen;
        Singleton.Instance.OnCulpritSelection += SetCulpritSelectionUI;
        Singleton.Instance.OnCulpritSelect += SetCulprit;
        culpritSelectionConfirmButton.onClick.AddListener(EndGame);
        prepareButton.onClick.AddListener(StartCountdown);
        nextTutorialButton.onClick.AddListener(NextTutorial);
        pauseButton.onClick.AddListener(TogglePauseState);
        resumeButton.onClick.AddListener(TogglePauseState);
        frailArgumentWarningTimerDuration = trialGround.animationDuration;
    }

    // Update is called once per frame
    void Update()
    {
        GetComponent<TrackedDeviceGraphicRaycaster>().enabled = Singleton.Instance.onTrial;
        if (Singleton.Instance.onTrial) {
            if (hajimeCounter.activeSelf) {
                hajimeCounterTimer += Time.deltaTime;
                if (hajimeCounterTimer >= hajimeCounterTimerMax) {
                    hajimeCounterTimer = 0;
                    hajimeCounter.SetActive(false);
                }
            }
            InputDevice device = InputDevices.GetDeviceAtXRNode(inputSource);
            device.TryGetFeatureValue(CommonUsages.primary2DAxis, out inputAxis);
            hpLabel.text = GameFlow.Instance.HP.ToString();

            if (shotTouched) {
                shotTimer += Time.deltaTime;
                if (shotTimer > shotCooldown) {
                    shotTimer = 0f;
                    shotTouched = false;
                }
            }
            if (Mathf.Abs(inputAxis.y) > 0.8f && !loadTouched) {
                loadTouched = true;
                UpdateLoadedBullet(inputAxis.y < 0);
            }
            if (loadTouched) {
                loadTimer += Time.deltaTime;
                if (loadTimer >= loadCooldown) {
                    loadTimer = 0f;
                    loadTouched = false;
                }
            }
            if (onCountdown) {
                countdownTimer += Time.deltaTime;
                prepareCountdown.text = (5 - (int)countdownTimer).ToString();
                if (countdownTimer >= countdownTimerMax) {
                    onCountdown = false;
                    trialGround.StartGame();
                    ShowTrialUI();
                }
            }
            if (frailArgumentWarning.activeSelf) {
                frailArgumentWarningTimer += Time.deltaTime;
                if (frailArgumentWarningTimer >= frailArgumentWarningTimerDuration) {
                    frailArgumentWarningTimer = 0f;
                }
                frailArgumentWarningBar.fillAmount = 1 - (frailArgumentWarningTimer / frailArgumentWarningTimerDuration);
            }
            if (onCulpritSelect) {
                culpritSelectionAlert.SetActive(GameFlow.Instance.culprit == null);
                culpritSelectionView.SetActive(GameFlow.Instance.culprit != null);
            }
        }
    }
    public void UpdateLoadedBullet(bool clockwise = false) {
        Debug.Log("Antigo índice e sua bala: " + loadedBulletIndex + " | " + bullets[loadedBulletIndex].title);
        AudioManager.Instance.PlayLoadBullet();
        loadedBulletIndex = clockwise ? GetBulletIndex(loadedBulletIndex + 1) : GetBulletIndex(loadedBulletIndex - 1);
        int centerIndex = loadedBulletIndex;
        int upperIndex = GetBulletIndex(centerIndex - 1);
        int bottomIndex = GetBulletIndex(centerIndex + 1);
        centerBullet.text = bullets[centerIndex].title;
        upperBullet.text = bullets[upperIndex].title;
        bottomBullet.text = bullets[bottomIndex].title;
        Debug.Log("Novo índice e sua bala: " + loadedBulletIndex + " | " + bullets[loadedBulletIndex].title);
        Debug.Log("bala no meio: " + centerBullet.text);
        StartCoroutine(RotateImageCoroutine(clockwise));
    }
    private int GetBulletIndex(int index) {
        return ((index % bullets.Count) + bullets.Count) % bullets.Count;
    }

    private IEnumerator RotateImageCoroutine(bool clockwise)
    {
        float targetAngle = 60f * (clockwise ? -1 : 1);
        Vector3 initialRotation = barrel.rectTransform.eulerAngles;
        Vector3 finalRotation = initialRotation + new Vector3(0, 0, targetAngle);

        float elapsed = 0f;
        while (elapsed < barrelAnimationDuration) {
        {
            elapsed += Time.deltaTime;
            Vector3 currentRotation = Vector3.Lerp(initialRotation, finalRotation, elapsed / barrelAnimationDuration);            barrel.rectTransform.rotation = Quaternion.Euler(currentRotation);
            yield return null;
        }
        
            // Garantir que a rotação final seja exatamente a esperada
            barrel.rectTransform.rotation = Quaternion.Euler(finalRotation);
        }
    }
    public void SetSpeakingCharacter(DialogueSO.DialogueLine dialogueLine) {
        speakingCharacterName.text = dialogueLine.character.characterName;
        speakingCharacter.sprite = dialogueLine.halfBodyExpression;
        SetText(dialogueLine.texts[trialGround.GetRandomLanguageIndex()]);
    }
    public BulletSO GetLoadedBullet() {
        return bullets[loadedBulletIndex];
    }
    public void PlayDamageAnimation() {
        animator.SetTrigger("Damage");
    }
    public void PlayCounterAnimation() {
        hajimeCounter.SetActive(true);
    }
    private void SetText(DialogueSO.DialogueText text) {
        if (text.language == GameLanguage.Japanese) {
            speakingCharacterDialogueText.gameObject.SetActive(false);
            speakingCharacterDialogueTextLegacy.gameObject.SetActive(true);
            speakingCharacterDialogueTextLegacy.text = text.text;
        } else {
            speakingCharacterDialogueText.gameObject.SetActive(true);
            speakingCharacterDialogueTextLegacy.gameObject.SetActive(false);
            speakingCharacterDialogueText.text = text.text;
        }
    }
    private void SetCulpritSelectionUI(object sender, EventArgs e) {
        hpWrapper.SetActive(false);
        bulletSelection.SetActive(false);
        speakingCharacterWrapper.SetActive(false);
        prepareScreen.SetActive(false);
        hajimeCounter.SetActive(false);
        culpritSelection.SetActive(true);
        onCulpritSelect = true;
    }
    
    private void SetCulprit(object sender, Singleton.CharacterEventArgs e)
    {
        culpritName.text = e.character.characterName;
    }
    private void EndGame() {
        Singleton.Instance.onTrial = false;
        Singleton.Instance.OnGameOver?.Invoke(this, EventArgs.Empty);
    }
    private void SetInitialState() {
        hpWrapper.SetActive(false);
        bulletSelection.SetActive(false);
        speakingCharacterWrapper.SetActive(false);
        prepareScreen.SetActive(false);
        prepareConfirmLabel.SetActive(false);
        prepareButton.gameObject.SetActive(false);
        prepareCountdown.gameObject.SetActive(false);
        hajimeCounter.SetActive(false);
        frailArgumentWarning.SetActive(false);
        culpritSelection.SetActive(false);
        pauseScreen.SetActive(false);
        NextTutorial();
    }
    private void ShowPrepareScreen(object sender, EventArgs e)
    {
        prepareScreen.SetActive(true);
    }
    private void NextTutorial() {
        if (tutorialImagesIndex >= tutorialImages.Length) {
            prepareTutorial.gameObject.SetActive(false);
            nextTutorialButton.gameObject.SetActive(false);
            prepareButton.gameObject.SetActive(true);
            prepareConfirmLabel.SetActive(true);
        } else {
            prepareTutorial.sprite = tutorialImages[tutorialImagesIndex];
            tutorialImagesIndex++;
        }
        AudioManager.Instance.PlayButtonLong();
    }
    private void StartCountdown() {
        onCountdown = true;
        prepareButton.gameObject.SetActive(false);
        prepareCountdown.gameObject.SetActive(true);
        AudioManager.Instance.PlayButtonLong();
    }
    private void ShowTrialUI() {
        hpWrapper.SetActive(true);
        bulletSelection.SetActive(true);
        speakingCharacterWrapper.SetActive(true);
        prepareScreen.SetActive(false);
    }
    public void ShowFrailArgumentWarning() {
        hpWrapper.SetActive(false);
        bulletSelection.SetActive(false);
        speakingCharacterWrapper.SetActive(false);
        frailArgumentWarning.SetActive(true);
    }
    public void HideFrailArgumentWarning() {
        hpWrapper.SetActive(true);
        bulletSelection.SetActive(true);
        speakingCharacterWrapper.SetActive(true);
        frailArgumentWarning.SetActive(false);
    }
    public bool OnFrailArgumentWarning() {
        return frailArgumentWarning.activeSelf;
    }
    public void UpdateHPBarVisual() {
        hpBar.fillAmount = GameFlow.Instance.HP / 100f;
        Debug.Log(GameFlow.Instance.HP / 100);
        if (GameFlow.Instance.HP <= 33) {
            hpBar.color = Color.red;
        } else {
            hpBar.color = Color.green;
        }
    }
    private void TogglePauseState() {
        pause = !pause;
        pauseScreen.SetActive(pause);
        hpWrapper.SetActive(!pause);
        bulletSelection.SetActive(!pause);
        speakingCharacterWrapper.SetActive(!pause);
    }
    public bool OnPause() {
        return pause;
    }
}
