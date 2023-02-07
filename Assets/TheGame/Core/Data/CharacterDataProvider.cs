namespace TheGame.Data
{
    public class CharacterDataProvider
    {
        private readonly CharacterInstanceDataProvider _instanceData;
        private readonly CharacterBaseDataProvider _baseData;

        public CharacterInstanceDataProvider InstanceData => _instanceData;
        public CharacterBaseDataProvider BaseData => _baseData;

        public CharacterDataProvider(CharacterInstanceDataProvider instanceDataProvider
            ,CharacterBaseDataProvider baseDataProvider)
        {
            _instanceData = instanceDataProvider;
            _baseData = baseDataProvider;
        }
    }
}
    


