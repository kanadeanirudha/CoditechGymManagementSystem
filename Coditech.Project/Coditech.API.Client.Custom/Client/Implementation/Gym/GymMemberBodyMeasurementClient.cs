using Coditech.API.Endpoint;
using Coditech.Common.API.Model;
using Coditech.Common.API.Model.Response;
using Coditech.Common.API.Model.Responses;
using Coditech.Common.Exceptions;
using Coditech.Common.Helper.Utilities;
using Newtonsoft.Json;
using System.Net;

namespace Coditech.API.Client
{
    public class GymMemberBodyMeasurementClient : BaseClient, IGymMemberBodyMeasurementClient
    {
        GymMemberBodyMeasurementEndpoint gymMemberBodyMeasurementEndpoint = null;
        public GymMemberBodyMeasurementClient()
        {
            gymMemberBodyMeasurementEndpoint = new GymMemberBodyMeasurementEndpoint();
        }

        public virtual GymMemberBodyMeasurementListResponse GetBodyMeasurementTypeListByMemberId(int gymMemberDetailId, long personId, short pageSize)
        {
            return Task.Run(async () => await GetBodyMeasurementTypeListByMemberIdAsync(gymMemberDetailId, personId, pageSize, System.Threading.CancellationToken.None)).GetAwaiter().GetResult();
        }

        public virtual async Task<GymMemberBodyMeasurementListResponse> GetBodyMeasurementTypeListByMemberIdAsync(int gymMemberDetailId, long personId, short pageSize, CancellationToken cancellationToken)
        {
            string endpoint = gymMemberBodyMeasurementEndpoint.GetBodyMeasurementTypeListByMemberIdAsync(gymMemberDetailId, personId, pageSize);
            HttpResponseMessage response = null;
            var disposeResponse = true;
            try
            {
                ApiStatus status = new ApiStatus();

                response = await GetResourceFromEndpointAsync(endpoint, status, cancellationToken).ConfigureAwait(false);
                Dictionary<string, IEnumerable<string>> headers_ = BindHeaders(response);
                var status_ = (int)response.StatusCode;
                if (status_ == 200)
                {
                    var objectResponse = await ReadObjectResponseAsync<GymMemberBodyMeasurementListResponse>(response, headers_, cancellationToken).ConfigureAwait(false);
                    if (objectResponse.Object == null)
                    {
                        throw new CoditechException(objectResponse.Object.ErrorCode, objectResponse.Object.ErrorMessage);
                    }
                    return objectResponse.Object;
                }
                else if (status_ == 204)
                {
                    return new GymMemberBodyMeasurementListResponse();
                }
                else
                {
                    string responseData = response.Content == null ? null : await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                    GymMemberBodyMeasurementListResponse typedBody = JsonConvert.DeserializeObject<GymMemberBodyMeasurementListResponse>(responseData);
                    UpdateApiStatus(typedBody, status, response);
                    throw new CoditechException(status.ErrorCode, status.ErrorMessage, status.StatusCode);
                }
            }
            finally
            {
                if (disposeResponse)
                    response.Dispose();
            }
        }

        public virtual GymMemberBodyMeasurementListResponse List(IEnumerable<string> expand, IEnumerable<FilterTuple> filter, IDictionary<string, string> sort, int? pageIndex, int? pageSize)
        {
            return Task.Run(async () => await ListAsync(expand, filter, sort, pageIndex, pageSize, System.Threading.CancellationToken.None)).GetAwaiter().GetResult();
        }

        public virtual async Task<GymMemberBodyMeasurementListResponse> ListAsync(IEnumerable<string> expand, IEnumerable<FilterTuple> filter, IDictionary<string, string> sort, int? pageIndex, int? pageSize, CancellationToken cancellationToken)
        {
            string endpoint = gymMemberBodyMeasurementEndpoint.ListAsync(expand, filter, sort, pageIndex, pageSize);
            HttpResponseMessage response = null;
            var disposeResponse = true;
            try
            {
                ApiStatus status = new ApiStatus();

                response = await GetResourceFromEndpointAsync(endpoint, status, cancellationToken).ConfigureAwait(false);
                Dictionary<string, IEnumerable<string>> headers_ = BindHeaders(response);
                var status_ = (int)response.StatusCode;
                if (status_ == 200)
                {
                    var objectResponse = await ReadObjectResponseAsync<GymMemberBodyMeasurementListResponse>(response, headers_, cancellationToken).ConfigureAwait(false);
                    if (objectResponse.Object == null)
                    {
                        throw new CoditechException(objectResponse.Object.ErrorCode, objectResponse.Object.ErrorMessage);
                    }
                    return objectResponse.Object;
                }
                else if (status_ == 204)
                {
                    return new GymMemberBodyMeasurementListResponse();
                }
                else
                {
                    string responseData = response.Content == null ? null : await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                    GymMemberBodyMeasurementListResponse typedBody = JsonConvert.DeserializeObject<GymMemberBodyMeasurementListResponse>(responseData);
                    UpdateApiStatus(typedBody, status, response);
                    throw new CoditechException(status.ErrorCode, status.ErrorMessage, status.StatusCode);
                }
            }
            finally
            {
                if (disposeResponse)
                    response.Dispose();
            }
        }

