using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using NaBatalhaDoCangaco.Engine;
using NaBatalhaDoCangaco.Engine.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NaBatalhaDoCangaco.Entidades
{
    public class Tiro : BaseObject<Main>
    {
        public Player Player { get; set; }
        public Vector2 Posicao { get; set; }
        public Vector2 Direcao { get; set; }

        public Tiro(Game game) : base(game)
        {
        }

        public override void Draw(GameTime gameTime)
        {
            if (Player.TiroTexture == null)
                return;

            var texture = Player.TiroTexture;
            ThisGame.SpriteBatch.Draw(texture, Posicao, null, Color.White, -Direcao.Angle(), new Vector2(texture.Width / 2, texture.Height / 2), 1f, SpriteEffects.None, 0);
        }

        public override void Initialize()
        {
            Player = ThisGame.Player;
        }

        public override void Update(GameTime gameTime)
        {
            var dt = (float)gameTime.ElapsedGameTime.TotalSeconds;
            Posicao += Direcao * dt * 1000;

            if (Posicao.X > ThisGame.Window.ClientBounds.Width || Posicao.X < 0)
                ThisGame.Components.Remove(this);

            if (Posicao.Y > ThisGame.Window.ClientBounds.Height || Posicao.Y < 0)
                ThisGame.Components.Remove(this);

            var meteoros = ThisGame.Components.OfType<Meteoro>();

            foreach (var meteoro in meteoros.Where(p => p.Contem(Posicao)).ToList())
            {
                ThisGame.Components.Remove(this);
                meteoro.Destruir();
            }
        }

    }
}
