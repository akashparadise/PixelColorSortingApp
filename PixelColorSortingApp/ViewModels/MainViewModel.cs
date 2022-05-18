using System;
using System.Drawing;
using System.IO;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using ColorSortingUtility.Repository;
using Microsoft.Extensions.Logging;
using PixelColorSortingApp.Commands;

namespace PixelColorSortingApp.ViewModels
{
    public class MainViewModel : ViewModelBase
    {
        private readonly IBitmapToImageConverter _bitmapToImageConverter;
        private readonly ICreateRandomPixels _createRandomPixels;
        private readonly ILogger<MainViewModel> _logger;

        public MainViewModel(IBitmapToImageConverter bitmapToImageConverter,
            ICreateRandomPixels createRandomPixels,
            ILogger<MainViewModel> logger)
        {
            _bitmapToImageConverter = bitmapToImageConverter;
            _createRandomPixels = createRandomPixels;
            _logger = logger;
        }

        public BitmapSource ImageToShow { get; set; }

        private string _status;
        public string Status { get => this._status; set { this._status = value; OnPropertyChanged(); } }

        private void CmdExec(object obj)
        {
            switch (obj.ToString())
            {
                case "Create":
                    CreateRandomPixel();
                    break;
                case "Sort":
                    CreateSortedPixel();
                    break;
                default:
                    break;
            }
        }
        private bool CanCmdExec(object obj) => (obj.ToString() == "Show") ? true : !this._isWorking;

        public ICommand Cmd { get => new PixelCommand(CmdExec, CanCmdExec); }

        private bool _isWorking = false;

        private bool IsWorking { get => this._isWorking; set { this._isWorking = value; OnPropertyChanged(nameof(Cmd)); } }

        public bool CreateRandomPixel()
        {
            IsWorking = true;
            string methodName = "CreateRandomPixel";
            try
                {
                    int width = 640;
                    int height = 320;

                _logger.LogInformation($"{ methodName }: Creating a bitmap source of random pixels");
                    ImageToShow = _createRandomPixels.CreateRandomBitmapSource(width, height);
                    OnPropertyChanged(nameof(ImageToShow));
                }
                catch (Exception ex) 
                {
                _logger.LogError($"{ methodName }: { ex.Message }");
                Status = ex.Message; 
                }
            IsWorking = false;
            return ImageToShow == null ? false : true;
        }

        public bool CreateSortedPixel()
        {
            IsWorking = true;
            string methodName = "ColorSortingButton_Click";
                try
                {
                    _logger.LogInformation($"{ methodName }: Color sorting button click event started");

                    if (ImageToShow != null)
                    {                     
                        Bitmap bmpOut = null;

                        _logger.LogInformation($"{ methodName }: converting image with random pixels to bitmap");

                        using (MemoryStream ms = new MemoryStream())
                        {
                            BmpBitmapEncoder encoder = new BmpBitmapEncoder();
                            encoder.Frames.Add(BitmapFrame.Create(ImageToShow));
                            encoder.Save(ms);

                            using (Bitmap bmp = new Bitmap(ms))
                            {
                                bmpOut = new Bitmap(bmp);
                            }
                        }

                    ImageToShow = _bitmapToImageConverter.BitmapToImageSource(bmpOut);
                    OnPropertyChanged(nameof(ImageToShow));

                    _logger.LogInformation($"{ methodName }: Color sorting button click event finished");
                }
                }
                catch (Exception ex)
                {
                    _logger.LogError($"{ methodName }: { ex.Message }");
                    Status = ex.Message;
                }
            IsWorking = false;
            return ImageToShow == null ? false : true;
        }
    }
}
