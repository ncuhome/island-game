using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Manager {
    /// <summary>
    /// 游戏地图管理器，线程非安全
    /// </summary>
    public class GameMapManager
    {
        public int mapWidth { get; private set; } = 3;
        public int mapHeight { get; private set; } = 3;
        const int MAX_MAP_LENGHT = 30;
        /// <summary>
        /// 最小岛屿合成数量
        /// </summary>
        public const int MIN_MIXED_NUM = 3;
        IslandType[,] gameMap = new IslandType[MAX_MAP_LENGHT,MAX_MAP_LENGHT];
        public IslandScript[,] pIslandObj = new IslandScript[MAX_MAP_LENGHT,MAX_MAP_LENGHT];

        /// <summary>
        /// 
        /// </summary>
        /// <param name="mapWidth"></param>
        /// <param name="mapHeight"></param>
        public GameMapManager(int mapWidth,int mapHeight) {
            if (mapHeight > MAX_MAP_LENGHT || mapWidth > MAX_MAP_LENGHT) {
                //超限
                Debug.LogError("out of MAX_MAP_LENGHT");
            }
            this.mapHeight = mapHeight;
            this.mapWidth = mapWidth;
        }
        /// <summary>
        /// 根据gameMap更新pIslandObj
        /// </summary>
        private void UpdateIslandGameObject() {
            for(int i = 0; i < mapWidth; i++) {
                for(int r = 0; r < mapHeight; r++) {
                    
                }
            }
        }

        /// <summary>
        /// 于坐标处放置岛屿
        /// </summary>
        /// <param name="island">岛屿类型</param>
        /// <param name="pos">坐标</param>
        /// <returns>是否成功放置岛屿</returns>
        public bool PlaceIsland(IslandType island, Vector2Int pos) {
            if (gameMap[pos.x, pos.y] != IslandType.EMPTY) {
                return false;
            }
            gameMap[pos.x, pos.y] = island;
            return true;
        }

        /// <summary>
        /// 摧毁岛屿
        /// </summary>
        /// <param name="pos">被摧毁岛屿的坐标</param>
        public void DestroyIsland(Vector2Int pos) {
            gameMap[pos.x, pos.y] = IslandType.EMPTY;
        }

        /// <summary>
        /// 合成岛屿
        /// </summary>
        /// <param name="list">被合成的岛屿的坐标链表,合成后的岛屿将会置于最后一个岛屿上</param>
        /// <returns>是否成功合成</returns>
        //如果MIN_MIXED_NUM>3，把这个方法改成并查集实现
        public bool MixedIsland(List<Vector2Int> list) {
            if (list.Count != 3) return false;
            for(int i = 0; i < MIN_MIXED_NUM; ++i) {
                for (int r = 0; r < MIN_MIXED_NUM; ++r) {
                    Vector2Int pos = list[i] - list[r];
                    if (!(pos == Vector2Int.up ||
                        pos == Vector2Int.down ||
                        pos == Vector2Int.left ||
                        pos == Vector2Int.right)) {
                        return false;
                    }
                }     
            }
            Vector2Int t = list[MIN_MIXED_NUM - 1];
            gameMap[t.x, t.y] = getNextIslandType(gameMap[t.x,t.y]);
            for(int i = 0; i < MIN_MIXED_NUM-1; ++i) {
                gameMap[list[i].x, list[i].y] = IslandType.EMPTY;
            }
            return true;
        }

        /// <summary>
        /// 检测在某坐标处是否允许合成岛屿
        /// </summary>
        /// <param name="island">岛屿类型</param>
        /// <param name="pos">坐标</param>
        /// <param name="list">如果允许合成，则返回一个该合成中会使用的岛屿坐标列表，如果不允许，则不保证返回值</param>
        /// <param name="finalIslandType">如果允许合成，则返回该合成最终会生成的岛屿类型</param>
        /// <returns>是否允许合成岛屿</returns>

        public bool MixedIsAllow(IslandType island,Vector2Int pos,out List<Vector2Int> list,out IslandType finalIslandType) {
            Queue<Vector2Int> queue = new Queue<Vector2Int>();
            bool[,] gameMapMemory = new bool[mapWidth, mapHeight];
            queue.Enqueue(pos);
            //搜索顺序
            Vector2Int[] posMove = { Vector2Int.up, Vector2Int.right, Vector2Int.down, Vector2Int.left };
            list = new List<Vector2Int>();
            while (queue.Count>0) {
                Vector2Int nowPos = queue.Dequeue();
                foreach(Vector2Int move in posMove) {
                    nowPos += move;
                    if (nowPos.x >= 0 &&
                        nowPos.x < mapWidth &&
                        nowPos.y >= 0 &&
                        nowPos.y < mapHeight &&
                        gameMapMemory[nowPos.x, nowPos.y] == false) {
                        if (canMixed(gameMap[nowPos.x, nowPos.y], island)) {
                            queue.Enqueue(nowPos);
                            list.Add(nowPos);
                            gameMapMemory[nowPos.x, nowPos.y] = true;
                        }
                    }
                    nowPos -= move;
                }
            }
            if (list.Count >= MIN_MIXED_NUM - 1) {
                List<Vector2Int> tmp;
                finalIslandType = getNextIslandType(island);
                if (MixedIsAllow(getNextIslandType(island), pos,out tmp,out finalIslandType)) {
                    list.AddRange(tmp);
                }
                return true;
            } else {
                list = null;
                finalIslandType = IslandType.EMPTY;
                return false;
            }
        }
        
        /// <summary>
        /// 判断两个岛屿是否可以进行合成（临时）
        /// </summary>
        /// <param name="a">第一个岛屿</param>
        /// <param name="b">第二个岛屿</param>
        /// <returns>是否可以合成</returns>
        private bool canMixed(IslandType a,IslandType b) {
            if(a>b) {
                IslandType tmp = b;
                b = a;
                a = tmp;
            }
            return a == b;
        }

        /// <summary>
        /// 获取下一个岛屿等级(临时）
        /// </summary>
        /// <param name="islandType">当前岛屿等级</param>
        /// <returns>下一个岛屿等级</returns>
        private IslandType getNextIslandType(IslandType islandType) {
            if (islandType == IslandType.LARGE_ISLAND) return IslandType.EMPTY;
            return islandType + 1;
        }
    }
}


