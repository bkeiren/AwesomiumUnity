using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif
using System.Collections;

public class AwesomiumUnityWebCoreHelper : MonoBehaviour 
{
	private static AwesomiumUnityWebCoreHelper m_Instance = null;
	public static AwesomiumUnityWebCoreHelper Instance
	{
		get 	
		{
			return m_Instance;
		}
	}

	/*void OnEnable()
	{
	#if UNITY_EDITOR
		EditorApplication.playmodeStateChanged += StateChange;
	#endif
	}
	
	#if UNITY_EDITOR
	void StateChange()
	{
		if (EditorApplication.isPlayingOrWillChangePlaymode && EditorApplication.isPlaying) {

				}

	}
	#endif


	void OnDisable()
	{
	#if UNITY_EDITOR
		EditorApplication.playmodeStateChanged -= StateChange;
	#endif
	}*/
	
	void Awake()
	{
		if (m_Instance != null)
		{
			Debug.LogError("An instance of AwesomiumUnityWebCoreHelper already exists!");
			DestroyImmediate(this);
		}
		
		m_Instance = this;
		AwesomiumUnityWebCore.EnsureInitialized();
		DontDestroyOnLoad(this.gameObject);
	}

	void LateUpdate () 
	{
	    AwesomiumUnityWebCore.Update();
	}
	
	void DoShutdown()
	{
		if (AwesomiumUnityWebCore.IsRunning)
		{
			AwesomiumUnityWebCore.Shutdown();	
		}	
	}
	
	void OnDestroy()
	{
		DoShutdown();
		m_Instance = null;
	}
	
	void OnApplicationQuit()
	{
		DoShutdown();
		m_Instance = null;
	}
}
