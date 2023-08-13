
namespace Infrastructure.Options
{
    public class Connection
    {
        public string ConnectionString { get; set; } = null!;
        public string? Version { get; set; }
    }
}
