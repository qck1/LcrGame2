﻿using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;

namespace LcrGame.ViewModels
{
    public class BarGraphViewModel : INotifyPrropertyChanged
    {
        private double _maximumItemValue;
        private int _totalItemCount;
        private double _sum;
        private double _average;

        public ObservableCollection<double> ItemsSource { get; } = new ObservableCollection<double>();

        public double MaximumItemValue
        {
            get => _maximumItemValue;
            set
            {
                if (_maximumItemValue != value)
                {
                    _maximumItemValue = value;
                    OnPropertyChanged();
                }
            }
        }

        public int TotalItemCount
        {
            get => _totalItemCount;
            set
            {
                if (_totalItemCount != value)
                {
                    _totalItemCount = value;
                    OnPropertyChanged();
                }
            }
        }

        public double Average
        {
            get => _average;
            set
            {
                if (_average != value)
                {
                    _average = value;
                    OnPropertyChanged();
                }
            }
        }

        public void AddItem(double item)
        {
            ItemsSource.Add(item);
            _sum += item;
            Average = _sum / ItemsSource.Count();

            if (item + 1 > MaximumItemValue)
            {
                MaximumItemValue = Math.Ceiling(item + 1);
                // This has to be done to recreat item, otherwise it is not adjusted when Maximum changed
                var holding = ItemsSource.Select(i => i).ToList();
                ItemsSource.Clear();
                holding.ForEach(i => ItemsSource.Add(i));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        internal void Reset(int totalItemCount)
        {
            ItemsSource.Clear();
            _sum = 0;
            MaximumItemValue = 4;
            TotalItemCount = totalItemCount;
        }
    }
}