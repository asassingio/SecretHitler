/*
 * Base class for singletons. Inherit from this class to create a monobehavior singleton.
 * This is useful for manager classes, main game logic, or any 'static' class that you want handy access to or need monobehavior functionality.
 *
 * Usage:
 * public class SomeManagerClass : Manager<SomeManagerClass>
 * {
 * 	public bool someBool;
 * }
 *
 * to get the value of someBool anywhere in code:
 * SomeManagerClass.Instance.someBool;
 * */

using System.Collections;
using UnityEngine;
using FishNet;
using FishNet.Connection;
using FishNet.Managing;
using FishNet.Managing.Client;
using FishNet.Object;
using FishNet.Object.Synchronizing;
using FishNet.Transporting;

namespace Managers.CanvasManager
{
	public class Manager<T> : NetworkBehaviour where T : NetworkBehaviour
	{
		[SerializeField] protected View[] views;

		// IsInitialized will be true at the end of Initialize
		// (at the end of a Coroutine so could take few frames)
		// for this reason if base.Initialize is called first in an override method
		// it's suggested to start a new Coroutine to check when IsInitialized will be set to true
		protected bool IsInitialized;
		
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

		protected virtual void Awake()
		{
			// If there is an instance, and it's not me, delete myself.
			if (_instance != null && Instance != this)
			{
				Destroy(gameObject);
				return;
			}

			_instance = this as T;

			// uncomment this to make it persist through scenes
			// DontDestroyOnLoad(gameObject);
		}
		
		public virtual void Initialize(PlayerController playerController = null)
		{
			StartCoroutine(InitializeCoroutine(playerController));
		}
		
		private IEnumerator InitializeCoroutine(PlayerController playerController)
		{
			while (!IsClientInitialized)
			{
				Debug.LogWarning($"{name} iIsClientInitialized: {IsClientInitialized}");
				yield return null;
			}
			Debug.Log($"{name} iIsClientInitialized: {IsClientInitialized}");

			if (playerController != null)
			{
				// static method
				View.SetOwnerPlayer(playerController);
			}
			else
			{
				Debug.LogError($"playerController reference is null");
			}

			foreach (View view in views)
			{
				view.Initialize();
				view.Hide();
			}

			IsInitialized = true;
		}
		
		
		public virtual void Show<TView>() where TView : View
		{
			foreach (View view in views)
			{
				if (view is TView)
				{
					view.Hide();
					view.Show();
				}
				else
				{
					view.Hide();
				}
			}
		}
	}
}
