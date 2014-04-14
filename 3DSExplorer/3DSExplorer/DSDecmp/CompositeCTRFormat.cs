namespace DSDecmp.Formats.Nitro
{
    /// <summary>
    /// A composite format with all formats supported natively by the NDS (but not LZ-Overlay)
    /// </summary>
    public class CompositeCTRFormat : CompositeFormat
    {
        /// <summary>
        /// Creates a new instance of the format composed of all native NDS compression formats.
        /// </summary>
        public CompositeCTRFormat()
            : base(new Huffman4(), new Huffman8(), new LZ10(), new LZ11(), new LZOvl()) { }

        /// <summary>
        /// Gets a short string identifying this compression format.
        /// </summary>
        public override string ShortFormatString
        {
            get { return "CTR"; }
        }

        /// <summary>
        /// Gets a short description of this compression format (used in the program usage).
        /// </summary>
        public override string Description
        {
            get { return "All formats natively supported by the CTR."; }
        }

        /// <summary>
        /// Gets if this format supports compressing a file.
        /// </summary>
        public override bool SupportsCompression
        {
            get { return true; }
        }

        /// <summary>
        /// Gets the value that must be given on the command line in order to compress using this format.
        /// </summary>
        public override string CompressionFlag
        {
            get { return "ctr*"; }
        }

        public CompressionFormat GetCompression(int index)
        {
            return formats[index];

        }
    }
}
