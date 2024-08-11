namespace GameFromScratch.App.Platform.Common.Textures
{
    // TODO(feature): Loads separate images for now. Modify this to have a texture atlas
    internal class TextureManager
    {
        private readonly IImageLoader imageLoader;
        private readonly Dictionary<string, Texture> textures;

        public TextureManager()
        {
            textures = new Dictionary<string, Texture>();
            imageLoader = new BmpLoader();
        }

        public void Load(string textureName)
        {
            var path = Path.Combine("Assets", "Textures", $"{textureName}{imageLoader.FileExtension}");
            var (width, height, buffer) = imageLoader.Load(path);

            var texture = new Texture(width, height, buffer, textureName);
            textures.Add(textureName, texture);
        }

        public Texture GetTexture(string textureName)
        {
            if (!textures.TryGetValue(textureName, out var texture))
            {
                Load(textureName);
                return textures[textureName];
            }
            else
            {
                return texture;
            }
        }
    }
}
