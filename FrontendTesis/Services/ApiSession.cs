using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;
using Contrato;

namespace FrontendTesis.Services;

public sealed class ApiSession(HttpClient httpClient)
{
    private string? _token;

    public bool IsAuthenticated => !string.IsNullOrWhiteSpace(_token);

    public async Task<string?> LoginAsync(LoginDto credentials)
    {
        using var response = await httpClient.PostAsJsonAsync("api/Auth/login", credentials);
        if (!response.IsSuccessStatusCode)
        {
            return response.StatusCode == System.Net.HttpStatusCode.Unauthorized
                ? "Usuario o contraseña incorrectos."
                : await ReadErrorAsync(response);
        }

        var result = await response.Content.ReadFromJsonAsync<LoginRespuestaDTO>();
        if (result is null || string.IsNullOrWhiteSpace(result.Token))
        {
            return "La API no devolvió un token de autenticación válido.";
        }

        _token = result.Token;
        httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _token);
        return null;
    }

    public async Task<string?> RegisterAsync(LoginDto credentials)
    {
        using var response = await httpClient.PostAsJsonAsync("api/Auth/registrar", credentials);
        return response.IsSuccessStatusCode ? null : await ReadErrorAsync(response);
    }

    public void Logout()
    {
        _token = null;
        httpClient.DefaultRequestHeaders.Authorization = null;
    }

    private static async Task<string> ReadErrorAsync(HttpResponseMessage response)
    {
        var body = await response.Content.ReadAsStringAsync();
        if (!string.IsNullOrWhiteSpace(body))
        {
            try
            {
                using var document = JsonDocument.Parse(body);
                if (document.RootElement.TryGetProperty("mensaje", out var message))
                {
                    return message.GetString() ?? body;
                }
            }
            catch (JsonException)
            {
            }
        }

        return $"Error de API ({(int)response.StatusCode}): {body}";
    }
}
