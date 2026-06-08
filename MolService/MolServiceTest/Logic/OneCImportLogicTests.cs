//using MolServiceBusinessLogic.Helpers;
//using MolServiceBusinessLogic.Implements;
//using MolServiceContracts.BindingModels;
//using MolServiceContracts.SearchModels;
//using MolServiceContracts.StorageContracts;
//using MolServiceContracts.ViewModels;
//using MolServiceDataModels.Enums;
//using Moq;
//using System.Net;
//using System.Text;

//namespace MolServiceTest.Logic
//{
//    public class OneCImportLogicTests
//    {
//        [Fact]
//        public async Task ImportFromOneCAsync_ShouldCreateMolAndEquipment_ForNewInventoryItem()
//        {
//            var responseJson = @"{
//  ""ОС"": [
//    {
//      ""ОсновноеСредствоНаименование"": ""Ноутбук Dell"",
//      ""ИнвентарныйНомер"": ""INV-777"",
//      ""ЦМОНаименование"": ""Петров П.П. - Кафедра ИС"",
//      ""СчетУчета"": ""101.34""
//    }
//  ],
//  ""МатериальныеЗапасы"": []
//}";

//            var httpClient = CreateHttpClient(responseJson);
//            var oneCApiService = new OneCApiService(httpClient);

//            var mtvStorageMock = new Mock<IMaterialTechnicalValueStorage>();
//            var mrpStorageMock = new Mock<IMaterialResponsiblePersonStorage>();

//            mrpStorageMock
//                .Setup(x => x.GetElement(It.Is<MaterialResponsiblePersonSearchModel>(
//                    m => m.FullName == "Петров П.П.")))
//                .Returns((MaterialResponsiblePersonViewModel?)null);

//            mrpStorageMock
//                .Setup(x => x.Insert(It.Is<MaterialResponsiblePersonBindingModel>(
//                    m => m.FullName == "Петров П.П.")))
//                .Returns(new MaterialResponsiblePersonViewModel
//                {
//                    Id = 11,
//                    FullName = "Петров П.П."
//                });

//            mtvStorageMock
//                .Setup(x => x.GetElement(It.Is<MaterialTechnicalValueSearchModel>(
//                    m => m.ExternalKey == "inventoryNumbers:inv-777")))
//                .Returns((MaterialTechnicalValueViewModel?)null);

//            mtvStorageMock
//                .Setup(x => x.GetElement(It.Is<MaterialTechnicalValueSearchModel>(
//                    m => m.InventoryNumber == "INV-777" &&
//                         m.SourceType == MaterialTechnicalValueSourceType.FixedAsset)))
//                .Returns((MaterialTechnicalValueViewModel?)null);

//            var logic = new OneCImportLogic(
//                oneCApiService,
//                mtvStorageMock.Object,
//                mrpStorageMock.Object);

//            var result = await logic.ImportFromOneCAsync(new OneCImportBindingModel
//            {
//                Username = "demo",
//                Password = "demo"
//            });

//            Assert.Equal(1, result.ImportedCount);
//            Assert.Equal(1, result.CreatedCount);
//            Assert.Equal(0, result.UpdatedCount);
//            Assert.Equal(0, result.ErrorCount);

//            mrpStorageMock.Verify(x => x.Insert(It.Is<MaterialResponsiblePersonBindingModel>(m =>
//                m.FullName == "Петров П.П.")), Times.Once);

//            mtvStorageMock.Verify(x => x.Insert(It.Is<MaterialTechnicalValueBindingModel>(m =>
//                m.InventoryNumber == "INV-777" &&
//                m.FullName == "Ноутбук Dell" &&
//                m.Description == "101.34" &&
//                m.Location == "Кафедра ИС" &&
//                m.Quantity == 1m &&
//                m.MaterialResponsiblePersonId == 11 &&
//                m.SourceType == MaterialTechnicalValueSourceType.FixedAsset &&
//                m.ExternalKey == "inventoryNumbers:inv-777")), Times.Once);
//        }

//        [Fact]
//        public async Task ImportFromOneCAsync_ShouldUpdateExistingEquipment_AndCollectErrors()
//        {
//            var responseJson = @"{
//  ""ОС"": [
//    {
//      ""ОсновноеСредствоНаименование"": ""Проектор Epson"",
//      ""ИнвентарныйНомер"": ""INV-500"",
//      ""ЦМОНаименование"": ""Иванова И.И. - Кафедра ИС"",
//      ""СчетУчета"": ""101.36""
//    },
//    {
//      ""ОсновноеСредствоНаименование"": """",
//      ""ИнвентарныйНомер"": ""INV-ERROR"",
//      ""ЦМОНаименование"": ""Некто - Кафедра ИС"",
//      ""СчетУчета"": ""101.99""
//    }
//  ],
//  ""МатериальныеЗапасы"": []
//}";

