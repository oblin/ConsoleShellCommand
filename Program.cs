using System;
using System.Text.Json;
using System.Threading;

namespace ConsoleShellCommand
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");

            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };

            //var result = "ls -l".Bash(true);
            //Console.WriteLine(result);
            //Console.ReadLine();

            int count = 1;
            BloodValue blood = null;
            do
            {
                var result = "sudo node /home/pi/sandbox/fora/fora".Bash(true);
                Console.WriteLine(result);

                var jsonResult = JsonSerializer.Deserialize<BloodResult>(result, options);
                Console.WriteLine($"Failed: {jsonResult.Failed}");

                if (!jsonResult.Failed)
                {
                    blood = jsonResult.Result;
                }
                else
                {
                    Console.WriteLine($"Current Times: {count++}, Retry...");
                    Thread.Sleep(10 * 1000);
                }
            } 
            while (blood == null && count < 6);

            if (blood != null)
                Console.WriteLine($"Sys: {blood.Systolic}, Pulse: {blood.Pulse}");
            else
                Console.WriteLine($"Retry {--count} but still failed");
        }
    }
}
