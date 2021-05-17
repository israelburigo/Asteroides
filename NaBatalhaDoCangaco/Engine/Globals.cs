using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;

namespace Asteroides.Engine
{
    public class Globals
    {
        public static SpriteBatch SpriteBatch;
        public static GameWindow GameWindow;

        public static void Serialize<T>(string path, T obj, Action<byte[]> serializer = null)
        {
            var bf = new BinaryFormatter();
            using var ms = new MemoryStream();
            bf.Serialize(ms, obj);

            var bytes = ms.ToArray();

            serializer?.Invoke(bytes);

            File.WriteAllBytes(path, bytes);
        }

        public static T Deserialize<T>(string path, Action<byte[]> deserializer = null)
        {
            var bf = new BinaryFormatter();
            using var ms = new MemoryStream();
            try
            {
                var bytes = File.ReadAllBytes(path);

                deserializer?.Invoke(bytes);

                ms.Write(bytes, 0, bytes.Length);
                ms.Position = 0;
                return (T)bf.Deserialize(ms);
            }
            catch
            {
                return default;
            }
        }
    }
}
