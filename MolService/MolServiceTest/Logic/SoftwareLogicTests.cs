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
    public class SoftwareLogicTests
    {
        [Fact]
        public void Create_ShouldThrow_WhenSoftwareNameAlreadyExists()
        {
            var storageMock = new Mock<ISoftwareStorage>();

            storageMock
                .Setup(x => x.GetElement(It.Is<SoftwareSearchModel>(m => m.SoftwareName == "Visual Studio")))
                .Returns(new SoftwareViewModel { Id = 1, SoftwareName = "Visual Studio" });

            var logic = new SoftwareLogic(storageMock.Object);

            var ex = Assert.Throws<InvalidOperationException>(() => logic.Create(new SoftwareBindingModel
            {
                SoftwareName = "Visual Studio"
            }));

            Assert.Equal("Программное обеспечение с таким названием уже существует", ex.Message);
        }

        [Fact]
        public void Delete_ShouldThrow_WhenSoftwareDoesNotExist()
        {
            var storageMock = new Mock<ISoftwareStorage>();

            storageMock
                .Setup(x => x.GetElement(It.Is<SoftwareSearchModel>(m => m.Id == 77)))
                .Returns((SoftwareViewModel?)null);

            var logic = new SoftwareLogic(storageMock.Object);

            var ex = Assert.Throws<InvalidOperationException>(() =>
                logic.Delete(new SoftwareBindingModel { Id = 77 }));

            Assert.Equal("Программное обеспечение не найдено", ex.Message);
        }
    }
}
