using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletCollisionHandler : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name.Contains("GunBullet")) {   
            Singleton.Instance.OnBulletShot?.Invoke(this, EventArgs.Empty);
        }
    }
}
