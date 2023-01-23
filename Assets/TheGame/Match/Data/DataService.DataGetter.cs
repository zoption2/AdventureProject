using System;


namespace TheGame
{

    public partial class DataService
    {
        private partial class DataGetter : IDataGetter
        {
            private DataService _service;

            public DataGetter(DataService service)
            {
                _service = service;
            }

            public CharacterInstanceData GetCharacterInstanceData(string id)
            {
                CharacterInstanceData data = default;
                try
                {
                    data = _service.Character.InstanceData.Get(id);
                }
                catch (Exception ex)
                {
                    
                }

                return data;
            }

            public CharacterInstanceData GetNewCharacterInstanceData(Character character)
            {
                var baseData = _service.Character.BaseData.Get(character);
                var instanceData = BuildNewCharacterInstanceDataFromBase(baseData);
                return instanceData;
            }

            private CharacterInstanceData BuildNewCharacterInstanceDataFromBase(CharacterBaseData baseData)
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
    


