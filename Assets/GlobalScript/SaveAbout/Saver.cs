using System;
using System.IO;
using Unity;
using UnityEngine;

public static class Saver {
    static public SaveDate saveDate;
    static public IslandDate pNowIslandDate;
    //Scene之间转换用的！！！！
    /// <summary>
    /// 加载Json存档
    /// </summary>
    /// <returns>是否成功加载</returns>
    static public bool LoadJsonSave() {
        Debug.Log(Application.dataPath + "/save.json");
        saveDate = new SaveDate();
        if (!File.Exists(Application.dataPath + "/save.json")) {
            return false;
        }
        string json = File.ReadAllText(Application.dataPath + "/save.json");
        JsonUtility.FromJsonOverwrite(json, saveDate);
        return true;
    }
    /// <summary>
    /// 保存存档
    /// </summary>
    static public void SaveJson() {
        if (saveDate == null) return;
        string json = JsonUtility.ToJson(saveDate);
        File.WriteAllText(Application.dataPath + "/save.json", json);
    }
}
