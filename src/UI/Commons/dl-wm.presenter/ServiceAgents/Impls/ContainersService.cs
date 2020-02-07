using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using dl.wm.models.DTOs.Containers;
using dl.wm.models.DTOs.Users;
using dl.wm.models.DTOs.Users.Accounts;
using dl.wm.presenter.Exceptions;
using dl.wm.presenter.ServiceAgents.Contracts;
using dl.wm.presenter.ServiceAgents.Impls.Base;
using Newtonsoft.Json;
using RestSharp;

namespace dl.wm.presenter.ServiceAgents.Impls
{
    public class ContainersService : BaseService<ContainerUiModel>, IContainersService
    {
        private static readonly string _serviceName = "ContainersService";

        public ContainersService() : base(_serviceName)
        {

        }

        public async Task<IList<ContainerUiModel>> GetAllActiveContainersAsync(bool active)
        {
            UriBuilder builder = CreateUriBuilder();
            builder.Path += active.ToString();
            return await RequestProvider.GetAsync<IList<ContainerUiModel>>(builder.ToString());
        }

        public async Task<ImageContainerDto> UploadImage(string imagePath, string imageFile)
        {
            UriBuilder builder = CreateUriBuilder();
            builder.Path += "image-upload";
            using (FileStream fs = new FileStream(imagePath, FileMode.Open, FileAccess.Read, FileShare.Read, 8,
                true))
            {
                HttpContent fileStreamContent = new StreamContent(fs);
                fileStreamContent.Headers.ContentDisposition =
                    new System.Net.Http.Headers.ContentDispositionHeaderValue("form-data")
                        {Name = "file", FileName = imageFile};
                fileStreamContent.Headers.ContentType =
                    new System.Net.Http.Headers.MediaTypeHeaderValue("application/octet-stream");
                using (var client = new HttpClient())
                using (var formData = new MultipartFormDataContent())
                {
                    formData.Add(fileStreamContent);
                    var response = await client.PostAsync(builder.Uri.ToString(), formData);
                    var imageContainerDto =
                        JsonConvert.DeserializeObject<ImageContainerDto>(await response.Content.ReadAsStringAsync());
                    return imageContainerDto;
                }
            }
        }

        public async Task<ContainerUiModel> CreateContainerAsync(ContainerUiModel changedContainer,
            string authorizationToken = null)
        {
            UriBuilder builder = CreateUriBuilder();

            ContainerUiModel result = new ContainerUiModel();

            var client = new RestClient(builder.Uri.ToString());
            var request = new RestRequest("", Method.POST);

            request.AddJsonBody(new ContainerForCreationModel()
            {
                ContainerName = changedContainer.ContainerName,
                ContainerLevel = changedContainer.ContainerLevel,
                ContainerFillLevel = changedContainer.ContainerFillLevel,
                ContainerLat = changedContainer.ContainerLocationLat,
                ContainerLong = changedContainer.ContainerLocationLong,
                ContainerType = changedContainer.ContainerType,
                ContainerStatus = changedContainer.ContainerStatus,
                ContainerPickupDate = changedContainer.ContainerMandatoryPickupDate,
                ContainerPickupOption = changedContainer.ContainerMandatoryPickupOption,
                ContainerPickupActive = changedContainer.ContainerMandatoryPickupActive,
                ContainerAddress = changedContainer.ContainerAddress,
                ContainerImagePath = changedContainer.ContainerImagePath,
                ContainerImageName = changedContainer.ContainerImageName,
            });

            request.AddHeader("Content-Type", "application/json");
            request.AddHeader("Authorization", $"bearer {authorizationToken}");

            var response = client.Execute(request);
            if (response.IsSuccessful)
            {
                result = JsonConvert.DeserializeObject<ContainerUiModel>(response.Content);
            }
            else if (response.StatusCode == HttpStatusCode.BadRequest)
            {
                ContainerErrorModel resultError = JsonConvert.DeserializeObject<ContainerErrorModel>(response.Content);
                throw new ServiceHttpRequestException<string>(response.StatusCode, resultError.errorMessage);
            }

            return result;
        }
    }
}