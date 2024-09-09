/***************************************************************************
 *  This is pretty much a complete rewrite of the DDS File Type Plugin     *
 *  for Paint.Net distributed as part of sims3tools.                       *
 *                                                                         *
 *  It has also been reviewed against the GIMP DDS plugin source (as a     *
 *  sanity check for my rewrite).                                          *
 *                                                                         *
 *  Hence, all the bugs are mine.                                          *
 *                                                                         *
 *  Copyright (C) 2013 by Peter L Jones                                    *
 *  pljones@users.sf.net                                                   *
 ***************************************************************************/
//------------------------------------------------------------------------------
/*
	@brief		DDS File Type Plugin for Paint.NET

	@note		Copyright (c) 2006 Dean Ashton         http://www.dmashton.co.uk

	Permission is hereby granted, free of charge, to any person obtaining
	a copy of this software and associated documentation files (the 
	"Software"), to	deal in the Software without restriction, including
	without limitation the rights to use, copy, modify, merge, publish,
	distribute, sublicense, and/or sell copies of the Software, and to 
	permit persons to whom the Software is furnished to do so, subject to 
	the following conditions:

	The above copyright notice and this permission notice shall be included
	in all copies or substantial portions of the Software.

	THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS
	OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF 
	MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT.
	IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY 
	CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, 
	TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE 
	SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
**/
/*
        DDS GIMP plugin

        Copyright (C) 2004-2012 Shawn Kirst <skirst@gmail.com>,
   with parts (C) 2003 Arne Reuter <homepage@arnereuter.de> where specified.

        This program is free software; you can redistribute it and/or
        modify it under the terms of the GNU General Public
        License as published by the Free Software Foundation; either
        version 2 of the License, or (at your option) any later version.

        This program is distributed in the hope that it will be useful,
        but WITHOUT ANY WARRANTY; without even the implied warranty of
        MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the GNU
        General Public License for more details.

        You should have received a copy of the GNU General Public License
        along with this program; see the file COPYING.  If not, write to
        the Free Software Foundation, 51 Franklin Street, Fifth Floor
        Boston, MA 02110-1301, USA.
*/
/*
** !!! COPYRIGHT NOTICE !!!
**
** The following is based on code (C) 2003 Arne Reuter <homepage@arnereuter.de>
** URL: http://www.dr-reuter.de/arne/dds.html
**
*/
//------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;

namespace System.Drawing
{
    class Dds
    {
        #region Sub-classes and Enums
        class DdsHeader
        {
            #region Sub-classes and Enums
            class DdsPixelFormat
            {
                [Flags]
                public enum PFFlags : uint
                {
                    alphaPixels = 0x01,
                    alpha = 0x02,
                    fourCC = 0x04,
                    // = 0x08,
                    // = 0x10,
                    paletteIndexed8 = 0x20,
                    rgb = 0x40,
                    // = 0x80,
                    // ...
                    yuv = 0x0200,
                    // ...
                    luminence = 0x00020000,
                    // ...
                    nVidiaNormal = 0x8000000,
                }

                // Either fourCC is set or one - and only one - of the following:
                const PFFlags notFourCC = PFFlags.alpha | PFFlags.paletteIndexed8 | PFFlags.rgb | PFFlags.yuv | PFFlags.luminence | PFFlags.nVidiaNormal;

                public enum PFFourCC : uint
                {
                    dxt1 = 0x31545844,
                    dxt3 = 0x33545844,
                    dxt5 = 0x35545844,
                }

                enum PFFourCCBlockSize8 : uint
                {
                    dxt1 = 0x31545844,
                    // bc1 and bc4 not supported
                }

                uint size;//76
                PFFlags flags;//80
                PFFourCC fourCC;//84
                uint rgbBitCount;//88
                uint rBitMask;//92
                uint gBitMask;//96
                uint bBitMask;//100
                uint aBitMask;//104

                public DdsPixelFormat(DdsFormat ddsFormat)
                {
                    if (!Enum.IsDefined(typeof(DdsFormat), ddsFormat))
                        throw new ArgumentException(String.Format("Requested DDS format 0x{0:8X} is unsupported", (uint)ddsFormat));

                    size = 32;
                    flags = 0;
                    fourCC = 0;
                    rgbBitCount = rBitMask = gBitMask = bBitMask = gBitMask = 0;
                    switch (ddsFormat)
                    {
                        case DdsFormat.DXT1:
                            flags |= PFFlags.fourCC;
                            fourCC = PFFourCC.dxt1;
                            break;
                        case DdsFormat.DXT3:
                            flags |= PFFlags.fourCC;
                            fourCC = PFFourCC.dxt3;
                            break;
                        case DdsFormat.DXT5:
                            flags |= PFFlags.fourCC;
                            fourCC = PFFourCC.dxt5;
                            break;
                        case DdsFormat.A8L8:
                            flags |= PFFlags.luminence | PFFlags.alphaPixels;
                            rgbBitCount = 16;
                            aBitMask = 0x0000ff00;
                            rBitMask = 0x000000ff;
                            break;
                        case DdsFormat.A8R8G8B8:
                            flags |= PFFlags.rgb | PFFlags.alphaPixels;
                            rgbBitCount = 32;
                            aBitMask = 0xff000000;
                            rBitMask = 0x00ff0000;
                            gBitMask = 0x0000ff00;
                            bBitMask = 0x000000ff;
                            break;
                        case DdsFormat.X8R8G8B8:
                            flags |= PFFlags.rgb;
                            rgbBitCount = 32;
                            rBitMask = 0x00ff0000;
                            gBitMask = 0x0000ff00;
                            bBitMask = 0x000000ff;
                            break;
                        case DdsFormat.A8B8G8R8:
                            flags |= PFFlags.rgb | PFFlags.alphaPixels;
                            rgbBitCount = 32;
                            aBitMask = 0xff000000;
                            rBitMask = 0x000000ff;
                            gBitMask = 0x0000ff00;
                            bBitMask = 0x00ff0000;
                            break;
                        case DdsFormat.X8B8G8R8:
                            flags |= PFFlags.rgb;
                            rgbBitCount = 32;
                            rBitMask = 0x000000ff;
                            gBitMask = 0x0000ff00;
                            bBitMask = 0x00ff0000;
                            break;
                        case DdsFormat.R8G8B8:
                            flags |= PFFlags.rgb;
                            rgbBitCount = 24;
                            rBitMask = 0x00ff0000;
                            gBitMask = 0x0000ff00;
                            bBitMask = 0x000000ff;
                            break;
                        case DdsFormat.A4R4G4B4:
                            flags |= PFFlags.rgb | PFFlags.alphaPixels;
                            rgbBitCount = 16;
                            aBitMask = 0x0000f000;
                            rBitMask = 0x00000f00;
                            gBitMask = 0x000000f0;
                            bBitMask = 0x0000000f;
                            break;
                        case DdsFormat.A1R5G5B5:
                            flags |= PFFlags.rgb | PFFlags.alphaPixels;
                            rgbBitCount = 16;
                            aBitMask = 0x00008000;
                            rBitMask = 0x00007c00;
                            gBitMask = 0x000003e0;
                            bBitMask = 0x0000001f;
                            break;
                        case DdsFormat.R5G6B5:
                            flags |= PFFlags.rgb;
                            rgbBitCount = 16;
                            rBitMask = 0x0000f800;
                            gBitMask = 0x000007e0;
                            bBitMask = 0x0000001f;
                            break;
                        default:
                            throw new ArgumentOutOfRangeException("Unsupported DDS format.");
                    }
                }

                public DdsPixelFormat(Stream s)
                {
                    BinaryReader r = new BinaryReader(s);
                    size = r.ReadUInt32();
                    if (size != 32)
                    {
                        Console.WriteLine(String.Format(
                            "DDS Pixel Format Size invalid.  Got {0}, expected 32.  At 0x{1:X8}.  Correcting..."
                            , size
                            , s.Position
                            ));
                        size = 32;
                    }

                    flags = (PFFlags)r.ReadUInt32();

                    fourCC = (PFFourCC)r.ReadUInt32();
                    rgbBitCount = r.ReadUInt32();
                    rBitMask = r.ReadUInt32();
                    gBitMask = r.ReadUInt32();
                    bBitMask = r.ReadUInt32();
                    aBitMask = r.ReadUInt32();

                    if ((flags & PFFlags.fourCC) != 0)
                    {
                        if (!Enum.IsDefined(typeof(PFFourCC), fourCC))
                            throw new NotSupportedException(String.Format(
                                "Unsupported DDS Pixel Format Four CC.  Got 0x{0:8}, only DXT1/3/5 supported."
                                , (uint)fourCC
                                )
                            );
                        flags &= ~notFourCC;
                        rgbBitCount = 0;
                    }
                    else if (Enum.IsDefined(typeof(PFFourCC), fourCC))
                    {
                        Console.WriteLine("Supported FourCC but Flags.FourCC not set.  Overruling flags.");
                        flags |= PFFlags.fourCC;
                        flags &= ~notFourCC;
                        rgbBitCount = 0;
                    }
                    else
                        fourCC = 0;

                    if (fourCC == 0)
                    {
                        // Fix some of my previous breakage...
                        if (((uint)(flags & (PFFlags.alphaPixels | PFFlags.alpha | PFFlags.rgb))).Bits() == 3)
                        {
                            Console.WriteLine("Alpha-only indicated but AlphaPixels and RGB also set.  Cleaing Alpha-only flag.");
                            flags &= ~PFFlags.alpha;
                        }
                        if (((uint)(flags & (PFFlags.alpha | PFFlags.rgb))).Bits() == 2)
                        {
                            Console.WriteLine("Alpha-only and RGB both set.  Cleaing Alpha-only flag and ensuring alpha-pixels set.");
                            flags &= ~PFFlags.alpha;
                            flags |= PFFlags.alphaPixels;
                        }

                        if (((uint)(flags & notFourCC)).Bits() > 1)
                            throw new NotSupportedException(String.Format(
                                "Unsupported DDS Pixel Format Flags.  Got {0}, only one of _alpha, _paletteIndexed8, _rgb, _yuv, _luminence or _nVidiaNormal should be set."
                                , flags
                                )
                            );

                        if ((flags & PFFlags.alpha) != 0)
                            throw new NotSupportedException("Alpha-only format data is not supported.");

                        if ((flags & PFFlags.paletteIndexed8) != 0)
                            throw new NotSupportedException("Palette indexed format data is not supported.");

                        if ((flags & PFFlags.yuv) != 0)
                            throw new NotSupportedException("YUV format data is not supported.");

                        if ((flags & PFFlags.nVidiaNormal) != 0)
                            throw new NotSupportedException("nVidia normal format data is not supported.");

                        if ((flags & PFFlags.luminence) != 0
                            && (
                                aBitMask != 0x0000ff00 || rBitMask != 0x000000ff
                                || gBitMask != 0 || bBitMask != 0
                            ))
                            throw new NotSupportedException("Only A8L8 luminence data is supported.");

                        if (((uint)(flags & notFourCC)).Bits() == 0)
                        {
                            Console.WriteLine("Uncompressed texture but no format indicated.  Defaulting to RGB.");
                            flags |= PFFlags.rgb;
                        }

                        if ((flags & PFFlags.alphaPixels) != 0
                            && aBitMask == 0)
                        {
                            Console.WriteLine("AlphaPixels indicated but no alpha mask.  Cleaing AlphaPixels flag.");
                            flags &= ~PFFlags.alphaPixels;
                        }

                        if ((flags & PFFlags.rgb) != 0
                            && rgbBitCount == 0)
                        {
                            Console.WriteLine("RGB Bit Count needed but not set - calculating.");
                            rgbBitCount = (uint)(aBitMask.Bits() + rBitMask.Bits() + gBitMask.Bits() + bBitMask.Bits());
                        }
                        if ((flags & PFFlags.rgb) != 0
                            && rgbBitCount == 0)
                            throw new InvalidDataException("Invalid uncompressed header.  RGB Bit Count is zero and no masks set.");

                        if ((flags & PFFlags.rgb) != 0
                            && (rBitMask == 0 || gBitMask == 0 || bBitMask == 0))
                            throw new NotSupportedException(String.Format(
                                "RGB is supported only where data is present for all channels.  RGB mask is {0:X6}."
                                , rBitMask | gBitMask | bBitMask
                                ));
                    }
                }

