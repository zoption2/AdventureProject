namespace TheGame.Data
{
    public interface IDataGetter
    {
        IUserDataGetter User { get; }
        ICharacterDataGetter Character { get; }
    }

    public partial class DataService
    {
        private partial class DataGetter : IDataGetter
        {
            public IUserDataGetter User => _service._userData;
            public ICharacterDataGetter Character => _service._characterData;

            private DataService _service;

            public DataGetter(DataService service)
            {
                _service = service;
            }
        }
    }
}
    


