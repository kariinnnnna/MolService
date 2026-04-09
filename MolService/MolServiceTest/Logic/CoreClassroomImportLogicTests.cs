using Microsoft.Extensions.Configuration;
using MolServiceBusinessLogic.Helpers;
using MolServiceBusinessLogic.Implements;
using MolServiceContracts.BindingModels;
using MolServiceContracts.SearchModels;
using MolServiceContracts.StorageContracts;
using MolServiceContracts.ViewModels;
using MolServiceDataModels.Enums;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace MolServiceTest.Logic
{
    public class CoreClassroomImportLogicTests
    {
        [Fact]
        public async Task ImportClassroomsAsync_ShouldInsert_WhenClassroomDoesNotExist()
        {
            var json = @"[
  {
    ""Id"": 101,
    ""Number"": ""305"",
    ""Type"": 0,
    ""Capacity"": 30,
    ""NotUseInSchedule"": false,
    ""HasProjector"": true
  }
]";


            var classroomStorageMock = new Mock<IClassroomStorage>();

            classroomStorageMock
                .Setup(x => x.GetElement(It.Is<ClassroomSearchModel>(m => m.CoreSystemId == 101)))
                .Returns((ClassroomViewModel?)null);

            var logic = new CoreClassroomImportLogic(
                classroomStorageMock.Object,
                CreateCoreApiService(json));

            await logic.ImportClassroomsAsync();

            classroomStorageMock.Verify(x => x.Insert(It.Is<ClassroomBindingModel>(m =>
                m.CoreSystemId == 101 &&
                m.Number == "305" &&
                m.Type == ClassroomType.Lecture &&
                m.Capacity == 30 &&
                m.NotUseInSchedule == false &&
                m.HasProjector == true &&
                m.Id == 0)), Times.Once);

            classroomStorageMock.Verify(x => x.Update(It.IsAny<ClassroomBindingModel>()), Times.Never);
        }

        [Fact]
        public async Task ImportClassroomsAsync_ShouldUpdate_WhenClassroomAlreadyExists()
        {
            var json = @"[
  {
    ""Id"": 202,
    ""Number"": ""401"",
    ""Type"": 2,
    ""Capacity"": 18,
    ""NotUseInSchedule"": true,
    ""HasProjector"": false
  }
]";

            var classroomStorageMock = new Mock<IClassroomStorage>();

            classroomStorageMock
                .Setup(x => x.GetElement(It.Is<ClassroomSearchModel>(m => m.CoreSystemId == 202)))
                .Returns(new ClassroomViewModel
                {
                    Id = 55,
                    CoreSystemId = 202,
                    Number = "Старый номер"
                });

            var logic = new CoreClassroomImportLogic(
                classroomStorageMock.Object,
                CreateCoreApiService(json));

            await logic.ImportClassroomsAsync();

            classroomStorageMock.Verify(x => x.Update(It.Is<ClassroomBindingModel>(m =>
                m.Id == 55 &&
                m.CoreSystemId == 202 &&
                m.Number == "401" &&
                m.Type == ClassroomType.Computer &&
                m.Capacity == 18 &&
                m.NotUseInSchedule == true &&
                m.HasProjector == false)), Times.Once);

            classroomStorageMock.Verify(x => x.Insert(It.IsAny<ClassroomBindingModel>()), Times.Never);
        }

        private static CoreApiService CreateCoreApiService(string json)
        {
            var httpClient = new HttpClient(new FakeHttpMessageHandler(json));

            var configuration = new ConfigurationBuilder()
                .AddInMemoryCollection(new Dictionary<string, string?>
                {
                    ["CoreApi:BaseUrl"] = "http://fake-core-api"
                })
                .Build();

            return new CoreApiService(httpClient, configuration);
        }

        private sealed class FakeHttpMessageHandler : HttpMessageHandler
        {
            private readonly string _json;

            public FakeHttpMessageHandler(string json)
            {
                _json = json;
            }

            protected override Task<HttpResponseMessage> SendAsync(
                HttpRequestMessage request,
                CancellationToken cancellationToken)
            {
                var response = new HttpResponseMessage(HttpStatusCode.OK)
                {
                    Content = new StringContent(_json, Encoding.UTF8, "application/json")
                };

                return Task.FromResult(response);
            }
        }
    }
}
