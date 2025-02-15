using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;
using System.Security.Permissions;

namespace Rhino.Display
{
  /// <summary>
  /// Represents a sRGBA (Red, Green, Blue, Alpha) color with double precision floating point channel.
  /// </summary>
  [StructLayout(LayoutKind.Sequential, Pack = 8, Size = 32)]
  [DebuggerDisplay("R={R}, G={G}, B={B}, A={A}")]
  [Serializable]
  public struct ColorRGBA : ISerializable, IFormattable, IComparable, IComparable<ColorRGBA>, IEquatable<ColorRGBA>, IEpsilonComparable<ColorRGBA>
  {
    #region members
    private double _r;
    private double _g;
    private double _b;
    private double _a;
    #endregion

    #region properties
    /// <summary>
    /// Gets or sets the red channel value. 
    /// Red channels are limited to a 0~1 range.
    /// </summary>
    /// <since>7.0</since>
    public double R
    {
      get => _r;
      set => _r = RhinoMath.Clamp(value, 0.0, 1.0);
    }

    /// <summary>
    /// Gets or sets the green channel value. 
    /// Green channels are limited to a 0~1 range.
    /// </summary>
    /// <since>7.0</since>
    public double G
    {
      get => _g;
      set => _g = RhinoMath.Clamp(value, 0.0, 1.0);
    }

    /// <summary>
    /// Gets or sets the blue channel value. 
    /// Blue channels are limited to a 0~1 range.
    /// </summary>
    /// <since>7.0</since>
    public double B
    {
      get => _b;
      set => _b = RhinoMath.Clamp(value, 0.0, 1.0);
    }

    /// <summary>
    /// Gets or sets the alpha channel value. 
    /// Alpha channels are limited to a 0~1 range.
    /// </summary>
    /// <since>7.0</since>
    public double A
    {
      get => _a;
      set => _a = RhinoMath.Clamp(value, 0.0, 1.0);
    }
    #endregion

    #region constructors
    /// <since>7.0</since>
    public ColorRGBA(ColorRGBA color)
    {
      _r = color._r;
      _g = color._g;
      _b = color._b;
      _a = color._a;
    }

    /// <since>8.0</since>
    public ColorRGBA(ColorRGBA color, double alpha)
    {
      _r = color._r;
      _g = color._g;
      _b = color._b;
      _a = alpha;
    }

    /// <since>7.0</since>
    public ColorRGBA(double red, double green, double blue)
    {
      _r = RhinoMath.Clamp(red, 0.0, 1.0);
      _g = RhinoMath.Clamp(green, 0.0, 1.0);
      _b = RhinoMath.Clamp(blue, 0.0, 1.0);
      _a = 1.0;
    }

    /// <since>7.0</since>
    public ColorRGBA(double red, double green, double blue, double alpha)
    {
      _r = RhinoMath.Clamp(red, 0.0, 1.0);
      _g = RhinoMath.Clamp(green, 0.0, 1.0);
      _b = RhinoMath.Clamp(blue, 0.0, 1.0);
      _a = RhinoMath.Clamp(alpha, 0.0, 1.0);
    }

    /// <summary>
    /// Constructs a new instance of ColorRGBA that is equivalent to an ARGB color.
    /// </summary>
    /// <param name="color">ARGB color to mimic.</param>
    /// <since>7.0</since>
    public ColorRGBA(System.Drawing.Color color)
    {
      _r = color.R / 255.0;
      _g = color.G / 255.0;
      _b = color.B / 255.0;
      _a = color.A / 255.0;
    }

    /// <summary>
    /// Constructs a new instance of ColorRGBA that is equivalent to an ARGB color.
    /// </summary>
    /// <param name="argb">ARGB color to mimic.</param>
    /// <since>7.0</since>
    public ColorRGBA(int argb)
    {
      _b = ((argb >> 0) & 255) / 255.0;
      _g = ((argb >> 8) & 255) / 255.0;
      _r = ((argb >> 16) & 255) / 255.0;
      _a = ((argb >> 24) & 255) / 255.0;
    }
    #endregion

    #region conversions
    /// <summary>
    /// Converts this color to a 32-bit ARGB value.
    /// </summary>
    /// <returns>The 32-bit ARGB value that corresponds to this color</returns>
    /// <since>7.35</since>
    public int ToArgb()
    {
      return (int)((uint)(B * byte.MaxValue) | (uint)(G * byte.MaxValue) << 8 | (uint)(R * byte.MaxValue) << 16 | (uint)(A * byte.MaxValue) << 24);
    }

    /// <summary>
    /// Constructs a RGBA color from the specified 8-bit components (red, green, and blue) values.
    /// The alpha value is implicitly 1.0 (fully opaque).
    /// </summary>
    /// <param name="red">The red component. Valid values are 0 through 255.</param>
    /// <param name="green">The green component. Valid values are 0 through 255.</param>
    /// <param name="blue">The blue component. Valid values are 0 through 255.</param>
    /// <returns>A RGBA color with the specified component values.</returns>
    /// <since>8.0</since>
    public static ColorRGBA CreateFromArgb(byte red, byte green, byte blue)
    {
      return new ColorRGBA(red / 255.0, green / 255.0, blue / 255.0, 1.0);
    }
    
    /// <summary>
    /// Constructs a RGBA color from the four ARGB components (alpha, red, green, and blue) values.
    /// </summary>
    /// <param name="alpha">The alpha component. Valid values are 0 through 255.</param>
    /// <param name="red">The red component. Valid values are 0 through 255.</param>
    /// <param name="green">The green component. Valid values are 0 through 255.</param>
    /// <param name="blue">The blue component. Valid values are 0 through 255.</param>
    /// <returns>A RGBA color with the specified component values.</returns>
    /// <since>8.0</since>
    public static ColorRGBA CreateFromArgb(byte alpha, byte red, byte green, byte blue)
    {
      return new ColorRGBA(red / 255.0, green / 255.0, blue / 255.0, alpha / 255.0);
    }

    /// <summary>
    /// Constructs the nearest RGBA equivalent of an HSL color.
    /// </summary>
    /// <param name="hsv">Target color in HSL space.</param>
    /// <returns>The RGBA equivalent of the HSL color.</returns>
    /// <since>7.0</since>
    public static ColorRGBA CreateFromHSV(ColorHSV hsv)
    {
      ColorConverter.HSV_To_RGB(hsv.H, hsv.S, hsv.V, out double r, out double g, out double b);
      return new ColorRGBA(r, g, b, hsv.A);
    }

    /// <summary>
    /// Constructs the nearest RGBA equivalent of an HSV color.
    /// </summary>
    /// <param name="hsl">Target color in HSL space.</param>
    /// <returns>The RGBA equivalent of the HSL color.</returns>
    /// <since>7.0</since>
    public static ColorRGBA CreateFromHSL(ColorHSL hsl)
    {
      ColorConverter.HSL_To_RGB(hsl.H, hsl.S, hsl.L, out double r, out double g, out double b);
      return new ColorRGBA(r, g, b, hsl.A);
    }

    /// <summary>
    /// Create the nearest RGBA equivalent of a CMYK color.
    /// </summary>
    /// <param name="cmyk">Target color in CMYK space.</param>
    /// <returns>The RGBA equivalent of the CMYK color.</returns>
    /// <since>7.0</since>
    public static ColorRGBA CreateFromCMYK(ColorCMYK cmyk)
    {
      ColorConverter.CMYK_To_CMY(cmyk.m_c, cmyk.m_m, cmyk.m_y, cmyk.m_k, out double c0, out double m0, out double y0);
      return new ColorRGBA(1.0 - c0, 1.0 - m0, 1.0 - y0, cmyk.A);
    }

    /// <summary>
    /// Create the nearest RGBA equivalent of an XYZ color.
    /// </summary>
    /// <param name="xyz">Target color in XYZ space.</param>
    /// <returns>The RGBA equivalent of the XYZ color.</returns>
    /// <since>7.0</since>
    public static ColorRGBA CreateFromXYZ(ColorXYZ xyz)
    {
      ColorConverter.XYZ_To_RGB(xyz.X, xyz.Y, xyz.Z, out double r, out double g, out double b);
      return new ColorRGBA(r, g, b, xyz.A);
    }

    /// <summary>
    /// Create the nearest RGBA equivalent of a LAB color.
    /// </summary>
    /// <param name="lab">Target color in LAB space.</param>
    /// <returns>The RGBA equivalent of the LAB color.</returns>
    /// <since>7.0</since>
    public static ColorRGBA CreateFromLAB(ColorLAB lab)
    {
      return CreateFromXYZ(ColorXYZ.CreateFromLAB(lab));
    }

    /// <summary>
    /// Create the nearest RGBA equivalent of a LCH color.
    /// </summary>
    /// <param name="lch">Target color in LCH space.</param>
    /// <returns>The RGBA equivalent of the LCH color.</returns>
    /// <since>7.0</since>
    public static ColorRGBA CreateFromLCH(ColorLCH lch)
    {
      return CreateFromLAB(ColorLAB.CreateFromLCH(lch));
    }
    #endregion

    #region ISerializable
    private ColorRGBA(SerializationInfo info, StreamingContext context)
    {
      _r = info.GetDouble("R");
      _g = info.GetDouble("G");
      _b = info.GetDouble("B");
      _a = info.GetDouble("A");
    }

    [SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.SerializationFormatter)]
    void ISerializable.GetObjectData(SerializationInfo info, StreamingContext context)
    {
      info.AddValue("R", _r);
      info.AddValue("G", _g);
      info.AddValue("B", _b);
      info.AddValue("A", _a);
    }
    #endregion

    #region IFormattable
    /// <since>7.0</since>
    public override string ToString() => ToString(null, System.Globalization.CultureInfo.InvariantCulture);

