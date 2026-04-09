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
    public class ClassroomLogicTests
    {
        [Fact]
        public void Create_ShouldThrowArgumentException_WhenNumberIsEmpty()
        {
            var storageMock = new Mock<IClassroomStorage>();
            var logic = new ClassroomLogic(storageMock.Object);

            var model = new ClassroomBindingModel
            {
                CoreSystemId = 10,
                Number = "   "
            };

            var ex = Assert.Throws<ArgumentException>(() => logic.Create(model));

            Assert.Equal("Не указан номер аудитории", ex.Message);
            storageMock.Verify(x => x.Insert(It.IsAny<ClassroomBindingModel>()), Times.Never);
        }

        [Fact]
        public void Create_ShouldThrowInvalidOperationException_WhenCoreSystemIdAlreadyExists()
        {
            var storageMock = new Mock<IClassroomStorage>();

            storageMock
                .Setup(x => x.GetElement(It.Is<ClassroomSearchModel>(m => m.CoreSystemId == 10)))
                .Returns(new ClassroomViewModel
                {
                    Id = 1,
                    CoreSystemId = 10,
                    Number = "101"
                });

            var logic = new ClassroomLogic(storageMock.Object);

            var model = new ClassroomBindingModel
            {
                CoreSystemId = 10,
                Number = "102"
            };

            var ex = Assert.Throws<InvalidOperationException>(() => logic.Create(model));

            Assert.Equal("Аудитория с таким CoreSystemId уже существует", ex.Message);
            storageMock.Verify(x => x.Insert(It.IsAny<ClassroomBindingModel>()), Times.Never);
        }

        [Fact]
        public void Create_ShouldInsertAndReturnCreatedItem_WhenModelIsValid()
        {
            var storageMock = new Mock<IClassroomStorage>();

            var model = new ClassroomBindingModel
            {
                CoreSystemId = 25,
                Number = "305"
            };

            var expected = new ClassroomViewModel
            {
                Id = 7,
                CoreSystemId = 25,
                Number = "305"
            };

            storageMock
                .Setup(x => x.GetElement(It.Is<ClassroomSearchModel>(m => m.CoreSystemId == 25)))
                .Returns((ClassroomViewModel?)null);

            storageMock
                .Setup(x => x.Insert(model))
                .Returns(expected);

            var logic = new ClassroomLogic(storageMock.Object);

            var result = logic.Create(model);

            Assert.NotNull(result);
            Assert.Equal(expected.Id, result!.Id);
            Assert.Equal(expected.Number, result.Number);

            storageMock.Verify(x => x.Insert(model), Times.Once);
        }

        [Fact]
        public void Update_ShouldAllowSameCoreSystemId_ForSameEntity()
        {
            var storageMock = new Mock<IClassroomStorage>();

            var model = new ClassroomBindingModel
            {
                Id = 5,
                CoreSystemId = 100,
                Number = "402"
            };

            var existing = new ClassroomViewModel
            {
                Id = 5,
                CoreSystemId = 100,
                Number = "401"
            };

            var updated = new ClassroomViewModel
            {
                Id = 5,
                CoreSystemId = 100,
                Number = "402"
            };

            storageMock
                .Setup(x => x.GetElement(It.Is<ClassroomSearchModel>(m => m.Id == 5)))
                .Returns(existing);

            storageMock
                .Setup(x => x.GetElement(It.Is<ClassroomSearchModel>(m => m.CoreSystemId == 100)))
                .Returns(existing);

            storageMock
                .Setup(x => x.Update(model))
                .Returns(updated);

            var logic = new ClassroomLogic(storageMock.Object);

            var result = logic.Update(model);

            Assert.NotNull(result);
            Assert.Equal("402", result!.Number);

            storageMock.Verify(x => x.Update(model), Times.Once);
        }

        [Fact]
        public void Delete_ShouldReturnTrue_WhenEntityExistsAndStorageDeletesIt()
        {
            var storageMock = new Mock<IClassroomStorage>();

            var model = new ClassroomBindingModel { Id = 3 };

            var existing = new ClassroomViewModel
            {
                Id = 3,
                CoreSystemId = 30,
                Number = "201"
            };

            storageMock
                .Setup(x => x.GetElement(It.Is<ClassroomSearchModel>(m => m.Id == 3)))
                .Returns(existing);

            storageMock
                .Setup(x => x.Delete(It.Is<ClassroomBindingModel>(m => m.Id == 3)))
                .Returns(existing);

            var logic = new ClassroomLogic(storageMock.Object);

            var result = logic.Delete(model);

            Assert.True(result);
            storageMock.Verify(x => x.Delete(It.Is<ClassroomBindingModel>(m => m.Id == 3)), Times.Once);
        }
    }
}
