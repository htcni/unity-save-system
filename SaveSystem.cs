using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using UnityEngine;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace BigFrogTools {
    public class SaveSystem {

        static string result;

        /// <summary>
        /// Returns value corresponding to key of type string.
        /// Typecast in vase of other type(int, float)
        /// </summary>
        /// <param name="dataKey"></param>
        /// <returns></returns>
        public static string GetData(string dataKey) {
            Data savedData = GetSavedData();
            if (savedData.localData.TryGetValue(dataKey, out result)) {
                return result;
            }
            else {
                return "Key does not exist";
            }

        }

        /// <summary>
        /// Sets the value of the data identified by the given key.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="dataKey"></param>
        /// <param name="dataValue"></param>
        public static void SetData<T>(string dataKey, T dataValue) {
            string data;
            if (typeof(string) != typeof(T)) {
                data = JsonConvert.SerializeObject(dataValue);
            }
            else {
                data = dataValue.ToString();
            }
            SaveData(dataKey, data);

        }

        /// <summary>
        /// Return the string dump of binary file.
        /// </summary>
        public static void GetSaveDump() {
            Data savedData = GetSavedData();
            var data = JsonConvert.SerializeObject(savedData);
            Debug.Log(data);
        }


        static string tempRes;
        private static void SaveData(string dataKey, string dataValue) {
            BinaryFormatter bf = new BinaryFormatter();
            Data savedData = GetSavedData();

            if (File.Exists(Application.persistentDataPath + "/gameData.dat")) {
                FileStream file = File.Open(Application.persistentDataPath + "/gameData.dat", FileMode.Open);
                if (savedData.localData.TryGetValue(dataKey, out tempRes)) {
                    savedData.localData[dataKey] = dataValue;
                }
                else {
                    savedData.localData.Add(dataKey, dataValue);
                }


                bf.Serialize(file, savedData);
                file.Close();
            }
            else {

                FileStream file = File.Create(Application.persistentDataPath + "/gameData.dat");
                Data data = new Data();

                data.localData.Add(dataKey, dataValue);

                bf.Serialize(file, data);
                file.Close();

            }
            Debug.Log("Data saved");
            GetSaveDump();
        }

        public static Data GetSavedData() {
            if (File.Exists(Application.persistentDataPath + "/gameData.dat")) {
                BinaryFormatter bf = new BinaryFormatter();
                FileStream file = File.Open(Application.persistentDataPath + "/gameData.dat", FileMode.Open);
                Data resultData = (Data)bf.Deserialize(file);
                file.Close();
                return resultData;
            }
            else {
                Debug.LogError("There is no save data");
                return new Data();
            }
        }

        /// <summary>
        /// Removes the given key and its corresponding value from the saved data.
        /// </summary>
        /// <param name="dataKey"></param>
        public static void RemoveData(string dataKey) {
            BinaryFormatter bf = new BinaryFormatter();
            Data savedData = GetSavedData();

            if (File.Exists(Application.persistentDataPath + "/gameData.dat")) {
                FileStream file = File.Open(Application.persistentDataPath + "/gameData.dat", FileMode.Open);
                if (savedData.localData.TryGetValue(dataKey, out tempRes)) {
                    savedData.localData.Remove(dataKey);
                }

                bf.Serialize(file, savedData);
                file.Close();
            }
            GetSaveDump();
        }

        /// <summary>
        /// Removes all keys and values from the saved data. Use with caution.
        /// </summary>
        public static void ResetSaveData() {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Create(Application.persistentDataPath + "/gameData.dat");
            Data data = new Data();
            bf.Serialize(file, data);
            file.Close();
            GetSaveDump();
        }


        /// <summary>
        /// Convert string data return by GetData(key) to specific type.
        /// Ex. float temp = 0;  ConvertValue(data, temp)
        /// string[] items; ConvertValue(data, items)
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="data"></param>
        /// <param name="valueType"></param>
        /// <returns></returns>
        public static T ConvertValue<T>(string data, T valueType) {
            T convertedValue = JsonConvert.DeserializeObject<T>(data);
            return (T)(convertedValue);
        }
    }
}

[System.Serializable]
public class Data {
    public Dictionary<string, string> localData = new Dictionary<string, string>();
    
}


