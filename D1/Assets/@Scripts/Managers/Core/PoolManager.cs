using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

internal class Pool
{
    private GameObject _prefab;
    private IObjectPool<GameObject> _pool;

    private Transform _root;
    public Transform Root
    {
        get
        {
            if (_root == null)
            {
                GameObject go = new GameObject() { name = $"@{_prefab.name}Pool" };
                _root = go.transform;
            }

            return _root;
        }
    }

    public Pool(GameObject prefab)
    {
        _prefab = prefab;
        _pool = new ObjectPool<GameObject>(OnCreate, OnGet, OnRelease, OnDestroy);
    }

    public void Push(GameObject go)
    {
        if (go.activeSelf)
            _pool.Release(go);
    }

    public GameObject Pop()
    {
        return _pool.Get();
    }

    #region Func
    private GameObject OnCreate()
    {
        GameObject go = GameObject.Instantiate(_prefab);
        go.transform.SetParent(Root);
        go.name = _prefab.name;
        return go;
    }

    private void OnGet(GameObject go)
    {
        go.SetActive(true);
    }

    private void OnRelease(GameObject go)
    {
        go.SetActive(false);
    }

    private void OnDestroy(GameObject go)
    {
        GameObject.Destroy(go);
    }
    #endregion

}


public class PoolManager
{
    private Dictionary<string, Pool> _pool = new Dictionary<string, Pool>();

    public GameObject Pop(GameObject prefab)
    {
        if (_pool.ContainsKey(prefab.name) == false)
            CreatePool(prefab);

        return _pool[prefab.name].Pop();
    }

    public bool Push(GameObject go)
    {
        if (_pool.ContainsKey(go.name) == false)
            return false;

        _pool[go.name].Push(go);
        return true;
    }

    public void Clear()
    {
        _pool.Clear();
    }

    private void CreatePool(GameObject original)
    {
        Pool pool = new Pool(original);
        _pool.Add(original.name, pool);
    }
}