                public void Write(Stream s)
                {
                    BinaryWriter w = new BinaryWriter(s);
                    w.Write((uint)32);
                    w.Write((uint)Flags);
                    w.Write((uint)FourCC);
                    w.Write(RGBBitCount);
                    if ((Flags & PFFlags.fourCC) != 0)
                    {
                        w.Write(0);
                        w.Write(0);
                        w.Write(0);
                        w.Write(0);
                    }
                    else
                    {
                        w.Write(rBitMask);
                        w.Write(gBitMask);
                        w.Write(bBitMask);
                        w.Write(aBitMask);
                    }
                }

                public PFFlags Flags
                {
                    get
                    {
                        if (Enum.IsDefined(typeof(PFFourCC), fourCC))
                        {
                            return PFFlags.fourCC; // Block compressed
                        }
                        else
                        {
                            PFFlags flags = 0;

                            if (aBitMask != 0)
                            {
                                flags |= PFFlags.alphaPixels; // Alpha pixels exist

                                // No Alpha only support

                                if (aBitMask == 0x0000ff00 && rBitMask == 0x000000ff && gBitMask == 0 && bBitMask == 0)
                                {
                                    return flags | PFFlags.luminence;
                                }
                            }

                            // No YUV support

                            return flags | PFFlags.rgb;
                        }
                    }
                }

                public PFFourCC FourCC { get { return (Flags & PFFlags.fourCC) != 0 ? fourCC : 0; } }

                public uint RGBBitCount { get { return (Flags & notFourCC) != 0 ? rgbBitCount : 0; } }
                
                public uint BlockSize
                {
                    get
                    {
                        if (!Enum.IsDefined(typeof(PFFourCC), fourCC))
                            throw new InvalidOperationException("BlockSize not supported for non-block compressed format DDS files.");

                        return (uint)(Enum.IsDefined(typeof(PFFourCCBlockSize8), (uint)fourCC) ? 8 : 16);
                    }
                }

                public DdsFormat FileFormat
                {
                    get
                    {
                        // OK, we make the presumption nothing has gone wrong.

                        // Is it DXTn?
                        if ((flags & PFFlags.fourCC) != 0)
                            switch (fourCC)
                            {
                                case PFFourCC.dxt1: return DdsFormat.DXT1;
                                case PFFourCC.dxt3: return DdsFormat.DXT3;
                                case PFFourCC.dxt5: return DdsFormat.DXT5;
                            }

                        // Is it a (supported) Luminance map?
                        if ((flags & PFFlags.luminence) != 0)
                            return DdsFormat.A8L8;

                        // Not a compressed texture and not a luminence map means
                        // we should have a valid RGB bit count.
                        // So use the bit depths and masks
                        if (rgbBitCount == 32)
                        {
                            if ((rBitMask == 0x00ff0000) && (gBitMask == 0x0000ff00) && (bBitMask == 0x000000ff))
                            {
                                return aBitMask == 0xff000000 ? DdsFormat.A8R8G8B8 : DdsFormat.X8R8G8B8;
                            }
                            else if ((rBitMask == 0x000000ff) && (gBitMask == 0x0000ff00) && (bBitMask == 0x00ff0000))
                            {
                                return aBitMask == 0xff000000 ? DdsFormat.A8B8G8R8 : DdsFormat.X8B8G8R8;
                            }
                        }
                        else if (rgbBitCount == 24)
                        {
                            if ((rBitMask == 0x00ff0000) && (gBitMask == 0x0000ff00) && (bBitMask == 0x000000ff) && (aBitMask == 0))
                            {
                                return DdsFormat.R8G8B8;
                            }
                        }
                        else if (rgbBitCount == 16)
                        {
                            if ((rBitMask == 0x00000f00) && (gBitMask == 0x000000f0) && (bBitMask == 0x0000000f) && (aBitMask == 0x0000f000))
                            {
                                return DdsFormat.A4R4G4B4;
                            }
                            else if ((rBitMask == 0x00007c00) && (gBitMask == 0x000003e0) && (bBitMask == 0x0000001f) && (aBitMask == 0x00008000))
                            {
                                return DdsFormat.A1R5G5B5;
                            }
                            else if (
                                (rBitMask == 0x0000f800) && (gBitMask == 0x000007e0) && (bBitMask == 0x0000001f) && (aBitMask == 0))
                            {
                                return DdsFormat.R5G6B5;
                            }
                        }

                        // Oh dear...
                        throw new NotSupportedException(String.Format(
                            "Uncompressed image with unsupported format.  Flags {0}.  RBG Bit Count is {1}.  ARGB mask is {2:X8}."
                            , flags
                            , rgbBitCount
                            , aBitMask | rBitMask | gBitMask | bBitMask
                            ));
                    }
                }

                #region Supported conversion methods - decode
                public Func<uint, uint> ToARGB()
                {
                    if (FourCC != 0) return fromDDS_A8B8G8R8;
                    switch (FileFormat)
                    {
                        case DdsFormat.A8R8G8B8: return fromDDS_A8R8G8B8;
                        case DdsFormat.X8R8G8B8: return fromDDS_X8R8G8B8;
                        case DdsFormat.A8B8G8R8: return fromDDS_A8B8G8R8;
                        case DdsFormat.X8B8G8R8: return fromDDS_X8B8G8R8;
                        case DdsFormat.A1R5G5B5: return fromDDS_A1R5G5B5;
                        case DdsFormat.A4R4G4B4: return fromDDS_A4R4G4B4;
                        case DdsFormat.R8G8B8: return fromDDS_R8G8B8;
                        case DdsFormat.R5G6B5: return fromDDS_R5G6B5;
                        case DdsFormat.A8L8: return fromDDS_A8L8;
                    }
                    return null;
                }

                static uint fromDDS_A8R8G8B8(uint pixelColour) { return pixelColour; }
                static uint fromDDS_X8R8G8B8(uint pixelColour) { return 0xFF000000 | pixelColour; }
                static uint fromDDS_A8B8G8R8(uint pixelColour) { return (pixelColour & 0xFF00FF00) | (pixelColour & 0x00FF0000) >> 16 | (pixelColour & 0x000000FF) << 16; }
                static uint fromDDS_X8B8G8R8(uint pixelColour) { return 0xFF000000 | (pixelColour & 0x00FF0000) >> 16 | (pixelColour & 0x000000FF) << 16; }
                static uint fromDDS_A1R5G5B5(uint pixelColour)
                {
                    uint A = (pixelColour >> 15) & 0xff;
                    uint R = (pixelColour >> 10) & 0x1f;
                    uint G = (pixelColour >> 5) & 0x1f;
                    uint B = (pixelColour >> 0) & 0x1f;

                    return (A << 24) | ((R << 3) | (R >> 2) << 16) | ((G << 3) | (G >> 2) << 8) | ((B << 3) | (B >> 2));
                }
                static uint fromDDS_A4R4G4B4(uint pixelColour)
                {
                    uint A = (pixelColour >> 12) & 0x0f;
                    uint R = (pixelColour >> 8) & 0x0f;
                    uint G = (pixelColour >> 4) & 0x0f;
                    uint B = (pixelColour >> 0) & 0x0f;

                    return ((A << 4) | A) << 24 | ((R << 4) | R) << 16 | ((G << 4) | G) << 8 | ((B << 4) | B);
                }
                static uint fromDDS_R8G8B8(uint pixelColour) { return 0xFF000000 | pixelColour; }
                static uint fromDDS_R5G6B5(uint pixelColour)
                {
                    uint R = (pixelColour >> 11) & 0x1f;
                    uint G = (pixelColour >> 5) & 0x3f;
                    uint B = (pixelColour >> 0) & 0x1f;

                    return ((uint)0xff << 24) | ((R << 3) | (R >> 2)) << 16 | ((G << 2) | (G >> 4)) << 8 | ((B << 3) | (B >> 2));
                }
                static uint fromDDS_A8L8(uint pixelColour)
                {
                    uint A = (pixelColour & 0x0000FF00) >> 8;
                    uint L = pixelColour & 0x000000FF;

                    return A << 24 | L << 16 | L << 8 | L;
                }
                #endregion

