﻿using System;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using JetBrains.Annotations;
using Mapping_Tools_Core.Tools.SnappingTools.DataStructure.RelevantObjectGenerators.GeneratorInputSelection;
using Newtonsoft.Json;

namespace Mapping_Tools_Core.Tools.SnappingTools.DataStructure.RelevantObjectGenerators {
    public class GeneratorSettings : BindableBase, ICloneable {
        [JsonIgnore]
        [CanBeNull]
        public RelevantObjectsGenerator Generator { get; set; }

        public GeneratorSettings() {
            _relevancyRatio = 0.8;
            _generatesInheritable = true;
            _inputPredicate = new SelectionPredicateCollection();
        }

        public GeneratorSettings(RelevantObjectsGenerator generator) {
            Generator = generator;

            _relevancyRatio = 0.8;
            _generatesInheritable = true;
            _inputPredicate = new SelectionPredicateCollection();
        }

        private bool _isActive;
        [DisplayName("Active")]
        [Description("Active generators can generate virtual objects from hit objects or other virtual objects.")]
        public bool IsActive
        {
            get => _isActive;
            set => Set(ref _isActive, value);
        }

        private bool _isSequential;
        [DisplayName("Sequential")]
        [Description("Sequential generators will only take sequential objects as input.")]
        public bool IsSequential {
            get => _isSequential;
            set => Set(ref _isSequential, value);
        }

        private bool _isDeep;
        [DisplayName("Deep")]
        [Description("Deep generators can take objects as input not only from the previous layer, but all previous layers.")]
        public bool IsDeep {
            get => _isDeep;
            set => Set(ref _isDeep, value);
        }

        private double _relevancyRatio;
        [DisplayName("Relevancy Ratio")]
        [Description("The multiplier for the relevancy of virtual objects generated by this generator.")]
        public double RelevancyRatio {
            get => _relevancyRatio;
            set => Set(ref _relevancyRatio, value);
        }

        private bool _generatesInheritable;
        [DisplayName("Generates Inheritable")]
        [Description("Specifies whether virtual objects generated by this generator are inheritable.")]
        public bool GeneratesInheritable {
            get => _generatesInheritable;
            set => Set(ref _generatesInheritable, value);
        }

        private SelectionPredicateCollection _inputPredicate;
        [DisplayName("Input Selection")]
        [Description("Specifies extra rules that virtual objects need to obey to be used by this generator. Only one of the predicates has to be met by any virtual object.")]
        public SelectionPredicateCollection InputPredicate {
            get => _inputPredicate;
            set => Set(ref _inputPredicate, value);
        }

        public void CopyTo(GeneratorSettings other) {
            var otherPropertyNames = other.GetType().GetProperties().Select(o => o.Name).ToArray();
            foreach (var prop in GetType().GetProperties()) {
                if (!prop.CanWrite || !prop.CanRead) continue;
                if (!otherPropertyNames.Contains(prop.Name)) continue;
                if (prop.GetCustomAttribute(typeof(JsonIgnoreAttribute)) != null) continue;
                try { prop.SetValue(other, prop.GetValue(this)); } catch (Exception ex) {
                    Console.WriteLine(ex.Message + ex.StackTrace);
                }
            }
        }

        public virtual object Clone() {
            return new GeneratorSettings {Generator = Generator, IsActive = IsActive, IsSequential = IsSequential, IsDeep = IsDeep, 
                RelevancyRatio = RelevancyRatio, GeneratesInheritable = GeneratesInheritable,
                InputPredicate = (SelectionPredicateCollection)InputPredicate.Clone()};
        }
    }
}