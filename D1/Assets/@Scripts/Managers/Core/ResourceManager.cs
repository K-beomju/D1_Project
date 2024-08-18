using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using Object = UnityEngine.Object;

public class ResourceManager
{
	private Dictionary<string, UnityEngine.Object> _resources = new Dictionary<string, UnityEngine.Object>();
	private Dictionary<string, AsyncOperationHandle> _handles = new Dictionary<string, AsyncOperationHandle>();

	#region Load Resource

    /// <summary>
	/// 리소스를 동기적으로 로드합니다.
	/// 이미 로드된 리소스가 있다면 캐시된 리소스를 반환합니다.
	/// </summary>
	/// <typeparam name="T">로드할 리소스의 타입</typeparam>
	/// <param name="key">리소스의 키(이름)</param>
	/// <returns>로드된 리소스 객체</returns>
	public T Load<T>(string key) where T : Object
	{
		if (_resources.TryGetValue(key, out Object resource))
			return resource as T;

		// if (typeof(T) == typeof(Sprite) && key.Contains(".sprite") == false)
		// {
		// 	if (_resources.TryGetValue($"{key}.sprite", out resource))
		// 		return resource as T;
		// }

		return null;
	}


    /// <summary>
	/// 로드된 리소스를 기반으로 게임 오브젝트를 인스턴스화합니다.
	/// 리소스를 찾을 수 없는 경우, 에러 메시지를 출력합니다.
	/// </summary>
	/// <param name="key">리소스의 키(이름)</param>
	/// <param name="parent">부모 오브젝트 트랜스폼(선택적)</param>
	/// <param name="pooling">풀링 시스템을 사용할지 여부(선택적)</param>
	/// <returns>인스턴스화된 게임 오브젝트</returns>
	public GameObject Instantiate(string key, Transform parent = null, bool pooling = false)
	{
		GameObject prefab = Load<GameObject>(key);
		if (prefab == null)
		{
			Debug.LogError($"Failed to load prefab : {key}");
			return null;
		}

		// if (pooling)
		// 	return Managers.Pool.Pop(prefab);

		GameObject go = Object.Instantiate(prefab, parent);
		go.name = prefab.name;

		return go;
	}

	public void Destroy(GameObject go)
	{
		if (go == null)
			return;

		if (Managers.Pool.Push(go))
			return;

		Object.Destroy(go);
	}
	#endregion

	#region Addressable

    /// <summary>
	/// 리소스를 비동기적으로 로드합니다.
	/// 로드가 완료되면 콜백 함수가 호출됩니다.
	/// </summary>
	/// <typeparam name="T">로드할 리소스의 타입</typeparam>
	/// <param name="key">리소스의 키(이름)</param>
	/// <param name="callback">로드가 완료되었을 때 호출될 콜백 함수(선택적)</param>
	private void LoadAsync<T>(string key, Action<T> callback = null) where T : UnityEngine.Object
	{
		// Cache
		if (_resources.TryGetValue(key, out Object resource))
		{
			callback?.Invoke(resource as T);
			return;
		}

		string loadKey = key;
		if (key.Contains(".sprite"))
			loadKey = $"{key}[{key.Replace(".sprite", "")}]";

		var asyncOperation = Addressables.LoadAssetAsync<T>(loadKey);
		asyncOperation.Completed += (op) =>
		{
			_resources.Add(key, op.Result);
			_handles.Add(key, asyncOperation);
			callback?.Invoke(op.Result);
		};
	}


    /// <summary>
	/// 특정 레이블의 모든 리소스를 비동기적으로 로드합니다.
	/// 각 리소스가 로드될 때마다 콜백 함수가 호출됩니다.
	/// </summary>
	/// <typeparam name="T">로드할 리소스의 타입</typeparam>
	/// <param name="label">리소스의 레이블</param>
	/// <param name="callback">리소스가 로드될 때 호출될 콜백 함수</param>
	public void LoadAllAsync<T>(string label, Action<string, int, int> callback) where T : UnityEngine.Object
	{
		var opHandle = Addressables.LoadResourceLocationsAsync(label, typeof(T));
		opHandle.Completed += (op) =>
		{
			int loadCount = 0;
			int totalCount = op.Result.Count;

			foreach (var result in op.Result)
			{
				if (result.PrimaryKey.Contains(".sprite"))
				{
					LoadAsync<Sprite>(result.PrimaryKey, (obj) =>
					{
						loadCount++;
						callback?.Invoke(result.PrimaryKey, loadCount, totalCount);
					});
				}
				else
				{
					LoadAsync<T>(result.PrimaryKey, (obj) =>
					{
						loadCount++;
						callback?.Invoke(result.PrimaryKey, loadCount, totalCount);
					});
				}
			}
		};
	}
    
    /// <summary>
	/// 모든 로드된 리소스와 핸들을 해제하여 메모리를 정리합니다.
	/// </summary>
	public void Clear()
	{
		_resources.Clear();

		foreach (var handle in _handles)
			Addressables.Release(handle);

		_handles.Clear();
	}
	#endregion
}
