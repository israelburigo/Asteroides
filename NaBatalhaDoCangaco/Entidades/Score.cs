using System;
using System.Collections.Generic;
using System.Text;

namespace Asteroides.Entidades
{
    [Serializable]
    public class Score
    {
        public int Valor { get; set; }        
        public int Max { get; set; }
    }
}
