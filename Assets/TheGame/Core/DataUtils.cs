using System.Security.Cryptography;
using System.Text;
using Newtonsoft.Json;
using System.IO;
using Cysharp.Threading.Tasks;
using System;
using System.Linq;


namespace TheGame.Utils
{
    public class SupportUtility
    {
        public static string GetUniqueKey(int size)
        {
            char[] chars =
                "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890".ToCharArray();
            byte[] data = new byte[size];
            using (RNGCryptoServiceProvider crypto = new RNGCryptoServiceProvider())
            {
                crypto.GetBytes(data);
            }
            StringBuilder result = new StringBuilder(size);
            foreach (byte b in data)
            {
                result.Append(chars[b % (chars.Length)]);
            }
            return result.ToString();
        }

        public static T[] GetEnumValues<T>() where T: Enum
        {
            var enums = Enum.GetValues(typeof(T)).Cast<T>().ToArray();
            return enums;
        }
    }

    public static class DataUtility
    {
        public static void SaveData<T>(string path, T data)
        {
            JsonUtility.SaveData(path, data);
        }

        public static T LoadData<T>(string path)
        {
            return JsonUtility.LoadData<T>(path);
        }
    }

    public static class JsonUtility
    {
        public static void SaveData<T>(string fileName, T data)
        {
            string jsonData = JsonConvert.SerializeObject(data);
            File.WriteAllText(fileName, jsonData);
        }


        public static T LoadData<T>(string fileName)
        {
            if (File.Exists(fileName))
            {
                string jsonData = File.ReadAllText(fileName);
                return JsonConvert.DeserializeObject<T>(jsonData);
            }
            else
            {
                return default(T);
            }
        }

        public static string ToJson(object obj)
        {
            return JsonConvert.SerializeObject(obj);
        }

        public static T FromJson<T>(string jsonString)
        {
            return JsonConvert.DeserializeObject<T>(jsonString);
        }

        public static async UniTask<string> ToJsonAsync(object obj)
        {
            return await UniTask.FromResult(JsonConvert.SerializeObject(obj));
        }

        public static async UniTask<T> FromJsonAsync<T>(string jsonString)
        {
            var task = await UniTask.FromResult(JsonConvert.DeserializeObject<T>(jsonString));
            return task;
        }
    }

    public class GPrefsUtility
    {
        public static void SetInt(string key, int value)
        {
            GPrefs.SetInt(key, value);
        }

        public static int GetInt(string key)
        {
            return GPrefs.GetInt(key);
        }

        public static void SetFloat(string key, float value)
        {
            GPrefs.SetFloat(key, value);
        }

        public static float GetFloat(string key)
        {
            return GPrefs.GetFloat(key);
        }

        public static void SetString(string key, string value)
        {
            GPrefs.SetString(key, value);
        }

        public static string GetString(string key)
        {
            return GPrefs.GetString(key);
        }

        public static void Save()
        {
            GPrefs.Save();
        }

        public static void LoadFile(SimpleJSON.JSONNode data)
        {
            GPrefs.LoadExternal(data);
        }

        public static void LoadData(byte[] data)
        {
            GPrefs.Load(data);
        }

        public static byte[] GetData()
        {
            return GPrefs.GetDataInByte();
        }

        public static void UpdatePath(string finalPart)
        {
            GPrefs.UpdateDataPath(finalPart);
        }
    }
}





