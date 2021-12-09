using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IslandMapManager
{
    public int islandWidth;
    public int islandHeight;
    const int MAX_ISLAND_LENGHT=30;
    //为什么要用这个东西而不是直接使用BuildScript数组存储地图？？
    //主要是为了存档,其次是为了加载岛屿
    public BuildType[,] buildMap = new BuildType[MAX_ISLAND_LENGHT, MAX_ISLAND_LENGHT];
    public BuildScript[,] pBuildScript = new BuildScript[MAX_ISLAND_LENGHT, MAX_ISLAND_LENGHT];


}