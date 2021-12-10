using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// 特效控制器
/// </summary>
/// 
namespace Effect {
    public interface IEffect {
        GameObject GetHighLightByNum(int num);
        void DestroyHighLightByNum(int num);
    }

    //应该实现一个特效池回收效果
    public class EffectController : MonoBehaviour,IEffect
    {
        public GameObject highLight;
        public Stack<GameObject> highLightPool = new Stack<GameObject>();
        public Hashtable highLightMap=new Hashtable(); 
        public void Awake() {
            Manager.InstanceManager.EffectInstance = this;
            if (Manager.InstanceManager.EffectInstance != this) {
                Destroy(gameObject);
                return;
            }
            DontDestroyOnLoad(gameObject);
        }
        
        public GameObject GetHighLightByNum(int num) {
            if (!highLightMap.ContainsKey(num)) highLightMap.Add(num, new List<GameObject>());
            GameObject tmp;
            if (highLightPool.Count > 0) {
                tmp = highLightPool.Pop();
                tmp.SetActive(true);
            } else {
                tmp = Instantiate(highLight);
            }
            ((List<GameObject>)highLightMap[num]).Add(tmp);
            return tmp;
        }

        public void DestroyHighLightByNum(int num) {
            if (!highLightMap.ContainsKey(num)) highLightMap.Add(num, new List<GameObject>());
            foreach(GameObject i in ((List<GameObject>)highLightMap[num])) {
                i.SetActive(false);
                highLightPool.Push(i);
            }
            ((List<GameObject>)highLightMap[num]).Clear();
        }
    }


}

