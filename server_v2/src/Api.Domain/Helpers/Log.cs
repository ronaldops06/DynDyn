using System;
using System.IO;

namespace Domain.Helpers
{
    public class Log
    {
        public static void Info<T>(string message)
        {
            string caminhoArquivo = "/home/ronaldo/Logs";
            string nomeArquivo = "sagemoney.txt";
            
            Directory.CreateDirectory(Path.GetDirectoryName(caminhoArquivo));
            
            using (StreamWriter writer = new StreamWriter($"{caminhoArquivo}/{nomeArquivo}", append: true))
            {
                writer.WriteLine($"{typeof(T)}|{DateTime.Now}: {message}");
            }
        }
    }
}