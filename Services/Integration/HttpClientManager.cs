using System.Net.Http;

namespace Services.Integration
{
    public static class HttpClientManager
    {
        private static HttpClient _httpClient;

        /// <summary>
        /// Obterm uma instância do HttpCliente
        /// <br>NÃO utilizar com o <c>using</c> ou executar o <c>dispose</c> do recurso.</br>
        /// </summary>
        public static HttpClient GetInstance => _httpClient ??= new HttpClient();

        /// <summary>
        /// Libera o recurso da instância do HttpClient.
        /// <br>SOMENTE utilizar esse método no fim do ciclo de vida da aplicação.</br> 
        /// </summary>
        public static void DisposeInstance()
        {
            if (_httpClient == null) { return; }
            _httpClient.Dispose();
        }
    }
}