    /// <since>7.0</since>
    public string ToString(string format, IFormatProvider formatProvider)
    {
      char separator = ',';
      if (System.Globalization.NumberFormatInfo.GetInstance(formatProvider) is System.Globalization.NumberFormatInfo info)
      {
        if (info.NumberDecimalSeparator.Length > 0 && separator == info.NumberDecimalSeparator[0])
          separator = ';';
      }

      return string.Format(formatProvider, "{1:" + format + "}{0}{2:" + format + "}{0}{3:" + format + "}{0}{4:" + format + "}", separator, _r, _g, _b, _a);
    }
    #endregion

    #region IComparable
    /// <summary>
    /// Compares this <see cref="ColorRGBA" /> with another <see cref="ColorRGBA" />.
    /// <para>Channels evaluation priority is first A, then R, then G, then B.</para>
    /// </summary>
    /// <param name="other">The other <see cref="ColorRGBA" /> to use in comparison.</param>
    /// <returns>
    /// <para> 0: if this is identical to other</para>
    /// <para>-1: if this &lt; other</para>
    /// <para>+1: if this &gt; other</para>
    /// </returns>
    /// <since>7.0</since>
    public int CompareTo(ColorRGBA other)
    {
      if (_a < other._a)
        return -1;
      if (_a > other._a)
        return 1;

      if (_r < other._r)
        return -1;
      if (_r > other._r)
        return 1;

      if (_g < other._g)
        return -1;
      if (_g > other._g)
        return 1;

      if (_b < other._b)
        return -1;
      if (_b > other._b)
        return 1;

      return 0;
    }

    int IComparable.CompareTo(object obj)
    {
      if (obj is ColorRGBA)
        return CompareTo((ColorRGBA)obj);

      throw new ArgumentException("Input must be of type ColorRGBA", "obj");
    }
    #endregion

    #region IEpsilonComparable
    /// <summary>
    /// Check that all values in other are within epsilon of the values in this
    /// </summary>
    /// <param name="other"></param>
    /// <param name="epsilon"></param>
    /// <returns></returns>
    /// <since>7.0</since>
    public bool EpsilonEquals(ColorRGBA other, double epsilon)
    {
      return RhinoMath.EpsilonEquals(_r, other._r, epsilon) &&
             RhinoMath.EpsilonEquals(_g, other._g, epsilon) &&
             RhinoMath.EpsilonEquals(_b, other._b, epsilon) &&
             RhinoMath.EpsilonEquals(_a, other._a, epsilon);
    }
    #endregion

    #region IEquatable
    /// <summary>
    /// Determines whether the specified ColorRGBA has the same values as the present color.
    /// </summary>
    /// <param name="other">The specified color.</param>
    /// <returns>true if color has the same channel values as this; otherwise false.</returns>
    /// <since>7.0</since>
    public bool Equals(ColorRGBA other)
    {
      return this == other;
    }

    /// <since>7.0</since>
    public override bool Equals(object obj)
    {
      return obj is ColorRGBA d && this == d;
    }

    /// <since>7.0</since>
    public override int GetHashCode()
    {
      // MSDN docs recommend XOR'ing the internal values to get a hash code
      return _r.GetHashCode() ^ _g.GetHashCode() ^ _b.GetHashCode() ^ _a.GetHashCode();
    }
    #endregion

    #region static properties
    /// <since>7.0</since>
    public static ColorRGBA Black => new ColorRGBA(0.0, 0.0, 0.0, 1.0);

    /// <since>7.0</since>
    public static ColorRGBA White => new ColorRGBA(1.0, 1.0, 1.0, 1.0);

    /// <since>7.0</since>
    public static ColorRGBA Red => new ColorRGBA(1.0, 0.0, 0.0, 1.0);

    /// <since>7.0</since>
    public static ColorRGBA Green => new ColorRGBA(0.0, 1.0, 0.0, 1.0);

    /// <since>7.0</since>
    public static ColorRGBA Blue => new ColorRGBA(0.0, 0.0, 1.0, 1.0);
    #endregion

    #region operators
    /// <since>7.0</since>
    public static bool operator ==(ColorRGBA a, ColorRGBA b)
    {
      return (a._r == b._r && a._g == b._g && a._b == b._b && a._a == b._a);
    }

    /// <since>7.0</since>
    public static bool operator !=(ColorRGBA a, ColorRGBA b)
    {
      return (a._r != b._r || a._g != b._g || a._b != b._b || a._a != b._a);
    }
    #endregion

    #region methods
    /// <since>7.0</since>
    public ColorRGBA BlendTo(ColorRGBA col, double coefficient)
    {
      double r = this._r + (coefficient * (col._r - this._r));
      double g = this._g + (coefficient * (col._g - this._g));
      double b = this._b + (coefficient * (col._b - this._b));
      double a = this._a + (coefficient * (col._a - this._a));

      return new ColorRGBA(r, g, b, a);
    }

    /// <since>7.0</since>
    public static ColorRGBA ApplyGamma(ColorRGBA col, double gamma)
    {
      if (Math.Abs(gamma - 1.0f) > double.Epsilon)
      {
        double r = (double)Math.Pow(col._r, gamma);
        double g = (double)Math.Pow(col._g, gamma);
        double b = (double)Math.Pow(col._b, gamma);

        return new ColorRGBA(r, g, b, col._a);
      }

      return col;
    }
    #endregion

    #region casting operators
    /// <summary>
    /// Converts a ColorRGBA in a <see cref="System.Drawing.Color"/>.
    /// </summary>
    /// <param name="value">A RGBA color.</param>
    /// <returns>An ARGB <see cref="System.Drawing.Color"/>.</returns>
    /// <since>7.0</since>
    public static explicit operator System.Drawing.Color(ColorRGBA value)
    {
      var r = (int) Math.Round(value._r * 255.0);
      var g = (int) Math.Round(value._g * 255.0);
      var b = (int) Math.Round(value._b * 255.0);
      var a = (int) Math.Round(value._a * 255.0);

      return System.Drawing.Color.FromArgb(a, r, g, b);
    }

