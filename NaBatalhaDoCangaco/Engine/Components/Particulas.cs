using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using NaBatalhaDoCangaco.Engine;
using System;
using System.Collections.Generic;
using System.Text;

namespace Asteroides.Engine.Components
{ 

    public class Particulas : GameComponent
    {
        public Vector2 Posicao { get; set; }
        public Texture2D Textura { get; set; }
        public MinMax Angulo { get; set; }
        public MinMax DuracaoDasParticulas { get; set; }
        public MinMax Velocidade { get; set; }        
        public MinMax Quant { get; set; }

        private List<Particula> _particulas = new List<Particula>();

        public Particulas(Game game)
            : base(game)
        {
            game.Components.Add(this);
        }
       
        public override void Update(GameTime gameTime)
        {
            _particulas.RemoveAll(p => p.Done);
            if (_particulas.Count == 0)
                Game.Components.Remove(this);
        }

        internal void Start()
        {
            for (int i = 0; i < Quant.RandomInt(); i++)
                _particulas.Add(new Particula(Game, this));
        }
    }
}
