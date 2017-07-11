#include "WebView.h"

#include <Awesomium/WebCore.h>
#include <Awesomium/STLHelpers.h>

#include "GraphicsInclude.h"

#include "WebCore.h"
#include "UnitySurface.h"

using namespace Awesomium;

namespace AwesomiumUnity
{

void ExecuteCallbacksForJSValue(WebView* _Instance, Awesomium::JSValue _Value, int _ExecutionID)
{
	if (_Value.IsBoolean() && AwesomiumUnity::g_WebView_JavaScriptResultBoolCallback != nullptr)
	{
		AwesomiumUnity::g_WebView_JavaScriptResultBoolCallback(_Instance, _Value.ToBoolean(), _ExecutionID);
	}
	else if (_Value.IsInteger() && AwesomiumUnity::g_WebView_JavaScriptResultIntCallback != nullptr)
	{
		AwesomiumUnity::g_WebView_JavaScriptResultIntCallback(_Instance, _Value.ToInteger(), _ExecutionID);
	}
	else if (_Value.IsDouble() && AwesomiumUnity::g_WebView_JavaScriptResultFloatCallback != nullptr)
	{
		AwesomiumUnity::g_WebView_JavaScriptResultFloatCallback(_Instance, (float)_Value.ToDouble(), _ExecutionID);
	}
	else if (_Value.IsString() && AwesomiumUnity::g_WebView_JavaScriptResultStringCallback != nullptr)
	{
		AwesomiumUnity::g_WebView_JavaScriptResultStringCallback(_Instance, _Value.ToString().data(), _ExecutionID);
	}
	else if (_Value.IsNull() && AwesomiumUnity::g_WebView_JavaScriptResultNullCallback != nullptr)
	{
		AwesomiumUnity::g_WebView_JavaScriptResultNullCallback(_Instance, _ExecutionID);
	}
	else if (_Value.IsUndefined() && AwesomiumUnity::g_WebView_JavaScriptResultUndefinedCallback != nullptr)
	{
		AwesomiumUnity::g_WebView_JavaScriptResultUndefinedCallback(_Instance, _ExecutionID);
	}
	else if (_Value.IsArray() && AwesomiumUnity::g_WebView_JavaScriptResultArrayCallback != nullptr)
	{
		Awesomium::JSArray array = _Value.ToArray();
		AwesomiumUnity::g_WebView_JavaScriptResultArrayCallback(_Instance, array.size(), _ExecutionID);
		for (unsigned int i = 0; i < array.size(); ++i)
		{
			Awesomium::JSValue array_item = array.At(i);
			ExecuteCallbacksForJSValue(_Instance, array_item, _ExecutionID);
		}
		AwesomiumUnity::g_WebView_JavaScriptResultArrayCallback(_Instance, -1, _ExecutionID);	// Call the array callback a second time, this time passing -1 as the length to indicate that array iteration has finished.
																				// We do this because we can not know how many other callbacks we will get in between (JS arrays can contain multiple value types as well as other arrays).
	}
}

}

extern "C" EXPORT_API void awe_webview_loadurl( WebView* _Instance, char* _URL )	// From C#: [MarshalAs(UnmanagedType.LPStr)]
{
	if (_Instance && _URL)
	{
		WebURL webURL = WebURL(WSLit(_URL));
		_Instance->LoadURL(webURL);
	}
}

extern "C" EXPORT_API bool awe_webview_iscrashed( WebView* _Instance )
{
	if (_Instance)
		return _Instance->IsCrashed();

	return false;
}

extern "C" EXPORT_API bool awe_webview_isloading( WebView* _Instance )
{
	if (_Instance)
		return _Instance->IsLoading();

	return false;
}

extern "C" EXPORT_API bool awe_webview_istransparent( WebView* _Instance )
{
	if (_Instance)
		return _Instance->IsTransparent();

	return false;
}

extern "C" EXPORT_API void awe_webview_destroy( WebView* _Instance )
{
	if (_Instance)
	{
		_Instance->Destroy();

		// Delete any attached JSMethodHandler.
		//AwesomiumUnity::DeleteJSMethodHandlerForWebView(_Instance);	

		// Remove this WebView's Unity object from the list because we no longer need
		// to keep track of it and it doesn't exist anymore anyway.
		//AwesomiumUnity::RemoveJSUnityObjectForWebView(_Instance);
	}
}

#if SUPPORT_OPENGL
unsigned char *gWebBuffer = NULL;
int gWebViewWidth = 0;
int gWebViewHeight = 0;
GLuint gWebViewTexture;
#endif

