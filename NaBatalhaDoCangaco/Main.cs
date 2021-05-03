using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using NaBatalhaDoCangaco.Engine;
using NaBatalhaDoCangaco.Entidades;
using System;

namespace NaBatalhaDoCangaco
{
    public class Main : Game
    {
        public GeradorMeteoro GeradorMeteoro { get; set; }

        public Main()
        {
            IsMouseVisible = true;
            Globals.Game = this;

            Globals.Graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";

            GeradorMeteoro = new GeradorMeteoro();

            Globals.Player = new Player();
            Components.Add(Globals.Player);
        }

        protected override void Initialize()
        {
            base.Initialize();

            Globals.Graphics.PreferredBackBufferWidth = 1000;
            Globals.Graphics.PreferredBackBufferHeight = 1000;

            Globals.Graphics.ApplyChanges();

            Globals.GetPlayer<Player>().Posicao = new Vector2(Globals.Game.Window.ClientBounds.Width / 2, Globals.Game.Window.ClientBounds.Height / 2);

            GeradorMeteoro.Gerar(10);
        }

        protected override void LoadContent()
        {
            Globals.SpriteBatch = new SpriteBatch(GraphicsDevice);
            Globals.GetPlayer<Player>().Texture = Content.Load<Texture2D>("2d/player");
            Globals.GetPlayer<Player>().TiroTexture = Content.Load<Texture2D>("2d/tiro");
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            base.Update(gameTime);

            Window.Title = Components.Count.ToString();

            GeradorMeteoro.Gerar(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            Globals.SpriteBatch.Begin();

            base.Draw(gameTime);

            Globals.SpriteBatch.End();
        }
    }
}
