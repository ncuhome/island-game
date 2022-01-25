using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[Serializable]
public class SaveData
{
    public int seaWidth;
    public int seaHeight;
    public System.Int64 lastTickTime;
    public double power;
    public double gold;
    public List<IslandData> islandDatas=new List<IslandData>();
    public BuildingType nextBuildingType;
    public SaveData() {
        seaHeight = seaWidth = 3;
        lastTickTime = (long)(DateTime.Now - Saver.timeZero).TotalSeconds;
        power = 2000;
        gold = 1000;
        nextBuildingType = BuildingType.BASIC_BUILDING;
    }
}

[Serializable]
public class IslandData {
    public IslandData(Vector2Int pos,IslandType islandType) {
        this.pos = pos;
        this.islandType = islandType;
    }
    public Vector2Int pos;
    public IslandType islandType;
    public List<BuildingData> buildingDatas=new List<BuildingData>();
}

[Serializable]
public class BuildingData {
    public BuildingData(Vector2Int pos,BuildingType buildingType) {
        this.pos = pos;
        this.buildingType = buildingType;
    }
    public BuildingData(Vector3 pos,BuildingType buildingType) {
        this.pos = new Vector2Int((int)pos.x, (int)pos.y);
        this.buildingType = buildingType;
    }
    public Vector2Int pos;
    public BuildingType buildingType;
}