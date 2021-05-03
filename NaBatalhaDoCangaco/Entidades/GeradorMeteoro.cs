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
    public class GeradorMeteoro
    {
        private float _tempoGeracao;

        internal void Gerar(int quant, EnumTipoMeteoro tipo, Vector2 origem, Vector2? dir = null)
        {
            for (int i = 0; i < quant; i++)
                Globals.Game.Components.Add(Gerar(tipo, origem, dir));
        }

        private IGameComponent Gerar(EnumTipoMeteoro tipo, Vector2 origem, Vector2? dir = null)
        {
            return new Meteoro(tipo)
            {
                Texture = Textura(tipo),
                Posicao = origem,
                Inercia = dir ?? Vector2.One.Rotate((float)Globals.Random.NextDouble() * MathHelper.TwoPi),
                Speed = Globals.Random.Next(10, 100),
                Rotacao = (float)Globals.Random.NextDouble() / 20
            };
        }

        private Texture2D Textura(EnumTipoMeteoro tipo)
        {
            switch (tipo)
            {
                case EnumTipoMeteoro.Grande: return Globals.Game.Content.Load<Texture2D>("2d/meteorao");
                case EnumTipoMeteoro.Medio: return Globals.Game.Content.Load<Texture2D>("2d/meteoro");
                case EnumTipoMeteoro.Pequeno: return Globals.Game.Content.Load<Texture2D>("2d/meteorinho");
                default: return null;
            }
        }

        internal void Gerar(int quant)
        {
            var e = Enum.GetValues(typeof(EnumTipoMeteoro));

            for (int i = 0; i < quant; i++)
            {
                var index = Globals.Random.Next(e.Length);

                var pos = MontaPosicaoIncial();
                var dir = MontaDirecao(pos);

                Gerar(1, (EnumTipoMeteoro)e.GetValue(index), pos, dir);
            }
        }

        internal void Gerar(GameTime gameTime)
        {
            var dt = (float)gameTime.ElapsedGameTime.TotalSeconds;

            if ((_tempoGeracao -= dt) > 0)
                return;

            _tempoGeracao = 2f;

            Gerar(1);
        }

        private Vector2 MontaDirecao(Vector2 pos)
        {
            var pl = Globals.GetPlayer<Player>();

            var dir = new Vector2(pl.Posicao.X - pos.X, pl.Posicao.Y - pos.Y);
            dir.Normalize();

            return dir;
        }

        private Vector2 MontaPosicaoIncial()
        {
            var pos = new[]
            {
                Globals.Random.Next(1, 1000),
                Globals.Random.Next(1, Globals.Game.Window.ClientBounds.Width - 1),
                Globals.Random.Next(1, Globals.Game.Window.ClientBounds.Height - 1),
            };

            var origem = pos[0];

            if (origem > 750)
                return new Vector2(pos[1], 1);
            else if (origem > 500)
                return new Vector2(pos[1], Globals.Game.Window.ClientBounds.Height - 1);
            else if (origem > 250)
                return new Vector2(1, pos[2]);
            else
                return new Vector2(Globals.Game.Window.ClientBounds.Width - 1, pos[2]);
        }
    }
}
