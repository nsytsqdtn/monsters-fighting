using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fire : MonoBehaviour {
    public float damage;

    void OnTriggerStay(Collider coll)
    {
        if (coll.tag == "Enemy")
        coll.GetComponent<Enemy>().takeDamage(damage * Time.deltaTime);
    }
}
