using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using NaBatalhaDoCangaco.Engine;
using System;
using System.Collections.Generic;
using System.Text;

namespace Asteroides.Engine.Components
{
    public enum EnumHowToRemoveParticles
    {
        PerTime,
        PerDistance
    }

    public class ParticlesObject<T> : BaseObject<T>
        where T : MainGame
    {
        public EnumHowToRemoveParticles ComoRemover { get; set; }
        public Vector2 Direcao { get; set; }
        public MinMax Angulo { get; set; }
        public MinMax DuracaoDasParticulas { get; set; }
        public MinMax Velocidade { get; set; }
        public float DistanciaMax { get; set; }
        public float Duracao { get; set; }
        public Vector2 Posicao { get; set; }
        public Texture2D Textura { get; internal set; }

        public int Quant { get; set; }
        public List<Particles<T>> Particulas { get; }

        public ParticlesObject(Game game) 
            : base(game)
        {
            Particulas = new List<Particles<T>>();
            ComoRemover = EnumHowToRemoveParticles.PerTime;
        }

        public override void Draw(GameTime gameTime)
        {
            Particulas.ForEach(p => p.Draw(gameTime));
        }

        public override void Update(GameTime gameTime)
        {
            var dt = (float)gameTime.ElapsedGameTime.TotalSeconds;

            if (Particulas.Count < Quant)
                Particulas.Add(new Particles<T>(ThisGame, this));

            Particulas.ForEach(p => p.Update(gameTime));
            Particulas.RemoveAll(p => p.Disposed);

            if ((Duracao -= dt) < 0)
                ThisGame.Components.Remove(this);
        }
    }
}
