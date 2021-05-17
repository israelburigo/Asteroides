using System;
using System.Collections.Generic;
using System.Text;

namespace Asteroides.Entidades
{
    [Serializable]
    public class Rank
    {
        public string Name { get; set; }
        public int Score { get; set; }
    }

    [Serializable]
    public class Score
    {
        public int Valor { get; set; }        
        public int Max { get; set; }

        public List<Rank> Ranks { get; set; } = new List<Rank>(10);
    }
}
