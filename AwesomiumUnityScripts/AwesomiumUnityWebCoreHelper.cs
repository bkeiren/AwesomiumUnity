using UnityEngine;
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
	
	void Awake()
	{
		if (m_Instance != null)
		{
			Debug.LogError("An instance of AwesomiumUnityWebCoreHelper already exists!");
			DestroyImmediate(this);
		}
		
		m_Instance = this;
		if (!AwesomiumUnityWebCore.IsRunning)
		{
			AwesomiumUnityWebCore.Initialize();	
		}
		DontDestroyOnLoad(this.gameObject);
	}
	
	void Update () 
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
