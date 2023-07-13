using LibVLCSharp.Shared;
using LibVLCSharp.WPF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace VLCDemo
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    /// 
    public partial class MainWindow : Window
    {
        public LibVLC _libVLC = null;
        LibVLCSharp.Shared.MediaPlayer _mediaPlayer;
        string filePath = string.Empty;
        Timer _timer = null;
        public MainWindow()
        {
            InitializeComponent();
            filePath = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Videos", "1.mp4");
        }

        private void VideoView_Loaded(object sender, RoutedEventArgs e)
        {
            _libVLC = new LibVLC(enableDebugLogs: true);
            _mediaPlayer = new LibVLCSharp.Shared.MediaPlayer(_libVLC);
            _mediaPlayer.EnableHardwareDecoding = true;

            VideoView.MediaPlayer = _mediaPlayer;
            PlayVideo(filePath);
            if (_timer == null)
            {
                _timer = new Timer();
                _timer.Interval = 10*1000;
                _timer.Elapsed += _timer_Elapsed;
                _timer.Start();
            }
        }

        private void _timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            this.Dispatcher.Invoke(() =>
            {
                if (VideoView.MediaPlayer.IsPlaying && VideoView.MediaPlayer.IsSeekable)
                {
                    VideoView.MediaPlayer.Time = 0;
                }
                else
                {
                    PlayVideo(filePath);
                }
            });
        }

        private void PlayVideo(string Path)
        {
            using (var media = new LibVLCSharp.Shared.Media(_libVLC, new Uri(Path)))
            {
                media.AddOption(new MediaConfiguration { EnableHardwareDecoding = true, FileCaching = 300, NetworkCaching = 300 });
                VideoView?.MediaPlayer?.Play(media);
            }
        }

    }
}
