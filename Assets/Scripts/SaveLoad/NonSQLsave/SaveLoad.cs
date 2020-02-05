
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

public static class SaveLoad
{
    public static void SavePlayer(PlayerController player)
    {
        BinaryFormatter binaryformatter = new BinaryFormatter();
        FileStream DataFile = File.Create(Application.persistentDataPath + "/savedGames.Rakettiryhma");

        Data data = new Data(player);

        binaryformatter.Serialize(DataFile, data);
        DataFile.Close();
    }
    public static Data LoadPlayer()
    {
        if (File.Exists(Application.persistentDataPath + "/savedGames.Rakettiryhma"))
        {
            BinaryFormatter binaryformatter = new BinaryFormatter();
            FileStream DataFile = File.Open(Application.persistentDataPath + "/savedGames.Rakettiryhma", FileMode.Open);

           Data data =  binaryformatter.Deserialize(DataFile) as Data;
           DataFile.Close();
            return data;
        }
        else
        {
            Debug.LogError("file not found" + Application.persistentDataPath + "/savedGames.Rakettiryhma");
            return null;
        }
        }

        /*public static List<Game> listOfSavedGames = new List<Game>();

        public static void Save()
        {
            listOfSavedGames.Add(Game.current);
            BinaryFormatter binaryformatter = new BinaryFormatter();
            FileStream file = File.Create(Application.persistentDataPath + "/savedGames.gd");
            binaryformatter.Serialize(file, SaveLoad.listOfSavedGames);
            file.Close();
        }

        public static void Load()
        {
            if (File.Exists(Application.persistentDataPath + "/savedGames.gd"))
            {
                BinaryFormatter bf = new BinaryFormatter();
                FileStream file = File.Open(Application.persistentDataPath + "/savedGames.gd", FileMode.Open);
                SaveLoad.listOfSavedGames = (List<Game>)bf.Deserialize(file);
                file.Close();
            }
        }*/
    }

