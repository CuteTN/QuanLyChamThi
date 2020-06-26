using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLyChamThi
{
    class Pair<T,U>: INotifyPropertyChanged
    {
        private T _first;
        public T First 
        {
            get { return _first; }
            set { _first = value;  OnPropertyChange("First"); } 
        }
        private U _second;
        public U Second 
        { 
            get { return _second; }
            set { _second = value; OnPropertyChange("Second"); } 
        }

        public Pair(T first, U second)
        {
            First = first;
            Second = second;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChange(string propertyName)
        {
            PropertyChangedEventHandler handler = this.PropertyChanged;
            if (handler != null)
            {
                var e = new PropertyChangedEventArgs(propertyName);
                handler(this, e);
            }
        }
    }
}
