using System;


namespace TheGame.Data
{

    public interface IDataGetter
    {
        CharacterData GetCharacterInstanceData(string id);
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

            public CharacterData GetCharacterInstanceData(string id)
            {
                CharacterData data = default;
                try
                {
                    data = _service.Character.InstanceData.Get(id);
                }
                catch (Exception ex)
                {
                    
                }

                return data;
            }

            public CharacterData GetNewCharacterInstanceData(Character character)
            {
                var baseData = _service.Character.BaseData.Get(character);
                var instanceData = BuildNewCharacterInstanceDataFromBase(baseData);
                return instanceData;
            }

            private CharacterData BuildNewCharacterInstanceDataFromBase(CharacterBaseData baseData)
            {
                var instanceData = new CharacterInstanceData();
                instanceData.SetBaseData(baseData);
                instanceData.ID = _service.CreateCharacterUniqID();
                instanceData.Name = "Noname";
                return instanceData;
            }
        }
    }
}
    


