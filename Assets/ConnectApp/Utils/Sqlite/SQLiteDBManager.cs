using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using SQLite4Unity3d;
#if !UNITY_EDITOR
using UnityEngine;
#endif

namespace ConnectApp.Utils {
    public class SQLiteDBManager {
        const string DBName = "messenger";

        static SQLiteDBManager m_Instance;

        public static SQLiteDBManager instance {
            get {
                if (m_Instance == null) {
                    m_Instance = new SQLiteDBManager();
                }

                return m_Instance;
            }
        }

        SQLiteConnection m_Connection;

        SQLiteDBManager() {
            this.Reset();
        }

        void Reset(bool force = false) {
            if (!force && this.m_Connection != null) {
                return;
            }

            this.m_Connection?.Close();

#if UNITY_EDITOR
            var dbPath = $"Editor/{DBName}.db";
#else
            var dbPath = $"{Application.persistentDataPath}/{DBName}.db";
#endif
            try {
                bool dbExists = File.Exists(dbPath);
                this.m_Connection = new SQLiteConnection(dbPath, SQLiteOpenFlags.ReadWrite | SQLiteOpenFlags.Create);

                if (!dbExists) {
                    this.InitDB();
                }
            }
            catch (Exception e) {
                Debuger.LogError($"fatal error: fail to connect to database: {DBName}, error msg = {e.Message}");
            }
        }


        public void ClearAll() {
            this.Reset();
            this.InitDB();
        }

        void InitDB() {
            this.m_Connection.DropTable<DBMessageLite>();
            this.m_Connection.CreateTable<DBMessageLite>();
            this.m_Connection.DropTable<FileRecordLite>();
            this.m_Connection.CreateTable<FileRecordLite>();
            this.m_Connection.DropTable<DBReadyStateLite>();
            this.m_Connection.CreateTable<DBReadyStateLite>();
        }

        public string GetCachedFilePath(string url) {
            var ret = this.m_Connection.Table<FileRecordLite>().Where(record => record.url == url);

            if (!ret.Any()) {
                return null;
            }

            if (ret.Count() == 1) {
                return ret.First().filepath;
            }

            DebugerUtils.DebugAssert(false, "fatal error: duplicated files are mapping to one url.");
            return null;
        }

        public void UpdateCachedFilePath(string url, string filePath) {
            this.m_Connection.Insert(new FileRecordLite {url = url, filepath = filePath}, extra: "OR REPLACE");
        }

        public void SaveMessages(List<DBMessageLite> data) {
            this.m_Connection.InsertAll(data, extra: "OR REPLACE");
        }

        public IEnumerable<DBMessageLite> QueryMessages(string channelId, long maxKey = -1, int maxCount = 5) {
            if (maxKey == -1) {
                return this.m_Connection.Table<DBMessageLite>().Where(message => message.channelId == channelId)
                    .OrderByDescending(message => message.messageKey).Take(maxCount);
            }
            else {
                return this.m_Connection.Table<DBMessageLite>().Where(message => message.channelId == channelId &&
                                                                                 message.messageKey < maxKey)
                    .OrderByDescending(message => message.messageKey).Take(maxCount);
            }
        }

        public void SaveReadyState(DBReadyStateLite data) {
            this.m_Connection.Insert(data, extra: "OR REPLACE");
        }

        public DBReadyStateLite LoadReadyState() {
            var ret = this.m_Connection.Table<DBReadyStateLite>()
                .Where(record => record.key == DBReadyStateLite.DefaultReadyStateKey);

            if (!ret.Any()) {
                return null;
            }

            if (ret.Count() == 1) {
                return ret.First();
            }

            DebugerUtils.DebugAssert(false, "fatal error: duplicated ready states are mapping to the default key.");
            return null;
        }
    }
}