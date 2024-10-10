using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MikanAnalysisHandler : OutdoorCharacterHandler
{
    // Start is called before the first frame update
    void Start()
    {
        gameObject.SetActive(false);
        Singleton.Instance.OnBloodAnalysis += Show;
        Singleton.Instance.OnFreeTimeStarts += Hide;
    }
    
}
