using TheGame.Utils;
using SimpleJSON;

namespace TheGame.Data
{
    public interface IDataSetter
    {
        void DeployUserData(UserAccountData data);
    }

    public partial class DataService
    {
        private partial class DataSetter : IDataSetter
        {
            private DataService _service;

            public DataSetter(DataService service)
            {
                _service = service;
            }

            public async void DeployUserData(UserAccountData data)
            {
                var progressTask = JsonDataService.ConvertFromStringAsync<JSONNode>(data.Data);
                await progressTask;
                JSONNode progress = progressTask.GetAwaiter().GetResult();
                GPrefsUtils.LoadFile(progress);
            }
        }
    }

    public class UserAccountData
    {
        public string Data;
    }
}
    


