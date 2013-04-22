using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;

namespace SfBot.Controls
{
    public class BusyIndicator : ContentControl, INotifyPropertyChanged
    {
        public static readonly DependencyProperty IsBusyProperty =
            DependencyProperty.Register("IsBusy", typeof(bool), typeof(BusyIndicator));

        public bool IsBusy
        {
            get { return (bool)GetValue(IsBusyProperty); }
            set
            {
                SetValue(IsBusyProperty, value);
                NotifyPropertyChanged();
            }
        }

        public static readonly DependencyProperty MessageProperty =
            DependencyProperty.Register("Message", typeof(string), typeof(BusyIndicator));

        public string Message
        {
            get { return (string) GetValue(MessageProperty); }
            set
            {
                SetValue(MessageProperty, value);
                NotifyPropertyChanged();
            }
        }

        static BusyIndicator()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof (BusyIndicator),
                                                     new FrameworkPropertyMetadata(typeof (BusyIndicator)));
        }

        #region NPC

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void NotifyPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion
    }
}