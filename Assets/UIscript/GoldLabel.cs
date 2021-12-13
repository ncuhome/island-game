using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GoldLabel : MonoBehaviour
{
    Text text;
    private void FixedUpdate() {
        if (text == null) text = gameObject.GetComponent<Text>();
        text.text = ((long)Saver.saveDate.gold).ToString();
    }
}
