﻿using TheGame.Utils;

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

    public class CharacterDataProvider : BaseProvider<CharacterDataUtils>, ICharacterDataGetter, ICharacterDataSetter
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
    }
}
    


