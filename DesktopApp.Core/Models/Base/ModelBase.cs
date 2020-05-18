using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;

namespace DesktopApp.Core.Models
{
    public abstract class ModelBase : INotifyPropertyChanged
    {
        protected string name;
        protected string directory;
        public string Name { get => name; set => SetProperty<string>(ref this.name, value, "Name"); }
        public string Directory { get => directory; set => SetProperty<string>(ref this.directory, value, "Directory"); }

        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Checks if a property already matches a desired value. Sets the property and notifies listeners only when necessary.
        /// </summary>
        /// <typeparam name="T">The type of the property.</typeparam>
        /// <param name="storage">Reference to a property with both getter and setter.</param>
        /// <param name="value">Desired value for the property.</param>
        /// <param name="propertyName">Name of the property used to notify listeners.</param>
        /// <returns>True if the value was changed, false if the existing value matched the desired value.</returns>
        protected virtual bool SetProperty<T>(ref T storage, T value, [CallerMemberName] string propertyName = null)
        {
            if (Equals(storage, value))
            {
                return false;
            }
            storage = value;
            OnPropertyChanged(propertyName);
            return true;
        }

        /// <summary>
        /// Notifies listeners that a property value has changed.
        /// </summary>
        /// <param name="propertyName">Name of the property used to notify listeners.</param>
        protected virtual void OnPropertyChanged(string propertyName)
        {
            var changedEventHandler = PropertyChanged;
            if (changedEventHandler == null)
            {
                return;
            }
            changedEventHandler(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
