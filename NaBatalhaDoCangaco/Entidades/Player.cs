using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using NaBatalhaDoCangaco.Engine;
using NaBatalhaDoCangaco.Engine.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NaBatalhaDoCangaco.Entidades
{
    public class Player : IBaseObject
    {
        public Texture2D Texture { get; set; }
        public Texture2D TiroTexture { get; set; }        

        public Vector2 Posicao { get; set; }
        public Vector2 Direcao { get; set; } = Vector2.UnitX;        
        public Vector2 Inercia { get; set; } = Vector2.Zero;

        public float Aceleracao { get; set; } = 10;

        public bool Enabled => true;
        public int UpdateOrder => 0;
        public int DrawOrder => 0;
        public bool Visible => true;

        public int Score { get; set; }
        public int MaxScore { get; set; }

        private bool _keydown = false;

        public event EventHandler<EventArgs> EnabledChanged;
        public event EventHandler<EventArgs> UpdateOrderChanged;
        public event EventHandler<EventArgs> DrawOrderChanged;
        public event EventHandler<EventArgs> VisibleChanged;

        public void Initialize()
        {

        }

        public void Draw(GameTime gameTime)
        {
            if (Texture == null)
                return;

            Globals.SpriteBatch.Draw(Texture, Posicao, null, Color.White, -Direcao.Angle(), new Vector2(Texture.Width / 2, Texture.Height / 2), 1f, SpriteEffects.None, 0);
        }

        public void Update(GameTime gameTime)
        {
            var dt = (float)gameTime.ElapsedGameTime.TotalSeconds;

            var keys = Keyboard.GetState().GetPressedKeys();

            if (keys.Contains(Keys.Up))
                Inercia += Direcao * dt * Aceleracao;

            if (keys.Contains(Keys.Right))
                Direcao = Direcao.Rotate(0.1f);
            else if (keys.Contains(Keys.Left))
                Direcao = Direcao.Rotate(-0.1f);

            if (Posicao.X > Globals.Game.Window.ClientBounds.Width)
                Posicao = new Vector2(0, Posicao.Y);
            if (Posicao.X < 0)
                Posicao = new Vector2(Globals.Game.Window.ClientBounds.Width, Posicao.Y);

            if (Posicao.Y > Globals.Game.Window.ClientBounds.Height)
                Posicao = new Vector2(Posicao.X, 0);
            if (Posicao.Y < 0)
                Posicao = new Vector2(Posicao.X, Globals.Game.Window.ClientBounds.Height);

            if (!_keydown && Keyboard.GetState().IsKeyDown(Keys.Space))
            {
                Atira(dt);
                _keydown = true;
            }

            if(Keyboard.GetState().IsKeyUp(Keys.Space))
                _keydown = false;

            Posicao += Inercia;
        }

        private void Atira(float dt)
        {          

            var tiro = new Tiro
            {
                Posicao = Posicao,
                Direcao = Direcao
            };

            Globals.Game.Components.Add(tiro);
        }
    }
}
