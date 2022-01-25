using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Manager {
    /// <summary>
    /// ��Ϸ��ͼ������
    /// </summary>
    public class GameMapManager
    {
        public int mapWidth { get; private set; } = 3;
        public int mapHeight { get; private set; } = 3;
        const int MAX_MAP_LENGHT = 30;
        public const int ISLAND_COST = 500;
        public Vector2Int interestEmpty=new Vector2Int(-1,-1);
        /// <summary>
        /// ��С����ϳ�����
        /// </summary>
        //��Чħ��
        public const int EF_NUM = 33123;
        public const int MIN_MIXED_NUM = 3;
        IslandType[,] gameMap = new IslandType[MAX_MAP_LENGHT,MAX_MAP_LENGHT];
        public IslandScript[,] pIslandScript = new IslandScript[MAX_MAP_LENGHT,MAX_MAP_LENGHT];

        /// <summary>
        /// 
        /// </summary>
        /// <param name="mapWidth"></param>
        /// <param name="mapHeight"></param>
        public GameMapManager(int mapWidth,int mapHeight) {
            if (mapHeight > MAX_MAP_LENGHT || mapWidth > MAX_MAP_LENGHT) {
                //����
                Debug.LogError("out of MAX_MAP_LENGHT");
            }
            this.mapHeight = mapHeight;
            this.mapWidth = mapWidth;
        }

        public void InitBySave(GameObject IslandObj,Transform mapPosTransform) {
            List<IslandDate> list = Saver.saveDate.islandDates;
            foreach(IslandDate date in list) {
                GameObject tmp = GameObject.Instantiate(IslandObj);
                tmp.transform.parent = mapPosTransform;
                tmp.transform.localScale = Vector3.one;
                tmp.transform.localPosition = new Vector3(date.pos.x, date.pos.y);

            }
        }


        /// <summary>
        /// ������Ч
        /// </summary>
        /// <param name="par">Map��Transform�����������Ч��С</param>
        public void UpdateEffectByController(Transform par) {
            InstanceManager.EffectInstance.DestroyHighLightByNum(EF_NUM);
            if (interestEmpty.x >= 0 && interestEmpty.y >= 0) {    
                GameObject tmp = InstanceManager.EffectInstance.GetHighLightByNum(EF_NUM);
                tmp.transform.parent = par;
                tmp.transform.localScale = Vector3.one;
                tmp.transform.localPosition = new Vector3(interestEmpty.x,interestEmpty.y,-1);
            } else {
                for (int i = 0; i < mapWidth; i++) {
                    for (int r = 0; r < mapHeight; r++) {
                        if (pIslandScript[i, r] != null&&pIslandScript[i,r].isInterestIsland) {
                            GameObject tmp = InstanceManager.EffectInstance.GetHighLightByNum(EF_NUM);
                            tmp.transform.parent = par;
                            tmp.transform.localScale = Vector3.one;
                            tmp.transform.localPosition = new Vector3(i, r,-1);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pos"></param>
        /// <returns></returns>
        public IslandScript touchIsland(Vector2Int pos,GameObject islandWaitPlace,out bool isOutRange) {
            IslandScript ret = null;
            isOutRange = false;
            if (pos.x >= mapWidth || pos.x < 0 || pos.y >= mapHeight || pos.y < 0) {
                isOutRange = true;
                return null;
            }
            if (pIslandScript[pos.x, pos.y] != null) {
                ret = pIslandScript[pos.x, pos.y];
                interestEmpty.x = -1;
                interestEmpty.y = -1;
                //���ÿյ���Ȥ��
                if (ret.isInterestIsland) {
                    ret.isInterestIsland = false;
                    ret = null;
                } else {
                    ret.isInterestIsland = true;
                }
            } else {
                if (interestEmpty == pos && Saver.saveData.power>ISLAND_COST) {
                    Saver.saveData.power -= ISLAND_COST;
                    PlaceIsland(IslandType.SMALL_ISLAND,pos,islandWaitPlace);
                    ret = pIslandScript[pos.x,pos.y];
                    interestEmpty.x = interestEmpty.y = -1;
                }
                else
                    interestEmpty = pos;
            }
            return ret;
        }

        /// <summary>
        /// ����gameMap����pIslandObj
        /// </summary>
        public void UpdateIslandGameObject() {
            for(int i = 0; i < mapWidth; i++) {
                for(int r = 0; r < mapHeight; r++) {
                    if (pIslandScript[i, r] != null) {
                        pIslandScript[i, r].islandType = gameMap[i, r];
                        pIslandScript[i, r].UpdateByManager(i, r);
                    }
                }
            }
        }

        /// <summary>
        /// ����date
        /// </summary>
        public void SaveToData() {
            SaveData sd = Saver.saveData;
            sd.islandDatas.Clear();
            for(int i = 0; i < mapWidth; ++i) {
                for(int r = 0; r < mapHeight; ++r) {
                    if (pIslandScript[i, r] != null) {
                        sd.islandDatas.Add(pIslandScript[i, r].pIslandData);
                    }
                }
            }
        }

        /// <summary>
        /// �����괦���õ���
        /// </summary>
        /// <param name="island">��������</param>
        /// <param name="pos">����</param>
        /// <returns>�Ƿ�ɹ����õ���</returns>
        public bool PlaceIsland(IslandType island, Vector2Int pos,GameObject islandWaitPlace) {
            if (gameMap[pos.x, pos.y] != IslandType.EMPTY) {
                return false;
            }
            gameMap[pos.x, pos.y] = island;
            islandWaitPlace.SetActive(true);
            pIslandScript[pos.x,pos.y] = islandWaitPlace.GetComponent<IslandScript>();
            pIslandScript[pos.x, pos.y].pIslandData = new IslandData(pos, island);
            UpdateIslandGameObject();
            return true;
        }

        /// <summary>
        /// �ݻٵ���
        /// </summary>
        /// <param name="pos">���ݻٵ��������</param>
        public void DestroyIsland(Vector2Int pos) {
            //����VS�ļ򻯣������Ƚ�����
            GameObject.Destroy(pIslandScript[pos.x, pos.y].gameObject);
            gameMap[pos.x, pos.y] = IslandType.EMPTY;
            UpdateIslandGameObject();
        }

        /// <summary>
        /// �ϳɵ���
        /// </summary>
        /// <param name="list">���ϳɵĵ������������,�ϳɺ�ĵ��콫���������һ��������</param>
        /// <returns>�Ƿ�ɹ��ϳ�</returns>
        //���MIN_MIXED_NUM>3������������ĳɲ��鼯ʵ��
        public bool MixedIsland(List<IslandScript> list) {
            if (list.Count != 3) return false;
            int cnt = 0;
            for(int i = 0; i < MIN_MIXED_NUM; ++i) {
                for (int r = 0; r < MIN_MIXED_NUM; ++r) {
                    Vector2Int pos = list[i].GetIslandPosInMap() - list[r].GetIslandPosInMap();
                    if ((pos == Vector2Int.up ||
                        pos == Vector2Int.down ||
                        pos == Vector2Int.left ||
                        pos == Vector2Int.right)&&
                        canMixed(list[i].islandType,list[r].islandType)) {
                        cnt += 1;
                        break;
                    }
                }     
            }
            if (cnt < MIN_MIXED_NUM) {
                foreach(IslandScript i in list) {
                    i.isInterestIsland = false;
                }
                UpdateEffectByController(list[0].transform.parent);
                return false;
            }
            Vector2Int t = list[MIN_MIXED_NUM - 1].GetIslandPosInMap();
            gameMap[t.x, t.y] = getNextIslandType(gameMap[t.x,t.y]);
            pIslandScript[t.x, t.y].MixedAsMain(list[0],list[1]);
            for(int i = 0; i < MIN_MIXED_NUM-1; ++i) {
                DestroyIsland(new Vector2Int(list[i].GetIslandPosInMap().x, list[i].GetIslandPosInMap().y));
            }
            UpdateEffectByController(pIslandScript[t.x, t.y].transform.parent);
            return true;
        }

        /// <summary>
        /// �����ĳ���괦�Ƿ�����ϳɵ���
        /// </summary>
        /// <param name="island">��������</param>
        /// <param name="pos">����</param>
        /// <param name="list">�������ϳɣ��򷵻�һ���úϳ��л�ʹ�õĵ��������б�����������򲻱�֤����ֵ</param>
        /// <param name="finalIslandType">�������ϳɣ��򷵻ظúϳ����ջ����ɵĵ�������</param>
        /// <returns>�Ƿ�����ϳɵ���</returns>

        public bool MixedIsAllow(IslandType island,Vector2Int pos,out List<Vector2Int> list,out IslandType finalIslandType) {
            Queue<Vector2Int> queue = new Queue<Vector2Int>();
            bool[,] gameMapMemory = new bool[mapWidth, mapHeight];
            queue.Enqueue(pos);
            //����˳��
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
        /// �ж����������Ƿ���Խ��кϳɣ���ʱ��
        /// </summary>
        /// <param name="a">��һ������</param>
        /// <param name="b">�ڶ�������</param>
        /// <returns>�Ƿ���Ժϳ�</returns>
        public static bool canMixed(IslandType a,IslandType b) {
            if (a == IslandType.EMPTY || a == IslandType.LARGE_ISLAND || b == IslandType.EMPTY || b == IslandType.LARGE_ISLAND) return false;
            return a == b;
        }

        /// <summary>
        /// ��ȡ��һ������ȼ�(��ʱ��
        /// </summary>
        /// <param name="islandType">��ǰ����ȼ�</param>
        /// <returns>��һ������ȼ�</returns>
        public static IslandType getNextIslandType(IslandType islandType) {
            if (islandType == IslandType.LARGE_ISLAND) return IslandType.EMPTY;
            return islandType + 1;
        }
    }
}


