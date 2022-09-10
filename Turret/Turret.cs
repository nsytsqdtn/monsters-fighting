using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turret : MonoBehaviour {
    public bool isdie = false;

    public float attackrate;//攻击速率
    private float timer = 0;//计时器

    public GameObject BulletPrefabs;//炮弹的预制体;
    public Transform Gun;//枪口的位置
    public Transform Head;//头部的位置

    public bool isLaser;//是否是激光攻击
    public bool isNormalturretFinal;//是否是普通炮塔的终极形态
    public bool isMissile;//是否是导弹塔
    public bool isFire;//是否是喷火攻击

    public GameObject Fire;//火焰的特效

    public GameObject gamemaneger;

    public float damagerate;//激光的伤害
    public LineRenderer LaserRenderer;//激光linerenderer的引用
    public LineRenderer LaserRenderer2;//激光linerenderer2的引用
    public GameObject LaserEffect;//激光特效的引用
    public GameObject LaserEffect2;//激光特效2的引用

    public GameObject fireaudio;
    private GameObject fireaudiosourse;
    private int fireaudioflag = 0;

    public GameObject laseraudio;
    private GameObject laseraudiosourse;
    private int audioflag = 0;
    private bool flag = true;
    //对敌人的碰撞检测
    public List<GameObject> enemy = new List<GameObject>();

    public void die()
    {
        isdie = true;
    }
     void OnTriggerEnter(Collider coll)
    {
        if(coll.tag == "Enemy")
        {
            enemy.Add(coll.gameObject);
        }
    }
    void OnTriggerStay(Collider coll)
    {
        if (coll.tag == "dier")
        {
            enemy.Remove(coll.gameObject);
        }
    }
    void OnTriggerExit(Collider coll)
    {
        if (coll.tag == "Enemy")
        {
            enemy.Remove(coll.gameObject);
        }
    }

    void Start()
    {
        gamemaneger = GameObject.Find("GameManager");
        if (isNormalturretFinal)
        {
            if (DateFile.StrengthNormal == 1)
            {
                attackrate = 0.1f;
            }
        }

           timer = attackrate;
    }
    
    void Update()
    {
        //改变枪口的位置，指向敌人,共同使用
        if (enemy.Count > 0 && enemy[0] != null)
        {
            Vector3 targetposition = enemy[0].transform.position;
            targetposition.y = Head.position.y;
            Head.LookAt(targetposition);
        }
        
        if(enemy.Count > 0 && isLaser == false && isFire == false)
        {
            gamemaneger.GetComponent<AudioPlay>().OpenGun();
            //炮弹攻击敌人的方式
            timer += Time.deltaTime;
            if (timer >= attackrate && enemy.Count > 0)
            {
                timer = 0;
                Attack();
            }
        }
        //激光炮塔的攻击方式
        //当敌人的数量大于0时，开始准备攻击
        else if(enemy.Count>0 && isLaser == true)
        {   
            LaserAttack();
        }
        else if (enemy.Count > 0 && isFire == true)
        {
            FireAttack();
        }
        else
        {
            if (isLaser)
            {
                LaserEffect.SetActive(false);
                LaserEffect2.SetActive(false);
                LaserRenderer.enabled = false;
                LaserRenderer2.enabled = false;
                flag = true;
                if (audioflag>0)
                {
                    Destroy(laseraudiosourse,0.1f);
                    audioflag--;
                }
                
            }
            if (Fire)
            {
                if (fireaudioflag > 0)
                {
                    Destroy(fireaudiosourse, 0.1f);
                    fireaudioflag--;
                }
                Fire.SetActive(false);
            }
        }
    }
    public void Attack()
    {
        //普通攻击
        if (DateFile.DoubleMissle == 0 )
        {
            for (int k = 0; k < enemy.Count; k++)
            {
                if (enemy[k] == null)//若有敌人已经为空，更新敌人链表
                {
                    UpdateEnemy();
                }
            }
            if (enemy.Count > 0)//如果敌人链表>0，进行攻击
            {
                gamemaneger.GetComponent<AudioPlay>().OpenMissile();
                GameObject bullet = GameObject.Instantiate(BulletPrefabs, Gun.position, Gun.rotation);
                bullet.GetComponent<Bullet>().setTarget(enemy[0].transform);
            }
            else//将炮塔重置为准备攻击状态
            {
                timer = attackrate;
            }
        }
        //双重攻击
        if (DateFile.DoubleMissle == 1)
        {
            if (isMissile)
            {
                for (int k = 0; k < enemy.Count; k++)
                {
                    if (enemy[k] == null)//若有敌人已经为空，更新敌人链表
                    {
                        UpdateEnemy();
                    }
                }
                if (enemy.Count > 0)//如果敌人链表>0，进行攻击
                {
                    gamemaneger.GetComponent<AudioPlay>().OpenMissile();
                    GameObject bullet = GameObject.Instantiate(BulletPrefabs, Gun.position, Gun.rotation);
                    bullet.GetComponent<Bullet>().setTarget(enemy[0].transform);
                    if (enemy.Count > 1)
                    {
                        GameObject bullet2 = GameObject.Instantiate(BulletPrefabs, Gun.position, Gun.rotation);
                        bullet2.GetComponent<Bullet>().setTarget(enemy[1].transform);
                    }
                }
            }
            else
            {
                for (int k = 0; k < enemy.Count; k++)
                {
                    if (enemy[k] == null)//若有敌人已经为空，更新敌人链表
                    {
                        UpdateEnemy();
                    }
                }
                if (enemy.Count > 0)//如果敌人链表>0，进行攻击
                {
                    gamemaneger.GetComponent<AudioPlay>().OpenMissile();
                    GameObject bullet = GameObject.Instantiate(BulletPrefabs, Gun.position, Gun.rotation);
                    bullet.GetComponent<Bullet>().setTarget(enemy[0].transform);
                }
                else//将炮塔重置为准备攻击状态
                {
                    timer = attackrate;
                }
            }
           
        }
    }
    //删除已经销毁的敌人，并更新链表
    void UpdateEnemy()
    {
        List<int> empty = new List<int>();//创建存放空敌人的链表
        //存放要销毁敌人的索引
        for (int index = 0; index < enemy.Count; index++)
        {
            if (enemy[index] == null)
            {
                empty.Add(index);
            }
        }
        //销毁
        for(int i = 0; i < empty.Count; i++)
        {
            enemy.RemoveAt(empty[i]-i);
        }
        empty.Clear();
    }
    //火焰喷射
    void FireAttack()
    {
        if(enemy.Count == 0)
        {
            Fire.SetActive(false);
        }
        for (int k = 0; k < enemy.Count; k++)
        {
            if (enemy[k] == null)//若有敌人已经为空，更新敌人链表
            {
                UpdateEnemy();
            }
        }
        if(enemy.Count > 0)
        {
            if(fireaudioflag == 0)
            {
                fireaudiosourse = GameObject.Instantiate(fireaudio, new Vector3(0, 0, 0), Quaternion.identity);
                fireaudioflag++;
            }
            Fire.SetActive(true);
        }
        else
        {
            Fire.SetActive(false);
        }
    }
    //产生激光，激光攻击的方法实现
    void LaserAttack()
    {
        if (DateFile.DoubleLaser == 0)
        { 
            if(flag == true)
            {
                LaserEffect2.SetActive(false);
                LaserRenderer2.enabled = false;
                LaserRenderer.enabled = true;
                LaserEffect.SetActive(true);
                flag = false;
            }
            for (int k = 0; k < enemy.Count; k++)
            {
                if (enemy[k] == null)//若有敌人已经为空，更新敌人链表
                {
                    UpdateEnemy();
                }
            }
            if (enemy.Count > 0)
            {
                if (audioflag == 0)
                {
                    laseraudiosourse = GameObject.Instantiate(laseraudio, new Vector3(0, 0, 0), Quaternion.identity);
                    audioflag++;
                }
                LaserRenderer.SetPositions(new Vector3[] { Gun.position, enemy[0].transform.position + new Vector3(0,1,0)});//激光产生的位置
                enemy[0].GetComponent<Enemy>().takeDamage(damagerate * Time.deltaTime);//激光的伤害
                LaserEffect.transform.position = enemy[0].transform.position + new Vector3(0,1,0);//激光特效产生的位置
                Vector3 pos = transform.position;
                pos.y = enemy[0].transform.position.y;//调整高度
                LaserEffect.transform.LookAt(pos);//将激光特效对准炮台的位置
            }
        }else if (DateFile.DoubleLaser == 1)
        {
            if (flag == true)
            {
                LaserEffect2.SetActive(true);
                LaserRenderer2.enabled = true;
                LaserRenderer.enabled = true;
                LaserEffect.SetActive(true);
                flag = false;
            }
            for (int k = 0; k < enemy.Count; k++)
            {
                if (enemy[k] == null)//若第有敌人已经为空，更新敌人链表
                {
                    UpdateEnemy();
                }
            }
            
            if (enemy.Count > 0)
            {
                if (audioflag == 0)
                {
                    laseraudiosourse = GameObject.Instantiate(laseraudio, new Vector3(0, 0, 0), Quaternion.identity);
                    audioflag++;
                }
                LaserRenderer.SetPositions(new Vector3[] { Gun.position, enemy[0].transform.position + new Vector3(0, 1, 0) });//激光产生的位置
                enemy[0].GetComponent<Enemy>().takeDamage(damagerate * Time.deltaTime);//激光的伤害
                LaserEffect.transform.position = enemy[0].transform.position;//激光特效产生的位置
                Vector3 pos = transform.position;
                pos.y = enemy[0].transform.position.y;//调整高度
                LaserEffect.transform.LookAt(pos);//将激光特效对准炮台的位置
                if (enemy.Count > 1)
                {
                    LaserRenderer2.SetPositions(new Vector3[] { Gun.position, enemy[1].transform.position + new Vector3(0, 1, 0) });
                    enemy[1].GetComponent<Enemy>().takeDamage(damagerate * Time.deltaTime);
                    LaserEffect2.transform.position = enemy[1].transform.position;//激光特效产生的位置
                    Vector3 pos1 = transform.position;
                    pos1.y = enemy[1].transform.position.y;//调整高度
                    LaserEffect2.transform.LookAt(pos1);//将激光特效对准炮台的位置
                }
                else if(enemy.Count == 1)
                {
                    LaserRenderer2.enabled = false;
                    LaserEffect2.SetActive(false);
                    flag = true;
                    
                }
            }
        }
        
    }
}
