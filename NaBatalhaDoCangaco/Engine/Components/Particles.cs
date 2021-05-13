using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using NaBatalhaDoCangaco.Engine;
using NaBatalhaDoCangaco.Engine.Extensions;
using System;
using System.Collections.Generic;
using System.Text;

namespace Asteroides.Engine.Components
{
    public class Particles<T> : BaseObject<T>
        where T : MainGame
    {
        private readonly ParticlesObject<T> _parent;
        private readonly Vector2 _direcao;
        private float _duracao;
        private float _velocidade;
        private Vector2 _posicao;
        private readonly Texture2D _textura;
        public bool Disposed { get; private set; }

        public Particles(MainGame game, ParticlesObject<T> parent) 
            : base(game)
        {
            _parent = parent;
            _velocidade = _parent.Velocidade?.Random() ?? 1f;
            _textura = parent.Textura;
            _duracao = _parent.DuracaoDasParticulas?.Random() ?? 1f;
            _direcao = Vector2.One.Rotate(MathHelper.ToRadians(_parent.Angulo?.RandomInt() ?? 1f));
            _posicao = new Vector2(_parent.Posicao.X, _parent.Posicao.Y);
        }

        public override void Draw(GameTime gameTime)
        {
            if (_textura == null)
                return;

            ThisGame.SpriteBatch.Draw(_textura, _posicao, Color.White);
        }

        public override void Update(GameTime gameTime)
        {
            var dt = (float)gameTime.ElapsedGameTime.TotalSeconds;

            if (_parent.ComoRemover == EnumHowToRemoveParticles.PerTime)
                Disposed = (_duracao -= dt) < 0;

            if (_parent.ComoRemover == EnumHowToRemoveParticles.PerDistance)
                Disposed = Vector2.Distance(_parent.Posicao, _posicao) > _parent.DistanciaMax;

            _posicao += _direcao * _velocidade * dt;

            if (Disposed)
                ThisGame.Components.Remove(this);
        }

    }
}
