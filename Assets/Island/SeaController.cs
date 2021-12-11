using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SeaController : MonoBehaviour
{
    public GameObject gameMap;
    public GameObject islandObj;
    public GameObject islandObjInHand;
    public GameObject gotoIslandButton;
    public GameObject NoPowerLabel;
    public float timer;
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
        mapPosBasement.mapWidth = sd.seaWidth;
        mapPosBasement.mapHeight = sd.seaHeight;
        mapPosBasement.ResetMapPos();
        foreach(IslandDate id in sd.islandDates) {
            GameObject tmp = Instantiate(islandObj);
            tmp.transform.parent = mapPosBasement.transform;
            IslandScript pTmp= tmp.GetComponent<IslandScript>();
            pTmp.islandType = id.islandType;
            gameMapManager.PlaceIsland(id.islandType, id.pos, tmp);
            pTmp.pIslandDate = id;
        }
    }

    private void Start() {
        Manager.InstanceManager.InputInstance.singleTouch += new Manager.ScreenInputEvent(SeaControlTouchEvent);
        InitBySave();
    }

    private void Update() {
        if (islandObjInHand == null) {
            gameMapManager.SaveToDate();
            islandObjInHand = Instantiate(islandObj);
            islandObjInHand.transform.parent = mapPosBasement.transform;
            islandObjInHand.SetActive(false);
        }
        if (Saver.saveDate.power < Manager.GameMapManager.ISLAND_COST && !NoPowerLabel.activeSelf) {
            NoPowerLabel.SetActive(true);
        }
        else if (NoPowerLabel.activeSelf && Saver.saveDate.power >= Manager.GameMapManager.ISLAND_COST) {
            NoPowerLabel.SetActive(false);
        }
    }

    public void gotoIslandScene() {
        if (interestIslandList.Count == 1) {
            gameMapManager.SaveToDate();
            Saver.pNowIslandDate = interestIslandList[0].pIslandDate;
            SceneManager.LoadScene("Scenes/IslandScene");
        }
    }

    public void returnToMain() {
        Manager.InstanceManager.EffectInstance.DestroyHighLightByNum(Manager.GameMapManager.EF_NUM);
        gameMapManager.SaveToDate();
        SceneManager.LoadScene("Scenes/StartScene");
    }
    
    public void SeaControlTouchEvent(Vector2 pos) {
        //防止置空
        if (islandObjInHand == null) return;
        bool isOutRange;
        IslandScript tmp = gameMapManager.touchIsland(mapPosBasement.ScreenToMapPoint(pos),islandObjInHand,out isOutRange);
        if (tmp != null) {
            if(tmp.gameObject==islandObjInHand) {
                islandObjInHand = null;
                gameMapManager.UpdateEffectByController(gameMap.transform);
                return;
            }
            interestIslandList.Add(tmp);
        } else if(!isOutRange){
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
        if (interestIslandList.Count == 1) {
            gotoIslandButton.SetActive(true);
        } else {
            gotoIslandButton.SetActive(false);
        }
        gameMapManager.UpdateEffectByController(gameMap.transform);
    }
}
