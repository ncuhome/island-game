using System;
using System.IO;
using Unity;
using UnityEngine;

public static class Saver {
    static public SaveDate saveDate;
    /// <summary>
    /// 加载Json存档
    /// </summary>
    /// <returns>是否成功加载</returns>
    static public bool LoadJsonSave() {
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
        string json = JsonUtility.ToJson(saveDate);
        File.WriteAllText(Application.dataPath + "/save.json", json);
    }
}
