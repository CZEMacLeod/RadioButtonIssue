using System;
using System.Runtime.CompilerServices;
using Xamarin.CommunityToolkit.Extensions;
using Xamarin.CommunityToolkit.ObjectModel;
using Xamarin.Forms;

namespace RadioButtonIssue
{
    public class MainPageViewModel : ObservableObject
    {
        private bool SetProperty<T>(ref T field, T newValue, [CallerMemberName] string propertyName = null)
        {
            return base.SetProperty(ref field, newValue, propertyName);
        }

        private UInt32 channel = 1;

        public MainPageViewModel()
        {
            Device.StartTimer(TimeSpan.FromSeconds(3), ChangeChannel);
            //var comp = StringComparer.InvariantCultureIgnoreCase;
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
        public object ChannelO { get => $"Ch{Channel}"; set => Channel = uint.Parse(((string)value).Substring(2)); }

        private async void ChannelChanged()
        {
            OnPropertyChanged(nameof(ChannelO));
            await Application.Current.MainPage.DisplayToastAsync($"Channel {Channel}").ConfigureAwait(false);
        }

        private string enteredCode;

        public string EnteredCode { get => enteredCode; set => SetProperty(ref enteredCode, value); }


    }
}
