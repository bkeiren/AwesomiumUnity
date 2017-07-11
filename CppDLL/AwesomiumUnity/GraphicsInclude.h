#pragma once

#if SUPPORT_D3D9
	#include <d3d9.h>
#endif
#if SUPPORT_D3D11
	#include <d3d11.h>
#endif
#if SUPPORT_OPENGL
	#if UNITY_WIN || UNITY_LINUX
		#include <GL/gl.h>
	#elif UNITY_OSX
		#include <OpenGL/OpenGL.h>
	#else
		#error Unknown platform
	#endif
#endif