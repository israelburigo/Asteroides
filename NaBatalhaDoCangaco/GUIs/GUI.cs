using Asteroides.Engine;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using NaBatalhaDoCangaco;
using NaBatalhaDoCangaco.Engine;

namespace Asteroides.GUIs
{
    public class GUI : DrawableGameComponent
    {
        public SpriteFont Font { get; set; }
        public string Score { get; set; }
        public string MaxScore { get; set; }

        public ButtonStart Start { get; set; }

        public GUI(Game game) : base(game)
        {
            game.Components.Add(this);
            Start = new ButtonStart(Game);
            SetFont(game.Content.Load<SpriteFont>("fonts/arial20"));
        }

        public override void Draw(GameTime gameTime)
        {
            Globals.SpriteBatch.DrawString(Font, Score, new Vector2(10, 10), Color.White);
            Globals.SpriteBatch.DrawString(Font, MaxScore, new Vector2(10, 50), Color.White);
        }

        public override void Update(GameTime gameTime)
        {
            var pl = (Game as Main).Player;

            Score = $"Score: {pl.Score.Valor}";
            MaxScore = $"Max: {pl.Score.Max}";
        }

        internal void SetFont(SpriteFont spriteFont)
        {
            Font = spriteFont;
            Start.Font = spriteFont;
        }
    }
}
