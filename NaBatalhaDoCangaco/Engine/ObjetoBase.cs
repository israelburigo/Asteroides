using Microsoft.Xna.Framework;

namespace NaBatalhaDoCangaco.Engine
{
    public abstract class ObjetoBase<T> : DrawableGameComponent 
        where T : Game
    {
        protected T ThisGame;

        protected ObjetoBase(Game game) 
            : base(game)
        {
            ThisGame = (T)game;
        }
    }
}
