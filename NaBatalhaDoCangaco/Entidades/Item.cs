using System;
using System.Linq;
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
        public Player Player { get; private set; }

        public EnumTipoItem TipoItem { get; private set; }
        public Texture2D Texture { get; private set; }
        public Vector2 Posicao { get; set; }
        public Vector2 Direcao { get; set; }
        public float Raio => Texture.Width / 2;
        
        public Item(Game game, EnumTipoItem tpItem) 
            : base(game)
        {
            TipoItem = tpItem;
            Texture = game.Content.Load<Texture2D>("2d/arma");
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

            if (Contem(Player.Bounds))
            {
                switch (TipoItem)
                {
                    case EnumTipoItem.CanhaoRapido: Player.Arma = new CanhaoRapido(); break;
                    case EnumTipoItem.CamnhaoTriplo: Player.Arma = new CanhaoTriplo(); break;
                    default: break;
                }
                ThisGame.Components.Remove(this);
            }
        }

        public override void Draw(GameTime gameTime)
        {
            if (Texture == null)
                return;

            var color = Color.White;
            switch (TipoItem)
            {
                case EnumTipoItem.CanhaoRapido: color = Color.Yellow; break;
                case EnumTipoItem.CamnhaoTriplo: color = Color.Green; break;
                default: break;
            }

            Globals.SpriteBatch.Draw(Texture, Posicao, null, color, -Direcao.Angle(), new Vector2(Texture.Width / 2, Texture.Height / 2), 1f, SpriteEffects.None, 0);
        }

        internal bool Contem(Vector2[] bounds)
        {
            return bounds.Any(Contem);
        }

        internal bool Contem(Vector2 v)
        {
            var dist = Vector2.Distance(v, Posicao);
            return dist < Raio;
        }
    }

    public enum EnumTipoItem
    {
        CanhaoRapido = 0,
        CamnhaoTriplo = 1
    }
}
