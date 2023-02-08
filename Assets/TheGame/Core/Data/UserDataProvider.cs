using SimpleJSON;
using TheGame.Utils;
using TheGame.Massager;
using Cysharp.Threading.Tasks;
using System;

namespace TheGame.Data
{
    public interface IUserDataGetter
    {

    }

    public interface IUserDataSetter
    {

    }

    public class UserDataProvider : BaseProvider<UserDataUtils>, IUserDataGetter, IUserDataSetter
    {
        private IMassageService _massageService;
        private UserAccountData _userData;

        public UserDataProvider(IMassageService massageService)
        {
            _massageService = massageService;
        }

        public async UniTask LoadUserData(string jsonString, Action onComplete, Action onFail)
        {
            _userData = Utils.TryGetUserDataFromJson(jsonString);
            if (_userData == null)
            {
                _massageService.SendMassage(SystemMassage.UserDataError, this);
                onFail?.Invoke();
                return;
            }

            await DeployUserData(_userData);
            onComplete?.Invoke();
        }

        public void CreateNewUserData()
        {
            var userID = Utils.GetUniqueID();
            var userData = new UserAccountData(userID);
            Utils.UpdateFilePath(userData.ID);
            Utils.Save();
        }

        private async UniTask DeployUserData(UserAccountData data)
        {
            var progress = await Utils.GetNodeFromJson(data.Data);
            Utils.LoadData(progress);
            Utils.UpdateFilePath(data.ID);
            Utils.Save();
        }
    }

    public class UserDataUtils : BaseDataUtility
    {
        public void LoadData(JSONNode data)
        {
            GPrefsUtility.LoadFile(data);
        }

        public void UpdateFilePath(string path)
        {
            GPrefsUtility.UpdatePath(path);
        }

        public async UniTask<JSONNode> GetNodeFromJson(string jsonString)
        {
            var result = await JsonUtility.ConvertFromStringAsync<JSONNode>(jsonString);
            return result;
        }

        public UserAccountData GetUserDataFromJson(string json)
        {
            return JsonUtility.ConvertFromString<UserAccountData>(json);
        }

        public UserAccountData TryGetUserDataFromJson(string json)
        {
            try
            {
                return JsonUtility.ConvertFromString<UserAccountData>(json);
            }
            catch (System.Exception)
            {
                return null;
                throw;
            }
        }
    }

    [System.Serializable]
    public class UserAccountData
    {
        public string UserName;
        public readonly string ID;
        public string Data = "{}";

        public UserAccountData(string id)
        {
            ID = id;
        }
    }
}
    


