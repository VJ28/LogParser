using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using LogParser.Entity;

namespace LogParser{
    public class LogParser
    {
        public void Start(string filePath){
            if(!File.Exists(filePath)){
                throw new FileNotFoundException();
            }
            List<LogData> logDatas = ParseAndProcessFile(filePath);
            PrintHighThroughPutApi(logDatas);
            PrintAllLogs(logDatas);
        }
        internal List<LogData> ParseAndProcessFile(string filePath){
            IDictionary<string, LogData> map = new Dictionary<string, LogData>();
            try 
            {
                using (var reader = new StreamReader(filePath))
                {
                    if(!reader.EndOfStream){
                        reader.ReadLine();
                    }
                    while(!reader.EndOfStream){
                        var data = reader.ReadLine().Split(',');
                        ProcessRows(data, map);
                    }
                }
            }
            catch(Exception ex)
            {
                throw ex;
            }
            return map.Values.ToList();
        }

        internal bool ProcessRows(string[] data, IDictionary<string, LogData> map)
        {
            try {
                var url = Utility.ReplaceIdInURL(data[1]);
                var method = data[2];
                var key = string.Format("{0}_{1}", url, method);
                var responseTime = Convert.ToInt32(data[3]);
                string status = data[4];
                LogData logData;
                if(map.ContainsKey(key)){
                    map.TryGetValue(key, out logData);
                    map.Remove(key);
                    logData.Total += responseTime;
                    logData.Frequency++;
                    logData.Max = Math.Max(logData.Max, responseTime);
                    logData.Min = Math.Min(logData.Min, responseTime);
                } else {
                    logData = new LogData{
                        Max = responseTime,
                        Min = responseTime,
                        Total = responseTime,
                        URL = url,
                        Method = method
                    };
                }
                map.Add(key, logData);
            }
            catch(Exception ex){
                throw ex;
            }
            return true;
        }

        internal void PrintAllLogs(List<LogData> logDatas){
            Console.WriteLine(String.Format("| {0,-6}| {1,-25}| {2,-10}| {3,-10}| {4,-12}|", "Method", "URL", "Min Time", "Max Time", "Average Time"));
            logDatas.ForEach(data => {
                Console.WriteLine(String.Format("| {0,-6}| {1,-25}| {2,-10}| {3,-10}| {4,-12}|", data.Method, data.URL, data.Min, data.Max, data.GetAverage()));
            });
            Console.WriteLine();
        }

        internal void PrintHighThroughPutApi(List<LogData> logDatas, int top = 5){
            logDatas.Sort((l1, l2) => l2.Frequency - l1.Frequency);
            Console.WriteLine(String.Format("| {0,-6}| {1,-25}| {2,-10}|", "Method", "URL", "Frequency"));
            logDatas.Take(top).ToList().ForEach(data => {
                Console.WriteLine(String.Format("| {0,-6}| {1,-25}| {2,-10}|", data.Method, data.URL, data.Frequency));
            });
            Console.WriteLine();
        }
    }
}