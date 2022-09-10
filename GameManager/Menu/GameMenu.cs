using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class GameMenu : MonoBehaviour {
    public GameObject model1;
    public GameObject model2;
    private int lunci1;
    private int lunci2;
    //金币的ui显示
    public Text GoldUI;
    private int Gold;
    public GameObject shop;//商店
    public GameObject Book;//图鉴
    public AudioSource Bg;
    void Start()
    {
        Bg.Play();
        //读取上次推出保存的数据
        if (DateFile.isBool)
        {
            DateFile.goldnum = PlayerPrefs.GetInt("TotalGold");
            DateFile.DoubleLaser = PlayerPrefs.GetInt("isBuyLaser");
            DateFile.DoubleMissle = PlayerPrefs.GetInt("isBuyMissle");
            DateFile.SecondWorld = PlayerPrefs.GetInt("isBuySecondWorld");
            DateFile.StrengthNormal = PlayerPrefs.GetInt("isBuyStrengthNormal");
            DateFile.Spike = PlayerPrefs.GetInt("isBuySpike");
        }
        DateFile.isBool = false;
        //金币显示在UI上
        Gold = DateFile.goldnum;
        GoldUI.text = Gold.ToString();
    }

    void Update()
    {
        lunci1 = Random.Range(1, 5);
        lunci2 = Random.Range(1, 5);
        if (!model1.GetComponent<Animation>().isPlaying)
        {
            switch (lunci1){
                case 1:
                    model1.GetComponent<Animation>().Play("dragon_stirr_on_ground");
                    break;
                case 2:
                    model1.GetComponent<Animation>().Play("dragon_laugh");
                    break;
                case 3:
                    model1.GetComponent<Animation>().Play("dragon_loop_da_loop");
                    break;
                case 4:
                    model1.GetComponent<Animation>().Play("dragon_panic");
                    break;
            }
        }
        if (!model2.GetComponent<Animation>().isPlaying)
        {
            switch (lunci2)
            {
                case 1:
                    model2.GetComponent<Animation>().Play("dragon_stirr_on_ground");
                    break;
                case 2:
                    model2.GetComponent<Animation>().Play("dragon_laugh");
                    break;
                case 3:
                    model2.GetComponent<Animation>().Play("dragon_talk");
                    break;
                case 4:
                    model2.GetComponent<Animation>().Play("dragon_take_off");
                    break;
            }
        }
    }

    //打开商店
    public void OnShop()
    {
        //BackGround.SetActive(true);
        shop.SetActive(true);
    }
    //关闭商店
    public void ExitShop()
    {
        //BackGround.SetActive(false);
        shop.SetActive(false);
    }
    //打开商店
    public void OnBook()
    {
        //BackGround.SetActive(true);
        Book.SetActive(true);
    }
    //关闭商店
    public void ExitBook()
    {
        //BackGround.SetActive(false);
        Book.SetActive(false);
    }
    //开始游戏按钮
    public void OnStartGame()
    {
        Bg.Stop();
        if (DateFile.SecondWorld == 1)
        {
            SceneManager.LoadScene(3);
            SceneManager.LoadScene(4, LoadSceneMode.Additive);
        }
        else if(DateFile.SecondWorld == 0)
        {
            SceneManager.LoadScene(1);
            SceneManager.LoadScene(2, LoadSceneMode.Additive);
        }else if(DateFile.SecondWorld == 2|| DateFile.SecondWorld == 4)
        {
            SceneManager.LoadScene(5);
        }
        
    }
    //退出游戏进行的操作
    void OnApplicationQuit()
    {
        DateFile.isBool = true;//让下一次可以开始读取数据
        SaveDate();
    }
    //推出游戏按钮
    public void OnExitGame()
    {
        
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
#endif
        Application.Quit();
    }
    
    void SaveDate()
    {  
        //游戏退出后保存已经得到的数据和游戏物品
        PlayerPrefs.SetInt("TotalGold", DateFile.goldnum);
        PlayerPrefs.SetInt("isBuyLaser", DateFile.DoubleLaser);
        PlayerPrefs.SetInt("isBuyMissle", DateFile.DoubleMissle);
        PlayerPrefs.SetInt("isBuySecondWorld", DateFile.SecondWorld);
        PlayerPrefs.SetInt("isBuyStrengthNormal", DateFile.StrengthNormal);
        PlayerPrefs.SetInt("isBuySpike", DateFile.Spike);
        
    }
}
