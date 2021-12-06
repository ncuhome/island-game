using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Manager {
    
    public class InstanceManager
    {
        static Effect.IEffect effectInstance;
        static public Effect.IEffect EffectInstance {
            get {
                return effectInstance; 
            }
            set {
                if (effectInstance == null) {
                    effectInstance = value;
                }
            }
        }
        static InputManager inputInstance;
        static public InputManager InputInstance {
            get {
                return inputInstance;
            }
            set {
                if (inputInstance == null) {
                    inputInstance = value;
                }
            }
        }
    }

}

