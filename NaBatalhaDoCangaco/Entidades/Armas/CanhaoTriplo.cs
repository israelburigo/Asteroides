using Microsoft.Xna.Framework;
using NaBatalhaDoCangaco.Engine.Extensions;

namespace Asteroides.Entidades.Armas
{
    public class CanhaoTriplo : CanhaoSimples
    {
        public CanhaoTriplo()
        {
            TempoTiroPadrao = 0.7f;
            Cor = Color.Green;
            Municao = 50;
        }

        public override void GeraTiro(Game game, Vector2[] posicoes, Vector2 direcao)
        {
            for (var i = -1; i <= 1; i++)
            {
                foreach (var posicao in posicoes)
                {
                     new Tiro(game)
                    {
                        Posicao = posicao,
                        Direcao = direcao.Rotate(MathHelper.ToRadians(20 * i))
                    }; 
                }
              
            }
        }
    }
}
