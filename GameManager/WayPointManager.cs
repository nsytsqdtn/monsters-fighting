using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WayPointManager : MonoBehaviour {
    public Transform[] positions;//每一个路径点的位置
	void Awake () {
        positions = new Transform[transform.childCount]; //得到所有孩子结点的索引
        for(int i = 0; i < positions.Length; i++)
        {
            positions[i] = transform.GetChild(i); //给每个子结点赋值
        }
	}
}
