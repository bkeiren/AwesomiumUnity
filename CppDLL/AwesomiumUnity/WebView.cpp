#include "WebView.h"

#include <Awesomium/WebCore.h>
#include <Awesomium/STLHelpers.h>

#include "GraphicsInclude.h"

#include "WebCore.h"
#include "UnitySurface.h"

using namespace Awesomium;

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

extern "C" EXPORT_API void awe_webview_copybuffertotexture( WebView* _Instance, void* _TextureNativePtr, int _UnityTextureWidth, int _UnityTextureHeight )
{
	if (!_Instance || !_TextureNativePtr) return;

	AwesomiumUnity::UnitySurface* surface = (AwesomiumUnity::UnitySurface*)_Instance->surface();

	// Make sure our surface is not NULL-- it may be NULL if the WebView 
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
				d3dtex->LockRect (0, &lr, NULL, 0);

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
			ID3D11DeviceContext* ctx = NULL;
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
				

				ctx->UpdateSubresource (d3dtex, 0, NULL, data, desc.Width*4, 0);
				delete[] data;
			}

			ctx->Release();
		}
#endif


#if SUPPORT_OPENGL
		// OpenGL case
		if (g_GraphicsDeviceType == kGfxRendererOpenGL)
		{
			// update native texture from code
			if (_TextureNativePtr)
			{
				GLuint gltex = (GLuint)(size_t)(_TextureNativePtr);
				glBindTexture (GL_TEXTURE_2D, gltex);
				int texWidth, texHeight;
				glGetTexLevelParameteriv (GL_TEXTURE_2D, 0, GL_TEXTURE_WIDTH, &texWidth);
				glGetTexLevelParameteriv (GL_TEXTURE_2D, 0, GL_TEXTURE_HEIGHT, &texHeight);

				unsigned char* data = new unsigned char[texWidth*texHeight*4];

				surface->CopyTo(data, texWidth * 4, 4, true, true);

				glTexSubImage2D (GL_TEXTURE_2D, 0, 0, 0, texWidth, texHeight, GL_RGBA, GL_UNSIGNED_BYTE, data);
				delete[] data;
			}
		}
#endif
	}
}

extern "C" EXPORT_API int awe_webview_surface_width( WebView* _Instance )
{
	if (_Instance)
		return ((AwesomiumUnity::UnitySurface*)_Instance->surface())->width();
	
	return -1;
}

extern "C" EXPORT_API int awe_webview_surface_height( WebView* _Instance )
{	
	if (_Instance)
		return ((AwesomiumUnity::UnitySurface*)_Instance->surface())->height();

	return -1;
}

extern "C" EXPORT_API bool awe_webview_surface_isdirty( WebView* _Instance )
{	
	if (_Instance)
		return ((AwesomiumUnity::UnitySurface*)_Instance->surface())->is_dirty();

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

extern "C" EXPORT_API void awe_webview_executejavascript( WebView* _Instance, char* _Script )
{
	if (_Instance)
	{
		_Instance->ExecuteJavascript(WSLit(_Script), WebString());
	}
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
		JSObject* jsObject = AwesomiumUnity::FindJSUnityObjectForWebView(_Instance);
		if (jsObject != 0)
		{
			jsObject->SetCustomMethod(WSLit(_MethodName), _HasReturnValue);
		}
	}
}