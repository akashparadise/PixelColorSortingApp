using ColorSortingUtility.Repository;
using Microsoft.Extensions.Logging;
using Moq;
using PixelColorSortingApp.ViewModels;
using System;
using System.Drawing;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Xunit;

namespace PixelColorSortingApp.Tests
{
    public class MainViewModelTests
    {
        private readonly MainViewModel _mainViewModel;
        private readonly Mock<ICreateRandomPixels> _createRandomPixels = new Mock<ICreateRandomPixels>();
        private readonly Mock<IBitmapToImageConverter> _bitmapToImageConverter = new Mock<IBitmapToImageConverter>();
        private readonly Mock<ILogger<MainViewModel>> _logger = new Mock<ILogger<MainViewModel>>();

        int height = 320;
        int width = 640;

        public MainViewModelTests()
        {
            _mainViewModel = new MainViewModel(_bitmapToImageConverter.Object, 
                _createRandomPixels.Object,
                _logger.Object 
                );
        }

        [Fact]
        public void CreateRandomBitmapSource_ValidCall()
        {
            //Arrange
            var randomPixels = new byte[3 * height * width];
            new Random().NextBytes(randomPixels);
            BitmapSource bitmapExpected = BitmapSource.Create(width, height, 96d, 96d, PixelFormats.Bgr24, null, randomPixels, width * 3);

            _createRandomPixels.Setup(x =>
            x.CreateRandomBitmapSource(width, height)).Returns(bitmapExpected);

            //Act
            bool actualResult = _mainViewModel.CreateRandomPixel();

            //Assert
            Assert.True(actualResult);
        }
    }
}
