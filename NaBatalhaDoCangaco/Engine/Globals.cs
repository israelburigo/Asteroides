using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace NaBatalhaDoCangaco.Engine
{
    public class Globals
    {
        public static Game Game;
        public static GraphicsDeviceManager Graphics;        
        public static SpriteBatch SpriteBatch;
        public static IBaseObject Player;
        public static Random Random = new Random();

        public static T GetGame<T>() where T : Game
        {
            return (T)Game;
        }

        public static T GetPlayer<T>()
        {
            return (T)Player;
        }
    }
}
