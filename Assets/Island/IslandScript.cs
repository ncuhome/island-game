using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IslandScript : MonoBehaviour
{
    //TODO 岛屿上建筑管理占位符
    public IslandMapManager islandMapManager;
    public IslandType islandType=IslandType.SMALL_ISLAND;
    public Sprite[] islandSprite;
    public bool isInterestIsland = false;
    SpriteRenderer spriteRenderer;

    private void Start() {
        spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
        if (spriteRenderer == null) {
            spriteRenderer = gameObject.AddComponent<SpriteRenderer>();
        }
    }
    /// <summary>
    /// 由Manager更新的时候调用
    /// </summary>
    public void UpdateByManager() {
        spriteRenderer.sprite = islandSprite[(int)islandType - 1];
    }
    public Vector2Int GetIslandPosInMap() {
        return new Vector2Int((int)(transform.localPosition.x + 0.5f), (int)(transform.localPosition.y + 0.5f));
    }
    //TODO 没写完！没写完！没写完！！！！
    public void MixedAsMain(IslandScript islandA,IslandScript islandB) {
        islandType = Manager.GameMapManager.getNextIslandType(islandType);
    }
}
