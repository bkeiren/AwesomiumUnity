#pragma once

#include "UnityPlugin.h"
#include "LoadListener.h"
#include "ViewListener.h"

namespace Awesomium
{
    class WebView;
    class WebKeyboardEvent;
    struct WebTouchEvent;
    enum Error;
}

namespace AwesomiumUnity
{

    typedef void(*ExecuteJSResultCallbackBool)(bool, int);				// Param 1: The bool. Param 2: User data.
    typedef void(*ExecuteJSResultCallbackInt)(int, int);				// Param 1: The integer. Param 2: User data.
    typedef void(*ExecuteJSResultCallbackFloat)(float, int);			// Param 1: The float. Param 2: User data.
    typedef void(*ExecuteJSResultCallbackString)(const wchar16*, int);	// Param 1: The string. Param 2: User data.
    typedef void(*ExecuteJSResultCallbackNullOrUndefined)(int);			// Param 1: User data.
    typedef void(*ExecuteJSResultCallbackArray)(int, int);				// Param 1: The length of the array OR -1 for the second callback after array iteration. Param 2: User data. Other callbacks will be called for each item in the array. This callback is called once before the items are iterated over, and once after iteration has finished (with its length argument being -1).

}


extern "C" EXPORT_API void awe_webview_loadurl(Awesomium::WebView* _Instance, char* _URL);

extern "C" EXPORT_API bool awe_webview_iscrashed(Awesomium::WebView* _Instance);

extern "C" EXPORT_API bool awe_webview_isloading(Awesomium::WebView* _Instance);

extern "C" EXPORT_API bool awe_webview_istransparent(Awesomium::WebView* _Instance);

extern "C" EXPORT_API void awe_webview_destroy(Awesomium::WebView* _Instance);

extern "C" EXPORT_API void awe_webview_copybuffertotexture(Awesomium::WebView* _Instance, void* _TextureNativePtr, int _UnityTextureWidth, int _UnityTextureHeight);

extern "C" EXPORT_API int awe_webview_surface_width(Awesomium::WebView* _Instance);

extern "C" EXPORT_API int awe_webview_surface_height(Awesomium::WebView* _Instance);

extern "C" EXPORT_API bool awe_webview_surface_isdirty(Awesomium::WebView* _Instance);

extern "C" EXPORT_API void awe_webview_reload(Awesomium::WebView* _Instance, bool _IgnoreCache);

extern "C" EXPORT_API void awe_webview_inject_mousemove(Awesomium::WebView* _Instance, int _X, int _Y);

extern "C" EXPORT_API void awe_webview_inject_mousedown(Awesomium::WebView* _Instance, int _Button);

extern "C" EXPORT_API void awe_webview_inject_mouseup(Awesomium::WebView* _Instance, int _Button);

extern "C" EXPORT_API void awe_webview_inject_mousewheel(Awesomium::WebView* _Instance, int _ScrollVert, int _ScrollHorz);

extern "C" EXPORT_API void awe_webview_inject_keyboardevent(Awesomium::WebView* _Instance, Awesomium::WebKeyboardEvent& _WebKeyBoardEvent);

extern "C" EXPORT_API void awe_webview_inject_touchevent(Awesomium::WebView* _Instance, Awesomium::WebTouchEvent& _WebTouchEvent);

extern "C" EXPORT_API void awe_webview_resize(Awesomium::WebView* _Instance, int _Width, int _Height);

extern "C" EXPORT_API void awe_webview_executejavascript(Awesomium::WebView* _Instance, char* _Script, int _ExecutionID);

extern "C" EXPORT_API void awe_webview_executejavascriptwithresult(Awesomium::WebView* _Instance, char* _Script, int _ExecutionID);

extern "C" EXPORT_API void awe_webview_focus(Awesomium::WebView* _Instance);

extern "C" EXPORT_API void awe_webview_unfocus(Awesomium::WebView* _Instance);

extern "C" EXPORT_API void awe_webview_stop(Awesomium::WebView* _Instance);

extern "C" EXPORT_API void awe_webview_settransparent(Awesomium::WebView* _Instance, bool _Transparent);

extern "C" EXPORT_API Awesomium::Error awe_webview_lasterror(Awesomium::WebView* _Instance);

extern "C" EXPORT_API bool awe_webview_cangoback(Awesomium::WebView* _Instance);

extern "C" EXPORT_API bool awe_webview_cangoforward(Awesomium::WebView* _Instance);

extern "C" EXPORT_API void awe_webview_goback(Awesomium::WebView* _Instance);

extern "C" EXPORT_API void awe_webview_goforward(Awesomium::WebView* _Instance);

extern "C" EXPORT_API void awe_webview_gotohistoryoffset(Awesomium::WebView* _Instance, int _Offset);

extern "C" EXPORT_API void awe_webview_js_setmethod(Awesomium::WebView* _Instance, char* _MethodName, bool _HasReturnValue);

//Extra Methods
extern "C" EXPORT_API void awe_webview_copyclipboard(Awesomium::WebView* _Instance);
extern "C" EXPORT_API void awe_webview_pasteclipboard(Awesomium::WebView* _Instance);
extern "C" EXPORT_API void awe_webview_cutclipboard(Awesomium::WebView* _Instance);
extern "C" EXPORT_API void awe_webview_undo(Awesomium::WebView* _Instance);
extern "C" EXPORT_API void awe_webview_redo(Awesomium::WebView* _Instance);
extern "C" EXPORT_API void awe_webview_selectall(Awesomium::WebView* _Instance);
extern "C" EXPORT_API void awe_webview_zoomin(Awesomium::WebView* _Instance);
extern "C" EXPORT_API void awe_webview_zoomout(Awesomium::WebView* _Instance);
extern "C" EXPORT_API void awe_webview_zoomreset(Awesomium::WebView* _Instance);
extern "C" EXPORT_API void awe_webview_requestpageinfo(Awesomium::WebView* _Instance);
//END OF Extra Methods