                #region Supported conversion methods - encode
                public Func<uint, uint> FromARGB()
                {
                    if (FourCC != 0) return toDDS_A8B8G8R8;
                    switch (FileFormat)
                    {
                        case DdsFormat.A8R8G8B8: return toDDS_A8R8G8B8;
                        case DdsFormat.X8R8G8B8: return toDDS_X8R8G8B8;
                        case DdsFormat.A8B8G8R8: return toDDS_A8B8G8R8;
                        case DdsFormat.X8B8G8R8: return toDDS_X8B8G8R8;
                        case DdsFormat.A1R5G5B5: return toDDS_A1R5G5B5;
                        case DdsFormat.A4R4G4B4: return toDDS_A4R4G4B4;
                        case DdsFormat.R8G8B8: return toDDS_R8G8B8;
                        case DdsFormat.R5G6B5: return toDDS_R5G6B5;
                        case DdsFormat.A8L8: return toDDS_A8L8;
                    }
                    return null;
                }

                static uint toDDS_A8R8G8B8(uint argb) { return argb; }
                static uint toDDS_X8R8G8B8(uint argb) { return argb & 0x00FFFFFF; }
                static uint toDDS_A8B8G8R8(uint argb) { return (argb & 0xFF00FF00) | (argb & 0x00FF0000) >> 16 | (argb & 0x000000FF) << 16; }
                static uint toDDS_X8B8G8R8(uint argb) { return (argb & 0x0000FF00) | (argb & 0x00FF0000) >> 16 | (argb & 0x000000FF) << 16; }
                static uint toDDS_A1R5G5B5(uint argb)
                {
                    uint A = (uint)((argb & 0xFF000000) == 0 ? 0 : 1) << 15;
                    uint R = ((argb & 0x00F80000) >> 3) >> 6;
                    uint G = ((argb & 0x0000F800) >> 3) >> 3;
                    uint B = ((argb & 0x000000F8) >> 3) >> 0;
                    return A | R | G | B;
                }
                static uint toDDS_A4R4G4B4(uint argb)
                {
                    uint A = ((argb & 0xF0000000) >> 4) >> 12;
                    uint R = ((argb & 0x00F00000) >> 4) >> 8;
                    uint G = ((argb & 0x0000F000) >> 4) >> 4;
                    uint B = ((argb & 0x000000F0) >> 4);
                    return A | R | G | B;
                }
                static uint toDDS_R8G8B8(uint argb) { return argb & 0x00FFFFFF; }
                static uint toDDS_R5G6B5(uint argb)
                {
                    uint R = ((argb & 0x00F80000) >> 3) >> 5;
                    uint G = ((argb & 0x0000FC00) >> 2) >> 3;
                    uint B = (argb & 0x000000F8) >> 3;
                    return (uint)0x00000000 | R | G | B;
                }
                static uint toDDS_A8L8(uint argb)
                {
                    uint L = (argb.R() + argb.G() + argb.B()) / 3;
                    return argb.A() << 8 | (L & 0xFF);
                }
                #endregion

            }

            enum DdsMagic : uint
            {
                dds_ = 0x20534444,
            }

            [Flags]
            enum DdsFlags : uint
            {
                caps = 0x01,
                height = 0x02,
                width = 0x04,
                pitch = 0x08,

                pixelFormat = 0x00001000,

                mipMapCount = 0x00020000,

                linearSize = 0x00080000,

                depth = 0x00800000,
            }
            const DdsFlags writeFlags = DdsFlags.caps | DdsFlags.height | DdsFlags.width | DdsFlags.pixelFormat;

            [Flags]
            enum DdsSurfaceCaps : uint
            {
                complex = 0x08,
                texture = 0x00001000,
                mipMap = 0x00400000,
            }

            [Flags]
            enum DdsCubeMapCaps : uint
            {
                isCubeMap = 0x00000200,
                isVolume = 0x00200000,

                positiveX = 0x00000400,
                negativeX = 0x00000800,
                positiveY = 0x00001000,
                negativeY = 0x00002000,
                positiveZ = 0x00004000,
                negativeZ = 0x00008000,
            }
            #endregion

            #region Attributes
            DdsMagic magic;//0
            uint size;//4
            DdsFlags flags;//8
            public uint height { get; private set; }//12
            public uint width { get; private set; }//16
            uint pitchOrLinearSize;//20
            uint depth;//24
            uint numMipMaps;//28
            uint[] reserved1 = new uint[11];//32
            DdsPixelFormat pixelFormat;//76
            DdsSurfaceCaps surfaceCaps;//108
            DdsCubeMapCaps cubeMapCaps;//112
            uint unusedCaps3;//116
            uint unusedCaps4;//120
            uint reserved2;//124
            #endregion

            public DdsHeader(DdsFormat ddsFormat, int width, int height)
            {
                this.magic = DdsMagic.dds_;
                this.size = 124;
                //flags
                this.height = (uint)height;
                this.width = (uint)width;
                //pitchOrLinearSize
                this.depth = 0;
                this.numMipMaps = 0;
                for (int i = 0; i < reserved1.Length; i++) this.reserved1[i] = 0;
                this.pixelFormat = new DdsPixelFormat(ddsFormat);
                this.surfaceCaps = DdsSurfaceCaps.texture;
                this.cubeMapCaps = 0;
                this.unusedCaps3 = 0;
                this.unusedCaps4 = 0;
                this.reserved2 = 0;

                this.flags = writeFlags | (IsBlockCompressed ? DdsFlags.linearSize : DdsFlags.pitch);
                this.pitchOrLinearSize = IsBlockCompressed ? LinearSize : Pitch;
            }

            public DdsHeader(Stream s)
            {
                BinaryReader r = new BinaryReader(s);

                // -- DWORD dwMagic --

                magic = (DdsMagic)r.ReadUInt32();
                if (!Enum.IsDefined(typeof(DdsMagic), magic))
                    throw new InvalidDataException(String.Format(
                        "DDS Magic Number invalid.  Got 0x{0:X8}, expected 0x{1:X8} ('DDS ').  At 0x{2:X8}."
                        , (uint)magic
                        , (uint)DdsMagic.dds_
                        , s.Position
                        )
                    );

                // -- DDS_HEADER header --

                size = r.ReadUInt32();
                if (size == 0)
                {
                    Console.WriteLine(String.Format(
                        "DDS Header Size is zero, expected 124.  At 0x{0:X8}.  Correcting..."
                        , s.Position
                        ));
                    size = 124;
                }
                else if (size != 124)
                    throw new InvalidDataException(String.Format(
                        "DDS Header Size invalid.  Got {0}, expected 124.  At 0x{1:X8}."
                        , size
                        , s.Position
                        ));

                flags = (DdsFlags)r.ReadUInt32();
                if (((uint)(flags & (writeFlags | DdsFlags.pitch | DdsFlags.linearSize))).Bits() != ((uint)writeFlags).Bits() + 1)
                {
                    Console.WriteLine(String.Format(
                        "DDS Header Flags invalid.  Got {0}, expected {1} and either _pitch or _linearSize.  At 0x{2:X8}.  Correcting..."
                        , flags
                        , writeFlags
                        , s.Position
                        ));
                    flags |= writeFlags;
                    // We can't know yet which is best to pick of _pitch and _linearSize
                }

                height = r.ReadUInt32();
                if (height == 0)
                    throw new InvalidDataException(String.Format(
                        "DDS Height invalid.  Got 0, expected non-zero value.  At 0x{1:X8}"
                        , s.Position
                        )
                    );

                width = r.ReadUInt32();
                if (width == 0)
                    throw new InvalidDataException(String.Format(
                    "DDS Width invalid.  Got 0, expected non-zero value.  At 0x{1:X8}"
                    , s.Position
                    )
                );

                pitchOrLinearSize = r.ReadUInt32();

                depth = r.ReadUInt32();
                if ((flags & DdsFlags.depth) != 0 && depth != 0)
                {
                    Console.WriteLine(String.Format(
                        "Only simple DDS Textures are supported.  This DDS has depth {0}.  At 0x{1:X8}.  Clearing flag and _depth to zero..."
                        , depth
                        , s.Position
                        ));
                    flags &= ~DdsFlags.depth;
                    depth = 0;
                }
                if ((flags & DdsFlags.depth) != 0 || depth != 0)
                {
                    Console.WriteLine(String.Format(
                        "DDS Header invalid.  Inconsistent depth flag ({0}) and _depth ({1}).  At 0x{2:X8}.  Clearing flag and _depth to zero..."
                        , flags & DdsFlags.depth
                        , depth
                        , s.Position
                        ));
                    flags &= ~DdsFlags.depth;
                    depth = 0;
                }

                numMipMaps = r.ReadUInt32();
                if ((flags & DdsFlags.mipMapCount) != 0 && numMipMaps != 1)
                {
                    Console.WriteLine(String.Format(
                        "Only simple DDS Textures are supported.  This DDS has {0} mip maps.  At 0x{1:X8}.  Clearing flag and setting count to one..."
                        , numMipMaps
                        , s.Position
                        ));
                    flags &= ~DdsFlags.mipMapCount;
                    numMipMaps = 1;
                }
                if ((flags & DdsFlags.mipMapCount) != 0 || numMipMaps == 0)
                {
                    Console.WriteLine(String.Format(
                        "DDS Header invalid.  Inconsistent mipMapCount flag ({0}) and _numMipMaps ({1}).  At 0x{2:X8}.  Clearing flag and setting count to one..."
                        , flags & DdsFlags.mipMapCount
                        , numMipMaps
                        , s.Position
                        ));
                    flags &= ~DdsFlags.mipMapCount;
                    numMipMaps = 1;
                }

                for (int i = 0; i < reserved1.Length; i++) reserved1[i] = r.ReadUInt32();
                for (int i = 0; i < reserved1.Length; i++) reserved1[i] = 0;

                pixelFormat = new DdsPixelFormat(s);

                var mask = (pixelFormat.FourCC == 0) ? DdsFlags.pitch : DdsFlags.linearSize;
                var notMask = (pixelFormat.FourCC != 0) ? DdsFlags.pitch : DdsFlags.linearSize;
                if ((flags & mask) == 0)
                {
                    Console.WriteLine(String.Format(
                        "DDS Header Flags invalid.  {0} not set.  At 0x{1:X8}.  Correcting..."
                        , mask
                        , s.Position
                        ));
                    flags &= ~notMask;
                    flags |= mask;
                }

                surfaceCaps = (DdsSurfaceCaps)r.ReadUInt32();
                if ((surfaceCaps & ~DdsSurfaceCaps.texture) != 0)
                {
                    Console.WriteLine(String.Format(
                        "Only simple DDS Textures are supported.  This DDS has other surface capabilities: {0}.  At 0x{1:X8}.  Clearing unsupported capabilities..."
                        , surfaceCaps
                        , s.Position
                        ));
                    surfaceCaps &= DdsSurfaceCaps.texture;
                }
                if (surfaceCaps == 0)
                {
                    Console.WriteLine(String.Format(
                        "DDS Header Surface Caps invalid.  Got zero, expected texture.  At 0x{0:X8}.  Correcting..."
                        , s.Position
                        ));
                    surfaceCaps |= DdsSurfaceCaps.texture;
                }

                cubeMapCaps = (DdsCubeMapCaps)r.ReadUInt32();
                if (cubeMapCaps != 0)
                {
                    Console.WriteLine(String.Format(
                        "Only simple DDS Textures are supported.  This DDS has cube map capabilities: {0}.  At 0x{1:X8}.  Clearing unsupported capabilities..."
                        , cubeMapCaps
                        , s.Position
                        ));
                    cubeMapCaps = 0;
                }

                unusedCaps3 = r.ReadUInt32();
                unusedCaps4 = r.ReadUInt32();
                reserved2 = r.ReadUInt32();

                unusedCaps3 = unusedCaps4 = reserved2 = 0;

                // -- DDS_HEADER_DX10 header10 -- Not Supported
            }

