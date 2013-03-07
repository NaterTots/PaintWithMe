using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace PaintWithMe
{
    class TextureManager : IService
    {
        Dictionary<string, Texture2D> textures = new Dictionary<string, Texture2D>();

        public void Initialize()
        {
            Game gameInstance = ServiceManager.Instance.GetService<Game>(ServiceType.Game);

            textures.Add("testsquare", gameInstance.Content.Load<Texture2D>("testsquare"));
            textures.Add("watercolor_left", gameInstance.Content.Load<Texture2D>("watercolor_left"));
            textures.Add("watercolor_right", gameInstance.Content.Load<Texture2D>("watercolor_right"));
            textures.Add("stroke_blur", gameInstance.Content.Load<Texture2D>("stroke_blur"));
            textures.Add("stroke_blur_left", gameInstance.Content.Load<Texture2D>("stroke_blur_left"));
        }

        public ServiceType GetServiceType()
        {
            return ServiceType.TextureManager;
        }

        public Texture2D GetTexture(string name)
        {
            return textures[name];
        }
    }
}
