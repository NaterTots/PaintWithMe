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

            textures.Add("cartoon\\fireworks1", gameInstance.Content.Load<Texture2D>("cartoon\\fireworks1"));
            textures.Add("cartoon\\fireworks2", gameInstance.Content.Load<Texture2D>("cartoon\\fireworks2"));
            textures.Add("cartoon\\fireworks3", gameInstance.Content.Load<Texture2D>("cartoon\\fireworks3"));
            textures.Add("cartoon\\fireworks4", gameInstance.Content.Load<Texture2D>("cartoon\\fireworks4"));

            textures.Add("cartoon\\square", gameInstance.Content.Load<Texture2D>("cartoon\\square"));
            textures.Add("cartoon\\triangle", gameInstance.Content.Load<Texture2D>("cartoon\\triangle"));
            textures.Add("cartoon\\circle", gameInstance.Content.Load<Texture2D>("cartoon\\circle"));
            textures.Add("cartoon\\star", gameInstance.Content.Load<Texture2D>("cartoon\\star"));
            textures.Add("cartoon\\heart", gameInstance.Content.Load<Texture2D>("cartoon\\heart"));

            textures.Add("cartoon\\Spiral", gameInstance.Content.Load<Texture2D>("cartoon\\Spiral"));
            textures.Add("cartoon\\grass", gameInstance.Content.Load<Texture2D>("cartoon\\grass"));
            textures.Add("cartoon\\quickstroke_upward", gameInstance.Content.Load<Texture2D>("cartoon\\quickstroke_upward"));
            textures.Add("cartoon\\quickstroke_upward2", gameInstance.Content.Load<Texture2D>("cartoon\\quickstroke_upward2"));

            textures.Add("cartoon\\smiley", gameInstance.Content.Load<Texture2D>("cartoon\\smiley"));
            textures.Add("cartoon\\frame", gameInstance.Content.Load<Texture2D>("cartoon\\frame"));
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
