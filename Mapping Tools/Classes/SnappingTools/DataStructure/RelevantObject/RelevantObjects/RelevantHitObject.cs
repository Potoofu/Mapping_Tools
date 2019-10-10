﻿using System.Collections.Generic;
using System.Linq;
using Mapping_Tools.Classes.BeatmapHelper;
using Mapping_Tools.Classes.MathUtil;

namespace Mapping_Tools.Classes.SnappingTools.DataStructure.RelevantObject {
    public class RelevantHitObject : RelevantObject {
        public HitObject HitObject;

        public bool IsSelected {
            get => HitObject.IsSelected;
            set => HitObject.IsSelected = value;
        }

        public void Consume(RelevantHitObject other) {
            ParentObjects = ParentObjects.Concat(other.ParentObjects).ToList();
            IsSelected = IsSelected || other.IsSelected;
        }

        public double Difference(RelevantHitObject other) {
            if (HitObject.ObjectType != other.HitObject.ObjectType) {
                return double.PositiveInfinity;
            }

            if (HitObject.SliderType != other.HitObject.SliderType) {
                return double.PositiveInfinity;
            }

            if (HitObject.CurvePoints.Count != other.HitObject.CurvePoints.Count) {
                return double.PositiveInfinity;
            }

            var differences = new List<double> {Vector2.DistanceSquared(HitObject.Pos, other.HitObject.Pos)};
            differences.AddRange(HitObject.CurvePoints.Select((t, i) => Vector2.DistanceSquared(t, other.HitObject.CurvePoints[i])));

            return differences.Sum() / differences.Count;
        }

        public override double DistanceTo(IRelevantObject relevantObject) {
            throw new System.NotImplementedException();
        }

        public override void Consume(IRelevantObject relevantObject) {
            throw new System.NotImplementedException();
        }
    }
}
