﻿using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using NaBatalhaDoCangaco;
using NaBatalhaDoCangaco.Engine;
using NaBatalhaDoCangaco.Engine.Extensions;

namespace Asteroides.Entidades
{
    public class Tiro : BaseObject<Main>
    {
        public Player Player { get; set; }
        public Vector2 Posicao { get; set; }
        public Vector2 Direcao { get; set; }
        public Texture2D Texture { get; set; }

        public Tiro(Game game) : base(game)
        {
            Texture = game.Content.Load<Texture2D>("2d/tiro");
            game.Components.Add(this);
        }

        public override void Draw(GameTime gameTime)
        {
            if (Texture == null)
                return;

            ThisGame.SpriteBatch.Draw(Texture, Posicao, null, Color.White, -Direcao.Angle(), new Vector2(Texture.Width / 2, Texture.Height / 2), 1f, SpriteEffects.None, 0);
        }

        public override void Initialize()
        {
            Player = ThisGame.Player;
        }

        public override void Update(GameTime gameTime)
        {
            var dt = (float)gameTime.ElapsedGameTime.TotalSeconds;
            Posicao += Direcao * dt * 700;

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
