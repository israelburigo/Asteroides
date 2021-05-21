using Microsoft.Xna.Framework;
using NaBatalhaDoCangaco;
using NaBatalhaDoCangaco.Engine;
using System;
using System.Collections.Generic;
using System.Text;

namespace Asteroides.Entidades
{
    public class PlayerBase : ObjetoBase<Main>
    {
        public Score Score { get; set; } = new Score();

        public PlayerBase(Game game) : base(game)
        {
        }
    }
}
