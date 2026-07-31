using System.Text.Json.Serialization;

namespace PedidosApiApp.Servicios;

public interface IServicioValidacionCliente
{
    Task<bool> ExisteClienteAsync(int clienteId);
}

public class ServicioValidacionCliente : IServicioValidacionCliente
{
    private readonly HttpClient _httpClient;

    public ServicioValidacionCliente(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<bool> ExisteClienteAsync(int clienteId)
    {
        try
        {
            var response = await _httpClient.GetAsync($"https://jsonplaceholder.typicode.com/users/{clienteId}");
            return response.IsSuccessStatusCode;
        }
        catch
        {
            return false;
        }
    }
}