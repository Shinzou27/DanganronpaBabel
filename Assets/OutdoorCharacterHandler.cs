using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OutdoorCharacterHandler : MonoBehaviour
{
    // Update is called once per frame
    void Update()
    {
        GetComponentInChildren<CharacterStandingVisual>().gameObject.transform.localPosition = Vector3.zero;
    }
    protected void Show(object sender, EventArgs e)
    {
        gameObject.SetActive(true);
    }
    protected void Hide(object sender, EventArgs e)
    {
        gameObject.SetActive(false);
    }
}