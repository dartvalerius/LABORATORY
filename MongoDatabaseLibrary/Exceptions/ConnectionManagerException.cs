using System;

namespace MongoDatabaseLibrary.Exceptions
{
    /// <summary>
    /// Класс исключения подключения к базе
    /// </summary>
    public class ConnectionManagerException : ArgumentException
    {
        /// <summary>
        /// Исключение MongoDb
        /// </summary>
        public Exception MongoException { get; }

        /// <summary>
        /// Конструктор класса исключений подключения к базе
        /// </summary>
        public ConnectionManagerException(string message): base(message)
        {
            
        }

        /// <summary>
        /// Конструктор класса исключений подключения к базе
        /// </summary>
        /// <param name="message">Сообщение</param>
        /// <param name="e">Внутреннее исключение драйвера MongoDb</param>
        public ConnectionManagerException(string message, Exception e) : base(message) => MongoException = e;
    }
}