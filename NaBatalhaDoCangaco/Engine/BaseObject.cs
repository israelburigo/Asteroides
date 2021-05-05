using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace NaBatalhaDoCangaco.Engine
{
    public abstract class BaseObject<T> : GameComponent, IDrawable
        where T : Game
    {
        protected T ThisGame;

        protected BaseObject(Game game) 
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
