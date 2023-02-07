using UnityEngine;

namespace TheGame.Data
{
    public interface IDatabaseProvider
    {
        ICharactersDatabase CharactersDatabase { get; }
    }

    [CreateAssetMenu(fileName = "BaseDataProvider", menuName = "ScriptableObjets/BaseDataProvider")]
    public class DatabaseProvider : ScriptableObject, IDatabaseProvider
    {
        [SerializeField] private CharactersDatabase _charactersDatabase;

        public ICharactersDatabase CharactersDatabase => _charactersDatabase;
    }
}


