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
            Input.multiTouchEnabled = false;
            InstanceManager.InputInstance = this;
        }

        private void Update() {
            if (Input.touchCount > 0) {
                if (Input.touches[0].phase == TouchPhase.Began) {
                    if(singleTouch!=null)
                        singleTouch(Input.touches[0].position);
                }
            }
        }
    }
}


