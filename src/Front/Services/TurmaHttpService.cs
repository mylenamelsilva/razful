using API.DTOs.Turma;
using Front.Models.Turma;

namespace Front.Services
{
    public class TurmaHttpService
    {
        private readonly HttpClient _httpClient;

        public TurmaHttpService(IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateClient("API");
        }

        public async Task<RetornoTodasTurmasDto> ListarTodasTurmas(int pagina, int registrosPorPagina)
        {
            var response = await _httpClient.GetAsync($"Turmas/ListarTurmas?pagina={pagina}&registrosPorPagina={registrosPorPagina}");

            return response.IsSuccessStatusCode
                ? await response.Content.ReadFromJsonAsync<RetornoTodasTurmasDto>()
                : null;
        }

        public async Task<RetornoTurmaDto> ListarTurmaPorIdAsync(int idTurma)
        {
            var response = await _httpClient.GetAsync($"Turmas/ListarTurmaPorId?idTurma={idTurma}");
            return response.IsSuccessStatusCode
                ? await response.Content.ReadFromJsonAsync<RetornoTurmaDto>()
                : null;
        }

        public async Task<TurmaRazorModel> CriarTurmaAsync(CriacaoAtualizacaoTurmaDto model)
        {
            var response = await _httpClient.PostAsJsonAsync("Turmas/CriarTurma", model);

            return response.IsSuccessStatusCode
                ? await response.Content.ReadFromJsonAsync<TurmaRazorModel>()
                : new TurmaRazorModel() { Erro = await response.Content.ReadAsStringAsync() };
        }

        public async Task<TurmaRazorModel> AtualizarTurmaAsync(CriacaoAtualizacaoTurmaDto model, int idTurma)
        {
            var response = await _httpClient.PutAsJsonAsync($"Turmas/AtualizarTurma?idTurma={idTurma}", model);

            return response.IsSuccessStatusCode
                ? await response.Content.ReadFromJsonAsync<TurmaRazorModel>()
                : new TurmaRazorModel() { Erro = await response.Content.ReadAsStringAsync() };
        }

        public async Task<TurmaRazorModel> RemoverTurmaAsync(int idTurma)
        {
            var response = await _httpClient.DeleteAsync($"Turmas/RemoverTurma?idTurma={idTurma}");

            return response.IsSuccessStatusCode
                ? new TurmaRazorModel()
                : new TurmaRazorModel() { Erro = await response.Content.ReadAsStringAsync() };
        }
    }
}
