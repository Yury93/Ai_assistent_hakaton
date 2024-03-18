using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using UnityEngine;

namespace Core.Configs
{
    [Serializable]
    public class CSVReader
    {
        [field:SerializeField] private List<TextAsset> csvFiles;
        public bool enableLog;
        public void SetupCSVFiles(List<TextAsset> csvFiles)
        {
            this.csvFiles = csvFiles;
        }
        public List<T> ReadCSV<T>( string targetName, bool log = false)
        {
            if (log)
            {
                foreach (var item in csvFiles)
                {
                     Log(item.name + " ==? " + targetName);
                }
            }
            var textAsset = csvFiles.First(s => s.name == targetName);
            string[] rowsOfTable = textAsset.text.Split(new string[] { "\n" }, StringSplitOptions.None);

            int row = 0;
            int column = 0;
            GetSizeTable(rowsOfTable, ref row, ref column);

            List<T> resultList = new List<T>();
            for (int i = 1; i < row; i++)
            {
                var data = rowsOfTable[i].Split(';');
                T obj = Activator.CreateInstance<T>();

                FillProperties(column, data, obj);
                resultList.Add(obj);

            }
            return resultList;
        }
        public List<T> ReadCSVOnline<T>( TextAsset textAsset, bool log = false)
        {  
            string[] rowsOfTable = textAsset.text.Split(new string[] { "\n" }, StringSplitOptions.None);

            int row = 0;
            int column = 0;
            GetSizeTable(rowsOfTable, ref row, ref column);

            List<T> resultList = new List<T>();
            for (int i = 1; i < row; i++)
            {
                var data = rowsOfTable[i].Split(';',',');
                T obj = Activator.CreateInstance<T>();

                FillProperties(column, data, obj);
                resultList.Add(obj);

            }
            return resultList;
        }

        private void FillProperties<T>(int column, string[] data, T obj)
        {
            PropertyInfo[] properties = typeof(T).GetProperties();
            for (int j = 0; j < column; j++)
            {
                var value = data[j];
                value = DeleteTransferSymbol(value);
                if (string.IsNullOrEmpty(value)) continue;

                try
                {
                    if (properties[j].PropertyType == typeof(int))
                    {
                        properties[j].SetValue(obj, Convert.ToInt16(value));
                    }
                    if (properties[j].PropertyType == typeof(string))
                    {
                        properties[j].SetValue(obj, value);
                        
                         Log(value);
                    }
                }
                catch (Exception e)
                {
                    Debug.LogError("value = " + value + " exseption => " + e.ToString());
                }
            }
        }

        private string DeleteTransferSymbol(string value)
        {
            if (value.Contains("\r"))
            {
                StringBuilder stringBuilder = new StringBuilder();
                foreach (var ch in value)
                {
                    if (ch != '\r')
                    {
                        stringBuilder.Append(ch);
                    }
                }
                value = stringBuilder.ToString();
            }

            return value;
        }

        private void GetSizeTable(string[] date, ref int row, ref int columnCount)
        {
            foreach (var item in date)
            {
                if (item.Contains("\r"))
                {
                    row++;
                    if (columnCount == 0)
                    {
                        var column = item.Split(';', ',');
                        columnCount = column.Length;
                    }
                }
            }
            row++;
        }

        private void Log(string message)
        {
            if(enableLog)
            Debug.Log(message);
        }
    }
}
 
