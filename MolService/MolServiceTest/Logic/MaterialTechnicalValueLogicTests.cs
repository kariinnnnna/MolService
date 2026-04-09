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
    public class MaterialTechnicalValueLogicTests
    {
        [Fact]
        public void Create_ShouldThrow_WhenInventoryNumberAlreadyExists()
        {
            var storageMock = new Mock<IMaterialTechnicalValueStorage>();

            storageMock
                .Setup(x => x.GetElement(It.Is<MaterialTechnicalValueSearchModel>(m => m.InventoryNumber == "INV-1")))
                .Returns(new MaterialTechnicalValueViewModel { Id = 1, InventoryNumber = "INV-1" });

            var logic = new MaterialTechnicalValueLogic(storageMock.Object);

            var ex = Assert.Throws<InvalidOperationException>(() => logic.Create(new MaterialTechnicalValueBindingModel
            {
                InventoryNumber = "INV-1",
                FullName = "Монитор",
                Quantity = 1,
                MaterialResponsiblePersonId = 2
            }));

            Assert.Equal("Оборудование с таким инвентарным номером уже существует", ex.Message);
        }

        [Fact]
        public void Update_ShouldThrow_WhenEquipmentDoesNotExist()
        {
            var storageMock = new Mock<IMaterialTechnicalValueStorage>();

            storageMock
                .Setup(x => x.GetElement(It.Is<MaterialTechnicalValueSearchModel>(m => m.Id == 10)))
                .Returns((MaterialTechnicalValueViewModel?)null);

            var logic = new MaterialTechnicalValueLogic(storageMock.Object);

            var ex = Assert.Throws<InvalidOperationException>(() => logic.Update(new MaterialTechnicalValueBindingModel
            {
                Id = 10,
                InventoryNumber = "INV-10",
                FullName = "Принтер",
                Quantity = 1,
                MaterialResponsiblePersonId = 3
            }));

            Assert.Equal("Оборудование не найдено", ex.Message);
        }
    }
}
