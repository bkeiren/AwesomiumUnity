using UnityEngine;
using System.Runtime.InteropServices;	// For DllImport.


public class SessionPreferences
{
    static public string InMemoryWebSessionPath = string.Empty;

    public string WebSessionPath = InMemoryWebSessionPath;
    public string PluginPath = Application.dataPath + "\\awe_plugins\\";
    public bool GPUAcceleration = true;
    public bool WebGL = true;
    public bool JavaScript = true;
    public bool Plugins = true;
    public bool WebAudio = true;
    public bool RemoteFonts = true;
    public bool AppCache = true;
    public bool Dart = true;
    public bool HTML5LocalStorage = true;
    public bool SmoothScrolling = true;
    public bool WebSecurity = true;
    public bool HideScrollBars = false;

    public override string ToString()
    {
        return string.Format("Session Path: {0} | Plugin Path: {1} | GPU Accel: {2} | WebGL: {3} | JavaScript: {4} | Plugins: {5} | Web Audio: {6} | Remote Fonts: {7} | App Cache: {8} | Dart: {9} | HTML5 Local Storage: {10} | Smooth Scrolling: {11} | Web Security: {12} | Hide Scrollbars {13}", WebSessionPath, PluginPath, GPUAcceleration, WebGL, JavaScript, Plugins, WebAudio, RemoteFonts, AppCache, Dart, HTML5LocalStorage, SmoothScrolling, WebSecurity, HideScrollBars);
    }
}


public class AwesomiumUnityWebSession
{
	internal const string DllName = "AwesomiumUnity";

	/// DLL Imported functions.
	[DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
	extern static private void awe_websession_clear_cache();

    [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
    extern static private void awe_websession_clear_cookies();

    [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
    extern static private bool awe_websession_isondisk();

    [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
    extern static private void awe_websession_setcookie([MarshalAs(UnmanagedType.LPWStr)]string _URL, [MarshalAs(UnmanagedType.LPWStr)]string _CookieString, bool _IsHTTPOnly, bool _ForceSessionCookie);


    // NOTE: Due to the way Awesomium internally uses the cache there is a specific set of steps that need to be done in order
    // before you can clear the cache. These steps are:
    // 1) Dispose (close/destroy) all WebViews.
    // 2) Update WebCore.
    // 3) Delete cache.
    public static void ClearCache()
    {
        awe_websession_clear_cache();
    }

    public static void ClearCookies()
    {
        awe_websession_clear_cookies();
    }

    public static bool IsOnDisk()
    {
        return awe_websession_isondisk();
    }

    public static void SetCookie(string _URL, string _CookieString, bool _IsHTTPOnly, bool _ForceSessionCookie)
    {
        awe_websession_setcookie(_URL, _CookieString, _IsHTTPOnly, _ForceSessionCookie);
    }

    // These are the settings that are used during WebCore initialization.
    // You can customize these settings by changing the initializer list of this object here.
    public static SessionPreferences Preferences = new SessionPreferences 
    {
        WebSessionPath = SessionPreferences.InMemoryWebSessionPath  // Examples paths: "C:\\MyCache", "MyCache" (will be relative to executable). An empty string signifies an in-memory session.
    };
}
