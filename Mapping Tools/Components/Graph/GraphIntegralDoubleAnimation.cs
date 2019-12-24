﻿using System.Windows;
using System.Windows.Media.Animation;

namespace Mapping_Tools.Components.Graph {
    public class GraphIntegralDoubleAnimation : DoubleAnimationBase {
        public static readonly DependencyProperty GraphStateProperty =
            DependencyProperty.Register("GraphState",
                typeof(GraphState),
                typeof(GraphIntegralDoubleAnimation),
                new PropertyMetadata(null));

        public static readonly DependencyProperty FromProperty =
            DependencyProperty.Register("From",
                typeof(double?),
                typeof(GraphIntegralDoubleAnimation),
                new PropertyMetadata(null));

        public static readonly DependencyProperty ToProperty =
            DependencyProperty.Register("To",
                typeof(double?),
                typeof(GraphIntegralDoubleAnimation),
                new PropertyMetadata(null));

        /// <summary>
        ///     Specifies which graph to use for the values.
        /// </summary>
        public GraphState GraphState {
            get => (GraphState) GetValue(GraphStateProperty);
            set => SetValue(GraphStateProperty, value);
        }

        /// <summary>
        ///     Specifies the starting value of the animation.
        /// </summary>
        public double? From {
            get => (double?) GetValue(FromProperty);
            set => SetValue(FromProperty, value);
        }

        /// <summary>
        ///     Specifies the ending value of the animation.
        /// </summary>
        public double? To {
            get => (double?) GetValue(ToProperty);
            set => SetValue(ToProperty, value);
        }

        protected override double GetCurrentValueCore(double defaultOriginValue, double defaultDestinationValue,
            AnimationClock clock) {
            var start = From ?? defaultOriginValue;
            var delta = To - start ?? defaultOriginValue - start;

            return clock.CurrentProgress == null ? 0 : GraphState.GetIntegral(start, start + clock.CurrentProgress.Value * delta);
        }

        protected override Freezable CreateInstanceCore() {
            return new GraphIntegralDoubleAnimation();
        }
    }
}