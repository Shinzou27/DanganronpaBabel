using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;
using System;
using UnityEngine.XR;

public class ArgumentSpawner : MonoBehaviour
{
    [SerializeField] private List<GameObject> trialCharacters;
    [SerializeField] private GameObject argument;
    [SerializeField] private GameObject argumentLegacy;
    [SerializeField] private TextMeshProUGUI argumentLabel;
    [SerializeField] private Text argumentLegacyLabel;
    [SerializeField] private Button argumentClick;
    [SerializeField] private Button argumentLegacyClick;
    [SerializeField] private TrialMenuVisual menu;
    [SerializeField] private TrialGround trialGround;
    [SerializeField] private XRNode inputSource;
    private int argumentIndex;
    public bool argumentAlreadyShot;
    public bool animating;
    public float animationTimer;
    public Vector3 moveDirection;
    private float frailArgumentWarningTimer;
    private float frailArgumentWarningTimerDuration;
    private enum ActiveTextGameObject {
        TextMeshPro,
        Legacy
    }
    private ActiveTextGameObject activeText;
    private GameLanguage argumentLanguage;

    void Start()
    {
        // argumentClick.onClick.AddListener(FireBullet);
        // argumentLegacyClick.onClick.AddListener(FireBullet);
        SpawnRandomText();
        Singleton.Instance.OnCulpritSelection += HideArguments;
        Singleton.Instance.OnBulletShot += FireBullet;
        frailArgumentWarningTimerDuration = trialGround.animationDuration;
    }

