using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NaBatalhaDoCangaco.Engine.Extensions
{
    public static class GameComponentCollectionExtension
    {
        public static IEnumerable<T> GetAll<T>(this GameComponentCollection list)
        {
            return list.Where(p => p is T).Cast<T>();
        }
    }
}
