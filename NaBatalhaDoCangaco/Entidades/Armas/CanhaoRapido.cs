using Microsoft.Xna.Framework;

namespace Asteroides.Entidades.Armas
{
    public class CanhaoRapido : CanhaoSimples
    {
        public CanhaoRapido()
        {
            Cor = Color.Yellow;
            TempoTiroPadrao = 0.1f;
            Municao = 100;
        }
    }
}
