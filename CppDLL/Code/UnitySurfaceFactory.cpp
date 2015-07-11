#include "UnitySurfaceFactory.h"
#include "UnitySurface.h"

namespace AwesomiumUnity
{

UnitySurfaceFactory::UnitySurfaceFactory()
{
	
}

UnitySurfaceFactory::~UnitySurfaceFactory()
{
	
}

Awesomium::Surface* UnitySurfaceFactory::CreateSurface( Awesomium::WebView* _View, int _Width, int _Height )
{
	return new UnitySurface(_Width, _Height);
}

void UnitySurfaceFactory::DestroySurface( Awesomium::Surface* _Surface )
{
	delete _Surface;
}

}