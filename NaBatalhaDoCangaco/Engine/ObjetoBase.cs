using Asteroides.Engine;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace NaBatalhaDoCangaco.Engine
{
    public abstract class ObjetoBase<T> : GameComponent, IDrawable
        where T : MainGame
    {
        protected T ThisGame;

        protected ObjetoBase(Game game) 
            : base(game)
        {
            ThisGame = (T)game;
        }

        public virtual int DrawOrder => 0;
        public virtual bool Visible => true;

        public event EventHandler<EventArgs> DrawOrderChanged;
        public event EventHandler<EventArgs> VisibleChanged;

        public virtual void Draw(GameTime gameTime)
        {
            throw new NotImplementedException();
        }
    }
}
