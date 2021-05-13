using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using NaBatalhaDoCangaco;
using NaBatalhaDoCangaco.Engine;
using System;
using System.Collections.Generic;
using System.Text;

namespace Asteroides.GUIs
{
    public class ButtonStart : ObjetoBase<Main>
    {
        public SpriteFont Font;
        public Rectangle Bounds { get; set; }

        private float _blink = 0;

        public ButtonStart(Game game)
            : base(game)
        {
        }

        public override void Draw(GameTime gameTime)
        {
            if (ThisGame.Started)
                return;

            if (_blink < 0.4)
                return;

            var txt = "Press ENTER";

            var w = ThisGame.Window.ClientBounds.Width / 2;
            var h = ThisGame.Window.ClientBounds.Height / 2;
            var size = Font.MeasureString(txt);
            var pos = new Vector2(w - size.X / 2, h - size.Y / 2);
            ThisGame.SpriteBatch.DrawString(Font, txt, pos, Color.White);
        }

        public override void Update(GameTime gameTime)
        {
            if (ThisGame.Started)
                return;

            var dt = (float)gameTime.ElapsedGameTime.TotalSeconds;

            if ((_blink -= dt) <= 0)
                _blink = 0.8f;

            if (Keyboard.GetState().IsKeyDown(Keys.Enter))
                ThisGame.Start();             
        }
    }
}
