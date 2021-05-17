using System;
using System.Collections.Generic;
using System.Text;

namespace Asteroides.Arquivos
{
    public class Cripto
    {
        public static void Encripta(byte[] bytes)
        {
            for (int i = 0; i < bytes.Length; i++)
                bytes[i] = (byte)(bytes[i] + 12);
        }

        public static void Decripta(byte[] bytes)
        {
            for (int i = 0; i < bytes.Length; i++)
                bytes[i] = (byte)(bytes[i] - 12);
        }
    }
}
