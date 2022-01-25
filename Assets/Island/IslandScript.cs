using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IslandScript : MonoBehaviour
{
    public IslandData pIslandData;
    public IslandType islandType=IslandType.SMALL_ISLAND;
    public Sprite[] islandSprite;
    public bool isInterestIsland = false;
    SpriteRenderer spriteRenderer;

    /// <summary>
    /// 由Manager更新的时候调用
    /// </summary>
    public void UpdateByManager(int x,int y) {
        if (spriteRenderer == null) {
            spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
        }
        transform.localScale =Vector3.one;
        transform.localPosition = new Vector3(x, y);
        if (islandType == IslandType.EMPTY) spriteRenderer.sprite = null;
        else spriteRenderer.sprite = islandSprite[(int)islandType - 1];
    }

    public void InitBySave() {

    }

    public Vector2Int GetIslandPosInMap() {
        return new Vector2Int((int)(transform.localPosition.x + 0.5f), (int)(transform.localPosition.y + 0.5f));
    }

    public int DestroyIslandGold() {
        int sum = 0;
        foreach (BuildingData bd in pIslandData.buildingDatas) {
            sum += BuildingScript.GetDestroyMoney(bd.buildingType);
        }
        return sum;
    }

    /// <summary>
    /// 作为主岛进行合并
    /// </summary>
    /// <param name="islandA"></param>
    /// <param name="islandB"></param>
    public void MixedAsMain(IslandScript islandA,IslandScript islandB) {
        Saver.saveData.gold += islandA.DestroyIslandGold() + islandB.DestroyIslandGold()+DestroyIslandGold();
        islandType = Manager.GameMapManager.getNextIslandType(islandType);
        pIslandData = new IslandData(pIslandData.pos, islandType);
        islandA.isInterestIsland = islandB.isInterestIsland = isInterestIsland = false;
    }
}