            public void Write(Stream s)
            {
                BinaryWriter w = new BinaryWriter(s);

                w.Write((uint)DdsMagic.dds_);//0
                w.Write((uint)124);//4
                w.Write((uint)(writeFlags | (IsBlockCompressed ? DdsFlags.linearSize : DdsFlags.pitch)));//8
                w.Write((uint)height);//12
                w.Write((uint)width);//16
                w.Write((uint)(IsBlockCompressed ? CompressedSize : Pitch));//20 ;; CompressedSize is what GIMP DDS plugin puts.
                w.Write((uint)0);//24
                w.Write((uint)1);//28
                for (int i = 0; i < reserved1.Length; i++) w.Write((uint)0);//32
                pixelFormat.Write(s);//76
                w.Write((uint)DdsSurfaceCaps.texture);//108
                w.Write((uint)0);//112
                w.Write((uint)0);//116
                w.Write((uint)0);//120
                w.Write((uint)0);//124
            }

            public DdsFormat FileFormat { get { return pixelFormat.FileFormat; } }

            public bool IsBlockCompressed { get { return pixelFormat.FourCC != 0; } }

            uint Pitch { get { return ((width * pixelFormat.RGBBitCount) + 7) / 8; } }

            uint LinearSize { get { return Math.Max(1, (width + 3) / 4) * pixelFormat.BlockSize; } }

            public uint DataSize { get { return IsBlockCompressed ? CompressedSize : UncompressedSize; } }

            uint CompressedSize
            {
                get
                {
                    if (!IsBlockCompressed)
                        throw new InvalidOperationException("DDS is not block compressed.");

                    return LinearSize * Math.Max(1, (height + 3) / 4);
                }
            }

            uint UncompressedSize
            {
                get
                {
                    if (IsBlockCompressed)
                        throw new InvalidOperationException("DDS is block compressed.");

                    // The simple case is to return the pitch supplied by the encoder in good faith
                    if ((flags & DdsFlags.pitch) != 0)
                        return pitchOrLinearSize * height;

                    // However, some encoders behave badly
                    if ((flags & DdsFlags.linearSize) != 0)
                    {
                        Console.WriteLine("Uncompressed image, pitch flag not set but linear size set.");
                        return pitchOrLinearSize;
                    }

                    // In the worst case, simply use the internal value
                    Console.WriteLine("Uncompressed image but neither pitch nor linear size flags set - computing pitch.");
                    return Pitch * height;
                }
            }

            public uint UncompressedPixelSize
            {
                get
                {
                    if (IsBlockCompressed)
                        throw new InvalidOperationException("DDS is block compressed.");
                    return pixelFormat.RGBBitCount / 8;
                }
            }

            public DdsSquish.SquishFlags SquishFourCC
            {
                get
                {
                    switch (pixelFormat.FourCC)
                    {
                        case DdsPixelFormat.PFFourCC.dxt1: return DdsSquish.SquishFlags.kDxt1;
                        case DdsPixelFormat.PFFourCC.dxt3: return DdsSquish.SquishFlags.kDxt3;
                        case DdsPixelFormat.PFFourCC.dxt5: return DdsSquish.SquishFlags.kDxt5;
                    }
                    throw new InvalidOperationException("Unsupported FourCC for Squish.");
                }
            }

            public Func<uint, uint> Decoder { get { return pixelFormat.ToARGB(); } }
            public Func<uint, uint> Encoder { get { return pixelFormat.FromARGB(); } }
        }

        public enum DdsFormat
        {
            DXT1,
            DXT3,
            DXT5,
            A8R8G8B8,
            X8R8G8B8,
            A8B8G8R8,
            X8B8G8R8,
            A1R5G5B5,
            A4R4G4B4,
            R8G8B8,
            R5G6B5,
            A8L8,
        }
        #endregion

        #region Attributes
        DdsHeader header;
        uint[] baseImage; //ARGB uint array
        #endregion

        public Dds(DdsFormat ddsFormat, int width, int height, uint[] imageData)
        {
            if (imageData.Length != width * height)
                throw new ArgumentException("Image data must contain width * height elements");

            header = new DdsHeader(ddsFormat, width, height);
            baseImage = (uint[])imageData.Clone();
        }

        public Dds(Stream s)
        {
            header = new DdsHeader(s);

            byte[] buffer = new byte[header.DataSize];
            if (s.Read(buffer, 0, buffer.Length) != buffer.Length)
                throw new IOException(String.Format("Failed to read DDS data.  At 0x{0:X8}.", s.Position));

            var decoder = header.Decoder;
            if (decoder == null)
                throw new FormatException("File is not a supported DDS format");

            if (header.IsBlockCompressed)
            {
                // Decompress
                byte[] pixelData = DdsSquish.DecompressImage(buffer, (int)header.width, (int)header.height, header.SquishFourCC);

                // Convert R, G, B, A byte array to ARGB uint array...
                baseImage = new uint[pixelData.Length / sizeof(uint)];
                Enumerable.Range(0, baseImage.Length).AsParallel()
                    .ForAll(i => baseImage[i] = decoder(BitConverter.ToUInt32(pixelData, i * sizeof(uint))));
            }
            else
            {
                // Convert encoded data to ARGB uint array
                baseImage = new uint[header.width * header.height];
                int rowPitch = buffer.Length / (int)header.height;
                int pixelSize = (int)header.UncompressedPixelSize;
                Enumerable.Range(0, (int)header.width).AsParallel()
                    .ForAll(destX => Enumerable.Range(0, (int)header.height).AsParallel()
                        .ForAll(destY =>
                        {
                            // Compute pixel offsets
                            int srcPixelOffset = (destY * rowPitch) + (destX * pixelSize);
                            int destPixelOffset = (destY * (int)header.width) + destX;

                            // Build our pixel colour as a DWORD - parallelism overhead costs too much
                            uint pixelColour = 0;
                            for (int loop = 0; loop < pixelSize; loop++)
                                pixelColour |= (uint)(buffer[srcPixelOffset + loop] << (8 * loop));

                            // delegate takes care of calculation
                            baseImage[destPixelOffset] = decoder(pixelColour);
                        }));
            }

        }

        public void Write(Stream s)
        {
            byte[] buffer;

            var encoder = header.Encoder;
            if (encoder == null)
                throw new FormatException("File is not a supported DDS format");

            if (header.IsBlockCompressed)
            {
                // Convert ARGB uint array to R, G, B, A byte array...
                byte[] pixelData = new byte[baseImage.Length * sizeof(uint)];
                Enumerable.Range(0, baseImage.Length).AsParallel()
                    .ForAll(i => Array.Copy(BitConverter.GetBytes(encoder(baseImage[i])), 0, pixelData, i * sizeof(uint), sizeof(uint)));

                // Compress
                buffer = DdsSquish.CompressImage(pixelData, (int)header.width, (int)header.height, header.SquishFourCC);
                if (buffer.Length != header.DataSize)
                    throw new Exception(String.Format("DdsSquish returned an unexpected buffer size, 0x{0:X8}.  Expected 0x{1:X8}", buffer.Length, header.DataSize));
            }
            else
            {
                buffer = new byte[header.DataSize];
                int rowPitch = buffer.Length / (int)header.height;
                int pixelSize = (int)header.UncompressedPixelSize;
                Enumerable.Range(0, (int)header.width).AsParallel()
                    .ForAll(srcX => Enumerable.Range(0, (int)header.height).AsParallel()
                        .ForAll(srcY =>
                        {
                            // Compute pixel offsets
                            int destPixelOffset = (srcY * rowPitch) + (srcX * pixelSize);
                            int srcPixelOffset = (srcY * (int)header.width) + srcX;

                            // delegate takes care of calculation
                            uint pixelColour = encoder(baseImage[destPixelOffset]);

                            // Store each computed byte - parallelism overhead costs too much
                            for (int loop = 0; loop < pixelSize; loop++)
                                buffer[destPixelOffset + loop] = (byte)((pixelColour >> (8 * loop)) & 0xff);
                        }));
            }

            header.Write(s);
            s.Write(buffer, 0, buffer.Length);
        }

