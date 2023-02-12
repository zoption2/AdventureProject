namespace TheGame.Data
{
    public interface IDataService
    {
        IDataGetter Getter { get; }
        IDataSetter Setter { get; }
    }



    public partial class DataService : IDataService
    {
        private readonly UserDataMediator _userData;
        private readonly CharacterDataMediator _characterData;
        private readonly IDatabase _databaseProvider;

        public IDataGetter Getter { get; }
        public IDataSetter Setter { get; }

        public DataService(UserDataMediator userDataProvider
            , CharacterDataMediator characterDataProvider
            , IDatabase databaseProvider)
        {
            _userData = userDataProvider;
            _characterData = characterDataProvider;
            _databaseProvider = databaseProvider;

            Getter = new DataGetter(this);
            Setter = new DataSetter(this);
        }

        public async Cysharp.Threading.Tasks.UniTask Initialize(string userAccountData = "NewPlayer")
        {
            await _userData.LoadUserData(userAccountData, OnSuccess, OnFail);

            async void OnSuccess()
            {

            }

            void OnFail()
            {

            }
        }

        private void InitCharactersBaseData()
        {
            _characterData.BaseData.Init(_databaseProvider.CharactersDatabase);
        }
    }
}
    


