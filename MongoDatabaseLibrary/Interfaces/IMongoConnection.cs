using System;
using System.Collections.Generic;
using MongoDB.Driver;

namespace MongoDatabaseLibrary.Interfaces
{
    /// <summary>
    /// Интерфейс подключения к базе данных
    /// </summary>
    internal interface IMongoConnection
    {
        /// <summary>
        /// Флаг подключения к базе
        /// </summary>
        public bool IsConnected { get; set; }


        /// <summary>
        /// Подключение к базе данных
        /// </summary>
        public void Connect();

        /// <summary>
        /// Подключение к базе данных
        /// </summary>
        /// <param name="connectionString">Строка подключения</param>
        public void Connect(string connectionString);

        /// <summary>
        /// Подключение к базе данных
        /// </summary>
        /// <param name="host">Имя хоста</param>
        /// <param name="port">Номер порта</param>
        public void Connect(string host, int port);

        /// <summary>
        /// Подключение к базе данных
        /// </summary>
        /// <param name="host">Имя хоста</param>
        /// <param name="port">Номер порта</param>
        /// <param name="userName">Имя пользователя</param>
        /// <param name="password">Пароль</param>
        /// <param name="databaseName">Имя базы данных</param>
        public void Connect(string host, int port, string userName, string password, string databaseName);

        /// <summary>
        /// Подключение к базе данных
        /// </summary>
        /// <param name="servers">Пара значений хост/порт</param>
        /// <param name="replicaSetName">Название реплики</param>
        public void Connect(Dictionary<string, int> servers, string replicaSetName);

        /// <summary>
        /// Подключение к базе данных
        /// </summary>
        /// <param name="servers">Пара значений хост/порт</param>
        /// <param name="replicaSetName">Название реплики</param>
        /// <param name="userName">Имя пользователя</param>
        /// <param name="password">Пароль</param>
        /// <param name="databaseName">Имя базы данных</param>
        public void Connect(Dictionary<string, int> servers, string replicaSetName, string userName, string password, string databaseName);

        /// <summary>
        /// Возвращает экземпляр клиента MongoDb
        /// </summary>
        /// <returns>Возвращает экземпляр клиента подключенного к базе</returns>
        public MongoClient GetClient();


        /// <summary>
        /// Событие изменения состояния подключения
        /// </summary>
        public event Action ConnectionStateChanged;
    }
}