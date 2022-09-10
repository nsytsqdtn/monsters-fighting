using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
public class TurretBuildManager : MonoBehaviour {
    //三种炮塔数据的实例化
    public TurretDate laserTurretdate;
    public TurretDate missileTurretdate;
    public TurretDate normalTurretdate;
    public TurretDate fireTurretdate;


    public int UpTime = 0;//升级过的次数
    private TurretDate selectTurret;//当前选择要创建的炮塔
    private MapCube selectmapcube;//当前选择的cube

    public Text Money;//ui的money的text组件
    private int money = 1000;//当前有的钱
    public Text UpCost;//升级花费的text组件

    private Animator UpgradeCanvasAnimator;//升级面板的出现和消失动画
    public Animator moneyBlink;//金币不够时的动画组件
    public AudioSource alarm;//金币不够时的音效组件

    //升级的ui显示
    public GameObject UpgradeCanvas;
    public Button UpgradeButton;

    public GameObject[] enemy;

    void Start()
    {
        if(DateFile.SecondWorld == 1)
        {
            money = 2000;
        }else if (DateFile.SecondWorld == 0|| DateFile.SecondWorld == 2|| DateFile.SecondWorld == 4)
        {
            money = 1000;
        }
        UpgradeCanvasAnimator = UpgradeCanvas.GetComponent<Animator>();
    }

    //更新金币的显示
    void ChangeMoney(int change = 0)
    {
        money += change;
        Money.text = money.ToString();
    }


    void Update()
    {
        enemy = GameObject.FindGameObjectsWithTag("Enemy");
        
        Build();
    }


    //三种炮塔选择各自触发的事件
    public void OnLaserSelected(bool isOn)
    {
        if (isOn)
        {
            selectTurret = laserTurretdate;
        }
    }
    public void OnMisiileSelected(bool isOn)
    {
        if (isOn)
        {
            selectTurret = missileTurretdate;
        }
    }
    public void OnNormalSelected(bool isOn)
    {
        if (isOn)
        {
            selectTurret = normalTurretdate;
        }
    }
    public void OnFireSelected(bool isOn)
    {
        if (isOn)
        {
            selectTurret = fireTurretdate;
        }
    }

    //点击鼠标建造炮塔
    public void Build()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (EventSystem.current.IsPointerOverGameObject() == false)//鼠标不碰撞到ui
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);//得到一个鼠标点击位置的射线
                RaycastHit hit;
                bool isCollider = Physics.Raycast(ray, out hit, 1000, LayerMask.GetMask("MapCube"));//判断碰撞的情况
                if (isCollider)
                {
                    MapCube mapcube = hit.collider.GetComponent<MapCube>();//得到点击的cube的MapCube组件
                    if (selectTurret != null)//在MapCube上建造一级的炮塔
                    {
                        if (mapcube.myturret == null)
                        {
                            if (money >= selectTurret.cost)//钱够就可以建造
                            {
                                ChangeMoney(-selectTurret.cost);
                                mapcube.BuildTurret(selectTurret);
                            }
                            else//提示钱不够的信息
                            {
                                moneyBlink.SetTrigger("Blink");//设置钱不够播放动画的条件
                                alarm.Play();//播放动画
                            }
                        }
                        else if (mapcube.myturret.tag == "Standard")//升级到upgrade
                        {
                            selectmapcube = mapcube;
                            UpTime = 0;
                            if (mapcube == selectmapcube && UpgradeCanvas.activeInHierarchy)//首先检测点击的cube是不是已经选择过的cube，如果是，则让升级面板隐藏
                            {
                                StartCoroutine(hideUpgradeUI());//调用协程
                            }
                            else
                            {
                                showUpgradeUI();//升级面板显示
                            }
                        }
                        else if (mapcube.myturret.tag == "Upgrade")//升级到Final
                        {
                            selectmapcube = mapcube;
                            UpTime = 1;
                            if (mapcube == selectmapcube && UpgradeCanvas.activeInHierarchy)//首先检测点击的cube是不是已经选择过的cube，如果是，则让升级面板隐藏
                            {
                                StartCoroutine(hideUpgradeUI());//调用协程
                            }
                            else
                            {
                                showUpgradeUI();//升级面板显示
                            }
                        }
                        else if (mapcube.myturret.tag == "Final")
                        {
                            UpTime = 2;
                            if (mapcube == selectmapcube && UpgradeCanvas.activeInHierarchy)//首先检测点击的cube是不是已经选择过的cube，如果是，则让升级面板隐藏
                            {
                                StartCoroutine(hideUpgradeUI());//调用协程
                            }
                            else
                            {
                                showUpgradeUI();//升级面板显示
                            }
                            selectmapcube = mapcube;
                        }
                    }
                }
            }
        }
    }
    //显示升级UI
    public void showUpgradeUI()
    {
        if (UpTime == 0)
        {
            UpCost.text = "$  " + selectmapcube.turretdate.costUpgrade.ToString();
        }
        if (UpTime == 1)
        {
            UpCost.text = "$  " + selectmapcube.turretdate.costFinal.ToString();
        }
        StopCoroutine("hideUpgradeUI");//防止和协程冲突
        UpgradeCanvas.SetActive(false);//先禁用，方便下一次转移到其他炮塔上时动画顺利播放出来
        UpgradeCanvas.SetActive(true);
    }
    //隐藏升级UI
    IEnumerator hideUpgradeUI()
    {
        UpgradeCanvasAnimator.SetTrigger("Hide");
        yield return new WaitForSeconds(0.25f);
        UpgradeCanvas.SetActive(false);
    }
    //升级按钮按下后的实现
    public void OnUpgradeButtonDown()
    {
        if(UpTime == 0)
        {
            if(money >= selectmapcube.turretdate.costUpgrade)//钱够升级
            {
                selectmapcube.UpgradeTurret();
                ChangeMoney(-selectmapcube.turretdate.costUpgrade);
                StartCoroutine("hideUpgradeUI");
            }
            else//提示钱不够的信息
            {
                moneyBlink.SetTrigger("Blink");//设置钱不够播放动画的条件
                alarm.Play();//播放动画
            }

        }
        else if(UpTime == 1)
        {
            if (money >= selectmapcube.turretdate.costFinal)//钱够升级
            {
                selectmapcube.FinalTurret();
                ChangeMoney(-selectmapcube.turretdate.costFinal);
                StartCoroutine("hideUpgradeUI");
            }
            else//提示钱不够的信息
            {
                moneyBlink.SetTrigger("Blink");//设置钱不够播放动画的条件
                alarm.Play();//播放动画
            }
        }
        else
        {
            return;
        }
            
    }
}
