using API.DTOs.Aluno;
using API.DTOs.AlunoTurma;
using API.DTOs.Turma;
using Front.Models.AlunoTurma;

namespace Front.Services
{
    public class AlunoTurmaViewService
    {
        private readonly HttpClient _httpClient;

        public AlunoTurmaViewService(IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateClient("API");
        }

        public async Task<RetornoTodasAssociacoesDto> ListarTodasAssociacoesAsync(int pagina, int registrosPorPagina)
        {
            var response = await _httpClient.GetAsync($"AlunoTurmas/ListarAssociacoes?pagina={pagina}&registrosPorPagina={registrosPorPagina}");

            return response.IsSuccessStatusCode
                ? await response.Content.ReadFromJsonAsync<RetornoTodasAssociacoesDto>()
                : null;
        }

        public async Task<AlunoTurmaViewModel> CriarAssociacaoAsync(CriacaoAtualizacaoAlunoTurmaDto model)
        {
            var response = await _httpClient.PostAsJsonAsync("AlunoTurmas/CriarAssociacao", model);

            return response.IsSuccessStatusCode
                ? await response.Content.ReadFromJsonAsync<AlunoTurmaViewModel>()
                : new AlunoTurmaViewModel() { Erro = await response.Content.ReadAsStringAsync() };
        }

        public async Task<AlunoTurmaViewModel> AtualizarAssociacaoAsync(CriacaoAtualizacaoAlunoTurmaDto model, string turma)
        {
            var response = await _httpClient.PutAsJsonAsync($"AlunoTurmas/AtualizarAssociacao?turma={turma}", model);

            return response.IsSuccessStatusCode
                ? await response.Content.ReadFromJsonAsync<AlunoTurmaViewModel>()
                : new AlunoTurmaViewModel() { Erro = await response.Content.ReadAsStringAsync() };
        }


        public async Task<RetornoTodosAlunosDto> ListarAlunosPorTurmaAsync(string turma, int pagina = 1, int registrosPorPagina = 10)
        {
            var response = await _httpClient.GetAsync($"AlunoTurmas/ListarAlunosPorTurma?turma={turma}&pagina={pagina}&registrosPorPagina={registrosPorPagina}");

            return response.IsSuccessStatusCode
                ? await response.Content.ReadFromJsonAsync<RetornoTodosAlunosDto>()
                : null;
        }

        public async Task<AlunoTurmaViewModel> RemoverAssociacaoAsync(string aluno, string turma)
        {
            var response = await _httpClient.DeleteAsync($"AlunoTurmas/RemoverAssociacao?aluno={aluno}&turma={turma}");

            return response.IsSuccessStatusCode
                ? new AlunoTurmaViewModel()
                : new AlunoTurmaViewModel() { Erro = await response.Content.ReadAsStringAsync() };
        }

        public async Task<AlunoTurmaViewModel> RemoverTodaAssociacaoAsync(string turma)
        {
            var response = await _httpClient.DeleteAsync($"AlunoTurmas/RemoverTodaAssociacao?turma={turma}");

            return response.IsSuccessStatusCode
                ? new AlunoTurmaViewModel()
                : new AlunoTurmaViewModel() { Erro = await response.Content.ReadAsStringAsync() };
        }
    }
}
