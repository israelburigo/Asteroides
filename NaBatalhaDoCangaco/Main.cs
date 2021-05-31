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
using System.Collections.Generic;
using RedeNeural.Models;
using System.IO;

namespace NaBatalhaDoCangaco
{
    public class Main : Game
    {
        //public Player Player { get; set; }
        public GeradorMeteoro GeradorMeteoro { get; set; }
        public GUI Gui { get; set; }
        public bool Started { get; private set; }
        public GraphicsDeviceManager Graphics { get; set; }

        public List<IA> IAs { get; set; } = new List<IA>();
        public IA MelhorIA { get; set; }
        public int Geracao { get; set; }

        public Main()
        {
            Graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";

            GeradorMeteoro = new GeradorMeteoro(this);

            //Player = new Player(this);
           // Components.Add(Player);

            Gui = new GUI(this);
            Components.Add(Gui);
        }

        protected override void Initialize()
        {
            base.Initialize();

            Graphics.PreferredBackBufferWidth = 1000;
            Graphics.PreferredBackBufferHeight = 1000;

            Graphics.ApplyChanges();

            Globals.GameWindow = Window;

        }

        protected override void LoadContent()
        {
            base.LoadContent();

            Globals.SpriteBatch = new SpriteBatch(GraphicsDevice);
    
            Gui.SetFont(Content.Load<SpriteFont>("fonts/arial20"));
        }

        internal void Start()
        {
            Geracao++;
            Started = true;

            if (MelhorIA == null && File.Exists("melhorCerebro.dat"))
            {
                var sinapses = Globals.Deserialize<List<Synapse>>("melhorCerebro.dat");
                MelhorIA = new IA(this);
                MelhorIA.Cerebro.Synapses = sinapses;
            }

            for (int i = 0; i < 10; i++)
            {
                var ia = new IA(this)
                {
                    Texture = Content.Load<Texture2D>("2d/player"),
                    Posicao = new Vector2(Window.ClientBounds.Width / 2, Window.ClientBounds.Height / 2),
                    Score = new Score(),
                    Inercia = Vector2.Zero,
                    Direcao = Vector2.UnitX,
                };

                var cor = new MinMax(0, 1f);

                ia.Arma = new CanhaoSimples(ia)
                {
                    Cor = new Color(cor.Random(), cor.Random(), cor.Random())
                };
                Components.Add(ia);
            }            

            IAs = Components.OfType<IA>().ToList();

            if (MelhorIA != null && MelhorIA.Pontos > 0)
                new GeradorMutacao().Gerar(MelhorIA, IAs);

            Components.OfType<Meteoro>().ToList().ForEach(p => Components.Remove(p));

            GeradorMeteoro.Gerar(10);
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            base.Update(gameTime);

            Window.Title = $"Geracao: {Geracao}   MelhorIA = Pontos:{MelhorIA?.Score.Valor ?? 0} - LifeTime:{MelhorIA?.LifeTime ?? 0}s";

            GeradorMeteoro.Gerar(gameTime);

            if (!Components.OfType<IA>().Any())
                End();
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

            if (!IAs.Any())
                return;

            var melhor = IAs.OrderByDescending(p => p.Pontos)
                          .First();

            if (melhor.Melhor(MelhorIA))
                MelhorIA = melhor;

            IAs.Clear();

            Globals.Serialize("melhorCerebro.dat", MelhorIA.Cerebro.Synapses);

            Start();
        }
    }
}
