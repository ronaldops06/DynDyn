using System;
using System.IO;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;

namespace Domain.Helpers
{
    public static class Log
    {
        private static ILoggerFactory _loggerFactory;

        public static void Configure(ILoggerFactory loggerFactory)
        {
            _loggerFactory = loggerFactory;
        }
        
        public static void Info<T>(string message)
        {
            GetLogger<T>().LogInformation(message);
            /*string caminhoArquivo = "/var/log/sagemoney";
            string nomeArquivo = "sagemoney.log";
            
            Directory.CreateDirectory(Path.GetDirectoryName(caminhoArquivo));
            
            using (StreamWriter writer = new StreamWriter($"{caminhoArquivo}/{nomeArquivo}", append: true))
            {
                writer.WriteLine($"{typeof(T)}|{DateTime.Now}: {message}");
            }*/
        }

        public static ILogger<T> GetLogger<T>()
        {
            if(_loggerFactory != null)
                return _loggerFactory.CreateLogger<T>();

            return new NullLogger<T>();
        }
    }
}