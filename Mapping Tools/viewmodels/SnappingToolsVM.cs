﻿using Mapping_Tools.Classes.SnappingTools.RelevantObjectGenerators;
using Mapping_Tools.Classes.SystemTools;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Controls;
using System.Windows.Data;

namespace Mapping_Tools.Viewmodels {
    public class SnappingToolsVM {
        public Hotkey SnapHotkey { get; set; }
        public ObservableCollection<IGenerateRelevantObjects> Generators { get; set; }
        private string _filter = "";
        public string Filter { get => _filter; set => SetFilter(value); }

        public SnappingToolsVM() {
            var interfaceType = typeof(IGenerateRelevantObjects);
            Generators = new ObservableCollection<IGenerateRelevantObjects>(AppDomain.CurrentDomain.GetAssemblies()
              .SelectMany(x => x.GetTypes())
              .Where(x => interfaceType.IsAssignableFrom(x) && !x.IsInterface && !x.IsAbstract)
              .Select(x => Activator.CreateInstance(x)).OfType<IGenerateRelevantObjects>());

            CollectionView view = (CollectionView)CollectionViewSource.GetDefaultView(Generators);
            PropertyGroupDescription groupDescription = new PropertyGroupDescription("GeneratorType");
            view.GroupDescriptions.Add(groupDescription);
            view.Filter = UserFilter;
        }

        private bool UserFilter(object item) {
            if (string.IsNullOrEmpty(Filter))
                return true;
            else
                return ((item as IGenerateRelevantObjects).Name.IndexOf(Filter, StringComparison.OrdinalIgnoreCase) >= 0);
        }

        private void SetFilter(string value) {
            _filter = value;
            CollectionViewSource.GetDefaultView(Generators).Refresh();
        }
    }
}
