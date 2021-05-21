﻿using System.Linq;
using Asteroides.Engine;
using Asteroides.Engine.Components;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using NaBatalhaDoCangaco;
using NaBatalhaDoCangaco.Engine;
using NaBatalhaDoCangaco.Engine.Extensions;

namespace Asteroides.Entidades
{
    public class Tiro : ObjetoBase<Main>
    {
        public PlayerBase Dono { get; set; }
        public Vector2 Posicao { get; set; }
        public Vector2 Direcao { get; set; }
        public Texture2D Texture { get; set; }

        public Tiro(Game game, PlayerBase dono) : base(game)
        {
            Texture = game.Content.Load<Texture2D>("2d/tiro");
            game.Components.Add(this);

            Dono = dono;
        }

        public override void Draw(GameTime gameTime)
        {
            if (Texture == null)
                return;

            Globals.SpriteBatch.Draw(Texture, Posicao, null, Color.White, -Direcao.Angle(), new Vector2(Texture.Width / 2, Texture.Height / 2), 1f, SpriteEffects.None, 0);
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
                CriaParticulas(Posicao);
                Dono.Score.Valor++;
                ThisGame.Components.Remove(this);
                meteoro.Destruir();
            }
        }

        private void CriaParticulas(Vector2 posicao)
        {
            new Particulas(Game)
            {
                Quant = new MinMax(5, 10),
                DuracaoDasParticulas = new MinMax(0.5f, 1.0f),
                Posicao = posicao,
                Textura = ThisGame.Content.Load<Texture2D>("2d/particula"),
                Angulo = new MinMax(0, 359),
                Velocidade = new MinMax(10, 100),                
            }.Start();
        }
    }
}