        public int Height { get { return (int)header.height; } }
        public int Width { get { return (int)header.width; } }
        public DdsFormat FileFormat { get { return header.FileFormat; } }
        public uint[] ImageData { get { return (uint[])baseImage.Clone(); } }
    }

    /// <summary>
    /// Represents an image encoded using one of the supported DDS mechanisms.
    /// </summary>
    /// <remarks>
    /// A &quot;DirectX Draw Surface&quot; stores compressed pixel data that is used when
    /// rendering scenes.  The pixel data may be used for purposes other than purely display,
    /// such as being used for masked operations on another DDS image.
    /// <para>Note that this assembly depends on two unmanaged libraries:
    /// <br/>squishinterface_Win32.dll - code for 32bit Windows systems.
    /// <br/>squishinterface_x64.dll - code for 64bit Windows systems.
    /// </para>
    /// </remarks>
    public class DdsFile : IDisposable
    {
        #region Attributes
        int width = 0;
        int height = 0;
        bool useBlockCompression = false;
        bool useLuminence = false;
        int alphaDepth = 0;

        // ARGB data as extracted from the DDS resource
        uint[] baseImage = null;

        // ARGB data as currently mutilated by effects
        uint[] currentImage = null;

        // HSVa data
        ColorHSVA[] hsvData = null;

        // The hsvShift applied to create hsvData
        HSVShift hsvShift;
        #endregion

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            baseImage = null;
            currentImage = null;
            maskInEffect = false;
            hsvData = null;
            hsvShift = HSVShift.Empty;
        }

        #region "Constructors", really
        /// <summary>
        /// Creates an image of a single colour, specified by the byte parameters,
        /// with the size given by the int parameters.
        /// If <paramref name="supportHSV"/> is true, also creates an HSVa-encoded version of the image.
        /// </summary>
        /// <param name="r">Amount of red per pixel.</param>
        /// <param name="g">Amount of green per pixel.</param>
        /// <param name="b">Amount of blue per pixel.</param>
        /// <param name="a">Amount of alpha per pixel.</param>
        /// <param name="width">Width of image.</param>
        /// <param name="height">Height of image.</param>
        /// <param name="supportHSV">When true, create an HSVa-encoded version of the image.</param>
        public void CreateImage(byte r, byte g, byte b, byte a, int width, int height, bool supportHSV)
        {
            CreateImage((uint)(a << 24 | r << 16 | g << 8 | b), width, height, supportHSV);
        }

        /// <summary>
        /// Creates an image of a single colour, specified by the <see cref="Color"/> parameter,
        /// with the size given by the int parameters.
        /// If <paramref name="supportHSV"/> is true, also creates an HSVa-encoded version of the image.
        /// </summary>
        /// <param name="color"><see cref="Color"/> of image.</param>
        /// <param name="width">Width of image.</param>
        /// <param name="height">Height of image.</param>
        /// <param name="supportHSV">When true, create an HSVa-encoded version of the image.</param>
        public void CreateImage(Color color, int width, int height, bool supportHSV)
        {
            CreateImage((uint)color.ToArgb(), width, height, supportHSV);
        }

        /// <summary>
        /// Creates an image of a single colour, specified by the uint parameter
        /// (low byte is "blue", then "green", then "red" then high byte is "alpha"),
        /// with the size given by the int parameters.
        /// If <paramref name="supportHSV"/> is true, also creates an HSVa-encoded version of the image.
        /// </summary>
        /// <param name="argb">Colour of image (low byte is "blue", then "green", then "red" then high byte is "alpha").</param>
        /// <param name="width">Width of image.</param>
        /// <param name="height">Height of image.</param>
        /// <param name="supportHSV">When true, create an HSVa-encoded version of the image.</param>
        public void CreateImage(uint argb, int width, int height, bool supportHSV)
        {
            this.width = width;
            this.height = height;
            useBlockCompression = true;
            useLuminence = false;
            alphaDepth = 5;

            // SetPixels operates on currentImage, so create this...
            currentImage = new uint[width * height];
            SetPixels((x, y, unused) => argb);

            // ...and set the baseImage from the current image
            baseImage = (uint[])currentImage.Clone();

            if (supportHSV) UpdateHSVData();
        }

        /// <summary>
        /// Creates an image from a given <see cref="T:DdsFile"/>.
        /// If <paramref name="supportHSV"/> is true, also creates an HSVa-encoded version of the image.
        /// </summary>
        /// <param name="image"><see cref="T:DdsFile"/> to clone.</param>
        /// <param name="supportHSV">When true, create an HSVa-encoded version of the image.</param>
        public void CreateImage(DdsFile image, bool supportHSV)
        {
            width = image.width;
            height = image.height;
            useBlockCompression = image.useBlockCompression;
            useLuminence = image.useLuminence;
            alphaDepth = image.alphaDepth;

            baseImage = (uint[])image.currentImage.Clone();

            currentImage = (uint[])baseImage.Clone();
            if (supportHSV) UpdateHSVData();
        }

        /// <summary>
        /// Creates an image from a given <see cref="T:DdsFile"/>, resized as requested.
        /// If <paramref name="supportHSV"/> is true, also creates an HSVa-encoded version of the image.
        /// </summary>
        /// <param name="image"><see cref="T:DdsFile"/> to clone.</param>
        /// <param name="width">Width of image.</param>
        /// <param name="height">Height of image.</param>
        /// <param name="supportHSV">When true, create an HSVa-encoded version of the image.</param>
        public void CreateImage(DdsFile image, int width, int height, bool supportHSV)
        {
            this.width = width;
            this.height = height;
            useBlockCompression = image.useBlockCompression;
            useLuminence = image.useLuminence;
            alphaDepth = image.alphaDepth;

            if (useLuminence)
            {
                // Life would be still relatively simple were it not for Luminence maps.
                // With these, you can't just strip the alpha channel -- they exist only for that channel,
                // so we need to deal with them separately.
                currentImage = new uint[width * height];
                SetPixels((x, y, unused) => 0);
                baseImage = (uint[])currentImage.Clone();

                Bitmap alpha = new Bitmap(image.GetGreyscaleFromAlpha(), width, height);
                SetAlphaFromGreyscale(alpha);
            }
            else if (alphaDepth == 0)
            {
                baseImage = new Bitmap(image.Image, width, height).ToARGBData();
                currentImage = (uint[])baseImage.Clone();
            }
            else
            {
                // Regardless of the pixel format, using Bitmap to resize the image pre-multiplies the alpha
                // so we need this mess.

                // Clone the image, strip the alpha, then resize
                using (DdsFile ddsFileBase = new DdsFile())
                {
                    ddsFileBase.CreateImage(image, false);
                    ddsFileBase.DeleteAlphaChannel();
                    baseImage = new Bitmap(ddsFileBase.Image, width, height).ToARGBData();
                }

                currentImage = (uint[])baseImage.Clone();

                // Now reapply the original alpha
                Bitmap alpha = new Bitmap(image.GetGreyscaleFromAlpha(), width, height);
                SetAlphaFromGreyscale(alpha);
            }

            if (supportHSV) UpdateHSVData();
        }

        /// <summary>
        /// Creates an image from a given <see cref="T:Image"/>.
        /// If <paramref name="supportHSV"/> is true, also creates an HSVa-encoded version of the image.
        /// </summary>
        /// <param name="image"><see cref="T:Image"/> from which to extract image pixels.</param>
        /// <param name="supportHSV">When true, create an HSVa-encoded version of the image.</param>
        public void CreateImage(Image image, bool supportHSV) { CreateImage(new Bitmap(image), supportHSV); }

        /// <summary>
        /// Creates an image from a given <see cref="T:Bitmap"/>.
        /// If <paramref name="supportHSV"/> is true, also creates an HSVa-encoded version of the image.
        /// </summary>
        /// <param name="image"><see cref="T:Bitmap"/> from which to extract image pixels.</param>
        /// <param name="supportHSV">When true, create an HSVa-encoded version of the image.</param>
        public void CreateImage(Bitmap image, bool supportHSV)
        {
            width = image.Width;
            height = image.Height;
            useBlockCompression = true;
            useLuminence = false;
            alphaDepth = 5;

            baseImage = image.ToARGBData();

            currentImage = (uint[])baseImage.Clone();
            if (supportHSV) UpdateHSVData();
        }

        /// <summary>
        /// Creates an image from a given <see cref="T:Bitmap"/>, resized as requested.
        /// If <paramref name="supportHSV"/> is true, also creates an HSVa-encoded version of the image.
        /// </summary>
        /// <param name="image"><see cref="T:Bitmap"/> from which to extract image pixels.</param>
        /// <param name="width">Width of image.</param>
        /// <param name="height">Height of image.</param>
        /// <param name="supportHSV">When true, create an HSVa-encoded version of the image.</param>
        public void CreateImage(Bitmap image, int width, int height, bool supportHSV)
        {
            using (DdsFile ddsFileBase = new DdsFile())
            {
                ddsFileBase.CreateImage(image, false);
                CreateImage(ddsFileBase.Resize(new Size(width, height)), supportHSV);
            }
        }
        #endregion

