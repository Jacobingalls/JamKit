using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ObjectPoolItem
{
    public GameObject objectToPool;
    public int amountToPool;
    public bool shouldExpand = true;
}

public class ObjectPooler : MonoBehaviour
{
    public static ObjectPooler Instance;

    public Dictionary<string, List<GameObject>> pooledObjects;
    public List<ObjectPoolItem> itemsToPool;
    public Dictionary<int, Coroutine> coroutines;
    private Dictionary<string, ObjectPoolItem> itemsToPoolLookup;

    private void Awake()
    {
        Instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        pooledObjects = new Dictionary<string, List<GameObject>>();
        itemsToPoolLookup = new Dictionary<string, ObjectPoolItem>();
        coroutines = new Dictionary<int, Coroutine>();
        foreach (ObjectPoolItem item in itemsToPool)
        {
            itemsToPoolLookup[item.objectToPool.name] = item;
            pooledObjects[item.objectToPool.name] = new List<GameObject>();
            for (int i = 0; i < item.amountToPool; i++)
            {
                var obj = Instantiate(item.objectToPool);
                obj.transform.parent = transform;
                obj.SetActive(false);
                pooledObjects[item.objectToPool.name].Add(obj);
            }
        }

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public GameObject GetPooledObject(string name)
    {
        if(!itemsToPoolLookup.ContainsKey(name)) { return null; }

        var pooledObjectsForName = pooledObjects[name];
        var item = itemsToPoolLookup[name];

        for (int i = 0; i < pooledObjectsForName.Count; i++)
        {
            if (!pooledObjectsForName[i].activeInHierarchy)
            {
                var go = pooledObjectsForName[i];
                if (coroutines.ContainsKey(go.GetInstanceID()))
                {
                    var existingCoroutine = coroutines[go.GetInstanceID()];
                    StopCoroutine(existingCoroutine);
                }

                return go;
            }
        }

        if (item.shouldExpand)
        {
            var obj = Instantiate(item.objectToPool);
            obj.transform.parent = transform;
            obj.SetActive(false);
            pooledObjectsForName.Add(obj);
            return obj;
        } else
        {
            return null;
        }
    }

    IEnumerator ReturnObjectToPoolAfterDelayHelper(GameObject go, float time)
    {
        yield return new WaitForSeconds(time);
        go.SetActive(false); // referencing 'self' as a local variable is probably not needed, but not wrong, either.
    }

    public void ReturnObjectToPoolAfterDelay(GameObject go, float time)
    {
        if (coroutines.ContainsKey(go.GetInstanceID()))
        {
            var existingCoroutine = coroutines[go.GetInstanceID()];
            StopCoroutine(existingCoroutine);
        }

        var e = StartCoroutine(ReturnObjectToPoolAfterDelayHelper(go, time));
        coroutines[go.GetInstanceID()] = e;
    }
}