        public virtual GymMemberBodyMeasurementResponse CreateMemberBodyMeasurement(GymMemberBodyMeasurementModel body)
        {
            return Task.Run(async () => await CreateMemberBodyMeasurementAsync(body, CancellationToken.None)).GetAwaiter().GetResult();
        }

        public virtual async Task<GymMemberBodyMeasurementResponse> CreateMemberBodyMeasurementAsync(GymMemberBodyMeasurementModel body, CancellationToken cancellationToken)
        {
            string endpoint = gymMemberBodyMeasurementEndpoint.CreateMemberBodyMeasurementAsync();
            HttpResponseMessage response = null;
            bool disposeResponse = true;
            try
            {
                ApiStatus status = new ApiStatus();
                response = await PostResourceToEndpointAsync(endpoint, JsonConvert.SerializeObject(body), status, cancellationToken).ConfigureAwait(continueOnCapturedContext: false);
                Dictionary<string, IEnumerable<string>> dictionary = BindHeaders(response);

                switch (response.StatusCode)
                {
                    case HttpStatusCode.OK:
                        {
                            ObjectResponseResult<GymMemberBodyMeasurementResponse> objectResponseResult2 = await ReadObjectResponseAsync<GymMemberBodyMeasurementResponse>(response, BindHeaders(response), cancellationToken).ConfigureAwait(continueOnCapturedContext: false);
                            if (objectResponseResult2.Object == null)
                            {
                                throw new CoditechException(objectResponseResult2.Object.ErrorCode, objectResponseResult2.Object.ErrorMessage);
                            }

                            return objectResponseResult2.Object;
                        }
                    case HttpStatusCode.Created:
                        {
                            ObjectResponseResult<GymMemberBodyMeasurementResponse> objectResponseResult = await ReadObjectResponseAsync<GymMemberBodyMeasurementResponse>(response, dictionary, cancellationToken).ConfigureAwait(continueOnCapturedContext: false);
                            if (objectResponseResult.Object == null)
                            {
                                throw new CoditechException(objectResponseResult.Object.ErrorCode, objectResponseResult.Object.ErrorMessage);
                            }

                            return objectResponseResult.Object;
                        }
                    default:
                        {
                            string value = ((response.Content != null) ? (await response.Content.ReadAsStringAsync().ConfigureAwait(continueOnCapturedContext: false)) : null);
                            GymMemberBodyMeasurementResponse result = JsonConvert.DeserializeObject<GymMemberBodyMeasurementResponse>(value);
                            UpdateApiStatus(result, status, response);
                            throw new CoditechException(status.ErrorCode, status.ErrorMessage, status.StatusCode);
                        }
                }
            }
            finally
            {
                if (disposeResponse)
                {
                    response.Dispose();
                }
            }
        }

        public virtual GymMemberBodyMeasurementResponse GetMemberBodyMeasurement(long gymMemberBodyMeasurementId, short gymBodyMeasurementTypeId)
        {
            return Task.Run(async () => await GetMemberBodyMeasurementAsync(gymMemberBodyMeasurementId, gymBodyMeasurementTypeId, System.Threading.CancellationToken.None)).GetAwaiter().GetResult();
        }

        public virtual async Task<GymMemberBodyMeasurementResponse> GetMemberBodyMeasurementAsync(long gymMemberBodyMeasurementId,short gymBodyMeasurementTypeId, System.Threading.CancellationToken cancellationToken)
        {
            if (gymMemberBodyMeasurementId <= 0)
                throw new System.ArgumentNullException("gymMemberBodyMeasurementId");

            string endpoint = gymMemberBodyMeasurementEndpoint.GetMemberBodyMeasurementAsync(gymMemberBodyMeasurementId);
            HttpResponseMessage response = null;
            var disposeResponse = true;
            try
            {
                ApiStatus status = new ApiStatus();

                response = await GetResourceFromEndpointAsync(endpoint, status, cancellationToken).ConfigureAwait(false);
                Dictionary<string, IEnumerable<string>> headers_ = BindHeaders(response);
                var status_ = (int)response.StatusCode;
                if (status_ == 200)
                {
                    var objectResponse = await ReadObjectResponseAsync<GymMemberBodyMeasurementResponse>(response, headers_, cancellationToken).ConfigureAwait(false);
                    if (objectResponse.Object == null)
                    {
                        throw new CoditechException(objectResponse.Object.ErrorCode, objectResponse.Object.ErrorMessage);
                    }
                    return objectResponse.Object;
                }
                else
                if (status_ == 204)
                {
                    return new GymMemberBodyMeasurementResponse();
                }
                else
                {
                    string responseData = response.Content == null ? null : await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                    GymMemberBodyMeasurementResponse typedBody = JsonConvert.DeserializeObject<GymMemberBodyMeasurementResponse>(responseData);
                    UpdateApiStatus(typedBody, status, response);
                    throw new CoditechException(status.ErrorCode, status.ErrorMessage, status.StatusCode);
                }
            }
            finally
            {
                if (disposeResponse)
                    response.Dispose();
            }
        }

