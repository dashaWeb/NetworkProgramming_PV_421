using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

public class Post
{
    public string Title { get; set; }
    public string Body { get; set; }
    public int UserId { get; set; }     
}
internal class Program
{
    private static async Task Main(string[] args)
    {
        //Get
        /*var url = "https://jsonplaceholder.typicode.com/users";*/
        /*using var client = new HttpClient();

        var response = await client.GetAsync(url);
        Console.WriteLine($"Status : {response.StatusCode}");
        var result = await response.Content.ReadAsStringAsync();
        Console.WriteLine(result);*/
        // Post
        var post = new Post()
        {
            Title = "TestTitle",
            Body = "TestBody",
            UserId = 1
        };
        var url = "https://jsonplaceholder.typicode.com/posts";
        var json = JsonSerializer.Serialize(post);
        var data = new StringContent(json, Encoding.UTF8, "application/json");
        using var client = new HttpClient();
        var response = await client.PostAsync(url, data);
        Console.WriteLine($"Status : {response.StatusCode}");
        var result = await response.Content.ReadAsStringAsync();
        Console.WriteLine(result);
    }
}