using System;
using System.Reflection.PortableExecutable;
using Microsoft.Xna.Framework;

namespace Asteroides.Entidades.Armas
{
    public class CanhaoSimples : IArma
    {
        public float TempoTiro { get; set; }
        public int Municao { get; set; }
        public Color Cor { get; set; }
        public float TempoTiroPadrao { get; set; }

        public CanhaoSimples()
        {
            Cor = Color.White;
            Municao = Int32.MaxValue;
            TempoTiroPadrao = TempoTiro = 0.3f;
        }

        public void Atira(Game game, GameTime gameTime, Vector2[] posicoes, Vector2 direcao)
        {
            var dt = (float)gameTime.ElapsedGameTime.TotalSeconds;

            if ((TempoTiro -= dt) > 0)
                return;

            Municao--;

            TempoTiro = TempoTiroPadrao;

            GeraTiro(game, posicoes, direcao);
        }

        public virtual void GeraTiro(Game game, Vector2[] posicao, Vector2 direcao)
        {
            foreach (var item in posicao)
            {
                new Tiro(game)
                {
                    Posicao = item,
                    Direcao = direcao
                }; 
            }
           
        }

        public void Reset()
        {
            TempoTiro = 0;
        }
    }
}
