using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BuffObjectHandler : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void EvaluateBuff(BuffObject holdingBuffObject,
        out float textSpeed,
        out float damageMultiplier,
        out float healMultiplier,
        out float bulletSpeed,
        out int fakeCounterableArgumentsChance,
        out bool slowMotionEnabled
        ) {
        textSpeed = 1.5f;
        damageMultiplier = 1f;
        healMultiplier = 1f;
        bulletSpeed = 1f;
        fakeCounterableArgumentsChance = 25;
        slowMotionEnabled = false;
        if (holdingBuffObject == BuffObject.Basketball) {
            damageMultiplier = 0.5f;
            Debug.Log("Jogador obteve o buff damageMultiplier");
        } else if (holdingBuffObject == BuffObject.Camera) {
            healMultiplier = 2f;
            Debug.Log("Jogador obteve o buff healMultiplier");
        } else if (holdingBuffObject == BuffObject.DragonStatue) {
            damageMultiplier = 0.5f;
            Debug.Log("Jogador obteve o buff damageMultiplier");
        } else if (holdingBuffObject == BuffObject.Dumbbell3KG) {
            healMultiplier = 2f;
            Debug.Log("Jogador obteve o buff healMultiplier");
        } else if (holdingBuffObject == BuffObject.Dumbbell5KG) {
            slowMotionEnabled = true;
            Debug.Log("Jogador obteve o buff slowMotionEnabled");
        } else if (holdingBuffObject == BuffObject.Gameboy) {
            textSpeed = 1f;
            Debug.Log("Jogador obteve o buff textSpeed");
        } else if (holdingBuffObject == BuffObject.GoldBar) {
            textSpeed = 1f;
            Debug.Log("Jogador obteve o buff textSpeed");
        } else if (holdingBuffObject == BuffObject.Guitar) {
            fakeCounterableArgumentsChance = 5;
            Debug.Log("Jogador obteve o buff fakeCounterableArgumentsChance");
        } else if (holdingBuffObject == BuffObject.Hamster) {
            fakeCounterableArgumentsChance = 5;
            Debug.Log("Jogador obteve o buff fakeCounterableArgumentsChance");
        } else if (holdingBuffObject == BuffObject.HandFan) {
            slowMotionEnabled = true;
            Debug.Log("Jogador obteve o buff slowMotionEnabled");
        } else if (holdingBuffObject == BuffObject.Katana) {
            damageMultiplier = 0.5f;
            Debug.Log("Jogador obteve o buff damageMultiplier");
        } else if (holdingBuffObject == BuffObject.Pan) {
            damageMultiplier = 0.5f;
            Debug.Log("Jogador obteve o buff damageMultiplier");
        } else if (holdingBuffObject == BuffObject.Screwdiver) {
            healMultiplier = 2f;
            Debug.Log("Jogador obteve o buff healMultiplier");
        } else if (holdingBuffObject == BuffObject.SparklingJustice) {
            fakeCounterableArgumentsChance = 5;
            Debug.Log("Jogador obteve o buff fakeCounterableArgumentsChance");
        } else if (holdingBuffObject == BuffObject.Syringe) {
            slowMotionEnabled = true;
            Debug.Log("Jogador obteve o buff slowMotionEnabled");
        } else {
            Debug.Log("Item não identificado.");
        }
    }
}
