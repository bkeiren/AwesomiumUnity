AwesomiumUnity
================================

AwesomiumUnity is a third-party Awesomium wrapper intended for use with Unity3D.

The wrapper consist of a custom native code C++ DLL which simply wraps Awesomium's C++ API (requires Unity Pro to use) and a set of C# scripts that interface with this DLL.

Folders
--------------------------------

### CppDLL

This folder contains a MSVC solution for the C++ DLL that wraps the Awesomium C++ API.
To compile this solution you need to:

* **Have the Awesomium SDK installed.** If you install it somewhere other than:
 _C:\Program Files (x86)\Awesomium Technologies LLC\Awesomium SDK\1.7.1.0_, 
 you will have to change the project's include directory (currently set to:
 _C:\Program Files (x86)\Awesomium Technologies LLC\Awesomium SDK\1.7.1.0\include_) 
 and the library directory (currently set to:
 _C:\Program Files (x86)\Awesomium Technologies LLC\Awesomium SDK\1.7.1.0\build\lib_).
* **Have OpenGL installed** (The project links with _'opengl32.lib'_).
* **Have the DirectX SDK installed** and **"DXSDK_DIR" as an environment variable** (the project's include directories currently have _$(DXSDK_DIR)\include_ set).

If succesfully built, the output will be located at either:

* _CppDLL\Debug\AwesomiumUnity.dll_
or
* _CppDLL\Release\AwesomiumUnity.dll_



External Links
--------------------------------

* Awesomium website: http://awesomium.com/
* Unity3D website: http://unity3d.com/