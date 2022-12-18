using System.Collections.Concurrent;
namespace number1;
class Program
{
    
    
    static void Main()
    {
        //StreamReader datar = new StreamReader("../../../NewFile1.txt");
        StreamReader datar = new StreamReader("../../../NewFile2.txt");
        List<string> data = new List<string>();
        while (!datar.EndOfStream)
        {
            string? line1 = datar.ReadLine();
            data.Add(line1);
        }
        var ans = new ConcurrentDictionary<string, decimal>();
        Parallel.ForEach(data, Print);
         
        void Print(string data)
        { 
            string sURL = $"https://query1.finance.yahoo.com/v7/finance/download/{data}?period1=1629072000&period2=1660608000&interval=1d&events=history&includeAdjustedClose=true";
            Task<HttpResponseMessage> request = new HttpClient().SendAsync(new HttpRequestMessage(HttpMethod.Get, sURL));
            Task<Stream> stream1 = request.Result.Content.ReadAsStreamAsync();
            StreamReader sr1 = new StreamReader(stream1.Result);
            string data1 = sr1.ReadToEnd();
            //Console.WriteLine($"Выполняется задача {Task.CurrentId}");
            Console.WriteLine(data1);
            Task three = Task.Run(() =>
            {
                List<string> days = new List<string>(data1.Split('\n'));
                days.RemoveAt(0);
                decimal sum = 0;
                foreach (var VARIABLE in days)
                {
                         string[] main = VARIABLE.Split(',');
                         sum += (decimal.Parse(main[2]) + decimal.Parse(main[3])) / 2;
                }
                ans.TryAdd(data, sum/days.Count);
            });
        }
        SortedDictionary<string, decimal> sortedans = new SortedDictionary<string, decimal>(ans);
        Console.WriteLine();
        string path = "../../../note1.txt";
        foreach (var VARIABLE in sortedans)
        {
            using (StreamWriter writer = new StreamWriter(path, true))
            {
                writer.WriteLineAsync($"{VARIABLE.Key}: {VARIABLE.Value:f2}");
            } 
        }
    }
}

