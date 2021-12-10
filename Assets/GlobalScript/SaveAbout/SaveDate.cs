using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[SerializeField]
public class SaveDate
{
    public int seaWidth;
    public int seaHeight;
    public System.UInt64 lastTickTime;
    public System.Int64 power;
    public System.Int64 gold;
    public List<IslandDate> islandDates=new List<IslandDate>();
}

[SerializeField]
public class IslandDate {
    public Vector2Int pos;
    public IslandType islandType;
    public List<BuildingDate> buildingDates=new List<BuildingDate>();
}

[SerializeField]
public class BuildingDate {
    public Vector2Int pos;
    public BuildingType buildingType;
}