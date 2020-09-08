using System;
using System.Collections.Generic;
using System.Linq;
using MongoDatabaseLibrary.Exceptions;
using MongoDatabaseLibrary.Interfaces;
using MongoDB.Driver;
using MongoDB.Driver.Core.Clusters;

namespace MongoDatabaseLibrary.Classes
{
    /// <summary>
    /// Менеджер подключения к базе данных
    /// </summary>
    public class ConnectionManager : IMongoConnection
    {
        /// <summary>
        /// Экземпляр подключения к базе данных
        /// </summary>
        private MongoClient _dbClient;

        /// <summary>
        /// Флаг подключения к базе
        /// </summary>
        public bool IsConnected { get; set; }

        /// <summary>
        /// Подключение к базе данных
        /// </summary>
        public void Connect()
        {
            try
            {
                _dbClient = new MongoClient();
                _dbClient.Cluster.DescriptionChanged += Cluster_DescriptionChanged;
            }
            catch (Exception e)
            {
                throw new ConnectionManagerException("Ошибка подключения к базе", e);
            }
        }


        /// <summary>
        /// Подключение к базе данных
        /// </summary>
        /// <param name="connectionString">Строка подключения</param>
        public void Connect(string connectionString)
        {
            if(string.IsNullOrEmpty(connectionString)) Connect();
            else
            {
                try
                {
                    _dbClient = new MongoClient(connectionString);
                    _dbClient.Cluster.DescriptionChanged += Cluster_DescriptionChanged;
                }
                catch (Exception e)
                {
                    throw new ConnectionManagerException("Ошибка подключения к базе", e);
                }
            }
        }

        /// <summary>
        /// Подключение к базе данных
        /// </summary>
        /// <param name="host">Имя хоста</param>
        /// <param name="port">Номер порта</param>
        public void Connect(string host, int port)
        {
            if(host == "localhost" && port == 27017) Connect();
            else
            {
                try
                {
                    var setting = new MongoClientSettings
                    {
                        ConnectionMode = ConnectionMode.Direct,
                        Server = new MongoServerAddress(host, port)
                    };

                    _dbClient = new MongoClient(setting);
                    _dbClient.Cluster.DescriptionChanged += Cluster_DescriptionChanged;
                }
                catch (Exception e)
                {
                    throw new ConnectionManagerException("Ошибка подключения к базе", e);
                }
            }
        }

        /// <summary>
        /// Подключение к базе данных
        /// </summary>
        /// <param name="host">Имя хоста</param>
        /// <param name="port">Номер порта</param>
        /// <param name="userName">Имя пользователя</param>
        /// <param name="password">Пароль</param>
        /// <param name="databaseName">Имя базы данных</param>
        public void Connect(string host, int port, string userName, string password, string databaseName)
        {
            try
            {
                var setting = new MongoClientSettings
                {
                    ConnectionMode = ConnectionMode.Direct,
                    Server = new MongoServerAddress(host, port),
                    Credential = MongoCredential.CreateCredential(databaseName, userName, password)
                };

                _dbClient = new MongoClient(setting);
                _dbClient.Cluster.DescriptionChanged += Cluster_DescriptionChanged;
            }
            catch (Exception e)
            {
                throw new ConnectionManagerException("Ошибка подключения к базе", e);
            }
        }

        /// <summary>
        /// Подключение к базе данных
        /// </summary>
        /// <param name="servers">Пара значений хост/порт</param>
        /// <param name="replicaSetName">Название реплики</param>
        public void Connect(Dictionary<string, int> servers, string replicaSetName)
        {
            try
            {
                var setting = new MongoClientSettings
                {
                    ConnectionMode = ConnectionMode.ReplicaSet,
                    ReplicaSetName = replicaSetName,
                    Servers = servers.Select(server => new MongoServerAddress(server.Key, server.Value)).ToList()
                };

                _dbClient = new MongoClient(setting);
                _dbClient.Cluster.DescriptionChanged += Cluster_DescriptionChanged;
            }
            catch (Exception e)
            {
                throw new ConnectionManagerException("Ошибка подключения к базе", e);
            }
        }

        /// <summary>
        /// Подключение к базе данных
        /// </summary>
        /// <param name="servers">Пара значений хост/порт</param>
        /// <param name="replicaSetName">Название реплики</param>
        /// <param name="userName">Имя пользователя</param>
        /// <param name="password">Пароль</param>
        /// <param name="databaseName">Имя базы данных</param>
        public void Connect(Dictionary<string, int> servers, string replicaSetName, string userName, string password,
            string databaseName)
        {
            try
            {
                var setting = new MongoClientSettings
                {
                    ConnectionMode = ConnectionMode.ReplicaSet,
                    ReplicaSetName = replicaSetName,
                    Credential = MongoCredential.CreateCredential(databaseName, userName, password),
                    Servers = servers.Select(server => new MongoServerAddress(server.Key, server.Value)).ToList()
                };

                _dbClient = new MongoClient(setting);
                _dbClient.Cluster.DescriptionChanged += Cluster_DescriptionChanged;
            }
            catch (Exception e)
            {
                throw new ConnectionManagerException("Ошибка подключения к базе", e);
            }
        }


        /// <summary>
        /// Возвращает экземпляр клиента MongoDb
        /// </summary>
        /// <returns>Возвращает экземпляр клиента подключенного к базе</returns>
        public MongoClient GetClient() => _dbClient;

        /// <summary>
        /// Обработчик события изменения описания кластера
        /// </summary>
        /// <param name="sender">Объект возникновения события</param>
        /// <param name="e">Аргументы события</param>
        private void Cluster_DescriptionChanged(object sender, ClusterDescriptionChangedEventArgs e)
        {
            if(e.NewClusterDescription.State == e.OldClusterDescription.State) return;

            IsConnected = e.NewClusterDescription.State == ClusterState.Connected;

            OnConnectionStateChanged();
        }


        /// <summary>
        /// Событие изменения состояния подключения
        /// </summary>
        public event Action ConnectionStateChanged;

        /// <summary>
        /// Экземпляр подключения к базе данных
        /// </summary>
        private void OnConnectionStateChanged() => ConnectionStateChanged?.Invoke();
    }
}