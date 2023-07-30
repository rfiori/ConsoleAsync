using System;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

Object obj = null;

Console.Beep();
obj = new();
Start();

void Start()
{
    EscreveLinha("Iniciando as Threads (thd1 e thd2)...");

    var thd1 = BaixaSiteContent();

    var thd2 = Task.Run(() =>
    {
        for (int i = 1; i <= 1000; i++)
        {
            var L = Console.CursorLeft;
            var T = Console.CursorTop;

            lock (obj)
            {
                Console.SetCursorPosition(0, 2);
                Console.Write($"Aguardando {i}");
                Console.SetCursorPosition(L, T);
            }
            Thread.Sleep(1);

            if (thd1.IsCompleted)
                break;

            if (i == 1000)
                EscreveLinha("\r\nThread \"thd2\" - Esperei 10 segundos e a \"thd1\" não finalizou. Fui... !!!\r\n");
        }
    });

    Task.WaitAll(new Task[] { thd2, thd1 }, 10000);
}


async Task BaixaSiteContent()
{
    var client = new HttpClient();

    EscreveLinha("Work.... \r\n\r\n");

    var tiker = new Stopwatch();
    string site = "";

    tiker.Start();
    site = "http://www.horizonteborrachas.com.br";
    string urlContents = await client.GetStringAsync(site);
    EscreveLinha($"site1:{site} - tempo:{tiker.ElapsedMilliseconds} ms - tamanho: {urlContents.Length.ToString("##0,000", CultureInfo.GetCultureInfo("pt-BR"))}");
    await CriaFileAsync("File1.html", Encoding.UTF8.GetBytes(urlContents));
    tiker.Stop();
    EscreveLinha($"Tempo decorrido site1: {tiker.ElapsedMilliseconds} ms\r\n");

    tiker.Restart();
    site = "http://www.youtube.com";
    urlContents = await client.GetStringAsync(site);
    EscreveLinha($"site2:{site} - tempo:{tiker.ElapsedMilliseconds} ms - tamanho: {urlContents.Length.ToString("##0,000", CultureInfo.GetCultureInfo("pt-BR"))}");
    await CriaFileAsync("File2.html", Encoding.UTF8.GetBytes(urlContents));
    tiker.Stop();
    EscreveLinha($"Tempo decorrido site2: {tiker.ElapsedMilliseconds} ms\r\n");

    tiker.Restart();
    site = "http://g1.com.br";
    urlContents = await client.GetStringAsync(site);
    EscreveLinha($"site3:{site} - tempo:{tiker.ElapsedMilliseconds} ms - tamanho: {urlContents.Length.ToString("##0,000", CultureInfo.GetCultureInfo("pt-BR"))}");
    await CriaFileAsync("File3.html", Encoding.UTF8.GetBytes(urlContents));
    tiker.Stop();
    EscreveLinha($"Tempo decorrido site3: {tiker.ElapsedMilliseconds} ms\r\n");

    tiker.Restart();
    site = "http://www.horizonteborrachas.com.br";
    urlContents = await client.GetStringAsync(site);
    EscreveLinha($"site4:{site} - tempo:{tiker.ElapsedMilliseconds} ms - tamanho: {urlContents.Length.ToString("##0,000", CultureInfo.GetCultureInfo("pt-BR"))}");
    await CriaFileAsync("File4.html", Encoding.UTF8.GetBytes(urlContents));
    tiker.Stop();
    EscreveLinha($"Tempo decorrido site4: {tiker.ElapsedMilliseconds} ms\r\n");

    tiker.Restart();
    site = "https://www.socarrao.com.br/";
    urlContents = await client.GetStringAsync(site);
    EscreveLinha($"site5:{site} - tempo:{tiker.ElapsedMilliseconds} ms - tamanho: {urlContents.Length.ToString("##0,000", CultureInfo.GetCultureInfo("pt-BR"))}");
    await CriaFileAsync("File5.html", Encoding.UTF8.GetBytes(urlContents));
    tiker.Stop();
    EscreveLinha($"Tempo decorrido site5: {tiker.ElapsedMilliseconds} ms\r\n");

    EscreveLinha($"\r\nTamanho do conteúdo baixado: {urlContents.Length.ToString("##0,000", CultureInfo.GetCultureInfo("pt-BR"))} bytes \r\n");
}

void EscreveLinha(string msg)
{
    lock (obj)
    {
        Console.WriteLine(msg);
    }
}

async Task CriaFileAsync(string name, byte[] contantent)
{
    var f = File.Create(name);
    using (f)
    {
        EscreveLinha($"Gravando Arquivo: {name} - tamanho: {contantent.Length.ToString("##0,000", CultureInfo.GetCultureInfo("pt-BR"))}");
        await f.WriteAsync(contantent);
        f.Flush();
        f.Close();
    }
}
