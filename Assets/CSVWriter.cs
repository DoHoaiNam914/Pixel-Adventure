using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class CSVWriter : MonoBehaviour
{
    private string _fileName = "";

    [SerializeField]
    private FruitList _fruitList = new FruitList();
    [SerializeField]
    private EnemyList _enemyList = new EnemyList();

    [Serializable]
    private class Fruit
    {
        public string name;
        public int point;
    }

    [Serializable]
    private class FruitList
    {
        public Fruit[] list;
    }

    [Serializable]
    private class Enemy
    {
        public string name;
        public int point;
    }

    [Serializable]
    private class EnemyList
    {
        public Fruit[] list;
    }

    private void WriteCSV()
    {
        var list = _enemyList;

        if (list.list.Length > 0)
        {
            TextWriter textWriter = new StreamWriter(_fileName, false);
            textWriter.WriteLine("Name,Point");
            textWriter.Close();

            textWriter = new StreamWriter(_fileName, true);

            foreach (var item in list.list)
            {
                textWriter.WriteLine($"{item.name},{item.point}");
            }

            textWriter.Close();
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        _fileName = Application.dataPath + "/enemies.csv";
        WriteCSV();
    }
}
