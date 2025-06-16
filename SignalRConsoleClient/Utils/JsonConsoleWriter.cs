using System.Text.Json;

namespace SignalRConsoleClient.Utils
{
    public static class JsonConsoleWriter
    {
        public static void Write<T>(T obj)
        {
            var json = JsonSerializer.Serialize(obj, new JsonSerializerOptions { WriteIndented = true });
            Console.WriteLine(json);
        }
    }
}
