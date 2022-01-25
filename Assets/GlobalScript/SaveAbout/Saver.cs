using System;
using System.IO;
using Unity;
using UnityEngine;

public static class Saver {
    static public SaveData saveData;
    static public IslandData pNowIslandData;
    static public DateTime timeZero=new DateTime(2019,1,1,0,0,0);
    //Scene之间转换用的！！！！
    /// <summary>
    /// 加载Json存档
    /// </summary>
    /// <returns>是否成功加载</returns>
    static public bool LoadJsonSave() {
        Debug.Log(Application.persistentDataPath + "/save.json");
        saveData = new SaveData();
        if (!File.Exists(Application.persistentDataPath + "/save.json")) {
            return false;
        }
        string json = File.ReadAllText(Application.persistentDataPath + "/save.json");
        JsonUtility.FromJsonOverwrite(json, saveData);
        return true;
    }
    /// <summary>
    /// 保存存档
    /// </summary>
    static public void SaveJson() {
        if (saveData == null) return;
        string json = JsonUtility.ToJson(saveData);
        File.WriteAllText(Application.persistentDataPath + "/save.json", json);
    }
}
