using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class ObjectPoolManager : MonoBehaviour
{
    public static List<PooledObjectInfo> objectPools = new List<PooledObjectInfo>();

    private GameObject _objectPoolEmptyHolder;

    private static GameObject _particleSystemsEmpty;
    private static GameObject _gameObjectsEmpty;
    public enum PoolType
    {
        ParticleSystem,
        GameObject,
        None
    }

    public static PoolType poolingType;

    private void Awake()
    {
        SetupEmpties();
    }

    private void SetupEmpties()
    {
        _objectPoolEmptyHolder = new GameObject("Pooled Objects");

        _particleSystemsEmpty = new GameObject("Particle Effects");
        _particleSystemsEmpty.transform.SetParent(_objectPoolEmptyHolder.transform);

        _gameObjectsEmpty = new GameObject("Game Objects");
        _gameObjectsEmpty.transform.SetParent(_objectPoolEmptyHolder.transform);
    }

    public static GameObject SpawnObject(GameObject objectToSpawn, Vector3 spawnPosition, Quaternion spawnRotation, PoolType poolType = PoolType.None)
    {
        PooledObjectInfo pool = objectPools.Find(p => p.lookupString == objectToSpawn.name);

        if (pool == null)
        {
            pool = new PooledObjectInfo() { lookupString = objectToSpawn.name };
            objectPools.Add(pool);
        }

        GameObject spawnableObject = pool.inactiveObjects.FirstOrDefault();

        if (spawnableObject == null)
        {
            GameObject parentObject = SetParentObject(poolType);
            spawnableObject = Instantiate(objectToSpawn, spawnPosition, spawnRotation);

            if (parentObject != null)
            {
                spawnableObject.transform.SetParent(parentObject.transform);
            }
        } else
        {
            spawnableObject.transform.position = spawnPosition;
            spawnableObject.transform.rotation = spawnRotation;
            pool.inactiveObjects.Remove(spawnableObject);
            spawnableObject.SetActive(true);
        }

        return spawnableObject;
    }

    public static void ReturnObjectToPool(GameObject obj)
    {
        string objName = obj.name.Substring(0, obj.name.Length - 7); // remove "Clone" from object names

        PooledObjectInfo pool = objectPools.Find(p => p.lookupString == objName);

        if (pool == null)
        {
            Debug.LogWarning("Trying to release an object that is not pooled: " + obj.name);

        } else
        {
            obj.SetActive(false);
            pool.inactiveObjects.Add(obj);
        }
    }

    private static GameObject SetParentObject(PoolType poolType)
    {
        switch (poolType)
        {
            case PoolType.ParticleSystem:
                return _particleSystemsEmpty;
            case PoolType.GameObject:
                return _gameObjectsEmpty;
            case PoolType.None:
                return null;
            default:
                return null;
        }
    }

}

public class PooledObjectInfo
{
    public string lookupString;
    public List<GameObject> inactiveObjects = new List<GameObject>();
}
