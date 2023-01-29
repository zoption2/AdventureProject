using System.Security.Cryptography;
using System.Text;
using Newtonsoft.Json;
using System.IO;


namespace TheGame
{
    public class DataUtils
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

        public static void SaveData<T>(string path, T data)
        {
            JsonDataService.SaveData(path, data);
        }

        public static T LoadData<T>(string path)
        {
            return JsonDataService.LoadData<T>(path);
        }
    }

    public static class JsonDataService
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
    }
}




