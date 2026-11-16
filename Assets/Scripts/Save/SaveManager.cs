using UnityEngine;
using System.IO;
using System.Drawing;
using System.Collections.Generic;
using System.Collections;

public class SaveManager 
{
   [System.Serializable]
   public class UpgradeSave
    {
        public string id;   // ej: "UpgradeLife"
        public int level;   // cuantas veces se compro
    }

   [System.Serializable]
   public class GameData
    {
        public float coins;
        public List<UpgradeSave> upgrades = new List<UpgradeSave>();
    }

    const string SAVE_FILE_NAME = "SaveData.json";

    static GameData lastLoadedGameData;
    public static GameData LastLoadedGameData
    {
        get 
        {
            if(lastLoadedGameData == null) Load();
            return lastLoadedGameData;
        }
    }
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    static void SubscribeToEvents()
    {
        EventManager.OnCoinsChanged -= SaveCoins;
        EventManager.OnCoinsChanged += SaveCoins;
    }
    static void SaveCoins(float totalCoins)
    {
        LastLoadedGameData.coins = totalCoins;
        Save();
    }
    /// <summary>Borra todos los datos guardados (monedas y upgrades) y deja el JSON en valores iniciales.</summary>
    public static void ResetAll()
    {
        lastLoadedGameData = new GameData();
        Save(lastLoadedGameData);
    }
    public static string GetSavePath()
    {
        return string.Format("{0}/{1}", Application.persistentDataPath, SAVE_FILE_NAME);
    }
    public static void Save(GameData data = null) 
    {
        if(data == null) 
        {
            if (lastLoadedGameData == null) Load();
            data = lastLoadedGameData;
        }
        File.WriteAllText(GetSavePath(), JsonUtility.ToJson(data));
    }
    public static GameData Load(bool usePreviousLoadIfAvailable = false) 
    {
        if(usePreviousLoadIfAvailable && lastLoadedGameData != null) return lastLoadedGameData;
        string path = GetSavePath();
        if (File.Exists(path)) 
        {
            string json = File.ReadAllText(path);
            lastLoadedGameData = JsonUtility.FromJson<GameData>(json);
            if(lastLoadedGameData == null) lastLoadedGameData = new GameData();
        }
        else 
        {
            lastLoadedGameData = new GameData();
        }
        return lastLoadedGameData;
    }
}
