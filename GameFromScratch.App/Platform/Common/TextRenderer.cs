using FreeTypeSharp;
using System.Runtime.InteropServices;
using static FreeTypeSharp.FT;
using static FreeTypeSharp.FT_LOAD;

namespace GameFromScratch.App.Platform.Common
{
    /// <summary>
    /// Renders individual character glyphs.
    /// </summary>
    internal unsafe class TextRenderer
    {
        private FT_LibraryRec_* lib;
        private FT_FaceRec_* face;

        private bool didInit;
        private int fontSize;
        private string fontName;
        private readonly Dictionary<(char Character, int FontSize), GlyphBitmap> glyphCache;

        public int FontSize { get => fontSize; }


        public TextRenderer()
        {
            didInit = false;
            fontSize = 16;
            fontName = "LiberationSans-Regular";
            glyphCache = new();
        }

        private unsafe void Initialize()
        {
            fixed (FT_LibraryRec_** libRef = &lib)
            {
                var error = FT_Init_FreeType(libRef);
                ThrowIfNotOk(error, "Failed to initialize");
            }
            didInit = true;
        }

        public void LoadFont()
        {
            if (!didInit)
            {
                Initialize();
            }

            var fontPath = Path.Combine("Assets", "Fonts", $"{fontName}.ttf");
            fixed (FT_FaceRec_** faceRef = &face)
            {
                var error = FT_New_Face(lib, (byte*)Marshal.StringToHGlobalAnsi(fontPath), 0, faceRef);
                ThrowIfNotOk(error, "Failed to load font");

                SetFontSize(fontSize);
            }
        }

        public void SetFontSize(int fontSize)
        {
            // TODO(improvement): don't hard code DPI here
            var dpi = 96u;
            var error = FT_Set_Char_Size(face, 0, fontSize * 64, dpi, dpi);
            ThrowIfNotOk(error, "Failed to set font size");

            this.fontSize = fontSize;
        }

        public GlyphBitmap DrawCharacter(char character)
        {
            if (glyphCache.TryGetValue((character, fontSize), out var glyphBitmap))
            {
                return glyphBitmap;
            }

            var error = FT_Load_Char(face, character, FT_LOAD_RENDER);
            ThrowIfNotOk(error, "Failed to draw character");

            var bitmap = face->glyph->bitmap;
            var glyphPixelData = bitmap.buffer;

            var output = new byte[bitmap.width * bitmap.rows];
            for (var i = 0; i < output.Length; i++)
            {
                output[i] = glyphPixelData[i];
            }

            var result = new GlyphBitmap
            {
                Buffer = output,
                Width = (int)bitmap.width,
                Height = (int)bitmap.rows,
                AdvanceX = (int)face->glyph->advance.x >> 6,
                Top = face->glyph->bitmap_top,
                Left = face->glyph->bitmap_left,
            };
            // TODO(leak): if a lot of different font sizes are used, this will keep growing
            glyphCache.Add((character, fontSize), result);

            return result;
        }

        private static void ThrowIfNotOk(FT_Error error, string message)
        {
            if (error != FT_Error.FT_Err_Ok)
            {
                throw new Exception($"FreeType Error - {message} - Error Code: {error}");
            }
        }

    }
}
