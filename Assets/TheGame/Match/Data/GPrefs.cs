using SimpleJSON;
using System.IO;
using UnityEngine;

namespace TheGame.Utils
{
    public static class GPrefs
    {

        public static JSONNode allGameData = null;

        public static string dataPath = Application.persistentDataPath + "/userData";
        private static string _baseDataPath = Application.persistentDataPath + "/userData";

        public static void UpdateDataPath(string name)
        {
            dataPath = Path.Combine(_baseDataPath, name);
        }

        #region Delete

        public static void DeleteAll()
        {
            if (File.Exists(dataPath))
                File.Delete(dataPath);
        }

        public static void DeleteKey(string key)
        {
            if (allGameData == null)
                Load();
            if (allGameData[key].ToString() != "")
            {
                allGameData.Remove(key);
                Save();
            }
        }

        #endregion

        #region Float

        public static float GetFloat(string key, float defaultValue = 0)
        {
            if (allGameData == null)
                Load();
            if (allGameData[key].ToString() == "")
                return defaultValue;
            return allGameData[key].AsFloat;
        }

        public static void SetFloat(string key, float value)
        {
            JSONNode node = value.ToString();
            SetData(key, node);
        }

        #endregion

        #region Int

        public static int GetInt(string key, int defaultValue = 0)
        {
            if (allGameData == null)
                Load();
            //     Debug.LogError(allGameData[key].ToString());
            if (allGameData[key].ToString() == "")
                return defaultValue;
            return allGameData[key].AsInt;
        }

        public static void SetInt(string key, int value)
        {
            JSONNode node = value.ToString();
            SetData(key, node);
        }

        #endregion

        #region Bool

        public static bool GetBool(string key, bool defaultValue = false)
        {
            if (allGameData == null)
                Load();
            if (allGameData[key].ToString() == "")
                return defaultValue;
            return allGameData[key].AsBool;
        }

        public static void SetBool(string key, bool value)
        {
            JSONNode node = value.ToString();
            SetData(key, node);
        }

        #endregion

        #region String

        public static string GetStringByEnum(System.Enum key, string defaultValue = "")
        {
            return GetString(key.ToString(), defaultValue);
        }

        public static string GetString(string key, string defaultValue = "")
        {
            if (allGameData == null)
                Load();
            if (allGameData[key].ToString() == "")
                return defaultValue;
            return allGameData[key].Value;

        }

        public static void SetString(string key, string value)
        {
            JSONNode node = value;
            SetData(key, node);
        }

        #endregion

        #region Set Value

        private static void SetData(string key, JSONNode value)
        {
            if (allGameData == null)
                Load();
            if (allGameData[key].Value == "")
            {
                allGameData.Add(key, value);
            }
            else
            {
                allGameData[key] = value;
            }
            //Debug.LogError(allGameData.ToString());
            Save();
        }

        #endregion

        #region Key Exists

        public static bool HasKey(string key)
        {
            if (allGameData == null)
                Load();
            return allGameData[key].ToString() != "";
        }

        #endregion

        #region SaveGame

        public static void Save()
        {
            //    Debug.LogError(allGameData.ToString());
            // if(PlayerPrefs.)
            allGameData.SaveToFile(dataPath);
        }

        #endregion

        #region LoadByte

        public static void Load(byte[] data = null)
        {
            if (data != null)
            {
                File.WriteAllBytes(dataPath, data);
            }
            if (File.Exists(dataPath))
                allGameData = JSONNode.LoadFromFile(dataPath);
            else
                allGameData = JSONClass.Parse("{}");
            // Debug.LogError(allGameData.ToString());
        }

        public static async Cysharp.Threading.Tasks.UniTask LoadAsync(byte[] data = null)
        {
            if (data != null)
            {
                await File.WriteAllBytesAsync(dataPath, data);
            }
            if (File.Exists(dataPath))
                allGameData = JSONNode.LoadFromFile(dataPath);
            else
                allGameData = JSONClass.Parse("{}");
        }

        #endregion

        #region GetDataInByteForm

        public static byte[] GetDataInByte()
        {
            Save();
            byte[] temp = File.ReadAllBytes(dataPath);
            return temp;
        }

        public static async Cysharp.Threading.Tasks.UniTask<byte[]> GetDataInByteAsync()
        {
            Save();
            byte[] temp = await File.ReadAllBytesAsync(dataPath);
            return temp;
        }
        #endregion

        public static void DeleteFile(string name)
        {
            var path = Path.Combine(_baseDataPath, name);
            if (File.Exists(path))
            {
                File.Delete(path);
            }
        }

        public static void RefreshData()
        {
            allGameData = JSONClass.Parse("{}");
        }
    }
}


