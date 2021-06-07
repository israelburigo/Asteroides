using System;
using System.Collections.Generic;
using System.Linq;
using Asteroides.Engine;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using NaBatalhaDoCangaco;
using NaBatalhaDoCangaco.Engine;
using NaBatalhaDoCangaco.Engine.Extensions;

namespace Asteroides.Entidades
{
    public enum EnumTipoMeteoro
    {
        Grande = 0,
        Medio = 1,
        Pequeno = 2
    }

    public class Meteoro : DrawableGameComponent
    {
        public Vector2 Posicao { get; set; }
        private Vector2 _direcao = Vector2.One;
        public Vector2 Inercia { get; set; } = Vector2.Zero;
        public float Velocidade { get; set; }
        public Texture2D Texture { get; set; }
        protected EnumTipoMeteoro Tipo { get; set; }
        public float Rotacao { get; set; }
        public float Raio => GetRaio();
        public Animation Animation { get; set; }

        public Meteoro(Game game, EnumTipoMeteoro e) 
            : base(game)
        {
            Tipo = e;
            game.Components.Add(this);
            Texture = game.Content.Load<Texture2D>("2d/meteoro");

            Animation = new Animation(Texture, 1, 3, 3)
                .SelectIndex((int)Tipo);
        }

        public float GetRaio()
        {
            switch(Tipo)
            {
                case EnumTipoMeteoro.Grande : return 45f/2;
                case EnumTipoMeteoro.Medio : return 28f/2;
                case EnumTipoMeteoro.Pequeno : return 16f/2;
                default : return 0;
            }
        }

        internal void Destruir()
        {
            var game = Game as Main;

            Game.Components.Remove(this);
            game.Player.Score.Valor++;   

             switch(Tipo)
            {
                case EnumTipoMeteoro.Grande : game.GeradorMeteoro.Gerar(2, EnumTipoMeteoro.Medio,Posicao);break;
                case EnumTipoMeteoro.Medio : game.GeradorMeteoro.Gerar(2, EnumTipoMeteoro.Pequeno, Posicao);break;
                default : break;
            }
        }

        internal bool Contem(Vector2 v)
        {
            var dist = Vector2.Distance(v, Posicao);
            return dist < Raio;
        }    

        public override void Update(GameTime gameTime)
        {
            var dt = (float)gameTime.ElapsedGameTime.TotalSeconds;
            Posicao += Inercia * dt * Velocidade;

            if (Posicao.X > Game.Window.ClientBounds.Width)
                Posicao = new Vector2(0, Posicao.Y);
            if (Posicao.X < 0)
                Posicao = new Vector2(Game.Window.ClientBounds.Width, Posicao.Y);

            if (Posicao.Y > Game.Window.ClientBounds.Height)
                Posicao = new Vector2(Posicao.X, 0);
            if (Posicao.Y < 0)
                Posicao = new Vector2(Posicao.X, Game.Window.ClientBounds.Height);

            _direcao = _direcao.Rotate(Rotacao);

            Animation.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            Globals.SpriteBatch.Draw(Texture, Posicao, Animation.Source, Color.White, -_direcao.Angle(), Animation.Center, 1f, SpriteEffects.None, 0);
        }

        internal bool Contem(IEnumerable<Vector2> bounds)
        {
            return bounds.Any(Contem);
        }
    }
}
