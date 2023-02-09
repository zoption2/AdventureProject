using SimpleJSON;
using TheGame.Utils;
using TheGame.Massager;
using Cysharp.Threading.Tasks;
using System;

namespace TheGame.Data
{
    public interface IUserDataGetter
    {
        string GetUserId();
        string GetUserName();
        string GetUserAccountDataJson();
    }

    public interface IUserDataSetter
    {
        void SetUserName(string name);
        UniTask LoadUserData(string jsonString, Action onComplete = null, Action onFail = null);
        void CreateNewUserData();
        void ClearUserAccountData();
    }

    public class UserDataMediator : BaseProvider<UserDataUtils>, IUserDataGetter, IUserDataSetter
    {
        private IMassageService _massageService;
        private UserAccountData _userData;

        public UserDataMediator(IMassageService massageService)
        {
            _massageService = massageService;
        }

        public async UniTask LoadUserData(string jsonString, Action onComplete = null, Action onFail = null)
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
            userData.UserName = "Player" + userData.ID;
            _userData = userData;
            Utils.UpdateFilePath(userData.ID);
            Utils.Save();
        }

        public string GetUserAccountDataJson()
        {
            Utils.Save();
            _userData.Data = Utils.GetData();
            var jsonUserData = Utils.ConvertUserDataToJson(_userData);
            return jsonUserData;
        }

        public void ClearUserAccountData()
        {
            Utils.ClearUserAccountData();
            Utils.Save();
            _userData.Data = Utils.GetData();
        }

        public string GetUserName()
        {
            if (_userData != null)
            {
                return _userData.UserName;
            }
            throw new System.NullReferenceException(
                string.Format("User data is not setted. Load User data first instead of create new one")
                );
        }

        public void SetUserName(string name)
        {
            if (_userData != null)
            {
                _userData.UserName = name;
            }
            throw new System.NullReferenceException(
                string.Format("User data is not setted. Load User data first instead of create new one")
                );
        }

        public string GetUserId()
        {
            if (_userData != null)
            {
                return _userData.ID;
            }
            throw new System.NullReferenceException(
                string.Format("User data is not setted. Load User data first instead of create new one")
                );
            
        }

        private async UniTask DeployUserData(UserAccountData data)
        {
            await Utils.LoadDataAsync(data.Data);
            Utils.UpdateFilePath(data.ID);
            Utils.Save();
        }
    }

    public class UserDataUtils : BaseDataUtility
    {
        public void LoadData(byte[] data)
        {
            GPrefs.Load(data);
        }

        public async UniTask LoadDataAsync(byte[] data)
        {
            await GPrefs.LoadAsync(data);
        }

        public byte[] GetData()
        {
            return GPrefs.GetDataInByte();
        }

        public void UpdateFilePath(string path)
        {
            GPrefs.UpdateDataPath(path);
        }

        public UserAccountData TryGetUserDataFromJson(string json)
        {
            try
            {
                return JsonUtility.FromJson<UserAccountData>(json);
            }
            catch (System.Exception)
            {
                return null;
                throw;
            }
        }

        public string ConvertUserDataToJson(UserAccountData data)
        {
            return JsonUtility.ToJson(data);
        }

        public void ClearUserAccountData()
        {
            GPrefs.RefreshData();
        }

        public void DeleteUserDataFile(string name)
        {
            GPrefs.DeleteFile(name);
        }
    }

    [System.Serializable]
    public class UserAccountData
    {
        public string UserName;
        public readonly string ID;
        public byte[] Data;

        public UserAccountData(string id)
        {
            ID = id;
        }
    }
}
    


