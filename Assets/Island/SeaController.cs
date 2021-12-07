using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SeaController : MonoBehaviour
{
    public GameObject map;
    public GameObject islandObj;
    MapPosBasement mapPosBasement;
    Manager.GameMapManager gameMapManager;
    /// <summary>
    /// 被高亮的岛屿标记
    /// </summary>
    List<IslandScript> interestIslandList;
    /// <summary>
    /// 初始海域长宽
    /// </summary>
    public const int START_LENGTH = 3;
    /// <summary>
    /// 未检测到存档文件，从0初始化
    /// </summary>
    void InitByStart() {
        gameMapManager = new Manager.GameMapManager(START_LENGTH, START_LENGTH);
        mapPosBasement = map.GetComponent<MapPosBasement>();
        mapPosBasement.mapWidth = mapPosBasement.mapHeight = START_LENGTH;
        mapPosBasement.ResetMapPos();
    }

    private void Start() {
        Manager.InstanceManager.InputInstance.singleTouch += new Manager.ScreenInputEvent(SeaControlTouchEvent);
    }

    public void SeaControlTouchEvent(Vector2 pos) {
        IslandScript tmp = gameMapManager.touchIsland(mapPosBasement.ScreenToMapPoint(pos));
        if (tmp != null) {
            interestIslandList.Add(tmp);
        }
        interestIslandList.RemoveAll(
            delegate(IslandScript island) {
                return !island.isInterestIsland;
            }
        );
        if (interestIslandList.Count >= 3) {
            gameMapManager.MixedIsland(interestIslandList);
        }
    }
}