    /// <since>7.0</since>
    public static implicit operator ColorRGBA(System.Drawing.Color value) => new ColorRGBA(value);
    #endregion
  }

  /// <summary>
  /// Represents an HSL (Hue, Saturation, Luminance) color with double precision floating point channels. 
  /// HSL colors are used primarily in Graphical User Interface environments as they provide a 
  /// very natural approach to picking colors.
  /// </summary>
  [Serializable]
  public struct ColorHSL
  {
    #region members
    /// <summary>
    /// Alpha values are inverted internally! 0 = opaque; 1 = transparent.
    /// </summary>
    internal double m_a;
    internal double m_h;
    internal double m_s;
    internal double m_l;
    #endregion

    #region constructors
    /// <summary>
    /// Constructs a new instance of ColorHSL that is equivalent to an ARGB color.
    /// </summary>
    /// <param name="rgb">ARGB color to mimic.</param>
    /// <remarks>Exact conversions between color spaces are often not possible.</remarks>
    /// <since>5.0</since>
    public ColorHSL(System.Drawing.Color rgb)
    {
      m_a = 1.0 - (rgb.A / 255.0);
      ColorConverter.RGB_To_HSL(rgb.R, rgb.G, rgb.B, out m_h, out m_s, out m_l);
    }
    /// <summary>
    /// Constructs a new instance of ColorHSL with custom channel values.
    /// </summary>
    /// <param name="hue">Hue channel value. Hue channels rotate between 0.0 and 1.0.</param>
    /// <param name="saturation">Saturation channel value. Channel will be limited to 0~1.</param>
    /// <param name="luminance">Luminance channel value. Channel will be limited to 0~1.</param>
    /// <since>5.0</since>
    public ColorHSL(double hue, double saturation, double luminance)
    {
      m_h = (float)hue;
      m_s = Clip(saturation);
      m_l = Clip(luminance);
      m_a = 0.0f;
    }
    /// <summary>
    /// Constructs a new instance of ColorHSL with custom channel values.
    /// </summary>
    /// <param name="alpha">Alpha channel value. Channel will be limited to 0~1.</param>
    /// <param name="hue">Hue channel value. Hue channels rotate between 0.0 and 1.0.</param>
    /// <param name="saturation">Saturation channel value. Channel will be limited to 0~1.</param>
    /// <param name="luminance">Luminance channel value. Channel will be limited to 0~1.</param>
    /// <since>5.0</since>
    public ColorHSL(double alpha, double hue, double saturation, double luminance)
    {
      m_h = (float)hue;
      m_s = Clip(saturation);
      m_l = Clip(luminance);
      m_a = 1.0 - Clip(alpha);
    }

    /// <summary>
    /// Create the nearest HSL equivalent of a RGBA color.
    /// </summary>
    /// <param name="rgba">Target color in RGBA space.</param>
    /// <returns>The HSL equivalent of the RGBA color.</returns>
    /// <since>7.0</since>
    public static ColorHSL CreateFromRGBA(ColorRGBA rgba)
    {
      ColorConverter.RGB_To_HSL(rgba.R, rgba.G, rgba.B, out var h, out var s, out var l);
      return new ColorHSL(rgba.A, h, s, l);
    }
    /// <summary>
    /// Create the nearest HSL equivalent of a CMYK color.
    /// </summary>
    /// <param name="cmyk">Target color in CMYK space.</param>
    /// <returns>The HSL equivalent of the CMYK color.</returns>
    /// <since>5.0</since>
    public static ColorHSL CreateFromCMYK(ColorCMYK cmyk)
    {
      return CreateFromRGBA(ColorRGBA.CreateFromCMYK(cmyk));
    }
    /// <summary>
    /// Create the nearest HSL equivalent of an XYZ color.
    /// </summary>
    /// <param name="xyz">Target color in XYZ space.</param>
    /// <returns>The HSL equivalent of the XYZ color.</returns>
    /// <since>5.0</since>
    public static ColorHSL CreateFromXYZ(ColorXYZ xyz)
    {
      return CreateFromRGBA(ColorRGBA.CreateFromXYZ(xyz));
    }
    /// <summary>
    /// Create the nearest HSL equivalent of a LAB color.
    /// </summary>
    /// <param name="lab">Target color in LAB space.</param>
    /// <returns>The HSL equivalent of the LAB color.</returns>
    /// <since>5.0</since>
    public static ColorHSL CreateFromLAB(ColorLAB lab)
    {
      return CreateFromRGBA(ColorRGBA.CreateFromLAB(lab));
    }
    /// <summary>
    /// Create the nearest HSL equivalent of a LCH color.
    /// </summary>
    /// <param name="lch">Target color in LCH space.</param>
    /// <returns>The HSL equivalent of the LCH color.</returns>
    /// <since>5.0</since>
    public static ColorHSL CreateFromLCH(ColorLCH lch)
    {
      return CreateFromRGBA(ColorRGBA.CreateFromLCH(lch));
    }
    /// <summary>
    /// Constructs the nearest HSL equivalent of an HSV color.
    /// </summary>
    /// <param name="hsv">Target color in HSV space.</param>
    /// <returns>The HSL equivalent of the HSV color.</returns>
    /// <since>6.0</since>
    public static ColorHSL CreateFromHSV(ColorHSV hsv)
    {
      return CreateFromRGBA(ColorRGBA.CreateFromHSV(hsv));
    }

    #endregion

    #region operators

    /// <summary>
    /// Implicitly converts a ColorHSL in a .Net library color.
    /// </summary>
    /// <param name="hsl">A HSL color.</param>
    /// <returns>A ARGB .Net library color.</returns>
    public static implicit operator System.Drawing.Color(ColorHSL hsl)
    {
      return (System.Drawing.Color)ColorRGBA.CreateFromHSL(hsl);
    }
    #endregion

    #region properties
    /// <summary>
    /// Gets or sets the hue channel value. 
    /// Hue channels rotate between 0.0 and 1.0.
    /// </summary>
    /// <since>5.0</since>
    public double H
    {
      get { return m_h; }
      set { m_h = value; } // (value + 1.0F) % 1.0F; }
    }
    /// <summary>
    /// Gets or sets the saturation channel value. 
    /// Saturation channels are limited to a 0~1 range.
    /// </summary>
    /// <since>5.0</since>
    public double S
    {
      get { return m_s; }
      set { m_s = Clip(value); }
    }
    /// <summary>
    /// Gets or sets the luminance channel value. 
    /// Luminance channels are limited to a 0~1 range.
    /// </summary>
    /// <since>5.0</since>
    public double L
    {
      get { return m_l; }
      set { m_l = Clip(value); }
    }
    /// <summary>
    /// Gets or sets the alpha channel value. 
    /// Alpha channels are limited to a 0~1 range.
    /// </summary>
    /// <since>5.0</since>
    public double A
    {
      get { return 1.0 - m_a; }
      set { m_a = 1.0 - Clip(value); }
    }
    #endregion

    #region methods
    internal static double Clip(double n)
    {
      if (n <= 0.0) { return 0.0; }
      if (n >= 1.0) { return 1.0; }
      return n;
    }

    /// <summary>
    /// Convert HSL color to an equivalent System.Drawing.Color.
    /// </summary>
    /// <returns>A .Net framework library color value.</returns>
    /// <since>5.0</since>
    public System.Drawing.Color ToArgbColor() => (System.Drawing.Color)this;

    #endregion
  }

  /// <summary>
  /// Represents a CMYK (Cyan, Magenta, Yellow, Key) color with double precision floating point channels. 
  /// CMYK colors are used primarily in printing environments as they provide a good simulation of physical ink.
  /// </summary>
  [Serializable]
  public struct ColorCMYK
  {
    #region members
    internal double m_c;
    internal double m_m;
    internal double m_y;
    internal double m_k;
    /// <summary>
    /// Alpha values are inverted internally! 0 = opaque; 1 = transparent!
    /// </summary>
    internal double m_a;
    #endregion

    #region constructors
    /// <summary>
    /// Initializes a new instance of ColorCMYK that is equivalent to an ARGB color.
    /// </summary>
    /// <param name="rgb">ARGB color to mimic.</param>
    /// <remarks>Exact conversions between color spaces are often not possible.</remarks>
    /// <since>5.0</since>
    public ColorCMYK(System.Drawing.Color rgb)
    {
      ColorConverter.RGB_To_CMY(rgb.R, rgb.G, rgb.B, out double c0, out double m0, out double y0);
      ColorConverter.CMY_To_CMYK(c0, m0, y0, out m_c, out m_m, out m_y, out m_k);

      m_a = 1.0 - (rgb.A / 255.0);
    }
    /// <summary>
    /// Initializes a new instance of ColorCMYK with custom channel values. 
    /// The cyan, magenta and yellow values will be adjusted based on their 
    /// combined darkness.
    /// </summary>
    /// <param name="cyan">Cyan channel hint.</param>
    /// <param name="magenta">Magenta channel hint.</param>
    /// <param name="yellow">Yellow channel hint.</param>
    /// <since>5.0</since>
    public ColorCMYK(double cyan, double magenta, double yellow)
    {
      m_a = 0.0;
      m_k = 0.0;
      m_c = Clip(cyan);
      m_m = Clip(magenta);
      m_y = Clip(yellow);

      if (m_c < m_k) { m_k = m_c; }
      if (m_m < m_k) { m_k = m_m; }
      if (m_y < m_k) { m_k = m_y; }
      if (m_k == 1.0)
      {
        m_c = 0.0;
        m_m = 0.0;
        m_y = 0.0;
      }
      else
      {
        m_c = (m_c - m_k) / (1.0 - m_k);
        m_m = (m_m - m_k) / (1.0 - m_k);
        m_y = (m_y - m_k) / (1.0 - m_k);
      }
    }
    /// <summary>
    /// Initializes a new instance of ColorCMYK with custom channel values. 
    /// </summary>
    /// <param name="cyan">Cyan channel value. Cyan channels are limited to the 0.0 and 1.0 range.</param>
    /// <param name="magenta">Magenta channel value. Magenta channels are limited to the 0.0 and 1.0 range.</param>
    /// <param name="yellow">Yellow channel value. Yellow channels are limited to the 0.0 and 1.0 range.</param>
    /// <param name="key">Key channel value. Key channels are limited to the 0.0 and 1.0 range.</param>
    /// <since>5.0</since>
    public ColorCMYK(double cyan, double magenta, double yellow, double key)
    {
      m_c = Clip(cyan);
      m_m = Clip(magenta);
      m_y = Clip(yellow);
      m_k = Clip(key);
      m_a = 0.0;
    }

    /// <summary>
    /// Initializes a new instance of ColorCMYK with custom channel values. 
    /// </summary>
    /// <param name="alpha">Alpha channel value. Alpha channels are limited to the 0.0 and 1.0 range.</param>
    /// <param name="cyan">Cyan channel value. Cyan channels are limited to the 0.0 and 1.0 range.</param>
    /// <param name="magenta">Magenta channel value. Magenta channels are limited to the 0.0 and 1.0 range.</param>
    /// <param name="yellow">Yellow channel value. Yellow channels are limited to the 0.0 and 1.0 range.</param>
    /// <param name="key">Key channel value. Key channels are limited to the 0.0 and 1.0 range.</param>
    /// <since>5.0</since>
    public ColorCMYK(double alpha, double cyan, double magenta, double yellow, double key)
    {
      m_c = Clip(cyan);
      m_m = Clip(magenta);
      m_y = Clip(yellow);
      m_k = Clip(key);
      m_a = 1.0 - Clip(alpha);
    }

    /// <summary>
    /// Create the nearest CMYK equivalent of a RGBA color.
    /// </summary>
    /// <param name="rgba">Target color in RGBA space.</param>
    /// <returns>The CMYK equivalent of the RGBA color.</returns>
    /// <since>7.0</since>
    public static ColorCMYK CreateFromRGBA(ColorRGBA rgba)
    {
      ColorConverter.CMY_To_CMYK(1.0 - rgba.R, 1.0 - rgba.G, 1.0 - rgba.B, out var c, out var m, out var y, out var k);
      return new ColorCMYK(rgba.A, c, m, y, k);
    }
    /// <summary>
    /// Constructs the nearest CMYK equivalent of an HSL color.
    /// </summary>
    /// <param name="hsl">Target color in HSL space.</param>
    /// <returns>The CMYK equivalent of the HSL color.</returns>
    /// <since>5.0</since>
    public static ColorCMYK CreateFromHSL(ColorHSL hsl)
    {
      return CreateFromRGBA(ColorRGBA.CreateFromHSL(hsl));
    }
    /// <summary>
    /// Constructs the nearest CMYK equivalent of an XYZ color.
    /// </summary>
    /// <param name="xyz">Target color in XYZ space.</param>
    /// <returns>The CMYK equivalent of the XYZ color.</returns>
    /// <since>5.0</since>
    public static ColorCMYK CreateFromXYZ(ColorXYZ xyz)
    {
      return CreateFromRGBA(ColorRGBA.CreateFromXYZ(xyz));
    }
    /// <summary>
    /// Constructs the nearest CMYK equivalent of a LAB color.
    /// </summary>
    /// <param name="lab">Target color in LAB space.</param>
    /// <returns>The CMYK equivalent of the LAB color.</returns>
    /// <since>5.0</since>
    public static ColorCMYK CreateFromLAB(ColorLAB lab)
    {
      return CreateFromRGBA(ColorRGBA.CreateFromLAB(lab));
    }
    /// <summary>
    /// Constructs the nearest CMYK equivalent of a LCH color.
    /// </summary>
    /// <param name="lch">Target color in LCH space.</param>
    /// <returns>The CMYK equivalent of the LCH color.</returns>
    /// <since>5.0</since>
    public static ColorCMYK CreateFromLCH(ColorLCH lch)
    {
      return CreateFromRGBA(ColorRGBA.CreateFromLCH(lch));
    }
    /// <summary>
    /// Constructs the nearest CMYK equivalent of an HSV color.
    /// </summary>
    /// <param name="hsv">Target color in HSV space.</param>
    /// <returns>The CMYK equivalent of the HSV color.</returns>
    /// <since>6.0</since>
    public static ColorCMYK CreateFromHSV(ColorHSV hsv)
    {
      return CreateFromRGBA(ColorRGBA.CreateFromHSV(hsv));
    }
    #endregion

    #region operators

    /// <summary>
    /// Implicitly converts a CMYK color into a .Net library color.
    /// </summary>
    /// <param name="cmyk">A CMYK color.</param>
    /// <returns>A ARGB .Net library color.</returns>
    public static implicit operator System.Drawing.Color(ColorCMYK cmyk)
    {
      return (System.Drawing.Color) ColorRGBA.CreateFromCMYK(cmyk);
    }
    #endregion

    #region properties
    /// <summary>
    /// Gets or sets the Cyan channel value. 
    /// Cyan channels are limited to the 0~1 range.
    /// </summary>
    /// <since>5.0</since>
    public double C
    {
      get { return m_c; }
      set { m_c = Clip(value); }
    }
    /// <summary>
    /// Gets or sets the Magenta channel value. 
    /// Magenta channels are limited to the 0~1 range.
    /// </summary>
    /// <since>5.0</since>
    public double M
    {
      get { return m_m; }
      set { m_m = Clip(value); }
    }
    /// <summary>
    /// Gets or sets the Yellow channel value. 
    /// Yellow channels are limited to the 0~1 range.
    /// </summary>
    /// <since>5.0</since>
    public double Y
    {
      get { return m_y; }
      set { m_y = Clip(value); }
    }
    /// <summary>
    /// Gets or sets the Key channel value. 
    /// Key channels are limited to the 0~1 range.
    /// </summary>
    /// <since>5.0</since>
    public double K
    {
      get { return m_k; }
      set { m_k = Clip(value); }
    }
    /// <summary>
    /// Gets or sets the Alpha channel value. 
    /// Alpha channels are limited to the 0~1 range.
    /// </summary>
    /// <since>5.0</since>
    public double A
    {
      get { return 1.0 - m_a; }
      set { m_a = 1.0 - Clip(value); }
    }
    #endregion

    #region methods
    internal static double Clip(double n)
    {
      if (n <= 0.0) { return 0.0; }
      if (n >= 1.0) { return 1.0; }
      return n;
    }
    #endregion
  }

  /// <summary>
  /// Represents an XYZ (Hue, Saturation, Luminance) color with double precision floating point channels. 
  /// XYZ colors are based on the CIE 1931 XYZ color space standard and they mimic the natural 
  /// sensitivity of cones in the human retina.
  /// </summary>
  [Serializable]
  public struct ColorXYZ
  {
    #region members
    internal double m_x;
    internal double m_y;
    internal double m_z;
    /// <summary>
    /// Alpha values are inverted internally! 0 = opaque; 1 = transparent!
    /// </summary>
    internal double m_a;
    #endregion

    #region constructors
    /// <summary>
    /// Constructs a new instance of ColorXYZ that is equivalent to an ARGB color.
    /// </summary>
    /// <param name="rgb">ARGB color to mimic.</param>
    /// <remarks>Exact conversions between color spaces are often not possible.</remarks>
    /// <since>5.0</since>
    public ColorXYZ(System.Drawing.Color rgb)
    {
      m_a = 1.0 - (rgb.A / 255.0);
      ColorConverter.RGB_To_XYZ(rgb.R, rgb.G, rgb.B, out m_x, out m_y, out m_z);
    }
    /// <summary>
    /// Constructs a new instance of ColorXYZ with custom channel values.
    /// </summary>
    /// <param name="x">X channel value, channel will be limited to 0~1.</param>
    /// <param name="y">Y channel value, channel will be limited to 0~1.</param>
    /// <param name="z">Z channel value, channel will be limited to 0~1.</param>
    /// <since>5.0</since>
    public ColorXYZ(double x, double y, double z)
    {
      m_x = Clip(x);
      m_y = Clip(y);
      m_z = Clip(z);
      m_a = 0.0;
    }
    /// <summary>
    /// Constructs a new instance of ColorXYZ with custom channel values.
    /// </summary>
    /// <param name="alpha">Alpha channel value, channel will be limited to 0~1.</param>
    /// <param name="x">X channel value, channel will be limited to 0~1.</param>
    /// <param name="y">Y channel value, channel will be limited to 0~1.</param>
    /// <param name="z">Z channel value, channel will be limited to 0~1.</param>
    /// <since>5.0</since>
    public ColorXYZ(double alpha, double x, double y, double z)
    {
      m_x = Clip(x);
      m_y = Clip(y);
      m_z = Clip(z);
      m_a = 1.0 - Clip(alpha);
    }

    /// <summary>
    /// Create the nearest XYZ equivalent of a RGBA color.
    /// </summary>
    /// <param name="rgba">Target color in RGBA space.</param>
    /// <returns>The XYZ equivalent of the RGBA color.</returns>
    /// <since>7.0</since>
    public static ColorXYZ CreateFromRGBA(ColorRGBA rgba)
    {
      ColorConverter.RGB_To_XYZ(rgba.R, rgba.G, rgba.B, out var x, out var y, out var z);
      return new ColorXYZ(rgba.A, x, y, z);
    }
    /// <summary>
    /// Create the nearest XYZ equivalent of an HSL color.
    /// </summary>
    /// <param name="hsl">Target color in HSL space.</param>
    /// <returns>The XYZ equivalent of the HSL color.</returns>
    /// <since>5.0</since>
    public static ColorXYZ CreateFromHSL(ColorHSL hsl)
    {
      return CreateFromRGBA(ColorRGBA.CreateFromHSL(hsl));
    }
    /// <summary>
    /// Create the nearest XYZ equivalent of a CMYK color.
    /// </summary>
    /// <param name="cmyk">Target color in CMYK space.</param>
    /// <returns>The XYZ equivalent of the CMYK color.</returns>
    /// <since>5.0</since>
    public static ColorXYZ CreateFromCMYK(ColorCMYK cmyk)
    {
      return CreateFromRGBA(ColorRGBA.CreateFromCMYK(cmyk));
    }
    /// <summary>
    /// Create the nearest XYZ equivalent of a Lab color.
    /// </summary>
    /// <param name="lab">Target color in LAB space.</param>
    /// <returns>The XYZ equivalent of the LAB color.</returns>
    /// <since>5.0</since>
    public static ColorXYZ CreateFromLAB(ColorLAB lab)
    {
      ColorConverter.CIELAB_To_XYZ(lab.m_l, lab.m_a, lab.m_b, out double x, out double y, out double z);
      return new ColorXYZ(lab.Alpha, x, y, z);
    }
    /// <summary>
    /// Create the nearest XYZ equivalent of an LCH color.
    /// </summary>
    /// <param name="lch">Target color in LCH space.</param>
    /// <returns>The XYZ equivalent of the LCH color.</returns>
    /// <since>5.0</since>
    public static ColorXYZ CreateFromLCH(ColorLCH lch)
    {
      return CreateFromLAB(ColorLAB.CreateFromLCH(lch));
    }

    /// <summary>
    /// Constructs the nearest XYZ equivalent of an HSV color.
    /// </summary>
    /// <param name="hsv">Target color in HSV space.</param>
    /// <returns>The XYZ equivalent of the HSV color.</returns>
    /// <since>6.0</since>
    public static ColorXYZ CreateFromHSV(ColorHSV hsv)
    {
      return CreateFromRGBA(ColorRGBA.CreateFromHSV(hsv));
    }
    #endregion

    #region operators

    /// <summary>
    /// Implicitly converts a XYZ color into a .Net library color.
    /// </summary>
    /// <param name="xyz">A XYZ color.</param>
    /// <returns>A ARGB .Net library color.</returns>
    public static implicit operator System.Drawing.Color(ColorXYZ xyz)
    {
      return (System.Drawing.Color)ColorRGBA.CreateFromXYZ(xyz);
    }
    #endregion

    #region properties
    /// <summary>
    /// Gets or set the X channel value. Channel will be limited to 0~1.
    /// </summary>
    /// <since>5.0</since>
    public double X
    {
      get { return m_x; }
      set { m_x = Clip(value); }
    }
    /// <summary>
    /// Gets or set the Y channel value. Channel will be limited to 0~1.
    /// </summary>
    /// <since>5.0</since>
    public double Y
    {
      get { return m_y; }
      set { m_y = Clip(value); }
    }
    /// <summary>
    /// Gets or set the Z channel value. Channel will be limited to 0~1.
    /// </summary>
    /// <since>5.0</since>
    public double Z
    {
      get { return m_z; }
      set { m_z = Clip(value); }
    }
    /// <summary>
    /// Gets or set the Alpha channel value. Channel will be limited to 0~1.
    /// </summary>
    /// <since>5.0</since>
    public double A
    {
      get { return 1.0 - m_a; }
      set { m_a = 1.0 - Clip(value); }
    }
    #endregion

    #region methods
    internal static double Clip(double n)
    {
      if (n <= 0.0) { return 0.0; }
      if (n >= 1.0) { return 1.0; }
      return n;
    }
    #endregion
  }

  /// <summary>
  /// Represents a LAB (Lightness, A, B) color with double precision floating point channels. 
  /// LAB colors are based on nonlinearly compressed CIE XYZ color space coordinates.  
  /// The A and B parameters of a LAB color represent the opponents.
  /// </summary>
  [Serializable]
  public struct ColorLAB
  {
    #region members
    internal double m_l;
    internal double m_a;
    internal double m_b;
    /// <summary>
    /// Alpha values are inverted internally! 0 = opaque; 1 = transparent!
    /// </summary>
    internal double m_alpha;
    #endregion

    #region constructors
    /// <summary>
    /// Constructs a new instance of ColorLAB that is equivalent to an ARGB color.
    /// </summary>
    /// <param name="rgb">ARGB color to mimic.</param>
    /// <remarks>Exact conversions between color spaces are often not possible.</remarks>
    /// <since>5.0</since>
    public ColorLAB(System.Drawing.Color rgb)
    {
      this = CreateFromXYZ(new ColorXYZ(rgb));
    }
    /// <summary>
    /// Constructs a new instance of ColorLAB with custom channel values.
    /// </summary>
    /// <since>5.0</since>
    public ColorLAB(double lightness, double a, double b)
    {
      m_l = Clip(lightness);
      m_a = ClipAB(a);
      m_b = ClipAB(b);
      m_alpha = 0.0;
    }
    /// <summary>
    /// Constructs a new instance of ColorLAB with custom channel values.
    /// </summary>
    /// <since>5.0</since>
    public ColorLAB(double alpha, double lightness, double a, double b)
    {
      m_l = Clip(lightness);
      m_a = ClipAB(a);
      m_b = ClipAB(b);
      m_alpha = 1.0 - Clip(alpha);
    }

    /// <summary>
    /// Create the nearest LAB equivalent of an RGBA color.
    /// </summary>
    /// <param name="rgb">Target color in RGBA space.</param>
    /// <returns>The LAB equivalent of the RGBA color.</returns>
    /// <since>7.1</since>
    public static ColorLAB CreateFromRGBA(ColorRGBA rgb)
    {
      return CreateFromXYZ(ColorXYZ.CreateFromRGBA(rgb));
    }
    /// <summary>
    /// Create the nearest LAB equivalent of an HSL color.
    /// </summary>
    /// <param name="hsl">Target color in HSL space.</param>
    /// <returns>The LAB equivalent of the HSL color.</returns>
    /// <since>5.0</since>
    public static ColorLAB CreateFromHSL(ColorHSL hsl)
    {
      return CreateFromXYZ(ColorXYZ.CreateFromHSL(hsl));
    }
    /// <summary>
    /// Create the nearest LAB equivalent of a CMYK color.
    /// </summary>
    /// <param name="cmyk">Target color in CMYK space.</param>
    /// <returns>The LAB equivalent of the CMYK color.</returns>
    /// <since>5.0</since>
    public static ColorLAB CreateFromCMYK(ColorCMYK cmyk)
    {
      return CreateFromXYZ(ColorXYZ.CreateFromCMYK(cmyk));
    }
    /// <summary>
    /// Create the nearest LAB equivalent of an XYZ color.
    /// </summary>
    /// <param name="xyz">Target color in XYZ space.</param>
    /// <returns>The LAB equivalent of the XYZ color.</returns>
    /// <since>5.0</since>
    public static ColorLAB CreateFromXYZ(ColorXYZ xyz)
    {
      ColorConverter.XYZ_To_CIELAB(xyz.m_x, xyz.m_y, xyz.m_z, out double l, out double a, out double b);
      return new ColorLAB(xyz.A, l, a, b);
    }
    /// <summary>
    /// Create the nearest LAB equivalent of an LCH color.
    /// </summary>
    /// <param name="lch">Target color in LCH space.</param>
    /// <returns>The LAB equivalent of the LCH color.</returns>
    /// <since>5.0</since>
    public static ColorLAB CreateFromLCH(ColorLCH lch)
    {
      double l, a, b;
      ColorConverter.CIELCH_To_CIELAB(lch.m_l, lch.m_c, lch.m_h, out l, out a, out b);

      ColorLAB lab = new ColorLAB(l, a, b) { m_alpha = lch.m_a };
      return lab;
    }

    /// <summary>
    /// Constructs the nearest LAB equivalent of an HSV color.
    /// </summary>
    /// <param name="hsv">Target color in HSV space.</param>
    /// <returns>The LAB equivalent of the HSV color.</returns>
    /// <since>6.0</since>
    public static ColorLAB CreateFromHSV(ColorHSV hsv)
    {
      return CreateFromXYZ(ColorXYZ.CreateFromHSV(hsv));
    }
    #endregion

    #region operators

    /// <summary>
    /// Implicitly converts a LAB color into a .Net library color.
    /// </summary>
    /// <param name="lab">A LAB color.</param>
    /// <returns>A ARGB .Net library color.</returns>
    public static implicit operator System.Drawing.Color(ColorLAB lab)
    {
      return (System.Drawing.Color) ColorRGBA.CreateFromLAB(lab);
    }
    #endregion

    #region properties
    /// <summary>
    /// Gets or sets the lightness channel. The channel is limited to 0~1.
    /// </summary>
    /// <since>5.0</since>
    public double L
    {
      get { return m_l; }
      set { m_l = Clip(value); }
    }
    /// <summary>
    /// Gets or sets the Base channel. The channel is limited to -1~+1.
    /// </summary>
    /// <since>5.0</since>
    public double A
    {
      get { return m_a; }
      set { m_a = ClipAB(value); }
    }
    /// <summary>
    /// Gets or sets the Opponent channel. The channel is limited to -1~+1.
    /// </summary>
    /// <since>5.0</since>
    public double B
    {
      get { return m_b; }
      set { m_b = ClipAB(value); }
    }
    /// <summary>
    /// Gets or sets the Alpha channel. The channel is limited to 0~1.
    /// </summary>
    /// <since>5.0</since>
    public double Alpha
    {
      get { return 1.0 - m_alpha; }
      set { m_alpha = 1.0 - Clip(value); }
    }
    #endregion

    #region methods
    internal static double Clip(double n)
    {
      if (n < 0.0) { return 0.0; }
      if (n > +1.0) { return +1.0; }
      return n;
    }

    internal static double ClipAB(double n)
    {
      if (n < -1.0) { return -1.0; }
      if (n > +1.0) { return +1.0; }
      return n;
    }
    #endregion
  }

  /// <summary>
  /// Represents an LCH (Lightness, A, B) color with double precision floating point channels. 
  /// LCH colors (also sometimes called CIELUV) are transformation of the 1931 CIE XYZ color space, 
  /// in order to approach perceptual uniformity. They are primarily used in computer graphics which 
  /// deal with colored lights.
  /// </summary>
  [Serializable]
  public struct ColorLCH
  {
    #region members
    internal double m_l;
    internal double m_c;
    internal double m_h;
    /// <summary>
    /// Alpha values are inverted internally! 0 = opaque; 1 = transparent!
    /// </summary>
    internal double m_a;
    #endregion

    #region constructors
    /// <summary>
    /// Constructs a new instance of ColorLCH that is equivalent to an ARGB color.
    /// </summary>
    /// <param name="rgb">ARGB color to mimic.</param>
    /// <remarks>Exact conversions between color spaces are often not possible.</remarks>
    /// <since>5.0</since>
    public ColorLCH(System.Drawing.Color rgb)
    {
      this = CreateFromLAB(new ColorLAB(rgb));
    }
    /// <summary>
    /// Constructs a new instance of ColorLCH with custom channel values.
    /// </summary>
    /// <param name="lightness">Value of lightness channel. This channel is limited to 0~1.</param>
    /// <param name="chroma">Value of chroma channel. This channel is limited to -1~1.</param>
    /// <param name="hue">Value of chroma channel. This channel is limited to 0~360.</param>
    /// <since>5.0</since>
    public ColorLCH(double lightness, double chroma, double hue)
    {
      m_l = ClipL(lightness);
      m_c = ClipC(chroma);
      m_h = ClipH(hue);
      m_a = 0.0;
    }
    /// <summary>
    /// Constructs a new instance of ColorLCH with custom channel values.
    /// </summary>
    /// <param name="alpha">Value of Alpha channel. This channel is limited to 0~1.</param>
    /// <param name="lightness">Value of Lightness channel. This channel is limited to 0~1.</param>
    /// <param name="chroma">Value of Chroma channel. This channel is limited to -1~1.</param>
    /// <param name="hue">Value of Hue channel. This channel is limited to 0~360.</param>
    /// <since>5.0</since>
    public ColorLCH(double alpha, double lightness, double chroma, double hue)
    {
      m_l = ClipL(lightness);
      m_c = ClipC(chroma);
      m_h = ClipH(hue);
      m_a = 1.0 - ClipL(alpha);
    }

    /// <summary>
    /// Create the nearest LCH equivalent of an RGBA color.
    /// </summary>
    /// <param name="rgb">Target color in RGBA space.</param>
    /// <returns>The LCH equivalent of the RGBA color.</returns>
    /// <since>7.1</since>
    public static ColorLCH CreateFromRGBA(ColorRGBA rgb)
    {
      return CreateFromLAB(ColorLAB.CreateFromRGBA(rgb));
    }
    /// <summary>
    /// Create the nearest LCH equivalent of an HSL color.
    /// </summary>
    /// <param name="hsl">Target color in HSL space.</param>
    /// <returns>The LCH equivalent of the HSL color.</returns>
    /// <since>5.0</since>
    public static ColorLCH CreateFromHSL(ColorHSL hsl)
    {
      return CreateFromLAB(ColorLAB.CreateFromHSL(hsl));
    }
    /// <summary>
    /// Create the nearest LCH equivalent of a CMYK color.
    /// </summary>
    /// <param name="cmyk">Target color in CMYK space.</param>
    /// <returns>The LCH equivalent of the CMYK color.</returns>
    /// <since>5.0</since>
    public static ColorLCH CreateFromCMYK(ColorCMYK cmyk)
    {
      return CreateFromLAB(ColorLAB.CreateFromCMYK(cmyk));
    }
    /// <summary>
    /// Create the nearest LCH equivalent of an XYZ color.
    /// </summary>
    /// <param name="xyz">Target color in XYZ space.</param>
    /// <returns>The LCH equivalent of the XYZ color.</returns>
    /// <since>5.0</since>
    public static ColorLCH CreateFromXYZ(ColorXYZ xyz)
    {
      return CreateFromLAB(ColorLAB.CreateFromXYZ(xyz));
    }
    /// <summary>
    /// Create the nearest LCH equivalent of a LAB color.
    /// </summary>
    /// <param name="lab">Target color in LAB space.</param>
    /// <returns>The LCH equivalent of the LAB color.</returns>
    /// <since>5.0</since>
    public static ColorLCH CreateFromLAB(ColorLAB lab)
    {
      ColorConverter.CIELAB_To_CIELCH(lab.m_l, lab.m_a, lab.m_b, out double l, out double c, out double h);
      return new ColorLCH(lab.Alpha, l, c, h);
    }
    #endregion

    #region operators

    /// <summary>
    /// Implicitly converts a LCH color into a .Net library color.
    /// </summary>
    /// <param name="lch">A LCH color.</param>
    /// <returns>A ARGB .Net library color.</returns>
    public static implicit operator System.Drawing.Color(ColorLCH lch)
    {
      return (System.Drawing.Color)ColorRGBA.CreateFromLCH(lch);
    }
    #endregion

    #region properties
    /// <summary>
    /// Gets or sets the Lightness channel.
    /// </summary>
    /// <since>5.0</since>
    public double L
    {
      get { return m_l; }
      set { m_l = ClipL(value); }
    }
    /// <summary>
    /// Gets or sets the Chroma channel. Chroma is defined from -1.0 to +1.0.
    /// </summary>
    /// <since>5.0</since>
    public double C
    {
      get { return m_c; }
      set { m_c = ClipC(value); }
    }
    /// <summary>
    /// Gets or sets the Hue channel. The hue channel is limited to the 0~360 degree range.
    /// </summary>
    /// <since>5.0</since>
    public double H
    {
      get { return m_h; }
      set { m_h = ClipH(value); }
    }
    /// <summary>
    /// Gets or sets the Alpha channel. The Alpha channel is limited to the 0~1 range.
    /// </summary>
    /// <since>5.0</since>
    public double A
    {
      get { return 1.0 - m_a; }
      set { m_a = 1.0 - ClipL(value); }
    }
    #endregion

    #region methods
    internal static double ClipC(double n)
    {
      if (n <= -1.0) { return -1.0; }
      if (n >= +1.0) { return +1.0; }
      return n;
    }
    internal static double ClipH(double n)
    {
      n = n % 360.0;
      if (n < 0) { n += 360.0; }
      return n;
    }
    internal static double ClipL(double n)
    {
      if (n <= 0.0) { return 0.0; }
      if (n >= 1.0) { return 1.0; }
      return n;
    }

    /// <summary>
    /// Ensure the Chromaticity of this color is positive. 
    /// </summary>
    /// <since>5.0</since>
    public void MakePositive()
    {
      if (C < 0)
      {
        C = Math.Abs(C);
        H = (H + 180.0) % 360.0;
      }
    }
    #endregion
  }

  /// <summary>
  /// Represents an HSV (Hue, Saturation, Value) color with double precision floating point channels. 
  /// HSV colors (also sometimes called HSB, where B means Brightness) are similar to HSL colors in that they
  /// represent colors in a cylindrical color space, and are intended to provide intuitive means to edit the 
  /// brightness of a particular color over RGB color space where each color channel would need to be 
  /// modified to affect the color brightness.
  /// </summary>
  [Serializable]
  public struct ColorHSV
  {
    #region members
    internal double m_h;
    internal double m_s;
    internal double m_v;
    /// <summary>
    /// Alpha values are inverted internally! 0 = opaque; 1 = transparent!
    /// </summary>
    internal double m_a;
    #endregion

    #region constructors

    /// <summary>
    /// Constructs a new instance of ColorHSV that is equivalent to an ARGB color.
    /// </summary>
    /// <param name="rgb">ARGB color to mimic.</param>
    /// <remarks>Exact conversions between color spaces are often not possible.</remarks>
    /// <since>6.0</since>
    public ColorHSV(System.Drawing.Color rgb)
    {
      m_a = 1.0 - (rgb.A / 255.0);
      ColorConverter.RGB_To_HSV(rgb.R, rgb.G, rgb.B, out m_h, out m_s, out m_v);
    }

    /// <summary>
    /// Constructs a new instance of ColorHSV with custom channel values.
    /// </summary>
    /// <param name="hue">Hue channel value. Hue channels rotate between 0.0 and 1.0.</param>
    /// <param name="saturation">Saturation channel value. Channel will be limited to 0~1.</param>
    /// <param name="value">Value (Brightness) channel value. Channel will be limited to 0~1.</param>
    /// <since>6.0</since>
    public ColorHSV(double hue, double saturation, double value)
    {
      m_h = (float)hue;
      m_s = Clip(saturation);
      m_v = Clip(value);
      m_a = 0.0f;
    }

    /// <summary>
    /// Constructs a new instance of ColorHSV with custom channel values.
    /// </summary>
    /// <param name="alpha">Alpha channel value. Channel will be limited to 0~1.</param>
    /// <param name="hue">Hue channel value. Hue channels rotate between 0.0 and 1.0.</param>
    /// <param name="saturation">Saturation channel value. Channel will be limited to 0~1.</param>
    /// <param name="value">Value (Brightness) channel value. Channel will be limited to 0~1.</param>
    /// <since>6.0</since>
    public ColorHSV(double alpha, double hue, double saturation, double value)
    {
      m_h = (float)hue;
      m_s = Clip(saturation);
      m_v = Clip(value);
      m_a = 1.0 - Clip(alpha);
    }

    /// <summary>
    /// Create the nearest HSV equivalent of a RGBA color.
    /// </summary>
    /// <param name="rgba">Target color in RGBA space.</param>
    /// <returns>The HSV equivalent of the RGBA color.</returns>
    /// <since>7.0</since>
    public static ColorHSV CreateFromRGBA(ColorRGBA rgba)
    {
      ColorConverter.RGB_To_HSV(rgba.R, rgba.G, rgba.B, out var h, out var s, out var v);
      return new ColorHSV(rgba.A, h, s, v);
    }
    /// <summary>
    /// Constructs the nearest HSV equivalent of an HSL color.
    /// </summary>
    /// <param name="hsl">Target color in HSL space.</param>
    /// <returns>The HSV equivalent of the HSL color.</returns>
    /// <since>6.0</since>
    public static ColorHSV CreateFromHSL(ColorHSL hsl)
    {
      return CreateFromRGBA(ColorRGBA.CreateFromHSL(hsl));
    }
    /// <summary>
    /// Create the nearest HSV equivalent of a CMYK color.
    /// </summary>
    /// <param name="cmyk">Target color in CMYK space.</param>
    /// <returns>The HSV equivalent of the CMYK color.</returns>
    /// <since>6.0</since>
    public static ColorHSV CreateFromCMYK(ColorCMYK cmyk)
    {
      return CreateFromRGBA(ColorRGBA.CreateFromCMYK(cmyk));
    }

    /// <summary>
    /// Create the nearest HSV equivalent of an XYZ color.
    /// </summary>
    /// <param name="xyz">Target color in XYZ space.</param>
    /// <returns>The HSV equivalent of the XYZ color.</returns>
    /// <since>6.0</since>
    public static ColorHSV CreateFromXYZ(ColorXYZ xyz)
    {
      return CreateFromRGBA(ColorRGBA.CreateFromXYZ(xyz));
    }
    /// <summary>
    /// Create the nearest HSV equivalent of a LAB color.
    /// </summary>
    /// <param name="lab">Target color in LAB space.</param>
    /// <returns>The HSV equivalent of the LAB color.</returns>
    /// <since>6.0</since>
    public static ColorHSV CreateFromLAB(ColorLAB lab)
    {
      return CreateFromRGBA(ColorRGBA.CreateFromLAB(lab));
    }
    /// <summary>
    /// Create the nearest HSV equivalent of a LCH color.
    /// </summary>
    /// <param name="lch">Target color in LCH space.</param>
    /// <returns>The HSV equivalent of the LCH color.</returns>
    /// <since>6.0</since>
    public static ColorHSV CreateFromLCH(ColorLCH lch)
    {
      return CreateFromRGBA(ColorRGBA.CreateFromLCH(lch));
    }

    #endregion

    #region operators

    /// <summary>
    /// Implicitly converts a ColorHSV in a .Net library color.
    /// </summary>
    /// <param name="hsv">A HSV color.</param>
    /// <returns>A ARGB .Net library color.</returns>
    public static implicit operator System.Drawing.Color(ColorHSV hsv)
    {
      return (System.Drawing.Color)ColorRGBA.CreateFromHSV(hsv);
    }
    #endregion

    #region operators

    #endregion

    #region properties
    /// <summary>
    /// Gets or sets the hue channel value. 
    /// Hue channels rotate between 0.0 and 1.0.
    /// </summary>
    /// <since>6.0</since>
    public double H
    {
      get { return m_h; }
      set { m_h = value; } // (value + 1.0F) % 1.0F; }
    }
    /// <summary>
    /// Gets or sets the saturation channel value. 
    /// Saturation channels are limited to a 0~1 range.
    /// </summary>
    /// <since>6.0</since>
    public double S
    {
      get { return m_s; }
      set { m_s = Clip(value); }
    }
    /// <summary>
    /// Gets or sets the value (brightness) channel value. 
    /// Value channels are limited to a 0~1 range.
    /// </summary>
    /// <since>6.0</since>
    public double V
    {
      get { return m_v; }
      set { m_v = Clip(value); }
    }
    /// <summary>
    /// Gets or sets the alpha channel value. 
    /// Alpha channels are limited to a 0~1 range.
    /// </summary>
    /// <since>6.0</since>
    public double A
    {
      get { return 1.0 - m_a; }
      set { m_a = 1.0 - Clip(value); }
    }
    #endregion

    #region methods
    internal static double Clip(double n)
    {
      if (n <= 0.0) { return 0.0; }
      if (n >= 1.0) { return 1.0; }
      return n;
    }

    /// <summary>
    /// Convert HSV color to an equivalent System.Drawing.Color.
    /// </summary>
    /// <returns>A .Net framework library color value.</returns>
    /// <since>6.0</since>
    public System.Drawing.Color ToArgbColor() => (System.Drawing.Color) this;

    #endregion
  }


  /// <summary>
  /// Exposes static color conversion methods.
  /// </summary>
  internal class ColorConverter
  {
    #region integer conversions
    internal static void RGB_To_HSV(byte r, byte g, byte b, out double h, out double s, out double v)
    {
      RGB_To_HSV(r / 255.0, g / 255.0, b / 255.0, out h, out s, out v);
    }
    internal static void RGB_To_HSL(byte r, byte g, byte b, out double h, out double s, out double l)
    {
      RGB_To_HSL(r / 255.0, g / 255.0, b / 255.0, out h, out s, out l);
    }
    internal static void RGB_To_XYZ(byte r, byte g, byte b, out double x, out double y, out double z)
    {
      RGB_To_XYZ(r / 255.0, g / 255.0, b / 255.0, out x, out y, out z);
    }
    #endregion

    /// <summary>
    /// Converts RGB space colors to HSV. 
    /// </summary>
    /// <param name="r">Red channel (0.0~1.0)</param>
    /// <param name="g">Green channel (0.0~1.0)</param>
    /// <param name="b">Blue channel (0.0~1.0)</param>
    /// <param name="h">Hue channel (0.0~1.0)</param>
    /// <param name="s">Saturation channel (0.0~1.0)</param>
    /// <param name="v">Value (Brightness) channel (0.0~1.0)</param>
    internal static void RGB_To_HSV(double r, double g, double b, out double h, out double s, out double v)
    {
      h = 0.0;
      s = 0.0;

      double vR = r;
      double vG = g;
      double vB = b;

      double vMin = Math.Min(Math.Min(vR, vG), vB);
      double vMax = Math.Max(Math.Max(vR, vG), vB);
      double vDel = vMax - vMin;

      //from http://www.easyrgb.com/index.php?X=MATH&H=19#text20

      v = vMax;

      if (vDel.Equals(0))                     //This is a gray, no chroma...
      {
        h = 0.0;                                //HSV results from 0 to 1
        s = 0.0;
      }
      else
      {
        s = vDel / vMax;

        double dR = (((vMax - vR) / 6.0) + (vDel / 2.0)) / vDel;
        double dG = (((vMax - vG) / 6.0) + (vDel / 2.0)) / vDel;
        double dB = (((vMax - vB) / 6.0) + (vDel / 2.0)) / vDel;

        if (vR.Equals(vMax)) h = dB - dG;
        else if (vG.Equals(vMax)) h = (1.0 / 3.0) + dR - dB;
        else if (vB.Equals(vMax)) h = (2.0 / 3.0) + dG - dR;

        if (h < 0) h += 1.0;
        if (h > 1) h -= 1.0;
      }
    }

    /// <summary>
    /// Converts HSV space colors to RGB. 
    /// </summary>
    /// <param name="h">Hue channel (0.0~1.0)</param>
    /// <param name="s">Saturation channel (0.0~1.0)</param>
    /// <param name="v">Value (Brightness) channel (0.0~1.0)</param>
    /// <param name="r">Red channel (0.0~1.0)</param>
    /// <param name="g">Green channel (0.0~1.0)</param>
    /// <param name="b">Blue channel (0.0~1.0)</param>
    internal static void HSV_To_RGB(double h, double s, double v, out double r, out double g, out double b)
    {
      if (s.Equals(0.0))
      {
        r = g = b = v;
      }
      else
      {
        //from http://www.easyrgb.com/index.php?X=MATH&H=21#text21

        double vH = h * 6.0;
        if (vH.Equals(6.0)) vH = 0.0;
        int vI = (int)vH;
        double v1 = v * (1.0 - s);
        double v2 = v * (1.0 - s * (vH - vI));
        double v3 = v * (1.0 - s * (1.0 - (vH - vI)));

        double vR = 0.0;
        double vG = 0.0;
        double vB = 0.0;

        if (vI == 0) { vR = v; vG = v3; vB = v1; }
        else if (vI == 1) { vR = v2; vG = v; vB = v1; }
        else if (vI == 2) { vR = v1; vG = v; vB = v3; }
        else if (vI == 3) { vR = v1; vG = v2; vB = v; }
        else if (vI == 4) { vR = v3; vG = v1; vB = v; }
        else { vR = v; vG = v1; vB = v2; }

        r = vR;
        g = vG;
        b = vB;
      }
    }

    /// <summary>
    /// Converts XYZ space colors to RGB.
    /// </summary>
    /// <param name="x">X channel (0.0~1.0)</param>
    /// <param name="y">Y channel (0.0~1.0)</param>
    /// <param name="z">Z channel (0.0~1.0)</param>
    /// <param name="r">Red channel (0.0~1.0)</param>
    /// <param name="g">Green channel (0.0~1.0)</param>
    /// <param name="b">Blue channel (0.0~1.0)</param>
    internal static void XYZ_To_RGB(double x, double y, double z, out double r, out double g, out double b)
    {
      x *= 100.0;
      y *= 100.0;
      z *= 100.0;

      double vR = x * +3.2404542 + y * -1.5371385 + z * -0.4985314;
      double vG = x * -0.9692660 + y * +1.8760108 + z * +0.0415560;
      double vB = x * +0.0556434 + y * -0.2040259 + z * +1.0572252;

      vR *= 0.01;
      vG *= 0.01;
      vB *= 0.01;

      r = xyzrgb_map(vR);
      g = xyzrgb_map(vG);
      b = xyzrgb_map(vB);
    }
    private static double xyzrgb_map(double v)
    {
      if (v > 0.0031308)
      {
        return 1.055 * Math.Pow(v, 1.0 / 2.4) - 0.055;
      }
      return 12.92 * v;
    }

    /// <summary>
    /// Converts RGB space colors to XYZ. 
    /// </summary>
    /// <param name="r">Red channel (0.0~1.0)</param>
    /// <param name="g">Green channel (0.0~1.0)</param>
    /// <param name="b">Blue channel (0.0~1.0)</param>
    /// <param name="x">X channel (0.0~1.0)</param>
    /// <param name="y">Y channel (0.0~1.0)</param>
    /// <param name="z">Z channel (0.0~1.0)</param>
    internal static void RGB_To_XYZ(double r, double g, double b, out double x, out double y, out double z)
    {
      double vR = rgbxyz_map(r);
      double vG = rgbxyz_map(g);
      double vB = rgbxyz_map(b);

      vR *= 100.0;
      vG *= 100.0;
      vB *= 100.0;

      x = vR * 0.4124564 + vG * 0.3575761 + vB * 0.1804375;
      y = vR * 0.2126729 + vG * 0.7151522 + vB * 0.0721750;
      z = vR * 0.0193339 + vG * 0.1191920 + vB * 0.9503041;

      x *= 0.01;
      y *= 0.01;
      z *= 0.01;
    }
    private static double rgbxyz_map(double v)
    {
      if (v > 0.04045)
      {
        return Math.Pow((v + 0.055) / 1.055, 2.4);
      }
      return v / 12.92;
    }

    /// <summary>
    /// Converts XYZ space colors to CIE-L*ab. 
    /// </summary>
    /// <param name="x">X channel (0.0~1.0)</param>
    /// <param name="y">Y channel (0.0~1.0)</param>
    /// <param name="z">Z channel (0.0~1.0)</param>
    /// <param name="l">Luminance channel (0.0~1.0)</param>
    /// <param name="a">A channel (0.0~1.0)</param>
    /// <param name="b">B channel (0.0~1.0)</param>
    internal static void XYZ_To_CIELAB(double x, double y, double z, out double l, out double a, out double b)
    {
      x *= 100.0;
      y *= 100.0;
      z *= 100.0;

      double vX = x / 95.047;
      double vY = y / 100.0;
      double vZ = z / 108.883;

      vX = xyzlab_map(vX);
      vY = xyzlab_map(vY);
      vZ = xyzlab_map(vZ);

      l = (116.0 * vY) - 16.0;
      a = 500 * (vX - vY);
      b = 200 * (vY - vZ);

      l *= 0.01;
      a *= 0.01;
      b *= 0.01;
    }
    private static double xyzlab_map(double v)
    {
      if (v > 0.008856)
      {
        return Math.Pow(v, 1.0 / 3.0);
      }
      return (v * 7.787) + (16.0 / 116.0);
    }

    /// <summary>
    /// Converts CIE-L*ab space colors to XYZ. 
    /// </summary>
    /// <param name="l">Luminance channel (0.0~1.0)</param>
    /// <param name="a">A channel (0.0~1.0)</param>
    /// <param name="b">B channel (0.0~1.0)</param>
    /// <param name="x">X channel (0.0~1.0)</param>
    /// <param name="y">Y channel (0.0~1.0)</param>
    /// <param name="z">Z channel (0.0~1.0)</param>
    internal static void CIELAB_To_XYZ(double l, double a, double b, out double x, out double y, out double z)
    {
      l *= 100.0;
      a *= 100.0;
      b *= 100.0;

      double vY = (l + 16.0) / 116.0;
      double vX = (a / 500.0) + vY;
      double vZ = vY - (b / 200.0);

      vY = labxyx_map(vY);
      vX = labxyx_map(vX);
      vZ = labxyx_map(vZ);

      x = 95.0470 * vX;
      y = 100.000 * vY;
      z = 108.883 * vZ;

      x *= 0.01;
      y *= 0.01;
      z *= 0.01;
    }
    private static double labxyx_map(double v)
    {
      double pv = Math.Pow(v, 3.0);
      if (pv > 0.008856)
      {
        return pv;
      }
      return (v - (16.0 / 116.0)) / 7.787;
    }

    /// <summary>
    /// Converts CIE-L*ab space colors to CIE-L*CH. 
    /// </summary>
    /// <param name="l">Luminance channel (0.0~1.0)</param>
    /// <param name="a">A channel (0.0~1.0)</param>
    /// <param name="b">B channel (0.0~1.0)</param>
    /// <param name="lum">Luminance channel (0.0~1.0)</param>
    /// <param name="c">Chroma channel (0.0~1.0)</param>
    /// <param name="h">Hue channel (0.0~1.0)</param>
    internal static void CIELAB_To_CIELCH(double l, double a, double b, out double lum, out double c, out double h)
    {
      l *= 100.0;
      a *= 100.0;
      b *= 100.0;

      double vH = Math.Atan2(b, a);
      if (vH > 0.0)
      {
        vH = (vH / Math.PI);
      }
      else
      {
        vH = 1.0 - (Math.Abs(vH) / Math.PI);
      }

      lum = l;
      c = Math.Sqrt(a * a + b * b);
      h = vH;


      lum *= 0.01;
      c *= 0.01;
      //h is already unitized.
    }

    /// <summary>
    /// Converts CIE-L*CH space colors to CIE-L*ab. 
    /// </summary>
    /// <param name="lum">Luminance channel (0.0~1.0)</param>
    /// <param name="c">Chroma channel (0.0~1.0)</param>
    /// <param name="h">Hue channel (0.0~1.0)</param>
    /// <param name="l">Luminance channel (0.0~1.0)</param>
    /// <param name="a">A channel (0.0~1.0)</param>
    /// <param name="b">B channel (0.0~1.0)</param>
    internal static void CIELCH_To_CIELAB(double lum, double c, double h, out double l, out double a, out double b)
    {
      l = lum;
      a = Math.Cos(h * 2 * Math.PI) * c;
      b = Math.Sin(h * 2 * Math.PI) * c;
    }

    /// <summary>
    /// Converts RGB space colors to HSL. 
    /// </summary>
    /// <param name="r">Red channel (0.0~1.0)</param>
    /// <param name="g">Green channel (0.0~1.0)</param>
    /// <param name="b">Blue channel (0.0~1.0)</param>
    /// <param name="h">Hue channel (0.0~1.0)</param>
    /// <param name="s">Saturation channel (0.0~1.0)</param>
    /// <param name="l">Luminance channel (0.0~1.0)</param>
    internal static void RGB_To_HSL(double r, double g, double b, out double h, out double s, out double l)
    {
      h = 0.0;
      s = 0.0;

      double vR = r;
      double vG = g;
      double vB = b;

      double vMin = Math.Min(Math.Min(vR, vG), vB);
      double vMax = Math.Max(Math.Max(vR, vG), vB);
      double vDel = vMax - vMin;

      l = 0.5 * (vMax + vMin);

      if (!vDel.Equals(0.0))
      {
        // non-greyscale
        if (l < 0.5)
        {
          s = vDel / (vMax + vMin);
        }
        else
        {
          s = vDel / (2.0 - vMax - vMin);
        }

        if (vR.Equals(vMax))
        {
          h = (vG - vB) / vDel;
        }
        else if (vG.Equals(vMax))
        {
          h = 2.0 + (vB - vR) / vDel;
        }
        else
        {
          h = 4.0 + (vR - vG) / vDel;
        }

        h /= 6.0;
        if (h < 0.0) { h += 1.0; }
        if (h > 1.0) { h -= 1.0; }
      }
    }

    /// <summary>
    /// Converts HSL space colors to RGB. 
    /// </summary>
    /// <param name="h">Hue channel (0.0~1.0)</param>
    /// <param name="s">Saturation channel (0.0~1.0)</param>
    /// <param name="l">Luminance channel (0.0~1.0)</param>
    /// <param name="r">Red channel (0.0~1.0)</param>
    /// <param name="g">Green channel (0.0~1.0)</param>
    /// <param name="b">Blue channel (0.0~1.0)</param>
    internal static void HSL_To_RGB(double h, double s, double l, out double r, out double g, out double b)
    {
      if (s.Equals(0.0))
      {
        r = g = b = l;
      }
      else
      {
        double v2;

        if (l < 0.5)
          v2 = l * (1.0 + s);
        else
          v2 = (l + s) - (l * s);

        double v1 = 2.0 * l - v2;

        r = huergb_map(v1, v2, h + (1.0 / 3.0));
        g = huergb_map(v1, v2, h);
        b = huergb_map(v1, v2, h - (1.0 / 3.0));
      }
    }
    private static double huergb_map(double v1, double v2, double vH)
    {
      if (vH < 0.0) { vH += 1.0; }
      if (vH > 1.0) { vH -= 1.0; }

      if ((6.0 * vH) < 1.0) { return v1 + (v2 - v1) * 6.0 * vH; }
      if ((2.0 * vH) < 1.0) { return v2; }
      if ((3.0 * vH) < 2.0) { return v1 + (v2 - v1) * 6.0 * ((2.0 / 3.0) - vH); }
      return v1;
    }

    /// <summary>
    /// Converts RGB space colors to CMY. 
    /// </summary>
    /// <param name="r">Red channel (0~255)</param>
    /// <param name="g">Green channel (0~255)</param>
    /// <param name="b">Blue channel (0~255)</param>
    /// <param name="c">Cyan channel (0.0~1.0)</param>
    /// <param name="m">Magenta channel (0.0~1.0)</param>
    /// <param name="y">Yellow channel (0.0~1.0)</param>
    internal static void RGB_To_CMY(byte r, byte g, byte b, out double c, out double m, out double y)
    {
      c = 1.0 - ((double)r / 255.0);
      m = 1.0 - ((double)g / 255.0);
      y = 1.0 - ((double)b / 255.0);
    }

    /// <summary>
    /// Converts CMY space colors to RGB. 
    /// </summary>
    /// <param name="c">Cyan channel (0.0~1.0)</param>
    /// <param name="y">Yellow channel (0.0~1.0)</param>
    /// <param name="m">Magenta channel (0.0~1.0)</param>
    /// <param name="r">Red channel (0~255)</param>
    /// <param name="g">Green channel (0~255)</param>
    /// <param name="b">Blue channel (0~255)</param>
    internal static void CMY_To_RGB(double c, double m, double y, out byte r, out byte g, out byte b)
    {
      r = (byte) Math.Round(255.0 * (1.0 - c));
      g = (byte) Math.Round(255.0 * (1.0 - m));
      b = (byte) Math.Round(255.0 * (1.0 - y));
    }

    /// <summary>
    /// Converts CMY space colors to CMYK. 
    /// </summary>
    /// <param name="c">Cyan channel (0.0~1.0)</param>
    /// <param name="y">Yellow channel (0.0~1.0)</param>
    /// <param name="m">Magenta channel (0.0~1.0)</param>
    /// <param name="cyan">Cyan channel (0.0~1.0)</param>
    /// <param name="magenta">Magenta channel (0.0~1.0)</param>
    /// <param name="yellow">Yellow channel (0.0~1.0)</param>
    /// <param name="k">Key channel (0.0~1.0)</param>
    internal static void CMY_To_CMYK(double cyan, double magenta, double yellow,
                                     out double c, out double m, out double y, out double k)
    {
      c = cyan;
      m = magenta;
      y = yellow;
      k = 1.0;

      if (cyan < k) { k = cyan; }
      if (magenta < k) { k = magenta; }
      if (yellow < k) { k = yellow; }

      if (k == 1.0)
      {
        c = 0.0;
        m = 0.0;
        y = 0.0;
      }
      else
      {
        c = (c - k) / (1.0 - k);
        m = (m - k) / (1.0 - k);
        y = (y - k) / (1.0 - k);
      }
    }

    /// <summary>
    /// Converts CMYK space colors to CMY. 
    /// </summary>
    /// <param name="cyan">Cyan channel (0.0~1.0)</param>
    /// <param name="magenta">Magenta channel (0.0~1.0)</param>
    /// <param name="yellow">Yellow channel (0.0~1.0)</param>
    /// <param name="key">Key channel (0.0~1.0)</param>
    /// <param name="c">Cyan channel (0.0~1.0)</param>
    /// <param name="y">Yellow channel (0.0~1.0)</param>
    /// <param name="m">Magenta channel (0.0~1.0)</param>
    internal static void CMYK_To_CMY(double cyan, double magenta, double yellow, double key,
                                     out double c, out double m, out double y)
    {
      c = cyan;
      m = magenta;
      y = yellow;

      c = c * (1.0 - key) + key;
      m = m * (1.0 - key) + key;
      y = y * (1.0 - key) + key;
    }
  }
}
