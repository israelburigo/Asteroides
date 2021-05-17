using System;
using Asteroides.Engine;
using Microsoft.Xna.Framework;
using NaBatalhaDoCangaco;
using NaBatalhaDoCangaco.Engine.Extensions;

namespace Asteroides.Entidades
{
    public class GeradorItem
    {
        private float _tempoGeracao;

        public Main ThisGame { get; set; }

        public GeradorItem(Main game)
        {
            ThisGame = game;
        }

        internal void Gerar(int quant)
        {
            if (!ThisGame.Started)
                return;

            var e = Enum.GetValues(typeof(EnumTipoItem));

            for (int i = 0; i < quant; i++)
            {
                var index = RandomSingleton.Instance.Next(e.Length);

                var pos = MontaPosicaoIncial();
                var dir = MontaDirecao(pos);

                new Item(ThisGame, (EnumTipoItem)e.GetValue(index))
                {
                    Posicao = pos,
                    Direcao = dir,
                    Rotacao = (float)RandomSingleton.Instance.NextDouble() / 20
                };
            }
        }

        internal void Gerar(GameTime gameTime)
        {
            if (!ThisGame.Started)
                return;

            var dt = (float)gameTime.ElapsedGameTime.TotalSeconds;

            if ((_tempoGeracao -= dt) > 0)
                return;

            _tempoGeracao = 10f;

            Gerar(1);
        }

        private Vector2 MontaDirecao(Vector2 pos)
        {
            var pl = ThisGame.Player;

            var dir = new Vector2(pl.Posicao.X - pos.X, pl.Posicao.Y - pos.Y);
            dir.Normalize();
            var ang = new MinMax(-90, 90).Random();
            return dir.Rotate(MathHelper.ToRadians(ang));
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
