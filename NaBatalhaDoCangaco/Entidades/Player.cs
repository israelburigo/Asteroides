using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using NaBatalhaDoCangaco.Engine;
using NaBatalhaDoCangaco.Engine.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NaBatalhaDoCangaco.Entidades
{
    public class Player : BaseObject<Main>
    {
        public Texture2D Texture { get; set; }
        public Texture2D TiroTexture { get; set; }

        public Vector2 Posicao { get; set; }
        public Vector2 Direcao { get; set; } = Vector2.UnitX;
        public Vector2 Inercia { get; set; } = Vector2.Zero;

        public float Aceleracao { get; set; } = 10;

        public int Score { get; set; }
        public int MaxScore { get; set; }

        private float _tempoTiro = 0;

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

            ThisGame.SpriteBatch.Draw(Texture, Posicao, null, Color.White, -Direcao.Angle(), new Vector2(Texture.Width / 2, Texture.Height / 2), 1f, SpriteEffects.None, 0);
        }

        public override void Update(GameTime gameTime)
        {
            if (!ThisGame.Started)
                return;

            var dt = (float)gameTime.ElapsedGameTime.TotalSeconds;

            var keys = Keyboard.GetState().GetPressedKeys();

            if (keys.Contains(Keys.Up))
                Inercia += Direcao * dt * Aceleracao;

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

            if (Keyboard.GetState().IsKeyDown(Keys.Space))
                Atira(dt);

            if (Keyboard.GetState().IsKeyUp(Keys.Space))
                _tempoTiro = 0;

            Posicao += Inercia;

            var meteoros = ThisGame.Components.OfType<Meteoro>();

            if (meteoros.Any(p => p.Contem(Posicao)))
                ThisGame.End();
        }

        private void Atira(float dt)
        {
            if ((_tempoTiro -= dt) > 0)
                return;

            _tempoTiro = 0.3f;

            var tiro = new Tiro(ThisGame)
            {
                Posicao = Posicao,
                Direcao = Direcao
            };

            ThisGame.Components.Add(tiro);
        }
    }
}
