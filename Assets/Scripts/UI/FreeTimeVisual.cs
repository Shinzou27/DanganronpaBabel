using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FreeTimeVisual : MonoBehaviour
{
    private bool ableToCall = false;
    private float timerCooldown = 1f;
    private float timer;
    // Start is called before the first frame update
    void Start()
    {
        Singleton.Instance.OnFreeTimeStarts += PrepareFreeTimeAnimation;
        gameObject.SetActive(false);
    }
    private void Update() {
        if (!GameFlow.Instance.inHouse && ableToCall) {
            timer += Time.deltaTime;
            if (timer >= timerCooldown) {
                TriggerFreeTimeAnimation();
            }
        }
    }

    private void PrepareFreeTimeAnimation(object sender, EventArgs e)
    {
        ableToCall = true;
        gameObject.SetActive(true);
    }
    private void TriggerFreeTimeAnimation()
    {
        gameObject.SetActive(true);
        GetComponent<Animator>().SetTrigger("EnterFreeTime");
        ableToCall = false;
        AudioManager.Instance.PlayFreeTimeStartsAudio();
        Singleton.Instance.OnFreeTimeAlertShow?.Invoke(this, EventArgs.Empty);
    }
}
