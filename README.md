AwesomiumUnity
================================

AwesomiumUnity is a third-party Awesomium wrapper intended for use with Unity3D.

*AwesomiumUnity allows you to display and use HTML documents in your Unity3D application. HTML5, CSS, and JavaScript are supported, and rendered views can be displayed anywhere a regular Unity Texture2D can be used. Possibilities for your application include:*

- Fully interactive live browsing of **the** internet, in-game.
- Rich, complex **2D or 3D** UI's created using HTML, CSS, and JavaScript.
- Quick iteration times and result. Just edit your HTML, CSS, or JavaScript and reload!
- **Fully functional** in the **Unity Editor**\*. Don't restrict yourself to just a standalone game!
_\* 64-bit editor not be supported due to Awesomium itself being 32-bit only_

The wrapper consist of a custom native code C++ DLL which simply wraps Awesomium's C++ API (requires Unity Pro to use) and a set of C# scripts that interface with this DLL.

**The current state of the project is:** 

**_Stable, the majority of important features implemented. Other functionality in development, code structure and architecture not completely final._**

Projects using AwesomiumUnity
--------------------------------

Besides individual users, AwesomiumUnity is also used by some game developers or other companies to integrate the power of HTML5 and web browsing in their application. 

The developers of **[TableTop Simulator](http://berserk-games.com/tabletop-simulator/)** by **[Berserk Games](http://berserk-games.com/)** are using AwesomiumUnity in their game for an [in-game tablet in the 3D game world](http://steamcommunity.com/games/TabletopSimulator/announcements/detail/154583549672473635), which users can interact with and use to browse the web.


Folders
--------------------------------

### CppDLL

This folder contains project files for Microsoft Visual Studio and Apple XCode for the C++ DLL that wraps the Awesomium C++ API.
To compile this solution you need to:

* **Have the Awesomium SDK installed.** 
* **Windows:** If you install it somewhere other than _C:\Program Files (x86)\Awesomium Technologies LLC\Awesomium SDK\1.7.5.0_, you will have to change the project's include directory (currently set to _C:\Program Files (x86)\Awesomium Technologies LLC\Awesomium SDK\1.7.5.0\include_) and the library directory (currently set to _C:\Program Files (x86)\Awesomium Technologies LLC\Awesomium SDK\1.7.5.0\build\lib_).
* **OS X:** If you install it somewhere other than _/Library/Frameworks/_, you will need to change the include path in XCode for the  *Awesomium.framework* library.
* **Have OpenGL installed** 
* **Windows:** Needed because the project links with _'opengl32.lib'_.
* **OS X**: OpenGL should already come installed with OS X by default.
* **_[Windows Only]_ Have the DirectX SDK installed** and **"DXSDK_DIR" as an environment variable** (the project's include directories currently have _$(DXSDK_DIR)\include_ set).

If successfully built using either Microsoft Visual Studio or XCode, the output will be located at either:

* **Windows:**
* _CppDLL\Win\bin\Debug\AwesomiumUnity.dll_
* _CppDLL\Win\bin\Release\AwesomiumUnity.dll_

* **OS X:**
* _CppDLL\Mac\AwesomiumUnity\Build\Products\Debug\AwesomiumUnity.bundle_
* _CppDLL\Mac\AwesomiumUnity\Build\Products\Release\AwesomiumUnity.bundle_


### AwesomiumUnityScripts

This folder contains the Unity scripts that interact with the native code (C++) DLL and the scripts that are actually attached to components in order to get webpages to render.

The files serve the following purposes:
* _AwesomiumUnityScripts\AwesomiumUnityError.cs_ : 
* Provides a binary-compatible structure to represent Awesomium errors.
* _AwesomiumUnityScripts\AwesomiumUnityVirtualKey.cs_ : 
* Provides a binary-compatible structure to represent Awesomium virtual keys.
* _AwesomiumUnityScripts\AwesomiumUnityWebCore.cs_ : 
* Provides a class with static functions that interface with the C++ DLL. This class is used for talking to the _Awesomium::WebCore_ C++ class. Initialization and shutdown of the WebCore is handled automatically when needed.
* _AwesomiumUnityScripts\AwesomiumUnityWebCoreHelper.cs_ : 
* Contains a class that derives from MonoBehaviour which is created automatically and updates the WebCore each frame.
* _AwesomiumUnityScripts\AwesomiumUnityWebKeyboardEvent_ : 
* Provides a binary-compatible structure to represent Awesomium's _WebKeyboardEvent_s.
* _AwesomiumUnityScripts\AwesomiumUnityWebKeyModifiers_ : 
* Provides a binary-compatible structure to represent Awesomium's _WebKeyModifiers_.
* _AwesomiumUnityScripts\AwesomiumUnityWebKeyType_ : 
* Provides a binary-compatible structure to represent Awesomium's _WebKeyType_.
* _AwesomiumUnityScripts\AwesomiumUnityWebView_ : 
* Provides a class that interfaces with the C++ DLL. This class is used for talking to instances of the _Awesomium::WebView_ C++ class.
* _AwesomiumUnityScripts\AwesomiumUnityWebTexture_ : 
* Contains the main class you need to use to render webpages. The class derives from MonoBehaviour and should be attached to a GameObject. A single instance of this class represent a single webpage. Input, interactiveness, etc. are all handled by this class. Note that this class acts as a wrapper to a WebView and can serve as an example of how you could implement such logic yourself. The WebTexture utilizes the WebView's API to control the view.
* _AwesomiumUnityScripts\AwesomiumUnityWebSession.cs_ :
* Provides an interface and data to customize certain settings that influence the session. Settings include things as switching on or off web audio, WebGL, HTML5 local storage, JavaScript, plugins (such as Flash), but also whether the session cache should be stored in memory or on disk, and in what exact location.

#### AwesomiumUnityScripts\Examples

This folder contains simple examples to get you going. In most cases you can simply add one of these example scripts to a GameObject that has been set up to render webpages to see it working. 

If an example is accompanied by a folder with the same name as the example script, it means that there is some HTML/CSS/JavaScript code that goes with it. These are generally loaded directly off of GitHub from within the example script.

Using AwesomiumUnity
--------------------------------

### Getting it to work in the Unity Editor

**_Note_: Due to limitations of Awesomium you can only use AwesomiumUnity in a 32-bit Unity Editor! **

1. Do one of the following two options (A or B):
* A) Copy the pre-built binaries to your Unity's project's _Assets/Plugins_ folder. The binaries can be found in:
* **Windows (AwesomiumUnity.dll):** 
* _CppDLL\Win\bin\Debug_, or
* _CppDLL\Win\bin\Release_
* **OS X (AwesomiumUnity.bundle):**
* _CppDLL\Mac\AwesomiumUnity\Build\Products\Debug_, or
* _CppDLL\Mac\AwesomiumUnity\Build\Products\Release_
OR
* B) 
* **Windows:** Build the MSVC project and copy the resulting .DLL file (_AwesomiumUnity.dll_) to your Unity project's _Assets\Plugins_ folder.
* **OS X:** Build the XCode project and copy the resulting .bundle file (_AwesomiumUnity.bundle_) to your Unity project's _Assets\Plugins_ folder.
2. **[Windows Only]** Copy the following files from your Awesomium SDK installation directory's _build\bin_ folder to your Unity installation directory's _Editor_ folder (**NOT** your project _Assets\Editor_ folder!).
* _awesomium_process.exe_
* _awesomium.dll_
* _icudt.dll_
* _libEGL.dll_
* _libGLESv2.dll_
* _xinput9_1_0.dll_
* _avcodec-53.dll_
* _avformat-53.dll_
* _avutil-51.dll_
3. Copy the folder _AwesomiumUnityScripts_ to your Unity project's _Assets_ folder (or any subdirectory within it).
4. Open your Unity project
5. Create a GameObject
6. Add either a _GUITexture_ component or a _Renderer_ component with a material and a _MeshCollider_ component to your GameObject
7. Add the _AwesomiumUnityWebTexture_ component to your GameObject
8. Hit play	

