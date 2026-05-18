using MolServiceBusinessLogic.Implements;
using MolServiceContracts.BindingModels;
using MolServiceContracts.SearchModels;
using MolServiceContracts.StorageContracts;
using MolServiceContracts.ViewModels;
using MolServiceDataModels.Enums;
using Moq;

namespace MolServiceTest.Logic
{
    public class MaterialTechnicalValueLogicTests
    {
        [Fact]
        public void Create_ShouldThrow_WhenInventoryNumberAlreadyExists()
        {
            var storageMock = new Mock<IMaterialTechnicalValueStorage>();

            storageMock
                .Setup(x => x.GetElement(It.Is<MaterialTechnicalValueSearchModel>(m =>
                    m.InventoryNumber == "INV-1" &&
                    m.SourceType == MaterialTechnicalValueSourceType.FixedAsset)))
                .Returns(new MaterialTechnicalValueViewModel
                {
                    Id = 1,
                    InventoryNumber = "INV-1",
                    SourceType = MaterialTechnicalValueSourceType.FixedAsset,
                    ExternalKey = string.Empty
                });

            var logic = new MaterialTechnicalValueLogic(storageMock.Object);

            var ex = Assert.Throws<InvalidOperationException>(() =>
                logic.Create(new MaterialTechnicalValueBindingModel
                {
                    InventoryNumber = "INV-1",
                    FullName = "Монитор",
                    Quantity = 1m,
                    MaterialResponsiblePersonId = 2,
                    SourceType = MaterialTechnicalValueSourceType.FixedAsset,
                    ExternalKey = string.Empty
                }));

            Assert.Equal(
                "Оборудование или материальный запас с таким номером уже существует",
                ex.Message);
        }

        [Fact]
        public void Create_ShouldCreateFixedAsset_WhenDataIsValid()
        {
            var storageMock = new Mock<IMaterialTechnicalValueStorage>();

            storageMock
                .Setup(x => x.GetElement(It.Is<MaterialTechnicalValueSearchModel>(m =>
                    m.InventoryNumber == "INV-2" &&
                    m.SourceType == MaterialTechnicalValueSourceType.FixedAsset)))
                .Returns((MaterialTechnicalValueViewModel?)null);

            storageMock
                .Setup(x => x.Insert(It.IsAny<MaterialTechnicalValueBindingModel>()))
                .Returns(new MaterialTechnicalValueViewModel
                {
                    Id = 1,
                    InventoryNumber = "INV-2",
                    FullName = "Системный блок",
                    Quantity = 1m,
                    Location = "Кафедра ИС",
                    MaterialResponsiblePersonId = 2,
                    SourceType = MaterialTechnicalValueSourceType.FixedAsset,
                    ExternalKey = string.Empty
                });

            var logic = new MaterialTechnicalValueLogic(storageMock.Object);

            var result = logic.Create(new MaterialTechnicalValueBindingModel
            {
                InventoryNumber = "INV-2",
                FullName = "Системный блок",
                Quantity = 1m,
                Description = "Описание",
                MaterialResponsiblePersonId = 2,
                SourceType = MaterialTechnicalValueSourceType.FixedAsset,
                ExternalKey = string.Empty
            });

            Assert.NotNull(result);
            Assert.Equal("INV-2", result!.InventoryNumber);
            Assert.Equal(MaterialTechnicalValueSourceType.FixedAsset, result.SourceType);

            storageMock.Verify(x => x.Insert(It.Is<MaterialTechnicalValueBindingModel>(m =>
                m.InventoryNumber == "INV-2" &&
                m.FullName == "Системный блок" &&
                m.Quantity == 1m &&
                m.Location == "Кафедра ИС" &&
                m.SourceType == MaterialTechnicalValueSourceType.FixedAsset &&
                m.ExternalKey == string.Empty)), Times.Once);
        }

