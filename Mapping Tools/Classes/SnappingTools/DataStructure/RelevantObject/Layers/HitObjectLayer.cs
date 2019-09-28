﻿using Mapping_Tools.Classes.SnappingTools.DataStructure.RelevantObjectGenerators;

namespace Mapping_Tools.Classes.SnappingTools.DataStructure.RelevantObject.Layers {
    /// <summary>
    /// Container for a list of HitObjects
    /// </summary>
    public class HitObjectLayer : ObjectLayer {
        // This list must always be sorted by time
        public HitObjectContext HitObjects {
            get => (HitObjectContext) Objects;
            set => Objects = value;
        }
        // Context is all the objects sorted by time of this layer and previous layers if that's your preference
        // A context must always have the same objects as the layers
        public HitObjectContext NextHitObjectContext {
            get => (HitObjectContext)NextContext;
            set => NextContext = value;
        }

        public HitObjectGeneratorCollection GeneratorCollection;
        public RelevantObjectLayer NextLayer;
        public LayerCollection Collection;

        public void Add(RelevantHitObject hitObject) {
            // Check if this object or something similar exists anywhere in the context or in this layer
            if (HitObjects.FindSimilar(hitObject, Collection.AcceptableDifference, out var similarObject)) {
                similarObject.Consume(hitObject);
            }

            Objects.SortedInsert(hitObject);
            NextContext.SortedInsert(hitObject);

            NextLayer.DeleteObjectsFromConcurrent();

            GeneratorCollection.GenerateNewObjects(NextLayer, NextHitObjectContext, hitObject);

            /*foreach (var pointsGenerator in Generators.OfType<IGeneratePointsFromHitObjects>()) {
                if (NextLayer is RelevantObjectLayer rol) {
                    rol.AddPoints(pointsGenerator.GetRelevantObjects(HitObjects));
                }
            }*/


            // Sort the object list
            // Redo any generators that need concurrent HitObjects
            // An object generated by a concurrent generator only cares about the index distance of the whole context of their parents to stay the same.
            // An object must remember what generator generated it.
            // An object becomes invalid once any of the parents become invalid or its generator got disabled
            // The entire context must be updated on a change of any layer
            // Before adding the object it must be checked if there are similar objects in the whole context
            // Similar in whole context means the object won't be added
            // Similar in this layer will merge it with that object
            // How do I know for concurrent generators which gaps to fill?
            // A concurrent generator will loop through it's context in the changed area to find the objects to invalidate and new objects to generate
            // Context is the sorted previous layer or all previous layers sorted by time
            // Non-concurrent generators will never have to remove objects from an add event
            // Generate objects to add to the next layer
            // Invoke event so the layer collection can update the next layer
        }
    }
}