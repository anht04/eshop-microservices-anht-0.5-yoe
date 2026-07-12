namespace Catalog.API.Settings;

public class DatabaseSettings
{
    public string ConnectionString { get; set; } = default!;
    public string DatabaseName { get; set; } = default!;
    public string ProductCollectionName { get; set; } = default!;
    public string BrandCollectionName { get; set; } = default!;
    public string TypeCollectionName { get; set; } = default!;
}