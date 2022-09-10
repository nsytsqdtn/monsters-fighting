using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour {
    public float damage;//伤害值
    public float speed;//移动的速度
    private Transform target;//子弹的目标
    public GameObject explorePrefabs;
    public bool isMissile;
    public bool isNormal;

    public GameObject gamamaneger;
    //得到子弹的目标

    void Start()
    {
        gamamaneger = GameObject.Find("GameManager");
    }
    public void setTarget(Transform m_target)
    {
        this.target = m_target;
    }

    void Update()
    {
        //目标为空时，销毁自身
        if(target == null)
        {
            die();
            return;
        }
        transform.LookAt(target.position + new Vector3(0,1f,0));//朝向敌人
        transform.Translate(Vector3.forward * speed * Time.deltaTime);//向敌人移动
    }

    //子弹碰撞到敌人后自身销毁
    void OnTriggerEnter(Collider coll)
    {
        if(coll.tag == "Enemy")
        {
            if (isMissile)
            {
                gamamaneger.GetComponent<AudioPlay>().OpenBoom();
            }

            coll.GetComponent<Enemy>().takeDamage(damage);
            die();
        }
        if (coll.tag == "dier")
        {
            if (isMissile)
            {
                gamamaneger.GetComponent<AudioPlay>().OpenBoom();
            }
            die();
        }
    }

    //子弹销毁的特效和死亡
    void die()
    {
        GameObject effect = GameObject.Instantiate(explorePrefabs, transform.position, transform.rotation);
        Destroy(this.gameObject);
        if (isMissile)
        {
            Destroy(effect, 1.5f);
        }else if (isNormal)
        {
            Destroy(effect, 0.2f);
        }
        
    }
}
