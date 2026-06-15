using EF_Client_App_Entity;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace EF_Client_App_DAL;

public partial class MetaDataClient
{
    private readonly HttpClient _httpClient;
    private string _baseUrl;
    private static readonly Lazy<JsonSerializerOptions> _settings = new(CreateSerializerSettings);
    private JsonSerializerOptions _instanceSettings;

    public MetaDataClient(HttpClient httpClient)
    {
        _httpClient = httpClient;
        BaseUrl = "https://localhost:50939";
        Initialize();
    }

    // =========================================================
    // CONFIG BASE URL
    // =========================================================

    public string BaseUrl
    {
        get => _baseUrl;
        set
        {
            _baseUrl = value;
            if (!string.IsNullOrEmpty(_baseUrl) && !_baseUrl.EndsWith("/"))
                _baseUrl += "/";
        }
    }

    protected JsonSerializerOptions JsonSerializerSettings => _instanceSettings ?? _settings.Value;

    // =========================================================
    // JSON SETTINGS
    // =========================================================

    private static JsonSerializerOptions CreateSerializerSettings()
    {
        var settings = new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            PropertyNameCaseInsensitive = true
        };
        UpdateJsonSerializerSettings(settings);
        return settings;
    }

    static partial void UpdateJsonSerializerSettings(JsonSerializerOptions settings);
    partial void Initialize();
    partial void PrepareRequest(HttpClient client, HttpRequestMessage request, string url);
    partial void PrepareRequest(HttpClient client, HttpRequestMessage request, StringBuilder urlBuilder);
    partial void ProcessResponse(HttpClient client, HttpResponseMessage response);

    // =========================================================
    // MÉTHODES GÉNÉRIQUES PLUMBING (GET, PUT, POST)
    // =========================================================

    private async Task<T> GetAsync<T>(string url, CancellationToken ct)
    {
        var request = new HttpRequestMessage(HttpMethod.Get, url);
        request.Headers.Add("Accept", "application/json");

        await PrepareAndSendRequest(request, url);

        var response = await _httpClient.SendAsync(request, ct);
        ProcessResponse(_httpClient, response);
        response.EnsureSuccessStatusCode();

        var json = await response.Content.ReadAsStringAsync(ct);
        return JsonSerializer.Deserialize<T>(json, JsonSerializerSettings)
               ?? throw new InvalidOperationException("Réponse JSON invalide");
    }

    private async Task PutAsync<TRequest>(string url, TRequest data, CancellationToken ct)
    {
        var request = new HttpRequestMessage(HttpMethod.Put, url);
        var json = JsonSerializer.Serialize(data, JsonSerializerSettings);
        request.Content = new StringContent(json, Encoding.UTF8, "application/json");

        await PrepareAndSendRequest(request, url);

        var response = await _httpClient.SendAsync(request, ct);
        ProcessResponse(_httpClient, response);
        response.EnsureSuccessStatusCode();
    }

    private async Task<TResponse> PostAsync<TRequest, TResponse>(string url, TRequest data, CancellationToken ct)
    {
        var request = new HttpRequestMessage(HttpMethod.Post, url);
        request.Headers.Add("Accept", "application/json");

        var json = JsonSerializer.Serialize(data, JsonSerializerSettings);
        request.Content = new StringContent(json, Encoding.UTF8, "application/json");

        await PrepareAndSendRequest(request, url);

        var response = await _httpClient.SendAsync(request, ct);
        ProcessResponse(_httpClient, response);
        response.EnsureSuccessStatusCode();

        var responseJson = await response.Content.ReadAsStringAsync(ct);
        return JsonSerializer.Deserialize<TResponse>(responseJson, JsonSerializerSettings)
               ?? throw new InvalidOperationException("Réponse JSON invalide");
    }

    private async Task PrepareAndSendRequest(HttpRequestMessage request, string url)
    {
        var urlBuilder = new StringBuilder(url);
        PrepareRequest(_httpClient, request, urlBuilder);

        var finalUrl = urlBuilder.ToString();
        request.RequestUri = new Uri(finalUrl, UriKind.RelativeOrAbsolute);
        PrepareRequest(_httpClient, request, finalUrl);

        await Task.CompletedTask;
    }

    // =========================================================
    // ARRAY
    // =========================================================

    public Task<ICollection<ArrayDTO>> GetArraysAsync(int? selection, CancellationToken ct = default)
    {
        var url = $"{BaseUrl}api/metadata/arrays";
        if (selection.HasValue) url += $"?selection={selection}";

        return GetAsync<ICollection<ArrayDTO>>(url, ct);
    }

    // =========================================================
    // DESCRIPTION
    // =========================================================

    public Task<ICollection<DescriptionDTO>> GetDescriptionsAsync(int? selection, CancellationToken ct = default)
    {
        var url = $"{BaseUrl}api/metadata/descriptions";
        if (selection.HasValue) url += $"?selection={selection}";

        return GetAsync<ICollection<DescriptionDTO>>(url, ct);
    }

    // =========================================================
    // NOTE
    // =========================================================

    public Task<ICollection<NoteDTO>> GetNotesAsync(int? selection, CancellationToken ct = default)
    {
        var url = $"{BaseUrl}api/metadata/notes";
        if (selection.HasValue) url += $"?selection={selection}";

        return GetAsync<ICollection<NoteDTO>>(url, ct);
    }

    // =========================================================
    // HISTORIC
    // =========================================================

    // GET : Récupération via QueryString (userId & prnSelection) -> retourne un HistoricDTO
    public Task<HistoricDTO> GetHistoricAsync(string userId, string prnSelection, CancellationToken ct = default)
    {
        var urlBuilder = new StringBuilder($"{BaseUrl}api/metadata/historic?");

        if (userId != null)
        {
            urlBuilder.Append("userId=").Append(Uri.EscapeDataString(userId)).Append('&');
        }
        if (prnSelection != null)
        {
            urlBuilder.Append("prnSelection=").Append(Uri.EscapeDataString(prnSelection)).Append('&');
        }

        urlBuilder.Length--; // Supprime le dernier '&' ou '?' si aucun paramètre n'est fourni

        return GetAsync<HistoricDTO>(urlBuilder.ToString(), ct);
    }

    // POST : Enregistrement (Création) -> Envoie l'objet dans le Body et retourne l'objet créé (201)
    public Task<HistoricDTO> SaveHistoricAsync(HistoricDTO historic, CancellationToken ct = default)
    {
        if (historic == null)
            throw new ArgumentNullException(nameof(historic));

        var url = $"{BaseUrl}api/metadata/historic";
        return PostAsync<HistoricDTO, HistoricDTO>(url, historic, ct);
    }

    // PUT : Mise à jour -> Envoie l'objet dans le Body et ne retourne rien (204)
    public Task UpdateHistoricAsync(HistoricDTO historic, CancellationToken ct = default)
    {
        if (historic == null)
            throw new ArgumentNullException(nameof(historic));

        var url = $"{BaseUrl}api/metadata/historic";
        return PutAsync(url, historic, ct);
    }
}