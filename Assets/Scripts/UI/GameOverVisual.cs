using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameOverVisual : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI resultLabel;
    [SerializeField] Image culpritSprite;
    [SerializeField] TextMeshProUGUI culpritName;
    [SerializeField] Button playAgain;
    [SerializeField] GameObject wrapper;

    // Start is called before the first frame update
    void Start()
    {
        Singleton.Instance.OnGameOver += SetUI;
        playAgain.onClick.AddListener(PlayAgain);
        wrapper.SetActive(false);
    }

    private void SetUI(object sender, EventArgs e)
    {
        wrapper.SetActive(true);
        if (GameFlow.Instance.culprit == null) {
            resultLabel.text = "Que pena! Seus colegas perderam a confiança em você te julgaram como culpado pelo sumiço de Fuyuhiko.";
        } else {
            if (GameFlow.Instance.culprit != null && GameFlow.Instance.culprit.characterIndex == 13) {
                resultLabel.text = "Parabéns! Você acertou o culpado pelo sumiço de Fuyuhiko!";
            } else {
                resultLabel.text = "Que pena! O culpado pelo sumiço de Fuyuhiko não é quem você escolheu.";
            }
            culpritSprite.sprite = GameFlow.Instance.culprit.pixelSprite;
            culpritName.text = GameFlow.Instance.culprit.characterName;
        }
    }
    private void PlayAgain() {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
