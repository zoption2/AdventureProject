using UnityEngine;

namespace TheGame.Data
{
    public interface IDatabase
    {
        ICharactersDatabase CharactersDatabase { get; }
    }

    [CreateAssetMenu(fileName = "BaseDataProvider", menuName = "ScriptableObjets/BaseDataProvider")]
    public class InternalDatabase : ScriptableObject, IDatabase
    {
        [SerializeField] private CharactersDatabase _charactersDatabase;

        public ICharactersDatabase CharactersDatabase => _charactersDatabase;
    }
}


