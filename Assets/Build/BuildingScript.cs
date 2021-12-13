using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingScript : MonoBehaviour {
    public Sprite[] buildingSprite;
    public BuildingType buildingType;
    public bool isInterestBuilding;
    SpriteRenderer spriteRenderer;


    /// <summary>
    /// 由管理器调用的Update
    /// </summary>
    /// <param name="x"></param>
    /// <param name="y"></param>
    public void UpdateByManager(int x, int y) {
        if (spriteRenderer == null) spriteRenderer=gameObject.GetComponent<SpriteRenderer>();
        transform.localPosition = new Vector3(x, y);
        transform.localScale = Vector3.one;
        spriteRenderer.sprite = buildingSprite[GetSpriteIdByBuildType(buildingType)];
    }

    /// <summary>
    /// 作为主建筑合成
    /// </summary>
    /// <param name="a"></param>
    /// <param name="b"></param>
    /// <param name="mixToWorkshop"></param>
    public void MixedAsMain(BuildingScript a,BuildingScript b,bool mixToWorkshop) {
        buildingType = GetNextBuildingType(buildingType,mixToWorkshop);
        UpdateByManager((int)transform.localPosition.x,(int)transform.localPosition.y);
        Destroy(a.gameObject);
        Destroy(b.gameObject);
    }

    public void UpdateByManager(Vector2Int pos) {
        UpdateByManager(pos.x, pos.y);
    }
    //等梁老师图
    static int GetSpriteIdByBuildType(BuildingType buildingType) {
        int bt = (int)buildingType;
        if (buildingType == BuildingType.EMPTY) return 10;
        if (bt < 1024) {
            return bt - 1;
        }
        if (bt < 2048) {
            return bt - 1024 + 5;
        }
        return bt-2048+1;
    }

    /*
     具体数值见数值附件
     */
    
    static public int BuildingPowerOutput(BuildingType buildingType) {
        switch (buildingType) {
            case BuildingType.EMPTY:
                return 0;
            case BuildingType.BASIC_BUILDING:
                return 3;
            case BuildingType.LEVEL1_HOUSE:
                return 15;
            case BuildingType.LEVEL2_HOUSE:
                return 50;
            case BuildingType.LEVEL3_HOUSE:
                return 120;
            case BuildingType.LEVEL4_HOUSE:
                return 200;
            default:
                return 0;
        }
    }

    static public int GetDestroyMoney(BuildingType buildingType) {
        switch (buildingType) {
            case BuildingType.BARRIER:
                return 500;
            case BuildingType.BASIC_BUILDING:
                return 5;
            case BuildingType.LEVEL1_HOUSE:
                return 20;
            case BuildingType.LEVEL1_WORKSHOP:
                return 30;
            case BuildingType.LEVEL2_WORKSHOP:
                return 100;
            case BuildingType.LEVEL2_HOUSE:
                return 80;
            case BuildingType.LEVEL3_WORKSHOP:
                return 300;
            case BuildingType.LEVEL3_HOUSE:
                return 200;
            case BuildingType.LEVEL4_HOUSE:
                return 400;
            case BuildingType.LEVEL4_WORKSHOP:
                return 600;
            default:
                return 0;
        }
    }

    static public int BuildingGoldOutput(BuildingType buildingType) {
        switch (buildingType) {
            case BuildingType.EMPTY:
                return 0;
            case BuildingType.BASIC_BUILDING:
                return 3;
            case BuildingType.LEVEL1_WORKSHOP:
                return 15;
            case BuildingType.LEVEL2_WORKSHOP:
                return 40;
            case BuildingType.LEVEL3_WORKSHOP:
                return 100;
            case BuildingType.LEVEL4_WORKSHOP:
                return 150;
            default:
                return 0;
        }
    }
    
    static public bool CanMixed(BuildingType a, BuildingType b) {
        if (a == BuildingType.BARRIER || b == BuildingType.BARRIER) return false;
        return a == b;
    }

    static public BuildingType GetNextBuildingType(BuildingType buildingType,bool mixToWorkshop) {
        switch (buildingType) {
            case BuildingType.BASIC_BUILDING:
                if (mixToWorkshop) return BuildingType.LEVEL1_WORKSHOP;
                else return BuildingType.LEVEL1_HOUSE;
            case BuildingType.LEVEL1_HOUSE:
            case BuildingType.LEVEL2_HOUSE:
            case BuildingType.LEVEL3_HOUSE:
            case BuildingType.LEVEL1_WORKSHOP:
            case BuildingType.LEVEL2_WORKSHOP:
            case BuildingType.LEVEL3_WORKSHOP:
                return buildingType + 1;
            default:
                return BuildingType.EMPTY;
        }
    }
}
