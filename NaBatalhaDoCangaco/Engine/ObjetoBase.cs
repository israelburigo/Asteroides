using Asteroides.Engine;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

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
