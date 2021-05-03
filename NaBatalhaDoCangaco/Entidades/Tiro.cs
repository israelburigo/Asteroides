using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using NaBatalhaDoCangaco.Engine;
using NaBatalhaDoCangaco.Engine.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NaBatalhaDoCangaco.Entidades
{
    public class Tiro : IBaseObject
    {
        public Vector2[] Bounds { get; set; }

        public Player Player { get; set; }

        public Vector2 Posicao { get; set; }
        public Vector2 Direcao { get; set; }

        public bool Enabled => true;
        public int UpdateOrder => 0;
        public int DrawOrder => 0;
        public bool Visible => true;

        public event EventHandler<EventArgs> EnabledChanged;
        public event EventHandler<EventArgs> UpdateOrderChanged;
        public event EventHandler<EventArgs> DrawOrderChanged;
        public event EventHandler<EventArgs> VisibleChanged;

        public void Initialize()
        {
            Player = Globals.GetPlayer<Player>();
        }

        public void Draw(GameTime gameTime)
        {
            if (Player.TiroTexture == null)
                return;

            var texture = Player.TiroTexture;
            Globals.SpriteBatch.Draw(texture, Posicao, null, Color.White, -Direcao.Angle(), new Vector2(texture.Width / 2, texture.Height / 2), 1f, SpriteEffects.None, 0);
        }

        public void Update(GameTime gameTime)
        {
            var dt = (float)gameTime.ElapsedGameTime.TotalSeconds;
            Posicao += Direcao * dt * 1000;

            if (Posicao.X > Globals.Game.Window.ClientBounds.Width || Posicao.X < 0)
                Globals.Game.Components.Remove(this);

            if (Posicao.Y > Globals.Game.Window.ClientBounds.Height || Posicao.Y < 0)
                Globals.Game.Components.Remove(this);

            var meteoros = Globals.Game.Components.GetAll<Meteoro>();

            foreach (var meteoro in meteoros.Where(p => p.Contem(Posicao)).ToList())
            {
                Globals.Game.Components.Remove(this);
                meteoro.Destruir();
            }
        }

    }
}
