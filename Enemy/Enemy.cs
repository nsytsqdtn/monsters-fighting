using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Enemy : MonoBehaviour {
    public bool isdouble;//双球敌人
    public GameObject doublePrefabs;//双球敌人的子物体
    private int currentindex;//当前位置的索引

    public float attack = 1;
    public float timer;

    public bool iscontinue;//继承者
    public bool isDeser;//破坏者
    public bool isSmall;//小球
    public bool isGhost;//鬼
    public bool isZombie;//僵尸
    public bool isskeleton;//骷髅
    public bool isboss;

    private bool flag1 = true;//可以改变方向的标志
    public GameObject hpslidertrans;//血条变了方向后需要改变回来，正对玩家
    
    //血量显示
    public float hp = 150;
    private float hptotal;
    public Slider hpslider;

    //加金币操作
    public int value;//敌人的价值
    private GameObject gamemanager;
    private int target;
    private bool isdie = false;
    private Transform[] position;//小球的位置
    public int index = 0;//索引号
    private WayPointManager waypoints;//敌人移动路上的路径点
    public float speed = 6;//默认敌人移动的速度
    private EnemySpawner enemyspaner;//敌人孵化器脚本

    private bool kuang = false;
    public GameObject Brain;

    GameObject[] standard;
    GameObject[] final;
    GameObject[] upgrade;
    void Start () {
        timer = attack;
        //播放动画
        if (isDeser)
        {
            gameObject.GetComponent<Animation>().Play("orc_walk");
        }
        else if (isboss)
        {
            gameObject.GetComponent<Animation>().Play("wolf_walk");
        }
        else if (isdouble)
        {
            gameObject.GetComponent<Animation>().Play("dragon_talk");
        }
        else if (isSmall)
        {
            gameObject.GetComponent<Animation>().Play("mummy_dizzy");
        }
        else if (iscontinue)
        {
            gameObject.GetComponent<Animation>().Play("dragon_talk");
        }
        else if (isGhost)
        {
            gameObject.GetComponent<Animation>().Play("ghost_toss_ball");
        }
        else if(isskeleton)
        {
            gameObject.GetComponent<Animation>().Play("skeleton_trans_fall");
        }
        
        gamemanager = GameObject.Find("GameManager");//得到挂载游戏管理器脚本的游戏物体
        enemyspaner = GameObject.Find("GameManager").GetComponent<EnemySpawner>();
        waypoints = GameObject.Find("WayPoints").GetComponent<WayPointManager>();
        position = waypoints.positions;//把WayPointManager得到的positions拿来对此position赋值
        currentindex = position.Length - 2;
        AddHp();
    }
    IEnumerator run()
    {
        yield return new WaitForSeconds(0.8f);
        move();
        kuang = true;
    }
    void Update() {
        if (!iscontinue)
        {
            if (!isdie)
            {
                if (isZombie)
                {
                    if (hp <= 1250 && kuang == false)
                    {
                        Brain.SetActive(false);
                        gameObject.GetComponent<Animation>().Play("zombie_panic");
                        speed = 6.5f;
                        StartCoroutine("run");
                    }
                    if (hp > 1000 || kuang == true)
                    {
                        if (hp > 1250)
                        {
                            gameObject.GetComponent<Animation>().Play("zombie_walk");
                        }
                        else
                        {
                            gameObject.GetComponent<Animation>().Play("zombie_jump");
                        }
                        move();
                    }
                }
                
                if (!isGhost&&!isZombie)
                {
                    move();//普通敌人的行动
                }
                if (isGhost && hp >= 1200)
                {
                    move();//普通敌人的行动
                }
                if (isGhost && hp < 1200)
                {
                    standard = GameObject.FindGameObjectsWithTag("Standard");
                    final = GameObject.FindGameObjectsWithTag("Final");
                    upgrade = GameObject.FindGameObjectsWithTag("Upgrade");
                    for (int i = 1; i < standard.Length; i++)
                    {
                       GameObject temp = standard[i];
                        for (int j = i - 1; j >= 0; j--)
                        {
                            if (Vector3.Distance(transform.position, standard[j].transform.position) > Vector3.Distance(transform.position, temp.transform.position))
                            {
                                standard[j + 1] = standard[j];
                                if (j == 0)
                                {
                                    standard[0] = temp;
                                    break;
                                }
                            }
                            else
                            {
                                standard[j + 1] = temp;
                                break;
                            }
                        }
                    }
                    if (standard.Length != 0)
                        {
                            Vector3 angel = transform.localEulerAngles;
                            transform.LookAt(standard[0].transform.position);
                            hpslidertrans.transform.Rotate(-1*(transform.localEulerAngles - angel));
                            transform.position = Vector3.MoveTowards(transform.position, standard[0].transform.position, 2 * Time.deltaTime);
                            if (Vector3.Distance(transform.position, standard[0].transform.position) < 1f)
                            {
                                timer += Time.deltaTime;
                                if (timer >= attack)
                                {
                                timer = 0;
                                gameObject.GetComponent<Animation>().Play("ghost_attack_with_ball");
                                Destroy(standard[0].gameObject);
                                standard[0].GetComponent<Turret>().die();
                            }
                                }
                        }
                        else
                        {
                            move();//普通敌人的行动
                        }
                }
            }
        }
        else if (iscontinue)
        {
            if (!isdie)
            {
                movecontinue();//double敌人生成小怪物的行动
            }
        }
        
    
    }
    //怪物的移动
    void move()
    {
        if (index <= position.Length - 1)
        {
            enemy2move();
            transform.LookAt(position[index].position);
            transform.Translate((position[index].position - transform.position).normalized * Time.deltaTime * speed, Space.World);
            if (Vector3.Distance(position[index].position, transform.position) < 0.3)
            {
                index++;
                flag1 = true;
                currentindex = index;
            }
            if (index> position.Length - 1)
            {
                ReachDenstination();
            }
        }
    }
    void enemy2move()
    {
        //移动时候的朝向改变
            if (DateFile.SecondWorld == 0)
            {
                switch (index)
                {
                case 0:
                    if (flag1)
                    {
                        hpslidertrans.transform.Rotate(new Vector3(0, -180, 0));
                    }
                    flag1 = false;
                    break;
                case 1:
                        if (flag1)
                        {
                            hpslidertrans.transform.Rotate(new Vector3(0, 0, 0));
                        }
                        flag1 = false;
                        break;
                    case 2:
                        if (flag1)
                        {
                            hpslidertrans.transform.Rotate(new Vector3(0, 0, 0));
                        }
                        flag1 = false;
                        break;
                    case 3:
                        if (flag1)
                        {
                            hpslidertrans.transform.Rotate(new Vector3(0, 90, 0));
                        }
                        flag1 = false;
                        break;
                    case 4:
                        if (flag1)
                        {
                            hpslidertrans.transform.Rotate(new Vector3(0, 90, 0));
                        }
                        flag1 = false;
                        break;
                    case 5:
                        if (flag1)
                        {
                            hpslidertrans.transform.Rotate(new Vector3(0, -90, 0));
                        }
                        flag1 = false;
                        break;
                    case 6:
                        if (flag1)
                        {
                            hpslidertrans.transform.Rotate(new Vector3(0, -90, 0));
                        }
                        flag1 = false;
                        break;
                    case 7:
                        if (flag1)
                        {
                            hpslidertrans.transform.Rotate(new Vector3(0, 90, 0));
                        }
                        flag1 = false;
                        break;
                    case 8:
                        if (flag1)
                        {
                            hpslidertrans.transform.Rotate(new Vector3(0, 90, 0));
                        }
                        flag1 = false;
                        break;
                    case 9:
                        if (flag1)
                        {
                            hpslidertrans.transform.Rotate(new Vector3(0, 0, 0));
                        }
                        flag1 = false;
                        break;
                }
            }
            else if(DateFile.SecondWorld == 1)
            {
                switch (index)
                {
                case 0:
                    if (flag1)
                    {
                        hpslidertrans.transform.Rotate(new Vector3(0, 0, 0));
                    }
                    flag1 = false;
                    break;
                case 1:
                    if (flag1)
                    {
                        hpslidertrans.transform.Rotate(new Vector3(0, 0, 0));
                    }
                    flag1 = false;
                    break;
                case 2:
                        if (flag1)
                        {
                            hpslidertrans.transform.Rotate(new Vector3(0, 90, 0));
                        }
                        flag1 = false;
                        break;
                    case 3:
                        if (flag1)
                        {
                            hpslidertrans.transform.Rotate(new Vector3(0, 90, 0));
                        }
                        flag1 = false;
                        break;
                case 4:
                    if (flag1)
                    {
                        hpslidertrans.transform.Rotate(new Vector3(0, -90, 0));
                    }
                    flag1 = false;
                    break;
                case 5:
                        if (flag1)
                        {
                            hpslidertrans.transform.Rotate(new Vector3(0, -90, 0));
                        }
                        flag1 = false;
                        break;
                    case 6:
                        if (flag1)
                        {
                            hpslidertrans.transform.Rotate(new Vector3(0, 90, 0));
                        }
                        flag1 = false;
                        break;
                    case 7:
                        if (flag1)
                        {
                            hpslidertrans.transform.Rotate(new Vector3(0, 90, 0));
                        }
                        flag1 = false;
                        break;
                    case 8:
                        if (flag1)
                        {
                            hpslidertrans.transform.Rotate(new Vector3(0, -90, 0));
                        }
                        flag1 = false;
                        break;
                    case 9:
                       if (flag1)
                       {
                        hpslidertrans.transform.Rotate(new Vector3(0, -90, 0));
                        }
                        flag1 = false;
                        break;
                    case 10:
                        if (flag1)
                        {
                        hpslidertrans.transform.Rotate(new Vector3(0, 90, 0));
                        }
                        flag1 = false;
                        break;
                    case 11:
                        if (flag1)
                        {
                        hpslidertrans.transform.Rotate(new Vector3(0, 90, 0));
                         }
                         flag1 = false;
                         break;
                    case 12:
                        if (flag1)
                        {
                        hpslidertrans.transform.Rotate(new Vector3(0, 0, 0));
                        }
                        flag1 = false;
                        break;
                }
            }
        else if (DateFile.SecondWorld == 2|| DateFile.SecondWorld == 4)
        {
            switch (index)
            {
                case 0:
                    if (flag1)
                    {
                        hpslidertrans.transform.Rotate(new Vector3(0, 0, 0));
                    }
                    flag1 = false;
                    break;
                case 4:
                    if (flag1)
                    {
                        hpslidertrans.transform.Rotate(new Vector3(0, 90, 0));
                    }
                    flag1 = false;
                    break;
                case 5:
                    if (flag1)
                    {
                        hpslidertrans.transform.Rotate(new Vector3(0, 90, 0));
                    }
                    flag1 = false;
                    break;
                case 6:
                    if (flag1)
                    {
                        hpslidertrans.transform.Rotate(new Vector3(0, 0, 0));
                    }
                    flag1 = false;
                    break;
                case 7:
                    if (flag1)
                    {
                        hpslidertrans.transform.Rotate(new Vector3(0, 0, 0));
                    }
                    flag1 = false;
                    break;
                case 8:
                    if (flag1)
                    {
                        hpslidertrans.transform.Rotate(new Vector3(0, 0, 0));
                    }
                    flag1 = false;
                    break;
                case 9:
                    if (flag1)
                    {
                        hpslidertrans.transform.Rotate(new Vector3(0, 0, 0));
                    }
                    flag1 = false;
                    break;
                case 10:
                    if (flag1)
                    {
                        hpslidertrans.transform.Rotate(new Vector3(0, 0, 0));
                    }
                    flag1 = false;
                    break;
                case 11:
                    if (flag1)
                    {
                        hpslidertrans.transform.Rotate(new Vector3(0, -90, 0));
                    }
                    flag1 = false;
                    break;
                case 12:
                    if (flag1)
                    {
                        hpslidertrans.transform.Rotate(new Vector3(0, -90, 0));
                    }
                    flag1 = false;
                    break;
                case 13:
                    if (flag1)
                    {
                        hpslidertrans.transform.Rotate(new Vector3(0, 0, 0));
                    }
                    flag1 = false;
                    break;
                case 18:
                    if (flag1)
                    {
                        hpslidertrans.transform.Rotate(new Vector3(0, 90, 0));
                    }
                    flag1 = false;
                    break;
                case 19:
                    if (flag1)
                    {
                        hpslidertrans.transform.Rotate(new Vector3(0, 90, 0));
                    }
                    flag1 = false;
                    break;
            }
        }
    }
    //double小球死亡后产生小球，从空中到目的地
    void movecontinue()
    {     //到倒数第二个目的地
        if(currentindex== position.Length - 2)
        {
            transform.Translate((position[currentindex].position - transform.position).normalized * Time.deltaTime * speed, Space.World);
        }//到倒数第一个目的地
        else if(currentindex == position.Length - 1)
        {
            transform.Translate((position[currentindex].position - transform.position).normalized * Time.deltaTime * speed, Space.World);
        }
        
        if (Vector3.Distance(position[currentindex].position, transform.position) < 0.3)
        {
            currentindex++;
        }
        if (currentindex > position.Length - 1)
        {
                ReachDenstination();
        }
    }

    //到达目的地后销毁
    void ReachDenstination()
    {
        //减少基地的血量
        GameManager.Instance.HomeHp--;
        if (GameManager.Instance.HomeHp > 0)
        {
            GameObject.Destroy(this.gameObject);
            
        }//基地血量为0时，游戏失败
        else if(GameManager.Instance.HomeHp == 0)
        {
            GameManager.Instance.Failed();
            GameObject.Destroy(this.gameObject);
        }//销毁游戏体
        else
        {
            GameObject.Destroy(this.gameObject);
        }
        
    }
    //销毁与死亡
    void OnDestroy()
    {
            EnemySpawner.countEnemyAlive--;
    }
    //被攻击到收到伤害
    public void takeDamage(float damage)
    {
        if (hp <= 0) return;
        hp -= damage;
        hpslider.value = hp / hptotal;
        if (hp <= 0)
        {
            die();
        }
    }
    //死亡处理
    void die()
    {   //普通敌人的死亡
        isdie = true;
        hpslidertrans.SetActive(false);
        gameObject.tag = "dier";
        if (!isdouble)
        {
            if (isDeser)
            {
                gameObject.GetComponent<Animation>().Stop("orc_walk");
                gameObject.GetComponent<Animation>().Play("orc_die");
            }
            else if (isSmall)
            {
                gameObject.GetComponent<Animation>().Stop("mummy_dizzy");
                gameObject.GetComponent<Animation>().Play("mummy_die");
            }
            else if (iscontinue)
            {
                gameObject.GetComponent<Animation>().Stop("dragon_talk");
                gameObject.GetComponent<Animation>().Play("dragon_die");
            }
            else if (isGhost)
            {
                gameObject.GetComponent<Animation>().Stop("ghost_toss_ball");
                gameObject.GetComponent<Animation>().Play("ghost_rise_from_floor");
            }
            else if (isZombie)
            {
                gameObject.GetComponent<Animation>().Stop("zombie_trans_fall");
                gameObject.GetComponent<Animation>().Play("zombie_die");
            }
            else if (isboss)
            {
                gameObject.GetComponent<Animation>().Stop("wolf_walk");
                gameObject.GetComponent<Animation>().Play("wolf_die");
            }
            else if(isskeleton)
            {
                gameObject.GetComponent<Animation>().Stop("skeleton_trans_fall");
                gameObject.GetComponent<Animation>().Play("skeleton_die");
            }
            if (!isGhost)
            {
                gamemanager.GetComponent<TurretBuildManager>().SendMessage("ChangeMoney", value);//击败一个敌人加金币
                StartCoroutine("Dieanddes");
            }else if (isGhost)
            {
                gamemanager.GetComponent<TurretBuildManager>().SendMessage("ChangeMoney", value);//击败一个敌人加金币
                StartCoroutine("Dieanddes2");
            }
                
            
        }//double敌人的死亡
        else if (isdouble)
        {
            gameObject.GetComponent<Animation>().Stop("dragon_talk");
            gameObject.GetComponent<Animation>().Play("dragon_die");
            Vector3 addheight = new Vector3(0, 5, 0);
            GameObject.Instantiate(doublePrefabs, transform.position+addheight, transform.rotation);
            EnemySpawner.countEnemyAlive++;
            gamemanager.GetComponent<TurretBuildManager>().SendMessage("ChangeMoney", value);//击败一个敌人加金币
            StartCoroutine("Dieanddes");
        }
    }
    IEnumerator  Dieanddes()
    {
        yield return new WaitForSeconds(1f);
        Destroy(this.gameObject);
    }
    IEnumerator Dieanddes2()
    {
        yield return new WaitForSeconds(0.6f);
        Destroy(this.gameObject);
    }
    //难度升级，血量增加
    void AddHp()
    {
        if (DateFile.SecondWorld == 0)
        {
            if (enemyspaner.dienum == 1)
            {
                if (iscontinue)
                {
                    hp += 200;//难度升级，全体加血
                }
                else if (isSmall)
                {
                    hp += 350;
                }
                else if (isdouble)
                {
                    hp += 500;
                }
                else
                {
                    hp += 600;
                }

            }
            if (enemyspaner.dienum == 2)
            {
                if (iscontinue)
                {
                    hp += 300;//难度升级，全体加血
                }
                else if (isSmall)
                {
                    hp += 450;
                }
                else if (isdouble)
                {
                    hp += 850;
                }
                else
                {
                    hp += 1800;
                }
            }
            hptotal = hp;
        }
        else if (DateFile.SecondWorld == 1 || DateFile.SecondWorld == 2|| DateFile.SecondWorld == 4)
        {
            if (enemyspaner.dienum == 0)
            {
                hptotal = hp;
            }
            if (enemyspaner.dienum == 1)
            {
                if (isSmall)
                {
                    hp += 450;
                }
                else if (isGhost)
                {
                    hp += 500;
                }
                else if(isDeser)
                {
                    hp += 1000;
                }
                else if (isskeleton)
                {
                    hp += 500;
                }
                hptotal = hp;
            }
            if (enemyspaner.dienum == 2)
            {
                if (isSmall)
                {
                    hp += 750;
                }
                else if (isGhost)
                {
                    hp += 1000;
                }
                else if (isDeser)
                {
                    hp += 2000;
                }
                else if (isskeleton)
                {
                    hp += 1250;
                }
                hptotal = hp;
            }
            if (enemyspaner.dienum == 3)
            {
                if (isSmall)
                {
                    hp += 850;
                }
                else if (isGhost)
                {
                    hp += 1500;
                }
                else if (isDeser)
                {
                    hp += 4000;
                }
                else if (isskeleton)
                {
                    hp += 2200;
                }
                hptotal = hp;
            }
            if (enemyspaner.dienum == 4)
            {
                if (isSmall)
                {
                    hp += 1000;
                }
                else if (isGhost)
                {
                    hp += 2500;
                }
                else if (isDeser)
                {
                    hp += 6000;
                }
                else if (isskeleton)
                {
                    hp += 2800;
                }
                hptotal = hp;
            }
        }
    }
}
