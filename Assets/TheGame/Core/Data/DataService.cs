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
        public IDataGetter Getter { get; }
        public IDataSetter Setter { get; }
        private CharacterDataProvider Character { get; } = new();

        public DataService()
        {
            Getter = new DataGetter(this);
            Setter = new DataSetter(this);
        }

        private string CreateCharacterUniqID()
        {
            return DataUtils.GetUniqueKey(kIdLength);
        }
    }
}
    


