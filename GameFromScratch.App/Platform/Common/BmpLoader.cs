namespace GameFromScratch.App.Platform.Common
{
    internal class BmpLoader
    {
        public static int[] Load(string texture)
        {
            var path = Path.Combine("Assets", "Textures", $"{texture}.bmp");

            using (var stream = File.Open(path, FileMode.Open))
            using (var reader = new BinaryReader(stream))
            {
                /* BMP Header */
                reader.ReadBytes(2); // BM
                reader.ReadBytes(4); // file size
                reader.ReadBytes(2); // application specific, unused
                reader.ReadBytes(2); // application specific, unused
                var offset = reader.ReadInt32(); // offset to where the pixel data starts

                /* DIB Header */
                reader.ReadBytes(4); // DIB header size
                var width = reader.ReadInt32(); // bitmap width
                var height = reader.ReadInt32(); // bitmap height
                // the remaining DIB header fields are ignored

                // skip until pixel data
                var numReadBytes = 26; // 14 BMP header + 12 DIB header
                var skip = offset - numReadBytes;
                reader.ReadBytes(skip);

                /* Pixel data */
                var buffer = new int[width * height];
                for (var y = height - 1; y >= 0; y--) // BMP data starts from the bottom left
                {
                    for (var x = 0; x < width; x++)
                    {
                        // assumes 32 bit depth and BGRA byte order
                        var b = reader.ReadByte();
                        var g = reader.ReadByte();
                        var r = reader.ReadByte();
                        var a = reader.ReadByte();

                        // convert to ARGB as it is used in System.Drawing.Color
                        var argb = (a << 24) | (r << 16) | (g << 8) | b;

                        var index = y * width + x;
                        buffer[index] = argb;
                    }
                }

                return buffer;
            }
        }
    }
}