        #region File I/O
        /// <summary>
        /// Loads the data from an image encoded using one of the supported DDS mechanisms.
        /// If <paramref name="supportHSV"/> is true, also creates an HSVa-encoded version of the image.
        /// </summary>
        /// <param name="input">A <see cref="System.IO.Stream"/> containing the DDS-encoded image.</param>
        /// <param name="supportHSV">When true, create an HSVa-encoded version of the image.</param>
        public void Load(System.IO.Stream input, bool supportHSV)
        {
            Dds dds = new Dds(input);

            width = dds.Width;
            height = dds.Height;
            useBlockCompression = false;
            useLuminence = false;
            alphaDepth = 0;

            Dds.DdsFormat fmt = dds.FileFormat;
            switch (fmt)
            {
                case Dds.DdsFormat.DXT1: useBlockCompression = true; alphaDepth = 1; break;
                case Dds.DdsFormat.DXT3: useBlockCompression = true; alphaDepth = 3; break;
                case Dds.DdsFormat.DXT5: useBlockCompression = true; alphaDepth = 5; break;
                case Dds.DdsFormat.A8L8: useLuminence = true; alphaDepth = 8; break;
                case Dds.DdsFormat.A1R5G5B5: alphaDepth = 1; break;
                case Dds.DdsFormat.A4R4G4B4: alphaDepth = 4; break;
                case Dds.DdsFormat.A8B8G8R8:
                case Dds.DdsFormat.A8R8G8B8: alphaDepth = 8; break;
            }

            baseImage = dds.ImageData;

            currentImage = (uint[])baseImage.Clone();
            if (supportHSV) UpdateHSVData();
        }

        ///  <summary>
        ///  Saves the current image using one of the supported DDS mechanisms.
        ///  </summary>
        ///  <param name="output">A <see cref="T:System.IO.Stream"/> to receive the DDS-encoded image.</param>
        public void Save(Stream output)
        {
            Dds.DdsFormat ddsFormat;
            if (useBlockCompression)
                switch (alphaDepth)
                {
                    case 1: ddsFormat = Dds.DdsFormat.DXT1; break;
                    case 3: ddsFormat = Dds.DdsFormat.DXT3; break;
                    default: ddsFormat = Dds.DdsFormat.DXT5; break;
                }
            else if (useLuminence)
                ddsFormat = Dds.DdsFormat.A8L8;
            else
                switch (alphaDepth)
                {
                    case 0: ddsFormat = Dds.DdsFormat.R8G8B8; break;
                    case 1: ddsFormat = Dds.DdsFormat.A1R5G5B5; break;
                    case 4: ddsFormat = Dds.DdsFormat.A4R4G4B4; break;
                    default: ddsFormat = Dds.DdsFormat.A8R8G8B8; break;
                }

            Dds dds = new Dds(ddsFormat, width, height, baseImage);
            dds.Write(output);
            output.Flush();
        }
        #endregion

        /// <summary>
        /// The image size.
        /// </summary>
        public Size Size { get { return new Size(width, height); } }

        /// <summary>
        /// Get a new DdsFile of the given size based on the current image.
        /// </summary>
        /// <param name="size">The new size.</param>
        /// <returns>A new DdsFile of the given size based on the current image.</returns>
        public DdsFile Resize(Size size)
        {
            DdsFile ddsFile = new DdsFile();
            ddsFile.CreateImage(this, size.Width, size.Height, SupportsHSV);
            return ddsFile;
        }

        /// <summary>
        /// If true, use DXT-type image compression for storage.
        /// The exact format will depend on the <see cref="AlphaDepth"/>.
        /// By default, DXT5 will be used.
        /// Setting to false (from true) will default to A8B8G8R8 format.
        /// </summary>
        public bool UseDXT
        {
            get { return useBlockCompression; }
            set
            {
                if (value == useBlockCompression) return;

                useBlockCompression = value;

                if (value)
                {
                    UseLuminance = false;
                    AlphaDepth = 5;
                }
                else
                    AlphaDepth = 8;
            }
        }

        /// <summary>
        /// If true, treat the image as a luminance (plus alpha) map for storage.
        /// Currently only A8L8, 16bit coding is supported.
        /// Setting to false (from true) will default to A8B8G8R8 (non-DXT) format.
        /// </summary>
        public bool UseLuminance
        {
            get { return useLuminence; }
            set
            {
                if (value == useLuminence) return;

                useLuminence = value;

                if (value)
                    UseDXT = false;
            }
        }

        /// <summary>
        /// When UseDXT is true, the DXT compression mode.
        /// Otherwise the number of bits per alpha pixel.
        /// </summary>
        /// <remarks>
        /// Value is used when saving a DDS.
        /// Invalid values may cause exceptions at that point.
        /// </remarks>
        public int AlphaDepth { get { return alphaDepth; } set { alphaDepth = value; } }

        #region Set Alpha channel
        /// <summary>
        /// Converts the R, G and B channels of the supplied <paramref name="image"/> to greyscale
        /// and loads this into the Alpha channel of the current image.
        /// The image format will be changed to one supporting an 8-bit Alpha channel, if required.
        /// </summary>
        /// <param name="image"><see cref="DdsFile"/> to extract greyscale data from for alpha channel.</param>
        public void SetAlphaFromGreyscale(DdsFile image)
        {
            AlphaDepth = UseDXT ? 5 : 8;

            SetPixels((x, y, value) =>
            {
                uint alpha;
                lock (image)
                {
                    uint srcValue = image.GetPixel(x % image.width, y % image.height);
                    alpha = ((srcValue.R() + srcValue.G() + srcValue.B()) / 3) & 0xff;
                }

                return (value & 0x00FFFFFF) | (alpha << 24);
            });

            if (SupportsHSV) UpdateHSVData();
        }

        /// <summary>
        /// Converts the R, G and B channels of the supplied <paramref name="image"/> to greyscale
        /// and loads this into the Alpha channel of the current image.
        /// </summary>
        /// <param name="image"><see cref="T:Image"/> to extract greyscale data from for alpha channel.</param>
        public void SetAlphaFromGreyscale(Image image) { SetAlphaFromGreyscale(new Bitmap(image)); }

        // (0 + 0 + 0) / 3 = 0
        // (255 + 255 + 255) / 3 = 255
        /// <summary>
        /// Converts the R, G and B channels of the supplied <paramref name="image"/> to greyscale
        /// and loads this into the Alpha channel of the current image.
        /// The image format will be changed to one supporting an Alpha channel, if required.
        /// </summary>
        /// <param name="image"><see cref="Bitmap"/> to extract greyscale data from for alpha channel.</param>
        public void SetAlphaFromGreyscale(Bitmap image)
        {
            AlphaDepth = UseDXT ? 5 : 8;

            SetPixels((x, y, value) =>
            {
                uint alpha;
                lock (image)
                {
                    Color srcValue = image.GetPixel(x % image.Width, y % image.Height);
                    alpha = ((uint)(srcValue.R + srcValue.G + srcValue.B) / 3) & 0xff;
                }

                return (value & 0x00FFFFFF) | (alpha << 24);
            });

            if (SupportsHSV) UpdateHSVData();
        }

        /// <summary>
        /// Set the image format to one without an alpha channel, clearing the alpha data.
        /// </summary>
        public void DeleteAlphaChannel()
        {
            if (UseLuminance)
                throw new InvalidOperationException("Alpha channel cannot be delete for Luminance map.");

            AlphaDepth = UseDXT ? 1 : 0;
            SetPixels((x, y, value) => value |= 0xFF000000);

            if (SupportsHSV) UpdateHSVData();
        }

        /// <summary>
        /// Get a greyscale image representing the alpha channel of the current image.
        /// </summary>
        /// <returns>A greyscale image representing the alpha channel of the current image.</returns>
        public Bitmap GetGreyscaleFromAlpha()
        {
            uint[] greyscale = new uint[currentImage.Length];

            DoAction((x, y, value) => greyscale[x + y * width] = ((uint)0xFF << 24) | value.A() << 16 | value.A() << 8 | value.A() << 0);

            return greyscale.ToBitmap(this.Size);
        }
        #endregion

        #region Image and Pixel manipulation
        /// <summary>
        /// The current image.
        /// </summary>
        public Bitmap Image { get { return GetImage(true, true, true, true, false); } }

        /// <summary>
        /// Extract a <see cref="T:Image"/> representing the current image, subject to the filtering requested.
        /// </summary>
        /// <param name="red">When true, the red channel of the DDS contributes to the red pixels of the returned image.</param>
        /// <param name="green">When true, the green channel of the DDS contributes to the green pixels of the returned image.</param>
        /// <param name="blue">When true, the blue channel of the DDS contributes to the blue pixels of the returned image.</param>
        /// <param name="alpha">When true, the alpha channel of the DDS contributes to the pixel transparency in the returned image.</param>
        /// <param name="invert">When true, the alpha channel of the DDS is inverted.</param>
        /// <returns>A <see cref="System.Drawing.Image"/> representation of the DDS encoded image in the loaded <see cref="System.IO.Stream"/>.</returns>
        /// <seealso cref="Load"/>
        public Bitmap GetImage(bool red, bool green, bool blue, bool alpha, bool invert)
        {
            uint mask = (alpha ? (uint)0xFF000000 : 0) | (red ? (uint)0x00FF0000 : 0) | (green ? (uint)0x0000FF00 : 0) | (blue ? (uint)0x000000FF : 0);
            uint[] sourcePixels = SupportsHSV && !hsvShift.IsEmpty ? ColorHSVA.ToArrayARGB(hsvData, hsvShift) : currentImage;
            uint[] destPixels;

            if (!red || !green || !blue || !alpha || invert)
            {
                destPixels = new uint[sourcePixels.Length];

                Enumerable.Range(0, height).AsParallel()
                    .ForAll(y =>
                    {
                        int offset = y * width;
                        Enumerable.Range(0, width).AsParallel()
                            .ForAll(x =>
                            {
                                uint pixel = sourcePixels[offset + x] & mask;
                                if (alpha) { if (invert) pixel = (pixel & 0x00FFFFFF) | ((255 - pixel.A()) << 24); }
                                else pixel |= 0xFF000000;

                                destPixels[offset + x] = pixel;
                            });
                    });
            }
            else
                destPixels = sourcePixels;

            System.Runtime.InteropServices.GCHandle g =
                System.Runtime.InteropServices.GCHandle.Alloc(destPixels, Runtime.InteropServices.GCHandleType.Pinned);
            IntPtr p_data = g.AddrOfPinnedObject();
            Bitmap bitmap = new Bitmap(width, height, width * sizeof(uint), alpha ? Imaging.PixelFormat.Format32bppArgb : Imaging.PixelFormat.Format32bppRgb, p_data);
            g.Free();

            return bitmap;
        }

