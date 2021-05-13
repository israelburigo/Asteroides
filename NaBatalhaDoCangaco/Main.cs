using Asteroides.GUIs;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using NaBatalhaDoCangaco.Engine;
using System;
using System.Linq;
using Asteroides.Entidades;
using Asteroides.Engine;

namespace NaBatalhaDoCangaco
{
    public class Main : MainGame
    {
        public Player Player { get; set; }
        public GeradorMeteoro GeradorMeteoro { get; set; }
        public GeradorItem GeradorItem { get; set; }
        public GUI Gui { get; set; }
        public bool Started { get; private set; }

        public Main()
        {
            GeradorMeteoro = new GeradorMeteoro(this);
            GeradorItem = new GeradorItem(this);

            Player = new Player(this);
            Components.Add(Player);

            Gui = new GUI(this);
            Components.Add(Gui);
        }

        protected override void Initialize()
        {
            base.Initialize();

            Graphics.PreferredBackBufferWidth = 1000;
            Graphics.PreferredBackBufferHeight = 1000;

            Graphics.ApplyChanges();
        }

        protected override void LoadContent()
        {
            base.LoadContent();
            Player.Texture = Content.Load<Texture2D>("2d/player");
            Gui.SetFont(Content.Load<SpriteFont>("fonts/arial20"));
        }

        internal void Start()
        {
            Started = true;

            Player.Posicao = new Vector2(Window.ClientBounds.Width / 2, Window.ClientBounds.Height / 2);

            Player.Inercia = Vector2.Zero;
            Player.Direcao = Vector2.UnitX;

            Components.OfType<Meteoro>().ToList().ForEach(p => Components.Remove(p));

            GeradorMeteoro.Gerar(10);
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            base.Update(gameTime);

            Window.Title = Components.Count.ToString();

            GeradorMeteoro.Gerar(gameTime);
            GeradorItem.Gerar(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            SpriteBatch.Begin();

            base.Draw(gameTime);

            SpriteBatch.End();
        }

        internal void End()
        {
            Started = false;

            if (Player.Score > Player.MaxScore)
                Player.MaxScore = Player.Score;
            Player.Score = 0;
        }
    }
}
