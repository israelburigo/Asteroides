using Microsoft.Xna.Framework;

namespace Asteroides.Entidades.Armas
{
    public class CanhaoRapido : CanhaoSimples
    {
        public CanhaoRapido()
        {
            Cor = Color.Blue;
            TempoTiroPadrao = 0.1f;
            Municao = 100;
        }
    }
}
