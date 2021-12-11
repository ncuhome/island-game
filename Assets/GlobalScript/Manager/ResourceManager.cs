using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceManager : MonoBehaviour
{
    static ResourceManager instance=null;

    private void Start() {
        if (instance == null) {
            instance = this;
        } else {
            Destroy(gameObject);
            return;
        }
        DontDestroyOnLoad(gameObject);
    }

    int timer = 60;
    void IDONTKNOWWHATTHISFUNSNAME() {
        int powerCount = 0, goldCount = 0 ;
        SaveDate sd = Saver.saveDate;
        foreach(IslandDate id in sd.islandDates) {
            foreach(BuildingDate bd in id.buildingDates) {
                powerCount += BuildingScript.BuildingPowerOutput(bd.buildingType);
                goldCount += BuildingScript.BuildingGoldOutput(bd.buildingType);
            }
        }
        long k = -sd.lastTickTime + (long)(DateTime.Now - Saver.timeZero).TotalSeconds;
        sd.lastTickTime += k;
        sd.gold += goldCount * k/3600f;
        sd.power += powerCount * k/3600f;

    }


    private void FixedUpdate() {
        if (timer <= 0) {
            timer = 60;
            IDONTKNOWWHATTHISFUNSNAME();
        } else {
            timer--;
        }
    }

}
