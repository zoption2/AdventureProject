using TheGame.Utils;
using SimpleJSON;

namespace TheGame.Data
{
    public interface IDataSetter
    {
        IUserDataSetter User { get; }
        ICharacterDataSetter Character { get; }
    }

    public partial class DataService
    {
        private partial class DataSetter : IDataSetter
        {
            public IUserDataSetter User => _service._userData;
            public ICharacterDataSetter Character => _service._characterData;

            private DataService _service;

            public DataSetter(DataService service)
            {
                _service = service;
            }
        }
    }
}
    


