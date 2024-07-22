using UnityEngine;
using FishNet.Object;

namespace Managers
{
	public class Manager<T> : NetworkBehaviour where T : NetworkBehaviour
	{
		public bool IsInitialized { get; private set; }
		
		private static T _instance;
		private static readonly object Lock = new object();

		public static T Instance
		{
			get
			{
				// Ensure that only one thread can access at a time.
				lock (Lock)
				{
					if (_instance == null)
					{
						_instance = (T)FindObjectOfType(typeof(T));

						if (FindObjectsOfType(typeof(T)).Length > 1)
						{
							return _instance;
						}

						if (_instance == null)
						{
							GameObject singleton = new GameObject();
							_instance = singleton.AddComponent<T>();
							singleton.name = "(singleton) " + typeof(T).ToString();
						}
					}
					return _instance;
				}
			}
		}

		private void Awake()
		{
			// If there is an instance, and it's not me, delete myself.
			if (_instance != null && Instance != this)
			{
				Destroy(gameObject);
				return;
			}

			_instance = this as T;
			// gameObject.SetActive(true);
			InitializeOnce();
		}
		
		public virtual void InitializeOnce()
		{
			IsInitialized = true;
		}
	}
}