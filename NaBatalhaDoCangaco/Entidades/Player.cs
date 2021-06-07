using System;
using System.Collections.Generic;
using System.Linq;
using Asteroides.Engine;
using Asteroides.Entidades.Armas;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using NaBatalhaDoCangaco;
using NaBatalhaDoCangaco.Engine;
using NaBatalhaDoCangaco.Engine.Extensions;

namespace Asteroides.Entidades
{
    public class Player : DrawableGameComponent
    {
        public List<Vector2> Bounds { get; set; } = new List<Vector2>();
        public Texture2D Texture { get; set; }
        public Vector2 Posicao { get; set; }
        public Vector2 Direcao { get; set; } = Vector2.UnitX;
        public Vector2 Inercia { get; set; } = Vector2.Zero;
        public IArma Arma { get; set; } = new CanhaoSimples();
        public Score Score { get; set; } = new Score();
        public float Aceleracao { get; set; } = 10;
        public Animation Animation { get; set; }

        public Player(Game game) 
            : base(game)
        {
            game.Components.Add(this);
            Texture = game.Content.Load<Texture2D>("2d/player");

            Animation = new Animation(Texture, 1, 7, 7)
            {
                StartIndex = 0,
                EndIndex = 5,            
                TimePerFrame = new [] { 0.05f },
                Loop = true
            }.SelectIndex(6);            
        }

        public override void Draw(GameTime gameTime)
        {
            if (!(Game as Main).Started)
                return;

            Globals.SpriteBatch.Draw(Texture, Posicao, Animation.Source, Arma.Cor, -Direcao.Angle(), Animation.Center, 1f, SpriteEffects.None, 0);

            // foreach (var item in Bounds)
            //     Globals.SpriteBatch.DrawPoint(item, 2f, Color.Red);
        }

        public override void Update(GameTime gameTime)
        {
            if (!(Game as Main).Started)
                return;

            var dt = (float)gameTime.ElapsedGameTime.TotalSeconds;

            var keys = Keyboard.GetState().GetPressedKeys();

            if (Arma.Municao <= 0)
                Arma = new CanhaoSimples();

            if (keys.Contains(Keys.Up))
            {
                Animation.Start();
                Inercia -= Direcao * dt * Aceleracao;
            }
            else if (keys.Contains(Keys.Down))
                Inercia += Direcao * dt * Aceleracao / 5;
            else 
            {
                Animation.Stop().SelectIndex(6);
            }

            Animation.Update(gameTime);

            if (keys.Contains(Keys.Right))
                Direcao = Direcao.Rotate(0.1f);
            else if (keys.Contains(Keys.Left))
                Direcao = Direcao.Rotate(-0.1f);

            if (Posicao.X > Game.Window.ClientBounds.Width)
                Posicao = new Vector2(0, Posicao.Y);
            if (Posicao.X < 0)
                Posicao = new Vector2(Game.Window.ClientBounds.Width, Posicao.Y);

            if (Posicao.Y > Game.Window.ClientBounds.Height)
                Posicao = new Vector2(Posicao.X, 0);
            if (Posicao.Y < 0)
                Posicao = new Vector2(Posicao.X, Game.Window.ClientBounds.Height);

            var angle = -Direcao.Angle();
            Bounds.Clear();
            
            Bounds.Add(Posicao);
            Bounds.Add(new Vector2(Posicao.X, Posicao.Y - Animation.Source.Height/2).Rotate(angle, Posicao));
            Bounds.Add(new Vector2(Posicao.X - Animation.Source.Width/2 + 5, Posicao.Y - Animation.Source.Height/2 + 5 ).Rotate(angle, Posicao));
            Bounds.Add(new Vector2(Posicao.X + Animation.Source.Width/2 - 5, Posicao.Y - Animation.Source.Height/2 + 5).Rotate(angle, Posicao));
            

            if (Keyboard.GetState().IsKeyDown(Keys.Space))
                Arma.Atira(Game, gameTime, new [] {Bounds[2], Bounds[3]}, -Direcao);

            if (Keyboard.GetState().IsKeyUp(Keys.Space))
                Arma.Reset();

            Posicao += Inercia;

            var meteoros = Game.Components.OfType<Meteoro>();

            // if (meteoros.Any(p => p.Contem(Bounds)))
            //     (Game as Main).End();
        }

    }
}