        [Fact]
        public void Create_ShouldCreateMaterialStock_WhenSameInventoryNumberExistsForFixedAsset()
        {
            var storageMock = new Mock<IMaterialTechnicalValueStorage>();

            storageMock
                .Setup(x => x.GetElement(It.Is<MaterialTechnicalValueSearchModel>(m =>
                    m.InventoryNumber == "CODE-1" &&
                    m.SourceType == MaterialTechnicalValueSourceType.MaterialStock)))
                .Returns((MaterialTechnicalValueViewModel?)null);

            storageMock
                .Setup(x => x.Insert(It.IsAny<MaterialTechnicalValueBindingModel>()))
                .Returns(new MaterialTechnicalValueViewModel
                {
                    Id = 2,
                    InventoryNumber = "CODE-1",
                    FullName = "Картридж",
                    Quantity = 3m,
                    Location = "Кафедра ИС",
                    MaterialResponsiblePersonId = 5,
                    SourceType = MaterialTechnicalValueSourceType.MaterialStock,
                    ExternalKey = "matzp:code-1:иванов|и|и"
                });

            var logic = new MaterialTechnicalValueLogic(storageMock.Object);

            var result = logic.Create(new MaterialTechnicalValueBindingModel
            {
                InventoryNumber = "CODE-1",
                FullName = "Картридж",
                Quantity = 3m,
                MaterialResponsiblePersonId = 5,
                SourceType = MaterialTechnicalValueSourceType.MaterialStock,
                ExternalKey = "matzp:code-1:иванов|и|и"
            });

            Assert.NotNull(result);
            Assert.Equal(MaterialTechnicalValueSourceType.MaterialStock, result!.SourceType);

            storageMock.Verify(x => x.GetElement(It.Is<MaterialTechnicalValueSearchModel>(m =>
                m.InventoryNumber == "CODE-1" &&
                m.SourceType == MaterialTechnicalValueSourceType.MaterialStock)), Times.Once);

            storageMock.Verify(x => x.Insert(It.Is<MaterialTechnicalValueBindingModel>(m =>
                m.InventoryNumber == "CODE-1" &&
                m.FullName == "Картридж" &&
                m.Quantity == 3m &&
                m.Location == "Кафедра ИС" &&
                m.SourceType == MaterialTechnicalValueSourceType.MaterialStock &&
                m.ExternalKey == "matzp:code-1:иванов|и|и")), Times.Once);
        }

        [Fact]
        public void Create_ShouldThrow_WhenQuantityIsLessOrEqualZero()
        {
            var storageMock = new Mock<IMaterialTechnicalValueStorage>();
            var logic = new MaterialTechnicalValueLogic(storageMock.Object);

            var ex = Assert.Throws<ArgumentException>(() =>
                logic.Create(new MaterialTechnicalValueBindingModel
                {
                    InventoryNumber = "INV-3",
                    FullName = "Монитор",
                    Quantity = 0m,
                    MaterialResponsiblePersonId = 2,
                    SourceType = MaterialTechnicalValueSourceType.FixedAsset,
                    ExternalKey = string.Empty
                }));

            Assert.Equal("Количество должно быть больше нуля", ex.Message);
        }

        [Fact]
        public void Update_ShouldThrow_WhenEquipmentDoesNotExist()
        {
            var storageMock = new Mock<IMaterialTechnicalValueStorage>();

            storageMock
                .Setup(x => x.GetElement(It.Is<MaterialTechnicalValueSearchModel>(m => m.Id == 10)))
                .Returns((MaterialTechnicalValueViewModel?)null);

            var logic = new MaterialTechnicalValueLogic(storageMock.Object);

            var ex = Assert.Throws<InvalidOperationException>(() =>
                logic.Update(new MaterialTechnicalValueBindingModel
                {
                    Id = 10,
                    InventoryNumber = "INV-10",
                    FullName = "Принтер",
                    Quantity = 1m,
                    MaterialResponsiblePersonId = 3,
                    SourceType = MaterialTechnicalValueSourceType.FixedAsset,
                    ExternalKey = string.Empty
                }));

            Assert.Equal("Оборудование не найдено", ex.Message);
        }

