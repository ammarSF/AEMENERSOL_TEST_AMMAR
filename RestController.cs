using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Reflection.Metadata;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace AEMENERSOL
{
    public class RestController
    {
        private HttpClient client = new HttpClient();

        public void setClient()
        {
            client.BaseAddress = new Uri("http://test-demo.aemenersol.com");
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json-patch+json"));
            client.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("text/plain"));
        }

        public string GetToken()
        {
            Task<string> resp = Task.Run<string>(async () => await GetTokenAsync());
            string remove_first = resp.Result.Remove(0,1);
            string remove_last = remove_first.Remove(remove_first.Length - 1,1);
            string token = remove_last;
            return token;
        }

        private async Task<string> GetTokenAsync()
        {
            try
            {
                HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, "/api/Account/Login");
                request.Content = new StringContent(
                    "{\"username\":\"user@aemenersol.com\",\"password\":\"Test@123\"}",
                    Encoding.UTF8,
                    "application/json");

                // Send login request
                HttpResponseMessage loginResponse = await client.SendAsync(request);
                loginResponse.EnsureSuccessStatusCode();

                // Read token from login response
                string token = await loginResponse.Content.ReadAsStringAsync();

                return token;
            } catch(Exception e) 
            {
                return e.Message;
            }
            
        }

        public string GetPlatformWell(string auth_token)
        {
            Console.WriteLine("Getting list of Platform and Well..");
            Task<string> result = Task.Run<string>(async () => await GetPlatformWellAsync(auth_token));
            return result.Result;
        }

        private async Task<string> GetPlatformWellAsync(string auth_token)
        {
            try
            {
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", auth_token);
                HttpResponseMessage response = await client.GetAsync("/api/PlatformWell/GetPlatformWellActual");
                string responseContent = await response.Content.ReadAsStringAsync();
                return responseContent;
            }
            catch (Exception e)
            {
                return e.Message;
            }
        }

        public string GetPlatformWellDummy(string auth_token) 
        {
            Console.WriteLine("Retrieving Dummy data...");
            Task<string> result = Task.Run<string>(async () => await GetPlatformWellDummyAsync(auth_token));
            return result.Result;
        }

        private async Task<string> GetPlatformWellDummyAsync(string auth_token)
        {
            try
            {
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", auth_token);
                HttpResponseMessage response = await client.GetAsync("/api/PlatformWell/GetPlatformWellDummy");
                string responseContent = await response.Content.ReadAsStringAsync();
                return responseContent;
            }
            catch (Exception e)
            {
                return e.Message;
            }
        }

    }
}
