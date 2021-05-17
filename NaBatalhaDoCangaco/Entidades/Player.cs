using System;
using System.Linq;
using Asteroides.Engine;
using Asteroides.Entidades.Armas;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using NaBatalhaDoCangaco;
using NaBatalhaDoCangaco.Engine;
using NaBatalhaDoCangaco.Engine.Extensions;

namespace Asteroides.Entidades
{
    public class Player : ObjetoBase<Main>
    {
        public Vector2[] Bounds { get; set; }

        public Texture2D Texture { get; set; }

        public Vector2 Posicao { get; set; }
        public Vector2 Direcao { get; set; } = Vector2.UnitX;
        public Vector2 Inercia { get; set; } = Vector2.Zero;
        public IArma Arma { get; set; } = new CanhaoSimples();
        public Score Score { get; set; } = new Score();

        public float Aceleracao { get; set; } = 10;

        public Player(Game game) 
            : base(game)
        {
        }

        public override void Draw(GameTime gameTime)
        {
            if (!ThisGame.Started)
                return;

            if (Texture == null)
                return;

            Globals.SpriteBatch.Draw(Texture, Posicao, null, Arma.Cor, -Direcao.Angle(), new Vector2(Texture.Width / 2, Texture.Height / 2), 1f, SpriteEffects.None, 0);
        }

        public override void Update(GameTime gameTime)
        {
            if (!ThisGame.Started)
                return;

            var dt = (float)gameTime.ElapsedGameTime.TotalSeconds;

            var keys = Keyboard.GetState().GetPressedKeys();

            if (Arma.Municao <= 0)
                Arma = new CanhaoSimples();

            if (keys.Contains(Keys.Up))
                Inercia += Direcao * dt * Aceleracao;
            else if (keys.Contains(Keys.Down))
                Inercia -= Direcao * dt * Aceleracao / 5;

            if (keys.Contains(Keys.Right))
                Direcao = Direcao.Rotate(0.1f);
            else if (keys.Contains(Keys.Left))
                Direcao = Direcao.Rotate(-0.1f);

            if (Posicao.X > ThisGame.Window.ClientBounds.Width)
                Posicao = new Vector2(0, Posicao.Y);
            if (Posicao.X < 0)
                Posicao = new Vector2(ThisGame.Window.ClientBounds.Width, Posicao.Y);

            if (Posicao.Y > ThisGame.Window.ClientBounds.Height)
                Posicao = new Vector2(Posicao.X, 0);
            if (Posicao.Y < 0)
                Posicao = new Vector2(Posicao.X, ThisGame.Window.ClientBounds.Height);

            var angle = -Direcao.Angle();
            Bounds = new[]
            {
                new Vector2(Posicao.X, Posicao.Y + Texture.Height/2).Rotate(angle, Posicao),
                new Vector2(Posicao.X - Texture.Width/2, Posicao.Y - Texture.Height/2 ).Rotate(angle, Posicao),
                new Vector2(Posicao.X + Texture.Width/2, Posicao.Y - Texture.Height/2 ).Rotate(angle, Posicao),
            };

            if (Keyboard.GetState().IsKeyDown(Keys.Space))
                Arma.Atira(ThisGame, gameTime, Bounds.First(), Direcao);

            if (Keyboard.GetState().IsKeyUp(Keys.Space))
                Arma.Reset();

            Posicao += Inercia;

            var meteoros = ThisGame.Components.OfType<Meteoro>();

            if (meteoros.Any(p => p.Contem(Bounds)))
                ThisGame.End();
        }

    }
}
