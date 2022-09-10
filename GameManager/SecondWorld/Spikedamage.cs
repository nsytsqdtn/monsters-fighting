using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spikedamage : MonoBehaviour {
    public float damage = 350;

    void OnTriggerEnter(Collider coll)
    {
        if (DateFile.Spike == 1)
        {
            if (coll.tag == "Enemy")
            {
                coll.GetComponent<Enemy>().takeDamage(damage);
            }
        }
        
    }
}
