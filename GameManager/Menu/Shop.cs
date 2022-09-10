using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Shop : MonoBehaviour {
    private int zuobi = 0;

    public Text Gold;//金币UI显示
    //四种商品的价格
    private int DoubleLasercost = 200;
    private int DoubleMisslecost = 400;
    private int SecondWorldcost = 300;
    private int StrengthNormalcost = 450;
    private int Spikecost = 150;

    public GameObject Tishi1;
    public GameObject Tishi2;
    public GameObject Tishi3;
    public GameObject Tishi4;
    public GameObject SecondBuyMessage;
    public GameObject SpikeBuyMessage;
    public GameObject NormalBuyMessage;
    public GameObject LaserBuyMessage;
    public GameObject MissiBuyleMessage;
    private int timesSecondMap = 0;
    private int timesSpike = 0;
    private int timesNormal = 0;
    private int timesLaser = 0;
    private int timesMissile= 0;

    //四种商品的购买按钮点击后的实现
    public void OnBuyDoubleLaser()
    {
        if (DateFile.DoubleLaser == 0)
        {
            if (DateFile.goldnum > DoubleLasercost)
            {
                DateFile.goldnum -= DoubleLasercost;
                Gold.text = DateFile.goldnum.ToString();
                DateFile.DoubleLaser = 1;
                StartCoroutine("Tishione");
            }
            else
            {
                StartCoroutine("Tishitwo");
            }
        }else if(DateFile.DoubleLaser == 1)
        {
            StartCoroutine("Tishifour");
        }

    }
    public void OnBuyDoubleMissle()
    {
        if (DateFile.DoubleMissle == 0)
        {
            if (DateFile.goldnum > DoubleMisslecost)
            {
                DateFile.goldnum -= DoubleMisslecost;
                Gold.text = DateFile.goldnum.ToString();
                DateFile.DoubleMissle = 1;
                StartCoroutine("Tishione");
            }
            else
            {
                StartCoroutine("Tishitwo");
            }
        }
        else if (DateFile.DoubleMissle == 1)
        {
            StartCoroutine("Tishifour");
        }

    }
    public void OnBuySecondWorld()
    {
        if (DateFile.SecondWorld == 4)
        {
            if (DateFile.goldnum > SecondWorldcost)
            {
                DateFile.goldnum -= SecondWorldcost;
                Gold.text =DateFile.goldnum.ToString();
                DateFile.SecondWorld = 1;
                StartCoroutine("Tishione");
            }
            else
            {
                StartCoroutine("Tishitwo");
            }
        }
        else if(DateFile.SecondWorld == 0 || DateFile.SecondWorld == 2)
        {
            StartCoroutine("Tishithree");
        }
        else if (DateFile.SecondWorld == 1)
        {
            StartCoroutine("Tishifour");
        }

    }
    public void OnBuyStrengthNormal()
    {
        if (DateFile.StrengthNormal == 0)
        {
            if (DateFile.goldnum > StrengthNormalcost)
            {
                DateFile.goldnum -= StrengthNormalcost;
                Gold.text = DateFile.goldnum.ToString();
                DateFile.StrengthNormal = 1;
                StartCoroutine("Tishione");
            }
            else
            {
                StartCoroutine("Tishitwo");
            }
        }
        else if (DateFile.StrengthNormal == 1)
        {
            StartCoroutine("Tishifour");
        }
    }
    public void OnBuySpike()
    {
        if (DateFile.Spike == 0)
        {
            if (DateFile.goldnum > Spikecost)
            {
                DateFile.goldnum -= Spikecost;
                Gold.text = DateFile.goldnum.ToString();
                DateFile.Spike = 1;
                StartCoroutine("Tishione");
            }
            else
            {
                StartCoroutine("Tishitwo");
            }
        }
        else if (DateFile.Spike == 1)
        {
            StartCoroutine("Tishifour");
        }
    }
    public void Disappear()
    {
        SecondBuyMessage.SetActive(false);
        SpikeBuyMessage.SetActive(false);
        NormalBuyMessage.SetActive(false);
        MissiBuyleMessage.SetActive(false);
        LaserBuyMessage.SetActive(false);
        timesLaser = 0;
        timesMissile = 0;
        timesNormal = 0;
        timesSecondMap = 0;
        timesSpike = 0;
    }
    public void OnMessageSecondMap()
    {
        
        if (timesSecondMap == 0)
        {
            Disappear();
            SecondBuyMessage.SetActive(true);
            timesSecondMap = 1;
        }else if(timesSecondMap == 1)
        {
            SecondBuyMessage.SetActive(false);
            timesSecondMap = 0;
        }
    }
    public void OnMessageSpike()
    {
        
        if (timesSpike == 0)
        {
            Disappear();
            SpikeBuyMessage.SetActive(true);
            timesSpike = 1;
        }
        else if (timesSpike == 1)
        {
            SpikeBuyMessage.SetActive(false);
            timesSpike = 0;
        }
    }
    public void OnMessageNormal()
    {
        
        if (timesNormal == 0)
        {
            Disappear();
            NormalBuyMessage.SetActive(true);
            timesNormal = 1;
        }
        else if (timesNormal == 1)
        {
            NormalBuyMessage.SetActive(false);
            timesNormal = 0;
        }
    }
    public void OnMessageLaser()
    {
        
        if (timesLaser == 0)
        {
            Disappear();
            LaserBuyMessage.SetActive(true);
            timesLaser = 1;
        }
        else if (timesLaser == 1)
        {
            LaserBuyMessage.SetActive(false);
            timesLaser = 0;
        }
    }
    public void OnMessageMissile()
    {
        
        if (timesMissile == 0)
        {
            Disappear();
            MissiBuyleMessage.SetActive(true);
            timesMissile = 1;
        }
        else if (timesMissile == 1)
        {
            MissiBuyleMessage.SetActive(false);
            timesMissile = 0;
        }
    }
    IEnumerator Tishitwo()
    {
        Tishi2.SetActive(true);
        yield return new WaitForSeconds(1);
        Tishi2.SetActive(false);
    }
    IEnumerator Tishione()
    {
        Tishi1.SetActive(true);
        yield return new WaitForSeconds(1);
        Tishi1.SetActive(false);
    }
    IEnumerator Tishithree()
    {
        Tishi3.SetActive(true);
        yield return new WaitForSeconds(1);
        Tishi3.SetActive(false);
    }
    IEnumerator Tishifour()
    {
        Tishi4.SetActive(true);
        yield return new WaitForSeconds(1);
        Tishi4.SetActive(false);
    }

    public void OnzuobiButton()
    {
        Debug.Log(zuobi);
        switch (zuobi)
        {
            case 0:
                zuobi = 1;
                break;
            case 1:
                zuobi = 2;
                break;
            case 2:
                zuobi = 3;
                break;
            case 3:
                zuobi = 4;
                break;
            case 4:
                zuobi = 5;
                break;
            case 5:
                zuobi = 6;
                break;
            case 6:
                DateFile.goldnum += 500;
                Gold.text = DateFile.goldnum.ToString();
                break;
        }
    }
}
