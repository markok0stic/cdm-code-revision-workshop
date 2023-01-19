var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

Console.WriteLine($"{DateTime.Now.ToLongTimeString()}: Start");
foreach (var item in await FetchItemsIEnumerableAsync())
{
    Console.WriteLine($"{DateTime.Now.ToLongTimeString()}: {item}");
}
Console.WriteLine($"{DateTime.Now.ToLongTimeString()}: End");

Console.WriteLine($"{DateTime.Now.ToLongTimeString()}: ===========");

Console.WriteLine($"{DateTime.Now.ToLongTimeString()}: Start");
await foreach (var item in FetchItemsIAsyncEnumerableAsync())
{
    Console.WriteLine($"{DateTime.Now.ToLongTimeString()}: {item}");
}
Console.WriteLine($"{DateTime.Now.ToLongTimeString()}: End");

app.MapGet("/iasyncenum", GetItemsIAsyncEnum);
app.MapGet("/ienum", GetItemsIEnum);
app.Run();



// async method with return - blocks main thread
static async Task<IEnumerable<int>> FetchItemsIEnumerableAsync()
{
    Console.WriteLine($"{DateTime.Now.ToLongTimeString()}: Main thread: {!Thread.CurrentThread.IsBackground}");
    var items = new List<int>();
    for (int i = 1; i <= 10; i++)
    {
        await Task.Delay(1000);
        items.Add(i);
    }
    return items;
}

// async method with yield return - usefully to stream data when needed and do not block main thread
static async IAsyncEnumerable<int> FetchItemsIAsyncEnumerableAsync()
{
    Console.WriteLine($"{DateTime.Now.ToLongTimeString()}: Main thread: {!Thread.CurrentThread.IsBackground}");
    for (int i = 1; i <= 10; i++)
    {
        await Task.Delay(1000);
        yield return i;
    }
}

static async IAsyncEnumerable<int> GetItemsIAsyncEnum()
{
    for (int i = 1; i <= 10; i++)
    {
        await Task.Delay(1000);
        yield return i;
    }
}
static async Task<IEnumerable<int>> GetItemsIEnum()
{
    var items = new List<int>();
    for (int i = 1; i <= 10; i++)
    {
        await Task.Delay(1000);
        items.Add(i);
    }
    return items;
}