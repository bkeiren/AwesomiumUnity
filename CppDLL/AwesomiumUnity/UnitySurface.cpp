#include "UnitySurface.h"

#include <Awesomium/BitmapSurface.h>

#include <algorithm>

namespace AwesomiumUnity
{

UnitySurface::UnitySurface( int _Width, int _Height )
{
	m_BitmapSurface = new Awesomium::BitmapSurface(_Width, _Height);
}

UnitySurface::UnitySurface()
{
	
}

UnitySurface::~UnitySurface()
{
	delete m_BitmapSurface;
	m_BitmapSurface = 0;
}

void UnitySurface::Paint( unsigned char* _SrcBuffer, int _SrcRowSpan, const Awesomium::Rect& _SrcRect, const Awesomium::Rect& _DestRect )
{
	m_BitmapSurface->Paint(_SrcBuffer, _SrcRowSpan, _SrcRect, _DestRect);

	if (m_LastChangedRect.IsEmpty())
	{
		m_LastChangedRect = _DestRect;
	}
	else
	{
		m_LastChangedRect.x = (std::min)(m_LastChangedRect.x, _DestRect.x);
		m_LastChangedRect.y = (std::min)(m_LastChangedRect.y, _DestRect.y);
		m_LastChangedRect.width = (std::max)(m_LastChangedRect.x + m_LastChangedRect.width, _DestRect.x + _DestRect.width) - m_LastChangedRect.x;
		m_LastChangedRect.height = (std::max)(m_LastChangedRect.y + m_LastChangedRect.height, _DestRect.y + _DestRect.height) - m_LastChangedRect.y;
	}
}

void UnitySurface::Scroll( int _DX, int _DY, const Awesomium::Rect& _ClipRect )
{
	m_BitmapSurface->Scroll(_DX, _DY, _ClipRect);
}



const unsigned char* UnitySurface::buffer() const 
{ 
	return m_BitmapSurface->buffer(); 
}

int UnitySurface::width() const 
{ 
	return m_BitmapSurface->width(); 
}

int UnitySurface::height() const 
{ 
	return m_BitmapSurface->height(); 
}

int UnitySurface::row_span() const 
{ 
	return m_BitmapSurface->row_span(); 
}

void UnitySurface::set_is_dirty(bool is_dirty)
{
	m_BitmapSurface->set_is_dirty(is_dirty);
}

bool UnitySurface::is_dirty() const
{
	return m_BitmapSurface->is_dirty();
}

void UnitySurface::CopyTo(	unsigned char* dest_buffer,
							int dest_row_span,
							int dest_depth,
							bool convert_to_rgba,
							bool flip_y)
{
	m_BitmapSurface->CopyTo(dest_buffer, dest_row_span, dest_depth, convert_to_rgba, flip_y);

	m_LastChangedRect.width = 0;
	m_LastChangedRect.height = 0;
}

bool UnitySurface::SaveToPNG(const Awesomium::WebString& file_path,
			   bool preserve_transparency ) const
{
	return m_BitmapSurface->SaveToPNG(file_path, preserve_transparency);
}

bool UnitySurface::SaveToJPEG(const Awesomium::WebString& file_path,
				int quality ) const
{
	return m_BitmapSurface->SaveToJPEG(file_path, quality);
}

unsigned char UnitySurface::GetAlphaAtPoint(int x, int y) const
{
	return m_BitmapSurface->GetAlphaAtPoint(x, y);
}

const Awesomium::Rect& UnitySurface::GetLastChanges() const
{
	return m_LastChangedRect;
}

}