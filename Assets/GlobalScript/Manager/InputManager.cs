using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Manager {
    public delegate void ScreenInputEvent(Vector2 pos);
    public class InputManager : MonoBehaviour
    {
        /// <summary>
        /// 单击事件绑定
        /// </summary>
        public void test(Vector2 pos) { }
        public ScreenInputEvent singleTouch;
        /// <summary>
        /// 双击事件绑定(未实现
        /// </summary>
        
        public ScreenInputEvent doubleTouch;
        /// <summary>
        /// 拖动操作(未实现
        /// </summary>

        public ScreenInputEvent drag;

        private void Awake() {
            InstanceManager.InputInstance = this;
            if (InstanceManager.InputInstance != this) {
                //单例失败
                Destroy(gameObject);
            }
            Input.multiTouchEnabled = false;
            DontDestroyOnLoad(gameObject);
        }

        private void Update() {
            if (Input.touchCount > 0) {
                if (Input.touches[0].phase == TouchPhase.Began) {
                    if(singleTouch!=null)
                        singleTouch(Input.touches[0].position);
                }
            }

            /* !!!WARNING!!! 以下是该死的测试代码 */
#if UNITY_EDITOR
            if (Input.GetMouseButtonDown(0)) {
                singleTouch(Input.mousePosition);
            }
#endif
        }
    }
}


