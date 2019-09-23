using System;
using System.Collections.Generic;
using System.IO;
using SQLite4Unity3d;
using UnityEngine;

namespace ConnectApp.Utils {
    public class SQLiteDBManager {
        SQLiteConnection m_Connection;

        public SQLiteDBManager(string dbName) {
            
#if UNITY_EDITOR
            var dbPath = $"Assets/{dbName}.db";
#else
            var dbPath = $"{Application.persistentDataPath}/{dbName}.db";
#endif
            try {
                bool dbExists = File.Exists(dbPath);
                this.m_Connection = new SQLiteConnection(dbPath, SQLiteOpenFlags.ReadWrite | SQLiteOpenFlags.Create);

                if (!dbExists) {
                    this.InitDB();
                }
            }
            catch (Exception e) {
                Debug.Log($"fatal error: fail to connect to database: {dbName}");
            }
        }

        void InitDB() {
            this.m_Connection.DropTable<DBMessageLite>();
            this.m_Connection.CreateTable<DBMessageLite>();
        }

        public void SaveMessages(List<DBMessageLite> data) {
            this.m_Connection.InsertAll(data, extra: "OR REPLACE");
        }

        public IEnumerable<DBMessageLite> QueryMessages(string channelId, int maxNonce = -1, int maxCount = 5) {
            if (maxNonce == -1) {
                return this.m_Connection.Table<DBMessageLite>().Where(message => message.channelId == channelId)
                    .OrderByDescending(message => message.nonce).Take(maxCount);
            }
            else {
                return this.m_Connection.Table<DBMessageLite>().Where(message => message.channelId == channelId &&
                                                                                 message.nonce < maxNonce)
                    .OrderByDescending(message => message.nonce).Take(maxCount);
            }
        }
    }
}