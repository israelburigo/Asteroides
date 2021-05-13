using System;
using Asteroides.Engine;
using Asteroides.Entidades.Armas;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using NaBatalhaDoCangaco;
using NaBatalhaDoCangaco.Engine;
using NaBatalhaDoCangaco.Engine.Extensions;

namespace Asteroides.Entidades
{
    public class Item : ObjetoBase<Main>
    {
        public Player Player { get; set; }
        public Texture2D Texture { get; set; }
        public Vector2 Posicao { get; set; }
        public Vector2 Direcao { get; set; }
        public float Rotacao { get; set; }
        public EnumTipoItem TipoItem { get; set; }

        public Item(Game game, EnumTipoItem tipoItem) : base(game)
        {
            switch (tipoItem)
            {
                case EnumTipoItem.CanhaoRapido:
                    Texture = game.Content.Load<Texture2D>("2d/arma1");
                    break;
                case EnumTipoItem.CamnhaoTriplo:
                    Texture = game.Content.Load<Texture2D>("2d/arma2");
                    break;
                default: break;
            }

            game.Components.Add(this);
        }

        public override void Initialize()
        {
            Player = ThisGame.Player;
        }

        public override void Update(GameTime gameTime)
        {
            var dt = (float)gameTime.ElapsedGameTime.TotalSeconds;
            Posicao += Direcao * dt * 100;

            if (Posicao.X > ThisGame.Window.ClientBounds.Width || Posicao.X < 0)
                ThisGame.Components.Remove(this);

            if (Posicao.Y > ThisGame.Window.ClientBounds.Height || Posicao.Y < 0)
                ThisGame.Components.Remove(this);


            if (Player.Contem(Posicao))
            {
                switch (TipoItem)
                {
                    case EnumTipoItem.CanhaoRapido: 
                        Player.Arma = new CanhaoRapido();
                        break;
                    case EnumTipoItem.CamnhaoTriplo:
                        Player.Arma = new CanhaoTriplo();
                        break;
                    default: break;
                }

                ThisGame.Components.Remove(this);
            }
        }

        public override void Draw(GameTime gameTime)
        {
            if (Texture == null)
                return;

            Globals.SpriteBatch.Draw(Texture, Posicao, null, Color.White, -Direcao.Angle(), new Vector2(Texture.Width / 2, Texture.Height / 2), 1f, SpriteEffects.None, 0);
        }
    }

    public enum EnumTipoItem
    {
        CanhaoRapido = 0,
        CamnhaoTriplo = 1
    }
}
