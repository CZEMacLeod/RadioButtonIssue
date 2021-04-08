using System;
using System.Runtime.CompilerServices;
using Xamarin.CommunityToolkit.Extensions;
using Xamarin.CommunityToolkit.ObjectModel;
using Xamarin.Forms;

namespace RadioButtonIssue
{
    public class MainPageViewModel : ObservableObject
    {
        private UInt32 channel = 1;

        public MainPageViewModel()
        {
            Device.StartTimer(TimeSpan.FromSeconds(3), ChangeChannel);
        }

        private bool ChangeChannel()
        {
            Device.BeginInvokeOnMainThread(() =>
            {
                if (Channel == 8)
                {
                    Channel = 1;
                    return;
                }
                Channel++;
            });

            return true;
        }

        public UInt32 Channel { get => channel; set => SetProperty(ref channel, value, onChanged: ChannelChanged); }

        private async void ChannelChanged()
        {
            await Application.Current.MainPage.DisplayToastAsync($"Channel {Channel}").ConfigureAwait(false);
        }
    }
}
