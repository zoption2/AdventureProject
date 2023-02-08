using System.Collections.Generic;
using TheGame.Utils;

namespace TheGame.Data
{
    public class CharacterInstanceDataProvider
    {
        private const int kIdLength = 7;
        private Dictionary<string, CharacterInstanceData> _charactersData = new();

        public void AddCharacterInstanceData(CharacterInstanceData data)
        {
            if (!_charactersData.ContainsKey(data.ID))
            {
                _charactersData.Add(data.ID, data);
            }
        }

        public CharacterInstanceData GetCharacterInstanceData(string id)
        {
            if (_charactersData.ContainsKey(id))
            {
                return _charactersData[id];
            }
            else throw new System.ArgumentNullException(
                string.Format("There is no instance finded with ID {0}", id));
        }

        public CharacterInstanceData[] GetAllInstancesDatas()
        {
            var data = new CharacterInstanceData[_charactersData.Count];
            _charactersData.Values.CopyTo(data, 0);
            return data;
        }

        public CharacterInstanceData BuildNewCharacterInstanceDataFromBase(string id, ICharacterBaseData baseData)
        {
            var instanceData = new CharacterInstanceData(id, baseData);
            return instanceData;
        }

        public string CreateCharacterUniqID()
        {
            string id = "";
            bool isIdAvailable = false;
            while (!isIdAvailable)
            {
                id = SupportUtility.GetUniqueKey(kIdLength);
                if (!_charactersData.ContainsKey(id))
                {
                    isIdAvailable = true;
                }
            }
            return id;
        }
    }
}
    


