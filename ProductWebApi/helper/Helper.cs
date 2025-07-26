using ProductWebApi.global;

namespace ProductWebApi.helper
{
    public class Helper
    {
        public HttpClient Intial()
        {
            var client = new HttpClient();
            client.BaseAddress = new Uri(Global.BaseAddress);
            return client;
        }
    }
}
