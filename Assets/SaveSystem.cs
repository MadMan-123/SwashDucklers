using System;
using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;



public static class SaveSystem
{
    static string SavePath => $"{Application.persistentDataPath}/";

    public static string GetFullPath(string filename) => $"{SavePath}{filename}";

    public static void SaveData<T>(T saveData, string filename)
    {
        try 
        {
            string fullPath = $"{SavePath}{filename}";
                
            using (FileStream stream = new FileStream(fullPath, FileMode.Create))
            {
                BinaryFormatter formatter = new BinaryFormatter();
                formatter.Serialize(stream, saveData);
            }
                
            Debug.Log($"Saved {filename}");
        }
        catch (Exception ex)
        {
            Debug.LogError($"Save failed: {ex.Message}");
        }
    }

    public static T LoadDataRaw<T>(string filename)
    {
        string fullPath = $"{filename}";

        if (!File.Exists(fullPath))
        {
            Debug.LogWarning($"File not found: {filename}");
            return default;
        }

        try 
        {
            using (FileStream stream = new FileStream(fullPath, FileMode.Open))
            {
                BinaryFormatter formatter = new BinaryFormatter();
                return (T)formatter.Deserialize(stream);
            }
        }
        catch (Exception ex)
        {
            Debug.LogError($"Load failed: {ex.Message}");
            return default;
        }
    }
    public static T LoadData<T>(string filename)
    {
        string fullPath = $"{SavePath}{filename}";

        if (!File.Exists(fullPath))
        {
            Debug.LogWarning($"File not found: {filename}");
            return default;
        }

        try 
        {
            using (FileStream stream = new FileStream(fullPath, FileMode.Open))
            {
                BinaryFormatter formatter = new BinaryFormatter();
                return (T)formatter.Deserialize(stream);
            }
        }
        catch (Exception ex)
        {
            Debug.LogError($"Load failed: {ex.Message}");
            return default;
        }
    }
}

/*
//created by Madoc Wolstencroft

    public static class SaveSystem
    {
        static string sPath = $"{Application.persistentDataPath}/";


        //Creates a save data file assosiated with the data type T
        public static void SaveData<T>(T SaveData) where T : new()
        {

            //Binary Formatting
            BinaryFormatter formatter = new BinaryFormatter();
            //create file stream
            FileStream stream = new FileStream(sPath + $"{typeof(T).ToString()}.Dat", FileMode.Create);
            //create as json
            string sJSONString = JsonUtility.ToJson(SaveData);

            //serialize the data
            formatter.Serialize(stream, sJSONString);
            stream.Close();

            if (File.Exists($"{sPath}{typeof(T).ToString()}.Dat"))
            {
                Debug.Log($"Saved in file {typeof(T).ToString()}.Dat");
            }
            else
            {
                Debug.Log($"Created save file {typeof(T).ToString()}.Dat");
            }
        }

        //Returns the dat from the file assosiated with that data name
        //example: SaveData data = SaveSystem.LoadData<SaveData>();
        public static T LoadData<T>()
        {
            //get datatype name
            string sDataTypeName = typeof(T).ToString();

            //check if file exsists
            if (File.Exists($"{sPath}{typeof(T).ToString()}.Dat"))
            {
                BinaryFormatter formatter = new BinaryFormatter();
                FileStream stream = new FileStream(sPath + $"{typeof(T).ToString()}.Dat", FileMode.Open);

                //create as json string
                string sJSONString = formatter.Deserialize(stream) as string;

                //close the stream
                stream.Close();

                //return the data from the unserialised data
                return JsonUtility.FromJson<T>(sJSONString);
            }
            else
            {
                Debug.LogError($"{sDataTypeName}.Dat not found in: " + sPath);
                //return null
                return default(T);

            }
        }
    };
    */

