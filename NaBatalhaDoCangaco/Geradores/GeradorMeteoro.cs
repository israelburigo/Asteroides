using System;
using Asteroides.Engine;
using Asteroides.Entidades;
using Microsoft.Xna.Framework;
using NaBatalhaDoCangaco;
using NaBatalhaDoCangaco.Engine.Extensions;

namespace Asteroides.Geradores
{
    public class GeradorMeteoro : GameComponent
    {
        public float Tempo { get; set; } = 2f;

        public GeradorMeteoro(Game game) 
            : base(game)
        {
             game.Components.Add(this);
        }

        public override void Update(GameTime gameTime)
        {
            if(!(Game as Main).Started)
                return;

            var dt = gameTime.GetDelta();

            if ((Tempo -= dt) > 0)
                return;

            Tempo = 2f;

            Gerar(1);
        }

        public void Gerar(int quant)
        {
            while(quant-- > 0)
                Gerar();
        }  
        public void Gerar(int quant, EnumTipoMeteoro tipo, Vector2 posicao)
        {
            var ang = new MinMax(0, 359);

            for (int i = 0; i < quant; i++)
                Gerar(posicao, tipo, ang.Random());
        }  

        private void Gerar(Vector2? posOrig = null , EnumTipoMeteoro? tipo = null, float? ang = null)
        {
            var rot = new MinMax(-5, 5).Random();
            var vel = new MinMax(30f, 100f).Random();

            Vector2 pos;
            if (posOrig.HasValue)
            {
                pos = posOrig.Value;
            }
            else
            {
                var bordaInfSup = new MinMax(0, Game.Window.ClientBounds.Width).RandomInt();
                var bordaEsqDir = new MinMax(0, Game.Window.ClientBounds.Height).RandomInt();

                var cantos = new MinMax(0, 3).RandomInt();

                switch (cantos)
                {
                    case 0: pos = new Vector2(bordaInfSup, 0); break;
                    case 1: pos = new Vector2(bordaInfSup, Game.Window.ClientBounds.Height); break;
                    case 2: pos = new Vector2(0, bordaEsqDir); break;
                    case 3: pos = new Vector2(Game.Window.ClientBounds.Width, bordaEsqDir); break;
                    default: pos = Vector2.Zero; break;
                }
            }

            var values = Enum.GetValues(typeof(EnumTipoMeteoro));
            var index = new MinMax(0, values.Length - 1).RandomInt();
            var tipoM = (EnumTipoMeteoro)values.GetValue(index);

            var pl = (Game as Main).Player;

            var inercia = GeradorInercia(pos, pl.Posicao);
            if (ang.HasValue)
                inercia = inercia.Rotate(MathHelper.ToRadians(ang.Value));

            new Meteoro(Game, tipo ?? tipoM)
            {
                Posicao = new Vector2(pos.X, pos.Y),
                Rotacao = MathHelper.ToRadians(rot),
                Velocidade = vel,
                Inercia = inercia
            };
        }

        private Vector2 GeradorInercia(Vector2 pos, Vector2 plPos)
        {
            var dx = plPos.X - pos.X;
            var dy = plPos.Y - pos.Y;
            var ine = new Vector2(dx, dy);
            ine.Normalize();
            return ine;
        }
    }
}
