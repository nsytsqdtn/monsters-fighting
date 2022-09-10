using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class TurretDate{
    
    public GameObject turrentPrefabs;//一级炮塔
    public int cost;//一级炮塔花费金额
    public GameObject turrentUpgradePrefabs;//二级炮塔
    public int costUpgrade;//二级炮塔花费金额
    public GameObject turrentFinalPrefabs;//三级炮塔
    public int costFinal;//三级炮塔花费金额
    public TurretType type;//炮塔种类
}
public enum TurretType
{
    LaserTurret,
    MissileTurret,
    NormalTurret,
    FireTurret
}