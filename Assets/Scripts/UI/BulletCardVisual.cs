using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BulletCardVisual : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI title;
    [SerializeField] TextMeshProUGUI description;
    [SerializeField] Image bulletSprite;
    [SerializeField] Sprite greenBullet;
    [SerializeField] Sprite redBullet;
    [SerializeField] BulletSO bullet;
    public void SetInfo(string _title, string _description, Sprite _bullet) {
        title.text = _title;
        description.text = _description;
        bulletSprite.sprite = _bullet;
    }
    void Update() {
        if (bullet.found) {
            SetInfo(bullet.title, bullet.texts[0].text, greenBullet);
            title.color = Color.white;
        } else {
            SetInfo("? ? ?", "Bala da verdade ainda não descoberta. Continue a investigação para adquiri-la.", redBullet);
            title.color = Color.red;
        }
    }
}