        /// <summary>
        /// Apply a transformation function to the current image.
        /// </summary>
        /// <param name="transform">
        /// A transformation function taking a current pixel value and returning a new value.
        /// </param>
        public void SetPixels(Func<uint, uint> transform) { SetPixels((x, y, value) => transform(value)); }

        /// <summary>
        /// Apply a transformation function to the current image.
        /// </summary>
        /// <param name="transform">A transformation function taking
        /// <c>x</c>, <c>y</c> and <c>pixel</c> parameters and
        /// returning a new <c>pixel</c> value.</param>
        public void SetPixels(Func<int, int, uint, uint> transform)
        {
            DoAction((x, y, value) => SetPixel(x, y, transform(x, y, value)));

            if (SupportsHSV) UpdateHSVData();
        }

        /// <summary>
        /// Perform an action on each pixel in the current image.
        /// </summary>
        /// <param name="action">An action taking <c>x</c>, <c>y</c> and <c>pixel</c> parameters.</param>
        public void DoAction(Action<int, int, uint> action)
        {
            Enumerable.Range(0, width).AsParallel()
                .ForAll(x => Enumerable.Range(0, height).AsParallel()
                    .ForAll(y => action(x, y, GetPixel(x, y))));
        }

        private uint GetPixel(int x, int y) { return currentImage[y * width + x]; }
        private void SetPixel(int x, int y, uint value) { currentImage[y * height + x] = value; }
        #endregion

        #region HSV Magic
        /// <summary>
        /// The HSVShift applied to the current image, when supported.
        /// </summary>
        /// <seealso cref="SupportsHSV"/>
        public HSVShift HSVShift { get { return hsvShift; } set { hsvShift = value; } }

        /// <summary>
        /// True if the image is prepared to process HSV requests.
        /// </summary>
        public bool SupportsHSV
        {
            get { return hsvData != null; }
            set { if (value != SupportsHSV) { if (value) UpdateHSVData(); else hsvData = null; } }
        }

        void UpdateHSVData() { hsvData = ColorHSVA.ToArrayColorHSVA(currentImage); }
        #endregion

        #region Mask Magic

        /// <summary>
        /// Set the colour of the image based on the channels in the <paramref name="mask"/>.
        /// </summary>
        /// <param name="mask">The <see cref="System.IO.Stream"/> containing the DDS image to use as a mask.</param>
        /// <param name="ch1Colour">(Nullable) ARGB colour to the image when the first channel of the mask is active.</param>
        /// <param name="ch2Colour">(Nullable) ARGB colour to the image when the second channel of the mask is active.</param>
        /// <param name="ch3Colour">(Nullable) ARGB colour to the image when the third channel of the mask is active.</param>
        /// <param name="ch4Colour">(Nullable) ARGB colour to the image when the fourth channel of the mask is active.</param>
        public void MaskedSetColour(DdsFile mask, uint? ch1Colour, uint? ch2Colour, uint? ch3Colour, uint? ch4Colour)
        {

            maskInEffect = maskInEffect || ch1Colour.HasValue || ch2Colour.HasValue || ch3Colour.HasValue || ch4Colour.HasValue;

            if (!maskInEffect) return;

            if (ch1Colour.HasValue) MaskedSetColour(mask, x => x.R() > 0, ch1Colour.Value);
            if (ch2Colour.HasValue) MaskedSetColour(mask, x => x.G() > 0, ch2Colour.Value);
            if (ch3Colour.HasValue) MaskedSetColour(mask, x => x.B() > 0, ch3Colour.Value);
            if (ch4Colour.HasValue) MaskedSetColour(mask, x => x.A() > 0, ch4Colour.Value);

            if (SupportsHSV) UpdateHSVData();
        }
        void MaskedSetColour(DdsFile mask, Channel channel, uint argb)
        {
            MaskedApply(this.currentImage, this.Size, mask.currentImage, mask.Size, channel, (x, y) => argb);
        }

        /// <summary>
        /// Use the <paramref name="mask"/> to apply the DDS image supplied.
        /// </summary>
        /// <param name="mask">Determines to which areas each DDS image is applied.</param>
        /// <param name="ch1DdsFile">DDS image applied to <paramref name="mask"/> channel 1 area.</param>
        /// <param name="ch2DdsFile">DDS image applied to <paramref name="mask"/> channel 2 area.</param>
        /// <param name="ch3DdsFile">DDS image applied to <paramref name="mask"/> channel 3 area.</param>
        /// <param name="ch4DdsFile">DDS image applied to <paramref name="mask"/> channel 4 area.</param>
        public void MaskedApplyImage(DdsFile mask, DdsFile ch1DdsFile, DdsFile ch2DdsFile, DdsFile ch3DdsFile, DdsFile ch4DdsFile)
        {
            this.MaskedApplyImage(mask,
                (ch1DdsFile == null) ? null : ch1DdsFile.currentImage, (ch1DdsFile == null) ? Size.Empty : ch1DdsFile.Size,
                (ch2DdsFile == null) ? null : ch2DdsFile.currentImage, (ch2DdsFile == null) ? Size.Empty : ch2DdsFile.Size,
                (ch3DdsFile == null) ? null : ch3DdsFile.currentImage, (ch3DdsFile == null) ? Size.Empty : ch3DdsFile.Size,
                (ch4DdsFile == null) ? null : ch4DdsFile.currentImage, (ch4DdsFile == null) ? Size.Empty : ch4DdsFile.Size);
        }

        /// <summary>
        /// Use the <paramref name="mask"/> to apply the supplied images.
        /// </summary>
        /// <param name="mask">Determines to which areas each image is applied.</param>
        /// <param name="ch1Image">Image applied to <paramref name="mask"/> channel 1 area.</param>
        /// <param name="ch2Image">Image applied to <paramref name="mask"/> channel 2 area.</param>
        /// <param name="ch3Image">Image applied to <paramref name="mask"/> channel 3 area.</param>
        /// <param name="ch4Image">Image applied to <paramref name="mask"/> channel 4 area.</param>
        public void MaskedApplyImage(DdsFile mask, Image ch1Image, Image ch2Image, Image ch3Image, Image ch4Image)
        {
            this.MaskedApplyImage(mask,
                (ch1Image == null) ? null : new Bitmap(ch1Image),
                (ch2Image == null) ? null : new Bitmap(ch2Image),
                (ch3Image == null) ? null : new Bitmap(ch3Image),
                (ch4Image == null) ? null : new Bitmap(ch4Image));
        }

        /// <summary>
        /// Use the <paramref name="mask"/> to apply the supplied bitmaps.
        /// </summary>
        /// <param name="mask">Determines to which areas each image is applied.</param>
        /// <param name="ch1Bitmap">Bitmap applied to <paramref name="mask"/> channel 1 area.</param>
        /// <param name="ch2Bitmap">Bitmap applied to <paramref name="mask"/> channel 2 area.</param>
        /// <param name="ch3Bitmap">Bitmap applied to <paramref name="mask"/> channel 3 area.</param>
        /// <param name="ch4Bitmap">Bitmap applied to <paramref name="mask"/> channel 4 area.</param>
        public void MaskedApplyImage(DdsFile mask, Bitmap ch1Bitmap, Bitmap ch2Bitmap, Bitmap ch3Bitmap, Bitmap ch4Bitmap)
        {
            this.MaskedApplyImage(mask,
                (ch1Bitmap == null) ? null : ch1Bitmap.ToARGBData(), (ch1Bitmap == null) ? Size.Empty : ch1Bitmap.Size,
                (ch2Bitmap == null) ? null : ch2Bitmap.ToARGBData(), (ch2Bitmap == null) ? Size.Empty : ch2Bitmap.Size,
                (ch3Bitmap == null) ? null : ch3Bitmap.ToARGBData(), (ch3Bitmap == null) ? Size.Empty : ch3Bitmap.Size,
                (ch4Bitmap == null) ? null : ch4Bitmap.ToARGBData(), (ch4Bitmap == null) ? Size.Empty : ch4Bitmap.Size);
        }

        void MaskedApplyImage(DdsFile mask,
            uint[] ch1image, Size ch1imageSize, uint[] ch2image, Size ch2imageSize, uint[] ch3image, Size ch3imageSize, uint[] ch4image, Size ch4imageSize)
        {

            maskInEffect = maskInEffect || ch1image != null || ch2image != null || ch3image != null || ch4image != null;

            if (!maskInEffect) return;

            if (ch1image != null) MaskedApplyImage(mask, x => x.R() > 0, ch1image, ch1imageSize);
            if (ch2image != null) MaskedApplyImage(mask, x => x.G() > 0, ch2image, ch2imageSize);
            if (ch3image != null) MaskedApplyImage(mask, x => x.B() > 0, ch3image, ch3imageSize);
            if (ch4image != null) MaskedApplyImage(mask, x => x.A() > 0, ch4image, ch4imageSize);

            if (SupportsHSV) UpdateHSVData();
        }
        void MaskedApplyImage(DdsFile mask, Channel channel, uint[] image, Size imageSize)
        {
            MaskedApply(this.currentImage, this.Size, mask.currentImage, mask.Size, channel, (x, y) => image[((y % imageSize.Height) * imageSize.Width) + (x % imageSize.Width)]);
        }

        /// <summary>
        /// Delegate to determine whether a channel of a given UInt32 ARGB format value is active.
        /// </summary>
        /// <param name="argb">The UInt32 ARGB format value.</param>
        /// <returns>True if the channel is active, otherwise false.</returns>
        delegate bool Channel(uint argb);

        /// <summary>
        /// Return the UInt32 ARGB format pixel value for a given X/Y coordinate.
        /// </summary>
        /// <param name="x">X coordinate.</param>
        /// <param name="y">Y coordinate.</param>
        /// <returns>A UInt32 ARGB format pixel value.</returns>
        delegate uint ARGBAt(int x, int y);

