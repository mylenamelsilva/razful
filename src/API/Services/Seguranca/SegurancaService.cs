namespace API.Services.Seguranca
{
    public class SegurancaService : ISegurancaService
    {
        public async Task<string> ConverterStringEmHash(string senha)
        {
            string senhaHasheada = BCrypt.Net.BCrypt.HashPassword(senha);

            return senhaHasheada.Length > 60 ? senhaHasheada.Substring(0, 60) : senhaHasheada;
        }
    }
}
