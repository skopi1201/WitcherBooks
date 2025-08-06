using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Klase
{
    [Serializable]
    public class Book : INotifyPropertyChanged
    {
        
        public int Id { get; set; }
        public string Title { get; set; }

        public string Date { get; set; }
        public string Imgpath { get; set; }
        public string Rtfpath { get; set; }

        private bool _isDeleting;
        public bool isDeleting
        {
            get => _isDeleting;
            set
            {
                if (_isDeleting != value)
                {
                    _isDeleting = value;
                    OnPropertyChanged(nameof(isDeleting));
                }
            }
        }


        public Book() { }

        public Book(int id,string title,string imgpath,string rtfpath,string date) 
        {
            isDeleting = false;
            Id = id;
            Title = title; 
            Imgpath = imgpath; 
            Rtfpath = rtfpath;
            Date = date;
        
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string name)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

    }
}
