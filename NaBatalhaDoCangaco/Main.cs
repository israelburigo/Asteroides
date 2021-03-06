using Asteroides.GUIs;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using NaBatalhaDoCangaco.Engine;
using System;
using System.Linq;
using Asteroides.Entidades;
using Asteroides.Engine;
using Asteroides.Arquivos;
using Asteroides.Entidades.Armas;
using Asteroides.Engine.Components;
using Asteroides.Geradores;

namespace NaBatalhaDoCangaco
{
    public class Main : Game
    {
        public Player Player { get; set; }
        public GeradorMeteoro GeradorMeteoro { get; set; }
        public GeradorItem GeradorItem { get; set; }
        public GUI Gui { get; set; }
        public bool Started { get; private set; }

        public GraphicsDeviceManager Graphics { get; set; }

        public Main()
        {
            Graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";

            GeradorMeteoro = new GeradorMeteoro(this);
            GeradorItem = new GeradorItem(this);
        }

        protected override void Initialize()
        {
            base.Initialize();

            Graphics.PreferredBackBufferWidth = 826;
            Graphics.PreferredBackBufferHeight = 640;
            Graphics.ApplyChanges();

            Player = new Player(this);
            Gui = new GUI(this);

            if (System.IO.File.Exists("save.dat"))
            {
                var score = Globals.Deserialize<Score>("save.dat", Cripto.Decripta);
                Player.Score = score ?? new Score();
                Player.Score.Valor = 0;
            }
        }

        protected override void LoadContent()
        {

            base.LoadContent();
            Globals.SpriteBatch = new SpriteBatch(GraphicsDevice);
        }

        internal void Start()
        {
            Started = true;

            Player.Posicao = new Vector2(Window.ClientBounds.Width / 2, Window.ClientBounds.Height / 2);
            Player.Score.Valor = 0;
            Player.Inercia = Vector2.Zero;
            Player.Direcao = Vector2.UnitX;
            Player.Arma = new CanhaoSimples();

            Components.OfType<Meteoro>().ToList().ForEach(p => Components.Remove(p));

            GeradorMeteoro.Gerar(10);
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            base.Update(gameTime);

            Window.Title = Components.Count.ToString();

            GeradorItem.Gerar(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            Globals.SpriteBatch.Begin();

            base.Draw(gameTime);

            Globals.SpriteBatch.End();
        }

        internal void End()
        {
            Started = false;

            new Particulas(this)
            {
                Quant = new MinMax(100),
                Angulo = new MinMax(0, 360),
                DuracaoDasParticulas = new MinMax(1f, 3f),
                Posicao = Player.Posicao,
                Textura = Content.Load<Texture2D>("2d/particula"),
                Velocidade = new MinMax(10, 100),
                Color = Color.OrangeRed,
            }.Start();

            if (Player.Score.Valor > Player.Score.Max)
            {
                Player.Score.Max = Player.Score.Valor;
                Globals.Serialize("save.dat", Player.Score, Cripto.Encripta);
            }            
        }
    }
}
