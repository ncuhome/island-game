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
    /// �������ĵ�����
    /// </summary>
    List<IslandScript> interestIslandList=new List<IslandScript>();
    /// <summary>
    /// ��ʼ���򳤿�
    /// </summary>
    public const int START_LENGTH = 3;
    /// <summary>
    /// δ��⵽�浵�ļ�����0��ʼ��
    /// </summary>
    void InitByStart() {
        gameMapManager = new Manager.GameMapManager(START_LENGTH, START_LENGTH);
        mapPosBasement = gameMap.GetComponent<MapPosBasement>();
        mapPosBasement.mapWidth = mapPosBasement.mapHeight = START_LENGTH;
        mapPosBasement.ResetMapPos();
    }

    void InitBySave() {
        SaveData sd = Saver.saveData;
        gameMapManager = new Manager.GameMapManager(sd.seaWidth, sd.seaHeight);
        mapPosBasement = gameMap.GetComponent<MapPosBasement>();
        mapPosBasement.mapWidth = sd.seaWidth;
        mapPosBasement.mapHeight = sd.seaHeight;
        mapPosBasement.ResetMapPos();
        foreach(IslandData id in sd.islandDatas) {
            GameObject tmp = Instantiate(islandObj);
            tmp.transform.parent = mapPosBasement.transform;
            IslandScript pTmp= tmp.GetComponent<IslandScript>();
            pTmp.islandType = id.islandType;
            gameMapManager.PlaceIsland(id.islandType, id.pos, tmp);
            pTmp.pIslandData = id;
        }
    }

    private void Start() {
        Manager.InstanceManager.InputInstance.singleTouch += new Manager.ScreenInputEvent(SeaControlTouchEvent);
        InitBySave();
    }

    private void Update() {
        if (islandObjInHand == null) {
            gameMapManager.SaveToData();
            islandObjInHand = Instantiate(islandObj);
            islandObjInHand.transform.parent = mapPosBasement.transform;
            islandObjInHand.SetActive(false);
        }
        if (Saver.saveData.power < Manager.GameMapManager.ISLAND_COST && !NoPowerLabel.activeSelf) {
            NoPowerLabel.SetActive(true);
        }
        else if (NoPowerLabel.activeSelf && Saver.saveData.power >= Manager.GameMapManager.ISLAND_COST) {
            NoPowerLabel.SetActive(false);
        }
    }

    public void gotoIslandScene() {
        if (interestIslandList.Count == 1) {
            gameMapManager.SaveToData();
            Saver.pNowIslandData = interestIslandList[0].pIslandData;
            SceneManager.LoadScene("Scenes/IslandScene");
        }
    }

    public void returnToMain() {
        Manager.InstanceManager.EffectInstance.DestroyHighLightByNum(Manager.GameMapManager.EF_NUM);
        gameMapManager.SaveToData();
        SceneManager.LoadScene("Scenes/StartScene");
    }
    
    public void SeaControlTouchEvent(Vector2 pos) {
        //��ֹ�ÿ�
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
        //������з���Ȥ����
        interestIslandList.RemoveAll(
            delegate(IslandScript island) {
                return !island.isInterestIsland;
            }
        );
        //print(interestIslandList.Count);
        if (interestIslandList.Count >= Manager.GameMapManager.MIN_MIXED_NUM) {
            gameMapManager.MixedIsland(interestIslandList);
            //ȫ�����
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
