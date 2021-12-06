using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Destroy : MonoBehaviour
{
    public int timer = 100;
    public void FixedUpdate() {
        timer--;
        if (timer < 0) {
            Destroy(gameObject);
        }
    }
}