extern "C" EXPORT_API void awe_webview_copybuffertotexture( WebView* _Instance, void* _TextureNativePtr, int _UnityTextureWidth, int _UnityTextureHeight )
{
	if (!_Instance || !_TextureNativePtr) return;

	AwesomiumUnity::UnitySurface* surface = (AwesomiumUnity::UnitySurface*)_Instance->surface();

	// Make sure our surface is not null -- it may be null if the WebView 
	// process has crashed.
	if (surface != 0) 
	{
		// Save our BitmapSurface to a JPEG image in the current
		// working directory.

		//surface->SaveToPNG(WSLit("./result.jpg"), true);

#if SUPPORT_D3D9
		// D3D9 case
		if (g_GraphicsDeviceType == kGfxRendererD3D9)
		{
			// Update native texture from code
			if (_TextureNativePtr)
			{
				IDirect3DTexture9* d3dtex = (IDirect3DTexture9*)_TextureNativePtr;
				D3DSURFACE_DESC desc;
				d3dtex->GetLevelDesc (0, &desc);
				D3DLOCKED_RECT lr;
				d3dtex->LockRect (0, &lr, nullptr, 0);

// 				int surfaceWidth = surface->width();
// 				int surfaceHeight = surface->height();
// 				int surfaceRowSpan = surface->row_span();
// 				int stride = desc.Width - surfaceWidth;

				surface->CopyTo((unsigned char*)lr.pBits, desc.Width * 4, 4, false, true);

// 				unsigned char* dst = (unsigned char*)lr.pBits;
// 				const unsigned char* src = surface->buffer();
// 				for (int y = 0; y < surfaceHeight; ++y)
// 				{
// 					for (int x = 0; x < surfaceWidth; ++x)
// 					{
// 						dst[0] = src[0];
// 						dst[1] = src[1];
// 						dst[2] = src[2];
// 						dst[3] = src[3];
// 						dst += 4;
// 						src += 4;
// 					}
// 					dst += stride;
// 				}

				d3dtex->UnlockRect (0);
			}
		}
#endif

#if SUPPORT_D3D11
		// D3D11 case
		if (g_GraphicsDeviceType == kGfxRendererD3D11)
		{
			ID3D11DeviceContext* ctx = nullptr;
			g_D3D11GraphicsDevice->GetImmediateContext (&ctx);

			// update native texture from code
			if (_TextureNativePtr)
			{
				ID3D11Texture2D* d3dtex = (ID3D11Texture2D*)_TextureNativePtr;
				D3D11_TEXTURE2D_DESC desc;
				d3dtex->GetDesc (&desc);

				unsigned char* data = new unsigned char[desc.Width*desc.Height*4];

				surface->CopyTo(data, desc.Width * 4, 4, true, true);
				// TODO: Use surface->GetLastChanges() to get a changed rectangle, then only update that
				// part of the texture.
				

				ctx->UpdateSubresource (d3dtex, 0, nullptr, data, desc.Width*4, 0);
				delete[] data;
			}

			ctx->Release();
		}
#endif


#if SUPPORT_OPENGL
		// OpenGL case
		if (g_GraphicsDeviceType == kGfxRendererOpenGL || g_GraphicsDeviceType == kGfxRendererOpenGLCore)
		{
			// Copy web surface to buffer here, blit buffer to texture in render callback
			// It needs to be separated this way to work with multithreaded rendering
			// TODO: Better concurrency protection
			gWebViewTexture = (GLuint)(size_t)(_TextureNativePtr);
			if (!gWebBuffer || gWebViewWidth != _UnityTextureWidth || gWebViewHeight != _UnityTextureHeight)
			{
				if (gWebBuffer)
					free(gWebBuffer);
				gWebBuffer = (unsigned char*)malloc(_UnityTextureWidth * _UnityTextureHeight * 4);
				gWebViewWidth = _UnityTextureWidth;
				gWebViewHeight = _UnityTextureHeight;
			}
			surface->CopyTo(gWebBuffer, _UnityTextureWidth * 4, 4, true, true);
		}
#endif
	}
}

extern "C" EXPORT_API int awe_webview_surface_width( WebView* _Instance )
{
	if (_Instance)
	{
		AwesomiumUnity::UnitySurface* surface = (AwesomiumUnity::UnitySurface*)_Instance->surface();
		if (surface)
			return surface->width();
	}
	
	return -1;
}

extern "C" EXPORT_API int awe_webview_surface_height( WebView* _Instance )
{	
	if (_Instance)
	{
		AwesomiumUnity::UnitySurface* surface = (AwesomiumUnity::UnitySurface*)_Instance->surface();
		if (surface)
			return surface->height();
	}

	return -1;
}

extern "C" EXPORT_API bool awe_webview_surface_isdirty( WebView* _Instance )
{	
	if (_Instance)
	{
		AwesomiumUnity::UnitySurface* surface = (AwesomiumUnity::UnitySurface*)_Instance->surface();
		if (surface)
			return surface->is_dirty();
	}

	return false;
}

extern "C" EXPORT_API void awe_webview_reload( WebView* _Instance, bool _IgnoreCache )
{
	if (_Instance)
		_Instance->Reload(_IgnoreCache);
}

