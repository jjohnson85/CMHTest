using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Net.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using CMHTest.InterviewCheck.Model;
using CMHTest.InterviewCheck.DataAccess;

namespace CMHTest.InterviewCheck.Controller
{
    /// <summary>
    /// The API controller for the Interview Checker test API. 
    /// </summary>
    [ApiController]
    [Route("api/CheckInterviews")]
    public class CheckInterviewsController : ControllerBase
    {
        private readonly ILogger<CheckInterviewsController> m_logger;
        private IHttpClientFactory m_httpClientFactory;

        /// <summary>
        /// Constructor for the API controller.
        /// </summary>
        public CheckInterviewsController(ILogger<CheckInterviewsController> logger, IHttpClientFactory httpClientFactory)
        {
            m_logger = logger;
            m_httpClientFactory = httpClientFactory;
        }

        /// <summary>
        /// Controller method that accepts a POST request with a date time in the body. The time portion is discarded
        /// and only the date is used to determine interview date matches. Does not account for time zones. 
        /// </summary>
        /// <param>
        /// NumberOfInterviewsRequest - Request JSON containing a datetime deserialized into a simple object.
        /// </param>
        /// <returns>
        /// NumberOfInterviewsResponse - Response POCO to be serialized and returned the API consumer. The number of 
        /// interview candidates that matched the provided date.
        /// </returns>
        [HttpPost]
        public async Task<ActionResult<NumberOfInterviewsResponse>> CheckInterviews([FromBody]NumberOfInterviewsRequest request)
        {
            try
            {
                if( request == null )
                {
                    return BadRequest();
                }

                NumberOfInterviewsResponse response = new NumberOfInterviewsResponse();
                List<Candidate> candidates = await new GetCandidatesDelegate(m_httpClientFactory).GetCandidates();
                response.numberOfInterviews =  candidates.Where(c => c.dateOfInterview.Date.Equals(request.dateOfInterview.Date)).Count();
                return Ok(response);
            }
            catch( Exception e )
            {
                m_logger.LogError(e, "InterviewCheck has encountered a critical application exception.");
                return StatusCode(StatusCodes.Status500InternalServerError, "InterviewCheck has encountered a critical application exception.");
            }
        }
    }
}
