/*///////////////////////////////////////////////////////////////////
<EasyFarm, general farming utility for FFXI.>
Copyright (C) <2013>  <Zerolimits>

This program is free software: you can redistribute it and/or modify
it under the terms of the GNU General Public License as published by
the Free Software Foundation, either version 3 of the License, or
(at your option) any later version.

This program is distributed in the hope that it will be useful,
but WITHOUT ANY WARRANTY; without even the implied warranty of
MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
GNU General Public License for more details.

You should have received a copy of the GNU General Public License
*/
///////////////////////////////////////////////////////////////////

using System.Collections.ObjectModel;
using System.Windows.Input;
using EasyFarm.Classes;
using EasyFarm.Mvvm;
using Microsoft.Practices.Prism.Commands;

namespace EasyFarm.ViewModels
{
    [ViewModel("Ignored")]
    public class IgnoredViewModel : ViewModelBase
    {
        public IgnoredViewModel()
        {
            AddIgnoredUnitCommand = new DelegateCommand(AddIgnoredUnit);
            DeleteIgnoredUnitCommand = new DelegateCommand(DeleteIgnoredUnit);
            ClearIgnoredUnitsCommand = new DelegateCommand(ClearIgnoredUnits);
        }

        public string Name
        {
            get { return Config.Instance.IgnoredName; }
            set { SetProperty(ref Config.Instance.IgnoredName, value); }
        }

        public ObservableCollection<string> Ignored
        {
            get { return Config.Instance.IgnoredMobs; }
            set { SetProperty(ref Config.Instance.IgnoredMobs, value); }
        }

        public ICommand AddIgnoredUnitCommand { get; set; }
        public ICommand DeleteIgnoredUnitCommand { get; set; }
        public ICommand ClearIgnoredUnitsCommand { get; set; }

        private void ClearIgnoredUnits()
        {
            Ignored.Clear();
        }

        private void DeleteIgnoredUnit()
        {
            if (Ignored.Contains(Name))
                Ignored.Remove(Name);
        }

        private void AddIgnoredUnit()
        {
            if (!Ignored.Contains(Name))
                Ignored.Add(Name);
        }
    }
}