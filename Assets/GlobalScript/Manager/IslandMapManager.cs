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
    public BuildingScript[,] pBuildingScript = new BuildingScript[MAX_ISLAND_LENGHT, MAX_ISLAND_LENGHT];

    /// <summary>
    /// 保存数据(没有必要的话，别一直用)
    /// </summary>
    public void SaveToDate() {
        islandDate.buildingDates.Clear();
        for(int i=0;i<islandWidth;++i)
            for(int r = 0; r < islandHeight; ++r) {
                if (pBuildingScript[i, r] != null) {
                    islandDate.buildingDates.Add(
                        new BuildingDate(
                            pBuildingScript[i, r].transform.position,
                            pBuildingScript[i, r].buildingType
                            )
                        );
                }
            }
    }
    /// <summary>
    /// 加载数据
    /// </summary>
    public void LoadByDate() {
        islandWidth = islandDate.pos.x;
        islandHeight = islandDate.pos.y;
        mapPosBasement.mapWidth = islandWidth;
        mapPosBasement.mapHeight = islandHeight;
        mapPosBasement.ResetMapPos();
        foreach (BuildingDate bd in islandDate.buildingDates) {
            GameObject tmp = GameObject.Instantiate(buildingObj);
            BuildingScript bs = pBuildingScript[bd.pos.x, bd.pos.y] = tmp.GetComponent<BuildingScript>();
            bs.transform.parent = mapPosBasement.transform;
            bs.UpdateByManager(bd.pos);
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



}