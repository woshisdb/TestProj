//����SQLite�����ռ�
using Mono.Data.Sqlite;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;

public class SQLiteTest : MonoBehaviour
{
    private void Start()
    {
        string filePath = Path.Combine(Application.streamingAssetsPath, "SQLiteData.db");
        OpenSQLiteFile(filePath);
    }


    /// <summary>
    /// �򿪻򴴽����ݿ�
    /// </summary>
    /// <param name="path"></param>
    public static void OpenSQLiteFile(string path)
    {
        try
        {
            var _connection = new SqliteConnection($"URI=file:{path}");
            _connection.Open();

            Debug.Log("Database Connect!!!");
        }
        catch (System.Exception e)
        {

            Debug.LogError(e.Message);
        }
    }
}