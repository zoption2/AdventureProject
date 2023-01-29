using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using Cysharp.Threading.Tasks;
using System.Collections.Generic;

namespace TheGame
{
    public interface ICharacterProvider
    {
        UniTask<GameObject> GetCharacterAsync(Character character);
        void Release(int instanceID);
    }

    [CreateAssetMenu(fileName = "CharacterProvider", menuName = "ScriptableObjects/CharacterProvider")]
    public sealed class CharacterProvider : ScriptableObject, ICharacterProvider
    {
        [SerializeField] private CharacterAssetData[] _charactersData;
        private Dictionary<int, AsyncOperationHandle<GameObject>> _loadedData = new();

        public async UniTask<GameObject> GetCharacterAsync(Character character)
        {
            AsyncOperationHandle<GameObject> operationHandle;
            for (int i = 0, j = _charactersData.Length; i < j; i++)
            {
                if (_charactersData[i].Character == character)
                {
                    try
                    {
                        operationHandle = Addressables.InstantiateAsync(_charactersData[i].PrefabRef);
                        await operationHandle;
                        GameObject go = operationHandle.Result;
                        _loadedData.Add(go.GetInstanceID(), operationHandle);
                        return operationHandle.Result;
                    }
                    catch (System.Exception ex)
                    {
                        Debug.LogFormat("Can't load async {0} because of {1}",character, ex);
                    }
                }
            }
            throw new System.ArgumentNullException(
                string.Format("Character type {0} not found at {1} database", character, this.GetType()));
        }

        public void Release(int instanceID)
        {
            if (_loadedData.ContainsKey(instanceID))
            {
                Addressables.Release(_loadedData[instanceID]);
                Debug.Log(string.Format("{0} was released", _loadedData[instanceID].Result.name));
            }
        }

        [System.Serializable]
        private class CharacterAssetData
        {
            [field: SerializeField] public Character Character { get; private set; }
            [field: SerializeField] public AssetReferenceGameObject PrefabRef { get; private set; }
        }
    }

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