//            var httpClient = CreateHttpClient(responseJson);
//            var oneCApiService = new OneCApiService(httpClient);

//            var mtvStorageMock = new Mock<IMaterialTechnicalValueStorage>();
//            var mrpStorageMock = new Mock<IMaterialResponsiblePersonStorage>();

//            mrpStorageMock
//                .Setup(x => x.GetElement(It.Is<MaterialResponsiblePersonSearchModel>(
//                    m => m.FullName == "Иванова И.И.")))
//                .Returns(new MaterialResponsiblePersonViewModel
//                {
//                    Id = 15,
//                    FullName = "Иванова И.И."
//                });

//            mtvStorageMock
//                .Setup(x => x.GetElement(It.Is<MaterialTechnicalValueSearchModel>(
//                    m => m.ExternalKey == "inventoryNumbers:inv-500")))
//                .Returns((MaterialTechnicalValueViewModel?)null);

//            mtvStorageMock
//                .Setup(x => x.GetElement(It.Is<MaterialTechnicalValueSearchModel>(
//                    m => m.InventoryNumber == "INV-500" &&
//                         m.SourceType == MaterialTechnicalValueSourceType.FixedAsset)))
//                .Returns(new MaterialTechnicalValueViewModel
//                {
//                    Id = 20,
//                    InventoryNumber = "INV-500",
//                    ClassroomId = 3,
//                    FullName = "Старое имя",
//                    Quantity = 1m,
//                    Description = "Старое описание",
//                    Location = "Старое место",
//                    MaterialResponsiblePersonId = 4,
//                    SourceType = MaterialTechnicalValueSourceType.FixedAsset,
//                    ExternalKey = "inventoryNumbers:inv-500"
//                });

//            var logic = new OneCImportLogic(
//                oneCApiService,
//                mtvStorageMock.Object,
//                mrpStorageMock.Object);

//            var result = await logic.ImportFromOneCAsync(new OneCImportBindingModel
//            {
//                Username = "demo",
//                Password = "demo"
//            });

//            Assert.Equal(2, result.ImportedCount);
//            Assert.Equal(0, result.CreatedCount);
//            Assert.Equal(1, result.UpdatedCount);
//            Assert.Equal(1, result.ErrorCount);
//            Assert.Single(result.Errors);
//            Assert.Contains("Пустое наименование объекта", result.Errors[0]);

//            mtvStorageMock.Verify(x => x.Update(It.Is<MaterialTechnicalValueBindingModel>(m =>
//                m.Id == 20 &&
//                m.InventoryNumber == "INV-500" &&
//                m.FullName == "Проектор Epson" &&
//                m.Description == "101.36" &&
//                m.Location == "Кафедра ИС" &&
//                m.MaterialResponsiblePersonId == 15 &&
//                m.Quantity == 1m &&
//                m.SourceType == MaterialTechnicalValueSourceType.FixedAsset &&
//                m.ExternalKey == "inventoryNumbers:inv-500")), Times.Once);
//        }

//        private static HttpClient CreateHttpClient(string json)
//        {
//            var handler = new FakeHttpMessageHandler(json);
//            return new HttpClient(handler);
//        }

//        private sealed class FakeHttpMessageHandler : HttpMessageHandler
//        {
//            private readonly string _json;

//            public FakeHttpMessageHandler(string json)
//            {
//                _json = json;
//            }

//            protected override Task<HttpResponseMessage> SendAsync(
//                HttpRequestMessage request,
//                CancellationToken cancellationToken)
//            {
//                var response = new HttpResponseMessage(HttpStatusCode.OK)
//                {
//                    Content = new StringContent(_json, Encoding.UTF8, "application/json")
//                };

//                return Task.FromResult(response);
//            }
//        }
//        [Fact]
//        public async Task ImportFromOneCAsync_ShouldCreateMaterialStock_WhenMolExistsInDepartment()
//        {
//            var responseJson = @"{
//  ""ОС"": [
//    {
//      ""ОсновноеСредствоНаименование"": ""Компьютер"",
//      ""ИнвентарныйНомер"": ""INV-100"",
//      ""ЦМОНаименование"": ""Иванов И.И. - Кафедра ИС"",
//      ""СчетУчета"": ""101.34""
//    }
//  ],
//  ""МатериальныеЗапасы"": [
//    {
//      ""Номенклатура"": ""Картридж"",
//      ""НоменклатураКод"": ""MAT-001"",
//      ""Количество"": 3,
//      ""МОЛ"": ""Иванов Иван Иванович""
//    }
//  ]
//}";

