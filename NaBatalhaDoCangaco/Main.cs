using Asteroides.GUIs;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using NaBatalhaDoCangaco.Engine;
using NaBatalhaDoCangaco.Entidades;
using System;
using System.Linq;

namespace NaBatalhaDoCangaco
{
    public class Main : Game
    {
        public GraphicsDeviceManager Graphics { get; set; }
        public SpriteBatch SpriteBatch { get; set; }
        public static Random Random = new Random();
        public Player Player { get; set; }
        public GeradorMeteoro GeradorMeteoro { get; set; }
        public GUI Gui { get; set; }
        public bool Started { get; private set; }

        public Main()
        {
            IsMouseVisible = true;

            Graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";

            //Graphics.SynchronizeWithVerticalRetrace = false;
            //IsFixedTimeStep = false;

            GeradorMeteoro = new GeradorMeteoro(this);

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
            SpriteBatch = new SpriteBatch(GraphicsDevice);
            Player.Texture = Content.Load<Texture2D>("2d/player");
            Player.TiroTexture = Content.Load<Texture2D>("2d/tiro");
            Gui.SetFont(Content.Load<SpriteFont>("fonts/arial20"));
        }

        internal void Start()
        {
            Started = true;

            Player.Posicao = new Vector2(Window.ClientBounds.Width / 2, Window.ClientBounds.Height / 2);

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
