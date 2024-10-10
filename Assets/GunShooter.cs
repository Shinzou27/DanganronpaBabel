using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunShooter : MonoBehaviour
{
    public float speed = 40;
    public GameObject bullet;
    public Transform shootPoint;
    public Animator animator;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void Fire() {
        GameObject spawnedBullet = Instantiate(bullet, shootPoint.position, shootPoint.rotation);
        spawnedBullet.GetComponent<Rigidbody>().velocity = speed * shootPoint.forward;
        animator.SetTrigger("Shoot");
        AudioManager.Instance.PlayShootBullet();
        Destroy(spawnedBullet, 1);
    }
}
