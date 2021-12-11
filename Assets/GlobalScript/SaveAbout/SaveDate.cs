using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[Serializable]
public class SaveDate
{
    public int seaWidth;
    public int seaHeight;
    public System.UInt64 lastTickTime;
    public System.Int64 power;
    public System.Int64 gold;
    public List<IslandDate> islandDates=new List<IslandDate>();
    public BuildingType nextBuildingType;
    public SaveDate() {
        seaHeight = seaWidth = 3;
        lastTickTime = (ulong)DateTime.Now.Ticks;
        power = 2000;
        gold = 1000;
        nextBuildingType = BuildingType.BASIC_BUILDING;
    }
}

[Serializable]
public class IslandDate {
    public IslandDate(Vector2Int pos,IslandType islandType) {
        this.pos = pos;
        this.islandType = islandType;
    }
    public Vector2Int pos;
    public IslandType islandType;
    public List<BuildingDate> buildingDates=new List<BuildingDate>();
}

[Serializable]
public class BuildingDate {
    public BuildingDate(Vector2Int pos,BuildingType buildingType) {
        this.pos = pos;
        this.buildingType = buildingType;
    }
    public BuildingDate(Vector3 pos,BuildingType buildingType) {
        this.pos = new Vector2Int((int)pos.x, (int)pos.y);
        this.buildingType = buildingType;
    }
    public Vector2Int pos;
    public BuildingType buildingType;
}