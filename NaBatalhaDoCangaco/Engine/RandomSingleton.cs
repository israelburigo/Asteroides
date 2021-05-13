using System;
using System.Collections.Generic;
using System.Text;

namespace Asteroides.Engine
{
    public class RandomSingleton : Random
    {
        public static RandomSingleton Instance { get; } = new RandomSingleton();

        private RandomSingleton() { }
    }
}
