using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using Cysharp.Threading.Tasks;

namespace TheGame
{

    public interface IPlayerView
    {

    }

    public class PlayerView : MonoBehaviour, IPlayerView
	{
        [SerializeField] private Transform _spellPoint;


	}

    [CreateAssetMenu(fileName = "CharacterProvider", menuName = "ScriptableObjects/CharacterProvider")]
    public sealed class CharacterProvider : ScriptableObject
    {
        [field: SerializeField] public Character Character { get; private set; }
        [field: SerializeField] public AssetReferenceGameObject PrefabRef { get; private set; }
        private AsyncOperationHandle<GameObject> _operationHandle;


        public async UniTask<GameObject> GetCharacter(Character character)
        {
            _operationHandle = Addressables.LoadAssetAsync<GameObject>(PrefabRef);
            await _operationHandle;
            return _operationHandle.Result;
        }

        public void Release()
        {
            Addressables.Release(_operationHandle);
        }
    }

    public enum Character
    {
        Samurai
    }
}


