using System;
using Asteroides.Engine;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using NaBatalhaDoCangaco;
using NaBatalhaDoCangaco.Engine.Extensions;

namespace Asteroides.Entidades
{
    public class GeradorMeteoro
    {
        private float _tempoGeracao;

        public Main ThisGame { get; set; }

        public GeradorMeteoro(Main game)
        {
            ThisGame = game;
        }

        internal void Gerar(int quant, EnumTipoMeteoro tipo, Vector2 origem, Vector2? dir = null)
        {
            if (!ThisGame.Started)
                return;

            for (int i = 0; i < quant; i++)
                ThisGame.Components.Add(Gerar(tipo, origem, dir));
        }

        private IGameComponent Gerar(EnumTipoMeteoro tipo, Vector2 origem, Vector2? dir = null)
        {
            return new Meteoro(ThisGame, tipo)
            {
                Texture = Textura(tipo),
                Posicao = origem,
                Inercia = dir ?? Vector2.One.Rotate((float)RandomSingleton.Instance.NextDouble() * MathHelper.TwoPi),
                Speed = RandomSingleton.Instance.Next(10, 100),
                Rotacao = (float)RandomSingleton.Instance.NextDouble() / 20
            };
        }

        private Texture2D Textura(EnumTipoMeteoro tipo)
        {
            switch (tipo)
            {
                case EnumTipoMeteoro.Grande: return ThisGame.Content.Load<Texture2D>("2d/meteorao");
                case EnumTipoMeteoro.Medio: return ThisGame.Content.Load<Texture2D>("2d/meteoro");
                case EnumTipoMeteoro.Pequeno: return ThisGame.Content.Load<Texture2D>("2d/meteorinho");
                default: return null;
            }
        }

        internal void Gerar(int quant)
        {
            if (!ThisGame.Started)
                return;

            var e = Enum.GetValues(typeof(EnumTipoMeteoro));

            for (int i = 0; i < quant; i++)
            {
                var index = RandomSingleton.Instance.Next(e.Length);

                var pos = MontaPosicaoIncial();
                var dir = MontaDirecao(pos);

                Gerar(1, (EnumTipoMeteoro)e.GetValue(index), pos, dir);
            }
        }

        internal void Gerar(GameTime gameTime)
        {
            if (!ThisGame.Started)
                return;

            var dt = (float)gameTime.ElapsedGameTime.TotalSeconds;

            if ((_tempoGeracao -= dt) > 0)
                return;

            _tempoGeracao = 2f;

            Gerar(1);
        }

        private Vector2 MontaDirecao(Vector2 pos)
        {
            var pl = ThisGame.Player;

            var dir = new Vector2(pl.Posicao.X - pos.X, pl.Posicao.Y - pos.Y);
            dir.Normalize();

            return dir;
        }

        private Vector2 MontaPosicaoIncial()
        {
            var pos = new[]
            {
                RandomSingleton.Instance.Next(1, 1000),
                RandomSingleton.Instance.Next(1, ThisGame.Window.ClientBounds.Width - 1),
                RandomSingleton.Instance.Next(1, ThisGame.Window.ClientBounds.Height - 1),
            };

            var origem = pos[0];

            if (origem > 750)
                return new Vector2(pos[1], 1);
            else if (origem > 500)
                return new Vector2(pos[1], ThisGame.Window.ClientBounds.Height - 1);
            else if (origem > 250)
                return new Vector2(1, pos[2]);
            else
                return new Vector2(ThisGame.Window.ClientBounds.Width - 1, pos[2]);
        }
    }
}
