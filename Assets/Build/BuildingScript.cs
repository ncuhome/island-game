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
        if (spriteRenderer == null) gameObject.GetComponent<SpriteRenderer>();
        transform.localPosition = new Vector3(x, y);
        transform.localScale = Vector3.one;
        spriteRenderer.sprite = buildingSprite[GetSpriteIdByBuildType(buildingType)];
    }

    public void UpdateByManager(Vector2Int pos) {
        UpdateByManager(pos.x, pos.y);
    }

    static int GetSpriteIdByBuildType(BuildingType buildingType) {
        //TODO
        return 0;
    }

    static public int BuildingPowerOutput(BuildingType buildingType) {
        switch (buildingType) {
            case BuildingType.EMPTY:
                return 0;
            default:
                return 0;
        }
    }

    static public int BuildingGoldOutput(BuildingType buildingType) {
        switch (buildingType) {
            case BuildingType.EMPTY:
                return 0;
            default:
                return 0;
        }
    }

    static public bool IsSame(BuildingType a, BuildingType b) {
        return a == b;
    }
}
