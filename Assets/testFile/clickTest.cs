using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class clickTest : MonoBehaviour
{
    public GameObject dan;
    public GameObject suang;
    void Update()
    {
        if (Input.touchCount > 0) {
            if (Input.touches[0].phase == TouchPhase.Began) {
                GameObject gj;
                if (Input.touches[0].tapCount == 1) {
                    gj = Instantiate(dan, Camera.main.ScreenToWorldPoint(Input.touches[0].position), transform.rotation);
                } else {
                    gj = Instantiate(suang, Camera.main.ScreenToWorldPoint(Input.touches[0].position), transform.rotation);         
                }
                gj.transform.position =new Vector3(gj.transform.position.x, gj.transform.position.y, 0);
            }
                
                    
        }
        if (Input.GetMouseButtonDown(0)) {
           GameObject gj= Instantiate(dan, Camera.main.ScreenToWorldPoint(Input.mousePosition), transform.rotation);
            gj.transform.position = new Vector3(gj.transform.position.x, gj.transform.position.y, 0);
        }
    }
}
