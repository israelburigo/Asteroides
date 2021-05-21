using Microsoft.Xna.Framework;

namespace Asteroides.Entidades.Armas
{
    public class CanhaoRapido : CanhaoSimples
    {
        public CanhaoRapido(PlayerBase pb)
            :base(pb)
        {
            Cor = Color.Yellow;
            TempoTiroPadrao = 0.1f;
            Municao = 100;
        }
    }
}
