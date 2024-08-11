namespace GameFromScratch.App.Platform.Common.Textures
{
    internal class Texture
    {
        public readonly int Width;
        public readonly int Height;
        public readonly int[] Buffer;
        public readonly string Name;

        public Texture(int width, int height, int[] buffer, string name)
        {
            Width = width;
            Height = height;
            Buffer = buffer;
            Name = name;
        }
    }
}
