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
    public List<BuildDate> buildDates=new List<BuildDate>();
}

[SerializeField]
public class BuildDate {
    public Vector2Int pos;
    public BuildType buildType;
}