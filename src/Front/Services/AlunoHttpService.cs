using API.DTOs.Aluno;
using Front.Models;

namespace Front.Services
{
    public class AlunoHttpService
    {
        private readonly HttpClient _httpClient;

        public AlunoHttpService(IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateClient("API");
        }

        public async Task<RetornoTodosAlunosDto> ListarTodosAlunos(int pagina, int registrosPorPagina)
        {
            var response = await _httpClient.GetAsync($"Alunos/ListarAlunos?pagina={pagina}&registrosPorPagina={registrosPorPagina}");
            
            return response.IsSuccessStatusCode 
                ? await response.Content.ReadFromJsonAsync<RetornoTodosAlunosDto>()
                : null;
        }

        public async Task<RetornoAlunoDto> ListarAlunoPorUsuarioAsync(string usuario)
        {
            var response = await _httpClient.GetAsync($"Alunos/ListarAlunoPorUsuario?usuario={usuario}");
            return response.IsSuccessStatusCode
                ? await response.Content.ReadFromJsonAsync<RetornoAlunoDto>()
                : null;
        }

        public async Task<AlunoRazorModel> CriarAlunoAsync(CriacaoAtualizacaoAlunoDto model)
        {
            var response = await _httpClient.PostAsJsonAsync("Alunos/CriarAluno", model);

            return response.IsSuccessStatusCode 
                ? await response.Content.ReadFromJsonAsync<AlunoRazorModel>()
                : new AlunoRazorModel() { Erro = await response.Content.ReadAsStringAsync() };
        }

        public async Task<AlunoRazorModel> AtualizarAlunoAsync(CriacaoAtualizacaoAlunoDto model, string usuario)
        {
            var response = await _httpClient.PutAsJsonAsync($"Alunos/AtualizarAluno?usuario={usuario}", model);

            return response.IsSuccessStatusCode
                ? await response.Content.ReadFromJsonAsync<AlunoRazorModel>()
                : new AlunoRazorModel() { Erro = await response.Content.ReadAsStringAsync() };
        }

        public async Task<AlunoRazorModel> RemoverAlunoAsync(string usuario)
        {
            var response = await _httpClient.DeleteAsync($"Alunos/RemoverAluno?usuario={usuario}");

            return response.IsSuccessStatusCode 
                ? new AlunoRazorModel()
                : new AlunoRazorModel() { Erro = await response.Content.ReadAsStringAsync() };
        }
    }
}
