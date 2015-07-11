#pragma once

#include <Awesomium/Surface.h>

namespace Awesomium
{
	class BitmapSurface;
	class WebString;
}

namespace AwesomiumUnity
{

class UnitySurface	: public Awesomium::Surface
{
public:
	UnitySurface( int _Width, int _Height );
	~UnitySurface();

	void Paint(	unsigned char* _SrcBuffer,
				int _SrcRowSpan,
				const Awesomium::Rect& _SrcRect,
				const Awesomium::Rect& _DestRect );

	void Scroll( int _DX, int _DY, const Awesomium::Rect& _ClipRect );



	const unsigned char* buffer() const;

	int width() const;

	int height() const;

	int row_span() const;

	void set_is_dirty(bool is_dirty);

	bool is_dirty() const;

	void CopyTo(unsigned char* dest_buffer,
		int dest_row_span,
		int dest_depth,
		bool convert_to_rgba,
		bool flip_y);

	bool SaveToPNG(const Awesomium::WebString& file_path,
		bool preserve_transparency = false) const;

	bool SaveToJPEG(const Awesomium::WebString& file_path,
		int quality = 90) const;

	unsigned char GetAlphaAtPoint(int x, int y) const;

	const Awesomium::Rect& GetLastChanges() const;
private:
	UnitySurface();

	Awesomium::BitmapSurface* m_BitmapSurface;

	// A rect encompassing all changed areas since the last time CopyTo was called.
	Awesomium::Rect m_LastChangedRect;
};

}