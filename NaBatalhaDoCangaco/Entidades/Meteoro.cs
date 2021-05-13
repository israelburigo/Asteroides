using System;
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
        Medio,
        Pequeno
    }

    public class Meteoro : ObjetoBase<Main>
    {
        public Vector2 Posicao { get; set; }
        private Vector2 _direcao = Vector2.One;
        public Vector2 Inercia { get; set; } = Vector2.Zero;
        public float Speed { get; set; }
        public Texture2D Texture { get; set; }
        protected EnumTipoMeteoro Tipo { get; set; }
        public float Rotacao { get; set; }

        public Meteoro(Game game) : base(game)
        {
        }

        public Meteoro(Game game, EnumTipoMeteoro e) 
            : this(game)
        {
            Tipo = e;
        }

        internal void Destruir()
        {
            ThisGame.Components.Remove(this);
            ThisGame.Player.Score++;

            var gerador = ThisGame.GeradorMeteoro;

            switch (Tipo)
            {
                case EnumTipoMeteoro.Grande: gerador.Gerar(2, EnumTipoMeteoro.Medio, Posicao); break;
                case EnumTipoMeteoro.Medio: gerador.Gerar(2, EnumTipoMeteoro.Pequeno, Posicao); break;
                default: break;
            }
        }

        internal bool Contem(Vector2 v)
        {
            var raio = Texture.Width / 2;

            var dx = Posicao.X - v.X;
            var dy = Posicao.Y - v.Y;

            var dist = MathF.Sqrt(dx * dx + dy * dy);

            return dist < raio;
        }    

        public override void Update(GameTime gameTime)
        {
            var dt = (float)gameTime.ElapsedGameTime.TotalSeconds;
            Posicao += Inercia * dt * Speed;

            if (Posicao.X > ThisGame.Window.ClientBounds.Width)
                Posicao = new Vector2(0, Posicao.Y);
            if (Posicao.X < 0)
                Posicao = new Vector2(ThisGame.Window.ClientBounds.Width, Posicao.Y);

            if (Posicao.Y > ThisGame.Window.ClientBounds.Height)
                Posicao = new Vector2(Posicao.X, 0);
            if (Posicao.Y < 0)
                Posicao = new Vector2(Posicao.X, ThisGame.Window.ClientBounds.Height);

            _direcao = _direcao.Rotate(Rotacao);
        }

        public override void Draw(GameTime gameTime)
        {
            if (Texture == null)
                return;

            ThisGame.SpriteBatch.Draw(Texture, Posicao, null, Color.White, -_direcao.Angle(), new Vector2(Texture.Width / 2, Texture.Height / 2), 1f, SpriteEffects.None, 0);
        }

        internal bool Contem(Vector2[] bounds)
        {
            foreach (var item in bounds)
                if (Contem(item))
                    return true;
            return false;
        }
    }
}
