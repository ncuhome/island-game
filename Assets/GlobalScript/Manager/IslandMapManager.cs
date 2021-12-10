using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IslandMapManager
{
    public int islandWidth;
    public int islandHeight;
    const int MAX_ISLAND_LENGHT=30;
    public BuildingType[,] buildMap = new BuildingType[MAX_ISLAND_LENGHT, MAX_ISLAND_LENGHT];
    public BuildingScript[,] pBuildScript = new BuildingScript[MAX_ISLAND_LENGHT, MAX_ISLAND_LENGHT];


}