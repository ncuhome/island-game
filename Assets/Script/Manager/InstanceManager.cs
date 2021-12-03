using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Manager {
    
    public class InstanceManager
    {
        Effect.IEffect effectInstance;
        Effect.IEffect EffectInstance {
            get {
                return effectInstance; 
            }
            set {
                if (effectInstance == null) {
                    effectInstance = value;
                }
            }
        }
    }

}

