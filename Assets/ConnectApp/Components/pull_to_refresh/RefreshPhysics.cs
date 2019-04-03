using System;
using Unity.UIWidgets.foundation;
using Unity.UIWidgets.physics;
using Unity.UIWidgets.ui;
using Unity.UIWidgets.widgets;

namespace ConnectApp.components.pull_to_refresh {
    public class RefreshScrollPhysics : ScrollPhysics {
        public RefreshScrollPhysics(
            ScrollPhysics parent = null,
            bool enableOverScroll = true
        ) : base(parent) {
            this.enableOverScroll = enableOverScroll;
        }

        public readonly bool enableOverScroll;

        private double frictionFactor(double overscrollFraction) {
            return 0.52 * Math.Pow(1 - overscrollFraction, 2);
        }

        public override ScrollPhysics applyTo(ScrollPhysics ancestor) {
            return new RefreshScrollPhysics(parent: buildParent(ancestor), enableOverScroll: enableOverScroll);
        }

        public override bool shouldAcceptUserOffset(ScrollMetrics position) {
            return true;
        }

        public override float applyPhysicsToUserOffset(ScrollMetrics position, float offset) {
            D.assert(offset != 0.0);
            D.assert(position.minScrollExtent <= position.maxScrollExtent);

            if (!position.outOfRange()) return offset;

            double overscrollPastStart =
                Math.Max(position.minScrollExtent - position.pixels, 0.0);
            double overscrollPastEnd =
                Math.Max(position.pixels - position.maxScrollExtent, 0.0);
            double overscrollPast =
                Math.Max(overscrollPastStart, overscrollPastEnd);
            bool easing = (overscrollPastStart > 0.0 && offset < 0.0) ||
                          (overscrollPastEnd > 0.0 && offset > 0.0);

            double friction = easing
                // Apply less resistance when easing the overscroll vs tensioning.
                ? frictionFactor(
                    (overscrollPast - offset.abs()) / position.viewportDimension)
                : frictionFactor(overscrollPast / position.viewportDimension);
            float direction = offset.sign();
            return direction * _applyFriction(overscrollPast, offset.abs(), friction);
        }

        private static float _applyFriction(
            double extentOutside, double absDelta, double gamma) {
            D.assert(absDelta > 0);
            double total = 0.0f;
            if (extentOutside > 0) {
                double deltaToLimit = extentOutside / gamma;
                if (absDelta < deltaToLimit) return float.Parse((absDelta * gamma).ToString());
                total += extentOutside;
                absDelta -= deltaToLimit;
            }

            return float.Parse((total + absDelta).ToString());
        }

        public override float applyBoundaryConditions(ScrollMetrics position, float value) {
            if (!enableOverScroll) {
                if (value < position.pixels &&
                    position.pixels <= position.minScrollExtent) // underscroll
                    return value - position.pixels;
                if (value < position.minScrollExtent &&
                    position.minScrollExtent < position.pixels) // hit top edge
                    return value - position.minScrollExtent;
                if (position.maxScrollExtent <= position.pixels &&
                    position.pixels < value) // overscroll
                    return value - position.pixels;

                if (position.pixels < position.maxScrollExtent &&
                    position.maxScrollExtent < value) // hit bottom edge
                    return value - position.maxScrollExtent;
            }

            return 0.0f;
        }

        public override Simulation createBallisticSimulation(ScrollMetrics position, float velocity) {
            Tolerance tolerance = this.tolerance;
            if (velocity.abs() >= tolerance.velocity || position.outOfRange())
                return new BouncingScrollSimulation(
                    spring: spring,
                    position: position.pixels,
                    velocity: velocity *
                              0.91f, // TODO(abarth): We should move this constant closer to the drag end.
                    leadingExtent: position.minScrollExtent,
                    trailingExtent: position.maxScrollExtent,
                    tolerance: tolerance
                );
            return null;
        }

        public override float minFlingVelocity => 2.5f * 2.0f;

        public override float carriedMomentum(float existingVelocity) {
            return float.Parse((existingVelocity.sign() *
                                Math.Min(0.000816f * Math.Pow(existingVelocity.abs(), 1.967f),
                                    40000.0f)).ToString());
        }

        public override float? dragStartDistanceMotionThreshold => 3.5f;
    }
}