        void MaskedApply(uint[] image, Size imageSize, uint[] mask, Size maskSize, Channel channel, ARGBAt argbAt)
        {
            for (int y = 0; y < imageSize.Height; y++)
            {
                int imageOffset = y * imageSize.Width;
                int maskOffset = (y % maskSize.Height) * maskSize.Width;

                for (int x = 0; x < imageSize.Width; x++)
                {
                    uint imagePixel = image[imageOffset + x];
                    uint maskPixel = mask[maskOffset + x % maskSize.Width];
                    if (channel(maskPixel)) image[imageOffset + x] = argbAt(x, y);
                }
            }
        }

        //----------------------------------------------------------------

        bool maskInEffect = false;

        /// <summary>
        /// Clears a previously-applied HSVShift mask.
        /// </summary>
        public void ClearMask()
        {
            if (!maskInEffect) return;
            currentImage = (uint[])baseImage.Clone();
            if (SupportsHSV) UpdateHSVData();
            maskInEffect = false;
        }

        /// <summary>
        /// Apply <see cref="HSVShift"/> values to this DDS image based on the
        /// channels in the <paramref name="mask"/>.
        /// </summary>
        /// <param name="mask">A DDS image file, each colourway acting as a mask channel.</param>
        /// <param name="ch1Shift">A shift to apply to the image when the first channel of the mask is active.</param>
        /// <param name="ch2Shift">A shift to apply to the image when the second channel of the mask is active.</param>
        /// <param name="ch3Shift">A shift to apply to the image when the third channel of the mask is active.</param>
        /// <param name="ch4Shift">A shift to apply to the image when the fourth channel of the mask is active.</param>
        public void MaskedHSVShift(DdsFile mask, HSVShift ch1Shift, HSVShift ch2Shift, HSVShift ch3Shift, HSVShift ch4Shift)
        {
            if (!SupportsHSV) return;

            maskInEffect = maskInEffect || !ch1Shift.IsEmpty || !ch2Shift.IsEmpty || !ch3Shift.IsEmpty || !ch4Shift.IsEmpty;

            if (!maskInEffect) return;

            if (!ch1Shift.IsEmpty) MaskedHSVShift(mask, hsvData, x => x.R() > 0, ch1Shift);
            if (!ch2Shift.IsEmpty) MaskedHSVShift(mask, hsvData, x => x.G() > 0, ch2Shift);
            if (!ch3Shift.IsEmpty) MaskedHSVShift(mask, hsvData, x => x.B() > 0, ch3Shift);
            if (!ch4Shift.IsEmpty) MaskedHSVShift(mask, hsvData, x => x.A() > 0, ch4Shift);

            currentImage = ColorHSVA.ToArrayARGB(hsvData);
        }

        /// <summary>
        /// Apply <see cref="HSVShift"/> values to this DDS image based on the
        /// channels in the <paramref name="mask"/>.
        /// Each channel of the mask acts independently, in order "R", "G", "B", "A".
        /// </summary>
        /// <param name="mask">A DDS image file, each colourway acting as a mask channel.</param>
        /// <param name="ch1Shift">A shift to apply to the image when the first channel of the mask is active.</param>
        /// <param name="ch2Shift">A shift to apply to the image when the second channel of the mask is active.</param>
        /// <param name="ch3Shift">A shift to apply to the image when the third channel of the mask is active.</param>
        /// <param name="ch4Shift">A shift to apply to the image when the fourth channel of the mask is active.</param>
        public void MaskedHSVShiftNoBlend(DdsFile mask, HSVShift ch1Shift, HSVShift ch2Shift, HSVShift ch3Shift, HSVShift ch4Shift)
        {
            if (!SupportsHSV) return;

            maskInEffect = maskInEffect || !ch1Shift.IsEmpty || !ch2Shift.IsEmpty || !ch3Shift.IsEmpty || !ch4Shift.IsEmpty;

            if (!maskInEffect) return;

            ColorHSVA[] result = new ColorHSVA[hsvData.Length];
            Array.Copy(hsvData, 0, result, 0, result.Length);

            if (!ch1Shift.IsEmpty) MaskedHSVShift(mask, result, x => x.R() > 0, ch1Shift);
            if (!ch2Shift.IsEmpty) MaskedHSVShift(mask, result, x => x.G() > 0, ch2Shift);
            if (!ch3Shift.IsEmpty) MaskedHSVShift(mask, result, x => x.B() > 0, ch3Shift);
            if (!ch4Shift.IsEmpty) MaskedHSVShift(mask, result, x => x.A() > 0, ch4Shift);

            hsvData = result;
            currentImage = ColorHSVA.ToArrayARGB(hsvData);
        }

        void MaskedHSVShift(DdsFile mask, ColorHSVA[] result, Channel channel, HSVShift hsvShift)
        {
            for (int y = 0; y < this.Size.Height; y++)
            {
                int imageOffset = y * this.Size.Width;
                int maskOffset = (y % mask.Size.Height) * mask.Size.Width;

                for (int x = 0; x < this.Size.Width; x++)
                {
                    uint maskPixel = mask.currentImage[maskOffset + x % mask.Size.Width];
                    if (channel(maskPixel)) result[imageOffset + x] = hsvData[imageOffset + x].HSVShift(hsvShift);
                }
            }
        }

        #endregion
    }

    /// <summary>
    /// Extensions for working with bitmap images.
    /// </summary>
    public static class Extensions
    {
        /// <summary>
        /// Convert an array of UInt32 ARGB elements into a <see cref="T:Bitmap"/>.
        /// </summary>
        /// <param name="argbData">The array of UInt32 ARGB elements to decode.</param>
        /// <param name="size">The size of the encoded image.</param>
        /// <returns>The decoded image.</returns>
        public static Bitmap ToBitmap(this uint[] argbData, Size size) { return argbData.ToBitmap(size.Width, size.Height); }
        /// <summary>
        /// Convert an array of UInt32 ARGB elements into a <see cref="T:Bitmap"/>.
        /// </summary>
        /// <param name="argbData">The array of UInt32 ARGB elements to decode.</param>
        /// <param name="width">The width of the encoded image.</param>
        /// <param name="height">The height of the encoded image.</param>
        /// <returns>The decoded bitmap image.</returns>
        public static Bitmap ToBitmap(this uint[] argbData, int width, int height)
        {
            Bitmap res = new Bitmap(width, height, Imaging.PixelFormat.Format32bppArgb);
            for (int y = 0; y < height; y++)
            {
                int offset = y * width;
                for (int x = 0; x < width; x++)
                    res.SetPixel(x, y, Color.FromArgb((int)argbData[offset + x]));
            }
            return res;
        }

        /// <summary>
        /// Converts a <see cref="T:Image"/> into an array of UInt32 ARGB elements.
        /// </summary>
        /// <param name="image">The image to encode.</param>
        /// <returns>An array of UInt32 ARGB elements.</returns>
        public static uint[] ToARGBData(this Image image) { return new Bitmap(image).ToARGBData(); }
        /// <summary>
        /// Converts a <see cref="T:Bitmap"/> into an array of UInt32 ARGB elements.
        /// </summary>
        /// <param name="bitmap">The bitmap image to encode.</param>
        /// <returns>An array of UInt32 ARGB elements.</returns>
        public static uint[] ToARGBData(this Bitmap bitmap) { return bitmap.ToPixelData(x => (uint)x.ToArgb()); }

        /// <summary>
        /// Converts a <see cref="T:Bitmap"/> into a pixel data array,
        /// using the provided encoder.
        /// </summary>
        /// <param name="bitmap">The bitmap image to encode.</param>
        /// <param name="encoder">The method to invoke to encode bitmap <see cref="T:Color"/> pixels.</param>
        /// <returns>An array of uint elements containing the encoded pixel data.</returns>
        public static uint[] ToPixelData(this Bitmap bitmap, Func<Color, uint> encoder)
        {
            uint[] pixelData = new uint[bitmap.Width * bitmap.Height];
            for (int y = 0; y < bitmap.Height; y++)
            {
                int offset = y * bitmap.Width;
                for (int x = 0; x < bitmap.Width; x++)
                    pixelData[offset + x] = encoder(bitmap.GetPixel(x, y));
            }
            return pixelData;
        }

        /// <summary>
        /// Extract the alpha channel from an ARGB format UInt32 value.
        /// </summary>
        /// <param name="argb">An ARGB format UInt32 value.</param>
        /// <returns>The alpha channel extracted.</returns>
        public static uint A(this uint argb) { return (argb & 0xFF000000) >> 24; }
        /// <summary>
        /// Extract the red channel from an ARGB format UInt32 value.
        /// </summary>
        /// <param name="argb">An ARGB format UInt32 value.</param>
        /// <returns>The red channel extracted.</returns>
        public static uint R(this uint argb) { return (argb & 0x00FF0000) >> 16; }
        /// <summary>
        /// Extract the green channel from an ARGB format UInt32 value.
        /// </summary>
        /// <param name="argb">An ARGB format UInt32 value.</param>
        /// <returns>The green channel extracted.</returns>
        public static uint G(this uint argb) { return (argb & 0x0000FF00) >> 8; }
        /// <summary>
        /// Extract the blue channel from an ARGB format UInt32 value.
        /// </summary>
        /// <param name="argb">An ARGB format UInt32 value.</param>
        /// <returns>The blue channel extracted.</returns>
        public static uint B(this uint argb) { return (argb & 0x000000FF) >> 0; }

        /// <summary>
        /// Count the number of set bits in <paramref name="value"/>.
        /// </summary>
        /// <param name="value">A uint value to have bits counted.</param>
        /// <returns>The number of set bits in <paramref name="value"/>.</returns>
        public static int Bits(this uint value)
        {
            int bits = 0;
            for (ulong i = 1; i < uint.MaxValue; i <<= 1)
                if ((value & i) != 0)
                    bits++;
            return bits;
        }
    }
}