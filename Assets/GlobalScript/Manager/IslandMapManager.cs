using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IslandMapManager:MonoBehaviour
{
    public GameObject buildingObj;
    public MapPosBasement mapPosBasement;
    public IslandDate islandDate;
    public int islandWidth;
    public int islandHeight;
    const int MAX_ISLAND_LENGHT=30;
    const int MIN_MIXED_NUM = 3;
    const int EF_NUM = 424233;
    public BuildingScript[,] pBuildingScript = new BuildingScript[MAX_ISLAND_LENGHT, MAX_ISLAND_LENGHT];
    public Vector2Int interestPos=new Vector2Int(-1,-1);
    public bool isMixToWorkShop = false;
    public BuildingType nextBuilding;


    private void Start() {
        LoadByDate();
        Manager.InstanceManager.InputInstance.singleTouch += this.BuildingTouchEvent;
    }

    private void OnDestroy() {
        SaveToDate();
        Manager.InstanceManager.InputInstance.singleTouch -= this.BuildingTouchEvent;
    }
    /// <summary>
    /// 保存数据
    /// </summary>
    public void SaveToDate() {
        Saver.saveDate.nextBuildingType = nextBuilding;
        islandDate.buildingDates.Clear();
        for(int i=0;i<islandWidth;++i)
            for(int r = 0; r < islandHeight; ++r) {
                if (pBuildingScript[i, r] != null) {
                    islandDate.buildingDates.Add(
                        new BuildingDate(
                            pBuildingScript[i, r].transform.localPosition,
                            pBuildingScript[i, r].buildingType
                            )
                        );
                }
            }
    }

    void setBuilding(Vector2Int pos, BuildingType buildingType) {
        GameObject tmp = GameObject.Instantiate(buildingObj);
        BuildingScript bs = pBuildingScript[pos.x, pos.y] = tmp.GetComponent<BuildingScript>();
        bs.buildingType = buildingType;
        bs.transform.parent = mapPosBasement.transform;
        bs.UpdateByManager(pos);
    }

    void GetNextSetBuilding() {
        int rand=Random.Range(0, 100);
        if (rand < 80) {
            nextBuilding = BuildingType.BASIC_BUILDING;
        }else if (rand < 88) {
            nextBuilding = BuildingType.LEVEL1_HOUSE;
        }else if (rand < 92) {
            nextBuilding = BuildingType.BARRIER;
        } else {
            nextBuilding = BuildingType.LEVEL1_WORKSHOP;
        }
        Saver.saveDate.nextBuildingType = nextBuilding;
    }
    /// <summary>
    /// 加载数据
    /// </summary>
    
    void SetEFInList(List<Vector2Int> list) {
        Manager.InstanceManager.EffectInstance.DestroyHighLightByNum(EF_NUM);
        foreach(Vector2Int pos in list) {
            GameObject tmp = Manager.InstanceManager.EffectInstance.GetHighLightByNum(EF_NUM);
            tmp.transform.parent = mapPosBasement.transform;
            tmp.transform.localScale = Vector3.one;
            tmp.transform.localPosition = new Vector3(pos.x, pos.y, -1);
        }
    }

    public void LoadByDate() {
        islandDate = Saver.pNowIslandDate;
        nextBuilding = Saver.saveDate.nextBuildingType;
        int len=1;
        switch (islandDate.islandType) {
            case IslandType.SMALL_ISLAND:
                len = 4;
                break;
            case IslandType.MEDIUM_ISLAND:
                len = 5;
                break;
            case IslandType.LARGE_ISLAND:
                len = 6;
                break;
        }
        islandWidth = len;
        islandHeight = len;
        mapPosBasement.mapWidth = islandWidth;
        mapPosBasement.mapHeight = islandHeight;
        mapPosBasement.ResetMapPos();
        mapPosBasement.SpawnSquare();
        foreach (BuildingDate bd in islandDate.buildingDates) {
            setBuilding(bd.pos, bd.buildingType);
        }
    }
    /// <summary>
    /// 使用IslandDate加载数据
    /// </summary>
    /// <param name="islandDate"></param>
    public void LoadByDate(IslandDate islandDate) {
        this.islandDate = islandDate;
        LoadByDate();
    }

    public void BuildingTouchEvent(Vector2 pos) {
        Vector2Int intPos = mapPosBasement.ScreenToMapPoint(pos);
        if (intPos.x >= islandWidth || intPos.x < 0 || intPos.y >= islandHeight || intPos.y < 0) return;
        //兴趣点不等于点击点 取消兴趣点
        if (intPos != interestPos) {
            interestPos = intPos;
            List<Vector2Int> list;//特效点位
            BuildingType finallyBuilding;
            if (MixedIsAllow(nextBuilding, intPos, out list, out finallyBuilding)) {
                list.Add(intPos);
                SetEFInList(list);
            } else {
                list = new List<Vector2Int>();
                list.Add(intPos);
                SetEFInList(list);
            }
            
        }
        //兴趣点等于点击点
        else {
            //如果是空地（为之后道具预留不是空地）
            if (pBuildingScript[intPos.x, intPos.y] == null) {
                List<Vector2Int> list;
                BuildingType finallyBuilding;
                if (MixedIsAllow(nextBuilding, intPos, out list, out finallyBuilding)) {
                    foreach(Vector2Int ipos in list) {
                        Destroy(pBuildingScript[ipos.x, ipos.y].gameObject);
                        pBuildingScript[ipos.x, ipos.y] = null;
                    }
                    setBuilding(intPos, finallyBuilding);
                } else {
                    setBuilding(intPos, nextBuilding);
                }
                Manager.InstanceManager.EffectInstance.DestroyHighLightByNum(EF_NUM);
                GetNextSetBuilding();
                SaveToDate();
            }
        }
    }




    /// <summary>
    /// 检测在某坐标处是否允许合成建筑
    /// </summary>
    /// <param name="building">建筑类型</param>
    /// <param name="pos">坐标</param>
    /// <param name="list">如果允许合成，则返回一个该合成中会使用的建筑坐标列表，如果不允许，则不保证返回值</param>
    /// <param name="finallyBuilding">如果允许合成，则返回该合成最终会生成的建筑类型</param>
    /// <returns>是否允许合成建筑</returns>
    /// 
    public bool MixedIsAllow(BuildingType building, Vector2Int pos, out List<Vector2Int> list, out BuildingType finallyBuilding) {
        Queue<Vector2Int> queue = new Queue<Vector2Int>();
        bool[,] gameMapMemory = new bool[islandWidth, islandHeight];
        queue.Enqueue(pos);
        //搜索顺序
        Vector2Int[] posMove = { Vector2Int.up, Vector2Int.right, Vector2Int.down, Vector2Int.left };
        list = new List<Vector2Int>();
        while (queue.Count > 0) {
            Vector2Int nowPos = queue.Dequeue();
            foreach (Vector2Int move in posMove) {
                nowPos += move;
                if (nowPos.x >= 0 &&
                    nowPos.x < islandWidth &&
                    nowPos.y >= 0 &&
                    nowPos.y < islandHeight &&
                    gameMapMemory[nowPos.x, nowPos.y] == false &&
                    pBuildingScript[nowPos.x,nowPos.y]!=null) {
                    if (BuildingScript.CanMixed(pBuildingScript[nowPos.x, nowPos.y].buildingType, building)) {
                        queue.Enqueue(nowPos);
                        list.Add(nowPos);
                        gameMapMemory[nowPos.x, nowPos.y] = true;
                    }
                }
                nowPos -= move;
            }
        }
        if (list.Count >= MIN_MIXED_NUM - 1) {
            List<Vector2Int> tmp;
            finallyBuilding = BuildingScript.GetNextBuildingType(building,isMixToWorkShop);
            BuildingType nxtfinally;
            if (MixedIsAllow(BuildingScript.GetNextBuildingType(building, isMixToWorkShop), pos, out tmp, out nxtfinally)) {
                list.AddRange(tmp);
                finallyBuilding = nxtfinally;
            }
            return true;
        } else {
            list = null;
            finallyBuilding = BuildingType.EMPTY;
            return false;
        }
    }
}