using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace Galery.VM
{
    public class MenuItem : INotifyPropertyChanged
    {
        public string Name { get; set; }

        readonly Func<FrameworkElement> _getContent;
        readonly Func<object> _vmSetter;
        private FrameworkElement _content;

        private ScrollBarVisibility _horizontalScrollBarVisibilityRequirement;
        private ScrollBarVisibility _verticalScrollBarVisibilityRequirement;

        public MenuItem(string name, Func<FrameworkElement> pageFactory, Func<object> VMsetter = null)
        {
            Name = name;
            _getContent = pageFactory;
            _vmSetter = VMsetter;
        }

        public FrameworkElement GetContent
        {
            get
            {
                if (_content == null)
                    _content = _getContent();
                if (_vmSetter != null)
                    _content.DataContext = _vmSetter();
                return _content;
            }
        }

        public ScrollBarVisibility HorizontalScrollBarVisibilityRequirement
        {
            get { return _horizontalScrollBarVisibilityRequirement; }
            set { this.MutateVerbose(ref _horizontalScrollBarVisibilityRequirement, value, RaisePropertyChanged()); }
        }

        public ScrollBarVisibility VerticalScrollBarVisibilityRequirement
        {
            get { return _verticalScrollBarVisibilityRequirement; }
            set { this.MutateVerbose(ref _verticalScrollBarVisibilityRequirement, value, RaisePropertyChanged()); }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public void MutateVerbose<TField>(ref TField field, TField newValue, Action<PropertyChangedEventArgs> raise, [CallerMemberName] string propertyName = null)
        {
            if (EqualityComparer<TField>.Default.Equals(field, newValue)) return;
            field = newValue;
            raise?.Invoke(new PropertyChangedEventArgs(propertyName));
        }

        private Action<PropertyChangedEventArgs> RaisePropertyChanged()
        {
            return args => PropertyChanged?.Invoke(this, args);
        }
    }
}
