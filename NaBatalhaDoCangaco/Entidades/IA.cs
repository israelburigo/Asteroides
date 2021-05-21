using Asteroides.Engine;
using Asteroides.Engine.Components;
using Asteroides.Entidades.Armas;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using NaBatalhaDoCangaco;
using NaBatalhaDoCangaco.Engine;
using NaBatalhaDoCangaco.Engine.Extensions;
using RedeNeural.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Asteroides.Entidades
{
    public class IA : PlayerBase
    {
        public Vector2[] Bounds { get; set; }
        public Texture2D Texture { get; set; }
        public Vector2 Posicao { get; set; }
        public Vector2 Direcao { get; set; } = Vector2.UnitX;
        public Vector2 Inercia { get; set; } = Vector2.Zero;
        public IArma Arma { get; set; }
        public float Aceleracao { get; set; } = 10;
        public float LifeTime { get; set; }

        public NeuralNetwork Cerebro { get; set; }

        public IA(Game game) : base(game)
        {
            Arma = new CanhaoSimples(this);

            Cerebro = new NeuralNetwork(RandomSingleton.Instance)
            {
                ActivationType = EnumActivation.Relu
            };

            Cerebro.Inputs.Add(new Neuron { Tag = "dist_x" });
            Cerebro.Inputs.Add(new Neuron { Tag = "dist_y" });
            Cerebro.Inputs.Add(new Neuron { Tag = "inercia_x" });
            Cerebro.Inputs.Add(new Neuron { Tag = "inercia_y" });
            Cerebro.Inputs.Add(new Neuron { Tag = "angulo" });

            Cerebro.Outputs.Add(new Neuron { Tag = "acelera" }); 
            Cerebro.Outputs.Add(new Neuron { Tag = "re" }); 
            Cerebro.Outputs.Add(new Neuron { Tag = "esq" }); 
            Cerebro.Outputs.Add(new Neuron { Tag = "dir" }); 
            Cerebro.Outputs.Add(new Neuron { Tag = "atira" }); 

            Cerebro.Hiddens.Add(new HiddenNeurons(10));            

            Cerebro.BuildSynapses();
        }

        public override void Draw(GameTime gameTime)
        {
            if (!ThisGame.Started)
                return;

            if (Texture == null)
                return;

            Globals.SpriteBatch.Draw(Texture, Posicao, null, Arma.Cor, -Direcao.Angle(), new Vector2(Texture.Width / 2, Texture.Height / 2), 1f, SpriteEffects.None, 0);
        }

        public override void Update(GameTime gameTime)
        {
            if (!ThisGame.Started)
                return;

            var dt = (float)gameTime.ElapsedGameTime.TotalSeconds;

            LifeTime += dt;

            var meteoros = ThisGame.Components.OfType<Meteoro>();

            var dist = PegaDistMeteoroMaisProximo(meteoros);
            
            Cerebro.SetInput("angulo", -Direcao.Angle());
            Cerebro.SetInput("inercia_y", Inercia.X);
            Cerebro.SetInput("inercia_x", Inercia.Y);
            Cerebro.SetInput("dist_x", dist.X);
            Cerebro.SetInput("dist_y", dist.Y);

            Cerebro.FeedForward();

            var acelera = Cerebro.GetOutput("acelera")?.Value ?? 0;
            var re = Cerebro.GetOutput("re")?.Value ?? 0;
            var esq = Cerebro.GetOutput("esq")?.Value ?? 0;
            var dir = Cerebro.GetOutput("dir")?.Value ?? 0;
            var atira = Cerebro.GetOutput("atira")?.Value ?? 0;

            if (Arma.Municao <= 0)
                Arma = new CanhaoSimples(this);

            if(acelera > 0)
                Inercia += Direcao * dt * Aceleracao;

            if (re > 0)
                Inercia -= Direcao * dt * Aceleracao / 5;

            if (esq > 0)
                Direcao = Direcao.Rotate(0.1f);

            if (dir > 0)
                Direcao = Direcao.Rotate(-0.1f);

            if (Posicao.X > ThisGame.Window.ClientBounds.Width)
                Posicao = new Vector2(0, Posicao.Y);
            if (Posicao.X < 0)
                Posicao = new Vector2(ThisGame.Window.ClientBounds.Width, Posicao.Y);

            if (Posicao.Y > ThisGame.Window.ClientBounds.Height)
                Posicao = new Vector2(Posicao.X, 0);
            if (Posicao.Y < 0)
                Posicao = new Vector2(Posicao.X, ThisGame.Window.ClientBounds.Height);

            var angle = -Direcao.Angle();
            Bounds = new[]
            {
                new Vector2(Posicao.X, Posicao.Y + Texture.Height/2).Rotate(angle, Posicao),
                new Vector2(Posicao.X - Texture.Width/2, Posicao.Y - Texture.Height/2 ).Rotate(angle, Posicao),
                new Vector2(Posicao.X + Texture.Width/2, Posicao.Y - Texture.Height/2 ).Rotate(angle, Posicao),
            };

            if (atira > 0)
                Arma.Atira(ThisGame, gameTime, Bounds.First(), Direcao);

            Posicao += Inercia;

            if (meteoros.Any(p => p.Contem(Bounds)))
            {
                new Particulas(Game)
                {
                    Quant = new MinMax(100),
                    Angulo = new MinMax(0, 360),
                    DuracaoDasParticulas = new MinMax(1f, 3f),
                    Posicao = Posicao,
                    Textura = ThisGame.Content.Load<Texture2D>("2d/particula"),
                    Velocidade = new MinMax(10, 100),
                    Color = Color.OrangeRed,
                }.Start();

                ThisGame.Components.Remove(this);
            }
        }

        private Vector2 PegaDistMeteoroMaisProximo(IEnumerable<Meteoro> meteoros)
        {
            var m = meteoros.FirstOrDefault();
            if (m == null)
                return Vector2.Zero;

            var dx = m.Posicao.X - Posicao.X;
            var dy = m.Posicao.Y - Posicao.Y;
            var dist = MathF.Sqrt(dx * dx + dy * dy);

            var distMin = dist;
            var ret = new Vector2(m.Posicao.X, m.Posicao.Y);

            foreach (var meteoro in meteoros)
            {
                var raio = meteoro.Texture.Width / 2;
                dx = meteoro.Posicao.X - Posicao.X;
                dy = meteoro.Posicao.Y - Posicao.Y;
                dist = MathF.Sqrt(dx * dx + dy * dy) - raio;
                if(dist < distMin)
                    ret = new Vector2(meteoro.Posicao.X, meteoro.Posicao.Y);
            }

            return ret;
        }
    }
}
