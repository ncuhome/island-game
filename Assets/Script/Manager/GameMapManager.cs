using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Manager {
    /// <summary>
    /// 游戏地图管理器，线程非安全
    /// </summary>
    public class GameMapManager
    {
        const int MAP_WIDTH = 7;
        const int MAP_HEIGHT = 7;
        const int MIN_MIXED_NUM = 3;
        IslandType[,] gameMap;

        public GameMapManager() {
            gameMap = new IslandType[MAP_WIDTH,MAP_HEIGHT];
        }

        /// <summary>
        /// 于坐标处放置岛屿
        /// </summary>
        /// <param name="island">岛屿类型</param>
        /// <param name="pos">坐标</param>
        /// <returns>是否成功放置岛屿</returns>
        public bool PlaceIsland(IslandType island,Vector2Int pos) {
            if (gameMap[pos.x, pos.y] != IslandType.EMPTY) {
                return false;
            }
            gameMap[pos.x, pos.y] = island;
            //TODO
            return true;
        }
        
        /// <summary>
        /// 检测在某坐标处是否允许合成岛屿
        /// </summary>
        /// <param name="island">岛屿类型</param>
        /// <param name="pos">坐标</param>
        /// <param name="list">如果允许合成，则返回一个该合成中会使用的岛屿坐标列表，如果不允许，则不保证返回值</param>
        /// <returns>是否允许合成岛屿</returns>

        public bool MixedIsAllow(IslandType island,Vector2Int pos,out List<Vector2Int> list) {
            Queue<Vector2Int> queue = new Queue<Vector2Int>();
            bool[,] gameMapMemory = new bool[MAP_WIDTH, MAP_HEIGHT];
            queue.Enqueue(pos);
            //搜索顺序
            Vector2Int[] posMove = { Vector2Int.up, Vector2Int.right, Vector2Int.down, Vector2Int.left };
            list = new List<Vector2Int>();
            while (queue.Count>0) {
                Vector2Int nowPos = queue.Dequeue();
                foreach(Vector2Int move in posMove) {
                    nowPos += move;
                    if (nowPos.x >= 0 &&
                        nowPos.x < MAP_WIDTH &&
                        nowPos.y >= 0 &&
                        nowPos.y < MAP_HEIGHT &&
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
                if (MixedIsAllow(getNextType(island), pos,out tmp)) {
                    list.AddRange(tmp);
                }
                return true;
            } else {
                list = null;
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
        private IslandType getNextType(IslandType islandType) {
            if (islandType == IslandType.LARGE_ISLAND) return IslandType.EMPTY;
            return islandType + 1;
        }
    }
}


