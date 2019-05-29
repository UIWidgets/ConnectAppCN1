using System;
using Unity.UIWidgets.foundation;
using Unity.UIWidgets.physics;
using Unity.UIWidgets.ui;
using Unity.UIWidgets.widgets;
using UnityEngine;

namespace ConnectApp.Components.pull_to_refresh {
    public class RefreshScrollPhysics : ScrollPhysics {
        public RefreshScrollPhysics(
            ScrollPhysics parent = null,
            bool enableOverScroll = true
        ) : base(parent) {
            this.enableOverScroll = enableOverScroll;
        }

        public readonly bool enableOverScroll;

        static float frictionFactor(float overScrollFraction) {
            return 0.52f * Mathf.Pow(1 - overScrollFraction, 2);
        }

        public override ScrollPhysics applyTo(ScrollPhysics ancestor) {
            return new RefreshScrollPhysics(parent: this.buildParent(ancestor),
                enableOverScroll: this.enableOverScroll);
        }

        public override bool shouldAcceptUserOffset(ScrollMetrics position) {
            return true;
        }

        public override float applyPhysicsToUserOffset(ScrollMetrics position, float offset) {
            D.assert(offset != 0.0);
            D.assert(position.minScrollExtent <= position.maxScrollExtent);

            if (!position.outOfRange()) {
                return offset;
            }

            var overScrollPastStart = Math.Max(position.minScrollExtent - position.pixels, 0.0f);
            var overScrollPastEnd = Math.Max(position.pixels - position.maxScrollExtent, 0.0f);
            var overScrollPast = Mathf.Max(overScrollPastStart, overScrollPastEnd);
            bool easing = (overScrollPastStart > 0.0 && offset < 0.0) ||
                          (overScrollPastEnd > 0.0 && offset > 0.0);

            var friction = easing
                ? frictionFactor(
                    (overScrollPast - offset.abs()) / position.viewportDimension)
                : frictionFactor(overScrollPast / position.viewportDimension);
            float direction = offset.sign();
            return direction * _applyFriction(overScrollPast, offset.abs(), friction);
        }

        static float _applyFriction(float extentOutside, float absDelta, float gamma) {
            D.assert(absDelta > 0);
            var total = 0.0f;
            if (extentOutside > 0) {
                var deltaToLimit = extentOutside / gamma;
                if (absDelta < deltaToLimit) {
                    return float.Parse((absDelta * gamma).ToString());
                }

                total += extentOutside;
                absDelta -= deltaToLimit;
            }

            return float.Parse((total + absDelta).ToString());
        }

        public override float applyBoundaryConditions(ScrollMetrics position, float value) {
            if (!this.enableOverScroll) {
                if (value < position.pixels &&
                    position.pixels <= position.minScrollExtent) {
                    return value - position.pixels;
                }

                if (value < position.minScrollExtent &&
                    position.minScrollExtent < position.pixels) {
                    return value - position.minScrollExtent;
                }

                if (position.maxScrollExtent <= position.pixels &&
                    position.pixels < value) {
                    return value - position.pixels;
                }

                if (position.pixels < position.maxScrollExtent &&
                    position.maxScrollExtent < value) {
                    return value - position.maxScrollExtent;
                }
            }

            return 0.0f;
        }

        public override Simulation createBallisticSimulation(ScrollMetrics position, float velocity) {
            Tolerance tolerance = this.tolerance;
            if (velocity.abs() >= tolerance.velocity || position.outOfRange()) {
                return new BouncingScrollSimulation(
                    spring: this.spring,
                    position: position.pixels,
                    velocity: velocity * 0.91f,
                    leadingExtent: position.minScrollExtent,
                    trailingExtent: position.maxScrollExtent,
                    tolerance: tolerance
                );
            }

            return null;
        }

        public override float minFlingVelocity {
            get { return 2.5f * 2.0f; }
        }

        public override float carriedMomentum(float existingVelocity) {
            return float.Parse((existingVelocity.sign() *
                                Math.Min(0.000816f * Math.Pow(existingVelocity.abs(), 1.967f),
                                    40000.0f)).ToString());
        }

        public override float? dragStartDistanceMotionThreshold {
            get { return 3.5f; }
        }
    }
}