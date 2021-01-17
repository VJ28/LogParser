using System;
using Xunit;
using Moq;
using System.IO;
using System.Collections.Generic;
using LogParser.Entity;

namespace LogParser.Tests
{
    public class LogParser_Test
    {
        [Fact]
        public void Should_Throw_Exception_For_Invalid_FilePath()
        {
            var obj = new LogParser();
            Assert.Throws<FileNotFoundException>(() => obj.Start("file.csv"));
        }

        [Fact]
        public void Should_Process_Valid_Data()
        {
            IDictionary<string, LogData> map = new Dictionary<string, LogData>();
            string[] mockedInput = {"1581589721" ,"/person/1/details" ,"GET", "35", "200"};
            var obj = new LogParser();
            obj.ProcessRows(mockedInput, map);
            Assert.NotEmpty(map);
            Assert.True(map.ContainsKey("/person/{id}/details_GET"));
        }

        [Fact]
        public void Should_Process_And_Output_Correct_Min_Max_Time()
        {
            IDictionary<string, LogData> map = new Dictionary<string, LogData>();
            string[][] mockedInput = new string[][]{
                new string[]{"1581589721" ,"/person/1/details" ,"GET", "35", "200"},
                new string[]{"1581589721" ,"/person/1/details" ,"GET", "78", "200"},
                new string[]{"1581589721" ,"/person/1/details" ,"GET", "96", "200"}
            };
            var obj = new LogParser();
            int i = 3;
            while(--i >= 0){
                obj.ProcessRows(mockedInput[i], map);
            }
            Assert.NotEmpty(map);
            string expectedKey = "/person/{id}/details_GET";
            Assert.True(map.ContainsKey(expectedKey));
            LogData output;
            map.TryGetValue(expectedKey, out output);
            Assert.Equal(35, output.Min);
            Assert.Equal(96, output.Max);
            Assert.Equal(3, output.Frequency);
            Assert.Equal(69.67, Math.Round(output.Total/(1.0*output.Frequency), 2));
        }

        [Fact]
        public void Should_Throw_Exception_For_Invalid_Data()
        {
            string[] mockedInput = {"1581589721" ,"" ,"GET", "35", "200"};
            var obj = new LogParser();
            Assert.Throws<ArgumentException>(() => obj.ProcessRows(mockedInput, It.IsAny<IDictionary<string, LogData>>()));
        }
    }
}
