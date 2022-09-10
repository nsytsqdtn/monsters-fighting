using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class GameManager : MonoBehaviour {
    public GameObject EndUI;//EndUI的引用
    public GameObject NextUI;//EndUI的引用
    public float HomeHp = 5;//基地的血量
    public Text homehp;//基地的血量
    private EnemySpawner enemyspawner;//方便调用敌人孵化器额的函数，停止产生敌人
    public static GameManager Instance;//将此脚本设置为单例模式，方便其他脚本调用
    public AudioSource bg;
    //菜单界面的UI
    public GameObject MenuUi;
    public GameObject Pause;
    public GameObject Conti;
    private bool menuflag = true;
    private bool speedflag = false;
    public GameObject speedone;
    public GameObject speedtwo;
    private bool paused = false;
    void Update()
    {
        float temp = HomeHp;
        if (!speedflag)
        {
            if (paused)
            {
                Time.timeScale = 0;
            }
            else
            {
                Time.timeScale = 1;
            }
            
        }
        else
        {
            if (paused)
            {
                Time.timeScale = 0;
            }
            else
            {
                Time.timeScale = 3;
            }
            
        }
        if (HomeHp >= 0)
        {
            homehp.text = HomeHp.ToString();//血量显示
        }
    }
    void Awake()
    {
        Instance = this;//单例
        bg.Play();
        if (DateFile.SecondWorld == 0)
        {
            HomeHp = 5;
            enemyspawner = GetComponent<EnemySpawner>();
        }
        else if(DateFile.SecondWorld == 1)
        {
            enemyspawner = GetComponent<EnemySpawner>();
            HomeHp = 10;
        }
        else if (DateFile.SecondWorld == 2|| DateFile.SecondWorld == 4)
        {
            enemyspawner = GetComponent<EnemySpawner>();
            HomeHp = 10;
        }

    }
    //胜利
    public void Win()
    {
        NextUI.SetActive(true);
        if(DateFile.SecondWorld == 0)
        {
            DateFile.goldnum += 100;
            DateFile.SecondWorld = 2;
        }
        else if (DateFile.SecondWorld == 2||DateFile.SecondWorld == 4)
        {
            DateFile.goldnum += 350;
            DateFile.SecondWorld = 4;
        }
        
    }
    //失败
    public void Failed()
    {
        bg.Stop();
        enemyspawner.Stop();//停止产生敌人
        EndUI.SetActive(true);
        DateFile.goldnum += 30;
    }
    public void OnButtonRetry()
    {
        if (DateFile.SecondWorld == 0)
        {
            SceneManager.LoadScene(1);
            SceneManager.LoadScene(2, LoadSceneMode.Additive);
        }
        else if(DateFile.SecondWorld == 1)
        {
            SceneManager.LoadScene(3);
            SceneManager.LoadScene(4, LoadSceneMode.Additive);
        }
        else if (DateFile.SecondWorld == 2|| DateFile.SecondWorld == 4)
        {
            SceneManager.LoadScene(5);
        }
        bg.Stop();
        bg.Play();
        Init();
    }
    //主菜单按钮
    public void OnButtonMenu()
    {
        SceneManager.LoadScene(0);
        bg.Stop();
        Init();
    }
    public void OnButtonNext()
    {
        SceneManager.LoadScene(5);
        bg.Stop();
        bg.Play();
        Init();
    }

    //游戏菜单界面
    public void OnButtonGameMenu()
    {
        if(menuflag)
        {
            MenuUi.SetActive(true);
            menuflag = false;
        }
        else
        {
            MenuUi.SetActive(false);
            menuflag = true;
        }
    }
    public void OnButtonPause()
    {
        paused = true;
        Pause.SetActive(false);
        Conti.SetActive(true);
    }
    public void OnButtonReturn()
    {
        if (!speedflag)
        {
            speedtwo.SetActive(false);
            speedone.SetActive(true);
            speedflag = true;
        }
        else
        {
            speedtwo.SetActive(true);
            speedone.SetActive(false);
            speedflag = false;
        }
        
    }
    public void OnButtonExit()
    {
        SceneManager.LoadScene(0);
    }
    public void OnButtonBegin()
    {
        paused = false;
        Pause.SetActive(true);
        Conti.SetActive(false);
    }
    void Init()
    {
        MenuUi.SetActive(false);
        menuflag = true;
        speedtwo.SetActive(true);
        speedone.SetActive(false);
        speedflag = false;
        Pause.SetActive(true);
        Conti.SetActive(false);
        paused = false;
}
}
