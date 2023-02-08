namespace TheGame.Data
{
    public interface IDataService
    {
        IDataGetter Getter { get; }
        IDataSetter Setter { get; }
    }



    public partial class DataService : IDataService
    {
        private readonly UserDataProvider _userData;
        private readonly CharacterDataProvider _characterData;
        private readonly IDatabaseProvider _databaseProvider;

        public IDataGetter Getter { get; }
        public IDataSetter Setter { get; }

        public DataService(UserDataProvider userDataProvider
            , CharacterDataProvider characterDataProvider
            , IDatabaseProvider databaseProvider)
        {
            _userData = userDataProvider;
            _characterData = characterDataProvider;
            _databaseProvider = databaseProvider;

            Getter = new DataGetter(this);
            Setter = new DataSetter(this);
        }

        private void InitCharactersBaseData()
        {
            _characterData.BaseData.Init(_databaseProvider.CharactersDatabase);
        }
    }
}
    


