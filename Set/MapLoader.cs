using System;
using System.Collections.Generic;
using System.IO.MemoryMappedFiles;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.UI;

public class MapLoader : MonoSingleton<MapLoader>
{
    [SerializeField] private RawImage raw;
    [SerializeField] private Camera mapCam;

    public event Action<int> loadMapComplete;

    private string prefabAddress;
    private List<GameObject> spawnOBJs = new();
    private int spawnStage;

    private void Start()
    {
        WhatIsTrackType();
    }

    private void WhatIsTrackType()
    {
        SpawnMap(1);
    }

    public void SpawnMap(int stage)
    {
        spawnStage = stage;
        prefabAddress = $"track{stage}";

        Addressables.LoadAssetAsync<GameObject>(prefabAddress).Completed += OnLoadCompleted;
        loadMapComplete?.Invoke(spawnStage);
    }

    public void StartMusic()
    {
        try
        {
            AudioManager.Instance.PlayMusic($"Stage{spawnStage}");
        }
        catch (Exception e)
        {
            Debug.Log(e);
        }
    }

    private void OnLoadCompleted(AsyncOperationHandle<GameObject> obj)
    {
        if (obj.Status == AsyncOperationStatus.Succeeded)
        {
            if (spawnOBJs.Count > 0)
                ClearSpawnObject();

            GameObject spawnObject = Instantiate(obj.Result, Vector3.zero, Quaternion.identity);
            MapCam mapCamPos = spawnObject.transform.Find("CamPos").GetComponent<MapCam>();
            mapCam.transform.SetPositionAndRotation(mapCamPos.transform.position, mapCamPos.transform.rotation);
            mapCam.orthographicSize = mapCamPos.camSize;

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
