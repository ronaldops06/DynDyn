using System;
using System.IO;

namespace Domain.Helpers
{
    public class Log
    {
        public static void Info<T>(string message)
        {
            string caminhoArquivo = "/var/log/sagemoney";
            string nomeArquivo = "sagemoney.log";
            
            Directory.CreateDirectory(Path.GetDirectoryName(caminhoArquivo));
            
            using (StreamWriter writer = new StreamWriter($"{caminhoArquivo}/{nomeArquivo}", append: true))
            {
                writer.WriteLine($"{typeof(T)}|{DateTime.Now}: {message}");
            }
        }
    }
}