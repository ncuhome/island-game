using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SeaController : MonoBehaviour
{
    public GameObject gameMap;
    public GameObject islandObj;
    public GameObject islandObjInHand;
    MapPosBasement mapPosBasement;
    Manager.GameMapManager gameMapManager;
    /// <summary>
    /// 被高亮的岛屿标记
    /// </summary>
    List<IslandScript> interestIslandList=new List<IslandScript>();
    /// <summary>
    /// 初始海域长宽
    /// </summary>
    public const int START_LENGTH = 3;
    /// <summary>
    /// 未检测到存档文件，从0初始化
    /// </summary>
    void InitByStart() {
        gameMapManager = new Manager.GameMapManager(START_LENGTH, START_LENGTH);
        mapPosBasement = gameMap.GetComponent<MapPosBasement>();
        mapPosBasement.mapWidth = mapPosBasement.mapHeight = START_LENGTH;
        mapPosBasement.ResetMapPos();
    }

    void InitBySave() {
        SaveDate sd = Saver.saveDate;
        gameMapManager = new Manager.GameMapManager(sd.seaWidth, sd.seaHeight);
        mapPosBasement = gameMap.GetComponent<MapPosBasement>();


    }

    private void Start() {
        Manager.InstanceManager.InputInstance.singleTouch += new Manager.ScreenInputEvent(SeaControlTouchEvent);
        if (GameStateManager.instance.isLoadDate) {

        }
        else {
            InitByStart();
        }
        
    }

    private void Update() {
        if (islandObjInHand == null) {
            islandObjInHand = Instantiate(islandObj);
            islandObjInHand.transform.parent = mapPosBasement.transform;
            islandObjInHand.SetActive(false);
        }
    }

    public void SeaControlTouchEvent(Vector2 pos) {
        //防止置空
        if (islandObjInHand == null) return;
        IslandScript tmp = gameMapManager.touchIsland(mapPosBasement.ScreenToMapPoint(pos),islandObjInHand);
        if (tmp != null) {
            if(tmp.gameObject==islandObjInHand) {
                islandObjInHand = null;
                gameMapManager.UpdateEffectByController(gameMap.transform);
                return;
            }
            interestIslandList.Add(tmp);
        } else {
            foreach(IslandScript i in interestIslandList) {
                i.isInterestIsland = false;
            }
        }
        //清除所有非兴趣岛屿
        interestIslandList.RemoveAll(
            delegate(IslandScript island) {
                return !island.isInterestIsland;
            }
        );
        //print(interestIslandList.Count);
        if (interestIslandList.Count >= Manager.GameMapManager.MIN_MIXED_NUM) {
            gameMapManager.MixedIsland(interestIslandList);
            //全部清除
            interestIslandList.RemoveAll(
                delegate(IslandScript island) {
                    return true;
                }
            );
        }
        gameMapManager.UpdateEffectByController(gameMap.transform);
    }
}
