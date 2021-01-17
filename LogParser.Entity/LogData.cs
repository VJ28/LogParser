using System;

namespace LogParser.Entity
{
    public class LogData
    {
        public string URL { get; set; }
        public string Method { get; set; }
        public int Frequency { get; set; } = 1;
        public int Max { get; set; }
        public int Min { get; set; }
        public int Total { get; set; }
        public double GetAverage(){
            return Math.Round(this.Total/(this.Frequency*1.0), 2);
        }
    }
}
