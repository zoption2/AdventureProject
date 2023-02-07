using UnityEngine;

namespace TheGame
{
    public interface IPrefabsProvider
    {
        ICharacterProvider CharacterProvider { get; }
    }

    [CreateAssetMenu(fileName = "PrefabsProvider", menuName = "ScriptableObjets/PrefabsProvider")]
    public class PrefabsProvider : ScriptableObject, IPrefabsProvider
    {
        [SerializeField] private CharacterProvider _characterProvider;

        public ICharacterProvider CharacterProvider => _characterProvider;
    }


}


