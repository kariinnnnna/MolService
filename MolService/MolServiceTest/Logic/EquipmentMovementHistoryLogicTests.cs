using MolServiceBusinessLogic.Implements;
using MolServiceContracts.BindingModels;
using MolServiceContracts.SearchModels;
using MolServiceContracts.StorageContracts;
using MolServiceContracts.ViewModels;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MolServiceTest.Logic
{
    public class EquipmentMovementHistoryLogicTests
    {
        [Fact]
        public void Create_ShouldDecreaseEquipmentQuantity_AfterWriteOff()
        {
            var historyStorageMock = new Mock<IEquipmentMovementHistoryStorage>();
            var mtvStorageMock = new Mock<IMaterialTechnicalValueStorage>();

            var equipment = new MaterialTechnicalValueViewModel
            {
                Id = 10,
                InventoryNumber = "INV-001",
                ClassroomId = 2,
                FullName = "Ноутбук Lenovo",
                Quantity = 7,
                Description = "Учебный",
                Location = "Аудитория 305",
                MaterialResponsiblePersonId = 4
            };

            var model = new EquipmentMovementHistoryBindingModel
            {
                MaterialTechnicalValueId = 10,
                MoveDate = new DateTime(2026, 4, 1),
                Reason = "Списание",
                Quantity = 2
            };

            historyStorageMock
                .Setup(x => x.Insert(model))
                .Returns(new EquipmentMovementHistoryViewModel
                {
                    Id = 1,
                    MaterialTechnicalValueId = 10,
                    MoveDate = model.MoveDate,
                    Reason = "Списание",
                    Quantity = 2
                });

            mtvStorageMock
                .Setup(x => x.GetElement(It.Is<MaterialTechnicalValueSearchModel>(m => m.Id == 10)))
                .Returns(equipment);

            var logic = new EquipmentMovementHistoryLogic(historyStorageMock.Object, mtvStorageMock.Object);

            var result = logic.Create(model);

            Assert.NotNull(result);
            mtvStorageMock.Verify(x => x.Update(It.Is<MaterialTechnicalValueBindingModel>(m =>
                m.Id == 10 &&
                m.InventoryNumber == "INV-001" &&
                m.Quantity == 5 &&
                m.FullName == "Ноутбук Lenovo" &&
                m.Location == "Аудитория 305" &&
                m.MaterialResponsiblePersonId == 4)), Times.Once);
        }

        [Fact]
        public void Create_ShouldThrow_WhenWriteOffQuantityGreaterThanAvailable()
        {
            var historyStorageMock = new Mock<IEquipmentMovementHistoryStorage>();
            var mtvStorageMock = new Mock<IMaterialTechnicalValueStorage>();

            mtvStorageMock
                .Setup(x => x.GetElement(It.Is<MaterialTechnicalValueSearchModel>(m => m.Id == 10)))
                .Returns(new MaterialTechnicalValueViewModel
                {
                    Id = 10,
                    InventoryNumber = "INV-001",
                    FullName = "Проектор",
                    Quantity = 1,
                    MaterialResponsiblePersonId = 2
                });

            var logic = new EquipmentMovementHistoryLogic(historyStorageMock.Object, mtvStorageMock.Object);

            var model = new EquipmentMovementHistoryBindingModel
            {
                MaterialTechnicalValueId = 10,
                MoveDate = DateTime.Today,
                Reason = "Поломка",
                Quantity = 3
            };

            var ex = Assert.Throws<ArgumentException>(() => logic.Create(model));

            Assert.Equal("Нельзя списать больше, чем есть в наличии", ex.Message);
            historyStorageMock.Verify(x => x.Insert(It.IsAny<EquipmentMovementHistoryBindingModel>()), Times.Never);
            mtvStorageMock.Verify(x => x.Update(It.IsAny<MaterialTechnicalValueBindingModel>()), Times.Never);
        }

        [Fact]
        public void Update_ShouldThrow_WhenHistoryRecordDoesNotExist()
        {
            var historyStorageMock = new Mock<IEquipmentMovementHistoryStorage>();
            var mtvStorageMock = new Mock<IMaterialTechnicalValueStorage>();

            historyStorageMock
                .Setup(x => x.GetElement(It.Is<EquipmentMovementHistorySearchModel>(m => m.Id == 99)))
                .Returns((EquipmentMovementHistoryViewModel?)null);

            var logic = new EquipmentMovementHistoryLogic(historyStorageMock.Object, mtvStorageMock.Object);

            var model = new EquipmentMovementHistoryBindingModel
            {
                Id = 99,
                MaterialTechnicalValueId = 10,
                MoveDate = DateTime.Today,
                Reason = "Поломка",
                Quantity = 1
            };

            var ex = Assert.Throws<InvalidOperationException>(() => logic.Update(model));

            Assert.Equal("Запись списания не найдена", ex.Message);
        }
    }
}
