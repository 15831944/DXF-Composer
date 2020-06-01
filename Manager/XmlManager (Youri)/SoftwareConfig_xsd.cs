
namespace XmlSystem
{

    /// <remarks/>
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "", IsNullable = false)]
    public partial class SoftwareConfig
    {

        private SoftwareConfigPaths pathsField;

        private SoftwareConfigStandardValues standardValuesField;

        /// <remarks/>
        public SoftwareConfigPaths paths
        {
            get
            {
                return this.pathsField;
            }
            set
            {
                this.pathsField = value;
            }
        }

        /// <remarks/>
        public SoftwareConfigStandardValues standardValues
        {
            get
            {
                return this.standardValuesField;
            }
            set
            {
                this.standardValuesField = value;
            }
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public partial class SoftwareConfigPaths
    {

        private SoftwareConfigPathsFile[] installedFontsField;

        private SoftwareConfigPathsFile1[] additionalFontsField;

        private string htwLogoOskarField;

        private string htwLogoBrandonField;

        private string apelTestField;

        private string testDxfField;

        private string weitererTestDxfField;

        /// <remarks/>
        [System.Xml.Serialization.XmlArrayItemAttribute("File", IsNullable = false)]
        public SoftwareConfigPathsFile[] installedFonts
        {
            get
            {
                return this.installedFontsField;
            }
            set
            {
                this.installedFontsField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlArrayItemAttribute("File", IsNullable = false)]
        public SoftwareConfigPathsFile1[] additionalFonts
        {
            get
            {
                return this.additionalFontsField;
            }
            set
            {
                this.additionalFontsField = value;
            }
        }

        /// <remarks/>
        public string HtwLogoOskar
        {
            get
            {
                return this.htwLogoOskarField;
            }
            set
            {
                this.htwLogoOskarField = value;
            }
        }

        /// <remarks/>
        public string HtwLogoBrandon
        {
            get
            {
                return this.htwLogoBrandonField;
            }
            set
            {
                this.htwLogoBrandonField = value;
            }
        }

        /// <remarks/>
        public string ApelTest
        {
            get
            {
                return this.apelTestField;
            }
            set
            {
                this.apelTestField = value;
            }
        }

        /// <remarks/>
        public string TestDxf
        {
            get
            {
                return this.testDxfField;
            }
            set
            {
                this.testDxfField = value;
            }
        }

        /// <remarks/>
        public string weitererTestDxf
        {
            get
            {
                return this.weitererTestDxfField;
            }
            set
            {
                this.weitererTestDxfField = value;
            }
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public partial class SoftwareConfigPathsFile
    {

        private string typeField;

        private string valueField;

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string type
        {
            get
            {
                return this.typeField;
            }
            set
            {
                this.typeField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlTextAttribute()]
        public string Value
        {
            get
            {
                return this.valueField;
            }
            set
            {
                this.valueField = value;
            }
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public partial class SoftwareConfigPathsFile1
    {

        private string typeField;

        private string valueField;

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string type
        {
            get
            {
                return this.typeField;
            }
            set
            {
                this.typeField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlTextAttribute()]
        public string Value
        {
            get
            {
                return this.valueField;
            }
            set
            {
                this.valueField = value;
            }
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public partial class SoftwareConfigStandardValues
    {

        private SoftwareConfigStandardValuesText textField;

        private SoftwareConfigStandardValuesDrawing drawingField;

        private SoftwareConfigStandardValuesImageManager imageManagerField;

        /// <remarks/>
        public SoftwareConfigStandardValuesText text
        {
            get
            {
                return this.textField;
            }
            set
            {
                this.textField = value;
            }
        }

        /// <remarks/>
        public SoftwareConfigStandardValuesDrawing drawing
        {
            get
            {
                return this.drawingField;
            }
            set
            {
                this.drawingField = value;
            }
        }

        /// <remarks/>
        public SoftwareConfigStandardValuesImageManager ImageManager
        {
            get
            {
                return this.imageManagerField;
            }
            set
            {
                this.imageManagerField = value;
            }
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public partial class SoftwareConfigStandardValuesText
    {

        private string tB_incomingTextField;

        private string cB_fontFamilyField;

        private byte cB_fontSizeField;

        /// <remarks/>
        public string tB_incomingText
        {
            get
            {
                return this.tB_incomingTextField;
            }
            set
            {
                this.tB_incomingTextField = value;
            }
        }

        /// <remarks/>
        public string cB_fontFamily
        {
            get
            {
                return this.cB_fontFamilyField;
            }
            set
            {
                this.cB_fontFamilyField = value;
            }
        }

        /// <remarks/>
        public byte cB_fontSize
        {
            get
            {
                return this.cB_fontSizeField;
            }
            set
            {
                this.cB_fontSizeField = value;
            }
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public partial class SoftwareConfigStandardValuesDrawing
    {

        private string blackStrokeField;

        private byte strokeThicknessField;

        /// <remarks/>
        public string blackStroke
        {
            get
            {
                return this.blackStrokeField;
            }
            set
            {
                this.blackStrokeField = value;
            }
        }

        /// <remarks/>
        public byte strokeThickness
        {
            get
            {
                return this.strokeThicknessField;
            }
            set
            {
                this.strokeThicknessField = value;
            }
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public partial class SoftwareConfigStandardValuesImageManager
    {

        private byte thresholdField;

        private decimal curveToleranceField;

        private bool curveoptimazingField;

        private byte alphamaxField;

        private byte ignoreAreaPixelField;

        private string penColorField;

        private byte penWidthField;

        /// <remarks/>
        public byte threshold
        {
            get
            {
                return this.thresholdField;
            }
            set
            {
                this.thresholdField = value;
            }
        }

        /// <remarks/>
        public decimal curveTolerance
        {
            get
            {
                return this.curveToleranceField;
            }
            set
            {
                this.curveToleranceField = value;
            }
        }

        /// <remarks/>
        public bool curveoptimazing
        {
            get
            {
                return this.curveoptimazingField;
            }
            set
            {
                this.curveoptimazingField = value;
            }
        }

        /// <remarks/>
        public byte Alphamax
        {
            get
            {
                return this.alphamaxField;
            }
            set
            {
                this.alphamaxField = value;
            }
        }

        /// <remarks/>
        public byte ignoreAreaPixel
        {
            get
            {
                return this.ignoreAreaPixelField;
            }
            set
            {
                this.ignoreAreaPixelField = value;
            }
        }

        /// <remarks/>
        public string penColor
        {
            get
            {
                return this.penColorField;
            }
            set
            {
                this.penColorField = value;
            }
        }

        /// <remarks/>
        public byte penWidth
        {
            get
            {
                return this.penWidthField;
            }
            set
            {
                this.penWidthField = value;
            }
        }
    }

}