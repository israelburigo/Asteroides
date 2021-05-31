﻿using Asteroides.Engine;
using Asteroides.Engine.Components;
using Asteroides.Engine.Extensions;
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
        public List<Sensor> Sensores { get; set; } = new List<Sensor>();
        public int QuantSensores { get; set; } = 20;
        public bool ShowSensors { get; set; }

        public NeuralNetwork Cerebro { get; set; }
        public float Pontos { get { return LifeTime * Score.Valor; } }

        public IA(Game game) : base(game)
        {
            Arma = new CanhaoSimples(this);

            Cerebro = new NeuralNetwork(RandomSingleton.Instance)
            {
                ActivationType = EnumActivation.Sigmoid
            };

            Cerebro.Inputs.Add(new Neuron { Tag = "direcao_x" });
            Cerebro.Inputs.Add(new Neuron { Tag = "direcao_y" });
            Cerebro.Inputs.Add(new Neuron { Tag = "inercia_x" });
            Cerebro.Inputs.Add(new Neuron { Tag = "inercia_y" });
            Cerebro.Inputs.Add(new Neuron { Tag = "tempo_tiro" });

            for (int i = 0; i < QuantSensores; i++)
            {
                Cerebro.Inputs.Add(new Neuron { Tag = $"sensor_{i}" });
            }

            Cerebro.Outputs.Add(new Neuron { Tag = "acelera" });
            Cerebro.Outputs.Add(new Neuron { Tag = "esq" });
            Cerebro.Outputs.Add(new Neuron { Tag = "dir" });
            Cerebro.Outputs.Add(new Neuron { Tag = "atira" });
            Cerebro.Outputs.Add(new Neuron { Tag = "re" });

            Cerebro.Hiddens.Add(new HiddenNeurons(20));
            Cerebro.Hiddens.Add(new HiddenNeurons(10));

            Cerebro.BuildSynapses();

        }

        public override void Draw(GameTime gameTime)
        {
            if (!ThisGame.Started)
                return;

            if (Texture == null)
                return;

            if (ShowSensors)
                Sensores.ForEach(p => Globals.SpriteBatch.DrawLine(Posicao, p.Linha, p.Ativo ? Color.Red : Color.White, 1));

            Globals.SpriteBatch.Draw(Texture, Posicao, null, Arma.Cor, -Direcao.Angle(), new Vector2(Texture.Width / 2, Texture.Height / 2), 1f, SpriteEffects.None, 0);
        }

        public override void Update(GameTime gameTime)
        {
            if (!ThisGame.Started)
                return;

            var dt = (float)gameTime.ElapsedGameTime.TotalSeconds;

            LifeTime += dt;

            ShowSensors = Keyboard.GetState().IsKeyDown(Keys.Space);

            var meteoros = ThisGame.Components.OfType<Meteoro>();

            Cerebro.SetInput("direcao_x", Direcao.X);
            Cerebro.SetInput("direcao_y", Direcao.Y);
            Cerebro.SetInput("inercia_x", Inercia.X);
            Cerebro.SetInput("inercia_y", Inercia.Y);
            Cerebro.SetInput("tempo_tiro", Arma.TempoTiro);

            for (int i = 0; i < Sensores.Count; i++)
            {
                Cerebro.SetInput($"sensor_{i}", Sensores[i].Ativo ? Sensores[i].Distancia : 0);
            }

            Cerebro.FeedForward();

            var acelera = Cerebro.GetOutput("acelera")?.Value ?? 0;
            var esq = Cerebro.GetOutput("esq")?.Value ?? 0;
            var dir = Cerebro.GetOutput("dir")?.Value ?? 0;
            var atira = Cerebro.GetOutput("atira")?.Value ?? 0;
            var re = Cerebro.GetOutput("re")?.Value ?? 0;

            var keys = Keyboard.GetState().GetPressedKeys();

            if (Arma.Municao <= 0)
                Arma = new CanhaoSimples(this);

            if (acelera > 0.5)
                //if(keys.Contains(Keys.Up))
                Inercia += Direcao * dt * Aceleracao;

            if (re > 0.5)
                //if (keys.Contains(Keys.Down))
                Inercia -= Direcao * dt * Aceleracao / 5;

            if (esq > 0.5)
                //if (keys.Contains(Keys.Left))
                Direcao = Direcao.Rotate(0.1f);

            if (dir > 0.5)
                //if (keys.Contains(Keys.Right))
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

            if (atira > 0.5)
                Arma.Atira(ThisGame, gameTime, Bounds.First(), Direcao);

            Posicao += Inercia;

            Sensores = new List<Sensor>();
            for (int i = 0; i < QuantSensores; i++)
            {
                var a = MathHelper.ToRadians(360 * i / QuantSensores);
                var v1 = (Vector2.UnitX * 300).Rotate(a + angle);
                Sensores.Add(new Sensor { Linha = v1 + Posicao });
            }

            Sensores.ForEach(p =>
            {
                p.Ativo = meteoros.Any(q =>
                {
                    //if (p.Linha.X < 0)
                    //{
                    //    p.Distancia = Vector2.Distance(Posicao, new Vector2(0, Posicao.Y));
                    //    return true;
                    //}

                    //if (p.Linha.Y < 0)
                    //{
                    //    p.Distancia = Vector2.Distance(Posicao, new Vector2(Posicao.X, 0));
                    //    return true;
                    //}

                    //if (p.Linha.X > Globals.GameWindow.ClientBounds.Width)
                    //{
                    //    p.Distancia = Vector2.Distance(Posicao, new Vector2(Globals.GameWindow.ClientBounds.Width, Posicao.Y));
                    //    return true;
                    //}

                    //if (p.Linha.Y > Globals.GameWindow.ClientBounds.Height)
                    //{
                    //    p.Distancia = Vector2.Distance(Posicao, new Vector2(Posicao.X, Globals.GameWindow.ClientBounds.Height));
                    //    return true;
                    //}

                    var ret = p.Linha.IntersectCircle(Posicao, q.Posicao, q.Texture.Width / 2);
                    if (ret.Item1 == 2)
                    {
                        var pontoInter = ret.Item2;
                        if (Vector2.Distance(ret.Item3, Posicao) < Vector2.Distance(ret.Item2, Posicao))
                            pontoInter = ret.Item3;

                        var tamLinha = Vector2.Distance(Posicao, p.Linha);
                        var dlp = Vector2.Distance(p.Linha, pontoInter);
                        var dpp = Vector2.Distance(Posicao, pontoInter);

                        if (dpp < tamLinha && dlp <= tamLinha)
                        {
                            p.Distancia = dpp;
                            return true;
                        }
                    }

                    return false;
                });
            });

            if (meteoros.Any(p => p.Contem(Bounds)))
            {
                //new Particulas(Game)
                //{
                //    Quant = new MinMax(100),
                //    Angulo = new MinMax(0, 360),
                //    DuracaoDasParticulas = new MinMax(1f, 3f),
                //    Posicao = Posicao,
                //    Textura = ThisGame.Content.Load<Texture2D>("2d/particula"),
                //    Velocidade = new MinMax(10, 100),
                //    Color = Color.OrangeRed,
                //}.Start();

                ThisGame.Components.Remove(this);
            }
        }

        internal bool Melhor(IA melhorIA)
        {
            return melhorIA == null || Pontos > melhorIA.Pontos;
        }
    }
}