//            var httpClient = CreateHttpClient(responseJson);
//            var oneCApiService = new OneCApiService(httpClient);

//            var mtvStorageMock = new Mock<IMaterialTechnicalValueStorage>();
//            var mrpStorageMock = new Mock<IMaterialResponsiblePersonStorage>();

//            mrpStorageMock
//                .Setup(x => x.GetElement(It.Is<MaterialResponsiblePersonSearchModel>(
//                    m => m.FullName == "Иванов И.И.")))
//                .Returns(new MaterialResponsiblePersonViewModel
//                {
//                    Id = 5,
//                    FullName = "Иванов И.И."
//                });

//            mtvStorageMock
//                .Setup(x => x.GetElement(It.IsAny<MaterialTechnicalValueSearchModel>()))
//                .Returns((MaterialTechnicalValueViewModel?)null);

//            var logic = new OneCImportLogic(
//                oneCApiService,
//                mtvStorageMock.Object,
//                mrpStorageMock.Object);

//            var result = await logic.ImportFromOneCAsync(new OneCImportBindingModel
//            {
//                Username = "demo",
//                Password = "demo"
//            });

//            Assert.Equal(2, result.ImportedCount);
//            Assert.Equal(2, result.CreatedCount);
//            Assert.Equal(0, result.UpdatedCount);
//            Assert.Equal(0, result.ErrorCount);

//            mtvStorageMock.Verify(x => x.Insert(It.Is<MaterialTechnicalValueBindingModel>(m =>
//                m.InventoryNumber == "MAT-001" &&
//                m.FullName == "Картридж" &&
//                m.Quantity == 3m &&
//                m.Location == "Кафедра ИС" &&
//                m.MaterialResponsiblePersonId == 5 &&
//                m.SourceType == MaterialTechnicalValueSourceType.MaterialStock &&
//                m.ExternalKey == "matzp:mat-001:иванов|и|и")), Times.Once);
//        }
//        [Fact]
//        public async Task ImportFromOneCAsync_ShouldNotCreateMaterialStock_WhenMolIsNotFromDepartment()
//        {
//            var responseJson = @"{
//  ""ОС"": [
//    {
//      ""ОсновноеСредствоНаименование"": ""Компьютер"",
//      ""ИнвентарныйНомер"": ""INV-100"",
//      ""ЦМОНаименование"": ""Иванов И.И. - Кафедра ИС"",
//      ""СчетУчета"": ""101.34""
//    }
//  ],
//  ""МатериальныеЗапасы"": [
//    {
//      ""Номенклатура"": ""Картридж"",
//      ""НоменклатураКод"": ""MAT-001"",
//      ""Количество"": 3,
//      ""МОЛ"": ""Петров Петр Петрович""
//    }
//  ]
//}";

//            var httpClient = CreateHttpClient(responseJson);
//            var oneCApiService = new OneCApiService(httpClient);

//            var mtvStorageMock = new Mock<IMaterialTechnicalValueStorage>();
//            var mrpStorageMock = new Mock<IMaterialResponsiblePersonStorage>();

//            mrpStorageMock
//                .Setup(x => x.GetElement(It.Is<MaterialResponsiblePersonSearchModel>(
//                    m => m.FullName == "Иванов И.И.")))
//                .Returns(new MaterialResponsiblePersonViewModel
//                {
//                    Id = 5,
//                    FullName = "Иванов И.И."
//                });

//            mtvStorageMock
//                .Setup(x => x.GetElement(It.IsAny<MaterialTechnicalValueSearchModel>()))
//                .Returns((MaterialTechnicalValueViewModel?)null);

//            var logic = new OneCImportLogic(
//                oneCApiService,
//                mtvStorageMock.Object,
//                mrpStorageMock.Object);

//            var result = await logic.ImportFromOneCAsync(new OneCImportBindingModel
//            {
//                Username = "demo",
//                Password = "demo"
//            });

//            Assert.Equal(1, result.ImportedCount);
//            Assert.Equal(1, result.CreatedCount);
//            Assert.Equal(0, result.UpdatedCount);
//            Assert.Equal(0, result.ErrorCount);

//            mtvStorageMock.Verify(x => x.Insert(It.Is<MaterialTechnicalValueBindingModel>(m =>
//                m.SourceType == MaterialTechnicalValueSourceType.MaterialStock)), Times.Never);
//        }
//    }
//}