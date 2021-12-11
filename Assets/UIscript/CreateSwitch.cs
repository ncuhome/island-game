using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CreateSwitch : MonoBehaviour
{
    static public CreateSwitch instance;
    private void Start() {
        instance = this;
    }
    public Sprite[] sprites;
    Image imgae;
    bool isWK = true;
    public void Exchange()
    {
        if (imgae == null) imgae = gameObject.GetComponent<Image>();
        if (isWK) imgae.sprite = sprites[0];
        else imgae.sprite = sprites[1];
        isWK = !isWK;
    }
}
