using System.Collections.Generic;


namespace TheGame
{
    public class CharacterInstanceDataProvider
    {
        private Dictionary<string, CharacterInstanceData> _charactersData = new();

        public void AddCharacterStatsData(CharacterInstanceData data)
        {
            if (!_charactersData.ContainsKey(data.ID))
            {
                _charactersData.Add(data.ID, data);
            }
        }

        public ICharacterInstanceData Get(string id)
        {
            if (_charactersData.ContainsKey(id))
            {
                return _charactersData[id];
            }
            else throw new System.ArgumentNullException(
                string.Format("There is no instance finded with ID {0}", id));
        }
    }
}
    


