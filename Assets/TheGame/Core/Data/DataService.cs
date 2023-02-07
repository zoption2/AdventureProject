using TheGame.Utils;

namespace TheGame.Data
{
    public interface IDataService
    {
        IDataGetter Getter { get; }
        IDataSetter Setter { get; }
    }

    public partial class DataService : IDataService
    {
        private const int kIdLength = 7;
        private readonly CharacterDataProvider _characterDataProvider;
        private readonly IDatabaseProvider _databaseProvider;

        public IDataGetter Getter { get; }
        public IDataSetter Setter { get; }

        public DataService(CharacterDataProvider characterDataProvider
            , IDatabaseProvider databaseProvider)
        {
            _characterDataProvider = characterDataProvider;
            _databaseProvider = databaseProvider;

            Getter = new DataGetter(this);
            Setter = new DataSetter(this);
        }

        private void InitCharactersBaseData()
        {
            _characterDataProvider.BaseData.Init(_databaseProvider.CharactersDatabase);
        }

        private string CreateCharacterUniqID()
        {
            return DataUtils.GetUniqueKey(kIdLength);
        }
    }
}
    


