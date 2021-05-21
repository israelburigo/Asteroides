using Asteroides.Engine;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using NaBatalhaDoCangaco;
using NaBatalhaDoCangaco.Engine;
using System;
using System.Collections.Generic;
using System.Text;

namespace Asteroides.GUIs
{
    public class GUI : ObjetoBase<Main>
    {
        public SpriteFont Font { get; set; }
        public string Score { get; set; }
        public string MaxScore { get; set; }

        public ButtonStart Start { get; set; }

        public GUI(Game game) : base(game)
        {
        }

        public override void Draw(GameTime gameTime)
        {
            //Globals.SpriteBatch.DrawString(Font, Score, new Vector2(10, 10), Color.White);
            //Globals.SpriteBatch.DrawString(Font, MaxScore, new Vector2(10, 50), Color.White);

            Start.Draw(gameTime);
        }

        public override void Initialize()
        {
            Start = new ButtonStart(Game);
        }

        public override void Update(GameTime gameTime)
        {
            //var pl = ThisGame.Player;

            //Score = $"Score: {pl.Score.Valor}";
            //MaxScore = $"Max: {pl.Score.Max}";

            Start.Update(gameTime);
        }

        internal void SetFont(SpriteFont spriteFont)
        {
            Font = spriteFont;
            Start.Font = spriteFont;
        }
    }
}
