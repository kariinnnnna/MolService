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
    public class SoftwareRecordLogicTests
    {
        [Fact]
        public void Create_ShouldThrow_WhenSoftwareAlreadyBoundToEquipment()
        {
            var storageMock = new Mock<ISoftwareRecordStorage>();
            var materialTechnicalValueStorageMock = new Mock<IMaterialTechnicalValueStorage>();

            storageMock
                .Setup(x => x.GetElement(It.Is<SoftwareRecordSearchModel>(m =>
                    m.MaterialTechnicalValueId == 10 && m.SoftwareId == 3)))
                .Returns(new SoftwareRecordViewModel
                {
                    Id = 7,
                    MaterialTechnicalValueId = 10,
                    SoftwareId = 3,
                    SetupDescription = "Уже установлено",
                    ClaimNumber = "CL-1"
                });

            var logic = new SoftwareRecordLogic(
                storageMock.Object,
                materialTechnicalValueStorageMock.Object);

            var model = new SoftwareRecordBindingModel
            {
                MaterialTechnicalValueId = 10,
                SoftwareId = 3,
                SetupDescription = "Повторная установка",
                ClaimNumber = "CL-2"
            };

            var ex = Assert.Throws<InvalidOperationException>(() => logic.Create(model));

            Assert.Equal("Это программное обеспечение уже привязано к данному оборудованию", ex.Message);
            storageMock.Verify(x => x.Insert(It.IsAny<SoftwareRecordBindingModel>()), Times.Never);
        }

        [Fact]
        public void Update_ShouldAllowSamePair_ForSameRecord()
        {
            var storageMock = new Mock<ISoftwareRecordStorage>();
            var materialTechnicalValueStorageMock = new Mock<IMaterialTechnicalValueStorage>();

            var existing = new SoftwareRecordViewModel
            {
                Id = 5,
                MaterialTechnicalValueId = 10,
                SoftwareId = 3,
                SetupDescription = "Старое описание",
                ClaimNumber = "A-1"
            };

            var model = new SoftwareRecordBindingModel
            {
                Id = 5,
                MaterialTechnicalValueId = 10,
                SoftwareId = 3,
                SetupDescription = "Новое описание",
                ClaimNumber = "A-2"
            };

            storageMock
                .Setup(x => x.GetElement(It.Is<SoftwareRecordSearchModel>(m => m.Id == 5)))
                .Returns(existing);

            storageMock
                .Setup(x => x.GetElement(It.Is<SoftwareRecordSearchModel>(m =>
                    m.MaterialTechnicalValueId == 10 && m.SoftwareId == 3)))
                .Returns(existing);

            storageMock
                .Setup(x => x.Update(model))
                .Returns(new SoftwareRecordViewModel
                {
                    Id = 5,
                    MaterialTechnicalValueId = 10,
                    SoftwareId = 3,
                    SetupDescription = "Новое описание",
                    ClaimNumber = "A-2"
                });

            var logic = new SoftwareRecordLogic(
                storageMock.Object,
                materialTechnicalValueStorageMock.Object);

            var result = logic.Update(model);

            Assert.NotNull(result);
            Assert.Equal("Новое описание", result!.SetupDescription);
            storageMock.Verify(x => x.Update(model), Times.Once);
        }

        [Fact]
        public void Delete_ShouldThrow_WhenRecordDoesNotExist()
        {
            var storageMock = new Mock<ISoftwareRecordStorage>();
            var materialTechnicalValueStorageMock = new Mock<IMaterialTechnicalValueStorage>();

            storageMock
                .Setup(x => x.GetElement(It.Is<SoftwareRecordSearchModel>(m => m.Id == 404)))
                .Returns((SoftwareRecordViewModel?)null);

            var logic = new SoftwareRecordLogic(
                storageMock.Object,
                materialTechnicalValueStorageMock.Object);

            var ex = Assert.Throws<InvalidOperationException>(() =>
                logic.Delete(new SoftwareRecordBindingModel { Id = 404 }));

            Assert.Equal("Запись о привязке ПО не найдена", ex.Message);
        }
    }
}
