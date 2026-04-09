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
    public class MaterialResponsiblePersonLogicTests
    {
        [Fact]
        public void Create_ShouldTrimFields_AndInsertNormalizedModel()
        {
            var storageMock = new Mock<IMaterialResponsiblePersonStorage>();

            storageMock
                .Setup(x => x.GetElement(It.IsAny<MaterialResponsiblePersonSearchModel>()))
                .Returns((MaterialResponsiblePersonViewModel?)null);

            storageMock
                .Setup(x => x.Insert(It.Is<MaterialResponsiblePersonBindingModel>(m =>
                    m.FullName == "Иванова И.И." &&
                    m.Phone == "+79990001122" &&
                    m.Email == "ivanova@test.ru")))
                .Returns(new MaterialResponsiblePersonViewModel
                {
                    Id = 1,
                    FullName = "Иванова И.И.",
                    Phone = "+79990001122",
                    Email = "ivanova@test.ru",
                    Position = "Лаборант"
                });

            var logic = new MaterialResponsiblePersonLogic(storageMock.Object);

            var model = new MaterialResponsiblePersonBindingModel
            {
                FullName = "  Иванова И.И.  ",
                Position = "Лаборант",
                Phone = "  +79990001122  ",
                Email = "  ivanova@test.ru  "
            };

            var result = logic.Create(model);

            Assert.NotNull(result);
            Assert.Equal("Иванова И.И.", model.FullName);
            Assert.Equal("+79990001122", model.Phone);
            Assert.Equal("ivanova@test.ru", model.Email);

            storageMock.Verify(x => x.Insert(It.IsAny<MaterialResponsiblePersonBindingModel>()), Times.Once);
        }

        [Fact]
        public void Create_ShouldThrow_WhenPhoneBelongsToAnotherPerson()
        {
            var storageMock = new Mock<IMaterialResponsiblePersonStorage>();

            storageMock
                .Setup(x => x.GetElement(It.Is<MaterialResponsiblePersonSearchModel>(m => m.FullName == "Петров П.П.")))
                .Returns((MaterialResponsiblePersonViewModel?)null);

            storageMock
                .Setup(x => x.GetElement(It.Is<MaterialResponsiblePersonSearchModel>(m => m.Phone == "+79990001122")))
                .Returns(new MaterialResponsiblePersonViewModel
                {
                    Id = 5,
                    FullName = "Другой человек",
                    Phone = "+79990001122"
                });

            var logic = new MaterialResponsiblePersonLogic(storageMock.Object);

            var model = new MaterialResponsiblePersonBindingModel
            {
                FullName = "Петров П.П.",
                Phone = "+79990001122",
                Email = "petrov@test.ru"
            };

            var ex = Assert.Throws<InvalidOperationException>(() => logic.Create(model));

            Assert.Equal("Ответственное лицо с таким телефоном уже существует", ex.Message);
            storageMock.Verify(x => x.Insert(It.IsAny<MaterialResponsiblePersonBindingModel>()), Times.Never);
        }

        [Fact]
        public void Update_ShouldAllowSameEmail_ForSameEntity()
        {
            var storageMock = new Mock<IMaterialResponsiblePersonStorage>();

            var model = new MaterialResponsiblePersonBindingModel
            {
                Id = 3,
                FullName = "Сидорова А.А.",
                Position = "Преподаватель",
                Phone = "+79995554433",
                Email = "same@test.ru"
            };

            var existing = new MaterialResponsiblePersonViewModel
            {
                Id = 3,
                FullName = "Сидорова А.А.",
                Position = "Старший преподаватель",
                Phone = "+79995554433",
                Email = "same@test.ru"
            };

            storageMock
                .Setup(x => x.GetElement(It.Is<MaterialResponsiblePersonSearchModel>(m => m.Id == 3)))
                .Returns(existing);

            storageMock
                .Setup(x => x.GetElement(It.Is<MaterialResponsiblePersonSearchModel>(m => m.FullName == "Сидорова А.А.")))
                .Returns(existing);

            storageMock
                .Setup(x => x.GetElement(It.Is<MaterialResponsiblePersonSearchModel>(m => m.Phone == "+79995554433")))
                .Returns(existing);

            storageMock
                .Setup(x => x.GetElement(It.Is<MaterialResponsiblePersonSearchModel>(m => m.Email == "same@test.ru")))
                .Returns(existing);

            storageMock
                .Setup(x => x.Update(model))
                .Returns(new MaterialResponsiblePersonViewModel
                {
                    Id = 3,
                    FullName = "Сидорова А.А.",
                    Position = "Преподаватель",
                    Phone = "+79995554433",
                    Email = "same@test.ru"
                });

            var logic = new MaterialResponsiblePersonLogic(storageMock.Object);

            var result = logic.Update(model);

            Assert.NotNull(result);
            Assert.Equal("Преподаватель", result!.Position);
            storageMock.Verify(x => x.Update(model), Times.Once);
        }
    }
}
