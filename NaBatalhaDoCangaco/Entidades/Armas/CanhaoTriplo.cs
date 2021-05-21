using Microsoft.Xna.Framework;
using NaBatalhaDoCangaco.Engine.Extensions;

namespace Asteroides.Entidades.Armas
{
    public class CanhaoTriplo : CanhaoSimples
    {
        public CanhaoTriplo(PlayerBase pb)
            : base(pb)
        {
            TempoTiroPadrao = 0.7f;
            Cor = Color.Green;
            Municao = 50;
        }

        public override void GeraTiro(Game game, Vector2 posicao, Vector2 direcao)
        {
            for (var i = -1; i <= 1; i++)
            {
                new Tiro(game, PlayerBase)
                {
                    Posicao = posicao,
                    Direcao = direcao.Rotate(MathHelper.ToRadians(20 * i))
                };
            }
        }
    }
}
