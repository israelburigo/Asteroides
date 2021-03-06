using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using NaBatalhaDoCangaco.Engine;
using NaBatalhaDoCangaco.Engine.Extensions;
using System;
using System.Collections.Generic;
using System.Text;

namespace Asteroides.Engine.Components
{
    public class Particula : DrawableGameComponent
    {
        private readonly Particulas _parent;
        private readonly Vector2 _direcao;        
        private readonly float _velocidade;
        private float _duracao;
        private Vector2 _posicao;
        private Texture2D _textura;
        private Color _color;

        public bool Done { get { return _duracao <= 0; } }

        public Particula(Game game, Particulas parent) 
             : base(game)
        {
            game.Components.Add(this);

            _parent = parent;

            var angulo = _parent.Angulo?.RandomInt() ?? 1f;

            _velocidade = _parent.Velocidade?.Random() ?? 1f;
            _textura = parent.Textura;
            _duracao = _parent.DuracaoDasParticulas?.Random() ?? 1f;
            _direcao = Vector2.One.Rotate(MathHelper.ToRadians(angulo));
            _posicao = new Vector2(_parent.Posicao.X, _parent.Posicao.Y);
            _color = _parent.Color ?? Color.White;
        }

        public override void Draw(GameTime gameTime)
        {
            if (_textura == null)
                return;

            Globals.SpriteBatch.Draw(_textura, _posicao, _color);
        }

        public override void Update(GameTime gameTime)
        {
            var dt = (float)gameTime.ElapsedGameTime.TotalSeconds;
            
            _posicao += _direcao * _velocidade * dt;

            if ((_duracao -= dt) < 0)
                Game.Components.Remove(this);
        }

    }
}
