using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LibVLCSharp.Forms.Shared;
using LibVLCSharp.Shared;
using Xamarin.Forms;

namespace VLCDemo
{
    public partial class MainPage : ContentPage
    {
        LibVLC _libVLC;
        MediaPlayer _mediaPlayer;
        LibVLCSharp.Forms.Shared.MediaPlayerElement _videoView;
        float _position;

        public MainPage()
        {
            InitializeComponent();

            MessagingCenter.Subscribe<string>(this, "OnPause", app =>
            {
                //VideoView.MediaPlayerChanged -= MediaPlayerChanged;
                _mediaPlayer.Pause();
                _position = _mediaPlayer.Position;
                _mediaPlayer.Stop();
                MainGrid.Children.Clear();
                Debug.WriteLine($"saving mediaplayer position {_position}");
            });

            MessagingCenter.Subscribe<string>(this, "OnRestart", app =>
            {
                _videoView = new LibVLCSharp.Forms.Shared.MediaPlayerElement
                {
                    HorizontalOptions = LayoutOptions.FillAndExpand,
                    VerticalOptions = LayoutOptions.FillAndExpand
                };
                MainGrid.Children.Add(_videoView);

                _videoView.PlaybackControls.IsCastButtonVisible = false;
                _videoView.PlaybackControls.KeepScreenOn = true;
                _videoView.PlaybackControls.ShowAndHideAutomatically = false;

                _mediaPlayer.Play();
                _videoView.MediaPlayer = _mediaPlayer;
                _videoView.MediaPlayer.Position = _position;
                _position = 0;
            });
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            Core.Initialize();

            _libVLC = new LibVLC();
            var media = new Media(_libVLC, new Uri("http://commondatastorage.googleapis.com/gtv-videos-bucket/sample/ElephantsDream.mp4"));

            _mediaPlayer = new MediaPlayer(_libVLC)
            {
                Media = media
            };

            VideoView.MediaPlayer = _mediaPlayer;

            _mediaPlayer.Play();
        }

        //private void MediaPlayerChanged(object sender, EventArgs e)
        //{
        //    Debug.WriteLine($"MediaPlayerChanged {e}");
        //    _mediaPlayer.Play();
        //}
    }

}
