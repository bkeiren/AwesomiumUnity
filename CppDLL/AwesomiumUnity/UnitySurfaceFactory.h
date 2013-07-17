#pragma once

#include <Awesomium/Surface.h>

namespace AwesomiumUnity
{

class UnitySurfaceFactory	: public Awesomium::SurfaceFactory
{
public:
	UnitySurfaceFactory();
	~UnitySurfaceFactory();

	Awesomium::Surface* CreateSurface( Awesomium::WebView* _View, int _Width, int _Height );

	void DestroySurface( Awesomium::Surface* _Surface );
};

}