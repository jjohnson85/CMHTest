using System.Collections.Generic;
using System.Net.Http;
using System;
using System.Threading.Tasks;
using System.IO;
using System.Text.Json;
using CMHTest.InterviewCheck.Model;

namespace CMHTest.InterviewCheck.DataAccess
{
    /// <summary>
    /// Delegate for the retrieval of interview candidates and their respective interview dates.
    /// </summary>
    public class GetCandidatesDelegate
    {
        private IHttpClientFactory m_clientFactory;

        public GetCandidatesDelegate(IHttpClientFactory clientFactory) 
        {
            m_clientFactory = clientFactory;
        }

        /// <summary>
        /// Method to retrieve the interview candidate information via a GET request to a restful service 
        /// hosting the data.
        /// </summary>
        /// <returns>
        /// List<Candidate> - List of interview candidates. 
        /// </returns>
        public async Task<List<Candidate>> GetCandidates()
        {
            List<Candidate> candidates = new List<Candidate>();

            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get,
                "https://cmricandidates.azurewebsites.net/api/getcandidates");
            HttpClient client = m_clientFactory.CreateClient();

            var response = await client.SendAsync(request);

            if (response.IsSuccessStatusCode)
            {
                Stream responseStream = await response.Content.ReadAsStreamAsync();
                candidates = await JsonSerializer.DeserializeAsync<List<Candidate>>(responseStream);
            }
            
            return candidates;
        }
    }
}