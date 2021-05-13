using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using NaBatalhaDoCangaco.Engine;
using System;
using System.Collections.Generic;
using System.Text;

namespace Asteroides.Engine.Components
{ 

    public class Particulas<T> : ObjetoBase<T>
        where T : Game
    {
        public Vector2 Posicao { get; set; }
        public Texture2D Textura { get; set; }
        public MinMax Angulo { get; set; }
        public MinMax DuracaoDasParticulas { get; set; }
        public MinMax Velocidade { get; set; }        
        public MinMax Quant { get; set; }

        private List<Particula<T>> _particulas = new List<Particula<T>>();

        public Particulas(Game game)
            : base(game)
        {
            game.Components.Add(this);
        }
       
        public override void Draw(GameTime gameTime)
        {
            _particulas.ForEach(p => p.Draw(gameTime));
        }

        public override void Update(GameTime gameTime)
        {
            _particulas.ForEach(p => p.Update(gameTime));
            _particulas.RemoveAll(p => p.Done);

            if (_particulas.Count == 0)
                ThisGame.Components.Remove(this);
        }

        internal void Start()
        {
            for (int i = 0; i < Quant.RandomInt(); i++)
                _particulas.Add(new Particula<T>(ThisGame, this));
        }
    }
}
