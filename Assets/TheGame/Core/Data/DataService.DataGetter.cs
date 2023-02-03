using System;


namespace TheGame.Data
{

    public interface IDataGetter
    {
        ICharacterData GetCharacterInstanceData(string id);
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

            public ICharacterData GetCharacterInstanceData(string id)
            {
                ICharacterData data = default;
                try
                {
                    data = _service.Character.InstanceData.Get(id);
                }
                catch (Exception ex)
                {
                    
                }

                return data;
            }

            public ICharacterData GetNewCharacterInstanceData(Character character)
            {
                var baseData = _service.Character.BaseData.Get(character);
                var instanceData = BuildNewCharacterInstanceDataFromBase(baseData);
                return instanceData;
            }

            private CharacterData BuildNewCharacterInstanceDataFromBase(CharacterBaseData baseData)
            {
                var id = _service.CreateCharacterUniqID();
                var instanceData = new CharacterData(id, baseData);
                return instanceData;
            }
        }
    }
}
    


