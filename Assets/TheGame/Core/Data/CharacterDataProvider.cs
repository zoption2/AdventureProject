using TheGame.Utils;

namespace TheGame.Data
{
    public interface IDataUtils
    {

    }

    public abstract class BaseProvider<T> where T: IDataUtils, new()
    {
        protected T Utils { get; private set; }

        public BaseProvider()
        {
            Utils = new T();
        }
    }

    public class CharacterDataUtils : IDataUtils
    {
        public void SaveInstanceData(CharacterInstanceData instanceData)
        {
            string jsonString = JsonDataService.ConvertToSting(instanceData);
            GPrefsUtils.SetString(instanceData.ID, jsonString);
        }

        public CharacterInstanceData LoadInstanceData(string id)
        {
            string jsonString = GPrefsUtils.GetString(id);
            var data = JsonDataService.ConvertFromString<CharacterInstanceData>(jsonString);
            return data;
        }
    }

    public class CharacterDataProvider : BaseProvider<CharacterDataUtils>
    {
        private readonly CharacterInstanceDataProvider _instanceData;
        private readonly CharacterBaseDataProvider _baseData;

        public CharacterInstanceDataProvider InstanceData => _instanceData;
        public CharacterBaseDataProvider BaseData => _baseData;

        public CharacterDataProvider(CharacterInstanceDataProvider instanceDataProvider
            ,CharacterBaseDataProvider baseDataProvider) : base()
        {
            _instanceData = instanceDataProvider;
            _baseData = baseDataProvider;
        }

        public ICharacterInstanceData CreateNewCharacterInstanceData(Character character)
        {
            var baseData = _baseData.Get(character);
            var id = _instanceData.CreateCharacterUniqID();
            var instance = _instanceData.BuildNewCharacterInstanceDataFromBase(id, baseData);
            _instanceData.AddCharacterInstanceData(instance);
            Utils.SaveInstanceData(instance);
            return instance;
        }
    }
}
    


