using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrialStartAnimationHandler : MonoBehaviour
{
    [SerializeField] private Animator trialGroundAnimator;
    [SerializeField] private Animator trialPathAnimator;
    private bool ableToCall = false;
    private float timerCooldown = 1f;
    private float timer;
    public bool debugging;
    private bool animatingTrial = false;
    private float trialAnimationTimerDuration = 8f;
    private float trialAnimationTimer;
    void Start()
    {
        if (debugging) {
            EnterDebugMode();
        } else {
            trialGroundAnimator.gameObject.SetActive(false);
        }
        // trialGroundAnimator.enabled = false;
        trialPathAnimator.gameObject.SetActive(false);
        Singleton.Instance.OnFreeTimeEnds += PrepareTrialAnimation;
        Singleton.Instance.OnTrialStarts += Animate;
    }
        private void Update() {
        if (!GameFlow.Instance.inHouse && ableToCall) {
            timer += Time.deltaTime;
            if (timer >= timerCooldown) {
                Singleton.Instance.OnLeavingFreeTime?.Invoke(this, EventArgs.Empty);
                AudioManager.Instance.PlayDoorbell();
                ableToCall = false;
            }
        }
        if (animatingTrial) {
            trialAnimationTimer += Time.deltaTime;
            if (trialAnimationTimer >= trialAnimationTimerDuration) {
                animatingTrial = false;
                trialAnimationTimer = 0f;
                trialPathAnimator.gameObject.SetActive(true);
            }
        }
    }
    private void PrepareTrialAnimation(object sender, EventArgs e)
    {
        ableToCall = true;
        gameObject.SetActive(true);
    }

    private void Animate(object sender, EventArgs e)
    {
        trialGroundAnimator.gameObject.SetActive(true);
        trialGroundAnimator.Play("Trial_Emerge");
        animatingTrial = true;
        AudioManager.Instance.PlayEarthquake();
    }
    public void EnterDebugMode() {
        GetComponent<Animator>().enabled = false;
        transform.position = new(90.66f, 0, 0);
    }
}
