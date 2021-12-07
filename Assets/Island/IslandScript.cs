using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IslandScript : MonoBehaviour
{
    //TODO 岛屿上建筑管理占位符
    public IslandMapManager islandMapManager;
    public IslandType islandType=IslandType.SMALL_ISLAND;
    public Sprite[] islandSprite;
    SpriteRenderer spriteRenderer;

    private void Start() {
        spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
        if (spriteRenderer == null) {
            spriteRenderer = gameObject.AddComponent<SpriteRenderer>();
        }
    }

    public void UpdateByManager() {
        spriteRenderer.sprite = islandSprite[(int)islandType - 1];
    }

    //TODO
    public void MixedAsMain(IslandScript islandA,IslandScript islandB) {

    }
}
