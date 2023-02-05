using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;
using UnityEngine.Playables;

public class PlayerDataSavingHelper : MonoBehaviour
{
    public PlayerData LocalCopyOfData;
    public bool IsSceneBeingLoaded = false;

    static public void SaveData()
    {
        if (!Directory.Exists("Saves"))
            Directory.CreateDirectory("Saves");

        BinaryFormatter formatter = new BinaryFormatter();
        //FileStream saveFile = File.Create("Saves/save.binary");

        //formatter.Serialize(saveFile, PlayerState.Instance.LocalPlayerData);

        //saveFile.Close();
    }

    static public PlayerData LoadData()
    {
        BinaryFormatter formatter = new BinaryFormatter();

        PlayerData LocalCopyOfData = new PlayerData();
        try
        {
            FileStream saveFile = File.Open("Saves/save.binary", FileMode.Open);
            LocalCopyOfData = (PlayerData)formatter.Deserialize(saveFile);

            saveFile.Close();
        }
        catch
        {
            SaveData();
            LoadData();
        }

        return LocalCopyOfData;
    }
}
