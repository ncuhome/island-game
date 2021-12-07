using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SeaController : MonoBehaviour
{
    public GameObject map;
    MapPosBasement mapPosBasement;
    Manager.GameMapManager gameMapManager;
    List<GameObject> islandList;
    /// <summary>
    /// 被高亮的岛屿标记
    /// </summary>
    List<GameObject> interestIslandList;
    /// <summary>
    /// 初始海域长宽
    /// </summary>
    public const int START_LENGTH = 3;
    /// <summary>
    /// 未检测到存档文件，从0初始化
    /// </summary>
    void InitByStart() {
        mapPosBasement = map.GetComponent<MapPosBasement>();
        mapPosBasement.mapWidth = mapPosBasement.mapHeight = START_LENGTH;
        mapPosBasement.ResetMapPos();

    }
}
