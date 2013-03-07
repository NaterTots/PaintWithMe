using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace PaintWithMe
{
    class TestPaintingAction : IPaintingAction
    {
        private int xcoord;
        private int ycoord;
        private int height, width;
        private float rotation;

        private Color color;
        private Texture2D texture;

        static Dictionary<ShapeActivationSource, Color> colorMap = new Dictionary<ShapeActivationSource, Color>();
        static Color[] PossibleColors = new Color[9] { Color.Red, Color.Blue, Color.Green, Color.Yellow, Color.Brown, Color.Orange, Color.Pink, Color.Purple, Color.Black };
        static RandomGenerator generatorService;
        static Game gameService;
        static Texture2D testPaintingTexture;
        static Texture2D rightStrokeTexture;
        static Texture2D leftStrokeTexture;

        static TestPaintingAction()
        {
            generatorService = ServiceManager.Instance.GetService<RandomGenerator>(ServiceType.RandomGenerator);
            gameService = ServiceManager.Instance.GetService<Game>(ServiceType.Game);
            testPaintingTexture = ServiceManager.Instance.GetService<TextureManager>(ServiceType.TextureManager).GetTexture("testsquare");
            rightStrokeTexture = ServiceManager.Instance.GetService<TextureManager>(ServiceType.TextureManager).GetTexture("stroke_blur");
            leftStrokeTexture = ServiceManager.Instance.GetService<TextureManager>(ServiceType.TextureManager).GetTexture("stroke_blur_left");
        }

        public TestPaintingAction(ShapeActivationSource source)
        {
            //figure out color, based on source
            if (colorMap.ContainsKey(source))
            {
                color = colorMap[source];
            }
            else
            {
                colorMap.Add(source, PossibleColors[colorMap.Count]);
                color = colorMap[source];
            }
            
            if (source == ShapeActivationSource.KinectLeftArmUp)
            {
                texture = leftStrokeTexture;
                height = 20; // generatorService.Next(5, 25); //leftStrokeTexture.Height;
                width = generatorService.Next(25, 75); //leftStrokeTexture.Width;
            }
            else if (source == ShapeActivationSource.KinectRightArmUp)
            {
                texture = rightStrokeTexture;

                height = 20; // generatorService.Next(5, 25);  //rightStrokeTexture.Height;
                width = generatorService.Next(25, 75);  //rightStrokeTexture.Width;
            }
            else
            {
                texture = testPaintingTexture;

                int size = generatorService.Next(5, 25);
                height = size;
                width = size;
            }

            //randomize location
            rotation = (float)generatorService.NextDouble(-.174, .174); //-10 degress to +10 degrees
            xcoord = generatorService.Next(gameService.Window.ClientBounds.Width);
            ycoord = generatorService.Next(gameService.Window.ClientBounds.Height);
            
        }

        public void Update(GameTime elapsedTime)
        {
            //this one is static, doesn't do anything
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texture, new Rectangle(xcoord, ycoord, width, height), null, color, rotation, new Vector2(0, 0), SpriteEffects.None, 0); 
        }
    }
}