### Getting it to work for a standalone application

**_Note:_ Due to limitations of Awesomium you can only use AwesomiumUnity in a 32-bit application!**

1. Do one of the following two options (A or B):
* A) Copy the pre-built binaries to the same folder as your executable. The binaries can be found in:
* **Windows (AwesomiumUnity.dll):** 
* _CppDLL\Win\bin\Debug_, or
* _CppDLL\Win\bin\Release_
* **OS X (AwesomiumUnity.bundle):**
* _CppDLL\Mac\AwesomiumUnity\Build\Products\Debug_, or
* _CppDLL\Mac\AwesomiumUnity\Build\Products\Release_
OR
* B) 
* **Windows:** Build the MSVC project and copy the resulting .DLL file (_AwesomiumUnity.dll_) to the same folder as your executable.
* **OS X:** Build the XCode project and copy the resulting .bundle file (_AwesomiumUnity.bundle_) to the same folder as your executable.
2. **[Windows Only]** Copy the following files from your Awesomium SDK installation directory's _build\bin_ folder to the same folder as your executable.
* _awesomium_process.exe_
* _awesomium.dll_
* _icudt.dll_
* _libEGL.dll_
* _libGLESv2.dll_
* _xinput9_1_0.dll_
* _avcodec-53.dll_
* _avformat-53.dll_
* _avutil-51.dll_
3. Copy the folder _AwesomiumUnityScripts_ to your Unity project's _Assets_ folder (or any subdirectory within it).
4. Open your Unity project
5. Create a GameObject
6. Add either a _GUITexture_ component or a _Renderer_ component with a material and a _MeshCollider_ component to your GameObject
7. Add the _AwesomiumUnityWebTexture_ component to your GameObject
8. Build an executable (NOTE: You'll probably have to have _AwesomiumUnity.dll_ or _AwesomiumUnity.bundle_ in your _Assets\Plugins_ folder as well before the build process will succeed).

External Links
--------------------------------

* Awesomium website: http://awesomium.com/
* Unity3D website: http://unity3d.com/
