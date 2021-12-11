using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PowerLabel : MonoBehaviour
{
    Text text;
    private void FixedUpdate() {
        if (text == null) text = gameObject.GetComponent<Text>();
        text.text =((int)Saver.saveDate.power).ToString();
    }
}