extern "C" EXPORT_API void awe_webview_inject_mousemove( WebView* _Instance, int _X, int _Y )
{
	if (_Instance)
		_Instance->InjectMouseMove(_X, _Y);
}

extern "C" EXPORT_API void awe_webview_inject_mousedown( WebView* _Instance, int _Button )
{
	if (_Instance)
		_Instance->InjectMouseDown((MouseButton)_Button);
}

extern "C" EXPORT_API void awe_webview_inject_mouseup( WebView* _Instance, int _Button )
{
	if (_Instance)
		_Instance->InjectMouseUp((MouseButton)_Button);
}

extern "C" EXPORT_API void awe_webview_inject_mousewheel( WebView* _Instance, int _ScrollVert, int _ScrollHorz )
{
	if (_Instance)
		_Instance->InjectMouseWheel(_ScrollVert, _ScrollHorz);
}

extern "C" EXPORT_API void awe_webview_inject_keyboardevent( WebView* _Instance, WebKeyboardEvent& _WebKeyBoardEvent )
{
	if (_Instance)
		_Instance->InjectKeyboardEvent(_WebKeyBoardEvent);
}

extern "C" EXPORT_API void awe_webview_inject_touchevent( WebView* _Instance, WebTouchEvent& _WebTouchEvent )
{
	if (_Instance)
		_Instance->InjectTouchEvent(_WebTouchEvent);
}

extern "C" EXPORT_API void awe_webview_resize( WebView* _Instance, int _Width, int _Height )
{
	if (_Instance)
		_Instance->Resize(_Width, _Height);
}

extern "C" EXPORT_API void awe_webview_executejavascript( WebView* _Instance, char* _Script, int _ExecutionID )
{
	if (_Instance == nullptr)
		return;

	_Instance->ExecuteJavascript(WSLit(_Script), WebString());

	if (AwesomiumUnity::g_WebView_JavaScriptExecFinishedCallback != nullptr)
		AwesomiumUnity::g_WebView_JavaScriptExecFinishedCallback(_Instance, _ExecutionID);
}


extern "C" EXPORT_API void awe_webview_executejavascriptwithresult( WebView* _Instance, char* _Script, int _ExecutionID )
{
	if (_Instance == nullptr)
		return;

	Awesomium::JSValue result = _Instance->ExecuteJavascriptWithResult(WSLit(_Script), WebString());

	AwesomiumUnity::ExecuteCallbacksForJSValue(_Instance, result, _ExecutionID);

	if (AwesomiumUnity::g_WebView_JavaScriptExecFinishedCallback != nullptr)
		AwesomiumUnity::g_WebView_JavaScriptExecFinishedCallback(_Instance, _ExecutionID);
}


extern "C" EXPORT_API void awe_webview_focus( WebView* _Instance )
{
	if (_Instance)
		_Instance->Focus();
}

extern "C" EXPORT_API void awe_webview_unfocus( WebView* _Instance )
{
	if (_Instance)
		_Instance->Unfocus();
}

extern "C" EXPORT_API void awe_webview_stop( WebView* _Instance )
{
	if (_Instance)
		_Instance->Stop();
}

extern "C" EXPORT_API void awe_webview_settransparent( WebView* _Instance, bool _Transparent )
{
	if (_Instance)
		_Instance->SetTransparent(_Transparent);
}

extern "C" EXPORT_API Awesomium::Error awe_webview_lasterror( Awesomium::WebView* _Instance )
{
	if (_Instance)
		return _Instance->last_error();
	
	return Awesomium::kError_None;
}

extern "C" EXPORT_API bool awe_webview_cangoback( WebView* _Instance )
{
	if (_Instance)
		return _Instance->CanGoBack();

	return false;
}

extern "C" EXPORT_API bool awe_webview_cangoforward( WebView* _Instance )
{
	if (_Instance)
		return _Instance->CanGoForward();

	return false;
}

extern "C" EXPORT_API void awe_webview_goback( WebView* _Instance )
{
	if (_Instance)
		_Instance->GoBack();
}

extern "C" EXPORT_API void awe_webview_goforward( WebView* _Instance )
{
	if (_Instance)
		_Instance->GoForward();
}

extern "C" EXPORT_API void awe_webview_gotohistoryoffset( WebView* _Instance, int _Offset )
{
	if (_Instance)
		_Instance->GoToHistoryOffset(_Offset);
}

extern "C" EXPORT_API void awe_webview_js_setmethod( WebView* _Instance, char* _MethodName, bool _HasReturnValue )
{
	if (_Instance)
	{
		Awesomium::JSObject* jsObject = AwesomiumUnity::FindJSUnityObjectForWebView(_Instance);
		if (jsObject != nullptr)
		{
			jsObject->SetCustomMethod(WSLit(_MethodName), _HasReturnValue);
		}
	}
}