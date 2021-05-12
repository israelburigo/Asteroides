using Microsoft.Xna.Framework;


namespace Asteroides.Entidades.Armas
{
    public interface IArma
    {
        float TempoTiro { get; set; }
        int Municao { get; set; }
        Color Cor { get; set; }
        void Atira(Game game, GameTime gameTime, Vector2 posicao, Vector2 direcao);
        void Reset();
    }
}