        public virtual GymMemberBodyMeasurementResponse UpdateMemberBodyMeasurement(GymMemberBodyMeasurementModel body)
        {
            return Task.Run(async () => await UpdateMemberBodyMeasurementAsync(body, System.Threading.CancellationToken.None)).GetAwaiter().GetResult();
        }

        public virtual async Task<GymMemberBodyMeasurementResponse> UpdateMemberBodyMeasurementAsync(GymMemberBodyMeasurementModel body, System.Threading.CancellationToken cancellationToken)
        {
            string endpoint = gymMemberBodyMeasurementEndpoint.UpdateMemberBodyMeasurementAsync();
            HttpResponseMessage response = null;
            var disposeResponse = true;
            try
            {
                ApiStatus status = new ApiStatus();

                response = await PutResourceToEndpointAsync(endpoint, JsonConvert.SerializeObject(body), status, cancellationToken).ConfigureAwait(false);

                var headers_ = BindHeaders(response);
                var status_ = (int)response.StatusCode;
                if (status_ == 200)
                {
                    var objectResponse = await ReadObjectResponseAsync<GymMemberBodyMeasurementResponse>(response, headers_, cancellationToken).ConfigureAwait(false);
                    if (objectResponse.Object == null)
                    {
                        throw new CoditechException(objectResponse.Object.ErrorCode, objectResponse.Object.ErrorMessage);
                    }
                    return objectResponse.Object;
                }
                else
                if (status_ == 201)
                {
                    var objectResponse = await ReadObjectResponseAsync<GymMemberBodyMeasurementResponse>(response, headers_, cancellationToken).ConfigureAwait(false);
                    if (objectResponse.Object == null)
                    {
                        throw new CoditechException(objectResponse.Object.ErrorCode, objectResponse.Object.ErrorMessage);
                    }
                    return objectResponse.Object;
                }
                else
                {
                    string responseData = response.Content == null ? null : await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                    GymMemberBodyMeasurementResponse typedBody = JsonConvert.DeserializeObject<GymMemberBodyMeasurementResponse>(responseData);
                    UpdateApiStatus(typedBody, status, response);
                    throw new CoditechException(status.ErrorCode, status.ErrorMessage, status.StatusCode);
                }

            }
            finally
            {
                if (disposeResponse)
                    response.Dispose();
            }
        }

        public virtual TrueFalseResponse DeleteMemberBodyMeasurement(ParameterModel body)
        {
            return Task.Run(async () => await DeleteMemberBodyMeasurementAsync(body, System.Threading.CancellationToken.None)).GetAwaiter().GetResult();
        }

        public virtual async Task<TrueFalseResponse> DeleteMemberBodyMeasurementAsync(ParameterModel body, System.Threading.CancellationToken cancellationToken)
        {
            string endpoint = gymMemberBodyMeasurementEndpoint.DeleteMemberBodyMeasurementAsync();
            HttpResponseMessage response = null;
            var disposeResponse = true;
            try
            {
                ApiStatus status = new ApiStatus();

                response = await PostResourceToEndpointAsync(endpoint, JsonConvert.SerializeObject(body), status, cancellationToken).ConfigureAwait(false);

                var headers_ = BindHeaders(response);
                var status_ = (int)response.StatusCode;
                if (status_ == 200)
                {
                    var objectResponse = await ReadObjectResponseAsync<TrueFalseResponse>(response, headers_, cancellationToken).ConfigureAwait(false);
                    if (objectResponse.Object == null)
                    {
                        throw new CoditechException(objectResponse.Object.ErrorCode, objectResponse.Object.ErrorMessage);
                    }
                    return objectResponse.Object;
                }
                else
                {
                    string responseData = response.Content == null ? null : await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                    TrueFalseResponse typedBody = JsonConvert.DeserializeObject<TrueFalseResponse>(responseData);
                    UpdateApiStatus(typedBody, status, response);
                    throw new CoditechException(status.ErrorCode, status.ErrorMessage, status.StatusCode);
                }
            }
            finally
            {
                if (disposeResponse)
                    response.Dispose();
            }
        }
    }
}
