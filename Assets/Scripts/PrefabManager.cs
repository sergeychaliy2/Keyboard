using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

public class PrefabManager : MonoBehaviour
{
    [System.Serializable]
    public class PrefabEntry
    {
        public AssetReference prefabReference;
    }

    public List<PrefabEntry> prefabList = new List<PrefabEntry>();

    void Start()
    {
        LoadAllPrefabs();
    }

    // loading all prefabs from the list
    private void LoadAllPrefabs()
    {
        foreach (var entry in prefabList)
        {
            Addressables.LoadAssetAsync<GameObject>(entry.prefabReference).Completed += OnPrefabLoaded;
        }
    }

    // Callback upon successful loading of the prefab
    private void OnPrefabLoaded(AsyncOperationHandle<GameObject> handle)
    {
        if (handle.Status == AsyncOperationStatus.Succeeded)
        {
            Instantiate(handle.Result);
        }
        else
        {
            ErrorType.UnknownError.LogCustomError("Failed to load prefab.");
        }
    }

    // Uploading a prefab by AssetReference
    public void UnloadPrefab(AssetReference prefabReference)
    {
        Addressables.Release(prefabReference);
    }
}
