using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
public class MapCube : MonoBehaviour {
    [HideInInspector]
    public GameObject myturret;//保存当前Cube身上的炮台
    public TurretDate turretdate;//保存当前Cube身上的炮台的种类
    public GameObject buildeffect;
    private Renderer rendere;//得到材质

    void Start()
    {
        rendere = GetComponent<Renderer>();
    }
    
    public void BuildTurret(TurretDate turretdate)//建造一级炮塔
    {
        this.turretdate = turretdate;
        Vector3 pos = new Vector3();
        if(DateFile.SecondWorld == 1)
        {
            //调整位置
            if (turretdate.type == TurretType.FireTurret)
            {
                pos = new Vector3(0, 0, 0);
            }
            else
            {
                pos = new Vector3(0, -0.5f, 0);
            }
            myturret = GameObject.Instantiate(turretdate.turrentPrefabs, transform.position + pos, Quaternion.identity);//将炮塔实例化
            GameObject effect = GameObject.Instantiate(buildeffect, transform.position, Quaternion.identity);
            Destroy(effect, 1);
        }
        else if(DateFile.SecondWorld == 0)
        {
            if (turretdate.type == TurretType.FireTurret)
            {
                pos = new Vector3(0, 0.25f, 0);
            }
            else
            {
                pos = new Vector3(0, -0.1f, 0);
            }
            myturret = GameObject.Instantiate(turretdate.turrentPrefabs, transform.position + pos, Quaternion.identity);//将炮塔实例化
            GameObject effect = GameObject.Instantiate(buildeffect, transform.position, Quaternion.identity);
            Destroy(effect, 1);
        }
        else if(DateFile.SecondWorld == 2|| DateFile.SecondWorld == 4)
        {
            //调整位置
            if (turretdate.type == TurretType.FireTurret)
            {
                pos = new Vector3(0, 0, 0);
            }
            else
            {
                pos = new Vector3(0, -0.5f, 0);
            }
            myturret = GameObject.Instantiate(turretdate.turrentPrefabs, transform.position + pos, Quaternion.identity);//将炮塔实例化
            GameObject effect = GameObject.Instantiate(buildeffect, transform.position, Quaternion.identity);
            Destroy(effect, 1);
        }
    }
    //鼠标移动到cube上面的特效
    void OnMouseEnter()
    {
        if (myturret == null && EventSystem.current.IsPointerOverGameObject()==false)
        {
            rendere.material.color = Color.red;
        }
    }
    //鼠标离开cube的特效
    void OnMouseExit()
    {
        rendere.material.color = Color.white;
    }
    //升级炮塔
    public void UpgradeTurret()
    {
        Vector3 pos = new Vector3();
        if (turretdate.type == TurretType.FireTurret)
        {
            pos = new Vector3(0, 0.25f, 0);
        }
        else
        {
            pos = new Vector3(0, -0.1f, 0);
        }
        Destroy(myturret);
        myturret = GameObject.Instantiate(turretdate.turrentUpgradePrefabs, transform.position + pos, Quaternion.identity);
        GameObject effect = GameObject.Instantiate(buildeffect, transform.position, Quaternion.identity);
        Destroy(effect, 1.5f);
    }
    public void FinalTurret()
    {
        Vector3 pos = new Vector3();
        if (turretdate.type == TurretType.FireTurret)
        {
            pos = new Vector3(0, 0.25f, 0);
        }
        else
        {
            pos = new Vector3(0, -0.1f, 0);
        }
        Destroy(myturret);
        myturret = GameObject.Instantiate(turretdate.turrentFinalPrefabs, transform.position + pos, Quaternion.identity);
        GameObject effect = GameObject.Instantiate(buildeffect, transform.position, Quaternion.identity);
        Destroy(effect, 1.5f);
    }
    //摧毁炮塔
    public void DestroyTurret()
    {
        Destroy(myturret);
        GameObject effect = GameObject.Instantiate(buildeffect, transform.position, Quaternion.identity);
        Destroy(effect, 1.5f);
        //重置
        myturret = null;
        turretdate = null;
    }
}
