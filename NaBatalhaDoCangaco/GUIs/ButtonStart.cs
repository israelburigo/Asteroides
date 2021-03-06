using Asteroides.Engine;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using NaBatalhaDoCangaco;
using NaBatalhaDoCangaco.Engine;

namespace Asteroides.GUIs
{
    public class ButtonStart : DrawableGameComponent
    {
        public SpriteFont Font;
        public Rectangle Bounds { get; set; }

        private float _blink = 0;

        public ButtonStart(Game game)
            : base(game)
        {
            game.Components.Add(this);
        }

        public override void Draw(GameTime gameTime)
        {
            if ((Game as Main).Started)
                return;

            if (_blink < 0.4)
                return;

            var txt = "Press ENTER";

            var w = Game.Window.ClientBounds.Width / 2;
            var h = Game.Window.ClientBounds.Height / 2;
            var size = Font.MeasureString(txt);
            var pos = new Vector2(w - size.X / 2, h - size.Y / 2);
            Globals.SpriteBatch.DrawString(Font, txt, pos, Color.White);
        }

        public override void Update(GameTime gameTime)
        {
            if ((Game as Main).Started)
                return;

            var dt = (float)gameTime.ElapsedGameTime.TotalSeconds;

            if ((_blink -= dt) <= 0)
                _blink = 0.8f;

            if (Keyboard.GetState().IsKeyDown(Keys.Enter))
                (Game as Main).Start();             
        }
    }
}
