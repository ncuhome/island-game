using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Manager {
    public class GameMapManager
    {
        const int MAP_WIDTH = 7;
        const int MAP_HEIGHT = 7;
        IslandType[,] gameMap;

        public GameMapManager() {
            gameMap = new IslandType[MAP_HEIGHT,MAP_WIDTH];
        }

        /// <summary>
        /// 于坐标处放置岛屿
        /// </summary>
        /// <param name="island">岛屿类型</param>
        /// <param name="x">x轴坐标</param>
        /// <param name="y">y轴坐标</param>
        /// <returns>返回是否成功放置岛屿</returns>
        public bool PlaceIsland(IslandType island,int x,int y) {
            if (gameMap[x, y] != IslandType.EMPTY) {
                return false;
            }
            gameMap[x, y] = island;

            return true;
        }

        /// <summary>
        /// 检测在某坐标处是否允许合成岛屿
        /// </summary>
        /// <param name="island">岛屿类型</param>
        /// <param name="x">x轴坐标</param>
        /// <param name="y">y轴坐标</param>
        /// <param name="list">如果允许合成，则返回一个该合成中会使用的岛屿坐标列表，如果不允许，则不保证返回值</param>
        /// <returns>返回是否允许合成岛屿</returns>

        public bool MixedIsAllow(IslandType island,int x,int y,out List<KeyValuePair<int,int>> list) {
            


            list = null;
            return false;
        }
    }
}


