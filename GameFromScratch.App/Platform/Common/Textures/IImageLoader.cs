namespace GameFromScratch.App.Platform.Common.Textures
{
    internal interface IImageLoader
    {
        /// <summary>
        /// Loads image data as a buffer of ARGB pixels
        /// </summary>
        public (int Width, int Height, int[] Buffer) Load(string path);

        public string FileExtension { get; }
    }
}
