using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using RestSharp;
using SharpenUp.Requests;
using SharpenUp.Results;

namespace SharpenUp
{
    public class UptimeRobot
    {
        private readonly string _apiKey;

        public UptimeRobot( string apiKey )
        {
            _apiKey = apiKey;
        }

        #region Account Details

        /// <summary>
        /// Account details (max number of monitors that can be added and number of up/down/paused monitors) can be grabbed using this method.
        /// </summary>
        /// <returns></returns>
        public async Task<AccountDetailsResult> GetAccountDetailsAsync()
        {
            RestClient restClient = new RestClient( "https://api.uptimerobot.com/v2/getAccountDetails" );
            RestRequest restRequest = new RestRequest( Method.POST );

            restRequest.AddHeader( "content-type", "application/x-www-form-urlencoded" );
            restRequest.AddHeader( "cache-control", "no-cache" );

            restRequest.AddParameter( "application/x-www-form-urlencoded", $"api_key={_apiKey}&format=json", ParameterType.RequestBody );

            IRestResponse response = await restClient.ExecuteAsync( restRequest );

            return JsonConvert.DeserializeObject<AccountDetailsResult>( response.Content );
        }

        #endregion

        #region Monitors

        public async Task<MonitorsResult> GetMonitorsAsync()
        {
            return await GetMonitorsAsync( new MonitorsRequest() );
        }

        private async Task<MonitorsResult> GetMonitorsAsync( MonitorsRequest request )
        {
            StringBuilder queryString = new StringBuilder( $"api_key={_apiKey}&format=json" );

            RestClient restClient = new RestClient( "https://api.uptimerobot.com/v2/getMonitors" );
            RestRequest restRequest = new RestRequest( Method.POST );

            restRequest.AddHeader( "content-type", "application/x-www-form-urlencoded" );
            restRequest.AddHeader( "cache-control", "no-cache" );

            restRequest.AddParameter( "application/x-www-form-urlencoded", queryString.ToString(), ParameterType.RequestBody );

            IRestResponse response = await restClient.ExecuteAsync( restRequest );

            return JsonConvert.DeserializeObject<MonitorsResult>( response.Content );
        }

        #endregion

        #region Alert Contacts

        /// <summary>
        /// The list of alert contacts can be called with this method.
        /// </summary>
        /// <returns></returns>
        public async Task<AlertContactsResult> GetAlertContactsAsync()
        {
            return await GetAlertContactsAsync( new AlertContactsRequest() );
        }

        /// <summary>
        /// The list of alert contacts can be called with this method.
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task<AlertContactsResult> GetAlertContactsAsync( AlertContactsRequest request )
        {
            StringBuilder queryString = new StringBuilder( $"api_key={_apiKey}&format=json" );

            if ( request.AlertContacts?.Count > 0 )
            {
                queryString.Append( "&alert_contacts=" );
                queryString.Append( string.Join( "-", request.AlertContacts ) );
            }

            if ( request.Offset != 0 )
            {
                queryString.Append( $"&offset={request.Offset}" );
            }

            if ( request.Limit != 50 )
            {
                queryString.Append( $"&limit={request.Limit}" );
            }

            RestClient restClient = new RestClient( "https://api.uptimerobot.com/v2/getAlertContacts" );
            RestRequest restRequest = new RestRequest( Method.POST );

            restRequest.AddHeader( "content-type", "application/x-www-form-urlencoded" );
            restRequest.AddHeader( "cache-control", "no-cache" );

            restRequest.AddParameter( "application/x-www-form-urlencoded", queryString.ToString(), ParameterType.RequestBody );

            IRestResponse response = await restClient.ExecuteAsync( restRequest );

            return JsonConvert.DeserializeObject<AlertContactsResult>( response.Content );
        }

        #endregion

        #region Public Status Pages

        /// <summary>
        /// The list of public status pages can be called with this method.
        /// </summary>
        /// <returns></returns>
        public async Task<PublicStatusPageResult> GetPublicStatusPagesAsync()
        {
            return await GetPublicStatusPagesAsync( new PublicStatusPageRequest() );
        }

        /// <summary>
        /// The list of public status pages can be called with this method.
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task<PublicStatusPageResult> GetPublicStatusPagesAsync( PublicStatusPageRequest request )
        {
            StringBuilder queryString = new StringBuilder( $"api_key={_apiKey}&format=json" );

            if ( request.PublicStatusPages?.Count > 0 )
            {
                queryString.Append( "&psps=" );
                queryString.Append( string.Join( "-", request.PublicStatusPages ) );
            }

            if ( request.Offset != 0 )
            {
                queryString.Append( $"&offset={request.Offset}" );
            }

            if ( request.Limit != 50 )
            {
                queryString.Append( $"&limit={request.Limit}" );
            }

            RestClient restClient = new RestClient( "https://api.uptimerobot.com/v2/getPSPs" );
            RestRequest restRequest = new RestRequest( Method.POST );

            restRequest.AddHeader( "content-type", "application/x-www-form-urlencoded" );
            restRequest.AddHeader( "cache-control", "no-cache" );

            restRequest.AddParameter( "application/x-www-form-urlencoded", queryString.ToString(), ParameterType.RequestBody );

            IRestResponse response = await restClient.ExecuteAsync( restRequest );

            return JsonConvert.DeserializeObject<PublicStatusPageResult>( response.Content );
        }

        #endregion
    }
}
