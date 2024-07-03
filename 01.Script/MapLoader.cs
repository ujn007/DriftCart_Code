using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

public class MapLoader : MonoSingleton<MapLoader>
{
    public event Action<int> loadMapComplete;

    private string prefabAddress;
    private List<GameObject> spawnOBJs = new();
    private float spawnStage;

    private void Start()
    {
        SpawnMap(1);
        DontDestroyOnLoad(gameObject);
    }

    public void SpawnMap(int stage)
    {
        spawnStage = stage;
        prefabAddress = $"track{stage}";

        Addressables.LoadAssetAsync<GameObject>(prefabAddress).Completed += OnLoadCompleted;
        loadMapComplete?.Invoke(stage);
    }

    public void StartMusic()
    {
        AudioManager.Instance.PlayMusic($"Stage{spawnStage}");
    }

    private void OnLoadCompleted(AsyncOperationHandle<GameObject> obj)
    {
        if (obj.Status == AsyncOperationStatus.Succeeded)
        {
            if (spawnOBJs.Count > 0)
                ClearSpawnObject();

            GameObject spawnObject = Instantiate(obj.Result, Vector3.zero, Quaternion.identity);
            spawnOBJs.Add(spawnObject);
        }
        else
        {
            Debug.LogError("Failed to load Addressable Asset: " + prefabAddress);
        }
    }

    public void ClearSpawnObject()
    {
        foreach (GameObject obj in spawnOBJs)
        {
            Destroy(obj);
        }
        spawnOBJs.Clear();
    }
}
