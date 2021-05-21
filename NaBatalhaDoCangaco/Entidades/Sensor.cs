using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace Asteroides.Entidades
{
    public class Sensor
    {
        public Vector2 Linha { get; set; }
        public bool Ativo { get; set; }
        public float Distancia { get; set; }
    }
}
