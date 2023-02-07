using UnityEngine;
using TheGame.Utils;

namespace TheGame.Data
{
    public interface ICharactersDatabase
    {
        ICharacterBaseData GetBaseData(Character character);
        ICharacterBaseData[] GetAllBaseDatas();
    }

    [CreateAssetMenu(fileName = "CharactersDatabase", menuName = "ScriptableObjects/CharactersDatabase")]
    public class CharactersDatabase : ScriptableObject, ICharactersDatabase
    {
        [field: SerializeField] public ScriptableCharacterData[] CharactersBaseData { get; private set; }

        public ICharacterBaseData GetBaseData(Character character)
        {
            for (int i = 0, j = CharactersBaseData.Length; i < j; i++)
            {
                if (CharactersBaseData[i].Character == character)
                {
                    return CharactersBaseData[i];
                }
            }
            throw new System.ArgumentException(
                string.Format("{0} character not found at database", character)
                );
        }

        public ICharacterBaseData[] GetAllBaseDatas()
        {
            var allData = new ICharacterBaseData[CharactersBaseData.Length];
            for (int i = 0, j = CharactersBaseData.Length; i < j; i++)
            {
                allData[i] = CharactersBaseData[i];
            }
            return allData;
        }
    }
}
    


