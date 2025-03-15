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
    public class GymWorkoutPlanClient : BaseClient, IGymWorkoutPlanClient
    {
        GymWorkoutPlanEndpoint gymWorkoutPlanEndpoint = null;

        public GymWorkoutPlanClient()
        {
            gymWorkoutPlanEndpoint = new GymWorkoutPlanEndpoint();
        }
        public virtual GymWorkoutPlanListResponse List(string SelectedCentreCode, IEnumerable<string> expand, IEnumerable<FilterTuple> filter, IDictionary<string, string> sort, int? pageIndex, int? pageSize)
        {
            return Task.Run(async () => await ListAsync(SelectedCentreCode, expand, filter, sort, pageIndex, pageSize, CancellationToken.None)).GetAwaiter().GetResult();
        }

        public virtual async Task<GymWorkoutPlanListResponse> ListAsync(string SelectedCentreCode, IEnumerable<string> expand, IEnumerable<FilterTuple> filter, IDictionary<string, string> sort, int? pageIndex, int? pageSize, CancellationToken cancellationToken)
        {
            string endpoint = gymWorkoutPlanEndpoint.ListAsync(SelectedCentreCode, expand, filter, sort, pageIndex, pageSize);
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
                    var objectResponse = await ReadObjectResponseAsync<GymWorkoutPlanListResponse>(response, headers_, cancellationToken).ConfigureAwait(false);
                    if (objectResponse.Object == null)
                    {
                        throw new CoditechException(objectResponse.Object.ErrorCode, objectResponse.Object.ErrorMessage);
                    }
                    return objectResponse.Object;
                }
                else if (status_ == 204)
                {
                    return new GymWorkoutPlanListResponse();
                }
                else
                {
                    string responseData = response.Content == null ? null : await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                    GymWorkoutPlanListResponse typedBody = JsonConvert.DeserializeObject<GymWorkoutPlanListResponse>(responseData);
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

        public virtual GymWorkoutPlanResponse CreateGymWorkoutPlan(GymWorkoutPlanModel body)
        {
            return Task.Run(async () => await CreateGymWorkoutPlanAsync(body, CancellationToken.None)).GetAwaiter().GetResult();
        }

        public virtual async Task<GymWorkoutPlanResponse> CreateGymWorkoutPlanAsync(GymWorkoutPlanModel body, CancellationToken cancellationToken)
        {
            string endpoint = gymWorkoutPlanEndpoint.CreateGymWorkoutPlanAsync();
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
                            ObjectResponseResult<GymWorkoutPlanResponse> objectResponseResult2 = await ReadObjectResponseAsync<GymWorkoutPlanResponse>(response, BindHeaders(response), cancellationToken).ConfigureAwait(continueOnCapturedContext: false);
                            if (objectResponseResult2.Object == null)
                            {
                                throw new CoditechException(objectResponseResult2.Object.ErrorCode, objectResponseResult2.Object.ErrorMessage);
                            }

                            return objectResponseResult2.Object;
                        }
                    case HttpStatusCode.Created:
                        {
                            ObjectResponseResult<GymWorkoutPlanResponse> objectResponseResult = await ReadObjectResponseAsync<GymWorkoutPlanResponse>(response, dictionary, cancellationToken).ConfigureAwait(continueOnCapturedContext: false);
                            if (objectResponseResult.Object == null)
                            {
                                throw new CoditechException(objectResponseResult.Object.ErrorCode, objectResponseResult.Object.ErrorMessage);
                            }

                            return objectResponseResult.Object;
                        }
                    default:
                        {
                            string value = ((response.Content != null) ? (await response.Content.ReadAsStringAsync().ConfigureAwait(continueOnCapturedContext: false)) : null);
                            GymWorkoutPlanResponse result = JsonConvert.DeserializeObject<GymWorkoutPlanResponse>(value);
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

        public virtual GymWorkoutPlanResponse GetGymWorkoutPlan(long gymWorkoutPlanId)
        {
            return Task.Run(async () => await GetGymWorkoutPlanAsync(gymWorkoutPlanId, CancellationToken.None)).GetAwaiter().GetResult();
        }

        public virtual async Task<GymWorkoutPlanResponse> GetGymWorkoutPlanAsync(long gymWorkoutPlanId, CancellationToken cancellationToken)
        {
            if (gymWorkoutPlanId <= 0)
                throw new System.ArgumentNullException("gymWorkoutPlanId");

            string endpoint = gymWorkoutPlanEndpoint.GetGymWorkoutPlanAsync(gymWorkoutPlanId);
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
                    var objectResponse = await ReadObjectResponseAsync<GymWorkoutPlanResponse>(response, headers_, cancellationToken).ConfigureAwait(false);
                    if (objectResponse.Object == null)
                    {
                        throw new CoditechException(objectResponse.Object.ErrorCode, objectResponse.Object.ErrorMessage);
                    }
                    return objectResponse.Object;
                }
                else
                if (status_ == 204)
                {
                    return new GymWorkoutPlanResponse();
                }
                else
                {
                    string responseData = response.Content == null ? null : await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                    GymWorkoutPlanResponse typedBody = JsonConvert.DeserializeObject<GymWorkoutPlanResponse>(responseData);
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

        public virtual GymWorkoutPlanResponse UpdateGymWorkoutPlan(GymWorkoutPlanModel body)
        {
            return Task.Run(async () => await UpdateGymWorkoutPlanAsync(body, CancellationToken.None)).GetAwaiter().GetResult();
        }

        public virtual async Task<GymWorkoutPlanResponse> UpdateGymWorkoutPlanAsync(GymWorkoutPlanModel body, CancellationToken cancellationToken)
        {
            string endpoint = gymWorkoutPlanEndpoint.UpdateGymWorkoutPlanAsync();
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
                    var objectResponse = await ReadObjectResponseAsync<GymWorkoutPlanResponse>(response, headers_, cancellationToken).ConfigureAwait(false);
                    if (objectResponse.Object == null)
                    {
                        throw new CoditechException(objectResponse.Object.ErrorCode, objectResponse.Object.ErrorMessage);
                    }
                    return objectResponse.Object;
                }
                else
                if (status_ == 201)
                {
                    var objectResponse = await ReadObjectResponseAsync<GymWorkoutPlanResponse>(response, headers_, cancellationToken).ConfigureAwait(false);
                    if (objectResponse.Object == null)
                    {
                        throw new CoditechException(objectResponse.Object.ErrorCode, objectResponse.Object.ErrorMessage);
                    }
                    return objectResponse.Object;
                }
                else
                {
                    string responseData = response.Content == null ? null : await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                    GymWorkoutPlanResponse typedBody = JsonConvert.DeserializeObject<GymWorkoutPlanResponse>(responseData);
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

        #region WorkoutPlanDetails
        public virtual GymWorkoutPlanResponse GetWorkoutPlanDetails(long gymWorkoutPlanId)
        {
            return Task.Run(async () => await GetWorkoutPlanDetailsAsync(gymWorkoutPlanId, CancellationToken.None)).GetAwaiter().GetResult();
        }

        public virtual async Task<GymWorkoutPlanResponse> GetWorkoutPlanDetailsAsync(long gymWorkoutPlanId, CancellationToken cancellationToken)
        {
            if (gymWorkoutPlanId <= 0)
                throw new System.ArgumentNullException("gymWorkoutPlanId");

            string endpoint = gymWorkoutPlanEndpoint.GetWorkoutPlanDetailsAsync(gymWorkoutPlanId);
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
                    var objectResponse = await ReadObjectResponseAsync<GymWorkoutPlanResponse>(response, headers_, cancellationToken).ConfigureAwait(false);
                    if (objectResponse.Object == null)
                    {
                        throw new CoditechException(objectResponse.Object.ErrorCode, objectResponse.Object.ErrorMessage);
                    }
                    return objectResponse.Object;
                }
                else
                if (status_ == 204)
                {
                    return new GymWorkoutPlanResponse();
                }
                else
                {
                    string responseData = response.Content == null ? null : await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                    GymWorkoutPlanResponse typedBody = JsonConvert.DeserializeObject<GymWorkoutPlanResponse>(responseData);
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

        //Create Workout Plan Details
        public virtual GymWorkoutPlanDetailsResponse AddWorkoutPlanDetails(GymWorkoutPlanDetailsModel body)
        {
            return Task.Run(async () => await AddWorkoutPlanDetailsAsync(body, CancellationToken.None)).GetAwaiter().GetResult();
        }

        public virtual async Task<GymWorkoutPlanDetailsResponse> AddWorkoutPlanDetailsAsync(GymWorkoutPlanDetailsModel body, CancellationToken cancellationToken)
        {
            string endpoint = gymWorkoutPlanEndpoint.AddWorkoutPlanDetailsAsync();
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
                            ObjectResponseResult<GymWorkoutPlanDetailsResponse> objectResponseResult2 = await ReadObjectResponseAsync<GymWorkoutPlanDetailsResponse>(response, BindHeaders(response), cancellationToken).ConfigureAwait(continueOnCapturedContext: false);
                            if (objectResponseResult2.Object == null)
                            {
                                throw new CoditechException(objectResponseResult2.Object.ErrorCode, objectResponseResult2.Object.ErrorMessage);
                            }

                            return objectResponseResult2.Object;
                        }
                    case HttpStatusCode.Created:
                        {
                            ObjectResponseResult<GymWorkoutPlanDetailsResponse> objectResponseResult = await ReadObjectResponseAsync<GymWorkoutPlanDetailsResponse>(response, dictionary, cancellationToken).ConfigureAwait(continueOnCapturedContext: false);
                            if (objectResponseResult.Object == null)
                            {
                                throw new CoditechException(objectResponseResult.Object.ErrorCode, objectResponseResult.Object.ErrorMessage);
                            }

                            return objectResponseResult.Object;
                        }
                    default:
                        {
                            string value = ((response.Content != null) ? (await response.Content.ReadAsStringAsync().ConfigureAwait(continueOnCapturedContext: false)) : null);
                            GymWorkoutPlanDetailsResponse result = JsonConvert.DeserializeObject<GymWorkoutPlanDetailsResponse>(value);
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


        #endregion
        #region Delete Workout Plan Details

        public virtual TrueFalseResponse DeleteGymWorkoutPlanDetails(DeleteWorkoutPlanDetailsModel body)
        {
            return Task.Run(async () => await DeleteGymWorkoutPlanDetailsAsync(body, CancellationToken.None)).GetAwaiter().GetResult();
        }

        public virtual async Task<TrueFalseResponse> DeleteGymWorkoutPlanDetailsAsync(DeleteWorkoutPlanDetailsModel body, CancellationToken cancellationToken)
        {
            string endpoint = gymWorkoutPlanEndpoint.DeleteGymWorkoutPlanDetailsAsync();
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
        #endregion
       
        #region Gym Workout Plan User 
        public virtual GymWorkoutPlanUserListResponse GetAssociatedMemberList(long gymWorkoutPlanId,IEnumerable<string> expand, IEnumerable<FilterTuple> filter, IDictionary<string, string> sort, int? pageIndex, int? pageSize)
        {
            return Task.Run(async () => await GetAssociatedMemberListAsync(gymWorkoutPlanId, expand, filter, sort, pageIndex, pageSize, System.Threading.CancellationToken.None)).GetAwaiter().GetResult();
        }

        public virtual async Task<GymWorkoutPlanUserListResponse> GetAssociatedMemberListAsync(long gymWorkoutPlanId,IEnumerable<string> expand, IEnumerable<FilterTuple> filter, IDictionary<string, string> sort, int? pageIndex, int? pageSize, CancellationToken cancellationToken)
        {
            string endpoint = gymWorkoutPlanEndpoint.GetAssociatedMemberListAsync(gymWorkoutPlanId, expand, filter, sort, pageIndex, pageSize);
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
                    var objectResponse = await ReadObjectResponseAsync<GymWorkoutPlanUserListResponse>(response, headers_, cancellationToken).ConfigureAwait(false);
                    if (objectResponse.Object == null)
                    {
                        throw new CoditechException(objectResponse.Object.ErrorCode, objectResponse.Object.ErrorMessage);
                    }
                    return objectResponse.Object;
                }
                else if (status_ == 204)
                {
                    return new GymWorkoutPlanUserListResponse();
                }
                else
                {
                    string responseData = response.Content == null ? null : await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                    GymWorkoutPlanUserListResponse typedBody = JsonConvert.DeserializeObject<GymWorkoutPlanUserListResponse>(responseData);
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

        public virtual GymWorkoutPlanUserResponse AssociateUnAssociateWorkoutPlanUser(GymWorkoutPlanUserModel body)
        {
            return Task.Run(async () => await AssociateUnAssociateWorkoutPlanUserAsync(body, System.Threading.CancellationToken.None)).GetAwaiter().GetResult();
        }

        public virtual async Task<GymWorkoutPlanUserResponse> AssociateUnAssociateWorkoutPlanUserAsync(GymWorkoutPlanUserModel body, System.Threading.CancellationToken cancellationToken)
        {
            string endpoint = gymWorkoutPlanEndpoint.AssociateUnAssociateWorkoutPlanUserAsync();
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
                    var objectResponse = await ReadObjectResponseAsync<GymWorkoutPlanUserResponse>(response, headers_, cancellationToken).ConfigureAwait(false);
                    if (objectResponse.Object == null)
                    {
                        throw new CoditechException(objectResponse.Object.ErrorCode, objectResponse.Object.ErrorMessage);
                    }
                    return objectResponse.Object;
                }
                else
                if (status_ == 201)
                {
                    var objectResponse = await ReadObjectResponseAsync<GymWorkoutPlanUserResponse>(response, headers_, cancellationToken).ConfigureAwait(false);
                    if (objectResponse.Object == null)
                    {
                        throw new CoditechException(objectResponse.Object.ErrorCode, objectResponse.Object.ErrorMessage);
                    }
                    return objectResponse.Object;
                }
                else
                {
                    string responseData = response.Content == null ? null : await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                    GymWorkoutPlanUserResponse typedBody = JsonConvert.DeserializeObject<GymWorkoutPlanUserResponse>(responseData);
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
        #endregion
    }
}
