namespace API.Services.Seguranca
{
    public interface ISegurancaService
    {
        public Task<string> ConverterStringEmHash(string senha);
    }
}
