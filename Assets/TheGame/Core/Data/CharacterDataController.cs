using Cysharp.Threading.Tasks;
using TheGame.Utils;

namespace TheGame.Data
{
    public interface ICharacterDataGetter
    {
        ICharacterInstanceData CreateNewInstanceData(Character character);
        ICharacterInstanceData[] GetAllInstancesData();
        ICharacterInstanceData GetInstanceData(string id);
    }

    public interface ICharacterDataSetter
    {
        void AddDataStatModifierToInstance(string instanceID, DataStatModifier modifier, string reason = null);
        void RemoveStatModifierFromInstance(string instanceID, StatType stat, TripleKey key, string reason = null);
    }

    public sealed class CharacterDataMediator : BaseMediator<CharacterDataUtils>, ICharacterDataGetter, ICharacterDataSetter
    {
        private const string kInstancesKeys = "characters_instances_keys";
        private readonly IDatabase _database;
        private readonly CharacterInstanceDataProvider _instanceData;
        private readonly CharacterBaseDataProvider _baseData;

        public CharacterDataMediator(IDatabase database) : base()
        {
            _database = database;
            _instanceData = new CharacterInstanceDataProvider();
            _baseData = new CharacterBaseDataProvider();
        }

        public override async UniTask Initialize()
        {
            await base.Initialize();
            _baseData.Init(_database.CharactersDatabase);
            string[] instanceIDs = Utils.Parse<string[]>(kInstancesKeys);
            for (int i = 0, j = instanceIDs.Length; i < j; i++)
            {
                var instance = Utils.Parse<CharacterInstanceData>(instanceIDs[i]);
                _instanceData.AddCharacterInstanceData(instance);
            }
        }

        public ICharacterInstanceData CreateNewInstanceData(Character character)
        {
            var baseData = _baseData.Get(character);
            var id = _instanceData.CreateCharacterUniqID();
            var instance = _instanceData.BuildNewCharacterInstanceDataFromBase(id, baseData);
            _instanceData.AddCharacterInstanceData(instance);
            Utils.SaveInstanceData(instance);
            return instance;
        }

        public ICharacterInstanceData GetInstanceData(string id)
        {
            return _instanceData.GetCharacterInstanceData(id);
        }

        public ICharacterInstanceData[] GetAllInstancesData()
        {
            return _instanceData.GetAllInstancesDatas();
        }

        public void AddDataStatModifierToInstance(string instanceID, DataStatModifier modifier, string reason = null)
        {
            var instance = _instanceData.GetCharacterInstanceData(instanceID);
            instance.AddStatModifier(modifier);
            Utils.SaveInstanceData(instance);
        }

        public void RemoveStatModifierFromInstance(string instanceID, StatType stat, TripleKey key, string reason = null)
        {
            var instance = _instanceData.GetCharacterInstanceData(instanceID);
            instance.RemoveStatModifier(stat, key);
            Utils.SaveInstanceData(instance);
        }
    }

    public class CharacterDataUtils : BaseDataUtility
    {
        public void SaveInstanceData(CharacterInstanceData instanceData)
        {
            string jsonString = JsonUtility.ToJson(instanceData);
            GPrefs.SetString(instanceData.ID, jsonString);
        }

        public CharacterInstanceData LoadInstanceData(string id)
        {
            string jsonString = GPrefs.GetString(id);
            var data = JsonUtility.FromJson<CharacterInstanceData>(jsonString);
            return data;
        }

        public T Parse<T> (string key)
        {
            try
            {
                string json = GPrefs.GetString(key);
                var result = JsonUtility.FromJson<T>(json);
                return result;
            }
            catch (System.Exception ex)
            {
                UnityEngine.Debug.LogFormat("Parsed {0} with {1} key can't be parsed", typeof(T), key);
                return default;
            }

        }
    }
}
    


