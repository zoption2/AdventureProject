using System;


namespace TheGame.Data
{

    public interface IDataGetter
    {
        ICharacterInstanceData GetCharacterInstanceData(string id);
    }

    public partial class DataService
    {
        private partial class DataGetter : IDataGetter
        {
            private DataService _service;

            public DataGetter(DataService service)
            {
                _service = service;
            }

            public ICharacterInstanceData GetCharacterInstanceData(string id)
            {
                ICharacterInstanceData data = default;
                try
                {
                    data = _service._characterDataProvider.InstanceData.Get(id);
                }
                catch (Exception ex)
                {
                    
                }

                return data;
            }

            public ICharacterInstanceData GetNewCharacterInstanceData(Character character)
            {
                var baseData = _service._characterDataProvider.BaseData.Get(character);
                var instanceData = BuildNewCharacterInstanceDataFromBase(baseData);
                return instanceData;
            }

            private CharacterInstanceData BuildNewCharacterInstanceDataFromBase(ICharacterBaseData baseData)
            {
                var id = _service.CreateCharacterUniqID();
                var instanceData = new CharacterInstanceData(id, baseData);
                return instanceData;
            }
        }
    }
}
    


