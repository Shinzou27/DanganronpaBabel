using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonokumaWarning : OutdoorCharacterHandler
{
    // Start is called before the first frame update
    void Start()
    {
        gameObject.SetActive(false);
        Singleton.Instance.OnLeavingFreeTime += Show;
        Singleton.Instance.OnTrialStarts += Hide;
    }
}