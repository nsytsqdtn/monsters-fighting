using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemySpawner : MonoBehaviour {
    public EnemyWave[] waves;//波数
    public Transform START;//初始的位置
    private float del_time_waves = 2;//每波产生的间隔
    public static int countEnemyAlive = 0;//存活的敌人数量
    public int dienum = 0;//游戏的难度，初始为0，慢慢增加
    public Coroutine coroutine;
    private int wavenum = 0;//波数
    public GameObject WaveAnime;//每一波的动画
    public GameObject WaveName;//每一波的动画
    public Text WaveText;//UI显示
    public Text WaveText2;//UI显示
    void Start()
    {
        coroutine = StartCoroutine(SpawnEnemy());
    }

    public void Stop()
    {
        StopCoroutine(coroutine);
    }

    //协程，每一波怎么生成敌人，等待时间等
    IEnumerator SpawnEnemy()
    {
        WaveAnime.SetActive(false);
        WaveName.SetActive(false);
        WaveName.SetActive(true);
        yield return new WaitForSeconds(del_time_waves - 2);
        foreach (EnemyWave wave in waves)
        {
            wavenum++;
            if (wavenum != 8)
            {
                WaveText.text = "第" + wavenum + "波敌人来袭";
                WaveText2.text = wavenum + "/" + 8;
            }
            else
            {
                WaveText.text = "最后一波敌人";
                WaveText2.text = 8 + "/" + 8;
            }
            WaveAnime.SetActive(false);
            WaveAnime.SetActive(true);
            yield return new WaitForSeconds(del_time_waves - 2);
            for (int i = 0; i < wave.count; i++)
            {   
                //游戏难度升级
                if (i > 20 && i <= 30)
                {
                    dienum = 1;
                }
                if (i > 30 && i <= 50)
                {
                    dienum = 2;
                }
                if (i > 50 && i<= 70)
                {
                    dienum = 3;
                }
                if (i >70)
                {
                    dienum = 4;
                }
                //生成敌人规则
                if (DateFile.SecondWorld == 0)
                {
                    if ((i >= 2 && i <= 5) || (i > 11 && i < 16) || (i == 26) || (i == 28) || (i > 32 && i < 36) || i == 47)
                    {
                        GameObject.Instantiate(wave.enemyPrefab1, START.position, Quaternion.identity);
                        countEnemyAlive++;
                    }
                    else if ((i >= 8 && i <= 11) || (i == 25) || (i == 27) || (i >= 38 && i < 42) || i == 48)
                    {
                        GameObject.Instantiate(wave.enemyPrefab2, START.position, Quaternion.identity);
                        countEnemyAlive++;
                    }
                    else if (i < 2 || (i > 5 && i < 8) || (i > 20 && i < 25) || i == 36 || i == 37 || i == 46)
                    {
                        GameObject.Instantiate(wave.enemyPrefab3, START.position, Quaternion.identity);
                        countEnemyAlive++;
                    }
                    else if ((i >= 16 && i <= 20) || (i > 28 && i <= 32) || (i >= 42 && i <= 45) || i == 49)
                    {
                        GameObject.Instantiate(wave.enemyPrefab4, START.position, Quaternion.identity);
                        countEnemyAlive++;
                    }
                    if (i != wave.count - 1)
                        yield return new WaitForSeconds(wave.del_time);//敌人产生间隔
                }else if(DateFile.SecondWorld == 2 || DateFile.SecondWorld == 4)
                {
                    int x = Random.Range(1, 70);
                    if (x < 25)
                    {
                        GameObject.Instantiate(wave.enemyPrefab1, START.position, Quaternion.identity);
                        countEnemyAlive++;
                    }
                    else if (x >= 25 && x < 40)
                    {
                        GameObject.Instantiate(wave.enemyPrefab2, START.position, Quaternion.identity);
                        countEnemyAlive++;
                    }
                    else if (x >= 40 && x < 55)
                    {
                        GameObject.Instantiate(wave.enemyPrefab3, START.position, Quaternion.identity);
                        countEnemyAlive++;
                    }
                    else if (x >= 55)
                    {
                        GameObject.Instantiate(wave.enemyPrefab4, START.position, Quaternion.identity);
                        countEnemyAlive++;
                    }
                    if (i != wave.count - 1)
                        yield return new WaitForSeconds(wave.del_time);//敌人产生间隔
                }
                else if (DateFile.SecondWorld == 1)
                {
                    int x = Random.Range(1, 75);
                    if (x < 30)
                    {
                        GameObject.Instantiate(wave.enemyPrefab1, START.position, Quaternion.identity);
                        countEnemyAlive++;
                    }
                    else if (x >= 30 && x < 45)
                    {
                        GameObject.Instantiate(wave.enemyPrefab2, START.position, Quaternion.identity);
                        countEnemyAlive++;
                    }
                    else if (x >= 45 && x < 60)
                    {
                        GameObject.Instantiate(wave.enemyPrefab3, START.position, Quaternion.identity);
                        countEnemyAlive++;
                    }
                    else if (x >= 60)
                    {
                        GameObject.Instantiate(wave.enemyPrefab4, START.position, Quaternion.identity);
                        countEnemyAlive++;
                    }
                    if (wavenum == 8 && i == wave.count - 1)
                    {
                        GameObject.Instantiate(wave.enemyPrefab5, START.position, Quaternion.identity);
                        countEnemyAlive++;
                    }
                    if (i != wave.count - 1)
                      yield return new WaitForSeconds(wave.del_time);//敌人产生间隔
                }
                
            }

            while (countEnemyAlive > 0)
            {
                yield return 0;
            }
            yield return new WaitForSeconds(del_time_waves);//所有敌人都销毁后，等待del_time_waves时间，下一波攻击
        }
        
        while (countEnemyAlive > 0)
        {
            yield return 0;//敌人生产完毕，没有死完的时候
        }
        GameManager.Instance.Win();
    }


}