    private void Update() {
        if (!trialGround.gameStarted) {
            argument.SetActive(false);
            argumentLegacy.SetActive(false);
        }
        if (Singleton.Instance.onTrial && trialGround.gameStarted && !menu.OnPause()) {
            if (animating) {
                animationTimer += Time.deltaTime;
                if (animationTimer > trialGround.animationDuration) {
                    NextLine();
                } else {
                    InputDevice device = InputDevices.GetDeviceAtXRNode(inputSource);
                    device.TryGetFeatureValue(CommonUsages.secondaryButton, out bool touchedSlowMotionButton);
                    device.TryGetFeatureValue(CommonUsages.primaryButton, out bool touchedFastForwardButton);
                    GameObject textToMove;
                    if (activeText == ActiveTextGameObject.TextMeshPro) {
                        argumentLabel.gameObject.SetActive(true);
                        argumentLegacyLabel.gameObject.SetActive(false);
                        textToMove = argument;
                    } else {
                        argumentLabel.gameObject.SetActive(false);
                        argumentLegacyLabel.gameObject.SetActive(true);
                        textToMove = argumentLegacy;
                    }
                    if (touchedSlowMotionButton && trialGround.slowMotionEnabled) {
                        textToMove.transform.position += trialGround.textSpeed * trialGround.slowMotionMultiplier * Time.deltaTime * moveDirection;
                    } else if (touchedFastForwardButton) {
                        animationTimer += 2 * Time.deltaTime;
                        textToMove.transform.position += trialGround.textSpeed * trialGround.fastForwardMultiplier * Time.deltaTime * moveDirection;
                    } else {
                        textToMove.transform.position += trialGround.textSpeed * Time.deltaTime * moveDirection;
                    }
                }
            } else if (menu.OnFrailArgumentWarning()) {
                frailArgumentWarningTimer += Time.deltaTime;
                if (frailArgumentWarningTimer >= frailArgumentWarningTimerDuration) {
                    HideFrailArgumentWarning();
                    frailArgumentWarningTimer = 0;
                }
            }
        }
    }
    void SpawnRandomText()
    {
        animationTimer = 0f;
        argumentAlreadyShot = false;
        // Escolher um canto aleatório
        Transform characterTransform = GetCharacterTransform(trialGround.trialDialogue.lines[argumentIndex].character);
        Vector3 spawnPosition = new(characterTransform.position.x, characterTransform.position.y+0.4f, characterTransform.position.z);
        System.Random random = new();
        SetText(trialGround.trialDialogue.lines[argumentIndex].texts[trialGround.SetAndGetRandomLanguageIndex()]);
        if (IsCounterable(argumentIndex) || (random.Next(100) < trialGround.fakeCounterableArgumentsChance)) {
            argumentLabel.color = Color.yellow;
            argumentLegacyLabel.color = Color.yellow;
        } else {
            argumentLabel.color = Color.white;
            argumentLegacyLabel.color = Color.white;
        }
        // Definir um ângulo aleatório de movimento
        // float angle = (characterTransform.eulerAngles.y + 270 ) % 360;
        float angle = characterTransform.eulerAngles.y;
        // moveDirection = new Vector3(Mathf.Cos(angle * Mathf.Deg2Rad), 0, Mathf.Sin(angle * Mathf.Deg2Rad));
        moveDirection = (GetMoveDirection(characterTransform) - characterTransform.position).normalized;
        argument.transform.position = spawnPosition;
        argumentLegacy.transform.position = spawnPosition;
        menu.SetSpeakingCharacter(trialGround.trialDialogue.lines[argumentIndex]);
        trialGround.SetFullBodySprite(trialGround.trialDialogue.lines[argumentIndex]);
        animating = true;
    }
    private Transform GetCharacterTransform(CharacterSO _character) {
        foreach (GameObject trialCharacter in trialCharacters) {
            if (trialCharacter.GetComponent<TrialCharacterVisual>().GetCharacter() == _character) {
                return trialCharacter.GetComponent<TrialCharacterVisual>().GetImageTransform();
            }
        }
        return null;
    }
    private Vector3 GetMoveDirection(Transform startCharacterTransform) {
        foreach (GameObject trialCharacter in trialCharacters) {
            Transform characterTransform = GetCharacterTransform(trialCharacter.GetComponent<TrialCharacterVisual>().GetCharacter());
            float charAngle = (characterTransform.eulerAngles.y +180) % 360;
            float startCharAngle = startCharacterTransform.eulerAngles.y;
            if (FloatEquals(charAngle, startCharAngle)) {
                return characterTransform.position;
            }
        }
        return Vector3.zero;
    }
    private void NextLine() {
        if (argumentIndex < trialGround.trialDialogue.lines.Count - 1) {
            if (IsCounterable(argumentIndex) && !AlreadyShot(argumentIndex)) {
                argumentIndex -= 2;
                ShowFrailArgumentWarning();
            } else {
                argumentIndex++;
                SpawnRandomText();
            }
        } else {
            Singleton.Instance.OnCulpritSelection?.Invoke(this, EventArgs.Empty);
            animating = false;
        }
    }
    private bool FloatEquals(float a, float b, float epsilon = 0.001f)
    {
        return Mathf.Abs(a - b) < epsilon;
    }
    private bool IsCounterable(int index) {
        foreach (TrialGround.CounterArgument counterArgument in trialGround.counterArguments) {
            if (counterArgument.argumentIndex == index) {
                return true;
            }
        }
        return false;
    }
    private void SetLastArgumentWon(int index) {
        foreach (TrialGround.CounterArgument counterArgument in trialGround.counterArguments) {
            if (counterArgument.argumentIndex == index) {
                trialGround.lastArgumentWon = counterArgument;
            }
        }
    }
    private bool AlreadyShot(int index) {
        foreach (TrialGround.CounterArgument counterArgument in trialGround.counterArguments) {
            if (counterArgument.argumentIndex == index) {
                return counterArgument.shot;
            }
        }
        return false;
    }
    private bool IsCounterable(int index, out BulletSO bullet) {
        foreach (TrialGround.CounterArgument counterArgument in trialGround.counterArguments) {
            if (counterArgument.argumentIndex == index){
                bullet = counterArgument.bullet;
                return true;
            }
        }
        bullet = null;
        return false;
    }
    private void FireBullet(object sender, EventArgs e) {
        if (!menu.OnPause()) {
            if (!argumentAlreadyShot) {
                if (!IsCounterable(argumentIndex)) {
                    GameFlow.Instance.Damage((int)(6 * trialGround.damageMultiplier));
                    menu.UpdateHPBarVisual();
                    menu.PlayDamageAnimation();
                    argumentIndex = trialGround.lastArgumentWon.bullet?.title != null ? argumentIndex - 2 : 0;
                } else if (IsCounterable(argumentIndex, out BulletSO bullet)) {
                    if (bullet != menu.GetLoadedBullet()) {
                        GameFlow.Instance.Damage((int)(12 *trialGround.damageMultiplier));
                        menu.UpdateHPBarVisual();
                        argumentIndex = trialGround.lastArgumentWon.bullet?.title != null ? argumentIndex - 2 : 0;
                        menu.PlayDamageAnimation();
                    } else {
                        GameFlow.Instance.Heal((int)(10 * trialGround.healMultiplier));
                        menu.UpdateHPBarVisual();
                        menu.PlayCounterAnimation();
                        trialGround.counterArguments.Find((ca) => ca.argumentIndex == argumentIndex).shot = true;
                        SetLastArgumentWon(argumentIndex);
                    }
                }
            }
            argumentAlreadyShot = true;
        }
    }
    private void SetText(DialogueSO.DialogueText text) {
        argumentLanguage = text.language;
        if (argumentLanguage == GameLanguage.Japanese) {
            activeText = ActiveTextGameObject.Legacy;
            argumentLegacyLabel.text = text.text;
        } else {
            argumentLabel.text = text.text;
            activeText = ActiveTextGameObject.TextMeshPro;
        }
    }
    private void HideArguments(object sender, EventArgs e)
    {
        argument.SetActive(false);
        argumentLegacy.SetActive(false);
    }
    private void ShowFrailArgumentWarning() {
        menu.ShowFrailArgumentWarning();
        animating = false;
        GameFlow.Instance.Damage(10);
        menu.UpdateHPBarVisual();
    }
    private void HideFrailArgumentWarning() {
        menu.HideFrailArgumentWarning();
        animating = true;
    }
}