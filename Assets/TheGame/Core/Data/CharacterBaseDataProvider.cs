using System.Collections.Generic;
using Cysharp.Threading.Tasks;


namespace TheGame.Data
{
    public class CharacterBaseDataProvider
    {
        private Dictionary<Character, ICharacterBaseData> _charactersData = new();

        public void Init(ICharactersDatabase database)
        {
            var allBaseData = database.GetAllBaseDatas();
            for (int i = 0, j = allBaseData.Length; i < j; i++)
            {
                AddBaseData(allBaseData[i]);
            }
        }

        public void AddBaseData(ICharacterBaseData data)
        {
            if (!_charactersData.ContainsKey(data.Character))
            {
                _charactersData.Add(data.Character, data);
            }
        }

        public ICharacterBaseData Get(Character character)
        {
            if (_charactersData.ContainsKey(character))
            {
                return _charactersData[character];
            }
            else throw new System.ArgumentNullException(
                string.Format("There is no base data exist for character {0}", character));
        }
    }
}
    


