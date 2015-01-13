using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace onvif.soapenv {


	[System.SerializableAttribute()]
	[System.Xml.Serialization.XmlTypeAttribute(TypeName = "Envelope", Namespace = "http://schemas.xmlsoap.org/soap/envelope/")]
	public partial class Envelope {

		private System.Xml.XmlAttribute[] _anyAttr;

		private Header _header;

		private Body _body;

		private System.Xml.XmlElement[] _any;

		[System.Xml.Serialization.XmlAnyAttributeAttribute()]
		public System.Xml.XmlAttribute[] anyAttr {
			get {
				return this._anyAttr;
			}
			set {
				this._anyAttr = value;
			}
		}

		/// <remarks>optional, order 0</remarks>
		[System.Xml.Serialization.XmlElementAttribute("Header", Namespace = "http://schemas.xmlsoap.org/soap/envelope/")]
		public Header header {
			get {
				return this._header;
			}
			set {
				this._header = value;
			}
		}

		/// <remarks>reqired, order 1</remarks>
		[System.Xml.Serialization.XmlElementAttribute("Body", Namespace = "http://schemas.xmlsoap.org/soap/envelope/")]
		public Body body {
			get {
				return this._body;
			}
			set {
				this._body = value;
			}
		}

		/// <remarks>optional, order 2, namespace ##other</remarks>
		[System.Xml.Serialization.XmlAnyElementAttribute()]
		public System.Xml.XmlElement[] any {
			get {
				return this._any;
			}
			set {
				this._any = value;
			}
		}
	}

	[System.SerializableAttribute()]
	[System.Xml.Serialization.XmlTypeAttribute(TypeName = "Header", Namespace = "http://schemas.xmlsoap.org/soap/envelope/")]
	public partial class Header {

		private System.Xml.XmlAttribute[] _anyAttr;

		private System.Xml.XmlElement[] _any;

		[System.Xml.Serialization.XmlAnyAttributeAttribute()]
		public System.Xml.XmlAttribute[] anyAttr {
			get {
				return this._anyAttr;
			}
			set {
				this._anyAttr = value;
			}
		}

		/// <remarks>optional, order 0, namespace ##other</remarks>
		[System.Xml.Serialization.XmlAnyElementAttribute()]
		public System.Xml.XmlElement[] any {
			get {
				return this._any;
			}
			set {
				this._any = value;
			}
		}
	}

	[System.SerializableAttribute()]
	[System.Xml.Serialization.XmlTypeAttribute(TypeName = "Body", Namespace = "http://schemas.xmlsoap.org/soap/envelope/")]
	public partial class Body {

		private System.Xml.XmlAttribute[] _anyAttr;

		private System.Xml.XmlElement[] _any;

		/// <summary>
		/// Prose in the spec does not specify that attributes are allowed on the Body element
		/// </summary>
		[System.Xml.Serialization.XmlAnyAttributeAttribute()]
		public System.Xml.XmlAttribute[] anyAttr {
			get {
				return this._anyAttr;
			}
			set {
				this._anyAttr = value;
			}
		}

		/// <remarks>optional, order 0, namespace ##any</remarks>
		[System.Xml.Serialization.XmlAnyElementAttribute()]
		public System.Xml.XmlElement[] any {
			get {
				return this._any;
			}
			set {
				this._any = value;
			}
		}
	}

	/// <summary>
	/// Fault reporting structure
	/// </summary>
	[System.SerializableAttribute()]
	[System.Xml.Serialization.XmlTypeAttribute(TypeName = "Fault", Namespace = "http://schemas.xmlsoap.org/soap/envelope/")]
	public partial class Fault {

		private System.Xml.XmlQualifiedName _faultcode;

		private string _faultstring;

		private string _faultactor;

		private Detail _detail;

		/// <remarks>reqired, order 0</remarks>
		[System.Xml.Serialization.XmlElementAttribute("faultcode", Namespace = "", DataType = "QName")]
		public System.Xml.XmlQualifiedName faultcode {
			get {
				return this._faultcode;
			}
			set {
				this._faultcode = value;
			}
		}

		/// <remarks>reqired, order 1</remarks>
		[System.Xml.Serialization.XmlElementAttribute("faultstring", Namespace = "", DataType = "string")]
		public string faultstring {
			get {
				return this._faultstring;
			}
			set {
				this._faultstring = value;
			}
		}

		/// <remarks>optional, order 2</remarks>
		[System.Xml.Serialization.XmlElementAttribute("faultactor", Namespace = "", DataType = "anyURI")]
		public string faultactor {
			get {
				return this._faultactor;
			}
			set {
				this._faultactor = value;
			}
		}

		/// <remarks>optional, order 3</remarks>
		[System.Xml.Serialization.XmlElementAttribute("detail", Namespace = "")]
		public Detail detail {
			get {
				return this._detail;
			}
			set {
				this._detail = value;
			}
		}
	}

	[System.SerializableAttribute()]
	[System.Xml.Serialization.XmlTypeAttribute(TypeName = "detail", Namespace = "http://schemas.xmlsoap.org/soap/envelope/")]
	public partial class Detail {

		private System.Xml.XmlAttribute[] _anyAttr;

		private System.Xml.XmlElement[] _any;

		[System.Xml.Serialization.XmlAnyAttributeAttribute()]
		public System.Xml.XmlAttribute[] anyAttr {
			get {
				return this._anyAttr;
			}
			set {
				this._anyAttr = value;
			}
		}

		/// <remarks>optional, order 0, namespace ##any</remarks>
		[System.Xml.Serialization.XmlAnyElementAttribute()]
		public System.Xml.XmlElement[] any {
			get {
				return this._any;
			}
			set {
				this._any = value;
			}
		}
	}
}

namespace onvif.services {


	/// <summary>
	/// Base class for physical entities like inputs and outputs.
	/// </summary>
	[System.SerializableAttribute()]
	[System.Xml.Serialization.XmlTypeAttribute(TypeName = "DeviceEntity", Namespace = "http://www.onvif.org/ver10/schema")]
	public partial class DeviceEntity {

		private string _token;

		/// <summary>
		/// Unique identifier referencing the physical entity.
		/// </summary>
		[System.Xml.Serialization.XmlAttributeAttribute("token")]
		public string token {
			get {
				return this._token;
			}
			set {
				this._token = value;
			}
		}
	}

	/// <summary>
	/// Rectangle defined by lower left corner position and size. Units are pixel.
	/// </summary>
	[System.SerializableAttribute()]
	[System.Xml.Serialization.XmlTypeAttribute(TypeName = "IntRectangle", Namespace = "http://www.onvif.org/ver10/schema")]
	public partial class IntRectangle {

		private int _x;

		private int _y;

		private int _width;

		private int _height;

		[System.Xml.Serialization.XmlAttributeAttribute("x", DataType = "int")]
		public int x {
			get {
				return this._x;
			}
			set {
				this._x = value;
			}
		}

		[System.Xml.Serialization.XmlAttributeAttribute("y", DataType = "int")]
		public int y {
			get {
				return this._y;
			}
			set {
				this._y = value;
			}
		}

		[System.Xml.Serialization.XmlAttributeAttribute("width", DataType = "int")]
		public int width {
			get {
				return this._width;
			}
			set {
				this._width = value;
			}
		}

		[System.Xml.Serialization.XmlAttributeAttribute("height", DataType = "int")]
		public int height {
			get {
				return this._height;
			}
			set {
				this._height = value;
			}
		}
	}

	/// <summary>
	/// Range of a rectangle. The rectangle itself is defined by lower left corner position and size. Units are pixel.
	/// </summary>
	[System.SerializableAttribute()]
	[System.Xml.Serialization.XmlTypeAttribute(TypeName = "IntRectangleRange", Namespace = "http://www.onvif.org/ver10/schema")]
	public partial class IntRectangleRange {

		private IntRange _xRange;

		private IntRange _yRange;

		private IntRange _widthRange;

		private IntRange _heightRange;

		/// <summary>
		/// Range of X-axis.
		/// </summary>
		/// <remarks>reqired, order 0</remarks>
		[System.Xml.Serialization.XmlElementAttribute("XRange", Namespace = "http://www.onvif.org/ver10/schema")]
		public IntRange xRange {
			get {
				return this._xRange;
			}
			set {
				this._xRange = value;
			}
		}

		/// <summary>
		/// Range of Y-axis.
		/// </summary>
		/// <remarks>reqired, order 1</remarks>
		[System.Xml.Serialization.XmlElementAttribute("YRange", Namespace = "http://www.onvif.org/ver10/schema")]
		public IntRange yRange {
			get {
				return this._yRange;
			}
			set {
				this._yRange = value;
			}
		}

		/// <summary>
		/// Range of width.
		/// </summary>
		/// <remarks>reqired, order 2</remarks>
		[System.Xml.Serialization.XmlElementAttribute("WidthRange", Namespace = "http://www.onvif.org/ver10/schema")]
		public IntRange widthRange {
			get {
				return this._widthRange;
			}
			set {
				this._widthRange = value;
			}
		}

		/// <summary>
		/// Range of height.
		/// </summary>
		/// <remarks>reqired, order 3</remarks>
		[System.Xml.Serialization.XmlElementAttribute("HeightRange", Namespace = "http://www.onvif.org/ver10/schema")]
		public IntRange heightRange {
			get {
				return this._heightRange;
			}
			set {
				this._heightRange = value;
			}
		}
	}

	/// <summary>
	/// Range of values greater equal Min value and less equal Max value.
	/// </summary>
	[System.SerializableAttribute()]
	[System.Xml.Serialization.XmlTypeAttribute(TypeName = "IntRange", Namespace = "http://www.onvif.org/ver10/schema")]
	public partial class IntRange {

		private int _min;

		private int _max;

		/// <remarks>reqired, order 0</remarks>
		[System.Xml.Serialization.XmlElementAttribute("Min", Namespace = "http://www.onvif.org/ver10/schema", DataType = "int")]
		public int min {
			get {
				return this._min;
			}
			set {
				this._min = value;
			}
		}

		/// <remarks>reqired, order 1</remarks>
		[System.Xml.Serialization.XmlElementAttribute("Max", Namespace = "http://www.onvif.org/ver10/schema", DataType = "int")]
		public int max {
			get {
				return this._max;
			}
			set {
				this._max = value;
			}
		}
	}

	/// <summary>
	/// Range of values greater equal Min value and less equal Max value.
	/// </summary>
	[System.SerializableAttribute()]
	[System.Xml.Serialization.XmlTypeAttribute(TypeName = "FloatRange", Namespace = "http://www.onvif.org/ver10/schema")]
	public partial class FloatRange {

		private float _min;

		private float _max;

		/// <remarks>reqired, order 0</remarks>
		[System.Xml.Serialization.XmlElementAttribute("Min", Namespace = "http://www.onvif.org/ver10/schema", DataType = "float")]
		public float min {
			get {
				return this._min;
			}
			set {
				this._min = value;
			}
		}

		/// <remarks>reqired, order 1</remarks>
		[System.Xml.Serialization.XmlElementAttribute("Max", Namespace = "http://www.onvif.org/ver10/schema", DataType = "float")]
		public float max {
			get {
				return this._max;
			}
			set {
				this._max = value;
			}
		}
	}

	/// <summary>
	/// Range of duration greater equal Min duration and less equal Max duration.
	/// </summary>
	[System.SerializableAttribute()]
	[System.Xml.Serialization.XmlTypeAttribute(TypeName = "DurationRange", Namespace = "http://www.onvif.org/ver10/schema")]
	public partial class DurationRange {

		private XsDuration _min;

		private XsDuration _max;

		/// <remarks>reqired, order 0</remarks>
		[System.Xml.Serialization.XmlElementAttribute("Min", Namespace = "http://www.onvif.org/ver10/schema")]
		public XsDuration min {
			get {
				return this._min;
			}
			set {
				this._min = value;
			}
		}

		/// <remarks>reqired, order 1</remarks>
		[System.Xml.Serialization.XmlElementAttribute("Max", Namespace = "http://www.onvif.org/ver10/schema")]
		public XsDuration max {
			get {
				return this._max;
			}
			set {
				this._max = value;
			}
		}
	}

	/// <summary>
	/// List of values.
	/// </summary>
	[System.SerializableAttribute()]
	[System.Xml.Serialization.XmlTypeAttribute(TypeName = "IntList", Namespace = "http://www.onvif.org/ver10/schema")]
	public partial class IntList {

		private int[] _items;

		/// <remarks>optional, order 0</remarks>
		[System.Xml.Serialization.XmlElementAttribute("Items", Namespace = "http://www.onvif.org/ver10/schema", DataType = "int")]
		public int[] items {
			get {
				return this._items;
			}
			set {
				this._items = value;
			}
		}
	}

	[System.SerializableAttribute()]
	[System.Xml.Serialization.XmlTypeAttribute(TypeName = "FloatList", Namespace = "http://www.onvif.org/ver10/schema")]
	public partial class FloatList {

		private float[] _items;

		/// <remarks>optional, order 0</remarks>
		[System.Xml.Serialization.XmlElementAttribute("Items", Namespace = "http://www.onvif.org/ver10/schema", DataType = "float")]
		public float[] items {
			get {
				return this._items;
			}
			set {
				this._items = value;
			}
		}
	}

	[System.SerializableAttribute()]
	[System.Xml.Serialization.XmlTypeAttribute(TypeName = "AnyHolder", Namespace = "http://www.onvif.org/ver10/schema")]
	public partial class AnyHolder {

		private System.Xml.XmlAttribute[] _anyAttr;

		private System.Xml.XmlElement[] _any;

		[System.Xml.Serialization.XmlAnyAttributeAttribute()]
		public System.Xml.XmlAttribute[] anyAttr {
			get {
				return this._anyAttr;
			}
			set {
				this._anyAttr = value;
			}
		}

		/// <remarks>optional, order 0, namespace ##any</remarks>
		[System.Xml.Serialization.XmlAnyElementAttribute()]
		public System.Xml.XmlElement[] any {
			get {
				return this._any;
			}
			set {
				this._any = value;
			}
		}
	}

	/// <summary>
	/// Representation of a physical video input.
	/// </summary>
	[System.SerializableAttribute()]
	[System.Xml.Serialization.XmlTypeAttribute(TypeName = "VideoSource", Namespace = "http://www.onvif.org/ver10/schema")]
	public partial class VideoSource {

		private string _token;

		private System.Xml.XmlAttribute[] _anyAttr;

		private float _framerate;

		private VideoResolution _resolution;

		private ImagingSettings _imaging;

		private VideoSourceExtension _extension;

		/// <summary>
		/// Unique identifier referencing the physical entity.
		/// </summary>
		[System.Xml.Serialization.XmlAttributeAttribute("token")]
		public string token {
			get {
				return this._token;
			}
			set {
				this._token = value;
			}
		}

		[System.Xml.Serialization.XmlAnyAttributeAttribute()]
		public System.Xml.XmlAttribute[] anyAttr {
			get {
				return this._anyAttr;
			}
			set {
				this._anyAttr = value;
			}
		}

		/// <summary>
		/// Frame rate in frames per second.
		/// </summary>
		/// <remarks>reqired, order 0</remarks>
		[System.Xml.Serialization.XmlElementAttribute("Framerate", Namespace = "http://www.onvif.org/ver10/schema", DataType = "float")]
		public float framerate {
			get {
				return this._framerate;
			}
			set {
				this._framerate = value;
			}
		}

		/// <summary>
		/// Horizontal and vertical resolution
		/// </summary>
		/// <remarks>reqired, order 1</remarks>
		[System.Xml.Serialization.XmlElementAttribute("Resolution", Namespace = "http://www.onvif.org/ver10/schema")]
		public VideoResolution resolution {
			get {
				return this._resolution;
			}
			set {
				this._resolution = value;
			}
		}

		/// <summary>
		/// Optional configuration of the image sensor.
		/// </summary>
		/// <remarks>optional, order 2</remarks>
		[System.Xml.Serialization.XmlElementAttribute("Imaging", Namespace = "http://www.onvif.org/ver10/schema")]
		public ImagingSettings imaging {
			get {
				return this._imaging;
			}
			set {
				this._imaging = value;
			}
		}

		/// <remarks>optional, order 3</remarks>
		[System.Xml.Serialization.XmlElementAttribute("Extension", Namespace = "http://www.onvif.org/ver10/schema")]
		public VideoSourceExtension extension {
			get {
				return this._extension;
			}
			set {
				this._extension = value;
			}
		}
	}

	[System.SerializableAttribute()]
	[System.Xml.Serialization.XmlTypeAttribute(TypeName = "VideoResolution", Namespace = "http://www.onvif.org/ver10/schema")]
	public partial class VideoResolution {

		private int _width;

		private int _height;

		/// <summary>
		/// Number of the columns of the Video image.
		/// </summary>
		/// <remarks>reqired, order 0</remarks>
		[System.Xml.Serialization.XmlElementAttribute("Width", Namespace = "http://www.onvif.org/ver10/schema", DataType = "int")]
		public int width {
			get {
				return this._width;
			}
			set {
				this._width = value;
			}
		}

		/// <summary>
		/// Number of the lines of the Video image.
		/// </summary>
		/// <remarks>reqired, order 1</remarks>
		[System.Xml.Serialization.XmlElementAttribute("Height", Namespace = "http://www.onvif.org/ver10/schema", DataType = "int")]
		public int height {
			get {
				return this._height;
			}
			set {
				this._height = value;
			}
		}
	}

	[System.SerializableAttribute()]
	[System.Xml.Serialization.XmlTypeAttribute(TypeName = "ImagingSettings", Namespace = "http://www.onvif.org/ver10/schema")]
	public partial class ImagingSettings {

		private System.Xml.XmlAttribute[] _anyAttr;

		private BacklightCompensation _backlightCompensation;

		private float _brightness;

		private bool _brightnessSpecified;

		private float _colorSaturation;

		private bool _colorSaturationSpecified;

		private float _contrast;

		private bool _contrastSpecified;

		private Exposure _exposure;

		private FocusConfiguration _focus;

		private IrCutFilterMode _irCutFilter;

		private bool _irCutFilterSpecified;

		private float _sharpness;

		private bool _sharpnessSpecified;

		private WideDynamicRange _wideDynamicRange;

		private WhiteBalance _whiteBalance;

		private ImagingSettingsExtension _extension;

		[System.Xml.Serialization.XmlAnyAttributeAttribute()]
		public System.Xml.XmlAttribute[] anyAttr {
			get {
				return this._anyAttr;
			}
			set {
				this._anyAttr = value;
			}
		}

		/// <summary>
		/// Enabled/disabled BLC mode (on/off).
		/// </summary>
		/// <remarks>optional, order 0</remarks>
		[System.Xml.Serialization.XmlElementAttribute("BacklightCompensation", Namespace = "http://www.onvif.org/ver10/schema")]
		public BacklightCompensation backlightCompensation {
			get {
				return this._backlightCompensation;
			}
			set {
				this._backlightCompensation = value;
			}
		}

		/// <summary>
		/// Image brightness (unit unspecified).
		/// </summary>
		/// <remarks>optional, order 1</remarks>
		[System.Xml.Serialization.XmlElementAttribute("Brightness", Namespace = "http://www.onvif.org/ver10/schema", DataType = "float")]
		public float brightness {
			get {
				return this._brightness;
			}
			set {
				this._brightness = value;
			}
		}

		[System.Xml.Serialization.XmlIgnoreAttribute()]
		public bool brightnessSpecified {
			get {
				return this._brightnessSpecified;
			}
			set {
				this._brightnessSpecified = value;
			}
		}

		/// <summary>
		/// Color saturation of the image (unit unspecified).
		/// </summary>
		/// <remarks>optional, order 2</remarks>
		[System.Xml.Serialization.XmlElementAttribute("ColorSaturation", Namespace = "http://www.onvif.org/ver10/schema", DataType = "float")]
		public float colorSaturation {
			get {
				return this._colorSaturation;
			}
			set {
				this._colorSaturation = value;
			}
		}

		[System.Xml.Serialization.XmlIgnoreAttribute()]
		public bool colorSaturationSpecified {
			get {
				return this._colorSaturationSpecified;
			}
			set {
				this._colorSaturationSpecified = value;
			}
		}

		/// <summary>
		/// Contrast of the image (unit unspecified).
		/// </summary>
		/// <remarks>optional, order 3</remarks>
		[System.Xml.Serialization.XmlElementAttribute("Contrast", Namespace = "http://www.onvif.org/ver10/schema", DataType = "float")]
		public float contrast {
			get {
				return this._contrast;
			}
			set {
				this._contrast = value;
			}
		}

		[System.Xml.Serialization.XmlIgnoreAttribute()]
		public bool contrastSpecified {
			get {
				return this._contrastSpecified;
			}
			set {
				this._contrastSpecified = value;
			}
		}

		/// <summary>
		/// Exposure mode of the device.
		/// </summary>
		/// <remarks>optional, order 4</remarks>
		[System.Xml.Serialization.XmlElementAttribute("Exposure", Namespace = "http://www.onvif.org/ver10/schema")]
		public Exposure exposure {
			get {
				return this._exposure;
			}
			set {
				this._exposure = value;
			}
		}

		/// <summary>
		/// Focus configuration.
		/// </summary>
		/// <remarks>optional, order 5</remarks>
		[System.Xml.Serialization.XmlElementAttribute("Focus", Namespace = "http://www.onvif.org/ver10/schema")]
		public FocusConfiguration focus {
			get {
				return this._focus;
			}
			set {
				this._focus = value;
			}
		}

		/// <summary>
		/// Infrared Cutoff Filter settings.
		/// </summary>
		/// <remarks>optional, order 6</remarks>
		[System.Xml.Serialization.XmlElementAttribute("IrCutFilter", Namespace = "http://www.onvif.org/ver10/schema")]
		public IrCutFilterMode irCutFilter {
			get {
				return this._irCutFilter;
			}
			set {
				this._irCutFilter = value;
			}
		}

		[System.Xml.Serialization.XmlIgnoreAttribute()]
		public bool irCutFilterSpecified {
			get {
				return this._irCutFilterSpecified;
			}
			set {
				this._irCutFilterSpecified = value;
			}
		}

		/// <summary>
		/// Sharpness of the Video image.
		/// </summary>
		/// <remarks>optional, order 7</remarks>
		[System.Xml.Serialization.XmlElementAttribute("Sharpness", Namespace = "http://www.onvif.org/ver10/schema", DataType = "float")]
		public float sharpness {
			get {
				return this._sharpness;
			}
			set {
				this._sharpness = value;
			}
		}

		[System.Xml.Serialization.XmlIgnoreAttribute()]
		public bool sharpnessSpecified {
			get {
				return this._sharpnessSpecified;
			}
			set {
				this._sharpnessSpecified = value;
			}
		}

		/// <summary>
		/// WDR settings.
		/// </summary>
		/// <remarks>optional, order 8</remarks>
		[System.Xml.Serialization.XmlElementAttribute("WideDynamicRange", Namespace = "http://www.onvif.org/ver10/schema")]
		public WideDynamicRange wideDynamicRange {
			get {
				return this._wideDynamicRange;
			}
			set {
				this._wideDynamicRange = value;
			}
		}

		/// <summary>
		/// White balance settings.
		/// </summary>
		/// <remarks>optional, order 9</remarks>
		[System.Xml.Serialization.XmlElementAttribute("WhiteBalance", Namespace = "http://www.onvif.org/ver10/schema")]
		public WhiteBalance whiteBalance {
			get {
				return this._whiteBalance;
			}
			set {
				this._whiteBalance = value;
			}
		}

		/// <remarks>optional, order 10</remarks>
		[System.Xml.Serialization.XmlElementAttribute("Extension", Namespace = "http://www.onvif.org/ver10/schema")]
		public ImagingSettingsExtension extension {
			get {
				return this._extension;
			}
			set {
				this._extension = value;
			}
		}
	}

	[System.SerializableAttribute()]
	[System.Xml.Serialization.XmlTypeAttribute(TypeName = "BacklightCompensation", Namespace = "http://www.onvif.org/ver10/schema")]
	public partial class BacklightCompensation {

		private BacklightCompensationMode _mode;

		private float _level;

		/// <summary>
		/// Backlight compensation mode (on/off).
		/// </summary>
		/// <remarks>reqired, order 0</remarks>
		[System.Xml.Serialization.XmlElementAttribute("Mode", Namespace = "http://www.onvif.org/ver10/schema")]
		public BacklightCompensationMode mode {
			get {
				return this._mode;
			}
			set {
				this._mode = value;
			}
		}

		/// <summary>
		/// Optional level parameter (unit unspecified).
		/// </summary>
		/// <remarks>reqired, order 1</remarks>
		[System.Xml.Serialization.XmlElementAttribute("Level", Namespace = "http://www.onvif.org/ver10/schema", DataType = "float")]
		public float level {
			get {
				return this._level;
			}
			set {
				this._level = value;
			}
		}
	}

	/// <summary>
	/// Enumeration describing the available backlight compenstation modes.
	/// </summary>
	[System.SerializableAttribute()]
	[System.Xml.Serialization.XmlTypeAttribute(TypeName = "BacklightCompensationMode", Namespace = "http://www.onvif.org/ver10/schema")]
	public enum BacklightCompensationMode {

		/// <summary>
		/// Backlight compensation is disabled.
		/// </summary>
		[System.Xml.Serialization.XmlEnumAttribute(Name = "OFF")]
		off,

		/// <summary>
		/// Backlight compensation is enabled.
		/// </summary>
		[System.Xml.Serialization.XmlEnumAttribute(Name = "ON")]
		on,
	}

	[System.SerializableAttribute()]
	[System.Xml.Serialization.XmlTypeAttribute(TypeName = "Exposure", Namespace = "http://www.onvif.org/ver10/schema")]
	public partial class Exposure {

		private ExposureMode _mode;

		private ExposurePriority _priority;

		private Rectangle _window;

		private float _minExposureTime;

		private float _maxExposureTime;

		private float _minGain;

		private float _maxGain;

		private float _minIris;

		private float _maxIris;

		private float _exposureTime;

		private float _gain;

		private float _iris;

		/// <summary>
		/// Exposure Mode
		/// </summary>
		/// <remarks>reqired, order 0</remarks>
		[System.Xml.Serialization.XmlElementAttribute("Mode", Namespace = "http://www.onvif.org/ver10/schema")]
		public ExposureMode mode {
			get {
				return this._mode;
			}
			set {
				this._mode = value;
			}
		}

		/// <summary>
		/// The exposure priority mode (low noise/framerate).
		/// </summary>
		/// <remarks>reqired, order 1</remarks>
		[System.Xml.Serialization.XmlElementAttribute("Priority", Namespace = "http://www.onvif.org/ver10/schema")]
		public ExposurePriority priority {
			get {
				return this._priority;
			}
			set {
				this._priority = value;
			}
		}

		/// <summary>
		/// Rectangular exposure mask.
		/// </summary>
		/// <remarks>reqired, order 2</remarks>
		[System.Xml.Serialization.XmlElementAttribute("Window", Namespace = "http://www.onvif.org/ver10/schema")]
		public Rectangle window {
			get {
				return this._window;
			}
			set {
				this._window = value;
			}
		}

		/// <summary>
		/// Minimum value of exposure time range allowed to be used by the algorithm.
		/// </summary>
		/// <remarks>reqired, order 3</remarks>
		[System.Xml.Serialization.XmlElementAttribute("MinExposureTime", Namespace = "http://www.onvif.org/ver10/schema", DataType = "float")]
		public float minExposureTime {
			get {
				return this._minExposureTime;
			}
			set {
				this._minExposureTime = value;
			}
		}

		/// <summary>
		/// Maximum value of exposure time range allowed to be used by the algorithm.
		/// </summary>
		/// <remarks>reqired, order 4</remarks>
		[System.Xml.Serialization.XmlElementAttribute("MaxExposureTime", Namespace = "http://www.onvif.org/ver10/schema", DataType = "float")]
		public float maxExposureTime {
			get {
				return this._maxExposureTime;
			}
			set {
				this._maxExposureTime = value;
			}
		}

		/// <summary>
		/// Minimum value of the sensor gain range that is allowed to be used by the algorithm.
		/// </summary>
		/// <remarks>reqired, order 5</remarks>
		[System.Xml.Serialization.XmlElementAttribute("MinGain", Namespace = "http://www.onvif.org/ver10/schema", DataType = "float")]
		public float minGain {
			get {
				return this._minGain;
			}
			set {
				this._minGain = value;
			}
		}

		/// <summary>
		/// Maximum value of the sensor gain range that is allowed to be used by the algorithm.
		/// </summary>
		/// <remarks>reqired, order 6</remarks>
		[System.Xml.Serialization.XmlElementAttribute("MaxGain", Namespace = "http://www.onvif.org/ver10/schema", DataType = "float")]
		public float maxGain {
			get {
				return this._maxGain;
			}
			set {
				this._maxGain = value;
			}
		}

		/// <summary>
		/// Minimum value of the iris range allowed to be used by the algorithm.
		/// </summary>
		/// <remarks>reqired, order 7</remarks>
		[System.Xml.Serialization.XmlElementAttribute("MinIris", Namespace = "http://www.onvif.org/ver10/schema", DataType = "float")]
		public float minIris {
			get {
				return this._minIris;
			}
			set {
				this._minIris = value;
			}
		}

		/// <summary>
		/// Maximum value of the iris range allowed to be used by the algorithm.
		/// </summary>
		/// <remarks>reqired, order 8</remarks>
		[System.Xml.Serialization.XmlElementAttribute("MaxIris", Namespace = "http://www.onvif.org/ver10/schema", DataType = "float")]
		public float maxIris {
			get {
				return this._maxIris;
			}
			set {
				this._maxIris = value;
			}
		}

		/// <summary>
		/// The fixed exposure time used by the image sensor (μs).
		/// </summary>
		/// <remarks>reqired, order 9</remarks>
		[System.Xml.Serialization.XmlElementAttribute("ExposureTime", Namespace = "http://www.onvif.org/ver10/schema", DataType = "float")]
		public float exposureTime {
			get {
				return this._exposureTime;
			}
			set {
				this._exposureTime = value;
			}
		}

		/// <summary>
		/// The fixed gain used by the image sensor (dB).
		/// </summary>
		/// <remarks>reqired, order 10</remarks>
		[System.Xml.Serialization.XmlElementAttribute("Gain", Namespace = "http://www.onvif.org/ver10/schema", DataType = "float")]
		public float gain {
			get {
				return this._gain;
			}
			set {
				this._gain = value;
			}
		}

		/// <summary>
		/// The fixed attenuation of input light affected by the iris (dB). 0dB maps to a fully opened iris.
		/// </summary>
		/// <remarks>reqired, order 11</remarks>
		[System.Xml.Serialization.XmlElementAttribute("Iris", Namespace = "http://www.onvif.org/ver10/schema", DataType = "float")]
		public float iris {
			get {
				return this._iris;
			}
			set {
				this._iris = value;
			}
		}
	}

	[System.SerializableAttribute()]
	[System.Xml.Serialization.XmlTypeAttribute(TypeName = "ExposureMode", Namespace = "http://www.onvif.org/ver10/schema")]
	public enum ExposureMode {

		[System.Xml.Serialization.XmlEnumAttribute(Name = "AUTO")]
		auto,

		[System.Xml.Serialization.XmlEnumAttribute(Name = "MANUAL")]
		manual,
	}

	[System.SerializableAttribute()]
	[System.Xml.Serialization.XmlTypeAttribute(TypeName = "ExposurePriority", Namespace = "http://www.onvif.org/ver10/schema")]
	public enum ExposurePriority {

		[System.Xml.Serialization.XmlEnumAttribute(Name = "LowNoise")]
		lowNoise,

		[System.Xml.Serialization.XmlEnumAttribute(Name = "FrameRate")]
		frameRate,
	}

	[System.SerializableAttribute()]
	[System.Xml.Serialization.XmlTypeAttribute(TypeName = "Rectangle", Namespace = "http://www.onvif.org/ver10/schema")]
	public partial class Rectangle {

		private float _bottom;

		private bool _bottomSpecified;

		private float _top;

		private bool _topSpecified;

		private float _right;

		private bool _rightSpecified;

		private float _left;

		private bool _leftSpecified;

		[System.Xml.Serialization.XmlAttributeAttribute("bottom", DataType = "float")]
		public float bottom {
			get {
				return this._bottom;
			}
			set {
				this._bottom = value;
			}
		}

		[System.Xml.Serialization.XmlIgnoreAttribute()]
		public bool bottomSpecified {
			get {
				return this._bottomSpecified;
			}
			set {
				this._bottomSpecified = value;
			}
		}

		[System.Xml.Serialization.XmlAttributeAttribute("top", DataType = "float")]
		public float top {
			get {
				return this._top;
			}
			set {
				this._top = value;
			}
		}

		[System.Xml.Serialization.XmlIgnoreAttribute()]
		public bool topSpecified {
			get {
				return this._topSpecified;
			}
			set {
				this._topSpecified = value;
			}
		}

		[System.Xml.Serialization.XmlAttributeAttribute("right", DataType = "float")]
		public float right {
			get {
				return this._right;
			}
			set {
				this._right = value;
			}
		}

		[System.Xml.Serialization.XmlIgnoreAttribute()]
		public bool rightSpecified {
			get {
				return this._rightSpecified;
			}
			set {
				this._rightSpecified = value;
			}
		}

		[System.Xml.Serialization.XmlAttributeAttribute("left", DataType = "float")]
		public float left {
			get {
				return this._left;
			}
			set {
				this._left = value;
			}
		}

		[System.Xml.Serialization.XmlIgnoreAttribute()]
		public bool leftSpecified {
			get {
				return this._leftSpecified;
			}
			set {
				this._leftSpecified = value;
			}
		}
	}

	[System.SerializableAttribute()]
	[System.Xml.Serialization.XmlTypeAttribute(TypeName = "FocusConfiguration", Namespace = "http://www.onvif.org/ver10/schema")]
	public partial class FocusConfiguration {

		private System.Xml.XmlAttribute[] _anyAttr;

		private AutoFocusMode _autoFocusMode;

		private float _defaultSpeed;

		private float _nearLimit;

		private float _farLimit;

		private System.Xml.XmlElement[] _any;

		[System.Xml.Serialization.XmlAnyAttributeAttribute()]
		public System.Xml.XmlAttribute[] anyAttr {
			get {
				return this._anyAttr;
			}
			set {
				this._anyAttr = value;
			}
		}

		/// <remarks>reqired, order 0</remarks>
		[System.Xml.Serialization.XmlElementAttribute("AutoFocusMode", Namespace = "http://www.onvif.org/ver10/schema")]
		public AutoFocusMode autoFocusMode {
			get {
				return this._autoFocusMode;
			}
			set {
				this._autoFocusMode = value;
			}
		}

		/// <remarks>reqired, order 1</remarks>
		[System.Xml.Serialization.XmlElementAttribute("DefaultSpeed", Namespace = "http://www.onvif.org/ver10/schema", DataType = "float")]
		public float defaultSpeed {
			get {
				return this._defaultSpeed;
			}
			set {
				this._defaultSpeed = value;
			}
		}

		/// <summary>
		/// Parameter to set autofocus near limit (unit: meter).
		/// </summary>
		/// <remarks>reqired, order 2</remarks>
		[System.Xml.Serialization.XmlElementAttribute("NearLimit", Namespace = "http://www.onvif.org/ver10/schema", DataType = "float")]
		public float nearLimit {
			get {
				return this._nearLimit;
			}
			set {
				this._nearLimit = value;
			}
		}

		/// <summary>
		/// Parameter to set autofocus far limit (unit: meter).
		/// If set to 0.0, infinity will be used.
		/// </summary>
		/// <remarks>reqired, order 3</remarks>
		[System.Xml.Serialization.XmlElementAttribute("FarLimit", Namespace = "http://www.onvif.org/ver10/schema", DataType = "float")]
		public float farLimit {
			get {
				return this._farLimit;
			}
			set {
				this._farLimit = value;
			}
		}

		/// <remarks>optional, order 4, namespace ##any</remarks>
		[System.Xml.Serialization.XmlAnyElementAttribute()]
		public System.Xml.XmlElement[] any {
			get {
				return this._any;
			}
			set {
				this._any = value;
			}
		}
	}

	[System.SerializableAttribute()]
	[System.Xml.Serialization.XmlTypeAttribute(TypeName = "AutoFocusMode", Namespace = "http://www.onvif.org/ver10/schema")]
	public enum AutoFocusMode {

		[System.Xml.Serialization.XmlEnumAttribute(Name = "AUTO")]
		auto,

		[System.Xml.Serialization.XmlEnumAttribute(Name = "MANUAL")]
		manual,
	}

	[System.SerializableAttribute()]
	[System.Xml.Serialization.XmlTypeAttribute(TypeName = "IrCutFilterMode", Namespace = "http://www.onvif.org/ver10/schema")]
	public enum IrCutFilterMode {

		[System.Xml.Serialization.XmlEnumAttribute(Name = "ON")]
		on,

		[System.Xml.Serialization.XmlEnumAttribute(Name = "OFF")]
		off,

		[System.Xml.Serialization.XmlEnumAttribute(Name = "AUTO")]
		auto,
	}

	[System.SerializableAttribute()]
	[System.Xml.Serialization.XmlTypeAttribute(TypeName = "WideDynamicRange", Namespace = "http://www.onvif.org/ver10/schema")]
	public partial class WideDynamicRange {

		private WideDynamicMode _mode;

		private float _level;

		/// <summary>
		/// White dynamic range (on/off)
		/// </summary>
		/// <remarks>reqired, order 0</remarks>
		[System.Xml.Serialization.XmlElementAttribute("Mode", Namespace = "http://www.onvif.org/ver10/schema")]
		public WideDynamicMode mode {
			get {
				return this._mode;
			}
			set {
				this._mode = value;
			}
		}

		/// <summary>
		/// Optional level parameter (unitless)
		/// </summary>
		/// <remarks>reqired, order 1</remarks>
		[System.Xml.Serialization.XmlElementAttribute("Level", Namespace = "http://www.onvif.org/ver10/schema", DataType = "float")]
		public float level {
			get {
				return this._level;
			}
			set {
				this._level = value;
			}
		}
	}

	[System.SerializableAttribute()]
	[System.Xml.Serialization.XmlTypeAttribute(TypeName = "WideDynamicMode", Namespace = "http://www.onvif.org/ver10/schema")]
	public enum WideDynamicMode {

		[System.Xml.Serialization.XmlEnumAttribute(Name = "OFF")]
		off,

		[System.Xml.Serialization.XmlEnumAttribute(Name = "ON")]
		on,
	}

	[System.SerializableAttribute()]
	[System.Xml.Serialization.XmlTypeAttribute(TypeName = "WhiteBalance", Namespace = "http://www.onvif.org/ver10/schema")]
	public partial class WhiteBalance {

		private System.Xml.XmlAttribute[] _anyAttr;

		private WhiteBalanceMode _mode;

		private float _crGain;

		private float _cbGain;

		private System.Xml.XmlElement[] _any;

		[System.Xml.Serialization.XmlAnyAttributeAttribute()]
		public System.Xml.XmlAttribute[] anyAttr {
			get {
				return this._anyAttr;
			}
			set {
				this._anyAttr = value;
			}
		}

		/// <summary>
		/// Auto whitebalancing mode (auto/manual).
		/// </summary>
		/// <remarks>reqired, order 0</remarks>
		[System.Xml.Serialization.XmlElementAttribute("Mode", Namespace = "http://www.onvif.org/ver10/schema")]
		public WhiteBalanceMode mode {
			get {
				return this._mode;
			}
			set {
				this._mode = value;
			}
		}

		/// <summary>
		/// Rgain (unitless).
		/// </summary>
		/// <remarks>reqired, order 1</remarks>
		[System.Xml.Serialization.XmlElementAttribute("CrGain", Namespace = "http://www.onvif.org/ver10/schema", DataType = "float")]
		public float crGain {
			get {
				return this._crGain;
			}
			set {
				this._crGain = value;
			}
		}

		/// <summary>
		/// Bgain (unitless).
		/// </summary>
		/// <remarks>reqired, order 2</remarks>
		[System.Xml.Serialization.XmlElementAttribute("CbGain", Namespace = "http://www.onvif.org/ver10/schema", DataType = "float")]
		public float cbGain {
			get {
				return this._cbGain;
			}
			set {
				this._cbGain = value;
			}
		}

		/// <remarks>optional, order 3, namespace ##any</remarks>
		[System.Xml.Serialization.XmlAnyElementAttribute()]
		public System.Xml.XmlElement[] any {
			get {
				return this._any;
			}
			set {
				this._any = value;
			}
		}
	}

	[System.SerializableAttribute()]
	[System.Xml.Serialization.XmlTypeAttribute(TypeName = "WhiteBalanceMode", Namespace = "http://www.onvif.org/ver10/schema")]
	public enum WhiteBalanceMode {

		[System.Xml.Serialization.XmlEnumAttribute(Name = "AUTO")]
		auto,

		[System.Xml.Serialization.XmlEnumAttribute(Name = "MANUAL")]
		manual,
	}

	[System.SerializableAttribute()]
	[System.Xml.Serialization.XmlTypeAttribute(TypeName = "ImagingSettingsExtension", Namespace = "http://www.onvif.org/ver10/schema")]
	public partial class ImagingSettingsExtension {

		private System.Xml.XmlElement[] _any;

		/// <remarks>optional, order 0, namespace ##any</remarks>
		[System.Xml.Serialization.XmlAnyElementAttribute()]
		public System.Xml.XmlElement[] any {
			get {
				return this._any;
			}
			set {
				this._any = value;
			}
		}
	}

	[System.SerializableAttribute()]
	[System.Xml.Serialization.XmlTypeAttribute(TypeName = "VideoSourceExtension", Namespace = "http://www.onvif.org/ver10/schema")]
	public partial class VideoSourceExtension {

		private System.Xml.XmlElement[] _any;

		private ImagingSettings20 _imaging;

		private VideoSourceExtension2 _extension;

		/// <remarks>optional, order 0, namespace ##other</remarks>
		[System.Xml.Serialization.XmlAnyElementAttribute()]
		public System.Xml.XmlElement[] any {
			get {
				return this._any;
			}
			set {
				this._any = value;
			}
		}

		/// <summary>
		/// Optional configuration of the image sensor. To be used if imaging service 2.00 is supported.
		/// </summary>
		/// <remarks>optional, order 1</remarks>
		[System.Xml.Serialization.XmlElementAttribute("Imaging", Namespace = "http://www.onvif.org/ver10/schema")]
		public ImagingSettings20 imaging {
			get {
				return this._imaging;
			}
			set {
				this._imaging = value;
			}
		}

		/// <remarks>optional, order 2</remarks>
		[System.Xml.Serialization.XmlElementAttribute("Extension", Namespace = "http://www.onvif.org/ver10/schema")]
		public VideoSourceExtension2 extension {
			get {
				return this._extension;
			}
			set {
				this._extension = value;
			}
		}
	}

	/// <summary>
	/// Type describing the ImagingSettings of a VideoSource. The supported options and ranges can be obtained via the GetOptions command.
	/// </summary>
	[System.SerializableAttribute()]
	[System.Xml.Serialization.XmlTypeAttribute(TypeName = "ImagingSettings20", Namespace = "http://www.onvif.org/ver10/schema")]
	public partial class ImagingSettings20 {

		private System.Xml.XmlAttribute[] _anyAttr;

		private BacklightCompensation20 _backlightCompensation;

		private float _brightness;

		private bool _brightnessSpecified;

		private float _colorSaturation;

		private bool _colorSaturationSpecified;

		private float _contrast;

		private bool _contrastSpecified;

		private Exposure20 _exposure;

		private FocusConfiguration20 _focus;

		private IrCutFilterMode _irCutFilter;

		private bool _irCutFilterSpecified;

		private float _sharpness;

		private bool _sharpnessSpecified;

		private WideDynamicRange20 _wideDynamicRange;

		private WhiteBalance20 _whiteBalance;

		private ImagingSettingsExtension20 _extension;

		[System.Xml.Serialization.XmlAnyAttributeAttribute()]
		public System.Xml.XmlAttribute[] anyAttr {
			get {
				return this._anyAttr;
			}
			set {
				this._anyAttr = value;
			}
		}

		/// <summary>
		/// Enabled/disabled BLC mode (on/off).
		/// </summary>
		/// <remarks>optional, order 0</remarks>
		[System.Xml.Serialization.XmlElementAttribute("BacklightCompensation", Namespace = "http://www.onvif.org/ver10/schema")]
		public BacklightCompensation20 backlightCompensation {
			get {
				return this._backlightCompensation;
			}
			set {
				this._backlightCompensation = value;
			}
		}

		/// <summary>
		/// Image brightness (unit unspecified).
		/// </summary>
		/// <remarks>optional, order 1</remarks>
		[System.Xml.Serialization.XmlElementAttribute("Brightness", Namespace = "http://www.onvif.org/ver10/schema", DataType = "float")]
		public float brightness {
			get {
				return this._brightness;
			}
			set {
				this._brightness = value;
			}
		}

		[System.Xml.Serialization.XmlIgnoreAttribute()]
		public bool brightnessSpecified {
			get {
				return this._brightnessSpecified;
			}
			set {
				this._brightnessSpecified = value;
			}
		}

		/// <summary>
		/// Color saturation of the image (unit unspecified).
		/// </summary>
		/// <remarks>optional, order 2</remarks>
		[System.Xml.Serialization.XmlElementAttribute("ColorSaturation", Namespace = "http://www.onvif.org/ver10/schema", DataType = "float")]
		public float colorSaturation {
			get {
				return this._colorSaturation;
			}
			set {
				this._colorSaturation = value;
			}
		}

		[System.Xml.Serialization.XmlIgnoreAttribute()]
		public bool colorSaturationSpecified {
			get {
				return this._colorSaturationSpecified;
			}
			set {
				this._colorSaturationSpecified = value;
			}
		}

		/// <summary>
		/// Contrast of the image (unit unspecified).
		/// </summary>
		/// <remarks>optional, order 3</remarks>
		[System.Xml.Serialization.XmlElementAttribute("Contrast", Namespace = "http://www.onvif.org/ver10/schema", DataType = "float")]
		public float contrast {
			get {
				return this._contrast;
			}
			set {
				this._contrast = value;
			}
		}

		[System.Xml.Serialization.XmlIgnoreAttribute()]
		public bool contrastSpecified {
			get {
				return this._contrastSpecified;
			}
			set {
				this._contrastSpecified = value;
			}
		}

		/// <summary>
		/// Exposure mode of the device.
		/// </summary>
		/// <remarks>optional, order 4</remarks>
		[System.Xml.Serialization.XmlElementAttribute("Exposure", Namespace = "http://www.onvif.org/ver10/schema")]
		public Exposure20 exposure {
			get {
				return this._exposure;
			}
			set {
				this._exposure = value;
			}
		}

		/// <summary>
		/// Focus configuration.
		/// </summary>
		/// <remarks>optional, order 5</remarks>
		[System.Xml.Serialization.XmlElementAttribute("Focus", Namespace = "http://www.onvif.org/ver10/schema")]
		public FocusConfiguration20 focus {
			get {
				return this._focus;
			}
			set {
				this._focus = value;
			}
		}

		/// <summary>
		/// Infrared Cutoff Filter settings.
		/// </summary>
		/// <remarks>optional, order 6</remarks>
		[System.Xml.Serialization.XmlElementAttribute("IrCutFilter", Namespace = "http://www.onvif.org/ver10/schema")]
		public IrCutFilterMode irCutFilter {
			get {
				return this._irCutFilter;
			}
			set {
				this._irCutFilter = value;
			}
		}

		[System.Xml.Serialization.XmlIgnoreAttribute()]
		public bool irCutFilterSpecified {
			get {
				return this._irCutFilterSpecified;
			}
			set {
				this._irCutFilterSpecified = value;
			}
		}

		/// <summary>
		/// Sharpness of the Video image.
		/// </summary>
		/// <remarks>optional, order 7</remarks>
		[System.Xml.Serialization.XmlElementAttribute("Sharpness", Namespace = "http://www.onvif.org/ver10/schema", DataType = "float")]
		public float sharpness {
			get {
				return this._sharpness;
			}
			set {
				this._sharpness = value;
			}
		}

		[System.Xml.Serialization.XmlIgnoreAttribute()]
		public bool sharpnessSpecified {
			get {
				return this._sharpnessSpecified;
			}
			set {
				this._sharpnessSpecified = value;
			}
		}

		/// <summary>
		/// WDR settings.
		/// </summary>
		/// <remarks>optional, order 8</remarks>
		[System.Xml.Serialization.XmlElementAttribute("WideDynamicRange", Namespace = "http://www.onvif.org/ver10/schema")]
		public WideDynamicRange20 wideDynamicRange {
			get {
				return this._wideDynamicRange;
			}
			set {
				this._wideDynamicRange = value;
			}
		}

		/// <summary>
		/// White balance settings.
		/// </summary>
		/// <remarks>optional, order 9</remarks>
		[System.Xml.Serialization.XmlElementAttribute("WhiteBalance", Namespace = "http://www.onvif.org/ver10/schema")]
		public WhiteBalance20 whiteBalance {
			get {
				return this._whiteBalance;
			}
			set {
				this._whiteBalance = value;
			}
		}

		/// <remarks>optional, order 10</remarks>
		[System.Xml.Serialization.XmlElementAttribute("Extension", Namespace = "http://www.onvif.org/ver10/schema")]
		public ImagingSettingsExtension20 extension {
			get {
				return this._extension;
			}
			set {
				this._extension = value;
			}
		}
	}

	/// <summary>
	/// Type describing whether BLC mode is enabled or disabled (on/off).
	/// </summary>
	[System.SerializableAttribute()]
	[System.Xml.Serialization.XmlTypeAttribute(TypeName = "BacklightCompensation20", Namespace = "http://www.onvif.org/ver10/schema")]
	public partial class BacklightCompensation20 {

		private BacklightCompensationMode _mode;

		private float _level;

		private bool _levelSpecified;

		/// <summary>
		/// Backlight compensation mode (on/off).
		/// </summary>
		/// <remarks>reqired, order 0</remarks>
		[System.Xml.Serialization.XmlElementAttribute("Mode", Namespace = "http://www.onvif.org/ver10/schema")]
		public BacklightCompensationMode mode {
			get {
				return this._mode;
			}
			set {
				this._mode = value;
			}
		}

		/// <summary>
		/// Optional level parameter (unit unspecified).
		/// </summary>
		/// <remarks>optional, order 1</remarks>
		[System.Xml.Serialization.XmlElementAttribute("Level", Namespace = "http://www.onvif.org/ver10/schema", DataType = "float")]
		public float level {
			get {
				return this._level;
			}
			set {
				this._level = value;
			}
		}

		[System.Xml.Serialization.XmlIgnoreAttribute()]
		public bool levelSpecified {
			get {
				return this._levelSpecified;
			}
			set {
				this._levelSpecified = value;
			}
		}
	}

	/// <summary>
	/// Type describing the exposure settings.
	/// </summary>
	[System.SerializableAttribute()]
	[System.Xml.Serialization.XmlTypeAttribute(TypeName = "Exposure20", Namespace = "http://www.onvif.org/ver10/schema")]
	public partial class Exposure20 {

		private ExposureMode _mode;

		private ExposurePriority _priority;

		private bool _prioritySpecified;

		private Rectangle _window;

		private float _minExposureTime;

		private bool _minExposureTimeSpecified;

		private float _maxExposureTime;

		private bool _maxExposureTimeSpecified;

		private float _minGain;

		private bool _minGainSpecified;

		private float _maxGain;

		private bool _maxGainSpecified;

		private float _minIris;

		private bool _minIrisSpecified;

		private float _maxIris;

		private bool _maxIrisSpecified;

		private float _exposureTime;

		private bool _exposureTimeSpecified;

		private float _gain;

		private bool _gainSpecified;

		private float _iris;

		private bool _irisSpecified;

		/// <summary>
		/// Exposure Mode
		/// </summary>
		/// <remarks>reqired, order 0</remarks>
		[System.Xml.Serialization.XmlElementAttribute("Mode", Namespace = "http://www.onvif.org/ver10/schema")]
		public ExposureMode mode {
			get {
				return this._mode;
			}
			set {
				this._mode = value;
			}
		}

		/// <summary>
		/// The exposure priority mode (low noise/framerate).
		/// </summary>
		/// <remarks>optional, order 1</remarks>
		[System.Xml.Serialization.XmlElementAttribute("Priority", Namespace = "http://www.onvif.org/ver10/schema")]
		public ExposurePriority priority {
			get {
				return this._priority;
			}
			set {
				this._priority = value;
			}
		}

		[System.Xml.Serialization.XmlIgnoreAttribute()]
		public bool prioritySpecified {
			get {
				return this._prioritySpecified;
			}
			set {
				this._prioritySpecified = value;
			}
		}

		/// <summary>
		/// Rectangular exposure mask.
		/// </summary>
		/// <remarks>optional, order 2</remarks>
		[System.Xml.Serialization.XmlElementAttribute("Window", Namespace = "http://www.onvif.org/ver10/schema")]
		public Rectangle window {
			get {
				return this._window;
			}
			set {
				this._window = value;
			}
		}

		/// <summary>
		/// Minimum value of exposure time range allowed to be used by the algorithm.
		/// </summary>
		/// <remarks>optional, order 3</remarks>
		[System.Xml.Serialization.XmlElementAttribute("MinExposureTime", Namespace = "http://www.onvif.org/ver10/schema", DataType = "float")]
		public float minExposureTime {
			get {
				return this._minExposureTime;
			}
			set {
				this._minExposureTime = value;
			}
		}

		[System.Xml.Serialization.XmlIgnoreAttribute()]
		public bool minExposureTimeSpecified {
			get {
				return this._minExposureTimeSpecified;
			}
			set {
				this._minExposureTimeSpecified = value;
			}
		}

		/// <summary>
		/// Maximum value of exposure time range allowed to be used by the algorithm.
		/// </summary>
		/// <remarks>optional, order 4</remarks>
		[System.Xml.Serialization.XmlElementAttribute("MaxExposureTime", Namespace = "http://www.onvif.org/ver10/schema", DataType = "float")]
		public float maxExposureTime {
			get {
				return this._maxExposureTime;
			}
			set {
				this._maxExposureTime = value;
			}
		}

		[System.Xml.Serialization.XmlIgnoreAttribute()]
		public bool maxExposureTimeSpecified {
			get {
				return this._maxExposureTimeSpecified;
			}
			set {
				this._maxExposureTimeSpecified = value;
			}
		}

		/// <summary>
		/// Minimum value of the sensor gain range that is allowed to be used by the algorithm.
		/// </summary>
		/// <remarks>optional, order 5</remarks>
		[System.Xml.Serialization.XmlElementAttribute("MinGain", Namespace = "http://www.onvif.org/ver10/schema", DataType = "float")]
		public float minGain {
			get {
				return this._minGain;
			}
			set {
				this._minGain = value;
			}
		}

		[System.Xml.Serialization.XmlIgnoreAttribute()]
		public bool minGainSpecified {
			get {
				return this._minGainSpecified;
			}
			set {
				this._minGainSpecified = value;
			}
		}

		/// <summary>
		/// Maximum value of the sensor gain range that is allowed to be used by the algorithm.
		/// </summary>
		/// <remarks>optional, order 6</remarks>
		[System.Xml.Serialization.XmlElementAttribute("MaxGain", Namespace = "http://www.onvif.org/ver10/schema", DataType = "float")]
		public float maxGain {
			get {
				return this._maxGain;
			}
			set {
				this._maxGain = value;
			}
		}

		[System.Xml.Serialization.XmlIgnoreAttribute()]
		public bool maxGainSpecified {
			get {
				return this._maxGainSpecified;
			}
			set {
				this._maxGainSpecified = value;
			}
		}

		/// <summary>
		/// Minimum value of the iris range allowed to be used by the algorithm.
		/// </summary>
		/// <remarks>optional, order 7</remarks>
		[System.Xml.Serialization.XmlElementAttribute("MinIris", Namespace = "http://www.onvif.org/ver10/schema", DataType = "float")]
		public float minIris {
			get {
				return this._minIris;
			}
			set {
				this._minIris = value;
			}
		}

		[System.Xml.Serialization.XmlIgnoreAttribute()]
		public bool minIrisSpecified {
			get {
				return this._minIrisSpecified;
			}
			set {
				this._minIrisSpecified = value;
			}
		}

		/// <summary>
		/// Maximum value of the iris range allowed to be used by the algorithm.
		/// </summary>
		/// <remarks>optional, order 8</remarks>
		[System.Xml.Serialization.XmlElementAttribute("MaxIris", Namespace = "http://www.onvif.org/ver10/schema", DataType = "float")]
		public float maxIris {
			get {
				return this._maxIris;
			}
			set {
				this._maxIris = value;
			}
		}

		[System.Xml.Serialization.XmlIgnoreAttribute()]
		public bool maxIrisSpecified {
			get {
				return this._maxIrisSpecified;
			}
			set {
				this._maxIrisSpecified = value;
			}
		}

		/// <summary>
		/// The fixed exposure time used by the image sensor (μs).
		/// </summary>
		/// <remarks>optional, order 9</remarks>
		[System.Xml.Serialization.XmlElementAttribute("ExposureTime", Namespace = "http://www.onvif.org/ver10/schema", DataType = "float")]
		public float exposureTime {
			get {
				return this._exposureTime;
			}
			set {
				this._exposureTime = value;
			}
		}

		[System.Xml.Serialization.XmlIgnoreAttribute()]
		public bool exposureTimeSpecified {
			get {
				return this._exposureTimeSpecified;
			}
			set {
				this._exposureTimeSpecified = value;
			}
		}

		/// <summary>
		/// The fixed gain used by the image sensor (dB).
		/// </summary>
		/// <remarks>optional, order 10</remarks>
		[System.Xml.Serialization.XmlElementAttribute("Gain", Namespace = "http://www.onvif.org/ver10/schema", DataType = "float")]
		public float gain {
			get {
				return this._gain;
			}
			set {
				this._gain = value;
			}
		}

		[System.Xml.Serialization.XmlIgnoreAttribute()]
		public bool gainSpecified {
			get {
				return this._gainSpecified;
			}
			set {
				this._gainSpecified = value;
			}
		}

		/// <summary>
		/// The fixed attenuation of input light affected by the iris (dB). 0dB maps to a fully opened iris.
		/// </summary>
		/// <remarks>optional, order 11</remarks>
		[System.Xml.Serialization.XmlElementAttribute("Iris", Namespace = "http://www.onvif.org/ver10/schema", DataType = "float")]
		public float iris {
			get {
				return this._iris;
			}
			set {
				this._iris = value;
			}
		}

		[System.Xml.Serialization.XmlIgnoreAttribute()]
		public bool irisSpecified {
			get {
				return this._irisSpecified;
			}
			set {
				this._irisSpecified = value;
			}
		}
	}

	[System.SerializableAttribute()]
	[System.Xml.Serialization.XmlTypeAttribute(TypeName = "FocusConfiguration20", Namespace = "http://www.onvif.org/ver10/schema")]
	public partial class FocusConfiguration20 {

		private System.Xml.XmlAttribute[] _anyAttr;

		private AutoFocusMode _autoFocusMode;

		private float _defaultSpeed;

		private bool _defaultSpeedSpecified;

		private float _nearLimit;

		private bool _nearLimitSpecified;

		private float _farLimit;

		private bool _farLimitSpecified;

		private FocusConfiguration20Extension _extension;

		[System.Xml.Serialization.XmlAnyAttributeAttribute()]
		public System.Xml.XmlAttribute[] anyAttr {
			get {
				return this._anyAttr;
			}
			set {
				this._anyAttr = value;
			}
		}

		/// <summary>
		/// Mode of auto fucus.
		/// </summary>
		/// <remarks>reqired, order 0</remarks>
		[System.Xml.Serialization.XmlElementAttribute("AutoFocusMode", Namespace = "http://www.onvif.org/ver10/schema")]
		public AutoFocusMode autoFocusMode {
			get {
				return this._autoFocusMode;
			}
			set {
				this._autoFocusMode = value;
			}
		}

		/// <remarks>optional, order 1</remarks>
		[System.Xml.Serialization.XmlElementAttribute("DefaultSpeed", Namespace = "http://www.onvif.org/ver10/schema", DataType = "float")]
		public float defaultSpeed {
			get {
				return this._defaultSpeed;
			}
			set {
				this._defaultSpeed = value;
			}
		}

		[System.Xml.Serialization.XmlIgnoreAttribute()]
		public bool defaultSpeedSpecified {
			get {
				return this._defaultSpeedSpecified;
			}
			set {
				this._defaultSpeedSpecified = value;
			}
		}

		/// <summary>
		/// Parameter to set autofocus near limit (unit: meter).
		/// </summary>
		/// <remarks>optional, order 2</remarks>
		[System.Xml.Serialization.XmlElementAttribute("NearLimit", Namespace = "http://www.onvif.org/ver10/schema", DataType = "float")]
		public float nearLimit {
			get {
				return this._nearLimit;
			}
			set {
				this._nearLimit = value;
			}
		}

		[System.Xml.Serialization.XmlIgnoreAttribute()]
		public bool nearLimitSpecified {
			get {
				return this._nearLimitSpecified;
			}
			set {
				this._nearLimitSpecified = value;
			}
		}

		/// <summary>
		/// Parameter to set autofocus far limit (unit: meter).
		/// </summary>
		/// <remarks>optional, order 3</remarks>
		[System.Xml.Serialization.XmlElementAttribute("FarLimit", Namespace = "http://www.onvif.org/ver10/schema", DataType = "float")]
		public float farLimit {
			get {
				return this._farLimit;
			}
			set {
				this._farLimit = value;
			}
		}

		[System.Xml.Serialization.XmlIgnoreAttribute()]
		public bool farLimitSpecified {
			get {
				return this._farLimitSpecified;
			}
			set {
				this._farLimitSpecified = value;
			}
		}

		/// <remarks>optional, order 4</remarks>
		[System.Xml.Serialization.XmlElementAttribute("Extension", Namespace = "http://www.onvif.org/ver10/schema")]
		public FocusConfiguration20Extension extension {
			get {
				return this._extension;
			}
			set {
				this._extension = value;
			}
		}
	}

	[System.SerializableAttribute()]
	[System.Xml.Serialization.XmlTypeAttribute(TypeName = "FocusConfiguration20Extension", Namespace = "http://www.onvif.org/ver10/schema")]
	public partial class FocusConfiguration20Extension {

		private System.Xml.XmlElement[] _any;

		/// <remarks>optional, order 0, namespace ##any</remarks>
		[System.Xml.Serialization.XmlAnyElementAttribute()]
		public System.Xml.XmlElement[] any {
			get {
				return this._any;
			}
			set {
				this._any = value;
			}
		}
	}

	/// <summary>
	/// Type describing whether WDR mode is enabled or disabled (on/off).
	/// </summary>
	[System.SerializableAttribute()]
	[System.Xml.Serialization.XmlTypeAttribute(TypeName = "WideDynamicRange20", Namespace = "http://www.onvif.org/ver10/schema")]
	public partial class WideDynamicRange20 {

		private WideDynamicMode _mode;

		private float _level;

		private bool _levelSpecified;

		/// <summary>
		/// Wide dynamic range mode (on/off).
		/// </summary>
		/// <remarks>reqired, order 0</remarks>
		[System.Xml.Serialization.XmlElementAttribute("Mode", Namespace = "http://www.onvif.org/ver10/schema")]
		public WideDynamicMode mode {
			get {
				return this._mode;
			}
			set {
				this._mode = value;
			}
		}

		/// <summary>
		/// Optional level parameter (unit unspecified).
		/// </summary>
		/// <remarks>optional, order 1</remarks>
		[System.Xml.Serialization.XmlElementAttribute("Level", Namespace = "http://www.onvif.org/ver10/schema", DataType = "float")]
		public float level {
			get {
				return this._level;
			}
			set {
				this._level = value;
			}
		}

		[System.Xml.Serialization.XmlIgnoreAttribute()]
		public bool levelSpecified {
			get {
				return this._levelSpecified;
			}
			set {
				this._levelSpecified = value;
			}
		}
	}

	[System.SerializableAttribute()]
	[System.Xml.Serialization.XmlTypeAttribute(TypeName = "WhiteBalance20", Namespace = "http://www.onvif.org/ver10/schema")]
	public partial class WhiteBalance20 {

		private System.Xml.XmlAttribute[] _anyAttr;

		private WhiteBalanceMode _mode;

		private float _crGain;

		private bool _crGainSpecified;

		private float _cbGain;

		private bool _cbGainSpecified;

		private WhiteBalance20Extension _extension;

		[System.Xml.Serialization.XmlAnyAttributeAttribute()]
		public System.Xml.XmlAttribute[] anyAttr {
			get {
				return this._anyAttr;
			}
			set {
				this._anyAttr = value;
			}
		}

		/// <summary>
		/// 'AUTO' or 'MANUAL'
		/// </summary>
		/// <remarks>reqired, order 0</remarks>
		[System.Xml.Serialization.XmlElementAttribute("Mode", Namespace = "http://www.onvif.org/ver10/schema")]
		public WhiteBalanceMode mode {
			get {
				return this._mode;
			}
			set {
				this._mode = value;
			}
		}

		/// <summary>
		/// Rgain (unitless).
		/// </summary>
		/// <remarks>optional, order 1</remarks>
		[System.Xml.Serialization.XmlElementAttribute("CrGain", Namespace = "http://www.onvif.org/ver10/schema", DataType = "float")]
		public float crGain {
			get {
				return this._crGain;
			}
			set {
				this._crGain = value;
			}
		}

		[System.Xml.Serialization.XmlIgnoreAttribute()]
		public bool crGainSpecified {
			get {
				return this._crGainSpecified;
			}
			set {
				this._crGainSpecified = value;
			}
		}

		/// <summary>
		/// Bgain (unitless).
		/// </summary>
		/// <remarks>optional, order 2</remarks>
		[System.Xml.Serialization.XmlElementAttribute("CbGain", Namespace = "http://www.onvif.org/ver10/schema", DataType = "float")]
		public float cbGain {
			get {
				return this._cbGain;
			}
			set {
				this._cbGain = value;
			}
		}

		[System.Xml.Serialization.XmlIgnoreAttribute()]
		public bool cbGainSpecified {
			get {
				return this._cbGainSpecified;
			}
			set {
				this._cbGainSpecified = value;
			}
		}

		/// <remarks>optional, order 3</remarks>
		[System.Xml.Serialization.XmlElementAttribute("Extension", Namespace = "http://www.onvif.org/ver10/schema")]
		public WhiteBalance20Extension extension {
			get {
				return this._extension;
			}
			set {
				this._extension = value;
			}
		}
	}

	[System.SerializableAttribute()]
	[System.Xml.Serialization.XmlTypeAttribute(TypeName = "WhiteBalance20Extension", Namespace = "http://www.onvif.org/ver10/schema")]
	public partial class WhiteBalance20Extension {

		private System.Xml.XmlElement[] _any;

		/// <remarks>optional, order 0, namespace ##any</remarks>
		[System.Xml.Serialization.XmlAnyElementAttribute()]
		public System.Xml.XmlElement[] any {
			get {
				return this._any;
			}
			set {
				this._any = value;
			}
		}
	}

	[System.SerializableAttribute()]
	[System.Xml.Serialization.XmlTypeAttribute(TypeName = "ImagingSettingsExtension20", Namespace = "http://www.onvif.org/ver10/schema")]
	public partial class ImagingSettingsExtension20 {

		private System.Xml.XmlElement[] _any;

		private ImageStabilization _imageStabilization;

		private ImagingSettingsExtension202 _extension;

		/// <remarks>optional, order 0, namespace ##other</remarks>
		[System.Xml.Serialization.XmlAnyElementAttribute()]
		public System.Xml.XmlElement[] any {
			get {
				return this._any;
			}
			set {
				this._any = value;
			}
		}

		/// <summary>
		/// Optional element to configure Image Stabilization feature.
		/// </summary>
		/// <remarks>optional, order 1</remarks>
		[System.Xml.Serialization.XmlElementAttribute("ImageStabilization", Namespace = "http://www.onvif.org/ver10/schema")]
		public ImageStabilization imageStabilization {
			get {
				return this._imageStabilization;
			}
			set {
				this._imageStabilization = value;
			}
		}

		/// <remarks>optional, order 2</remarks>
		[System.Xml.Serialization.XmlElementAttribute("Extension", Namespace = "http://www.onvif.org/ver10/schema")]
		public ImagingSettingsExtension202 extension {
			get {
				return this._extension;
			}
			set {
				this._extension = value;
			}
		}
	}

	[System.SerializableAttribute()]
	[System.Xml.Serialization.XmlTypeAttribute(TypeName = "ImageStabilization", Namespace = "http://www.onvif.org/ver10/schema")]
	public partial class ImageStabilization {

		private System.Xml.XmlAttribute[] _anyAttr;

		private ImageStabilizationMode _mode;

		private float _level;

		private bool _levelSpecified;

		private ImageStabilizationExtension _extension;

		[System.Xml.Serialization.XmlAnyAttributeAttribute()]
		public System.Xml.XmlAttribute[] anyAttr {
			get {
				return this._anyAttr;
			}
			set {
				this._anyAttr = value;
			}
		}

		/// <summary>
		/// Parameter to enable/disable Image Stabilization feature.
		/// </summary>
		/// <remarks>reqired, order 0</remarks>
		[System.Xml.Serialization.XmlElementAttribute("Mode", Namespace = "http://www.onvif.org/ver10/schema")]
		public ImageStabilizationMode mode {
			get {
				return this._mode;
			}
			set {
				this._mode = value;
			}
		}

		/// <summary>
		/// Optional level parameter (unit unspecified)
		/// </summary>
		/// <remarks>optional, order 1</remarks>
		[System.Xml.Serialization.XmlElementAttribute("Level", Namespace = "http://www.onvif.org/ver10/schema", DataType = "float")]
		public float level {
			get {
				return this._level;
			}
			set {
				this._level = value;
			}
		}

		[System.Xml.Serialization.XmlIgnoreAttribute()]
		public bool levelSpecified {
			get {
				return this._levelSpecified;
			}
			set {
				this._levelSpecified = value;
			}
		}

		/// <remarks>optional, order 2</remarks>
		[System.Xml.Serialization.XmlElementAttribute("Extension", Namespace = "http://www.onvif.org/ver10/schema")]
		public ImageStabilizationExtension extension {
			get {
				return this._extension;
			}
			set {
				this._extension = value;
			}
		}
	}

	[System.SerializableAttribute()]
	[System.Xml.Serialization.XmlTypeAttribute(TypeName = "ImageStabilizationMode", Namespace = "http://www.onvif.org/ver10/schema")]
	public enum ImageStabilizationMode {

		[System.Xml.Serialization.XmlEnumAttribute(Name = "OFF")]
		off,

		[System.Xml.Serialization.XmlEnumAttribute(Name = "ON")]
		on,

		[System.Xml.Serialization.XmlEnumAttribute(Name = "AUTO")]
		auto,

		[System.Xml.Serialization.XmlEnumAttribute(Name = "Extended")]
		extended,
	}

	[System.SerializableAttribute()]
	[System.Xml.Serialization.XmlTypeAttribute(TypeName = "ImageStabilizationExtension", Namespace = "http://www.onvif.org/ver10/schema")]
	public partial class ImageStabilizationExtension {

		private System.Xml.XmlElement[] _any;

		/// <remarks>optional, order 0, namespace ##any</remarks>
		[System.Xml.Serialization.XmlAnyElementAttribute()]
		public System.Xml.XmlElement[] any {
			get {
				return this._any;
			}
			set {
				this._any = value;
			}
		}
	}

	[System.SerializableAttribute()]
	[System.Xml.Serialization.XmlTypeAttribute(TypeName = "ImagingSettingsExtension202", Namespace = "http://www.onvif.org/ver10/schema")]
	public partial class ImagingSettingsExtension202 {

		private System.Xml.XmlElement[] _any;

		/// <remarks>optional, order 0, namespace ##targetNamespace</remarks>
		[System.Xml.Serialization.XmlAnyElementAttribute(Namespace = "http://www.onvif.org/ver10/schema")]
		public System.Xml.XmlElement[] any {
			get {
				return this._any;
			}
			set {
				this._any = value;
			}
		}
	}

	[System.SerializableAttribute()]
	[System.Xml.Serialization.XmlTypeAttribute(TypeName = "VideoSourceExtension2", Namespace = "http://www.onvif.org/ver10/schema")]
	public partial class VideoSourceExtension2 {

		private System.Xml.XmlElement[] _any;

		/// <remarks>optional, order 0, namespace ##targetNamespace</remarks>
		[System.Xml.Serialization.XmlAnyElementAttribute(Namespace = "http://www.onvif.org/ver10/schema")]
		public System.Xml.XmlElement[] any {
			get {
				return this._any;
			}
			set {
				this._any = value;
			}
		}
	}

	/// <summary>
	/// Representation of a physical audio input.
	/// </summary>
	[System.SerializableAttribute()]
	[System.Xml.Serialization.XmlTypeAttribute(TypeName = "AudioSource", Namespace = "http://www.onvif.org/ver10/schema")]
	public partial class AudioSource {

		private string _token;

		private System.Xml.XmlAttribute[] _anyAttr;

		private int _channels;

		private System.Xml.XmlElement[] _any;

		/// <summary>
		/// Unique identifier referencing the physical entity.
		/// </summary>
		[System.Xml.Serialization.XmlAttributeAttribute("token")]
		public string token {
			get {
				return this._token;
			}
			set {
				this._token = value;
			}
		}

		[System.Xml.Serialization.XmlAnyAttributeAttribute()]
		public System.Xml.XmlAttribute[] anyAttr {
			get {
				return this._anyAttr;
			}
			set {
				this._anyAttr = value;
			}
		}

		/// <summary>
		/// number of available audio channels. (1: mono, 2: stereo)
		/// </summary>
		/// <remarks>reqired, order 0</remarks>
		[System.Xml.Serialization.XmlElementAttribute("Channels", Namespace = "http://www.onvif.org/ver10/schema", DataType = "int")]
		public int channels {
			get {
				return this._channels;
			}
			set {
				this._channels = value;
			}
		}

		/// <remarks>optional, order 1, namespace ##any</remarks>
		[System.Xml.Serialization.XmlAnyElementAttribute()]
		public System.Xml.XmlElement[] any {
			get {
				return this._any;
			}
			set {
				this._any = value;
			}
		}
	}

	/// <summary>
	/// A media profile consists of a set of media configurations. Media profiles are used by a client
	/// to configure properties of a media stream from an NVT.
	/// An NVT shall provide at least one media profile at boot. An NVT should provide "ready to use"
	/// profiles for the most common media configurations that the device offers.
	/// A profile consists of a set of interconnected configuration entities. Configurations are provided
	/// by the NVT and can be either static or created dynamically by the NVT. For example, the
	/// dynamic configurations can be created by the NVT depending on current available encoding
	/// resources.
	/// </summary>
	[System.SerializableAttribute()]
	[System.Xml.Serialization.XmlTypeAttribute(TypeName = "Profile", Namespace = "http://www.onvif.org/ver10/schema")]
	public partial class Profile {

		private string _token;

		private bool _fixed;

		private bool _fixedSpecified;

		private System.Xml.XmlAttribute[] _anyAttr;

		private string _name;

		private VideoSourceConfiguration _videoSourceConfiguration;

		private AudioSourceConfiguration _audioSourceConfiguration;

		private VideoEncoderConfiguration _videoEncoderConfiguration;

		private AudioEncoderConfiguration _audioEncoderConfiguration;

		private VideoAnalyticsConfiguration _videoAnalyticsConfiguration;

		private PTZConfiguration _ptzConfiguration;

		private MetadataConfiguration _metadataConfiguration;

		private ProfileExtension _extension;

		/// <summary>
		/// Unique identifier of the profile.
		/// </summary>
		[System.Xml.Serialization.XmlAttributeAttribute("token")]
		public string token {
			get {
				return this._token;
			}
			set {
				this._token = value;
			}
		}

		/// <summary>
		/// A value of true signals that the profile cannot be deleted. Default is false.
		/// </summary>
		[System.Xml.Serialization.XmlAttributeAttribute("fixed", DataType = "boolean")]
		public bool @fixed {
			get {
				return this._fixed;
			}
			set {
				this._fixed = value;
			}
		}

		[System.Xml.Serialization.XmlIgnoreAttribute()]
		public bool fixedSpecified {
			get {
				return this._fixedSpecified;
			}
			set {
				this._fixedSpecified = value;
			}
		}

		[System.Xml.Serialization.XmlAnyAttributeAttribute()]
		public System.Xml.XmlAttribute[] anyAttr {
			get {
				return this._anyAttr;
			}
			set {
				this._anyAttr = value;
			}
		}

		/// <summary>
		/// User readable name of the profile.
		/// </summary>
		/// <remarks>reqired, order 0</remarks>
		[System.Xml.Serialization.XmlElementAttribute("Name", Namespace = "http://www.onvif.org/ver10/schema", DataType = "string")]
		public string name {
			get {
				return this._name;
			}
			set {
				this._name = value;
			}
		}

		/// <summary>
		/// Optional configuration of the Video input.
		/// </summary>
		/// <remarks>optional, order 1</remarks>
		[System.Xml.Serialization.XmlElementAttribute("VideoSourceConfiguration", Namespace = "http://www.onvif.org/ver10/schema")]
		public VideoSourceConfiguration videoSourceConfiguration {
			get {
				return this._videoSourceConfiguration;
			}
			set {
				this._videoSourceConfiguration = value;
			}
		}

		/// <summary>
		/// Optional configuration of the Audio input.
		/// </summary>
		/// <remarks>optional, order 2</remarks>
		[System.Xml.Serialization.XmlElementAttribute("AudioSourceConfiguration", Namespace = "http://www.onvif.org/ver10/schema")]
		public AudioSourceConfiguration audioSourceConfiguration {
			get {
				return this._audioSourceConfiguration;
			}
			set {
				this._audioSourceConfiguration = value;
			}
		}

		/// <summary>
		/// Optional configuration of the Video encoder.
		/// </summary>
		/// <remarks>optional, order 3</remarks>
		[System.Xml.Serialization.XmlElementAttribute("VideoEncoderConfiguration", Namespace = "http://www.onvif.org/ver10/schema")]
		public VideoEncoderConfiguration videoEncoderConfiguration {
			get {
				return this._videoEncoderConfiguration;
			}
			set {
				this._videoEncoderConfiguration = value;
			}
		}

		/// <summary>
		/// Optional configuration of the Audio encoder.
		/// </summary>
		/// <remarks>optional, order 4</remarks>
		[System.Xml.Serialization.XmlElementAttribute("AudioEncoderConfiguration", Namespace = "http://www.onvif.org/ver10/schema")]
		public AudioEncoderConfiguration audioEncoderConfiguration {
			get {
				return this._audioEncoderConfiguration;
			}
			set {
				this._audioEncoderConfiguration = value;
			}
		}

		/// <summary>
		/// Optional configuration of the video analytics module and rule engine.
		/// </summary>
		/// <remarks>optional, order 5</remarks>
		[System.Xml.Serialization.XmlElementAttribute("VideoAnalyticsConfiguration", Namespace = "http://www.onvif.org/ver10/schema")]
		public VideoAnalyticsConfiguration videoAnalyticsConfiguration {
			get {
				return this._videoAnalyticsConfiguration;
			}
			set {
				this._videoAnalyticsConfiguration = value;
			}
		}

		/// <summary>
		/// Optional configuration of the pan tilt zoom unit.
		/// </summary>
		/// <remarks>optional, order 6</remarks>
		[System.Xml.Serialization.XmlElementAttribute("PTZConfiguration", Namespace = "http://www.onvif.org/ver10/schema")]
		public PTZConfiguration ptzConfiguration {
			get {
				return this._ptzConfiguration;
			}
			set {
				this._ptzConfiguration = value;
			}
		}

		/// <summary>
		/// Optional configuration of the metadata stream.
		/// </summary>
		/// <remarks>optional, order 7</remarks>
		[System.Xml.Serialization.XmlElementAttribute("MetadataConfiguration", Namespace = "http://www.onvif.org/ver10/schema")]
		public MetadataConfiguration metadataConfiguration {
			get {
				return this._metadataConfiguration;
			}
			set {
				this._metadataConfiguration = value;
			}
		}

		/// <summary>
		/// Extensions defined in ONVIF 2.0
		/// </summary>
		/// <remarks>optional, order 8</remarks>
		[System.Xml.Serialization.XmlElementAttribute("Extension", Namespace = "http://www.onvif.org/ver10/schema")]
		public ProfileExtension extension {
			get {
				return this._extension;
			}
			set {
				this._extension = value;
			}
		}
	}

	[System.SerializableAttribute()]
	[System.Xml.Serialization.XmlTypeAttribute(TypeName = "VideoSourceConfiguration", Namespace = "http://www.onvif.org/ver10/schema")]
	public partial class VideoSourceConfiguration {

		private string _token;

		private System.Xml.XmlAttribute[] _anyAttr;

		private string _name;

		private int _useCount;

		private string _sourceToken;

		private IntRectangle _bounds;

		private System.Xml.XmlElement[] _any;

		private VideoSourceConfigurationExtension _extension;

		/// <summary>
		/// Token that uniquely refernces this configuration. Length up to 64 characters.
		/// </summary>
		[System.Xml.Serialization.XmlAttributeAttribute("token")]
		public string token {
			get {
				return this._token;
			}
			set {
				this._token = value;
			}
		}

		[System.Xml.Serialization.XmlAnyAttributeAttribute()]
		public System.Xml.XmlAttribute[] anyAttr {
			get {
				return this._anyAttr;
			}
			set {
				this._anyAttr = value;
			}
		}

		/// <summary>
		/// User readable name. Length up to 64 characters.
		/// </summary>
		/// <remarks>reqired, order 0</remarks>
		[System.Xml.Serialization.XmlElementAttribute("Name", Namespace = "http://www.onvif.org/ver10/schema", DataType = "string")]
		public string name {
			get {
				return this._name;
			}
			set {
				this._name = value;
			}
		}

		/// <summary>
		/// Number of internal references currently using this configuration.
		/// This parameter is read-only and cannot be changed by a set request.
		/// For example the value increases if the configuration is added to a media profile or attached to a PaneConfiguration.
		/// </summary>
		/// <remarks>reqired, order 1</remarks>
		[System.Xml.Serialization.XmlElementAttribute("UseCount", Namespace = "http://www.onvif.org/ver10/schema", DataType = "int")]
		public int useCount {
			get {
				return this._useCount;
			}
			set {
				this._useCount = value;
			}
		}

		/// <summary>
		/// Reference to the physical input.
		/// </summary>
		/// <remarks>reqired, order 2</remarks>
		[System.Xml.Serialization.XmlElementAttribute("SourceToken", Namespace = "http://www.onvif.org/ver10/schema", DataType = "string")]
		public string sourceToken {
			get {
				return this._sourceToken;
			}
			set {
				this._sourceToken = value;
			}
		}

		/// <summary>
		/// Rectangle specifying the Video capturing area. The capturing area shall not be larger than the whole Video source area.
		/// </summary>
		/// <remarks>reqired, order 3</remarks>
		[System.Xml.Serialization.XmlElementAttribute("Bounds", Namespace = "http://www.onvif.org/ver10/schema")]
		public IntRectangle bounds {
			get {
				return this._bounds;
			}
			set {
				this._bounds = value;
			}
		}

		/// <remarks>optional, order 4, namespace ##other</remarks>
		[System.Xml.Serialization.XmlAnyElementAttribute()]
		public System.Xml.XmlElement[] any {
			get {
				return this._any;
			}
			set {
				this._any = value;
			}
		}

		/// <remarks>optional, order 5</remarks>
		[System.Xml.Serialization.XmlElementAttribute("Extension", Namespace = "http://www.onvif.org/ver10/schema")]
		public VideoSourceConfigurationExtension extension {
			get {
				return this._extension;
			}
			set {
				this._extension = value;
			}
		}
	}

	[System.SerializableAttribute()]
	[System.Xml.Serialization.XmlTypeAttribute(TypeName = "VideoSourceConfigurationExtension", Namespace = "http://www.onvif.org/ver10/schema")]
	public partial class VideoSourceConfigurationExtension {

		private Rotate _rotate;

		private VideoSourceConfigurationExtension2 _extension;

		/// <summary>
		/// Optional element to configure rotation of captured image.
		/// </summary>
		/// <remarks>optional, order 0</remarks>
		[System.Xml.Serialization.XmlElementAttribute("Rotate", Namespace = "http://www.onvif.org/ver10/schema")]
		public Rotate rotate {
			get {
				return this._rotate;
			}
			set {
				this._rotate = value;
			}
		}

		/// <remarks>optional, order 1</remarks>
		[System.Xml.Serialization.XmlElementAttribute("Extension", Namespace = "http://www.onvif.org/ver10/schema")]
		public VideoSourceConfigurationExtension2 extension {
			get {
				return this._extension;
			}
			set {
				this._extension = value;
			}
		}
	}

	[System.SerializableAttribute()]
	[System.Xml.Serialization.XmlTypeAttribute(TypeName = "Rotate", Namespace = "http://www.onvif.org/ver10/schema")]
	public partial class Rotate {

		private System.Xml.XmlAttribute[] _anyAttr;

		private RotateMode _mode;

		private int _degree;

		private bool _degreeSpecified;

		private RotateExtension _extension;

		[System.Xml.Serialization.XmlAnyAttributeAttribute()]
		public System.Xml.XmlAttribute[] anyAttr {
			get {
				return this._anyAttr;
			}
			set {
				this._anyAttr = value;
			}
		}

		/// <summary>
		/// Parameter to enable/disable Rotation feature.
		/// </summary>
		/// <remarks>reqired, order 0</remarks>
		[System.Xml.Serialization.XmlElementAttribute("Mode", Namespace = "http://www.onvif.org/ver10/schema")]
		public RotateMode mode {
			get {
				return this._mode;
			}
			set {
				this._mode = value;
			}
		}

		/// <summary>
		/// Optional parameter to configure how much degree of clockwise rotation of image  for On mode. Omitting this parameter for On mode means 180 degree rotation.
		/// </summary>
		/// <remarks>optional, order 1</remarks>
		[System.Xml.Serialization.XmlElementAttribute("Degree", Namespace = "http://www.onvif.org/ver10/schema", DataType = "int")]
		public int degree {
			get {
				return this._degree;
			}
			set {
				this._degree = value;
			}
		}

		[System.Xml.Serialization.XmlIgnoreAttribute()]
		public bool degreeSpecified {
			get {
				return this._degreeSpecified;
			}
			set {
				this._degreeSpecified = value;
			}
		}

		/// <remarks>optional, order 2</remarks>
		[System.Xml.Serialization.XmlElementAttribute("Extension", Namespace = "http://www.onvif.org/ver10/schema")]
		public RotateExtension extension {
			get {
				return this._extension;
			}
			set {
				this._extension = value;
			}
		}
	}

	[System.SerializableAttribute()]
	[System.Xml.Serialization.XmlTypeAttribute(TypeName = "RotateMode", Namespace = "http://www.onvif.org/ver10/schema")]
	public enum RotateMode {

		[System.Xml.Serialization.XmlEnumAttribute(Name = "OFF")]
		off,

		[System.Xml.Serialization.XmlEnumAttribute(Name = "ON")]
		on,

		[System.Xml.Serialization.XmlEnumAttribute(Name = "AUTO")]
		auto,
	}

	[System.SerializableAttribute()]
	[System.Xml.Serialization.XmlTypeAttribute(TypeName = "RotateExtension", Namespace = "http://www.onvif.org/ver10/schema")]
	public partial class RotateExtension {

		private System.Xml.XmlElement[] _any;

		/// <remarks>optional, order 0, namespace ##any</remarks>
		[System.Xml.Serialization.XmlAnyElementAttribute()]
		public System.Xml.XmlElement[] any {
			get {
				return this._any;
			}
			set {
				this._any = value;
			}
		}
	}

	[System.SerializableAttribute()]
	[System.Xml.Serialization.XmlTypeAttribute(TypeName = "VideoSourceConfigurationExtension2", Namespace = "http://www.onvif.org/ver10/schema")]
	public partial class VideoSourceConfigurationExtension2 {

		private System.Xml.XmlElement[] _any;

		/// <remarks>optional, order 0, namespace ##any</remarks>
		[System.Xml.Serialization.XmlAnyElementAttribute()]
		public System.Xml.XmlElement[] any {
			get {
				return this._any;
			}
			set {
				this._any = value;
			}
		}
	}

	[System.SerializableAttribute()]
	[System.Xml.Serialization.XmlTypeAttribute(TypeName = "AudioSourceConfiguration", Namespace = "http://www.onvif.org/ver10/schema")]
	public partial class AudioSourceConfiguration {

		private string _token;

		private System.Xml.XmlAttribute[] _anyAttr;

		private string _name;

		private int _useCount;

		private string _sourceToken;

		private System.Xml.XmlElement[] _any;

		/// <summary>
		/// Token that uniquely refernces this configuration. Length up to 64 characters.
		/// </summary>
		[System.Xml.Serialization.XmlAttributeAttribute("token")]
		public string token {
			get {
				return this._token;
			}
			set {
				this._token = value;
			}
		}

		[System.Xml.Serialization.XmlAnyAttributeAttribute()]
		public System.Xml.XmlAttribute[] anyAttr {
			get {
				return this._anyAttr;
			}
			set {
				this._anyAttr = value;
			}
		}

		/// <summary>
		/// User readable name. Length up to 64 characters.
		/// </summary>
		/// <remarks>reqired, order 0</remarks>
		[System.Xml.Serialization.XmlElementAttribute("Name", Namespace = "http://www.onvif.org/ver10/schema", DataType = "string")]
		public string name {
			get {
				return this._name;
			}
			set {
				this._name = value;
			}
		}

		/// <summary>
		/// Number of internal references currently using this configuration.
		/// This parameter is read-only and cannot be changed by a set request.
		/// For example the value increases if the configuration is added to a media profile or attached to a PaneConfiguration.
		/// </summary>
		/// <remarks>reqired, order 1</remarks>
		[System.Xml.Serialization.XmlElementAttribute("UseCount", Namespace = "http://www.onvif.org/ver10/schema", DataType = "int")]
		public int useCount {
			get {
				return this._useCount;
			}
			set {
				this._useCount = value;
			}
		}

		/// <summary>
		/// Token of the Audio Source the configuration applies to
		/// </summary>
		/// <remarks>reqired, order 2</remarks>
		[System.Xml.Serialization.XmlElementAttribute("SourceToken", Namespace = "http://www.onvif.org/ver10/schema", DataType = "string")]
		public string sourceToken {
			get {
				return this._sourceToken;
			}
			set {
				this._sourceToken = value;
			}
		}

		/// <remarks>optional, order 3, namespace ##any</remarks>
		[System.Xml.Serialization.XmlAnyElementAttribute()]
		public System.Xml.XmlElement[] any {
			get {
				return this._any;
			}
			set {
				this._any = value;
			}
		}
	}

	[System.SerializableAttribute()]
	[System.Xml.Serialization.XmlTypeAttribute(TypeName = "VideoEncoderConfiguration", Namespace = "http://www.onvif.org/ver10/schema")]
	public partial class VideoEncoderConfiguration {

		private string _token;

		private System.Xml.XmlAttribute[] _anyAttr;

		private string _name;

		private int _useCount;

		private VideoEncoding _encoding;

		private VideoResolution _resolution;

		private float _quality;

		private VideoRateControl _rateControl;

		private Mpeg4Configuration _mpeg4;

		private H264Configuration _h264;

		private MulticastConfiguration _multicast;

		private XsDuration _sessionTimeout;

		private System.Xml.XmlElement[] _any;

		/// <summary>
		/// Token that uniquely refernces this configuration. Length up to 64 characters.
		/// </summary>
		[System.Xml.Serialization.XmlAttributeAttribute("token")]
		public string token {
			get {
				return this._token;
			}
			set {
				this._token = value;
			}
		}

		[System.Xml.Serialization.XmlAnyAttributeAttribute()]
		public System.Xml.XmlAttribute[] anyAttr {
			get {
				return this._anyAttr;
			}
			set {
				this._anyAttr = value;
			}
		}

		/// <summary>
		/// User readable name. Length up to 64 characters.
		/// </summary>
		/// <remarks>reqired, order 0</remarks>
		[System.Xml.Serialization.XmlElementAttribute("Name", Namespace = "http://www.onvif.org/ver10/schema", DataType = "string")]
		public string name {
			get {
				return this._name;
			}
			set {
				this._name = value;
			}
		}

		/// <summary>
		/// Number of internal references currently using this configuration.
		/// This parameter is read-only and cannot be changed by a set request.
		/// For example the value increases if the configuration is added to a media profile or attached to a PaneConfiguration.
		/// </summary>
		/// <remarks>reqired, order 1</remarks>
		[System.Xml.Serialization.XmlElementAttribute("UseCount", Namespace = "http://www.onvif.org/ver10/schema", DataType = "int")]
		public int useCount {
			get {
				return this._useCount;
			}
			set {
				this._useCount = value;
			}
		}

		/// <summary>
		/// Used video codec, either Jpeg, H.264 or Mpeg4
		/// </summary>
		/// <remarks>reqired, order 2</remarks>
		[System.Xml.Serialization.XmlElementAttribute("Encoding", Namespace = "http://www.onvif.org/ver10/schema")]
		public VideoEncoding encoding {
			get {
				return this._encoding;
			}
			set {
				this._encoding = value;
			}
		}

		/// <summary>
		/// Configured video resolution
		/// </summary>
		/// <remarks>reqired, order 3</remarks>
		[System.Xml.Serialization.XmlElementAttribute("Resolution", Namespace = "http://www.onvif.org/ver10/schema")]
		public VideoResolution resolution {
			get {
				return this._resolution;
			}
			set {
				this._resolution = value;
			}
		}

		/// <summary>
		/// Relative value for the video quantizers and the quality of the video. A high value within supported quality range means higher quality
		/// </summary>
		/// <remarks>reqired, order 4</remarks>
		[System.Xml.Serialization.XmlElementAttribute("Quality", Namespace = "http://www.onvif.org/ver10/schema", DataType = "float")]
		public float quality {
			get {
				return this._quality;
			}
			set {
				this._quality = value;
			}
		}

		/// <summary>
		/// Optional element to configure rate control related parameters.
		/// </summary>
		/// <remarks>optional, order 5</remarks>
		[System.Xml.Serialization.XmlElementAttribute("RateControl", Namespace = "http://www.onvif.org/ver10/schema")]
		public VideoRateControl rateControl {
			get {
				return this._rateControl;
			}
			set {
				this._rateControl = value;
			}
		}

		/// <summary>
		/// Optional element to configure Mpeg4 related parameters.
		/// </summary>
		/// <remarks>optional, order 6</remarks>
		[System.Xml.Serialization.XmlElementAttribute("MPEG4", Namespace = "http://www.onvif.org/ver10/schema")]
		public Mpeg4Configuration mpeg4 {
			get {
				return this._mpeg4;
			}
			set {
				this._mpeg4 = value;
			}
		}

		/// <summary>
		/// Optional element to configure H.264 related parameters.
		/// </summary>
		/// <remarks>optional, order 7</remarks>
		[System.Xml.Serialization.XmlElementAttribute("H264", Namespace = "http://www.onvif.org/ver10/schema")]
		public H264Configuration h264 {
			get {
				return this._h264;
			}
			set {
				this._h264 = value;
			}
		}

		/// <summary>
		/// Defines the multicast settings that could be used for video streaming.
		/// </summary>
		/// <remarks>reqired, order 8</remarks>
		[System.Xml.Serialization.XmlElementAttribute("Multicast", Namespace = "http://www.onvif.org/ver10/schema")]
		public MulticastConfiguration multicast {
			get {
				return this._multicast;
			}
			set {
				this._multicast = value;
			}
		}

		/// <summary>
		/// The rtsp session timeout for the related video stream
		/// </summary>
		/// <remarks>reqired, order 9</remarks>
		[System.Xml.Serialization.XmlElementAttribute("SessionTimeout", Namespace = "http://www.onvif.org/ver10/schema")]
		public XsDuration sessionTimeout {
			get {
				return this._sessionTimeout;
			}
			set {
				this._sessionTimeout = value;
			}
		}

		/// <remarks>optional, order 10, namespace ##any</remarks>
		[System.Xml.Serialization.XmlAnyElementAttribute()]
		public System.Xml.XmlElement[] any {
			get {
				return this._any;
			}
			set {
				this._any = value;
			}
		}
	}

	[System.SerializableAttribute()]
	[System.Xml.Serialization.XmlTypeAttribute(TypeName = "VideoEncoding", Namespace = "http://www.onvif.org/ver10/schema")]
	public enum VideoEncoding {

		[System.Xml.Serialization.XmlEnumAttribute(Name = "JPEG")]
		jpeg,

		[System.Xml.Serialization.XmlEnumAttribute(Name = "MPEG4")]
		mpeg4,

		[System.Xml.Serialization.XmlEnumAttribute(Name = "H264")]
		h264,
	}

	[System.SerializableAttribute()]
	[System.Xml.Serialization.XmlTypeAttribute(TypeName = "VideoRateControl", Namespace = "http://www.onvif.org/ver10/schema")]
	public partial class VideoRateControl {

		private int _frameRateLimit;

		private int _encodingInterval;

		private int _bitrateLimit;

		/// <summary>
		/// Maximum output framerate in fps. If an EncodingInterval is provided the resulting encoded framerate will be reduced by the given factor.
		/// </summary>
		/// <remarks>reqired, order 0</remarks>
		[System.Xml.Serialization.XmlElementAttribute("FrameRateLimit", Namespace = "http://www.onvif.org/ver10/schema", DataType = "int")]
		public int frameRateLimit {
			get {
				return this._frameRateLimit;
			}
			set {
				this._frameRateLimit = value;
			}
		}

		/// <summary>
		/// Interval at which images are encoded and transmitted. (A value of 1 means that every frame is encoded, a value of 2 means that every 2nd frame is encoded ...)
		/// </summary>
		/// <remarks>reqired, order 1</remarks>
		[System.Xml.Serialization.XmlElementAttribute("EncodingInterval", Namespace = "http://www.onvif.org/ver10/schema", DataType = "int")]
		public int encodingInterval {
			get {
				return this._encodingInterval;
			}
			set {
				this._encodingInterval = value;
			}
		}

		/// <summary>
		/// the maximum output bitrate in kbps
		/// </summary>
		/// <remarks>reqired, order 2</remarks>
		[System.Xml.Serialization.XmlElementAttribute("BitrateLimit", Namespace = "http://www.onvif.org/ver10/schema", DataType = "int")]
		public int bitrateLimit {
			get {
				return this._bitrateLimit;
			}
			set {
				this._bitrateLimit = value;
			}
		}
	}

	[System.SerializableAttribute()]
	[System.Xml.Serialization.XmlTypeAttribute(TypeName = "Mpeg4Configuration", Namespace = "http://www.onvif.org/ver10/schema")]
	public partial class Mpeg4Configuration {

		private int _govLength;

		private Mpeg4Profile _mpeg4Profile;

		/// <summary>
		/// Determines the interval in which the I-Frames will be coded. An entry of 1 indicates I-Frames are continuously generated. An entry of 2 indicates that every 2nd image is an I-Frame, and 3 only every 3rd frame, etc. The frames in between are coded as P or B Frames.
		/// </summary>
		/// <remarks>reqired, order 0</remarks>
		[System.Xml.Serialization.XmlElementAttribute("GovLength", Namespace = "http://www.onvif.org/ver10/schema", DataType = "int")]
		public int govLength {
			get {
				return this._govLength;
			}
			set {
				this._govLength = value;
			}
		}

		/// <summary>
		/// the Mpeg4 profile, either simple profile (SP) or advanced simple profile (ASP)
		/// </summary>
		/// <remarks>reqired, order 1</remarks>
		[System.Xml.Serialization.XmlElementAttribute("Mpeg4Profile", Namespace = "http://www.onvif.org/ver10/schema")]
		public Mpeg4Profile mpeg4Profile {
			get {
				return this._mpeg4Profile;
			}
			set {
				this._mpeg4Profile = value;
			}
		}
	}

	[System.SerializableAttribute()]
	[System.Xml.Serialization.XmlTypeAttribute(TypeName = "Mpeg4Profile", Namespace = "http://www.onvif.org/ver10/schema")]
	public enum Mpeg4Profile {

		[System.Xml.Serialization.XmlEnumAttribute(Name = "SP")]
		sp,

		[System.Xml.Serialization.XmlEnumAttribute(Name = "ASP")]
		asp,
	}

	[System.SerializableAttribute()]
	[System.Xml.Serialization.XmlTypeAttribute(TypeName = "H264Configuration", Namespace = "http://www.onvif.org/ver10/schema")]
	public partial class H264Configuration {

		private int _govLength;

		private H264Profile _h264Profile;

		/// <summary>
		/// Group of Video frames length. Determines typically the interval in which the I-Frames will be coded. An entry of 1 indicates I-Frames are continuously generated. An entry of 2 indicates that every 2nd image is an I-Frame, and 3 only every 3rd frame, etc. The frames in between are coded as P or B Frames.
		/// </summary>
		/// <remarks>reqired, order 0</remarks>
		[System.Xml.Serialization.XmlElementAttribute("GovLength", Namespace = "http://www.onvif.org/ver10/schema", DataType = "int")]
		public int govLength {
			get {
				return this._govLength;
			}
			set {
				this._govLength = value;
			}
		}

		/// <summary>
		/// the H.264 profile, either baseline, main, extended or high
		/// </summary>
		/// <remarks>reqired, order 1</remarks>
		[System.Xml.Serialization.XmlElementAttribute("H264Profile", Namespace = "http://www.onvif.org/ver10/schema")]
		public H264Profile h264Profile {
			get {
				return this._h264Profile;
			}
			set {
				this._h264Profile = value;
			}
		}
	}

	[System.SerializableAttribute()]
	[System.Xml.Serialization.XmlTypeAttribute(TypeName = "H264Profile", Namespace = "http://www.onvif.org/ver10/schema")]
	public enum H264Profile {

		[System.Xml.Serialization.XmlEnumAttribute(Name = "Baseline")]
		baseline,

		[System.Xml.Serialization.XmlEnumAttribute(Name = "Main")]
		main,

		[System.Xml.Serialization.XmlEnumAttribute(Name = "Extended")]
		extended,

		[System.Xml.Serialization.XmlEnumAttribute(Name = "High")]
		high,
	}

	[System.SerializableAttribute()]
	[System.Xml.Serialization.XmlTypeAttribute(TypeName = "MulticastConfiguration", Namespace = "http://www.onvif.org/ver10/schema")]
	public partial class MulticastConfiguration {

		private System.Xml.XmlAttribute[] _anyAttr;

		private IPAddress _address;

		private int _port;

		private int _ttl;

		private bool _autoStart;

		private System.Xml.XmlElement[] _any;

		[System.Xml.Serialization.XmlAnyAttributeAttribute()]
		public System.Xml.XmlAttribute[] anyAttr {
			get {
				return this._anyAttr;
			}
			set {
				this._anyAttr = value;
			}
		}

		/// <summary>
		/// The multicast address (if this address is set to 0 no multicast streaming is enaled)
		/// </summary>
		/// <remarks>reqired, order 0</remarks>
		[System.Xml.Serialization.XmlElementAttribute("Address", Namespace = "http://www.onvif.org/ver10/schema")]
		public IPAddress address {
			get {
				return this._address;
			}
			set {
				this._address = value;
			}
		}

		/// <summary>
		/// The RTP mutlicast destination port. A device may support RTCP. In this case the port value shall be even to allow the corresponding RTCP stream to be mapped to the next higher (odd) destination port number as defined in the RTSP specification.
		/// </summary>
		/// <remarks>reqired, order 1</remarks>
		[System.Xml.Serialization.XmlElementAttribute("Port", Namespace = "http://www.onvif.org/ver10/schema", DataType = "int")]
		public int port {
			get {
				return this._port;
			}
			set {
				this._port = value;
			}
		}

		/// <summary>
		/// In case of IPv6 the TTL value is assumed as the hop limit. Note that for IPV6 and administratively scoped IPv4 multicast the primary use for hop limit / TTL is to prevent packets from (endlessly) circulating and not limiting scope. In these cases the address contains the scope.
		/// </summary>
		/// <remarks>reqired, order 2</remarks>
		[System.Xml.Serialization.XmlElementAttribute("TTL", Namespace = "http://www.onvif.org/ver10/schema", DataType = "int")]
		public int ttl {
			get {
				return this._ttl;
			}
			set {
				this._ttl = value;
			}
		}

		/// <summary>
		/// Read only property signalling that streaming is persistant. Use the methods StartMulticastStreaming and StopMulticastStreaming to switch its state.
		/// </summary>
		/// <remarks>reqired, order 3</remarks>
		[System.Xml.Serialization.XmlElementAttribute("AutoStart", Namespace = "http://www.onvif.org/ver10/schema", DataType = "boolean")]
		public bool autoStart {
			get {
				return this._autoStart;
			}
			set {
				this._autoStart = value;
			}
		}

		/// <remarks>optional, order 4, namespace ##any</remarks>
		[System.Xml.Serialization.XmlAnyElementAttribute()]
		public System.Xml.XmlElement[] any {
			get {
				return this._any;
			}
			set {
				this._any = value;
			}
		}
	}

	[System.SerializableAttribute()]
	[System.Xml.Serialization.XmlTypeAttribute(TypeName = "IPAddress", Namespace = "http://www.onvif.org/ver10/schema")]
	public partial class IPAddress {

		private IPType _type;

		private string _iPv4Address;

		private string _iPv6Address;

		/// <summary>
		/// Indicates if the address is an IPv4 or IPv6 address.
		/// </summary>
		/// <remarks>reqired, order 0</remarks>
		[System.Xml.Serialization.XmlElementAttribute("Type", Namespace = "http://www.onvif.org/ver10/schema")]
		public IPType type {
			get {
				return this._type;
			}
			set {
				this._type = value;
			}
		}

		/// <summary>
		/// IPv4 address.
		/// </summary>
		/// <remarks>optional, order 1</remarks>
		[System.Xml.Serialization.XmlElementAttribute("IPv4Address", Namespace = "http://www.onvif.org/ver10/schema", DataType = "token")]
		public string iPv4Address {
			get {
				return this._iPv4Address;
			}
			set {
				this._iPv4Address = value;
			}
		}

		/// <summary>
		/// IPv6 address
		/// </summary>
		/// <remarks>optional, order 2</remarks>
		[System.Xml.Serialization.XmlElementAttribute("IPv6Address", Namespace = "http://www.onvif.org/ver10/schema", DataType = "token")]
		public string iPv6Address {
			get {
				return this._iPv6Address;
			}
			set {
				this._iPv6Address = value;
			}
		}
	}

	[System.SerializableAttribute()]
	[System.Xml.Serialization.XmlTypeAttribute(TypeName = "IPType", Namespace = "http://www.onvif.org/ver10/schema")]
	public enum IPType {

		[System.Xml.Serialization.XmlEnumAttribute(Name = "IPv4")]
		iPv4,

		[System.Xml.Serialization.XmlEnumAttribute(Name = "IPv6")]
		iPv6,
	}

	[System.SerializableAttribute()]
	[System.Xml.Serialization.XmlTypeAttribute(TypeName = "AudioEncoderConfiguration", Namespace = "http://www.onvif.org/ver10/schema")]
	public partial class AudioEncoderConfiguration {

		private string _token;

		private System.Xml.XmlAttribute[] _anyAttr;

		private string _name;

		private int _useCount;

		private AudioEncoding _encoding;

		private int _bitrate;

		private int _sampleRate;

		private MulticastConfiguration _multicast;

		private XsDuration _sessionTimeout;

		private System.Xml.XmlElement[] _any;

		/// <summary>
		/// Token that uniquely refernces this configuration. Length up to 64 characters.
		/// </summary>
		[System.Xml.Serialization.XmlAttributeAttribute("token")]
		public string token {
			get {
				return this._token;
			}
			set {
				this._token = value;
			}
		}

		[System.Xml.Serialization.XmlAnyAttributeAttribute()]
		public System.Xml.XmlAttribute[] anyAttr {
			get {
				return this._anyAttr;
			}
			set {
				this._anyAttr = value;
			}
		}

		/// <summary>
		/// User readable name. Length up to 64 characters.
		/// </summary>
		/// <remarks>reqired, order 0</remarks>
		[System.Xml.Serialization.XmlElementAttribute("Name", Namespace = "http://www.onvif.org/ver10/schema", DataType = "string")]
		public string name {
			get {
				return this._name;
			}
			set {
				this._name = value;
			}
		}

		/// <summary>
		/// Number of internal references currently using this configuration.
		/// This parameter is read-only and cannot be changed by a set request.
		/// For example the value increases if the configuration is added to a media profile or attached to a PaneConfiguration.
		/// </summary>
		/// <remarks>reqired, order 1</remarks>
		[System.Xml.Serialization.XmlElementAttribute("UseCount", Namespace = "http://www.onvif.org/ver10/schema", DataType = "int")]
		public int useCount {
			get {
				return this._useCount;
			}
			set {
				this._useCount = value;
			}
		}

		/// <summary>
		/// Audio codec used for encoding the audio input (either G.711, G.726 or AAC)
		/// </summary>
		/// <remarks>reqired, order 2</remarks>
		[System.Xml.Serialization.XmlElementAttribute("Encoding", Namespace = "http://www.onvif.org/ver10/schema")]
		public AudioEncoding encoding {
			get {
				return this._encoding;
			}
			set {
				this._encoding = value;
			}
		}

		/// <summary>
		/// The output bitrate in kbps.
		/// </summary>
		/// <remarks>reqired, order 3</remarks>
		[System.Xml.Serialization.XmlElementAttribute("Bitrate", Namespace = "http://www.onvif.org/ver10/schema", DataType = "int")]
		public int bitrate {
			get {
				return this._bitrate;
			}
			set {
				this._bitrate = value;
			}
		}

		/// <summary>
		/// The output sample rate in kHz.
		/// </summary>
		/// <remarks>reqired, order 4</remarks>
		[System.Xml.Serialization.XmlElementAttribute("SampleRate", Namespace = "http://www.onvif.org/ver10/schema", DataType = "int")]
		public int sampleRate {
			get {
				return this._sampleRate;
			}
			set {
				this._sampleRate = value;
			}
		}

		/// <summary>
		/// Defines the multicast settings that could be used for video streaming.
		/// </summary>
		/// <remarks>reqired, order 5</remarks>
		[System.Xml.Serialization.XmlElementAttribute("Multicast", Namespace = "http://www.onvif.org/ver10/schema")]
		public MulticastConfiguration multicast {
			get {
				return this._multicast;
			}
			set {
				this._multicast = value;
			}
		}

		/// <summary>
		/// The rtsp session timeout for the related audio stream
		/// </summary>
		/// <remarks>reqired, order 6</remarks>
		[System.Xml.Serialization.XmlElementAttribute("SessionTimeout", Namespace = "http://www.onvif.org/ver10/schema")]
		public XsDuration sessionTimeout {
			get {
				return this._sessionTimeout;
			}
			set {
				this._sessionTimeout = value;
			}
		}

		/// <remarks>optional, order 7, namespace ##any</remarks>
		[System.Xml.Serialization.XmlAnyElementAttribute()]
		public System.Xml.XmlElement[] any {
			get {
				return this._any;
			}
			set {
				this._any = value;
			}
		}
	}

	[System.SerializableAttribute()]
	[System.Xml.Serialization.XmlTypeAttribute(TypeName = "AudioEncoding", Namespace = "http://www.onvif.org/ver10/schema")]
	public enum AudioEncoding {

		[System.Xml.Serialization.XmlEnumAttribute(Name = "G711")]
		g711,

		[System.Xml.Serialization.XmlEnumAttribute(Name = "G726")]
		g726,

		[System.Xml.Serialization.XmlEnumAttribute(Name = "AAC")]
		aac,
	}

	[System.SerializableAttribute()]
	[System.Xml.Serialization.XmlTypeAttribute(TypeName = "VideoAnalyticsConfiguration", Namespace = "http://www.onvif.org/ver10/schema")]
	public partial class VideoAnalyticsConfiguration {

		private string _token;

		private System.Xml.XmlAttribute[] _anyAttr;

		private string _name;

		private int _useCount;

		private AnalyticsEngineConfiguration _analyticsEngineConfiguration;

		private RuleEngineConfiguration _ruleEngineConfiguration;

		private System.Xml.XmlElement[] _any;

		/// <summary>
		/// Token that uniquely refernces this configuration. Length up to 64 characters.
		/// </summary>
		[System.Xml.Serialization.XmlAttributeAttribute("token")]
		public string token {
			get {
				return this._token;
			}
			set {
				this._token = value;
			}
		}

		[System.Xml.Serialization.XmlAnyAttributeAttribute()]
		public System.Xml.XmlAttribute[] anyAttr {
			get {
				return this._anyAttr;
			}
			set {
				this._anyAttr = value;
			}
		}

		/// <summary>
		/// User readable name. Length up to 64 characters.
		/// </summary>
		/// <remarks>reqired, order 0</remarks>
		[System.Xml.Serialization.XmlElementAttribute("Name", Namespace = "http://www.onvif.org/ver10/schema", DataType = "string")]
		public string name {
			get {
				return this._name;
			}
			set {
				this._name = value;
			}
		}

		/// <summary>
		/// Number of internal references currently using this configuration.
		/// This parameter is read-only and cannot be changed by a set request.
		/// For example the value increases if the configuration is added to a media profile or attached to a PaneConfiguration.
		/// </summary>
		/// <remarks>reqired, order 1</remarks>
		[System.Xml.Serialization.XmlElementAttribute("UseCount", Namespace = "http://www.onvif.org/ver10/schema", DataType = "int")]
		public int useCount {
			get {
				return this._useCount;
			}
			set {
				this._useCount = value;
			}
		}

		/// <remarks>reqired, order 2</remarks>
		[System.Xml.Serialization.XmlElementAttribute("AnalyticsEngineConfiguration", Namespace = "http://www.onvif.org/ver10/schema")]
		public AnalyticsEngineConfiguration analyticsEngineConfiguration {
			get {
				return this._analyticsEngineConfiguration;
			}
			set {
				this._analyticsEngineConfiguration = value;
			}
		}

		/// <remarks>reqired, order 3</remarks>
		[System.Xml.Serialization.XmlElementAttribute("RuleEngineConfiguration", Namespace = "http://www.onvif.org/ver10/schema")]
		public RuleEngineConfiguration ruleEngineConfiguration {
			get {
				return this._ruleEngineConfiguration;
			}
			set {
				this._ruleEngineConfiguration = value;
			}
		}

		/// <remarks>optional, order 4, namespace ##any</remarks>
		[System.Xml.Serialization.XmlAnyElementAttribute()]
		public System.Xml.XmlElement[] any {
			get {
				return this._any;
			}
			set {
				this._any = value;
			}
		}
	}

	[System.SerializableAttribute()]
	[System.Xml.Serialization.XmlTypeAttribute(TypeName = "AnalyticsEngineConfiguration", Namespace = "http://www.onvif.org/ver10/schema")]
	public partial class AnalyticsEngineConfiguration {

		private System.Xml.XmlAttribute[] _anyAttr;

		private Config[] _analyticsModule;

		private AnalyticsEngineConfigurationExtension _extension;

		[System.Xml.Serialization.XmlAnyAttributeAttribute()]
		public System.Xml.XmlAttribute[] anyAttr {
			get {
				return this._anyAttr;
			}
			set {
				this._anyAttr = value;
			}
		}

		/// <remarks>optional, order 0</remarks>
		[System.Xml.Serialization.XmlElementAttribute("AnalyticsModule", Namespace = "http://www.onvif.org/ver10/schema")]
		public Config[] analyticsModule {
			get {
				return this._analyticsModule;
			}
			set {
				this._analyticsModule = value;
			}
		}

		/// <remarks>optional, order 1</remarks>
		[System.Xml.Serialization.XmlElementAttribute("Extension", Namespace = "http://www.onvif.org/ver10/schema")]
		public AnalyticsEngineConfigurationExtension extension {
			get {
				return this._extension;
			}
			set {
				this._extension = value;
			}
		}
	}

	[System.SerializableAttribute()]
	[System.Xml.Serialization.XmlTypeAttribute(TypeName = "Config", Namespace = "http://www.onvif.org/ver10/schema")]
	public partial class Config {

		private string _name;

		private System.Xml.XmlQualifiedName _type;

		private ItemList _parameters;

		[System.Xml.Serialization.XmlNamespaceDeclarations]
		public XmlSerializerNamespaces xmlns;

		/// <summary>
		/// Name of the configuration.
		/// </summary>
		[System.Xml.Serialization.XmlAttributeAttribute("Name", DataType = "string")]
		public string name {
			get {
				return this._name;
			}
			set {
				this._name = value;
			}
		}

		/// <summary>
		/// Type of the configuration represented by a unique QName. The Type characterizes a ConfigDescription defining the Parameters.
		/// </summary>
		[System.Xml.Serialization.XmlAttributeAttribute("Type", DataType = "QName")]
		public System.Xml.XmlQualifiedName type {
			get {
				return this._type;
			}
			set {
				this._type = value;
			}
		}

		/// <summary>
		/// List of configuration parameters as defined in the correspding description.
		/// </summary>
		/// <remarks>reqired, order 0</remarks>
		[System.Xml.Serialization.XmlElementAttribute("Parameters", Namespace = "http://www.onvif.org/ver10/schema")]
		public ItemList parameters {
			get {
				return this._parameters;
			}
			set {
				this._parameters = value;
			}
		}
	}

	/// <summary>
	/// List of parameters according to the corresponding ItemListDescription.
	/// Each item in the list shall have a unique name.
	/// </summary>
	[System.SerializableAttribute()]
	[System.Xml.Serialization.XmlTypeAttribute(TypeName = "ItemList", Namespace = "http://www.onvif.org/ver10/schema")]
	public partial class ItemList {

		private System.Xml.XmlAttribute[] _anyAttr;

		private SimpleItem[] _simpleItem;

		private ElementItem[] _elementItem;

		private ItemListExtension _extension;

		[System.Xml.Serialization.XmlAnyAttributeAttribute()]
		public System.Xml.XmlAttribute[] anyAttr {
			get {
				return this._anyAttr;
			}
			set {
				this._anyAttr = value;
			}
		}

		/// <summary>
		/// Value name pair as defined by the corresponding description.
		/// </summary>
		/// <remarks>optional, order 0</remarks>
		[System.Xml.Serialization.XmlElementAttribute("SimpleItem", Namespace = "http://www.onvif.org/ver10/schema")]
		public SimpleItem[] simpleItem {
			get {
				return this._simpleItem;
			}
			set {
				this._simpleItem = value;
			}
		}

		/// <summary>
		/// Complex value structure.
		/// </summary>
		/// <remarks>optional, order 1</remarks>
		[System.Xml.Serialization.XmlElementAttribute("ElementItem", Namespace = "http://www.onvif.org/ver10/schema")]
		public ElementItem[] elementItem {
			get {
				return this._elementItem;
			}
			set {
				this._elementItem = value;
			}
		}

		/// <remarks>optional, order 2</remarks>
		[System.Xml.Serialization.XmlElementAttribute("Extension", Namespace = "http://www.onvif.org/ver10/schema")]
		public ItemListExtension extension {
			get {
				return this._extension;
			}
			set {
				this._extension = value;
			}
		}

		[System.SerializableAttribute()]
		[System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://www.onvif.org/ver10/schema")]
		public partial class SimpleItem {

			private string _name;

			private string _value;

			/// <summary>
			/// Item name.
			/// </summary>
			[System.Xml.Serialization.XmlAttributeAttribute("Name", DataType = "string")]
			public string name {
				get {
					return this._name;
				}
				set {
					this._name = value;
				}
			}

			/// <summary>
			/// Item value. The type is defined in the corresponding description.
			/// </summary>
			[System.Xml.Serialization.XmlAttributeAttribute("Value")]
			public string value {
				get {
					return this._value;
				}
				set {
					this._value = value;
				}
			}
		}

		[System.SerializableAttribute()]
		[System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://www.onvif.org/ver10/schema")]
		public partial class ElementItem {

			private string _name;

			private System.Xml.XmlElement _any;

			/// <summary>
			/// Item name.
			/// </summary>
			[System.Xml.Serialization.XmlAttributeAttribute("Name", DataType = "string")]
			public string name {
				get {
					return this._name;
				}
				set {
					this._name = value;
				}
			}

			/// <remarks>reqired, order 0, namespace ##any</remarks>
			/// <summary>
			/// XML tree contiaing the element value as defined in the corresponding description.
			/// </summary>
			[System.Xml.Serialization.XmlAnyElementAttribute()]
			public System.Xml.XmlElement any {
				get {
					return this._any;
				}
				set {
					this._any = value;
				}
			}
		}
	}

	[System.SerializableAttribute()]
	[System.Xml.Serialization.XmlTypeAttribute(TypeName = "ItemListExtension", Namespace = "http://www.onvif.org/ver10/schema")]
	public partial class ItemListExtension {

		private System.Xml.XmlElement[] _any;

		/// <remarks>optional, order 0, namespace ##any</remarks>
		[System.Xml.Serialization.XmlAnyElementAttribute()]
		public System.Xml.XmlElement[] any {
			get {
				return this._any;
			}
			set {
				this._any = value;
			}
		}
	}

	[System.SerializableAttribute()]
	[System.Xml.Serialization.XmlTypeAttribute(TypeName = "AnalyticsEngineConfigurationExtension", Namespace = "http://www.onvif.org/ver10/schema")]
	public partial class AnalyticsEngineConfigurationExtension {

		private System.Xml.XmlElement[] _any;

		/// <remarks>optional, order 0, namespace ##any</remarks>
		[System.Xml.Serialization.XmlAnyElementAttribute()]
		public System.Xml.XmlElement[] any {
			get {
				return this._any;
			}
			set {
				this._any = value;
			}
		}
	}

	[System.SerializableAttribute()]
	[System.Xml.Serialization.XmlTypeAttribute(TypeName = "RuleEngineConfiguration", Namespace = "http://www.onvif.org/ver10/schema")]
	public partial class RuleEngineConfiguration {

		private System.Xml.XmlAttribute[] _anyAttr;

		private Config[] _rule;

		private RuleEngineConfigurationExtension _extension;

		[System.Xml.Serialization.XmlAnyAttributeAttribute()]
		public System.Xml.XmlAttribute[] anyAttr {
			get {
				return this._anyAttr;
			}
			set {
				this._anyAttr = value;
			}
		}

		/// <remarks>optional, order 0</remarks>
		[System.Xml.Serialization.XmlElementAttribute("Rule", Namespace = "http://www.onvif.org/ver10/schema")]
		public Config[] rule {
			get {
				return this._rule;
			}
			set {
				this._rule = value;
			}
		}

		/// <remarks>optional, order 1</remarks>
		[System.Xml.Serialization.XmlElementAttribute("Extension", Namespace = "http://www.onvif.org/ver10/schema")]
		public RuleEngineConfigurationExtension extension {
			get {
				return this._extension;
			}
			set {
				this._extension = value;
			}
		}
	}

	[System.SerializableAttribute()]
	[System.Xml.Serialization.XmlTypeAttribute(TypeName = "RuleEngineConfigurationExtension", Namespace = "http://www.onvif.org/ver10/schema")]
	public partial class RuleEngineConfigurationExtension {

		private System.Xml.XmlElement[] _any;

		/// <remarks>optional, order 0, namespace ##any</remarks>
		[System.Xml.Serialization.XmlAnyElementAttribute()]
		public System.Xml.XmlElement[] any {
			get {
				return this._any;
			}
			set {
				this._any = value;
			}
		}
	}

	[System.SerializableAttribute()]
	[System.Xml.Serialization.XmlTypeAttribute(TypeName = "PTZConfiguration", Namespace = "http://www.onvif.org/ver10/schema")]
	public partial class PTZConfiguration {

		private string _token;

		private System.Xml.XmlAttribute[] _anyAttr;

		private string _name;

		private int _useCount;

		private string _nodeToken;

		private string _defaultAbsolutePantTiltPositionSpace;

		private string _defaultAbsoluteZoomPositionSpace;

		private string _defaultRelativePanTiltTranslationSpace;

		private string _defaultRelativeZoomTranslationSpace;

		private string _defaultContinuousPanTiltVelocitySpace;

		private string _defaultContinuousZoomVelocitySpace;

		private PTZSpeed _defaultPTZSpeed;

		private XsDuration _defaultPTZTimeout;

		private PanTiltLimits _panTiltLimits;

		private ZoomLimits _zoomLimits;

		private PTZConfigurationExtension _extension;

		/// <summary>
		/// Token that uniquely refernces this configuration. Length up to 64 characters.
		/// </summary>
		[System.Xml.Serialization.XmlAttributeAttribute("token")]
		public string token {
			get {
				return this._token;
			}
			set {
				this._token = value;
			}
		}

		[System.Xml.Serialization.XmlAnyAttributeAttribute()]
		public System.Xml.XmlAttribute[] anyAttr {
			get {
				return this._anyAttr;
			}
			set {
				this._anyAttr = value;
			}
		}

		/// <summary>
		/// User readable name. Length up to 64 characters.
		/// </summary>
		/// <remarks>reqired, order 0</remarks>
		[System.Xml.Serialization.XmlElementAttribute("Name", Namespace = "http://www.onvif.org/ver10/schema", DataType = "string")]
		public string name {
			get {
				return this._name;
			}
			set {
				this._name = value;
			}
		}

		/// <summary>
		/// Number of internal references currently using this configuration.
		/// This parameter is read-only and cannot be changed by a set request.
		/// For example the value increases if the configuration is added to a media profile or attached to a PaneConfiguration.
		/// </summary>
		/// <remarks>reqired, order 1</remarks>
		[System.Xml.Serialization.XmlElementAttribute("UseCount", Namespace = "http://www.onvif.org/ver10/schema", DataType = "int")]
		public int useCount {
			get {
				return this._useCount;
			}
			set {
				this._useCount = value;
			}
		}

		/// <summary>
		/// A mandatory reference to the PTZ Node that the PTZ Configuration belongs to.
		/// </summary>
		/// <remarks>reqired, order 2</remarks>
		[System.Xml.Serialization.XmlElementAttribute("NodeToken", Namespace = "http://www.onvif.org/ver10/schema", DataType = "string")]
		public string nodeToken {
			get {
				return this._nodeToken;
			}
			set {
				this._nodeToken = value;
			}
		}

		/// <summary>
		/// If the PTZ Node supports absolute Pan/Tilt movements, it shall specify one Absolute Pan/Tilt Position Space as default.
		/// </summary>
		/// <remarks>optional, order 3</remarks>
		[System.Xml.Serialization.XmlElementAttribute("DefaultAbsolutePantTiltPositionSpace", Namespace = "http://www.onvif.org/ver10/schema", DataType = "anyURI")]
		public string defaultAbsolutePantTiltPositionSpace {
			get {
				return this._defaultAbsolutePantTiltPositionSpace;
			}
			set {
				this._defaultAbsolutePantTiltPositionSpace = value;
			}
		}

		/// <summary>
		/// If the PTZ Node supports absolute zoom movements, it shall specify one Absolute Zoom Position Space as default.
		/// </summary>
		/// <remarks>optional, order 4</remarks>
		[System.Xml.Serialization.XmlElementAttribute("DefaultAbsoluteZoomPositionSpace", Namespace = "http://www.onvif.org/ver10/schema", DataType = "anyURI")]
		public string defaultAbsoluteZoomPositionSpace {
			get {
				return this._defaultAbsoluteZoomPositionSpace;
			}
			set {
				this._defaultAbsoluteZoomPositionSpace = value;
			}
		}

		/// <summary>
		/// If the PTZ Node supports relative Pan/Tilt movements, it shall specify one RelativePan/Tilt Translation Space as default.
		/// </summary>
		/// <remarks>optional, order 5</remarks>
		[System.Xml.Serialization.XmlElementAttribute("DefaultRelativePanTiltTranslationSpace", Namespace = "http://www.onvif.org/ver10/schema", DataType = "anyURI")]
		public string defaultRelativePanTiltTranslationSpace {
			get {
				return this._defaultRelativePanTiltTranslationSpace;
			}
			set {
				this._defaultRelativePanTiltTranslationSpace = value;
			}
		}

		/// <summary>
		/// If the PTZ Node supports relative zoom movements, it shall specify one Relative Zoom Translation Space as default.
		/// </summary>
		/// <remarks>optional, order 6</remarks>
		[System.Xml.Serialization.XmlElementAttribute("DefaultRelativeZoomTranslationSpace", Namespace = "http://www.onvif.org/ver10/schema", DataType = "anyURI")]
		public string defaultRelativeZoomTranslationSpace {
			get {
				return this._defaultRelativeZoomTranslationSpace;
			}
			set {
				this._defaultRelativeZoomTranslationSpace = value;
			}
		}

		/// <summary>
		/// If the PTZ Node supports continuous Pan/Tilt movements, it shall specify one Continuous Pan/Tilt Velocity Space as default.
		/// </summary>
		/// <remarks>optional, order 7</remarks>
		[System.Xml.Serialization.XmlElementAttribute("DefaultContinuousPanTiltVelocitySpace", Namespace = "http://www.onvif.org/ver10/schema", DataType = "anyURI")]
		public string defaultContinuousPanTiltVelocitySpace {
			get {
				return this._defaultContinuousPanTiltVelocitySpace;
			}
			set {
				this._defaultContinuousPanTiltVelocitySpace = value;
			}
		}

		/// <summary>
		/// If the PTZ Node supports continuous zoom movements, it shall specify one Continuous Zoom Velocity Space as default.
		/// </summary>
		/// <remarks>optional, order 8</remarks>
		[System.Xml.Serialization.XmlElementAttribute("DefaultContinuousZoomVelocitySpace", Namespace = "http://www.onvif.org/ver10/schema", DataType = "anyURI")]
		public string defaultContinuousZoomVelocitySpace {
			get {
				return this._defaultContinuousZoomVelocitySpace;
			}
			set {
				this._defaultContinuousZoomVelocitySpace = value;
			}
		}

		/// <summary>
		/// If the PTZ Node supports absolute or relative PTZ movements, it shall specify corresponding default Pan/Tilt and Zoom speeds.
		/// </summary>
		/// <remarks>optional, order 9</remarks>
		[System.Xml.Serialization.XmlElementAttribute("DefaultPTZSpeed", Namespace = "http://www.onvif.org/ver10/schema")]
		public PTZSpeed defaultPTZSpeed {
			get {
				return this._defaultPTZSpeed;
			}
			set {
				this._defaultPTZSpeed = value;
			}
		}

		/// <summary>
		/// If the PTZ Node supports continuous movements, it shall specify a default timeout, after which the movement stops.
		/// </summary>
		/// <remarks>optional, order 10</remarks>
		[System.Xml.Serialization.XmlElementAttribute("DefaultPTZTimeout", Namespace = "http://www.onvif.org/ver10/schema")]
		public XsDuration defaultPTZTimeout {
			get {
				return this._defaultPTZTimeout;
			}
			set {
				this._defaultPTZTimeout = value;
			}
		}

		/// <summary>
		/// The Pan/Tilt limits element should be present for a PTZ Node that supports an absolute Pan/Tilt. If the element is present it signals the support for configurable Pan/Tilt limits. If limits are enabled, the Pan/Tilt movements shall always stay within the specified range. The Pan/Tilt limits are disabled by setting the limits to –INF or +INF.
		/// </summary>
		/// <remarks>optional, order 11</remarks>
		[System.Xml.Serialization.XmlElementAttribute("PanTiltLimits", Namespace = "http://www.onvif.org/ver10/schema")]
		public PanTiltLimits panTiltLimits {
			get {
				return this._panTiltLimits;
			}
			set {
				this._panTiltLimits = value;
			}
		}

		/// <summary>
		/// The Zoom limits element should be present for a PTZ Node that supports absolute zoom. If the element is present it signals the supports for configurable Zoom limits. If limits are enabled the zoom movements shall always stay within the specified range. The Zoom limits are disabled by settings the limits to -INF and +INF.
		/// </summary>
		/// <remarks>optional, order 12</remarks>
		[System.Xml.Serialization.XmlElementAttribute("ZoomLimits", Namespace = "http://www.onvif.org/ver10/schema")]
		public ZoomLimits zoomLimits {
			get {
				return this._zoomLimits;
			}
			set {
				this._zoomLimits = value;
			}
		}

		/// <summary>
		/// </summary>
		/// <remarks>optional, order 13</remarks>
		[System.Xml.Serialization.XmlElementAttribute("Extension", Namespace = "http://www.onvif.org/ver10/schema")]
		public PTZConfigurationExtension extension {
			get {
				return this._extension;
			}
			set {
				this._extension = value;
			}
		}
	}

	[System.SerializableAttribute()]
	[System.Xml.Serialization.XmlTypeAttribute(TypeName = "PTZSpeed", Namespace = "http://www.onvif.org/ver10/schema")]
	public partial class PTZSpeed {

		private Vector2D _panTilt;

		private Vector1D _zoom;

		/// <summary>
		/// Pan and tilt speed. The x component corresponds to pan and the y component to tilt. If omitted in a request, the current (if any) PanTilt movement should not be affected.
		/// </summary>
		/// <remarks>optional, order 0</remarks>
		[System.Xml.Serialization.XmlElementAttribute("PanTilt", Namespace = "http://www.onvif.org/ver10/schema")]
		public Vector2D panTilt {
			get {
				return this._panTilt;
			}
			set {
				this._panTilt = value;
			}
		}

		/// <summary>
		/// A zoom speed. If omitted in a request, the current (if any) Zoom movement should not be affected.
		/// </summary>
		/// <remarks>optional, order 1</remarks>
		[System.Xml.Serialization.XmlElementAttribute("Zoom", Namespace = "http://www.onvif.org/ver10/schema")]
		public Vector1D zoom {
			get {
				return this._zoom;
			}
			set {
				this._zoom = value;
			}
		}
	}

	[System.SerializableAttribute()]
	[System.Xml.Serialization.XmlTypeAttribute(TypeName = "Vector2D", Namespace = "http://www.onvif.org/ver10/schema")]
	public partial class Vector2D {

		private float _x;

		private float _y;

		private string _space;

		[System.Xml.Serialization.XmlAttributeAttribute("x", DataType = "float")]
		public float x {
			get {
				return this._x;
			}
			set {
				this._x = value;
			}
		}

		[System.Xml.Serialization.XmlAttributeAttribute("y", DataType = "float")]
		public float y {
			get {
				return this._y;
			}
			set {
				this._y = value;
			}
		}

		/// <summary>
		/// Pan/tilt coordinate space selector. The following options are defined:
		/// </summary>
		[System.Xml.Serialization.XmlAttributeAttribute("space", DataType = "anyURI")]
		public string space {
			get {
				return this._space;
			}
			set {
				this._space = value;
			}
		}
	}

	[System.SerializableAttribute()]
	[System.Xml.Serialization.XmlTypeAttribute(TypeName = "Vector1D", Namespace = "http://www.onvif.org/ver10/schema")]
	public partial class Vector1D {

		private float _x;

		private string _space;

		[System.Xml.Serialization.XmlAttributeAttribute("x", DataType = "float")]
		public float x {
			get {
				return this._x;
			}
			set {
				this._x = value;
			}
		}

		/// <summary>
		/// Pan/tilt coordinate space selector. The following options are defined:
		/// </summary>
		[System.Xml.Serialization.XmlAttributeAttribute("space", DataType = "anyURI")]
		public string space {
			get {
				return this._space;
			}
			set {
				this._space = value;
			}
		}
	}

	[System.SerializableAttribute()]
	[System.Xml.Serialization.XmlTypeAttribute(TypeName = "PanTiltLimits", Namespace = "http://www.onvif.org/ver10/schema")]
	public partial class PanTiltLimits {

		private Space2DDescription _range;

		/// <summary>
		/// A range of pan tilt limits.
		/// </summary>
		/// <remarks>reqired, order 0</remarks>
		[System.Xml.Serialization.XmlElementAttribute("Range", Namespace = "http://www.onvif.org/ver10/schema")]
		public Space2DDescription range {
			get {
				return this._range;
			}
			set {
				this._range = value;
			}
		}
	}

	[System.SerializableAttribute()]
	[System.Xml.Serialization.XmlTypeAttribute(TypeName = "Space2DDescription", Namespace = "http://www.onvif.org/ver10/schema")]
	public partial class Space2DDescription {

		private string _uri;

		private FloatRange _xRange;

		private FloatRange _yRange;

		/// <summary>
		/// A URI of coordinate systems.
		/// </summary>
		/// <remarks>reqired, order 0</remarks>
		[System.Xml.Serialization.XmlElementAttribute("URI", Namespace = "http://www.onvif.org/ver10/schema", DataType = "anyURI")]
		public string uri {
			get {
				return this._uri;
			}
			set {
				this._uri = value;
			}
		}

		/// <summary>
		/// A range of x-axis.
		/// </summary>
		/// <remarks>reqired, order 1</remarks>
		[System.Xml.Serialization.XmlElementAttribute("XRange", Namespace = "http://www.onvif.org/ver10/schema")]
		public FloatRange xRange {
			get {
				return this._xRange;
			}
			set {
				this._xRange = value;
			}
		}

		/// <summary>
		/// A range of y-axis.
		/// </summary>
		/// <remarks>reqired, order 2</remarks>
		[System.Xml.Serialization.XmlElementAttribute("YRange", Namespace = "http://www.onvif.org/ver10/schema")]
		public FloatRange yRange {
			get {
				return this._yRange;
			}
			set {
				this._yRange = value;
			}
		}
	}

	[System.SerializableAttribute()]
	[System.Xml.Serialization.XmlTypeAttribute(TypeName = "ZoomLimits", Namespace = "http://www.onvif.org/ver10/schema")]
	public partial class ZoomLimits {

		private Space1DDescription _range;

		/// <summary>
		/// A range of zoom limit
		/// </summary>
		/// <remarks>reqired, order 0</remarks>
		[System.Xml.Serialization.XmlElementAttribute("Range", Namespace = "http://www.onvif.org/ver10/schema")]
		public Space1DDescription range {
			get {
				return this._range;
			}
			set {
				this._range = value;
			}
		}
	}

	[System.SerializableAttribute()]
	[System.Xml.Serialization.XmlTypeAttribute(TypeName = "Space1DDescription", Namespace = "http://www.onvif.org/ver10/schema")]
	public partial class Space1DDescription {

		private string _uri;

		private FloatRange _xRange;

		/// <summary>
		/// A URI of coordinate systems.
		/// </summary>
		/// <remarks>reqired, order 0</remarks>
		[System.Xml.Serialization.XmlElementAttribute("URI", Namespace = "http://www.onvif.org/ver10/schema", DataType = "anyURI")]
		public string uri {
			get {
				return this._uri;
			}
			set {
				this._uri = value;
			}
		}

		/// <summary>
		/// A range of x-axis.
		/// </summary>
		/// <remarks>reqired, order 1</remarks>
		[System.Xml.Serialization.XmlElementAttribute("XRange", Namespace = "http://www.onvif.org/ver10/schema")]
		public FloatRange xRange {
			get {
				return this._xRange;
			}
			set {
				this._xRange = value;
			}
		}
	}

	[System.SerializableAttribute()]
	[System.Xml.Serialization.XmlTypeAttribute(TypeName = "PTZConfigurationExtension", Namespace = "http://www.onvif.org/ver10/schema")]
	public partial class PTZConfigurationExtension {

		private System.Xml.XmlElement[] _any;

		private PTControlDirection _ptControlDirection;

		private PTZConfigurationExtension2 _extension;

		/// <remarks>optional, order 0, namespace ##other</remarks>
		[System.Xml.Serialization.XmlAnyElementAttribute()]
		public System.Xml.XmlElement[] any {
			get {
				return this._any;
			}
			set {
				this._any = value;
			}
		}

		/// <summary>
		/// Optional element to configure PT Control Direction related features.
		/// </summary>
		/// <remarks>optional, order 1</remarks>
		[System.Xml.Serialization.XmlElementAttribute("PTControlDirection", Namespace = "http://www.onvif.org/ver10/schema")]
		public PTControlDirection ptControlDirection {
			get {
				return this._ptControlDirection;
			}
			set {
				this._ptControlDirection = value;
			}
		}

		/// <remarks>optional, order 2</remarks>
		[System.Xml.Serialization.XmlElementAttribute("Extension", Namespace = "http://www.onvif.org/ver10/schema")]
		public PTZConfigurationExtension2 extension {
			get {
				return this._extension;
			}
			set {
				this._extension = value;
			}
		}
	}

	[System.SerializableAttribute()]
	[System.Xml.Serialization.XmlTypeAttribute(TypeName = "PTControlDirection", Namespace = "http://www.onvif.org/ver10/schema")]
	public partial class PTControlDirection {

		private System.Xml.XmlAttribute[] _anyAttr;

		private EFlip _eFlip;

		private Reverse _reverse;

		private PTControlDirectionExtension _extension;

		[System.Xml.Serialization.XmlAnyAttributeAttribute()]
		public System.Xml.XmlAttribute[] anyAttr {
			get {
				return this._anyAttr;
			}
			set {
				this._anyAttr = value;
			}
		}

		/// <summary>
		/// Optional element to configure related parameters for E-Flip.
		/// </summary>
		/// <remarks>optional, order 0</remarks>
		[System.Xml.Serialization.XmlElementAttribute("EFlip", Namespace = "http://www.onvif.org/ver10/schema")]
		public EFlip eFlip {
			get {
				return this._eFlip;
			}
			set {
				this._eFlip = value;
			}
		}

		/// <summary>
		/// Optional element to configure related parameters for reversing of PT Control Direction.
		/// </summary>
		/// <remarks>optional, order 1</remarks>
		[System.Xml.Serialization.XmlElementAttribute("Reverse", Namespace = "http://www.onvif.org/ver10/schema")]
		public Reverse reverse {
			get {
				return this._reverse;
			}
			set {
				this._reverse = value;
			}
		}

		/// <remarks>optional, order 2</remarks>
		[System.Xml.Serialization.XmlElementAttribute("Extension", Namespace = "http://www.onvif.org/ver10/schema")]
		public PTControlDirectionExtension extension {
			get {
				return this._extension;
			}
			set {
				this._extension = value;
			}
		}
	}

	[System.SerializableAttribute()]
	[System.Xml.Serialization.XmlTypeAttribute(TypeName = "EFlip", Namespace = "http://www.onvif.org/ver10/schema")]
	public partial class EFlip {

		private System.Xml.XmlAttribute[] _anyAttr;

		private EFlipMode _mode;

		private System.Xml.XmlElement[] _any;

		[System.Xml.Serialization.XmlAnyAttributeAttribute()]
		public System.Xml.XmlAttribute[] anyAttr {
			get {
				return this._anyAttr;
			}
			set {
				this._anyAttr = value;
			}
		}

		/// <summary>
		/// Parameter to enable/disable E-Flip feature.
		/// </summary>
		/// <remarks>reqired, order 0</remarks>
		[System.Xml.Serialization.XmlElementAttribute("Mode", Namespace = "http://www.onvif.org/ver10/schema")]
		public EFlipMode mode {
			get {
				return this._mode;
			}
			set {
				this._mode = value;
			}
		}

		/// <remarks>optional, order 1, namespace ##any</remarks>
		[System.Xml.Serialization.XmlAnyElementAttribute()]
		public System.Xml.XmlElement[] any {
			get {
				return this._any;
			}
			set {
				this._any = value;
			}
		}
	}

	[System.SerializableAttribute()]
	[System.Xml.Serialization.XmlTypeAttribute(TypeName = "EFlipMode", Namespace = "http://www.onvif.org/ver10/schema")]
	public enum EFlipMode {

		[System.Xml.Serialization.XmlEnumAttribute(Name = "OFF")]
		off,

		[System.Xml.Serialization.XmlEnumAttribute(Name = "ON")]
		on,

		[System.Xml.Serialization.XmlEnumAttribute(Name = "Extended")]
		extended,
	}

	[System.SerializableAttribute()]
	[System.Xml.Serialization.XmlTypeAttribute(TypeName = "Reverse", Namespace = "http://www.onvif.org/ver10/schema")]
	public partial class Reverse {

		private System.Xml.XmlAttribute[] _anyAttr;

		private ReverseMode _mode;

		private System.Xml.XmlElement[] _any;

		[System.Xml.Serialization.XmlAnyAttributeAttribute()]
		public System.Xml.XmlAttribute[] anyAttr {
			get {
				return this._anyAttr;
			}
			set {
				this._anyAttr = value;
			}
		}

		/// <summary>
		/// Parameter to enable/disable Reverse feature.
		/// </summary>
		/// <remarks>reqired, order 0</remarks>
		[System.Xml.Serialization.XmlElementAttribute("Mode", Namespace = "http://www.onvif.org/ver10/schema")]
		public ReverseMode mode {
			get {
				return this._mode;
			}
			set {
				this._mode = value;
			}
		}

		/// <remarks>optional, order 1, namespace ##any</remarks>
		[System.Xml.Serialization.XmlAnyElementAttribute()]
		public System.Xml.XmlElement[] any {
			get {
				return this._any;
			}
			set {
				this._any = value;
			}
		}
	}

	[System.SerializableAttribute()]
	[System.Xml.Serialization.XmlTypeAttribute(TypeName = "ReverseMode", Namespace = "http://www.onvif.org/ver10/schema")]
	public enum ReverseMode {

		[System.Xml.Serialization.XmlEnumAttribute(Name = "OFF")]
		off,

		[System.Xml.Serialization.XmlEnumAttribute(Name = "ON")]
		on,

		[System.Xml.Serialization.XmlEnumAttribute(Name = "AUTO")]
		auto,

		[System.Xml.Serialization.XmlEnumAttribute(Name = "Extended")]
		extended,
	}

	[System.SerializableAttribute()]
	[System.Xml.Serialization.XmlTypeAttribute(TypeName = "PTControlDirectionExtension", Namespace = "http://www.onvif.org/ver10/schema")]
	public partial class PTControlDirectionExtension {

		private System.Xml.XmlAttribute[] _anyAttr;

		private System.Xml.XmlElement[] _any;

		[System.Xml.Serialization.XmlAnyAttributeAttribute()]
		public System.Xml.XmlAttribute[] anyAttr {
			get {
				return this._anyAttr;
			}
			set {
				this._anyAttr = value;
			}
		}

		/// <remarks>optional, order 0, namespace ##any</remarks>
		[System.Xml.Serialization.XmlAnyElementAttribute()]
		public System.Xml.XmlElement[] any {
			get {
				return this._any;
			}
			set {
				this._any = value;
			}
		}
	}

	[System.SerializableAttribute()]
	[System.Xml.Serialization.XmlTypeAttribute(TypeName = "PTZConfigurationExtension2", Namespace = "http://www.onvif.org/ver10/schema")]
	public partial class PTZConfigurationExtension2 {

		private System.Xml.XmlElement[] _any;

		/// <remarks>optional, order 0, namespace ##targetNamespace</remarks>
		[System.Xml.Serialization.XmlAnyElementAttribute(Namespace = "http://www.onvif.org/ver10/schema")]
		public System.Xml.XmlElement[] any {
			get {
				return this._any;
			}
			set {
				this._any = value;
			}
		}
	}

	[System.SerializableAttribute()]
	[System.Xml.Serialization.XmlTypeAttribute(TypeName = "MetadataConfiguration", Namespace = "http://www.onvif.org/ver10/schema")]
	public partial class MetadataConfiguration {

		private string _token;

		private System.Xml.XmlAttribute[] _anyAttr;

		private string _name;

		private int _useCount;

		private PTZFilter _ptzStatus;

		private EventSubscription _events;

		private bool _analytics;

		private bool _analyticsSpecified;

		private MulticastConfiguration _multicast;

		private XsDuration _sessionTimeout;

		private System.Xml.XmlElement[] _any;

		private AnalyticsEngineConfiguration _analyticsEngineConfiguration;

		private MetadataConfigurationExtension _extension;

		/// <summary>
		/// Token that uniquely refernces this configuration. Length up to 64 characters.
		/// </summary>
		[System.Xml.Serialization.XmlAttributeAttribute("token")]
		public string token {
			get {
				return this._token;
			}
			set {
				this._token = value;
			}
		}

		[System.Xml.Serialization.XmlAnyAttributeAttribute()]
		public System.Xml.XmlAttribute[] anyAttr {
			get {
				return this._anyAttr;
			}
			set {
				this._anyAttr = value;
			}
		}

		/// <summary>
		/// User readable name. Length up to 64 characters.
		/// </summary>
		/// <remarks>reqired, order 0</remarks>
		[System.Xml.Serialization.XmlElementAttribute("Name", Namespace = "http://www.onvif.org/ver10/schema", DataType = "string")]
		public string name {
			get {
				return this._name;
			}
			set {
				this._name = value;
			}
		}

		/// <summary>
		/// Number of internal references currently using this configuration.
		/// This parameter is read-only and cannot be changed by a set request.
		/// For example the value increases if the configuration is added to a media profile or attached to a PaneConfiguration.
		/// </summary>
		/// <remarks>reqired, order 1</remarks>
		[System.Xml.Serialization.XmlElementAttribute("UseCount", Namespace = "http://www.onvif.org/ver10/schema", DataType = "int")]
		public int useCount {
			get {
				return this._useCount;
			}
			set {
				this._useCount = value;
			}
		}

		/// <summary>
		/// optional element to configure which PTZ related data is to include in the metadata stream
		/// </summary>
		/// <remarks>optional, order 2</remarks>
		[System.Xml.Serialization.XmlElementAttribute("PTZStatus", Namespace = "http://www.onvif.org/ver10/schema")]
		public PTZFilter ptzStatus {
			get {
				return this._ptzStatus;
			}
			set {
				this._ptzStatus = value;
			}
		}

		/// <summary>
		/// Optional element to configure the streaming of events. A client might be interested in receiving all,
		/// none or some of the events produced by the device:
		/// </summary>
		/// <remarks>optional, order 3</remarks>
		[System.Xml.Serialization.XmlElementAttribute("Events", Namespace = "http://www.onvif.org/ver10/schema")]
		public EventSubscription events {
			get {
				return this._events;
			}
			set {
				this._events = value;
			}
		}

		/// <summary>
		/// Defines if data to include from the analytics engine part shall be included in the stream
		/// </summary>
		/// <remarks>optional, order 4</remarks>
		[System.Xml.Serialization.XmlElementAttribute("Analytics", Namespace = "http://www.onvif.org/ver10/schema", DataType = "boolean")]
		public bool analytics {
			get {
				return this._analytics;
			}
			set {
				this._analytics = value;
			}
		}

		[System.Xml.Serialization.XmlIgnoreAttribute()]
		public bool analyticsSpecified {
			get {
				return this._analyticsSpecified;
			}
			set {
				this._analyticsSpecified = value;
			}
		}

		/// <summary>
		/// Defines the multicast settings that could be used for video streaming.
		/// </summary>
		/// <remarks>reqired, order 5</remarks>
		[System.Xml.Serialization.XmlElementAttribute("Multicast", Namespace = "http://www.onvif.org/ver10/schema")]
		public MulticastConfiguration multicast {
			get {
				return this._multicast;
			}
			set {
				this._multicast = value;
			}
		}

		/// <summary>
		/// The rtsp session timeout for the related audio stream
		/// </summary>
		/// <remarks>reqired, order 6</remarks>
		[System.Xml.Serialization.XmlElementAttribute("SessionTimeout", Namespace = "http://www.onvif.org/ver10/schema")]
		public XsDuration sessionTimeout {
			get {
				return this._sessionTimeout;
			}
			set {
				this._sessionTimeout = value;
			}
		}

		/// <remarks>optional, order 7, namespace ##other</remarks>
		[System.Xml.Serialization.XmlAnyElementAttribute()]
		public System.Xml.XmlElement[] any {
			get {
				return this._any;
			}
			set {
				this._any = value;
			}
		}

		/// <remarks>optional, order 8</remarks>
		[System.Xml.Serialization.XmlElementAttribute("AnalyticsEngineConfiguration", Namespace = "http://www.onvif.org/ver10/schema")]
		public AnalyticsEngineConfiguration analyticsEngineConfiguration {
			get {
				return this._analyticsEngineConfiguration;
			}
			set {
				this._analyticsEngineConfiguration = value;
			}
		}

		/// <remarks>optional, order 9</remarks>
		[System.Xml.Serialization.XmlElementAttribute("Extension", Namespace = "http://www.onvif.org/ver10/schema")]
		public MetadataConfigurationExtension extension {
			get {
				return this._extension;
			}
			set {
				this._extension = value;
			}
		}
	}

	[System.SerializableAttribute()]
	[System.Xml.Serialization.XmlTypeAttribute(TypeName = "PTZFilter", Namespace = "http://www.onvif.org/ver10/schema")]
	public partial class PTZFilter {

		private System.Xml.XmlAttribute[] _anyAttr;

		private bool _status;

		private bool _position;

		[System.Xml.Serialization.XmlAnyAttributeAttribute()]
		public System.Xml.XmlAttribute[] anyAttr {
			get {
				return this._anyAttr;
			}
			set {
				this._anyAttr = value;
			}
		}

		/// <summary>
		/// True if the metadata stream shall contain the PTZ status (IDLE, MOVING or UNKNOWN)
		/// </summary>
		/// <remarks>reqired, order 0</remarks>
		[System.Xml.Serialization.XmlElementAttribute("Status", Namespace = "http://www.onvif.org/ver10/schema", DataType = "boolean")]
		public bool status {
			get {
				return this._status;
			}
			set {
				this._status = value;
			}
		}

		/// <summary>
		/// True if the metadata stream shall contain the PTZ position
		/// </summary>
		/// <remarks>reqired, order 1</remarks>
		[System.Xml.Serialization.XmlElementAttribute("Position", Namespace = "http://www.onvif.org/ver10/schema", DataType = "boolean")]
		public bool position {
			get {
				return this._position;
			}
			set {
				this._position = value;
			}
		}
	}

	/// <summary>
	/// Subcription handling in the same way as base notification subscription.
	/// </summary>
	[System.SerializableAttribute()]
	[System.Xml.Serialization.XmlTypeAttribute(TypeName = "EventSubscription", Namespace = "http://www.onvif.org/ver10/schema")]
	public partial class EventSubscription {

		private System.Xml.XmlAttribute[] _anyAttr;

		private FilterType _filter;

		private SubscriptionPolicy _subscriptionPolicy;

		private System.Xml.XmlElement[] _any;

		[System.Xml.Serialization.XmlAnyAttributeAttribute()]
		public System.Xml.XmlAttribute[] anyAttr {
			get {
				return this._anyAttr;
			}
			set {
				this._anyAttr = value;
			}
		}

		/// <remarks>optional, order 0</remarks>
		[System.Xml.Serialization.XmlElementAttribute("Filter", Namespace = "http://www.onvif.org/ver10/schema")]
		public FilterType filter {
			get {
				return this._filter;
			}
			set {
				this._filter = value;
			}
		}

		/// <remarks>optional, order 1</remarks>
		[System.Xml.Serialization.XmlElementAttribute("SubscriptionPolicy", Namespace = "http://www.onvif.org/ver10/schema")]
		public SubscriptionPolicy subscriptionPolicy {
			get {
				return this._subscriptionPolicy;
			}
			set {
				this._subscriptionPolicy = value;
			}
		}

		/// <remarks>optional, order 2, namespace ##other</remarks>
		[System.Xml.Serialization.XmlAnyElementAttribute()]
		public System.Xml.XmlElement[] any {
			get {
				return this._any;
			}
			set {
				this._any = value;
			}
		}

		[System.SerializableAttribute()]
		[System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://www.onvif.org/ver10/schema")]
		public partial class SubscriptionPolicy {

			private System.Xml.XmlElement[] _any;

			/// <remarks>optional, order 0, namespace ##any</remarks>
			[System.Xml.Serialization.XmlAnyElementAttribute()]
			public System.Xml.XmlElement[] any {
				get {
					return this._any;
				}
				set {
					this._any = value;
				}
			}
		}
	}

	[System.SerializableAttribute()]
	[System.Xml.Serialization.XmlTypeAttribute(TypeName = "MetadataConfigurationExtension", Namespace = "http://www.onvif.org/ver10/schema")]
	public partial class MetadataConfigurationExtension {

		private System.Xml.XmlElement[] _any;

		/// <remarks>optional, order 0, namespace ##targetNamespace</remarks>
		[System.Xml.Serialization.XmlAnyElementAttribute(Namespace = "http://www.onvif.org/ver10/schema")]
		public System.Xml.XmlElement[] any {
			get {
				return this._any;
			}
			set {
				this._any = value;
			}
		}
	}

	[System.SerializableAttribute()]
	[System.Xml.Serialization.XmlTypeAttribute(TypeName = "ProfileExtension", Namespace = "http://www.onvif.org/ver10/schema")]
	public partial class ProfileExtension {

		private System.Xml.XmlAttribute[] _anyAttr;

		private System.Xml.XmlElement[] _any;

		private AudioOutputConfiguration _audioOutputConfiguration;

		private AudioDecoderConfiguration _audioDecoderConfiguration;

		private ProfileExtension2 _extension;

		[System.Xml.Serialization.XmlAnyAttributeAttribute()]
		public System.Xml.XmlAttribute[] anyAttr {
			get {
				return this._anyAttr;
			}
			set {
				this._anyAttr = value;
			}
		}

		/// <remarks>optional, order 0, namespace ##other</remarks>
		[System.Xml.Serialization.XmlAnyElementAttribute()]
		public System.Xml.XmlElement[] any {
			get {
				return this._any;
			}
			set {
				this._any = value;
			}
		}

		/// <summary>
		/// Optional configuration of the Audio output.
		/// </summary>
		/// <remarks>optional, order 1</remarks>
		[System.Xml.Serialization.XmlElementAttribute("AudioOutputConfiguration", Namespace = "http://www.onvif.org/ver10/schema")]
		public AudioOutputConfiguration audioOutputConfiguration {
			get {
				return this._audioOutputConfiguration;
			}
			set {
				this._audioOutputConfiguration = value;
			}
		}

		/// <summary>
		/// Optional configuration of the Audio decoder.
		/// </summary>
		/// <remarks>optional, order 2</remarks>
		[System.Xml.Serialization.XmlElementAttribute("AudioDecoderConfiguration", Namespace = "http://www.onvif.org/ver10/schema")]
		public AudioDecoderConfiguration audioDecoderConfiguration {
			get {
				return this._audioDecoderConfiguration;
			}
			set {
				this._audioDecoderConfiguration = value;
			}
		}

		/// <remarks>optional, order 3</remarks>
		[System.Xml.Serialization.XmlElementAttribute("Extension", Namespace = "http://www.onvif.org/ver10/schema")]
		public ProfileExtension2 extension {
			get {
				return this._extension;
			}
			set {
				this._extension = value;
			}
		}
	}

	[System.SerializableAttribute()]
	[System.Xml.Serialization.XmlTypeAttribute(TypeName = "AudioOutputConfiguration", Namespace = "http://www.onvif.org/ver10/schema")]
	public partial class AudioOutputConfiguration {

		private string _token;

		private System.Xml.XmlAttribute[] _anyAttr;

		private string _name;

		private int _useCount;

		private string _outputToken;

		private string _sendPrimacy;

		private int _outputLevel;

		private System.Xml.XmlElement[] _any;

		/// <summary>
		/// Token that uniquely refernces this configuration. Length up to 64 characters.
		/// </summary>
		[System.Xml.Serialization.XmlAttributeAttribute("token")]
		public string token {
			get {
				return this._token;
			}
			set {
				this._token = value;
			}
		}

		[System.Xml.Serialization.XmlAnyAttributeAttribute()]
		public System.Xml.XmlAttribute[] anyAttr {
			get {
				return this._anyAttr;
			}
			set {
				this._anyAttr = value;
			}
		}

		/// <summary>
		/// User readable name. Length up to 64 characters.
		/// </summary>
		/// <remarks>reqired, order 0</remarks>
		[System.Xml.Serialization.XmlElementAttribute("Name", Namespace = "http://www.onvif.org/ver10/schema", DataType = "string")]
		public string name {
			get {
				return this._name;
			}
			set {
				this._name = value;
			}
		}

		/// <summary>
		/// Number of internal references currently using this configuration.
		/// This parameter is read-only and cannot be changed by a set request.
		/// For example the value increases if the configuration is added to a media profile or attached to a PaneConfiguration.
		/// </summary>
		/// <remarks>reqired, order 1</remarks>
		[System.Xml.Serialization.XmlElementAttribute("UseCount", Namespace = "http://www.onvif.org/ver10/schema", DataType = "int")]
		public int useCount {
			get {
				return this._useCount;
			}
			set {
				this._useCount = value;
			}
		}

		/// <summary>
		/// Token of the phsycial Audio output.
		/// </summary>
		/// <remarks>reqired, order 2</remarks>
		[System.Xml.Serialization.XmlElementAttribute("OutputToken", Namespace = "http://www.onvif.org/ver10/schema", DataType = "string")]
		public string outputToken {
			get {
				return this._outputToken;
			}
			set {
				this._outputToken = value;
			}
		}

		/// <summary>
		/// An audio channel MAY support different types of audio transmission. While for full duplex
		/// operation no special handling is required, in half duplex operation the transmission direction
		/// needs to be switched.
		/// The optional SendPrimacy parameter inside the AudioOutputConfiguration indicates which
		/// direction is currently active. An NVC can switch between different modes by setting the
		/// AudioOutputConfiguration.
		/// The following modes for the Send-Primacy are defined:
		/// Acoustic echo cancellation is out of ONVIF scope.
		/// </summary>
		/// <remarks>optional, order 3</remarks>
		[System.Xml.Serialization.XmlElementAttribute("SendPrimacy", Namespace = "http://www.onvif.org/ver10/schema", DataType = "anyURI")]
		public string sendPrimacy {
			get {
				return this._sendPrimacy;
			}
			set {
				this._sendPrimacy = value;
			}
		}

		/// <summary>
		/// Volume setting of the output. The applicable range is defined via the option AudioOutputOptions.OutputLevelRange.
		/// </summary>
		/// <remarks>reqired, order 4</remarks>
		[System.Xml.Serialization.XmlElementAttribute("OutputLevel", Namespace = "http://www.onvif.org/ver10/schema", DataType = "int")]
		public int outputLevel {
			get {
				return this._outputLevel;
			}
			set {
				this._outputLevel = value;
			}
		}

		/// <remarks>optional, order 5, namespace ##any</remarks>
		[System.Xml.Serialization.XmlAnyElementAttribute()]
		public System.Xml.XmlElement[] any {
			get {
				return this._any;
			}
			set {
				this._any = value;
			}
		}
	}

	/// <summary>
	/// The Audio Decoder Configuration does not contain any that parameter to configure the
	/// decoding .A decoder shall decode every data it receives (according to its capabilities).
	/// </summary>
	[System.SerializableAttribute()]
	[System.Xml.Serialization.XmlTypeAttribute(TypeName = "AudioDecoderConfiguration", Namespace = "http://www.onvif.org/ver10/schema")]
	public partial class AudioDecoderConfiguration {

		private string _token;

		private System.Xml.XmlAttribute[] _anyAttr;

		private string _name;

		private int _useCount;

		private System.Xml.XmlElement[] _any;

		/// <summary>
		/// Token that uniquely refernces this configuration. Length up to 64 characters.
		/// </summary>
		[System.Xml.Serialization.XmlAttributeAttribute("token")]
		public string token {
			get {
				return this._token;
			}
			set {
				this._token = value;
			}
		}

		[System.Xml.Serialization.XmlAnyAttributeAttribute()]
		public System.Xml.XmlAttribute[] anyAttr {
			get {
				return this._anyAttr;
			}
			set {
				this._anyAttr = value;
			}
		}

		/// <summary>
		/// User readable name. Length up to 64 characters.
		/// </summary>
		/// <remarks>reqired, order 0</remarks>
		[System.Xml.Serialization.XmlElementAttribute("Name", Namespace = "http://www.onvif.org/ver10/schema", DataType = "string")]
		public string name {
			get {
				return this._name;
			}
			set {
				this._name = value;
			}
		}

		/// <summary>
		/// Number of internal references currently using this configuration.
		/// This parameter is read-only and cannot be changed by a set request.
		/// For example the value increases if the configuration is added to a media profile or attached to a PaneConfiguration.
		/// </summary>
		/// <remarks>reqired, order 1</remarks>
		[System.Xml.Serialization.XmlElementAttribute("UseCount", Namespace = "http://www.onvif.org/ver10/schema", DataType = "int")]
		public int useCount {
			get {
				return this._useCount;
			}
			set {
				this._useCount = value;
			}
		}

		/// <remarks>optional, order 2, namespace ##any</remarks>
		[System.Xml.Serialization.XmlAnyElementAttribute()]
		public System.Xml.XmlElement[] any {
			get {
				return this._any;
			}
			set {
				this._any = value;
			}
		}
	}

	[System.SerializableAttribute()]
	[System.Xml.Serialization.XmlTypeAttribute(TypeName = "ProfileExtension2", Namespace = "http://www.onvif.org/ver10/schema")]
	public partial class ProfileExtension2 {

		private System.Xml.XmlElement[] _any;

		/// <remarks>optional, order 0, namespace ##targetNamespace</remarks>
		[System.Xml.Serialization.XmlAnyElementAttribute(Namespace = "http://www.onvif.org/ver10/schema")]
		public System.Xml.XmlElement[] any {
			get {
				return this._any;
			}
			set {
				this._any = value;
			}
		}
	}

	/// <summary>
	/// Base type defining the common properties of a configuration.
	/// </summary>
	[System.SerializableAttribute()]
	[System.Xml.Serialization.XmlTypeAttribute(TypeName = "ConfigurationEntity", Namespace = "http://www.onvif.org/ver10/schema")]
	public partial class ConfigurationEntity {

		private string _token;

		private string _name;

		private int _useCount;

		/// <summary>
		/// Token that uniquely refernces this configuration. Length up to 64 characters.
		/// </summary>
		[System.Xml.Serialization.XmlAttributeAttribute("token")]
		public string token {
			get {
				return this._token;
			}
			set {
				this._token = value;
			}
		}

		/// <summary>
		/// User readable name. Length up to 64 characters.
		/// </summary>
		/// <remarks>reqired, order 0</remarks>
		[System.Xml.Serialization.XmlElementAttribute("Name", Namespace = "http://www.onvif.org/ver10/schema", DataType = "string")]
		public string name {
			get {
				return this._name;
			}
			set {
				this._name = value;
			}
		}

		/// <summary>
		/// Number of internal references currently using this configuration.
		/// This parameter is read-only and cannot be changed by a set request.
		/// For example the value increases if the configuration is added to a media profile or attached to a PaneConfiguration.
		/// </summary>
		/// <remarks>reqired, order 1</remarks>
		[System.Xml.Serialization.XmlElementAttribute("UseCount", Namespace = "http://www.onvif.org/ver10/schema", DataType = "int")]
		public int useCount {
			get {
				return this._useCount;
			}
			set {
				this._useCount = value;
			}
		}
	}

	[System.SerializableAttribute()]
	[System.Xml.Serialization.XmlTypeAttribute(TypeName = "VideoSourceConfigurationOptions", Namespace = "http://www.onvif.org/ver10/schema")]
	public partial class VideoSourceConfigurationOptions {

		private System.Xml.XmlAttribute[] _anyAttr;

		private IntRectangleRange _boundsRange;

		private string[] _videoSourceTokensAvailable;

		private VideoSourceConfigurationOptionsExtension _extension;

		[System.Xml.Serialization.XmlAnyAttributeAttribute()]
		public System.Xml.XmlAttribute[] anyAttr {
			get {
				return this._anyAttr;
			}
			set {
				this._anyAttr = value;
			}
		}

		/// <summary>
		/// Supported range for the capturing area.
		/// </summary>
		/// <remarks>reqired, order 0</remarks>
		[System.Xml.Serialization.XmlElementAttribute("BoundsRange", Namespace = "http://www.onvif.org/ver10/schema")]
		public IntRectangleRange boundsRange {
			get {
				return this._boundsRange;
			}
			set {
				this._boundsRange = value;
			}
		}

		/// <summary>
		/// List of physical inputs.
		/// </summary>
		/// <remarks>reqired, order 1</remarks>
		[System.Xml.Serialization.XmlElementAttribute("VideoSourceTokensAvailable", Namespace = "http://www.onvif.org/ver10/schema", DataType = "string")]
		public string[] videoSourceTokensAvailable {
			get {
				return this._videoSourceTokensAvailable;
			}
			set {
				this._videoSourceTokensAvailable = value;
			}
		}

		/// <remarks>optional, order 2</remarks>
		[System.Xml.Serialization.XmlElementAttribute("Extension", Namespace = "http://www.onvif.org/ver10/schema")]
		public VideoSourceConfigurationOptionsExtension extension {
			get {
				return this._extension;
			}
			set {
				this._extension = value;
			}
		}
	}

	[System.SerializableAttribute()]
	[System.Xml.Serialization.XmlTypeAttribute(TypeName = "VideoSourceConfigurationOptionsExtension", Namespace = "http://www.onvif.org/ver10/schema")]
	public partial class VideoSourceConfigurationOptionsExtension {

		private System.Xml.XmlElement[] _any;

		private RotateOptions _rotate;

		private VideoSourceConfigurationOptionsExtension2 _extension;

		/// <remarks>optional, order 0, namespace ##other</remarks>
		[System.Xml.Serialization.XmlAnyElementAttribute()]
		public System.Xml.XmlElement[] any {
			get {
				return this._any;
			}
			set {
				this._any = value;
			}
		}

		/// <summary>
		/// Options of parameters for Rotation feature.
		/// </summary>
		/// <remarks>optional, order 1</remarks>
		[System.Xml.Serialization.XmlElementAttribute("Rotate", Namespace = "http://www.onvif.org/ver10/schema")]
		public RotateOptions rotate {
			get {
				return this._rotate;
			}
			set {
				this._rotate = value;
			}
		}

		/// <remarks>optional, order 2</remarks>
		[System.Xml.Serialization.XmlElementAttribute("Extension", Namespace = "http://www.onvif.org/ver10/schema")]
		public VideoSourceConfigurationOptionsExtension2 extension {
			get {
				return this._extension;
			}
			set {
				this._extension = value;
			}
		}
	}

	[System.SerializableAttribute()]
	[System.Xml.Serialization.XmlTypeAttribute(TypeName = "RotateOptions", Namespace = "http://www.onvif.org/ver10/schema")]
	public partial class RotateOptions {

		private System.Xml.XmlAttribute[] _anyAttr;

		private RotateMode[] _mode;

		private IntList _degreeList;

		private RotateOptionsExtension _extension;

		[System.Xml.Serialization.XmlAnyAttributeAttribute()]
		public System.Xml.XmlAttribute[] anyAttr {
			get {
				return this._anyAttr;
			}
			set {
				this._anyAttr = value;
			}
		}

		/// <summary>
		/// Supported options of Rotate mode parameter.
		/// </summary>
		/// <remarks>reqired, order 0</remarks>
		[System.Xml.Serialization.XmlElementAttribute("Mode", Namespace = "http://www.onvif.org/ver10/schema")]
		public RotateMode[] mode {
			get {
				return this._mode;
			}
			set {
				this._mode = value;
			}
		}

		/// <summary>
		/// List of supported degree value for rotation.
		/// </summary>
		/// <remarks>optional, order 1</remarks>
		[System.Xml.Serialization.XmlElementAttribute("DegreeList", Namespace = "http://www.onvif.org/ver10/schema")]
		public IntList degreeList {
			get {
				return this._degreeList;
			}
			set {
				this._degreeList = value;
			}
		}

		/// <remarks>optional, order 2</remarks>
		[System.Xml.Serialization.XmlElementAttribute("Extension", Namespace = "http://www.onvif.org/ver10/schema")]
		public RotateOptionsExtension extension {
			get {
				return this._extension;
			}
			set {
				this._extension = value;
			}
		}
	}

	[System.SerializableAttribute()]
	[System.Xml.Serialization.XmlTypeAttribute(TypeName = "RotateOptionsExtension", Namespace = "http://www.onvif.org/ver10/schema")]
	public partial class RotateOptionsExtension {

		private System.Xml.XmlElement[] _any;

		/// <remarks>optional, order 0, namespace ##any</remarks>
		[System.Xml.Serialization.XmlAnyElementAttribute()]
		public System.Xml.XmlElement[] any {
			get {
				return this._any;
			}
			set {
				this._any = value;
			}
		}
	}

	[System.SerializableAttribute()]
	[System.Xml.Serialization.XmlTypeAttribute(TypeName = "VideoSourceConfigurationOptionsExtension2", Namespace = "http://www.onvif.org/ver10/schema")]
	public partial class VideoSourceConfigurationOptionsExtension2 {

		private System.Xml.XmlElement[] _any;

		/// <remarks>optional, order 0, namespace ##any</remarks>
		[System.Xml.Serialization.XmlAnyElementAttribute()]
		public System.Xml.XmlElement[] any {
			get {
				return this._any;
			}
			set {
				this._any = value;
			}
		}
	}

	[System.SerializableAttribute()]
	[System.Xml.Serialization.XmlTypeAttribute(TypeName = "VideoEncoderConfigurationOptions", Namespace = "http://www.onvif.org/ver10/schema")]
	public partial class VideoEncoderConfigurationOptions {

		private System.Xml.XmlAttribute[] _anyAttr;

		private IntRange _qualityRange;

		private JpegOptions _jpeg;

		private Mpeg4Options _mpeg4;

		private H264Options _h264;

		private VideoEncoderOptionsExtension _extension;

		[System.Xml.Serialization.XmlAnyAttributeAttribute()]
		public System.Xml.XmlAttribute[] anyAttr {
			get {
				return this._anyAttr;
			}
			set {
				this._anyAttr = value;
			}
		}

		/// <summary>
		/// Range of the quality values. A high value means higher quality.
		/// </summary>
		/// <remarks>reqired, order 0</remarks>
		[System.Xml.Serialization.XmlElementAttribute("QualityRange", Namespace = "http://www.onvif.org/ver10/schema")]
		public IntRange qualityRange {
			get {
				return this._qualityRange;
			}
			set {
				this._qualityRange = value;
			}
		}

		/// <summary>
		/// Optional JPEG encoder settings ranges (See also Extension element).
		/// </summary>
		/// <remarks>optional, order 1</remarks>
		[System.Xml.Serialization.XmlElementAttribute("JPEG", Namespace = "http://www.onvif.org/ver10/schema")]
		public JpegOptions jpeg {
			get {
				return this._jpeg;
			}
			set {
				this._jpeg = value;
			}
		}

		/// <summary>
		/// Optional MPEG-4 encoder settings ranges (See also Extension element).
		/// </summary>
		/// <remarks>optional, order 2</remarks>
		[System.Xml.Serialization.XmlElementAttribute("MPEG4", Namespace = "http://www.onvif.org/ver10/schema")]
		public Mpeg4Options mpeg4 {
			get {
				return this._mpeg4;
			}
			set {
				this._mpeg4 = value;
			}
		}

		/// <summary>
		/// Optional H.264 encoder settings ranges (See also Extension element).
		/// </summary>
		/// <remarks>optional, order 3</remarks>
		[System.Xml.Serialization.XmlElementAttribute("H264", Namespace = "http://www.onvif.org/ver10/schema")]
		public H264Options h264 {
			get {
				return this._h264;
			}
			set {
				this._h264 = value;
			}
		}

		/// <remarks>optional, order 4</remarks>
		[System.Xml.Serialization.XmlElementAttribute("Extension", Namespace = "http://www.onvif.org/ver10/schema")]
		public VideoEncoderOptionsExtension extension {
			get {
				return this._extension;
			}
			set {
				this._extension = value;
			}
		}
	}

	[System.SerializableAttribute()]
	[System.Xml.Serialization.XmlTypeAttribute(TypeName = "JpegOptions", Namespace = "http://www.onvif.org/ver10/schema")]
	public partial class JpegOptions {

		private VideoResolution[] _resolutionsAvailable;

		private IntRange _frameRateRange;

		private IntRange _encodingIntervalRange;

		/// <summary>
		/// List of supported image sizes.
		/// </summary>
		/// <remarks>reqired, order 0</remarks>
		[System.Xml.Serialization.XmlElementAttribute("ResolutionsAvailable", Namespace = "http://www.onvif.org/ver10/schema")]
		public VideoResolution[] resolutionsAvailable {
			get {
				return this._resolutionsAvailable;
			}
			set {
				this._resolutionsAvailable = value;
			}
		}

		/// <summary>
		/// Supported frame rate in fps (frames per second).
		/// </summary>
		/// <remarks>reqired, order 1</remarks>
		[System.Xml.Serialization.XmlElementAttribute("FrameRateRange", Namespace = "http://www.onvif.org/ver10/schema")]
		public IntRange frameRateRange {
			get {
				return this._frameRateRange;
			}
			set {
				this._frameRateRange = value;
			}
		}

		/// <summary>
		/// Supported encoding interval range. The encoding interval corresponds to the number of frames devided by the encoded frames. An encoding interval value of "1" means that all frames are encoded.
		/// </summary>
		/// <remarks>reqired, order 2</remarks>
		[System.Xml.Serialization.XmlElementAttribute("EncodingIntervalRange", Namespace = "http://www.onvif.org/ver10/schema")]
		public IntRange encodingIntervalRange {
			get {
				return this._encodingIntervalRange;
			}
			set {
				this._encodingIntervalRange = value;
			}
		}
	}

	[System.SerializableAttribute()]
	[System.Xml.Serialization.XmlTypeAttribute(TypeName = "Mpeg4Options", Namespace = "http://www.onvif.org/ver10/schema")]
	public partial class Mpeg4Options {

		private VideoResolution[] _resolutionsAvailable;

		private IntRange _govLengthRange;

		private IntRange _frameRateRange;

		private IntRange _encodingIntervalRange;

		private Mpeg4Profile[] _mpeg4ProfilesSupported;

		/// <summary>
		/// List of supported image sizes.
		/// </summary>
		/// <remarks>reqired, order 0</remarks>
		[System.Xml.Serialization.XmlElementAttribute("ResolutionsAvailable", Namespace = "http://www.onvif.org/ver10/schema")]
		public VideoResolution[] resolutionsAvailable {
			get {
				return this._resolutionsAvailable;
			}
			set {
				this._resolutionsAvailable = value;
			}
		}

		/// <summary>
		/// Supported group of Video frames length. This value typically corresponds to the I-Frame distance.
		/// </summary>
		/// <remarks>reqired, order 1</remarks>
		[System.Xml.Serialization.XmlElementAttribute("GovLengthRange", Namespace = "http://www.onvif.org/ver10/schema")]
		public IntRange govLengthRange {
			get {
				return this._govLengthRange;
			}
			set {
				this._govLengthRange = value;
			}
		}

		/// <summary>
		/// Supported frame rate in fps (frames per second).
		/// </summary>
		/// <remarks>reqired, order 2</remarks>
		[System.Xml.Serialization.XmlElementAttribute("FrameRateRange", Namespace = "http://www.onvif.org/ver10/schema")]
		public IntRange frameRateRange {
			get {
				return this._frameRateRange;
			}
			set {
				this._frameRateRange = value;
			}
		}

		/// <summary>
		/// Supported encoding interval range. The encoding interval corresponds to the number of frames devided by the encoded frames. An encoding interval value of "1" means that all frames are encoded.
		/// </summary>
		/// <remarks>reqired, order 3</remarks>
		[System.Xml.Serialization.XmlElementAttribute("EncodingIntervalRange", Namespace = "http://www.onvif.org/ver10/schema")]
		public IntRange encodingIntervalRange {
			get {
				return this._encodingIntervalRange;
			}
			set {
				this._encodingIntervalRange = value;
			}
		}

		/// <summary>
		/// List of supported MPEG-4 profiles.
		/// </summary>
		/// <remarks>reqired, order 4</remarks>
		[System.Xml.Serialization.XmlElementAttribute("Mpeg4ProfilesSupported", Namespace = "http://www.onvif.org/ver10/schema")]
		public Mpeg4Profile[] mpeg4ProfilesSupported {
			get {
				return this._mpeg4ProfilesSupported;
			}
			set {
				this._mpeg4ProfilesSupported = value;
			}
		}
	}

	[System.SerializableAttribute()]
	[System.Xml.Serialization.XmlTypeAttribute(TypeName = "H264Options", Namespace = "http://www.onvif.org/ver10/schema")]
	public partial class H264Options {

		private VideoResolution[] _resolutionsAvailable;

		private IntRange _govLengthRange;

		private IntRange _frameRateRange;

		private IntRange _encodingIntervalRange;

		private H264Profile[] _h264ProfilesSupported;

		/// <summary>
		/// List of supported image sizes.
		/// </summary>
		/// <remarks>reqired, order 0</remarks>
		[System.Xml.Serialization.XmlElementAttribute("ResolutionsAvailable", Namespace = "http://www.onvif.org/ver10/schema")]
		public VideoResolution[] resolutionsAvailable {
			get {
				return this._resolutionsAvailable;
			}
			set {
				this._resolutionsAvailable = value;
			}
		}

		/// <summary>
		/// Supported group of Video frames length. This value typically corresponds to the I-Frame distance.
		/// </summary>
		/// <remarks>reqired, order 1</remarks>
		[System.Xml.Serialization.XmlElementAttribute("GovLengthRange", Namespace = "http://www.onvif.org/ver10/schema")]
		public IntRange govLengthRange {
			get {
				return this._govLengthRange;
			}
			set {
				this._govLengthRange = value;
			}
		}

		/// <summary>
		/// Supported frame rate in fps (frames per second).
		/// </summary>
		/// <remarks>reqired, order 2</remarks>
		[System.Xml.Serialization.XmlElementAttribute("FrameRateRange", Namespace = "http://www.onvif.org/ver10/schema")]
		public IntRange frameRateRange {
			get {
				return this._frameRateRange;
			}
			set {
				this._frameRateRange = value;
			}
		}

		/// <summary>
		/// Supported encoding interval range. The encoding interval corresponds to the number of frames devided by the encoded frames. An encoding interval value of "1" means that all frames are encoded.
		/// </summary>
		/// <remarks>reqired, order 3</remarks>
		[System.Xml.Serialization.XmlElementAttribute("EncodingIntervalRange", Namespace = "http://www.onvif.org/ver10/schema")]
		public IntRange encodingIntervalRange {
			get {
				return this._encodingIntervalRange;
			}
			set {
				this._encodingIntervalRange = value;
			}
		}

		/// <summary>
		/// List of supported H.264 profiles.
		/// </summary>
		/// <remarks>reqired, order 4</remarks>
		[System.Xml.Serialization.XmlElementAttribute("H264ProfilesSupported", Namespace = "http://www.onvif.org/ver10/schema")]
		public H264Profile[] h264ProfilesSupported {
			get {
				return this._h264ProfilesSupported;
			}
			set {
				this._h264ProfilesSupported = value;
			}
		}
	}

	[System.SerializableAttribute()]
	[System.Xml.Serialization.XmlTypeAttribute(TypeName = "VideoEncoderOptionsExtension", Namespace = "http://www.onvif.org/ver10/schema")]
	public partial class VideoEncoderOptionsExtension {

		private System.Xml.XmlElement[] _any;

		private JpegOptions2 _jpeg;

		private Mpeg4Options2 _mpeg4;

		private H264Options2 _h264;

		private VideoEncoderOptionsExtension2 _extension;

		/// <remarks>optional, order 0, namespace ##other</remarks>
		[System.Xml.Serialization.XmlAnyElementAttribute()]
		public System.Xml.XmlElement[] any {
			get {
				return this._any;
			}
			set {
				this._any = value;
			}
		}

		/// <summary>
		/// Optional JPEG encoder settings ranges.
		/// </summary>
		/// <remarks>optional, order 1</remarks>
		[System.Xml.Serialization.XmlElementAttribute("JPEG", Namespace = "http://www.onvif.org/ver10/schema")]
		public JpegOptions2 jpeg {
			get {
				return this._jpeg;
			}
			set {
				this._jpeg = value;
			}
		}

		/// <summary>
		/// Optional MPEG-4 encoder settings ranges.
		/// </summary>
		/// <remarks>optional, order 2</remarks>
		[System.Xml.Serialization.XmlElementAttribute("MPEG4", Namespace = "http://www.onvif.org/ver10/schema")]
		public Mpeg4Options2 mpeg4 {
			get {
				return this._mpeg4;
			}
			set {
				this._mpeg4 = value;
			}
		}

		/// <summary>
		/// Optional H.264 encoder settings ranges.
		/// </summary>
		/// <remarks>optional, order 3</remarks>
		[System.Xml.Serialization.XmlElementAttribute("H264", Namespace = "http://www.onvif.org/ver10/schema")]
		public H264Options2 h264 {
			get {
				return this._h264;
			}
			set {
				this._h264 = value;
			}
		}

		/// <remarks>optional, order 4</remarks>
		[System.Xml.Serialization.XmlElementAttribute("Extension", Namespace = "http://www.onvif.org/ver10/schema")]
		public VideoEncoderOptionsExtension2 extension {
			get {
				return this._extension;
			}
			set {
				this._extension = value;
			}
		}
	}

	[System.SerializableAttribute()]
	[System.Xml.Serialization.XmlTypeAttribute(TypeName = "JpegOptions2", Namespace = "http://www.onvif.org/ver10/schema")]
	public partial class JpegOptions2 {

		private System.Xml.XmlAttribute[] _anyAttr;

		private VideoResolution[] _resolutionsAvailable;

		private IntRange _frameRateRange;

		private IntRange _encodingIntervalRange;

		private IntRange _bitrateRange;

		private System.Xml.XmlElement[] _any;

		[System.Xml.Serialization.XmlAnyAttributeAttribute()]
		public System.Xml.XmlAttribute[] anyAttr {
			get {
				return this._anyAttr;
			}
			set {
				this._anyAttr = value;
			}
		}

		/// <summary>
		/// List of supported image sizes.
		/// </summary>
		/// <remarks>reqired, order 0</remarks>
		[System.Xml.Serialization.XmlElementAttribute("ResolutionsAvailable", Namespace = "http://www.onvif.org/ver10/schema")]
		public VideoResolution[] resolutionsAvailable {
			get {
				return this._resolutionsAvailable;
			}
			set {
				this._resolutionsAvailable = value;
			}
		}

		/// <summary>
		/// Supported frame rate in fps (frames per second).
		/// </summary>
		/// <remarks>reqired, order 1</remarks>
		[System.Xml.Serialization.XmlElementAttribute("FrameRateRange", Namespace = "http://www.onvif.org/ver10/schema")]
		public IntRange frameRateRange {
			get {
				return this._frameRateRange;
			}
			set {
				this._frameRateRange = value;
			}
		}

		/// <summary>
		/// Supported encoding interval range. The encoding interval corresponds to the number of frames devided by the encoded frames. An encoding interval value of "1" means that all frames are encoded.
		/// </summary>
		/// <remarks>reqired, order 2</remarks>
		[System.Xml.Serialization.XmlElementAttribute("EncodingIntervalRange", Namespace = "http://www.onvif.org/ver10/schema")]
		public IntRange encodingIntervalRange {
			get {
				return this._encodingIntervalRange;
			}
			set {
				this._encodingIntervalRange = value;
			}
		}

		/// <summary>
		/// Supported range of encoded bitrate in kbps.
		/// </summary>
		/// <remarks>reqired, order 3</remarks>
		[System.Xml.Serialization.XmlElementAttribute("BitrateRange", Namespace = "http://www.onvif.org/ver10/schema")]
		public IntRange bitrateRange {
			get {
				return this._bitrateRange;
			}
			set {
				this._bitrateRange = value;
			}
		}

		/// <remarks>optional, order 4, namespace ##any</remarks>
		[System.Xml.Serialization.XmlAnyElementAttribute()]
		public System.Xml.XmlElement[] any {
			get {
				return this._any;
			}
			set {
				this._any = value;
			}
		}
	}

	[System.SerializableAttribute()]
	[System.Xml.Serialization.XmlTypeAttribute(TypeName = "Mpeg4Options2", Namespace = "http://www.onvif.org/ver10/schema")]
	public partial class Mpeg4Options2 {

		private System.Xml.XmlAttribute[] _anyAttr;

		private VideoResolution[] _resolutionsAvailable;

		private IntRange _govLengthRange;

		private IntRange _frameRateRange;

		private IntRange _encodingIntervalRange;

		private Mpeg4Profile[] _mpeg4ProfilesSupported;

		private IntRange _bitrateRange;

		private System.Xml.XmlElement[] _any;

		[System.Xml.Serialization.XmlAnyAttributeAttribute()]
		public System.Xml.XmlAttribute[] anyAttr {
			get {
				return this._anyAttr;
			}
			set {
				this._anyAttr = value;
			}
		}

		/// <summary>
		/// List of supported image sizes.
		/// </summary>
		/// <remarks>reqired, order 0</remarks>
		[System.Xml.Serialization.XmlElementAttribute("ResolutionsAvailable", Namespace = "http://www.onvif.org/ver10/schema")]
		public VideoResolution[] resolutionsAvailable {
			get {
				return this._resolutionsAvailable;
			}
			set {
				this._resolutionsAvailable = value;
			}
		}

		/// <summary>
		/// Supported group of Video frames length. This value typically corresponds to the I-Frame distance.
		/// </summary>
		/// <remarks>reqired, order 1</remarks>
		[System.Xml.Serialization.XmlElementAttribute("GovLengthRange", Namespace = "http://www.onvif.org/ver10/schema")]
		public IntRange govLengthRange {
			get {
				return this._govLengthRange;
			}
			set {
				this._govLengthRange = value;
			}
		}

		/// <summary>
		/// Supported frame rate in fps (frames per second).
		/// </summary>
		/// <remarks>reqired, order 2</remarks>
		[System.Xml.Serialization.XmlElementAttribute("FrameRateRange", Namespace = "http://www.onvif.org/ver10/schema")]
		public IntRange frameRateRange {
			get {
				return this._frameRateRange;
			}
			set {
				this._frameRateRange = value;
			}
		}

		/// <summary>
		/// Supported encoding interval range. The encoding interval corresponds to the number of frames devided by the encoded frames. An encoding interval value of "1" means that all frames are encoded.
		/// </summary>
		/// <remarks>reqired, order 3</remarks>
		[System.Xml.Serialization.XmlElementAttribute("EncodingIntervalRange", Namespace = "http://www.onvif.org/ver10/schema")]
		public IntRange encodingIntervalRange {
			get {
				return this._encodingIntervalRange;
			}
			set {
				this._encodingIntervalRange = value;
			}
		}

		/// <summary>
		/// List of supported MPEG-4 profiles.
		/// </summary>
		/// <remarks>reqired, order 4</remarks>
		[System.Xml.Serialization.XmlElementAttribute("Mpeg4ProfilesSupported", Namespace = "http://www.onvif.org/ver10/schema")]
		public Mpeg4Profile[] mpeg4ProfilesSupported {
			get {
				return this._mpeg4ProfilesSupported;
			}
			set {
				this._mpeg4ProfilesSupported = value;
			}
		}

		/// <summary>
		/// Supported range of encoded bitrate in kbps.
		/// </summary>
		/// <remarks>reqired, order 5</remarks>
		[System.Xml.Serialization.XmlElementAttribute("BitrateRange", Namespace = "http://www.onvif.org/ver10/schema")]
		public IntRange bitrateRange {
			get {
				return this._bitrateRange;
			}
			set {
				this._bitrateRange = value;
			}
		}

		/// <remarks>optional, order 6, namespace ##any</remarks>
		[System.Xml.Serialization.XmlAnyElementAttribute()]
		public System.Xml.XmlElement[] any {
			get {
				return this._any;
			}
			set {
				this._any = value;
			}
		}
	}

	[System.SerializableAttribute()]
	[System.Xml.Serialization.XmlTypeAttribute(TypeName = "H264Options2", Namespace = "http://www.onvif.org/ver10/schema")]
	public partial class H264Options2 {

		private System.Xml.XmlAttribute[] _anyAttr;

		private VideoResolution[] _resolutionsAvailable;

		private IntRange _govLengthRange;

		private IntRange _frameRateRange;

		private IntRange _encodingIntervalRange;

		private H264Profile[] _h264ProfilesSupported;

		private IntRange _bitrateRange;

		private System.Xml.XmlElement[] _any;

		[System.Xml.Serialization.XmlAnyAttributeAttribute()]
		public System.Xml.XmlAttribute[] anyAttr {
			get {
				return this._anyAttr;
			}
			set {
				this._anyAttr = value;
			}
		}

		/// <summary>
		/// List of supported image sizes.
		/// </summary>
		/// <remarks>reqired, order 0</remarks>
		[System.Xml.Serialization.XmlElementAttribute("ResolutionsAvailable", Namespace = "http://www.onvif.org/ver10/schema")]
		public VideoResolution[] resolutionsAvailable {
			get {
				return this._resolutionsAvailable;
			}
			set {
				this._resolutionsAvailable = value;
			}
		}

		/// <summary>
		/// Supported group of Video frames length. This value typically corresponds to the I-Frame distance.
		/// </summary>
		/// <remarks>reqired, order 1</remarks>
		[System.Xml.Serialization.XmlElementAttribute("GovLengthRange", Namespace = "http://www.onvif.org/ver10/schema")]
		public IntRange govLengthRange {
			get {
				return this._govLengthRange;
			}
			set {
				this._govLengthRange = value;
			}
		}

		/// <summary>
		/// Supported frame rate in fps (frames per second).
		/// </summary>
		/// <remarks>reqired, order 2</remarks>
		[System.Xml.Serialization.XmlElementAttribute("FrameRateRange", Namespace = "http://www.onvif.org/ver10/schema")]
		public IntRange frameRateRange {
			get {
				return this._frameRateRange;
			}
			set {
				this._frameRateRange = value;
			}
		}

		/// <summary>
		/// Supported encoding interval range. The encoding interval corresponds to the number of frames devided by the encoded frames. An encoding interval value of "1" means that all frames are encoded.
		/// </summary>
		/// <remarks>reqired, order 3</remarks>
		[System.Xml.Serialization.XmlElementAttribute("EncodingIntervalRange", Namespace = "http://www.onvif.org/ver10/schema")]
		public IntRange encodingIntervalRange {
			get {
				return this._encodingIntervalRange;
			}
			set {
				this._encodingIntervalRange = value;
			}
		}

		/// <summary>
		/// List of supported H.264 profiles.
		/// </summary>
		/// <remarks>reqired, order 4</remarks>
		[System.Xml.Serialization.XmlElementAttribute("H264ProfilesSupported", Namespace = "http://www.onvif.org/ver10/schema")]
		public H264Profile[] h264ProfilesSupported {
			get {
				return this._h264ProfilesSupported;
			}
			set {
				this._h264ProfilesSupported = value;
			}
		}

		/// <summary>
		/// Supported range of encoded bitrate in kbps.
		/// </summary>
		/// <remarks>reqired, order 5</remarks>
		[System.Xml.Serialization.XmlElementAttribute("BitrateRange", Namespace = "http://www.onvif.org/ver10/schema")]
		public IntRange bitrateRange {
			get {
				return this._bitrateRange;
			}
			set {
				this._bitrateRange = value;
			}
		}

		/// <remarks>optional, order 6, namespace ##any</remarks>
		[System.Xml.Serialization.XmlAnyElementAttribute()]
		public System.Xml.XmlElement[] any {
			get {
				return this._any;
			}
			set {
				this._any = value;
			}
		}
	}

	[System.SerializableAttribute()]
	[System.Xml.Serialization.XmlTypeAttribute(TypeName = "VideoEncoderOptionsExtension2", Namespace = "http://www.onvif.org/ver10/schema")]
	public partial class VideoEncoderOptionsExtension2 {

		private System.Xml.XmlElement[] _any;

		/// <remarks>optional, order 0, namespace ##targetNamespace</remarks>
		[System.Xml.Serialization.XmlAnyElementAttribute(Namespace = "http://www.onvif.org/ver10/schema")]
		public System.Xml.XmlElement[] any {
			get {
				return this._any;
			}
			set {
				this._any = value;
			}
		}
	}

	[System.SerializableAttribute()]
	[System.Xml.Serialization.XmlTypeAttribute(TypeName = "AudioSourceConfigurationOptions", Namespace = "http://www.onvif.org/ver10/schema")]
	public partial class AudioSourceConfigurationOptions {

		private System.Xml.XmlAttribute[] _anyAttr;

		private string[] _inputTokensAvailable;

		private AudioSourceOptionsExtension _extension;

		[System.Xml.Serialization.XmlAnyAttributeAttribute()]
		public System.Xml.XmlAttribute[] anyAttr {
			get {
				return this._anyAttr;
			}
			set {
				this._anyAttr = value;
			}
		}

		/// <summary>
		/// Tokens of the audio source the configuration can be used for.
		/// </summary>
		/// <remarks>reqired, order 0</remarks>
		[System.Xml.Serialization.XmlElementAttribute("InputTokensAvailable", Namespace = "http://www.onvif.org/ver10/schema", DataType = "string")]
		public string[] inputTokensAvailable {
			get {
				return this._inputTokensAvailable;
			}
			set {
				this._inputTokensAvailable = value;
			}
		}

		/// <remarks>optional, order 1</remarks>
		[System.Xml.Serialization.XmlElementAttribute("Extension", Namespace = "http://www.onvif.org/ver10/schema")]
		public AudioSourceOptionsExtension extension {
			get {
				return this._extension;
			}
			set {
				this._extension = value;
			}
		}
	}

	[System.SerializableAttribute()]
	[System.Xml.Serialization.XmlTypeAttribute(TypeName = "AudioSourceOptionsExtension", Namespace = "http://www.onvif.org/ver10/schema")]
	public partial class AudioSourceOptionsExtension {

		private System.Xml.XmlElement[] _any;

		/// <remarks>optional, order 0, namespace ##any</remarks>
		[System.Xml.Serialization.XmlAnyElementAttribute()]
		public System.Xml.XmlElement[] any {
			get {
				return this._any;
			}
			set {
				this._any = value;
			}
		}
	}

	[System.SerializableAttribute()]
	[System.Xml.Serialization.XmlTypeAttribute(TypeName = "AudioEncoderConfigurationOptions", Namespace = "http://www.onvif.org/ver10/schema")]
	public partial class AudioEncoderConfigurationOptions {

		private System.Xml.XmlAttribute[] _anyAttr;

		private AudioEncoderConfigurationOption[] _options;

		[System.Xml.Serialization.XmlAnyAttributeAttribute()]
		public System.Xml.XmlAttribute[] anyAttr {
			get {
				return this._anyAttr;
			}
			set {
				this._anyAttr = value;
			}
		}

		/// <summary>
		/// list of supported AudioEncoderConfigurations
		/// </summary>
		/// <remarks>optional, order 0</remarks>
		[System.Xml.Serialization.XmlElementAttribute("Options", Namespace = "http://www.onvif.org/ver10/schema")]
		public AudioEncoderConfigurationOption[] options {
			get {
				return this._options;
			}
			set {
				this._options = value;
			}
		}
	}

	[System.SerializableAttribute()]
	[System.Xml.Serialization.XmlTypeAttribute(TypeName = "AudioEncoderConfigurationOption", Namespace = "http://www.onvif.org/ver10/schema")]
	public partial class AudioEncoderConfigurationOption {

		private System.Xml.XmlAttribute[] _anyAttr;

		private AudioEncoding _encoding;

		private IntList _bitrateList;

		private IntList _sampleRateList;

		private System.Xml.XmlElement[] _any;

		[System.Xml.Serialization.XmlAnyAttributeAttribute()]
		public System.Xml.XmlAttribute[] anyAttr {
			get {
				return this._anyAttr;
			}
			set {
				this._anyAttr = value;
			}
		}

		/// <summary>
		/// The enoding used for audio data (either G.711, G.726 or AAC)
		/// </summary>
		/// <remarks>reqired, order 0</remarks>
		[System.Xml.Serialization.XmlElementAttribute("Encoding", Namespace = "http://www.onvif.org/ver10/schema")]
		public AudioEncoding encoding {
			get {
				return this._encoding;
			}
			set {
				this._encoding = value;
			}
		}

		/// <summary>
		/// List of supported bitrates in kbps for the specified Encoding
		/// </summary>
		/// <remarks>reqired, order 1</remarks>
		[System.Xml.Serialization.XmlElementAttribute("BitrateList", Namespace = "http://www.onvif.org/ver10/schema")]
		public IntList bitrateList {
			get {
				return this._bitrateList;
			}
			set {
				this._bitrateList = value;
			}
		}

		/// <summary>
		/// List of supported Sample Rates in kHz for the specified Encoding
		/// </summary>
		/// <remarks>reqired, order 2</remarks>
		[System.Xml.Serialization.XmlElementAttribute("SampleRateList", Namespace = "http://www.onvif.org/ver10/schema")]
		public IntList sampleRateList {
			get {
				return this._sampleRateList;
			}
			set {
				this._sampleRateList = value;
			}
		}

		/// <remarks>optional, order 3, namespace ##any</remarks>
		[System.Xml.Serialization.XmlAnyElementAttribute()]
		public System.Xml.XmlElement[] any {
			get {
				return this._any;
			}
			set {
				this._any = value;
			}
		}
	}

	[System.SerializableAttribute()]
	[System.Xml.Serialization.XmlTypeAttribute(TypeName = "MetadataConfigurationOptions", Namespace = "http://www.onvif.org/ver10/schema")]
	public partial class MetadataConfigurationOptions {

		private System.Xml.XmlAttribute[] _anyAttr;

		private PTZStatusFilterOptions _ptzStatusFilterOptions;

		private System.Xml.XmlElement[] _any;

		[System.Xml.Serialization.XmlAnyAttributeAttribute()]
		public System.Xml.XmlAttribute[] anyAttr {
			get {
				return this._anyAttr;
			}
			set {
				this._anyAttr = value;
			}
		}

		/// <remarks>reqired, order 0</remarks>
		[System.Xml.Serialization.XmlElementAttribute("PTZStatusFilterOptions", Namespace = "http://www.onvif.org/ver10/schema")]
		public PTZStatusFilterOptions ptzStatusFilterOptions {
			get {
				return this._ptzStatusFilterOptions;
			}
			set {
				this._ptzStatusFilterOptions = value;
			}
		}

		/// <remarks>optional, order 1, namespace ##any</remarks>
		[System.Xml.Serialization.XmlAnyElementAttribute()]
		public System.Xml.XmlElement[] any {
			get {
				return this._any;
			}
			set {
				this._any = value;
			}
		}
	}

	[System.SerializableAttribute()]
	[System.Xml.Serialization.XmlTypeAttribute(TypeName = "PTZStatusFilterOptions", Namespace = "http://www.onvif.org/ver10/schema")]
	public partial class PTZStatusFilterOptions {

		private System.Xml.XmlAttribute[] _anyAttr;

		private bool _panTiltStatusSupported;

		private bool _zoomStatusSupported;

		private System.Xml.XmlElement[] _any;

		private bool _panTiltPositionSupported;

		private bool _panTiltPositionSupportedSpecified;

		private bool _zoomPositionSupported;

		private bool _zoomPositionSupportedSpecified;

		private PTZStatusFilterOptionsExtension _extension;

		[System.Xml.Serialization.XmlAnyAttributeAttribute()]
		public System.Xml.XmlAttribute[] anyAttr {
			get {
				return this._anyAttr;
			}
			set {
				this._anyAttr = value;
			}
		}

		/// <summary>
		/// True if the device is able to stream pan or tilt status information.
		/// </summary>
		/// <remarks>reqired, order 0</remarks>
		[System.Xml.Serialization.XmlElementAttribute("PanTiltStatusSupported", Namespace = "http://www.onvif.org/ver10/schema", DataType = "boolean")]
		public bool panTiltStatusSupported {
			get {
				return this._panTiltStatusSupported;
			}
			set {
				this._panTiltStatusSupported = value;
			}
		}

		/// <summary>
		/// True if the device is able to stream zoom status inforamtion.
		/// </summary>
		/// <remarks>reqired, order 1</remarks>
		[System.Xml.Serialization.XmlElementAttribute("ZoomStatusSupported", Namespace = "http://www.onvif.org/ver10/schema", DataType = "boolean")]
		public bool zoomStatusSupported {
			get {
				return this._zoomStatusSupported;
			}
			set {
				this._zoomStatusSupported = value;
			}
		}

		/// <remarks>optional, order 2, namespace ##other</remarks>
		[System.Xml.Serialization.XmlAnyElementAttribute()]
		public System.Xml.XmlElement[] any {
			get {
				return this._any;
			}
			set {
				this._any = value;
			}
		}

		/// <summary>
		/// True if the device is able to stream the pan or tilt position.
		/// </summary>
		/// <remarks>optional, order 3</remarks>
		[System.Xml.Serialization.XmlElementAttribute("PanTiltPositionSupported", Namespace = "http://www.onvif.org/ver10/schema", DataType = "boolean")]
		public bool panTiltPositionSupported {
			get {
				return this._panTiltPositionSupported;
			}
			set {
				this._panTiltPositionSupported = value;
			}
		}

		[System.Xml.Serialization.XmlIgnoreAttribute()]
		public bool panTiltPositionSupportedSpecified {
			get {
				return this._panTiltPositionSupportedSpecified;
			}
			set {
				this._panTiltPositionSupportedSpecified = value;
			}
		}

		/// <summary>
		/// True if the device is able to stream zoom position information.
		/// </summary>
		/// <remarks>optional, order 4</remarks>
		[System.Xml.Serialization.XmlElementAttribute("ZoomPositionSupported", Namespace = "http://www.onvif.org/ver10/schema", DataType = "boolean")]
		public bool zoomPositionSupported {
			get {
				return this._zoomPositionSupported;
			}
			set {
				this._zoomPositionSupported = value;
			}
		}

		[System.Xml.Serialization.XmlIgnoreAttribute()]
		public bool zoomPositionSupportedSpecified {
			get {
				return this._zoomPositionSupportedSpecified;
			}
			set {
				this._zoomPositionSupportedSpecified = value;
			}
		}

		/// <remarks>optional, order 5</remarks>
		[System.Xml.Serialization.XmlElementAttribute("Extension", Namespace = "http://www.onvif.org/ver10/schema")]
		public PTZStatusFilterOptionsExtension extension {
			get {
				return this._extension;
			}
			set {
				this._extension = value;
			}
		}
	}

	[System.SerializableAttribute()]
	[System.Xml.Serialization.XmlTypeAttribute(TypeName = "PTZStatusFilterOptionsExtension", Namespace = "http://www.onvif.org/ver10/schema")]
	public partial class PTZStatusFilterOptionsExtension {

		private System.Xml.XmlElement[] _any;

		/// <remarks>optional, order 0, namespace ##targetNamespace</remarks>
		[System.Xml.Serialization.XmlAnyElementAttribute(Namespace = "http://www.onvif.org/ver10/schema")]
		public System.Xml.XmlElement[] any {
			get {
				return this._any;
			}
			set {
				this._any = value;
			}
		}
	}

	/// <summary>
	/// Representation of a physical video outputs.
	/// </summary>
	[System.SerializableAttribute()]
	[System.Xml.Serialization.XmlTypeAttribute(TypeName = "VideoOutput", Namespace = "http://www.onvif.org/ver10/schema")]
	public partial class VideoOutput {

		private string _token;

		private System.Xml.XmlAttribute[] _anyAttr;

		private Layout _layout;

		private VideoResolution _resolution;

		private float _refreshRate;

		private bool _refreshRateSpecified;

		private float _aspectRatio;

		private bool _aspectRatioSpecified;

		private VideoOutputExtension _extension;

		/// <summary>
		/// Unique identifier referencing the physical entity.
		/// </summary>
		[System.Xml.Serialization.XmlAttributeAttribute("token")]
		public string token {
			get {
				return this._token;
			}
			set {
				this._token = value;
			}
		}

		[System.Xml.Serialization.XmlAnyAttributeAttribute()]
		public System.Xml.XmlAttribute[] anyAttr {
			get {
				return this._anyAttr;
			}
			set {
				this._anyAttr = value;
			}
		}

		/// <remarks>reqired, order 0</remarks>
		[System.Xml.Serialization.XmlElementAttribute("Layout", Namespace = "http://www.onvif.org/ver10/schema")]
		public Layout layout {
			get {
				return this._layout;
			}
			set {
				this._layout = value;
			}
		}

		/// <summary>
		/// Resolution of the display in Pixel.
		/// </summary>
		/// <remarks>optional, order 1</remarks>
		[System.Xml.Serialization.XmlElementAttribute("Resolution", Namespace = "http://www.onvif.org/ver10/schema")]
		public VideoResolution resolution {
			get {
				return this._resolution;
			}
			set {
				this._resolution = value;
			}
		}

		/// <summary>
		/// Refresh rate of the display in Hertz.
		/// </summary>
		/// <remarks>optional, order 2</remarks>
		[System.Xml.Serialization.XmlElementAttribute("RefreshRate", Namespace = "http://www.onvif.org/ver10/schema", DataType = "float")]
		public float refreshRate {
			get {
				return this._refreshRate;
			}
			set {
				this._refreshRate = value;
			}
		}

		[System.Xml.Serialization.XmlIgnoreAttribute()]
		public bool refreshRateSpecified {
			get {
				return this._refreshRateSpecified;
			}
			set {
				this._refreshRateSpecified = value;
			}
		}

		/// <summary>
		/// Aspect ratio of the display as physical extent of width divided by height.
		/// </summary>
		/// <remarks>optional, order 3</remarks>
		[System.Xml.Serialization.XmlElementAttribute("AspectRatio", Namespace = "http://www.onvif.org/ver10/schema", DataType = "float")]
		public float aspectRatio {
			get {
				return this._aspectRatio;
			}
			set {
				this._aspectRatio = value;
			}
		}

		[System.Xml.Serialization.XmlIgnoreAttribute()]
		public bool aspectRatioSpecified {
			get {
				return this._aspectRatioSpecified;
			}
			set {
				this._aspectRatioSpecified = value;
			}
		}

		/// <remarks>optional, order 4</remarks>
		[System.Xml.Serialization.XmlElementAttribute("Extension", Namespace = "http://www.onvif.org/ver10/schema")]
		public VideoOutputExtension extension {
			get {
				return this._extension;
			}
			set {
				this._extension = value;
			}
		}
	}

	/// <summary>
	/// A layout describes a set of Video windows that are displayed simultaniously on a display.
	/// </summary>
	[System.SerializableAttribute()]
	[System.Xml.Serialization.XmlTypeAttribute(TypeName = "Layout", Namespace = "http://www.onvif.org/ver10/schema")]
	public partial class Layout {

		private System.Xml.XmlAttribute[] _anyAttr;

		private PaneLayout[] _paneLayout;

		private LayoutExtension _extension;

		[System.Xml.Serialization.XmlAnyAttributeAttribute()]
		public System.Xml.XmlAttribute[] anyAttr {
			get {
				return this._anyAttr;
			}
			set {
				this._anyAttr = value;
			}
		}

		/// <summary>
		/// List of panes assembling the display layout.
		/// </summary>
		/// <remarks>reqired, order 0</remarks>
		[System.Xml.Serialization.XmlElementAttribute("PaneLayout", Namespace = "http://www.onvif.org/ver10/schema")]
		public PaneLayout[] paneLayout {
			get {
				return this._paneLayout;
			}
			set {
				this._paneLayout = value;
			}
		}

		/// <remarks>optional, order 1</remarks>
		[System.Xml.Serialization.XmlElementAttribute("Extension", Namespace = "http://www.onvif.org/ver10/schema")]
		public LayoutExtension extension {
			get {
				return this._extension;
			}
			set {
				this._extension = value;
			}
		}
	}

	/// <summary>
	/// A pane layout describes one Video window of a display. It links a pane configuration to a region of the screen.
	/// </summary>
	[System.SerializableAttribute()]
	[System.Xml.Serialization.XmlTypeAttribute(TypeName = "PaneLayout", Namespace = "http://www.onvif.org/ver10/schema")]
	public partial class PaneLayout {

		private System.Xml.XmlAttribute[] _anyAttr;

		private string _pane;

		private Rectangle _area;

		private System.Xml.XmlElement[] _any;

		[System.Xml.Serialization.XmlAnyAttributeAttribute()]
		public System.Xml.XmlAttribute[] anyAttr {
			get {
				return this._anyAttr;
			}
			set {
				this._anyAttr = value;
			}
		}

		/// <summary>
		/// Reference to the configuration of the streaming and coding parameters.
		/// </summary>
		/// <remarks>reqired, order 0</remarks>
		[System.Xml.Serialization.XmlElementAttribute("Pane", Namespace = "http://www.onvif.org/ver10/schema", DataType = "string")]
		public string pane {
			get {
				return this._pane;
			}
			set {
				this._pane = value;
			}
		}

		/// <summary>
		/// Describes the location and size of the area on the monitor. The area coordinate values are espressed in normalized units [-1.0, 1.0].
		/// </summary>
		/// <remarks>reqired, order 1</remarks>
		[System.Xml.Serialization.XmlElementAttribute("Area", Namespace = "http://www.onvif.org/ver10/schema")]
		public Rectangle area {
			get {
				return this._area;
			}
			set {
				this._area = value;
			}
		}

		/// <remarks>optional, order 2, namespace ##any</remarks>
		[System.Xml.Serialization.XmlAnyElementAttribute()]
		public System.Xml.XmlElement[] any {
			get {
				return this._any;
			}
			set {
				this._any = value;
			}
		}
	}

	[System.SerializableAttribute()]
	[System.Xml.Serialization.XmlTypeAttribute(TypeName = "LayoutExtension", Namespace = "http://www.onvif.org/ver10/schema")]
	public partial class LayoutExtension {

		private System.Xml.XmlElement[] _any;

		/// <remarks>optional, order 0, namespace ##any</remarks>
		[System.Xml.Serialization.XmlAnyElementAttribute()]
		public System.Xml.XmlElement[] any {
			get {
				return this._any;
			}
			set {
				this._any = value;
			}
		}
	}

	[System.SerializableAttribute()]
	[System.Xml.Serialization.XmlTypeAttribute(TypeName = "VideoOutputExtension", Namespace = "http://www.onvif.org/ver10/schema")]
	public partial class VideoOutputExtension {

		private System.Xml.XmlElement[] _any;

		/// <remarks>optional, order 0, namespace ##targetNamespace</remarks>
		[System.Xml.Serialization.XmlAnyElementAttribute(Namespace = "http://www.onvif.org/ver10/schema")]
		public System.Xml.XmlElement[] any {
			get {
				return this._any;
			}
			set {
				this._any = value;
			}
		}
	}

	[System.SerializableAttribute()]
	[System.Xml.Serialization.XmlTypeAttribute(TypeName = "VideoOutputConfiguration", Namespace = "http://www.onvif.org/ver10/schema")]
	public partial class VideoOutputConfiguration {

		private string _token;

		private System.Xml.XmlAttribute[] _anyAttr;

		private string _name;

		private int _useCount;

		private string _outputToken;

		private System.Xml.XmlElement[] _any;

		/// <summary>
		/// Token that uniquely refernces this configuration. Length up to 64 characters.
		/// </summary>
		[System.Xml.Serialization.XmlAttributeAttribute("token")]
		public string token {
			get {
				return this._token;
			}
			set {
				this._token = value;
			}
		}

		[System.Xml.Serialization.XmlAnyAttributeAttribute()]
		public System.Xml.XmlAttribute[] anyAttr {
			get {
				return this._anyAttr;
			}
			set {
				this._anyAttr = value;
			}
		}

		/// <summary>
		/// User readable name. Length up to 64 characters.
		/// </summary>
		/// <remarks>reqired, order 0</remarks>
		[System.Xml.Serialization.XmlElementAttribute("Name", Namespace = "http://www.onvif.org/ver10/schema", DataType = "string")]
		public string name {
			get {
				return this._name;
			}
			set {
				this._name = value;
			}
		}

		/// <summary>
		/// Number of internal references currently using this configuration.
		/// This parameter is read-only and cannot be changed by a set request.
		/// For example the value increases if the configuration is added to a media profile or attached to a PaneConfiguration.
		/// </summary>
		/// <remarks>reqired, order 1</remarks>
		[System.Xml.Serialization.XmlElementAttribute("UseCount", Namespace = "http://www.onvif.org/ver10/schema", DataType = "int")]
		public int useCount {
			get {
				return this._useCount;
			}
			set {
				this._useCount = value;
			}
		}

		/// <summary>
		/// Token of the Video Output the configuration applies to
		/// </summary>
		/// <remarks>reqired, order 2</remarks>
		[System.Xml.Serialization.XmlElementAttribute("OutputToken", Namespace = "http://www.onvif.org/ver10/schema", DataType = "string")]
		public string outputToken {
			get {
				return this._outputToken;
			}
			set {
				this._outputToken = value;
			}
		}

		/// <remarks>optional, order 3, namespace ##any</remarks>
		[System.Xml.Serialization.XmlAnyElementAttribute()]
		public System.Xml.XmlElement[] any {
			get {
				return this._any;
			}
			set {
				this._any = value;
			}
		}
	}

	[System.SerializableAttribute()]
	[System.Xml.Serialization.XmlTypeAttribute(TypeName = "VideoOutputConfigurationOptions", Namespace = "http://www.onvif.org/ver10/schema")]
	public partial class VideoOutputConfigurationOptions {

		private System.Xml.XmlAttribute[] _anyAttr;

		private System.Xml.XmlElement[] _any;

		[System.Xml.Serialization.XmlAnyAttributeAttribute()]
		public System.Xml.XmlAttribute[] anyAttr {
			get {
				return this._anyAttr;
			}
			set {
				this._anyAttr = value;
			}
		}

		/// <remarks>optional, order 0, namespace ##any</remarks>
		[System.Xml.Serialization.XmlAnyElementAttribute()]
		public System.Xml.XmlElement[] any {
			get {
				return this._any;
			}
			set {
				this._any = value;
			}
		}
	}

	[System.SerializableAttribute()]
	[System.Xml.Serialization.XmlTypeAttribute(TypeName = "VideoDecoderConfigurationOptions", Namespace = "http://www.onvif.org/ver10/schema")]
	public partial class VideoDecoderConfigurationOptions {

		private System.Xml.XmlAttribute[] _anyAttr;

		private JpegDecOptions _jpegDecOptions;

		private H264DecOptions _h264DecOptions;

		private Mpeg4DecOptions _mpeg4DecOptions;

		private VideoDecoderConfigurationOptionsExtension _extension;

		[System.Xml.Serialization.XmlAnyAttributeAttribute()]
		public System.Xml.XmlAttribute[] anyAttr {
			get {
				return this._anyAttr;
			}
			set {
				this._anyAttr = value;
			}
		}

		/// <summary>
		/// If the device is able to decode Jpeg streams this element describes the supported codecs and configurations
		/// </summary>
		/// <remarks>optional, order 0</remarks>
		[System.Xml.Serialization.XmlElementAttribute("JpegDecOptions", Namespace = "http://www.onvif.org/ver10/schema")]
		public JpegDecOptions jpegDecOptions {
			get {
				return this._jpegDecOptions;
			}
			set {
				this._jpegDecOptions = value;
			}
		}

		/// <summary>
		/// If the device is able to decode H.264 streams this element describes the supported codecs and configurations
		/// </summary>
		/// <remarks>optional, order 1</remarks>
		[System.Xml.Serialization.XmlElementAttribute("H264DecOptions", Namespace = "http://www.onvif.org/ver10/schema")]
		public H264DecOptions h264DecOptions {
			get {
				return this._h264DecOptions;
			}
			set {
				this._h264DecOptions = value;
			}
		}

		/// <summary>
		/// If the device is able to decode Mpeg4 streams this element describes the supported codecs and configurations
		/// </summary>
		/// <remarks>optional, order 2</remarks>
		[System.Xml.Serialization.XmlElementAttribute("Mpeg4DecOptions", Namespace = "http://www.onvif.org/ver10/schema")]
		public Mpeg4DecOptions mpeg4DecOptions {
			get {
				return this._mpeg4DecOptions;
			}
			set {
				this._mpeg4DecOptions = value;
			}
		}

		/// <remarks>optional, order 3</remarks>
		[System.Xml.Serialization.XmlElementAttribute("Extension", Namespace = "http://www.onvif.org/ver10/schema")]
		public VideoDecoderConfigurationOptionsExtension extension {
			get {
				return this._extension;
			}
			set {
				this._extension = value;
			}
		}
	}

	[System.SerializableAttribute()]
	[System.Xml.Serialization.XmlTypeAttribute(TypeName = "JpegDecOptions", Namespace = "http://www.onvif.org/ver10/schema")]
	public partial class JpegDecOptions {

		private System.Xml.XmlAttribute[] _anyAttr;

		private VideoResolution[] _resolutionsAvailable;

		private IntRange _supportedInputBitrate;

		private IntRange _supportedFrameRate;

		private System.Xml.XmlElement[] _any;

		[System.Xml.Serialization.XmlAnyAttributeAttribute()]
		public System.Xml.XmlAttribute[] anyAttr {
			get {
				return this._anyAttr;
			}
			set {
				this._anyAttr = value;
			}
		}

		/// <summary>
		/// List of supported Jpeg Video Resolutions
		/// </summary>
		/// <remarks>reqired, order 0</remarks>
		[System.Xml.Serialization.XmlElementAttribute("ResolutionsAvailable", Namespace = "http://www.onvif.org/ver10/schema")]
		public VideoResolution[] resolutionsAvailable {
			get {
				return this._resolutionsAvailable;
			}
			set {
				this._resolutionsAvailable = value;
			}
		}

		/// <summary>
		/// Supported Jpeg bitrate range in kbps
		/// </summary>
		/// <remarks>reqired, order 1</remarks>
		[System.Xml.Serialization.XmlElementAttribute("SupportedInputBitrate", Namespace = "http://www.onvif.org/ver10/schema")]
		public IntRange supportedInputBitrate {
			get {
				return this._supportedInputBitrate;
			}
			set {
				this._supportedInputBitrate = value;
			}
		}

		/// <summary>
		/// Supported Jpeg framerate range in fps
		/// </summary>
		/// <remarks>reqired, order 2</remarks>
		[System.Xml.Serialization.XmlElementAttribute("SupportedFrameRate", Namespace = "http://www.onvif.org/ver10/schema")]
		public IntRange supportedFrameRate {
			get {
				return this._supportedFrameRate;
			}
			set {
				this._supportedFrameRate = value;
			}
		}

		/// <remarks>optional, order 3, namespace ##any</remarks>
		[System.Xml.Serialization.XmlAnyElementAttribute()]
		public System.Xml.XmlElement[] any {
			get {
				return this._any;
			}
			set {
				this._any = value;
			}
		}
	}

	[System.SerializableAttribute()]
	[System.Xml.Serialization.XmlTypeAttribute(TypeName = "H264DecOptions", Namespace = "http://www.onvif.org/ver10/schema")]
	public partial class H264DecOptions {

		private System.Xml.XmlAttribute[] _anyAttr;

		private VideoResolution[] _resolutionsAvailable;

		private H264Profile[] _supportedH264Profiles;

		private IntRange _supportedInputBitrate;

		private IntRange _supportedFrameRate;

		private System.Xml.XmlElement[] _any;

		[System.Xml.Serialization.XmlAnyAttributeAttribute()]
		public System.Xml.XmlAttribute[] anyAttr {
			get {
				return this._anyAttr;
			}
			set {
				this._anyAttr = value;
			}
		}

		/// <summary>
		/// List of supported H.264 Video Resolutions
		/// </summary>
		/// <remarks>reqired, order 0</remarks>
		[System.Xml.Serialization.XmlElementAttribute("ResolutionsAvailable", Namespace = "http://www.onvif.org/ver10/schema")]
		public VideoResolution[] resolutionsAvailable {
			get {
				return this._resolutionsAvailable;
			}
			set {
				this._resolutionsAvailable = value;
			}
		}

		/// <summary>
		/// List of supported H264 Profiles (either baseline, main, extended or high)
		/// </summary>
		/// <remarks>reqired, order 1</remarks>
		[System.Xml.Serialization.XmlElementAttribute("SupportedH264Profiles", Namespace = "http://www.onvif.org/ver10/schema")]
		public H264Profile[] supportedH264Profiles {
			get {
				return this._supportedH264Profiles;
			}
			set {
				this._supportedH264Profiles = value;
			}
		}

		/// <summary>
		/// Supported H.264 bitrate range in kbps
		/// </summary>
		/// <remarks>reqired, order 2</remarks>
		[System.Xml.Serialization.XmlElementAttribute("SupportedInputBitrate", Namespace = "http://www.onvif.org/ver10/schema")]
		public IntRange supportedInputBitrate {
			get {
				return this._supportedInputBitrate;
			}
			set {
				this._supportedInputBitrate = value;
			}
		}

		/// <summary>
		/// Supported H.264 framerate range in fps
		/// </summary>
		/// <remarks>reqired, order 3</remarks>
		[System.Xml.Serialization.XmlElementAttribute("SupportedFrameRate", Namespace = "http://www.onvif.org/ver10/schema")]
		public IntRange supportedFrameRate {
			get {
				return this._supportedFrameRate;
			}
			set {
				this._supportedFrameRate = value;
			}
		}

		/// <remarks>optional, order 4, namespace ##any</remarks>
		[System.Xml.Serialization.XmlAnyElementAttribute()]
		public System.Xml.XmlElement[] any {
			get {
				return this._any;
			}
			set {
				this._any = value;
			}
		}
	}

	[System.SerializableAttribute()]
	[System.Xml.Serialization.XmlTypeAttribute(TypeName = "Mpeg4DecOptions", Namespace = "http://www.onvif.org/ver10/schema")]
	public partial class Mpeg4DecOptions {

		private System.Xml.XmlAttribute[] _anyAttr;

		private VideoResolution[] _resolutionsAvailable;

		private Mpeg4Profile[] _supportedMpeg4Profiles;

		private IntRange _supportedInputBitrate;

		private IntRange _supportedFrameRate;

		private System.Xml.XmlElement[] _any;

		[System.Xml.Serialization.XmlAnyAttributeAttribute()]
		public System.Xml.XmlAttribute[] anyAttr {
			get {
				return this._anyAttr;
			}
			set {
				this._anyAttr = value;
			}
		}

		/// <summary>
		/// List of supported Mpeg4 Video Resolutions
		/// </summary>
		/// <remarks>reqired, order 0</remarks>
		[System.Xml.Serialization.XmlElementAttribute("ResolutionsAvailable", Namespace = "http://www.onvif.org/ver10/schema")]
		public VideoResolution[] resolutionsAvailable {
			get {
				return this._resolutionsAvailable;
			}
			set {
				this._resolutionsAvailable = value;
			}
		}

		/// <summary>
		/// List of supported Mpeg4 Profiles (either SP or ASP)
		/// </summary>
		/// <remarks>reqired, order 1</remarks>
		[System.Xml.Serialization.XmlElementAttribute("SupportedMpeg4Profiles", Namespace = "http://www.onvif.org/ver10/schema")]
		public Mpeg4Profile[] supportedMpeg4Profiles {
			get {
				return this._supportedMpeg4Profiles;
			}
			set {
				this._supportedMpeg4Profiles = value;
			}
		}

		/// <summary>
		/// Supported Mpeg4 bitrate range in kbps
		/// </summary>
		/// <remarks>reqired, order 2</remarks>
		[System.Xml.Serialization.XmlElementAttribute("SupportedInputBitrate", Namespace = "http://www.onvif.org/ver10/schema")]
		public IntRange supportedInputBitrate {
			get {
				return this._supportedInputBitrate;
			}
			set {
				this._supportedInputBitrate = value;
			}
		}

		/// <summary>
		/// Supported Mpeg4 framerate range in fps
		/// </summary>
		/// <remarks>reqired, order 3</remarks>
		[System.Xml.Serialization.XmlElementAttribute("SupportedFrameRate", Namespace = "http://www.onvif.org/ver10/schema")]
		public IntRange supportedFrameRate {
			get {
				return this._supportedFrameRate;
			}
			set {
				this._supportedFrameRate = value;
			}
		}

		/// <remarks>optional, order 4, namespace ##any</remarks>
		[System.Xml.Serialization.XmlAnyElementAttribute()]
		public System.Xml.XmlElement[] any {
			get {
				return this._any;
			}
			set {
				this._any = value;
			}
		}
	}

	[System.SerializableAttribute()]
	[System.Xml.Serialization.XmlTypeAttribute(TypeName = "VideoDecoderConfigurationOptionsExtension", Namespace = "http://www.onvif.org/ver10/schema")]
	public partial class VideoDecoderConfigurationOptionsExtension {

		private System.Xml.XmlElement[] _any;

		/// <remarks>optional, order 0, namespace ##any</remarks>
		[System.Xml.Serialization.XmlAnyElementAttribute()]
		public System.Xml.XmlElement[] any {
			get {
				return this._any;
			}
			set {
				this._any = value;
			}
		}
	}

	/// <summary>
	/// Representation of a physical audio outputs.
	/// </summary>
	[System.SerializableAttribute()]
	[System.Xml.Serialization.XmlTypeAttribute(TypeName = "AudioOutput", Namespace = "http://www.onvif.org/ver10/schema")]
	public partial class AudioOutput {

		private string _token;

		private System.Xml.XmlAttribute[] _anyAttr;

		private System.Xml.XmlElement[] _any;

		/// <summary>
		/// Unique identifier referencing the physical entity.
		/// </summary>
		[System.Xml.Serialization.XmlAttributeAttribute("token")]
		public string token {
			get {
				return this._token;
			}
			set {
				this._token = value;
			}
		}

		[System.Xml.Serialization.XmlAnyAttributeAttribute()]
		public System.Xml.XmlAttribute[] anyAttr {
			get {
				return this._anyAttr;
			}
			set {
				this._anyAttr = value;
			}
		}

		/// <remarks>optional, order 0, namespace ##any</remarks>
		[System.Xml.Serialization.XmlAnyElementAttribute()]
		public System.Xml.XmlElement[] any {
			get {
				return this._any;
			}
			set {
				this._any = value;
			}
		}
	}

	[System.SerializableAttribute()]
	[System.Xml.Serialization.XmlTypeAttribute(TypeName = "AudioOutputConfigurationOptions", Namespace = "http://www.onvif.org/ver10/schema")]
	public partial class AudioOutputConfigurationOptions {

		private System.Xml.XmlAttribute[] _anyAttr;

		private string[] _outputTokensAvailable;

		private string[] _sendPrimacyOptions;

		private IntRange _outputLevelRange;

		private System.Xml.XmlElement[] _any;

		[System.Xml.Serialization.XmlAnyAttributeAttribute()]
		public System.Xml.XmlAttribute[] anyAttr {
			get {
				return this._anyAttr;
			}
			set {
				this._anyAttr = value;
			}
		}

		/// <summary>
		/// Tokens of the physical Audio outputs (typically one).
		/// </summary>
		/// <remarks>reqired, order 0</remarks>
		[System.Xml.Serialization.XmlElementAttribute("OutputTokensAvailable", Namespace = "http://www.onvif.org/ver10/schema", DataType = "string")]
		public string[] outputTokensAvailable {
			get {
				return this._outputTokensAvailable;
			}
			set {
				this._outputTokensAvailable = value;
			}
		}

		/// <summary>
		/// An
		/// channel MAY support different types of audio transmission. While for full duplex
		/// operation no special handling is required, in half duplex operation the transmission direction
		/// needs to be switched.
		/// The optional SendPrimacy parameter inside the AudioOutputConfiguration indicates which
		/// direction is currently active. An NVC can switch between different modes by setting the
		/// AudioOutputConfiguration.
		/// The following modes for the Send-Primacy are defined:
		/// Acoustic echo cancellation is out of ONVIF scope.
		/// </summary>
		/// <remarks>optional, order 1</remarks>
		[System.Xml.Serialization.XmlElementAttribute("SendPrimacyOptions", Namespace = "http://www.onvif.org/ver10/schema", DataType = "anyURI")]
		public string[] sendPrimacyOptions {
			get {
				return this._sendPrimacyOptions;
			}
			set {
				this._sendPrimacyOptions = value;
			}
		}

		/// <summary>
		/// Minimum and maximum level range supported for this Output.
		/// </summary>
		/// <remarks>reqired, order 2</remarks>
		[System.Xml.Serialization.XmlElementAttribute("OutputLevelRange", Namespace = "http://www.onvif.org/ver10/schema")]
		public IntRange outputLevelRange {
			get {
				return this._outputLevelRange;
			}
			set {
				this._outputLevelRange = value;
			}
		}

		/// <remarks>optional, order 3, namespace ##any</remarks>
		[System.Xml.Serialization.XmlAnyElementAttribute()]
		public System.Xml.XmlElement[] any {
			get {
				return this._any;
			}
			set {
				this._any = value;
			}
		}
	}

	[System.SerializableAttribute()]
	[System.Xml.Serialization.XmlTypeAttribute(TypeName = "AudioDecoderConfigurationOptions", Namespace = "http://www.onvif.org/ver10/schema")]
	public partial class AudioDecoderConfigurationOptions {

		private System.Xml.XmlAttribute[] _anyAttr;

		private AACDecOptions _aacDecOptions;

		private G711DecOptions _g711DecOptions;

		private G726DecOptions _g726DecOptions;

		private AudioDecoderConfigurationOptionsExtension _extension;

		[System.Xml.Serialization.XmlAnyAttributeAttribute()]
		public System.Xml.XmlAttribute[] anyAttr {
			get {
				return this._anyAttr;
			}
			set {
				this._anyAttr = value;
			}
		}

		/// <summary>
		/// If the device is able to decode AAC encoded audio this section describes the supported configurations
		/// </summary>
		/// <remarks>optional, order 0</remarks>
		[System.Xml.Serialization.XmlElementAttribute("AACDecOptions", Namespace = "http://www.onvif.org/ver10/schema")]
		public AACDecOptions aacDecOptions {
			get {
				return this._aacDecOptions;
			}
			set {
				this._aacDecOptions = value;
			}
		}

		/// <summary>
		/// If the device is able to decode G711 encoded audio this section describes the supported configurations
		/// </summary>
		/// <remarks>optional, order 1</remarks>
		[System.Xml.Serialization.XmlElementAttribute("G711DecOptions", Namespace = "http://www.onvif.org/ver10/schema")]
		public G711DecOptions g711DecOptions {
			get {
				return this._g711DecOptions;
			}
			set {
				this._g711DecOptions = value;
			}
		}

		/// <summary>
		/// If the device is able to decode G726 encoded audio this section describes the supported configurations
		/// </summary>
		/// <remarks>optional, order 2</remarks>
		[System.Xml.Serialization.XmlElementAttribute("G726DecOptions", Namespace = "http://www.onvif.org/ver10/schema")]
		public G726DecOptions g726DecOptions {
			get {
				return this._g726DecOptions;
			}
			set {
				this._g726DecOptions = value;
			}
		}

		/// <remarks>optional, order 3</remarks>
		[System.Xml.Serialization.XmlElementAttribute("Extension", Namespace = "http://www.onvif.org/ver10/schema")]
		public AudioDecoderConfigurationOptionsExtension extension {
			get {
				return this._extension;
			}
			set {
				this._extension = value;
			}
		}
	}

	[System.SerializableAttribute()]
	[System.Xml.Serialization.XmlTypeAttribute(TypeName = "AACDecOptions", Namespace = "http://www.onvif.org/ver10/schema")]
	public partial class AACDecOptions {

		private System.Xml.XmlAttribute[] _anyAttr;

		private IntList _bitrate;

		private IntList _sampleRateRange;

		private System.Xml.XmlElement[] _any;

		[System.Xml.Serialization.XmlAnyAttributeAttribute()]
		public System.Xml.XmlAttribute[] anyAttr {
			get {
				return this._anyAttr;
			}
			set {
				this._anyAttr = value;
			}
		}

		/// <summary>
		/// List of supported bitrates in kbps
		/// </summary>
		/// <remarks>reqired, order 0</remarks>
		[System.Xml.Serialization.XmlElementAttribute("Bitrate", Namespace = "http://www.onvif.org/ver10/schema")]
		public IntList bitrate {
			get {
				return this._bitrate;
			}
			set {
				this._bitrate = value;
			}
		}

		/// <summary>
		/// List of supported sample rates in kHz
		/// </summary>
		/// <remarks>reqired, order 1</remarks>
		[System.Xml.Serialization.XmlElementAttribute("SampleRateRange", Namespace = "http://www.onvif.org/ver10/schema")]
		public IntList sampleRateRange {
			get {
				return this._sampleRateRange;
			}
			set {
				this._sampleRateRange = value;
			}
		}

		/// <remarks>optional, order 2, namespace ##any</remarks>
		[System.Xml.Serialization.XmlAnyElementAttribute()]
		public System.Xml.XmlElement[] any {
			get {
				return this._any;
			}
			set {
				this._any = value;
			}
		}
	}

	[System.SerializableAttribute()]
	[System.Xml.Serialization.XmlTypeAttribute(TypeName = "G711DecOptions", Namespace = "http://www.onvif.org/ver10/schema")]
	public partial class G711DecOptions {

		private System.Xml.XmlAttribute[] _anyAttr;

		private IntList _bitrate;

		private IntList _sampleRateRange;

		private System.Xml.XmlElement[] _any;

		[System.Xml.Serialization.XmlAnyAttributeAttribute()]
		public System.Xml.XmlAttribute[] anyAttr {
			get {
				return this._anyAttr;
			}
			set {
				this._anyAttr = value;
			}
		}

		/// <summary>
		/// List of supported bitrates in kbps
		/// </summary>
		/// <remarks>reqired, order 0</remarks>
		[System.Xml.Serialization.XmlElementAttribute("Bitrate", Namespace = "http://www.onvif.org/ver10/schema")]
		public IntList bitrate {
			get {
				return this._bitrate;
			}
			set {
				this._bitrate = value;
			}
		}

		/// <summary>
		/// List of supported sample rates in kHz
		/// </summary>
		/// <remarks>reqired, order 1</remarks>
		[System.Xml.Serialization.XmlElementAttribute("SampleRateRange", Namespace = "http://www.onvif.org/ver10/schema")]
		public IntList sampleRateRange {
			get {
				return this._sampleRateRange;
			}
			set {
				this._sampleRateRange = value;
			}
		}

		/// <remarks>optional, order 2, namespace ##any</remarks>
		[System.Xml.Serialization.XmlAnyElementAttribute()]
		public System.Xml.XmlElement[] any {
			get {
				return this._any;
			}
			set {
				this._any = value;
			}
		}
	}

	[System.SerializableAttribute()]
	[System.Xml.Serialization.XmlTypeAttribute(TypeName = "G726DecOptions", Namespace = "http://www.onvif.org/ver10/schema")]
	public partial class G726DecOptions {

		private System.Xml.XmlAttribute[] _anyAttr;

		private IntList _bitrate;

		private IntList _sampleRateRange;

		private System.Xml.XmlElement[] _any;

		[System.Xml.Serialization.XmlAnyAttributeAttribute()]
		public System.Xml.XmlAttribute[] anyAttr {
			get {
				return this._anyAttr;
			}
			set {
				this._anyAttr = value;
			}
		}

		/// <summary>
		/// List of supported bitrates in kbps
		/// </summary>
		/// <remarks>reqired, order 0</remarks>
		[System.Xml.Serialization.XmlElementAttribute("Bitrate", Namespace = "http://www.onvif.org/ver10/schema")]
		public IntList bitrate {
			get {
				return this._bitrate;
			}
			set {
				this._bitrate = value;
			}
		}

		/// <summary>
		/// List of supported sample rates in kHz
		/// </summary>
		/// <remarks>reqired, order 1</remarks>
		[System.Xml.Serialization.XmlElementAttribute("SampleRateRange", Namespace = "http://www.onvif.org/ver10/schema")]
		public IntList sampleRateRange {
			get {
				return this._sampleRateRange;
			}
			set {
				this._sampleRateRange = value;
			}
		}

		/// <remarks>optional, order 2, namespace ##any</remarks>
		[System.Xml.Serialization.XmlAnyElementAttribute()]
		public System.Xml.XmlElement[] any {
			get {
				return this._any;
			}
			set {
				this._any = value;
			}
		}
	}

	[System.SerializableAttribute()]
	[System.Xml.Serialization.XmlTypeAttribute(TypeName = "AudioDecoderConfigurationOptionsExtension", Namespace = "http://www.onvif.org/ver10/schema")]
	public partial class AudioDecoderConfigurationOptionsExtension {

		private System.Xml.XmlElement[] _any;

		/// <remarks>optional, order 0, namespace ##any</remarks>
		[System.Xml.Serialization.XmlAnyElementAttribute()]
		public System.Xml.XmlElement[] any {
			get {
				return this._any;
			}
			set {
				this._any = value;
			}
		}
	}

	[System.SerializableAttribute()]
	[System.Xml.Serialization.XmlTypeAttribute(TypeName = "StreamSetup", Namespace = "http://www.onvif.org/ver10/schema")]
	public partial class StreamSetup {

		private System.Xml.XmlAttribute[] _anyAttr;

		private StreamType _stream;

		private Transport _transport;

		private System.Xml.XmlElement[] _any;

		[System.Xml.Serialization.XmlAnyAttributeAttribute()]
		public System.Xml.XmlAttribute[] anyAttr {
			get {
				return this._anyAttr;
			}
			set {
				this._anyAttr = value;
			}
		}

		/// <summary>
		/// Defines if a multicast or unicast stream is requested
		/// </summary>
		/// <remarks>reqired, order 0</remarks>
		[System.Xml.Serialization.XmlElementAttribute("Stream", Namespace = "http://www.onvif.org/ver10/schema")]
		public StreamType stream {
			get {
				return this._stream;
			}
			set {
				this._stream = value;
			}
		}

		/// <remarks>reqired, order 1</remarks>
		[System.Xml.Serialization.XmlElementAttribute("Transport", Namespace = "http://www.onvif.org/ver10/schema")]
		public Transport transport {
			get {
				return this._transport;
			}
			set {
				this._transport = value;
			}
		}

		/// <remarks>optional, order 2, namespace ##any</remarks>
		[System.Xml.Serialization.XmlAnyElementAttribute()]
		public System.Xml.XmlElement[] any {
			get {
				return this._any;
			}
			set {
				this._any = value;
			}
		}
	}

	[System.SerializableAttribute()]
	[System.Xml.Serialization.XmlTypeAttribute(TypeName = "StreamType", Namespace = "http://www.onvif.org/ver10/schema")]
	public enum StreamType {

		[System.Xml.Serialization.XmlEnumAttribute(Name = "RTP-Unicast")]
		rtpUnicast,

		[System.Xml.Serialization.XmlEnumAttribute(Name = "RTP-Multicast")]
		rtpMulticast,
	}

	[System.SerializableAttribute()]
	[System.Xml.Serialization.XmlTypeAttribute(TypeName = "Transport", Namespace = "http://www.onvif.org/ver10/schema")]
	public partial class Transport {

		private TransportProtocol _protocol;

		private Transport _tunnel;

		/// <summary>
		/// Defines the network protocol for streaming, either UDP=RTP/UDP, RTSP=RTP/RTSP/TCP or HTTP=RTP/RTSP/HTTP/TCP
		/// </summary>
		/// <remarks>reqired, order 0</remarks>
		[System.Xml.Serialization.XmlElementAttribute("Protocol", Namespace = "http://www.onvif.org/ver10/schema")]
		public TransportProtocol protocol {
			get {
				return this._protocol;
			}
			set {
				this._protocol = value;
			}
		}

		/// <summary>
		/// Optional element to describe further tunnel options. This element is normally not needed
		/// </summary>
		/// <remarks>optional, order 1</remarks>
		[System.Xml.Serialization.XmlElementAttribute("Tunnel", Namespace = "http://www.onvif.org/ver10/schema")]
		public Transport tunnel {
			get {
				return this._tunnel;
			}
			set {
				this._tunnel = value;
			}
		}
	}

	[System.SerializableAttribute()]
	[System.Xml.Serialization.XmlTypeAttribute(TypeName = "TransportProtocol", Namespace = "http://www.onvif.org/ver10/schema")]
	public enum TransportProtocol {

		[System.Xml.Serialization.XmlEnumAttribute(Name = "UDP")]
		udp,

		[System.Xml.Serialization.XmlEnumAttribute(Name = "TCP")]
		tcp,

		[System.Xml.Serialization.XmlEnumAttribute(Name = "RTSP")]
		rtsp,

		[System.Xml.Serialization.XmlEnumAttribute(Name = "HTTP")]
		http,
	}

	[System.SerializableAttribute()]
	[System.Xml.Serialization.XmlTypeAttribute(TypeName = "MediaUri", Namespace = "http://www.onvif.org/ver10/schema")]
	public partial class MediaUri {

		private System.Xml.XmlAttribute[] _anyAttr;

		private string _uri;

		private bool _invalidAfterConnect;

		private bool _invalidAfterReboot;

		private XsDuration _timeout;

		private System.Xml.XmlElement[] _any;

		[System.Xml.Serialization.XmlAnyAttributeAttribute()]
		public System.Xml.XmlAttribute[] anyAttr {
			get {
				return this._anyAttr;
			}
			set {
				this._anyAttr = value;
			}
		}

		/// <summary>
		/// Stable Uri to be used for requesting the media stream
		/// </summary>
		/// <remarks>reqired, order 0</remarks>
		[System.Xml.Serialization.XmlElementAttribute("Uri", Namespace = "http://www.onvif.org/ver10/schema", DataType = "anyURI")]
		public string uri {
			get {
				return this._uri;
			}
			set {
				this._uri = value;
			}
		}

		/// <summary>
		/// Indicates if the Uri is only valid until the connection is established. The value shall be set to "false".
		/// </summary>
		/// <remarks>reqired, order 1</remarks>
		[System.Xml.Serialization.XmlElementAttribute("InvalidAfterConnect", Namespace = "http://www.onvif.org/ver10/schema", DataType = "boolean")]
		public bool invalidAfterConnect {
			get {
				return this._invalidAfterConnect;
			}
			set {
				this._invalidAfterConnect = value;
			}
		}

		/// <summary>
		/// Indicates if the Uri is invalid after a reboot of the device. The value shall be set to "false".
		/// </summary>
		/// <remarks>reqired, order 2</remarks>
		[System.Xml.Serialization.XmlElementAttribute("InvalidAfterReboot", Namespace = "http://www.onvif.org/ver10/schema", DataType = "boolean")]
		public bool invalidAfterReboot {
			get {
				return this._invalidAfterReboot;
			}
			set {
				this._invalidAfterReboot = value;
			}
		}

		/// <summary>
		/// Duration how long the Uri is valid. This parameter shall be set to PT0S to indicate that this stream URI is indefinitely valid even if the profile changes
		/// </summary>
		/// <remarks>reqired, order 3</remarks>
		[System.Xml.Serialization.XmlElementAttribute("Timeout", Namespace = "http://www.onvif.org/ver10/schema")]
		public XsDuration timeout {
			get {
				return this._timeout;
			}
			set {
				this._timeout = value;
			}
		}

		/// <remarks>optional, order 4, namespace ##any</remarks>
		[System.Xml.Serialization.XmlAnyElementAttribute()]
		public System.Xml.XmlElement[] any {
			get {
				return this._any;
			}
			set {
				this._any = value;
			}
		}
	}

	[System.SerializableAttribute()]
	[System.Xml.Serialization.XmlTypeAttribute(TypeName = "ScopeDefinition", Namespace = "http://www.onvif.org/ver10/schema")]
	public enum ScopeDefinition {

		[System.Xml.Serialization.XmlEnumAttribute(Name = "Fixed")]
		@fixed,

		[System.Xml.Serialization.XmlEnumAttribute(Name = "Configurable")]
		configurable,
	}

	[System.SerializableAttribute()]
	[System.Xml.Serialization.XmlTypeAttribute(TypeName = "Scope", Namespace = "http://www.onvif.org/ver10/schema")]
	public partial class Scope {

		private ScopeDefinition _scopeDef;

		private string _scopeItem;

		/// <summary>
		/// Indicates if the scope is fixed or configurable.
		/// </summary>
		/// <remarks>reqired, order 0</remarks>
		[System.Xml.Serialization.XmlElementAttribute("ScopeDef", Namespace = "http://www.onvif.org/ver10/schema")]
		public ScopeDefinition scopeDef {
			get {
				return this._scopeDef;
			}
			set {
				this._scopeDef = value;
			}
		}

		/// <summary>
		/// Scope item URI.
		/// </summary>
		/// <remarks>reqired, order 1</remarks>
		[System.Xml.Serialization.XmlElementAttribute("ScopeItem", Namespace = "http://www.onvif.org/ver10/schema", DataType = "anyURI")]
		public string scopeItem {
			get {
				return this._scopeItem;
			}
			set {
				this._scopeItem = value;
			}
		}
	}

	[System.SerializableAttribute()]
	[System.Xml.Serialization.XmlTypeAttribute(TypeName = "DiscoveryMode", Namespace = "http://www.onvif.org/ver10/schema")]
	public enum DiscoveryMode {

		[System.Xml.Serialization.XmlEnumAttribute(Name = "Discoverable")]
		discoverable,

		[System.Xml.Serialization.XmlEnumAttribute(Name = "NonDiscoverable")]
		nonDiscoverable,
	}

	[System.SerializableAttribute()]
	[System.Xml.Serialization.XmlTypeAttribute(TypeName = "NetworkInterface", Namespace = "http://www.onvif.org/ver10/schema")]
	public partial class NetworkInterface {

		private string _token;

		private System.Xml.XmlAttribute[] _anyAttr;

		private bool _enabled;

		private NetworkInterfaceInfo _info;

		private NetworkInterfaceLink _link;

		private IPv4NetworkInterface _iPv4;

		private IPv6NetworkInterface _iPv6;

		private NetworkInterfaceExtension _extension;

		/// <summary>
		/// Unique identifier referencing the physical entity.
		/// </summary>
		[System.Xml.Serialization.XmlAttributeAttribute("token")]
		public string token {
			get {
				return this._token;
			}
			set {
				this._token = value;
			}
		}

		[System.Xml.Serialization.XmlAnyAttributeAttribute()]
		public System.Xml.XmlAttribute[] anyAttr {
			get {
				return this._anyAttr;
			}
			set {
				this._anyAttr = value;
			}
		}

		/// <summary>
		/// Indicates whether or not an interface is enabled.
		/// </summary>
		/// <remarks>reqired, order 0</remarks>
		[System.Xml.Serialization.XmlElementAttribute("Enabled", Namespace = "http://www.onvif.org/ver10/schema", DataType = "boolean")]
		public bool enabled {
			get {
				return this._enabled;
			}
			set {
				this._enabled = value;
			}
		}

		/// <summary>
		/// Network interface information
		/// </summary>
		/// <remarks>optional, order 1</remarks>
		[System.Xml.Serialization.XmlElementAttribute("Info", Namespace = "http://www.onvif.org/ver10/schema")]
		public NetworkInterfaceInfo info {
			get {
				return this._info;
			}
			set {
				this._info = value;
			}
		}

		/// <summary>
		/// Link configuration.
		/// </summary>
		/// <remarks>optional, order 2</remarks>
		[System.Xml.Serialization.XmlElementAttribute("Link", Namespace = "http://www.onvif.org/ver10/schema")]
		public NetworkInterfaceLink link {
			get {
				return this._link;
			}
			set {
				this._link = value;
			}
		}

		/// <summary>
		/// IPv4 network interface configuration.
		/// </summary>
		/// <remarks>optional, order 3</remarks>
		[System.Xml.Serialization.XmlElementAttribute("IPv4", Namespace = "http://www.onvif.org/ver10/schema")]
		public IPv4NetworkInterface iPv4 {
			get {
				return this._iPv4;
			}
			set {
				this._iPv4 = value;
			}
		}

		/// <summary>
		/// IPv6 network interface configuration.
		/// </summary>
		/// <remarks>optional, order 4</remarks>
		[System.Xml.Serialization.XmlElementAttribute("IPv6", Namespace = "http://www.onvif.org/ver10/schema")]
		public IPv6NetworkInterface iPv6 {
			get {
				return this._iPv6;
			}
			set {
				this._iPv6 = value;
			}
		}

		/// <remarks>optional, order 5</remarks>
		[System.Xml.Serialization.XmlElementAttribute("Extension", Namespace = "http://www.onvif.org/ver10/schema")]
		public NetworkInterfaceExtension extension {
			get {
				return this._extension;
			}
			set {
				this._extension = value;
			}
		}
	}

	[System.SerializableAttribute()]
	[System.Xml.Serialization.XmlTypeAttribute(TypeName = "NetworkInterfaceInfo", Namespace = "http://www.onvif.org/ver10/schema")]
	public partial class NetworkInterfaceInfo {

		private string _name;

		private string _hwAddress;

		private int _mtu;

		private bool _mtuSpecified;

		/// <summary>
		/// Network interface name, for example eth0.
		/// </summary>
		/// <remarks>optional, order 0</remarks>
		[System.Xml.Serialization.XmlElementAttribute("Name", Namespace = "http://www.onvif.org/ver10/schema", DataType = "string")]
		public string name {
			get {
				return this._name;
			}
			set {
				this._name = value;
			}
		}

		/// <summary>
		/// Network interface MAC address.
		/// </summary>
		/// <remarks>reqired, order 1</remarks>
		[System.Xml.Serialization.XmlElementAttribute("HwAddress", Namespace = "http://www.onvif.org/ver10/schema", DataType = "token")]
		public string hwAddress {
			get {
				return this._hwAddress;
			}
			set {
				this._hwAddress = value;
			}
		}

		/// <summary>
		/// Maximum transmission unit.
		/// </summary>
		/// <remarks>optional, order 2</remarks>
		[System.Xml.Serialization.XmlElementAttribute("MTU", Namespace = "http://www.onvif.org/ver10/schema", DataType = "int")]
		public int mtu {
			get {
				return this._mtu;
			}
			set {
				this._mtu = value;
			}
		}

		[System.Xml.Serialization.XmlIgnoreAttribute()]
		public bool mtuSpecified {
			get {
				return this._mtuSpecified;
			}
			set {
				this._mtuSpecified = value;
			}
		}
	}

	[System.SerializableAttribute()]
	[System.Xml.Serialization.XmlTypeAttribute(TypeName = "NetworkInterfaceLink", Namespace = "http://www.onvif.org/ver10/schema")]
	public partial class NetworkInterfaceLink {

		private NetworkInterfaceConnectionSetting _adminSettings;

		private NetworkInterfaceConnectionSetting _operSettings;

		private int _interfaceType;

		/// <summary>
		/// Configured link settings.
		/// </summary>
		/// <remarks>reqired, order 0</remarks>
		[System.Xml.Serialization.XmlElementAttribute("AdminSettings", Namespace = "http://www.onvif.org/ver10/schema")]
		public NetworkInterfaceConnectionSetting adminSettings {
			get {
				return this._adminSettings;
			}
			set {
				this._adminSettings = value;
			}
		}

		/// <summary>
		/// Current active link settings.
		/// </summary>
		/// <remarks>reqired, order 1</remarks>
		[System.Xml.Serialization.XmlElementAttribute("OperSettings", Namespace = "http://www.onvif.org/ver10/schema")]
		public NetworkInterfaceConnectionSetting operSettings {
			get {
				return this._operSettings;
			}
			set {
				this._operSettings = value;
			}
		}

		/// <summary>
		/// Integer indicating interface type, for example: 6 is ethernet.
		/// </summary>
		/// <remarks>reqired, order 2</remarks>
		[System.Xml.Serialization.XmlElementAttribute("InterfaceType", Namespace = "http://www.onvif.org/ver10/schema", DataType = "int")]
		public int interfaceType {
			get {
				return this._interfaceType;
			}
			set {
				this._interfaceType = value;
			}
		}
	}

	[System.SerializableAttribute()]
	[System.Xml.Serialization.XmlTypeAttribute(TypeName = "NetworkInterfaceConnectionSetting", Namespace = "http://www.onvif.org/ver10/schema")]
	public partial class NetworkInterfaceConnectionSetting {

		private bool _autoNegotiation;

		private int _speed;

		private Duplex _duplex;

		/// <summary>
		/// Auto negotiation on/off.
		/// </summary>
		/// <remarks>reqired, order 0</remarks>
		[System.Xml.Serialization.XmlElementAttribute("AutoNegotiation", Namespace = "http://www.onvif.org/ver10/schema", DataType = "boolean")]
		public bool autoNegotiation {
			get {
				return this._autoNegotiation;
			}
			set {
				this._autoNegotiation = value;
			}
		}

		/// <summary>
		/// Speed.
		/// </summary>
		/// <remarks>reqired, order 1</remarks>
		[System.Xml.Serialization.XmlElementAttribute("Speed", Namespace = "http://www.onvif.org/ver10/schema", DataType = "int")]
		public int speed {
			get {
				return this._speed;
			}
			set {
				this._speed = value;
			}
		}

		/// <summary>
		/// Duplex type, Half or Full.
		/// </summary>
		/// <remarks>reqired, order 2</remarks>
		[System.Xml.Serialization.XmlElementAttribute("Duplex", Namespace = "http://www.onvif.org/ver10/schema")]
		public Duplex duplex {
			get {
				return this._duplex;
			}
			set {
				this._duplex = value;
			}
		}
	}

	[System.SerializableAttribute()]
	[System.Xml.Serialization.XmlTypeAttribute(TypeName = "Duplex", Namespace = "http://www.onvif.org/ver10/schema")]
	public enum Duplex {

		[System.Xml.Serialization.XmlEnumAttribute(Name = "Full")]
		full,

		[System.Xml.Serialization.XmlEnumAttribute(Name = "Half")]
		half,
	}

	[System.SerializableAttribute()]
	[System.Xml.Serialization.XmlTypeAttribute(TypeName = "IPv4NetworkInterface", Namespace = "http://www.onvif.org/ver10/schema")]
	public partial class IPv4NetworkInterface {

		private bool _enabled;

		private IPv4Configuration _config;

		/// <summary>
		/// Indicates whether or not IPv4 is enabled.
		/// </summary>
		/// <remarks>reqired, order 0</remarks>
		[System.Xml.Serialization.XmlElementAttribute("Enabled", Namespace = "http://www.onvif.org/ver10/schema", DataType = "boolean")]
		public bool enabled {
			get {
				return this._enabled;
			}
			set {
				this._enabled = value;
			}
		}

		/// <summary>
		/// IPv4 configuration.
		/// </summary>
		/// <remarks>reqired, order 1</remarks>
		[System.Xml.Serialization.XmlElementAttribute("Config", Namespace = "http://www.onvif.org/ver10/schema")]
		public IPv4Configuration config {
			get {
				return this._config;
			}
			set {
				this._config = value;
			}
		}
	}

	[System.SerializableAttribute()]
	[System.Xml.Serialization.XmlTypeAttribute(TypeName = "IPv4Configuration", Namespace = "http://www.onvif.org/ver10/schema")]
	public partial class IPv4Configuration {

		private System.Xml.XmlAttribute[] _anyAttr;

		private PrefixedIPv4Address[] _manual;

		private PrefixedIPv4Address _linkLocal;

		private PrefixedIPv4Address _fromDHCP;

		private bool _dhcp;

		private System.Xml.XmlElement[] _any;

		[System.Xml.Serialization.XmlAnyAttributeAttribute()]
		public System.Xml.XmlAttribute[] anyAttr {
			get {
				return this._anyAttr;
			}
			set {
				this._anyAttr = value;
			}
		}

		/// <summary>
		/// List of manually added IPv4 addresses.
		/// </summary>
		/// <remarks>optional, order 0</remarks>
		[System.Xml.Serialization.XmlElementAttribute("Manual", Namespace = "http://www.onvif.org/ver10/schema")]
		public PrefixedIPv4Address[] manual {
			get {
				return this._manual;
			}
			set {
				this._manual = value;
			}
		}

		/// <summary>
		/// Link local address.
		/// </summary>
		/// <remarks>optional, order 1</remarks>
		[System.Xml.Serialization.XmlElementAttribute("LinkLocal", Namespace = "http://www.onvif.org/ver10/schema")]
		public PrefixedIPv4Address linkLocal {
			get {
				return this._linkLocal;
			}
			set {
				this._linkLocal = value;
			}
		}

		/// <summary>
		/// IPv4 address configured by using DHCP.
		/// </summary>
		/// <remarks>optional, order 2</remarks>
		[System.Xml.Serialization.XmlElementAttribute("FromDHCP", Namespace = "http://www.onvif.org/ver10/schema")]
		public PrefixedIPv4Address fromDHCP {
			get {
				return this._fromDHCP;
			}
			set {
				this._fromDHCP = value;
			}
		}

		/// <summary>
		/// Indicates whether or not DHCP is used.
		/// </summary>
		/// <remarks>reqired, order 3</remarks>
		[System.Xml.Serialization.XmlElementAttribute("DHCP", Namespace = "http://www.onvif.org/ver10/schema", DataType = "boolean")]
		public bool dhcp {
			get {
				return this._dhcp;
			}
			set {
				this._dhcp = value;
			}
		}

		/// <remarks>optional, order 4, namespace ##any</remarks>
		[System.Xml.Serialization.XmlAnyElementAttribute()]
		public System.Xml.XmlElement[] any {
			get {
				return this._any;
			}
			set {
				this._any = value;
			}
		}
	}

	[System.SerializableAttribute()]
	[System.Xml.Serialization.XmlTypeAttribute(TypeName = "PrefixedIPv4Address", Namespace = "http://www.onvif.org/ver10/schema")]
	public partial class PrefixedIPv4Address {

		private string _address;

		private int _prefixLength;

		/// <summary>
		/// IPv4 address
		/// </summary>
		/// <remarks>reqired, order 0</remarks>
		[System.Xml.Serialization.XmlElementAttribute("Address", Namespace = "http://www.onvif.org/ver10/schema", DataType = "token")]
		public string address {
			get {
				return this._address;
			}
			set {
				this._address = value;
			}
		}

		/// <summary>
		/// Prefix/submask length
		/// </summary>
		/// <remarks>reqired, order 1</remarks>
		[System.Xml.Serialization.XmlElementAttribute("PrefixLength", Namespace = "http://www.onvif.org/ver10/schema", DataType = "int")]
		public int prefixLength {
			get {
				return this._prefixLength;
			}
			set {
				this._prefixLength = value;
			}
		}
	}

	[System.SerializableAttribute()]
	[System.Xml.Serialization.XmlTypeAttribute(TypeName = "IPv6NetworkInterface", Namespace = "http://www.onvif.org/ver10/schema")]
	public partial class IPv6NetworkInterface {

		private bool _enabled;

		private IPv6Configuration _config;

		/// <summary>
		/// Indicates whether or not IPv6 is enabled.
		/// </summary>
		/// <remarks>reqired, order 0</remarks>
		[System.Xml.Serialization.XmlElementAttribute("Enabled", Namespace = "http://www.onvif.org/ver10/schema", DataType = "boolean")]
		public bool enabled {
			get {
				return this._enabled;
			}
			set {
				this._enabled = value;
			}
		}

		/// <summary>
		/// IPv6 configuration.
		/// </summary>
		/// <remarks>optional, order 1</remarks>
		[System.Xml.Serialization.XmlElementAttribute("Config", Namespace = "http://www.onvif.org/ver10/schema")]
		public IPv6Configuration config {
			get {
				return this._config;
			}
			set {
				this._config = value;
			}
		}
	}

	[System.SerializableAttribute()]
	[System.Xml.Serialization.XmlTypeAttribute(TypeName = "IPv6Configuration", Namespace = "http://www.onvif.org/ver10/schema")]
	public partial class IPv6Configuration {

		private System.Xml.XmlAttribute[] _anyAttr;

		private bool _acceptRouterAdvert;

		private bool _acceptRouterAdvertSpecified;

		private IPv6DHCPConfiguration _dhcp;

		private PrefixedIPv6Address[] _manual;

		private PrefixedIPv6Address[] _linkLocal;

		private PrefixedIPv6Address[] _fromDHCP;

		private PrefixedIPv6Address[] _fromRA;

		private IPv6ConfigurationExtension _extension;

		[System.Xml.Serialization.XmlAnyAttributeAttribute()]
		public System.Xml.XmlAttribute[] anyAttr {
			get {
				return this._anyAttr;
			}
			set {
				this._anyAttr = value;
			}
		}

		/// <summary>
		/// Indicates whether router advertisment is used.
		/// </summary>
		/// <remarks>optional, order 0</remarks>
		[System.Xml.Serialization.XmlElementAttribute("AcceptRouterAdvert", Namespace = "http://www.onvif.org/ver10/schema", DataType = "boolean")]
		public bool acceptRouterAdvert {
			get {
				return this._acceptRouterAdvert;
			}
			set {
				this._acceptRouterAdvert = value;
			}
		}

		[System.Xml.Serialization.XmlIgnoreAttribute()]
		public bool acceptRouterAdvertSpecified {
			get {
				return this._acceptRouterAdvertSpecified;
			}
			set {
				this._acceptRouterAdvertSpecified = value;
			}
		}

		/// <summary>
		/// DHCP configuration.
		/// </summary>
		/// <remarks>reqired, order 1</remarks>
		[System.Xml.Serialization.XmlElementAttribute("DHCP", Namespace = "http://www.onvif.org/ver10/schema")]
		public IPv6DHCPConfiguration dhcp {
			get {
				return this._dhcp;
			}
			set {
				this._dhcp = value;
			}
		}

		/// <summary>
		/// List of manually entered IPv6 addresses.
		/// </summary>
		/// <remarks>optional, order 2</remarks>
		[System.Xml.Serialization.XmlElementAttribute("Manual", Namespace = "http://www.onvif.org/ver10/schema")]
		public PrefixedIPv6Address[] manual {
			get {
				return this._manual;
			}
			set {
				this._manual = value;
			}
		}

		/// <summary>
		/// List of link local IPv6 addresses.
		/// </summary>
		/// <remarks>optional, order 3</remarks>
		[System.Xml.Serialization.XmlElementAttribute("LinkLocal", Namespace = "http://www.onvif.org/ver10/schema")]
		public PrefixedIPv6Address[] linkLocal {
			get {
				return this._linkLocal;
			}
			set {
				this._linkLocal = value;
			}
		}

		/// <summary>
		/// List of IPv6 addresses configured by using DHCP.
		/// </summary>
		/// <remarks>optional, order 4</remarks>
		[System.Xml.Serialization.XmlElementAttribute("FromDHCP", Namespace = "http://www.onvif.org/ver10/schema")]
		public PrefixedIPv6Address[] fromDHCP {
			get {
				return this._fromDHCP;
			}
			set {
				this._fromDHCP = value;
			}
		}

		/// <summary>
		/// List of IPv6 addresses configured by using router advertisment.
		/// </summary>
		/// <remarks>optional, order 5</remarks>
		[System.Xml.Serialization.XmlElementAttribute("FromRA", Namespace = "http://www.onvif.org/ver10/schema")]
		public PrefixedIPv6Address[] fromRA {
			get {
				return this._fromRA;
			}
			set {
				this._fromRA = value;
			}
		}

		/// <remarks>optional, order 6</remarks>
		[System.Xml.Serialization.XmlElementAttribute("Extension", Namespace = "http://www.onvif.org/ver10/schema")]
		public IPv6ConfigurationExtension extension {
			get {
				return this._extension;
			}
			set {
				this._extension = value;
			}
		}
	}

	[System.SerializableAttribute()]
	[System.Xml.Serialization.XmlTypeAttribute(TypeName = "IPv6DHCPConfiguration", Namespace = "http://www.onvif.org/ver10/schema")]
	public enum IPv6DHCPConfiguration {

		[System.Xml.Serialization.XmlEnumAttribute(Name = "Auto")]
		auto,

		[System.Xml.Serialization.XmlEnumAttribute(Name = "Stateful")]
		stateful,

		[System.Xml.Serialization.XmlEnumAttribute(Name = "Stateless")]
		stateless,

		[System.Xml.Serialization.XmlEnumAttribute(Name = "Off")]
		off,
	}

	[System.SerializableAttribute()]
	[System.Xml.Serialization.XmlTypeAttribute(TypeName = "PrefixedIPv6Address", Namespace = "http://www.onvif.org/ver10/schema")]
	public partial class PrefixedIPv6Address {

		private string _address;

		private int _prefixLength;

		/// <summary>
		/// IPv6 address
		/// </summary>
		/// <remarks>reqired, order 0</remarks>
		[System.Xml.Serialization.XmlElementAttribute("Address", Namespace = "http://www.onvif.org/ver10/schema", DataType = "token")]
		public string address {
			get {
				return this._address;
			}
			set {
				this._address = value;
			}
		}

		/// <summary>
		/// Prefix/submask length
		/// </summary>
		/// <remarks>reqired, order 1</remarks>
		[System.Xml.Serialization.XmlElementAttribute("PrefixLength", Namespace = "http://www.onvif.org/ver10/schema", DataType = "int")]
		public int prefixLength {
			get {
				return this._prefixLength;
			}
			set {
				this._prefixLength = value;
			}
		}
	}

	[System.SerializableAttribute()]
	[System.Xml.Serialization.XmlTypeAttribute(TypeName = "IPv6ConfigurationExtension", Namespace = "http://www.onvif.org/ver10/schema")]
	public partial class IPv6ConfigurationExtension {

		private System.Xml.XmlElement[] _any;

		/// <remarks>optional, order 0, namespace ##any</remarks>
		[System.Xml.Serialization.XmlAnyElementAttribute()]
		public System.Xml.XmlElement[] any {
			get {
				return this._any;
			}
			set {
				this._any = value;
			}
		}
	}

	[System.SerializableAttribute()]
	[System.Xml.Serialization.XmlTypeAttribute(TypeName = "NetworkInterfaceExtension", Namespace = "http://www.onvif.org/ver10/schema")]
	public partial class NetworkInterfaceExtension {

		private System.Xml.XmlElement[] _any;

		private int _interfaceType;

		private Dot3Configuration[] _dot3;

		private Dot11Configuration[] _dot11;

		private NetworkInterfaceExtension2 _extension;

		/// <remarks>optional, order 0, namespace ##other</remarks>
		[System.Xml.Serialization.XmlAnyElementAttribute()]
		public System.Xml.XmlElement[] any {
			get {
				return this._any;
			}
			set {
				this._any = value;
			}
		}

		/// <remarks>reqired, order 1</remarks>
		[System.Xml.Serialization.XmlElementAttribute("InterfaceType", Namespace = "http://www.onvif.org/ver10/schema", DataType = "int")]
		public int interfaceType {
			get {
				return this._interfaceType;
			}
			set {
				this._interfaceType = value;
			}
		}

		/// <summary>
		/// Extension point prepared for future 802.3 configuration.
		/// </summary>
		/// <remarks>optional, order 2</remarks>
		[System.Xml.Serialization.XmlElementAttribute("Dot3", Namespace = "http://www.onvif.org/ver10/schema")]
		public Dot3Configuration[] dot3 {
			get {
				return this._dot3;
			}
			set {
				this._dot3 = value;
			}
		}

		/// <remarks>optional, order 3</remarks>
		[System.Xml.Serialization.XmlElementAttribute("Dot11", Namespace = "http://www.onvif.org/ver10/schema")]
		public Dot11Configuration[] dot11 {
			get {
				return this._dot11;
			}
			set {
				this._dot11 = value;
			}
		}

		/// <remarks>optional, order 4</remarks>
		[System.Xml.Serialization.XmlElementAttribute("Extension", Namespace = "http://www.onvif.org/ver10/schema")]
		public NetworkInterfaceExtension2 extension {
			get {
				return this._extension;
			}
			set {
				this._extension = value;
			}
		}
	}

	[System.SerializableAttribute()]
	[System.Xml.Serialization.XmlTypeAttribute(TypeName = "Dot3Configuration", Namespace = "http://www.onvif.org/ver10/schema")]
	public partial class Dot3Configuration {

		private System.Xml.XmlAttribute[] _anyAttr;

		private System.Xml.XmlElement[] _any;

		[System.Xml.Serialization.XmlAnyAttributeAttribute()]
		public System.Xml.XmlAttribute[] anyAttr {
			get {
				return this._anyAttr;
			}
			set {
				this._anyAttr = value;
			}
		}

		/// <remarks>optional, order 0, namespace ##any</remarks>
		[System.Xml.Serialization.XmlAnyElementAttribute()]
		public System.Xml.XmlElement[] any {
			get {
				return this._any;
			}
			set {
				this._any = value;
			}
		}
	}

	[System.SerializableAttribute()]
	[System.Xml.Serialization.XmlTypeAttribute(TypeName = "Dot11Configuration", Namespace = "http://www.onvif.org/ver10/schema")]
	public partial class Dot11Configuration {

		private System.Xml.XmlAttribute[] _anyAttr;

		private byte[] _ssid;

		private Dot11StationMode _mode;

		private string _alias;

		private string _priority;

		private Dot11SecurityConfiguration _security;

		private System.Xml.XmlElement[] _any;

		[System.Xml.Serialization.XmlAnyAttributeAttribute()]
		public System.Xml.XmlAttribute[] anyAttr {
			get {
				return this._anyAttr;
			}
			set {
				this._anyAttr = value;
			}
		}

		/// <remarks>reqired, order 0</remarks>
		[System.Xml.Serialization.XmlElementAttribute("SSID", Namespace = "http://www.onvif.org/ver10/schema", DataType = "hexBinary")]
		public byte[] ssid {
			get {
				return this._ssid;
			}
			set {
				this._ssid = value;
			}
		}

		/// <remarks>reqired, order 1</remarks>
		[System.Xml.Serialization.XmlElementAttribute("Mode", Namespace = "http://www.onvif.org/ver10/schema")]
		public Dot11StationMode mode {
			get {
				return this._mode;
			}
			set {
				this._mode = value;
			}
		}

		/// <remarks>reqired, order 2</remarks>
		[System.Xml.Serialization.XmlElementAttribute("Alias", Namespace = "http://www.onvif.org/ver10/schema", DataType = "string")]
		public string alias {
			get {
				return this._alias;
			}
			set {
				this._alias = value;
			}
		}

		/// <remarks>reqired, order 3</remarks>
		[System.Xml.Serialization.XmlElementAttribute("Priority", Namespace = "http://www.onvif.org/ver10/schema", DataType = "integer")]
		public string priority {
			get {
				return this._priority;
			}
			set {
				this._priority = value;
			}
		}

		/// <remarks>reqired, order 4</remarks>
		[System.Xml.Serialization.XmlElementAttribute("Security", Namespace = "http://www.onvif.org/ver10/schema")]
		public Dot11SecurityConfiguration security {
			get {
				return this._security;
			}
			set {
				this._security = value;
			}
		}

		/// <remarks>optional, order 5, namespace ##any</remarks>
		[System.Xml.Serialization.XmlAnyElementAttribute()]
		public System.Xml.XmlElement[] any {
			get {
				return this._any;
			}
			set {
				this._any = value;
			}
		}
	}

	[System.SerializableAttribute()]
	[System.Xml.Serialization.XmlTypeAttribute(TypeName = "Dot11StationMode", Namespace = "http://www.onvif.org/ver10/schema")]
	public enum Dot11StationMode {

		[System.Xml.Serialization.XmlEnumAttribute(Name = "Ad-hoc")]
		adhoc,

		[System.Xml.Serialization.XmlEnumAttribute(Name = "Infrastructure")]
		infrastructure,

		[System.Xml.Serialization.XmlEnumAttribute(Name = "Extended")]
		extended,
	}

	[System.SerializableAttribute()]
	[System.Xml.Serialization.XmlTypeAttribute(TypeName = "Dot11SecurityConfiguration", Namespace = "http://www.onvif.org/ver10/schema")]
	public partial class Dot11SecurityConfiguration {

		private System.Xml.XmlAttribute[] _anyAttr;

		private Dot11SecurityMode _mode;

		private Dot11Cipher _algorithm;

		private bool _algorithmSpecified;

		private Dot11PSKSet _psk;

		private string _dot1X;

		private Dot11SecurityConfigurationExtension _extension;

		[System.Xml.Serialization.XmlAnyAttributeAttribute()]
		public System.Xml.XmlAttribute[] anyAttr {
			get {
				return this._anyAttr;
			}
			set {
				this._anyAttr = value;
			}
		}

		/// <remarks>reqired, order 0</remarks>
		[System.Xml.Serialization.XmlElementAttribute("Mode", Namespace = "http://www.onvif.org/ver10/schema")]
		public Dot11SecurityMode mode {
			get {
				return this._mode;
			}
			set {
				this._mode = value;
			}
		}

		/// <remarks>optional, order 1</remarks>
		[System.Xml.Serialization.XmlElementAttribute("Algorithm", Namespace = "http://www.onvif.org/ver10/schema")]
		public Dot11Cipher algorithm {
			get {
				return this._algorithm;
			}
			set {
				this._algorithm = value;
			}
		}

		[System.Xml.Serialization.XmlIgnoreAttribute()]
		public bool algorithmSpecified {
			get {
				return this._algorithmSpecified;
			}
			set {
				this._algorithmSpecified = value;
			}
		}

		/// <remarks>optional, order 2</remarks>
		[System.Xml.Serialization.XmlElementAttribute("PSK", Namespace = "http://www.onvif.org/ver10/schema")]
		public Dot11PSKSet psk {
			get {
				return this._psk;
			}
			set {
				this._psk = value;
			}
		}

		/// <remarks>optional, order 3</remarks>
		[System.Xml.Serialization.XmlElementAttribute("Dot1X", Namespace = "http://www.onvif.org/ver10/schema", DataType = "string")]
		public string dot1X {
			get {
				return this._dot1X;
			}
			set {
				this._dot1X = value;
			}
		}

		/// <remarks>optional, order 4</remarks>
		[System.Xml.Serialization.XmlElementAttribute("Extension", Namespace = "http://www.onvif.org/ver10/schema")]
		public Dot11SecurityConfigurationExtension extension {
			get {
				return this._extension;
			}
			set {
				this._extension = value;
			}
		}
	}

	[System.SerializableAttribute()]
	[System.Xml.Serialization.XmlTypeAttribute(TypeName = "Dot11SecurityMode", Namespace = "http://www.onvif.org/ver10/schema")]
	public enum Dot11SecurityMode {

		[System.Xml.Serialization.XmlEnumAttribute(Name = "None")]
		none,

		[System.Xml.Serialization.XmlEnumAttribute(Name = "WEP")]
		wep,

		[System.Xml.Serialization.XmlEnumAttribute(Name = "PSK")]
		psk,

		[System.Xml.Serialization.XmlEnumAttribute(Name = "Dot1X")]
		dot1X,

		[System.Xml.Serialization.XmlEnumAttribute(Name = "Extended")]
		extended,
	}

	[System.SerializableAttribute()]
	[System.Xml.Serialization.XmlTypeAttribute(TypeName = "Dot11Cipher", Namespace = "http://www.onvif.org/ver10/schema")]
	public enum Dot11Cipher {

		[System.Xml.Serialization.XmlEnumAttribute(Name = "CCMP")]
		ccmp,

		[System.Xml.Serialization.XmlEnumAttribute(Name = "TKIP")]
		tkip,

		[System.Xml.Serialization.XmlEnumAttribute(Name = "Any")]
		any,

		[System.Xml.Serialization.XmlEnumAttribute(Name = "Extended")]
		extended,
	}

	[System.SerializableAttribute()]
	[System.Xml.Serialization.XmlTypeAttribute(TypeName = "Dot11PSKSet", Namespace = "http://www.onvif.org/ver10/schema")]
	public partial class Dot11PSKSet {

		private System.Xml.XmlAttribute[] _anyAttr;

		private byte[] _key;

		private string _passphrase;

		private Dot11PSKSetExtension _extension;

		[System.Xml.Serialization.XmlAnyAttributeAttribute()]
		public System.Xml.XmlAttribute[] anyAttr {
			get {
				return this._anyAttr;
			}
			set {
				this._anyAttr = value;
			}
		}

		/// <summary>
		/// According to IEEE802.11-2007 H.4.1 the RSNA PSK consists of 256 bits, or 64 octets when represented in hex
		/// Either Key or Passphrase shall be given, if both are supplied Key shall be used by the device and Passphrase ignored.
		/// </summary>
		/// <remarks>optional, order 0</remarks>
		[System.Xml.Serialization.XmlElementAttribute("Key", Namespace = "http://www.onvif.org/ver10/schema", DataType = "hexBinary")]
		public byte[] key {
			get {
				return this._key;
			}
			set {
				this._key = value;
			}
		}

		/// <summary>
		/// According to IEEE802.11-2007 H.4.1 a pass-phrase is a sequence of between 8 and 63 ASCII-encoded characters and
		/// each character in the pass-phrase must have an encoding in the range of 32 to 126 (decimal),inclusive.
		/// If only Passpharse is supplied the Key shall be derived using the algorithm described in IEEE802.11-2007 section H.4
		/// </summary>
		/// <remarks>optional, order 1</remarks>
		[System.Xml.Serialization.XmlElementAttribute("Passphrase", Namespace = "http://www.onvif.org/ver10/schema", DataType = "string")]
		public string passphrase {
			get {
				return this._passphrase;
			}
			set {
				this._passphrase = value;
			}
		}

		/// <remarks>optional, order 2</remarks>
		[System.Xml.Serialization.XmlElementAttribute("Extension", Namespace = "http://www.onvif.org/ver10/schema")]
		public Dot11PSKSetExtension extension {
			get {
				return this._extension;
			}
			set {
				this._extension = value;
			}
		}
	}

	[System.SerializableAttribute()]
	[System.Xml.Serialization.XmlTypeAttribute(TypeName = "Dot11PSKSetExtension", Namespace = "http://www.onvif.org/ver10/schema")]
	public partial class Dot11PSKSetExtension {

		private System.Xml.XmlElement[] _any;

		/// <remarks>optional, order 0, namespace ##any</remarks>
		[System.Xml.Serialization.XmlAnyElementAttribute()]
		public System.Xml.XmlElement[] any {
			get {
				return this._any;
			}
			set {
				this._any = value;
			}
		}
	}

	[System.SerializableAttribute()]
	[System.Xml.Serialization.XmlTypeAttribute(TypeName = "Dot11SecurityConfigurationExtension", Namespace = "http://www.onvif.org/ver10/schema")]
	public partial class Dot11SecurityConfigurationExtension {

		private System.Xml.XmlAttribute[] _anyAttr;

		private System.Xml.XmlElement[] _any;

		[System.Xml.Serialization.XmlAnyAttributeAttribute()]
		public System.Xml.XmlAttribute[] anyAttr {
			get {
				return this._anyAttr;
			}
			set {
				this._anyAttr = value;
			}
		}

		/// <remarks>optional, order 0, namespace ##any</remarks>
		[System.Xml.Serialization.XmlAnyElementAttribute()]
		public System.Xml.XmlElement[] any {
			get {
				return this._any;
			}
			set {
				this._any = value;
			}
		}
	}

	[System.SerializableAttribute()]
	[System.Xml.Serialization.XmlTypeAttribute(TypeName = "NetworkInterfaceExtension2", Namespace = "http://www.onvif.org/ver10/schema")]
	public partial class NetworkInterfaceExtension2 {

		private System.Xml.XmlElement[] _any;

		/// <remarks>optional, order 0, namespace ##targetNamespace</remarks>
		[System.Xml.Serialization.XmlAnyElementAttribute(Namespace = "http://www.onvif.org/ver10/schema")]
		public System.Xml.XmlElement[] any {
			get {
				return this._any;
			}
			set {
				this._any = value;
			}
		}
	}

	[System.SerializableAttribute()]
	[System.Xml.Serialization.XmlTypeAttribute(TypeName = "NetworkProtocol", Namespace = "http://www.onvif.org/ver10/schema")]
	public partial class NetworkProtocol {

		private System.Xml.XmlAttribute[] _anyAttr;

		private NetworkProtocolType _name;

		private bool _enabled;

		private int[] _port;

		private NetworkProtocolExtension _extension;

		[System.Xml.Serialization.XmlAnyAttributeAttribute()]
		public System.Xml.XmlAttribute[] anyAttr {
			get {
				return this._anyAttr;
			}
			set {
				this._anyAttr = value;
			}
		}

		/// <summary>
		/// Network protocol type string.
		/// </summary>
		/// <remarks>reqired, order 0</remarks>
		[System.Xml.Serialization.XmlElementAttribute("Name", Namespace = "http://www.onvif.org/ver10/schema")]
		public NetworkProtocolType name {
			get {
				return this._name;
			}
			set {
				this._name = value;
			}
		}

		/// <summary>
		/// Indicates if the protocol is enabled or not.
		/// </summary>
		/// <remarks>reqired, order 1</remarks>
		[System.Xml.Serialization.XmlElementAttribute("Enabled", Namespace = "http://www.onvif.org/ver10/schema", DataType = "boolean")]
		public bool enabled {
			get {
				return this._enabled;
			}
			set {
				this._enabled = value;
			}
		}

		/// <summary>
		/// The port that is used by the protocol.
		/// </summary>
		/// <remarks>reqired, order 2</remarks>
		[System.Xml.Serialization.XmlElementAttribute("Port", Namespace = "http://www.onvif.org/ver10/schema", DataType = "int")]
		public int[] port {
			get {
				return this._port;
			}
			set {
				this._port = value;
			}
		}

		/// <remarks>optional, order 3</remarks>
		[System.Xml.Serialization.XmlElementAttribute("Extension", Namespace = "http://www.onvif.org/ver10/schema")]
		public NetworkProtocolExtension extension {
			get {
				return this._extension;
			}
			set {
				this._extension = value;
			}
		}
	}

	[System.SerializableAttribute()]
	[System.Xml.Serialization.XmlTypeAttribute(TypeName = "NetworkProtocolType", Namespace = "http://www.onvif.org/ver10/schema")]
	public enum NetworkProtocolType {

		[System.Xml.Serialization.XmlEnumAttribute(Name = "HTTP")]
		http,

		[System.Xml.Serialization.XmlEnumAttribute(Name = "HTTPS")]
		https,

		[System.Xml.Serialization.XmlEnumAttribute(Name = "RTSP")]
		rtsp,
	}

	[System.SerializableAttribute()]
	[System.Xml.Serialization.XmlTypeAttribute(TypeName = "NetworkProtocolExtension", Namespace = "http://www.onvif.org/ver10/schema")]
	public partial class NetworkProtocolExtension {

		private System.Xml.XmlElement[] _any;

		/// <remarks>optional, order 0, namespace ##any</remarks>
		[System.Xml.Serialization.XmlAnyElementAttribute()]
		public System.Xml.XmlElement[] any {
			get {
				return this._any;
			}
			set {
				this._any = value;
			}
		}
	}

	[System.SerializableAttribute()]
	[System.Xml.Serialization.XmlTypeAttribute(TypeName = "NetworkHostType", Namespace = "http://www.onvif.org/ver10/schema")]
	public enum NetworkHostType {

		[System.Xml.Serialization.XmlEnumAttribute(Name = "IPv4")]
		iPv4,

		[System.Xml.Serialization.XmlEnumAttribute(Name = "IPv6")]
		iPv6,

		[System.Xml.Serialization.XmlEnumAttribute(Name = "DNS")]
		dns,
	}

	[System.SerializableAttribute()]
	[System.Xml.Serialization.XmlTypeAttribute(TypeName = "NetworkHost", Namespace = "http://www.onvif.org/ver10/schema")]
	public partial class NetworkHost {

		private System.Xml.XmlAttribute[] _anyAttr;

		private NetworkHostType _type;

		private string _iPv4Address;

		private string _iPv6Address;

		private string _dnSname;

		private NetworkHostExtension _extension;

		[System.Xml.Serialization.XmlAnyAttributeAttribute()]
		public System.Xml.XmlAttribute[] anyAttr {
			get {
				return this._anyAttr;
			}
			set {
				this._anyAttr = value;
			}
		}

		/// <summary>
		/// Network host type: IPv4, IPv6 or DNS.
		/// </summary>
		/// <remarks>reqired, order 0</remarks>
		[System.Xml.Serialization.XmlElementAttribute("Type", Namespace = "http://www.onvif.org/ver10/schema")]
		public NetworkHostType type {
			get {
				return this._type;
			}
			set {
				this._type = value;
			}
		}

		/// <summary>
		/// IPv4 address.
		/// </summary>
		/// <remarks>optional, order 1</remarks>
		[System.Xml.Serialization.XmlElementAttribute("IPv4Address", Namespace = "http://www.onvif.org/ver10/schema", DataType = "token")]
		public string iPv4Address {
			get {
				return this._iPv4Address;
			}
			set {
				this._iPv4Address = value;
			}
		}

		/// <summary>
		/// IPv6 address.
		/// </summary>
		/// <remarks>optional, order 2</remarks>
		[System.Xml.Serialization.XmlElementAttribute("IPv6Address", Namespace = "http://www.onvif.org/ver10/schema", DataType = "token")]
		public string iPv6Address {
			get {
				return this._iPv6Address;
			}
			set {
				this._iPv6Address = value;
			}
		}

		/// <summary>
		/// DNS name.
		/// </summary>
		/// <remarks>optional, order 3</remarks>
		[System.Xml.Serialization.XmlElementAttribute("DNSname", Namespace = "http://www.onvif.org/ver10/schema", DataType = "token")]
		public string dnSname {
			get {
				return this._dnSname;
			}
			set {
				this._dnSname = value;
			}
		}

		/// <remarks>optional, order 4</remarks>
		[System.Xml.Serialization.XmlElementAttribute("Extension", Namespace = "http://www.onvif.org/ver10/schema")]
		public NetworkHostExtension extension {
			get {
				return this._extension;
			}
			set {
				this._extension = value;
			}
		}
	}

	[System.SerializableAttribute()]
	[System.Xml.Serialization.XmlTypeAttribute(TypeName = "NetworkHostExtension", Namespace = "http://www.onvif.org/ver10/schema")]
	public partial class NetworkHostExtension {

		private System.Xml.XmlElement[] _any;

		/// <remarks>optional, order 0, namespace ##any</remarks>
		[System.Xml.Serialization.XmlAnyElementAttribute()]
		public System.Xml.XmlElement[] any {
			get {
				return this._any;
			}
			set {
				this._any = value;
			}
		}
	}

	[System.SerializableAttribute()]
	[System.Xml.Serialization.XmlTypeAttribute(TypeName = "HostnameInformation", Namespace = "http://www.onvif.org/ver10/schema")]
	public partial class HostnameInformation {

		private System.Xml.XmlAttribute[] _anyAttr;

		private bool _fromDHCP;

		private string _name;

		private HostnameInformationExtension _extension;

		[System.Xml.Serialization.XmlAnyAttributeAttribute()]
		public System.Xml.XmlAttribute[] anyAttr {
			get {
				return this._anyAttr;
			}
			set {
				this._anyAttr = value;
			}
		}

		/// <summary>
		/// Indicates whether the hostname is obtained from DHCP or not.
		/// </summary>
		/// <remarks>reqired, order 0</remarks>
		[System.Xml.Serialization.XmlElementAttribute("FromDHCP", Namespace = "http://www.onvif.org/ver10/schema", DataType = "boolean")]
		public bool fromDHCP {
			get {
				return this._fromDHCP;
			}
			set {
				this._fromDHCP = value;
			}
		}

		/// <summary>
		/// Indicates the hostname.
		/// </summary>
		/// <remarks>optional, order 1</remarks>
		[System.Xml.Serialization.XmlElementAttribute("Name", Namespace = "http://www.onvif.org/ver10/schema", DataType = "token")]
		public string name {
			get {
				return this._name;
			}
			set {
				this._name = value;
			}
		}

		/// <remarks>optional, order 2</remarks>
		[System.Xml.Serialization.XmlElementAttribute("Extension", Namespace = "http://www.onvif.org/ver10/schema")]
		public HostnameInformationExtension extension {
			get {
				return this._extension;
			}
			set {
				this._extension = value;
			}
		}
	}

	[System.SerializableAttribute()]
	[System.Xml.Serialization.XmlTypeAttribute(TypeName = "HostnameInformationExtension", Namespace = "http://www.onvif.org/ver10/schema")]
	public partial class HostnameInformationExtension {

		private System.Xml.XmlElement[] _any;

		/// <remarks>optional, order 0, namespace ##any</remarks>
		[System.Xml.Serialization.XmlAnyElementAttribute()]
		public System.Xml.XmlElement[] any {
			get {
				return this._any;
			}
			set {
				this._any = value;
			}
		}
	}

	[System.SerializableAttribute()]
	[System.Xml.Serialization.XmlTypeAttribute(TypeName = "DNSInformation", Namespace = "http://www.onvif.org/ver10/schema")]
	public partial class DNSInformation {

		private System.Xml.XmlAttribute[] _anyAttr;

		private bool _fromDHCP;

		private string[] _searchDomain;

		private IPAddress[] _dnsFromDHCP;

		private IPAddress[] _dnsManual;

		private DNSInformationExtension _extension;

		[System.Xml.Serialization.XmlAnyAttributeAttribute()]
		public System.Xml.XmlAttribute[] anyAttr {
			get {
				return this._anyAttr;
			}
			set {
				this._anyAttr = value;
			}
		}

		/// <summary>
		/// Indicates whether or not DNS information is retrieved from DHCP.
		/// </summary>
		/// <remarks>reqired, order 0</remarks>
		[System.Xml.Serialization.XmlElementAttribute("FromDHCP", Namespace = "http://www.onvif.org/ver10/schema", DataType = "boolean")]
		public bool fromDHCP {
			get {
				return this._fromDHCP;
			}
			set {
				this._fromDHCP = value;
			}
		}

		/// <summary>
		/// Search domain.
		/// </summary>
		/// <remarks>optional, order 1</remarks>
		[System.Xml.Serialization.XmlElementAttribute("SearchDomain", Namespace = "http://www.onvif.org/ver10/schema", DataType = "token")]
		public string[] searchDomain {
			get {
				return this._searchDomain;
			}
			set {
				this._searchDomain = value;
			}
		}

		/// <summary>
		/// List of DNS addresses received from DHCP.
		/// </summary>
		/// <remarks>optional, order 2</remarks>
		[System.Xml.Serialization.XmlElementAttribute("DNSFromDHCP", Namespace = "http://www.onvif.org/ver10/schema")]
		public IPAddress[] dnsFromDHCP {
			get {
				return this._dnsFromDHCP;
			}
			set {
				this._dnsFromDHCP = value;
			}
		}

		/// <summary>
		/// List of manually entered DNS addresses.
		/// </summary>
		/// <remarks>optional, order 3</remarks>
		[System.Xml.Serialization.XmlElementAttribute("DNSManual", Namespace = "http://www.onvif.org/ver10/schema")]
		public IPAddress[] dnsManual {
			get {
				return this._dnsManual;
			}
			set {
				this._dnsManual = value;
			}
		}

		/// <remarks>optional, order 4</remarks>
		[System.Xml.Serialization.XmlElementAttribute("Extension", Namespace = "http://www.onvif.org/ver10/schema")]
		public DNSInformationExtension extension {
			get {
				return this._extension;
			}
			set {
				this._extension = value;
			}
		}
	}

	[System.SerializableAttribute()]
	[System.Xml.Serialization.XmlTypeAttribute(TypeName = "DNSInformationExtension", Namespace = "http://www.onvif.org/ver10/schema")]
	public partial class DNSInformationExtension {

		private System.Xml.XmlElement[] _any;

		/// <remarks>optional, order 0, namespace ##any</remarks>
		[System.Xml.Serialization.XmlAnyElementAttribute()]
		public System.Xml.XmlElement[] any {
			get {
				return this._any;
			}
			set {
				this._any = value;
			}
		}
	}

	[System.SerializableAttribute()]
	[System.Xml.Serialization.XmlTypeAttribute(TypeName = "NTPInformation", Namespace = "http://www.onvif.org/ver10/schema")]
	public partial class NTPInformation {

		private System.Xml.XmlAttribute[] _anyAttr;

		private bool _fromDHCP;

		private NetworkHost[] _ntpFromDHCP;

		private NetworkHost[] _ntpManual;

		private NTPInformationExtension _extension;

		[System.Xml.Serialization.XmlAnyAttributeAttribute()]
		public System.Xml.XmlAttribute[] anyAttr {
			get {
				return this._anyAttr;
			}
			set {
				this._anyAttr = value;
			}
		}

		/// <summary>
		/// Indicates if NTP information is to be retrieved by using DHCP.
		/// </summary>
		/// <remarks>reqired, order 0</remarks>
		[System.Xml.Serialization.XmlElementAttribute("FromDHCP", Namespace = "http://www.onvif.org/ver10/schema", DataType = "boolean")]
		public bool fromDHCP {
			get {
				return this._fromDHCP;
			}
			set {
				this._fromDHCP = value;
			}
		}

		/// <summary>
		/// List of NTP addresses retrieved by using DHCP.
		/// </summary>
		/// <remarks>optional, order 1</remarks>
		[System.Xml.Serialization.XmlElementAttribute("NTPFromDHCP", Namespace = "http://www.onvif.org/ver10/schema")]
		public NetworkHost[] ntpFromDHCP {
			get {
				return this._ntpFromDHCP;
			}
			set {
				this._ntpFromDHCP = value;
			}
		}

		/// <summary>
		/// List of manually entered NTP addresses.
		/// </summary>
		/// <remarks>optional, order 2</remarks>
		[System.Xml.Serialization.XmlElementAttribute("NTPManual", Namespace = "http://www.onvif.org/ver10/schema")]
		public NetworkHost[] ntpManual {
			get {
				return this._ntpManual;
			}
			set {
				this._ntpManual = value;
			}
		}

		/// <remarks>optional, order 3</remarks>
		[System.Xml.Serialization.XmlElementAttribute("Extension", Namespace = "http://www.onvif.org/ver10/schema")]
		public NTPInformationExtension extension {
			get {
				return this._extension;
			}
			set {
				this._extension = value;
			}
		}
	}

	[System.SerializableAttribute()]
	[System.Xml.Serialization.XmlTypeAttribute(TypeName = "NTPInformationExtension", Namespace = "http://www.onvif.org/ver10/schema")]
	public partial class NTPInformationExtension {

		private System.Xml.XmlElement[] _any;

		/// <remarks>optional, order 0, namespace ##any</remarks>
		[System.Xml.Serialization.XmlAnyElementAttribute()]
		public System.Xml.XmlElement[] any {
			get {
				return this._any;
			}
			set {
				this._any = value;
			}
		}
	}

	[System.SerializableAttribute()]
	[System.Xml.Serialization.XmlTypeAttribute(TypeName = "IPAddressFilterType", Namespace = "http://www.onvif.org/ver10/schema")]
	public enum IPAddressFilterType {

		[System.Xml.Serialization.XmlEnumAttribute(Name = "Allow")]
		allow,

		[System.Xml.Serialization.XmlEnumAttribute(Name = "Deny")]
		deny,
	}

	[System.SerializableAttribute()]
	[System.Xml.Serialization.XmlTypeAttribute(TypeName = "DynamicDNSInformation", Namespace = "http://www.onvif.org/ver10/schema")]
	public partial class DynamicDNSInformation {

		private System.Xml.XmlAttribute[] _anyAttr;

		private DynamicDNSType _type;

		private string _name;

		private XsDuration _ttl;

		private DynamicDNSInformationExtension _extension;

		[System.Xml.Serialization.XmlAnyAttributeAttribute()]
		public System.Xml.XmlAttribute[] anyAttr {
			get {
				return this._anyAttr;
			}
			set {
				this._anyAttr = value;
			}
		}

		/// <summary>
		/// Dynamic DNS type.
		/// </summary>
		/// <remarks>reqired, order 0</remarks>
		[System.Xml.Serialization.XmlElementAttribute("Type", Namespace = "http://www.onvif.org/ver10/schema")]
		public DynamicDNSType type {
			get {
				return this._type;
			}
			set {
				this._type = value;
			}
		}

		/// <summary>
		/// DNS name.
		/// </summary>
		/// <remarks>optional, order 1</remarks>
		[System.Xml.Serialization.XmlElementAttribute("Name", Namespace = "http://www.onvif.org/ver10/schema", DataType = "token")]
		public string name {
			get {
				return this._name;
			}
			set {
				this._name = value;
			}
		}

		/// <summary>
		/// Time to live.
		/// </summary>
		/// <remarks>optional, order 2</remarks>
		[System.Xml.Serialization.XmlElementAttribute("TTL", Namespace = "http://www.onvif.org/ver10/schema")]
		public XsDuration ttl {
			get {
				return this._ttl;
			}
			set {
				this._ttl = value;
			}
		}

		/// <remarks>optional, order 3</remarks>
		[System.Xml.Serialization.XmlElementAttribute("Extension", Namespace = "http://www.onvif.org/ver10/schema")]
		public DynamicDNSInformationExtension extension {
			get {
				return this._extension;
			}
			set {
				this._extension = value;
			}
		}
	}

	[System.SerializableAttribute()]
	[System.Xml.Serialization.XmlTypeAttribute(TypeName = "DynamicDNSType", Namespace = "http://www.onvif.org/ver10/schema")]
	public enum DynamicDNSType {

		[System.Xml.Serialization.XmlEnumAttribute(Name = "NoUpdate")]
		noUpdate,

		[System.Xml.Serialization.XmlEnumAttribute(Name = "ClientUpdates")]
		clientUpdates,

		[System.Xml.Serialization.XmlEnumAttribute(Name = "ServerUpdates")]
		serverUpdates,
	}

	[System.SerializableAttribute()]
	[System.Xml.Serialization.XmlTypeAttribute(TypeName = "DynamicDNSInformationExtension", Namespace = "http://www.onvif.org/ver10/schema")]
	public partial class DynamicDNSInformationExtension {

		private System.Xml.XmlElement[] _any;

		/// <remarks>optional, order 0, namespace ##any</remarks>
		[System.Xml.Serialization.XmlAnyElementAttribute()]
		public System.Xml.XmlElement[] any {
			get {
				return this._any;
			}
			set {
				this._any = value;
			}
		}
	}

	[System.SerializableAttribute()]
	[System.Xml.Serialization.XmlTypeAttribute(TypeName = "NetworkInterfaceSetConfiguration", Namespace = "http://www.onvif.org/ver10/schema")]
	public partial class NetworkInterfaceSetConfiguration {

		private System.Xml.XmlAttribute[] _anyAttr;

		private bool _enabled;

		private bool _enabledSpecified;

		private NetworkInterfaceConnectionSetting _link;

		private int _mtu;

		private bool _mtuSpecified;

		private IPv4NetworkInterfaceSetConfiguration _iPv4;

		private IPv6NetworkInterfaceSetConfiguration _iPv6;

		private NetworkInterfaceSetConfigurationExtension _extension;

		[System.Xml.Serialization.XmlAnyAttributeAttribute()]
		public System.Xml.XmlAttribute[] anyAttr {
			get {
				return this._anyAttr;
			}
			set {
				this._anyAttr = value;
			}
		}

		/// <summary>
		/// Indicates whether or not an interface is enabled.
		/// </summary>
		/// <remarks>optional, order 0</remarks>
		[System.Xml.Serialization.XmlElementAttribute("Enabled", Namespace = "http://www.onvif.org/ver10/schema", DataType = "boolean")]
		public bool enabled {
			get {
				return this._enabled;
			}
			set {
				this._enabled = value;
			}
		}

		[System.Xml.Serialization.XmlIgnoreAttribute()]
		public bool enabledSpecified {
			get {
				return this._enabledSpecified;
			}
			set {
				this._enabledSpecified = value;
			}
		}

		/// <summary>
		/// Link configuration.
		/// </summary>
		/// <remarks>optional, order 1</remarks>
		[System.Xml.Serialization.XmlElementAttribute("Link", Namespace = "http://www.onvif.org/ver10/schema")]
		public NetworkInterfaceConnectionSetting link {
			get {
				return this._link;
			}
			set {
				this._link = value;
			}
		}

		/// <summary>
		/// Maximum transmission unit.
		/// </summary>
		/// <remarks>optional, order 2</remarks>
		[System.Xml.Serialization.XmlElementAttribute("MTU", Namespace = "http://www.onvif.org/ver10/schema", DataType = "int")]
		public int mtu {
			get {
				return this._mtu;
			}
			set {
				this._mtu = value;
			}
		}

		[System.Xml.Serialization.XmlIgnoreAttribute()]
		public bool mtuSpecified {
			get {
				return this._mtuSpecified;
			}
			set {
				this._mtuSpecified = value;
			}
		}

		/// <summary>
		/// IPv4 network interface configuration.
		/// </summary>
		/// <remarks>optional, order 3</remarks>
		[System.Xml.Serialization.XmlElementAttribute("IPv4", Namespace = "http://www.onvif.org/ver10/schema")]
		public IPv4NetworkInterfaceSetConfiguration iPv4 {
			get {
				return this._iPv4;
			}
			set {
				this._iPv4 = value;
			}
		}

		/// <summary>
		/// IPv6 network interface configuration.
		/// </summary>
		/// <remarks>optional, order 4</remarks>
		[System.Xml.Serialization.XmlElementAttribute("IPv6", Namespace = "http://www.onvif.org/ver10/schema")]
		public IPv6NetworkInterfaceSetConfiguration iPv6 {
			get {
				return this._iPv6;
			}
			set {
				this._iPv6 = value;
			}
		}

		/// <remarks>optional, order 5</remarks>
		[System.Xml.Serialization.XmlElementAttribute("Extension", Namespace = "http://www.onvif.org/ver10/schema")]
		public NetworkInterfaceSetConfigurationExtension extension {
			get {
				return this._extension;
			}
			set {
				this._extension = value;
			}
		}
	}

	[System.SerializableAttribute()]
	[System.Xml.Serialization.XmlTypeAttribute(TypeName = "IPv4NetworkInterfaceSetConfiguration", Namespace = "http://www.onvif.org/ver10/schema")]
	public partial class IPv4NetworkInterfaceSetConfiguration {

		private bool _enabled;

		private bool _enabledSpecified;

		private PrefixedIPv4Address[] _manual;

		private bool _dhcp;

		private bool _dhcpSpecified;

		/// <summary>
		/// Indicates whether or not IPv4 is enabled.
		/// </summary>
		/// <remarks>optional, order 0</remarks>
		[System.Xml.Serialization.XmlElementAttribute("Enabled", Namespace = "http://www.onvif.org/ver10/schema", DataType = "boolean")]
		public bool enabled {
			get {
				return this._enabled;
			}
			set {
				this._enabled = value;
			}
		}

		[System.Xml.Serialization.XmlIgnoreAttribute()]
		public bool enabledSpecified {
			get {
				return this._enabledSpecified;
			}
			set {
				this._enabledSpecified = value;
			}
		}

		/// <summary>
		/// List of manually added IPv4 addresses.
		/// </summary>
		/// <remarks>optional, order 1</remarks>
		[System.Xml.Serialization.XmlElementAttribute("Manual", Namespace = "http://www.onvif.org/ver10/schema")]
		public PrefixedIPv4Address[] manual {
			get {
				return this._manual;
			}
			set {
				this._manual = value;
			}
		}

		/// <summary>
		/// Indicates whether or not DHCP is used.
		/// </summary>
		/// <remarks>optional, order 2</remarks>
		[System.Xml.Serialization.XmlElementAttribute("DHCP", Namespace = "http://www.onvif.org/ver10/schema", DataType = "boolean")]
		public bool dhcp {
			get {
				return this._dhcp;
			}
			set {
				this._dhcp = value;
			}
		}

		[System.Xml.Serialization.XmlIgnoreAttribute()]
		public bool dhcpSpecified {
			get {
				return this._dhcpSpecified;
			}
			set {
				this._dhcpSpecified = value;
			}
		}
	}

	[System.SerializableAttribute()]
	[System.Xml.Serialization.XmlTypeAttribute(TypeName = "IPv6NetworkInterfaceSetConfiguration", Namespace = "http://www.onvif.org/ver10/schema")]
	public partial class IPv6NetworkInterfaceSetConfiguration {

		private bool _enabled;

		private bool _enabledSpecified;

		private bool _acceptRouterAdvert;

		private bool _acceptRouterAdvertSpecified;

		private PrefixedIPv6Address[] _manual;

		private IPv6DHCPConfiguration _dhcp;

		private bool _dhcpSpecified;

		/// <summary>
		/// Indicates whether or not IPv6 is enabled.
		/// </summary>
		/// <remarks>optional, order 0</remarks>
		[System.Xml.Serialization.XmlElementAttribute("Enabled", Namespace = "http://www.onvif.org/ver10/schema", DataType = "boolean")]
		public bool enabled {
			get {
				return this._enabled;
			}
			set {
				this._enabled = value;
			}
		}

		[System.Xml.Serialization.XmlIgnoreAttribute()]
		public bool enabledSpecified {
			get {
				return this._enabledSpecified;
			}
			set {
				this._enabledSpecified = value;
			}
		}

		/// <summary>
		/// Indicates whether router advertisment is used.
		/// </summary>
		/// <remarks>optional, order 1</remarks>
		[System.Xml.Serialization.XmlElementAttribute("AcceptRouterAdvert", Namespace = "http://www.onvif.org/ver10/schema", DataType = "boolean")]
		public bool acceptRouterAdvert {
			get {
				return this._acceptRouterAdvert;
			}
			set {
				this._acceptRouterAdvert = value;
			}
		}

		[System.Xml.Serialization.XmlIgnoreAttribute()]
		public bool acceptRouterAdvertSpecified {
			get {
				return this._acceptRouterAdvertSpecified;
			}
			set {
				this._acceptRouterAdvertSpecified = value;
			}
		}

		/// <summary>
		/// List of manually added IPv6 addresses.
		/// </summary>
		/// <remarks>optional, order 2</remarks>
		[System.Xml.Serialization.XmlElementAttribute("Manual", Namespace = "http://www.onvif.org/ver10/schema")]
		public PrefixedIPv6Address[] manual {
			get {
				return this._manual;
			}
			set {
				this._manual = value;
			}
		}

		/// <summary>
		/// DHCP configuration.
		/// </summary>
		/// <remarks>optional, order 3</remarks>
		[System.Xml.Serialization.XmlElementAttribute("DHCP", Namespace = "http://www.onvif.org/ver10/schema")]
		public IPv6DHCPConfiguration dhcp {
			get {
				return this._dhcp;
			}
			set {
				this._dhcp = value;
			}
		}

		[System.Xml.Serialization.XmlIgnoreAttribute()]
		public bool dhcpSpecified {
			get {
				return this._dhcpSpecified;
			}
			set {
				this._dhcpSpecified = value;
			}
		}
	}

	[System.SerializableAttribute()]
	[System.Xml.Serialization.XmlTypeAttribute(TypeName = "NetworkInterfaceSetConfigurationExtension", Namespace = "http://www.onvif.org/ver10/schema")]
	public partial class NetworkInterfaceSetConfigurationExtension {

		private System.Xml.XmlElement[] _any;

		private Dot3Configuration[] _dot3;

		private Dot11Configuration[] _dot11;

		private NetworkInterfaceSetConfigurationExtension2 _extension;

		/// <remarks>optional, order 0, namespace ##other</remarks>
		[System.Xml.Serialization.XmlAnyElementAttribute()]
		public System.Xml.XmlElement[] any {
			get {
				return this._any;
			}
			set {
				this._any = value;
			}
		}

		/// <remarks>optional, order 1</remarks>
		[System.Xml.Serialization.XmlElementAttribute("Dot3", Namespace = "http://www.onvif.org/ver10/schema")]
		public Dot3Configuration[] dot3 {
			get {
				return this._dot3;
			}
			set {
				this._dot3 = value;
			}
		}

		/// <remarks>optional, order 2</remarks>
		[System.Xml.Serialization.XmlElementAttribute("Dot11", Namespace = "http://www.onvif.org/ver10/schema")]
		public Dot11Configuration[] dot11 {
			get {
				return this._dot11;
			}
			set {
				this._dot11 = value;
			}
		}

		/// <remarks>optional, order 3</remarks>
		[System.Xml.Serialization.XmlElementAttribute("Extension", Namespace = "http://www.onvif.org/ver10/schema")]
		public NetworkInterfaceSetConfigurationExtension2 extension {
			get {
				return this._extension;
			}
			set {
				this._extension = value;
			}
		}
	}

	[System.SerializableAttribute()]
	[System.Xml.Serialization.XmlTypeAttribute(TypeName = "NetworkInterfaceSetConfigurationExtension2", Namespace = "http://www.onvif.org/ver10/schema")]
	public partial class NetworkInterfaceSetConfigurationExtension2 {

		private System.Xml.XmlElement[] _any;

		/// <remarks>optional, order 0, namespace ##targetNamespace</remarks>
		[System.Xml.Serialization.XmlAnyElementAttribute(Namespace = "http://www.onvif.org/ver10/schema")]
		public System.Xml.XmlElement[] any {
			get {
				return this._any;
			}
			set {
				this._any = value;
			}
		}
	}

	[System.SerializableAttribute()]
	[System.Xml.Serialization.XmlTypeAttribute(TypeName = "NetworkGateway", Namespace = "http://www.onvif.org/ver10/schema")]
	public partial class NetworkGateway {

		private string[] _iPv4Address;

		private string[] _iPv6Address;

		/// <summary>
		/// IPv4 address string.
		/// </summary>
		/// <remarks>optional, order 0</remarks>
		[System.Xml.Serialization.XmlElementAttribute("IPv4Address", Namespace = "http://www.onvif.org/ver10/schema", DataType = "token")]
		public string[] iPv4Address {
			get {
				return this._iPv4Address;
			}
			set {
				this._iPv4Address = value;
			}
		}

		/// <summary>
		/// IPv6 address string.
		/// </summary>
		/// <remarks>optional, order 1</remarks>
		[System.Xml.Serialization.XmlElementAttribute("IPv6Address", Namespace = "http://www.onvif.org/ver10/schema", DataType = "token")]
		public string[] iPv6Address {
			get {
				return this._iPv6Address;
			}
			set {
				this._iPv6Address = value;
			}
		}
	}

	[System.SerializableAttribute()]
	[System.Xml.Serialization.XmlTypeAttribute(TypeName = "NetworkZeroConfiguration", Namespace = "http://www.onvif.org/ver10/schema")]
	public partial class NetworkZeroConfiguration {

		private System.Xml.XmlAttribute[] _anyAttr;

		private string _interfaceToken;

		private bool _enabled;

		private string[] _addresses;

		private NetworkZeroConfigurationExtension _extension;

		[System.Xml.Serialization.XmlAnyAttributeAttribute()]
		public System.Xml.XmlAttribute[] anyAttr {
			get {
				return this._anyAttr;
			}
			set {
				this._anyAttr = value;
			}
		}

		/// <summary>
		/// Unique identifier of network interface.
		/// </summary>
		/// <remarks>reqired, order 0</remarks>
		[System.Xml.Serialization.XmlElementAttribute("InterfaceToken", Namespace = "http://www.onvif.org/ver10/schema", DataType = "string")]
		public string interfaceToken {
			get {
				return this._interfaceToken;
			}
			set {
				this._interfaceToken = value;
			}
		}

		/// <summary>
		/// Indicates whether the zero-configuration is enabled or not.
		/// </summary>
		/// <remarks>reqired, order 1</remarks>
		[System.Xml.Serialization.XmlElementAttribute("Enabled", Namespace = "http://www.onvif.org/ver10/schema", DataType = "boolean")]
		public bool enabled {
			get {
				return this._enabled;
			}
			set {
				this._enabled = value;
			}
		}

		/// <summary>
		/// The zero-configuration IPv4 address(es)
		/// </summary>
		/// <remarks>optional, order 2</remarks>
		[System.Xml.Serialization.XmlElementAttribute("Addresses", Namespace = "http://www.onvif.org/ver10/schema", DataType = "token")]
		public string[] addresses {
			get {
				return this._addresses;
			}
			set {
				this._addresses = value;
			}
		}

		/// <remarks>optional, order 3</remarks>
		[System.Xml.Serialization.XmlElementAttribute("Extension", Namespace = "http://www.onvif.org/ver10/schema")]
		public NetworkZeroConfigurationExtension extension {
			get {
				return this._extension;
			}
			set {
				this._extension = value;
			}
		}
	}

	[System.SerializableAttribute()]
	[System.Xml.Serialization.XmlTypeAttribute(TypeName = "NetworkZeroConfigurationExtension", Namespace = "http://www.onvif.org/ver10/schema")]
	public partial class NetworkZeroConfigurationExtension {

		private System.Xml.XmlElement[] _any;

		private NetworkZeroConfiguration[] _additional;

		private NetworkZeroConfigurationExtension2 _extension;

		/// <remarks>optional, order 0, namespace ##other</remarks>
		[System.Xml.Serialization.XmlAnyElementAttribute()]
		public System.Xml.XmlElement[] any {
			get {
				return this._any;
			}
			set {
				this._any = value;
			}
		}

		/// <summary>
		/// Optional array holding the configuration for the second and possibly further interfaces.
		/// </summary>
		/// <remarks>optional, order 1</remarks>
		[System.Xml.Serialization.XmlElementAttribute("Additional", Namespace = "http://www.onvif.org/ver10/schema")]
		public NetworkZeroConfiguration[] additional {
			get {
				return this._additional;
			}
			set {
				this._additional = value;
			}
		}

		/// <remarks>optional, order 2</remarks>
		[System.Xml.Serialization.XmlElementAttribute("Extension", Namespace = "http://www.onvif.org/ver10/schema")]
		public NetworkZeroConfigurationExtension2 extension {
			get {
				return this._extension;
			}
			set {
				this._extension = value;
			}
		}
	}

	[System.SerializableAttribute()]
	[System.Xml.Serialization.XmlTypeAttribute(TypeName = "NetworkZeroConfigurationExtension2", Namespace = "http://www.onvif.org/ver10/schema")]
	public partial class NetworkZeroConfigurationExtension2 {

		private System.Xml.XmlElement[] _any;

		/// <remarks>optional, order 0, namespace ##targetNamespace</remarks>
		[System.Xml.Serialization.XmlAnyElementAttribute(Namespace = "http://www.onvif.org/ver10/schema")]
		public System.Xml.XmlElement[] any {
			get {
				return this._any;
			}
			set {
				this._any = value;
			}
		}
	}

	[System.SerializableAttribute()]
	[System.Xml.Serialization.XmlTypeAttribute(TypeName = "IPAddressFilter", Namespace = "http://www.onvif.org/ver10/schema")]
	public partial class IPAddressFilter {

		private System.Xml.XmlAttribute[] _anyAttr;

		private IPAddressFilterType _type;

		private PrefixedIPv4Address[] _iPv4Address;

		private PrefixedIPv6Address[] _iPv6Address;

		private IPAddressFilterExtension _extension;

		[System.Xml.Serialization.XmlAnyAttributeAttribute()]
		public System.Xml.XmlAttribute[] anyAttr {
			get {
				return this._anyAttr;
			}
			set {
				this._anyAttr = value;
			}
		}

		/// <remarks>reqired, order 0</remarks>
		[System.Xml.Serialization.XmlElementAttribute("Type", Namespace = "http://www.onvif.org/ver10/schema")]
		public IPAddressFilterType type {
			get {
				return this._type;
			}
			set {
				this._type = value;
			}
		}

		/// <remarks>optional, order 1</remarks>
		[System.Xml.Serialization.XmlElementAttribute("IPv4Address", Namespace = "http://www.onvif.org/ver10/schema")]
		public PrefixedIPv4Address[] iPv4Address {
			get {
				return this._iPv4Address;
			}
			set {
				this._iPv4Address = value;
			}
		}

		/// <remarks>optional, order 2</remarks>
		[System.Xml.Serialization.XmlElementAttribute("IPv6Address", Namespace = "http://www.onvif.org/ver10/schema")]
		public PrefixedIPv6Address[] iPv6Address {
			get {
				return this._iPv6Address;
			}
			set {
				this._iPv6Address = value;
			}
		}

		/// <remarks>optional, order 3</remarks>
		[System.Xml.Serialization.XmlElementAttribute("Extension", Namespace = "http://www.onvif.org/ver10/schema")]
		public IPAddressFilterExtension extension {
			get {
				return this._extension;
			}
			set {
				this._extension = value;
			}
		}
	}

	[System.SerializableAttribute()]
	[System.Xml.Serialization.XmlTypeAttribute(TypeName = "IPAddressFilterExtension", Namespace = "http://www.onvif.org/ver10/schema")]
	public partial class IPAddressFilterExtension {

		private System.Xml.XmlElement[] _any;

		/// <remarks>optional, order 0, namespace ##any</remarks>
		[System.Xml.Serialization.XmlAnyElementAttribute()]
		public System.Xml.XmlElement[] any {
			get {
				return this._any;
			}
			set {
				this._any = value;
			}
		}
	}

	[System.SerializableAttribute()]
	[System.Xml.Serialization.XmlTypeAttribute(TypeName = "Dot11Capabilities", Namespace = "http://www.onvif.org/ver10/schema")]
	public partial class Dot11Capabilities {

		private System.Xml.XmlAttribute[] _anyAttr;

		private bool _tkip;

		private bool _scanAvailableNetworks;

		private bool _multipleConfiguration;

		private bool _adHocStationMode;

		private bool _wep;

		private System.Xml.XmlElement[] _any;

		[System.Xml.Serialization.XmlAnyAttributeAttribute()]
		public System.Xml.XmlAttribute[] anyAttr {
			get {
				return this._anyAttr;
			}
			set {
				this._anyAttr = value;
			}
		}

		/// <remarks>reqired, order 0</remarks>
		[System.Xml.Serialization.XmlElementAttribute("TKIP", Namespace = "http://www.onvif.org/ver10/schema", DataType = "boolean")]
		public bool tkip {
			get {
				return this._tkip;
			}
			set {
				this._tkip = value;
			}
		}

		/// <remarks>reqired, order 1</remarks>
		[System.Xml.Serialization.XmlElementAttribute("ScanAvailableNetworks", Namespace = "http://www.onvif.org/ver10/schema", DataType = "boolean")]
		public bool scanAvailableNetworks {
			get {
				return this._scanAvailableNetworks;
			}
			set {
				this._scanAvailableNetworks = value;
			}
		}

		/// <remarks>reqired, order 2</remarks>
		[System.Xml.Serialization.XmlElementAttribute("MultipleConfiguration", Namespace = "http://www.onvif.org/ver10/schema", DataType = "boolean")]
		public bool multipleConfiguration {
			get {
				return this._multipleConfiguration;
			}
			set {
				this._multipleConfiguration = value;
			}
		}

		/// <remarks>reqired, order 3</remarks>
		[System.Xml.Serialization.XmlElementAttribute("AdHocStationMode", Namespace = "http://www.onvif.org/ver10/schema", DataType = "boolean")]
		public bool adHocStationMode {
			get {
				return this._adHocStationMode;
			}
			set {
				this._adHocStationMode = value;
			}
		}

		/// <remarks>reqired, order 4</remarks>
		[System.Xml.Serialization.XmlElementAttribute("WEP", Namespace = "http://www.onvif.org/ver10/schema", DataType = "boolean")]
		public bool wep {
			get {
				return this._wep;
			}
			set {
				this._wep = value;
			}
		}

		/// <remarks>optional, order 5, namespace ##any</remarks>
		[System.Xml.Serialization.XmlAnyElementAttribute()]
		public System.Xml.XmlElement[] any {
			get {
				return this._any;
			}
			set {
				this._any = value;
			}
		}
	}

	[System.SerializableAttribute()]
	[System.Xml.Serialization.XmlTypeAttribute(TypeName = "Dot11SignalStrength", Namespace = "http://www.onvif.org/ver10/schema")]
	public enum Dot11SignalStrength {

		[System.Xml.Serialization.XmlEnumAttribute(Name = "None")]
		none,

		[System.Xml.Serialization.XmlEnumAttribute(Name = "Very Bad")]
		veryBad,

		[System.Xml.Serialization.XmlEnumAttribute(Name = "Bad")]
		bad,

		[System.Xml.Serialization.XmlEnumAttribute(Name = "Good")]
		good,

		[System.Xml.Serialization.XmlEnumAttribute(Name = "Very Good")]
		veryGood,

		[System.Xml.Serialization.XmlEnumAttribute(Name = "Extended")]
		extended,
	}

	[System.SerializableAttribute()]
	[System.Xml.Serialization.XmlTypeAttribute(TypeName = "Dot11Status", Namespace = "http://www.onvif.org/ver10/schema")]
	public partial class Dot11Status {

		private System.Xml.XmlAttribute[] _anyAttr;

		private byte[] _ssid;

		private string _bssid;

		private Dot11Cipher _pairCipher;

		private bool _pairCipherSpecified;

		private Dot11Cipher _groupCipher;

		private bool _groupCipherSpecified;

		private Dot11SignalStrength _signalStrength;

		private bool _signalStrengthSpecified;

		private string _activeConfigAlias;

		private System.Xml.XmlElement[] _any;

		[System.Xml.Serialization.XmlAnyAttributeAttribute()]
		public System.Xml.XmlAttribute[] anyAttr {
			get {
				return this._anyAttr;
			}
			set {
				this._anyAttr = value;
			}
		}

		/// <remarks>reqired, order 0</remarks>
		[System.Xml.Serialization.XmlElementAttribute("SSID", Namespace = "http://www.onvif.org/ver10/schema", DataType = "hexBinary")]
		public byte[] ssid {
			get {
				return this._ssid;
			}
			set {
				this._ssid = value;
			}
		}

		/// <remarks>optional, order 1</remarks>
		[System.Xml.Serialization.XmlElementAttribute("BSSID", Namespace = "http://www.onvif.org/ver10/schema", DataType = "string")]
		public string bssid {
			get {
				return this._bssid;
			}
			set {
				this._bssid = value;
			}
		}

		/// <remarks>optional, order 2</remarks>
		[System.Xml.Serialization.XmlElementAttribute("PairCipher", Namespace = "http://www.onvif.org/ver10/schema")]
		public Dot11Cipher pairCipher {
			get {
				return this._pairCipher;
			}
			set {
				this._pairCipher = value;
			}
		}

		[System.Xml.Serialization.XmlIgnoreAttribute()]
		public bool pairCipherSpecified {
			get {
				return this._pairCipherSpecified;
			}
			set {
				this._pairCipherSpecified = value;
			}
		}

		/// <remarks>optional, order 3</remarks>
		[System.Xml.Serialization.XmlElementAttribute("GroupCipher", Namespace = "http://www.onvif.org/ver10/schema")]
		public Dot11Cipher groupCipher {
			get {
				return this._groupCipher;
			}
			set {
				this._groupCipher = value;
			}
		}

		[System.Xml.Serialization.XmlIgnoreAttribute()]
		public bool groupCipherSpecified {
			get {
				return this._groupCipherSpecified;
			}
			set {
				this._groupCipherSpecified = value;
			}
		}

		/// <remarks>optional, order 4</remarks>
		[System.Xml.Serialization.XmlElementAttribute("SignalStrength", Namespace = "http://www.onvif.org/ver10/schema")]
		public Dot11SignalStrength signalStrength {
			get {
				return this._signalStrength;
			}
			set {
				this._signalStrength = value;
			}
		}

		[System.Xml.Serialization.XmlIgnoreAttribute()]
		public bool signalStrengthSpecified {
			get {
				return this._signalStrengthSpecified;
			}
			set {
				this._signalStrengthSpecified = value;
			}
		}

		/// <remarks>reqired, order 5</remarks>
		[System.Xml.Serialization.XmlElementAttribute("ActiveConfigAlias", Namespace = "http://www.onvif.org/ver10/schema", DataType = "string")]
		public string activeConfigAlias {
			get {
				return this._activeConfigAlias;
			}
			set {
				this._activeConfigAlias = value;
			}
		}

		/// <remarks>optional, order 6, namespace ##any</remarks>
		[System.Xml.Serialization.XmlAnyElementAttribute()]
		public System.Xml.XmlElement[] any {
			get {
				return this._any;
			}
			set {
				this._any = value;
			}
		}
	}

	[System.SerializableAttribute()]
	[System.Xml.Serialization.XmlTypeAttribute(TypeName = "Dot11AuthAndMangementSuite", Namespace = "http://www.onvif.org/ver10/schema")]
	public enum Dot11AuthAndMangementSuite {

		[System.Xml.Serialization.XmlEnumAttribute(Name = "None")]
		none,

		[System.Xml.Serialization.XmlEnumAttribute(Name = "Dot1X")]
		dot1X,

		[System.Xml.Serialization.XmlEnumAttribute(Name = "PSK")]
		psk,

		[System.Xml.Serialization.XmlEnumAttribute(Name = "Extended")]
		extended,
	}

	[System.SerializableAttribute()]
	[System.Xml.Serialization.XmlTypeAttribute(TypeName = "Dot11AvailableNetworks", Namespace = "http://www.onvif.org/ver10/schema")]
	public partial class Dot11AvailableNetworks {

		private System.Xml.XmlAttribute[] _anyAttr;

		private byte[] _ssid;

		private string _bssid;

		private Dot11AuthAndMangementSuite[] _authAndMangementSuite;

		private Dot11Cipher[] _pairCipher;

		private Dot11Cipher[] _groupCipher;

		private Dot11SignalStrength _signalStrength;

		private bool _signalStrengthSpecified;

		private Dot11AvailableNetworksExtension _extension;

		[System.Xml.Serialization.XmlAnyAttributeAttribute()]
		public System.Xml.XmlAttribute[] anyAttr {
			get {
				return this._anyAttr;
			}
			set {
				this._anyAttr = value;
			}
		}

		/// <remarks>reqired, order 0</remarks>
		[System.Xml.Serialization.XmlElementAttribute("SSID", Namespace = "http://www.onvif.org/ver10/schema", DataType = "hexBinary")]
		public byte[] ssid {
			get {
				return this._ssid;
			}
			set {
				this._ssid = value;
			}
		}

		/// <remarks>optional, order 1</remarks>
		[System.Xml.Serialization.XmlElementAttribute("BSSID", Namespace = "http://www.onvif.org/ver10/schema", DataType = "string")]
		public string bssid {
			get {
				return this._bssid;
			}
			set {
				this._bssid = value;
			}
		}

		/// <summary>
		/// See IEEE802.11 7.3.2.25.2 for details.
		/// </summary>
		/// <remarks>optional, order 2</remarks>
		[System.Xml.Serialization.XmlElementAttribute("AuthAndMangementSuite", Namespace = "http://www.onvif.org/ver10/schema")]
		public Dot11AuthAndMangementSuite[] authAndMangementSuite {
			get {
				return this._authAndMangementSuite;
			}
			set {
				this._authAndMangementSuite = value;
			}
		}

		/// <remarks>optional, order 3</remarks>
		[System.Xml.Serialization.XmlElementAttribute("PairCipher", Namespace = "http://www.onvif.org/ver10/schema")]
		public Dot11Cipher[] pairCipher {
			get {
				return this._pairCipher;
			}
			set {
				this._pairCipher = value;
			}
		}

		/// <remarks>optional, order 4</remarks>
		[System.Xml.Serialization.XmlElementAttribute("GroupCipher", Namespace = "http://www.onvif.org/ver10/schema")]
		public Dot11Cipher[] groupCipher {
			get {
				return this._groupCipher;
			}
			set {
				this._groupCipher = value;
			}
		}

		/// <remarks>optional, order 5</remarks>
		[System.Xml.Serialization.XmlElementAttribute("SignalStrength", Namespace = "http://www.onvif.org/ver10/schema")]
		public Dot11SignalStrength signalStrength {
			get {
				return this._signalStrength;
			}
			set {
				this._signalStrength = value;
			}
		}

		[System.Xml.Serialization.XmlIgnoreAttribute()]
		public bool signalStrengthSpecified {
			get {
				return this._signalStrengthSpecified;
			}
			set {
				this._signalStrengthSpecified = value;
			}
		}

		/// <remarks>optional, order 6</remarks>
		[System.Xml.Serialization.XmlElementAttribute("Extension", Namespace = "http://www.onvif.org/ver10/schema")]
		public Dot11AvailableNetworksExtension extension {
			get {
				return this._extension;
			}
			set {
				this._extension = value;
			}
		}
	}

	[System.SerializableAttribute()]
	[System.Xml.Serialization.XmlTypeAttribute(TypeName = "Dot11AvailableNetworksExtension", Namespace = "http://www.onvif.org/ver10/schema")]
	public partial class Dot11AvailableNetworksExtension {

		private System.Xml.XmlElement[] _any;

		/// <remarks>optional, order 0, namespace ##any</remarks>
		[System.Xml.Serialization.XmlAnyElementAttribute()]
		public System.Xml.XmlElement[] any {
			get {
				return this._any;
			}
			set {
				this._any = value;
			}
		}
	}

	[System.SerializableAttribute()]
	[System.Xml.Serialization.XmlTypeAttribute(TypeName = "CapabilityCategory", Namespace = "http://www.onvif.org/ver10/schema")]
	public enum CapabilityCategory {

		[System.Xml.Serialization.XmlEnumAttribute(Name = "All")]
		all,

		[System.Xml.Serialization.XmlEnumAttribute(Name = "Analytics")]
		analytics,

		[System.Xml.Serialization.XmlEnumAttribute(Name = "Device")]
		device,

		[System.Xml.Serialization.XmlEnumAttribute(Name = "Events")]
		events,

		[System.Xml.Serialization.XmlEnumAttribute(Name = "Imaging")]
		imaging,

		[System.Xml.Serialization.XmlEnumAttribute(Name = "Media")]
		media,

		[System.Xml.Serialization.XmlEnumAttribute(Name = "PTZ")]
		ptz,
	}

	[System.SerializableAttribute()]
	[System.Xml.Serialization.XmlTypeAttribute(TypeName = "Capabilities", Namespace = "http://www.onvif.org/ver10/schema")]
	public partial class Capabilities {

		private System.Xml.XmlAttribute[] _anyAttr;

		private AnalyticsCapabilities _analytics;

		private DeviceCapabilities _device;

		private EventCapabilities _events;

		private ImagingCapabilities _imaging;

		private MediaCapabilities _media;

		private PTZCapabilities _ptz;

		private CapabilitiesExtension _extension;

		[System.Xml.Serialization.XmlAnyAttributeAttribute()]
		public System.Xml.XmlAttribute[] anyAttr {
			get {
				return this._anyAttr;
			}
			set {
				this._anyAttr = value;
			}
		}

		/// <summary>
		/// Analytics capabilities
		/// </summary>
		/// <remarks>optional, order 0</remarks>
		[System.Xml.Serialization.XmlElementAttribute("Analytics", Namespace = "http://www.onvif.org/ver10/schema")]
		public AnalyticsCapabilities analytics {
			get {
				return this._analytics;
			}
			set {
				this._analytics = value;
			}
		}

		/// <summary>
		/// Device capabilities
		/// </summary>
		/// <remarks>optional, order 1</remarks>
		[System.Xml.Serialization.XmlElementAttribute("Device", Namespace = "http://www.onvif.org/ver10/schema")]
		public DeviceCapabilities device {
			get {
				return this._device;
			}
			set {
				this._device = value;
			}
		}

		/// <summary>
		/// Event capabilities
		/// </summary>
		/// <remarks>optional, order 2</remarks>
		[System.Xml.Serialization.XmlElementAttribute("Events", Namespace = "http://www.onvif.org/ver10/schema")]
		public EventCapabilities events {
			get {
				return this._events;
			}
			set {
				this._events = value;
			}
		}

		/// <summary>
		/// Imaging capabilities
		/// </summary>
		/// <remarks>optional, order 3</remarks>
		[System.Xml.Serialization.XmlElementAttribute("Imaging", Namespace = "http://www.onvif.org/ver10/schema")]
		public ImagingCapabilities imaging {
			get {
				return this._imaging;
			}
			set {
				this._imaging = value;
			}
		}

		/// <summary>
		/// Media capabilities
		/// </summary>
		/// <remarks>optional, order 4</remarks>
		[System.Xml.Serialization.XmlElementAttribute("Media", Namespace = "http://www.onvif.org/ver10/schema")]
		public MediaCapabilities media {
			get {
				return this._media;
			}
			set {
				this._media = value;
			}
		}

		/// <summary>
		/// PTZ capabilities
		/// </summary>
		/// <remarks>optional, order 5</remarks>
		[System.Xml.Serialization.XmlElementAttribute("PTZ", Namespace = "http://www.onvif.org/ver10/schema")]
		public PTZCapabilities ptz {
			get {
				return this._ptz;
			}
			set {
				this._ptz = value;
			}
		}

		[XmlIgnore]
		public ActionEngineCapabilities actionEngine { get; set; }

		/// <remarks>optional, order 6</remarks>
		[System.Xml.Serialization.XmlElementAttribute("Extension", Namespace = "http://www.onvif.org/ver10/schema")]
		public CapabilitiesExtension extension {
			get {
				return this._extension;
			}
			set {
				this._extension = value;
			}
		}
	}

	[System.SerializableAttribute()]
	[System.Xml.Serialization.XmlTypeAttribute(TypeName = "AnalyticsCapabilities", Namespace = "http://www.onvif.org/ver10/schema")]
	public partial class AnalyticsCapabilities {

		private System.Xml.XmlAttribute[] _anyAttr;

		private string _xAddr;

		private bool _ruleSupport;

		private bool _analyticsModuleSupport;

		private System.Xml.XmlElement[] _any;

		[System.Xml.Serialization.XmlAnyAttributeAttribute()]
		public System.Xml.XmlAttribute[] anyAttr {
			get {
				return this._anyAttr;
			}
			set {
				this._anyAttr = value;
			}
		}

		/// <summary>
		/// Analytics service URI.
		/// </summary>
		/// <remarks>reqired, order 0</remarks>
		[System.Xml.Serialization.XmlElementAttribute("XAddr", Namespace = "http://www.onvif.org/ver10/schema", DataType = "anyURI")]
		public string xAddr {
			get {
				return this._xAddr;
			}
			set {
				this._xAddr = value;
			}
		}

		/// <summary>
		/// Indicates whether or not rules are supported.
		/// </summary>
		/// <remarks>reqired, order 1</remarks>
		[System.Xml.Serialization.XmlElementAttribute("RuleSupport", Namespace = "http://www.onvif.org/ver10/schema", DataType = "boolean")]
		public bool ruleSupport {
			get {
				return this._ruleSupport;
			}
			set {
				this._ruleSupport = value;
			}
		}

		/// <summary>
		/// Indicates whether or not modules are supported.
		/// </summary>
		/// <remarks>reqired, order 2</remarks>
		[System.Xml.Serialization.XmlElementAttribute("AnalyticsModuleSupport", Namespace = "http://www.onvif.org/ver10/schema", DataType = "boolean")]
		public bool analyticsModuleSupport {
			get {
				return this._analyticsModuleSupport;
			}
			set {
				this._analyticsModuleSupport = value;
			}
		}

		/// <remarks>optional, order 3, namespace ##any</remarks>
		[System.Xml.Serialization.XmlAnyElementAttribute()]
		public System.Xml.XmlElement[] any {
			get {
				return this._any;
			}
			set {
				this._any = value;
			}
		}
	}

	[System.SerializableAttribute()]
	[System.Xml.Serialization.XmlTypeAttribute(TypeName = "DeviceCapabilities", Namespace = "http://www.onvif.org/ver10/schema")]
	public partial class DeviceCapabilities {

		private System.Xml.XmlAttribute[] _anyAttr;

		private string _xAddr;

		private NetworkCapabilities _network;

		private SystemCapabilities _system;

		private IOCapabilities _io;

		private SecurityCapabilities _security;

		private DeviceCapabilitiesExtension _extension;

		[System.Xml.Serialization.XmlAnyAttributeAttribute()]
		public System.Xml.XmlAttribute[] anyAttr {
			get {
				return this._anyAttr;
			}
			set {
				this._anyAttr = value;
			}
		}

		/// <summary>
		/// Device service URI.
		/// </summary>
		/// <remarks>reqired, order 0</remarks>
		[System.Xml.Serialization.XmlElementAttribute("XAddr", Namespace = "http://www.onvif.org/ver10/schema", DataType = "anyURI")]
		public string xAddr {
			get {
				return this._xAddr;
			}
			set {
				this._xAddr = value;
			}
		}

		/// <summary>
		/// Network capabilities.
		/// </summary>
		/// <remarks>optional, order 1</remarks>
		[System.Xml.Serialization.XmlElementAttribute("Network", Namespace = "http://www.onvif.org/ver10/schema")]
		public NetworkCapabilities network {
			get {
				return this._network;
			}
			set {
				this._network = value;
			}
		}

		/// <summary>
		/// System capabilities.
		/// </summary>
		/// <remarks>optional, order 2</remarks>
		[System.Xml.Serialization.XmlElementAttribute("System", Namespace = "http://www.onvif.org/ver10/schema")]
		public SystemCapabilities system {
			get {
				return this._system;
			}
			set {
				this._system = value;
			}
		}

		/// <summary>
		/// I/O capabilities.
		/// </summary>
		/// <remarks>optional, order 3</remarks>
		[System.Xml.Serialization.XmlElementAttribute("IO", Namespace = "http://www.onvif.org/ver10/schema")]
		public IOCapabilities io {
			get {
				return this._io;
			}
			set {
				this._io = value;
			}
		}

		/// <summary>
		/// Security capabilities.
		/// </summary>
		/// <remarks>optional, order 4</remarks>
		[System.Xml.Serialization.XmlElementAttribute("Security", Namespace = "http://www.onvif.org/ver10/schema")]
		public SecurityCapabilities security {
			get {
				return this._security;
			}
			set {
				this._security = value;
			}
		}

		/// <remarks>optional, order 5</remarks>
		[System.Xml.Serialization.XmlElementAttribute("Extension", Namespace = "http://www.onvif.org/ver10/schema")]
		public DeviceCapabilitiesExtension extension {
			get {
				return this._extension;
			}
			set {
				this._extension = value;
			}
		}
	}

	[System.SerializableAttribute()]
	[System.Xml.Serialization.XmlTypeAttribute(TypeName = "NetworkCapabilities", Namespace = "http://www.onvif.org/ver10/schema")]
	public partial class NetworkCapabilities {

		private System.Xml.XmlAttribute[] _anyAttr;

		private bool _ipFilter;

		private bool _ipFilterSpecified;

		private bool _zeroConfiguration;

		private bool _zeroConfigurationSpecified;

		private bool _ipVersion6;

		private bool _ipVersion6Specified;

		private bool _dynDNS;

		private bool _dynDNSSpecified;

		private NetworkCapabilitiesExtension _extension;

		[System.Xml.Serialization.XmlAnyAttributeAttribute()]
		public System.Xml.XmlAttribute[] anyAttr {
			get {
				return this._anyAttr;
			}
			set {
				this._anyAttr = value;
			}
		}

		/// <summary>
		/// Indicates whether or not IP filtering is supported.
		/// </summary>
		/// <remarks>optional, order 0</remarks>
		[System.Xml.Serialization.XmlElementAttribute("IPFilter", Namespace = "http://www.onvif.org/ver10/schema", DataType = "boolean")]
		public bool ipFilter {
			get {
				return this._ipFilter;
			}
			set {
				this._ipFilter = value;
			}
		}

		[System.Xml.Serialization.XmlIgnoreAttribute()]
		public bool ipFilterSpecified {
			get {
				return this._ipFilterSpecified;
			}
			set {
				this._ipFilterSpecified = value;
			}
		}

		/// <summary>
		/// Indicates whether or not zeroconf is supported.
		/// </summary>
		/// <remarks>optional, order 1</remarks>
		[System.Xml.Serialization.XmlElementAttribute("ZeroConfiguration", Namespace = "http://www.onvif.org/ver10/schema", DataType = "boolean")]
		public bool zeroConfiguration {
			get {
				return this._zeroConfiguration;
			}
			set {
				this._zeroConfiguration = value;
			}
		}

		[System.Xml.Serialization.XmlIgnoreAttribute()]
		public bool zeroConfigurationSpecified {
			get {
				return this._zeroConfigurationSpecified;
			}
			set {
				this._zeroConfigurationSpecified = value;
			}
		}

		/// <summary>
		/// Indicates whether or not IPv6 is supported.
		/// </summary>
		/// <remarks>optional, order 2</remarks>
		[System.Xml.Serialization.XmlElementAttribute("IPVersion6", Namespace = "http://www.onvif.org/ver10/schema", DataType = "boolean")]
		public bool ipVersion6 {
			get {
				return this._ipVersion6;
			}
			set {
				this._ipVersion6 = value;
			}
		}

		[System.Xml.Serialization.XmlIgnoreAttribute()]
		public bool ipVersion6Specified {
			get {
				return this._ipVersion6Specified;
			}
			set {
				this._ipVersion6Specified = value;
			}
		}

		/// <summary>
		/// Indicates whether or not  is supported.
		/// </summary>
		/// <remarks>optional, order 3</remarks>
		[System.Xml.Serialization.XmlElementAttribute("DynDNS", Namespace = "http://www.onvif.org/ver10/schema", DataType = "boolean")]
		public bool dynDNS {
			get {
				return this._dynDNS;
			}
			set {
				this._dynDNS = value;
			}
		}

		[System.Xml.Serialization.XmlIgnoreAttribute()]
		public bool dynDNSSpecified {
			get {
				return this._dynDNSSpecified;
			}
			set {
				this._dynDNSSpecified = value;
			}
		}

		/// <remarks>optional, order 4</remarks>
		[System.Xml.Serialization.XmlElementAttribute("Extension", Namespace = "http://www.onvif.org/ver10/schema")]
		public NetworkCapabilitiesExtension extension {
			get {
				return this._extension;
			}
			set {
				this._extension = value;
			}
		}
	}

	[System.SerializableAttribute()]
	[System.Xml.Serialization.XmlTypeAttribute(TypeName = "NetworkCapabilitiesExtension", Namespace = "http://www.onvif.org/ver10/schema")]
	public partial class NetworkCapabilitiesExtension {

		private System.Xml.XmlElement[] _any;

		private bool _dot11Configuration;

		private bool _dot11ConfigurationSpecified;

		private NetworkCapabilitiesExtension2 _extension;

		/// <remarks>optional, order 0, namespace ##other</remarks>
		[System.Xml.Serialization.XmlAnyElementAttribute()]
		public System.Xml.XmlElement[] any {
			get {
				return this._any;
			}
			set {
				this._any = value;
			}
		}

		/// <remarks>optional, order 1</remarks>
		[System.Xml.Serialization.XmlElementAttribute("Dot11Configuration", Namespace = "http://www.onvif.org/ver10/schema", DataType = "boolean")]
		public bool dot11Configuration {
			get {
				return this._dot11Configuration;
			}
			set {
				this._dot11Configuration = value;
			}
		}

		[System.Xml.Serialization.XmlIgnoreAttribute()]
		public bool dot11ConfigurationSpecified {
			get {
				return this._dot11ConfigurationSpecified;
			}
			set {
				this._dot11ConfigurationSpecified = value;
			}
		}

		/// <remarks>optional, order 2</remarks>
		[System.Xml.Serialization.XmlElementAttribute("Extension", Namespace = "http://www.onvif.org/ver10/schema")]
		public NetworkCapabilitiesExtension2 extension {
			get {
				return this._extension;
			}
			set {
				this._extension = value;
			}
		}
	}

	[System.SerializableAttribute()]
	[System.Xml.Serialization.XmlTypeAttribute(TypeName = "NetworkCapabilitiesExtension2", Namespace = "http://www.onvif.org/ver10/schema")]
	public partial class NetworkCapabilitiesExtension2 {

		private System.Xml.XmlElement[] _any;

		/// <remarks>optional, order 0, namespace ##targetNamespace</remarks>
		[System.Xml.Serialization.XmlAnyElementAttribute(Namespace = "http://www.onvif.org/ver10/schema")]
		public System.Xml.XmlElement[] any {
			get {
				return this._any;
			}
			set {
				this._any = value;
			}
		}
	}

	[System.SerializableAttribute()]
	[System.Xml.Serialization.XmlTypeAttribute(TypeName = "SystemCapabilities", Namespace = "http://www.onvif.org/ver10/schema")]
	public partial class SystemCapabilities {

		private System.Xml.XmlAttribute[] _anyAttr;

		private bool _discoveryResolve;

		private bool _discoveryBye;

		private bool _remoteDiscovery;

		private bool _systemBackup;

		private bool _systemLogging;

		private bool _firmwareUpgrade;

		private OnvifVersion[] _supportedVersions;

		private SystemCapabilitiesExtension _extension;

		[System.Xml.Serialization.XmlAnyAttributeAttribute()]
		public System.Xml.XmlAttribute[] anyAttr {
			get {
				return this._anyAttr;
			}
			set {
				this._anyAttr = value;
			}
		}

		/// <summary>
		/// Indicates whether or not WS Discovery resolve requests are supported.
		/// </summary>
		/// <remarks>reqired, order 0</remarks>
		[System.Xml.Serialization.XmlElementAttribute("DiscoveryResolve", Namespace = "http://www.onvif.org/ver10/schema", DataType = "boolean")]
		public bool discoveryResolve {
			get {
				return this._discoveryResolve;
			}
			set {
				this._discoveryResolve = value;
			}
		}

		/// <summary>
		/// Indicates whether or not WS-Discovery Bye is supported.
		/// </summary>
		/// <remarks>reqired, order 1</remarks>
		[System.Xml.Serialization.XmlElementAttribute("DiscoveryBye", Namespace = "http://www.onvif.org/ver10/schema", DataType = "boolean")]
		public bool discoveryBye {
			get {
				return this._discoveryBye;
			}
			set {
				this._discoveryBye = value;
			}
		}

		/// <summary>
		/// Indicates whether or not remote discovery, see WS-Discovery, is supported.
		/// </summary>
		/// <remarks>reqired, order 2</remarks>
		[System.Xml.Serialization.XmlElementAttribute("RemoteDiscovery", Namespace = "http://www.onvif.org/ver10/schema", DataType = "boolean")]
		public bool remoteDiscovery {
			get {
				return this._remoteDiscovery;
			}
			set {
				this._remoteDiscovery = value;
			}
		}

		/// <summary>
		/// Indicates whether or not system backup is supported.
		/// </summary>
		/// <remarks>reqired, order 3</remarks>
		[System.Xml.Serialization.XmlElementAttribute("SystemBackup", Namespace = "http://www.onvif.org/ver10/schema", DataType = "boolean")]
		public bool systemBackup {
			get {
				return this._systemBackup;
			}
			set {
				this._systemBackup = value;
			}
		}

		/// <summary>
		/// Indicates whether or not system logging is supported.
		/// </summary>
		/// <remarks>reqired, order 4</remarks>
		[System.Xml.Serialization.XmlElementAttribute("SystemLogging", Namespace = "http://www.onvif.org/ver10/schema", DataType = "boolean")]
		public bool systemLogging {
			get {
				return this._systemLogging;
			}
			set {
				this._systemLogging = value;
			}
		}

		/// <summary>
		/// Indicates whether or not firmware upgrade is supported.
		/// </summary>
		/// <remarks>reqired, order 5</remarks>
		[System.Xml.Serialization.XmlElementAttribute("FirmwareUpgrade", Namespace = "http://www.onvif.org/ver10/schema", DataType = "boolean")]
		public bool firmwareUpgrade {
			get {
				return this._firmwareUpgrade;
			}
			set {
				this._firmwareUpgrade = value;
			}
		}

		/// <summary>
		/// Indicates supported ONVIF version(s).
		/// </summary>
		/// <remarks>reqired, order 6</remarks>
		[System.Xml.Serialization.XmlElementAttribute("SupportedVersions", Namespace = "http://www.onvif.org/ver10/schema")]
		public OnvifVersion[] supportedVersions {
			get {
				return this._supportedVersions;
			}
			set {
				this._supportedVersions = value;
			}
		}

		/// <remarks>optional, order 7</remarks>
		[System.Xml.Serialization.XmlElementAttribute("Extension", Namespace = "http://www.onvif.org/ver10/schema")]
		public SystemCapabilitiesExtension extension {
			get {
				return this._extension;
			}
			set {
				this._extension = value;
			}
		}
	}

	[System.SerializableAttribute()]
	[System.Xml.Serialization.XmlTypeAttribute(TypeName = "OnvifVersion", Namespace = "http://www.onvif.org/ver10/schema")]
	public partial class OnvifVersion {

		private int _major;

		private int _minor;

		/// <summary>
		/// Major version number.
		/// </summary>
		/// <remarks>reqired, order 0</remarks>
		[System.Xml.Serialization.XmlElementAttribute("Major", Namespace = "http://www.onvif.org/ver10/schema", DataType = "int")]
		public int major {
			get {
				return this._major;
			}
			set {
				this._major = value;
			}
		}

		/// <summary>
		/// Two digit minor version number (e.g. 1 maps to "01" and 20 maps to "20").
		/// </summary>
		/// <remarks>reqired, order 1</remarks>
		[System.Xml.Serialization.XmlElementAttribute("Minor", Namespace = "http://www.onvif.org/ver10/schema", DataType = "int")]
		public int minor {
			get {
				return this._minor;
			}
			set {
				this._minor = value;
			}
		}
	}


	[System.SerializableAttribute()]
	[System.Xml.Serialization.XmlTypeAttribute(TypeName = "SystemCapabilitiesExtension", Namespace = "http://www.onvif.org/ver10/schema")]
	public partial class SystemCapabilitiesExtension {

		private System.Xml.XmlElement[] _any;

		private bool _httpFirmwareUpgrade;

		private bool _httpFirmwareUpgradeSpecified;

		private bool _httpSystemBackup;

		private bool _httpSystemBackupSpecified;

		private bool _httpSystemLogging;

		private bool _httpSystemLoggingSpecified;

		private bool _httpSupportInformation;

		private bool _httpSupportInformationSpecified;

		private SystemCapabilitiesExtension2 _extension;

		/// <remarks>optional, order 0, namespace ##other</remarks>
		[System.Xml.Serialization.XmlAnyElementAttribute()]
		public System.Xml.XmlElement[] any {
			get {
				return this._any;
			}
			set {
				this._any = value;
			}
		}

		/// <remarks>optional, order 1</remarks>
		[System.Xml.Serialization.XmlElementAttribute("HttpFirmwareUpgrade", Namespace = "http://www.onvif.org/ver10/schema", DataType = "boolean")]
		public bool httpFirmwareUpgrade {
			get {
				return this._httpFirmwareUpgrade;
			}
			set {
				this._httpFirmwareUpgrade = value;
			}
		}

		[System.Xml.Serialization.XmlIgnoreAttribute()]
		public bool httpFirmwareUpgradeSpecified {
			get {
				return this._httpFirmwareUpgradeSpecified;
			}
			set {
				this._httpFirmwareUpgradeSpecified = value;
			}
		}

		/// <remarks>optional, order 2</remarks>
		[System.Xml.Serialization.XmlElementAttribute("HttpSystemBackup", Namespace = "http://www.onvif.org/ver10/schema", DataType = "boolean")]
		public bool httpSystemBackup {
			get {
				return this._httpSystemBackup;
			}
			set {
				this._httpSystemBackup = value;
			}
		}

		[System.Xml.Serialization.XmlIgnoreAttribute()]
		public bool httpSystemBackupSpecified {
			get {
				return this._httpSystemBackupSpecified;
			}
			set {
				this._httpSystemBackupSpecified = value;
			}
		}

		/// <remarks>optional, order 3</remarks>
		[System.Xml.Serialization.XmlElementAttribute("HttpSystemLogging", Namespace = "http://www.onvif.org/ver10/schema", DataType = "boolean")]
		public bool httpSystemLogging {
			get {
				return this._httpSystemLogging;
			}
			set {
				this._httpSystemLogging = value;
			}
		}

		[System.Xml.Serialization.XmlIgnoreAttribute()]
		public bool httpSystemLoggingSpecified {
			get {
				return this._httpSystemLoggingSpecified;
			}
			set {
				this._httpSystemLoggingSpecified = value;
			}
		}

		/// <remarks>optional, order 4</remarks>
		[System.Xml.Serialization.XmlElementAttribute("HttpSupportInformation", Namespace = "http://www.onvif.org/ver10/schema", DataType = "boolean")]
		public bool httpSupportInformation {
			get {
				return this._httpSupportInformation;
			}
			set {
				this._httpSupportInformation = value;
			}
		}

		[System.Xml.Serialization.XmlIgnoreAttribute()]
		public bool httpSupportInformationSpecified {
			get {
				return this._httpSupportInformationSpecified;
			}
			set {
				this._httpSupportInformationSpecified = value;
			}
		}

		/// <remarks>optional, order 5</remarks>
		[System.Xml.Serialization.XmlElementAttribute("Extension", Namespace = "http://www.onvif.org/ver10/schema")]
		public SystemCapabilitiesExtension2 extension {
			get {
				return this._extension;
			}
			set {
				this._extension = value;
			}
		}
	}

	[System.SerializableAttribute()]
	[System.Xml.Serialization.XmlTypeAttribute(TypeName = "SystemCapabilitiesExtension2", Namespace = "http://www.onvif.org/ver10/schema")]
	public partial class SystemCapabilitiesExtension2 {

		private System.Xml.XmlElement[] _any;

		/// <remarks>optional, order 0, namespace ##any</remarks>
		[System.Xml.Serialization.XmlAnyElementAttribute()]
		public System.Xml.XmlElement[] any {
			get {
				return this._any;
			}
			set {
				this._any = value;
			}
		}
	}

	[System.SerializableAttribute()]
	[System.Xml.Serialization.XmlTypeAttribute(TypeName = "IOCapabilities", Namespace = "http://www.onvif.org/ver10/schema")]
	public partial class IOCapabilities {

		private System.Xml.XmlAttribute[] _anyAttr;

		private int _inputConnectors;

		private bool _inputConnectorsSpecified;

		private int _relayOutputs;

		private bool _relayOutputsSpecified;

		private IOCapabilitiesExtension _extension;

		[System.Xml.Serialization.XmlAnyAttributeAttribute()]
		public System.Xml.XmlAttribute[] anyAttr {
			get {
				return this._anyAttr;
			}
			set {
				this._anyAttr = value;
			}
		}

		/// <summary>
		/// Number of input connectors.
		/// </summary>
		/// <remarks>optional, order 0</remarks>
		[System.Xml.Serialization.XmlElementAttribute("InputConnectors", Namespace = "http://www.onvif.org/ver10/schema", DataType = "int")]
		public int inputConnectors {
			get {
				return this._inputConnectors;
			}
			set {
				this._inputConnectors = value;
			}
		}

		[System.Xml.Serialization.XmlIgnoreAttribute()]
		public bool inputConnectorsSpecified {
			get {
				return this._inputConnectorsSpecified;
			}
			set {
				this._inputConnectorsSpecified = value;
			}
		}

		/// <summary>
		/// Number of relay outputs.
		/// </summary>
		/// <remarks>optional, order 1</remarks>
		[System.Xml.Serialization.XmlElementAttribute("RelayOutputs", Namespace = "http://www.onvif.org/ver10/schema", DataType = "int")]
		public int relayOutputs {
			get {
				return this._relayOutputs;
			}
			set {
				this._relayOutputs = value;
			}
		}

		[System.Xml.Serialization.XmlIgnoreAttribute()]
		public bool relayOutputsSpecified {
			get {
				return this._relayOutputsSpecified;
			}
			set {
				this._relayOutputsSpecified = value;
			}
		}

		/// <remarks>optional, order 2</remarks>
		[System.Xml.Serialization.XmlElementAttribute("Extension", Namespace = "http://www.onvif.org/ver10/schema")]
		public IOCapabilitiesExtension extension {
			get {
				return this._extension;
			}
			set {
				this._extension = value;
			}
		}
	}

	[System.SerializableAttribute()]
	[System.Xml.Serialization.XmlTypeAttribute(TypeName = "IOCapabilitiesExtension", Namespace = "http://www.onvif.org/ver10/schema")]
	public partial class IOCapabilitiesExtension {

		private System.Xml.XmlAttribute[] _anyAttr;

		private System.Xml.XmlElement[] _any;

		private bool _auxiliary;

		private bool _auxiliarySpecified;

		private string[] _auxiliaryCommands;

		private IOCapabilitiesExtension2 _extension;

		[System.Xml.Serialization.XmlAnyAttributeAttribute()]
		public System.Xml.XmlAttribute[] anyAttr {
			get {
				return this._anyAttr;
			}
			set {
				this._anyAttr = value;
			}
		}

		/// <remarks>optional, order 0, namespace ##other</remarks>
		[System.Xml.Serialization.XmlAnyElementAttribute()]
		public System.Xml.XmlElement[] any {
			get {
				return this._any;
			}
			set {
				this._any = value;
			}
		}

		/// <remarks>optional, order 1</remarks>
		[System.Xml.Serialization.XmlElementAttribute("Auxiliary", Namespace = "http://www.onvif.org/ver10/schema", DataType = "boolean")]
		public bool auxiliary {
			get {
				return this._auxiliary;
			}
			set {
				this._auxiliary = value;
			}
		}

		[System.Xml.Serialization.XmlIgnoreAttribute()]
		public bool auxiliarySpecified {
			get {
				return this._auxiliarySpecified;
			}
			set {
				this._auxiliarySpecified = value;
			}
		}

		/// <remarks>optional, order 2</remarks>
		[System.Xml.Serialization.XmlElementAttribute("AuxiliaryCommands", Namespace = "http://www.onvif.org/ver10/schema", DataType = "string")]
		public string[] auxiliaryCommands {
			get {
				return this._auxiliaryCommands;
			}
			set {
				this._auxiliaryCommands = value;
			}
		}

		/// <remarks>reqired, order 3</remarks>
		[System.Xml.Serialization.XmlElementAttribute("Extension", Namespace = "http://www.onvif.org/ver10/schema")]
		public IOCapabilitiesExtension2 extension {
			get {
				return this._extension;
			}
			set {
				this._extension = value;
			}
		}
	}

	[System.SerializableAttribute()]
	[System.Xml.Serialization.XmlTypeAttribute(TypeName = "IOCapabilitiesExtension2", Namespace = "http://www.onvif.org/ver10/schema")]
	public partial class IOCapabilitiesExtension2 {

		private System.Xml.XmlElement[] _any;

		/// <remarks>optional, order 0, namespace ##targetNamespace</remarks>
		[System.Xml.Serialization.XmlAnyElementAttribute(Namespace = "http://www.onvif.org/ver10/schema")]
		public System.Xml.XmlElement[] any {
			get {
				return this._any;
			}
			set {
				this._any = value;
			}
		}
	}

	[System.SerializableAttribute()]
	[System.Xml.Serialization.XmlTypeAttribute(TypeName = "SecurityCapabilities", Namespace = "http://www.onvif.org/ver10/schema")]
	public partial class SecurityCapabilities {

		private System.Xml.XmlAttribute[] _anyAttr;

		private bool _tls11;

		private bool _tls12;

		private bool _onboardKeyGeneration;

		private bool _accessPolicyConfig;

		private bool _x509Token;

		private bool _samlToken;

		private bool _kerberosToken;

		private bool _relToken;

		private System.Xml.XmlElement[] _any;

		private SecurityCapabilitiesExtension _extension;

		[System.Xml.Serialization.XmlAnyAttributeAttribute()]
		public System.Xml.XmlAttribute[] anyAttr {
			get {
				return this._anyAttr;
			}
			set {
				this._anyAttr = value;
			}
		}

		/// <summary>
		/// Indicates whether or not TLS 1.1 is supported.
		/// </summary>
		/// <remarks>reqired, order 0</remarks>
		[System.Xml.Serialization.XmlElementAttribute("TLS1.1", Namespace = "http://www.onvif.org/ver10/schema", DataType = "boolean")]
		public bool tls11 {
			get {
				return this._tls11;
			}
			set {
				this._tls11 = value;
			}
		}

		/// <summary>
		/// Indicates whether or not TLS 1.2 is supported.
		/// </summary>
		/// <remarks>reqired, order 1</remarks>
		[System.Xml.Serialization.XmlElementAttribute("TLS1.2", Namespace = "http://www.onvif.org/ver10/schema", DataType = "boolean")]
		public bool tls12 {
			get {
				return this._tls12;
			}
			set {
				this._tls12 = value;
			}
		}

		/// <summary>
		/// Indicates whether or not onboard key generation is supported.
		/// </summary>
		/// <remarks>reqired, order 2</remarks>
		[System.Xml.Serialization.XmlElementAttribute("OnboardKeyGeneration", Namespace = "http://www.onvif.org/ver10/schema", DataType = "boolean")]
		public bool onboardKeyGeneration {
			get {
				return this._onboardKeyGeneration;
			}
			set {
				this._onboardKeyGeneration = value;
			}
		}

		/// <summary>
		/// Indicates whether or not access policy configuration is supported.
		/// </summary>
		/// <remarks>reqired, order 3</remarks>
		[System.Xml.Serialization.XmlElementAttribute("AccessPolicyConfig", Namespace = "http://www.onvif.org/ver10/schema", DataType = "boolean")]
		public bool accessPolicyConfig {
			get {
				return this._accessPolicyConfig;
			}
			set {
				this._accessPolicyConfig = value;
			}
		}

		/// <summary>
		/// Indicates whether or not WS-Security X.509 token is supported.
		/// </summary>
		/// <remarks>reqired, order 4</remarks>
		[System.Xml.Serialization.XmlElementAttribute("X.509Token", Namespace = "http://www.onvif.org/ver10/schema", DataType = "boolean")]
		public bool x509Token {
			get {
				return this._x509Token;
			}
			set {
				this._x509Token = value;
			}
		}

		/// <summary>
		/// Indicates whether or not WS-Security SAML token is supported.
		/// </summary>
		/// <remarks>reqired, order 5</remarks>
		[System.Xml.Serialization.XmlElementAttribute("SAMLToken", Namespace = "http://www.onvif.org/ver10/schema", DataType = "boolean")]
		public bool samlToken {
			get {
				return this._samlToken;
			}
			set {
				this._samlToken = value;
			}
		}

		/// <summary>
		/// Indicates whether or not WS-Security Kerberos token is supported.
		/// </summary>
		/// <remarks>reqired, order 6</remarks>
		[System.Xml.Serialization.XmlElementAttribute("KerberosToken", Namespace = "http://www.onvif.org/ver10/schema", DataType = "boolean")]
		public bool kerberosToken {
			get {
				return this._kerberosToken;
			}
			set {
				this._kerberosToken = value;
			}
		}

		/// <summary>
		/// Indicates whether or not WS-Security REL token is supported.
		/// </summary>
		/// <remarks>reqired, order 7</remarks>
		[System.Xml.Serialization.XmlElementAttribute("RELToken", Namespace = "http://www.onvif.org/ver10/schema", DataType = "boolean")]
		public bool relToken {
			get {
				return this._relToken;
			}
			set {
				this._relToken = value;
			}
		}

		/// <remarks>optional, order 8, namespace ##other</remarks>
		[System.Xml.Serialization.XmlAnyElementAttribute()]
		public System.Xml.XmlElement[] any {
			get {
				return this._any;
			}
			set {
				this._any = value;
			}
		}

		/// <remarks>optional, order 9</remarks>
		[System.Xml.Serialization.XmlElementAttribute("Extension", Namespace = "http://www.onvif.org/ver10/schema")]
		public SecurityCapabilitiesExtension extension {
			get {
				return this._extension;
			}
			set {
				this._extension = value;
			}
		}
	}

	[System.SerializableAttribute()]
	[System.Xml.Serialization.XmlTypeAttribute(TypeName = "SecurityCapabilitiesExtension", Namespace = "http://www.onvif.org/ver10/schema")]
	public partial class SecurityCapabilitiesExtension {

		private bool _tls10;

		private SecurityCapabilitiesExtension2 _extension;

		/// <remarks>reqired, order 0</remarks>
		[System.Xml.Serialization.XmlElementAttribute("TLS1.0", Namespace = "http://www.onvif.org/ver10/schema", DataType = "boolean")]
		public bool tls10 {
			get {
				return this._tls10;
			}
			set {
				this._tls10 = value;
			}
		}

		/// <remarks>optional, order 1</remarks>
		[System.Xml.Serialization.XmlElementAttribute("Extension", Namespace = "http://www.onvif.org/ver10/schema")]
		public SecurityCapabilitiesExtension2 extension {
			get {
				return this._extension;
			}
			set {
				this._extension = value;
			}
		}
	}

	[System.SerializableAttribute()]
	[System.Xml.Serialization.XmlTypeAttribute(TypeName = "SecurityCapabilitiesExtension2", Namespace = "http://www.onvif.org/ver10/schema")]
	public partial class SecurityCapabilitiesExtension2 {

		private bool _dot1X;

		private int[] _supportedEAPMethod;

		private bool _remoteUserHandling;

		private System.Xml.XmlElement[] _any;

		/// <remarks>reqired, order 0</remarks>
		[System.Xml.Serialization.XmlElementAttribute("Dot1X", Namespace = "http://www.onvif.org/ver10/schema", DataType = "boolean")]
		public bool dot1X {
			get {
				return this._dot1X;
			}
			set {
				this._dot1X = value;
			}
		}

		/// <summary>
		/// EAP Methods supported by the device. The int values refer to the
		/// .
		/// </summary>
		/// <remarks>optional, order 1</remarks>
		[System.Xml.Serialization.XmlElementAttribute("SupportedEAPMethod", Namespace = "http://www.onvif.org/ver10/schema", DataType = "int")]
		public int[] supportedEAPMethod {
			get {
				return this._supportedEAPMethod;
			}
			set {
				this._supportedEAPMethod = value;
			}
		}

		/// <remarks>reqired, order 2</remarks>
		[System.Xml.Serialization.XmlElementAttribute("RemoteUserHandling", Namespace = "http://www.onvif.org/ver10/schema", DataType = "boolean")]
		public bool remoteUserHandling {
			get {
				return this._remoteUserHandling;
			}
			set {
				this._remoteUserHandling = value;
			}
		}

		/// <remarks>optional, order 3, namespace ##targetNamespace</remarks>
		[System.Xml.Serialization.XmlAnyElementAttribute(Namespace = "http://www.onvif.org/ver10/schema")]
		public System.Xml.XmlElement[] any {
			get {
				return this._any;
			}
			set {
				this._any = value;
			}
		}
	}

	[System.SerializableAttribute()]
	[System.Xml.Serialization.XmlTypeAttribute(TypeName = "DeviceCapabilitiesExtension", Namespace = "http://www.onvif.org/ver10/schema")]
	public partial class DeviceCapabilitiesExtension {

		private System.Xml.XmlElement[] _any;

		/// <remarks>optional, order 0, namespace ##any</remarks>
		[System.Xml.Serialization.XmlAnyElementAttribute()]
		public System.Xml.XmlElement[] any {
			get {
				return this._any;
			}
			set {
				this._any = value;
			}
		}
	}

	[System.SerializableAttribute()]
	[System.Xml.Serialization.XmlTypeAttribute(TypeName = "EventCapabilities", Namespace = "http://www.onvif.org/ver10/schema")]
	public partial class EventCapabilities {

		private System.Xml.XmlAttribute[] _anyAttr;

		private string _xAddr;

		private bool _wsSubscriptionPolicySupport;

		private bool _wsPullPointSupport;

		private bool _wsPausableSubscriptionManagerInterfaceSupport;

		private System.Xml.XmlElement[] _any;

		[System.Xml.Serialization.XmlAnyAttributeAttribute()]
		public System.Xml.XmlAttribute[] anyAttr {
			get {
				return this._anyAttr;
			}
			set {
				this._anyAttr = value;
			}
		}

		/// <summary>
		/// Event service URI.
		/// </summary>
		/// <remarks>reqired, order 0</remarks>
		[System.Xml.Serialization.XmlElementAttribute("XAddr", Namespace = "http://www.onvif.org/ver10/schema", DataType = "anyURI")]
		public string xAddr {
			get {
				return this._xAddr;
			}
			set {
				this._xAddr = value;
			}
		}

		/// <summary>
		/// Indicates whether or not WS Subscription policy is supported.
		/// </summary>
		/// <remarks>reqired, order 1</remarks>
		[System.Xml.Serialization.XmlElementAttribute("WSSubscriptionPolicySupport", Namespace = "http://www.onvif.org/ver10/schema", DataType = "boolean")]
		public bool wsSubscriptionPolicySupport {
			get {
				return this._wsSubscriptionPolicySupport;
			}
			set {
				this._wsSubscriptionPolicySupport = value;
			}
		}

		/// <summary>
		/// Indicates whether or not WS Pull Point is supported.
		/// </summary>
		/// <remarks>reqired, order 2</remarks>
		[System.Xml.Serialization.XmlElementAttribute("WSPullPointSupport", Namespace = "http://www.onvif.org/ver10/schema", DataType = "boolean")]
		public bool wsPullPointSupport {
			get {
				return this._wsPullPointSupport;
			}
			set {
				this._wsPullPointSupport = value;
			}
		}

		/// <summary>
		/// Indicates whether or not WS Pausable Subscription Manager Interface is supported.
		/// </summary>
		/// <remarks>reqired, order 3</remarks>
		[System.Xml.Serialization.XmlElementAttribute("WSPausableSubscriptionManagerInterfaceSupport", Namespace = "http://www.onvif.org/ver10/schema", DataType = "boolean")]
		public bool wsPausableSubscriptionManagerInterfaceSupport {
			get {
				return this._wsPausableSubscriptionManagerInterfaceSupport;
			}
			set {
				this._wsPausableSubscriptionManagerInterfaceSupport = value;
			}
		}

		/// <remarks>optional, order 4, namespace ##any</remarks>
		[System.Xml.Serialization.XmlAnyElementAttribute()]
		public System.Xml.XmlElement[] any {
			get {
				return this._any;
			}
			set {
				this._any = value;
			}
		}
	}

	[System.SerializableAttribute()]
	[System.Xml.Serialization.XmlTypeAttribute(TypeName = "ImagingCapabilities", Namespace = "http://www.onvif.org/ver10/schema")]
	public partial class ImagingCapabilities {

		private System.Xml.XmlAttribute[] _anyAttr;

		private string _xAddr;

		[System.Xml.Serialization.XmlAnyAttributeAttribute()]
		public System.Xml.XmlAttribute[] anyAttr {
			get {
				return this._anyAttr;
			}
			set {
				this._anyAttr = value;
			}
		}

		/// <summary>
		/// Imaging service URI.
		/// </summary>
		/// <remarks>reqired, order 0</remarks>
		[System.Xml.Serialization.XmlElementAttribute("XAddr", Namespace = "http://www.onvif.org/ver10/schema", DataType = "anyURI")]
		public string xAddr {
			get {
				return this._xAddr;
			}
			set {
				this._xAddr = value;
			}
		}
	}

	[System.SerializableAttribute()]
	[System.Xml.Serialization.XmlTypeAttribute(TypeName = "MediaCapabilities", Namespace = "http://www.onvif.org/ver10/schema")]
	public partial class MediaCapabilities {

		private System.Xml.XmlAttribute[] _anyAttr;

		private string _xAddr;

		private RealTimeStreamingCapabilities _streamingCapabilities;

		private System.Xml.XmlElement[] _any;

		private MediaCapabilitiesExtension _extension;

		[System.Xml.Serialization.XmlAnyAttributeAttribute()]
		public System.Xml.XmlAttribute[] anyAttr {
			get {
				return this._anyAttr;
			}
			set {
				this._anyAttr = value;
			}
		}

		/// <summary>
		/// Media service URI.
		/// </summary>
		/// <remarks>reqired, order 0</remarks>
		[System.Xml.Serialization.XmlElementAttribute("XAddr", Namespace = "http://www.onvif.org/ver10/schema", DataType = "anyURI")]
		public string xAddr {
			get {
				return this._xAddr;
			}
			set {
				this._xAddr = value;
			}
		}

		/// <summary>
		/// Streaming capabilities.
		/// </summary>
		/// <remarks>reqired, order 1</remarks>
		[System.Xml.Serialization.XmlElementAttribute("StreamingCapabilities", Namespace = "http://www.onvif.org/ver10/schema")]
		public RealTimeStreamingCapabilities streamingCapabilities {
			get {
				return this._streamingCapabilities;
			}
			set {
				this._streamingCapabilities = value;
			}
		}

		/// <remarks>optional, order 2, namespace ##other</remarks>
		[System.Xml.Serialization.XmlAnyElementAttribute()]
		public System.Xml.XmlElement[] any {
			get {
				return this._any;
			}
			set {
				this._any = value;
			}
		}

		/// <remarks>optional, order 3</remarks>
		[System.Xml.Serialization.XmlElementAttribute("Extension", Namespace = "http://www.onvif.org/ver10/schema")]
		public MediaCapabilitiesExtension extension {
			get {
				return this._extension;
			}
			set {
				this._extension = value;
			}
		}
	}

	[System.SerializableAttribute()]
	[System.Xml.Serialization.XmlTypeAttribute(TypeName = "RealTimeStreamingCapabilities", Namespace = "http://www.onvif.org/ver10/schema")]
	public partial class RealTimeStreamingCapabilities {

		private System.Xml.XmlAttribute[] _anyAttr;

		private bool _rtpMulticast;

		private bool _rtpMulticastSpecified;

		private bool _rtp_tcp;

		private bool _rtp_tcpSpecified;

		private bool _rtp_rtsp_tcp;

		private bool _rtp_rtsp_tcpSpecified;

		private RealTimeStreamingCapabilitiesExtension _extension;

		[System.Xml.Serialization.XmlAnyAttributeAttribute()]
		public System.Xml.XmlAttribute[] anyAttr {
			get {
				return this._anyAttr;
			}
			set {
				this._anyAttr = value;
			}
		}

		/// <summary>
		/// Indicates whether or not RTP multicast is supported.
		/// </summary>
		/// <remarks>optional, order 0</remarks>
		[System.Xml.Serialization.XmlElementAttribute("RTPMulticast", Namespace = "http://www.onvif.org/ver10/schema", DataType = "boolean")]
		public bool rtpMulticast {
			get {
				return this._rtpMulticast;
			}
			set {
				this._rtpMulticast = value;
			}
		}

		[System.Xml.Serialization.XmlIgnoreAttribute()]
		public bool rtpMulticastSpecified {
			get {
				return this._rtpMulticastSpecified;
			}
			set {
				this._rtpMulticastSpecified = value;
			}
		}

		/// <summary>
		/// Indicates whether or not RTP over TCP is supported.
		/// </summary>
		/// <remarks>optional, order 1</remarks>
		[System.Xml.Serialization.XmlElementAttribute("RTP_TCP", Namespace = "http://www.onvif.org/ver10/schema", DataType = "boolean")]
		public bool rtp_tcp {
			get {
				return this._rtp_tcp;
			}
			set {
				this._rtp_tcp = value;
			}
		}

		[System.Xml.Serialization.XmlIgnoreAttribute()]
		public bool rtp_tcpSpecified {
			get {
				return this._rtp_tcpSpecified;
			}
			set {
				this._rtp_tcpSpecified = value;
			}
		}

		/// <summary>
		/// Indicates whether or not RTP/RTSP/TCP is supported.
		/// </summary>
		/// <remarks>optional, order 2</remarks>
		[System.Xml.Serialization.XmlElementAttribute("RTP_RTSP_TCP", Namespace = "http://www.onvif.org/ver10/schema", DataType = "boolean")]
		public bool rtp_rtsp_tcp {
			get {
				return this._rtp_rtsp_tcp;
			}
			set {
				this._rtp_rtsp_tcp = value;
			}
		}

		[System.Xml.Serialization.XmlIgnoreAttribute()]
		public bool rtp_rtsp_tcpSpecified {
			get {
				return this._rtp_rtsp_tcpSpecified;
			}
			set {
				this._rtp_rtsp_tcpSpecified = value;
			}
		}

		/// <remarks>optional, order 3</remarks>
		[System.Xml.Serialization.XmlElementAttribute("Extension", Namespace = "http://www.onvif.org/ver10/schema")]
		public RealTimeStreamingCapabilitiesExtension extension {
			get {
				return this._extension;
			}
			set {
				this._extension = value;
			}
		}
	}

	[System.SerializableAttribute()]
	[System.Xml.Serialization.XmlTypeAttribute(TypeName = "RealTimeStreamingCapabilitiesExtension", Namespace = "http://www.onvif.org/ver10/schema")]
	public partial class RealTimeStreamingCapabilitiesExtension {

		private System.Xml.XmlElement[] _any;

		/// <remarks>optional, order 0, namespace ##any</remarks>
		[System.Xml.Serialization.XmlAnyElementAttribute()]
		public System.Xml.XmlElement[] any {
			get {
				return this._any;
			}
			set {
				this._any = value;
			}
		}
	}

	[System.SerializableAttribute()]
	[System.Xml.Serialization.XmlTypeAttribute(TypeName = "MediaCapabilitiesExtension", Namespace = "http://www.onvif.org/ver10/schema")]
	public partial class MediaCapabilitiesExtension {

		private System.Xml.XmlAttribute[] _anyAttr;

		private ProfileCapabilities _profileCapabilities;

		private System.Xml.XmlElement[] _any;

		[System.Xml.Serialization.XmlAnyAttributeAttribute()]
		public System.Xml.XmlAttribute[] anyAttr {
			get {
				return this._anyAttr;
			}
			set {
				this._anyAttr = value;
			}
		}

		/// <remarks>reqired, order 0</remarks>
		[System.Xml.Serialization.XmlElementAttribute("ProfileCapabilities", Namespace = "http://www.onvif.org/ver10/schema")]
		public ProfileCapabilities profileCapabilities {
			get {
				return this._profileCapabilities;
			}
			set {
				this._profileCapabilities = value;
			}
		}

		/// <remarks>optional, order 1, namespace ##targetNamespace</remarks>
		[System.Xml.Serialization.XmlAnyElementAttribute(Namespace = "http://www.onvif.org/ver10/schema")]
		public System.Xml.XmlElement[] any {
			get {
				return this._any;
			}
			set {
				this._any = value;
			}
		}
	}

	[System.SerializableAttribute()]
	[System.Xml.Serialization.XmlTypeAttribute(TypeName = "ProfileCapabilities", Namespace = "http://www.onvif.org/ver10/schema")]
	public partial class ProfileCapabilities {

		private System.Xml.XmlAttribute[] _anyAttr;

		private int _maximumNumberOfProfiles;

		private System.Xml.XmlElement[] _any;

		[System.Xml.Serialization.XmlAnyAttributeAttribute()]
		public System.Xml.XmlAttribute[] anyAttr {
			get {
				return this._anyAttr;
			}
			set {
				this._anyAttr = value;
			}
		}

		/// <summary>
		/// Maximum number of profiles.
		/// </summary>
		/// <remarks>reqired, order 0</remarks>
		[System.Xml.Serialization.XmlElementAttribute("MaximumNumberOfProfiles", Namespace = "http://www.onvif.org/ver10/schema", DataType = "int")]
		public int maximumNumberOfProfiles {
			get {
				return this._maximumNumberOfProfiles;
			}
			set {
				this._maximumNumberOfProfiles = value;
			}
		}

		/// <remarks>optional, order 1, namespace ##any</remarks>
		[System.Xml.Serialization.XmlAnyElementAttribute()]
		public System.Xml.XmlElement[] any {
			get {
				return this._any;
			}
			set {
				this._any = value;
			}
		}
	}

	/// <remarks/>
	[System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.0.30319.17929")]
	[System.SerializableAttribute()]
	[System.Diagnostics.DebuggerStepThroughAttribute()]
	[System.ComponentModel.DesignerCategoryAttribute("code")]
	[System.Xml.Serialization.XmlTypeAttribute(TypeName = "ProfileCapabilities", Namespace = "http://www.onvif.org/ver10/media/wsdl")]
	public partial class ProfileCapabilities1 {

		private System.Xml.XmlAttribute[] anyAttrField;

		private System.Xml.XmlElement[] anyField;

		private int maximumNumberOfProfilesField;

		private bool maximumNumberOfProfilesFieldSpecified;

		/// <remarks/>
		[System.Xml.Serialization.XmlAnyAttributeAttribute()]
		public System.Xml.XmlAttribute[] AnyAttr {
			get {
				return this.anyAttrField;
			}
			set {
				this.anyAttrField = value;
			}
		}

		/// <remarks/>
		[System.Xml.Serialization.XmlAnyElementAttribute(Order = 0)]
		public System.Xml.XmlElement[] Any {
			get {
				return this.anyField;
			}
			set {
				this.anyField = value;
			}
		}

		/// <remarks/>
		[System.Xml.Serialization.XmlAttributeAttribute()]
		public int MaximumNumberOfProfiles {
			get {
				return this.maximumNumberOfProfilesField;
			}
			set {
				this.maximumNumberOfProfilesField = value;
			}
		}

		/// <remarks/>
		[System.Xml.Serialization.XmlIgnoreAttribute()]
		public bool MaximumNumberOfProfilesSpecified {
			get {
				return this.maximumNumberOfProfilesFieldSpecified;
			}
			set {
				this.maximumNumberOfProfilesFieldSpecified = value;
			}
		}
	}

	[System.SerializableAttribute()]
	[System.Xml.Serialization.XmlTypeAttribute(TypeName = "PTZCapabilities", Namespace = "http://www.onvif.org/ver10/schema")]
	public partial class PTZCapabilities {

		private System.Xml.XmlAttribute[] _anyAttr;

		private string _xAddr;

		private System.Xml.XmlElement[] _any;

		[System.Xml.Serialization.XmlAnyAttributeAttribute()]
		public System.Xml.XmlAttribute[] anyAttr {
			get {
				return this._anyAttr;
			}
			set {
				this._anyAttr = value;
			}
		}

		/// <summary>
		/// PTZ service URI.
		/// </summary>
		/// <remarks>reqired, order 0</remarks>
		[System.Xml.Serialization.XmlElementAttribute("XAddr", Namespace = "http://www.onvif.org/ver10/schema", DataType = "anyURI")]
		public string xAddr {
			get {
				return this._xAddr;
			}
			set {
				this._xAddr = value;
			}
		}

		/// <remarks>optional, order 1, namespace ##any</remarks>
		[System.Xml.Serialization.XmlAnyElementAttribute()]
		public System.Xml.XmlElement[] any {
			get {
				return this._any;
			}
			set {
				this._any = value;
			}
		}
	}

	[System.SerializableAttribute()]
	[System.Xml.Serialization.XmlTypeAttribute(TypeName = "CapabilitiesExtension", Namespace = "http://www.onvif.org/ver10/schema")]
	public partial class CapabilitiesExtension {

		private System.Xml.XmlElement[] _any;

		private DeviceIOCapabilities _deviceIO;

		private DisplayCapabilities _display;

		private RecordingCapabilities _recording;

		private SearchCapabilities _search;

		private ReplayCapabilities _replay;

		private ReceiverCapabilities _receiver;

		private AnalyticsDeviceCapabilities _analyticsDevice;

		private CapabilitiesExtension2 _extensions;

		/// <remarks>optional, order 0, namespace ##other</remarks>
		[System.Xml.Serialization.XmlAnyElementAttribute()]
		public System.Xml.XmlElement[] any {
			get {
				return this._any;
			}
			set {
				this._any = value;
			}
		}

		/// <remarks>optional, order 1</remarks>
		[System.Xml.Serialization.XmlElementAttribute("DeviceIO", Namespace = "http://www.onvif.org/ver10/schema")]
		public DeviceIOCapabilities deviceIO {
			get {
				return this._deviceIO;
			}
			set {
				this._deviceIO = value;
			}
		}

		/// <remarks>optional, order 2</remarks>
		[System.Xml.Serialization.XmlElementAttribute("Display", Namespace = "http://www.onvif.org/ver10/schema")]
		public DisplayCapabilities display {
			get {
				return this._display;
			}
			set {
				this._display = value;
			}
		}

		/// <remarks>optional, order 3</remarks>
		[System.Xml.Serialization.XmlElementAttribute("Recording", Namespace = "http://www.onvif.org/ver10/schema")]
		public RecordingCapabilities recording {
			get {
				return this._recording;
			}
			set {
				this._recording = value;
			}
		}

		/// <remarks>optional, order 4</remarks>
		[System.Xml.Serialization.XmlElementAttribute("Search", Namespace = "http://www.onvif.org/ver10/schema")]
		public SearchCapabilities search {
			get {
				return this._search;
			}
			set {
				this._search = value;
			}
		}

		/// <remarks>optional, order 5</remarks>
		[System.Xml.Serialization.XmlElementAttribute("Replay", Namespace = "http://www.onvif.org/ver10/schema")]
		public ReplayCapabilities replay {
			get {
				return this._replay;
			}
			set {
				this._replay = value;
			}
		}

		/// <remarks>optional, order 6</remarks>
		[System.Xml.Serialization.XmlElementAttribute("Receiver", Namespace = "http://www.onvif.org/ver10/schema")]
		public ReceiverCapabilities receiver {
			get {
				return this._receiver;
			}
			set {
				this._receiver = value;
			}
		}

		/// <remarks>optional, order 7</remarks>
		[System.Xml.Serialization.XmlElementAttribute("AnalyticsDevice", Namespace = "http://www.onvif.org/ver10/schema")]
		public AnalyticsDeviceCapabilities analyticsDevice {
			get {
				return this._analyticsDevice;
			}
			set {
				this._analyticsDevice = value;
			}
		}

		/// <remarks>optional, order 8</remarks>
		[System.Xml.Serialization.XmlElementAttribute("Extensions", Namespace = "http://www.onvif.org/ver10/schema")]
		public CapabilitiesExtension2 extensions {
			get {
				return this._extensions;
			}
			set {
				this._extensions = value;
			}
		}
	}

	[System.SerializableAttribute()]
	[System.Xml.Serialization.XmlTypeAttribute(TypeName = "DeviceIOCapabilities", Namespace = "http://www.onvif.org/ver10/schema")]
	public partial class DeviceIOCapabilities {

		private System.Xml.XmlAttribute[] _anyAttr;

		private string _xAddr;

		private int _videoSources;

		private int _videoOutputs;

		private int _audioSources;

		private int _audioOutputs;

		private int _relayOutputs;

		private System.Xml.XmlElement[] _any;

		[System.Xml.Serialization.XmlAnyAttributeAttribute()]
		public System.Xml.XmlAttribute[] anyAttr {
			get {
				return this._anyAttr;
			}
			set {
				this._anyAttr = value;
			}
		}

		/// <remarks>reqired, order 0</remarks>
		[System.Xml.Serialization.XmlElementAttribute("XAddr", Namespace = "http://www.onvif.org/ver10/schema", DataType = "anyURI")]
		public string xAddr {
			get {
				return this._xAddr;
			}
			set {
				this._xAddr = value;
			}
		}

		/// <remarks>reqired, order 1</remarks>
		[System.Xml.Serialization.XmlElementAttribute("VideoSources", Namespace = "http://www.onvif.org/ver10/schema", DataType = "int")]
		public int videoSources {
			get {
				return this._videoSources;
			}
			set {
				this._videoSources = value;
			}
		}

		/// <remarks>reqired, order 2</remarks>
		[System.Xml.Serialization.XmlElementAttribute("VideoOutputs", Namespace = "http://www.onvif.org/ver10/schema", DataType = "int")]
		public int videoOutputs {
			get {
				return this._videoOutputs;
			}
			set {
				this._videoOutputs = value;
			}
		}

		/// <remarks>reqired, order 3</remarks>
		[System.Xml.Serialization.XmlElementAttribute("AudioSources", Namespace = "http://www.onvif.org/ver10/schema", DataType = "int")]
		public int audioSources {
			get {
				return this._audioSources;
			}
			set {
				this._audioSources = value;
			}
		}

		/// <remarks>reqired, order 4</remarks>
		[System.Xml.Serialization.XmlElementAttribute("AudioOutputs", Namespace = "http://www.onvif.org/ver10/schema", DataType = "int")]
		public int audioOutputs {
			get {
				return this._audioOutputs;
			}
			set {
				this._audioOutputs = value;
			}
		}

		/// <remarks>reqired, order 5</remarks>
		[System.Xml.Serialization.XmlElementAttribute("RelayOutputs", Namespace = "http://www.onvif.org/ver10/schema", DataType = "int")]
		public int relayOutputs {
			get {
				return this._relayOutputs;
			}
			set {
				this._relayOutputs = value;
			}
		}

		/// <remarks>optional, order 6, namespace ##any</remarks>
		[System.Xml.Serialization.XmlAnyElementAttribute()]
		public System.Xml.XmlElement[] any {
			get {
				return this._any;
			}
			set {
				this._any = value;
			}
		}
	}

	[System.SerializableAttribute()]
	[System.Xml.Serialization.XmlTypeAttribute(TypeName = "DisplayCapabilities", Namespace = "http://www.onvif.org/ver10/schema")]
	public partial class DisplayCapabilities {

		private System.Xml.XmlAttribute[] _anyAttr;

		private string _xAddr;

		private bool _fixedLayout;

		private System.Xml.XmlElement[] _any;

		[System.Xml.Serialization.XmlAnyAttributeAttribute()]
		public System.Xml.XmlAttribute[] anyAttr {
			get {
				return this._anyAttr;
			}
			set {
				this._anyAttr = value;
			}
		}

		/// <remarks>reqired, order 0</remarks>
		[System.Xml.Serialization.XmlElementAttribute("XAddr", Namespace = "http://www.onvif.org/ver10/schema", DataType = "anyURI")]
		public string xAddr {
			get {
				return this._xAddr;
			}
			set {
				this._xAddr = value;
			}
		}

		/// <summary>
		/// Indication that the SetLayout command supports only predefined layouts.
		/// </summary>
		/// <remarks>reqired, order 1</remarks>
		[System.Xml.Serialization.XmlElementAttribute("FixedLayout", Namespace = "http://www.onvif.org/ver10/schema", DataType = "boolean")]
		public bool fixedLayout {
			get {
				return this._fixedLayout;
			}
			set {
				this._fixedLayout = value;
			}
		}

		/// <remarks>optional, order 2, namespace ##any</remarks>
		[System.Xml.Serialization.XmlAnyElementAttribute()]
		public System.Xml.XmlElement[] any {
			get {
				return this._any;
			}
			set {
				this._any = value;
			}
		}
	}

	[System.SerializableAttribute()]
	[System.Xml.Serialization.XmlTypeAttribute(TypeName = "RecordingCapabilities", Namespace = "http://www.onvif.org/ver10/schema")]
	public partial class RecordingCapabilities {

		private System.Xml.XmlAttribute[] _anyAttr;

		private string _xAddr;

		private bool _receiverSource;

		private bool _mediaProfileSource;

		private bool _dynamicRecordings;

		private bool _dynamicTracks;

		private int _maxStringLength;

		private System.Xml.XmlElement[] _any;

		[System.Xml.Serialization.XmlAnyAttributeAttribute()]
		public System.Xml.XmlAttribute[] anyAttr {
			get {
				return this._anyAttr;
			}
			set {
				this._anyAttr = value;
			}
		}

		/// <remarks>reqired, order 0</remarks>
		[System.Xml.Serialization.XmlElementAttribute("XAddr", Namespace = "http://www.onvif.org/ver10/schema", DataType = "anyURI")]
		public string xAddr {
			get {
				return this._xAddr;
			}
			set {
				this._xAddr = value;
			}
		}

		/// <remarks>reqired, order 1</remarks>
		[System.Xml.Serialization.XmlElementAttribute("ReceiverSource", Namespace = "http://www.onvif.org/ver10/schema", DataType = "boolean")]
		public bool receiverSource {
			get {
				return this._receiverSource;
			}
			set {
				this._receiverSource = value;
			}
		}

		/// <remarks>reqired, order 2</remarks>
		[System.Xml.Serialization.XmlElementAttribute("MediaProfileSource", Namespace = "http://www.onvif.org/ver10/schema", DataType = "boolean")]
		public bool mediaProfileSource {
			get {
				return this._mediaProfileSource;
			}
			set {
				this._mediaProfileSource = value;
			}
		}

		/// <remarks>reqired, order 3</remarks>
		[System.Xml.Serialization.XmlElementAttribute("DynamicRecordings", Namespace = "http://www.onvif.org/ver10/schema", DataType = "boolean")]
		public bool dynamicRecordings {
			get {
				return this._dynamicRecordings;
			}
			set {
				this._dynamicRecordings = value;
			}
		}

		/// <remarks>reqired, order 4</remarks>
		[System.Xml.Serialization.XmlElementAttribute("DynamicTracks", Namespace = "http://www.onvif.org/ver10/schema", DataType = "boolean")]
		public bool dynamicTracks {
			get {
				return this._dynamicTracks;
			}
			set {
				this._dynamicTracks = value;
			}
		}

		/// <remarks>reqired, order 5</remarks>
		[System.Xml.Serialization.XmlElementAttribute("MaxStringLength", Namespace = "http://www.onvif.org/ver10/schema", DataType = "int")]
		public int maxStringLength {
			get {
				return this._maxStringLength;
			}
			set {
				this._maxStringLength = value;
			}
		}

		/// <remarks>optional, order 6, namespace ##any</remarks>
		[System.Xml.Serialization.XmlAnyElementAttribute()]
		public System.Xml.XmlElement[] any {
			get {
				return this._any;
			}
			set {
				this._any = value;
			}
		}
	}

	[System.SerializableAttribute()]
	[System.Xml.Serialization.XmlTypeAttribute(TypeName = "SearchCapabilities", Namespace = "http://www.onvif.org/ver10/schema")]
	public partial class SearchCapabilities {

		private System.Xml.XmlAttribute[] _anyAttr;

		private string _xAddr;

		private bool _metadataSearch;

		private System.Xml.XmlElement[] _any;

		[System.Xml.Serialization.XmlAnyAttributeAttribute()]
		public System.Xml.XmlAttribute[] anyAttr {
			get {
				return this._anyAttr;
			}
			set {
				this._anyAttr = value;
			}
		}

		/// <remarks>reqired, order 0</remarks>
		[System.Xml.Serialization.XmlElementAttribute("XAddr", Namespace = "http://www.onvif.org/ver10/schema", DataType = "anyURI")]
		public string xAddr {
			get {
				return this._xAddr;
			}
			set {
				this._xAddr = value;
			}
		}

		/// <remarks>reqired, order 1</remarks>
		[System.Xml.Serialization.XmlElementAttribute("MetadataSearch", Namespace = "http://www.onvif.org/ver10/schema", DataType = "boolean")]
		public bool metadataSearch {
			get {
				return this._metadataSearch;
			}
			set {
				this._metadataSearch = value;
			}
		}

		/// <remarks>optional, order 2, namespace ##any</remarks>
		[System.Xml.Serialization.XmlAnyElementAttribute()]
		public System.Xml.XmlElement[] any {
			get {
				return this._any;
			}
			set {
				this._any = value;
			}
		}
	}

	[System.SerializableAttribute()]
	[System.Xml.Serialization.XmlTypeAttribute(TypeName = "ReplayCapabilities", Namespace = "http://www.onvif.org/ver10/schema")]
	public partial class ReplayCapabilities {

		private System.Xml.XmlAttribute[] _anyAttr;

		private string _xAddr;

		private System.Xml.XmlElement[] _any;

		[System.Xml.Serialization.XmlAnyAttributeAttribute()]
		public System.Xml.XmlAttribute[] anyAttr {
			get {
				return this._anyAttr;
			}
			set {
				this._anyAttr = value;
			}
		}

		/// <summary>
		/// The address of the replay service.
		/// </summary>
		/// <remarks>reqired, order 0</remarks>
		[System.Xml.Serialization.XmlElementAttribute("XAddr", Namespace = "http://www.onvif.org/ver10/schema", DataType = "anyURI")]
		public string xAddr {
			get {
				return this._xAddr;
			}
			set {
				this._xAddr = value;
			}
		}

		/// <remarks>optional, order 1, namespace ##any</remarks>
		[System.Xml.Serialization.XmlAnyElementAttribute()]
		public System.Xml.XmlElement[] any {
			get {
				return this._any;
			}
			set {
				this._any = value;
			}
		}
	}

	[System.SerializableAttribute()]
	[System.Xml.Serialization.XmlTypeAttribute(TypeName = "ReceiverCapabilities", Namespace = "http://www.onvif.org/ver10/schema")]
	public partial class ReceiverCapabilities {

		private System.Xml.XmlAttribute[] _anyAttr;

		private string _xAddr;

		private bool _rtp_multicast;

		private bool _rtp_tcp;

		private bool _rtp_rtsp_tcp;

		private int _supportedReceivers;

		private int _maximumRTSPURILength;

		private System.Xml.XmlElement[] _any;

		[System.Xml.Serialization.XmlAnyAttributeAttribute()]
		public System.Xml.XmlAttribute[] anyAttr {
			get {
				return this._anyAttr;
			}
			set {
				this._anyAttr = value;
			}
		}

		/// <summary>
		/// The address of the receiver service.
		/// </summary>
		/// <remarks>reqired, order 0</remarks>
		[System.Xml.Serialization.XmlElementAttribute("XAddr", Namespace = "http://www.onvif.org/ver10/schema", DataType = "anyURI")]
		public string xAddr {
			get {
				return this._xAddr;
			}
			set {
				this._xAddr = value;
			}
		}

		/// <summary>
		/// Indicates whether the device can receive RTP multicast streams.
		/// </summary>
		/// <remarks>reqired, order 1</remarks>
		[System.Xml.Serialization.XmlElementAttribute("RTP_Multicast", Namespace = "http://www.onvif.org/ver10/schema", DataType = "boolean")]
		public bool rtp_multicast {
			get {
				return this._rtp_multicast;
			}
			set {
				this._rtp_multicast = value;
			}
		}

		/// <summary>
		/// Indicates whether the device can receive RTP/TCP streams
		/// </summary>
		/// <remarks>reqired, order 2</remarks>
		[System.Xml.Serialization.XmlElementAttribute("RTP_TCP", Namespace = "http://www.onvif.org/ver10/schema", DataType = "boolean")]
		public bool rtp_tcp {
			get {
				return this._rtp_tcp;
			}
			set {
				this._rtp_tcp = value;
			}
		}

		/// <summary>
		/// Indicates whether the device can receive RTP/RTSP/TCP streams.
		/// </summary>
		/// <remarks>reqired, order 3</remarks>
		[System.Xml.Serialization.XmlElementAttribute("RTP_RTSP_TCP", Namespace = "http://www.onvif.org/ver10/schema", DataType = "boolean")]
		public bool rtp_rtsp_tcp {
			get {
				return this._rtp_rtsp_tcp;
			}
			set {
				this._rtp_rtsp_tcp = value;
			}
		}

		/// <summary>
		/// The maximum number of receivers supported by the device.
		/// </summary>
		/// <remarks>reqired, order 4</remarks>
		[System.Xml.Serialization.XmlElementAttribute("SupportedReceivers", Namespace = "http://www.onvif.org/ver10/schema", DataType = "int")]
		public int supportedReceivers {
			get {
				return this._supportedReceivers;
			}
			set {
				this._supportedReceivers = value;
			}
		}

		/// <summary>
		/// The maximum allowed length for RTSP URIs.
		/// </summary>
		/// <remarks>reqired, order 5</remarks>
		[System.Xml.Serialization.XmlElementAttribute("MaximumRTSPURILength", Namespace = "http://www.onvif.org/ver10/schema", DataType = "int")]
		public int maximumRTSPURILength {
			get {
				return this._maximumRTSPURILength;
			}
			set {
				this._maximumRTSPURILength = value;
			}
		}

		/// <remarks>optional, order 6, namespace ##any</remarks>
		[System.Xml.Serialization.XmlAnyElementAttribute()]
		public System.Xml.XmlElement[] any {
			get {
				return this._any;
			}
			set {
				this._any = value;
			}
		}
	}

	[System.SerializableAttribute()]
	[System.Xml.Serialization.XmlTypeAttribute(TypeName = "AnalyticsDeviceCapabilities", Namespace = "http://www.onvif.org/ver10/schema")]
	public partial class AnalyticsDeviceCapabilities {

		private System.Xml.XmlAttribute[] _anyAttr;

		private string _xAddr;

		private bool _ruleSupport;

		private bool _ruleSupportSpecified;

		private AnalyticsDeviceExtension _extension;

		[System.Xml.Serialization.XmlAnyAttributeAttribute()]
		public System.Xml.XmlAttribute[] anyAttr {
			get {
				return this._anyAttr;
			}
			set {
				this._anyAttr = value;
			}
		}

		/// <remarks>reqired, order 0</remarks>
		[System.Xml.Serialization.XmlElementAttribute("XAddr", Namespace = "http://www.onvif.org/ver10/schema", DataType = "anyURI")]
		public string xAddr {
			get {
				return this._xAddr;
			}
			set {
				this._xAddr = value;
			}
		}

		/// <summary>
		/// Obsolete property.
		/// </summary>
		/// <remarks>optional, order 1</remarks>
		[System.Xml.Serialization.XmlElementAttribute("RuleSupport", Namespace = "http://www.onvif.org/ver10/schema", DataType = "boolean")]
		public bool ruleSupport {
			get {
				return this._ruleSupport;
			}
			set {
				this._ruleSupport = value;
			}
		}

		[System.Xml.Serialization.XmlIgnoreAttribute()]
		public bool ruleSupportSpecified {
			get {
				return this._ruleSupportSpecified;
			}
			set {
				this._ruleSupportSpecified = value;
			}
		}

		/// <remarks>optional, order 2</remarks>
		[System.Xml.Serialization.XmlElementAttribute("Extension", Namespace = "http://www.onvif.org/ver10/schema")]
		public AnalyticsDeviceExtension extension {
			get {
				return this._extension;
			}
			set {
				this._extension = value;
			}
		}
	}

	[System.SerializableAttribute()]
	[System.Xml.Serialization.XmlTypeAttribute(TypeName = "AnalyticsDeviceExtension", Namespace = "http://www.onvif.org/ver10/schema")]
	public partial class AnalyticsDeviceExtension {

		private System.Xml.XmlElement[] _any;

		/// <remarks>optional, order 0, namespace ##any</remarks>
		[System.Xml.Serialization.XmlAnyElementAttribute()]
		public System.Xml.XmlElement[] any {
			get {
				return this._any;
			}
			set {
				this._any = value;
			}
		}
	}

	[System.SerializableAttribute()]
	[System.Xml.Serialization.XmlTypeAttribute(TypeName = "CapabilitiesExtension2", Namespace = "http://www.onvif.org/ver10/schema")]
	public partial class CapabilitiesExtension2 {

		private System.Xml.XmlElement[] _any;

		/// <remarks>optional, order 0, namespace ##targetNamespace</remarks>
		[System.Xml.Serialization.XmlAnyElementAttribute(Namespace = "http://www.onvif.org/ver10/schema")]
		public System.Xml.XmlElement[] any {
			get {
				return this._any;
			}
			set {
				this._any = value;
			}
		}
	}

	/// <summary>
	/// Enumeration describing the available system log modes.
	/// </summary>
	[System.SerializableAttribute()]
	[System.Xml.Serialization.XmlTypeAttribute(TypeName = "SystemLogType", Namespace = "http://www.onvif.org/ver10/schema")]
	public enum SystemLogType {

		/// <summary>
		/// Indicates that a system log is requested.
		/// </summary>
		[System.Xml.Serialization.XmlEnumAttribute(Name = "System")]
		system,

		/// <summary>
		/// Indicates that a access log is requested.
		/// </summary>
		[System.Xml.Serialization.XmlEnumAttribute(Name = "Access")]
		access,
	}

	[System.SerializableAttribute()]
	[System.Xml.Serialization.XmlTypeAttribute(TypeName = "SystemLog", Namespace = "http://www.onvif.org/ver10/schema")]
	public partial class SystemLog {

		private AttachmentData _binary;

		private string _string;

		/// <summary>
		/// The log information as attachment data.
		/// </summary>
		/// <remarks>optional, order 0</remarks>
		[System.Xml.Serialization.XmlElementAttribute("Binary", Namespace = "http://www.onvif.org/ver10/schema")]
		public AttachmentData binary {
			get {
				return this._binary;
			}
			set {
				this._binary = value;
			}
		}

		/// <summary>
		/// The log information as character data.
		/// </summary>
		/// <remarks>optional, order 1</remarks>
		[System.Xml.Serialization.XmlElementAttribute("String", Namespace = "http://www.onvif.org/ver10/schema", DataType = "string")]
		public string @string {
			get {
				return this._string;
			}
			set {
				this._string = value;
			}
		}
	}

	//[System.SerializableAttribute()]
	//[System.Xml.Serialization.XmlTypeAttribute(Namespace="http://www.onvif.org/ver10/schema")]
	//public partial class AttachmentData {

	//    private string _contentType;

	//    private Include _include;

	//    [System.Xml.Serialization.XmlAttributeAttribute(Form=System.Xml.Schema.XmlSchemaForm.Qualified, Namespace="http://www.w3.org/2005/05/xmlmime")]
	//    public string contentType {
	//        get {
	//            return this._contentType;
	//        }
	//        set {
	//            this._contentType = value;
	//        }
	//    }

	//    /// <remarks>reqired, order 0</remarks>
	//    [System.Xml.Serialization.XmlElementAttribute("Include", Namespace = "http://www.w3.org/2004/08/xop/include")]
	//    public Include include {
	//        get {
	//            return this._include;
	//        }
	//        set {
	//            this._include = value;
	//        }
	//    }
	//}

	[System.SerializableAttribute()]
	[System.Xml.Serialization.XmlTypeAttribute(TypeName = "SupportInformation", Namespace = "http://www.onvif.org/ver10/schema")]
	public partial class SupportInformation {

		private AttachmentData _binary;

		private string _string;

		/// <summary>
		/// The support information as attachment data.
		/// </summary>
		/// <remarks>optional, order 0</remarks>
		[System.Xml.Serialization.XmlElementAttribute("Binary", Namespace = "http://www.onvif.org/ver10/schema")]
		public AttachmentData binary {
			get {
				return this._binary;
			}
			set {
				this._binary = value;
			}
		}

		/// <summary>
		/// The support information as character data.
		/// </summary>
		/// <remarks>optional, order 1</remarks>
		[System.Xml.Serialization.XmlElementAttribute("String", Namespace = "http://www.onvif.org/ver10/schema", DataType = "string")]
		public string @string {
			get {
				return this._string;
			}
			set {
				this._string = value;
			}
		}
	}

	[System.SerializableAttribute()]
	[System.Xml.Serialization.XmlTypeAttribute(TypeName = "BinaryData", Namespace = "http://www.onvif.org/ver10/schema")]
	public partial class BinaryData {

		private string _contentType;

		private byte[] _data;

		[System.Xml.Serialization.XmlAttributeAttribute(Form = System.Xml.Schema.XmlSchemaForm.Qualified, Namespace = "http://www.w3.org/2005/05/xmlmime")]
		public string contentType {
			get {
				return this._contentType;
			}
			set {
				this._contentType = value;
			}
		}

		/// <summary>
		/// base64 encoded binary data.
		/// </summary>
		/// <remarks>reqired, order 0</remarks>
		[System.Xml.Serialization.XmlElementAttribute("Data", Namespace = "http://www.onvif.org/ver10/schema", DataType = "base64Binary")]
		public byte[] data {
			get {
				return this._data;
			}
			set {
				this._data = value;
			}
		}
	}

	[System.SerializableAttribute()]
	[System.Xml.Serialization.XmlTypeAttribute(TypeName = "BackupFile", Namespace = "http://www.onvif.org/ver10/schema")]
	public partial class BackupFile {

		private string _name;

		private AttachmentData _data;

		/// <remarks>reqired, order 0</remarks>
		[System.Xml.Serialization.XmlElementAttribute("Name", Namespace = "http://www.onvif.org/ver10/schema", DataType = "string")]
		public string name {
			get {
				return this._name;
			}
			set {
				this._name = value;
			}
		}

		/// <remarks>reqired, order 1</remarks>
		[System.Xml.Serialization.XmlElementAttribute("Data", Namespace = "http://www.onvif.org/ver10/schema")]
		public AttachmentData data {
			get {
				return this._data;
			}
			set {
				this._data = value;
			}
		}
	}

	[System.SerializableAttribute()]
	[System.Xml.Serialization.XmlTypeAttribute(TypeName = "SystemLogUriList", Namespace = "http://www.onvif.org/ver10/schema")]
	public partial class SystemLogUriList {

		private SystemLogUri[] _systemLog;

		/// <remarks>optional, order 0</remarks>
		[System.Xml.Serialization.XmlElementAttribute("SystemLog", Namespace = "http://www.onvif.org/ver10/schema")]
		public SystemLogUri[] systemLog {
			get {
				return this._systemLog;
			}
			set {
				this._systemLog = value;
			}
		}
	}

	[System.SerializableAttribute()]
	[System.Xml.Serialization.XmlTypeAttribute(TypeName = "SystemLogUri", Namespace = "http://www.onvif.org/ver10/schema")]
	public partial class SystemLogUri {

		private System.Xml.XmlAttribute[] _anyAttr;

		private SystemLogType _type;

		private string _uri;

		private System.Xml.XmlElement[] _any;

		[System.Xml.Serialization.XmlAnyAttributeAttribute()]
		public System.Xml.XmlAttribute[] anyAttr {
			get {
				return this._anyAttr;
			}
			set {
				this._anyAttr = value;
			}
		}

		/// <remarks>reqired, order 0</remarks>
		[System.Xml.Serialization.XmlElementAttribute("Type", Namespace = "http://www.onvif.org/ver10/schema")]
		public SystemLogType type {
			get {
				return this._type;
			}
			set {
				this._type = value;
			}
		}

		/// <remarks>reqired, order 1</remarks>
		[System.Xml.Serialization.XmlElementAttribute("Uri", Namespace = "http://www.onvif.org/ver10/schema", DataType = "anyURI")]
		public string uri {
			get {
				return this._uri;
			}
			set {
				this._uri = value;
			}
		}

		/// <remarks>optional, order 2, namespace ##any</remarks>
		[System.Xml.Serialization.XmlAnyElementAttribute()]
		public System.Xml.XmlElement[] any {
			get {
				return this._any;
			}
			set {
				this._any = value;
			}
		}
	}

	/// <summary>
	/// Enumeration describing the available factory default modes.
	/// </summary>
	[System.SerializableAttribute()]
	[System.Xml.Serialization.XmlTypeAttribute(TypeName = "FactoryDefaultType", Namespace = "http://www.onvif.org/ver10/schema")]
	public enum FactoryDefaultType {

		/// <summary>
		/// Indicates that a hard factory default is requested.
		/// </summary>
		[System.Xml.Serialization.XmlEnumAttribute(Name = "Hard")]
		hard,

		/// <summary>
		/// Indicates that a soft factory default is requested.
		/// </summary>
		[System.Xml.Serialization.XmlEnumAttribute(Name = "Soft")]
		soft,
	}

	[System.SerializableAttribute()]
	[System.Xml.Serialization.XmlTypeAttribute(TypeName = "SetDateTimeType", Namespace = "http://www.onvif.org/ver10/schema")]
	public enum SetDateTimeType {

		/// <summary>
		/// Indicates that the date and time are set manually.
		/// </summary>
		[System.Xml.Serialization.XmlEnumAttribute(Name = "Manual")]
		manual,

		/// <summary>
		/// Indicates that the date and time are set through NTP
		/// </summary>
		[System.Xml.Serialization.XmlEnumAttribute(Name = "NTP")]
		ntp,
	}

	/// <summary>
	/// General date time inforamtion returned by the GetSystemDateTime method.
	/// </summary>
	[System.SerializableAttribute()]
	[System.Xml.Serialization.XmlTypeAttribute(TypeName = "SystemDateTime", Namespace = "http://www.onvif.org/ver10/schema")]
	public partial class SystemDateTime {

		private System.Xml.XmlAttribute[] _anyAttr;

		private SetDateTimeType _dateTimeType;

		private bool _daylightSavings;

		private TimeZone _timeZone;

		private DateTime _utcDateTime;

		private DateTime _localDateTime;

		private SystemDateTimeExtension _extension;

		[System.Xml.Serialization.XmlAnyAttributeAttribute()]
		public System.Xml.XmlAttribute[] anyAttr {
			get {
				return this._anyAttr;
			}
			set {
				this._anyAttr = value;
			}
		}

		/// <summary>
		/// Indicates if the time is set manully or through NTP.
		/// </summary>
		/// <remarks>reqired, order 0</remarks>
		[System.Xml.Serialization.XmlElementAttribute("DateTimeType", Namespace = "http://www.onvif.org/ver10/schema")]
		public SetDateTimeType dateTimeType {
			get {
				return this._dateTimeType;
			}
			set {
				this._dateTimeType = value;
			}
		}

		/// <summary>
		/// Informative indicator whether daylight savings is currently on/off.
		/// </summary>
		/// <remarks>reqired, order 1</remarks>
		[System.Xml.Serialization.XmlElementAttribute("DaylightSavings", Namespace = "http://www.onvif.org/ver10/schema", DataType = "boolean")]
		public bool daylightSavings {
			get {
				return this._daylightSavings;
			}
			set {
				this._daylightSavings = value;
			}
		}

		/// <summary>
		/// Timezone information in Posix format.
		/// </summary>
		/// <remarks>optional, order 2</remarks>
		[System.Xml.Serialization.XmlElementAttribute("TimeZone", Namespace = "http://www.onvif.org/ver10/schema")]
		public TimeZone timeZone {
			get {
				return this._timeZone;
			}
			set {
				this._timeZone = value;
			}
		}

		/// <summary>
		/// Current system date and time in UTC format. This field is mandatory since version 2.0.
		/// </summary>
		/// <remarks>optional, order 3</remarks>
		[System.Xml.Serialization.XmlElementAttribute("UTCDateTime", Namespace = "http://www.onvif.org/ver10/schema")]
		public DateTime utcDateTime {
			get {
				return this._utcDateTime;
			}
			set {
				this._utcDateTime = value;
			}
		}

		/// <summary>
		/// Date and time in local format.
		/// </summary>
		/// <remarks>optional, order 4</remarks>
		[System.Xml.Serialization.XmlElementAttribute("LocalDateTime", Namespace = "http://www.onvif.org/ver10/schema")]
		public DateTime localDateTime {
			get {
				return this._localDateTime;
			}
			set {
				this._localDateTime = value;
			}
		}

		/// <remarks>optional, order 5</remarks>
		[System.Xml.Serialization.XmlElementAttribute("Extension", Namespace = "http://www.onvif.org/ver10/schema")]
		public SystemDateTimeExtension extension {
			get {
				return this._extension;
			}
			set {
				this._extension = value;
			}
		}
	}

	/// <summary>
	/// The TZ format is specified by POSIX, please refer to POSIX 1003.1 section 8.3
	/// Example: Europe, Paris TZ=CET-1CEST,M3.5.0/2,M10.5.0/3
	/// CET = designation for standard time when daylight saving is not in force
	/// -1 = offset in hours = negative so 1 hour east of Greenwich meridian
	/// CEST = designation when daylight saving is in force ("Central European Summer Time")
	/// , = no offset number between code and comma, so default to one hour ahead for daylight saving
	/// M3.5.0 = when daylight saving starts = the last Sunday in March (the "5th" week means the last in the month)
	/// /2, = the local time when the switch occurs = 2 a.m. in this case
	/// M10.5.0 = when daylight saving ends = the last Sunday in October.
	/// /3, = the local time when the switch occurs = 3 a.m. in this case
	/// </summary>
	[System.SerializableAttribute()]
	[System.Xml.Serialization.XmlTypeAttribute(TypeName = "TimeZone", Namespace = "http://www.onvif.org/ver10/schema")]
	public partial class TimeZone {

		private string _tz;

		/// <summary>
		/// Posix timezone string.
		/// </summary>
		/// <remarks>reqired, order 0</remarks>
		[System.Xml.Serialization.XmlElementAttribute("TZ", Namespace = "http://www.onvif.org/ver10/schema", DataType = "token")]
		public string tz {
			get {
				return this._tz;
			}
			set {
				this._tz = value;
			}
		}
	}

	[System.SerializableAttribute()]
	[System.Xml.Serialization.XmlTypeAttribute(TypeName = "DateTime", Namespace = "http://www.onvif.org/ver10/schema")]
	public partial class DateTime {

		private Time _time;

		private Date _date;

		/// <remarks>reqired, order 0</remarks>
		[System.Xml.Serialization.XmlElementAttribute("Time", Namespace = "http://www.onvif.org/ver10/schema")]
		public Time time {
			get {
				return this._time;
			}
			set {
				this._time = value;
			}
		}

		/// <remarks>reqired, order 1</remarks>
		[System.Xml.Serialization.XmlElementAttribute("Date", Namespace = "http://www.onvif.org/ver10/schema")]
		public Date date {
			get {
				return this._date;
			}
			set {
				this._date = value;
			}
		}
	}

	[System.SerializableAttribute()]
	[System.Xml.Serialization.XmlTypeAttribute(TypeName = "Time", Namespace = "http://www.onvif.org/ver10/schema")]
	public partial class Time {

		private int _hour;

		private int _minute;

		private int _second;

		/// <summary>
		/// Range is 0 to 23.
		/// </summary>
		/// <remarks>reqired, order 0</remarks>
		[System.Xml.Serialization.XmlElementAttribute("Hour", Namespace = "http://www.onvif.org/ver10/schema", DataType = "int")]
		public int hour {
			get {
				return this._hour;
			}
			set {
				this._hour = value;
			}
		}

		/// <summary>
		/// Range is 0 to 59.
		/// </summary>
		/// <remarks>reqired, order 1</remarks>
		[System.Xml.Serialization.XmlElementAttribute("Minute", Namespace = "http://www.onvif.org/ver10/schema", DataType = "int")]
		public int minute {
			get {
				return this._minute;
			}
			set {
				this._minute = value;
			}
		}

		/// <summary>
		/// Range is 0 to 61 (typically 59).
		/// </summary>
		/// <remarks>reqired, order 2</remarks>
		[System.Xml.Serialization.XmlElementAttribute("Second", Namespace = "http://www.onvif.org/ver10/schema", DataType = "int")]
		public int second {
			get {
				return this._second;
			}
			set {
				this._second = value;
			}
		}
	}

	[System.SerializableAttribute()]
	[System.Xml.Serialization.XmlTypeAttribute(TypeName = "Date", Namespace = "http://www.onvif.org/ver10/schema")]
	public partial class Date {

		private int _year;

		private int _month;

		private int _day;

		/// <remarks>reqired, order 0</remarks>
		[System.Xml.Serialization.XmlElementAttribute("Year", Namespace = "http://www.onvif.org/ver10/schema", DataType = "int")]
		public int year {
			get {
				return this._year;
			}
			set {
				this._year = value;
			}
		}

		/// <summary>
		/// Range is 1 to 12.
		/// </summary>
		/// <remarks>reqired, order 1</remarks>
		[System.Xml.Serialization.XmlElementAttribute("Month", Namespace = "http://www.onvif.org/ver10/schema", DataType = "int")]
		public int month {
			get {
				return this._month;
			}
			set {
				this._month = value;
			}
		}

		/// <summary>
		/// Range is 1 to 31.
		/// </summary>
		/// <remarks>reqired, order 2</remarks>
		[System.Xml.Serialization.XmlElementAttribute("Day", Namespace = "http://www.onvif.org/ver10/schema", DataType = "int")]
		public int day {
			get {
				return this._day;
			}
			set {
				this._day = value;
			}
		}
	}

	[System.SerializableAttribute()]
	[System.Xml.Serialization.XmlTypeAttribute(TypeName = "SystemDateTimeExtension", Namespace = "http://www.onvif.org/ver10/schema")]
	public partial class SystemDateTimeExtension {

		private System.Xml.XmlElement[] _any;

		/// <remarks>optional, order 0, namespace ##any</remarks>
		[System.Xml.Serialization.XmlAnyElementAttribute()]
		public System.Xml.XmlElement[] any {
			get {
				return this._any;
			}
			set {
				this._any = value;
			}
		}
	}

	[System.SerializableAttribute()]
	[System.Xml.Serialization.XmlTypeAttribute(TypeName = "RemoteUser", Namespace = "http://www.onvif.org/ver10/schema")]
	public partial class RemoteUser {

		private System.Xml.XmlAttribute[] _anyAttr;

		private string _username;

		private string _password;

		private bool _useDerivedPassword;

		private System.Xml.XmlElement[] _any;

		[System.Xml.Serialization.XmlAnyAttributeAttribute()]
		public System.Xml.XmlAttribute[] anyAttr {
			get {
				return this._anyAttr;
			}
			set {
				this._anyAttr = value;
			}
		}

		/// <remarks>reqired, order 0</remarks>
		[System.Xml.Serialization.XmlElementAttribute("Username", Namespace = "http://www.onvif.org/ver10/schema", DataType = "string")]
		public string username {
			get {
				return this._username;
			}
			set {
				this._username = value;
			}
		}

		/// <remarks>optional, order 1</remarks>
		[System.Xml.Serialization.XmlElementAttribute("Password", Namespace = "http://www.onvif.org/ver10/schema", DataType = "string")]
		public string password {
			get {
				return this._password;
			}
			set {
				this._password = value;
			}
		}

		/// <remarks>reqired, order 2</remarks>
		[System.Xml.Serialization.XmlElementAttribute("UseDerivedPassword", Namespace = "http://www.onvif.org/ver10/schema", DataType = "boolean")]
		public bool useDerivedPassword {
			get {
				return this._useDerivedPassword;
			}
			set {
				this._useDerivedPassword = value;
			}
		}

		/// <remarks>optional, order 3, namespace ##any</remarks>
		[System.Xml.Serialization.XmlAnyElementAttribute()]
		public System.Xml.XmlElement[] any {
			get {
				return this._any;
			}
			set {
				this._any = value;
			}
		}
	}

	[System.SerializableAttribute()]
	[System.Xml.Serialization.XmlTypeAttribute(TypeName = "UserLevel", Namespace = "http://www.onvif.org/ver10/schema")]
	public enum UserLevel {

		[System.Xml.Serialization.XmlEnumAttribute(Name = "Administrator")]
		administrator,

		[System.Xml.Serialization.XmlEnumAttribute(Name = "Operator")]
		@operator,

		[System.Xml.Serialization.XmlEnumAttribute(Name = "User")]
		user,

		[System.Xml.Serialization.XmlEnumAttribute(Name = "Anonymous")]
		anonymous,

		[System.Xml.Serialization.XmlEnumAttribute(Name = "Extended")]
		extended,
	}

	[System.SerializableAttribute()]
	[System.Xml.Serialization.XmlTypeAttribute(TypeName = "User", Namespace = "http://www.onvif.org/ver10/schema")]
	public partial class User {

		private System.Xml.XmlAttribute[] _anyAttr;

		private string _username;

		private string _password;

		private UserLevel _userLevel;

		private UserExtension _extension;

		[System.Xml.Serialization.XmlAnyAttributeAttribute()]
		public System.Xml.XmlAttribute[] anyAttr {
			get {
				return this._anyAttr;
			}
			set {
				this._anyAttr = value;
			}
		}

		/// <summary>
		/// Username string.
		/// </summary>
		/// <remarks>reqired, order 0</remarks>
		[System.Xml.Serialization.XmlElementAttribute("Username", Namespace = "http://www.onvif.org/ver10/schema", DataType = "string")]
		public string username {
			get {
				return this._username;
			}
			set {
				this._username = value;
			}
		}

		/// <summary>
		/// Password string.
		/// </summary>
		/// <remarks>optional, order 1</remarks>
		[System.Xml.Serialization.XmlElementAttribute("Password", Namespace = "http://www.onvif.org/ver10/schema", DataType = "string")]
		public string password {
			get {
				return this._password;
			}
			set {
				this._password = value;
			}
		}

		/// <summary>
		/// User level string.
		/// </summary>
		/// <remarks>reqired, order 2</remarks>
		[System.Xml.Serialization.XmlElementAttribute("UserLevel", Namespace = "http://www.onvif.org/ver10/schema")]
		public UserLevel userLevel {
			get {
				return this._userLevel;
			}
			set {
				this._userLevel = value;
			}
		}

		/// <remarks>optional, order 3</remarks>
		[System.Xml.Serialization.XmlElementAttribute("Extension", Namespace = "http://www.onvif.org/ver10/schema")]
		public UserExtension extension {
			get {
				return this._extension;
			}
			set {
				this._extension = value;
			}
		}
	}

	[System.SerializableAttribute()]
	[System.Xml.Serialization.XmlTypeAttribute(TypeName = "UserExtension", Namespace = "http://www.onvif.org/ver10/schema")]
	public partial class UserExtension {

		private System.Xml.XmlElement[] _any;

		/// <remarks>optional, order 0, namespace ##any</remarks>
		[System.Xml.Serialization.XmlAnyElementAttribute()]
		public System.Xml.XmlElement[] any {
			get {
				return this._any;
			}
			set {
				this._any = value;
			}
		}
	}

	[System.SerializableAttribute()]
	[System.Xml.Serialization.XmlTypeAttribute(TypeName = "CertificateGenerationParameters", Namespace = "http://www.onvif.org/ver10/schema")]
	public partial class CertificateGenerationParameters {

		private System.Xml.XmlAttribute[] _anyAttr;

		private string _certificateID;

		private string _subject;

		private string _validNotBefore;

		private string _validNotAfter;

		private CertificateGenerationParametersExtension _extension;

		[System.Xml.Serialization.XmlAnyAttributeAttribute()]
		public System.Xml.XmlAttribute[] anyAttr {
			get {
				return this._anyAttr;
			}
			set {
				this._anyAttr = value;
			}
		}

		/// <remarks>optional, order 0</remarks>
		[System.Xml.Serialization.XmlElementAttribute("CertificateID", Namespace = "http://www.onvif.org/ver10/schema", DataType = "token")]
		public string certificateID {
			get {
				return this._certificateID;
			}
			set {
				this._certificateID = value;
			}
		}

		/// <remarks>optional, order 1</remarks>
		[System.Xml.Serialization.XmlElementAttribute("Subject", Namespace = "http://www.onvif.org/ver10/schema", DataType = "string")]
		public string subject {
			get {
				return this._subject;
			}
			set {
				this._subject = value;
			}
		}

		/// <remarks>optional, order 2</remarks>
		[System.Xml.Serialization.XmlElementAttribute("ValidNotBefore", Namespace = "http://www.onvif.org/ver10/schema", DataType = "token")]
		public string validNotBefore {
			get {
				return this._validNotBefore;
			}
			set {
				this._validNotBefore = value;
			}
		}

		/// <remarks>optional, order 3</remarks>
		[System.Xml.Serialization.XmlElementAttribute("ValidNotAfter", Namespace = "http://www.onvif.org/ver10/schema", DataType = "token")]
		public string validNotAfter {
			get {
				return this._validNotAfter;
			}
			set {
				this._validNotAfter = value;
			}
		}

		/// <remarks>optional, order 4</remarks>
		[System.Xml.Serialization.XmlElementAttribute("Extension", Namespace = "http://www.onvif.org/ver10/schema")]
		public CertificateGenerationParametersExtension extension {
			get {
				return this._extension;
			}
			set {
				this._extension = value;
			}
		}
	}

	[System.SerializableAttribute()]
	[System.Xml.Serialization.XmlTypeAttribute(TypeName = "CertificateGenerationParametersExtension", Namespace = "http://www.onvif.org/ver10/schema")]
	public partial class CertificateGenerationParametersExtension {

		private System.Xml.XmlElement[] _any;

		/// <remarks>optional, order 0, namespace ##any</remarks>
		[System.Xml.Serialization.XmlAnyElementAttribute()]
		public System.Xml.XmlElement[] any {
			get {
				return this._any;
			}
			set {
				this._any = value;
			}
		}
	}

	[System.SerializableAttribute()]
	[System.Xml.Serialization.XmlTypeAttribute(TypeName = "Certificate", Namespace = "http://www.onvif.org/ver10/schema")]
	public partial class Certificate {

		private string _certificateID;

		private BinaryData _certificate;

		/// <summary>
		/// Certificate id.
		/// </summary>
		/// <remarks>reqired, order 0</remarks>
		[System.Xml.Serialization.XmlElementAttribute("CertificateID", Namespace = "http://www.onvif.org/ver10/schema", DataType = "token")]
		public string certificateID {
			get {
				return this._certificateID;
			}
			set {
				this._certificateID = value;
			}
		}

		/// <summary>
		/// base64 encoded DER representation of certificate.
		/// </summary>
		/// <remarks>reqired, order 1</remarks>
		[System.Xml.Serialization.XmlElementAttribute("Certificate", Namespace = "http://www.onvif.org/ver10/schema")]
		public BinaryData certificate {
			get {
				return this._certificate;
			}
			set {
				this._certificate = value;
			}
		}
	}

	[System.SerializableAttribute()]
	[System.Xml.Serialization.XmlTypeAttribute(TypeName = "CertificateStatus", Namespace = "http://www.onvif.org/ver10/schema")]
	public partial class CertificateStatus {

		private System.Xml.XmlAttribute[] _anyAttr;

		private string _certificateID;

		private bool _status;

		private System.Xml.XmlElement[] _any;

		[System.Xml.Serialization.XmlAnyAttributeAttribute()]
		public System.Xml.XmlAttribute[] anyAttr {
			get {
				return this._anyAttr;
			}
			set {
				this._anyAttr = value;
			}
		}

		/// <summary>
		/// Certificate id.
		/// </summary>
		/// <remarks>reqired, order 0</remarks>
		[System.Xml.Serialization.XmlElementAttribute("CertificateID", Namespace = "http://www.onvif.org/ver10/schema", DataType = "token")]
		public string certificateID {
			get {
				return this._certificateID;
			}
			set {
				this._certificateID = value;
			}
		}

		/// <summary>
		/// Indicates whether or not a certificate is used in a HTTPS configuration.
		/// </summary>
		/// <remarks>reqired, order 1</remarks>
		[System.Xml.Serialization.XmlElementAttribute("Status", Namespace = "http://www.onvif.org/ver10/schema", DataType = "boolean")]
		public bool status {
			get {
				return this._status;
			}
			set {
				this._status = value;
			}
		}

		/// <remarks>optional, order 2, namespace ##any</remarks>
		[System.Xml.Serialization.XmlAnyElementAttribute()]
		public System.Xml.XmlElement[] any {
			get {
				return this._any;
			}
			set {
				this._any = value;
			}
		}
	}

	[System.SerializableAttribute()]
	[System.Xml.Serialization.XmlTypeAttribute(TypeName = "CertificateWithPrivateKey", Namespace = "http://www.onvif.org/ver10/schema")]
	public partial class CertificateWithPrivateKey {

		private System.Xml.XmlAttribute[] _anyAttr;

		private string _certificateID;

		private BinaryData _certificate;

		private BinaryData _privateKey;

		private System.Xml.XmlElement[] _any;

		[System.Xml.Serialization.XmlAnyAttributeAttribute()]
		public System.Xml.XmlAttribute[] anyAttr {
			get {
				return this._anyAttr;
			}
			set {
				this._anyAttr = value;
			}
		}

		/// <remarks>optional, order 0</remarks>
		[System.Xml.Serialization.XmlElementAttribute("CertificateID", Namespace = "http://www.onvif.org/ver10/schema", DataType = "token")]
		public string certificateID {
			get {
				return this._certificateID;
			}
			set {
				this._certificateID = value;
			}
		}

		/// <remarks>reqired, order 1</remarks>
		[System.Xml.Serialization.XmlElementAttribute("Certificate", Namespace = "http://www.onvif.org/ver10/schema")]
		public BinaryData certificate {
			get {
				return this._certificate;
			}
			set {
				this._certificate = value;
			}
		}

		/// <remarks>reqired, order 2</remarks>
		[System.Xml.Serialization.XmlElementAttribute("PrivateKey", Namespace = "http://www.onvif.org/ver10/schema")]
		public BinaryData privateKey {
			get {
				return this._privateKey;
			}
			set {
				this._privateKey = value;
			}
		}

		/// <remarks>optional, order 3, namespace ##any</remarks>
		[System.Xml.Serialization.XmlAnyElementAttribute()]
		public System.Xml.XmlElement[] any {
			get {
				return this._any;
			}
			set {
				this._any = value;
			}
		}
	}

	[System.SerializableAttribute()]
	[System.Xml.Serialization.XmlTypeAttribute(TypeName = "CertificateInformation", Namespace = "http://www.onvif.org/ver10/schema")]
	public partial class CertificateInformation {

		private System.Xml.XmlAttribute[] _anyAttr;

		private string _certificateID;

		private string _issuerDN;

		private string _subjectDN;

		private CertificateUsage _keyUsage;

		private CertificateUsage _extendedKeyUsage;

		private int _keyLength;

		private bool _keyLengthSpecified;

		private string _version;

		private string _serialNum;

		private string _signatureAlgorithm;

		private DateTimeRange _validity;

		private CertificateInformationExtension _extension;

		[System.Xml.Serialization.XmlAnyAttributeAttribute()]
		public System.Xml.XmlAttribute[] anyAttr {
			get {
				return this._anyAttr;
			}
			set {
				this._anyAttr = value;
			}
		}

		/// <remarks>reqired, order 0</remarks>
		[System.Xml.Serialization.XmlElementAttribute("CertificateID", Namespace = "http://www.onvif.org/ver10/schema", DataType = "token")]
		public string certificateID {
			get {
				return this._certificateID;
			}
			set {
				this._certificateID = value;
			}
		}

		/// <remarks>optional, order 1</remarks>
		[System.Xml.Serialization.XmlElementAttribute("IssuerDN", Namespace = "http://www.onvif.org/ver10/schema", DataType = "string")]
		public string issuerDN {
			get {
				return this._issuerDN;
			}
			set {
				this._issuerDN = value;
			}
		}

		/// <remarks>optional, order 2</remarks>
		[System.Xml.Serialization.XmlElementAttribute("SubjectDN", Namespace = "http://www.onvif.org/ver10/schema", DataType = "string")]
		public string subjectDN {
			get {
				return this._subjectDN;
			}
			set {
				this._subjectDN = value;
			}
		}

		/// <remarks>optional, order 3</remarks>
		[System.Xml.Serialization.XmlElementAttribute("KeyUsage", Namespace = "http://www.onvif.org/ver10/schema")]
		public CertificateUsage keyUsage {
			get {
				return this._keyUsage;
			}
			set {
				this._keyUsage = value;
			}
		}

		/// <remarks>optional, order 4</remarks>
		[System.Xml.Serialization.XmlElementAttribute("ExtendedKeyUsage", Namespace = "http://www.onvif.org/ver10/schema")]
		public CertificateUsage extendedKeyUsage {
			get {
				return this._extendedKeyUsage;
			}
			set {
				this._extendedKeyUsage = value;
			}
		}

		/// <remarks>optional, order 5</remarks>
		[System.Xml.Serialization.XmlElementAttribute("KeyLength", Namespace = "http://www.onvif.org/ver10/schema", DataType = "int")]
		public int keyLength {
			get {
				return this._keyLength;
			}
			set {
				this._keyLength = value;
			}
		}

		[System.Xml.Serialization.XmlIgnoreAttribute()]
		public bool keyLengthSpecified {
			get {
				return this._keyLengthSpecified;
			}
			set {
				this._keyLengthSpecified = value;
			}
		}

		/// <remarks>optional, order 6</remarks>
		[System.Xml.Serialization.XmlElementAttribute("Version", Namespace = "http://www.onvif.org/ver10/schema", DataType = "string")]
		public string version {
			get {
				return this._version;
			}
			set {
				this._version = value;
			}
		}

		/// <remarks>optional, order 7</remarks>
		[System.Xml.Serialization.XmlElementAttribute("SerialNum", Namespace = "http://www.onvif.org/ver10/schema", DataType = "string")]
		public string serialNum {
			get {
				return this._serialNum;
			}
			set {
				this._serialNum = value;
			}
		}

		/// <summary>
		/// Validity Range is from "NotBefore" to "NotAfter"; the corresponding DateTimeRange is from "From" to "Until"
		/// </summary>
		/// <remarks>optional, order 8</remarks>
		[System.Xml.Serialization.XmlElementAttribute("SignatureAlgorithm", Namespace = "http://www.onvif.org/ver10/schema", DataType = "string")]
		public string signatureAlgorithm {
			get {
				return this._signatureAlgorithm;
			}
			set {
				this._signatureAlgorithm = value;
			}
		}

		/// <remarks>optional, order 9</remarks>
		[System.Xml.Serialization.XmlElementAttribute("Validity", Namespace = "http://www.onvif.org/ver10/schema")]
		public DateTimeRange validity {
			get {
				return this._validity;
			}
			set {
				this._validity = value;
			}
		}

		/// <remarks>optional, order 10</remarks>
		[System.Xml.Serialization.XmlElementAttribute("Extension", Namespace = "http://www.onvif.org/ver10/schema")]
		public CertificateInformationExtension extension {
			get {
				return this._extension;
			}
			set {
				this._extension = value;
			}
		}
	}

	[System.SerializableAttribute()]
	[System.Xml.Serialization.XmlTypeAttribute(TypeName = "CertificateUsage", Namespace = "http://www.onvif.org/ver10/schema")]
	public partial class CertificateUsage {

		private bool _critical;

		private string valueField;

		[System.Xml.Serialization.XmlAttributeAttribute("Critical", DataType = "boolean")]
		public bool critical {
			get {
				return this._critical;
			}
			set {
				this._critical = value;
			}
		}

		/// <remarks/>
		[System.Xml.Serialization.XmlTextAttribute()]
		public string Value {
			get {
				return this.valueField;
			}
			set {
				this.valueField = value;
			}
		}

	}

	[System.SerializableAttribute()]
	[System.Xml.Serialization.XmlTypeAttribute(TypeName = "DateTimeRange", Namespace = "http://www.onvif.org/ver10/schema")]
	public partial class DateTimeRange {

		private System.Xml.XmlAttribute[] _anyAttr;

		private System.DateTime _from;

		private System.DateTime _until;

		private System.Xml.XmlElement[] _any;

		[System.Xml.Serialization.XmlAnyAttributeAttribute()]
		public System.Xml.XmlAttribute[] anyAttr {
			get {
				return this._anyAttr;
			}
			set {
				this._anyAttr = value;
			}
		}

		/// <remarks>reqired, order 0</remarks>
		[System.Xml.Serialization.XmlElementAttribute("From", Namespace = "http://www.onvif.org/ver10/schema", DataType = "dateTime")]
		public System.DateTime from {
			get {
				return this._from;
			}
			set {
				this._from = value;
			}
		}

		/// <remarks>reqired, order 1</remarks>
		[System.Xml.Serialization.XmlElementAttribute("Until", Namespace = "http://www.onvif.org/ver10/schema", DataType = "dateTime")]
		public System.DateTime until {
			get {
				return this._until;
			}
			set {
				this._until = value;
			}
		}

		/// <remarks>optional, order 2, namespace ##any</remarks>
		[System.Xml.Serialization.XmlAnyElementAttribute()]
		public System.Xml.XmlElement[] any {
			get {
				return this._any;
			}
			set {
				this._any = value;
			}
		}
	}

	[System.SerializableAttribute()]
	[System.Xml.Serialization.XmlTypeAttribute(TypeName = "CertificateInformationExtension", Namespace = "http://www.onvif.org/ver10/schema")]
	public partial class CertificateInformationExtension {

		private System.Xml.XmlElement[] _any;

		/// <remarks>optional, order 0, namespace ##any</remarks>
		[System.Xml.Serialization.XmlAnyElementAttribute()]
		public System.Xml.XmlElement[] any {
			get {
				return this._any;
			}
			set {
				this._any = value;
			}
		}
	}

	[System.SerializableAttribute()]
	[System.Xml.Serialization.XmlTypeAttribute(TypeName = "Dot1XConfiguration", Namespace = "http://www.onvif.org/ver10/schema")]
	public partial class Dot1XConfiguration {

		private System.Xml.XmlAttribute[] _anyAttr;

		private string _dot1XConfigurationToken;

		private string _identity;

		private string _anonymousID;

		private int _eapMethod;

		private string[] _caCertificateID;

		private EAPMethodConfiguration _eapMethodConfiguration;

		private Dot1XConfigurationExtension _extension;

		[System.Xml.Serialization.XmlAnyAttributeAttribute()]
		public System.Xml.XmlAttribute[] anyAttr {
			get {
				return this._anyAttr;
			}
			set {
				this._anyAttr = value;
			}
		}

		/// <remarks>reqired, order 0</remarks>
		[System.Xml.Serialization.XmlElementAttribute("Dot1XConfigurationToken", Namespace = "http://www.onvif.org/ver10/schema", DataType = "string")]
		public string dot1XConfigurationToken {
			get {
				return this._dot1XConfigurationToken;
			}
			set {
				this._dot1XConfigurationToken = value;
			}
		}

		/// <remarks>reqired, order 1</remarks>
		[System.Xml.Serialization.XmlElementAttribute("Identity", Namespace = "http://www.onvif.org/ver10/schema", DataType = "string")]
		public string identity {
			get {
				return this._identity;
			}
			set {
				this._identity = value;
			}
		}

		/// <remarks>optional, order 2</remarks>
		[System.Xml.Serialization.XmlElementAttribute("AnonymousID", Namespace = "http://www.onvif.org/ver10/schema", DataType = "string")]
		public string anonymousID {
			get {
				return this._anonymousID;
			}
			set {
				this._anonymousID = value;
			}
		}

		/// <summary>
		/// EAP Method type as defined in
		/// .
		/// </summary>
		/// <remarks>reqired, order 3</remarks>
		[System.Xml.Serialization.XmlElementAttribute("EAPMethod", Namespace = "http://www.onvif.org/ver10/schema", DataType = "int")]
		public int eapMethod {
			get {
				return this._eapMethod;
			}
			set {
				this._eapMethod = value;
			}
		}

		/// <remarks>optional, order 4</remarks>
		[System.Xml.Serialization.XmlElementAttribute("CACertificateID", Namespace = "http://www.onvif.org/ver10/schema", DataType = "token")]
		public string[] caCertificateID {
			get {
				return this._caCertificateID;
			}
			set {
				this._caCertificateID = value;
			}
		}

		/// <remarks>optional, order 5</remarks>
		[System.Xml.Serialization.XmlElementAttribute("EAPMethodConfiguration", Namespace = "http://www.onvif.org/ver10/schema")]
		public EAPMethodConfiguration eapMethodConfiguration {
			get {
				return this._eapMethodConfiguration;
			}
			set {
				this._eapMethodConfiguration = value;
			}
		}

		/// <remarks>optional, order 6</remarks>
		[System.Xml.Serialization.XmlElementAttribute("Extension", Namespace = "http://www.onvif.org/ver10/schema")]
		public Dot1XConfigurationExtension extension {
			get {
				return this._extension;
			}
			set {
				this._extension = value;
			}
		}
	}

	[System.SerializableAttribute()]
	[System.Xml.Serialization.XmlTypeAttribute(TypeName = "EAPMethodConfiguration", Namespace = "http://www.onvif.org/ver10/schema")]
	public partial class EAPMethodConfiguration {

		private System.Xml.XmlAttribute[] _anyAttr;

		private TLSConfiguration _tlsConfiguration;

		private string _password;

		private EapMethodExtension _extension;

		[System.Xml.Serialization.XmlAnyAttributeAttribute()]
		public System.Xml.XmlAttribute[] anyAttr {
			get {
				return this._anyAttr;
			}
			set {
				this._anyAttr = value;
			}
		}

		/// <summary>
		/// Confgiuration information for TLS Method.
		/// </summary>
		/// <remarks>optional, order 0</remarks>
		[System.Xml.Serialization.XmlElementAttribute("TLSConfiguration", Namespace = "http://www.onvif.org/ver10/schema")]
		public TLSConfiguration tlsConfiguration {
			get {
				return this._tlsConfiguration;
			}
			set {
				this._tlsConfiguration = value;
			}
		}

		/// <summary>
		/// Password for those EAP Methods that require a password. The password shall never be returned on a get method.
		/// </summary>
		/// <remarks>optional, order 1</remarks>
		[System.Xml.Serialization.XmlElementAttribute("Password", Namespace = "http://www.onvif.org/ver10/schema", DataType = "string")]
		public string password {
			get {
				return this._password;
			}
			set {
				this._password = value;
			}
		}

		/// <remarks>optional, order 2</remarks>
		[System.Xml.Serialization.XmlElementAttribute("Extension", Namespace = "http://www.onvif.org/ver10/schema")]
		public EapMethodExtension extension {
			get {
				return this._extension;
			}
			set {
				this._extension = value;
			}
		}
	}

	[System.SerializableAttribute()]
	[System.Xml.Serialization.XmlTypeAttribute(TypeName = "TLSConfiguration", Namespace = "http://www.onvif.org/ver10/schema")]
	public partial class TLSConfiguration {

		private System.Xml.XmlAttribute[] _anyAttr;

		private string _certificateID;

		private System.Xml.XmlElement[] _any;

		[System.Xml.Serialization.XmlAnyAttributeAttribute()]
		public System.Xml.XmlAttribute[] anyAttr {
			get {
				return this._anyAttr;
			}
			set {
				this._anyAttr = value;
			}
		}

		/// <remarks>reqired, order 0</remarks>
		[System.Xml.Serialization.XmlElementAttribute("CertificateID", Namespace = "http://www.onvif.org/ver10/schema", DataType = "token")]
		public string certificateID {
			get {
				return this._certificateID;
			}
			set {
				this._certificateID = value;
			}
		}

		/// <remarks>optional, order 1, namespace ##any</remarks>
		[System.Xml.Serialization.XmlAnyElementAttribute()]
		public System.Xml.XmlElement[] any {
			get {
				return this._any;
			}
			set {
				this._any = value;
			}
		}
	}

	[System.SerializableAttribute()]
	[System.Xml.Serialization.XmlTypeAttribute(TypeName = "EapMethodExtension", Namespace = "http://www.onvif.org/ver10/schema")]
	public partial class EapMethodExtension {

		private System.Xml.XmlElement[] _any;

		/// <remarks>optional, order 0, namespace ##any</remarks>
		[System.Xml.Serialization.XmlAnyElementAttribute()]
		public System.Xml.XmlElement[] any {
			get {
				return this._any;
			}
			set {
				this._any = value;
			}
		}
	}

	[System.SerializableAttribute()]
	[System.Xml.Serialization.XmlTypeAttribute(TypeName = "Dot1XConfigurationExtension", Namespace = "http://www.onvif.org/ver10/schema")]
	public partial class Dot1XConfigurationExtension {

		private System.Xml.XmlElement[] _any;

		/// <remarks>optional, order 0, namespace ##any</remarks>
		[System.Xml.Serialization.XmlAnyElementAttribute()]
		public System.Xml.XmlElement[] any {
			get {
				return this._any;
			}
			set {
				this._any = value;
			}
		}
	}

	[System.SerializableAttribute()]
	[System.Xml.Serialization.XmlTypeAttribute(TypeName = "GenericEapPwdConfigurationExtension", Namespace = "http://www.onvif.org/ver10/schema")]
	public partial class GenericEapPwdConfigurationExtension {

		private System.Xml.XmlElement[] _any;

		/// <remarks>optional, order 0, namespace ##any</remarks>
		[System.Xml.Serialization.XmlAnyElementAttribute()]
		public System.Xml.XmlElement[] any {
			get {
				return this._any;
			}
			set {
				this._any = value;
			}
		}
	}

	[System.SerializableAttribute()]
	[System.Xml.Serialization.XmlTypeAttribute(TypeName = "RelayLogicalState", Namespace = "http://www.onvif.org/ver10/schema")]
	public enum RelayLogicalState {

		[System.Xml.Serialization.XmlEnumAttribute(Name = "active")]
		active,

		[System.Xml.Serialization.XmlEnumAttribute(Name = "inactive")]
		inactive,
	}

	[System.SerializableAttribute()]
	[System.Xml.Serialization.XmlTypeAttribute(TypeName = "RelayIdleState", Namespace = "http://www.onvif.org/ver10/schema")]
	public enum RelayIdleState {

		[System.Xml.Serialization.XmlEnumAttribute(Name = "closed")]
		closed,

		[System.Xml.Serialization.XmlEnumAttribute(Name = "open")]
		open,
	}

	[System.SerializableAttribute()]
	[System.Xml.Serialization.XmlTypeAttribute(TypeName = "RelayOutputSettings", Namespace = "http://www.onvif.org/ver10/schema")]
	public partial class RelayOutputSettings {

		private RelayMode _mode;

		private XsDuration _delayTime;

		private RelayIdleState _idleState;

		/// <summary>
		/// 'Bistable' or 'Monostable'
		/// </summary>
		/// <remarks>reqired, order 0</remarks>
		[System.Xml.Serialization.XmlElementAttribute("Mode", Namespace = "http://www.onvif.org/ver10/schema")]
		public RelayMode mode {
			get {
				return this._mode;
			}
			set {
				this._mode = value;
			}
		}

		/// <summary>
		/// Time after which the relay returns to its idle state if it is in monostable mode. If the Mode field is set to bistable mode the value of the parameter can be ignored.
		/// </summary>
		/// <remarks>reqired, order 1</remarks>
		[System.Xml.Serialization.XmlElementAttribute("DelayTime", Namespace = "http://www.onvif.org/ver10/schema")]
		public XsDuration delayTime {
			get {
				return this._delayTime;
			}
			set {
				this._delayTime = value;
			}
		}

		/// <summary>
		/// 'open' or 'closed'
		/// </summary>
		/// <remarks>reqired, order 2</remarks>
		[System.Xml.Serialization.XmlElementAttribute("IdleState", Namespace = "http://www.onvif.org/ver10/schema")]
		public RelayIdleState idleState {
			get {
				return this._idleState;
			}
			set {
				this._idleState = value;
			}
		}
	}

	[System.SerializableAttribute()]
	[System.Xml.Serialization.XmlTypeAttribute(TypeName = "RelayMode", Namespace = "http://www.onvif.org/ver10/schema")]
	public enum RelayMode {

		[System.Xml.Serialization.XmlEnumAttribute(Name = "Monostable")]
		monostable,

		[System.Xml.Serialization.XmlEnumAttribute(Name = "Bistable")]
		bistable,
	}

	[System.SerializableAttribute()]
	[System.Xml.Serialization.XmlTypeAttribute(TypeName = "RelayOutput", Namespace = "http://www.onvif.org/ver10/schema")]
	public partial class RelayOutput {

		private string _token;

		private System.Xml.XmlAttribute[] _anyAttr;

		private RelayOutputSettings _properties;

		private System.Xml.XmlElement[] _any;

		/// <summary>
		/// Unique identifier referencing the physical entity.
		/// </summary>
		[System.Xml.Serialization.XmlAttributeAttribute("token")]
		public string token {
			get {
				return this._token;
			}
			set {
				this._token = value;
			}
		}

		[System.Xml.Serialization.XmlAnyAttributeAttribute()]
		public System.Xml.XmlAttribute[] anyAttr {
			get {
				return this._anyAttr;
			}
			set {
				this._anyAttr = value;
			}
		}

		/// <remarks>reqired, order 0</remarks>
		[System.Xml.Serialization.XmlElementAttribute("Properties", Namespace = "http://www.onvif.org/ver10/schema")]
		public RelayOutputSettings properties {
			get {
				return this._properties;
			}
			set {
				this._properties = value;
			}
		}

		/// <remarks>optional, order 1, namespace ##any</remarks>
		[System.Xml.Serialization.XmlAnyElementAttribute()]
		public System.Xml.XmlElement[] any {
			get {
				return this._any;
			}
			set {
				this._any = value;
			}
		}
	}

	[System.SerializableAttribute()]
	[System.Xml.Serialization.XmlTypeAttribute(TypeName = "DigitalInput", Namespace = "http://www.onvif.org/ver10/schema")]
	public partial class DigitalInput {

		private string _token;

		private System.Xml.XmlAttribute[] _anyAttr;

		private System.Xml.XmlElement[] _any;

		/// <summary>
		/// Unique identifier referencing the physical entity.
		/// </summary>
		[System.Xml.Serialization.XmlAttributeAttribute("token")]
		public string token {
			get {
				return this._token;
			}
			set {
				this._token = value;
			}
		}

		[System.Xml.Serialization.XmlAnyAttributeAttribute()]
		public System.Xml.XmlAttribute[] anyAttr {
			get {
				return this._anyAttr;
			}
			set {
				this._anyAttr = value;
			}
		}

		/// <remarks>optional, order 0, namespace ##any</remarks>
		[System.Xml.Serialization.XmlAnyElementAttribute()]
		public System.Xml.XmlElement[] any {
			get {
				return this._any;
			}
			set {
				this._any = value;
			}
		}
	}

	[System.SerializableAttribute()]
	[System.Xml.Serialization.XmlTypeAttribute(TypeName = "PTZNode", Namespace = "http://www.onvif.org/ver10/schema")]
	public partial class PTZNode {

		private bool _fixedHomePosition;

		private bool _fixedHomePositionSpecified;

		private string _token;

		private System.Xml.XmlAttribute[] _anyAttr;

		private string _name;

		private PTZSpaces _supportedPTZSpaces;

		private int _maximumNumberOfPresets;

		private bool _homeSupported;

		private string[] _auxiliaryCommands;

		private PTZNodeExtension _extension;

		/// <summary>
		/// Indication whether the HomePosition of a Node is fixed or it can be changed via the SetHomePosition command.
		/// </summary>
		[System.Xml.Serialization.XmlAttributeAttribute("FixedHomePosition", DataType = "boolean")]
		public bool fixedHomePosition {
			get {
				return this._fixedHomePosition;
			}
			set {
				this._fixedHomePosition = value;
			}
		}

		[System.Xml.Serialization.XmlIgnoreAttribute()]
		public bool fixedHomePositionSpecified {
			get {
				return this._fixedHomePositionSpecified;
			}
			set {
				this._fixedHomePositionSpecified = value;
			}
		}

		/// <summary>
		/// Unique identifier referencing the physical entity.
		/// </summary>
		[System.Xml.Serialization.XmlAttributeAttribute("token")]
		public string token {
			get {
				return this._token;
			}
			set {
				this._token = value;
			}
		}

		[System.Xml.Serialization.XmlAnyAttributeAttribute()]
		public System.Xml.XmlAttribute[] anyAttr {
			get {
				return this._anyAttr;
			}
			set {
				this._anyAttr = value;
			}
		}

		/// <summary>
		/// A unique identifier that is used to reference PTZ Nodes.
		/// </summary>
		/// <remarks>optional, order 0</remarks>
		[System.Xml.Serialization.XmlElementAttribute("Name", Namespace = "http://www.onvif.org/ver10/schema", DataType = "string")]
		public string name {
			get {
				return this._name;
			}
			set {
				this._name = value;
			}
		}

		/// <summary>
		/// A list of Coordinate Systems available for the PTZ Node. For each Coordinate System, the PTZ Node MUST specify its allowed range.
		/// </summary>
		/// <remarks>reqired, order 1</remarks>
		[System.Xml.Serialization.XmlElementAttribute("SupportedPTZSpaces", Namespace = "http://www.onvif.org/ver10/schema")]
		public PTZSpaces supportedPTZSpaces {
			get {
				return this._supportedPTZSpaces;
			}
			set {
				this._supportedPTZSpaces = value;
			}
		}

		/// <summary>
		/// All preset operations MUST be available for this PTZ Node if one preset is supported.
		/// </summary>
		/// <remarks>reqired, order 2</remarks>
		[System.Xml.Serialization.XmlElementAttribute("MaximumNumberOfPresets", Namespace = "http://www.onvif.org/ver10/schema", DataType = "int")]
		public int maximumNumberOfPresets {
			get {
				return this._maximumNumberOfPresets;
			}
			set {
				this._maximumNumberOfPresets = value;
			}
		}

		/// <summary>
		/// A boolean operator specifying the availability of a home position. If set to true, the Home Position Operations MUST be available for this PTZ Node.
		/// </summary>
		/// <remarks>reqired, order 3</remarks>
		[System.Xml.Serialization.XmlElementAttribute("HomeSupported", Namespace = "http://www.onvif.org/ver10/schema", DataType = "boolean")]
		public bool homeSupported {
			get {
				return this._homeSupported;
			}
			set {
				this._homeSupported = value;
			}
		}

		/// <summary>
		/// A list of supported Auxiliary commands. If the list is not empty, the Auxiliary Operations MUST be available for this PTZ Node.
		/// </summary>
		/// <remarks>optional, order 4</remarks>
		[System.Xml.Serialization.XmlElementAttribute("AuxiliaryCommands", Namespace = "http://www.onvif.org/ver10/schema", DataType = "string")]
		public string[] auxiliaryCommands {
			get {
				return this._auxiliaryCommands;
			}
			set {
				this._auxiliaryCommands = value;
			}
		}

		/// <remarks>optional, order 5</remarks>
		[System.Xml.Serialization.XmlElementAttribute("Extension", Namespace = "http://www.onvif.org/ver10/schema")]
		public PTZNodeExtension extension {
			get {
				return this._extension;
			}
			set {
				this._extension = value;
			}
		}
	}

	[System.SerializableAttribute()]
	[System.Xml.Serialization.XmlTypeAttribute(TypeName = "PTZSpaces", Namespace = "http://www.onvif.org/ver10/schema")]
	public partial class PTZSpaces {

		private System.Xml.XmlAttribute[] _anyAttr;

		private Space2DDescription[] _absolutePanTiltPositionSpace;

		private Space1DDescription[] _absoluteZoomPositionSpace;

		private Space2DDescription[] _relativePanTiltTranslationSpace;

		private Space1DDescription[] _relativeZoomTranslationSpace;

		private Space2DDescription[] _continuousPanTiltVelocitySpace;

		private Space1DDescription[] _continuousZoomVelocitySpace;

		private Space1DDescription[] _panTiltSpeedSpace;

		private Space1DDescription[] _zoomSpeedSpace;

		private PTZSpacesExtension _extension;

		[System.Xml.Serialization.XmlAnyAttributeAttribute()]
		public System.Xml.XmlAttribute[] anyAttr {
			get {
				return this._anyAttr;
			}
			set {
				this._anyAttr = value;
			}
		}

		/// <summary>
		/// The Generic Pan/Tilt Position space is provided by every PTZ node that supports absolute Pan/Tilt, since it does not relate to a specific physical range.
		/// Instead, the range should be defined as the full range of the PTZ unit normalized to the range -1 to 1 resulting in the following space description.
		/// </summary>
		/// <remarks>optional, order 0</remarks>
		[System.Xml.Serialization.XmlElementAttribute("AbsolutePanTiltPositionSpace", Namespace = "http://www.onvif.org/ver10/schema")]
		public Space2DDescription[] absolutePanTiltPositionSpace {
			get {
				return this._absolutePanTiltPositionSpace;
			}
			set {
				this._absolutePanTiltPositionSpace = value;
			}
		}

		/// <summary>
		/// The Generic Zoom Position Space is provided by every PTZ node that supports absolute Zoom, since it does not relate to a specific physical range.
		/// Instead, the range should be defined as the full range of the Zoom normalized to the range 0 (wide) to 1 (tele).
		/// There is no assumption about how the generic zoom range is mapped to magnification, FOV or other physical zoom dimension.
		/// </summary>
		/// <remarks>optional, order 1</remarks>
		[System.Xml.Serialization.XmlElementAttribute("AbsoluteZoomPositionSpace", Namespace = "http://www.onvif.org/ver10/schema")]
		public Space1DDescription[] absoluteZoomPositionSpace {
			get {
				return this._absoluteZoomPositionSpace;
			}
			set {
				this._absoluteZoomPositionSpace = value;
			}
		}

		/// <summary>
		/// The Generic Pan/Tilt translation space is provided by every PTZ node that supports relative Pan/Tilt, since it does not relate to a specific physical range.
		/// Instead, the range should be defined as the full positive and negative translation range of the PTZ unit normalized to the range -1 to 1,
		/// where positive translation would mean clockwise rotation or movement in right/up direction resulting in the following space description.
		/// </summary>
		/// <remarks>optional, order 2</remarks>
		[System.Xml.Serialization.XmlElementAttribute("RelativePanTiltTranslationSpace", Namespace = "http://www.onvif.org/ver10/schema")]
		public Space2DDescription[] relativePanTiltTranslationSpace {
			get {
				return this._relativePanTiltTranslationSpace;
			}
			set {
				this._relativePanTiltTranslationSpace = value;
			}
		}

		/// <summary>
		/// The Generic Zoom Translation Space is provided by every PTZ node that supports relative Zoom, since it does not relate to a specific physical range.
		/// Instead, the corresponding absolute range should be defined as the full positive and negative translation range of the Zoom normalized to the range -1 to1,
		/// where a positive translation maps to a movement in TELE direction. The translation is signed to indicate direction (negative is to wide, positive is to tele).
		/// There is no assumption about how the generic zoom range is mapped to magnification, FOV or other physical zoom dimension. This results in the following space description.
		/// </summary>
		/// <remarks>optional, order 3</remarks>
		[System.Xml.Serialization.XmlElementAttribute("RelativeZoomTranslationSpace", Namespace = "http://www.onvif.org/ver10/schema")]
		public Space1DDescription[] relativeZoomTranslationSpace {
			get {
				return this._relativeZoomTranslationSpace;
			}
			set {
				this._relativeZoomTranslationSpace = value;
			}
		}

		/// <summary>
		/// The generic Pan/Tilt velocity space shall be provided by every PTZ node, since it does not relate to a specific physical range.
		/// Instead, the range should be defined as a range of the PTZ unit"s speed normalized to the range -1 to 1, where a positive velocity would map to clockwise
		/// rotation or movement in the right/up direction. A signed speed can be independently specified for the pan and tilt component resulting in the following space description.
		/// </summary>
		/// <remarks>optional, order 4</remarks>
		[System.Xml.Serialization.XmlElementAttribute("ContinuousPanTiltVelocitySpace", Namespace = "http://www.onvif.org/ver10/schema")]
		public Space2DDescription[] continuousPanTiltVelocitySpace {
			get {
				return this._continuousPanTiltVelocitySpace;
			}
			set {
				this._continuousPanTiltVelocitySpace = value;
			}
		}

		/// <summary>
		/// The generic zoom velocity space specifies a zoom factor velocity without knowing the underlying physical model. The range should be normalized from -1 to 1,
		/// where a positive velocity would map to TELE direction. A generic zoom velocity space description resembles the following.
		/// </summary>
		/// <remarks>optional, order 5</remarks>
		[System.Xml.Serialization.XmlElementAttribute("ContinuousZoomVelocitySpace", Namespace = "http://www.onvif.org/ver10/schema")]
		public Space1DDescription[] continuousZoomVelocitySpace {
			get {
				return this._continuousZoomVelocitySpace;
			}
			set {
				this._continuousZoomVelocitySpace = value;
			}
		}

		/// <summary>
		/// The speed space specifies the speed for a Pan/Tilt movement when moving to an absolute position or to a relative translation.
		/// In contrast to the velocity spaces, speed spaces do not contain any directional information. The speed of a combined Pan/Tilt
		/// movement is represented by a single non-negative scalar value.
		/// </summary>
		/// <remarks>optional, order 6</remarks>
		[System.Xml.Serialization.XmlElementAttribute("PanTiltSpeedSpace", Namespace = "http://www.onvif.org/ver10/schema")]
		public Space1DDescription[] panTiltSpeedSpace {
			get {
				return this._panTiltSpeedSpace;
			}
			set {
				this._panTiltSpeedSpace = value;
			}
		}

		/// <summary>
		/// The speed space specifies the speed for a Zoom movement when moving to an absolute position or to a relative translation.
		/// In contrast to the velocity spaces, speed spaces do not contain any directional information.
		/// </summary>
		/// <remarks>optional, order 7</remarks>
		[System.Xml.Serialization.XmlElementAttribute("ZoomSpeedSpace", Namespace = "http://www.onvif.org/ver10/schema")]
		public Space1DDescription[] zoomSpeedSpace {
			get {
				return this._zoomSpeedSpace;
			}
			set {
				this._zoomSpeedSpace = value;
			}
		}

		/// <remarks>optional, order 8</remarks>
		[System.Xml.Serialization.XmlElementAttribute("Extension", Namespace = "http://www.onvif.org/ver10/schema")]
		public PTZSpacesExtension extension {
			get {
				return this._extension;
			}
			set {
				this._extension = value;
			}
		}
	}

	[System.SerializableAttribute()]
	[System.Xml.Serialization.XmlTypeAttribute(TypeName = "PTZSpacesExtension", Namespace = "http://www.onvif.org/ver10/schema")]
	public partial class PTZSpacesExtension {

		private System.Xml.XmlElement[] _any;

		/// <remarks>optional, order 0, namespace ##any</remarks>
		[System.Xml.Serialization.XmlAnyElementAttribute()]
		public System.Xml.XmlElement[] any {
			get {
				return this._any;
			}
			set {
				this._any = value;
			}
		}
	}

	[System.SerializableAttribute()]
	[System.Xml.Serialization.XmlTypeAttribute(TypeName = "PTZNodeExtension", Namespace = "http://www.onvif.org/ver10/schema")]
	public partial class PTZNodeExtension {

		private System.Xml.XmlElement[] _any;

		private PTZPresetTourSupported _supportedPresetTour;

		private PTZNodeExtension2 _extension;

		/// <remarks>optional, order 0, namespace ##other</remarks>
		[System.Xml.Serialization.XmlAnyElementAttribute()]
		public System.Xml.XmlElement[] any {
			get {
				return this._any;
			}
			set {
				this._any = value;
			}
		}

		/// <summary>
		/// Detail of supported Preset Tour feature.
		/// </summary>
		/// <remarks>reqired, order 1</remarks>
		[System.Xml.Serialization.XmlElementAttribute("SupportedPresetTour", Namespace = "http://www.onvif.org/ver10/schema")]
		public PTZPresetTourSupported supportedPresetTour {
			get {
				return this._supportedPresetTour;
			}
			set {
				this._supportedPresetTour = value;
			}
		}

		/// <remarks>optional, order 2</remarks>
		[System.Xml.Serialization.XmlElementAttribute("Extension", Namespace = "http://www.onvif.org/ver10/schema")]
		public PTZNodeExtension2 extension {
			get {
				return this._extension;
			}
			set {
				this._extension = value;
			}
		}
	}

	[System.SerializableAttribute()]
	[System.Xml.Serialization.XmlTypeAttribute(TypeName = "PTZPresetTourSupported", Namespace = "http://www.onvif.org/ver10/schema")]
	public partial class PTZPresetTourSupported {

		private System.Xml.XmlAttribute[] _anyAttr;

		private int _maximumNumberOfPresetTours;

		private PTZPresetTourOperation[] _ptzPresetTourOperation;

		private PTZPresetTourSupportedExtension _extension;

		[System.Xml.Serialization.XmlAnyAttributeAttribute()]
		public System.Xml.XmlAttribute[] anyAttr {
			get {
				return this._anyAttr;
			}
			set {
				this._anyAttr = value;
			}
		}

		/// <summary>
		/// Indicates number of preset tours that can be created. Required preset tour operations shall be available for this PTZ Node if one or more preset tour is supported.
		/// </summary>
		/// <remarks>reqired, order 0</remarks>
		[System.Xml.Serialization.XmlElementAttribute("MaximumNumberOfPresetTours", Namespace = "http://www.onvif.org/ver10/schema", DataType = "int")]
		public int maximumNumberOfPresetTours {
			get {
				return this._maximumNumberOfPresetTours;
			}
			set {
				this._maximumNumberOfPresetTours = value;
			}
		}

		/// <summary>
		/// Indicates which preset tour operations are available for this PTZ Node.
		/// </summary>
		/// <remarks>optional, order 1</remarks>
		[System.Xml.Serialization.XmlElementAttribute("PTZPresetTourOperation", Namespace = "http://www.onvif.org/ver10/schema")]
		public PTZPresetTourOperation[] ptzPresetTourOperation {
			get {
				return this._ptzPresetTourOperation;
			}
			set {
				this._ptzPresetTourOperation = value;
			}
		}

		/// <remarks>optional, order 2</remarks>
		[System.Xml.Serialization.XmlElementAttribute("Extension", Namespace = "http://www.onvif.org/ver10/schema")]
		public PTZPresetTourSupportedExtension extension {
			get {
				return this._extension;
			}
			set {
				this._extension = value;
			}
		}
	}

	[System.SerializableAttribute()]
	[System.Xml.Serialization.XmlTypeAttribute(TypeName = "PTZPresetTourOperation", Namespace = "http://www.onvif.org/ver10/schema")]
	public enum PTZPresetTourOperation {

		[System.Xml.Serialization.XmlEnumAttribute(Name = "Start")]
		start,

		[System.Xml.Serialization.XmlEnumAttribute(Name = "Stop")]
		stop,

		[System.Xml.Serialization.XmlEnumAttribute(Name = "Pause")]
		pause,

		[System.Xml.Serialization.XmlEnumAttribute(Name = "Extended")]
		extended,
	}

	[System.SerializableAttribute()]
	[System.Xml.Serialization.XmlTypeAttribute(TypeName = "PTZPresetTourSupportedExtension", Namespace = "http://www.onvif.org/ver10/schema")]
	public partial class PTZPresetTourSupportedExtension {

		private System.Xml.XmlElement[] _any;

		/// <remarks>optional, order 0, namespace ##any</remarks>
		[System.Xml.Serialization.XmlAnyElementAttribute()]
		public System.Xml.XmlElement[] any {
			get {
				return this._any;
			}
			set {
				this._any = value;
			}
		}
	}

	[System.SerializableAttribute()]
	[System.Xml.Serialization.XmlTypeAttribute(TypeName = "PTZNodeExtension2", Namespace = "http://www.onvif.org/ver10/schema")]
	public partial class PTZNodeExtension2 {

		private System.Xml.XmlElement[] _any;

		/// <remarks>optional, order 0, namespace ##targetNamespace</remarks>
		[System.Xml.Serialization.XmlAnyElementAttribute(Namespace = "http://www.onvif.org/ver10/schema")]
		public System.Xml.XmlElement[] any {
			get {
				return this._any;
			}
			set {
				this._any = value;
			}
		}
	}

	[System.SerializableAttribute()]
	[System.Xml.Serialization.XmlTypeAttribute(TypeName = "PTZConfigurationOptions", Namespace = "http://www.onvif.org/ver10/schema")]
	public partial class PTZConfigurationOptions {

		private System.Xml.XmlAttribute[] _anyAttr;

		private PTZSpaces _spaces;

		private DurationRange _ptzTimeout;

		private System.Xml.XmlElement[] _any;

		private PTControlDirectionOptions _ptControlDirection;

		private PTZConfigurationOptions2 _extension;

		[System.Xml.Serialization.XmlAnyAttributeAttribute()]
		public System.Xml.XmlAttribute[] anyAttr {
			get {
				return this._anyAttr;
			}
			set {
				this._anyAttr = value;
			}
		}

		/// <summary>
		/// A list of supported coordinate systems including their range limitations.
		/// </summary>
		/// <remarks>reqired, order 0</remarks>
		[System.Xml.Serialization.XmlElementAttribute("Spaces", Namespace = "http://www.onvif.org/ver10/schema")]
		public PTZSpaces spaces {
			get {
				return this._spaces;
			}
			set {
				this._spaces = value;
			}
		}

		/// <summary>
		/// A timeout Range within which Timeouts are accepted by the PTZ Node.
		/// </summary>
		/// <remarks>reqired, order 1</remarks>
		[System.Xml.Serialization.XmlElementAttribute("PTZTimeout", Namespace = "http://www.onvif.org/ver10/schema")]
		public DurationRange ptzTimeout {
			get {
				return this._ptzTimeout;
			}
			set {
				this._ptzTimeout = value;
			}
		}

		/// <remarks>optional, order 2, namespace ##other</remarks>
		[System.Xml.Serialization.XmlAnyElementAttribute()]
		public System.Xml.XmlElement[] any {
			get {
				return this._any;
			}
			set {
				this._any = value;
			}
		}

		/// <summary>
		/// Supported options for PT Direction Control.
		/// </summary>
		/// <remarks>optional, order 3</remarks>
		[System.Xml.Serialization.XmlElementAttribute("PTControlDirection", Namespace = "http://www.onvif.org/ver10/schema")]
		public PTControlDirectionOptions ptControlDirection {
			get {
				return this._ptControlDirection;
			}
			set {
				this._ptControlDirection = value;
			}
		}

		/// <remarks>optional, order 4</remarks>
		[System.Xml.Serialization.XmlElementAttribute("Extension", Namespace = "http://www.onvif.org/ver10/schema")]
		public PTZConfigurationOptions2 extension {
			get {
				return this._extension;
			}
			set {
				this._extension = value;
			}
		}
	}

	[System.SerializableAttribute()]
	[System.Xml.Serialization.XmlTypeAttribute(TypeName = "PTControlDirectionOptions", Namespace = "http://www.onvif.org/ver10/schema")]
	public partial class PTControlDirectionOptions {

		private System.Xml.XmlAttribute[] _anyAttr;

		private EFlipOptions _eFlip;

		private ReverseOptions _reverse;

		private PTControlDirectionOptionsExtension _extension;

		[System.Xml.Serialization.XmlAnyAttributeAttribute()]
		public System.Xml.XmlAttribute[] anyAttr {
			get {
				return this._anyAttr;
			}
			set {
				this._anyAttr = value;
			}
		}

		/// <summary>
		/// Supported options for EFlip feature.
		/// </summary>
		/// <remarks>optional, order 0</remarks>
		[System.Xml.Serialization.XmlElementAttribute("EFlip", Namespace = "http://www.onvif.org/ver10/schema")]
		public EFlipOptions eFlip {
			get {
				return this._eFlip;
			}
			set {
				this._eFlip = value;
			}
		}

		/// <summary>
		/// Supported options for Reverse feature.
		/// </summary>
		/// <remarks>optional, order 1</remarks>
		[System.Xml.Serialization.XmlElementAttribute("Reverse", Namespace = "http://www.onvif.org/ver10/schema")]
		public ReverseOptions reverse {
			get {
				return this._reverse;
			}
			set {
				this._reverse = value;
			}
		}

		/// <remarks>optional, order 2</remarks>
		[System.Xml.Serialization.XmlElementAttribute("Extension", Namespace = "http://www.onvif.org/ver10/schema")]
		public PTControlDirectionOptionsExtension extension {
			get {
				return this._extension;
			}
			set {
				this._extension = value;
			}
		}
	}

	[System.SerializableAttribute()]
	[System.Xml.Serialization.XmlTypeAttribute(TypeName = "EFlipOptions", Namespace = "http://www.onvif.org/ver10/schema")]
	public partial class EFlipOptions {

		private System.Xml.XmlAttribute[] _anyAttr;

		private EFlipMode[] _mode;

		private EFlipOptionsExtension _extension;

		[System.Xml.Serialization.XmlAnyAttributeAttribute()]
		public System.Xml.XmlAttribute[] anyAttr {
			get {
				return this._anyAttr;
			}
			set {
				this._anyAttr = value;
			}
		}

		/// <summary>
		/// Options of EFlip mode parameter.
		/// </summary>
		/// <remarks>optional, order 0</remarks>
		[System.Xml.Serialization.XmlElementAttribute("Mode", Namespace = "http://www.onvif.org/ver10/schema")]
		public EFlipMode[] mode {
			get {
				return this._mode;
			}
			set {
				this._mode = value;
			}
		}

		/// <remarks>optional, order 1</remarks>
		[System.Xml.Serialization.XmlElementAttribute("Extension", Namespace = "http://www.onvif.org/ver10/schema")]
		public EFlipOptionsExtension extension {
			get {
				return this._extension;
			}
			set {
				this._extension = value;
			}
		}
	}

	[System.SerializableAttribute()]
	[System.Xml.Serialization.XmlTypeAttribute(TypeName = "EFlipOptionsExtension", Namespace = "http://www.onvif.org/ver10/schema")]
	public partial class EFlipOptionsExtension {

		private System.Xml.XmlElement[] _any;

		/// <remarks>optional, order 0, namespace ##any</remarks>
		[System.Xml.Serialization.XmlAnyElementAttribute()]
		public System.Xml.XmlElement[] any {
			get {
				return this._any;
			}
			set {
				this._any = value;
			}
		}
	}

	[System.SerializableAttribute()]
	[System.Xml.Serialization.XmlTypeAttribute(TypeName = "ReverseOptions", Namespace = "http://www.onvif.org/ver10/schema")]
	public partial class ReverseOptions {

		private System.Xml.XmlAttribute[] _anyAttr;

		private ReverseMode[] _mode;

		private ReverseOptionsExtension _extension;

		[System.Xml.Serialization.XmlAnyAttributeAttribute()]
		public System.Xml.XmlAttribute[] anyAttr {
			get {
				return this._anyAttr;
			}
			set {
				this._anyAttr = value;
			}
		}

		/// <summary>
		/// Options of Reverse mode parameter.
		/// </summary>
		/// <remarks>optional, order 0</remarks>
		[System.Xml.Serialization.XmlElementAttribute("Mode", Namespace = "http://www.onvif.org/ver10/schema")]
		public ReverseMode[] mode {
			get {
				return this._mode;
			}
			set {
				this._mode = value;
			}
		}

		/// <remarks>optional, order 1</remarks>
		[System.Xml.Serialization.XmlElementAttribute("Extension", Namespace = "http://www.onvif.org/ver10/schema")]
		public ReverseOptionsExtension extension {
			get {
				return this._extension;
			}
			set {
				this._extension = value;
			}
		}
	}

	[System.SerializableAttribute()]
	[System.Xml.Serialization.XmlTypeAttribute(TypeName = "ReverseOptionsExtension", Namespace = "http://www.onvif.org/ver10/schema")]
	public partial class ReverseOptionsExtension {

		private System.Xml.XmlElement[] _any;

		/// <remarks>optional, order 0, namespace ##any</remarks>
		[System.Xml.Serialization.XmlAnyElementAttribute()]
		public System.Xml.XmlElement[] any {
			get {
				return this._any;
			}
			set {
				this._any = value;
			}
		}
	}

	[System.SerializableAttribute()]
	[System.Xml.Serialization.XmlTypeAttribute(TypeName = "PTControlDirectionOptionsExtension", Namespace = "http://www.onvif.org/ver10/schema")]
	public partial class PTControlDirectionOptionsExtension {

		private System.Xml.XmlElement[] _any;

		/// <remarks>optional, order 0, namespace ##any</remarks>
		[System.Xml.Serialization.XmlAnyElementAttribute()]
		public System.Xml.XmlElement[] any {
			get {
				return this._any;
			}
			set {
				this._any = value;
			}
		}
	}

	[System.SerializableAttribute()]
	[System.Xml.Serialization.XmlTypeAttribute(TypeName = "PTZConfigurationOptions2", Namespace = "http://www.onvif.org/ver10/schema")]
	public partial class PTZConfigurationOptions2 {

		private System.Xml.XmlElement[] _any;

		/// <remarks>optional, order 0, namespace ##targetNamespace</remarks>
		[System.Xml.Serialization.XmlAnyElementAttribute(Namespace = "http://www.onvif.org/ver10/schema")]
		public System.Xml.XmlElement[] any {
			get {
				return this._any;
			}
			set {
				this._any = value;
			}
		}
	}

	[System.SerializableAttribute()]
	[System.Xml.Serialization.XmlTypeAttribute(TypeName = "PTZVector", Namespace = "http://www.onvif.org/ver10/schema")]
	public partial class PTZVector {

		private Vector2D _panTilt;

		private Vector1D _zoom;

		/// <summary>
		/// Pan and tilt position. The x component corresponds to pan and the y component to tilt.
		/// </summary>
		/// <remarks>optional, order 0</remarks>
		[System.Xml.Serialization.XmlElementAttribute("PanTilt", Namespace = "http://www.onvif.org/ver10/schema")]
		public Vector2D panTilt {
			get {
				return this._panTilt;
			}
			set {
				this._panTilt = value;
			}
		}

		/// <summary>
		/// A zoom position.
		/// </summary>
		/// <remarks>optional, order 1</remarks>
		[System.Xml.Serialization.XmlElementAttribute("Zoom", Namespace = "http://www.onvif.org/ver10/schema")]
		public Vector1D zoom {
			get {
				return this._zoom;
			}
			set {
				this._zoom = value;
			}
		}
	}

	[System.SerializableAttribute()]
	[System.Xml.Serialization.XmlTypeAttribute(TypeName = "PTZStatus", Namespace = "http://www.onvif.org/ver10/schema")]
	public partial class PTZStatus {

		private System.Xml.XmlAttribute[] _anyAttr;

		private PTZVector _position;

		private PTZMoveStatus _moveStatus;

		private string _error;

		private System.DateTime _utcTime;

		private System.Xml.XmlElement[] _any;

		[System.Xml.Serialization.XmlAnyAttributeAttribute()]
		public System.Xml.XmlAttribute[] anyAttr {
			get {
				return this._anyAttr;
			}
			set {
				this._anyAttr = value;
			}
		}

		/// <summary>
		/// Specifies the absolute position of the PTZ unit together with the Space references. The default absolute spaces of the corresponding PTZ configuration MUST be referenced within the Position element.
		/// </summary>
		/// <remarks>optional, order 0</remarks>
		[System.Xml.Serialization.XmlElementAttribute("Position", Namespace = "http://www.onvif.org/ver10/schema")]
		public PTZVector position {
			get {
				return this._position;
			}
			set {
				this._position = value;
			}
		}

		/// <summary>
		/// Indicates if the Pan/Tilt/Zoom device unit is currently moving, idle or in an unknown state.
		/// </summary>
		/// <remarks>optional, order 1</remarks>
		[System.Xml.Serialization.XmlElementAttribute("MoveStatus", Namespace = "http://www.onvif.org/ver10/schema")]
		public PTZMoveStatus moveStatus {
			get {
				return this._moveStatus;
			}
			set {
				this._moveStatus = value;
			}
		}

		/// <summary>
		/// States a current PTZ error.
		/// </summary>
		/// <remarks>optional, order 2</remarks>
		[System.Xml.Serialization.XmlElementAttribute("Error", Namespace = "http://www.onvif.org/ver10/schema", DataType = "string")]
		public string error {
			get {
				return this._error;
			}
			set {
				this._error = value;
			}
		}

		/// <summary>
		/// Specifies the UTC time when this status was generated.
		/// </summary>
		/// <remarks>reqired, order 3</remarks>
		[System.Xml.Serialization.XmlElementAttribute("UtcTime", Namespace = "http://www.onvif.org/ver10/schema", DataType = "dateTime")]
		public System.DateTime utcTime {
			get {
				return this._utcTime;
			}
			set {
				this._utcTime = value;
			}
		}

		/// <remarks>optional, order 4, namespace ##any</remarks>
		[System.Xml.Serialization.XmlAnyElementAttribute()]
		public System.Xml.XmlElement[] any {
			get {
				return this._any;
			}
			set {
				this._any = value;
			}
		}
	}

	[System.SerializableAttribute()]
	[System.Xml.Serialization.XmlTypeAttribute(TypeName = "PTZMoveStatus", Namespace = "http://www.onvif.org/ver10/schema")]
	public partial class PTZMoveStatus {

		private MoveStatus _panTilt;

		private bool _panTiltSpecified;

		private MoveStatus _zoom;

		private bool _zoomSpecified;

		/// <summary>
		/// </summary>
		/// <remarks>optional, order 0</remarks>
		[System.Xml.Serialization.XmlElementAttribute("PanTilt", Namespace = "http://www.onvif.org/ver10/schema")]
		public MoveStatus panTilt {
			get {
				return this._panTilt;
			}
			set {
				this._panTilt = value;
			}
		}

		[System.Xml.Serialization.XmlIgnoreAttribute()]
		public bool panTiltSpecified {
			get {
				return this._panTiltSpecified;
			}
			set {
				this._panTiltSpecified = value;
			}
		}

		/// <summary>
		/// </summary>
		/// <remarks>optional, order 1</remarks>
		[System.Xml.Serialization.XmlElementAttribute("Zoom", Namespace = "http://www.onvif.org/ver10/schema")]
		public MoveStatus zoom {
			get {
				return this._zoom;
			}
			set {
				this._zoom = value;
			}
		}

		[System.Xml.Serialization.XmlIgnoreAttribute()]
		public bool zoomSpecified {
			get {
				return this._zoomSpecified;
			}
			set {
				this._zoomSpecified = value;
			}
		}
	}

	[System.SerializableAttribute()]
	[System.Xml.Serialization.XmlTypeAttribute(TypeName = "MoveStatus", Namespace = "http://www.onvif.org/ver10/schema")]
	public enum MoveStatus {

		[System.Xml.Serialization.XmlEnumAttribute(Name = "IDLE")]
		idle,

		[System.Xml.Serialization.XmlEnumAttribute(Name = "MOVING")]
		moving,

		[System.Xml.Serialization.XmlEnumAttribute(Name = "UNKNOWN")]
		unknown,
	}

	[System.SerializableAttribute()]
	[System.Xml.Serialization.XmlTypeAttribute(TypeName = "PTZPreset", Namespace = "http://www.onvif.org/ver10/schema")]
	public partial class PTZPreset {

		private string _token;

		private System.Xml.XmlAttribute[] _anyAttr;

		private string _name;

		private PTZVector _ptzPosition;

		/// <summary>
		/// </summary>
		[System.Xml.Serialization.XmlAttributeAttribute("token")]
		public string token {
			get {
				return this._token;
			}
			set {
				this._token = value;
			}
		}

		[System.Xml.Serialization.XmlAnyAttributeAttribute()]
		public System.Xml.XmlAttribute[] anyAttr {
			get {
				return this._anyAttr;
			}
			set {
				this._anyAttr = value;
			}
		}

		/// <summary>
		/// A list of preset position name.
		/// </summary>
		/// <remarks>optional, order 0</remarks>
		[System.Xml.Serialization.XmlElementAttribute("Name", Namespace = "http://www.onvif.org/ver10/schema", DataType = "string")]
		public string name {
			get {
				return this._name;
			}
			set {
				this._name = value;
			}
		}

		/// <summary>
		/// A list of preset position.
		/// </summary>
		/// <remarks>optional, order 1</remarks>
		[System.Xml.Serialization.XmlElementAttribute("PTZPosition", Namespace = "http://www.onvif.org/ver10/schema")]
		public PTZVector ptzPosition {
			get {
				return this._ptzPosition;
			}
			set {
				this._ptzPosition = value;
			}
		}
	}

	[System.SerializableAttribute()]
	[System.Xml.Serialization.XmlTypeAttribute(TypeName = "PTZPresetTourState", Namespace = "http://www.onvif.org/ver10/schema")]
	public enum PTZPresetTourState {

		[System.Xml.Serialization.XmlEnumAttribute(Name = "Idle")]
		idle,

		[System.Xml.Serialization.XmlEnumAttribute(Name = "Touring")]
		touring,

		[System.Xml.Serialization.XmlEnumAttribute(Name = "Paused")]
		paused,

		[System.Xml.Serialization.XmlEnumAttribute(Name = "Extended")]
		extended,
	}

	[System.SerializableAttribute()]
	[System.Xml.Serialization.XmlTypeAttribute(TypeName = "PTZPresetTourDirection", Namespace = "http://www.onvif.org/ver10/schema")]
	public enum PTZPresetTourDirection {

		[System.Xml.Serialization.XmlEnumAttribute(Name = "Forward")]
		forward,

		[System.Xml.Serialization.XmlEnumAttribute(Name = "Backward")]
		backward,

		[System.Xml.Serialization.XmlEnumAttribute(Name = "Extended")]
		extended,
	}
	[System.SerializableAttribute()]
	[System.Xml.Serialization.XmlTypeAttribute(TypeName = "PresetTour", Namespace = "http://www.onvif.org/ver10/schema")]
	public partial class PresetTour {

		private string _token;

		private System.Xml.XmlAttribute[] _anyAttr;

		private string _name;

		private PTZPresetTourStatus _status;

		private bool _autoStart;

		private PTZPresetTourStartingCondition _startingCondition;

		private PTZPresetTourSpot[] _tourSpot;

		private PTZPresetTourExtension _extension;

		/// <summary>
		/// Unique identifier of this preset tour.
		/// </summary>
		[System.Xml.Serialization.XmlAttributeAttribute("token")]
		public string token {
			get {
				return this._token;
			}
			set {
				this._token = value;
			}
		}

		[System.Xml.Serialization.XmlAnyAttributeAttribute()]
		public System.Xml.XmlAttribute[] anyAttr {
			get {
				return this._anyAttr;
			}
			set {
				this._anyAttr = value;
			}
		}

		/// <summary>
		/// Readable name of the preset tour.
		/// </summary>
		/// <remarks>optional, order 0</remarks>
		[System.Xml.Serialization.XmlElementAttribute("Name", Namespace = "http://www.onvif.org/ver10/schema", DataType = "string")]
		public string name {
			get {
				return this._name;
			}
			set {
				this._name = value;
			}
		}

		/// <summary>
		/// Read only parameters to indicate the status of the preset tour.
		/// </summary>
		/// <remarks>reqired, order 1</remarks>
		[System.Xml.Serialization.XmlElementAttribute("Status", Namespace = "http://www.onvif.org/ver10/schema")]
		public PTZPresetTourStatus status {
			get {
				return this._status;
			}
			set {
				this._status = value;
			}
		}

		/// <summary>
		/// Auto Start flag of the preset tour. True allows the preset tour to be activated always.
		/// </summary>
		/// <remarks>reqired, order 2</remarks>
		[System.Xml.Serialization.XmlElementAttribute("AutoStart", Namespace = "http://www.onvif.org/ver10/schema", DataType = "boolean")]
		public bool autoStart {
			get {
				return this._autoStart;
			}
			set {
				this._autoStart = value;
			}
		}

		/// <summary>
		/// Parameters to specify the detail behavior of the preset tour.
		/// </summary>
		/// <remarks>reqired, order 3</remarks>
		[System.Xml.Serialization.XmlElementAttribute("StartingCondition", Namespace = "http://www.onvif.org/ver10/schema")]
		public PTZPresetTourStartingCondition startingCondition {
			get {
				return this._startingCondition;
			}
			set {
				this._startingCondition = value;
			}
		}

		/// <summary>
		/// A list of detail of touring spots including preset positions.
		/// </summary>
		/// <remarks>optional, order 4</remarks>
		[System.Xml.Serialization.XmlElementAttribute("TourSpot", Namespace = "http://www.onvif.org/ver10/schema")]
		public PTZPresetTourSpot[] tourSpot {
			get {
				return this._tourSpot;
			}
			set {
				this._tourSpot = value;
			}
		}

		/// <remarks>optional, order 5</remarks>
		[System.Xml.Serialization.XmlElementAttribute("Extension", Namespace = "http://www.onvif.org/ver10/schema")]
		public PTZPresetTourExtension extension {
			get {
				return this._extension;
			}
			set {
				this._extension = value;
			}
		}
	}

	[System.SerializableAttribute()]
	[System.Xml.Serialization.XmlTypeAttribute(TypeName = "PTZPresetTourStatus", Namespace = "http://www.onvif.org/ver10/schema")]
	public partial class PTZPresetTourStatus {

		private System.Xml.XmlAttribute[] _anyAttr;

		private PTZPresetTourState _state;

		private PTZPresetTourSpot _currentTourSpot;

		private PTZPresetTourStatusExtension _extension;

		[System.Xml.Serialization.XmlAnyAttributeAttribute()]
		public System.Xml.XmlAttribute[] anyAttr {
			get {
				return this._anyAttr;
			}
			set {
				this._anyAttr = value;
			}
		}

		/// <summary>
		/// Indicates state of this preset tour by Idle/Touring/Paused.
		/// </summary>
		/// <remarks>reqired, order 0</remarks>
		[System.Xml.Serialization.XmlElementAttribute("State", Namespace = "http://www.onvif.org/ver10/schema")]
		public PTZPresetTourState state {
			get {
				return this._state;
			}
			set {
				this._state = value;
			}
		}

		/// <summary>
		/// Indicates a tour spot currently staying.
		/// </summary>
		/// <remarks>optional, order 1</remarks>
		[System.Xml.Serialization.XmlElementAttribute("CurrentTourSpot", Namespace = "http://www.onvif.org/ver10/schema")]
		public PTZPresetTourSpot currentTourSpot {
			get {
				return this._currentTourSpot;
			}
			set {
				this._currentTourSpot = value;
			}
		}

		/// <remarks>optional, order 2</remarks>
		[System.Xml.Serialization.XmlElementAttribute("Extension", Namespace = "http://www.onvif.org/ver10/schema")]
		public PTZPresetTourStatusExtension extension {
			get {
				return this._extension;
			}
			set {
				this._extension = value;
			}
		}
	}

	[System.SerializableAttribute()]
	[System.Xml.Serialization.XmlTypeAttribute(TypeName = "PTZPresetTourSpot", Namespace = "http://www.onvif.org/ver10/schema")]
	public partial class PTZPresetTourSpot {

		private System.Xml.XmlAttribute[] _anyAttr;

		private PTZPresetTourPresetDetail _presetDetail;

		private PTZSpeed _speed;

		private XsDuration _stayTime;

		private PTZPresetTourSpotExtension _extension;

		[System.Xml.Serialization.XmlAnyAttributeAttribute()]
		public System.Xml.XmlAttribute[] anyAttr {
			get {
				return this._anyAttr;
			}
			set {
				this._anyAttr = value;
			}
		}

		/// <summary>
		/// Detail definition of preset position of the tour spot.
		/// </summary>
		/// <remarks>reqired, order 0</remarks>
		[System.Xml.Serialization.XmlElementAttribute("PresetDetail", Namespace = "http://www.onvif.org/ver10/schema")]
		public PTZPresetTourPresetDetail presetDetail {
			get {
				return this._presetDetail;
			}
			set {
				this._presetDetail = value;
			}
		}

		/// <summary>
		/// Optional parameter to specify Pan/Tilt and Zoom speed on moving toward this tour spot.
		/// </summary>
		/// <remarks>optional, order 1</remarks>
		[System.Xml.Serialization.XmlElementAttribute("Speed", Namespace = "http://www.onvif.org/ver10/schema")]
		public PTZSpeed speed {
			get {
				return this._speed;
			}
			set {
				this._speed = value;
			}
		}

		/// <summary>
		/// Optional parameter to specify time duration of staying on this tour sport.
		/// </summary>
		/// <remarks>optional, order 2</remarks>
		[System.Xml.Serialization.XmlElementAttribute("StayTime", Namespace = "http://www.onvif.org/ver10/schema")]
		public XsDuration stayTime {
			get {
				return this._stayTime;
			}
			set {
				this._stayTime = value;
			}
		}

		/// <remarks>optional, order 3</remarks>
		[System.Xml.Serialization.XmlElementAttribute("Extension", Namespace = "http://www.onvif.org/ver10/schema")]
		public PTZPresetTourSpotExtension extension {
			get {
				return this._extension;
			}
			set {
				this._extension = value;
			}
		}
	}

	[System.SerializableAttribute()]
	[System.Xml.Serialization.XmlTypeAttribute(TypeName = "PTZPresetTourPresetDetail", Namespace = "http://www.onvif.org/ver10/schema")]
	public partial class PTZPresetTourPresetDetail {

		private System.Xml.XmlAttribute[] _anyAttr;

		private object _item;

		private System.Xml.XmlElement[] _any;

		[System.Xml.Serialization.XmlAnyAttributeAttribute()]
		public System.Xml.XmlAttribute[] anyAttr {
			get {
				return this._anyAttr;
			}
			set {
				this._anyAttr = value;
			}
		}

		/// <summary>
		/// PresetToken of type <see cref="System.String"/>: 
		/// Option to specify the preset position with Preset Token defined in advance.
		/// Home of type <see cref="System.Boolean"/>: 
		/// Option to specify the preset position with the home position of this PTZ Node. "False" to this parameter shall be treated as an invalid argument.
		/// PTZPosition of type <see cref="onvif.types.PTZVector"/>: 
		/// Option to specify the preset position with vector of PTZ node directly.
		/// TypeExtension of type <see cref="onvif.types.PTZPresetTourTypeExtension"/>: 
		/// </summary>
		/// <remarks>reqired, order 0</remarks>
		[System.Xml.Serialization.XmlElementAttribute("PresetToken", Namespace = "http://www.onvif.org/ver10/schema", Type = typeof(string))]
		[System.Xml.Serialization.XmlElementAttribute("Home", Namespace = "http://www.onvif.org/ver10/schema", Type = typeof(bool))]
		[System.Xml.Serialization.XmlElementAttribute("PTZPosition", Namespace = "http://www.onvif.org/ver10/schema", Type = typeof(PTZVector))]
		[System.Xml.Serialization.XmlElementAttribute("TypeExtension", Namespace = "http://www.onvif.org/ver10/schema", Type = typeof(PTZPresetTourTypeExtension))]
		public object item {
			get {
				return this._item;
			}
			set {
				this._item = value;
			}
		}

		/// <remarks>optional, order 1, namespace ##any</remarks>
		[System.Xml.Serialization.XmlAnyElementAttribute()]
		public System.Xml.XmlElement[] any {
			get {
				return this._any;
			}
			set {
				this._any = value;
			}
		}
	}

	[System.SerializableAttribute()]
	[System.Xml.Serialization.XmlTypeAttribute(TypeName = "PTZPresetTourTypeExtension", Namespace = "http://www.onvif.org/ver10/schema")]
	public partial class PTZPresetTourTypeExtension {

		private System.Xml.XmlElement[] _any;

		/// <remarks>optional, order 0, namespace ##any</remarks>
		[System.Xml.Serialization.XmlAnyElementAttribute()]
		public System.Xml.XmlElement[] any {
			get {
				return this._any;
			}
			set {
				this._any = value;
			}
		}
	}

	[System.SerializableAttribute()]
	[System.Xml.Serialization.XmlTypeAttribute(TypeName = "PTZPresetTourSpotExtension", Namespace = "http://www.onvif.org/ver10/schema")]
	public partial class PTZPresetTourSpotExtension {

		private System.Xml.XmlElement[] _any;

		/// <remarks>optional, order 0, namespace ##any</remarks>
		[System.Xml.Serialization.XmlAnyElementAttribute()]
		public System.Xml.XmlElement[] any {
			get {
				return this._any;
			}
			set {
				this._any = value;
			}
		}
	}

	[System.SerializableAttribute()]
	[System.Xml.Serialization.XmlTypeAttribute(TypeName = "PTZPresetTourStatusExtension", Namespace = "http://www.onvif.org/ver10/schema")]
	public partial class PTZPresetTourStatusExtension {

		private System.Xml.XmlElement[] _any;

		/// <remarks>optional, order 0, namespace ##any</remarks>
		[System.Xml.Serialization.XmlAnyElementAttribute()]
		public System.Xml.XmlElement[] any {
			get {
				return this._any;
			}
			set {
				this._any = value;
			}
		}
	}

	[System.SerializableAttribute()]
	[System.Xml.Serialization.XmlTypeAttribute(TypeName = "PTZPresetTourStartingCondition", Namespace = "http://www.onvif.org/ver10/schema")]
	public partial class PTZPresetTourStartingCondition {

		private System.Xml.XmlAttribute[] _anyAttr;

		private int _recurringTime;

		private bool _recurringTimeSpecified;

		private XsDuration _recurringDuration;

		private PTZPresetTourDirection _direction;

		private bool _directionSpecified;

		private PTZPresetTourStartingConditionExtension _extension;

		[System.Xml.Serialization.XmlAnyAttributeAttribute()]
		public System.Xml.XmlAttribute[] anyAttr {
			get {
				return this._anyAttr;
			}
			set {
				this._anyAttr = value;
			}
		}

		/// <summary>
		/// Optional parameter to specify how many times the preset tour is recurred.
		/// </summary>
		/// <remarks>optional, order 0</remarks>
		[System.Xml.Serialization.XmlElementAttribute("RecurringTime", Namespace = "http://www.onvif.org/ver10/schema", DataType = "int")]
		public int recurringTime {
			get {
				return this._recurringTime;
			}
			set {
				this._recurringTime = value;
			}
		}

		[System.Xml.Serialization.XmlIgnoreAttribute()]
		public bool recurringTimeSpecified {
			get {
				return this._recurringTimeSpecified;
			}
			set {
				this._recurringTimeSpecified = value;
			}
		}

		/// <summary>
		/// Optional parameter to specify how long time duration the preset tour is recurred.
		/// </summary>
		/// <remarks>optional, order 1</remarks>
		[System.Xml.Serialization.XmlElementAttribute("RecurringDuration", Namespace = "http://www.onvif.org/ver10/schema")]
		public XsDuration recurringDuration {
			get {
				return this._recurringDuration;
			}
			set {
				this._recurringDuration = value;
			}
		}

		/// <summary>
		/// Optional parameter to choose which direction the preset tour goes. Forward shall be chosen in case it is omitted.
		/// </summary>
		/// <remarks>optional, order 2</remarks>
		[System.Xml.Serialization.XmlElementAttribute("Direction", Namespace = "http://www.onvif.org/ver10/schema")]
		public PTZPresetTourDirection direction {
			get {
				return this._direction;
			}
			set {
				this._direction = value;
			}
		}

		[System.Xml.Serialization.XmlIgnoreAttribute()]
		public bool directionSpecified {
			get {
				return this._directionSpecified;
			}
			set {
				this._directionSpecified = value;
			}
		}

		/// <remarks>optional, order 3</remarks>
		[System.Xml.Serialization.XmlElementAttribute("Extension", Namespace = "http://www.onvif.org/ver10/schema")]
		public PTZPresetTourStartingConditionExtension extension {
			get {
				return this._extension;
			}
			set {
				this._extension = value;
			}
		}
	}

	[System.SerializableAttribute()]
	[System.Xml.Serialization.XmlTypeAttribute(TypeName = "PTZPresetTourStartingConditionExtension", Namespace = "http://www.onvif.org/ver10/schema")]
	public partial class PTZPresetTourStartingConditionExtension {

		private System.Xml.XmlElement[] _any;

		/// <remarks>optional, order 0, namespace ##any</remarks>
		[System.Xml.Serialization.XmlAnyElementAttribute()]
		public System.Xml.XmlElement[] any {
			get {
				return this._any;
			}
			set {
				this._any = value;
			}
		}
	}

	[System.SerializableAttribute()]
	[System.Xml.Serialization.XmlTypeAttribute(TypeName = "PTZPresetTourExtension", Namespace = "http://www.onvif.org/ver10/schema")]
	public partial class PTZPresetTourExtension {

		private System.Xml.XmlElement[] _any;

		/// <remarks>optional, order 0, namespace ##any</remarks>
		[System.Xml.Serialization.XmlAnyElementAttribute()]
		public System.Xml.XmlElement[] any {
			get {
				return this._any;
			}
			set {
				this._any = value;
			}
		}
	}

	[System.SerializableAttribute()]
	[System.Xml.Serialization.XmlTypeAttribute(TypeName = "PTZPresetTourOptions", Namespace = "http://www.onvif.org/ver10/schema")]
	public partial class PTZPresetTourOptions {

		private System.Xml.XmlAttribute[] _anyAttr;

		private bool _autoStart;

		private PTZPresetTourStartingConditionOptions _startingCondition;

		private PTZPresetTourSpotOptions _tourSpot;

		private System.Xml.XmlElement[] _any;

		[System.Xml.Serialization.XmlAnyAttributeAttribute()]
		public System.Xml.XmlAttribute[] anyAttr {
			get {
				return this._anyAttr;
			}
			set {
				this._anyAttr = value;
			}
		}

		/// <summary>
		/// Indicates whether or not the AutoStart is supported.
		/// </summary>
		/// <remarks>reqired, order 0</remarks>
		[System.Xml.Serialization.XmlElementAttribute("AutoStart", Namespace = "http://www.onvif.org/ver10/schema", DataType = "boolean")]
		public bool autoStart {
			get {
				return this._autoStart;
			}
			set {
				this._autoStart = value;
			}
		}

		/// <summary>
		/// Supported options for Preset Tour Starting Condition.
		/// </summary>
		/// <remarks>reqired, order 1</remarks>
		[System.Xml.Serialization.XmlElementAttribute("StartingCondition", Namespace = "http://www.onvif.org/ver10/schema")]
		public PTZPresetTourStartingConditionOptions startingCondition {
			get {
				return this._startingCondition;
			}
			set {
				this._startingCondition = value;
			}
		}

		/// <summary>
		/// Supported options for Preset Tour Spot.
		/// </summary>
		/// <remarks>reqired, order 2</remarks>
		[System.Xml.Serialization.XmlElementAttribute("TourSpot", Namespace = "http://www.onvif.org/ver10/schema")]
		public PTZPresetTourSpotOptions tourSpot {
			get {
				return this._tourSpot;
			}
			set {
				this._tourSpot = value;
			}
		}

		/// <remarks>optional, order 3, namespace ##any</remarks>
		[System.Xml.Serialization.XmlAnyElementAttribute()]
		public System.Xml.XmlElement[] any {
			get {
				return this._any;
			}
			set {
				this._any = value;
			}
		}
	}

	[System.SerializableAttribute()]
	[System.Xml.Serialization.XmlTypeAttribute(TypeName = "PTZPresetTourStartingConditionOptions", Namespace = "http://www.onvif.org/ver10/schema")]
	public partial class PTZPresetTourStartingConditionOptions {

		private System.Xml.XmlAttribute[] _anyAttr;

		private IntRange _recurringTime;

		private DurationRange _recurringDuration;

		private PTZPresetTourDirection[] _direction;

		private PTZPresetTourStartingConditionOptionsExtension _extension;

		[System.Xml.Serialization.XmlAnyAttributeAttribute()]
		public System.Xml.XmlAttribute[] anyAttr {
			get {
				return this._anyAttr;
			}
			set {
				this._anyAttr = value;
			}
		}

		/// <summary>
		/// Supported range of Recurring Time.
		/// </summary>
		/// <remarks>optional, order 0</remarks>
		[System.Xml.Serialization.XmlElementAttribute("RecurringTime", Namespace = "http://www.onvif.org/ver10/schema")]
		public IntRange recurringTime {
			get {
				return this._recurringTime;
			}
			set {
				this._recurringTime = value;
			}
		}

		/// <summary>
		/// Supported range of Recurring Duration.
		/// </summary>
		/// <remarks>optional, order 1</remarks>
		[System.Xml.Serialization.XmlElementAttribute("RecurringDuration", Namespace = "http://www.onvif.org/ver10/schema")]
		public DurationRange recurringDuration {
			get {
				return this._recurringDuration;
			}
			set {
				this._recurringDuration = value;
			}
		}

		/// <summary>
		/// Supported options for Direction of Preset Tour.
		/// </summary>
		/// <remarks>optional, order 2</remarks>
		[System.Xml.Serialization.XmlElementAttribute("Direction", Namespace = "http://www.onvif.org/ver10/schema")]
		public PTZPresetTourDirection[] direction {
			get {
				return this._direction;
			}
			set {
				this._direction = value;
			}
		}

		/// <remarks>optional, order 3</remarks>
		[System.Xml.Serialization.XmlElementAttribute("Extension", Namespace = "http://www.onvif.org/ver10/schema")]
		public PTZPresetTourStartingConditionOptionsExtension extension {
			get {
				return this._extension;
			}
			set {
				this._extension = value;
			}
		}
	}

	[System.SerializableAttribute()]
	[System.Xml.Serialization.XmlTypeAttribute(TypeName = "PTZPresetTourStartingConditionOptionsExtension", Namespace = "http://www.onvif.org/ver10/schema")]
	public partial class PTZPresetTourStartingConditionOptionsExtension {

		private System.Xml.XmlElement[] _any;

		/// <remarks>optional, order 0, namespace ##any</remarks>
		[System.Xml.Serialization.XmlAnyElementAttribute()]
		public System.Xml.XmlElement[] any {
			get {
				return this._any;
			}
			set {
				this._any = value;
			}
		}
	}

	[System.SerializableAttribute()]
	[System.Xml.Serialization.XmlTypeAttribute(TypeName = "PTZPresetTourSpotOptions", Namespace = "http://www.onvif.org/ver10/schema")]
	public partial class PTZPresetTourSpotOptions {

		private System.Xml.XmlAttribute[] _anyAttr;

		private PTZPresetTourPresetDetailOptions _presetDetail;

		private DurationRange _stayTime;

		private System.Xml.XmlElement[] _any;

		[System.Xml.Serialization.XmlAnyAttributeAttribute()]
		public System.Xml.XmlAttribute[] anyAttr {
			get {
				return this._anyAttr;
			}
			set {
				this._anyAttr = value;
			}
		}

		/// <summary>
		/// Supported options for detail definition of preset position of the tour spot.
		/// </summary>
		/// <remarks>reqired, order 0</remarks>
		[System.Xml.Serialization.XmlElementAttribute("PresetDetail", Namespace = "http://www.onvif.org/ver10/schema")]
		public PTZPresetTourPresetDetailOptions presetDetail {
			get {
				return this._presetDetail;
			}
			set {
				this._presetDetail = value;
			}
		}

		/// <summary>
		/// Supported range of stay time for a tour spot.
		/// </summary>
		/// <remarks>reqired, order 1</remarks>
		[System.Xml.Serialization.XmlElementAttribute("StayTime", Namespace = "http://www.onvif.org/ver10/schema")]
		public DurationRange stayTime {
			get {
				return this._stayTime;
			}
			set {
				this._stayTime = value;
			}
		}

		/// <remarks>optional, order 2, namespace ##any</remarks>
		[System.Xml.Serialization.XmlAnyElementAttribute()]
		public System.Xml.XmlElement[] any {
			get {
				return this._any;
			}
			set {
				this._any = value;
			}
		}
	}

	[System.SerializableAttribute()]
	[System.Xml.Serialization.XmlTypeAttribute(TypeName = "PTZPresetTourPresetDetailOptions", Namespace = "http://www.onvif.org/ver10/schema")]
	public partial class PTZPresetTourPresetDetailOptions {

		private System.Xml.XmlAttribute[] _anyAttr;

		private string[] _presetToken;

		private bool _home;

		private bool _homeSpecified;

		private Space2DDescription _panTiltPositionSpace;

		private Space1DDescription _zoomPositionSpace;

		private PTZPresetTourPresetDetailOptionsExtension _extension;

		[System.Xml.Serialization.XmlAnyAttributeAttribute()]
		public System.Xml.XmlAttribute[] anyAttr {
			get {
				return this._anyAttr;
			}
			set {
				this._anyAttr = value;
			}
		}

		/// <summary>
		/// A list of available Preset Tokens for tour spots.
		/// </summary>
		/// <remarks>optional, order 0</remarks>
		[System.Xml.Serialization.XmlElementAttribute("PresetToken", Namespace = "http://www.onvif.org/ver10/schema", DataType = "string")]
		public string[] presetToken {
			get {
				return this._presetToken;
			}
			set {
				this._presetToken = value;
			}
		}

		/// <summary>
		/// An option to indicate Home postion for tour spots.
		/// </summary>
		/// <remarks>optional, order 1</remarks>
		[System.Xml.Serialization.XmlElementAttribute("Home", Namespace = "http://www.onvif.org/ver10/schema", DataType = "boolean")]
		public bool home {
			get {
				return this._home;
			}
			set {
				this._home = value;
			}
		}

		[System.Xml.Serialization.XmlIgnoreAttribute()]
		public bool homeSpecified {
			get {
				return this._homeSpecified;
			}
			set {
				this._homeSpecified = value;
			}
		}

		/// <summary>
		/// Supported range of Pan and Tilt for tour spots.
		/// </summary>
		/// <remarks>optional, order 2</remarks>
		[System.Xml.Serialization.XmlElementAttribute("PanTiltPositionSpace", Namespace = "http://www.onvif.org/ver10/schema")]
		public Space2DDescription panTiltPositionSpace {
			get {
				return this._panTiltPositionSpace;
			}
			set {
				this._panTiltPositionSpace = value;
			}
		}

		/// <summary>
		/// Supported range of Zoom for a tour spot.
		/// </summary>
		/// <remarks>optional, order 3</remarks>
		[System.Xml.Serialization.XmlElementAttribute("ZoomPositionSpace", Namespace = "http://www.onvif.org/ver10/schema")]
		public Space1DDescription zoomPositionSpace {
			get {
				return this._zoomPositionSpace;
			}
			set {
				this._zoomPositionSpace = value;
			}
		}

		/// <remarks>optional, order 4</remarks>
		[System.Xml.Serialization.XmlElementAttribute("Extension", Namespace = "http://www.onvif.org/ver10/schema")]
		public PTZPresetTourPresetDetailOptionsExtension extension {
			get {
				return this._extension;
			}
			set {
				this._extension = value;
			}
		}
	}

	[System.SerializableAttribute()]
	[System.Xml.Serialization.XmlTypeAttribute(TypeName = "PTZPresetTourPresetDetailOptionsExtension", Namespace = "http://www.onvif.org/ver10/schema")]
	public partial class PTZPresetTourPresetDetailOptionsExtension {

		private System.Xml.XmlElement[] _any;

		/// <remarks>optional, order 0, namespace ##any</remarks>
		[System.Xml.Serialization.XmlAnyElementAttribute()]
		public System.Xml.XmlElement[] any {
			get {
				return this._any;
			}
			set {
				this._any = value;
			}
		}
	}

	[System.SerializableAttribute()]
	[System.Xml.Serialization.XmlTypeAttribute(TypeName = "ImagingStatus", Namespace = "http://www.onvif.org/ver10/schema")]
	public partial class ImagingStatus {

		private System.Xml.XmlAttribute[] _anyAttr;

		private FocusStatus _focusStatus;

		private System.Xml.XmlElement[] _any;

		[System.Xml.Serialization.XmlAnyAttributeAttribute()]
		public System.Xml.XmlAttribute[] anyAttr {
			get {
				return this._anyAttr;
			}
			set {
				this._anyAttr = value;
			}
		}

		/// <remarks>reqired, order 0</remarks>
		[System.Xml.Serialization.XmlElementAttribute("FocusStatus", Namespace = "http://www.onvif.org/ver10/schema")]
		public FocusStatus focusStatus {
			get {
				return this._focusStatus;
			}
			set {
				this._focusStatus = value;
			}
		}

		/// <remarks>optional, order 1, namespace ##any</remarks>
		[System.Xml.Serialization.XmlAnyElementAttribute()]
		public System.Xml.XmlElement[] any {
			get {
				return this._any;
			}
			set {
				this._any = value;
			}
		}
	}

	[System.SerializableAttribute()]
	[System.Xml.Serialization.XmlTypeAttribute(TypeName = "FocusStatus", Namespace = "http://www.onvif.org/ver10/schema")]
	public partial class FocusStatus {

		private System.Xml.XmlAttribute[] _anyAttr;

		private float _position;

		private MoveStatus _moveStatus;

		private string _error;

		private System.Xml.XmlElement[] _any;

		[System.Xml.Serialization.XmlAnyAttributeAttribute()]
		public System.Xml.XmlAttribute[] anyAttr {
			get {
				return this._anyAttr;
			}
			set {
				this._anyAttr = value;
			}
		}

		/// <summary>
		/// Status of focus position.
		/// </summary>
		/// <remarks>reqired, order 0</remarks>
		[System.Xml.Serialization.XmlElementAttribute("Position", Namespace = "http://www.onvif.org/ver10/schema", DataType = "float")]
		public float position {
			get {
				return this._position;
			}
			set {
				this._position = value;
			}
		}

		/// <summary>
		/// Status of focus MoveStatus.
		/// </summary>
		/// <remarks>reqired, order 1</remarks>
		[System.Xml.Serialization.XmlElementAttribute("MoveStatus", Namespace = "http://www.onvif.org/ver10/schema")]
		public MoveStatus moveStatus {
			get {
				return this._moveStatus;
			}
			set {
				this._moveStatus = value;
			}
		}

		/// <summary>
		/// Error status of focus.
		/// </summary>
		/// <remarks>reqired, order 2</remarks>
		[System.Xml.Serialization.XmlElementAttribute("Error", Namespace = "http://www.onvif.org/ver10/schema", DataType = "string")]
		public string error {
			get {
				return this._error;
			}
			set {
				this._error = value;
			}
		}

		/// <remarks>optional, order 3, namespace ##any</remarks>
		[System.Xml.Serialization.XmlAnyElementAttribute()]
		public System.Xml.XmlElement[] any {
			get {
				return this._any;
			}
			set {
				this._any = value;
			}
		}
	}

	[System.SerializableAttribute()]
	[System.Xml.Serialization.XmlTypeAttribute(TypeName = "ImagingOptions", Namespace = "http://www.onvif.org/ver10/schema")]
	public partial class ImagingOptions {

		private System.Xml.XmlAttribute[] _anyAttr;

		private BacklightCompensationOptions _backlightCompensation;

		private FloatRange _brightness;

		private FloatRange _colorSaturation;

		private FloatRange _contrast;

		private ExposureOptions _exposure;

		private FocusOptions _focus;

		private IrCutFilterMode[] _irCutFilterModes;

		private FloatRange _sharpness;

		private WideDynamicRangeOptions _wideDynamicRange;

		private WhiteBalanceOptions _whiteBalance;

		private System.Xml.XmlElement[] _any;

		[System.Xml.Serialization.XmlAnyAttributeAttribute()]
		public System.Xml.XmlAttribute[] anyAttr {
			get {
				return this._anyAttr;
			}
			set {
				this._anyAttr = value;
			}
		}

		/// <remarks>reqired, order 0</remarks>
		[System.Xml.Serialization.XmlElementAttribute("BacklightCompensation", Namespace = "http://www.onvif.org/ver10/schema")]
		public BacklightCompensationOptions backlightCompensation {
			get {
				return this._backlightCompensation;
			}
			set {
				this._backlightCompensation = value;
			}
		}

		/// <remarks>reqired, order 1</remarks>
		[System.Xml.Serialization.XmlElementAttribute("Brightness", Namespace = "http://www.onvif.org/ver10/schema")]
		public FloatRange brightness {
			get {
				return this._brightness;
			}
			set {
				this._brightness = value;
			}
		}

		/// <remarks>reqired, order 2</remarks>
		[System.Xml.Serialization.XmlElementAttribute("ColorSaturation", Namespace = "http://www.onvif.org/ver10/schema")]
		public FloatRange colorSaturation {
			get {
				return this._colorSaturation;
			}
			set {
				this._colorSaturation = value;
			}
		}

		/// <remarks>reqired, order 3</remarks>
		[System.Xml.Serialization.XmlElementAttribute("Contrast", Namespace = "http://www.onvif.org/ver10/schema")]
		public FloatRange contrast {
			get {
				return this._contrast;
			}
			set {
				this._contrast = value;
			}
		}

		/// <remarks>reqired, order 4</remarks>
		[System.Xml.Serialization.XmlElementAttribute("Exposure", Namespace = "http://www.onvif.org/ver10/schema")]
		public ExposureOptions exposure {
			get {
				return this._exposure;
			}
			set {
				this._exposure = value;
			}
		}

		/// <remarks>reqired, order 5</remarks>
		[System.Xml.Serialization.XmlElementAttribute("Focus", Namespace = "http://www.onvif.org/ver10/schema")]
		public FocusOptions focus {
			get {
				return this._focus;
			}
			set {
				this._focus = value;
			}
		}

		/// <remarks>reqired, order 6</remarks>
		[System.Xml.Serialization.XmlElementAttribute("IrCutFilterModes", Namespace = "http://www.onvif.org/ver10/schema")]
		public IrCutFilterMode[] irCutFilterModes {
			get {
				return this._irCutFilterModes;
			}
			set {
				this._irCutFilterModes = value;
			}
		}

		/// <remarks>reqired, order 7</remarks>
		[System.Xml.Serialization.XmlElementAttribute("Sharpness", Namespace = "http://www.onvif.org/ver10/schema")]
		public FloatRange sharpness {
			get {
				return this._sharpness;
			}
			set {
				this._sharpness = value;
			}
		}

		/// <remarks>reqired, order 8</remarks>
		[System.Xml.Serialization.XmlElementAttribute("WideDynamicRange", Namespace = "http://www.onvif.org/ver10/schema")]
		public WideDynamicRangeOptions wideDynamicRange {
			get {
				return this._wideDynamicRange;
			}
			set {
				this._wideDynamicRange = value;
			}
		}

		/// <remarks>reqired, order 9</remarks>
		[System.Xml.Serialization.XmlElementAttribute("WhiteBalance", Namespace = "http://www.onvif.org/ver10/schema")]
		public WhiteBalanceOptions whiteBalance {
			get {
				return this._whiteBalance;
			}
			set {
				this._whiteBalance = value;
			}
		}

		/// <remarks>optional, order 10, namespace ##any</remarks>
		[System.Xml.Serialization.XmlAnyElementAttribute()]
		public System.Xml.XmlElement[] any {
			get {
				return this._any;
			}
			set {
				this._any = value;
			}
		}
	}

	[System.SerializableAttribute()]
	[System.Xml.Serialization.XmlTypeAttribute(TypeName = "BacklightCompensationOptions", Namespace = "http://www.onvif.org/ver10/schema")]
	public partial class BacklightCompensationOptions {

		private WideDynamicMode[] _mode;

		private FloatRange _level;

		/// <remarks>reqired, order 0</remarks>
		[System.Xml.Serialization.XmlElementAttribute("Mode", Namespace = "http://www.onvif.org/ver10/schema")]
		public WideDynamicMode[] mode {
			get {
				return this._mode;
			}
			set {
				this._mode = value;
			}
		}

		/// <remarks>reqired, order 1</remarks>
		[System.Xml.Serialization.XmlElementAttribute("Level", Namespace = "http://www.onvif.org/ver10/schema")]
		public FloatRange level {
			get {
				return this._level;
			}
			set {
				this._level = value;
			}
		}
	}

	[System.SerializableAttribute()]
	[System.Xml.Serialization.XmlTypeAttribute(TypeName = "ExposureOptions", Namespace = "http://www.onvif.org/ver10/schema")]
	public partial class ExposureOptions {

		private ExposureMode[] _mode;

		private ExposurePriority[] _priority;

		private FloatRange _minExposureTime;

		private FloatRange _maxExposureTime;

		private FloatRange _minGain;

		private FloatRange _maxGain;

		private FloatRange _minIris;

		private FloatRange _maxIris;

		private FloatRange _exposureTime;

		private FloatRange _gain;

		private FloatRange _iris;

		/// <remarks>reqired, order 0</remarks>
		[System.Xml.Serialization.XmlElementAttribute("Mode", Namespace = "http://www.onvif.org/ver10/schema")]
		public ExposureMode[] mode {
			get {
				return this._mode;
			}
			set {
				this._mode = value;
			}
		}

		/// <remarks>reqired, order 1</remarks>
		[System.Xml.Serialization.XmlElementAttribute("Priority", Namespace = "http://www.onvif.org/ver10/schema")]
		public ExposurePriority[] priority {
			get {
				return this._priority;
			}
			set {
				this._priority = value;
			}
		}

		/// <remarks>reqired, order 2</remarks>
		[System.Xml.Serialization.XmlElementAttribute("MinExposureTime", Namespace = "http://www.onvif.org/ver10/schema")]
		public FloatRange minExposureTime {
			get {
				return this._minExposureTime;
			}
			set {
				this._minExposureTime = value;
			}
		}

		/// <remarks>reqired, order 3</remarks>
		[System.Xml.Serialization.XmlElementAttribute("MaxExposureTime", Namespace = "http://www.onvif.org/ver10/schema")]
		public FloatRange maxExposureTime {
			get {
				return this._maxExposureTime;
			}
			set {
				this._maxExposureTime = value;
			}
		}

		/// <remarks>reqired, order 4</remarks>
		[System.Xml.Serialization.XmlElementAttribute("MinGain", Namespace = "http://www.onvif.org/ver10/schema")]
		public FloatRange minGain {
			get {
				return this._minGain;
			}
			set {
				this._minGain = value;
			}
		}

		/// <remarks>reqired, order 5</remarks>
		[System.Xml.Serialization.XmlElementAttribute("MaxGain", Namespace = "http://www.onvif.org/ver10/schema")]
		public FloatRange maxGain {
			get {
				return this._maxGain;
			}
			set {
				this._maxGain = value;
			}
		}

		/// <remarks>reqired, order 6</remarks>
		[System.Xml.Serialization.XmlElementAttribute("MinIris", Namespace = "http://www.onvif.org/ver10/schema")]
		public FloatRange minIris {
			get {
				return this._minIris;
			}
			set {
				this._minIris = value;
			}
		}

		/// <remarks>reqired, order 7</remarks>
		[System.Xml.Serialization.XmlElementAttribute("MaxIris", Namespace = "http://www.onvif.org/ver10/schema")]
		public FloatRange maxIris {
			get {
				return this._maxIris;
			}
			set {
				this._maxIris = value;
			}
		}

		/// <remarks>reqired, order 8</remarks>
		[System.Xml.Serialization.XmlElementAttribute("ExposureTime", Namespace = "http://www.onvif.org/ver10/schema")]
		public FloatRange exposureTime {
			get {
				return this._exposureTime;
			}
			set {
				this._exposureTime = value;
			}
		}

		/// <remarks>reqired, order 9</remarks>
		[System.Xml.Serialization.XmlElementAttribute("Gain", Namespace = "http://www.onvif.org/ver10/schema")]
		public FloatRange gain {
			get {
				return this._gain;
			}
			set {
				this._gain = value;
			}
		}

		/// <remarks>reqired, order 10</remarks>
		[System.Xml.Serialization.XmlElementAttribute("Iris", Namespace = "http://www.onvif.org/ver10/schema")]
		public FloatRange iris {
			get {
				return this._iris;
			}
			set {
				this._iris = value;
			}
		}
	}

	[System.SerializableAttribute()]
	[System.Xml.Serialization.XmlTypeAttribute(TypeName = "FocusOptions", Namespace = "http://www.onvif.org/ver10/schema")]
	public partial class FocusOptions {

		private AutoFocusMode[] _autoFocusModes;

		private FloatRange _defaultSpeed;

		private FloatRange _nearLimit;

		private FloatRange _farLimit;

		/// <remarks>optional, order 0</remarks>
		[System.Xml.Serialization.XmlElementAttribute("AutoFocusModes", Namespace = "http://www.onvif.org/ver10/schema")]
		public AutoFocusMode[] autoFocusModes {
			get {
				return this._autoFocusModes;
			}
			set {
				this._autoFocusModes = value;
			}
		}

		/// <remarks>reqired, order 1</remarks>
		[System.Xml.Serialization.XmlElementAttribute("DefaultSpeed", Namespace = "http://www.onvif.org/ver10/schema")]
		public FloatRange defaultSpeed {
			get {
				return this._defaultSpeed;
			}
			set {
				this._defaultSpeed = value;
			}
		}

		/// <remarks>reqired, order 2</remarks>
		[System.Xml.Serialization.XmlElementAttribute("NearLimit", Namespace = "http://www.onvif.org/ver10/schema")]
		public FloatRange nearLimit {
			get {
				return this._nearLimit;
			}
			set {
				this._nearLimit = value;
			}
		}

		/// <remarks>reqired, order 3</remarks>
		[System.Xml.Serialization.XmlElementAttribute("FarLimit", Namespace = "http://www.onvif.org/ver10/schema")]
		public FloatRange farLimit {
			get {
				return this._farLimit;
			}
			set {
				this._farLimit = value;
			}
		}
	}

	[System.SerializableAttribute()]
	[System.Xml.Serialization.XmlTypeAttribute(TypeName = "WideDynamicRangeOptions", Namespace = "http://www.onvif.org/ver10/schema")]
	public partial class WideDynamicRangeOptions {

		private WideDynamicMode[] _mode;

		private FloatRange _level;

		/// <remarks>reqired, order 0</remarks>
		[System.Xml.Serialization.XmlElementAttribute("Mode", Namespace = "http://www.onvif.org/ver10/schema")]
		public WideDynamicMode[] mode {
			get {
				return this._mode;
			}
			set {
				this._mode = value;
			}
		}

		/// <remarks>reqired, order 1</remarks>
		[System.Xml.Serialization.XmlElementAttribute("Level", Namespace = "http://www.onvif.org/ver10/schema")]
		public FloatRange level {
			get {
				return this._level;
			}
			set {
				this._level = value;
			}
		}
	}

	[System.SerializableAttribute()]
	[System.Xml.Serialization.XmlTypeAttribute(TypeName = "WhiteBalanceOptions", Namespace = "http://www.onvif.org/ver10/schema")]
	public partial class WhiteBalanceOptions {

		private WhiteBalanceMode[] _mode;

		private FloatRange _yrGain;

		private FloatRange _ybGain;

		/// <remarks>reqired, order 0</remarks>
		[System.Xml.Serialization.XmlElementAttribute("Mode", Namespace = "http://www.onvif.org/ver10/schema")]
		public WhiteBalanceMode[] mode {
			get {
				return this._mode;
			}
			set {
				this._mode = value;
			}
		}

		/// <remarks>reqired, order 1</remarks>
		[System.Xml.Serialization.XmlElementAttribute("YrGain", Namespace = "http://www.onvif.org/ver10/schema")]
		public FloatRange yrGain {
			get {
				return this._yrGain;
			}
			set {
				this._yrGain = value;
			}
		}

		/// <remarks>reqired, order 2</remarks>
		[System.Xml.Serialization.XmlElementAttribute("YbGain", Namespace = "http://www.onvif.org/ver10/schema")]
		public FloatRange ybGain {
			get {
				return this._ybGain;
			}
			set {
				this._ybGain = value;
			}
		}
	}

	[System.SerializableAttribute()]
	[System.Xml.Serialization.XmlTypeAttribute(TypeName = "FocusMove", Namespace = "http://www.onvif.org/ver10/schema")]
	public partial class FocusMove {

		private AbsoluteFocus _absolute;

		private RelativeFocus _relative;

		private ContinuousFocus _continuous;

		/// <summary>
		/// Parameters for the absolute focus control.
		/// </summary>
		/// <remarks>optional, order 0</remarks>
		[System.Xml.Serialization.XmlElementAttribute("Absolute", Namespace = "http://www.onvif.org/ver10/schema")]
		public AbsoluteFocus absolute {
			get {
				return this._absolute;
			}
			set {
				this._absolute = value;
			}
		}

		/// <summary>
		/// Parameters for the relative focus control.
		/// </summary>
		/// <remarks>optional, order 1</remarks>
		[System.Xml.Serialization.XmlElementAttribute("Relative", Namespace = "http://www.onvif.org/ver10/schema")]
		public RelativeFocus relative {
			get {
				return this._relative;
			}
			set {
				this._relative = value;
			}
		}

		/// <summary>
		/// Parameter for the continuous focus control.
		/// </summary>
		/// <remarks>optional, order 2</remarks>
		[System.Xml.Serialization.XmlElementAttribute("Continuous", Namespace = "http://www.onvif.org/ver10/schema")]
		public ContinuousFocus continuous {
			get {
				return this._continuous;
			}
			set {
				this._continuous = value;
			}
		}
	}

	[System.SerializableAttribute()]
	[System.Xml.Serialization.XmlTypeAttribute(TypeName = "AbsoluteFocus", Namespace = "http://www.onvif.org/ver10/schema")]
	public partial class AbsoluteFocus {

		private float _position;

		private float _speed;

		private bool _speedSpecified;

		/// <summary>
		/// Position parameter for the absolute focus control.
		/// </summary>
		/// <remarks>reqired, order 0</remarks>
		[System.Xml.Serialization.XmlElementAttribute("Position", Namespace = "http://www.onvif.org/ver10/schema", DataType = "float")]
		public float position {
			get {
				return this._position;
			}
			set {
				this._position = value;
			}
		}

		/// <summary>
		/// Speed parameter for the absolute focus control.
		/// </summary>
		/// <remarks>optional, order 1</remarks>
		[System.Xml.Serialization.XmlElementAttribute("Speed", Namespace = "http://www.onvif.org/ver10/schema", DataType = "float")]
		public float speed {
			get {
				return this._speed;
			}
			set {
				this._speed = value;
			}
		}

		[System.Xml.Serialization.XmlIgnoreAttribute()]
		public bool speedSpecified {
			get {
				return this._speedSpecified;
			}
			set {
				this._speedSpecified = value;
			}
		}
	}

	[System.SerializableAttribute()]
	[System.Xml.Serialization.XmlTypeAttribute(TypeName = "RelativeFocus", Namespace = "http://www.onvif.org/ver10/schema")]
	public partial class RelativeFocus {

		private float _distance;

		private float _speed;

		private bool _speedSpecified;

		/// <summary>
		/// Distance parameter for the relative focus control.
		/// </summary>
		/// <remarks>reqired, order 0</remarks>
		[System.Xml.Serialization.XmlElementAttribute("Distance", Namespace = "http://www.onvif.org/ver10/schema", DataType = "float")]
		public float distance {
			get {
				return this._distance;
			}
			set {
				this._distance = value;
			}
		}

		/// <summary>
		/// Speed parameter for the relative focus control.
		/// </summary>
		/// <remarks>optional, order 1</remarks>
		[System.Xml.Serialization.XmlElementAttribute("Speed", Namespace = "http://www.onvif.org/ver10/schema", DataType = "float")]
		public float speed {
			get {
				return this._speed;
			}
			set {
				this._speed = value;
			}
		}

		[System.Xml.Serialization.XmlIgnoreAttribute()]
		public bool speedSpecified {
			get {
				return this._speedSpecified;
			}
			set {
				this._speedSpecified = value;
			}
		}
	}

	[System.SerializableAttribute()]
	[System.Xml.Serialization.XmlTypeAttribute(TypeName = "ContinuousFocus", Namespace = "http://www.onvif.org/ver10/schema")]
	public partial class ContinuousFocus {

		private float _speed;

		/// <summary>
		/// Speed parameter for the Continuous focus control.
		/// </summary>
		/// <remarks>reqired, order 0</remarks>
		[System.Xml.Serialization.XmlElementAttribute("Speed", Namespace = "http://www.onvif.org/ver10/schema", DataType = "float")]
		public float speed {
			get {
				return this._speed;
			}
			set {
				this._speed = value;
			}
		}
	}

	[System.SerializableAttribute()]
	[System.Xml.Serialization.XmlTypeAttribute(TypeName = "MoveOptions", Namespace = "http://www.onvif.org/ver10/schema")]
	public partial class MoveOptions {

		private AbsoluteFocusOptions _absolute;

		private RelativeFocusOptions _relative;

		private ContinuousFocusOptions _continuous;

		/// <remarks>optional, order 0</remarks>
		[System.Xml.Serialization.XmlElementAttribute("Absolute", Namespace = "http://www.onvif.org/ver10/schema")]
		public AbsoluteFocusOptions absolute {
			get {
				return this._absolute;
			}
			set {
				this._absolute = value;
			}
		}

		/// <remarks>optional, order 1</remarks>
		[System.Xml.Serialization.XmlElementAttribute("Relative", Namespace = "http://www.onvif.org/ver10/schema")]
		public RelativeFocusOptions relative {
			get {
				return this._relative;
			}
			set {
				this._relative = value;
			}
		}

		/// <remarks>optional, order 2</remarks>
		[System.Xml.Serialization.XmlElementAttribute("Continuous", Namespace = "http://www.onvif.org/ver10/schema")]
		public ContinuousFocusOptions continuous {
			get {
				return this._continuous;
			}
			set {
				this._continuous = value;
			}
		}
	}

	[System.SerializableAttribute()]
	[System.Xml.Serialization.XmlTypeAttribute(TypeName = "AbsoluteFocusOptions", Namespace = "http://www.onvif.org/ver10/schema")]
	public partial class AbsoluteFocusOptions {

		private FloatRange _position;

		private FloatRange _speed;

		/// <summary>
		/// Valid ranges of the position.
		/// </summary>
		/// <remarks>reqired, order 0</remarks>
		[System.Xml.Serialization.XmlElementAttribute("Position", Namespace = "http://www.onvif.org/ver10/schema")]
		public FloatRange position {
			get {
				return this._position;
			}
			set {
				this._position = value;
			}
		}

		/// <summary>
		/// Valid ranges of the speed.
		/// </summary>
		/// <remarks>optional, order 1</remarks>
		[System.Xml.Serialization.XmlElementAttribute("Speed", Namespace = "http://www.onvif.org/ver10/schema")]
		public FloatRange speed {
			get {
				return this._speed;
			}
			set {
				this._speed = value;
			}
		}
	}

	[System.SerializableAttribute()]
	[System.Xml.Serialization.XmlTypeAttribute(TypeName = "RelativeFocusOptions", Namespace = "http://www.onvif.org/ver10/schema")]
	public partial class RelativeFocusOptions {

		private FloatRange _distance;

		private FloatRange _speed;

		/// <summary>
		/// Valid ranges of the distance.
		/// </summary>
		/// <remarks>reqired, order 0</remarks>
		[System.Xml.Serialization.XmlElementAttribute("Distance", Namespace = "http://www.onvif.org/ver10/schema")]
		public FloatRange distance {
			get {
				return this._distance;
			}
			set {
				this._distance = value;
			}
		}

		/// <summary>
		/// Valid ranges of the speed.
		/// </summary>
		/// <remarks>reqired, order 1</remarks>
		[System.Xml.Serialization.XmlElementAttribute("Speed", Namespace = "http://www.onvif.org/ver10/schema")]
		public FloatRange speed {
			get {
				return this._speed;
			}
			set {
				this._speed = value;
			}
		}
	}

	[System.SerializableAttribute()]
	[System.Xml.Serialization.XmlTypeAttribute(TypeName = "ContinuousFocusOptions", Namespace = "http://www.onvif.org/ver10/schema")]
	public partial class ContinuousFocusOptions {

		private FloatRange _speed;

		/// <summary>
		/// Valid ranges of the speed.
		/// </summary>
		/// <remarks>reqired, order 0</remarks>
		[System.Xml.Serialization.XmlElementAttribute("Speed", Namespace = "http://www.onvif.org/ver10/schema")]
		public FloatRange speed {
			get {
				return this._speed;
			}
			set {
				this._speed = value;
			}
		}
	}

	[System.SerializableAttribute()]
	[System.Xml.Serialization.XmlTypeAttribute(TypeName = "Enabled", Namespace = "http://www.onvif.org/ver10/schema")]
	public enum Enabled {

		[System.Xml.Serialization.XmlEnumAttribute(Name = "ENABLED")]
		enabled,

		[System.Xml.Serialization.XmlEnumAttribute(Name = "DISABLED")]
		disabled,
	}

	[System.SerializableAttribute()]
	[System.Xml.Serialization.XmlTypeAttribute(TypeName = "ImagingStatus20", Namespace = "http://www.onvif.org/ver10/schema")]
	public partial class ImagingStatus20 {

		private System.Xml.XmlAttribute[] _anyAttr;

		private FocusStatus20 _focusStatus20;

		private ImagingStatus20Extension _extension;

		[System.Xml.Serialization.XmlAnyAttributeAttribute()]
		public System.Xml.XmlAttribute[] anyAttr {
			get {
				return this._anyAttr;
			}
			set {
				this._anyAttr = value;
			}
		}

		/// <summary>
		/// Status of focus.
		/// </summary>
		/// <remarks>optional, order 0</remarks>
		[System.Xml.Serialization.XmlElementAttribute("FocusStatus20", Namespace = "http://www.onvif.org/ver10/schema")]
		public FocusStatus20 focusStatus20 {
			get {
				return this._focusStatus20;
			}
			set {
				this._focusStatus20 = value;
			}
		}

		/// <remarks>optional, order 1</remarks>
		[System.Xml.Serialization.XmlElementAttribute("Extension", Namespace = "http://www.onvif.org/ver10/schema")]
		public ImagingStatus20Extension extension {
			get {
				return this._extension;
			}
			set {
				this._extension = value;
			}
		}
	}

	[System.SerializableAttribute()]
	[System.Xml.Serialization.XmlTypeAttribute(TypeName = "FocusStatus20", Namespace = "http://www.onvif.org/ver10/schema")]
	public partial class FocusStatus20 {

		private System.Xml.XmlAttribute[] _anyAttr;

		private float _position;

		private MoveStatus _moveStatus;

		private string _error;

		private FocusStatus20Extension _extension;

		[System.Xml.Serialization.XmlAnyAttributeAttribute()]
		public System.Xml.XmlAttribute[] anyAttr {
			get {
				return this._anyAttr;
			}
			set {
				this._anyAttr = value;
			}
		}

		/// <summary>
		/// Status of focus position.
		/// </summary>
		/// <remarks>reqired, order 0</remarks>
		[System.Xml.Serialization.XmlElementAttribute("Position", Namespace = "http://www.onvif.org/ver10/schema", DataType = "float")]
		public float position {
			get {
				return this._position;
			}
			set {
				this._position = value;
			}
		}

		/// <summary>
		/// Status of focus MoveStatus.
		/// </summary>
		/// <remarks>reqired, order 1</remarks>
		[System.Xml.Serialization.XmlElementAttribute("MoveStatus", Namespace = "http://www.onvif.org/ver10/schema")]
		public MoveStatus moveStatus {
			get {
				return this._moveStatus;
			}
			set {
				this._moveStatus = value;
			}
		}

		/// <summary>
		/// Error status of focus.
		/// </summary>
		/// <remarks>optional, order 2</remarks>
		[System.Xml.Serialization.XmlElementAttribute("Error", Namespace = "http://www.onvif.org/ver10/schema", DataType = "string")]
		public string error {
			get {
				return this._error;
			}
			set {
				this._error = value;
			}
		}

		/// <remarks>optional, order 3</remarks>
		[System.Xml.Serialization.XmlElementAttribute("Extension", Namespace = "http://www.onvif.org/ver10/schema")]
		public FocusStatus20Extension extension {
			get {
				return this._extension;
			}
			set {
				this._extension = value;
			}
		}
	}

	[System.SerializableAttribute()]
	[System.Xml.Serialization.XmlTypeAttribute(TypeName = "FocusStatus20Extension", Namespace = "http://www.onvif.org/ver10/schema")]
	public partial class FocusStatus20Extension {

		private System.Xml.XmlElement[] _any;

		/// <remarks>optional, order 0, namespace ##any</remarks>
		[System.Xml.Serialization.XmlAnyElementAttribute()]
		public System.Xml.XmlElement[] any {
			get {
				return this._any;
			}
			set {
				this._any = value;
			}
		}
	}

	[System.SerializableAttribute()]
	[System.Xml.Serialization.XmlTypeAttribute(TypeName = "ImagingStatus20Extension", Namespace = "http://www.onvif.org/ver10/schema")]
	public partial class ImagingStatus20Extension {

		private System.Xml.XmlElement[] _any;

		/// <remarks>optional, order 0, namespace ##any</remarks>
		[System.Xml.Serialization.XmlAnyElementAttribute()]
		public System.Xml.XmlElement[] any {
			get {
				return this._any;
			}
			set {
				this._any = value;
			}
		}
	}

	[System.SerializableAttribute()]
	[System.Xml.Serialization.XmlTypeAttribute(TypeName = "ImagingOptions20", Namespace = "http://www.onvif.org/ver10/schema")]
	public partial class ImagingOptions20 {

		private System.Xml.XmlAttribute[] _anyAttr;

		private BacklightCompensationOptions20 _backlightCompensation;

		private FloatRange _brightness;

		private FloatRange _colorSaturation;

		private FloatRange _contrast;

		private ExposureOptions20 _exposure;

		private FocusOptions20 _focus;

		private IrCutFilterMode[] _irCutFilterModes;

		private FloatRange _sharpness;

		private WideDynamicRangeOptions20 _wideDynamicRange;

		private WhiteBalanceOptions20 _whiteBalance;

		private ImagingOptions20Extension _extension;

		[System.Xml.Serialization.XmlAnyAttributeAttribute()]
		public System.Xml.XmlAttribute[] anyAttr {
			get {
				return this._anyAttr;
			}
			set {
				this._anyAttr = value;
			}
		}

		/// <summary>
		/// Valid range of Backlight Compensation.
		/// </summary>
		/// <remarks>optional, order 0</remarks>
		[System.Xml.Serialization.XmlElementAttribute("BacklightCompensation", Namespace = "http://www.onvif.org/ver10/schema")]
		public BacklightCompensationOptions20 backlightCompensation {
			get {
				return this._backlightCompensation;
			}
			set {
				this._backlightCompensation = value;
			}
		}

		/// <summary>
		/// Valid range of Brightness.
		/// </summary>
		/// <remarks>optional, order 1</remarks>
		[System.Xml.Serialization.XmlElementAttribute("Brightness", Namespace = "http://www.onvif.org/ver10/schema")]
		public FloatRange brightness {
			get {
				return this._brightness;
			}
			set {
				this._brightness = value;
			}
		}

		/// <summary>
		/// Valid range of Color Saturation.
		/// </summary>
		/// <remarks>optional, order 2</remarks>
		[System.Xml.Serialization.XmlElementAttribute("ColorSaturation", Namespace = "http://www.onvif.org/ver10/schema")]
		public FloatRange colorSaturation {
			get {
				return this._colorSaturation;
			}
			set {
				this._colorSaturation = value;
			}
		}

		/// <summary>
		/// Valid range of Contrast.
		/// </summary>
		/// <remarks>optional, order 3</remarks>
		[System.Xml.Serialization.XmlElementAttribute("Contrast", Namespace = "http://www.onvif.org/ver10/schema")]
		public FloatRange contrast {
			get {
				return this._contrast;
			}
			set {
				this._contrast = value;
			}
		}

		/// <summary>
		/// Valid range of Exposure.
		/// </summary>
		/// <remarks>optional, order 4</remarks>
		[System.Xml.Serialization.XmlElementAttribute("Exposure", Namespace = "http://www.onvif.org/ver10/schema")]
		public ExposureOptions20 exposure {
			get {
				return this._exposure;
			}
			set {
				this._exposure = value;
			}
		}

		/// <summary>
		/// Valid range of Focus.
		/// </summary>
		/// <remarks>optional, order 5</remarks>
		[System.Xml.Serialization.XmlElementAttribute("Focus", Namespace = "http://www.onvif.org/ver10/schema")]
		public FocusOptions20 focus {
			get {
				return this._focus;
			}
			set {
				this._focus = value;
			}
		}

		/// <summary>
		/// Valid range of IrCutFilterModes.
		/// </summary>
		/// <remarks>optional, order 6</remarks>
		[System.Xml.Serialization.XmlElementAttribute("IrCutFilterModes", Namespace = "http://www.onvif.org/ver10/schema")]
		public IrCutFilterMode[] irCutFilterModes {
			get {
				return this._irCutFilterModes;
			}
			set {
				this._irCutFilterModes = value;
			}
		}

		/// <summary>
		/// Valid range of Sharpness.
		/// </summary>
		/// <remarks>optional, order 7</remarks>
		[System.Xml.Serialization.XmlElementAttribute("Sharpness", Namespace = "http://www.onvif.org/ver10/schema")]
		public FloatRange sharpness {
			get {
				return this._sharpness;
			}
			set {
				this._sharpness = value;
			}
		}

		/// <summary>
		/// Valid range of WideDynamicRange.
		/// </summary>
		/// <remarks>optional, order 8</remarks>
		[System.Xml.Serialization.XmlElementAttribute("WideDynamicRange", Namespace = "http://www.onvif.org/ver10/schema")]
		public WideDynamicRangeOptions20 wideDynamicRange {
			get {
				return this._wideDynamicRange;
			}
			set {
				this._wideDynamicRange = value;
			}
		}

		/// <summary>
		/// Valid range of WhiteBalance.
		/// </summary>
		/// <remarks>optional, order 9</remarks>
		[System.Xml.Serialization.XmlElementAttribute("WhiteBalance", Namespace = "http://www.onvif.org/ver10/schema")]
		public WhiteBalanceOptions20 whiteBalance {
			get {
				return this._whiteBalance;
			}
			set {
				this._whiteBalance = value;
			}
		}

		/// <remarks>optional, order 10</remarks>
		[System.Xml.Serialization.XmlElementAttribute("Extension", Namespace = "http://www.onvif.org/ver10/schema")]
		public ImagingOptions20Extension extension {
			get {
				return this._extension;
			}
			set {
				this._extension = value;
			}
		}
	}

	[System.SerializableAttribute()]
	[System.Xml.Serialization.XmlTypeAttribute(TypeName = "BacklightCompensationOptions20", Namespace = "http://www.onvif.org/ver10/schema")]
	public partial class BacklightCompensationOptions20 {

		private BacklightCompensationMode[] _mode;

		private FloatRange _level;

		/// <summary>
		/// 'ON' or 'OFF'
		/// </summary>
		/// <remarks>reqired, order 0</remarks>
		[System.Xml.Serialization.XmlElementAttribute("Mode", Namespace = "http://www.onvif.org/ver10/schema")]
		public BacklightCompensationMode[] mode {
			get {
				return this._mode;
			}
			set {
				this._mode = value;
			}
		}

		/// <summary>
		/// Level range of BacklightCompensation.
		/// </summary>
		/// <remarks>optional, order 1</remarks>
		[System.Xml.Serialization.XmlElementAttribute("Level", Namespace = "http://www.onvif.org/ver10/schema")]
		public FloatRange level {
			get {
				return this._level;
			}
			set {
				this._level = value;
			}
		}
	}

	[System.SerializableAttribute()]
	[System.Xml.Serialization.XmlTypeAttribute(TypeName = "ExposureOptions20", Namespace = "http://www.onvif.org/ver10/schema")]
	public partial class ExposureOptions20 {

		private ExposureMode[] _mode;

		private ExposurePriority[] _priority;

		private FloatRange _minExposureTime;

		private FloatRange _maxExposureTime;

		private FloatRange _minGain;

		private FloatRange _maxGain;

		private FloatRange _minIris;

		private FloatRange _maxIris;

		private FloatRange _exposureTime;

		private FloatRange _gain;

		private FloatRange _iris;

		/// <summary>
		/// Exposure Mode
		/// </summary>
		/// <remarks>reqired, order 0</remarks>
		[System.Xml.Serialization.XmlElementAttribute("Mode", Namespace = "http://www.onvif.org/ver10/schema")]
		public ExposureMode[] mode {
			get {
				return this._mode;
			}
			set {
				this._mode = value;
			}
		}

		/// <summary>
		/// The exposure priority mode (low noise/framerate).
		/// </summary>
		/// <remarks>optional, order 1</remarks>
		[System.Xml.Serialization.XmlElementAttribute("Priority", Namespace = "http://www.onvif.org/ver10/schema")]
		public ExposurePriority[] priority {
			get {
				return this._priority;
			}
			set {
				this._priority = value;
			}
		}

		/// <summary>
		/// Valid range of the Minimum ExposureTime.
		/// </summary>
		/// <remarks>optional, order 2</remarks>
		[System.Xml.Serialization.XmlElementAttribute("MinExposureTime", Namespace = "http://www.onvif.org/ver10/schema")]
		public FloatRange minExposureTime {
			get {
				return this._minExposureTime;
			}
			set {
				this._minExposureTime = value;
			}
		}

		/// <summary>
		/// Valid range of the Maximum ExposureTime.
		/// </summary>
		/// <remarks>optional, order 3</remarks>
		[System.Xml.Serialization.XmlElementAttribute("MaxExposureTime", Namespace = "http://www.onvif.org/ver10/schema")]
		public FloatRange maxExposureTime {
			get {
				return this._maxExposureTime;
			}
			set {
				this._maxExposureTime = value;
			}
		}

		/// <summary>
		/// Valid range of the Minimum Gain.
		/// </summary>
		/// <remarks>optional, order 4</remarks>
		[System.Xml.Serialization.XmlElementAttribute("MinGain", Namespace = "http://www.onvif.org/ver10/schema")]
		public FloatRange minGain {
			get {
				return this._minGain;
			}
			set {
				this._minGain = value;
			}
		}

		/// <summary>
		/// Valid range of the Maximum Gain.
		/// </summary>
		/// <remarks>optional, order 5</remarks>
		[System.Xml.Serialization.XmlElementAttribute("MaxGain", Namespace = "http://www.onvif.org/ver10/schema")]
		public FloatRange maxGain {
			get {
				return this._maxGain;
			}
			set {
				this._maxGain = value;
			}
		}

		/// <summary>
		/// Valid range of the Minimum Iris.
		/// </summary>
		/// <remarks>optional, order 6</remarks>
		[System.Xml.Serialization.XmlElementAttribute("MinIris", Namespace = "http://www.onvif.org/ver10/schema")]
		public FloatRange minIris {
			get {
				return this._minIris;
			}
			set {
				this._minIris = value;
			}
		}

		/// <summary>
		/// Valid range of the Maximum Iris.
		/// </summary>
		/// <remarks>optional, order 7</remarks>
		[System.Xml.Serialization.XmlElementAttribute("MaxIris", Namespace = "http://www.onvif.org/ver10/schema")]
		public FloatRange maxIris {
			get {
				return this._maxIris;
			}
			set {
				this._maxIris = value;
			}
		}

		/// <summary>
		/// Valid range of the ExposureTime.
		/// </summary>
		/// <remarks>optional, order 8</remarks>
		[System.Xml.Serialization.XmlElementAttribute("ExposureTime", Namespace = "http://www.onvif.org/ver10/schema")]
		public FloatRange exposureTime {
			get {
				return this._exposureTime;
			}
			set {
				this._exposureTime = value;
			}
		}

		/// <summary>
		/// Valid range of the Gain.
		/// </summary>
		/// <remarks>optional, order 9</remarks>
		[System.Xml.Serialization.XmlElementAttribute("Gain", Namespace = "http://www.onvif.org/ver10/schema")]
		public FloatRange gain {
			get {
				return this._gain;
			}
			set {
				this._gain = value;
			}
		}

		/// <summary>
		/// Valid range of the Iris.
		/// </summary>
		/// <remarks>optional, order 10</remarks>
		[System.Xml.Serialization.XmlElementAttribute("Iris", Namespace = "http://www.onvif.org/ver10/schema")]
		public FloatRange iris {
			get {
				return this._iris;
			}
			set {
				this._iris = value;
			}
		}
	}

	[System.SerializableAttribute()]
	[System.Xml.Serialization.XmlTypeAttribute(TypeName = "FocusOptions20", Namespace = "http://www.onvif.org/ver10/schema")]
	public partial class FocusOptions20 {

		private AutoFocusMode[] _autoFocusModes;

		private FloatRange _defaultSpeed;

		private FloatRange _nearLimit;

		private FloatRange _farLimit;

		private FocusOptions20Extension _extension;

		/// <summary>
		/// Mode of Auto Focus.
		/// </summary>
		/// <remarks>optional, order 0</remarks>
		[System.Xml.Serialization.XmlElementAttribute("AutoFocusModes", Namespace = "http://www.onvif.org/ver10/schema")]
		public AutoFocusMode[] autoFocusModes {
			get {
				return this._autoFocusModes;
			}
			set {
				this._autoFocusModes = value;
			}
		}

		/// <summary>
		/// Valid range of DefaultSpeed.
		/// </summary>
		/// <remarks>optional, order 1</remarks>
		[System.Xml.Serialization.XmlElementAttribute("DefaultSpeed", Namespace = "http://www.onvif.org/ver10/schema")]
		public FloatRange defaultSpeed {
			get {
				return this._defaultSpeed;
			}
			set {
				this._defaultSpeed = value;
			}
		}

		/// <summary>
		/// Valid range of NearLimit.
		/// </summary>
		/// <remarks>optional, order 2</remarks>
		[System.Xml.Serialization.XmlElementAttribute("NearLimit", Namespace = "http://www.onvif.org/ver10/schema")]
		public FloatRange nearLimit {
			get {
				return this._nearLimit;
			}
			set {
				this._nearLimit = value;
			}
		}

		/// <summary>
		/// Valid range of FarLimit.
		/// </summary>
		/// <remarks>optional, order 3</remarks>
		[System.Xml.Serialization.XmlElementAttribute("FarLimit", Namespace = "http://www.onvif.org/ver10/schema")]
		public FloatRange farLimit {
			get {
				return this._farLimit;
			}
			set {
				this._farLimit = value;
			}
		}

		/// <remarks>optional, order 4</remarks>
		[System.Xml.Serialization.XmlElementAttribute("Extension", Namespace = "http://www.onvif.org/ver10/schema")]
		public FocusOptions20Extension extension {
			get {
				return this._extension;
			}
			set {
				this._extension = value;
			}
		}
	}

	[System.SerializableAttribute()]
	[System.Xml.Serialization.XmlTypeAttribute(TypeName = "FocusOptions20Extension", Namespace = "http://www.onvif.org/ver10/schema")]
	public partial class FocusOptions20Extension {

		private System.Xml.XmlElement[] _any;

		/// <remarks>optional, order 0, namespace ##any</remarks>
		[System.Xml.Serialization.XmlAnyElementAttribute()]
		public System.Xml.XmlElement[] any {
			get {
				return this._any;
			}
			set {
				this._any = value;
			}
		}
	}

	[System.SerializableAttribute()]
	[System.Xml.Serialization.XmlTypeAttribute(TypeName = "WideDynamicRangeOptions20", Namespace = "http://www.onvif.org/ver10/schema")]
	public partial class WideDynamicRangeOptions20 {

		private WideDynamicMode[] _mode;

		private FloatRange _level;

		/// <remarks>reqired, order 0</remarks>
		[System.Xml.Serialization.XmlElementAttribute("Mode", Namespace = "http://www.onvif.org/ver10/schema")]
		public WideDynamicMode[] mode {
			get {
				return this._mode;
			}
			set {
				this._mode = value;
			}
		}

		/// <remarks>optional, order 1</remarks>
		[System.Xml.Serialization.XmlElementAttribute("Level", Namespace = "http://www.onvif.org/ver10/schema")]
		public FloatRange level {
			get {
				return this._level;
			}
			set {
				this._level = value;
			}
		}
	}

	[System.SerializableAttribute()]
	[System.Xml.Serialization.XmlTypeAttribute(TypeName = "WhiteBalanceOptions20", Namespace = "http://www.onvif.org/ver10/schema")]
	public partial class WhiteBalanceOptions20 {

		private WhiteBalanceMode[] _mode;

		private FloatRange _yrGain;

		private FloatRange _ybGain;

		private WhiteBalanceOptions20Extension _extension;

		/// <summary>
		/// Mode of WhiteBalance.
		/// </summary>
		/// <remarks>reqired, order 0</remarks>
		[System.Xml.Serialization.XmlElementAttribute("Mode", Namespace = "http://www.onvif.org/ver10/schema")]
		public WhiteBalanceMode[] mode {
			get {
				return this._mode;
			}
			set {
				this._mode = value;
			}
		}

		/// <remarks>optional, order 1</remarks>
		[System.Xml.Serialization.XmlElementAttribute("YrGain", Namespace = "http://www.onvif.org/ver10/schema")]
		public FloatRange yrGain {
			get {
				return this._yrGain;
			}
			set {
				this._yrGain = value;
			}
		}

		/// <remarks>optional, order 2</remarks>
		[System.Xml.Serialization.XmlElementAttribute("YbGain", Namespace = "http://www.onvif.org/ver10/schema")]
		public FloatRange ybGain {
			get {
				return this._ybGain;
			}
			set {
				this._ybGain = value;
			}
		}

		/// <remarks>optional, order 3</remarks>
		[System.Xml.Serialization.XmlElementAttribute("Extension", Namespace = "http://www.onvif.org/ver10/schema")]
		public WhiteBalanceOptions20Extension extension {
			get {
				return this._extension;
			}
			set {
				this._extension = value;
			}
		}
	}

	[System.SerializableAttribute()]
	[System.Xml.Serialization.XmlTypeAttribute(TypeName = "WhiteBalanceOptions20Extension", Namespace = "http://www.onvif.org/ver10/schema")]
	public partial class WhiteBalanceOptions20Extension {

		private System.Xml.XmlElement[] _any;

		/// <remarks>optional, order 0, namespace ##any</remarks>
		[System.Xml.Serialization.XmlAnyElementAttribute()]
		public System.Xml.XmlElement[] any {
			get {
				return this._any;
			}
			set {
				this._any = value;
			}
		}
	}

	[System.SerializableAttribute()]
	[System.Xml.Serialization.XmlTypeAttribute(TypeName = "ImagingOptions20Extension", Namespace = "http://www.onvif.org/ver10/schema")]
	public partial class ImagingOptions20Extension {

		private System.Xml.XmlElement[] _any;

		private ImageStabilizationOptions _imageStabilization;

		private ImagingOptions20Extension2 _extension;

		/// <remarks>optional, order 0, namespace ##other</remarks>
		[System.Xml.Serialization.XmlAnyElementAttribute()]
		public System.Xml.XmlElement[] any {
			get {
				return this._any;
			}
			set {
				this._any = value;
			}
		}

		/// <summary>
		/// Options of parameters for Image Stabilization feature.
		/// </summary>
		/// <remarks>optional, order 1</remarks>
		[System.Xml.Serialization.XmlElementAttribute("ImageStabilization", Namespace = "http://www.onvif.org/ver10/schema")]
		public ImageStabilizationOptions imageStabilization {
			get {
				return this._imageStabilization;
			}
			set {
				this._imageStabilization = value;
			}
		}

		/// <remarks>optional, order 2</remarks>
		[System.Xml.Serialization.XmlElementAttribute("Extension", Namespace = "http://www.onvif.org/ver10/schema")]
		public ImagingOptions20Extension2 extension {
			get {
				return this._extension;
			}
			set {
				this._extension = value;
			}
		}
	}

	[System.SerializableAttribute()]
	[System.Xml.Serialization.XmlTypeAttribute(TypeName = "ImageStabilizationOptions", Namespace = "http://www.onvif.org/ver10/schema")]
	public partial class ImageStabilizationOptions {

		private System.Xml.XmlAttribute[] _anyAttr;

		private ImageStabilizationMode[] _mode;

		private FloatRange _level;

		private ImageStabilizationOptionsExtension _extension;

		[System.Xml.Serialization.XmlAnyAttributeAttribute()]
		public System.Xml.XmlAttribute[] anyAttr {
			get {
				return this._anyAttr;
			}
			set {
				this._anyAttr = value;
			}
		}

		/// <summary>
		/// Supported options of Image Stabilization mode parameter.
		/// </summary>
		/// <remarks>reqired, order 0</remarks>
		[System.Xml.Serialization.XmlElementAttribute("Mode", Namespace = "http://www.onvif.org/ver10/schema")]
		public ImageStabilizationMode[] mode {
			get {
				return this._mode;
			}
			set {
				this._mode = value;
			}
		}

		/// <summary>
		/// Wide dynamic range mode (on/off).
		/// </summary>
		/// <remarks>optional, order 1</remarks>
		[System.Xml.Serialization.XmlElementAttribute("Level", Namespace = "http://www.onvif.org/ver10/schema")]
		public FloatRange level {
			get {
				return this._level;
			}
			set {
				this._level = value;
			}
		}

		/// <remarks>optional, order 2</remarks>
		[System.Xml.Serialization.XmlElementAttribute("Extension", Namespace = "http://www.onvif.org/ver10/schema")]
		public ImageStabilizationOptionsExtension extension {
			get {
				return this._extension;
			}
			set {
				this._extension = value;
			}
		}
	}

	[System.SerializableAttribute()]
	[System.Xml.Serialization.XmlTypeAttribute(TypeName = "ImageStabilizationOptionsExtension", Namespace = "http://www.onvif.org/ver10/schema")]
	public partial class ImageStabilizationOptionsExtension {

		private System.Xml.XmlElement[] _any;

		/// <remarks>optional, order 0, namespace ##any</remarks>
		[System.Xml.Serialization.XmlAnyElementAttribute()]
		public System.Xml.XmlElement[] any {
			get {
				return this._any;
			}
			set {
				this._any = value;
			}
		}
	}

	[System.SerializableAttribute()]
	[System.Xml.Serialization.XmlTypeAttribute(TypeName = "ImagingOptions20Extension2", Namespace = "http://www.onvif.org/ver10/schema")]
	public partial class ImagingOptions20Extension2 {

		private System.Xml.XmlElement[] _any;

		/// <remarks>optional, order 0, namespace ##targetNamespace</remarks>
		[System.Xml.Serialization.XmlAnyElementAttribute(Namespace = "http://www.onvif.org/ver10/schema")]
		public System.Xml.XmlElement[] any {
			get {
				return this._any;
			}
			set {
				this._any = value;
			}
		}
	}

	[System.SerializableAttribute()]
	[System.Xml.Serialization.XmlTypeAttribute(TypeName = "MoveOptions20", Namespace = "http://www.onvif.org/ver10/schema")]
	public partial class MoveOptions20 {

		private AbsoluteFocusOptions _absolute;

		private RelativeFocusOptions20 _relative;

		private ContinuousFocusOptions _continuous;

		/// <summary>
		/// Valid ranges for the absolute control.
		/// </summary>
		/// <remarks>optional, order 0</remarks>
		[System.Xml.Serialization.XmlElementAttribute("Absolute", Namespace = "http://www.onvif.org/ver10/schema")]
		public AbsoluteFocusOptions absolute {
			get {
				return this._absolute;
			}
			set {
				this._absolute = value;
			}
		}

		/// <summary>
		/// Valid ranges for the relative control.
		/// </summary>
		/// <remarks>optional, order 1</remarks>
		[System.Xml.Serialization.XmlElementAttribute("Relative", Namespace = "http://www.onvif.org/ver10/schema")]
		public RelativeFocusOptions20 relative {
			get {
				return this._relative;
			}
			set {
				this._relative = value;
			}
		}

		/// <summary>
		/// Valid ranges for the continuous control.
		/// </summary>
		/// <remarks>optional, order 2</remarks>
		[System.Xml.Serialization.XmlElementAttribute("Continuous", Namespace = "http://www.onvif.org/ver10/schema")]
		public ContinuousFocusOptions continuous {
			get {
				return this._continuous;
			}
			set {
				this._continuous = value;
			}
		}
	}

	[System.SerializableAttribute()]
	[System.Xml.Serialization.XmlTypeAttribute(TypeName = "RelativeFocusOptions20", Namespace = "http://www.onvif.org/ver10/schema")]
	public partial class RelativeFocusOptions20 {

		private FloatRange _distance;

		private FloatRange _speed;

		/// <summary>
		/// Valid ranges of the distance.
		/// </summary>
		/// <remarks>reqired, order 0</remarks>
		[System.Xml.Serialization.XmlElementAttribute("Distance", Namespace = "http://www.onvif.org/ver10/schema")]
		public FloatRange distance {
			get {
				return this._distance;
			}
			set {
				this._distance = value;
			}
		}

		/// <summary>
		/// Valid ranges of the speed.
		/// </summary>
		/// <remarks>optional, order 1</remarks>
		[System.Xml.Serialization.XmlElementAttribute("Speed", Namespace = "http://www.onvif.org/ver10/schema")]
		public FloatRange speed {
			get {
				return this._speed;
			}
			set {
				this._speed = value;
			}
		}
	}

	[System.SerializableAttribute()]
	[System.Xml.Serialization.XmlTypeAttribute(TypeName = "PropertyOperation", Namespace = "http://www.onvif.org/ver10/schema")]
	public enum PropertyOperation {

		[System.Xml.Serialization.XmlEnumAttribute(Name = "Initialized")]
		initialized,

		[System.Xml.Serialization.XmlEnumAttribute(Name = "Deleted")]
		deleted,

		[System.Xml.Serialization.XmlEnumAttribute(Name = "Changed")]
		changed,
	}

	[System.SerializableAttribute()]
	[System.Xml.Serialization.XmlTypeAttribute(TypeName = "MessageExtension", Namespace = "http://www.onvif.org/ver10/schema")]
	[XmlRoot(Namespace = "http://www.onvif.org/ver10/schema", IsNullable = true)]
	public partial class MessageExtension {

		private System.Xml.XmlElement[] _any;

		/// <remarks>optional, order 0, namespace ##any</remarks>
		[System.Xml.Serialization.XmlAnyElementAttribute()]
		public System.Xml.XmlElement[] any {
			get {
				return this._any;
			}
			set {
				this._any = value;
			}
		}
	}

	[System.SerializableAttribute()]
	[System.Xml.Serialization.XmlTypeAttribute(TypeName = "MessageDescription", Namespace = "http://www.onvif.org/ver10/schema")]
	public partial class MessageDescription {

		private bool _isProperty;

		private bool _isPropertySpecified;

		private System.Xml.XmlAttribute[] _anyAttr;

		private ItemListDescription _source;

		private ItemListDescription _key;

		private ItemListDescription _data;

		private MessageDescriptionExtension _extension;

		/// <summary>
		/// Must be set to true when the described Message relates to a property. An alternative term of "property" is a "state" in contrast to a pure event, which contains relevant information for only a single point in time.
		/// Default is false.
		/// </summary>
		[System.Xml.Serialization.XmlAttributeAttribute("IsProperty", DataType = "boolean")]
		public bool isProperty {
			get {
				return this._isProperty;
			}
			set {
				this._isProperty = value;
			}
		}

		[System.Xml.Serialization.XmlIgnoreAttribute()]
		public bool isPropertySpecified {
			get {
				return this._isPropertySpecified;
			}
			set {
				this._isPropertySpecified = value;
			}
		}

		[System.Xml.Serialization.XmlAnyAttributeAttribute()]
		public System.Xml.XmlAttribute[] anyAttr {
			get {
				return this._anyAttr;
			}
			set {
				this._anyAttr = value;
			}
		}

		/// <summary>
		/// Set of tokens producing this message. The list may only contain SimpleItemDescription items.
		/// The set of tokens identify the component within the WS-Endpoint, which is responsible for the producing the message.
		/// For analytics events the token set shall include the VideoSourceConfigurationToken, the VideoAnalyticsConfigurationToken
		/// and the name of the analytics module or rule.
		/// </summary>
		/// <remarks>optional, order 0</remarks>
		[System.Xml.Serialization.XmlElementAttribute("Source", Namespace = "http://www.onvif.org/ver10/schema")]
		public ItemListDescription source {
			get {
				return this._source;
			}
			set {
				this._source = value;
			}
		}

		/// <summary>
		/// Describes optional message payload parameters that may be used as key. E.g. object IDs of tracked objects are conveyed as key.
		/// </summary>
		/// <remarks>optional, order 1</remarks>
		[System.Xml.Serialization.XmlElementAttribute("Key", Namespace = "http://www.onvif.org/ver10/schema")]
		public ItemListDescription key {
			get {
				return this._key;
			}
			set {
				this._key = value;
			}
		}

		/// <summary>
		/// Describes the payload of the message.
		/// </summary>
		/// <remarks>optional, order 2</remarks>
		[System.Xml.Serialization.XmlElementAttribute("Data", Namespace = "http://www.onvif.org/ver10/schema")]
		public ItemListDescription data {
			get {
				return this._data;
			}
			set {
				this._data = value;
			}
		}

		/// <remarks>optional, order 3</remarks>
		[System.Xml.Serialization.XmlElementAttribute("Extension", Namespace = "http://www.onvif.org/ver10/schema")]
		public MessageDescriptionExtension extension {
			get {
				return this._extension;
			}
			set {
				this._extension = value;
			}
		}
	}

	/// <summary>
	/// Describes a list of items. Each item in the list shall have a unique name.
	/// The list is designed as linear structure without optional or unbounded elements.
	/// Use ElementItems only when complex structures are inevitable.
	/// </summary>
	[System.SerializableAttribute()]
	[System.Xml.Serialization.XmlTypeAttribute(TypeName = "ItemListDescription", Namespace = "http://www.onvif.org/ver10/schema")]
	public partial class ItemListDescription {

		private System.Xml.XmlAttribute[] _anyAttr;

		private SimpleItemDescription[] _simpleItemDescription;

		private ElementItemDescription[] _elementItemDescription;

		private ItemListDescriptionExtension _extension;

		[System.Xml.Serialization.XmlAnyAttributeAttribute()]
		public System.Xml.XmlAttribute[] anyAttr {
			get {
				return this._anyAttr;
			}
			set {
				this._anyAttr = value;
			}
		}

		/// <summary>
		/// Description of a simple item. The type must be of cathegory simpleType (xs:string, xs:integer, xs:float, ...).
		/// </summary>
		/// <remarks>optional, order 0</remarks>
		[System.Xml.Serialization.XmlElementAttribute("SimpleItemDescription", Namespace = "http://www.onvif.org/ver10/schema")]
		public SimpleItemDescription[] simpleItemDescription {
			get {
				return this._simpleItemDescription;
			}
			set {
				this._simpleItemDescription = value;
			}
		}

		/// <summary>
		/// Description of a complex type. The Type must reference a defined type.
		/// </summary>
		/// <remarks>optional, order 1</remarks>
		[System.Xml.Serialization.XmlElementAttribute("ElementItemDescription", Namespace = "http://www.onvif.org/ver10/schema")]
		public ElementItemDescription[] elementItemDescription {
			get {
				return this._elementItemDescription;
			}
			set {
				this._elementItemDescription = value;
			}
		}

		/// <remarks>optional, order 2</remarks>
		[System.Xml.Serialization.XmlElementAttribute("Extension", Namespace = "http://www.onvif.org/ver10/schema")]
		public ItemListDescriptionExtension extension {
			get {
				return this._extension;
			}
			set {
				this._extension = value;
			}
		}

		[System.SerializableAttribute()]
		[System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://www.onvif.org/ver10/schema")]
		public partial class SimpleItemDescription {

			private string _name;

			private System.Xml.XmlQualifiedName _type;

			/// <summary>
			/// Item name. Must be unique within a list.
			/// </summary>
			[System.Xml.Serialization.XmlAttributeAttribute("Name", DataType = "string")]
			public string name {
				get {
					return this._name;
				}
				set {
					this._name = value;
				}
			}

			[System.Xml.Serialization.XmlAttributeAttribute("Type", DataType = "QName")]
			public System.Xml.XmlQualifiedName type {
				get {
					return this._type;
				}
				set {
					this._type = value;
				}
			}
		}

		[System.SerializableAttribute()]
		[System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://www.onvif.org/ver10/schema")]
		public partial class ElementItemDescription {

			private string _name;

			private System.Xml.XmlQualifiedName _type;

			/// <summary>
			/// Item name. Must be unique within a list.
			/// </summary>
			[System.Xml.Serialization.XmlAttributeAttribute("Name", DataType = "string")]
			public string name {
				get {
					return this._name;
				}
				set {
					this._name = value;
				}
			}

			/// <summary>
			/// The type of the item. The Type must reference a defined type.
			/// </summary>
			[System.Xml.Serialization.XmlAttributeAttribute("Type", DataType = "QName")]
			public System.Xml.XmlQualifiedName type {
				get {
					return this._type;
				}
				set {
					this._type = value;
				}
			}
		}
	}

	[System.SerializableAttribute()]
	[System.Xml.Serialization.XmlTypeAttribute(TypeName = "ItemListDescriptionExtension", Namespace = "http://www.onvif.org/ver10/schema")]
	public partial class ItemListDescriptionExtension {

		private System.Xml.XmlElement[] _any;

		/// <remarks>optional, order 0, namespace ##any</remarks>
		[System.Xml.Serialization.XmlAnyElementAttribute()]
		public System.Xml.XmlElement[] any {
			get {
				return this._any;
			}
			set {
				this._any = value;
			}
		}
	}

	[System.SerializableAttribute()]
	[System.Xml.Serialization.XmlTypeAttribute(TypeName = "MessageDescriptionExtension", Namespace = "http://www.onvif.org/ver10/schema")]
	public partial class MessageDescriptionExtension {

		private System.Xml.XmlElement[] _any;

		/// <remarks>optional, order 0, namespace ##any</remarks>
		[System.Xml.Serialization.XmlAnyElementAttribute()]
		public System.Xml.XmlElement[] any {
			get {
				return this._any;
			}
			set {
				this._any = value;
			}
		}
	}

	[System.SerializableAttribute()]
	[System.Xml.Serialization.XmlTypeAttribute(TypeName = "Vector", Namespace = "http://www.onvif.org/ver10/schema")]
	public partial class Vector {

		private float _x;

		private bool _xSpecified;

		private float _y;

		private bool _ySpecified;

		[System.Xml.Serialization.XmlAttributeAttribute("x", DataType = "float")]
		public float x {
			get {
				return this._x;
			}
			set {
				this._x = value;
			}
		}

		[System.Xml.Serialization.XmlIgnoreAttribute()]
		public bool xSpecified {
			get {
				return this._xSpecified;
			}
			set {
				this._xSpecified = value;
			}
		}

		[System.Xml.Serialization.XmlAttributeAttribute("y", DataType = "float")]
		public float y {
			get {
				return this._y;
			}
			set {
				this._y = value;
			}
		}

		[System.Xml.Serialization.XmlIgnoreAttribute()]
		public bool ySpecified {
			get {
				return this._ySpecified;
			}
			set {
				this._ySpecified = value;
			}
		}
	}

	[System.SerializableAttribute()]
	[System.Xml.Serialization.XmlTypeAttribute(TypeName = "Polygon", Namespace = "http://www.onvif.org/ver10/schema")]
	public partial class Polygon {

		private Vector[] _point;

		/// <remarks>reqired, order 0</remarks>
		[System.Xml.Serialization.XmlElementAttribute("Point", Namespace = "http://www.onvif.org/ver10/schema")]
		public Vector[] point {
			get {
				return this._point;
			}
			set {
				this._point = value;
			}
		}
	}

	[System.SerializableAttribute()]
	[System.Xml.Serialization.XmlTypeAttribute(TypeName = "Polyline", Namespace = "http://www.onvif.org/ver10/schema")]
	public partial class Polyline {

		private Vector[] _point;

		/// <remarks>reqired, order 0</remarks>
		[System.Xml.Serialization.XmlElementAttribute("Point", Namespace = "http://www.onvif.org/ver10/schema")]
		public Vector[] point {
			get {
				return this._point;
			}
			set {
				this._point = value;
			}
		}
	}

	[System.SerializableAttribute()]
	[System.Xml.Serialization.XmlTypeAttribute(TypeName = "Direction", Namespace = "http://www.onvif.org/ver10/schema")]
	public enum Direction {

		[System.Xml.Serialization.XmlEnumAttribute(Name = "Left")]
		left,

		[System.Xml.Serialization.XmlEnumAttribute(Name = "Right")]
		right,

		[System.Xml.Serialization.XmlEnumAttribute(Name = "Any")]
		any,
	}

	[System.SerializableAttribute()]
	[System.Xml.Serialization.XmlTypeAttribute(TypeName = "Color", Namespace = "http://www.onvif.org/ver10/schema")]
	public partial class Color {

		private float _x;

		private float _y;

		private float _z;

		private string _colorspace;

		[System.Xml.Serialization.XmlAttributeAttribute("X", DataType = "float")]
		public float x {
			get {
				return this._x;
			}
			set {
				this._x = value;
			}
		}

		[System.Xml.Serialization.XmlAttributeAttribute("Y", DataType = "float")]
		public float y {
			get {
				return this._y;
			}
			set {
				this._y = value;
			}
		}

		[System.Xml.Serialization.XmlAttributeAttribute("Z", DataType = "float")]
		public float z {
			get {
				return this._z;
			}
			set {
				this._z = value;
			}
		}

		[System.Xml.Serialization.XmlAttributeAttribute("Colorspace", DataType = "anyURI")]
		public string colorspace {
			get {
				return this._colorspace;
			}
			set {
				this._colorspace = value;
			}
		}
	}

	[System.SerializableAttribute()]
	[System.Xml.Serialization.XmlTypeAttribute(TypeName = "ColorCovariance", Namespace = "http://www.onvif.org/ver10/schema")]
	public partial class ColorCovariance {

		private float _xx;

		private float _yy;

		private float _zz;

		private float _xy;

		private bool _xySpecified;

		private float _xz;

		private bool _xzSpecified;

		private float _yz;

		private bool _yzSpecified;

		private string _colorspace;

		[System.Xml.Serialization.XmlAttributeAttribute("XX", DataType = "float")]
		public float xx {
			get {
				return this._xx;
			}
			set {
				this._xx = value;
			}
		}

		[System.Xml.Serialization.XmlAttributeAttribute("YY", DataType = "float")]
		public float yy {
			get {
				return this._yy;
			}
			set {
				this._yy = value;
			}
		}

		[System.Xml.Serialization.XmlAttributeAttribute("ZZ", DataType = "float")]
		public float zz {
			get {
				return this._zz;
			}
			set {
				this._zz = value;
			}
		}

		[System.Xml.Serialization.XmlAttributeAttribute("XY", DataType = "float")]
		public float xy {
			get {
				return this._xy;
			}
			set {
				this._xy = value;
			}
		}

		[System.Xml.Serialization.XmlIgnoreAttribute()]
		public bool xySpecified {
			get {
				return this._xySpecified;
			}
			set {
				this._xySpecified = value;
			}
		}

		[System.Xml.Serialization.XmlAttributeAttribute("XZ", DataType = "float")]
		public float xz {
			get {
				return this._xz;
			}
			set {
				this._xz = value;
			}
		}

		[System.Xml.Serialization.XmlIgnoreAttribute()]
		public bool xzSpecified {
			get {
				return this._xzSpecified;
			}
			set {
				this._xzSpecified = value;
			}
		}

		[System.Xml.Serialization.XmlAttributeAttribute("YZ", DataType = "float")]
		public float yz {
			get {
				return this._yz;
			}
			set {
				this._yz = value;
			}
		}

		[System.Xml.Serialization.XmlIgnoreAttribute()]
		public bool yzSpecified {
			get {
				return this._yzSpecified;
			}
			set {
				this._yzSpecified = value;
			}
		}

		[System.Xml.Serialization.XmlAttributeAttribute("Colorspace", DataType = "anyURI")]
		public string colorspace {
			get {
				return this._colorspace;
			}
			set {
				this._colorspace = value;
			}
		}
	}

	[System.SerializableAttribute()]
	[System.Xml.Serialization.XmlTypeAttribute(TypeName = "Appearance", Namespace = "http://www.onvif.org/ver10/schema")]
	public partial class Appearance {

		private System.Xml.XmlAttribute[] _anyAttr;

		private Transformation _transformation;

		private ShapeDescriptor _shape;

		private ColorDescriptor _color;

		private ClassDescriptor _class;

		private AppearanceExtension _extension;

		[System.Xml.Serialization.XmlAnyAttributeAttribute()]
		public System.Xml.XmlAttribute[] anyAttr {
			get {
				return this._anyAttr;
			}
			set {
				this._anyAttr = value;
			}
		}

		/// <remarks>optional, order 0</remarks>
		[System.Xml.Serialization.XmlElementAttribute("Transformation", Namespace = "http://www.onvif.org/ver10/schema")]
		public Transformation transformation {
			get {
				return this._transformation;
			}
			set {
				this._transformation = value;
			}
		}

		/// <remarks>optional, order 1</remarks>
		[System.Xml.Serialization.XmlElementAttribute("Shape", Namespace = "http://www.onvif.org/ver10/schema")]
		public ShapeDescriptor shape {
			get {
				return this._shape;
			}
			set {
				this._shape = value;
			}
		}

		/// <remarks>optional, order 2</remarks>
		[System.Xml.Serialization.XmlElementAttribute("Color", Namespace = "http://www.onvif.org/ver10/schema")]
		public ColorDescriptor color {
			get {
				return this._color;
			}
			set {
				this._color = value;
			}
		}

		/// <remarks>optional, order 3</remarks>
		[System.Xml.Serialization.XmlElementAttribute("Class", Namespace = "http://www.onvif.org/ver10/schema")]
		public ClassDescriptor @class {
			get {
				return this._class;
			}
			set {
				this._class = value;
			}
		}

		/// <remarks>optional, order 4</remarks>
		[System.Xml.Serialization.XmlElementAttribute("Extension", Namespace = "http://www.onvif.org/ver10/schema")]
		public AppearanceExtension extension {
			get {
				return this._extension;
			}
			set {
				this._extension = value;
			}
		}
	}

	[System.SerializableAttribute()]
	[System.Xml.Serialization.XmlTypeAttribute(TypeName = "Transformation", Namespace = "http://www.onvif.org/ver10/schema")]
	public partial class Transformation {

		private System.Xml.XmlAttribute[] _anyAttr;

		private Vector _translate;

		private Vector _scale;

		private TransformationExtension _extension;

		[System.Xml.Serialization.XmlAnyAttributeAttribute()]
		public System.Xml.XmlAttribute[] anyAttr {
			get {
				return this._anyAttr;
			}
			set {
				this._anyAttr = value;
			}
		}

		/// <remarks>optional, order 0</remarks>
		[System.Xml.Serialization.XmlElementAttribute("Translate", Namespace = "http://www.onvif.org/ver10/schema")]
		public Vector translate {
			get {
				return this._translate;
			}
			set {
				this._translate = value;
			}
		}

		/// <remarks>optional, order 1</remarks>
		[System.Xml.Serialization.XmlElementAttribute("Scale", Namespace = "http://www.onvif.org/ver10/schema")]
		public Vector scale {
			get {
				return this._scale;
			}
			set {
				this._scale = value;
			}
		}

		/// <remarks>optional, order 2</remarks>
		[System.Xml.Serialization.XmlElementAttribute("Extension", Namespace = "http://www.onvif.org/ver10/schema")]
		public TransformationExtension extension {
			get {
				return this._extension;
			}
			set {
				this._extension = value;
			}
		}
	}

	[System.SerializableAttribute()]
	[System.Xml.Serialization.XmlTypeAttribute(TypeName = "TransformationExtension", Namespace = "http://www.onvif.org/ver10/schema")]
	public partial class TransformationExtension {

		private System.Xml.XmlElement[] _any;

		/// <remarks>optional, order 0, namespace ##any</remarks>
		[System.Xml.Serialization.XmlAnyElementAttribute()]
		public System.Xml.XmlElement[] any {
			get {
				return this._any;
			}
			set {
				this._any = value;
			}
		}
	}

	[System.SerializableAttribute()]
	[System.Xml.Serialization.XmlTypeAttribute(TypeName = "ShapeDescriptor", Namespace = "http://www.onvif.org/ver10/schema")]
	public partial class ShapeDescriptor {

		private System.Xml.XmlAttribute[] _anyAttr;

		private Rectangle _boundingBox;

		private Vector _centerOfGravity;

		private Polygon[] _polygon;

		private ShapeDescriptorExtension _extension;

		[System.Xml.Serialization.XmlAnyAttributeAttribute()]
		public System.Xml.XmlAttribute[] anyAttr {
			get {
				return this._anyAttr;
			}
			set {
				this._anyAttr = value;
			}
		}

		/// <remarks>reqired, order 0</remarks>
		[System.Xml.Serialization.XmlElementAttribute("BoundingBox", Namespace = "http://www.onvif.org/ver10/schema")]
		public Rectangle boundingBox {
			get {
				return this._boundingBox;
			}
			set {
				this._boundingBox = value;
			}
		}

		/// <remarks>reqired, order 1</remarks>
		[System.Xml.Serialization.XmlElementAttribute("CenterOfGravity", Namespace = "http://www.onvif.org/ver10/schema")]
		public Vector centerOfGravity {
			get {
				return this._centerOfGravity;
			}
			set {
				this._centerOfGravity = value;
			}
		}

		/// <remarks>optional, order 2</remarks>
		[System.Xml.Serialization.XmlElementAttribute("Polygon", Namespace = "http://www.onvif.org/ver10/schema")]
		public Polygon[] polygon {
			get {
				return this._polygon;
			}
			set {
				this._polygon = value;
			}
		}

		/// <remarks>optional, order 3</remarks>
		[System.Xml.Serialization.XmlElementAttribute("Extension", Namespace = "http://www.onvif.org/ver10/schema")]
		public ShapeDescriptorExtension extension {
			get {
				return this._extension;
			}
			set {
				this._extension = value;
			}
		}
	}

	[System.SerializableAttribute()]
	[System.Xml.Serialization.XmlTypeAttribute(TypeName = "ShapeDescriptorExtension", Namespace = "http://www.onvif.org/ver10/schema")]
	public partial class ShapeDescriptorExtension {

		private System.Xml.XmlElement[] _any;

		/// <remarks>optional, order 0, namespace ##any</remarks>
		[System.Xml.Serialization.XmlAnyElementAttribute()]
		public System.Xml.XmlElement[] any {
			get {
				return this._any;
			}
			set {
				this._any = value;
			}
		}
	}

	[System.SerializableAttribute()]
	[System.Xml.Serialization.XmlTypeAttribute(TypeName = "ColorDescriptor", Namespace = "http://www.onvif.org/ver10/schema")]
	public partial class ColorDescriptor {

		private System.Xml.XmlAttribute[] _anyAttr;

		private ColorCluster[] _colorCluster;

		private ColorDescriptorExtension _extension;

		[System.Xml.Serialization.XmlAnyAttributeAttribute()]
		public System.Xml.XmlAttribute[] anyAttr {
			get {
				return this._anyAttr;
			}
			set {
				this._anyAttr = value;
			}
		}

		/// <remarks>optional, order 0</remarks>
		[System.Xml.Serialization.XmlElementAttribute("ColorCluster", Namespace = "http://www.onvif.org/ver10/schema")]
		public ColorCluster[] colorCluster {
			get {
				return this._colorCluster;
			}
			set {
				this._colorCluster = value;
			}
		}

		/// <remarks>optional, order 1</remarks>
		[System.Xml.Serialization.XmlElementAttribute("Extension", Namespace = "http://www.onvif.org/ver10/schema")]
		public ColorDescriptorExtension extension {
			get {
				return this._extension;
			}
			set {
				this._extension = value;
			}
		}

		[System.SerializableAttribute()]
		[System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://www.onvif.org/ver10/schema")]
		public partial class ColorCluster {

			private Color _color;

			private float _weight;

			private bool _weightSpecified;

			private ColorCovariance _covariance;

			/// <remarks>reqired, order 0</remarks>
			[System.Xml.Serialization.XmlElementAttribute("Color", Namespace = "http://www.onvif.org/ver10/schema")]
			public Color color {
				get {
					return this._color;
				}
				set {
					this._color = value;
				}
			}

			/// <remarks>optional, order 1</remarks>
			[System.Xml.Serialization.XmlElementAttribute("Weight", Namespace = "http://www.onvif.org/ver10/schema", DataType = "float")]
			public float weight {
				get {
					return this._weight;
				}
				set {
					this._weight = value;
				}
			}

			[System.Xml.Serialization.XmlIgnoreAttribute()]
			public bool weightSpecified {
				get {
					return this._weightSpecified;
				}
				set {
					this._weightSpecified = value;
				}
			}

			/// <remarks>optional, order 2</remarks>
			[System.Xml.Serialization.XmlElementAttribute("Covariance", Namespace = "http://www.onvif.org/ver10/schema")]
			public ColorCovariance covariance {
				get {
					return this._covariance;
				}
				set {
					this._covariance = value;
				}
			}
		}
	}

	[System.SerializableAttribute()]
	[System.Xml.Serialization.XmlTypeAttribute(TypeName = "ColorDescriptorExtension", Namespace = "http://www.onvif.org/ver10/schema")]
	public partial class ColorDescriptorExtension {

		private System.Xml.XmlElement[] _any;

		/// <remarks>optional, order 0, namespace ##any</remarks>
		[System.Xml.Serialization.XmlAnyElementAttribute()]
		public System.Xml.XmlElement[] any {
			get {
				return this._any;
			}
			set {
				this._any = value;
			}
		}
	}

	[System.SerializableAttribute()]
	[System.Xml.Serialization.XmlTypeAttribute(TypeName = "ClassDescriptor", Namespace = "http://www.onvif.org/ver10/schema")]
	public partial class ClassDescriptor {

		private ClassCandidate[] _classCandidate;

		private ClassDescriptorExtension _extension;

		/// <remarks>optional, order 0</remarks>
		[System.Xml.Serialization.XmlElementAttribute("ClassCandidate", Namespace = "http://www.onvif.org/ver10/schema")]
		public ClassCandidate[] classCandidate {
			get {
				return this._classCandidate;
			}
			set {
				this._classCandidate = value;
			}
		}

		/// <remarks>optional, order 1</remarks>
		[System.Xml.Serialization.XmlElementAttribute("Extension", Namespace = "http://www.onvif.org/ver10/schema")]
		public ClassDescriptorExtension extension {
			get {
				return this._extension;
			}
			set {
				this._extension = value;
			}
		}

		[System.SerializableAttribute()]
		[System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://www.onvif.org/ver10/schema")]
		public partial class ClassCandidate {

			private ClassType _type;

			private float _likelihood;

			/// <remarks>reqired, order 0</remarks>
			[System.Xml.Serialization.XmlElementAttribute("Type", Namespace = "http://www.onvif.org/ver10/schema")]
			public ClassType type {
				get {
					return this._type;
				}
				set {
					this._type = value;
				}
			}

			/// <remarks>reqired, order 1</remarks>
			[System.Xml.Serialization.XmlElementAttribute("Likelihood", Namespace = "http://www.onvif.org/ver10/schema", DataType = "float")]
			public float likelihood {
				get {
					return this._likelihood;
				}
				set {
					this._likelihood = value;
				}
			}
		}
	}

	[System.SerializableAttribute()]
	[System.Xml.Serialization.XmlTypeAttribute(TypeName = "ClassType", Namespace = "http://www.onvif.org/ver10/schema")]
	public enum ClassType {

		[System.Xml.Serialization.XmlEnumAttribute(Name = "Animal")]
		animal,

		[System.Xml.Serialization.XmlEnumAttribute(Name = "Face")]
		face,

		[System.Xml.Serialization.XmlEnumAttribute(Name = "Human")]
		human,

		[System.Xml.Serialization.XmlEnumAttribute(Name = "Vehical")]
		vehical,

		[System.Xml.Serialization.XmlEnumAttribute(Name = "Other")]
		other,
	}

	[System.SerializableAttribute()]
	[System.Xml.Serialization.XmlTypeAttribute(TypeName = "ClassDescriptorExtension", Namespace = "http://www.onvif.org/ver10/schema")]
	public partial class ClassDescriptorExtension {

		private System.Xml.XmlElement[] _any;

		private OtherType[] _otherTypes;

		private ClassDescriptorExtension2 _extension;

		/// <remarks>optional, order 0, namespace ##other</remarks>
		[System.Xml.Serialization.XmlAnyElementAttribute()]
		public System.Xml.XmlElement[] any {
			get {
				return this._any;
			}
			set {
				this._any = value;
			}
		}

		/// <remarks>reqired, order 1</remarks>
		[System.Xml.Serialization.XmlElementAttribute("OtherTypes", Namespace = "http://www.onvif.org/ver10/schema")]
		public OtherType[] otherTypes {
			get {
				return this._otherTypes;
			}
			set {
				this._otherTypes = value;
			}
		}

		/// <remarks>optional, order 2</remarks>
		[System.Xml.Serialization.XmlElementAttribute("Extension", Namespace = "http://www.onvif.org/ver10/schema")]
		public ClassDescriptorExtension2 extension {
			get {
				return this._extension;
			}
			set {
				this._extension = value;
			}
		}
	}

	[System.SerializableAttribute()]
	[System.Xml.Serialization.XmlTypeAttribute(TypeName = "OtherType", Namespace = "http://www.onvif.org/ver10/schema")]
	public partial class OtherType {

		private System.Xml.XmlAttribute[] _anyAttr;

		private string _type;

		private float _likelihood;

		private System.Xml.XmlElement[] _any;

		[System.Xml.Serialization.XmlAnyAttributeAttribute()]
		public System.Xml.XmlAttribute[] anyAttr {
			get {
				return this._anyAttr;
			}
			set {
				this._anyAttr = value;
			}
		}

		/// <summary>
		/// Object Class Type
		/// </summary>
		/// <remarks>reqired, order 0</remarks>
		[System.Xml.Serialization.XmlElementAttribute("Type", Namespace = "http://www.onvif.org/ver10/schema", DataType = "string")]
		public string type {
			get {
				return this._type;
			}
			set {
				this._type = value;
			}
		}

		/// <summary>
		/// A likelihood/probability that the corresponding object belongs to this class. The sum of the likelihoods shall NOT exceed 1
		/// </summary>
		/// <remarks>reqired, order 1</remarks>
		[System.Xml.Serialization.XmlElementAttribute("Likelihood", Namespace = "http://www.onvif.org/ver10/schema", DataType = "float")]
		public float likelihood {
			get {
				return this._likelihood;
			}
			set {
				this._likelihood = value;
			}
		}

		/// <remarks>reqired, order 2, namespace ##any</remarks>
		[System.Xml.Serialization.XmlAnyElementAttribute()]
		public System.Xml.XmlElement[] any {
			get {
				return this._any;
			}
			set {
				this._any = value;
			}
		}
	}

	[System.SerializableAttribute()]
	[System.Xml.Serialization.XmlTypeAttribute(TypeName = "ClassDescriptorExtension2", Namespace = "http://www.onvif.org/ver10/schema")]
	public partial class ClassDescriptorExtension2 {

		private System.Xml.XmlAttribute[] _anyAttr;

		private System.Xml.XmlElement[] _any;

		[System.Xml.Serialization.XmlAnyAttributeAttribute()]
		public System.Xml.XmlAttribute[] anyAttr {
			get {
				return this._anyAttr;
			}
			set {
				this._anyAttr = value;
			}
		}

		/// <remarks>optional, order 0, namespace ##targetNamespace</remarks>
		[System.Xml.Serialization.XmlAnyElementAttribute(Namespace = "http://www.onvif.org/ver10/schema")]
		public System.Xml.XmlElement[] any {
			get {
				return this._any;
			}
			set {
				this._any = value;
			}
		}
	}

	[System.SerializableAttribute()]
	[System.Xml.Serialization.XmlTypeAttribute(TypeName = "AppearanceExtension", Namespace = "http://www.onvif.org/ver10/schema")]
	public partial class AppearanceExtension {

		private System.Xml.XmlElement[] _any;

		/// <remarks>optional, order 0, namespace ##any</remarks>
		[System.Xml.Serialization.XmlAnyElementAttribute()]
		public System.Xml.XmlElement[] any {
			get {
				return this._any;
			}
			set {
				this._any = value;
			}
		}
	}

	[System.SerializableAttribute()]
	[System.Xml.Serialization.XmlTypeAttribute(TypeName = "Object", Namespace = "http://www.onvif.org/ver10/schema")]
	public partial class Object : ObjectId {

		private System.Xml.XmlAttribute[] _anyAttr;

		private Appearance _appearance;

		private Behaviour _behaviour;

		private ObjectExtension _extension;


		[System.Xml.Serialization.XmlAnyAttributeAttribute()]
		public System.Xml.XmlAttribute[] anyAttr {
			get {
				return this._anyAttr;
			}
			set {
				this._anyAttr = value;
			}
		}

		/// <remarks>optional, order 0</remarks>
		[System.Xml.Serialization.XmlElementAttribute("Appearance", Namespace = "http://www.onvif.org/ver10/schema")]
		public Appearance appearance {
			get {
				return this._appearance;
			}
			set {
				this._appearance = value;
			}
		}

		/// <remarks>optional, order 1</remarks>
		[System.Xml.Serialization.XmlElementAttribute("Behaviour", Namespace = "http://www.onvif.org/ver10/schema")]
		public Behaviour behaviour {
			get {
				return this._behaviour;
			}
			set {
				this._behaviour = value;
			}
		}

		/// <remarks>optional, order 2</remarks>
		[System.Xml.Serialization.XmlElementAttribute("Extension", Namespace = "http://www.onvif.org/ver10/schema")]
		public ObjectExtension extension {
			get {
				return this._extension;
			}
			set {
				this._extension = value;
			}
		}
	}

	[System.SerializableAttribute()]
	[System.Xml.Serialization.XmlTypeAttribute(TypeName = "Behaviour", Namespace = "http://www.onvif.org/ver10/schema")]
	public partial class Behaviour {

		private System.Xml.XmlAttribute[] _anyAttr;

		private Removed _removed;

		private Idle _idle;

		private BehaviourExtension _extension;

		[System.Xml.Serialization.XmlAnyAttributeAttribute()]
		public System.Xml.XmlAttribute[] anyAttr {
			get {
				return this._anyAttr;
			}
			set {
				this._anyAttr = value;
			}
		}

		/// <remarks>optional, order 0</remarks>
		[System.Xml.Serialization.XmlElementAttribute("Removed", Namespace = "http://www.onvif.org/ver10/schema")]
		public Removed removed {
			get {
				return this._removed;
			}
			set {
				this._removed = value;
			}
		}

		/// <remarks>optional, order 1</remarks>
		[System.Xml.Serialization.XmlElementAttribute("Idle", Namespace = "http://www.onvif.org/ver10/schema")]
		public Idle idle {
			get {
				return this._idle;
			}
			set {
				this._idle = value;
			}
		}

		/// <remarks>optional, order 2</remarks>
		[System.Xml.Serialization.XmlElementAttribute("Extension", Namespace = "http://www.onvif.org/ver10/schema")]
		public BehaviourExtension extension {
			get {
				return this._extension;
			}
			set {
				this._extension = value;
			}
		}

		[System.SerializableAttribute()]
		[System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://www.onvif.org/ver10/schema")]
		public partial class Removed {

			private System.Xml.XmlElement[] _any;

			/// <remarks>optional, order 0, namespace ##other</remarks>
			[System.Xml.Serialization.XmlAnyElementAttribute()]
			public System.Xml.XmlElement[] any {
				get {
					return this._any;
				}
				set {
					this._any = value;
				}
			}
		}

		[System.SerializableAttribute()]
		[System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://www.onvif.org/ver10/schema")]
		public partial class Idle {

			private System.Xml.XmlElement[] _any;

			/// <remarks>optional, order 0, namespace ##other</remarks>
			[System.Xml.Serialization.XmlAnyElementAttribute()]
			public System.Xml.XmlElement[] any {
				get {
					return this._any;
				}
				set {
					this._any = value;
				}
			}
		}
	}

	[System.SerializableAttribute()]
	[System.Xml.Serialization.XmlTypeAttribute(TypeName = "BehaviourExtension", Namespace = "http://www.onvif.org/ver10/schema")]
	public partial class BehaviourExtension {

		private System.Xml.XmlElement[] _any;

		/// <remarks>optional, order 0, namespace ##any</remarks>
		[System.Xml.Serialization.XmlAnyElementAttribute()]
		public System.Xml.XmlElement[] any {
			get {
				return this._any;
			}
			set {
				this._any = value;
			}
		}
	}

	[System.SerializableAttribute()]
	[System.Xml.Serialization.XmlTypeAttribute(TypeName = "ObjectExtension", Namespace = "http://www.onvif.org/ver10/schema")]
	public partial class ObjectExtension {

		private System.Xml.XmlElement[] _any;

		/// <remarks>optional, order 0, namespace ##any</remarks>
		[System.Xml.Serialization.XmlAnyElementAttribute()]
		public System.Xml.XmlElement[] any {
			get {
				return this._any;
			}
			set {
				this._any = value;
			}
		}
	}

	[System.SerializableAttribute()]
	[System.Xml.Serialization.XmlTypeAttribute(TypeName = "Frame", Namespace = "http://www.onvif.org/ver10/schema")]
	public partial class Frame {

		private System.DateTime _utcTime;

		private System.Xml.XmlAttribute[] _anyAttr;

		private PTZStatus _ptzStatus;

		private Transformation _transformation;

		private Object[] _object;

		private ObjectTree _objectTree;

		private FrameExtension _extension;

		[System.Xml.Serialization.XmlAttributeAttribute("UtcTime", DataType = "dateTime")]
		public System.DateTime utcTime {
			get {
				return this._utcTime;
			}
			set {
				this._utcTime = value;
			}
		}

		[System.Xml.Serialization.XmlAnyAttributeAttribute()]
		public System.Xml.XmlAttribute[] anyAttr {
			get {
				return this._anyAttr;
			}
			set {
				this._anyAttr = value;
			}
		}

		/// <remarks>optional, order 0</remarks>
		[System.Xml.Serialization.XmlElementAttribute("PTZStatus", Namespace = "http://www.onvif.org/ver10/schema")]
		public PTZStatus ptzStatus {
			get {
				return this._ptzStatus;
			}
			set {
				this._ptzStatus = value;
			}
		}

		/// <remarks>optional, order 1</remarks>
		[System.Xml.Serialization.XmlElementAttribute("Transformation", Namespace = "http://www.onvif.org/ver10/schema")]
		public Transformation transformation {
			get {
				return this._transformation;
			}
			set {
				this._transformation = value;
			}
		}

		/// <remarks>optional, order 2</remarks>
		[System.Xml.Serialization.XmlElementAttribute("Object", Namespace = "http://www.onvif.org/ver10/schema")]
		public Object[] @object {
			get {
				return this._object;
			}
			set {
				this._object = value;
			}
		}

		/// <remarks>optional, order 3</remarks>
		[System.Xml.Serialization.XmlElementAttribute("ObjectTree", Namespace = "http://www.onvif.org/ver10/schema")]
		public ObjectTree objectTree {
			get {
				return this._objectTree;
			}
			set {
				this._objectTree = value;
			}
		}

		/// <remarks>optional, order 4</remarks>
		[System.Xml.Serialization.XmlElementAttribute("Extension", Namespace = "http://www.onvif.org/ver10/schema")]
		public FrameExtension extension {
			get {
				return this._extension;
			}
			set {
				this._extension = value;
			}
		}
	}

	[System.SerializableAttribute()]
	[System.Xml.Serialization.XmlTypeAttribute(TypeName = "ObjectTree", Namespace = "http://www.onvif.org/ver10/schema")]
	public partial class ObjectTree {

		private System.Xml.XmlAttribute[] _anyAttr;

		private Rename[] _rename;

		private Split[] _split;

		private Merge[] _merge;

		private ObjectId[] _delete;

		private ObjectTreeExtension _extension;

		[System.Xml.Serialization.XmlAnyAttributeAttribute()]
		public System.Xml.XmlAttribute[] anyAttr {
			get {
				return this._anyAttr;
			}
			set {
				this._anyAttr = value;
			}
		}

		/// <remarks>optional, order 0</remarks>
		[System.Xml.Serialization.XmlElementAttribute("Rename", Namespace = "http://www.onvif.org/ver10/schema")]
		public Rename[] rename {
			get {
				return this._rename;
			}
			set {
				this._rename = value;
			}
		}

		/// <remarks>optional, order 1</remarks>
		[System.Xml.Serialization.XmlElementAttribute("Split", Namespace = "http://www.onvif.org/ver10/schema")]
		public Split[] split {
			get {
				return this._split;
			}
			set {
				this._split = value;
			}
		}

		/// <remarks>optional, order 2</remarks>
		[System.Xml.Serialization.XmlElementAttribute("Merge", Namespace = "http://www.onvif.org/ver10/schema")]
		public Merge[] merge {
			get {
				return this._merge;
			}
			set {
				this._merge = value;
			}
		}

		/// <remarks>optional, order 3</remarks>
		[System.Xml.Serialization.XmlElementAttribute("Delete", Namespace = "http://www.onvif.org/ver10/schema")]
		public ObjectId[] delete {
			get {
				return this._delete;
			}
			set {
				this._delete = value;
			}
		}

		/// <remarks>optional, order 4</remarks>
		[System.Xml.Serialization.XmlElementAttribute("Extension", Namespace = "http://www.onvif.org/ver10/schema")]
		public ObjectTreeExtension extension {
			get {
				return this._extension;
			}
			set {
				this._extension = value;
			}
		}
	}

	[System.SerializableAttribute()]
	[System.Xml.Serialization.XmlTypeAttribute(TypeName = "Rename", Namespace = "http://www.onvif.org/ver10/schema")]
	public partial class Rename {

		private ObjectId _from;

		private ObjectId _to;

		/// <remarks>reqired, order 0</remarks>
		[System.Xml.Serialization.XmlElementAttribute("from", Namespace = "http://www.onvif.org/ver10/schema")]
		public ObjectId from {
			get {
				return this._from;
			}
			set {
				this._from = value;
			}
		}

		/// <remarks>reqired, order 1</remarks>
		[System.Xml.Serialization.XmlElementAttribute("to", Namespace = "http://www.onvif.org/ver10/schema")]
		public ObjectId to {
			get {
				return this._to;
			}
			set {
				this._to = value;
			}
		}
	}

	[System.Xml.Serialization.XmlIncludeAttribute(typeof(Object))]
	[System.SerializableAttribute()]
	[System.Xml.Serialization.XmlTypeAttribute(TypeName = "ObjectId", Namespace = "http://www.onvif.org/ver10/schema")]
	public partial class ObjectId {

		private string _objectId;

		[System.Xml.Serialization.XmlAttributeAttribute("ObjectId", DataType = "integer")]
		public string objectId {
			get {
				return this._objectId;
			}
			set {
				this._objectId = value;
			}
		}
	}

	[System.SerializableAttribute()]
	[System.Xml.Serialization.XmlTypeAttribute(TypeName = "Split", Namespace = "http://www.onvif.org/ver10/schema")]
	public partial class Split {

		private ObjectId _from;

		private ObjectId[] _to;

		/// <remarks>reqired, order 0</remarks>
		[System.Xml.Serialization.XmlElementAttribute("from", Namespace = "http://www.onvif.org/ver10/schema")]
		public ObjectId from {
			get {
				return this._from;
			}
			set {
				this._from = value;
			}
		}

		/// <remarks>reqired, order 1</remarks>
		[System.Xml.Serialization.XmlElementAttribute("to", Namespace = "http://www.onvif.org/ver10/schema")]
		public ObjectId[] to {
			get {
				return this._to;
			}
			set {
				this._to = value;
			}
		}
	}

	[System.SerializableAttribute()]
	[System.Xml.Serialization.XmlTypeAttribute(TypeName = "Merge", Namespace = "http://www.onvif.org/ver10/schema")]
	public partial class Merge {

		private ObjectId[] _from;

		private ObjectId _to;

		/// <remarks>reqired, order 0</remarks>
		[System.Xml.Serialization.XmlElementAttribute("from", Namespace = "http://www.onvif.org/ver10/schema")]
		public ObjectId[] from {
			get {
				return this._from;
			}
			set {
				this._from = value;
			}
		}

		/// <remarks>reqired, order 1</remarks>
		[System.Xml.Serialization.XmlElementAttribute("to", Namespace = "http://www.onvif.org/ver10/schema")]
		public ObjectId to {
			get {
				return this._to;
			}
			set {
				this._to = value;
			}
		}
	}

	[System.SerializableAttribute()]
	[System.Xml.Serialization.XmlTypeAttribute(TypeName = "ObjectTreeExtension", Namespace = "http://www.onvif.org/ver10/schema")]
	public partial class ObjectTreeExtension {

		private System.Xml.XmlElement[] _any;

		/// <remarks>optional, order 0, namespace ##any</remarks>
		[System.Xml.Serialization.XmlAnyElementAttribute()]
		public System.Xml.XmlElement[] any {
			get {
				return this._any;
			}
			set {
				this._any = value;
			}
		}
	}

	[System.SerializableAttribute()]
	[System.Xml.Serialization.XmlTypeAttribute(TypeName = "FrameExtension", Namespace = "http://www.onvif.org/ver10/schema")]
	public partial class FrameExtension {

		private System.Xml.XmlElement[] _any;

		private MotionInCells _motionInCells;

		private FrameExtension2 _extension;

		/// <remarks>optional, order 0, namespace ##other</remarks>
		[System.Xml.Serialization.XmlAnyElementAttribute()]
		public System.Xml.XmlElement[] any {
			get {
				return this._any;
			}
			set {
				this._any = value;
			}
		}

		/// <remarks>optional, order 1</remarks>
		[System.Xml.Serialization.XmlElementAttribute("MotionInCells", Namespace = "http://www.onvif.org/ver10/schema")]
		public MotionInCells motionInCells {
			get {
				return this._motionInCells;
			}
			set {
				this._motionInCells = value;
			}
		}

		/// <remarks>optional, order 2</remarks>
		[System.Xml.Serialization.XmlElementAttribute("Extension", Namespace = "http://www.onvif.org/ver10/schema")]
		public FrameExtension2 extension {
			get {
				return this._extension;
			}
			set {
				this._extension = value;
			}
		}
	}

	[System.SerializableAttribute()]
	[System.Xml.Serialization.XmlTypeAttribute(TypeName = "MotionInCells", Namespace = "http://www.onvif.org/ver10/schema")]
	public partial class MotionInCells {

		private string _columns;

		private string _rows;

		private byte[] _cells;

		private System.Xml.XmlAttribute[] _anyAttr;

		private System.Xml.XmlElement[] _any;

		/// <summary>
		/// Number of columns of the cell grid (x dimension)
		/// </summary>
		[System.Xml.Serialization.XmlAttributeAttribute("Columns", DataType = "integer")]
		public string columns {
			get {
				return this._columns;
			}
			set {
				this._columns = value;
			}
		}

		/// <summary>
		/// Number of rows of the cell grid (y dimension)
		/// </summary>
		[System.Xml.Serialization.XmlAttributeAttribute("Rows", DataType = "integer")]
		public string rows {
			get {
				return this._rows;
			}
			set {
				this._rows = value;
			}
		}

		/// <summary>
		/// A "1" denotes a cell where motion is detected and a "0" an empty cell. The first cell is in the upper left corner. Then the cell order goes first from left to right and then from up to down.  If the number of cells is not a multiple of 8 the last byte is filled with zeros. The information is run length encoded according to Packbit coding in ISO 12369 (TIFF, Revision 6.0).
		/// </summary>
		[System.Xml.Serialization.XmlAttributeAttribute("Cells", DataType = "base64Binary")]
		public byte[] cells {
			get {
				return this._cells;
			}
			set {
				this._cells = value;
			}
		}

		[System.Xml.Serialization.XmlAnyAttributeAttribute()]
		public System.Xml.XmlAttribute[] anyAttr {
			get {
				return this._anyAttr;
			}
			set {
				this._anyAttr = value;
			}
		}

		/// <remarks>optional, order 0, namespace ##any</remarks>
		[System.Xml.Serialization.XmlAnyElementAttribute()]
		public System.Xml.XmlElement[] any {
			get {
				return this._any;
			}
			set {
				this._any = value;
			}
		}
	}

	[System.SerializableAttribute()]
	[System.Xml.Serialization.XmlTypeAttribute(TypeName = "FrameExtension2", Namespace = "http://www.onvif.org/ver10/schema")]
	public partial class FrameExtension2 {

		private System.Xml.XmlElement[] _any;

		/// <remarks>optional, order 0, namespace ##targetNamespace</remarks>
		[System.Xml.Serialization.XmlAnyElementAttribute(Namespace = "http://www.onvif.org/ver10/schema")]
		public System.Xml.XmlElement[] any {
			get {
				return this._any;
			}
			set {
				this._any = value;
			}
		}
	}

	[System.SerializableAttribute()]
	[System.Xml.Serialization.XmlTypeAttribute(TypeName = "ConfigDescription", Namespace = "http://www.onvif.org/ver10/schema")]
	public partial class ConfigDescription {

		private System.Xml.XmlQualifiedName _name;

		private System.Xml.XmlAttribute[] _anyAttr;

		private ItemListDescription _parameters;

		private Messages[] _messages;

		private ConfigDescriptionExtension _extension;

		/// <summary>
		/// XML Type of the Configuration (e.g. "tt::LineDetector").
		/// </summary>
		[System.Xml.Serialization.XmlAttributeAttribute("Name", DataType = "QName")]
		public System.Xml.XmlQualifiedName name {
			get {
				return this._name;
			}
			set {
				this._name = value;
			}
		}

		[System.Xml.Serialization.XmlAnyAttributeAttribute()]
		public System.Xml.XmlAttribute[] anyAttr {
			get {
				return this._anyAttr;
			}
			set {
				this._anyAttr = value;
			}
		}

		/// <summary>
		/// List describing the configuration parameters. The names of the parameters must be unique. If possible SimpleItems
		/// should be used to transport the information to ease parsing of dynamically defined messages by a client
		/// application.
		/// </summary>
		/// <remarks>reqired, order 0</remarks>
		[System.Xml.Serialization.XmlElementAttribute("Parameters", Namespace = "http://www.onvif.org/ver10/schema")]
		public ItemListDescription parameters {
			get {
				return this._parameters;
			}
			set {
				this._parameters = value;
			}
		}

		/// <summary>
		/// The analytics modules and rule engine produce Events, which must be listed within the Analytics Module Description. In order to do so
		/// the structure of the Message is defined and consists of three groups: Source, Key, and Data. It is recommended to use SimpleItemDescriptions wherever applicable.
		/// The name of all Items must be unique within all Items contained in any group of this Message.
		/// Depending on the component multiple parameters or none may be needed to identify the component uniquely.
		/// </summary>
		/// <remarks>optional, order 1</remarks>
		[System.Xml.Serialization.XmlElementAttribute("Messages", Namespace = "http://www.onvif.org/ver10/schema")]
		public Messages[] messages {
			get {
				return this._messages;
			}
			set {
				this._messages = value;
			}
		}

		/// <remarks>optional, order 2</remarks>
		[System.Xml.Serialization.XmlElementAttribute("Extension", Namespace = "http://www.onvif.org/ver10/schema")]
		public ConfigDescriptionExtension extension {
			get {
				return this._extension;
			}
			set {
				this._extension = value;
			}
		}

		[System.SerializableAttribute()]
		[System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://www.onvif.org/ver10/schema")]
		public partial class Messages : MessageDescription {

			private string _parentTopic;

			/// <summary>
			/// The ParentTopic labels the message (e.g. "nn:RuleEngine/LineCrossing"). The real message can extend the ParentTopic
			/// by for example the name of the instaniated rule (e.g. "nn:RuleEngine/LineCrossing/corssMyFirstLine").
			/// Even without knowing the complete topic name, the subscriber will be able to distiguish the
			/// messages produced by different rule instances of the same type via the Source fields of the message.
			/// There the name of the rule instance, which produced the message, must be listed.
			/// </summary>
			/// <remarks>reqired, order 4</remarks>
			[System.Xml.Serialization.XmlElementAttribute("ParentTopic", Namespace = "http://www.onvif.org/ver10/schema", DataType = "string")]
			public string parentTopic {
				get {
					return this._parentTopic;
				}
				set {
					this._parentTopic = value;
				}
			}
		}
	}

	[System.SerializableAttribute()]
	[System.Xml.Serialization.XmlTypeAttribute(TypeName = "ConfigDescriptionExtension", Namespace = "http://www.onvif.org/ver10/schema")]
	public partial class ConfigDescriptionExtension {

		private System.Xml.XmlElement[] _any;

		/// <remarks>optional, order 0, namespace ##any</remarks>
		[System.Xml.Serialization.XmlAnyElementAttribute()]
		public System.Xml.XmlElement[] any {
			get {
				return this._any;
			}
			set {
				this._any = value;
			}
		}
	}

	[System.SerializableAttribute()]
	[System.Xml.Serialization.XmlTypeAttribute(TypeName = "SupportedRules", Namespace = "http://www.onvif.org/ver10/schema")]
	public partial class SupportedRules {

		private System.Xml.XmlAttribute[] _anyAttr;

		private string[] _ruleContentSchemaLocation;

		private ConfigDescription[] _ruleDescription;

		private SupportedRulesExtension _extension;

		[System.Xml.Serialization.XmlAnyAttributeAttribute()]
		public System.Xml.XmlAttribute[] anyAttr {
			get {
				return this._anyAttr;
			}
			set {
				this._anyAttr = value;
			}
		}

		/// <summary>
		/// Lists the location of all schemas that are referenced in the rules.
		/// </summary>
		/// <remarks>optional, order 0</remarks>
		[System.Xml.Serialization.XmlElementAttribute("RuleContentSchemaLocation", Namespace = "http://www.onvif.org/ver10/schema", DataType = "anyURI")]
		public string[] ruleContentSchemaLocation {
			get {
				return this._ruleContentSchemaLocation;
			}
			set {
				this._ruleContentSchemaLocation = value;
			}
		}

		/// <summary>
		/// List of rules supported by the Video Analytics configuration..
		/// </summary>
		/// <remarks>optional, order 1</remarks>
		[System.Xml.Serialization.XmlElementAttribute("RuleDescription", Namespace = "http://www.onvif.org/ver10/schema")]
		public ConfigDescription[] ruleDescription {
			get {
				return this._ruleDescription;
			}
			set {
				this._ruleDescription = value;
			}
		}

		/// <remarks>optional, order 2</remarks>
		[System.Xml.Serialization.XmlElementAttribute("Extension", Namespace = "http://www.onvif.org/ver10/schema")]
		public SupportedRulesExtension extension {
			get {
				return this._extension;
			}
			set {
				this._extension = value;
			}
		}
	}

	[System.SerializableAttribute()]
	[System.Xml.Serialization.XmlTypeAttribute(TypeName = "SupportedRulesExtension", Namespace = "http://www.onvif.org/ver10/schema")]
	public partial class SupportedRulesExtension {

		private System.Xml.XmlElement[] _any;

		/// <remarks>optional, order 0, namespace ##any</remarks>
		[System.Xml.Serialization.XmlAnyElementAttribute()]
		public System.Xml.XmlElement[] any {
			get {
				return this._any;
			}
			set {
				this._any = value;
			}
		}
	}

	[System.SerializableAttribute()]
	[System.Xml.Serialization.XmlTypeAttribute(TypeName = "SupportedAnalyticsModules", Namespace = "http://www.onvif.org/ver10/schema")]
	public partial class SupportedAnalyticsModules {

		private System.Xml.XmlAttribute[] _anyAttr;

		private string[] _analyticsModuleContentSchemaLocation;

		private ConfigDescription[] _analyticsModuleDescription;

		private SupportedAnalyticsModulesExtension _extension;

		[System.Xml.Serialization.XmlAnyAttributeAttribute()]
		public System.Xml.XmlAttribute[] anyAttr {
			get {
				return this._anyAttr;
			}
			set {
				this._anyAttr = value;
			}
		}

		/// <summary>
		/// It optionally contains a list of URLs that provide the location of schema files.
		/// These schema files describe the types and elements used in the analytics module descriptions.
		/// If the analytics module descriptions reference types or elements of the ONVIF schema file,
		/// the ONVIF schema file MUST be explicitly listed.
		/// </summary>
		/// <remarks>optional, order 0</remarks>
		[System.Xml.Serialization.XmlElementAttribute("AnalyticsModuleContentSchemaLocation", Namespace = "http://www.onvif.org/ver10/schema", DataType = "anyURI")]
		public string[] analyticsModuleContentSchemaLocation {
			get {
				return this._analyticsModuleContentSchemaLocation;
			}
			set {
				this._analyticsModuleContentSchemaLocation = value;
			}
		}

		/// <remarks>optional, order 1</remarks>
		[System.Xml.Serialization.XmlElementAttribute("AnalyticsModuleDescription", Namespace = "http://www.onvif.org/ver10/schema")]
		public ConfigDescription[] analyticsModuleDescription {
			get {
				return this._analyticsModuleDescription;
			}
			set {
				this._analyticsModuleDescription = value;
			}
		}

		/// <remarks>optional, order 2</remarks>
		[System.Xml.Serialization.XmlElementAttribute("Extension", Namespace = "http://www.onvif.org/ver10/schema")]
		public SupportedAnalyticsModulesExtension extension {
			get {
				return this._extension;
			}
			set {
				this._extension = value;
			}
		}
	}

	[System.SerializableAttribute()]
	[System.Xml.Serialization.XmlTypeAttribute(TypeName = "SupportedAnalyticsModulesExtension", Namespace = "http://www.onvif.org/ver10/schema")]
	public partial class SupportedAnalyticsModulesExtension {

		private System.Xml.XmlElement[] _any;

		/// <remarks>optional, order 0, namespace ##any</remarks>
		[System.Xml.Serialization.XmlAnyElementAttribute()]
		public System.Xml.XmlElement[] any {
			get {
				return this._any;
			}
			set {
				this._any = value;
			}
		}
	}

	[System.SerializableAttribute()]
	[System.Xml.Serialization.XmlTypeAttribute(TypeName = "PolygonConfiguration", Namespace = "http://www.onvif.org/ver10/schema")]
	public partial class PolygonConfiguration {

		private System.Xml.XmlAttribute[] _anyAttr;

		private Polygon _polygon;

		private System.Xml.XmlElement[] _any;

		[System.Xml.Serialization.XmlAnyAttributeAttribute()]
		public System.Xml.XmlAttribute[] anyAttr {
			get {
				return this._anyAttr;
			}
			set {
				this._anyAttr = value;
			}
		}

		/// <summary>
		/// Contains Polygon configuration for rule parameters
		/// </summary>
		/// <remarks>reqired, order 0</remarks>
		[System.Xml.Serialization.XmlElementAttribute("Polygon", Namespace = "http://www.onvif.org/ver10/schema")]
		public Polygon polygon {
			get {
				return this._polygon;
			}
			set {
				this._polygon = value;
			}
		}

		/// <remarks>optional, order 1, namespace ##any</remarks>
		[System.Xml.Serialization.XmlAnyElementAttribute()]
		public System.Xml.XmlElement[] any {
			get {
				return this._any;
			}
			set {
				this._any = value;
			}
		}
	}

	[System.SerializableAttribute()]
	[System.Xml.Serialization.XmlTypeAttribute(TypeName = "PolylineArray", Namespace = "http://www.onvif.org/ver10/schema")]
	public partial class PolylineArray {

		private System.Xml.XmlAttribute[] _anyAttr;

		private Polyline[] _segment;

		private PolylineArrayExtension _extension;

		[System.Xml.Serialization.XmlAnyAttributeAttribute()]
		public System.Xml.XmlAttribute[] anyAttr {
			get {
				return this._anyAttr;
			}
			set {
				this._anyAttr = value;
			}
		}

		/// <summary>
		/// Contains array of Polyline
		/// </summary>
		/// <remarks>reqired, order 0</remarks>
		[System.Xml.Serialization.XmlElementAttribute("Segment", Namespace = "http://www.onvif.org/ver10/schema")]
		public Polyline[] segment {
			get {
				return this._segment;
			}
			set {
				this._segment = value;
			}
		}

		/// <remarks>optional, order 1</remarks>
		[System.Xml.Serialization.XmlElementAttribute("Extension", Namespace = "http://www.onvif.org/ver10/schema")]
		public PolylineArrayExtension extension {
			get {
				return this._extension;
			}
			set {
				this._extension = value;
			}
		}
	}

	[System.SerializableAttribute()]
	[System.Xml.Serialization.XmlTypeAttribute(TypeName = "PolylineArrayExtension", Namespace = "http://www.onvif.org/ver10/schema")]
	public partial class PolylineArrayExtension {

		private System.Xml.XmlElement[] _any;

		/// <remarks>optional, order 0, namespace ##any</remarks>
		[System.Xml.Serialization.XmlAnyElementAttribute()]
		public System.Xml.XmlElement[] any {
			get {
				return this._any;
			}
			set {
				this._any = value;
			}
		}
	}

	[System.SerializableAttribute()]
	[System.Xml.Serialization.XmlTypeAttribute(TypeName = "PolylineArrayConfiguration", Namespace = "http://www.onvif.org/ver10/schema")]
	public partial class PolylineArrayConfiguration {

		private System.Xml.XmlAttribute[] _anyAttr;

		private PolylineArray _polylineArray;

		private System.Xml.XmlElement[] _any;

		[System.Xml.Serialization.XmlAnyAttributeAttribute()]
		public System.Xml.XmlAttribute[] anyAttr {
			get {
				return this._anyAttr;
			}
			set {
				this._anyAttr = value;
			}
		}

		/// <summary>
		/// Contains PolylineArray configuration data
		/// </summary>
		/// <remarks>reqired, order 0</remarks>
		[System.Xml.Serialization.XmlElementAttribute("PolylineArray", Namespace = "http://www.onvif.org/ver10/schema")]
		public PolylineArray polylineArray {
			get {
				return this._polylineArray;
			}
			set {
				this._polylineArray = value;
			}
		}

		/// <remarks>optional, order 1, namespace ##any</remarks>
		[System.Xml.Serialization.XmlAnyElementAttribute()]
		public System.Xml.XmlElement[] any {
			get {
				return this._any;
			}
			set {
				this._any = value;
			}
		}
	}

	[System.SerializableAttribute()]
	[System.Xml.Serialization.XmlTypeAttribute(TypeName = "MotionExpression", Namespace = "http://www.onvif.org/ver10/schema")]
	public partial class MotionExpression {

		private string _type;

		private System.Xml.XmlAttribute[] _anyAttr;

		private string _expression;

		private System.Xml.XmlElement[] _any;

		[System.Xml.Serialization.XmlAttributeAttribute("Type", DataType = "string")]
		public string type {
			get {
				return this._type;
			}
			set {
				this._type = value;
			}
		}

		[System.Xml.Serialization.XmlAnyAttributeAttribute()]
		public System.Xml.XmlAttribute[] anyAttr {
			get {
				return this._anyAttr;
			}
			set {
				this._anyAttr = value;
			}
		}

		/// <summary>
		/// Motion Expression data structure contains motion expression which is based on Scene Descriptor schema with XPATH syntax. The Type argument could allow introduction of different dialects
		/// </summary>
		/// <remarks>reqired, order 0</remarks>
		[System.Xml.Serialization.XmlElementAttribute("Expression", Namespace = "http://www.onvif.org/ver10/schema", DataType = "string")]
		public string expression {
			get {
				return this._expression;
			}
			set {
				this._expression = value;
			}
		}

		/// <remarks>optional, order 1, namespace ##any</remarks>
		[System.Xml.Serialization.XmlAnyElementAttribute()]
		public System.Xml.XmlElement[] any {
			get {
				return this._any;
			}
			set {
				this._any = value;
			}
		}
	}

	[System.SerializableAttribute()]
	[System.Xml.Serialization.XmlTypeAttribute(TypeName = "MotionExpressionConfiguration", Namespace = "http://www.onvif.org/ver10/schema")]
	public partial class MotionExpressionConfiguration {

		private System.Xml.XmlAttribute[] _anyAttr;

		private MotionExpression _motionExpression;

		private System.Xml.XmlElement[] _any;

		[System.Xml.Serialization.XmlAnyAttributeAttribute()]
		public System.Xml.XmlAttribute[] anyAttr {
			get {
				return this._anyAttr;
			}
			set {
				this._anyAttr = value;
			}
		}

		/// <summary>
		/// Contains Rule MotionExpression configuration
		/// </summary>
		/// <remarks>reqired, order 0</remarks>
		[System.Xml.Serialization.XmlElementAttribute("MotionExpression", Namespace = "http://www.onvif.org/ver10/schema")]
		public MotionExpression motionExpression {
			get {
				return this._motionExpression;
			}
			set {
				this._motionExpression = value;
			}
		}

		/// <remarks>optional, order 1, namespace ##any</remarks>
		[System.Xml.Serialization.XmlAnyElementAttribute()]
		public System.Xml.XmlElement[] any {
			get {
				return this._any;
			}
			set {
				this._any = value;
			}
		}
	}

	[System.SerializableAttribute()]
	[System.Xml.Serialization.XmlTypeAttribute(TypeName = "CellLayout", Namespace = "http://www.onvif.org/ver10/schema")]
	public partial class CellLayout {

		private string _columns;

		private string _rows;

		private System.Xml.XmlAttribute[] _anyAttr;

		private Transformation _transformation;

		private System.Xml.XmlElement[] _any;

		/// <summary>
		/// Number of columns of the cell grid (x dimension)
		/// </summary>
		[System.Xml.Serialization.XmlAttributeAttribute("Columns", DataType = "integer")]
		public string columns {
			get {
				return this._columns;
			}
			set {
				this._columns = value;
			}
		}

		/// <summary>
		/// Number of rows of the cell grid (y dimension)
		/// </summary>
		[System.Xml.Serialization.XmlAttributeAttribute("Rows", DataType = "integer")]
		public string rows {
			get {
				return this._rows;
			}
			set {
				this._rows = value;
			}
		}

		[System.Xml.Serialization.XmlAnyAttributeAttribute()]
		public System.Xml.XmlAttribute[] anyAttr {
			get {
				return this._anyAttr;
			}
			set {
				this._anyAttr = value;
			}
		}

		/// <summary>
		/// Mapping of the cell grid to the Video frame. The cell grid is starting from the upper left corner and x dimension is going from left to right and the y dimension from up to down.
		/// </summary>
		/// <remarks>reqired, order 0</remarks>
		[System.Xml.Serialization.XmlElementAttribute("Transformation", Namespace = "http://www.onvif.org/ver10/schema")]
		public Transformation transformation {
			get {
				return this._transformation;
			}
			set {
				this._transformation = value;
			}
		}

		/// <remarks>optional, order 1, namespace ##any</remarks>
		[System.Xml.Serialization.XmlAnyElementAttribute()]
		public System.Xml.XmlElement[] any {
			get {
				return this._any;
			}
			set {
				this._any = value;
			}
		}
	}

	[System.SerializableAttribute()]
	[System.Xml.Serialization.XmlTypeAttribute(TypeName = "MetadataStream", Namespace = "http://www.onvif.org/ver10/schema")]
	[System.Xml.Serialization.XmlRootAttribute(Namespace = "http://www.onvif.org/ver10/schema", IsNullable = false)]
	public partial class MetadataStream {

		private System.Xml.XmlAttribute[] _anyAttr;

		private object[] _items;

		[System.Xml.Serialization.XmlAnyAttributeAttribute()]
		public System.Xml.XmlAttribute[] anyAttr {
			get {
				return this._anyAttr;
			}
			set {
				this._anyAttr = value;
			}
		}

		/// <summary>
		/// VideoAnalytics of type <see cref="onvif.types.VideoAnalyticsStream"/>: 
		/// PTZ of type <see cref="onvif.types.PTZStream"/>: 
		/// Event of type <see cref="onvif.types.EventStream"/>: 
		/// Extension of type <see cref="onvif.types.MetadataStreamExtension"/>: 
		/// </summary>
		/// <remarks>optional, order 0</remarks>
		[System.Xml.Serialization.XmlElementAttribute("VideoAnalytics", Namespace = "http://www.onvif.org/ver10/schema", Type = typeof(VideoAnalyticsStream))]
		[System.Xml.Serialization.XmlElementAttribute("PTZ", Namespace = "http://www.onvif.org/ver10/schema", Type = typeof(PTZStream))]
		[System.Xml.Serialization.XmlElementAttribute("Event", Namespace = "http://www.onvif.org/ver10/schema", Type = typeof(EventStream))]
		[System.Xml.Serialization.XmlElementAttribute("Extension", Namespace = "http://www.onvif.org/ver10/schema", Type = typeof(MetadataStreamExtension))]
		public object[] items {
			get {
				return this._items;
			}
			set {
				this._items = value;
			}
		}
	}

	[System.SerializableAttribute()]
	[System.Xml.Serialization.XmlTypeAttribute(TypeName = "VideoAnalyticsStream", Namespace = "http://www.onvif.org/ver10/schema")]
	public partial class VideoAnalyticsStream {

		private object[] _items;

		/// <summary>
		/// Frame of type <see cref="onvif.types.Frame"/>: 
		/// Extension of type <see cref="onvif.types.VideoAnalyticsStreamExtension"/>: 
		/// </summary>
		/// <remarks>optional, order 0</remarks>
		[System.Xml.Serialization.XmlElementAttribute("Frame", Namespace = "http://www.onvif.org/ver10/schema", Type = typeof(Frame))]
		[System.Xml.Serialization.XmlElementAttribute("Extension", Namespace = "http://www.onvif.org/ver10/schema", Type = typeof(VideoAnalyticsStreamExtension))]
		public object[] items {
			get {
				return this._items;
			}
			set {
				this._items = value;
			}
		}
	}

	[System.SerializableAttribute()]
	[System.Xml.Serialization.XmlTypeAttribute(TypeName = "VideoAnalyticsStreamExtension", Namespace = "http://www.onvif.org/ver10/schema")]
	public partial class VideoAnalyticsStreamExtension {

		private System.Xml.XmlElement[] _any;

		/// <remarks>optional, order 0, namespace ##any</remarks>
		[System.Xml.Serialization.XmlAnyElementAttribute()]
		public System.Xml.XmlElement[] any {
			get {
				return this._any;
			}
			set {
				this._any = value;
			}
		}
	}

	[System.SerializableAttribute()]
	[System.Xml.Serialization.XmlTypeAttribute(TypeName = "PTZStream", Namespace = "http://www.onvif.org/ver10/schema")]
	public partial class PTZStream {

		private object[] _items;

		/// <summary>
		/// PTZStatus of type <see cref="onvif.types.PTZStatus"/>: 
		/// Extension of type <see cref="onvif.types.PTZStreamExtension"/>: 
		/// </summary>
		/// <remarks>optional, order 0</remarks>
		[System.Xml.Serialization.XmlElementAttribute("PTZStatus", Namespace = "http://www.onvif.org/ver10/schema", Type = typeof(PTZStatus))]
		[System.Xml.Serialization.XmlElementAttribute("Extension", Namespace = "http://www.onvif.org/ver10/schema", Type = typeof(PTZStreamExtension))]
		public object[] items {
			get {
				return this._items;
			}
			set {
				this._items = value;
			}
		}
	}

	[System.SerializableAttribute()]
	[System.Xml.Serialization.XmlTypeAttribute(TypeName = "PTZStreamExtension", Namespace = "http://www.onvif.org/ver10/schema")]
	public partial class PTZStreamExtension {

		private System.Xml.XmlElement[] _any;

		/// <remarks>optional, order 0, namespace ##any</remarks>
		[System.Xml.Serialization.XmlAnyElementAttribute()]
		public System.Xml.XmlElement[] any {
			get {
				return this._any;
			}
			set {
				this._any = value;
			}
		}
	}

	[System.SerializableAttribute()]
	[System.Xml.Serialization.XmlTypeAttribute(TypeName = "EventStream", Namespace = "http://www.onvif.org/ver10/schema")]
	public partial class EventStream {

		private object[] _items;

		/// <summary>
		/// NotificationMessage of type <see cref="onvif.wsnt.NotificationMessageHolderType"/>: 
		/// Extension of type <see cref="onvif.types.EventStreamExtension"/>: 
		/// </summary>
		/// <remarks>optional, order 0</remarks>
		[System.Xml.Serialization.XmlElementAttribute("NotificationMessage", Namespace = "http://docs.oasis-open.org/wsn/b-2", Type = typeof(NotificationMessageHolderType))]
		[System.Xml.Serialization.XmlElementAttribute("Extension", Namespace = "http://www.onvif.org/ver10/schema", Type = typeof(EventStreamExtension))]
		public object[] items {
			get {
				return this._items;
			}
			set {
				this._items = value;
			}
		}
	}

	[System.SerializableAttribute()]
	[System.Xml.Serialization.XmlTypeAttribute(TypeName = "EventStreamExtension", Namespace = "http://www.onvif.org/ver10/schema")]
	public partial class EventStreamExtension {

		private System.Xml.XmlElement[] _any;

		/// <remarks>optional, order 0, namespace ##any</remarks>
		[System.Xml.Serialization.XmlAnyElementAttribute()]
		public System.Xml.XmlElement[] any {
			get {
				return this._any;
			}
			set {
				this._any = value;
			}
		}
	}

	[System.SerializableAttribute()]
	[System.Xml.Serialization.XmlTypeAttribute(TypeName = "MetadataStreamExtension", Namespace = "http://www.onvif.org/ver10/schema")]
	public partial class MetadataStreamExtension {

		private System.Xml.XmlElement[] _any;

		/// <remarks>optional, order 0, namespace ##any</remarks>
		[System.Xml.Serialization.XmlAnyElementAttribute()]
		public System.Xml.XmlElement[] any {
			get {
				return this._any;
			}
			set {
				this._any = value;
			}
		}
	}

	/// <summary>
	/// Configuration of the streaming and coding settings of a Video window.
	/// </summary>
	[System.SerializableAttribute()]
	[System.Xml.Serialization.XmlTypeAttribute(TypeName = "PaneConfiguration", Namespace = "http://www.onvif.org/ver10/schema")]
	public partial class PaneConfiguration {

		private System.Xml.XmlAttribute[] _anyAttr;

		private string _paneName;

		private string _audioOutputToken;

		private string _audioSourceToken;

		private AudioEncoderConfiguration _audioEncoderConfiguration;

		private string _receiverToken;

		private string _token;

		private System.Xml.XmlElement[] _any;

		[System.Xml.Serialization.XmlAnyAttributeAttribute()]
		public System.Xml.XmlAttribute[] anyAttr {
			get {
				return this._anyAttr;
			}
			set {
				this._anyAttr = value;
			}
		}

		/// <summary>
		/// Optional name of the pane configuration.
		/// </summary>
		/// <remarks>optional, order 0</remarks>
		[System.Xml.Serialization.XmlElementAttribute("PaneName", Namespace = "http://www.onvif.org/ver10/schema", DataType = "string")]
		public string paneName {
			get {
				return this._paneName;
			}
			set {
				this._paneName = value;
			}
		}

		/// <summary>
		/// If the device has audio outputs, this element contains a pointer to the audio output that is associated with the pane. A client
		/// can retrieve the available audio outputs of a device using the GetAudioOutputs command of the DeviceIO service.
		/// </summary>
		/// <remarks>optional, order 1</remarks>
		[System.Xml.Serialization.XmlElementAttribute("AudioOutputToken", Namespace = "http://www.onvif.org/ver10/schema", DataType = "string")]
		public string audioOutputToken {
			get {
				return this._audioOutputToken;
			}
			set {
				this._audioOutputToken = value;
			}
		}

		/// <summary>
		/// If the device has audio sources, this element contains a pointer to the audio source that is associated with this pane.
		/// The audio connection from a decoder device to the NVT is established using the backchannel mechanism. A client can retrieve the available audio sources of a device using the GetAudioSources command of the
		/// DeviceIO service.
		/// </summary>
		/// <remarks>optional, order 2</remarks>
		[System.Xml.Serialization.XmlElementAttribute("AudioSourceToken", Namespace = "http://www.onvif.org/ver10/schema", DataType = "string")]
		public string audioSourceToken {
			get {
				return this._audioSourceToken;
			}
			set {
				this._audioSourceToken = value;
			}
		}

		/// <summary>
		/// The configuration of the audio encoder including codec, bitrate
		/// and sample rate.
		/// </summary>
		/// <remarks>optional, order 3</remarks>
		[System.Xml.Serialization.XmlElementAttribute("AudioEncoderConfiguration", Namespace = "http://www.onvif.org/ver10/schema")]
		public AudioEncoderConfiguration audioEncoderConfiguration {
			get {
				return this._audioEncoderConfiguration;
			}
			set {
				this._audioEncoderConfiguration = value;
			}
		}

		/// <summary>
		/// A pointer to a Receiver that has the necessary information to receive
		/// data from a Transmitter. This Receiver can be connected and the network video decoder displays the received data on the specified outputs. A client can retrieve the available Receivers using the
		/// GetReceivers command of the Receiver Service.
		/// </summary>
		/// <remarks>optional, order 4</remarks>
		[System.Xml.Serialization.XmlElementAttribute("ReceiverToken", Namespace = "http://www.onvif.org/ver10/schema", DataType = "string")]
		public string receiverToken {
			get {
				return this._receiverToken;
			}
			set {
				this._receiverToken = value;
			}
		}

		/// <summary>
		/// A unique identifier in the display device.
		/// </summary>
		/// <remarks>reqired, order 5</remarks>
		[System.Xml.Serialization.XmlElementAttribute("Token", Namespace = "http://www.onvif.org/ver10/schema", DataType = "string")]
		public string token {
			get {
				return this._token;
			}
			set {
				this._token = value;
			}
		}

		/// <remarks>optional, order 6, namespace ##any</remarks>
		[System.Xml.Serialization.XmlAnyElementAttribute()]
		public System.Xml.XmlElement[] any {
			get {
				return this._any;
			}
			set {
				this._any = value;
			}
		}
	}

	/// <summary>
	/// This type contains the Audio and Video coding capabilities of a display service.
	/// </summary>
	[System.SerializableAttribute()]
	[System.Xml.Serialization.XmlTypeAttribute(TypeName = "CodingCapabilities", Namespace = "http://www.onvif.org/ver10/schema")]
	public partial class CodingCapabilities {

		private System.Xml.XmlAttribute[] _anyAttr;

		private AudioEncoderConfigurationOptions _audioEncodingCapabilities;

		private AudioDecoderConfigurationOptions _audioDecodingCapabilities;

		private VideoDecoderConfigurationOptions _videoDecodingCapabilities;

		private System.Xml.XmlElement[] _any;

		[System.Xml.Serialization.XmlAnyAttributeAttribute()]
		public System.Xml.XmlAttribute[] anyAttr {
			get {
				return this._anyAttr;
			}
			set {
				this._anyAttr = value;
			}
		}

		/// <summary>
		/// If the device supports audio encoding this section describes the supported codecs and their configuration.
		/// </summary>
		/// <remarks>optional, order 0</remarks>
		[System.Xml.Serialization.XmlElementAttribute("AudioEncodingCapabilities", Namespace = "http://www.onvif.org/ver10/schema")]
		public AudioEncoderConfigurationOptions audioEncodingCapabilities {
			get {
				return this._audioEncodingCapabilities;
			}
			set {
				this._audioEncodingCapabilities = value;
			}
		}

		/// <summary>
		/// If the device supports audio decoding this section describes the supported codecs and their settings.
		/// </summary>
		/// <remarks>optional, order 1</remarks>
		[System.Xml.Serialization.XmlElementAttribute("AudioDecodingCapabilities", Namespace = "http://www.onvif.org/ver10/schema")]
		public AudioDecoderConfigurationOptions audioDecodingCapabilities {
			get {
				return this._audioDecodingCapabilities;
			}
			set {
				this._audioDecodingCapabilities = value;
			}
		}

		/// <summary>
		/// This section describes the supported video codesc and their configuration.
		/// </summary>
		/// <remarks>reqired, order 2</remarks>
		[System.Xml.Serialization.XmlElementAttribute("VideoDecodingCapabilities", Namespace = "http://www.onvif.org/ver10/schema")]
		public VideoDecoderConfigurationOptions videoDecodingCapabilities {
			get {
				return this._videoDecodingCapabilities;
			}
			set {
				this._videoDecodingCapabilities = value;
			}
		}

		/// <remarks>optional, order 3, namespace ##any</remarks>
		[System.Xml.Serialization.XmlAnyElementAttribute()]
		public System.Xml.XmlElement[] any {
			get {
				return this._any;
			}
			set {
				this._any = value;
			}
		}
	}

	/// <summary>
	/// The options supported for a display layout.
	/// </summary>
	[System.SerializableAttribute()]
	[System.Xml.Serialization.XmlTypeAttribute(TypeName = "LayoutOptions", Namespace = "http://www.onvif.org/ver10/schema")]
	public partial class LayoutOptions {

		private System.Xml.XmlAttribute[] _anyAttr;

		private PaneLayoutOptions[] _paneLayoutOptions;

		private LayoutOptionsExtension _extension;

		[System.Xml.Serialization.XmlAnyAttributeAttribute()]
		public System.Xml.XmlAttribute[] anyAttr {
			get {
				return this._anyAttr;
			}
			set {
				this._anyAttr = value;
			}
		}

		/// <summary>
		/// Lists the possible Pane Layouts of the Video Output
		/// </summary>
		/// <remarks>reqired, order 0</remarks>
		[System.Xml.Serialization.XmlElementAttribute("PaneLayoutOptions", Namespace = "http://www.onvif.org/ver10/schema")]
		public PaneLayoutOptions[] paneLayoutOptions {
			get {
				return this._paneLayoutOptions;
			}
			set {
				this._paneLayoutOptions = value;
			}
		}

		/// <remarks>optional, order 1</remarks>
		[System.Xml.Serialization.XmlElementAttribute("Extension", Namespace = "http://www.onvif.org/ver10/schema")]
		public LayoutOptionsExtension extension {
			get {
				return this._extension;
			}
			set {
				this._extension = value;
			}
		}
	}

	/// <summary>
	/// Description of a pane layout describing a complete display layout.
	/// </summary>
	[System.SerializableAttribute()]
	[System.Xml.Serialization.XmlTypeAttribute(TypeName = "PaneLayoutOptions", Namespace = "http://www.onvif.org/ver10/schema")]
	public partial class PaneLayoutOptions {

		private System.Xml.XmlAttribute[] _anyAttr;

		private Rectangle[] _area;

		private PaneOptionExtension _extension;

		[System.Xml.Serialization.XmlAnyAttributeAttribute()]
		public System.Xml.XmlAttribute[] anyAttr {
			get {
				return this._anyAttr;
			}
			set {
				this._anyAttr = value;
			}
		}

		/// <summary>
		/// List of areas assembling a layout. Coordinate values are in the range [-1.0, 1.0].
		/// </summary>
		/// <remarks>reqired, order 0</remarks>
		[System.Xml.Serialization.XmlElementAttribute("Area", Namespace = "http://www.onvif.org/ver10/schema")]
		public Rectangle[] area {
			get {
				return this._area;
			}
			set {
				this._area = value;
			}
		}

		/// <remarks>optional, order 1</remarks>
		[System.Xml.Serialization.XmlElementAttribute("Extension", Namespace = "http://www.onvif.org/ver10/schema")]
		public PaneOptionExtension extension {
			get {
				return this._extension;
			}
			set {
				this._extension = value;
			}
		}
	}

	[System.SerializableAttribute()]
	[System.Xml.Serialization.XmlTypeAttribute(TypeName = "PaneOptionExtension", Namespace = "http://www.onvif.org/ver10/schema")]
	public partial class PaneOptionExtension {

		private System.Xml.XmlElement[] _any;

		/// <remarks>optional, order 0, namespace ##any</remarks>
		[System.Xml.Serialization.XmlAnyElementAttribute()]
		public System.Xml.XmlElement[] any {
			get {
				return this._any;
			}
			set {
				this._any = value;
			}
		}
	}

	[System.SerializableAttribute()]
	[System.Xml.Serialization.XmlTypeAttribute(TypeName = "LayoutOptionsExtension", Namespace = "http://www.onvif.org/ver10/schema")]
	public partial class LayoutOptionsExtension {

		private System.Xml.XmlElement[] _any;

		/// <remarks>optional, order 0, namespace ##any</remarks>
		[System.Xml.Serialization.XmlAnyElementAttribute()]
		public System.Xml.XmlElement[] any {
			get {
				return this._any;
			}
			set {
				this._any = value;
			}
		}
	}

	/// <summary>
	/// Description of a receiver, including its token and configuration.
	/// </summary>
	[System.SerializableAttribute()]
	[System.Xml.Serialization.XmlTypeAttribute(TypeName = "Receiver", Namespace = "http://www.onvif.org/ver10/schema")]
	public partial class Receiver {

		private System.Xml.XmlAttribute[] _anyAttr;

		private string _token;

		private ReceiverConfiguration _configuration;

		private System.Xml.XmlElement[] _any;

		[System.Xml.Serialization.XmlAnyAttributeAttribute()]
		public System.Xml.XmlAttribute[] anyAttr {
			get {
				return this._anyAttr;
			}
			set {
				this._anyAttr = value;
			}
		}

		/// <summary>
		/// Unique identifier of the receiver.
		/// </summary>
		/// <remarks>reqired, order 0</remarks>
		[System.Xml.Serialization.XmlElementAttribute("Token", Namespace = "http://www.onvif.org/ver10/schema", DataType = "string")]
		public string token {
			get {
				return this._token;
			}
			set {
				this._token = value;
			}
		}

		/// <summary>
		/// Describes the configuration of the receiver.
		/// </summary>
		/// <remarks>reqired, order 1</remarks>
		[System.Xml.Serialization.XmlElementAttribute("Configuration", Namespace = "http://www.onvif.org/ver10/schema")]
		public ReceiverConfiguration configuration {
			get {
				return this._configuration;
			}
			set {
				this._configuration = value;
			}
		}

		/// <remarks>optional, order 2, namespace ##any</remarks>
		[System.Xml.Serialization.XmlAnyElementAttribute()]
		public System.Xml.XmlElement[] any {
			get {
				return this._any;
			}
			set {
				this._any = value;
			}
		}
	}

	/// <summary>
	/// Describes the configuration of a receiver.
	/// </summary>
	[System.SerializableAttribute()]
	[System.Xml.Serialization.XmlTypeAttribute(TypeName = "ReceiverConfiguration", Namespace = "http://www.onvif.org/ver10/schema")]
	public partial class ReceiverConfiguration {

		private System.Xml.XmlAttribute[] _anyAttr;

		private ReceiverMode _mode;

		private string _mediaUri;

		private StreamSetup _streamSetup;

		private System.Xml.XmlElement[] _any;

		[System.Xml.Serialization.XmlAnyAttributeAttribute()]
		public System.Xml.XmlAttribute[] anyAttr {
			get {
				return this._anyAttr;
			}
			set {
				this._anyAttr = value;
			}
		}

		/// <summary>
		/// The following connection modes are defined:
		/// </summary>
		/// <remarks>reqired, order 0</remarks>
		[System.Xml.Serialization.XmlElementAttribute("Mode", Namespace = "http://www.onvif.org/ver10/schema")]
		public ReceiverMode mode {
			get {
				return this._mode;
			}
			set {
				this._mode = value;
			}
		}

		/// <summary>
		/// Details of the URI to which the receiver should connect.
		/// </summary>
		/// <remarks>reqired, order 1</remarks>
		[System.Xml.Serialization.XmlElementAttribute("MediaUri", Namespace = "http://www.onvif.org/ver10/schema", DataType = "anyURI")]
		public string mediaUri {
			get {
				return this._mediaUri;
			}
			set {
				this._mediaUri = value;
			}
		}

		/// <summary>
		/// Stream connection parameters.
		/// </summary>
		/// <remarks>reqired, order 2</remarks>
		[System.Xml.Serialization.XmlElementAttribute("StreamSetup", Namespace = "http://www.onvif.org/ver10/schema")]
		public StreamSetup streamSetup {
			get {
				return this._streamSetup;
			}
			set {
				this._streamSetup = value;
			}
		}

		/// <remarks>optional, order 3, namespace ##any</remarks>
		[System.Xml.Serialization.XmlAnyElementAttribute()]
		public System.Xml.XmlElement[] any {
			get {
				return this._any;
			}
			set {
				this._any = value;
			}
		}
	}

	/// <summary>
	/// Specifies a receiver connection mode.
	/// </summary>
	[System.SerializableAttribute()]
	[System.Xml.Serialization.XmlTypeAttribute(TypeName = "ReceiverMode", Namespace = "http://www.onvif.org/ver10/schema")]
	public enum ReceiverMode {

		/// <summary>
		/// The receiver connects on demand, as required by consumers of the media streams.
		/// </summary>
		[System.Xml.Serialization.XmlEnumAttribute(Name = "AutoConnect")]
		autoConnect,

		/// <summary>
		/// The receiver attempts to maintain a persistent connection to the configured endpoint.
		/// </summary>
		[System.Xml.Serialization.XmlEnumAttribute(Name = "AlwaysConnect")]
		alwaysConnect,

		/// <summary>
		/// The receiver does not attempt to connect.
		/// </summary>
		[System.Xml.Serialization.XmlEnumAttribute(Name = "NeverConnect")]
		neverConnect,

		/// <summary>
		/// This case should never happen.
		/// </summary>
		[System.Xml.Serialization.XmlEnumAttribute(Name = "Unknown")]
		unknown,
	}

	/// <summary>
	/// Specifies the current connection state of the receiver.
	/// </summary>
	[System.SerializableAttribute()]
	[System.Xml.Serialization.XmlTypeAttribute(TypeName = "ReceiverState", Namespace = "http://www.onvif.org/ver10/schema")]
	public enum ReceiverState {

		/// <summary>
		/// The receiver is not connected.
		/// </summary>
		[System.Xml.Serialization.XmlEnumAttribute(Name = "NotConnected")]
		notConnected,

		/// <summary>
		/// The receiver is attempting to connect.
		/// </summary>
		[System.Xml.Serialization.XmlEnumAttribute(Name = "Connecting")]
		connecting,

		/// <summary>
		/// The receiver is connected.
		/// </summary>
		[System.Xml.Serialization.XmlEnumAttribute(Name = "Connected")]
		connected,

		/// <summary>
		/// This case should never happen.
		/// </summary>
		[System.Xml.Serialization.XmlEnumAttribute(Name = "Unknown")]
		unknown,
	}

	/// <summary>
	/// Contains information about a receiver's current state.
	/// </summary>
	[System.SerializableAttribute()]
	[System.Xml.Serialization.XmlTypeAttribute(TypeName = "ReceiverStateInformation", Namespace = "http://www.onvif.org/ver10/schema")]
	public partial class ReceiverStateInformation {

		private System.Xml.XmlAttribute[] _anyAttr;

		private ReceiverState _state;

		private bool _autoCreated;

		private System.Xml.XmlElement[] _any;

		[System.Xml.Serialization.XmlAnyAttributeAttribute()]
		public System.Xml.XmlAttribute[] anyAttr {
			get {
				return this._anyAttr;
			}
			set {
				this._anyAttr = value;
			}
		}

		/// <summary>
		/// The connection state of the receiver may have one of the following states:
		/// </summary>
		/// <remarks>reqired, order 0</remarks>
		[System.Xml.Serialization.XmlElementAttribute("State", Namespace = "http://www.onvif.org/ver10/schema")]
		public ReceiverState state {
			get {
				return this._state;
			}
			set {
				this._state = value;
			}
		}

		/// <summary>
		/// Indicates whether or not the receiver was created automatically.
		/// </summary>
		/// <remarks>reqired, order 1</remarks>
		[System.Xml.Serialization.XmlElementAttribute("AutoCreated", Namespace = "http://www.onvif.org/ver10/schema", DataType = "boolean")]
		public bool autoCreated {
			get {
				return this._autoCreated;
			}
			set {
				this._autoCreated = value;
			}
		}

		/// <remarks>optional, order 2, namespace ##any</remarks>
		[System.Xml.Serialization.XmlAnyElementAttribute()]
		public System.Xml.XmlElement[] any {
			get {
				return this._any;
			}
			set {
				this._any = value;
			}
		}
	}

	[System.SerializableAttribute()]
	[System.Xml.Serialization.XmlTypeAttribute(TypeName = "SourceReference", Namespace = "http://www.onvif.org/ver10/schema")]
	public partial class SourceReference {

		private string _type;

		private System.Xml.XmlAttribute[] _anyAttr;

		private string _token;

		private System.Xml.XmlElement[] _any;

		[System.Xml.Serialization.XmlAttributeAttribute("Type", DataType = "anyURI")]
		[System.ComponentModel.DefaultValueAttribute("http://www.onvif.org/ver10/schema/Receiver")]
		public string type {
			get {
				return this._type;
			}
			set {
				this._type = value;
			}
		}

		[System.Xml.Serialization.XmlAnyAttributeAttribute()]
		public System.Xml.XmlAttribute[] anyAttr {
			get {
				return this._anyAttr;
			}
			set {
				this._anyAttr = value;
			}
		}

		public SourceReference() {
			this._type = "http://www.onvif.org/ver10/schema/Receiver";
		}

		/// <remarks>reqired, order 0</remarks>
		[System.Xml.Serialization.XmlElementAttribute("Token", Namespace = "http://www.onvif.org/ver10/schema", DataType = "string")]
		public string token {
			get {
				return this._token;
			}
			set {
				this._token = value;
			}
		}

		/// <remarks>optional, order 1, namespace ##any</remarks>
		[System.Xml.Serialization.XmlAnyElementAttribute()]
		public System.Xml.XmlElement[] any {
			get {
				return this._any;
			}
			set {
				this._any = value;
			}
		}
	}

	[System.SerializableAttribute()]
	[System.Xml.Serialization.XmlTypeAttribute(TypeName = "RecordingSummary", Namespace = "http://www.onvif.org/ver10/schema")]
	public partial class RecordingSummary {

		private System.Xml.XmlAttribute[] _anyAttr;

		private System.DateTime _dataFrom;

		private System.DateTime _dataUntil;

		private int _numberRecordings;

		private System.Xml.XmlElement[] _any;

		[System.Xml.Serialization.XmlAnyAttributeAttribute()]
		public System.Xml.XmlAttribute[] anyAttr {
			get {
				return this._anyAttr;
			}
			set {
				this._anyAttr = value;
			}
		}

		/// <summary>
		/// The earliest point in time where there is recorded data on the device.
		/// </summary>
		/// <remarks>reqired, order 0</remarks>
		[System.Xml.Serialization.XmlElementAttribute("DataFrom", Namespace = "http://www.onvif.org/ver10/schema", DataType = "dateTime")]
		public System.DateTime dataFrom {
			get {
				return this._dataFrom;
			}
			set {
				this._dataFrom = value;
			}
		}

		/// <summary>
		/// The most recent point in time where there is recorded data on the device.
		/// </summary>
		/// <remarks>reqired, order 1</remarks>
		[System.Xml.Serialization.XmlElementAttribute("DataUntil", Namespace = "http://www.onvif.org/ver10/schema", DataType = "dateTime")]
		public System.DateTime dataUntil {
			get {
				return this._dataUntil;
			}
			set {
				this._dataUntil = value;
			}
		}

		/// <summary>
		/// The device contains this many recordings.
		/// </summary>
		/// <remarks>reqired, order 2</remarks>
		[System.Xml.Serialization.XmlElementAttribute("NumberRecordings", Namespace = "http://www.onvif.org/ver10/schema", DataType = "int")]
		public int numberRecordings {
			get {
				return this._numberRecordings;
			}
			set {
				this._numberRecordings = value;
			}
		}

		/// <remarks>optional, order 3, namespace ##any</remarks>
		[System.Xml.Serialization.XmlAnyElementAttribute()]
		public System.Xml.XmlElement[] any {
			get {
				return this._any;
			}
			set {
				this._any = value;
			}
		}
	}

	/// <summary>
	/// A structure for defining a limited scope when searching in recorded data.
	/// </summary>
	[System.SerializableAttribute()]
	[System.Xml.Serialization.XmlTypeAttribute(TypeName = "SearchScope", Namespace = "http://www.onvif.org/ver10/schema")]
	public partial class SearchScope {

		private System.Xml.XmlAttribute[] _anyAttr;

		private SourceReference[] _includedSources;

		private string[] _includedRecordings;

		private string _recordingInformationFilter;

		private SearchScopeExtension _extension;

		[System.Xml.Serialization.XmlAnyAttributeAttribute()]
		public System.Xml.XmlAttribute[] anyAttr {
			get {
				return this._anyAttr;
			}
			set {
				this._anyAttr = value;
			}
		}

		/// <summary>
		/// A list of sources that are included in the scope. If this list is included, only data from one of these sources shall be searched.
		/// </summary>
		/// <remarks>optional, order 0</remarks>
		[System.Xml.Serialization.XmlElementAttribute("IncludedSources", Namespace = "http://www.onvif.org/ver10/schema")]
		public SourceReference[] includedSources {
			get {
				return this._includedSources;
			}
			set {
				this._includedSources = value;
			}
		}

		/// <summary>
		/// A list of recordings that are included in the scope. If this list is included, only data from one of these recordings shall be searched.
		/// </summary>
		/// <remarks>optional, order 1</remarks>
		[System.Xml.Serialization.XmlElementAttribute("IncludedRecordings", Namespace = "http://www.onvif.org/ver10/schema", DataType = "string")]
		public string[] includedRecordings {
			get {
				return this._includedRecordings;
			}
			set {
				this._includedRecordings = value;
			}
		}

		/// <summary>
		/// An xpath expression used to specify what recordings to search. Only those recordings with an RecordingInformation structure that matches the filter shall be searched.
		/// </summary>
		/// <remarks>optional, order 2</remarks>
		[System.Xml.Serialization.XmlElementAttribute("RecordingInformationFilter", Namespace = "http://www.onvif.org/ver10/schema", DataType = "string")]
		public string recordingInformationFilter {
			get {
				return this._recordingInformationFilter;
			}
			set {
				this._recordingInformationFilter = value;
			}
		}

		/// <summary>
		/// Extension point
		/// </summary>
		/// <remarks>optional, order 3</remarks>
		[System.Xml.Serialization.XmlElementAttribute("Extension", Namespace = "http://www.onvif.org/ver10/schema")]
		public SearchScopeExtension extension {
			get {
				return this._extension;
			}
			set {
				this._extension = value;
			}
		}
	}

	[System.SerializableAttribute()]
	[System.Xml.Serialization.XmlTypeAttribute(TypeName = "SearchScopeExtension", Namespace = "http://www.onvif.org/ver10/schema")]
	public partial class SearchScopeExtension {

		private System.Xml.XmlElement[] _any;

		/// <remarks>optional, order 0, namespace ##any</remarks>
		[System.Xml.Serialization.XmlAnyElementAttribute()]
		public System.Xml.XmlElement[] any {
			get {
				return this._any;
			}
			set {
				this._any = value;
			}
		}
	}

	[System.SerializableAttribute()]
	[System.Xml.Serialization.XmlTypeAttribute(TypeName = "EventFilter", Namespace = "http://www.onvif.org/ver10/schema")]
	public partial class EventFilter : FilterType {
		private System.Xml.XmlAttribute[] _anyAttr;

		[System.Xml.Serialization.XmlAnyAttributeAttribute()]
		public System.Xml.XmlAttribute[] anyAttr {
			get {
				return this._anyAttr;
			}
			set {
				this._anyAttr = value;
			}
		}
	}

	[System.SerializableAttribute()]
	[System.Xml.Serialization.XmlTypeAttribute(TypeName = "PTZPositionFilter", Namespace = "http://www.onvif.org/ver10/schema")]
	public partial class PTZPositionFilter {

		private System.Xml.XmlAttribute[] _anyAttr;

		private PTZVector _minPosition;

		private PTZVector _maxPosition;

		private bool _enterOrExit;

		private System.Xml.XmlElement[] _any;

		[System.Xml.Serialization.XmlAnyAttributeAttribute()]
		public System.Xml.XmlAttribute[] anyAttr {
			get {
				return this._anyAttr;
			}
			set {
				this._anyAttr = value;
			}
		}

		/// <summary>
		/// The lower boundary of the PTZ volume to look for.
		/// </summary>
		/// <remarks>reqired, order 0</remarks>
		[System.Xml.Serialization.XmlElementAttribute("MinPosition", Namespace = "http://www.onvif.org/ver10/schema")]
		public PTZVector minPosition {
			get {
				return this._minPosition;
			}
			set {
				this._minPosition = value;
			}
		}

		/// <summary>
		/// The upper boundary of the PTZ volume to look for.
		/// </summary>
		/// <remarks>reqired, order 1</remarks>
		[System.Xml.Serialization.XmlElementAttribute("MaxPosition", Namespace = "http://www.onvif.org/ver10/schema")]
		public PTZVector maxPosition {
			get {
				return this._maxPosition;
			}
			set {
				this._maxPosition = value;
			}
		}

		/// <summary>
		/// If true, search for when entering the specified PTZ volume.
		/// </summary>
		/// <remarks>reqired, order 2</remarks>
		[System.Xml.Serialization.XmlElementAttribute("EnterOrExit", Namespace = "http://www.onvif.org/ver10/schema", DataType = "boolean")]
		public bool enterOrExit {
			get {
				return this._enterOrExit;
			}
			set {
				this._enterOrExit = value;
			}
		}

		/// <remarks>optional, order 3, namespace ##any</remarks>
		[System.Xml.Serialization.XmlAnyElementAttribute()]
		public System.Xml.XmlElement[] any {
			get {
				return this._any;
			}
			set {
				this._any = value;
			}
		}
	}

	[System.SerializableAttribute()]
	[System.Xml.Serialization.XmlTypeAttribute(TypeName = "MetadataFilter", Namespace = "http://www.onvif.org/ver10/schema")]
	public partial class MetadataFilter {

		private System.Xml.XmlAttribute[] _anyAttr;

		private string _metadataStreamFilter;

		private System.Xml.XmlElement[] _any;

		[System.Xml.Serialization.XmlAnyAttributeAttribute()]
		public System.Xml.XmlAttribute[] anyAttr {
			get {
				return this._anyAttr;
			}
			set {
				this._anyAttr = value;
			}
		}

		/// <remarks>reqired, order 0</remarks>
		[System.Xml.Serialization.XmlElementAttribute("MetadataStreamFilter", Namespace = "http://www.onvif.org/ver10/schema", DataType = "string")]
		public string metadataStreamFilter {
			get {
				return this._metadataStreamFilter;
			}
			set {
				this._metadataStreamFilter = value;
			}
		}

		/// <remarks>optional, order 1, namespace ##any</remarks>
		[System.Xml.Serialization.XmlAnyElementAttribute()]
		public System.Xml.XmlElement[] any {
			get {
				return this._any;
			}
			set {
				this._any = value;
			}
		}
	}

	[System.SerializableAttribute()]
	[System.Xml.Serialization.XmlTypeAttribute(TypeName = "FindRecordingResultList", Namespace = "http://www.onvif.org/ver10/schema")]
	public partial class FindRecordingResultList {

		private SearchState _searchState;

		private RecordingInformation[] _recordingInformation;

		/// <summary>
		/// The state of the search when the result is returned. Indicates if there can be more results, or if the search is completed.
		/// </summary>
		/// <remarks>reqired, order 0</remarks>
		[System.Xml.Serialization.XmlElementAttribute("SearchState", Namespace = "http://www.onvif.org/ver10/schema")]
		public SearchState searchState {
			get {
				return this._searchState;
			}
			set {
				this._searchState = value;
			}
		}

		/// <summary>
		/// A RecordingInformation structure for each found recording matching the search.
		/// </summary>
		/// <remarks>optional, order 1</remarks>
		[System.Xml.Serialization.XmlElementAttribute("RecordingInformation", Namespace = "http://www.onvif.org/ver10/schema")]
		public RecordingInformation[] recordingInformation {
			get {
				return this._recordingInformation;
			}
			set {
				this._recordingInformation = value;
			}
		}
	}

	[System.SerializableAttribute()]
	[System.Xml.Serialization.XmlTypeAttribute(TypeName = "SearchState", Namespace = "http://www.onvif.org/ver10/schema")]
	public enum SearchState {

		/// <summary>
		/// The search is queued and not yet started.
		/// </summary>
		[System.Xml.Serialization.XmlEnumAttribute(Name = "Queued")]
		queued,

		/// <summary>
		/// The search is underway and not yet completed.
		/// </summary>
		[System.Xml.Serialization.XmlEnumAttribute(Name = "Searching")]
		searching,

		/// <summary>
		/// The search has been completed and no new results will be found.
		/// </summary>
		[System.Xml.Serialization.XmlEnumAttribute(Name = "Completed")]
		completed,

		/// <summary>
		/// The state of the search is unknown. (This is not a valid response from GetSearchState.)
		/// </summary>
		[System.Xml.Serialization.XmlEnumAttribute(Name = "Unknown")]
		unknown,
	}

	[System.SerializableAttribute()]
	[System.Xml.Serialization.XmlTypeAttribute(TypeName = "RecordingInformation", Namespace = "http://www.onvif.org/ver10/schema")]
	public partial class RecordingInformation {

		private System.Xml.XmlAttribute[] _anyAttr;

		private string _recordingToken;

		private RecordingSourceInformation _source;

		private System.DateTime _earliestRecording;

		private bool _earliestRecordingSpecified;

		private System.DateTime _latestRecording;

		private bool _latestRecordingSpecified;

		private string _content;

		private TrackInformation[] _track;

		private RecordingStatus _recordingStatus;

		private System.Xml.XmlElement[] _any;

		[System.Xml.Serialization.XmlAnyAttributeAttribute()]
		public System.Xml.XmlAttribute[] anyAttr {
			get {
				return this._anyAttr;
			}
			set {
				this._anyAttr = value;
			}
		}

		/// <remarks>reqired, order 0</remarks>
		[System.Xml.Serialization.XmlElementAttribute("RecordingToken", Namespace = "http://www.onvif.org/ver10/schema", DataType = "string")]
		public string recordingToken {
			get {
				return this._recordingToken;
			}
			set {
				this._recordingToken = value;
			}
		}

		/// <summary>
		/// Information about the source of the recording. This gives a description of where the data in the recording comes from. Since a single
		/// recording is intended to record related material, there is just one source. It is indicates the physical location or the
		/// major data source for the recording. Currently the recordingconfiguration cannot describe each individual data source.
		/// </summary>
		/// <remarks>reqired, order 1</remarks>
		[System.Xml.Serialization.XmlElementAttribute("Source", Namespace = "http://www.onvif.org/ver10/schema")]
		public RecordingSourceInformation source {
			get {
				return this._source;
			}
			set {
				this._source = value;
			}
		}

		/// <remarks>optional, order 2</remarks>
		[System.Xml.Serialization.XmlElementAttribute("EarliestRecording", Namespace = "http://www.onvif.org/ver10/schema", DataType = "dateTime")]
		public System.DateTime earliestRecording {
			get {
				return this._earliestRecording;
			}
			set {
				this._earliestRecording = value;
			}
		}

		[System.Xml.Serialization.XmlIgnoreAttribute()]
		public bool earliestRecordingSpecified {
			get {
				return this._earliestRecordingSpecified;
			}
			set {
				this._earliestRecordingSpecified = value;
			}
		}

		/// <remarks>optional, order 3</remarks>
		[System.Xml.Serialization.XmlElementAttribute("LatestRecording", Namespace = "http://www.onvif.org/ver10/schema", DataType = "dateTime")]
		public System.DateTime latestRecording {
			get {
				return this._latestRecording;
			}
			set {
				this._latestRecording = value;
			}
		}

		[System.Xml.Serialization.XmlIgnoreAttribute()]
		public bool latestRecordingSpecified {
			get {
				return this._latestRecordingSpecified;
			}
			set {
				this._latestRecordingSpecified = value;
			}
		}

		/// <remarks>reqired, order 4</remarks>
		[System.Xml.Serialization.XmlElementAttribute("Content", Namespace = "http://www.onvif.org/ver10/schema", DataType = "string")]
		public string content {
			get {
				return this._content;
			}
			set {
				this._content = value;
			}
		}

		/// <summary>
		/// Basic information about the track. Note that a track may represent a single contiguous time span or consist of multiple slices.
		/// </summary>
		/// <remarks>optional, order 5</remarks>
		[System.Xml.Serialization.XmlElementAttribute("Track", Namespace = "http://www.onvif.org/ver10/schema")]
		public TrackInformation[] track {
			get {
				return this._track;
			}
			set {
				this._track = value;
			}
		}

		/// <remarks>reqired, order 6</remarks>
		[System.Xml.Serialization.XmlElementAttribute("RecordingStatus", Namespace = "http://www.onvif.org/ver10/schema")]
		public RecordingStatus recordingStatus {
			get {
				return this._recordingStatus;
			}
			set {
				this._recordingStatus = value;
			}
		}

		/// <remarks>optional, order 7, namespace ##any</remarks>
		[System.Xml.Serialization.XmlAnyElementAttribute()]
		public System.Xml.XmlElement[] any {
			get {
				return this._any;
			}
			set {
				this._any = value;
			}
		}
	}

	/// <summary>
	/// A set of informative desciptions of a data source. The Search searvice allows a client to filter on recordings based on information in this structure.
	/// </summary>
	[System.SerializableAttribute()]
	[System.Xml.Serialization.XmlTypeAttribute(TypeName = "RecordingSourceInformation", Namespace = "http://www.onvif.org/ver10/schema")]
	public partial class RecordingSourceInformation {

		private System.Xml.XmlAttribute[] _anyAttr;

		private string _sourceId;

		private string _name;

		private string _location;

		private string _description;

		private string _address;

		private System.Xml.XmlElement[] _any;

		[System.Xml.Serialization.XmlAnyAttributeAttribute()]
		public System.Xml.XmlAttribute[] anyAttr {
			get {
				return this._anyAttr;
			}
			set {
				this._anyAttr = value;
			}
		}

		/// <summary>
		/// Identifier for the source chosen by the client that creates the structure.
		/// This identifier is opaque to the device. Clients may use any type of URI for this field. A device shall support at least 128 characters.
		/// </summary>
		/// <remarks>reqired, order 0</remarks>
		[System.Xml.Serialization.XmlElementAttribute("SourceId", Namespace = "http://www.onvif.org/ver10/schema", DataType = "anyURI")]
		public string sourceId {
			get {
				return this._sourceId;
			}
			set {
				this._sourceId = value;
			}
		}

		/// <summary>
		/// Informative user readable name of the source, e.g. "Camera23". A device shall support at least 20 characters.
		/// </summary>
		/// <remarks>reqired, order 1</remarks>
		[System.Xml.Serialization.XmlElementAttribute("Name", Namespace = "http://www.onvif.org/ver10/schema", DataType = "string")]
		public string name {
			get {
				return this._name;
			}
			set {
				this._name = value;
			}
		}

		/// <summary>
		/// Informative description of the physical location of the source, e.g. the coordinates on a map.
		/// </summary>
		/// <remarks>reqired, order 2</remarks>
		[System.Xml.Serialization.XmlElementAttribute("Location", Namespace = "http://www.onvif.org/ver10/schema", DataType = "string")]
		public string location {
			get {
				return this._location;
			}
			set {
				this._location = value;
			}
		}

		/// <summary>
		/// Informative description of the source.
		/// </summary>
		/// <remarks>reqired, order 3</remarks>
		[System.Xml.Serialization.XmlElementAttribute("Description", Namespace = "http://www.onvif.org/ver10/schema", DataType = "string")]
		public string description {
			get {
				return this._description;
			}
			set {
				this._description = value;
			}
		}

		/// <summary>
		/// URI provided by the service supplying data to be recorded. A device shall support at least 128 characters.
		/// </summary>
		/// <remarks>reqired, order 4</remarks>
		[System.Xml.Serialization.XmlElementAttribute("Address", Namespace = "http://www.onvif.org/ver10/schema", DataType = "anyURI")]
		public string address {
			get {
				return this._address;
			}
			set {
				this._address = value;
			}
		}

		/// <remarks>optional, order 5, namespace ##any</remarks>
		[System.Xml.Serialization.XmlAnyElementAttribute()]
		public System.Xml.XmlElement[] any {
			get {
				return this._any;
			}
			set {
				this._any = value;
			}
		}
	}

	[System.SerializableAttribute()]
	[System.Xml.Serialization.XmlTypeAttribute(TypeName = "TrackInformation", Namespace = "http://www.onvif.org/ver10/schema")]
	public partial class TrackInformation {

		private System.Xml.XmlAttribute[] _anyAttr;

		private string _trackToken;

		private TrackType _trackType;

		private string _description;

		private System.DateTime _dataFrom;

		private System.DateTime _dataTo;

		private System.Xml.XmlElement[] _any;

		[System.Xml.Serialization.XmlAnyAttributeAttribute()]
		public System.Xml.XmlAttribute[] anyAttr {
			get {
				return this._anyAttr;
			}
			set {
				this._anyAttr = value;
			}
		}

		/// <remarks>reqired, order 0</remarks>
		[System.Xml.Serialization.XmlElementAttribute("TrackToken", Namespace = "http://www.onvif.org/ver10/schema", DataType = "string")]
		public string trackToken {
			get {
				return this._trackToken;
			}
			set {
				this._trackToken = value;
			}
		}

		/// <summary>
		/// Type of the track: "Video", "Audio" or "Metadata".
		/// The track shall only be able to hold data of that type.
		/// </summary>
		/// <remarks>reqired, order 1</remarks>
		[System.Xml.Serialization.XmlElementAttribute("TrackType", Namespace = "http://www.onvif.org/ver10/schema")]
		public TrackType trackType {
			get {
				return this._trackType;
			}
			set {
				this._trackType = value;
			}
		}

		/// <summary>
		/// Informative description of the contents of the track.
		/// </summary>
		/// <remarks>reqired, order 2</remarks>
		[System.Xml.Serialization.XmlElementAttribute("Description", Namespace = "http://www.onvif.org/ver10/schema", DataType = "string")]
		public string description {
			get {
				return this._description;
			}
			set {
				this._description = value;
			}
		}

		/// <summary>
		/// The start date and time of the oldest recorded data in the track.
		/// </summary>
		/// <remarks>reqired, order 3</remarks>
		[System.Xml.Serialization.XmlElementAttribute("DataFrom", Namespace = "http://www.onvif.org/ver10/schema", DataType = "dateTime")]
		public System.DateTime dataFrom {
			get {
				return this._dataFrom;
			}
			set {
				this._dataFrom = value;
			}
		}

		/// <summary>
		/// The stop date and time of the newest recorded data in the track.
		/// </summary>
		/// <remarks>reqired, order 4</remarks>
		[System.Xml.Serialization.XmlElementAttribute("DataTo", Namespace = "http://www.onvif.org/ver10/schema", DataType = "dateTime")]
		public System.DateTime dataTo {
			get {
				return this._dataTo;
			}
			set {
				this._dataTo = value;
			}
		}

		/// <remarks>optional, order 5, namespace ##any</remarks>
		[System.Xml.Serialization.XmlAnyElementAttribute()]
		public System.Xml.XmlElement[] any {
			get {
				return this._any;
			}
			set {
				this._any = value;
			}
		}
	}

	[System.SerializableAttribute()]
	[System.Xml.Serialization.XmlTypeAttribute(TypeName = "TrackType", Namespace = "http://www.onvif.org/ver10/schema")]
	public enum TrackType {

		[System.Xml.Serialization.XmlEnumAttribute(Name = "Video")]
		video,

		[System.Xml.Serialization.XmlEnumAttribute(Name = "Audio")]
		audio,

		[System.Xml.Serialization.XmlEnumAttribute(Name = "Metadata")]
		metadata,

		/// <summary>
		/// Placeholder for future extension.
		/// </summary>
		[System.Xml.Serialization.XmlEnumAttribute(Name = "Extended")]
		extended,
	}

	[System.SerializableAttribute()]
	[System.Xml.Serialization.XmlTypeAttribute(TypeName = "RecordingStatus", Namespace = "http://www.onvif.org/ver10/schema")]
	public enum RecordingStatus {

		[System.Xml.Serialization.XmlEnumAttribute(Name = "Initiated")]
		initiated,

		[System.Xml.Serialization.XmlEnumAttribute(Name = "Recording")]
		recording,

		[System.Xml.Serialization.XmlEnumAttribute(Name = "Stopped")]
		stopped,

		[System.Xml.Serialization.XmlEnumAttribute(Name = "Removing")]
		removing,

		[System.Xml.Serialization.XmlEnumAttribute(Name = "Removed")]
		removed,

		/// <summary>
		/// This case should never happen.
		/// </summary>
		[System.Xml.Serialization.XmlEnumAttribute(Name = "Unknown")]
		unknown,
	}

	[System.SerializableAttribute()]
	[System.Xml.Serialization.XmlTypeAttribute(TypeName = "FindEventResultList", Namespace = "http://www.onvif.org/ver10/schema")]
	public partial class FindEventResultList {

		private SearchState _searchState;

		private FindEventResult[] _result;

		/// <summary>
		/// The state of the search when the result is returned. Indicates if there can be more results, or if the search is completed.
		/// </summary>
		/// <remarks>reqired, order 0</remarks>
		[System.Xml.Serialization.XmlElementAttribute("SearchState", Namespace = "http://www.onvif.org/ver10/schema")]
		public SearchState searchState {
			get {
				return this._searchState;
			}
			set {
				this._searchState = value;
			}
		}

		/// <summary>
		/// A FindEventResult structure for each found event matching the search.
		/// </summary>
		/// <remarks>optional, order 1</remarks>
		[System.Xml.Serialization.XmlElementAttribute("Result", Namespace = "http://www.onvif.org/ver10/schema")]
		public FindEventResult[] result {
			get {
				return this._result;
			}
			set {
				this._result = value;
			}
		}
	}

	[System.SerializableAttribute()]
	[System.Xml.Serialization.XmlTypeAttribute(TypeName = "FindEventResult", Namespace = "http://www.onvif.org/ver10/schema")]
	public partial class FindEventResult {

		private System.Xml.XmlAttribute[] _anyAttr;

		private string _recordingToken;

		private string _trackToken;

		private System.DateTime _time;

		private NotificationMessageHolderType _event;

		private bool _startStateEvent;

		private System.Xml.XmlElement[] _any;

		[System.Xml.Serialization.XmlAnyAttributeAttribute()]
		public System.Xml.XmlAttribute[] anyAttr {
			get {
				return this._anyAttr;
			}
			set {
				this._anyAttr = value;
			}
		}

		/// <summary>
		/// The recording where this event was found. Empty string if no recording is associated with this event.
		/// </summary>
		/// <remarks>reqired, order 0</remarks>
		[System.Xml.Serialization.XmlElementAttribute("RecordingToken", Namespace = "http://www.onvif.org/ver10/schema", DataType = "string")]
		public string recordingToken {
			get {
				return this._recordingToken;
			}
			set {
				this._recordingToken = value;
			}
		}

		/// <summary>
		/// A reference to the track where this event was found. Empty string if no track is associated with this event.
		/// </summary>
		/// <remarks>reqired, order 1</remarks>
		[System.Xml.Serialization.XmlElementAttribute("TrackToken", Namespace = "http://www.onvif.org/ver10/schema", DataType = "string")]
		public string trackToken {
			get {
				return this._trackToken;
			}
			set {
				this._trackToken = value;
			}
		}

		/// <summary>
		/// The time when the event occured.
		/// </summary>
		/// <remarks>reqired, order 2</remarks>
		[System.Xml.Serialization.XmlElementAttribute("Time", Namespace = "http://www.onvif.org/ver10/schema", DataType = "dateTime")]
		public System.DateTime time {
			get {
				return this._time;
			}
			set {
				this._time = value;
			}
		}

		/// <summary>
		/// The description of the event.
		/// </summary>
		/// <remarks>reqired, order 3</remarks>
		[System.Xml.Serialization.XmlElementAttribute("Event", Namespace = "http://www.onvif.org/ver10/schema")]
		public NotificationMessageHolderType @event {
			get {
				return this._event;
			}
			set {
				this._event = value;
			}
		}

		/// <summary>
		/// If true, indicates that the event is a virtual event generated for this particular search session to give the state of a property at the start time of the search.
		/// </summary>
		/// <remarks>reqired, order 4</remarks>
		[System.Xml.Serialization.XmlElementAttribute("StartStateEvent", Namespace = "http://www.onvif.org/ver10/schema", DataType = "boolean")]
		public bool startStateEvent {
			get {
				return this._startStateEvent;
			}
			set {
				this._startStateEvent = value;
			}
		}

		/// <remarks>optional, order 5, namespace ##any</remarks>
		[System.Xml.Serialization.XmlAnyElementAttribute()]
		public System.Xml.XmlElement[] any {
			get {
				return this._any;
			}
			set {
				this._any = value;
			}
		}
	}

	[System.SerializableAttribute()]
	[System.Xml.Serialization.XmlTypeAttribute(TypeName = "FindPTZPositionResultList", Namespace = "http://www.onvif.org/ver10/schema")]
	public partial class FindPTZPositionResultList {

		private SearchState _searchState;

		private FindPTZPositionResult[] _result;

		/// <summary>
		/// The state of the search when the result is returned. Indicates if there can be more results, or if the search is completed.
		/// </summary>
		/// <remarks>reqired, order 0</remarks>
		[System.Xml.Serialization.XmlElementAttribute("SearchState", Namespace = "http://www.onvif.org/ver10/schema")]
		public SearchState searchState {
			get {
				return this._searchState;
			}
			set {
				this._searchState = value;
			}
		}

		/// <summary>
		/// A FindPTZPositionResult structure for each found PTZ position matching the search.
		/// </summary>
		/// <remarks>optional, order 1</remarks>
		[System.Xml.Serialization.XmlElementAttribute("Result", Namespace = "http://www.onvif.org/ver10/schema")]
		public FindPTZPositionResult[] result {
			get {
				return this._result;
			}
			set {
				this._result = value;
			}
		}
	}

	[System.SerializableAttribute()]
	[System.Xml.Serialization.XmlTypeAttribute(TypeName = "FindPTZPositionResult", Namespace = "http://www.onvif.org/ver10/schema")]
	public partial class FindPTZPositionResult {

		private System.Xml.XmlAttribute[] _anyAttr;

		private string _recordingToken;

		private string _trackToken;

		private System.DateTime _time;

		private PTZVector _position;

		private System.Xml.XmlElement[] _any;

		[System.Xml.Serialization.XmlAnyAttributeAttribute()]
		public System.Xml.XmlAttribute[] anyAttr {
			get {
				return this._anyAttr;
			}
			set {
				this._anyAttr = value;
			}
		}

		/// <summary>
		/// A reference to the recording containing the PTZ position.
		/// </summary>
		/// <remarks>reqired, order 0</remarks>
		[System.Xml.Serialization.XmlElementAttribute("RecordingToken", Namespace = "http://www.onvif.org/ver10/schema", DataType = "string")]
		public string recordingToken {
			get {
				return this._recordingToken;
			}
			set {
				this._recordingToken = value;
			}
		}

		/// <summary>
		/// A reference to the metadata track containing the PTZ position.
		/// </summary>
		/// <remarks>reqired, order 1</remarks>
		[System.Xml.Serialization.XmlElementAttribute("TrackToken", Namespace = "http://www.onvif.org/ver10/schema", DataType = "string")]
		public string trackToken {
			get {
				return this._trackToken;
			}
			set {
				this._trackToken = value;
			}
		}

		/// <summary>
		/// The time when the PTZ position was valid.
		/// </summary>
		/// <remarks>reqired, order 2</remarks>
		[System.Xml.Serialization.XmlElementAttribute("Time", Namespace = "http://www.onvif.org/ver10/schema", DataType = "dateTime")]
		public System.DateTime time {
			get {
				return this._time;
			}
			set {
				this._time = value;
			}
		}

		/// <summary>
		/// The PTZ position.
		/// </summary>
		/// <remarks>reqired, order 3</remarks>
		[System.Xml.Serialization.XmlElementAttribute("Position", Namespace = "http://www.onvif.org/ver10/schema")]
		public PTZVector position {
			get {
				return this._position;
			}
			set {
				this._position = value;
			}
		}

		/// <remarks>optional, order 4, namespace ##any</remarks>
		[System.Xml.Serialization.XmlAnyElementAttribute()]
		public System.Xml.XmlElement[] any {
			get {
				return this._any;
			}
			set {
				this._any = value;
			}
		}
	}

	[System.SerializableAttribute()]
	[System.Xml.Serialization.XmlTypeAttribute(TypeName = "FindMetadataResultList", Namespace = "http://www.onvif.org/ver10/schema")]
	public partial class FindMetadataResultList {

		private SearchState _searchState;

		private FindMetadataResult[] _result;

		/// <summary>
		/// The state of the search when the result is returned. Indicates if there can be more results, or if the search is completed.
		/// </summary>
		/// <remarks>reqired, order 0</remarks>
		[System.Xml.Serialization.XmlElementAttribute("SearchState", Namespace = "http://www.onvif.org/ver10/schema")]
		public SearchState searchState {
			get {
				return this._searchState;
			}
			set {
				this._searchState = value;
			}
		}

		/// <summary>
		/// A FindMetadataResult structure for each found set of Metadata matching the search.
		/// </summary>
		/// <remarks>optional, order 1</remarks>
		[System.Xml.Serialization.XmlElementAttribute("Result", Namespace = "http://www.onvif.org/ver10/schema")]
		public FindMetadataResult[] result {
			get {
				return this._result;
			}
			set {
				this._result = value;
			}
		}
	}

	[System.SerializableAttribute()]
	[System.Xml.Serialization.XmlTypeAttribute(TypeName = "FindMetadataResult", Namespace = "http://www.onvif.org/ver10/schema")]
	public partial class FindMetadataResult {

		private System.Xml.XmlAttribute[] _anyAttr;

		private string _recordingToken;

		private string _trackToken;

		private System.DateTime _time;

		private System.Xml.XmlElement[] _any;

		[System.Xml.Serialization.XmlAnyAttributeAttribute()]
		public System.Xml.XmlAttribute[] anyAttr {
			get {
				return this._anyAttr;
			}
			set {
				this._anyAttr = value;
			}
		}

		/// <summary>
		/// A reference to the recording containing the metadata.
		/// </summary>
		/// <remarks>reqired, order 0</remarks>
		[System.Xml.Serialization.XmlElementAttribute("RecordingToken", Namespace = "http://www.onvif.org/ver10/schema", DataType = "string")]
		public string recordingToken {
			get {
				return this._recordingToken;
			}
			set {
				this._recordingToken = value;
			}
		}

		/// <summary>
		/// A reference to the metadata track containing the matching metadata.
		/// </summary>
		/// <remarks>reqired, order 1</remarks>
		[System.Xml.Serialization.XmlElementAttribute("TrackToken", Namespace = "http://www.onvif.org/ver10/schema", DataType = "string")]
		public string trackToken {
			get {
				return this._trackToken;
			}
			set {
				this._trackToken = value;
			}
		}

		/// <summary>
		/// The point in time when the matching metadata occurs in the metadata track.
		/// </summary>
		/// <remarks>reqired, order 2</remarks>
		[System.Xml.Serialization.XmlElementAttribute("Time", Namespace = "http://www.onvif.org/ver10/schema", DataType = "dateTime")]
		public System.DateTime time {
			get {
				return this._time;
			}
			set {
				this._time = value;
			}
		}

		/// <remarks>optional, order 3, namespace ##any</remarks>
		[System.Xml.Serialization.XmlAnyElementAttribute()]
		public System.Xml.XmlElement[] any {
			get {
				return this._any;
			}
			set {
				this._any = value;
			}
		}
	}

	/// <summary>
	/// A set of media attributes valid for a recording at a point in time or for a time interval.
	/// </summary>
	[System.SerializableAttribute()]
	[System.Xml.Serialization.XmlTypeAttribute(TypeName = "MediaAttributes", Namespace = "http://www.onvif.org/ver10/schema")]
	public partial class MediaAttributes {

		private System.Xml.XmlAttribute[] _anyAttr;

		private string _recordingToken;

		private TrackAttributes[] _trackAttributes;

		private System.DateTime _from;

		private System.DateTime _until;

		private System.Xml.XmlElement[] _any;

		[System.Xml.Serialization.XmlAnyAttributeAttribute()]
		public System.Xml.XmlAttribute[] anyAttr {
			get {
				return this._anyAttr;
			}
			set {
				this._anyAttr = value;
			}
		}

		/// <summary>
		/// A reference to the recording that has these attributes.
		/// </summary>
		/// <remarks>reqired, order 0</remarks>
		[System.Xml.Serialization.XmlElementAttribute("RecordingToken", Namespace = "http://www.onvif.org/ver10/schema", DataType = "string")]
		public string recordingToken {
			get {
				return this._recordingToken;
			}
			set {
				this._recordingToken = value;
			}
		}

		/// <summary>
		/// A set of attributes for each track.
		/// </summary>
		/// <remarks>optional, order 1</remarks>
		[System.Xml.Serialization.XmlElementAttribute("TrackAttributes", Namespace = "http://www.onvif.org/ver10/schema")]
		public TrackAttributes[] trackAttributes {
			get {
				return this._trackAttributes;
			}
			set {
				this._trackAttributes = value;
			}
		}

		/// <summary>
		/// The attributes are valid from this point in time in the recording.
		/// </summary>
		/// <remarks>reqired, order 2</remarks>
		[System.Xml.Serialization.XmlElementAttribute("From", Namespace = "http://www.onvif.org/ver10/schema", DataType = "dateTime")]
		public System.DateTime from {
			get {
				return this._from;
			}
			set {
				this._from = value;
			}
		}

		/// <summary>
		/// The attributes are valid until this point in time in the recording. Can be equal to 'From' to indicate that the attributes are only known to be valid for this particular point in time.
		/// </summary>
		/// <remarks>reqired, order 3</remarks>
		[System.Xml.Serialization.XmlElementAttribute("Until", Namespace = "http://www.onvif.org/ver10/schema", DataType = "dateTime")]
		public System.DateTime until {
			get {
				return this._until;
			}
			set {
				this._until = value;
			}
		}

		/// <remarks>optional, order 4, namespace ##any</remarks>
		[System.Xml.Serialization.XmlAnyElementAttribute()]
		public System.Xml.XmlElement[] any {
			get {
				return this._any;
			}
			set {
				this._any = value;
			}
		}
	}

	[System.SerializableAttribute()]
	[System.Xml.Serialization.XmlTypeAttribute(TypeName = "TrackAttributes", Namespace = "http://www.onvif.org/ver10/schema")]
	public partial class TrackAttributes {

		private System.Xml.XmlAttribute[] _anyAttr;

		private TrackInformation _trackInformation;

		private VideoAttributes _videoAttributes;

		private AudioAttributes _audioAttributes;

		private MetadataAttributes _metadataAttributes;

		private TrackAttributesExtension _extension;

		[System.Xml.Serialization.XmlAnyAttributeAttribute()]
		public System.Xml.XmlAttribute[] anyAttr {
			get {
				return this._anyAttr;
			}
			set {
				this._anyAttr = value;
			}
		}

		/// <summary>
		/// The basic information about the track. Note that a track may represent a single contiguous time span or consist of multiple slices.
		/// </summary>
		/// <remarks>reqired, order 0</remarks>
		[System.Xml.Serialization.XmlElementAttribute("TrackInformation", Namespace = "http://www.onvif.org/ver10/schema")]
		public TrackInformation trackInformation {
			get {
				return this._trackInformation;
			}
			set {
				this._trackInformation = value;
			}
		}

		/// <summary>
		/// If the track is a video track, exactly one of this structure shall be present and contain the video attributes.
		/// </summary>
		/// <remarks>optional, order 1</remarks>
		[System.Xml.Serialization.XmlElementAttribute("VideoAttributes", Namespace = "http://www.onvif.org/ver10/schema")]
		public VideoAttributes videoAttributes {
			get {
				return this._videoAttributes;
			}
			set {
				this._videoAttributes = value;
			}
		}

		/// <summary>
		/// If the track is an audio track, exactly one of this structure shall be present and contain the audio attributes.
		/// </summary>
		/// <remarks>optional, order 2</remarks>
		[System.Xml.Serialization.XmlElementAttribute("AudioAttributes", Namespace = "http://www.onvif.org/ver10/schema")]
		public AudioAttributes audioAttributes {
			get {
				return this._audioAttributes;
			}
			set {
				this._audioAttributes = value;
			}
		}

		/// <summary>
		/// If the track is an metadata track, exactly one of this structure shall be present and contain the metadata attributes.
		/// </summary>
		/// <remarks>optional, order 3</remarks>
		[System.Xml.Serialization.XmlElementAttribute("MetadataAttributes", Namespace = "http://www.onvif.org/ver10/schema")]
		public MetadataAttributes metadataAttributes {
			get {
				return this._metadataAttributes;
			}
			set {
				this._metadataAttributes = value;
			}
		}

		/// <summary>
		/// </summary>
		/// <remarks>optional, order 4</remarks>
		[System.Xml.Serialization.XmlElementAttribute("Extension", Namespace = "http://www.onvif.org/ver10/schema")]
		public TrackAttributesExtension extension {
			get {
				return this._extension;
			}
			set {
				this._extension = value;
			}
		}
	}

	[System.SerializableAttribute()]
	[System.Xml.Serialization.XmlTypeAttribute(TypeName = "VideoAttributes", Namespace = "http://www.onvif.org/ver10/schema")]
	public partial class VideoAttributes {

		private System.Xml.XmlAttribute[] _anyAttr;

		private int _bitrate;

		private bool _bitrateSpecified;

		private int _width;

		private int _height;

		private VideoEncoding _encoding;

		private float _framerate;

		private System.Xml.XmlElement[] _any;

		[System.Xml.Serialization.XmlAnyAttributeAttribute()]
		public System.Xml.XmlAttribute[] anyAttr {
			get {
				return this._anyAttr;
			}
			set {
				this._anyAttr = value;
			}
		}

		/// <summary>
		/// Average bitrate in kbps.
		/// </summary>
		/// <remarks>optional, order 0</remarks>
		[System.Xml.Serialization.XmlElementAttribute("Bitrate", Namespace = "http://www.onvif.org/ver10/schema", DataType = "int")]
		public int bitrate {
			get {
				return this._bitrate;
			}
			set {
				this._bitrate = value;
			}
		}

		[System.Xml.Serialization.XmlIgnoreAttribute()]
		public bool bitrateSpecified {
			get {
				return this._bitrateSpecified;
			}
			set {
				this._bitrateSpecified = value;
			}
		}

		/// <summary>
		/// The width of the video in pixels.
		/// </summary>
		/// <remarks>reqired, order 1</remarks>
		[System.Xml.Serialization.XmlElementAttribute("Width", Namespace = "http://www.onvif.org/ver10/schema", DataType = "int")]
		public int width {
			get {
				return this._width;
			}
			set {
				this._width = value;
			}
		}

		/// <summary>
		/// The height of the video in pixels.
		/// </summary>
		/// <remarks>reqired, order 2</remarks>
		[System.Xml.Serialization.XmlElementAttribute("Height", Namespace = "http://www.onvif.org/ver10/schema", DataType = "int")]
		public int height {
			get {
				return this._height;
			}
			set {
				this._height = value;
			}
		}

		/// <summary>
		/// Used video codec, either Jpeg, H.264 or Mpeg4
		/// </summary>
		/// <remarks>reqired, order 3</remarks>
		[System.Xml.Serialization.XmlElementAttribute("Encoding", Namespace = "http://www.onvif.org/ver10/schema")]
		public VideoEncoding encoding {
			get {
				return this._encoding;
			}
			set {
				this._encoding = value;
			}
		}

		/// <summary>
		/// Average framerate in frames per second.
		/// </summary>
		/// <remarks>reqired, order 4</remarks>
		[System.Xml.Serialization.XmlElementAttribute("Framerate", Namespace = "http://www.onvif.org/ver10/schema", DataType = "float")]
		public float framerate {
			get {
				return this._framerate;
			}
			set {
				this._framerate = value;
			}
		}

		/// <remarks>optional, order 5, namespace ##any</remarks>
		[System.Xml.Serialization.XmlAnyElementAttribute()]
		public System.Xml.XmlElement[] any {
			get {
				return this._any;
			}
			set {
				this._any = value;
			}
		}
	}

	[System.SerializableAttribute()]
	[System.Xml.Serialization.XmlTypeAttribute(TypeName = "AudioAttributes", Namespace = "http://www.onvif.org/ver10/schema")]
	public partial class AudioAttributes {

		private System.Xml.XmlAttribute[] _anyAttr;

		private int _bitrate;

		private bool _bitrateSpecified;

		private AudioEncoding _encoding;

		private int _samplerate;

		private System.Xml.XmlElement[] _any;

		[System.Xml.Serialization.XmlAnyAttributeAttribute()]
		public System.Xml.XmlAttribute[] anyAttr {
			get {
				return this._anyAttr;
			}
			set {
				this._anyAttr = value;
			}
		}

		/// <summary>
		/// The bitrate in kbps.
		/// </summary>
		/// <remarks>optional, order 0</remarks>
		[System.Xml.Serialization.XmlElementAttribute("Bitrate", Namespace = "http://www.onvif.org/ver10/schema", DataType = "int")]
		public int bitrate {
			get {
				return this._bitrate;
			}
			set {
				this._bitrate = value;
			}
		}

		[System.Xml.Serialization.XmlIgnoreAttribute()]
		public bool bitrateSpecified {
			get {
				return this._bitrateSpecified;
			}
			set {
				this._bitrateSpecified = value;
			}
		}

		/// <summary>
		/// Audio codec used for encoding the audio (either G.711, G.726 or AAC)
		/// </summary>
		/// <remarks>reqired, order 1</remarks>
		[System.Xml.Serialization.XmlElementAttribute("Encoding", Namespace = "http://www.onvif.org/ver10/schema")]
		public AudioEncoding encoding {
			get {
				return this._encoding;
			}
			set {
				this._encoding = value;
			}
		}

		/// <summary>
		/// The sample rate in kHz.
		/// </summary>
		/// <remarks>reqired, order 2</remarks>
		[System.Xml.Serialization.XmlElementAttribute("Samplerate", Namespace = "http://www.onvif.org/ver10/schema", DataType = "int")]
		public int samplerate {
			get {
				return this._samplerate;
			}
			set {
				this._samplerate = value;
			}
		}

		/// <remarks>optional, order 3, namespace ##any</remarks>
		[System.Xml.Serialization.XmlAnyElementAttribute()]
		public System.Xml.XmlElement[] any {
			get {
				return this._any;
			}
			set {
				this._any = value;
			}
		}
	}

	[System.SerializableAttribute()]
	[System.Xml.Serialization.XmlTypeAttribute(TypeName = "MetadataAttributes", Namespace = "http://www.onvif.org/ver10/schema")]
	public partial class MetadataAttributes {

		private System.Xml.XmlAttribute[] _anyAttr;

		private bool _canContainPTZ;

		private bool _canContainAnalytics;

		private bool _canContainNotifications;

		private System.Xml.XmlElement[] _any;

		[System.Xml.Serialization.XmlAnyAttributeAttribute()]
		public System.Xml.XmlAttribute[] anyAttr {
			get {
				return this._anyAttr;
			}
			set {
				this._anyAttr = value;
			}
		}

		/// <summary>
		/// Indicates that there can be PTZ data in the metadata track in the specified time interval.
		/// </summary>
		/// <remarks>reqired, order 0</remarks>
		[System.Xml.Serialization.XmlElementAttribute("CanContainPTZ", Namespace = "http://www.onvif.org/ver10/schema", DataType = "boolean")]
		public bool canContainPTZ {
			get {
				return this._canContainPTZ;
			}
			set {
				this._canContainPTZ = value;
			}
		}

		/// <summary>
		/// Indicates that there can be analytics data in the metadata track in the specified time interval.
		/// </summary>
		/// <remarks>reqired, order 1</remarks>
		[System.Xml.Serialization.XmlElementAttribute("CanContainAnalytics", Namespace = "http://www.onvif.org/ver10/schema", DataType = "boolean")]
		public bool canContainAnalytics {
			get {
				return this._canContainAnalytics;
			}
			set {
				this._canContainAnalytics = value;
			}
		}

		/// <summary>
		/// Indicates that there can be notifications in the metadata track in the specified time interval.
		/// </summary>
		/// <remarks>reqired, order 2</remarks>
		[System.Xml.Serialization.XmlElementAttribute("CanContainNotifications", Namespace = "http://www.onvif.org/ver10/schema", DataType = "boolean")]
		public bool canContainNotifications {
			get {
				return this._canContainNotifications;
			}
			set {
				this._canContainNotifications = value;
			}
		}

		/// <remarks>optional, order 3, namespace ##any</remarks>
		[System.Xml.Serialization.XmlAnyElementAttribute()]
		public System.Xml.XmlElement[] any {
			get {
				return this._any;
			}
			set {
				this._any = value;
			}
		}
	}

	[System.SerializableAttribute()]
	[System.Xml.Serialization.XmlTypeAttribute(TypeName = "TrackAttributesExtension", Namespace = "http://www.onvif.org/ver10/schema")]
	public partial class TrackAttributesExtension {

		private System.Xml.XmlElement[] _any;

		/// <remarks>optional, order 0, namespace ##any</remarks>
		[System.Xml.Serialization.XmlAnyElementAttribute()]
		public System.Xml.XmlElement[] any {
			get {
				return this._any;
			}
			set {
				this._any = value;
			}
		}
	}

	[System.SerializableAttribute()]
	[System.Xml.Serialization.XmlTypeAttribute(TypeName = "RecordingConfiguration", Namespace = "http://www.onvif.org/ver10/schema")]
	public partial class RecordingConfiguration {

		private System.Xml.XmlAttribute[] _anyAttr;

		private RecordingSourceInformation _source;

		private string _content;

		private XsDuration _maximumRetentionTime;

		private System.Xml.XmlElement[] _any;

		[System.Xml.Serialization.XmlAnyAttributeAttribute()]
		public System.Xml.XmlAttribute[] anyAttr {
			get {
				return this._anyAttr;
			}
			set {
				this._anyAttr = value;
			}
		}

		/// <summary>
		/// Information about the source of the recording.
		/// </summary>
		/// <remarks>reqired, order 0</remarks>
		[System.Xml.Serialization.XmlElementAttribute("Source", Namespace = "http://www.onvif.org/ver10/schema")]
		public RecordingSourceInformation source {
			get {
				return this._source;
			}
			set {
				this._source = value;
			}
		}

		/// <summary>
		/// Informative description of the source.
		/// </summary>
		/// <remarks>reqired, order 1</remarks>
		[System.Xml.Serialization.XmlElementAttribute("Content", Namespace = "http://www.onvif.org/ver10/schema", DataType = "string")]
		public string content {
			get {
				return this._content;
			}
			set {
				this._content = value;
			}
		}

		/// <summary>
		/// Sspecifies the maximum time that data in any track within the
		/// recording shall be stored. The device shall delete any data older than the maximum retention
		/// time. Such data shall not be accessible anymore. If the MaximumRetentionPeriod is set to 0,
		/// the device shall not limit the retention time of stored data, except by resource constraints.
		/// Whatever the value of MaximumRetentionTime, the device may automatically delete
		/// recordings to free up storage space for new recordings.
		/// </summary>
		/// <remarks>reqired, order 2</remarks>
		[System.Xml.Serialization.XmlElementAttribute("MaximumRetentionTime", Namespace = "http://www.onvif.org/ver10/schema")]
		public XsDuration maximumRetentionTime {
			get {
				return this._maximumRetentionTime;
			}
			set {
				this._maximumRetentionTime = value;
			}
		}

		/// <remarks>optional, order 3, namespace ##any</remarks>
		[System.Xml.Serialization.XmlAnyElementAttribute()]
		public System.Xml.XmlElement[] any {
			get {
				return this._any;
			}
			set {
				this._any = value;
			}
		}
	}

	[System.SerializableAttribute()]
	[System.Xml.Serialization.XmlTypeAttribute(TypeName = "TrackConfiguration", Namespace = "http://www.onvif.org/ver10/schema")]
	public partial class TrackConfiguration {

		private System.Xml.XmlAttribute[] _anyAttr;

		private TrackType _trackType;

		private string _description;

		private System.Xml.XmlElement[] _any;

		[System.Xml.Serialization.XmlAnyAttributeAttribute()]
		public System.Xml.XmlAttribute[] anyAttr {
			get {
				return this._anyAttr;
			}
			set {
				this._anyAttr = value;
			}
		}

		/// <summary>
		/// Type of the track. It shall be equal to the strings "Video",
		/// "Audio" or "Metadata". The track shall only be able to hold data of that type.
		/// </summary>
		/// <remarks>reqired, order 0</remarks>
		[System.Xml.Serialization.XmlElementAttribute("TrackType", Namespace = "http://www.onvif.org/ver10/schema")]
		public TrackType trackType {
			get {
				return this._trackType;
			}
			set {
				this._trackType = value;
			}
		}

		/// <summary>
		/// Informative description of the track.
		/// </summary>
		/// <remarks>reqired, order 1</remarks>
		[System.Xml.Serialization.XmlElementAttribute("Description", Namespace = "http://www.onvif.org/ver10/schema", DataType = "string")]
		public string description {
			get {
				return this._description;
			}
			set {
				this._description = value;
			}
		}

		/// <remarks>optional, order 2, namespace ##any</remarks>
		[System.Xml.Serialization.XmlAnyElementAttribute()]
		public System.Xml.XmlElement[] any {
			get {
				return this._any;
			}
			set {
				this._any = value;
			}
		}
	}

	[System.SerializableAttribute()]
	[System.Xml.Serialization.XmlTypeAttribute(TypeName = "GetRecordingsResponseItem", Namespace = "http://www.onvif.org/ver10/schema")]
	public partial class GetRecordingsResponseItem {

		private System.Xml.XmlAttribute[] _anyAttr;

		private string _recordingToken;

		private RecordingConfiguration _configuration;

		private GetTracksResponseList _tracks;

		private System.Xml.XmlElement[] _any;

		[System.Xml.Serialization.XmlAnyAttributeAttribute()]
		public System.Xml.XmlAttribute[] anyAttr {
			get {
				return this._anyAttr;
			}
			set {
				this._anyAttr = value;
			}
		}

		/// <summary>
		/// Token of the recording.
		/// </summary>
		/// <remarks>reqired, order 0</remarks>
		[System.Xml.Serialization.XmlElementAttribute("RecordingToken", Namespace = "http://www.onvif.org/ver10/schema", DataType = "string")]
		public string recordingToken {
			get {
				return this._recordingToken;
			}
			set {
				this._recordingToken = value;
			}
		}

		/// <summary>
		/// Configuration of the recording.
		/// </summary>
		/// <remarks>reqired, order 1</remarks>
		[System.Xml.Serialization.XmlElementAttribute("Configuration", Namespace = "http://www.onvif.org/ver10/schema")]
		public RecordingConfiguration configuration {
			get {
				return this._configuration;
			}
			set {
				this._configuration = value;
			}
		}

		/// <summary>
		/// List of tracks.
		/// </summary>
		/// <remarks>reqired, order 2</remarks>
		[System.Xml.Serialization.XmlElementAttribute("Tracks", Namespace = "http://www.onvif.org/ver10/schema")]
		public GetTracksResponseList tracks {
			get {
				return this._tracks;
			}
			set {
				this._tracks = value;
			}
		}

		/// <remarks>optional, order 3, namespace ##any</remarks>
		[System.Xml.Serialization.XmlAnyElementAttribute()]
		public System.Xml.XmlElement[] any {
			get {
				return this._any;
			}
			set {
				this._any = value;
			}
		}
	}

	[System.SerializableAttribute()]
	[System.Xml.Serialization.XmlTypeAttribute(TypeName = "GetTracksResponseList", Namespace = "http://www.onvif.org/ver10/schema")]
	public partial class GetTracksResponseList {

		private System.Xml.XmlAttribute[] _anyAttr;

		private GetTracksResponseItem[] _track;

		[System.Xml.Serialization.XmlAnyAttributeAttribute()]
		public System.Xml.XmlAttribute[] anyAttr {
			get {
				return this._anyAttr;
			}
			set {
				this._anyAttr = value;
			}
		}

		/// <summary>
		/// Configuration of a track.
		/// </summary>
		/// <remarks>optional, order 0</remarks>
		[System.Xml.Serialization.XmlElementAttribute("Track", Namespace = "http://www.onvif.org/ver10/schema")]
		public GetTracksResponseItem[] track {
			get {
				return this._track;
			}
			set {
				this._track = value;
			}
		}
	}

	[System.SerializableAttribute()]
	[System.Xml.Serialization.XmlTypeAttribute(TypeName = "GetTracksResponseItem", Namespace = "http://www.onvif.org/ver10/schema")]
	public partial class GetTracksResponseItem {

		private System.Xml.XmlAttribute[] _anyAttr;

		private string _trackToken;

		private TrackConfiguration _configuration;

		private System.Xml.XmlElement[] _any;

		[System.Xml.Serialization.XmlAnyAttributeAttribute()]
		public System.Xml.XmlAttribute[] anyAttr {
			get {
				return this._anyAttr;
			}
			set {
				this._anyAttr = value;
			}
		}

		/// <summary>
		/// Token of the track.
		/// </summary>
		/// <remarks>reqired, order 0</remarks>
		[System.Xml.Serialization.XmlElementAttribute("TrackToken", Namespace = "http://www.onvif.org/ver10/schema", DataType = "string")]
		public string trackToken {
			get {
				return this._trackToken;
			}
			set {
				this._trackToken = value;
			}
		}

		/// <summary>
		/// Configuration of the track.
		/// </summary>
		/// <remarks>reqired, order 1</remarks>
		[System.Xml.Serialization.XmlElementAttribute("Configuration", Namespace = "http://www.onvif.org/ver10/schema")]
		public TrackConfiguration configuration {
			get {
				return this._configuration;
			}
			set {
				this._configuration = value;
			}
		}

		/// <remarks>optional, order 2, namespace ##any</remarks>
		[System.Xml.Serialization.XmlAnyElementAttribute()]
		public System.Xml.XmlElement[] any {
			get {
				return this._any;
			}
			set {
				this._any = value;
			}
		}
	}

	[System.SerializableAttribute()]
	[System.Xml.Serialization.XmlTypeAttribute(TypeName = "RecordingJobConfiguration", Namespace = "http://www.onvif.org/ver10/schema")]
	public partial class RecordingJobConfiguration {

		private System.Xml.XmlAttribute[] _anyAttr;

		private string _recordingToken;

		private string _mode;

		private int _priority;

		private RecordingJobSource[] _source;

		private RecordingJobConfigurationExtension _extension;

		[System.Xml.Serialization.XmlAnyAttributeAttribute()]
		public System.Xml.XmlAttribute[] anyAttr {
			get {
				return this._anyAttr;
			}
			set {
				this._anyAttr = value;
			}
		}

		/// <summary>
		/// Identifies the recording to which this job shall store the received data.
		/// </summary>
		/// <remarks>reqired, order 0</remarks>
		[System.Xml.Serialization.XmlElementAttribute("RecordingToken", Namespace = "http://www.onvif.org/ver10/schema", DataType = "string")]
		public string recordingToken {
			get {
				return this._recordingToken;
			}
			set {
				this._recordingToken = value;
			}
		}

		/// <summary>
		/// The mode of the job. If it is idle, nothing shall happen. If it is active, the device shall try
		/// to obtain data from the receivers. A client shall use GetRecordingJobState to determine if data transfer is really taking place.
		/// The only valid values for Mode shall be "Idle" and "Active".
		/// </summary>
		/// <remarks>reqired, order 1</remarks>
		[System.Xml.Serialization.XmlElementAttribute("Mode", Namespace = "http://www.onvif.org/ver10/schema", DataType = "string")]
		public string mode {
			get {
				return this._mode;
			}
			set {
				this._mode = value;
			}
		}

		/// <summary>
		/// This shall be a positive number. If there are multiple recording jobs that store data to
		/// the same track, the device will only store the data for the recording job with the highest
		/// priority. The priority is specified per recording job, but the device shall determine the priority
		/// of each track individually. If there are two recording jobs with the same priority, the device
		/// shall record the data corresponding to the recording job that was activated the latest.
		/// </summary>
		/// <remarks>reqired, order 2</remarks>
		[System.Xml.Serialization.XmlElementAttribute("Priority", Namespace = "http://www.onvif.org/ver10/schema", DataType = "int")]
		public int priority {
			get {
				return this._priority;
			}
			set {
				this._priority = value;
			}
		}

		/// <summary>
		/// Source of the recording.
		/// </summary>
		/// <remarks>optional, order 3</remarks>
		[System.Xml.Serialization.XmlElementAttribute("Source", Namespace = "http://www.onvif.org/ver10/schema")]
		public RecordingJobSource[] source {
			get {
				return this._source;
			}
			set {
				this._source = value;
			}
		}

		/// <remarks>optional, order 4</remarks>
		[System.Xml.Serialization.XmlElementAttribute("Extension", Namespace = "http://www.onvif.org/ver10/schema")]
		public RecordingJobConfigurationExtension extension {
			get {
				return this._extension;
			}
			set {
				this._extension = value;
			}
		}
	}

	[System.SerializableAttribute()]
	[System.Xml.Serialization.XmlTypeAttribute(TypeName = "RecordingJobSource", Namespace = "http://www.onvif.org/ver10/schema")]
	public partial class RecordingJobSource {

		private System.Xml.XmlAttribute[] _anyAttr;

		private SourceReference _sourceToken;

		private bool _autoCreateReceiver;

		private bool _autoCreateReceiverSpecified;

		private RecordingJobTrack[] _tracks;

		private RecordingJobSourceExtension _extension;

		[System.Xml.Serialization.XmlAnyAttributeAttribute()]
		public System.Xml.XmlAttribute[] anyAttr {
			get {
				return this._anyAttr;
			}
			set {
				this._anyAttr = value;
			}
		}

		/// <summary>
		/// This field shall be a reference to the source of the data. The type of the source
		/// is determined by the attribute Type in the SourceToken structure. If Type is
		/// http://www.onvif.org/ver10/schema/Receiver, the token is a ReceiverReference. In this case
		/// the device shall receive the data over the network. If Type is
		/// http://www.onvif.org/ver10/schema/Profile, the token identifies a media profile, instructing the
		/// device to obtain data from a profile that exists on the local device.
		/// </summary>
		/// <remarks>optional, order 0</remarks>
		[System.Xml.Serialization.XmlElementAttribute("SourceToken", Namespace = "http://www.onvif.org/ver10/schema")]
		public SourceReference sourceToken {
			get {
				return this._sourceToken;
			}
			set {
				this._sourceToken = value;
			}
		}

		/// <summary>
		/// If this field is TRUE, and if the SourceToken is omitted, the device
		/// shall create a receiver object (through the receiver service) and assign the
		/// ReceiverReference to the SourceToken field. When retrieving the RecordingJobConfiguration
		/// from the device, the AutoCreateReceiver field shall never be present.
		/// </summary>
		/// <remarks>optional, order 1</remarks>
		[System.Xml.Serialization.XmlElementAttribute("AutoCreateReceiver", Namespace = "http://www.onvif.org/ver10/schema", DataType = "boolean")]
		public bool autoCreateReceiver {
			get {
				return this._autoCreateReceiver;
			}
			set {
				this._autoCreateReceiver = value;
			}
		}

		[System.Xml.Serialization.XmlIgnoreAttribute()]
		public bool autoCreateReceiverSpecified {
			get {
				return this._autoCreateReceiverSpecified;
			}
			set {
				this._autoCreateReceiverSpecified = value;
			}
		}

		/// <summary>
		/// List of tracks associated with the recording.
		/// </summary>
		/// <remarks>optional, order 2</remarks>
		[System.Xml.Serialization.XmlElementAttribute("Tracks", Namespace = "http://www.onvif.org/ver10/schema")]
		public RecordingJobTrack[] tracks {
			get {
				return this._tracks;
			}
			set {
				this._tracks = value;
			}
		}

		/// <remarks>optional, order 3</remarks>
		[System.Xml.Serialization.XmlElementAttribute("Extension", Namespace = "http://www.onvif.org/ver10/schema")]
		public RecordingJobSourceExtension extension {
			get {
				return this._extension;
			}
			set {
				this._extension = value;
			}
		}
	}

	[System.SerializableAttribute()]
	[System.Xml.Serialization.XmlTypeAttribute(TypeName = "RecordingJobTrack", Namespace = "http://www.onvif.org/ver10/schema")]
	public partial class RecordingJobTrack {

		private System.Xml.XmlAttribute[] _anyAttr;

		private string _sourceTag;

		private string _destination;

		private System.Xml.XmlElement[] _any;

		[System.Xml.Serialization.XmlAnyAttributeAttribute()]
		public System.Xml.XmlAttribute[] anyAttr {
			get {
				return this._anyAttr;
			}
			set {
				this._anyAttr = value;
			}
		}

		/// <summary>
		/// If the received RTSP stream contains multiple tracks of the same type, the
		/// SourceTag differentiates between those Tracks. This field can be ignored in case of recording a local source.
		/// </summary>
		/// <remarks>reqired, order 0</remarks>
		[System.Xml.Serialization.XmlElementAttribute("SourceTag", Namespace = "http://www.onvif.org/ver10/schema", DataType = "string")]
		public string sourceTag {
			get {
				return this._sourceTag;
			}
			set {
				this._sourceTag = value;
			}
		}

		/// <summary>
		/// The destination is the tracktoken of the track to which the device shall store the
		/// received data.
		/// </summary>
		/// <remarks>reqired, order 1</remarks>
		[System.Xml.Serialization.XmlElementAttribute("Destination", Namespace = "http://www.onvif.org/ver10/schema", DataType = "string")]
		public string destination {
			get {
				return this._destination;
			}
			set {
				this._destination = value;
			}
		}

		/// <remarks>optional, order 2, namespace ##any</remarks>
		[System.Xml.Serialization.XmlAnyElementAttribute()]
		public System.Xml.XmlElement[] any {
			get {
				return this._any;
			}
			set {
				this._any = value;
			}
		}
	}

	[System.SerializableAttribute()]
	[System.Xml.Serialization.XmlTypeAttribute(TypeName = "RecordingJobSourceExtension", Namespace = "http://www.onvif.org/ver10/schema")]
	public partial class RecordingJobSourceExtension {

		private System.Xml.XmlElement[] _any;

		/// <remarks>optional, order 0, namespace ##any</remarks>
		[System.Xml.Serialization.XmlAnyElementAttribute()]
		public System.Xml.XmlElement[] any {
			get {
				return this._any;
			}
			set {
				this._any = value;
			}
		}
	}

	[System.SerializableAttribute()]
	[System.Xml.Serialization.XmlTypeAttribute(TypeName = "RecordingJobConfigurationExtension", Namespace = "http://www.onvif.org/ver10/schema")]
	public partial class RecordingJobConfigurationExtension {

		private System.Xml.XmlElement[] _any;

		/// <remarks>optional, order 0, namespace ##any</remarks>
		[System.Xml.Serialization.XmlAnyElementAttribute()]
		public System.Xml.XmlElement[] any {
			get {
				return this._any;
			}
			set {
				this._any = value;
			}
		}
	}

	[System.SerializableAttribute()]
	[System.Xml.Serialization.XmlTypeAttribute(TypeName = "RecordingJobStateInformation", Namespace = "http://www.onvif.org/ver10/schema")]
	public partial class RecordingJobStateInformation {

		private System.Xml.XmlAttribute[] _anyAttr;

		private string _recordingToken;

		private string _state;

		private RecordingJobStateSource[] _sources;

		private RecordingJobStateInformationExtension _extension;

		[System.Xml.Serialization.XmlAnyAttributeAttribute()]
		public System.Xml.XmlAttribute[] anyAttr {
			get {
				return this._anyAttr;
			}
			set {
				this._anyAttr = value;
			}
		}

		/// <summary>
		/// Identification of the recording that the recording job records to.
		/// </summary>
		/// <remarks>reqired, order 0</remarks>
		[System.Xml.Serialization.XmlElementAttribute("RecordingToken", Namespace = "http://www.onvif.org/ver10/schema", DataType = "string")]
		public string recordingToken {
			get {
				return this._recordingToken;
			}
			set {
				this._recordingToken = value;
			}
		}

		/// <summary>
		/// Holds the aggregated state over the whole RecordingJobInformation structure.
		/// </summary>
		/// <remarks>reqired, order 1</remarks>
		[System.Xml.Serialization.XmlElementAttribute("State", Namespace = "http://www.onvif.org/ver10/schema", DataType = "string")]
		public string state {
			get {
				return this._state;
			}
			set {
				this._state = value;
			}
		}

		/// <summary>
		/// Identifies the data source of the recording job.
		/// </summary>
		/// <remarks>optional, order 2</remarks>
		[System.Xml.Serialization.XmlElementAttribute("Sources", Namespace = "http://www.onvif.org/ver10/schema")]
		public RecordingJobStateSource[] sources {
			get {
				return this._sources;
			}
			set {
				this._sources = value;
			}
		}

		/// <remarks>optional, order 3</remarks>
		[System.Xml.Serialization.XmlElementAttribute("Extension", Namespace = "http://www.onvif.org/ver10/schema")]
		public RecordingJobStateInformationExtension extension {
			get {
				return this._extension;
			}
			set {
				this._extension = value;
			}
		}
	}

	[System.SerializableAttribute()]
	[System.Xml.Serialization.XmlTypeAttribute(TypeName = "RecordingJobStateSource", Namespace = "http://www.onvif.org/ver10/schema")]
	public partial class RecordingJobStateSource {

		private System.Xml.XmlAttribute[] _anyAttr;

		private SourceReference _sourceToken;

		private string _state;

		private RecordingJobStateTracks _tracks;

		private System.Xml.XmlElement[] _any;

		[System.Xml.Serialization.XmlAnyAttributeAttribute()]
		public System.Xml.XmlAttribute[] anyAttr {
			get {
				return this._anyAttr;
			}
			set {
				this._anyAttr = value;
			}
		}

		/// <summary>
		/// Identifies the data source of the recording job.
		/// </summary>
		/// <remarks>reqired, order 0</remarks>
		[System.Xml.Serialization.XmlElementAttribute("SourceToken", Namespace = "http://www.onvif.org/ver10/schema")]
		public SourceReference sourceToken {
			get {
				return this._sourceToken;
			}
			set {
				this._sourceToken = value;
			}
		}

		/// <summary>
		/// Holds the aggregated state over all substructures of RecordingJobStateSource.
		/// </summary>
		/// <remarks>reqired, order 1</remarks>
		[System.Xml.Serialization.XmlElementAttribute("State", Namespace = "http://www.onvif.org/ver10/schema", DataType = "string")]
		public string state {
			get {
				return this._state;
			}
			set {
				this._state = value;
			}
		}

		/// <summary>
		/// List of track items.
		/// </summary>
		/// <remarks>reqired, order 2</remarks>
		[System.Xml.Serialization.XmlElementAttribute("Tracks", Namespace = "http://www.onvif.org/ver10/schema")]
		public RecordingJobStateTracks tracks {
			get {
				return this._tracks;
			}
			set {
				this._tracks = value;
			}
		}

		/// <remarks>optional, order 3, namespace ##any</remarks>
		[System.Xml.Serialization.XmlAnyElementAttribute()]
		public System.Xml.XmlElement[] any {
			get {
				return this._any;
			}
			set {
				this._any = value;
			}
		}
	}

	[System.SerializableAttribute()]
	[System.Xml.Serialization.XmlTypeAttribute(TypeName = "RecordingJobStateTracks", Namespace = "http://www.onvif.org/ver10/schema")]
	public partial class RecordingJobStateTracks {

		private System.Xml.XmlAttribute[] _anyAttr;

		private RecordingJobStateTrack[] _track;

		[System.Xml.Serialization.XmlAnyAttributeAttribute()]
		public System.Xml.XmlAttribute[] anyAttr {
			get {
				return this._anyAttr;
			}
			set {
				this._anyAttr = value;
			}
		}

		/// <remarks>optional, order 0</remarks>
		[System.Xml.Serialization.XmlElementAttribute("Track", Namespace = "http://www.onvif.org/ver10/schema")]
		public RecordingJobStateTrack[] track {
			get {
				return this._track;
			}
			set {
				this._track = value;
			}
		}
	}

	[System.SerializableAttribute()]
	[System.Xml.Serialization.XmlTypeAttribute(TypeName = "RecordingJobStateTrack", Namespace = "http://www.onvif.org/ver10/schema")]
	public partial class RecordingJobStateTrack {

		private System.Xml.XmlAttribute[] _anyAttr;

		private string _sourceTag;

		private string _destination;

		private string _error;

		private string _state;

		private System.Xml.XmlElement[] _any;

		[System.Xml.Serialization.XmlAnyAttributeAttribute()]
		public System.Xml.XmlAttribute[] anyAttr {
			get {
				return this._anyAttr;
			}
			set {
				this._anyAttr = value;
			}
		}

		/// <summary>
		/// Identifies the track of the data source that provides the data.
		/// </summary>
		/// <remarks>reqired, order 0</remarks>
		[System.Xml.Serialization.XmlElementAttribute("SourceTag", Namespace = "http://www.onvif.org/ver10/schema", DataType = "string")]
		public string sourceTag {
			get {
				return this._sourceTag;
			}
			set {
				this._sourceTag = value;
			}
		}

		/// <summary>
		/// Indicates the destination track.
		/// </summary>
		/// <remarks>reqired, order 1</remarks>
		[System.Xml.Serialization.XmlElementAttribute("Destination", Namespace = "http://www.onvif.org/ver10/schema", DataType = "string")]
		public string destination {
			get {
				return this._destination;
			}
			set {
				this._destination = value;
			}
		}

		/// <summary>
		/// Optionally holds an implementation defined string value that describes the error.
		/// The string should be in the English language.
		/// </summary>
		/// <remarks>optional, order 2</remarks>
		[System.Xml.Serialization.XmlElementAttribute("Error", Namespace = "http://www.onvif.org/ver10/schema", DataType = "string")]
		public string error {
			get {
				return this._error;
			}
			set {
				this._error = value;
			}
		}

		/// <summary>
		/// Provides the job state of the track. The valid
		/// values of state shall be "Idle", "Active" and "Error". If state equals "Error", the Error field may be filled in with an implementation defined value.
		/// </summary>
		/// <remarks>reqired, order 3</remarks>
		[System.Xml.Serialization.XmlElementAttribute("State", Namespace = "http://www.onvif.org/ver10/schema", DataType = "string")]
		public string state {
			get {
				return this._state;
			}
			set {
				this._state = value;
			}
		}

		/// <remarks>optional, order 4, namespace ##any</remarks>
		[System.Xml.Serialization.XmlAnyElementAttribute()]
		public System.Xml.XmlElement[] any {
			get {
				return this._any;
			}
			set {
				this._any = value;
			}
		}
	}

	[System.SerializableAttribute()]
	[System.Xml.Serialization.XmlTypeAttribute(TypeName = "RecordingJobStateInformationExtension", Namespace = "http://www.onvif.org/ver10/schema")]
	public partial class RecordingJobStateInformationExtension {

		private System.Xml.XmlElement[] _any;

		/// <remarks>optional, order 0, namespace ##any</remarks>
		[System.Xml.Serialization.XmlAnyElementAttribute()]
		public System.Xml.XmlElement[] any {
			get {
				return this._any;
			}
			set {
				this._any = value;
			}
		}
	}

	[System.SerializableAttribute()]
	[System.Xml.Serialization.XmlTypeAttribute(TypeName = "GetRecordingJobsResponseItem", Namespace = "http://www.onvif.org/ver10/schema")]
	public partial class GetRecordingJobsResponseItem {

		private System.Xml.XmlAttribute[] _anyAttr;

		private string _jobToken;

		private RecordingJobConfiguration _jobConfiguration;

		private System.Xml.XmlElement[] _any;

		[System.Xml.Serialization.XmlAnyAttributeAttribute()]
		public System.Xml.XmlAttribute[] anyAttr {
			get {
				return this._anyAttr;
			}
			set {
				this._anyAttr = value;
			}
		}

		/// <remarks>reqired, order 0</remarks>
		[System.Xml.Serialization.XmlElementAttribute("JobToken", Namespace = "http://www.onvif.org/ver10/schema", DataType = "string")]
		public string jobToken {
			get {
				return this._jobToken;
			}
			set {
				this._jobToken = value;
			}
		}

		/// <remarks>reqired, order 1</remarks>
		[System.Xml.Serialization.XmlElementAttribute("JobConfiguration", Namespace = "http://www.onvif.org/ver10/schema")]
		public RecordingJobConfiguration jobConfiguration {
			get {
				return this._jobConfiguration;
			}
			set {
				this._jobConfiguration = value;
			}
		}

		/// <remarks>optional, order 2, namespace ##any</remarks>
		[System.Xml.Serialization.XmlAnyElementAttribute()]
		public System.Xml.XmlElement[] any {
			get {
				return this._any;
			}
			set {
				this._any = value;
			}
		}
	}

	/// <summary>
	/// Configuration parameters for the replay service.
	/// </summary>
	[System.SerializableAttribute()]
	[System.Xml.Serialization.XmlTypeAttribute(TypeName = "ReplayConfiguration", Namespace = "http://www.onvif.org/ver10/schema")]
	public partial class ReplayConfiguration {

		private System.Xml.XmlAttribute[] _anyAttr;

		private XsDuration _sessionTimeout;

		private System.Xml.XmlElement[] _any;

		[System.Xml.Serialization.XmlAnyAttributeAttribute()]
		public System.Xml.XmlAttribute[] anyAttr {
			get {
				return this._anyAttr;
			}
			set {
				this._anyAttr = value;
			}
		}

		/// <summary>
		/// The RTSP session timeout.
		/// </summary>
		/// <remarks>reqired, order 0</remarks>
		[System.Xml.Serialization.XmlElementAttribute("SessionTimeout", Namespace = "http://www.onvif.org/ver10/schema")]
		public XsDuration sessionTimeout {
			get {
				return this._sessionTimeout;
			}
			set {
				this._sessionTimeout = value;
			}
		}

		/// <remarks>optional, order 1, namespace ##any</remarks>
		[System.Xml.Serialization.XmlAnyElementAttribute()]
		public System.Xml.XmlElement[] any {
			get {
				return this._any;
			}
			set {
				this._any = value;
			}
		}
	}

	[System.SerializableAttribute()]
	[System.Xml.Serialization.XmlTypeAttribute(TypeName = "AnalyticsEngine", Namespace = "http://www.onvif.org/ver10/schema")]
	public partial class AnalyticsEngine {

		private string _token;

		private System.Xml.XmlAttribute[] _anyAttr;

		private string _name;

		private int _useCount;

		private AnalyticsDeviceEngineConfiguration _analyticsEngineConfiguration;

		private System.Xml.XmlElement[] _any;

		/// <summary>
		/// Token that uniquely refernces this configuration. Length up to 64 characters.
		/// </summary>
		[System.Xml.Serialization.XmlAttributeAttribute("token")]
		public string token {
			get {
				return this._token;
			}
			set {
				this._token = value;
			}
		}

		[System.Xml.Serialization.XmlAnyAttributeAttribute()]
		public System.Xml.XmlAttribute[] anyAttr {
			get {
				return this._anyAttr;
			}
			set {
				this._anyAttr = value;
			}
		}

		/// <summary>
		/// User readable name. Length up to 64 characters.
		/// </summary>
		/// <remarks>reqired, order 0</remarks>
		[System.Xml.Serialization.XmlElementAttribute("Name", Namespace = "http://www.onvif.org/ver10/schema", DataType = "string")]
		public string name {
			get {
				return this._name;
			}
			set {
				this._name = value;
			}
		}

		/// <summary>
		/// Number of internal references currently using this configuration.
		/// This parameter is read-only and cannot be changed by a set request.
		/// For example the value increases if the configuration is added to a media profile or attached to a PaneConfiguration.
		/// </summary>
		/// <remarks>reqired, order 1</remarks>
		[System.Xml.Serialization.XmlElementAttribute("UseCount", Namespace = "http://www.onvif.org/ver10/schema", DataType = "int")]
		public int useCount {
			get {
				return this._useCount;
			}
			set {
				this._useCount = value;
			}
		}

		/// <remarks>reqired, order 2</remarks>
		[System.Xml.Serialization.XmlElementAttribute("AnalyticsEngineConfiguration", Namespace = "http://www.onvif.org/ver10/schema")]
		public AnalyticsDeviceEngineConfiguration analyticsEngineConfiguration {
			get {
				return this._analyticsEngineConfiguration;
			}
			set {
				this._analyticsEngineConfiguration = value;
			}
		}

		/// <remarks>optional, order 3, namespace ##any</remarks>
		[System.Xml.Serialization.XmlAnyElementAttribute()]
		public System.Xml.XmlElement[] any {
			get {
				return this._any;
			}
			set {
				this._any = value;
			}
		}
	}

	[System.SerializableAttribute()]
	[System.Xml.Serialization.XmlTypeAttribute(TypeName = "AnalyticsDeviceEngineConfiguration", Namespace = "http://www.onvif.org/ver10/schema")]
	public partial class AnalyticsDeviceEngineConfiguration {

		private System.Xml.XmlAttribute[] _anyAttr;

		private EngineConfiguration[] _engineConfiguration;

		private AnalyticsDeviceEngineConfigurationExtension _extension;

		[System.Xml.Serialization.XmlAnyAttributeAttribute()]
		public System.Xml.XmlAttribute[] anyAttr {
			get {
				return this._anyAttr;
			}
			set {
				this._anyAttr = value;
			}
		}

		/// <remarks>reqired, order 0</remarks>
		[System.Xml.Serialization.XmlElementAttribute("EngineConfiguration", Namespace = "http://www.onvif.org/ver10/schema")]
		public EngineConfiguration[] engineConfiguration {
			get {
				return this._engineConfiguration;
			}
			set {
				this._engineConfiguration = value;
			}
		}

		/// <remarks>optional, order 1</remarks>
		[System.Xml.Serialization.XmlElementAttribute("Extension", Namespace = "http://www.onvif.org/ver10/schema")]
		public AnalyticsDeviceEngineConfigurationExtension extension {
			get {
				return this._extension;
			}
			set {
				this._extension = value;
			}
		}
	}

	[System.SerializableAttribute()]
	[System.Xml.Serialization.XmlTypeAttribute(TypeName = "EngineConfiguration", Namespace = "http://www.onvif.org/ver10/schema")]
	public partial class EngineConfiguration {

		private System.Xml.XmlAttribute[] _anyAttr;

		private VideoAnalyticsConfiguration _videoAnalyticsConfiguration;

		private AnalyticsEngineInputInfo _analyticsEngineInputInfo;

		private System.Xml.XmlElement[] _any;

		[System.Xml.Serialization.XmlAnyAttributeAttribute()]
		public System.Xml.XmlAttribute[] anyAttr {
			get {
				return this._anyAttr;
			}
			set {
				this._anyAttr = value;
			}
		}

		/// <remarks>reqired, order 0</remarks>
		[System.Xml.Serialization.XmlElementAttribute("VideoAnalyticsConfiguration", Namespace = "http://www.onvif.org/ver10/schema")]
		public VideoAnalyticsConfiguration videoAnalyticsConfiguration {
			get {
				return this._videoAnalyticsConfiguration;
			}
			set {
				this._videoAnalyticsConfiguration = value;
			}
		}

		/// <remarks>reqired, order 1</remarks>
		[System.Xml.Serialization.XmlElementAttribute("AnalyticsEngineInputInfo", Namespace = "http://www.onvif.org/ver10/schema")]
		public AnalyticsEngineInputInfo analyticsEngineInputInfo {
			get {
				return this._analyticsEngineInputInfo;
			}
			set {
				this._analyticsEngineInputInfo = value;
			}
		}

		/// <remarks>optional, order 2, namespace ##any</remarks>
		[System.Xml.Serialization.XmlAnyElementAttribute()]
		public System.Xml.XmlElement[] any {
			get {
				return this._any;
			}
			set {
				this._any = value;
			}
		}
	}

	[System.SerializableAttribute()]
	[System.Xml.Serialization.XmlTypeAttribute(TypeName = "AnalyticsEngineInputInfo", Namespace = "http://www.onvif.org/ver10/schema")]
	public partial class AnalyticsEngineInputInfo {

		private System.Xml.XmlAttribute[] _anyAttr;

		private Config _inputInfo;

		private AnalyticsEngineInputInfoExtension _extension;

		[System.Xml.Serialization.XmlAnyAttributeAttribute()]
		public System.Xml.XmlAttribute[] anyAttr {
			get {
				return this._anyAttr;
			}
			set {
				this._anyAttr = value;
			}
		}

		/// <remarks>optional, order 0</remarks>
		[System.Xml.Serialization.XmlElementAttribute("InputInfo", Namespace = "http://www.onvif.org/ver10/schema")]
		public Config inputInfo {
			get {
				return this._inputInfo;
			}
			set {
				this._inputInfo = value;
			}
		}

		/// <remarks>optional, order 1</remarks>
		[System.Xml.Serialization.XmlElementAttribute("Extension", Namespace = "http://www.onvif.org/ver10/schema")]
		public AnalyticsEngineInputInfoExtension extension {
			get {
				return this._extension;
			}
			set {
				this._extension = value;
			}
		}
	}

	[System.SerializableAttribute()]
	[System.Xml.Serialization.XmlTypeAttribute(TypeName = "AnalyticsEngineInputInfoExtension", Namespace = "http://www.onvif.org/ver10/schema")]
	public partial class AnalyticsEngineInputInfoExtension {

		private System.Xml.XmlElement[] _any;

		/// <remarks>optional, order 0, namespace ##any</remarks>
		[System.Xml.Serialization.XmlAnyElementAttribute()]
		public System.Xml.XmlElement[] any {
			get {
				return this._any;
			}
			set {
				this._any = value;
			}
		}
	}

	[System.SerializableAttribute()]
	[System.Xml.Serialization.XmlTypeAttribute(TypeName = "AnalyticsDeviceEngineConfigurationExtension", Namespace = "http://www.onvif.org/ver10/schema")]
	public partial class AnalyticsDeviceEngineConfigurationExtension {

		private System.Xml.XmlElement[] _any;

		/// <remarks>optional, order 0, namespace ##any</remarks>
		[System.Xml.Serialization.XmlAnyElementAttribute()]
		public System.Xml.XmlElement[] any {
			get {
				return this._any;
			}
			set {
				this._any = value;
			}
		}
	}

	[System.SerializableAttribute()]
	[System.Xml.Serialization.XmlTypeAttribute(TypeName = "AnalyticsEngineInput", Namespace = "http://www.onvif.org/ver10/schema")]
	public partial class AnalyticsEngineInput {

		private string _token;

		private System.Xml.XmlAttribute[] _anyAttr;

		private string _name;

		private int _useCount;

		private SourceIdentification _sourceIdentification;

		private VideoEncoderConfiguration _videoInput;

		private MetadataInput _metadataInput;

		private System.Xml.XmlElement[] _any;

		/// <summary>
		/// Token that uniquely refernces this configuration. Length up to 64 characters.
		/// </summary>
		[System.Xml.Serialization.XmlAttributeAttribute("token")]
		public string token {
			get {
				return this._token;
			}
			set {
				this._token = value;
			}
		}

		[System.Xml.Serialization.XmlAnyAttributeAttribute()]
		public System.Xml.XmlAttribute[] anyAttr {
			get {
				return this._anyAttr;
			}
			set {
				this._anyAttr = value;
			}
		}

		/// <summary>
		/// User readable name. Length up to 64 characters.
		/// </summary>
		/// <remarks>reqired, order 0</remarks>
		[System.Xml.Serialization.XmlElementAttribute("Name", Namespace = "http://www.onvif.org/ver10/schema", DataType = "string")]
		public string name {
			get {
				return this._name;
			}
			set {
				this._name = value;
			}
		}

		/// <summary>
		/// Number of internal references currently using this configuration.
		/// This parameter is read-only and cannot be changed by a set request.
		/// For example the value increases if the configuration is added to a media profile or attached to a PaneConfiguration.
		/// </summary>
		/// <remarks>reqired, order 1</remarks>
		[System.Xml.Serialization.XmlElementAttribute("UseCount", Namespace = "http://www.onvif.org/ver10/schema", DataType = "int")]
		public int useCount {
			get {
				return this._useCount;
			}
			set {
				this._useCount = value;
			}
		}

		/// <remarks>reqired, order 2</remarks>
		[System.Xml.Serialization.XmlElementAttribute("SourceIdentification", Namespace = "http://www.onvif.org/ver10/schema")]
		public SourceIdentification sourceIdentification {
			get {
				return this._sourceIdentification;
			}
			set {
				this._sourceIdentification = value;
			}
		}

		/// <remarks>reqired, order 3</remarks>
		[System.Xml.Serialization.XmlElementAttribute("VideoInput", Namespace = "http://www.onvif.org/ver10/schema")]
		public VideoEncoderConfiguration videoInput {
			get {
				return this._videoInput;
			}
			set {
				this._videoInput = value;
			}
		}

		/// <remarks>reqired, order 4</remarks>
		[System.Xml.Serialization.XmlElementAttribute("MetadataInput", Namespace = "http://www.onvif.org/ver10/schema")]
		public MetadataInput metadataInput {
			get {
				return this._metadataInput;
			}
			set {
				this._metadataInput = value;
			}
		}

		/// <remarks>optional, order 5, namespace ##any</remarks>
		[System.Xml.Serialization.XmlAnyElementAttribute()]
		public System.Xml.XmlElement[] any {
			get {
				return this._any;
			}
			set {
				this._any = value;
			}
		}
	}

	[System.SerializableAttribute()]
	[System.Xml.Serialization.XmlTypeAttribute(TypeName = "SourceIdentification", Namespace = "http://www.onvif.org/ver10/schema")]
	public partial class SourceIdentification {

		private System.Xml.XmlAttribute[] _anyAttr;

		private string _name;

		private string[] _token;

		private SourceIdentificationExtension _extension;

		[System.Xml.Serialization.XmlAnyAttributeAttribute()]
		public System.Xml.XmlAttribute[] anyAttr {
			get {
				return this._anyAttr;
			}
			set {
				this._anyAttr = value;
			}
		}

		/// <remarks>reqired, order 0</remarks>
		[System.Xml.Serialization.XmlElementAttribute("Name", Namespace = "http://www.onvif.org/ver10/schema", DataType = "string")]
		public string name {
			get {
				return this._name;
			}
			set {
				this._name = value;
			}
		}

		/// <remarks>reqired, order 1</remarks>
		[System.Xml.Serialization.XmlElementAttribute("Token", Namespace = "http://www.onvif.org/ver10/schema", DataType = "string")]
		public string[] token {
			get {
				return this._token;
			}
			set {
				this._token = value;
			}
		}

		/// <remarks>optional, order 2</remarks>
		[System.Xml.Serialization.XmlElementAttribute("Extension", Namespace = "http://www.onvif.org/ver10/schema")]
		public SourceIdentificationExtension extension {
			get {
				return this._extension;
			}
			set {
				this._extension = value;
			}
		}
	}

	[System.SerializableAttribute()]
	[System.Xml.Serialization.XmlTypeAttribute(TypeName = "SourceIdentificationExtension", Namespace = "http://www.onvif.org/ver10/schema")]
	public partial class SourceIdentificationExtension {

		private System.Xml.XmlElement[] _any;

		/// <remarks>optional, order 0, namespace ##any</remarks>
		[System.Xml.Serialization.XmlAnyElementAttribute()]
		public System.Xml.XmlElement[] any {
			get {
				return this._any;
			}
			set {
				this._any = value;
			}
		}
	}

	[System.SerializableAttribute()]
	[System.Xml.Serialization.XmlTypeAttribute(TypeName = "MetadataInput", Namespace = "http://www.onvif.org/ver10/schema")]
	public partial class MetadataInput {

		private System.Xml.XmlAttribute[] _anyAttr;

		private Config[] _metadataConfig;

		private MetadataInputExtension _extension;

		[System.Xml.Serialization.XmlAnyAttributeAttribute()]
		public System.Xml.XmlAttribute[] anyAttr {
			get {
				return this._anyAttr;
			}
			set {
				this._anyAttr = value;
			}
		}

		/// <remarks>optional, order 0</remarks>
		[System.Xml.Serialization.XmlElementAttribute("MetadataConfig", Namespace = "http://www.onvif.org/ver10/schema")]
		public Config[] metadataConfig {
			get {
				return this._metadataConfig;
			}
			set {
				this._metadataConfig = value;
			}
		}

		/// <remarks>optional, order 1</remarks>
		[System.Xml.Serialization.XmlElementAttribute("Extension", Namespace = "http://www.onvif.org/ver10/schema")]
		public MetadataInputExtension extension {
			get {
				return this._extension;
			}
			set {
				this._extension = value;
			}
		}
	}

	[System.SerializableAttribute()]
	[System.Xml.Serialization.XmlTypeAttribute(TypeName = "MetadataInputExtension", Namespace = "http://www.onvif.org/ver10/schema")]
	public partial class MetadataInputExtension {

		private System.Xml.XmlElement[] _any;

		/// <remarks>optional, order 0, namespace ##any</remarks>
		[System.Xml.Serialization.XmlAnyElementAttribute()]
		public System.Xml.XmlElement[] any {
			get {
				return this._any;
			}
			set {
				this._any = value;
			}
		}
	}

	[System.SerializableAttribute()]
	[System.Xml.Serialization.XmlTypeAttribute(TypeName = "AnalyticsEngineControl", Namespace = "http://www.onvif.org/ver10/schema")]
	public partial class AnalyticsEngineControl {

		private string _token;

		private System.Xml.XmlAttribute[] _anyAttr;

		private string _name;

		private int _useCount;

		private string _engineToken;

		private string _engineConfigToken;

		private string[] _inputToken;

		private string[] _receiverToken;

		private MulticastConfiguration _multicast;

		private Config _subscription;

		private ModeOfOperation _mode;

		private System.Xml.XmlElement[] _any;

		/// <summary>
		/// Token that uniquely refernces this configuration. Length up to 64 characters.
		/// </summary>
		[System.Xml.Serialization.XmlAttributeAttribute("token")]
		public string token {
			get {
				return this._token;
			}
			set {
				this._token = value;
			}
		}

		[System.Xml.Serialization.XmlAnyAttributeAttribute()]
		public System.Xml.XmlAttribute[] anyAttr {
			get {
				return this._anyAttr;
			}
			set {
				this._anyAttr = value;
			}
		}

		/// <summary>
		/// User readable name. Length up to 64 characters.
		/// </summary>
		/// <remarks>reqired, order 0</remarks>
		[System.Xml.Serialization.XmlElementAttribute("Name", Namespace = "http://www.onvif.org/ver10/schema", DataType = "string")]
		public string name {
			get {
				return this._name;
			}
			set {
				this._name = value;
			}
		}

		/// <summary>
		/// Number of internal references currently using this configuration.
		/// This parameter is read-only and cannot be changed by a set request.
		/// For example the value increases if the configuration is added to a media profile or attached to a PaneConfiguration.
		/// </summary>
		/// <remarks>reqired, order 1</remarks>
		[System.Xml.Serialization.XmlElementAttribute("UseCount", Namespace = "http://www.onvif.org/ver10/schema", DataType = "int")]
		public int useCount {
			get {
				return this._useCount;
			}
			set {
				this._useCount = value;
			}
		}

		/// <summary>
		/// Token of the analytics engine (AnalyticsEngine) being controlled.
		/// </summary>
		/// <remarks>reqired, order 2</remarks>
		[System.Xml.Serialization.XmlElementAttribute("EngineToken", Namespace = "http://www.onvif.org/ver10/schema", DataType = "string")]
		public string engineToken {
			get {
				return this._engineToken;
			}
			set {
				this._engineToken = value;
			}
		}

		/// <summary>
		/// Token of the analytics engine configuration (VideoAnalyticsConfiguration) in effect.
		/// </summary>
		/// <remarks>reqired, order 3</remarks>
		[System.Xml.Serialization.XmlElementAttribute("EngineConfigToken", Namespace = "http://www.onvif.org/ver10/schema", DataType = "string")]
		public string engineConfigToken {
			get {
				return this._engineConfigToken;
			}
			set {
				this._engineConfigToken = value;
			}
		}

		/// <summary>
		/// Tokens of the input (AnalyticsEngineInput) configuration applied.
		/// </summary>
		/// <remarks>reqired, order 4</remarks>
		[System.Xml.Serialization.XmlElementAttribute("InputToken", Namespace = "http://www.onvif.org/ver10/schema", DataType = "string")]
		public string[] inputToken {
			get {
				return this._inputToken;
			}
			set {
				this._inputToken = value;
			}
		}

		/// <summary>
		/// Tokens of the receiver providing media input data. The order of ReceiverToken shall exactly match the order of InputToken.
		/// </summary>
		/// <remarks>reqired, order 5</remarks>
		[System.Xml.Serialization.XmlElementAttribute("ReceiverToken", Namespace = "http://www.onvif.org/ver10/schema", DataType = "string")]
		public string[] receiverToken {
			get {
				return this._receiverToken;
			}
			set {
				this._receiverToken = value;
			}
		}

		/// <remarks>optional, order 6</remarks>
		[System.Xml.Serialization.XmlElementAttribute("Multicast", Namespace = "http://www.onvif.org/ver10/schema")]
		public MulticastConfiguration multicast {
			get {
				return this._multicast;
			}
			set {
				this._multicast = value;
			}
		}

		/// <remarks>reqired, order 7</remarks>
		[System.Xml.Serialization.XmlElementAttribute("Subscription", Namespace = "http://www.onvif.org/ver10/schema")]
		public Config subscription {
			get {
				return this._subscription;
			}
			set {
				this._subscription = value;
			}
		}

		/// <remarks>reqired, order 8</remarks>
		[System.Xml.Serialization.XmlElementAttribute("Mode", Namespace = "http://www.onvif.org/ver10/schema")]
		public ModeOfOperation mode {
			get {
				return this._mode;
			}
			set {
				this._mode = value;
			}
		}

		/// <remarks>optional, order 9, namespace ##any</remarks>
		[System.Xml.Serialization.XmlAnyElementAttribute()]
		public System.Xml.XmlElement[] any {
			get {
				return this._any;
			}
			set {
				this._any = value;
			}
		}
	}

	[System.SerializableAttribute()]
	[System.Xml.Serialization.XmlTypeAttribute(TypeName = "ModeOfOperation", Namespace = "http://www.onvif.org/ver10/schema")]
	public enum ModeOfOperation {

		[System.Xml.Serialization.XmlEnumAttribute(Name = "Idle")]
		idle,

		[System.Xml.Serialization.XmlEnumAttribute(Name = "Active")]
		active,

		/// <summary>
		/// This case should never happen.
		/// </summary>
		[System.Xml.Serialization.XmlEnumAttribute(Name = "Unknown")]
		unknown,
	}

	[System.SerializableAttribute()]
	[System.Xml.Serialization.XmlTypeAttribute(TypeName = "AnalyticsStateInformation", Namespace = "http://www.onvif.org/ver10/schema")]
	public partial class AnalyticsStateInformation {

		private System.Xml.XmlAttribute[] _anyAttr;

		private string _analyticsEngineControlToken;

		private AnalyticsState _state;

		private System.Xml.XmlElement[] _any;

		[System.Xml.Serialization.XmlAnyAttributeAttribute()]
		public System.Xml.XmlAttribute[] anyAttr {
			get {
				return this._anyAttr;
			}
			set {
				this._anyAttr = value;
			}
		}

		/// <summary>
		/// Token of the control object whose status is requested.
		/// </summary>
		/// <remarks>reqired, order 0</remarks>
		[System.Xml.Serialization.XmlElementAttribute("AnalyticsEngineControlToken", Namespace = "http://www.onvif.org/ver10/schema", DataType = "string")]
		public string analyticsEngineControlToken {
			get {
				return this._analyticsEngineControlToken;
			}
			set {
				this._analyticsEngineControlToken = value;
			}
		}

		/// <remarks>reqired, order 1</remarks>
		[System.Xml.Serialization.XmlElementAttribute("State", Namespace = "http://www.onvif.org/ver10/schema")]
		public AnalyticsState state {
			get {
				return this._state;
			}
			set {
				this._state = value;
			}
		}

		/// <remarks>optional, order 2, namespace ##any</remarks>
		[System.Xml.Serialization.XmlAnyElementAttribute()]
		public System.Xml.XmlElement[] any {
			get {
				return this._any;
			}
			set {
				this._any = value;
			}
		}
	}

	[System.SerializableAttribute()]
	[System.Xml.Serialization.XmlTypeAttribute(TypeName = "AnalyticsState", Namespace = "http://www.onvif.org/ver10/schema")]
	public partial class AnalyticsState {

		private System.Xml.XmlAttribute[] _anyAttr;

		private string _error;

		private string _state;

		private System.Xml.XmlElement[] _any;

		[System.Xml.Serialization.XmlAnyAttributeAttribute()]
		public System.Xml.XmlAttribute[] anyAttr {
			get {
				return this._anyAttr;
			}
			set {
				this._anyAttr = value;
			}
		}

		/// <remarks>optional, order 0</remarks>
		[System.Xml.Serialization.XmlElementAttribute("Error", Namespace = "http://www.onvif.org/ver10/schema", DataType = "string")]
		public string error {
			get {
				return this._error;
			}
			set {
				this._error = value;
			}
		}

		/// <remarks>reqired, order 1</remarks>
		[System.Xml.Serialization.XmlElementAttribute("State", Namespace = "http://www.onvif.org/ver10/schema", DataType = "string")]
		public string state {
			get {
				return this._state;
			}
			set {
				this._state = value;
			}
		}

		/// <remarks>optional, order 2, namespace ##any</remarks>
		[System.Xml.Serialization.XmlAnyElementAttribute()]
		public System.Xml.XmlElement[] any {
			get {
				return this._any;
			}
			set {
				this._any = value;
			}
		}
	}

	/// <summary>
	/// Action Engine Event Payload data structure contains the information about the ONVIF command invocations. Since this event could be generated by other or proprietary actions, the command invocation specific fields are defined as optional and additional extension mechanism is provided for future or additional action definitions.
	/// </summary>
	[System.SerializableAttribute()]
	[System.Xml.Serialization.XmlTypeAttribute(TypeName = "ActionEngineEventPayload", Namespace = "http://www.onvif.org/ver10/schema")]
	public partial class ActionEngineEventPayload {

		private System.Xml.XmlAttribute[] _anyAttr;

		private soapenv.Envelope _requestInfo;

		private soapenv.Envelope _responseInfo;

		private soapenv.Fault _fault;

		private ActionEngineEventPayloadExtension _extension;

		[System.Xml.Serialization.XmlAnyAttributeAttribute()]
		public System.Xml.XmlAttribute[] anyAttr {
			get {
				return this._anyAttr;
			}
			set {
				this._anyAttr = value;
			}
		}

		/// <summary>
		/// Request Message
		/// </summary>
		/// <remarks>optional, order 0</remarks>
		[System.Xml.Serialization.XmlElementAttribute("RequestInfo", Namespace = "http://www.onvif.org/ver10/schema")]
		public soapenv.Envelope requestInfo {
			get {
				return this._requestInfo;
			}
			set {
				this._requestInfo = value;
			}
		}

		/// <summary>
		/// Response Message
		/// </summary>
		/// <remarks>optional, order 1</remarks>
		[System.Xml.Serialization.XmlElementAttribute("ResponseInfo", Namespace = "http://www.onvif.org/ver10/schema")]
		public soapenv.Envelope responseInfo {
			get {
				return this._responseInfo;
			}
			set {
				this._responseInfo = value;
			}
		}

		/// <summary>
		/// Fault Message
		/// </summary>
		/// <remarks>optional, order 2</remarks>
		[System.Xml.Serialization.XmlElementAttribute("Fault", Namespace = "http://www.onvif.org/ver10/schema")]
		public soapenv.Fault fault {
			get {
				return this._fault;
			}
			set {
				this._fault = value;
			}
		}

		/// <remarks>optional, order 3</remarks>
		[System.Xml.Serialization.XmlElementAttribute("Extension", Namespace = "http://www.onvif.org/ver10/schema")]
		public ActionEngineEventPayloadExtension extension {
			get {
				return this._extension;
			}
			set {
				this._extension = value;
			}
		}
	}

	[System.SerializableAttribute()]
	[System.Xml.Serialization.XmlTypeAttribute(TypeName = "ActionEngineEventPayloadExtension", Namespace = "http://www.onvif.org/ver10/schema")]
	public partial class ActionEngineEventPayloadExtension {

		private System.Xml.XmlAttribute[] _anyAttr;

		private System.Xml.XmlElement[] _any;

		[System.Xml.Serialization.XmlAnyAttributeAttribute()]
		public System.Xml.XmlAttribute[] anyAttr {
			get {
				return this._anyAttr;
			}
			set {
				this._anyAttr = value;
			}
		}

		/// <remarks>optional, order 0, namespace ##any</remarks>
		[System.Xml.Serialization.XmlAnyElementAttribute()]
		public System.Xml.XmlElement[] any {
			get {
				return this._any;
			}
			set {
				this._any = value;
			}
		}
	}

	[System.SerializableAttribute()]
	[System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://www.onvif.org/ver10/schema")]
	[XmlRoot(Namespace = "http://www.onvif.org/ver10/schema", IsNullable = false)]
	public partial class Message {

		private System.DateTime _utcTime;

		private PropertyOperation _propertyOperation;

		private bool _propertyOperationSpecified;

		private System.Xml.XmlAttribute[] _anyAttr;

		private ItemList _source;

		private ItemList _key;

		private ItemList _data;

		private MessageExtension _extension;

		[System.Xml.Serialization.XmlAttributeAttribute("UtcTime", DataType = "dateTime")]
		public System.DateTime utcTime {
			get {
				return this._utcTime;
			}
			set {
				this._utcTime = value;
			}
		}

		[System.Xml.Serialization.XmlAttributeAttribute("PropertyOperation")]
		public PropertyOperation propertyOperation {
			get {
				return this._propertyOperation;
			}
			set {
				this._propertyOperation = value;
			}
		}

		[System.Xml.Serialization.XmlIgnoreAttribute()]
		public bool propertyOperationSpecified {
			get {
				return this._propertyOperationSpecified;
			}
			set {
				this._propertyOperationSpecified = value;
			}
		}

		[System.Xml.Serialization.XmlAnyAttributeAttribute()]
		public System.Xml.XmlAttribute[] anyAttr {
			get {
				return this._anyAttr;
			}
			set {
				this._anyAttr = value;
			}
		}

		/// <summary>
		/// Token value pairs that triggered this message. Typically only one item is present.
		/// </summary>
		/// <remarks>optional, order 0</remarks>
		[System.Xml.Serialization.XmlElementAttribute("Source", Namespace = "http://www.onvif.org/ver10/schema")]
		public ItemList source {
			get {
				return this._source;
			}
			set {
				this._source = value;
			}
		}

		/// <remarks>optional, order 1</remarks>
		[System.Xml.Serialization.XmlElementAttribute("Key", Namespace = "http://www.onvif.org/ver10/schema")]
		public ItemList key {
			get {
				return this._key;
			}
			set {
				this._key = value;
			}
		}

		/// <remarks>optional, order 2</remarks>
		[System.Xml.Serialization.XmlElementAttribute("Data", Namespace = "http://www.onvif.org/ver10/schema")]
		public ItemList data {
			get {
				return this._data;
			}
			set {
				this._data = value;
			}
		}

		/// <remarks>optional, order 3</remarks>
		[System.Xml.Serialization.XmlElementAttribute("Extension", Namespace = "http://www.onvif.org/ver10/schema")]
		public MessageExtension extension {
			get {
				return this._extension;
			}
			set {
				this._extension = value;
			}
		}
	}

	/// <remarks/>
	[System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.0.30319.17929")]
	[System.SerializableAttribute()]
	[System.Diagnostics.DebuggerStepThroughAttribute()]
	[System.ComponentModel.DesignerCategoryAttribute("code")]
	[System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.onvif.org/ver10/deviceIO/wsdl")]
	public partial class RelayOutputOptions {

		private System.Xml.XmlAttribute[] anyAttrField;

		private RelayMode[] modeField;

		private string delayTimesField;

		private bool discreteField;

		private bool discreteFieldSpecified;

		private RelayOutputOptionsExtension extensionField;

		private string tokenField;

		/// <remarks/>
		[System.Xml.Serialization.XmlAnyAttributeAttribute()]
		public System.Xml.XmlAttribute[] AnyAttr {
			get {
				return this.anyAttrField;
			}
			set {
				this.anyAttrField = value;
			}
		}

		/// <remarks/>
		[XmlElement("Mode", Order = 0)]
		public RelayMode[] Mode {
			get {
				return this.modeField;
			}
			set {
				this.modeField = value;
			}
		}

		/// <remarks/>
		[XmlElement(Order = 1)]
		public string DelayTimes {
			get {
				return this.delayTimesField;
			}
			set {
				this.delayTimesField = value;
			}
		}

		/// <remarks/>
		[XmlElement(Order = 2)]
		public bool Discrete {
			get {
				return this.discreteField;
			}
			set {
				this.discreteField = value;
			}
		}

		/// <remarks/>
		[System.Xml.Serialization.XmlIgnoreAttribute()]
		public bool DiscreteSpecified {
			get {
				return this.discreteFieldSpecified;
			}
			set {
				this.discreteFieldSpecified = value;
			}
		}

		/// <remarks/>
		[XmlElement(Order = 3)]
		public RelayOutputOptionsExtension Extension {
			get {
				return this.extensionField;
			}
			set {
				this.extensionField = value;
			}
		}

		/// <remarks/>
		[System.Xml.Serialization.XmlAttributeAttribute()]
		public string token {
			get {
				return this.tokenField;
			}
			set {
				this.tokenField = value;
			}
		}
	}





	/// <remarks/>
	[System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.0.30319.17929")]
	[System.SerializableAttribute()]
	[System.Diagnostics.DebuggerStepThroughAttribute()]
	[System.ComponentModel.DesignerCategoryAttribute("code")]
	[System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://www.onvif.org/ver10/events/wsdl")]
	public partial class PullMessagesFaultResponse : object, System.ComponentModel.INotifyPropertyChanged {

		private string maxTimeoutField;

		private int maxMessageLimitField;

		private System.Xml.XmlElement[] anyField;

		/// <remarks/>
		[XmlElement(DataType = "duration", Order = 0)]
		public string MaxTimeout {
			get {
				return this.maxTimeoutField;
			}
			set {
				this.maxTimeoutField = value;
				this.RaisePropertyChanged("MaxTimeout");
			}
		}

		/// <remarks/>
		[XmlElement(Order = 1)]
		public int MaxMessageLimit {
			get {
				return this.maxMessageLimitField;
			}
			set {
				this.maxMessageLimitField = value;
				this.RaisePropertyChanged("MaxMessageLimit");
			}
		}

		/// <remarks/>
		[System.Xml.Serialization.XmlAnyElementAttribute(Order = 2)]
		public System.Xml.XmlElement[] Any {
			get {
				return this.anyField;
			}
			set {
				this.anyField = value;
				this.RaisePropertyChanged("Any");
			}
		}

		public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;

		protected void RaisePropertyChanged(string propertyName) {
			System.ComponentModel.PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
			if ((propertyChanged != null)) {
				propertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
			}
		}
	}

	/// <remarks/>
	[System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.0.30319.17929")]
	[System.SerializableAttribute()]
	[System.Diagnostics.DebuggerStepThroughAttribute()]
	[System.ComponentModel.DesignerCategoryAttribute("code")]
	[System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://docs.oasis-open.org/wsn/b-2")]
	public partial class TopicExpressionType : object, System.ComponentModel.INotifyPropertyChanged {

		private System.Xml.XmlNode[] anyField;

		private string dialectField;

		private System.Xml.XmlAttribute[] anyAttrField;

		/// <remarks/>
		[System.Xml.Serialization.XmlTextAttribute()]
		[System.Xml.Serialization.XmlAnyElementAttribute(Order = 0)]
		public System.Xml.XmlNode[] Any {
			get {
				return this.anyField;
			}
			set {
				this.anyField = value;
				this.RaisePropertyChanged("Any");
			}
		}

		/// <remarks/>
		[System.Xml.Serialization.XmlAttributeAttribute(DataType = "anyURI")]
		public string Dialect {
			get {
				return this.dialectField;
			}
			set {
				this.dialectField = value;
				this.RaisePropertyChanged("Dialect");
			}
		}

		/// <remarks/>
		[System.Xml.Serialization.XmlAnyAttributeAttribute()]
		public System.Xml.XmlAttribute[] AnyAttr {
			get {
				return this.anyAttrField;
			}
			set {
				this.anyAttrField = value;
				this.RaisePropertyChanged("AnyAttr");
			}
		}

		public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;

		protected void RaisePropertyChanged(string propertyName) {
			System.ComponentModel.PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
			if ((propertyChanged != null)) {
				propertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
			}
		}
	}

	/// <remarks/>
	[System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.0.30319.17929")]
	[System.SerializableAttribute()]
	[System.Diagnostics.DebuggerStepThroughAttribute()]
	[System.ComponentModel.DesignerCategoryAttribute("code")]
	[System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.w3.org/2005/08/addressing")]
	public partial class MetadataType : object, System.ComponentModel.INotifyPropertyChanged {

		private System.Xml.XmlElement[] anyField;

		private System.Xml.XmlAttribute[] anyAttrField;

		/// <remarks/>
		[System.Xml.Serialization.XmlAnyElementAttribute(Order = 0)]
		public System.Xml.XmlElement[] Any {
			get {
				return this.anyField;
			}
			set {
				this.anyField = value;
				this.RaisePropertyChanged("Any");
			}
		}

		/// <remarks/>
		[System.Xml.Serialization.XmlAnyAttributeAttribute()]
		public System.Xml.XmlAttribute[] AnyAttr {
			get {
				return this.anyAttrField;
			}
			set {
				this.anyAttrField = value;
				this.RaisePropertyChanged("AnyAttr");
			}
		}

		public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;

		protected void RaisePropertyChanged(string propertyName) {
			System.ComponentModel.PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
			if ((propertyChanged != null)) {
				propertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
			}
		}
	}

	/// <remarks/>
	[System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.0.30319.17929")]
	[System.SerializableAttribute()]
	[System.Diagnostics.DebuggerStepThroughAttribute()]
	[System.ComponentModel.DesignerCategoryAttribute("code")]
	[System.Xml.Serialization.XmlTypeAttribute(TypeName = "ReferenceParametersType", Namespace = "http://www.w3.org/2005/08/addressing")]
	public partial class ReferenceParametersType1 : object, System.ComponentModel.INotifyPropertyChanged {

		private System.Xml.XmlElement[] anyField;

		private System.Xml.XmlAttribute[] anyAttrField;

		/// <remarks/>
		[System.Xml.Serialization.XmlAnyElementAttribute(Order = 0)]
		public System.Xml.XmlElement[] Any {
			get {
				return this.anyField;
			}
			set {
				this.anyField = value;
				this.RaisePropertyChanged("Any");
			}
		}

		/// <remarks/>
		[System.Xml.Serialization.XmlAnyAttributeAttribute()]
		public System.Xml.XmlAttribute[] AnyAttr {
			get {
				return this.anyAttrField;
			}
			set {
				this.anyAttrField = value;
				this.RaisePropertyChanged("AnyAttr");
			}
		}

		public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;

		protected void RaisePropertyChanged(string propertyName) {
			System.ComponentModel.PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
			if ((propertyChanged != null)) {
				propertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
			}
		}
	}

	/// <remarks/>
	[System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.0.30319.17929")]
	[System.SerializableAttribute()]
	[System.Diagnostics.DebuggerStepThroughAttribute()]
	[System.ComponentModel.DesignerCategoryAttribute("code")]
	[System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.w3.org/2005/08/addressing")]
	public partial class AttributedURIType : object, System.ComponentModel.INotifyPropertyChanged {

		private System.Xml.XmlAttribute[] anyAttrField;

		private string valueField;

		/// <remarks/>
		[System.Xml.Serialization.XmlAnyAttributeAttribute()]
		public System.Xml.XmlAttribute[] AnyAttr {
			get {
				return this.anyAttrField;
			}
			set {
				this.anyAttrField = value;
				this.RaisePropertyChanged("AnyAttr");
			}
		}

		/// <remarks/>
		[System.Xml.Serialization.XmlTextAttribute(DataType = "anyURI")]
		public string Value {
			get {
				return this.valueField;
			}
			set {
				this.valueField = value;
				this.RaisePropertyChanged("Value");
			}
		}

		public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;

		protected void RaisePropertyChanged(string propertyName) {
			System.ComponentModel.PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
			if ((propertyChanged != null)) {
				propertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
			}
		}
	}

	/// <remarks/>
	[System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.0.30319.17929")]
	[System.SerializableAttribute()]
	[System.Diagnostics.DebuggerStepThroughAttribute()]
	[System.ComponentModel.DesignerCategoryAttribute("code")]
	[System.Xml.Serialization.XmlTypeAttribute(TypeName = "EndpointReferenceType", Namespace = "http://www.w3.org/2005/08/addressing")]
	public partial class EndpointReferenceType1 : object, System.ComponentModel.INotifyPropertyChanged {

		private AttributedURIType addressField;

		private ReferenceParametersType1 referenceParametersField;

		private MetadataType metadataField;

		private System.Xml.XmlElement[] anyField;

		private System.Xml.XmlAttribute[] anyAttrField;

		/// <remarks/>
		[XmlElement(Order = 0)]
		public AttributedURIType Address {
			get {
				return this.addressField;
			}
			set {
				this.addressField = value;
				this.RaisePropertyChanged("Address");
			}
		}

		/// <remarks/>
		[XmlElement(Order = 1)]
		public ReferenceParametersType1 ReferenceParameters {
			get {
				return this.referenceParametersField;
			}
			set {
				this.referenceParametersField = value;
				this.RaisePropertyChanged("ReferenceParameters");
			}
		}

		/// <remarks/>
		[XmlElement(Order = 2)]
		public MetadataType Metadata {
			get {
				return this.metadataField;
			}
			set {
				this.metadataField = value;
				this.RaisePropertyChanged("Metadata");
			}
		}

		/// <remarks/>
		[System.Xml.Serialization.XmlAnyElementAttribute(Order = 3)]
		public System.Xml.XmlElement[] Any {
			get {
				return this.anyField;
			}
			set {
				this.anyField = value;
				this.RaisePropertyChanged("Any");
			}
		}

		/// <remarks/>
		[System.Xml.Serialization.XmlAnyAttributeAttribute()]
		public System.Xml.XmlAttribute[] AnyAttr {
			get {
				return this.anyAttrField;
			}
			set {
				this.anyAttrField = value;
				this.RaisePropertyChanged("AnyAttr");
			}
		}

		public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;

		protected void RaisePropertyChanged(string propertyName) {
			System.ComponentModel.PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
			if ((propertyChanged != null)) {
				propertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
			}
		}
	}

	/// <remarks/>
	[System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.0.30319.17929")]
	[System.SerializableAttribute()]
	[System.Diagnostics.DebuggerStepThroughAttribute()]
	[System.ComponentModel.DesignerCategoryAttribute("code")]
	[System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://docs.oasis-open.org/wsn/b-2")]
	public partial class NotificationMessageHolderType : object, System.ComponentModel.INotifyPropertyChanged {

		private EndpointReferenceType1 subscriptionReferenceField;

		private TopicExpressionType topicField;

		private EndpointReferenceType1 producerReferenceField;

		private System.Xml.XmlElement messageField;

		/// <remarks/>
		[XmlElement(Order = 0)]
		public EndpointReferenceType1 SubscriptionReference {
			get {
				return this.subscriptionReferenceField;
			}
			set {
				this.subscriptionReferenceField = value;
				this.RaisePropertyChanged("SubscriptionReference");
			}
		}

		/// <remarks/>
		[XmlElement(Order = 1)]
		public TopicExpressionType Topic {
			get {
				return this.topicField;
			}
			set {
				this.topicField = value;
				this.RaisePropertyChanged("Topic");
			}
		}

		/// <remarks/>
		[XmlElement(Order = 2)]
		public EndpointReferenceType1 ProducerReference {
			get {
				return this.producerReferenceField;
			}
			set {
				this.producerReferenceField = value;
				this.RaisePropertyChanged("ProducerReference");
			}
		}

		/// <remarks/>
		[XmlElement(Order = 3)]
		public System.Xml.XmlElement Message {
			get {
				return this.messageField;
			}
			set {
				this.messageField = value;
				this.RaisePropertyChanged("Message");
			}
		}

		public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;

		protected void RaisePropertyChanged(string propertyName) {
			System.ComponentModel.PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
			if ((propertyChanged != null)) {
				propertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
			}
		}
	}

	/// <remarks/>
	[System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.0.30319.17929")]
	[System.SerializableAttribute()]
	[System.Diagnostics.DebuggerStepThroughAttribute()]
	[System.ComponentModel.DesignerCategoryAttribute("code")]
	[System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://schemas.xmlsoap.org/ws/2005/04/discovery")]
	public partial class ProbeMatchType : object, System.ComponentModel.INotifyPropertyChanged {

		private EndpointReferenceType endpointReferenceField;

		private string typesField;

		private ScopesType scopesField;

		private string xAddrsField;

		private uint metadataVersionField;

		private System.Xml.XmlElement[] anyField;

		private System.Xml.XmlAttribute[] anyAttrField;

		/// <remarks/>
		[XmlElement(Namespace = "http://schemas.xmlsoap.org/ws/2004/08/addressing", Order = 0)]
		public EndpointReferenceType EndpointReference {
			get {
				return this.endpointReferenceField;
			}
			set {
				this.endpointReferenceField = value;
				this.RaisePropertyChanged("EndpointReference");
			}
		}

		/// <remarks/>
		[XmlElement(Order = 1)]
		public string Types {
			get {
				return this.typesField;
			}
			set {
				this.typesField = value;
				this.RaisePropertyChanged("Types");
			}
		}

		/// <remarks/>
		[XmlElement(Order = 2)]
		public ScopesType Scopes {
			get {
				return this.scopesField;
			}
			set {
				this.scopesField = value;
				this.RaisePropertyChanged("Scopes");
			}
		}

		/// <remarks/>
		[XmlElement(Order = 3)]
		public string XAddrs {
			get {
				return this.xAddrsField;
			}
			set {
				this.xAddrsField = value;
				this.RaisePropertyChanged("XAddrs");
			}
		}

		/// <remarks/>
		[XmlElement(Order = 4)]
		public uint MetadataVersion {
			get {
				return this.metadataVersionField;
			}
			set {
				this.metadataVersionField = value;
				this.RaisePropertyChanged("MetadataVersion");
			}
		}

		/// <remarks/>
		[System.Xml.Serialization.XmlAnyElementAttribute(Order = 5)]
		public System.Xml.XmlElement[] Any {
			get {
				return this.anyField;
			}
			set {
				this.anyField = value;
				this.RaisePropertyChanged("Any");
			}
		}

		/// <remarks/>
		[System.Xml.Serialization.XmlAnyAttributeAttribute()]
		public System.Xml.XmlAttribute[] AnyAttr {
			get {
				return this.anyAttrField;
			}
			set {
				this.anyAttrField = value;
				this.RaisePropertyChanged("AnyAttr");
			}
		}

		public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;

		protected void RaisePropertyChanged(string propertyName) {
			System.ComponentModel.PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
			if ((propertyChanged != null)) {
				propertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
			}
		}
	}















	/// <remarks/>
	[System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.0.30319.17929")]
	[System.SerializableAttribute()]
	[System.Diagnostics.DebuggerStepThroughAttribute()]
	[System.ComponentModel.DesignerCategoryAttribute("code")]
	[System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://schemas.xmlsoap.org/ws/2004/08/addressing")]
	public partial class EndpointReferenceType : object, System.ComponentModel.INotifyPropertyChanged {

		private AttributedURI addressField;

		private ReferencePropertiesType referencePropertiesField;

		private ReferenceParametersType referenceParametersField;

		private AttributedQName portTypeField;

		private ServiceNameType serviceNameField;

		private System.Xml.XmlElement[] anyField;

		private System.Xml.XmlAttribute[] anyAttrField;

		/// <remarks/>
		[XmlElement(Order = 0)]
		public AttributedURI Address {
			get {
				return this.addressField;
			}
			set {
				this.addressField = value;
				this.RaisePropertyChanged("Address");
			}
		}

		/// <remarks/>
		[XmlElement(Order = 1)]
		public ReferencePropertiesType ReferenceProperties {
			get {
				return this.referencePropertiesField;
			}
			set {
				this.referencePropertiesField = value;
				this.RaisePropertyChanged("ReferenceProperties");
			}
		}

		/// <remarks/>
		[XmlElement(Order = 2)]
		public ReferenceParametersType ReferenceParameters {
			get {
				return this.referenceParametersField;
			}
			set {
				this.referenceParametersField = value;
				this.RaisePropertyChanged("ReferenceParameters");
			}
		}

		/// <remarks/>
		[XmlElement(Order = 3)]
		public AttributedQName PortType {
			get {
				return this.portTypeField;
			}
			set {
				this.portTypeField = value;
				this.RaisePropertyChanged("PortType");
			}
		}

		/// <remarks/>
		[XmlElement(Order = 4)]
		public ServiceNameType ServiceName {
			get {
				return this.serviceNameField;
			}
			set {
				this.serviceNameField = value;
				this.RaisePropertyChanged("ServiceName");
			}
		}

		/// <remarks/>
		[System.Xml.Serialization.XmlAnyElementAttribute(Order = 5)]
		public System.Xml.XmlElement[] Any {
			get {
				return this.anyField;
			}
			set {
				this.anyField = value;
				this.RaisePropertyChanged("Any");
			}
		}

		/// <remarks/>
		[System.Xml.Serialization.XmlAnyAttributeAttribute()]
		public System.Xml.XmlAttribute[] AnyAttr {
			get {
				return this.anyAttrField;
			}
			set {
				this.anyAttrField = value;
				this.RaisePropertyChanged("AnyAttr");
			}
		}

		public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;

		protected void RaisePropertyChanged(string propertyName) {
			System.ComponentModel.PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
			if ((propertyChanged != null)) {
				propertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
			}
		}
	}

	/// <remarks/>
	[System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.0.30319.17929")]
	[System.SerializableAttribute()]
	[System.Diagnostics.DebuggerStepThroughAttribute()]
	[System.ComponentModel.DesignerCategoryAttribute("code")]
	[System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://schemas.xmlsoap.org/ws/2004/08/addressing")]
	public partial class AttributedURI : object, System.ComponentModel.INotifyPropertyChanged {

		private System.Xml.XmlAttribute[] anyAttrField;

		private string valueField;

		/// <remarks/>
		[System.Xml.Serialization.XmlAnyAttributeAttribute()]
		public System.Xml.XmlAttribute[] AnyAttr {
			get {
				return this.anyAttrField;
			}
			set {
				this.anyAttrField = value;
				this.RaisePropertyChanged("AnyAttr");
			}
		}

		/// <remarks/>
		[System.Xml.Serialization.XmlTextAttribute(DataType = "anyURI")]
		public string Value {
			get {
				return this.valueField;
			}
			set {
				this.valueField = value;
				this.RaisePropertyChanged("Value");
			}
		}

		public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;

		protected void RaisePropertyChanged(string propertyName) {
			System.ComponentModel.PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
			if ((propertyChanged != null)) {
				propertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
			}
		}
	}

	/// <remarks/>
	[System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.0.30319.17929")]
	[System.SerializableAttribute()]
	[System.Diagnostics.DebuggerStepThroughAttribute()]
	[System.ComponentModel.DesignerCategoryAttribute("code")]
	[System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://schemas.xmlsoap.org/ws/2004/08/addressing")]
	public partial class ReferencePropertiesType : object, System.ComponentModel.INotifyPropertyChanged {

		private System.Xml.XmlElement[] anyField;

		/// <remarks/>
		[System.Xml.Serialization.XmlAnyElementAttribute(Order = 0)]
		public System.Xml.XmlElement[] Any {
			get {
				return this.anyField;
			}
			set {
				this.anyField = value;
				this.RaisePropertyChanged("Any");
			}
		}

		public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;

		protected void RaisePropertyChanged(string propertyName) {
			System.ComponentModel.PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
			if ((propertyChanged != null)) {
				propertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
			}
		}
	}

	/// <remarks/>
	[System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.0.30319.17929")]
	[System.SerializableAttribute()]
	[System.Diagnostics.DebuggerStepThroughAttribute()]
	[System.ComponentModel.DesignerCategoryAttribute("code")]
	[System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://schemas.xmlsoap.org/ws/2004/08/addressing")]
	public partial class ReferenceParametersType : object, System.ComponentModel.INotifyPropertyChanged {

		private System.Xml.XmlElement[] anyField;

		/// <remarks/>
		[System.Xml.Serialization.XmlAnyElementAttribute(Order = 0)]
		public System.Xml.XmlElement[] Any {
			get {
				return this.anyField;
			}
			set {
				this.anyField = value;
				this.RaisePropertyChanged("Any");
			}
		}

		public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;

		protected void RaisePropertyChanged(string propertyName) {
			System.ComponentModel.PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
			if ((propertyChanged != null)) {
				propertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
			}
		}
	}

	/// <remarks/>
	[System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.0.30319.17929")]
	[System.SerializableAttribute()]
	[System.Diagnostics.DebuggerStepThroughAttribute()]
	[System.ComponentModel.DesignerCategoryAttribute("code")]
	[System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://schemas.xmlsoap.org/ws/2004/08/addressing")]
	public partial class AttributedQName : object, System.ComponentModel.INotifyPropertyChanged {

		private System.Xml.XmlAttribute[] anyAttrField;

		private System.Xml.XmlQualifiedName valueField;

		/// <remarks/>
		[System.Xml.Serialization.XmlAnyAttributeAttribute()]
		public System.Xml.XmlAttribute[] AnyAttr {
			get {
				return this.anyAttrField;
			}
			set {
				this.anyAttrField = value;
				this.RaisePropertyChanged("AnyAttr");
			}
		}

		/// <remarks/>
		[System.Xml.Serialization.XmlTextAttribute()]
		public System.Xml.XmlQualifiedName Value {
			get {
				return this.valueField;
			}
			set {
				this.valueField = value;
				this.RaisePropertyChanged("Value");
			}
		}

		public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;

		protected void RaisePropertyChanged(string propertyName) {
			System.ComponentModel.PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
			if ((propertyChanged != null)) {
				propertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
			}
		}
	}

	/// <remarks/>
	[System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.0.30319.17929")]
	[System.SerializableAttribute()]
	[System.Diagnostics.DebuggerStepThroughAttribute()]
	[System.ComponentModel.DesignerCategoryAttribute("code")]
	[System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://schemas.xmlsoap.org/ws/2004/08/addressing")]
	public partial class ServiceNameType : object, System.ComponentModel.INotifyPropertyChanged {

		private string portNameField;

		private System.Xml.XmlAttribute[] anyAttrField;

		private System.Xml.XmlQualifiedName valueField;

		/// <remarks/>
		[System.Xml.Serialization.XmlAttributeAttribute(DataType = "NCName")]
		public string PortName {
			get {
				return this.portNameField;
			}
			set {
				this.portNameField = value;
				this.RaisePropertyChanged("PortName");
			}
		}

		/// <remarks/>
		[System.Xml.Serialization.XmlAnyAttributeAttribute()]
		public System.Xml.XmlAttribute[] AnyAttr {
			get {
				return this.anyAttrField;
			}
			set {
				this.anyAttrField = value;
				this.RaisePropertyChanged("AnyAttr");
			}
		}

		/// <remarks/>
		[System.Xml.Serialization.XmlTextAttribute()]
		public System.Xml.XmlQualifiedName Value {
			get {
				return this.valueField;
			}
			set {
				this.valueField = value;
				this.RaisePropertyChanged("Value");
			}
		}

		public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;

		protected void RaisePropertyChanged(string propertyName) {
			System.ComponentModel.PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
			if ((propertyChanged != null)) {
				propertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
			}
		}
	}

	/// <remarks/>
	[System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.0.30319.17929")]
	[System.SerializableAttribute()]
	[System.Diagnostics.DebuggerStepThroughAttribute()]
	[System.ComponentModel.DesignerCategoryAttribute("code")]
	[System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://schemas.xmlsoap.org/ws/2005/04/discovery")]
	public partial class ScopesType : object, System.ComponentModel.INotifyPropertyChanged {

		private string matchByField;

		private System.Xml.XmlAttribute[] anyAttrField;

		private string[] textField;

		/// <remarks/>
		[System.Xml.Serialization.XmlAttributeAttribute(DataType = "anyURI")]
		public string MatchBy {
			get {
				return this.matchByField;
			}
			set {
				this.matchByField = value;
				this.RaisePropertyChanged("MatchBy");
			}
		}

		/// <remarks/>
		[System.Xml.Serialization.XmlAnyAttributeAttribute()]
		public System.Xml.XmlAttribute[] AnyAttr {
			get {
				return this.anyAttrField;
			}
			set {
				this.anyAttrField = value;
				this.RaisePropertyChanged("AnyAttr");
			}
		}

		/// <remarks/>
		[System.Xml.Serialization.XmlTextAttribute(DataType = "anyURI")]
		public string[] Text {
			get {
				return this.textField;
			}
			set {
				this.textField = value;
				this.RaisePropertyChanged("Text");
			}
		}

		public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;

		protected void RaisePropertyChanged(string propertyName) {
			System.ComponentModel.PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
			if ((propertyChanged != null)) {
				propertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
			}
		}
	}


	/// <remarks/>
	[System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.0.30319.17929")]
	[System.SerializableAttribute()]
	[System.Diagnostics.DebuggerStepThroughAttribute()]
	[System.ComponentModel.DesignerCategoryAttribute("code")]
	[System.Xml.Serialization.XmlTypeAttribute(TypeName = "Capabilities", Namespace = "http://www.onvif.org/ver10/recording/wsdl")]
	public partial class Capabilities7 : object, System.ComponentModel.INotifyPropertyChanged {

		private System.Xml.XmlElement[] anyField;

		private bool dynamicRecordingsField;

		private bool dynamicRecordingsFieldSpecified;

		private bool dynamicTracksField;

		private bool dynamicTracksFieldSpecified;

		private string[] encodingField;

		private float maxRateField;

		private bool maxRateFieldSpecified;

		private float maxTotalRateField;

		private bool maxTotalRateFieldSpecified;

		private float maxRecordingsField;

		private bool maxRecordingsFieldSpecified;

		private System.Xml.XmlAttribute[] anyAttrField;

		/// <remarks/>
		[System.Xml.Serialization.XmlAnyElementAttribute(Order = 0)]
		public System.Xml.XmlElement[] Any {
			get {
				return this.anyField;
			}
			set {
				this.anyField = value;
				this.RaisePropertyChanged("Any");
			}
		}

		/// <remarks/>
		[System.Xml.Serialization.XmlAttributeAttribute()]
		public bool DynamicRecordings {
			get {
				return this.dynamicRecordingsField;
			}
			set {
				this.dynamicRecordingsField = value;
				this.RaisePropertyChanged("DynamicRecordings");
			}
		}

		/// <remarks/>
		[System.Xml.Serialization.XmlIgnoreAttribute()]
		public bool DynamicRecordingsSpecified {
			get {
				return this.dynamicRecordingsFieldSpecified;
			}
			set {
				this.dynamicRecordingsFieldSpecified = value;
				this.RaisePropertyChanged("DynamicRecordingsSpecified");
			}
		}

		/// <remarks/>
		[System.Xml.Serialization.XmlAttributeAttribute()]
		public bool DynamicTracks {
			get {
				return this.dynamicTracksField;
			}
			set {
				this.dynamicTracksField = value;
				this.RaisePropertyChanged("DynamicTracks");
			}
		}

		/// <remarks/>
		[System.Xml.Serialization.XmlIgnoreAttribute()]
		public bool DynamicTracksSpecified {
			get {
				return this.dynamicTracksFieldSpecified;
			}
			set {
				this.dynamicTracksFieldSpecified = value;
				this.RaisePropertyChanged("DynamicTracksSpecified");
			}
		}

		/// <remarks/>
		[System.Xml.Serialization.XmlAttributeAttribute()]
		public string[] Encoding {
			get {
				return this.encodingField;
			}
			set {
				this.encodingField = value;
				this.RaisePropertyChanged("Encoding");
			}
		}

		/// <remarks/>
		[System.Xml.Serialization.XmlAttributeAttribute()]
		public float MaxRate {
			get {
				return this.maxRateField;
			}
			set {
				this.maxRateField = value;
				this.RaisePropertyChanged("MaxRate");
			}
		}

		/// <remarks/>
		[System.Xml.Serialization.XmlIgnoreAttribute()]
		public bool MaxRateSpecified {
			get {
				return this.maxRateFieldSpecified;
			}
			set {
				this.maxRateFieldSpecified = value;
				this.RaisePropertyChanged("MaxRateSpecified");
			}
		}

		/// <remarks/>
		[System.Xml.Serialization.XmlAttributeAttribute()]
		public float MaxTotalRate {
			get {
				return this.maxTotalRateField;
			}
			set {
				this.maxTotalRateField = value;
				this.RaisePropertyChanged("MaxTotalRate");
			}
		}

		/// <remarks/>
		[System.Xml.Serialization.XmlIgnoreAttribute()]
		public bool MaxTotalRateSpecified {
			get {
				return this.maxTotalRateFieldSpecified;
			}
			set {
				this.maxTotalRateFieldSpecified = value;
				this.RaisePropertyChanged("MaxTotalRateSpecified");
			}
		}

		/// <remarks/>
		[System.Xml.Serialization.XmlAttributeAttribute()]
		public float MaxRecordings {
			get {
				return this.maxRecordingsField;
			}
			set {
				this.maxRecordingsField = value;
				this.RaisePropertyChanged("MaxRecordings");
			}
		}

		/// <remarks/>
		[System.Xml.Serialization.XmlIgnoreAttribute()]
		public bool MaxRecordingsSpecified {
			get {
				return this.maxRecordingsFieldSpecified;
			}
			set {
				this.maxRecordingsFieldSpecified = value;
				this.RaisePropertyChanged("MaxRecordingsSpecified");
			}
		}

		/// <remarks/>
		[System.Xml.Serialization.XmlAnyAttributeAttribute()]
		public System.Xml.XmlAttribute[] AnyAttr {
			get {
				return this.anyAttrField;
			}
			set {
				this.anyAttrField = value;
				this.RaisePropertyChanged("AnyAttr");
			}
		}

		public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;

		protected void RaisePropertyChanged(string propertyName) {
			System.ComponentModel.PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
			if ((propertyChanged != null)) {
				propertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
			}
		}
	}














	///// <remarks/>
	//[System.Xml.Serialization.XmlIncludeAttribute(typeof(EventFilter))]
	//[System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.0.30319.17929")]
	//[System.SerializableAttribute()]
	//[System.Diagnostics.DebuggerStepThroughAttribute()]
	//[System.ComponentModel.DesignerCategoryAttribute("code")]
	//[System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://docs.oasis-open.org/wsn/b-2")]
	//public partial class FilterType : object, System.ComponentModel.INotifyPropertyChanged {

	//	private System.Xml.XmlElement[] anyField;

	//	/// <remarks/>
	//	[System.Xml.Serialization.XmlAnyElementAttribute(Order = 0)]
	//	public System.Xml.XmlElement[] Any {
	//		get {
	//			return this.anyField;
	//		}
	//		set {
	//			this.anyField = value;
	//			this.RaisePropertyChanged("Any");
	//		}
	//	}

	//	public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;

	//	protected void RaisePropertyChanged(string propertyName) {
	//		System.ComponentModel.PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
	//		if ((propertyChanged != null)) {
	//			propertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
	//		}
	//	}
	//}

	/// <remarks/>












	/// <remarks/>
	[System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.0.30319.17929")]
	[System.SerializableAttribute()]
	[System.Diagnostics.DebuggerStepThroughAttribute()]
	[System.ComponentModel.DesignerCategoryAttribute("code")]
	[System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.onvif.org/ver10/deviceIO/wsdl")]
	public partial class RelayOutputOptionsExtension : object, System.ComponentModel.INotifyPropertyChanged {

		private System.Xml.XmlElement[] anyField;

		/// <remarks/>
		[System.Xml.Serialization.XmlAnyElementAttribute(Order = 0)]
		public System.Xml.XmlElement[] Any {
			get {
				return this.anyField;
			}
			set {
				this.anyField = value;
				this.RaisePropertyChanged("Any");
			}
		}

		public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;

		protected void RaisePropertyChanged(string propertyName) {
			System.ComponentModel.PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
			if ((propertyChanged != null)) {
				propertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
			}
		}
	}

	/// <remarks/>
	[System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.0.30319.17929")]
	[System.SerializableAttribute()]
	[System.Diagnostics.DebuggerStepThroughAttribute()]
	[System.ComponentModel.DesignerCategoryAttribute("code")]
	[System.Xml.Serialization.XmlTypeAttribute(TypeName = "Capabilities", Namespace = "http://www.onvif.org/ver10/deviceIO/wsdl")]
	public partial class Capabilities4 : object, System.ComponentModel.INotifyPropertyChanged {

		private System.Xml.XmlElement[] anyField;

		private int videoSourcesField;

		private int videoOutputsField;

		private int audioSourcesField;

		private int audioOutputsField;

		private int relayOutputsField;


		private System.Xml.XmlAttribute[] anyAttrField;

		public Capabilities4() {
			this.videoSourcesField = 0;
			this.videoOutputsField = 0;
			this.audioSourcesField = 0;
			this.audioOutputsField = 0;
			this.relayOutputsField = 0;
		}

		/// <remarks/>
		[System.Xml.Serialization.XmlAnyElementAttribute(Order = 0)]
		public System.Xml.XmlElement[] Any {
			get {
				return this.anyField;
			}
			set {
				this.anyField = value;
				this.RaisePropertyChanged("Any");
			}
		}

		/// <remarks/>
		[System.Xml.Serialization.XmlAttributeAttribute()]
		[System.ComponentModel.DefaultValueAttribute(0)]
		public int VideoSources {
			get {
				return this.videoSourcesField;
			}
			set {
				this.videoSourcesField = value;
				this.RaisePropertyChanged("VideoSources");
			}
		}

		/// <remarks/>
		[System.Xml.Serialization.XmlAttributeAttribute()]
		[System.ComponentModel.DefaultValueAttribute(0)]
		public int VideoOutputs {
			get {
				return this.videoOutputsField;
			}
			set {
				this.videoOutputsField = value;
				this.RaisePropertyChanged("VideoOutputs");
			}
		}

		/// <remarks/>
		[System.Xml.Serialization.XmlAttributeAttribute()]
		[System.ComponentModel.DefaultValueAttribute(0)]
		public int AudioSources {
			get {
				return this.audioSourcesField;
			}
			set {
				this.audioSourcesField = value;
				this.RaisePropertyChanged("AudioSources");
			}
		}

		/// <remarks/>
		[System.Xml.Serialization.XmlAttributeAttribute()]
		[System.ComponentModel.DefaultValueAttribute(0)]
		public int AudioOutputs {
			get {
				return this.audioOutputsField;
			}
			set {
				this.audioOutputsField = value;
				this.RaisePropertyChanged("AudioOutputs");
			}
		}

		/// <remarks/>
		[System.Xml.Serialization.XmlAttributeAttribute()]
		[System.ComponentModel.DefaultValueAttribute(0)]
		public int RelayOutputs {
			get {
				return this.relayOutputsField;
			}
			set {
				this.relayOutputsField = value;
				this.RaisePropertyChanged("RelayOutputs");
			}
		}


		/// <remarks/>
		[System.Xml.Serialization.XmlAnyAttributeAttribute()]
		public System.Xml.XmlAttribute[] AnyAttr {
			get {
				return this.anyAttrField;
			}
			set {
				this.anyAttrField = value;
				this.RaisePropertyChanged("AnyAttr");
			}
		}

		public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;

		protected void RaisePropertyChanged(string propertyName) {
			System.ComponentModel.PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
			if ((propertyChanged != null)) {
				propertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
			}
		}
	}

	/// <remarks/>
	[System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.0.30319.17929")]
	[System.SerializableAttribute()]
	[System.Diagnostics.DebuggerStepThroughAttribute()]
	[System.ComponentModel.DesignerCategoryAttribute("code")]
	[System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://docs.oasis-open.org/wsrf/r-2")]
	public partial class ResourceUnknownFaultType : BaseFaultType {
	}

	/// <remarks/>
	[System.Xml.Serialization.XmlIncludeAttribute(typeof(ResourceUnavailableFaultType))]
	[System.Xml.Serialization.XmlIncludeAttribute(typeof(ResourceUnknownFaultType))]
	[System.Xml.Serialization.XmlIncludeAttribute(typeof(ResumeFailedFaultType))]
	[System.Xml.Serialization.XmlIncludeAttribute(typeof(PauseFailedFaultType))]
	[System.Xml.Serialization.XmlIncludeAttribute(typeof(UnableToDestroySubscriptionFaultType))]
	[System.Xml.Serialization.XmlIncludeAttribute(typeof(UnacceptableTerminationTimeFaultType))]
	[System.Xml.Serialization.XmlIncludeAttribute(typeof(UnableToCreatePullPointFaultType))]
	[System.Xml.Serialization.XmlIncludeAttribute(typeof(UnableToDestroyPullPointFaultType))]
	[System.Xml.Serialization.XmlIncludeAttribute(typeof(UnableToGetMessagesFaultType))]
	[System.Xml.Serialization.XmlIncludeAttribute(typeof(NoCurrentMessageOnTopicFaultType))]
	[System.Xml.Serialization.XmlIncludeAttribute(typeof(UnacceptableInitialTerminationTimeFaultType))]
	[System.Xml.Serialization.XmlIncludeAttribute(typeof(NotifyMessageNotSupportedFaultType))]
	[System.Xml.Serialization.XmlIncludeAttribute(typeof(UnsupportedPolicyRequestFaultType))]
	[System.Xml.Serialization.XmlIncludeAttribute(typeof(UnrecognizedPolicyRequestFaultType))]
	[System.Xml.Serialization.XmlIncludeAttribute(typeof(InvalidMessageContentExpressionFaultType))]
	[System.Xml.Serialization.XmlIncludeAttribute(typeof(InvalidProducerPropertiesExpressionFaultType))]
	[System.Xml.Serialization.XmlIncludeAttribute(typeof(MultipleTopicsSpecifiedFaultType))]
	[System.Xml.Serialization.XmlIncludeAttribute(typeof(TopicNotSupportedFaultType))]
	[System.Xml.Serialization.XmlIncludeAttribute(typeof(InvalidTopicExpressionFaultType))]
	[System.Xml.Serialization.XmlIncludeAttribute(typeof(TopicExpressionDialectUnknownFaultType))]
	[System.Xml.Serialization.XmlIncludeAttribute(typeof(InvalidFilterFaultType))]
	[System.Xml.Serialization.XmlIncludeAttribute(typeof(SubscribeCreationFailedFaultType))]
	[System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.0.30319.17929")]
	[System.SerializableAttribute()]
	[System.Diagnostics.DebuggerStepThroughAttribute()]
	[System.ComponentModel.DesignerCategoryAttribute("code")]
	[System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://docs.oasis-open.org/wsrf/bf-2")]
	public partial class BaseFaultType : object, System.ComponentModel.INotifyPropertyChanged {

		private System.Xml.XmlElement[] anyField;

		private System.DateTime timestampField;

		private EndpointReferenceType1 originatorField;

		private BaseFaultTypeErrorCode errorCodeField;

		private BaseFaultTypeDescription[] descriptionField;

		private System.Xml.XmlElement faultCauseField;

		private System.Xml.XmlAttribute[] anyAttrField;

		/// <remarks/>
		[System.Xml.Serialization.XmlAnyElementAttribute(Order = 0)]
		public System.Xml.XmlElement[] Any {
			get {
				return this.anyField;
			}
			set {
				this.anyField = value;
				this.RaisePropertyChanged("Any");
			}
		}

		/// <remarks/>
		[XmlElement(Order = 1)]
		public System.DateTime Timestamp {
			get {
				return this.timestampField;
			}
			set {
				this.timestampField = value;
				this.RaisePropertyChanged("Timestamp");
			}
		}

		/// <remarks/>
		[XmlElement(Order = 2)]
		public EndpointReferenceType1 Originator {
			get {
				return this.originatorField;
			}
			set {
				this.originatorField = value;
				this.RaisePropertyChanged("Originator");
			}
		}

		/// <remarks/>
		[XmlElement(Order = 3)]
		public BaseFaultTypeErrorCode ErrorCode {
			get {
				return this.errorCodeField;
			}
			set {
				this.errorCodeField = value;
				this.RaisePropertyChanged("ErrorCode");
			}
		}

		/// <remarks/>
		[XmlElement("Description", Order = 4)]
		public BaseFaultTypeDescription[] Description {
			get {
				return this.descriptionField;
			}
			set {
				this.descriptionField = value;
				this.RaisePropertyChanged("Description");
			}
		}

		/// <remarks/>
		[XmlElement(Order = 5)]
		public System.Xml.XmlElement FaultCause {
			get {
				return this.faultCauseField;
			}
			set {
				this.faultCauseField = value;
				this.RaisePropertyChanged("FaultCause");
			}
		}

		/// <remarks/>
		[System.Xml.Serialization.XmlAnyAttributeAttribute()]
		public System.Xml.XmlAttribute[] AnyAttr {
			get {
				return this.anyAttrField;
			}
			set {
				this.anyAttrField = value;
				this.RaisePropertyChanged("AnyAttr");
			}
		}

		public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;

		protected void RaisePropertyChanged(string propertyName) {
			System.ComponentModel.PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
			if ((propertyChanged != null)) {
				propertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
			}
		}
	}

	/// <remarks/>
	[System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.0.30319.17929")]
	[System.SerializableAttribute()]
	[System.Diagnostics.DebuggerStepThroughAttribute()]
	[System.ComponentModel.DesignerCategoryAttribute("code")]
	[System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://docs.oasis-open.org/wsrf/bf-2")]
	public partial class BaseFaultTypeErrorCode : object, System.ComponentModel.INotifyPropertyChanged {

		private string dialectField;

		private string[] textField;

		/// <remarks/>
		[System.Xml.Serialization.XmlAttributeAttribute(DataType = "anyURI")]
		public string dialect {
			get {
				return this.dialectField;
			}
			set {
				this.dialectField = value;
				this.RaisePropertyChanged("dialect");
			}
		}

		/// <remarks/>
		[System.Xml.Serialization.XmlTextAttribute()]
		public string[] Text {
			get {
				return this.textField;
			}
			set {
				this.textField = value;
				this.RaisePropertyChanged("Text");
			}
		}

		public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;

		protected void RaisePropertyChanged(string propertyName) {
			System.ComponentModel.PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
			if ((propertyChanged != null)) {
				propertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
			}
		}
	}

	/// <remarks/>
	[System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.0.30319.17929")]
	[System.SerializableAttribute()]
	[System.Diagnostics.DebuggerStepThroughAttribute()]
	[System.ComponentModel.DesignerCategoryAttribute("code")]
	[System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://docs.oasis-open.org/wsrf/bf-2")]
	public partial class BaseFaultTypeDescription : object, System.ComponentModel.INotifyPropertyChanged {

		private string langField;

		private string valueField;

		/// <remarks/>
		[System.Xml.Serialization.XmlAttributeAttribute(Form = System.Xml.Schema.XmlSchemaForm.Qualified, Namespace = "http://www.w3.org/XML/1998/namespace")]
		public string lang {
			get {
				return this.langField;
			}
			set {
				this.langField = value;
				this.RaisePropertyChanged("lang");
			}
		}

		/// <remarks/>
		[System.Xml.Serialization.XmlTextAttribute()]
		public string Value {
			get {
				return this.valueField;
			}
			set {
				this.valueField = value;
				this.RaisePropertyChanged("Value");
			}
		}

		public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;

		protected void RaisePropertyChanged(string propertyName) {
			System.ComponentModel.PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
			if ((propertyChanged != null)) {
				propertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
			}
		}
	}

	/// <remarks/>
	[System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.0.30319.17929")]
	[System.SerializableAttribute()]
	[System.Diagnostics.DebuggerStepThroughAttribute()]
	[System.ComponentModel.DesignerCategoryAttribute("code")]
	[System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://docs.oasis-open.org/wsrf/r-2")]
	public partial class ResourceUnavailableFaultType : BaseFaultType {
	}

	/// <remarks/>
	[System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.0.30319.17929")]
	[System.SerializableAttribute()]
	[System.Diagnostics.DebuggerStepThroughAttribute()]
	[System.ComponentModel.DesignerCategoryAttribute("code")]
	[System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://docs.oasis-open.org/wsn/b-2")]
	public partial class ResumeFailedFaultType : BaseFaultType {
	}

	/// <remarks/>
	[System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.0.30319.17929")]
	[System.SerializableAttribute()]
	[System.Diagnostics.DebuggerStepThroughAttribute()]
	[System.ComponentModel.DesignerCategoryAttribute("code")]
	[System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://docs.oasis-open.org/wsn/b-2")]
	public partial class PauseFailedFaultType : BaseFaultType {
	}

	/// <remarks/>
	[System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.0.30319.17929")]
	[System.SerializableAttribute()]
	[System.Diagnostics.DebuggerStepThroughAttribute()]
	[System.ComponentModel.DesignerCategoryAttribute("code")]
	[System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://docs.oasis-open.org/wsn/b-2")]
	public partial class UnableToDestroySubscriptionFaultType : BaseFaultType {
	}

	/// <remarks/>
	[System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.0.30319.17929")]
	[System.SerializableAttribute()]
	[System.Diagnostics.DebuggerStepThroughAttribute()]
	[System.ComponentModel.DesignerCategoryAttribute("code")]
	[System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://docs.oasis-open.org/wsn/b-2")]
	public partial class UnacceptableTerminationTimeFaultType : BaseFaultType {

		private System.DateTime minimumTimeField;

		private System.DateTime maximumTimeField;

		private bool maximumTimeFieldSpecified;

		/// <remarks/>
		[XmlElement(Order = 0)]
		public System.DateTime MinimumTime {
			get {
				return this.minimumTimeField;
			}
			set {
				this.minimumTimeField = value;
				this.RaisePropertyChanged("MinimumTime");
			}
		}

		/// <remarks/>
		[XmlElement(Order = 1)]
		public System.DateTime MaximumTime {
			get {
				return this.maximumTimeField;
			}
			set {
				this.maximumTimeField = value;
				this.RaisePropertyChanged("MaximumTime");
			}
		}

		/// <remarks/>
		[System.Xml.Serialization.XmlIgnoreAttribute()]
		public bool MaximumTimeSpecified {
			get {
				return this.maximumTimeFieldSpecified;
			}
			set {
				this.maximumTimeFieldSpecified = value;
				this.RaisePropertyChanged("MaximumTimeSpecified");
			}
		}
	}

	/// <remarks/>
	[System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.0.30319.17929")]
	[System.SerializableAttribute()]
	[System.Diagnostics.DebuggerStepThroughAttribute()]
	[System.ComponentModel.DesignerCategoryAttribute("code")]
	[System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://docs.oasis-open.org/wsn/b-2")]
	public partial class UnableToCreatePullPointFaultType : BaseFaultType {
	}

	/// <remarks/>
	[System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.0.30319.17929")]
	[System.SerializableAttribute()]
	[System.Diagnostics.DebuggerStepThroughAttribute()]
	[System.ComponentModel.DesignerCategoryAttribute("code")]
	[System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://docs.oasis-open.org/wsn/b-2")]
	public partial class UnableToDestroyPullPointFaultType : BaseFaultType {
	}

	/// <remarks/>
	[System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.0.30319.17929")]
	[System.SerializableAttribute()]
	[System.Diagnostics.DebuggerStepThroughAttribute()]
	[System.ComponentModel.DesignerCategoryAttribute("code")]
	[System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://docs.oasis-open.org/wsn/b-2")]
	public partial class UnableToGetMessagesFaultType : BaseFaultType {
	}

	/// <remarks/>
	[System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.0.30319.17929")]
	[System.SerializableAttribute()]
	[System.Diagnostics.DebuggerStepThroughAttribute()]
	[System.ComponentModel.DesignerCategoryAttribute("code")]
	[System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://docs.oasis-open.org/wsn/b-2")]
	public partial class NoCurrentMessageOnTopicFaultType : BaseFaultType {
	}

	/// <remarks/>
	[System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.0.30319.17929")]
	[System.SerializableAttribute()]
	[System.Diagnostics.DebuggerStepThroughAttribute()]
	[System.ComponentModel.DesignerCategoryAttribute("code")]
	[System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://docs.oasis-open.org/wsn/b-2")]
	public partial class UnacceptableInitialTerminationTimeFaultType : BaseFaultType {

		private System.DateTime minimumTimeField;

		private System.DateTime maximumTimeField;

		private bool maximumTimeFieldSpecified;

		/// <remarks/>
		[XmlElement(Order = 0)]
		public System.DateTime MinimumTime {
			get {
				return this.minimumTimeField;
			}
			set {
				this.minimumTimeField = value;
				this.RaisePropertyChanged("MinimumTime");
			}
		}

		/// <remarks/>
		[XmlElement(Order = 1)]
		public System.DateTime MaximumTime {
			get {
				return this.maximumTimeField;
			}
			set {
				this.maximumTimeField = value;
				this.RaisePropertyChanged("MaximumTime");
			}
		}

		/// <remarks/>
		[System.Xml.Serialization.XmlIgnoreAttribute()]
		public bool MaximumTimeSpecified {
			get {
				return this.maximumTimeFieldSpecified;
			}
			set {
				this.maximumTimeFieldSpecified = value;
				this.RaisePropertyChanged("MaximumTimeSpecified");
			}
		}
	}

	/// <remarks/>
	[System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.0.30319.17929")]
	[System.SerializableAttribute()]
	[System.Diagnostics.DebuggerStepThroughAttribute()]
	[System.ComponentModel.DesignerCategoryAttribute("code")]
	[System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://docs.oasis-open.org/wsn/b-2")]
	public partial class NotifyMessageNotSupportedFaultType : BaseFaultType {
	}

	/// <remarks/>
	[System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.0.30319.17929")]
	[System.SerializableAttribute()]
	[System.Diagnostics.DebuggerStepThroughAttribute()]
	[System.ComponentModel.DesignerCategoryAttribute("code")]
	[System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://docs.oasis-open.org/wsn/b-2")]
	public partial class UnsupportedPolicyRequestFaultType : BaseFaultType {

		private System.Xml.XmlQualifiedName[] unsupportedPolicyField;

		/// <remarks/>
		[XmlElement("UnsupportedPolicy", Order = 0)]
		public System.Xml.XmlQualifiedName[] UnsupportedPolicy {
			get {
				return this.unsupportedPolicyField;
			}
			set {
				this.unsupportedPolicyField = value;
				this.RaisePropertyChanged("UnsupportedPolicy");
			}
		}
	}

	/// <remarks/>
	[System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.0.30319.17929")]
	[System.SerializableAttribute()]
	[System.Diagnostics.DebuggerStepThroughAttribute()]
	[System.ComponentModel.DesignerCategoryAttribute("code")]
	[System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://docs.oasis-open.org/wsn/b-2")]
	public partial class UnrecognizedPolicyRequestFaultType : BaseFaultType {

		private System.Xml.XmlQualifiedName[] unrecognizedPolicyField;

		/// <remarks/>
		[XmlElement("UnrecognizedPolicy", Order = 0)]
		public System.Xml.XmlQualifiedName[] UnrecognizedPolicy {
			get {
				return this.unrecognizedPolicyField;
			}
			set {
				this.unrecognizedPolicyField = value;
				this.RaisePropertyChanged("UnrecognizedPolicy");
			}
		}
	}

	/// <remarks/>
	[System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.0.30319.17929")]
	[System.SerializableAttribute()]
	[System.Diagnostics.DebuggerStepThroughAttribute()]
	[System.ComponentModel.DesignerCategoryAttribute("code")]
	[System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://docs.oasis-open.org/wsn/b-2")]
	public partial class InvalidMessageContentExpressionFaultType : BaseFaultType {
	}

	/// <remarks/>
	[System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.0.30319.17929")]
	[System.SerializableAttribute()]
	[System.Diagnostics.DebuggerStepThroughAttribute()]
	[System.ComponentModel.DesignerCategoryAttribute("code")]
	[System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://docs.oasis-open.org/wsn/b-2")]
	public partial class InvalidProducerPropertiesExpressionFaultType : BaseFaultType {
	}

	/// <remarks/>
	[System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.0.30319.17929")]
	[System.SerializableAttribute()]
	[System.Diagnostics.DebuggerStepThroughAttribute()]
	[System.ComponentModel.DesignerCategoryAttribute("code")]
	[System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://docs.oasis-open.org/wsn/b-2")]
	public partial class MultipleTopicsSpecifiedFaultType : BaseFaultType {
	}

	/// <remarks/>
	[System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.0.30319.17929")]
	[System.SerializableAttribute()]
	[System.Diagnostics.DebuggerStepThroughAttribute()]
	[System.ComponentModel.DesignerCategoryAttribute("code")]
	[System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://docs.oasis-open.org/wsn/b-2")]
	public partial class TopicNotSupportedFaultType : BaseFaultType {
	}

	/// <remarks/>
	[System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.0.30319.17929")]
	[System.SerializableAttribute()]
	[System.Diagnostics.DebuggerStepThroughAttribute()]
	[System.ComponentModel.DesignerCategoryAttribute("code")]
	[System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://docs.oasis-open.org/wsn/b-2")]
	public partial class InvalidTopicExpressionFaultType : BaseFaultType {
	}

	/// <remarks/>
	[System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.0.30319.17929")]
	[System.SerializableAttribute()]
	[System.Diagnostics.DebuggerStepThroughAttribute()]
	[System.ComponentModel.DesignerCategoryAttribute("code")]
	[System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://docs.oasis-open.org/wsn/b-2")]
	public partial class TopicExpressionDialectUnknownFaultType : BaseFaultType {
	}

	/// <remarks/>
	[System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.0.30319.17929")]
	[System.SerializableAttribute()]
	[System.Diagnostics.DebuggerStepThroughAttribute()]
	[System.ComponentModel.DesignerCategoryAttribute("code")]
	[System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://docs.oasis-open.org/wsn/b-2")]
	public partial class InvalidFilterFaultType : BaseFaultType {

		private System.Xml.XmlQualifiedName[] unknownFilterField;

		/// <remarks/>
		[XmlElement("UnknownFilter", Order = 0)]
		public System.Xml.XmlQualifiedName[] UnknownFilter {
			get {
				return this.unknownFilterField;
			}
			set {
				this.unknownFilterField = value;
				this.RaisePropertyChanged("UnknownFilter");
			}
		}
	}

	/// <remarks/>
	[System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.0.30319.17929")]
	[System.SerializableAttribute()]
	[System.Diagnostics.DebuggerStepThroughAttribute()]
	[System.ComponentModel.DesignerCategoryAttribute("code")]
	[System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://docs.oasis-open.org/wsn/b-2")]
	public partial class SubscribeCreationFailedFaultType : BaseFaultType {
	}


	/// <remarks/>
	[System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.0.30319.17929")]
	[System.SerializableAttribute()]
	[System.Diagnostics.DebuggerStepThroughAttribute()]
	[System.ComponentModel.DesignerCategoryAttribute("code")]
	[System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://docs.oasis-open.org/wsn/b-2")]
	public partial class Notify : object, System.ComponentModel.INotifyPropertyChanged {

		private NotificationMessageHolderType[] notificationMessageField;

		private System.Xml.XmlElement[] anyField;

		/// <remarks/>
		[XmlElement("NotificationMessage", Order = 0)]
		public NotificationMessageHolderType[] NotificationMessage {
			get {
				return this.notificationMessageField;
			}
			set {
				this.notificationMessageField = value;
				this.RaisePropertyChanged("NotificationMessage");
			}
		}

		/// <remarks/>
		[System.Xml.Serialization.XmlAnyElementAttribute(Order = 1)]
		public System.Xml.XmlElement[] Any {
			get {
				return this.anyField;
			}
			set {
				this.anyField = value;
				this.RaisePropertyChanged("Any");
			}
		}

		public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;

		protected void RaisePropertyChanged(string propertyName) {
			System.ComponentModel.PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
			if ((propertyChanged != null)) {
				propertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
			}
		}
	}

	/// <remarks/>
	[System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.0.30319.17929")]
	[System.SerializableAttribute()]
	[System.Diagnostics.DebuggerStepThroughAttribute()]
	[System.ComponentModel.DesignerCategoryAttribute("code")]
	[System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://docs.oasis-open.org/wsn/b-2")]
	public partial class Subscribe : object, System.ComponentModel.INotifyPropertyChanged {

		private EndpointReferenceType1 consumerReferenceField;

		private FilterType filterField;

		private string initialTerminationTimeField;

		private SubscribeSubscriptionPolicy subscriptionPolicyField;

		private System.Xml.XmlElement[] anyField;

		/// <remarks/>
		[XmlElement(Order = 0)]
		public EndpointReferenceType1 ConsumerReference {
			get {
				return this.consumerReferenceField;
			}
			set {
				this.consumerReferenceField = value;
				this.RaisePropertyChanged("ConsumerReference");
			}
		}

		/// <remarks/>
		[XmlElement(Order = 1)]
		public FilterType Filter {
			get {
				return this.filterField;
			}
			set {
				this.filterField = value;
				this.RaisePropertyChanged("Filter");
			}
		}

		/// <remarks/>
		[XmlElement(IsNullable = true, Order = 2)]
		public string InitialTerminationTime {
			get {
				return this.initialTerminationTimeField;
			}
			set {
				this.initialTerminationTimeField = value;
				this.RaisePropertyChanged("InitialTerminationTime");
			}
		}

		/// <remarks/>
		[XmlElement(Order = 3)]
		public SubscribeSubscriptionPolicy SubscriptionPolicy {
			get {
				return this.subscriptionPolicyField;
			}
			set {
				this.subscriptionPolicyField = value;
				this.RaisePropertyChanged("SubscriptionPolicy");
			}
		}

		/// <remarks/>
		[System.Xml.Serialization.XmlAnyElementAttribute(Order = 4)]
		public System.Xml.XmlElement[] Any {
			get {
				return this.anyField;
			}
			set {
				this.anyField = value;
				this.RaisePropertyChanged("Any");
			}
		}

		public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;

		protected void RaisePropertyChanged(string propertyName) {
			System.ComponentModel.PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
			if ((propertyChanged != null)) {
				propertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
			}
		}
	}

	/// <remarks/>
	[System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.0.30319.17929")]
	[System.SerializableAttribute()]
	[System.Diagnostics.DebuggerStepThroughAttribute()]
	[System.ComponentModel.DesignerCategoryAttribute("code")]
	[System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://docs.oasis-open.org/wsn/b-2")]
	public partial class SubscribeSubscriptionPolicy : object, System.ComponentModel.INotifyPropertyChanged {

		private System.Xml.XmlElement[] anyField;

		/// <remarks/>
		[System.Xml.Serialization.XmlAnyElementAttribute(Order = 0)]
		public System.Xml.XmlElement[] Any {
			get {
				return this.anyField;
			}
			set {
				this.anyField = value;
				this.RaisePropertyChanged("Any");
			}
		}

		public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;

		protected void RaisePropertyChanged(string propertyName) {
			System.ComponentModel.PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
			if ((propertyChanged != null)) {
				propertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
			}
		}
	}

	/// <remarks/>
	[System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.0.30319.17929")]
	[System.SerializableAttribute()]
	[System.Diagnostics.DebuggerStepThroughAttribute()]
	[System.ComponentModel.DesignerCategoryAttribute("code")]
	[System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://docs.oasis-open.org/wsn/b-2")]
	public partial class SubscribeResponse : object, System.ComponentModel.INotifyPropertyChanged {

		private EndpointReferenceType1 subscriptionReferenceField;

		private System.DateTime currentTimeField;

		private bool currentTimeFieldSpecified;

		private System.Nullable<System.DateTime> terminationTimeField;

		private bool terminationTimeFieldSpecified;

		private System.Xml.XmlElement[] anyField;

		/// <remarks/>
		[XmlElement(Order = 0)]
		public EndpointReferenceType1 SubscriptionReference {
			get {
				return this.subscriptionReferenceField;
			}
			set {
				this.subscriptionReferenceField = value;
				this.RaisePropertyChanged("SubscriptionReference");
			}
		}

		/// <remarks/>
		[XmlElement(Order = 1)]
		public System.DateTime CurrentTime {
			get {
				return this.currentTimeField;
			}
			set {
				this.currentTimeField = value;
				this.RaisePropertyChanged("CurrentTime");
			}
		}

		/// <remarks/>
		[System.Xml.Serialization.XmlIgnoreAttribute()]
		public bool CurrentTimeSpecified {
			get {
				return this.currentTimeFieldSpecified;
			}
			set {
				this.currentTimeFieldSpecified = value;
				this.RaisePropertyChanged("CurrentTimeSpecified");
			}
		}

		/// <remarks/>
		[XmlElement(IsNullable = true, Order = 2)]
		public System.Nullable<System.DateTime> TerminationTime {
			get {
				return this.terminationTimeField;
			}
			set {
				this.terminationTimeField = value;
				this.RaisePropertyChanged("TerminationTime");
			}
		}

		/// <remarks/>
		[System.Xml.Serialization.XmlIgnoreAttribute()]
		public bool TerminationTimeSpecified {
			get {
				return this.terminationTimeFieldSpecified;
			}
			set {
				this.terminationTimeFieldSpecified = value;
				this.RaisePropertyChanged("TerminationTimeSpecified");
			}
		}

		/// <remarks/>
		[System.Xml.Serialization.XmlAnyElementAttribute(Order = 3)]
		public System.Xml.XmlElement[] Any {
			get {
				return this.anyField;
			}
			set {
				this.anyField = value;
				this.RaisePropertyChanged("Any");
			}
		}

		public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;

		protected void RaisePropertyChanged(string propertyName) {
			System.ComponentModel.PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
			if ((propertyChanged != null)) {
				propertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
			}
		}
	}

	/// <remarks/>
	[System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.0.30319.17929")]
	[System.SerializableAttribute()]
	[System.Diagnostics.DebuggerStepThroughAttribute()]
	[System.ComponentModel.DesignerCategoryAttribute("code")]
	[System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://docs.oasis-open.org/wsn/b-2")]
	public partial class GetCurrentMessage : object, System.ComponentModel.INotifyPropertyChanged {

		private TopicExpressionType topicField;

		private System.Xml.XmlElement[] anyField;

		/// <remarks/>
		[XmlElement(Order = 0)]
		public TopicExpressionType Topic {
			get {
				return this.topicField;
			}
			set {
				this.topicField = value;
				this.RaisePropertyChanged("Topic");
			}
		}

		/// <remarks/>
		[System.Xml.Serialization.XmlAnyElementAttribute(Order = 1)]
		public System.Xml.XmlElement[] Any {
			get {
				return this.anyField;
			}
			set {
				this.anyField = value;
				this.RaisePropertyChanged("Any");
			}
		}

		public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;

		protected void RaisePropertyChanged(string propertyName) {
			System.ComponentModel.PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
			if ((propertyChanged != null)) {
				propertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
			}
		}
	}

	/// <remarks/>
	[System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.0.30319.17929")]
	[System.SerializableAttribute()]
	[System.Diagnostics.DebuggerStepThroughAttribute()]
	[System.ComponentModel.DesignerCategoryAttribute("code")]
	[System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://docs.oasis-open.org/wsn/b-2")]
	public partial class GetCurrentMessageResponse : object, System.ComponentModel.INotifyPropertyChanged {

		private System.Xml.XmlElement[] anyField;

		/// <remarks/>
		[System.Xml.Serialization.XmlAnyElementAttribute(Order = 0)]
		public System.Xml.XmlElement[] Any {
			get {
				return this.anyField;
			}
			set {
				this.anyField = value;
				this.RaisePropertyChanged("Any");
			}
		}

		public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;

		protected void RaisePropertyChanged(string propertyName) {
			System.ComponentModel.PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
			if ((propertyChanged != null)) {
				propertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
			}
		}
	}

	/// <remarks/>
	[System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.0.30319.17929")]
	[System.SerializableAttribute()]
	[System.Diagnostics.DebuggerStepThroughAttribute()]
	[System.ComponentModel.DesignerCategoryAttribute("code")]
	[System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://docs.oasis-open.org/wsn/b-2")]
	public partial class GetMessages : object, System.ComponentModel.INotifyPropertyChanged {

		private string maximumNumberField;

		private System.Xml.XmlElement[] anyField;

		private System.Xml.XmlAttribute[] anyAttrField;

		/// <remarks/>
		[XmlElement(DataType = "nonNegativeInteger", Order = 0)]
		public string MaximumNumber {
			get {
				return this.maximumNumberField;
			}
			set {
				this.maximumNumberField = value;
				this.RaisePropertyChanged("MaximumNumber");
			}
		}

		/// <remarks/>
		[System.Xml.Serialization.XmlAnyElementAttribute(Order = 1)]
		public System.Xml.XmlElement[] Any {
			get {
				return this.anyField;
			}
			set {
				this.anyField = value;
				this.RaisePropertyChanged("Any");
			}
		}

		/// <remarks/>
		[System.Xml.Serialization.XmlAnyAttributeAttribute()]
		public System.Xml.XmlAttribute[] AnyAttr {
			get {
				return this.anyAttrField;
			}
			set {
				this.anyAttrField = value;
				this.RaisePropertyChanged("AnyAttr");
			}
		}

		public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;

		protected void RaisePropertyChanged(string propertyName) {
			System.ComponentModel.PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
			if ((propertyChanged != null)) {
				propertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
			}
		}
	}

	/// <remarks/>
	[System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.0.30319.17929")]
	[System.SerializableAttribute()]
	[System.Diagnostics.DebuggerStepThroughAttribute()]
	[System.ComponentModel.DesignerCategoryAttribute("code")]
	[System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://docs.oasis-open.org/wsn/b-2")]
	public partial class GetMessagesResponse : object, System.ComponentModel.INotifyPropertyChanged {

		private NotificationMessageHolderType[] notificationMessageField;

		private System.Xml.XmlElement[] anyField;

		private System.Xml.XmlAttribute[] anyAttrField;

		/// <remarks/>
		[XmlElement("NotificationMessage", Order = 0)]
		public NotificationMessageHolderType[] NotificationMessage {
			get {
				return this.notificationMessageField;
			}
			set {
				this.notificationMessageField = value;
				this.RaisePropertyChanged("NotificationMessage");
			}
		}

		/// <remarks/>
		[System.Xml.Serialization.XmlAnyElementAttribute(Order = 1)]
		public System.Xml.XmlElement[] Any {
			get {
				return this.anyField;
			}
			set {
				this.anyField = value;
				this.RaisePropertyChanged("Any");
			}
		}

		/// <remarks/>
		[System.Xml.Serialization.XmlAnyAttributeAttribute()]
		public System.Xml.XmlAttribute[] AnyAttr {
			get {
				return this.anyAttrField;
			}
			set {
				this.anyAttrField = value;
				this.RaisePropertyChanged("AnyAttr");
			}
		}

		public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;

		protected void RaisePropertyChanged(string propertyName) {
			System.ComponentModel.PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
			if ((propertyChanged != null)) {
				propertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
			}
		}
	}

	/// <remarks/>
	[System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.0.30319.17929")]
	[System.SerializableAttribute()]
	[System.Diagnostics.DebuggerStepThroughAttribute()]
	[System.ComponentModel.DesignerCategoryAttribute("code")]
	[System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://docs.oasis-open.org/wsn/b-2")]
	public partial class DestroyPullPoint : object, System.ComponentModel.INotifyPropertyChanged {

		private System.Xml.XmlElement[] anyField;

		private System.Xml.XmlAttribute[] anyAttrField;

		/// <remarks/>
		[System.Xml.Serialization.XmlAnyElementAttribute(Order = 0)]
		public System.Xml.XmlElement[] Any {
			get {
				return this.anyField;
			}
			set {
				this.anyField = value;
				this.RaisePropertyChanged("Any");
			}
		}

		/// <remarks/>
		[System.Xml.Serialization.XmlAnyAttributeAttribute()]
		public System.Xml.XmlAttribute[] AnyAttr {
			get {
				return this.anyAttrField;
			}
			set {
				this.anyAttrField = value;
				this.RaisePropertyChanged("AnyAttr");
			}
		}

		public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;

		protected void RaisePropertyChanged(string propertyName) {
			System.ComponentModel.PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
			if ((propertyChanged != null)) {
				propertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
			}
		}
	}

	/// <remarks/>
	[System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.0.30319.17929")]
	[System.SerializableAttribute()]
	[System.Diagnostics.DebuggerStepThroughAttribute()]
	[System.ComponentModel.DesignerCategoryAttribute("code")]
	[System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://docs.oasis-open.org/wsn/b-2")]
	public partial class DestroyPullPointResponse : object, System.ComponentModel.INotifyPropertyChanged {

		private System.Xml.XmlElement[] anyField;

		private System.Xml.XmlAttribute[] anyAttrField;

		/// <remarks/>
		[System.Xml.Serialization.XmlAnyElementAttribute(Order = 0)]
		public System.Xml.XmlElement[] Any {
			get {
				return this.anyField;
			}
			set {
				this.anyField = value;
				this.RaisePropertyChanged("Any");
			}
		}

		/// <remarks/>
		[System.Xml.Serialization.XmlAnyAttributeAttribute()]
		public System.Xml.XmlAttribute[] AnyAttr {
			get {
				return this.anyAttrField;
			}
			set {
				this.anyAttrField = value;
				this.RaisePropertyChanged("AnyAttr");
			}
		}

		public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;

		protected void RaisePropertyChanged(string propertyName) {
			System.ComponentModel.PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
			if ((propertyChanged != null)) {
				propertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
			}
		}
	}

	/// <remarks/>
	[System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.0.30319.17929")]
	[System.SerializableAttribute()]
	[System.Diagnostics.DebuggerStepThroughAttribute()]
	[System.ComponentModel.DesignerCategoryAttribute("code")]
	[System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://docs.oasis-open.org/wsn/b-2")]
	public partial class CreatePullPoint : object, System.ComponentModel.INotifyPropertyChanged {

		private System.Xml.XmlElement[] anyField;

		private System.Xml.XmlAttribute[] anyAttrField;

		/// <remarks/>
		[System.Xml.Serialization.XmlAnyElementAttribute(Order = 0)]
		public System.Xml.XmlElement[] Any {
			get {
				return this.anyField;
			}
			set {
				this.anyField = value;
				this.RaisePropertyChanged("Any");
			}
		}

		/// <remarks/>
		[System.Xml.Serialization.XmlAnyAttributeAttribute()]
		public System.Xml.XmlAttribute[] AnyAttr {
			get {
				return this.anyAttrField;
			}
			set {
				this.anyAttrField = value;
				this.RaisePropertyChanged("AnyAttr");
			}
		}

		public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;

		protected void RaisePropertyChanged(string propertyName) {
			System.ComponentModel.PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
			if ((propertyChanged != null)) {
				propertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
			}
		}
	}

	/// <remarks/>
	[System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.0.30319.17929")]
	[System.SerializableAttribute()]
	[System.Diagnostics.DebuggerStepThroughAttribute()]
	[System.ComponentModel.DesignerCategoryAttribute("code")]
	[System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://docs.oasis-open.org/wsn/b-2")]
	public partial class CreatePullPointResponse : object, System.ComponentModel.INotifyPropertyChanged {

		private EndpointReferenceType1 pullPointField;

		private System.Xml.XmlElement[] anyField;

		private System.Xml.XmlAttribute[] anyAttrField;

		/// <remarks/>
		[XmlElement(Order = 0)]
		public EndpointReferenceType1 PullPoint {
			get {
				return this.pullPointField;
			}
			set {
				this.pullPointField = value;
				this.RaisePropertyChanged("PullPoint");
			}
		}

		/// <remarks/>
		[System.Xml.Serialization.XmlAnyElementAttribute(Order = 1)]
		public System.Xml.XmlElement[] Any {
			get {
				return this.anyField;
			}
			set {
				this.anyField = value;
				this.RaisePropertyChanged("Any");
			}
		}

		/// <remarks/>
		[System.Xml.Serialization.XmlAnyAttributeAttribute()]
		public System.Xml.XmlAttribute[] AnyAttr {
			get {
				return this.anyAttrField;
			}
			set {
				this.anyAttrField = value;
				this.RaisePropertyChanged("AnyAttr");
			}
		}

		public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;

		protected void RaisePropertyChanged(string propertyName) {
			System.ComponentModel.PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
			if ((propertyChanged != null)) {
				propertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
			}
		}
	}

	/// <remarks/>
	[System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.0.30319.17929")]
	[System.SerializableAttribute()]
	[System.Diagnostics.DebuggerStepThroughAttribute()]
	[System.ComponentModel.DesignerCategoryAttribute("code")]
	[System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://docs.oasis-open.org/wsn/b-2")]
	public partial class Renew : object, System.ComponentModel.INotifyPropertyChanged {

		private string terminationTimeField;

		private System.Xml.XmlElement[] anyField;

		/// <remarks/>
		[XmlElement(IsNullable = true, Order = 0)]
		public string TerminationTime {
			get {
				return this.terminationTimeField;
			}
			set {
				this.terminationTimeField = value;
				this.RaisePropertyChanged("TerminationTime");
			}
		}

		/// <remarks/>
		[System.Xml.Serialization.XmlAnyElementAttribute(Order = 1)]
		public System.Xml.XmlElement[] Any {
			get {
				return this.anyField;
			}
			set {
				this.anyField = value;
				this.RaisePropertyChanged("Any");
			}
		}

		public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;

		protected void RaisePropertyChanged(string propertyName) {
			System.ComponentModel.PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
			if ((propertyChanged != null)) {
				propertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
			}
		}
	}

	/// <remarks/>
	[System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.0.30319.17929")]
	[System.SerializableAttribute()]
	[System.Diagnostics.DebuggerStepThroughAttribute()]
	[System.ComponentModel.DesignerCategoryAttribute("code")]
	[System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://docs.oasis-open.org/wsn/b-2")]
	public partial class RenewResponse : object, System.ComponentModel.INotifyPropertyChanged {

		private System.Nullable<System.DateTime> terminationTimeField;

		private System.DateTime currentTimeField;

		private bool currentTimeFieldSpecified;

		private System.Xml.XmlElement[] anyField;

		/// <remarks/>
		[XmlElement(IsNullable = true, Order = 0)]
		public System.Nullable<System.DateTime> TerminationTime {
			get {
				return this.terminationTimeField;
			}
			set {
				this.terminationTimeField = value;
				this.RaisePropertyChanged("TerminationTime");
			}
		}

		/// <remarks/>
		[XmlElement(Order = 1)]
		public System.DateTime CurrentTime {
			get {
				return this.currentTimeField;
			}
			set {
				this.currentTimeField = value;
				this.RaisePropertyChanged("CurrentTime");
			}
		}

		/// <remarks/>
		[System.Xml.Serialization.XmlIgnoreAttribute()]
		public bool CurrentTimeSpecified {
			get {
				return this.currentTimeFieldSpecified;
			}
			set {
				this.currentTimeFieldSpecified = value;
				this.RaisePropertyChanged("CurrentTimeSpecified");
			}
		}

		/// <remarks/>
		[System.Xml.Serialization.XmlAnyElementAttribute(Order = 2)]
		public System.Xml.XmlElement[] Any {
			get {
				return this.anyField;
			}
			set {
				this.anyField = value;
				this.RaisePropertyChanged("Any");
			}
		}

		public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;

		protected void RaisePropertyChanged(string propertyName) {
			System.ComponentModel.PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
			if ((propertyChanged != null)) {
				propertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
			}
		}
	}

	/// <remarks/>
	[System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.0.30319.17929")]
	[System.SerializableAttribute()]
	[System.Diagnostics.DebuggerStepThroughAttribute()]
	[System.ComponentModel.DesignerCategoryAttribute("code")]
	[System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://docs.oasis-open.org/wsn/b-2")]
	public partial class Unsubscribe : object, System.ComponentModel.INotifyPropertyChanged {

		private System.Xml.XmlElement[] anyField;

		/// <remarks/>
		[System.Xml.Serialization.XmlAnyElementAttribute(Order = 0)]
		public System.Xml.XmlElement[] Any {
			get {
				return this.anyField;
			}
			set {
				this.anyField = value;
				this.RaisePropertyChanged("Any");
			}
		}

		public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;

		protected void RaisePropertyChanged(string propertyName) {
			System.ComponentModel.PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
			if ((propertyChanged != null)) {
				propertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
			}
		}
	}

	/// <remarks/>
	[System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.0.30319.17929")]
	[System.SerializableAttribute()]
	[System.Diagnostics.DebuggerStepThroughAttribute()]
	[System.ComponentModel.DesignerCategoryAttribute("code")]
	[System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://docs.oasis-open.org/wsn/b-2")]
	public partial class UnsubscribeResponse : object, System.ComponentModel.INotifyPropertyChanged {

		private System.Xml.XmlElement[] anyField;

		/// <remarks/>
		[System.Xml.Serialization.XmlAnyElementAttribute(Order = 0)]
		public System.Xml.XmlElement[] Any {
			get {
				return this.anyField;
			}
			set {
				this.anyField = value;
				this.RaisePropertyChanged("Any");
			}
		}

		public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;

		protected void RaisePropertyChanged(string propertyName) {
			System.ComponentModel.PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
			if ((propertyChanged != null)) {
				propertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
			}
		}
	}

	/// <remarks/>
	[System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.0.30319.17929")]
	[System.SerializableAttribute()]
	[System.Diagnostics.DebuggerStepThroughAttribute()]
	[System.ComponentModel.DesignerCategoryAttribute("code")]
	[System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://docs.oasis-open.org/wsn/b-2")]
	public partial class PauseSubscription : object, System.ComponentModel.INotifyPropertyChanged {

		private System.Xml.XmlElement[] anyField;

		/// <remarks/>
		[System.Xml.Serialization.XmlAnyElementAttribute(Order = 0)]
		public System.Xml.XmlElement[] Any {
			get {
				return this.anyField;
			}
			set {
				this.anyField = value;
				this.RaisePropertyChanged("Any");
			}
		}

		public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;

		protected void RaisePropertyChanged(string propertyName) {
			System.ComponentModel.PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
			if ((propertyChanged != null)) {
				propertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
			}
		}
	}

	/// <remarks/>
	[System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.0.30319.17929")]
	[System.SerializableAttribute()]
	[System.Diagnostics.DebuggerStepThroughAttribute()]
	[System.ComponentModel.DesignerCategoryAttribute("code")]
	[System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://docs.oasis-open.org/wsn/b-2")]
	public partial class PauseSubscriptionResponse : object, System.ComponentModel.INotifyPropertyChanged {

		private System.Xml.XmlElement[] anyField;

		/// <remarks/>
		[System.Xml.Serialization.XmlAnyElementAttribute(Order = 0)]
		public System.Xml.XmlElement[] Any {
			get {
				return this.anyField;
			}
			set {
				this.anyField = value;
				this.RaisePropertyChanged("Any");
			}
		}

		public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;

		protected void RaisePropertyChanged(string propertyName) {
			System.ComponentModel.PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
			if ((propertyChanged != null)) {
				propertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
			}
		}
	}

	/// <remarks/>
	[System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.0.30319.17929")]
	[System.SerializableAttribute()]
	[System.Diagnostics.DebuggerStepThroughAttribute()]
	[System.ComponentModel.DesignerCategoryAttribute("code")]
	[System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://docs.oasis-open.org/wsn/b-2")]
	public partial class ResumeSubscription : object, System.ComponentModel.INotifyPropertyChanged {

		private System.Xml.XmlElement[] anyField;

		/// <remarks/>
		[System.Xml.Serialization.XmlAnyElementAttribute(Order = 0)]
		public System.Xml.XmlElement[] Any {
			get {
				return this.anyField;
			}
			set {
				this.anyField = value;
				this.RaisePropertyChanged("Any");
			}
		}

		public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;

		protected void RaisePropertyChanged(string propertyName) {
			System.ComponentModel.PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
			if ((propertyChanged != null)) {
				propertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
			}
		}
	}

	/// <remarks/>
	[System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.0.30319.17929")]
	[System.SerializableAttribute()]
	[System.Diagnostics.DebuggerStepThroughAttribute()]
	[System.ComponentModel.DesignerCategoryAttribute("code")]
	[System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://docs.oasis-open.org/wsn/b-2")]
	public partial class ResumeSubscriptionResponse : object, System.ComponentModel.INotifyPropertyChanged {

		private System.Xml.XmlElement[] anyField;

		/// <remarks/>
		[System.Xml.Serialization.XmlAnyElementAttribute(Order = 0)]
		public System.Xml.XmlElement[] Any {
			get {
				return this.anyField;
			}
			set {
				this.anyField = value;
				this.RaisePropertyChanged("Any");
			}
		}

		public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;

		protected void RaisePropertyChanged(string propertyName) {
			System.ComponentModel.PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
			if ((propertyChanged != null)) {
				propertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
			}
		}
	}

	/// <remarks/>
	[System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.0.30319.17929")]
	[System.SerializableAttribute()]
	[System.Diagnostics.DebuggerStepThroughAttribute()]
	[System.ComponentModel.DesignerCategoryAttribute("code")]
	[System.Xml.Serialization.XmlTypeAttribute(TypeName = "Capabilities", Namespace = "http://www.onvif.org/ver10/events/wsdl")]
	public partial class Capabilities2 : object, System.ComponentModel.INotifyPropertyChanged {

		private System.Xml.XmlElement[] anyField;

		private bool wSSubscriptionPolicySupportField;

		private bool wSSubscriptionPolicySupportFieldSpecified;

		private bool wSPullPointSupportField;

		private bool wSPullPointSupportFieldSpecified;

		private bool wSPausableSubscriptionManagerInterfaceSupportField;

		private bool wSPausableSubscriptionManagerInterfaceSupportFieldSpecified;

		private System.Xml.XmlAttribute[] anyAttrField;

		/// <remarks/>
		[System.Xml.Serialization.XmlAnyElementAttribute(Order = 0)]
		public System.Xml.XmlElement[] Any {
			get {
				return this.anyField;
			}
			set {
				this.anyField = value;
				this.RaisePropertyChanged("Any");
			}
		}

		/// <remarks/>
		[System.Xml.Serialization.XmlAttributeAttribute()]
		public bool WSSubscriptionPolicySupport {
			get {
				return this.wSSubscriptionPolicySupportField;
			}
			set {
				this.wSSubscriptionPolicySupportField = value;
				this.RaisePropertyChanged("WSSubscriptionPolicySupport");
			}
		}

		/// <remarks/>
		[System.Xml.Serialization.XmlIgnoreAttribute()]
		public bool WSSubscriptionPolicySupportSpecified {
			get {
				return this.wSSubscriptionPolicySupportFieldSpecified;
			}
			set {
				this.wSSubscriptionPolicySupportFieldSpecified = value;
				this.RaisePropertyChanged("WSSubscriptionPolicySupportSpecified");
			}
		}

		/// <remarks/>
		[System.Xml.Serialization.XmlAttributeAttribute()]
		public bool WSPullPointSupport {
			get {
				return this.wSPullPointSupportField;
			}
			set {
				this.wSPullPointSupportField = value;
				this.RaisePropertyChanged("WSPullPointSupport");
			}
		}

		/// <remarks/>
		[System.Xml.Serialization.XmlIgnoreAttribute()]
		public bool WSPullPointSupportSpecified {
			get {
				return this.wSPullPointSupportFieldSpecified;
			}
			set {
				this.wSPullPointSupportFieldSpecified = value;
				this.RaisePropertyChanged("WSPullPointSupportSpecified");
			}
		}

		/// <remarks/>
		[System.Xml.Serialization.XmlAttributeAttribute()]
		public bool WSPausableSubscriptionManagerInterfaceSupport {
			get {
				return this.wSPausableSubscriptionManagerInterfaceSupportField;
			}
			set {
				this.wSPausableSubscriptionManagerInterfaceSupportField = value;
				this.RaisePropertyChanged("WSPausableSubscriptionManagerInterfaceSupport");
			}
		}

		/// <remarks/>
		[System.Xml.Serialization.XmlIgnoreAttribute()]
		public bool WSPausableSubscriptionManagerInterfaceSupportSpecified {
			get {
				return this.wSPausableSubscriptionManagerInterfaceSupportFieldSpecified;
			}
			set {
				this.wSPausableSubscriptionManagerInterfaceSupportFieldSpecified = value;
				this.RaisePropertyChanged("WSPausableSubscriptionManagerInterfaceSupportSpecified");
			}
		}

		/// <remarks/>
		[System.Xml.Serialization.XmlAnyAttributeAttribute()]
		public System.Xml.XmlAttribute[] AnyAttr {
			get {
				return this.anyAttrField;
			}
			set {
				this.anyAttrField = value;
				this.RaisePropertyChanged("AnyAttr");
			}
		}

		public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;

		protected void RaisePropertyChanged(string propertyName) {
			System.ComponentModel.PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
			if ((propertyChanged != null)) {
				propertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
			}
		}
	}

	/// <remarks/>
	[System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.0.30319.17929")]
	[System.SerializableAttribute()]
	[System.Diagnostics.DebuggerStepThroughAttribute()]
	[System.ComponentModel.DesignerCategoryAttribute("code")]
	[System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://www.onvif.org/ver10/events/wsdl")]
	public partial class CreatePullPointSubscriptionSubscriptionPolicy : object, System.ComponentModel.INotifyPropertyChanged {

		private System.Xml.XmlElement[] anyField;

		/// <remarks/>
		[System.Xml.Serialization.XmlAnyElementAttribute(Order = 0)]
		public System.Xml.XmlElement[] Any {
			get {
				return this.anyField;
			}
			set {
				this.anyField = value;
				this.RaisePropertyChanged("Any");
			}
		}

		public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;

		protected void RaisePropertyChanged(string propertyName) {
			System.ComponentModel.PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
			if ((propertyChanged != null)) {
				propertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
			}
		}
	}

	/// <remarks/>
	[System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.0.30319.17929")]
	[System.SerializableAttribute()]
	[System.Diagnostics.DebuggerStepThroughAttribute()]
	[System.ComponentModel.DesignerCategoryAttribute("code")]
	[System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://docs.oasis-open.org/wsn/t-1")]
	public partial class TopicSetType : ExtensibleDocumented {

		private System.Xml.XmlElement[] anyField;

		/// <remarks/>
		[System.Xml.Serialization.XmlAnyElementAttribute(Order = 0)]
		public System.Xml.XmlElement[] Any {
			get {
				return this.anyField;
			}
			set {
				this.anyField = value;
				this.RaisePropertyChanged("Any");
			}
		}
	}

	/// <remarks/>
	[System.Xml.Serialization.XmlIncludeAttribute(typeof(TopicSetType))]
	[System.Xml.Serialization.XmlIncludeAttribute(typeof(TopicType))]
	[System.Xml.Serialization.XmlIncludeAttribute(typeof(TopicNamespaceType))]
	[System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.0.30319.17929")]
	[System.SerializableAttribute()]
	[System.Diagnostics.DebuggerStepThroughAttribute()]
	[System.ComponentModel.DesignerCategoryAttribute("code")]
	[System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://docs.oasis-open.org/wsn/t-1")]
	public abstract partial class ExtensibleDocumented : object, System.ComponentModel.INotifyPropertyChanged {

		private Documentation documentationField;

		private System.Xml.XmlAttribute[] anyAttrField;

		/// <remarks/>
		[XmlElement(Order = 0)]
		public Documentation documentation {
			get {
				return this.documentationField;
			}
			set {
				this.documentationField = value;
				this.RaisePropertyChanged("documentation");
			}
		}

		/// <remarks/>
		[System.Xml.Serialization.XmlAnyAttributeAttribute()]
		public System.Xml.XmlAttribute[] AnyAttr {
			get {
				return this.anyAttrField;
			}
			set {
				this.anyAttrField = value;
				this.RaisePropertyChanged("AnyAttr");
			}
		}

		public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;

		protected void RaisePropertyChanged(string propertyName) {
			System.ComponentModel.PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
			if ((propertyChanged != null)) {
				propertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
			}
		}
	}

	/// <remarks/>
	[System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.0.30319.17929")]
	[System.SerializableAttribute()]
	[System.Diagnostics.DebuggerStepThroughAttribute()]
	[System.ComponentModel.DesignerCategoryAttribute("code")]
	[System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://docs.oasis-open.org/wsn/t-1")]
	public partial class Documentation : object, System.ComponentModel.INotifyPropertyChanged {

		private System.Xml.XmlNode[] anyField;

		/// <remarks/>
		[System.Xml.Serialization.XmlTextAttribute()]
		[System.Xml.Serialization.XmlAnyElementAttribute(Order = 0)]
		public System.Xml.XmlNode[] Any {
			get {
				return this.anyField;
			}
			set {
				this.anyField = value;
				this.RaisePropertyChanged("Any");
			}
		}

		public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;

		protected void RaisePropertyChanged(string propertyName) {
			System.ComponentModel.PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
			if ((propertyChanged != null)) {
				propertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
			}
		}
	}

	/// <remarks/>
	[System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.0.30319.17929")]
	[System.SerializableAttribute()]
	[System.Diagnostics.DebuggerStepThroughAttribute()]
	[System.ComponentModel.DesignerCategoryAttribute("code")]
	[System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://docs.oasis-open.org/wsn/t-1")]
	public partial class TopicType : ExtensibleDocumented {

		private QueryExpressionType1 messagePatternField;

		private TopicType[] topicField;

		private System.Xml.XmlElement[] anyField;

		private string nameField;

		private System.Xml.XmlQualifiedName[] messageTypesField;

		private bool finalField;

		public TopicType() {
			this.finalField = false;
		}

		/// <remarks/>
		[XmlElement(Order = 0)]
		public QueryExpressionType1 MessagePattern {
			get {
				return this.messagePatternField;
			}
			set {
				this.messagePatternField = value;
				this.RaisePropertyChanged("MessagePattern");
			}
		}

		/// <remarks/>
		[XmlElement("Topic", Order = 1)]
		public TopicType[] Topic {
			get {
				return this.topicField;
			}
			set {
				this.topicField = value;
				this.RaisePropertyChanged("Topic");
			}
		}

		/// <remarks/>
		[System.Xml.Serialization.XmlAnyElementAttribute(Order = 2)]
		public System.Xml.XmlElement[] Any {
			get {
				return this.anyField;
			}
			set {
				this.anyField = value;
				this.RaisePropertyChanged("Any");
			}
		}

		/// <remarks/>
		[System.Xml.Serialization.XmlAttributeAttribute(DataType = "NCName")]
		public string name {
			get {
				return this.nameField;
			}
			set {
				this.nameField = value;
				this.RaisePropertyChanged("name");
			}
		}

		/// <remarks/>
		[System.Xml.Serialization.XmlAttributeAttribute()]
		public System.Xml.XmlQualifiedName[] messageTypes {
			get {
				return this.messageTypesField;
			}
			set {
				this.messageTypesField = value;
				this.RaisePropertyChanged("messageTypes");
			}
		}

		/// <remarks/>
		[System.Xml.Serialization.XmlAttributeAttribute()]
		[System.ComponentModel.DefaultValueAttribute(false)]
		public bool final {
			get {
				return this.finalField;
			}
			set {
				this.finalField = value;
				this.RaisePropertyChanged("final");
			}
		}
	}

	/// <remarks/>
	[System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.0.30319.17929")]
	[System.SerializableAttribute()]
	[System.Diagnostics.DebuggerStepThroughAttribute()]
	[System.ComponentModel.DesignerCategoryAttribute("code")]
	[System.Xml.Serialization.XmlTypeAttribute(TypeName = "QueryExpressionType", Namespace = "http://docs.oasis-open.org/wsn/t-1")]
	public partial class QueryExpressionType1 : object, System.ComponentModel.INotifyPropertyChanged {

		private System.Xml.XmlNode[] anyField;

		private string dialectField;

		/// <remarks/>
		[System.Xml.Serialization.XmlTextAttribute()]
		[System.Xml.Serialization.XmlAnyElementAttribute(Order = 0)]
		public System.Xml.XmlNode[] Any {
			get {
				return this.anyField;
			}
			set {
				this.anyField = value;
				this.RaisePropertyChanged("Any");
			}
		}

		/// <remarks/>
		[System.Xml.Serialization.XmlAttributeAttribute(DataType = "anyURI")]
		public string Dialect {
			get {
				return this.dialectField;
			}
			set {
				this.dialectField = value;
				this.RaisePropertyChanged("Dialect");
			}
		}

		public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;

		protected void RaisePropertyChanged(string propertyName) {
			System.ComponentModel.PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
			if ((propertyChanged != null)) {
				propertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
			}
		}
	}

	/// <remarks/>
	[System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.0.30319.17929")]
	[System.SerializableAttribute()]
	[System.Diagnostics.DebuggerStepThroughAttribute()]
	[System.ComponentModel.DesignerCategoryAttribute("code")]
	[System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://docs.oasis-open.org/wsn/t-1")]
	public partial class TopicNamespaceType : ExtensibleDocumented {

		private TopicNamespaceTypeTopic[] topicField;

		private System.Xml.XmlElement[] anyField;

		private string nameField;

		private string targetNamespaceField;

		private bool finalField;

		public TopicNamespaceType() {
			this.finalField = false;
		}

		/// <remarks/>
		[XmlElement("Topic", Order = 0)]
		public TopicNamespaceTypeTopic[] Topic {
			get {
				return this.topicField;
			}
			set {
				this.topicField = value;
				this.RaisePropertyChanged("Topic");
			}
		}

		/// <remarks/>
		[System.Xml.Serialization.XmlAnyElementAttribute(Order = 1)]
		public System.Xml.XmlElement[] Any {
			get {
				return this.anyField;
			}
			set {
				this.anyField = value;
				this.RaisePropertyChanged("Any");
			}
		}

		/// <remarks/>
		[System.Xml.Serialization.XmlAttributeAttribute(DataType = "NCName")]
		public string name {
			get {
				return this.nameField;
			}
			set {
				this.nameField = value;
				this.RaisePropertyChanged("name");
			}
		}

		/// <remarks/>
		[System.Xml.Serialization.XmlAttributeAttribute(DataType = "anyURI")]
		public string targetNamespace {
			get {
				return this.targetNamespaceField;
			}
			set {
				this.targetNamespaceField = value;
				this.RaisePropertyChanged("targetNamespace");
			}
		}

		/// <remarks/>
		[System.Xml.Serialization.XmlAttributeAttribute()]
		[System.ComponentModel.DefaultValueAttribute(false)]
		public bool final {
			get {
				return this.finalField;
			}
			set {
				this.finalField = value;
				this.RaisePropertyChanged("final");
			}
		}
	}

	/// <remarks/>
	[System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.0.30319.17929")]
	[System.SerializableAttribute()]
	[System.Diagnostics.DebuggerStepThroughAttribute()]
	[System.ComponentModel.DesignerCategoryAttribute("code")]
	[System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://docs.oasis-open.org/wsn/t-1")]
	public partial class TopicNamespaceTypeTopic : TopicType {

		private string parentField;

		/// <remarks/>
		[System.Xml.Serialization.XmlAttributeAttribute(DataType = "token")]
		public string parent {
			get {
				return this.parentField;
			}
			set {
				this.parentField = value;
				this.RaisePropertyChanged("parent");
			}
		}
	}

	/// <remarks/>
	[System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.0.30319.17929")]
	[System.SerializableAttribute()]
	[System.Diagnostics.DebuggerStepThroughAttribute()]
	[System.ComponentModel.DesignerCategoryAttribute("code")]
	[System.Xml.Serialization.XmlTypeAttribute(TypeName = "Capabilities", Namespace = "http://www.onvif.org/ver20/analytics/wsdl")]
	public partial class Capabilities6 : object, System.ComponentModel.INotifyPropertyChanged {

		private System.Xml.XmlElement[] anyField;

		private bool ruleSupportField;

		private bool ruleSupportFieldSpecified;

		private bool analyticsModuleSupportField;

		private bool analyticsModuleSupportFieldSpecified;

		private System.Xml.XmlAttribute[] anyAttrField;

		/// <remarks/>
		[System.Xml.Serialization.XmlAnyElementAttribute(Order = 0)]
		public System.Xml.XmlElement[] Any {
			get {
				return this.anyField;
			}
			set {
				this.anyField = value;
				this.RaisePropertyChanged("Any");
			}
		}

		/// <remarks/>
		[System.Xml.Serialization.XmlAttributeAttribute()]
		public bool RuleSupport {
			get {
				return this.ruleSupportField;
			}
			set {
				this.ruleSupportField = value;
				this.RaisePropertyChanged("RuleSupport");
			}
		}

		/// <remarks/>
		[System.Xml.Serialization.XmlIgnoreAttribute()]
		public bool RuleSupportSpecified {
			get {
				return this.ruleSupportFieldSpecified;
			}
			set {
				this.ruleSupportFieldSpecified = value;
				this.RaisePropertyChanged("RuleSupportSpecified");
			}
		}

		/// <remarks/>
		[System.Xml.Serialization.XmlAttributeAttribute()]
		public bool AnalyticsModuleSupport {
			get {
				return this.analyticsModuleSupportField;
			}
			set {
				this.analyticsModuleSupportField = value;
				this.RaisePropertyChanged("AnalyticsModuleSupport");
			}
		}

		/// <remarks/>
		[System.Xml.Serialization.XmlIgnoreAttribute()]
		public bool AnalyticsModuleSupportSpecified {
			get {
				return this.analyticsModuleSupportFieldSpecified;
			}
			set {
				this.analyticsModuleSupportFieldSpecified = value;
				this.RaisePropertyChanged("AnalyticsModuleSupportSpecified");
			}
		}


		/// <remarks/>
		[System.Xml.Serialization.XmlAnyAttributeAttribute()]
		public System.Xml.XmlAttribute[] AnyAttr {
			get {
				return this.anyAttrField;
			}
			set {
				this.anyAttrField = value;
				this.RaisePropertyChanged("AnyAttr");
			}
		}

		public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;

		protected void RaisePropertyChanged(string propertyName) {
			System.ComponentModel.PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
			if ((propertyChanged != null)) {
				propertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
			}
		}
	}

	/// <remarks/>
	[System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.0.30319.17929")]
	[System.SerializableAttribute()]
	[System.Diagnostics.DebuggerStepThroughAttribute()]
	[System.ComponentModel.DesignerCategoryAttribute("code")]
	[System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.onvif.org/ver10/device/wsdl")]
	public partial class Service : object, System.ComponentModel.INotifyPropertyChanged {

		private string namespaceField;

		private string xAddrField;

		private System.Xml.XmlElement capabilitiesField;

		private OnvifVersion versionField;

		private System.Xml.XmlElement[] anyField;

		private System.Xml.XmlAttribute[] anyAttrField;

		/// <remarks/>
		[XmlElement(DataType = "anyURI", Order = 0)]
		public string Namespace {
			get {
				return this.namespaceField;
			}
			set {
				this.namespaceField = value;
				this.RaisePropertyChanged("Namespace");
			}
		}

		/// <remarks/>
		[XmlElement(DataType = "anyURI", Order = 1)]
		public string XAddr {
			get {
				return this.xAddrField;
			}
			set {
				this.xAddrField = value;
				this.RaisePropertyChanged("XAddr");
			}
		}

		/// <remarks/>
		[XmlElement(Order = 2)]
		public System.Xml.XmlElement Capabilities {
			get {
				return this.capabilitiesField;
			}
			set {
				this.capabilitiesField = value;
				this.RaisePropertyChanged("Capabilities");
			}
		}

		/// <remarks/>
		[XmlElement(Order = 3)]
		public OnvifVersion Version {
			get {
				return this.versionField;
			}
			set {
				this.versionField = value;
				this.RaisePropertyChanged("Version");
			}
		}

		/// <remarks/>
		[System.Xml.Serialization.XmlAnyElementAttribute(Order = 4)]
		public System.Xml.XmlElement[] Any {
			get {
				return this.anyField;
			}
			set {
				this.anyField = value;
				this.RaisePropertyChanged("Any");
			}
		}

		/// <remarks/>
		[System.Xml.Serialization.XmlAnyAttributeAttribute()]
		public System.Xml.XmlAttribute[] AnyAttr {
			get {
				return this.anyAttrField;
			}
			set {
				this.anyAttrField = value;
				this.RaisePropertyChanged("AnyAttr");
			}
		}

		public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;

		protected void RaisePropertyChanged(string propertyName) {
			System.ComponentModel.PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
			if ((propertyChanged != null)) {
				propertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
			}
		}
	}


	/// <remarks/>
	[System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.0.30319.17929")]
	[System.SerializableAttribute()]
	[System.Diagnostics.DebuggerStepThroughAttribute()]
	[System.ComponentModel.DesignerCategoryAttribute("code")]
	[System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.onvif.org/ver10/device/wsdl")]
	public partial class DeviceServiceCapabilities1 : object, System.ComponentModel.INotifyPropertyChanged {

		private NetworkCapabilities1 networkField;

		private SecurityCapabilities1 securityField;

		private SystemCapabilities1 systemField;

		private System.Xml.XmlElement[] anyField;

		/// <remarks/>
		[XmlElement(Order = 0)]
		public NetworkCapabilities1 Network {
			get {
				return this.networkField;
			}
			set {
				this.networkField = value;
				this.RaisePropertyChanged("Network");
			}
		}

		/// <remarks/>
		[XmlElement(Order = 1)]
		public SecurityCapabilities1 Security {
			get {
				return this.securityField;
			}
			set {
				this.securityField = value;
				this.RaisePropertyChanged("Security");
			}
		}

		/// <remarks/>
		[XmlElement(Order = 2)]
		public SystemCapabilities1 System {
			get {
				return this.systemField;
			}
			set {
				this.systemField = value;
				this.RaisePropertyChanged("System");
			}
		}

		/// <remarks/>
		[System.Xml.Serialization.XmlAnyElementAttribute(Order = 3)]
		public System.Xml.XmlElement[] Any {
			get {
				return this.anyField;
			}
			set {
				this.anyField = value;
				this.RaisePropertyChanged("Any");
			}
		}

		public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;

		protected void RaisePropertyChanged(string propertyName) {
			System.ComponentModel.PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
			if ((propertyChanged != null)) {
				propertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
			}
		}
	}

	/// <remarks/>
	[System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.0.30319.17929")]
	[System.SerializableAttribute()]
	[System.Diagnostics.DebuggerStepThroughAttribute()]
	[System.ComponentModel.DesignerCategoryAttribute("code")]
	[System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.onvif.org/ver10/device/wsdl")]
	public partial class NetworkCapabilities1 : object, System.ComponentModel.INotifyPropertyChanged {

		private bool iPFilterField;

		private bool iPFilterFieldSpecified;

		private bool zeroConfigurationField;

		private bool zeroConfigurationFieldSpecified;

		private bool iPVersion6Field;

		private bool iPVersion6FieldSpecified;

		private bool dynDNSField;

		private bool dynDNSFieldSpecified;

		private bool dot11ConfigurationField;

		private bool dot11ConfigurationFieldSpecified;

		private bool hostnameFromDHCPField;

		private bool hostnameFromDHCPFieldSpecified;

		private int nTPField;

		private bool nTPFieldSpecified;

		private System.Xml.XmlAttribute[] anyAttrField;

		/// <remarks/>
		[System.Xml.Serialization.XmlAttributeAttribute()]
		public bool IPFilter {
			get {
				return this.iPFilterField;
			}
			set {
				this.iPFilterField = value;
				this.RaisePropertyChanged("IPFilter");
			}
		}

		/// <remarks/>
		[System.Xml.Serialization.XmlIgnoreAttribute()]
		public bool IPFilterSpecified {
			get {
				return this.iPFilterFieldSpecified;
			}
			set {
				this.iPFilterFieldSpecified = value;
				this.RaisePropertyChanged("IPFilterSpecified");
			}
		}

		/// <remarks/>
		[System.Xml.Serialization.XmlAttributeAttribute()]
		public bool ZeroConfiguration {
			get {
				return this.zeroConfigurationField;
			}
			set {
				this.zeroConfigurationField = value;
				this.RaisePropertyChanged("ZeroConfiguration");
			}
		}

		/// <remarks/>
		[System.Xml.Serialization.XmlIgnoreAttribute()]
		public bool ZeroConfigurationSpecified {
			get {
				return this.zeroConfigurationFieldSpecified;
			}
			set {
				this.zeroConfigurationFieldSpecified = value;
				this.RaisePropertyChanged("ZeroConfigurationSpecified");
			}
		}

		/// <remarks/>
		[System.Xml.Serialization.XmlAttributeAttribute()]
		public bool IPVersion6 {
			get {
				return this.iPVersion6Field;
			}
			set {
				this.iPVersion6Field = value;
				this.RaisePropertyChanged("IPVersion6");
			}
		}

		/// <remarks/>
		[System.Xml.Serialization.XmlIgnoreAttribute()]
		public bool IPVersion6Specified {
			get {
				return this.iPVersion6FieldSpecified;
			}
			set {
				this.iPVersion6FieldSpecified = value;
				this.RaisePropertyChanged("IPVersion6Specified");
			}
		}

		/// <remarks/>
		[System.Xml.Serialization.XmlAttributeAttribute()]
		public bool DynDNS {
			get {
				return this.dynDNSField;
			}
			set {
				this.dynDNSField = value;
				this.RaisePropertyChanged("DynDNS");
			}
		}

		/// <remarks/>
		[System.Xml.Serialization.XmlIgnoreAttribute()]
		public bool DynDNSSpecified {
			get {
				return this.dynDNSFieldSpecified;
			}
			set {
				this.dynDNSFieldSpecified = value;
				this.RaisePropertyChanged("DynDNSSpecified");
			}
		}

		/// <remarks/>
		[System.Xml.Serialization.XmlAttributeAttribute()]
		public bool Dot11Configuration {
			get {
				return this.dot11ConfigurationField;
			}
			set {
				this.dot11ConfigurationField = value;
				this.RaisePropertyChanged("Dot11Configuration");
			}
		}

		/// <remarks/>
		[System.Xml.Serialization.XmlIgnoreAttribute()]
		public bool Dot11ConfigurationSpecified {
			get {
				return this.dot11ConfigurationFieldSpecified;
			}
			set {
				this.dot11ConfigurationFieldSpecified = value;
				this.RaisePropertyChanged("Dot11ConfigurationSpecified");
			}
		}

		/// <remarks/>
		[System.Xml.Serialization.XmlAttributeAttribute()]
		public bool HostnameFromDHCP {
			get {
				return this.hostnameFromDHCPField;
			}
			set {
				this.hostnameFromDHCPField = value;
				this.RaisePropertyChanged("HostnameFromDHCP");
			}
		}

		/// <remarks/>
		[System.Xml.Serialization.XmlIgnoreAttribute()]
		public bool HostnameFromDHCPSpecified {
			get {
				return this.hostnameFromDHCPFieldSpecified;
			}
			set {
				this.hostnameFromDHCPFieldSpecified = value;
				this.RaisePropertyChanged("HostnameFromDHCPSpecified");
			}
		}

		/// <remarks/>
		[System.Xml.Serialization.XmlAttributeAttribute()]
		public int NTP {
			get {
				return this.nTPField;
			}
			set {
				this.nTPField = value;
				this.RaisePropertyChanged("NTP");
			}
		}

		/// <remarks/>
		[System.Xml.Serialization.XmlIgnoreAttribute()]
		public bool NTPSpecified {
			get {
				return this.nTPFieldSpecified;
			}
			set {
				this.nTPFieldSpecified = value;
				this.RaisePropertyChanged("NTPSpecified");
			}
		}

		/// <remarks/>
		[System.Xml.Serialization.XmlAnyAttributeAttribute()]
		public System.Xml.XmlAttribute[] AnyAttr {
			get {
				return this.anyAttrField;
			}
			set {
				this.anyAttrField = value;
				this.RaisePropertyChanged("AnyAttr");
			}
		}

		public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;

		protected void RaisePropertyChanged(string propertyName) {
			System.ComponentModel.PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
			if ((propertyChanged != null)) {
				propertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
			}
		}
	}

	/// <remarks/>
	[System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.0.30319.17929")]
	[System.SerializableAttribute()]
	[System.Diagnostics.DebuggerStepThroughAttribute()]
	[System.ComponentModel.DesignerCategoryAttribute("code")]
	[System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.onvif.org/ver10/device/wsdl")]
	public partial class SecurityCapabilities1 : object, System.ComponentModel.INotifyPropertyChanged {

		private bool tLS10Field;

		private bool tLS10FieldSpecified;

		private bool tLS11Field;

		private bool tLS11FieldSpecified;

		private bool tLS12Field;

		private bool tLS12FieldSpecified;

		private bool onboardKeyGenerationField;

		private bool onboardKeyGenerationFieldSpecified;

		private bool accessPolicyConfigField;

		private bool accessPolicyConfigFieldSpecified;

		private bool defaultAccessPolicyField;

		private bool defaultAccessPolicyFieldSpecified;

		private bool dot1XField;

		private bool dot1XFieldSpecified;

		private bool remoteUserHandlingField;

		private bool remoteUserHandlingFieldSpecified;

		private bool x509TokenField;

		private bool x509TokenFieldSpecified;

		private bool sAMLTokenField;

		private bool sAMLTokenFieldSpecified;

		private bool kerberosTokenField;

		private bool kerberosTokenFieldSpecified;

		private bool usernameTokenField;

		private bool usernameTokenFieldSpecified;

		private bool httpDigestField;

		private bool httpDigestFieldSpecified;

		private bool rELTokenField;

		private bool rELTokenFieldSpecified;

		private int[] supportedEAPMethodsField;

		private System.Xml.XmlAttribute[] anyAttrField;

		/// <remarks/>
		[System.Xml.Serialization.XmlAttributeAttribute("TLS1.0")]
		public bool TLS10 {
			get {
				return this.tLS10Field;
			}
			set {
				this.tLS10Field = value;
				this.RaisePropertyChanged("TLS10");
			}
		}

		/// <remarks/>
		[System.Xml.Serialization.XmlIgnoreAttribute()]
		public bool TLS10Specified {
			get {
				return this.tLS10FieldSpecified;
			}
			set {
				this.tLS10FieldSpecified = value;
				this.RaisePropertyChanged("TLS10Specified");
			}
		}

		/// <remarks/>
		[System.Xml.Serialization.XmlAttributeAttribute("TLS1.1")]
		public bool TLS11 {
			get {
				return this.tLS11Field;
			}
			set {
				this.tLS11Field = value;
				this.RaisePropertyChanged("TLS11");
			}
		}

		/// <remarks/>
		[System.Xml.Serialization.XmlIgnoreAttribute()]
		public bool TLS11Specified {
			get {
				return this.tLS11FieldSpecified;
			}
			set {
				this.tLS11FieldSpecified = value;
				this.RaisePropertyChanged("TLS11Specified");
			}
		}

		/// <remarks/>
		[System.Xml.Serialization.XmlAttributeAttribute("TLS1.2")]
		public bool TLS12 {
			get {
				return this.tLS12Field;
			}
			set {
				this.tLS12Field = value;
				this.RaisePropertyChanged("TLS12");
			}
		}

		/// <remarks/>
		[System.Xml.Serialization.XmlIgnoreAttribute()]
		public bool TLS12Specified {
			get {
				return this.tLS12FieldSpecified;
			}
			set {
				this.tLS12FieldSpecified = value;
				this.RaisePropertyChanged("TLS12Specified");
			}
		}

		/// <remarks/>
		[System.Xml.Serialization.XmlAttributeAttribute()]
		public bool OnboardKeyGeneration {
			get {
				return this.onboardKeyGenerationField;
			}
			set {
				this.onboardKeyGenerationField = value;
				this.RaisePropertyChanged("OnboardKeyGeneration");
			}
		}

		/// <remarks/>
		[System.Xml.Serialization.XmlIgnoreAttribute()]
		public bool OnboardKeyGenerationSpecified {
			get {
				return this.onboardKeyGenerationFieldSpecified;
			}
			set {
				this.onboardKeyGenerationFieldSpecified = value;
				this.RaisePropertyChanged("OnboardKeyGenerationSpecified");
			}
		}

		/// <remarks/>
		[System.Xml.Serialization.XmlAttributeAttribute()]
		public bool AccessPolicyConfig {
			get {
				return this.accessPolicyConfigField;
			}
			set {
				this.accessPolicyConfigField = value;
				this.RaisePropertyChanged("AccessPolicyConfig");
			}
		}

		/// <remarks/>
		[System.Xml.Serialization.XmlIgnoreAttribute()]
		public bool AccessPolicyConfigSpecified {
			get {
				return this.accessPolicyConfigFieldSpecified;
			}
			set {
				this.accessPolicyConfigFieldSpecified = value;
				this.RaisePropertyChanged("AccessPolicyConfigSpecified");
			}
		}

		/// <remarks/>
		[System.Xml.Serialization.XmlAttributeAttribute()]
		public bool DefaultAccessPolicy {
			get {
				return this.defaultAccessPolicyField;
			}
			set {
				this.defaultAccessPolicyField = value;
				this.RaisePropertyChanged("DefaultAccessPolicy");
			}
		}

		/// <remarks/>
		[System.Xml.Serialization.XmlIgnoreAttribute()]
		public bool DefaultAccessPolicySpecified {
			get {
				return this.defaultAccessPolicyFieldSpecified;
			}
			set {
				this.defaultAccessPolicyFieldSpecified = value;
				this.RaisePropertyChanged("DefaultAccessPolicySpecified");
			}
		}

		/// <remarks/>
		[System.Xml.Serialization.XmlAttributeAttribute()]
		public bool Dot1X {
			get {
				return this.dot1XField;
			}
			set {
				this.dot1XField = value;
				this.RaisePropertyChanged("Dot1X");
			}
		}

		/// <remarks/>
		[System.Xml.Serialization.XmlIgnoreAttribute()]
		public bool Dot1XSpecified {
			get {
				return this.dot1XFieldSpecified;
			}
			set {
				this.dot1XFieldSpecified = value;
				this.RaisePropertyChanged("Dot1XSpecified");
			}
		}

		/// <remarks/>
		[System.Xml.Serialization.XmlAttributeAttribute()]
		public bool RemoteUserHandling {
			get {
				return this.remoteUserHandlingField;
			}
			set {
				this.remoteUserHandlingField = value;
				this.RaisePropertyChanged("RemoteUserHandling");
			}
		}

		/// <remarks/>
		[System.Xml.Serialization.XmlIgnoreAttribute()]
		public bool RemoteUserHandlingSpecified {
			get {
				return this.remoteUserHandlingFieldSpecified;
			}
			set {
				this.remoteUserHandlingFieldSpecified = value;
				this.RaisePropertyChanged("RemoteUserHandlingSpecified");
			}
		}

		/// <remarks/>
		[System.Xml.Serialization.XmlAttributeAttribute("X.509Token")]
		public bool X509Token {
			get {
				return this.x509TokenField;
			}
			set {
				this.x509TokenField = value;
				this.RaisePropertyChanged("X509Token");
			}
		}

		/// <remarks/>
		[System.Xml.Serialization.XmlIgnoreAttribute()]
		public bool X509TokenSpecified {
			get {
				return this.x509TokenFieldSpecified;
			}
			set {
				this.x509TokenFieldSpecified = value;
				this.RaisePropertyChanged("X509TokenSpecified");
			}
		}

		/// <remarks/>
		[System.Xml.Serialization.XmlAttributeAttribute()]
		public bool SAMLToken {
			get {
				return this.sAMLTokenField;
			}
			set {
				this.sAMLTokenField = value;
				this.RaisePropertyChanged("SAMLToken");
			}
		}

		/// <remarks/>
		[System.Xml.Serialization.XmlIgnoreAttribute()]
		public bool SAMLTokenSpecified {
			get {
				return this.sAMLTokenFieldSpecified;
			}
			set {
				this.sAMLTokenFieldSpecified = value;
				this.RaisePropertyChanged("SAMLTokenSpecified");
			}
		}

		/// <remarks/>
		[System.Xml.Serialization.XmlAttributeAttribute()]
		public bool KerberosToken {
			get {
				return this.kerberosTokenField;
			}
			set {
				this.kerberosTokenField = value;
				this.RaisePropertyChanged("KerberosToken");
			}
		}

		/// <remarks/>
		[System.Xml.Serialization.XmlIgnoreAttribute()]
		public bool KerberosTokenSpecified {
			get {
				return this.kerberosTokenFieldSpecified;
			}
			set {
				this.kerberosTokenFieldSpecified = value;
				this.RaisePropertyChanged("KerberosTokenSpecified");
			}
		}

		/// <remarks/>
		[System.Xml.Serialization.XmlAttributeAttribute()]
		public bool UsernameToken {
			get {
				return this.usernameTokenField;
			}
			set {
				this.usernameTokenField = value;
				this.RaisePropertyChanged("UsernameToken");
			}
		}

		/// <remarks/>
		[System.Xml.Serialization.XmlIgnoreAttribute()]
		public bool UsernameTokenSpecified {
			get {
				return this.usernameTokenFieldSpecified;
			}
			set {
				this.usernameTokenFieldSpecified = value;
				this.RaisePropertyChanged("UsernameTokenSpecified");
			}
		}

		/// <remarks/>
		[System.Xml.Serialization.XmlAttributeAttribute()]
		public bool HttpDigest {
			get {
				return this.httpDigestField;
			}
			set {
				this.httpDigestField = value;
				this.RaisePropertyChanged("HttpDigest");
			}
		}

		/// <remarks/>
		[System.Xml.Serialization.XmlIgnoreAttribute()]
		public bool HttpDigestSpecified {
			get {
				return this.httpDigestFieldSpecified;
			}
			set {
				this.httpDigestFieldSpecified = value;
				this.RaisePropertyChanged("HttpDigestSpecified");
			}
		}

		/// <remarks/>
		[System.Xml.Serialization.XmlAttributeAttribute()]
		public bool RELToken {
			get {
				return this.rELTokenField;
			}
			set {
				this.rELTokenField = value;
				this.RaisePropertyChanged("RELToken");
			}
		}

		/// <remarks/>
		[System.Xml.Serialization.XmlIgnoreAttribute()]
		public bool RELTokenSpecified {
			get {
				return this.rELTokenFieldSpecified;
			}
			set {
				this.rELTokenFieldSpecified = value;
				this.RaisePropertyChanged("RELTokenSpecified");
			}
		}

		/// <remarks/>
		[System.Xml.Serialization.XmlAttributeAttribute()]
		public int[] SupportedEAPMethods {
			get {
				return this.supportedEAPMethodsField;
			}
			set {
				this.supportedEAPMethodsField = value;
				this.RaisePropertyChanged("SupportedEAPMethods");
			}
		}

		/// <remarks/>
		[System.Xml.Serialization.XmlAnyAttributeAttribute()]
		public System.Xml.XmlAttribute[] AnyAttr {
			get {
				return this.anyAttrField;
			}
			set {
				this.anyAttrField = value;
				this.RaisePropertyChanged("AnyAttr");
			}
		}

		public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;

		protected void RaisePropertyChanged(string propertyName) {
			System.ComponentModel.PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
			if ((propertyChanged != null)) {
				propertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
			}
		}
	}

	/// <remarks/>
	[System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.0.30319.17929")]
	[System.SerializableAttribute()]
	[System.Diagnostics.DebuggerStepThroughAttribute()]
	[System.ComponentModel.DesignerCategoryAttribute("code")]
	[System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.onvif.org/ver10/device/wsdl")]
	public partial class SystemCapabilities1 : object, System.ComponentModel.INotifyPropertyChanged {

		private bool discoveryResolveField;

		private bool discoveryResolveFieldSpecified;

		private bool discoveryByeField;

		private bool discoveryByeFieldSpecified;

		private bool remoteDiscoveryField;

		private bool remoteDiscoveryFieldSpecified;

		private bool systemBackupField;

		private bool systemBackupFieldSpecified;

		private bool systemLoggingField;

		private bool systemLoggingFieldSpecified;

		private bool firmwareUpgradeField;

		private bool firmwareUpgradeFieldSpecified;

		private bool httpFirmwareUpgradeField;

		private bool httpFirmwareUpgradeFieldSpecified;

		private bool httpSystemBackupField;

		private bool httpSystemBackupFieldSpecified;

		private bool httpSystemLoggingField;

		private bool httpSystemLoggingFieldSpecified;

		private bool httpSupportInformationField;

		private bool httpSupportInformationFieldSpecified;

		private System.Xml.XmlAttribute[] anyAttrField;

		/// <remarks/>
		[System.Xml.Serialization.XmlAttributeAttribute()]
		public bool DiscoveryResolve {
			get {
				return this.discoveryResolveField;
			}
			set {
				this.discoveryResolveField = value;
				this.RaisePropertyChanged("DiscoveryResolve");
			}
		}

		/// <remarks/>
		[System.Xml.Serialization.XmlIgnoreAttribute()]
		public bool DiscoveryResolveSpecified {
			get {
				return this.discoveryResolveFieldSpecified;
			}
			set {
				this.discoveryResolveFieldSpecified = value;
				this.RaisePropertyChanged("DiscoveryResolveSpecified");
			}
		}

		/// <remarks/>
		[System.Xml.Serialization.XmlAttributeAttribute()]
		public bool DiscoveryBye {
			get {
				return this.discoveryByeField;
			}
			set {
				this.discoveryByeField = value;
				this.RaisePropertyChanged("DiscoveryBye");
			}
		}

		/// <remarks/>
		[System.Xml.Serialization.XmlIgnoreAttribute()]
		public bool DiscoveryByeSpecified {
			get {
				return this.discoveryByeFieldSpecified;
			}
			set {
				this.discoveryByeFieldSpecified = value;
				this.RaisePropertyChanged("DiscoveryByeSpecified");
			}
		}

		/// <remarks/>
		[System.Xml.Serialization.XmlAttributeAttribute()]
		public bool RemoteDiscovery {
			get {
				return this.remoteDiscoveryField;
			}
			set {
				this.remoteDiscoveryField = value;
				this.RaisePropertyChanged("RemoteDiscovery");
			}
		}

		/// <remarks/>
		[System.Xml.Serialization.XmlIgnoreAttribute()]
		public bool RemoteDiscoverySpecified {
			get {
				return this.remoteDiscoveryFieldSpecified;
			}
			set {
				this.remoteDiscoveryFieldSpecified = value;
				this.RaisePropertyChanged("RemoteDiscoverySpecified");
			}
		}

		/// <remarks/>
		[System.Xml.Serialization.XmlAttributeAttribute()]
		public bool SystemBackup {
			get {
				return this.systemBackupField;
			}
			set {
				this.systemBackupField = value;
				this.RaisePropertyChanged("SystemBackup");
			}
		}

		/// <remarks/>
		[System.Xml.Serialization.XmlIgnoreAttribute()]
		public bool SystemBackupSpecified {
			get {
				return this.systemBackupFieldSpecified;
			}
			set {
				this.systemBackupFieldSpecified = value;
				this.RaisePropertyChanged("SystemBackupSpecified");
			}
		}

		/// <remarks/>
		[System.Xml.Serialization.XmlAttributeAttribute()]
		public bool SystemLogging {
			get {
				return this.systemLoggingField;
			}
			set {
				this.systemLoggingField = value;
				this.RaisePropertyChanged("SystemLogging");
			}
		}

		/// <remarks/>
		[System.Xml.Serialization.XmlIgnoreAttribute()]
		public bool SystemLoggingSpecified {
			get {
				return this.systemLoggingFieldSpecified;
			}
			set {
				this.systemLoggingFieldSpecified = value;
				this.RaisePropertyChanged("SystemLoggingSpecified");
			}
		}

		/// <remarks/>
		[System.Xml.Serialization.XmlAttributeAttribute()]
		public bool FirmwareUpgrade {
			get {
				return this.firmwareUpgradeField;
			}
			set {
				this.firmwareUpgradeField = value;
				this.RaisePropertyChanged("FirmwareUpgrade");
			}
		}

		/// <remarks/>
		[System.Xml.Serialization.XmlIgnoreAttribute()]
		public bool FirmwareUpgradeSpecified {
			get {
				return this.firmwareUpgradeFieldSpecified;
			}
			set {
				this.firmwareUpgradeFieldSpecified = value;
				this.RaisePropertyChanged("FirmwareUpgradeSpecified");
			}
		}

		/// <remarks/>
		[System.Xml.Serialization.XmlAttributeAttribute()]
		public bool HttpFirmwareUpgrade {
			get {
				return this.httpFirmwareUpgradeField;
			}
			set {
				this.httpFirmwareUpgradeField = value;
				this.RaisePropertyChanged("HttpFirmwareUpgrade");
			}
		}

		/// <remarks/>
		[System.Xml.Serialization.XmlIgnoreAttribute()]
		public bool HttpFirmwareUpgradeSpecified {
			get {
				return this.httpFirmwareUpgradeFieldSpecified;
			}
			set {
				this.httpFirmwareUpgradeFieldSpecified = value;
				this.RaisePropertyChanged("HttpFirmwareUpgradeSpecified");
			}
		}

		/// <remarks/>
		[System.Xml.Serialization.XmlAttributeAttribute()]
		public bool HttpSystemBackup {
			get {
				return this.httpSystemBackupField;
			}
			set {
				this.httpSystemBackupField = value;
				this.RaisePropertyChanged("HttpSystemBackup");
			}
		}

		/// <remarks/>
		[System.Xml.Serialization.XmlIgnoreAttribute()]
		public bool HttpSystemBackupSpecified {
			get {
				return this.httpSystemBackupFieldSpecified;
			}
			set {
				this.httpSystemBackupFieldSpecified = value;
				this.RaisePropertyChanged("HttpSystemBackupSpecified");
			}
		}

		/// <remarks/>
		[System.Xml.Serialization.XmlAttributeAttribute()]
		public bool HttpSystemLogging {
			get {
				return this.httpSystemLoggingField;
			}
			set {
				this.httpSystemLoggingField = value;
				this.RaisePropertyChanged("HttpSystemLogging");
			}
		}

		/// <remarks/>
		[System.Xml.Serialization.XmlIgnoreAttribute()]
		public bool HttpSystemLoggingSpecified {
			get {
				return this.httpSystemLoggingFieldSpecified;
			}
			set {
				this.httpSystemLoggingFieldSpecified = value;
				this.RaisePropertyChanged("HttpSystemLoggingSpecified");
			}
		}

		/// <remarks/>
		[System.Xml.Serialization.XmlAttributeAttribute()]
		public bool HttpSupportInformation {
			get {
				return this.httpSupportInformationField;
			}
			set {
				this.httpSupportInformationField = value;
				this.RaisePropertyChanged("HttpSupportInformation");
			}
		}

		/// <remarks/>
		[System.Xml.Serialization.XmlIgnoreAttribute()]
		public bool HttpSupportInformationSpecified {
			get {
				return this.httpSupportInformationFieldSpecified;
			}
			set {
				this.httpSupportInformationFieldSpecified = value;
				this.RaisePropertyChanged("HttpSupportInformationSpecified");
			}
		}

		/// <remarks/>
		[System.Xml.Serialization.XmlAnyAttributeAttribute()]
		public System.Xml.XmlAttribute[] AnyAttr {
			get {
				return this.anyAttrField;
			}
			set {
				this.anyAttrField = value;
				this.RaisePropertyChanged("AnyAttr");
			}
		}

		public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;

		protected void RaisePropertyChanged(string propertyName) {
			System.ComponentModel.PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
			if ((propertyChanged != null)) {
				propertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
			}
		}
	}





	/// <remarks/>
	[System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.0.30319.17929")]
	[System.SerializableAttribute()]
	[System.Diagnostics.DebuggerStepThroughAttribute()]
	[System.ComponentModel.DesignerCategoryAttribute("code")]
	[System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.w3.org/2004/08/xop/include")]
	public partial class Include : object, System.ComponentModel.INotifyPropertyChanged {

		private System.Xml.XmlElement[] anyField;

		private string hrefField;

		private System.Xml.XmlAttribute[] anyAttrField;

		/// <remarks/>
		[System.Xml.Serialization.XmlAnyElementAttribute(Order = 0)]
		public System.Xml.XmlElement[] Any {
			get {
				return this.anyField;
			}
			set {
				this.anyField = value;
				this.RaisePropertyChanged("Any");
			}
		}

		/// <remarks/>
		[System.Xml.Serialization.XmlAttributeAttribute(DataType = "anyURI")]
		public string href {
			get {
				return this.hrefField;
			}
			set {
				this.hrefField = value;
				this.RaisePropertyChanged("href");
			}
		}

		/// <remarks/>
		[System.Xml.Serialization.XmlAnyAttributeAttribute()]
		public System.Xml.XmlAttribute[] AnyAttr {
			get {
				return this.anyAttrField;
			}
			set {
				this.anyAttrField = value;
				this.RaisePropertyChanged("AnyAttr");
			}
		}

		public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;

		protected void RaisePropertyChanged(string propertyName) {
			System.ComponentModel.PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
			if ((propertyChanged != null)) {
				propertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
			}
		}
	}






















	/// <remarks/>
	[System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.0.30319.17929")]
	[System.SerializableAttribute()]
	[System.Diagnostics.DebuggerStepThroughAttribute()]
	[System.ComponentModel.DesignerCategoryAttribute("code")]
	[System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://www.onvif.org/ver10/device/wsdl")]
	public partial class GetSystemUrisResponseExtension : object, System.ComponentModel.INotifyPropertyChanged {

		private System.Xml.XmlElement[] anyField;

		/// <remarks/>
		[System.Xml.Serialization.XmlAnyElementAttribute(Order = 0)]
		public System.Xml.XmlElement[] Any {
			get {
				return this.anyField;
			}
			set {
				this.anyField = value;
				this.RaisePropertyChanged("Any");
			}
		}

		public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;

		protected void RaisePropertyChanged(string propertyName) {
			System.ComponentModel.PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
			if ((propertyChanged != null)) {
				propertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
			}
		}
	}

	/// <remarks/>
	[System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.0.30319.17929")]
	[System.SerializableAttribute()]
	[System.Diagnostics.DebuggerStepThroughAttribute()]
	[System.ComponentModel.DesignerCategoryAttribute("code")]
	[System.Xml.Serialization.XmlTypeAttribute(TypeName = "Capabilities", Namespace = "http://www.onvif.org/ver10/replay/wsdl")]
	public partial class Capabilities10 : object, System.ComponentModel.INotifyPropertyChanged {

		private System.Xml.XmlElement[] anyField;

		private bool reversePlaybackField;

		private float[] sessionTimeoutRangeField;

		private System.Xml.XmlAttribute[] anyAttrField;

		public Capabilities10() {
			this.reversePlaybackField = false;
		}

		/// <remarks/>
		[System.Xml.Serialization.XmlAnyElementAttribute(Order = 0)]
		public System.Xml.XmlElement[] Any {
			get {
				return this.anyField;
			}
			set {
				this.anyField = value;
				this.RaisePropertyChanged("Any");
			}
		}

		/// <remarks/>
		[System.Xml.Serialization.XmlAttributeAttribute()]
		[System.ComponentModel.DefaultValueAttribute(false)]
		public bool ReversePlayback {
			get {
				return this.reversePlaybackField;
			}
			set {
				this.reversePlaybackField = value;
				this.RaisePropertyChanged("ReversePlayback");
			}
		}

		/// <remarks/>
		[System.Xml.Serialization.XmlAttributeAttribute()]
		public float[] SessionTimeoutRange {
			get {
				return this.sessionTimeoutRangeField;
			}
			set {
				this.sessionTimeoutRangeField = value;
				this.RaisePropertyChanged("SessionTimeoutRange");
			}
		}

		/// <remarks/>
		[System.Xml.Serialization.XmlAnyAttributeAttribute()]
		public System.Xml.XmlAttribute[] AnyAttr {
			get {
				return this.anyAttrField;
			}
			set {
				this.anyAttrField = value;
				this.RaisePropertyChanged("AnyAttr");
			}
		}

		public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;

		protected void RaisePropertyChanged(string propertyName) {
			System.ComponentModel.PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
			if ((propertyChanged != null)) {
				propertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
			}
		}
	}



	/// <remarks/>
	[System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.0.30319.17929")]
	[System.SerializableAttribute()]
	[System.Diagnostics.DebuggerStepThroughAttribute()]
	[System.ComponentModel.DesignerCategoryAttribute("code")]
	[System.Xml.Serialization.XmlTypeAttribute(TypeName = "Capabilities", Namespace = "http://www.onvif.org/ver10/analyticsdevice/wsdl")]
	public partial class Capabilities12 : object, System.ComponentModel.INotifyPropertyChanged {

		private System.Xml.XmlElement[] anyField;

		private System.Xml.XmlAttribute[] anyAttrField;

		/// <remarks/>
		[System.Xml.Serialization.XmlAnyElementAttribute(Order = 0)]
		public System.Xml.XmlElement[] Any {
			get {
				return this.anyField;
			}
			set {
				this.anyField = value;
				this.RaisePropertyChanged("Any");
			}
		}

		/// <remarks/>
		[System.Xml.Serialization.XmlAnyAttributeAttribute()]
		public System.Xml.XmlAttribute[] AnyAttr {
			get {
				return this.anyAttrField;
			}
			set {
				this.anyAttrField = value;
				this.RaisePropertyChanged("AnyAttr");
			}
		}

		public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;

		protected void RaisePropertyChanged(string propertyName) {
			System.ComponentModel.PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
			if ((propertyChanged != null)) {
				propertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
			}
		}
	}

	/// <remarks/>
	[System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.0.30319.17929")]
	[System.SerializableAttribute()]
	[System.Diagnostics.DebuggerStepThroughAttribute()]
	[System.ComponentModel.DesignerCategoryAttribute("code")]
	[System.Xml.Serialization.XmlTypeAttribute(TypeName = "Capabilities", Namespace = "http://www.onvif.org/ver10/receiver/wsdl")]
	public partial class Capabilities11 : object, System.ComponentModel.INotifyPropertyChanged {

		private System.Xml.XmlElement[] anyField;

		private bool rTP_MulticastField;

		private bool rTP_MulticastFieldSpecified;

		private bool rTP_TCPField;

		private bool rTP_TCPFieldSpecified;

		private bool rTP_RTSP_TCPField;

		private bool rTP_RTSP_TCPFieldSpecified;

		private int supportedReceiversField;

		private int maximumRTSPURILengthField;

		private bool maximumRTSPURILengthFieldSpecified;

		private System.Xml.XmlAttribute[] anyAttrField;

		/// <remarks/>
		[System.Xml.Serialization.XmlAnyElementAttribute(Order = 0)]
		public System.Xml.XmlElement[] Any {
			get {
				return this.anyField;
			}
			set {
				this.anyField = value;
				this.RaisePropertyChanged("Any");
			}
		}

		/// <remarks/>
		[System.Xml.Serialization.XmlAttributeAttribute()]
		public bool RTP_Multicast {
			get {
				return this.rTP_MulticastField;
			}
			set {
				this.rTP_MulticastField = value;
				this.RaisePropertyChanged("RTP_Multicast");
			}
		}

		/// <remarks/>
		[System.Xml.Serialization.XmlIgnoreAttribute()]
		public bool RTP_MulticastSpecified {
			get {
				return this.rTP_MulticastFieldSpecified;
			}
			set {
				this.rTP_MulticastFieldSpecified = value;
				this.RaisePropertyChanged("RTP_MulticastSpecified");
			}
		}

		/// <remarks/>
		[System.Xml.Serialization.XmlAttributeAttribute()]
		public bool RTP_TCP {
			get {
				return this.rTP_TCPField;
			}
			set {
				this.rTP_TCPField = value;
				this.RaisePropertyChanged("RTP_TCP");
			}
		}

		/// <remarks/>
		[System.Xml.Serialization.XmlIgnoreAttribute()]
		public bool RTP_TCPSpecified {
			get {
				return this.rTP_TCPFieldSpecified;
			}
			set {
				this.rTP_TCPFieldSpecified = value;
				this.RaisePropertyChanged("RTP_TCPSpecified");
			}
		}

		/// <remarks/>
		[System.Xml.Serialization.XmlAttributeAttribute()]
		public bool RTP_RTSP_TCP {
			get {
				return this.rTP_RTSP_TCPField;
			}
			set {
				this.rTP_RTSP_TCPField = value;
				this.RaisePropertyChanged("RTP_RTSP_TCP");
			}
		}

		/// <remarks/>
		[System.Xml.Serialization.XmlIgnoreAttribute()]
		public bool RTP_RTSP_TCPSpecified {
			get {
				return this.rTP_RTSP_TCPFieldSpecified;
			}
			set {
				this.rTP_RTSP_TCPFieldSpecified = value;
				this.RaisePropertyChanged("RTP_RTSP_TCPSpecified");
			}
		}

		/// <remarks/>
		[System.Xml.Serialization.XmlAttributeAttribute()]
		public int SupportedReceivers {
			get {
				return this.supportedReceiversField;
			}
			set {
				this.supportedReceiversField = value;
				this.RaisePropertyChanged("SupportedReceivers");
			}
		}

		/// <remarks/>
		[System.Xml.Serialization.XmlAttributeAttribute()]
		public int MaximumRTSPURILength {
			get {
				return this.maximumRTSPURILengthField;
			}
			set {
				this.maximumRTSPURILengthField = value;
				this.RaisePropertyChanged("MaximumRTSPURILength");
			}
		}

		/// <remarks/>
		[System.Xml.Serialization.XmlIgnoreAttribute()]
		public bool MaximumRTSPURILengthSpecified {
			get {
				return this.maximumRTSPURILengthFieldSpecified;
			}
			set {
				this.maximumRTSPURILengthFieldSpecified = value;
				this.RaisePropertyChanged("MaximumRTSPURILengthSpecified");
			}
		}

		/// <remarks/>
		[System.Xml.Serialization.XmlAnyAttributeAttribute()]
		public System.Xml.XmlAttribute[] AnyAttr {
			get {
				return this.anyAttrField;
			}
			set {
				this.anyAttrField = value;
				this.RaisePropertyChanged("AnyAttr");
			}
		}

		public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;

		protected void RaisePropertyChanged(string propertyName) {
			System.ComponentModel.PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
			if ((propertyChanged != null)) {
				propertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
			}
		}
	}


	/// <remarks/>
	[System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.0.30319.17929")]
	[System.SerializableAttribute()]
	[System.Diagnostics.DebuggerStepThroughAttribute()]
	[System.ComponentModel.DesignerCategoryAttribute("code")]
	[System.Xml.Serialization.XmlTypeAttribute(TypeName = "Capabilities", Namespace = "http://www.onvif.org/ver10/search/wsdl")]
	public partial class Capabilities8 : object, System.ComponentModel.INotifyPropertyChanged {

		private System.Xml.XmlElement[] anyField;

		private bool metadataSearchField;

		private bool metadataSearchFieldSpecified;

		private System.Xml.XmlAttribute[] anyAttrField;

		/// <remarks/>
		[System.Xml.Serialization.XmlAnyElementAttribute(Order = 0)]
		public System.Xml.XmlElement[] Any {
			get {
				return this.anyField;
			}
			set {
				this.anyField = value;
				this.RaisePropertyChanged("Any");
			}
		}

		/// <remarks/>
		[System.Xml.Serialization.XmlAttributeAttribute()]
		public bool MetadataSearch {
			get {
				return this.metadataSearchField;
			}
			set {
				this.metadataSearchField = value;
				this.RaisePropertyChanged("MetadataSearch");
			}
		}

		/// <remarks/>
		[System.Xml.Serialization.XmlIgnoreAttribute()]
		public bool MetadataSearchSpecified {
			get {
				return this.metadataSearchFieldSpecified;
			}
			set {
				this.metadataSearchFieldSpecified = value;
				this.RaisePropertyChanged("MetadataSearchSpecified");
			}
		}

		/// <remarks/>
		[System.Xml.Serialization.XmlAnyAttributeAttribute()]
		public System.Xml.XmlAttribute[] AnyAttr {
			get {
				return this.anyAttrField;
			}
			set {
				this.anyAttrField = value;
				this.RaisePropertyChanged("AnyAttr");
			}
		}

		public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;

		protected void RaisePropertyChanged(string propertyName) {
			System.ComponentModel.PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
			if ((propertyChanged != null)) {
				propertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
			}
		}
	}







	/// <remarks/>
	[System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.0.30319.17929")]
	[System.SerializableAttribute()]
	[System.Diagnostics.DebuggerStepThroughAttribute()]
	[System.ComponentModel.DesignerCategoryAttribute("code")]
	[System.Xml.Serialization.XmlTypeAttribute(TypeName = "Capabilities", Namespace = "http://www.onvif.org/ver10/display/wsdl")]
	public partial class Capabilities9 : object, System.ComponentModel.INotifyPropertyChanged {

		private System.Xml.XmlElement[] anyField;

		private bool fixedLayoutField;

		private bool fixedLayoutFieldSpecified;

		private System.Xml.XmlAttribute[] anyAttrField;

		/// <remarks/>
		[System.Xml.Serialization.XmlAnyElementAttribute(Order = 0)]
		public System.Xml.XmlElement[] Any {
			get {
				return this.anyField;
			}
			set {
				this.anyField = value;
				this.RaisePropertyChanged("Any");
			}
		}

		/// <remarks/>
		[System.Xml.Serialization.XmlAttributeAttribute()]
		public bool FixedLayout {
			get {
				return this.fixedLayoutField;
			}
			set {
				this.fixedLayoutField = value;
				this.RaisePropertyChanged("FixedLayout");
			}
		}

		/// <remarks/>
		[System.Xml.Serialization.XmlIgnoreAttribute()]
		public bool FixedLayoutSpecified {
			get {
				return this.fixedLayoutFieldSpecified;
			}
			set {
				this.fixedLayoutFieldSpecified = value;
				this.RaisePropertyChanged("FixedLayoutSpecified");
			}
		}

		/// <remarks/>
		[System.Xml.Serialization.XmlAnyAttributeAttribute()]
		public System.Xml.XmlAttribute[] AnyAttr {
			get {
				return this.anyAttrField;
			}
			set {
				this.anyAttrField = value;
				this.RaisePropertyChanged("AnyAttr");
			}
		}

		public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;

		protected void RaisePropertyChanged(string propertyName) {
			System.ComponentModel.PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
			if ((propertyChanged != null)) {
				propertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
			}
		}
	}





	/// <remarks/>
	[System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.0.30319.17929")]
	[System.SerializableAttribute()]
	[System.Diagnostics.DebuggerStepThroughAttribute()]
	[System.ComponentModel.DesignerCategoryAttribute("code")]
	[System.Xml.Serialization.XmlTypeAttribute(TypeName = "Capabilities", Namespace = "http://www.onvif.org/ver20/imaging/wsdl")]
	public partial class Capabilities3 : object, System.ComponentModel.INotifyPropertyChanged {

		private System.Xml.XmlElement[] anyField;

		private System.Xml.XmlAttribute[] anyAttrField;

		/// <remarks/>
		[System.Xml.Serialization.XmlAnyElementAttribute(Order = 0)]
		public System.Xml.XmlElement[] Any {
			get {
				return this.anyField;
			}
			set {
				this.anyField = value;
				this.RaisePropertyChanged("Any");
			}
		}


		/// <remarks/>
		[System.Xml.Serialization.XmlAnyAttributeAttribute()]
		public System.Xml.XmlAttribute[] AnyAttr {
			get {
				return this.anyAttrField;
			}
			set {
				this.anyAttrField = value;
				this.RaisePropertyChanged("AnyAttr");
			}
		}

		public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;

		protected void RaisePropertyChanged(string propertyName) {
			System.ComponentModel.PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
			if ((propertyChanged != null)) {
				propertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
			}
		}
	}




	/// <remarks/>
	[System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.0.30319.17929")]
	[System.SerializableAttribute()]
	[System.Diagnostics.DebuggerStepThroughAttribute()]
	[System.ComponentModel.DesignerCategoryAttribute("code")]
	[System.Xml.Serialization.XmlTypeAttribute(TypeName = "Capabilities", Namespace = "http://www.onvif.org/ver20/ptz/wsdl")]
	public partial class Capabilities1 : object, System.ComponentModel.INotifyPropertyChanged {

		private System.Xml.XmlElement[] anyField;

		private System.Xml.XmlAttribute[] anyAttrField;

		/// <remarks/>
		[System.Xml.Serialization.XmlAnyElementAttribute(Order = 0)]
		public System.Xml.XmlElement[] Any {
			get {
				return this.anyField;
			}
			set {
				this.anyField = value;
				this.RaisePropertyChanged("Any");
			}
		}



		/// <remarks/>
		[System.Xml.Serialization.XmlAnyAttributeAttribute()]
		public System.Xml.XmlAttribute[] AnyAttr {
			get {
				return this.anyAttrField;
			}
			set {
				this.anyAttrField = value;
				this.RaisePropertyChanged("AnyAttr");
			}
		}

		public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;

		protected void RaisePropertyChanged(string propertyName) {
			System.ComponentModel.PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
			if ((propertyChanged != null)) {
				propertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
			}
		}
	}










	/// <remarks/>
	[System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.0.30319.17929")]
	[System.SerializableAttribute()]
	[System.Diagnostics.DebuggerStepThroughAttribute()]
	[System.ComponentModel.DesignerCategoryAttribute("code")]
	[System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://www.onvif.org/ver10/media/wsdl")]
	public partial class GetServiceCapabilities : object, System.ComponentModel.INotifyPropertyChanged {

		public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;

		protected void RaisePropertyChanged(string propertyName) {
			System.ComponentModel.PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
			if ((propertyChanged != null)) {
				propertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
			}
		}
	}

	/// <remarks/>
	[System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.0.30319.17929")]
	[System.SerializableAttribute()]
	[System.Diagnostics.DebuggerStepThroughAttribute()]
	[System.ComponentModel.DesignerCategoryAttribute("code")]
	[System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://www.onvif.org/ver10/media/wsdl")]
	public partial class GetServiceCapabilitiesResponse : object, System.ComponentModel.INotifyPropertyChanged {

		private MediaServiceCapabilities capabilitiesField;

		/// <remarks/>
		[XmlElement(Order = 0)]
		public MediaServiceCapabilities Capabilities {
			get {
				return this.capabilitiesField;
			}
			set {
				this.capabilitiesField = value;
				this.RaisePropertyChanged("Capabilities");
			}
		}

		public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;

		protected void RaisePropertyChanged(string propertyName) {
			System.ComponentModel.PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
			if ((propertyChanged != null)) {
				propertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
			}
		}
	}

	/// <remarks/>
	[System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.0.30319.17929")]
	[System.SerializableAttribute()]
	[System.Diagnostics.DebuggerStepThroughAttribute()]
	[System.ComponentModel.DesignerCategoryAttribute("code")]
	[System.Xml.Serialization.XmlTypeAttribute(TypeName = "Capabilities", Namespace = "http://www.onvif.org/ver10/media/wsdl")]
	public partial class MediaServiceCapabilities : object, System.ComponentModel.INotifyPropertyChanged {

		private ProfileCapabilities1 profileCapabilitiesField;

		private StreamingCapabilities streamingCapabilitiesField;

		private System.Xml.XmlElement[] anyField;

		private bool snapshotUriField;

		private bool snapshotUriFieldSpecified;


		private System.Xml.XmlAttribute[] anyAttrField;

		/// <remarks/>
		[XmlElement(Order = 0)]
		public ProfileCapabilities1 ProfileCapabilities {
			get {
				return this.profileCapabilitiesField;
			}
			set {
				this.profileCapabilitiesField = value;
				this.RaisePropertyChanged("ProfileCapabilities");
			}
		}

		/// <remarks/>
		[XmlElement(Order = 1)]
		public StreamingCapabilities StreamingCapabilities {
			get {
				return this.streamingCapabilitiesField;
			}
			set {
				this.streamingCapabilitiesField = value;
				this.RaisePropertyChanged("StreamingCapabilities");
			}
		}

		/// <remarks/>
		[System.Xml.Serialization.XmlAnyElementAttribute(Order = 2)]
		public System.Xml.XmlElement[] Any {
			get {
				return this.anyField;
			}
			set {
				this.anyField = value;
				this.RaisePropertyChanged("Any");
			}
		}

		/// <remarks/>
		[System.Xml.Serialization.XmlAttributeAttribute()]
		public bool SnapshotUri {
			get {
				return this.snapshotUriField;
			}
			set {
				this.snapshotUriField = value;
				this.RaisePropertyChanged("SnapshotUri");
			}
		}

		/// <remarks/>
		[System.Xml.Serialization.XmlIgnoreAttribute()]
		public bool SnapshotUriSpecified {
			get {
				return this.snapshotUriFieldSpecified;
			}
			set {
				this.snapshotUriFieldSpecified = value;
				this.RaisePropertyChanged("SnapshotUriSpecified");
			}
		}

		/// <remarks/>
		[System.Xml.Serialization.XmlAnyAttributeAttribute()]
		public System.Xml.XmlAttribute[] AnyAttr {
			get {
				return this.anyAttrField;
			}
			set {
				this.anyAttrField = value;
				this.RaisePropertyChanged("AnyAttr");
			}
		}

		public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;

		protected void RaisePropertyChanged(string propertyName) {
			System.ComponentModel.PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
			if ((propertyChanged != null)) {
				propertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
			}
		}
	}


	/// <remarks/>
	[System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.0.30319.17929")]
	[System.SerializableAttribute()]
	[System.Diagnostics.DebuggerStepThroughAttribute()]
	[System.ComponentModel.DesignerCategoryAttribute("code")]
	[System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.onvif.org/ver10/media/wsdl")]
	public partial class StreamingCapabilities : object, System.ComponentModel.INotifyPropertyChanged {

		private System.Xml.XmlElement[] anyField;

		private bool rTPMulticastField;

		private bool rTPMulticastFieldSpecified;

		private bool rTP_TCPField;

		private bool rTP_TCPFieldSpecified;

		private bool rTP_RTSP_TCPField;

		private bool rTP_RTSP_TCPFieldSpecified;

		private bool nonAggregateControlField;

		private bool nonAggregateControlFieldSpecified;

		private System.Xml.XmlAttribute[] anyAttrField;

		/// <remarks/>
		[System.Xml.Serialization.XmlAnyElementAttribute(Order = 0)]
		public System.Xml.XmlElement[] Any {
			get {
				return this.anyField;
			}
			set {
				this.anyField = value;
				this.RaisePropertyChanged("Any");
			}
		}

		/// <remarks/>
		[System.Xml.Serialization.XmlAttributeAttribute()]
		public bool RTPMulticast {
			get {
				return this.rTPMulticastField;
			}
			set {
				this.rTPMulticastField = value;
				this.RaisePropertyChanged("RTPMulticast");
			}
		}

		/// <remarks/>
		[System.Xml.Serialization.XmlIgnoreAttribute()]
		public bool RTPMulticastSpecified {
			get {
				return this.rTPMulticastFieldSpecified;
			}
			set {
				this.rTPMulticastFieldSpecified = value;
				this.RaisePropertyChanged("RTPMulticastSpecified");
			}
		}

		/// <remarks/>
		[System.Xml.Serialization.XmlAttributeAttribute()]
		public bool RTP_TCP {
			get {
				return this.rTP_TCPField;
			}
			set {
				this.rTP_TCPField = value;
				this.RaisePropertyChanged("RTP_TCP");
			}
		}

		/// <remarks/>
		[System.Xml.Serialization.XmlIgnoreAttribute()]
		public bool RTP_TCPSpecified {
			get {
				return this.rTP_TCPFieldSpecified;
			}
			set {
				this.rTP_TCPFieldSpecified = value;
				this.RaisePropertyChanged("RTP_TCPSpecified");
			}
		}

		/// <remarks/>
		[System.Xml.Serialization.XmlAttributeAttribute()]
		public bool RTP_RTSP_TCP {
			get {
				return this.rTP_RTSP_TCPField;
			}
			set {
				this.rTP_RTSP_TCPField = value;
				this.RaisePropertyChanged("RTP_RTSP_TCP");
			}
		}

		/// <remarks/>
		[System.Xml.Serialization.XmlIgnoreAttribute()]
		public bool RTP_RTSP_TCPSpecified {
			get {
				return this.rTP_RTSP_TCPFieldSpecified;
			}
			set {
				this.rTP_RTSP_TCPFieldSpecified = value;
				this.RaisePropertyChanged("RTP_RTSP_TCPSpecified");
			}
		}

		/// <remarks/>
		[System.Xml.Serialization.XmlAttributeAttribute()]
		public bool NonAggregateControl {
			get {
				return this.nonAggregateControlField;
			}
			set {
				this.nonAggregateControlField = value;
				this.RaisePropertyChanged("NonAggregateControl");
			}
		}

		/// <remarks/>
		[System.Xml.Serialization.XmlIgnoreAttribute()]
		public bool NonAggregateControlSpecified {
			get {
				return this.nonAggregateControlFieldSpecified;
			}
			set {
				this.nonAggregateControlFieldSpecified = value;
				this.RaisePropertyChanged("NonAggregateControlSpecified");
			}
		}

		/// <remarks/>
		[System.Xml.Serialization.XmlAnyAttributeAttribute()]
		public System.Xml.XmlAttribute[] AnyAttr {
			get {
				return this.anyAttrField;
			}
			set {
				this.anyAttrField = value;
				this.RaisePropertyChanged("AnyAttr");
			}
		}

		public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;

		protected void RaisePropertyChanged(string propertyName) {
			System.ComponentModel.PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
			if ((propertyChanged != null)) {
				propertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
			}
		}
	}










}