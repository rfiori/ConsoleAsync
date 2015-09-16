using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace FirstAsyncExemplo
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.BackgroundColor = ConsoleColor.Blue;
            Console.Beep();

            Start();
        }

        static private void Start()
        {
            Console.WriteLine("Iniciando as Threads (thd1 e thd2)...");

            var thd1 = AccessTheWebAsync();

            var thd2 = Task.Run(() =>
            {
                for (int i = 1; i <= 1000; i++)
                {
                    Console.SetCursorPosition(0, 2);
                    Console.Write($"Aguardando {i}");
                    System.Threading.Thread.Sleep(10);

                    if (thd1.IsCompleted)
                        break;

                    if (i == 1000)
                        Console.Write($"\r\nThread \"thd2\" - Esperei 10 segundos e a \"thd1\" não finalizou. Fui... !!!\r\n");
                }
            });

            Task.WaitAll(thd2, thd1);

            Console.ReadLine();
        }


        static async Task AccessTheWebAsync()
        {
            HttpClient client = new HttpClient();

            Task<string> getStringTask = client.GetStringAsync("http://msdn.microsoft.com");

            DoIndependentWork();

            string urlContents = await getStringTask;

            Console.WriteLine($"\r\nLength of the downloaded string: {urlContents.Length}");
        }


        static void DoIndependentWork()
        {
            Console.WriteLine("Working . . . . . . .");
        }
    }
}