        [Fact]
        public void Update_ShouldKeepSourceTypeAndExternalKey_WhenUpdatingExistingItem()
        {
            var storageMock = new Mock<IMaterialTechnicalValueStorage>();

            storageMock
                .Setup(x => x.GetElement(It.Is<MaterialTechnicalValueSearchModel>(m => m.Id == 15)))
                .Returns(new MaterialTechnicalValueViewModel
                {
                    Id = 15,
                    InventoryNumber = "INV-15",
                    FullName = "Старое название",
                    Quantity = 1m,
                    Location = "Кафедра ИС",
                    MaterialResponsiblePersonId = 3,
                    SourceType = MaterialTechnicalValueSourceType.FixedAsset,
                    ExternalKey = "inventoryNumbers:inv-15"
                });

            storageMock
                .Setup(x => x.Update(It.IsAny<MaterialTechnicalValueBindingModel>()))
                .Returns(new MaterialTechnicalValueViewModel
                {
                    Id = 15,
                    InventoryNumber = "INV-15",
                    FullName = "Новое название",
                    Quantity = 1m,
                    Location = "Кафедра ИС",
                    MaterialResponsiblePersonId = 3,
                    SourceType = MaterialTechnicalValueSourceType.FixedAsset,
                    ExternalKey = "inventoryNumbers:inv-15"
                });

            var logic = new MaterialTechnicalValueLogic(storageMock.Object);

            var result = logic.Update(new MaterialTechnicalValueBindingModel
            {
                Id = 15,
                InventoryNumber = "INV-15",
                FullName = "Новое название",
                Quantity = 1m,
                MaterialResponsiblePersonId = 3,
                SourceType = MaterialTechnicalValueSourceType.FixedAsset,
                ExternalKey = "inventoryNumbers:inv-15"
            });

            Assert.NotNull(result);

            storageMock.Verify(x => x.Update(It.Is<MaterialTechnicalValueBindingModel>(m =>
                m.Id == 15 &&
                m.InventoryNumber == "INV-15" &&
                m.FullName == "Новое название" &&
                m.Location == "Кафедра ИС" &&
                m.SourceType == MaterialTechnicalValueSourceType.FixedAsset &&
                m.ExternalKey == "inventoryNumbers:inv-15")), Times.Once);
        }

        [Fact]
        public void ReadPagedList_ShouldReturnPagedResult()
        {
            var storageMock = new Mock<IMaterialTechnicalValueStorage>();

            var expectedResult = new PagedResultViewModel<MaterialTechnicalValueViewModel>
            {
                Items = new List<MaterialTechnicalValueViewModel>
                {
                    new MaterialTechnicalValueViewModel
                    {
                        Id = 1,
                        InventoryNumber = "INV-1",
                        FullName = "Монитор",
                        Quantity = 1m,
                        SourceType = MaterialTechnicalValueSourceType.FixedAsset,
                        ExternalKey = string.Empty
                    }
                },
                Page = 1,
                PageSize = 20,
                TotalCount = 1
            };

            storageMock
                .Setup(x => x.GetPagedList(It.Is<MaterialTechnicalValueSearchModel>(m =>
                    m.Page == 1 &&
                    m.PageSize == 20 &&
                    m.SourceType == MaterialTechnicalValueSourceType.FixedAsset)))
                .Returns(expectedResult);

            var logic = new MaterialTechnicalValueLogic(storageMock.Object);

            var result = logic.ReadPagedList(new MaterialTechnicalValueSearchModel
            {
                Page = 1,
                PageSize = 20,
                SourceType = MaterialTechnicalValueSourceType.FixedAsset
            });

            Assert.NotNull(result);
            Assert.Single(result.Items);
            Assert.Equal(1, result.TotalCount);
            Assert.Equal(1, result.Page);
            Assert.Equal(20, result.PageSize);

            storageMock.Verify(x => x.GetPagedList(It.Is<MaterialTechnicalValueSearchModel>(m =>
                m.Page == 1 &&
                m.PageSize == 20 &&
                m.SourceType == MaterialTechnicalValueSourceType.FixedAsset)), Times.Once);
        }
    }
}