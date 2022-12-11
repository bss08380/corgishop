namespace CorgiShop.Client.Services.Base;

public enum EndpointType
{
    GetPaginated,
    GetById,
    Count,
    Create,
    Update,
    Delete
}

public record CrudServiceAuthConfiguration
{
    public List<EndpointType> AuthedEndpoints { get; } = new List<EndpointType>();

    public static CrudServiceAuthConfiguration NoAuth() => new CrudServiceAuthConfiguration();

    public static CrudServiceAuthConfiguration FullAuth()
    {
        var config = new CrudServiceAuthConfiguration();
        foreach (var endpointType in typeof(EndpointType).GetEnumValues())
        {
            config.AuthedEndpoints.Add((EndpointType)endpointType);
        }
        return config;
    }

    public static CrudServiceAuthConfiguration FromAuthedEndpoints(params EndpointType[] endpointsToAuth)
    {
        var config = new CrudServiceAuthConfiguration();
        foreach (var endpoint in endpointsToAuth) config.AuthedEndpoints.Add(endpoint);
        return config;
    }

    public static CrudServiceAuthConfiguration ReadOnlyWithoutAuth()
    {
        var config =  new CrudServiceAuthConfiguration();
        config.AuthedEndpoints.Add(EndpointType.GetPaginated);
        config.AuthedEndpoints.Add(EndpointType.GetById);
        config.AuthedEndpoints.Add(EndpointType.Count);
        return config;
    }
}
