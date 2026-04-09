using MolServiceBusinessLogic.Helpers;
using MolServiceBusinessLogic.Implements;
using MolServiceContracts.BindingModels;
using MolServiceContracts.SearchModels;
using MolServiceContracts.StorageContracts;
using MolServiceContracts.ViewModels;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace MolServiceTest.Logic
{
    public class OneCImportLogicTests
    {
        [Fact]
        public async Task ImportFromOneCAsync_ShouldCreateMolAndEquipment_ForNewInventoryItem()
        {
            var responseJson = @"{
  ""ОС"": [
    {
      ""ОсновноеСредствоНаименование"": ""Ноутбук Dell"",
      ""ИнвентарныйНомер"": ""INV-777"",
      ""ЦМОНаименование"": ""Петров П.П. - Лаборатория 12"",
      ""СчетУчета"": ""101.34""
    }
  ]
}";

            var httpClient = CreateHttpClient(responseJson);
            var oneCApiService = new OneCApiService(httpClient);

            var mtvStorageMock = new Mock<IMaterialTechnicalValueStorage>();
            var mrpStorageMock = new Mock<IMaterialResponsiblePersonStorage>();

            mrpStorageMock
                .Setup(x => x.GetElement(It.Is<MaterialResponsiblePersonSearchModel>(m => m.FullName == "Петров П.П.")))
                .Returns((MaterialResponsiblePersonViewModel?)null);

            mrpStorageMock
                .Setup(x => x.Insert(It.Is<MaterialResponsiblePersonBindingModel>(m => m.FullName == "Петров П.П.")))
                .Returns(new MaterialResponsiblePersonViewModel
                {
                    Id = 11,
                    FullName = "Петров П.П."
                });

            mtvStorageMock
                .Setup(x => x.GetElement(It.Is<MaterialTechnicalValueSearchModel>(m => m.InventoryNumber == "INV-777")))
                .Returns((MaterialTechnicalValueViewModel?)null);

            var logic = new OneCImportLogic(oneCApiService, mtvStorageMock.Object, mrpStorageMock.Object);

            var result = await logic.ImportFromOneCAsync(new OneCImportBindingModel
            {
                Username = "demo",
                Password = "demo"
            });

            Assert.Equal(1, result.ImportedCount);
            Assert.Equal(1, result.CreatedCount);
            Assert.Equal(0, result.UpdatedCount);
            Assert.Equal(0, result.ErrorCount);

            mrpStorageMock.Verify(x => x.Insert(It.Is<MaterialResponsiblePersonBindingModel>(m =>
                m.FullName == "Петров П.П.")), Times.Once);

            mtvStorageMock.Verify(x => x.Insert(It.Is<MaterialTechnicalValueBindingModel>(m =>
                m.InventoryNumber == "INV-777" &&
                m.FullName == "Ноутбук Dell" &&
                m.Description == "101.34" &&
                m.Location == "Лаборатория 12" &&
                m.Quantity == 1 &&
                m.MaterialResponsiblePersonId == 11)), Times.Once);
        }

        [Fact]
        public async Task ImportFromOneCAsync_ShouldUpdateExistingEquipment_AndCollectErrors()
        {
            var responseJson = @"{
  ""ОС"": [
    {
      ""ОсновноеСредствоНаименование"": ""Проектор Epson"",
      ""ИнвентарныйНомер"": ""INV-500"",
      ""ЦМОНаименование"": ""Иванова И.И. - Аудитория 401"",
      ""СчетУчета"": ""101.36""
    },
    {
      ""ОсновноеСредствоНаименование"": """",
      ""ИнвентарныйНомер"": ""INV-ERROR"",
      ""ЦМОНаименование"": ""Некто - Склад"",
      ""СчетУчета"": ""101.99""
    }
  ]
}";

            var httpClient = CreateHttpClient(responseJson);
            var oneCApiService = new OneCApiService(httpClient);

            var mtvStorageMock = new Mock<IMaterialTechnicalValueStorage>();
            var mrpStorageMock = new Mock<IMaterialResponsiblePersonStorage>();

            mrpStorageMock
                .Setup(x => x.GetElement(It.Is<MaterialResponsiblePersonSearchModel>(m => m.FullName == "Иванова И.И.")))
                .Returns(new MaterialResponsiblePersonViewModel
                {
                    Id = 15,
                    FullName = "Иванова И.И."
                });

            mtvStorageMock
                .Setup(x => x.GetElement(It.Is<MaterialTechnicalValueSearchModel>(m => m.InventoryNumber == "INV-500")))
                .Returns(new MaterialTechnicalValueViewModel
                {
                    Id = 20,
                    InventoryNumber = "INV-500",
                    ClassroomId = 3,
                    FullName = "Старое имя",
                    Quantity = 1,
                    Description = "Старое описание",
                    Location = "Старое место",
                    MaterialResponsiblePersonId = 4
                });

            var logic = new OneCImportLogic(oneCApiService, mtvStorageMock.Object, mrpStorageMock.Object);

            var result = await logic.ImportFromOneCAsync(new OneCImportBindingModel
            {
                Username = "demo",
                Password = "demo"
            });

            Assert.Equal(2, result.ImportedCount);
            Assert.Equal(0, result.CreatedCount);
            Assert.Equal(1, result.UpdatedCount);
            Assert.Equal(1, result.ErrorCount);
            Assert.Single(result.Errors);
            Assert.Contains("Пустое наименование объекта", result.Errors[0]);

            mtvStorageMock.Verify(x => x.Update(It.Is<MaterialTechnicalValueBindingModel>(m =>
                m.Id == 20 &&
                m.InventoryNumber == "INV-500" &&
                m.FullName == "Проектор Epson" &&
                m.Description == "101.36" &&
                m.Location == "Аудитория 401" &&
                m.MaterialResponsiblePersonId == 15 &&
                m.Quantity == 1)), Times.Once);
        }

        private static HttpClient CreateHttpClient(string json)
        {
            var handler = new FakeHttpMessageHandler(json);
            return new HttpClient(handler);
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