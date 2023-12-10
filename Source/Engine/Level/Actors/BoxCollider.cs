namespace FlaxEngine
{
    partial class BoxCollider
    {
        private void BoxExcluding(Actor target, ref BoundingBox output, Actor excluded)
        {
            foreach (Actor child in target.Children)
            {
                if (child == excluded)
                {
                    continue;
                }

                output = BoundingBox.Merge(output, child.Box);
                BoxExcluding(child, ref output, excluded);
            }
        }

        /// <summary>
        /// Resizes the box collider based on the bounds of it's parent.
        /// </summary>
        public void AutoResize()
        {
            if (Parent is Scene)
            {
                return;
            }

            Vector3 parentScale = Parent.Scale;
            BoundingBox parentBox = Parent.Box;
            BoxExcluding(Parent, ref parentBox, this);

            Vector3 parentSize = parentBox.Size;
            Vector3 parentCenter = parentBox.Center - Parent.Position;

            // Avoid division by zero
            if (parentScale.X == 0 || parentScale.Y == 0 || parentScale.Z == 0)
            {
                return;
            }

            Size = parentSize / parentScale;
            Center = parentCenter / parentScale;

            // Undo Rotation
            Orientation *= Quaternion.Invert(Orientation);
        }

        /// <inheritdoc />
        public override void OnActorSpawned()
        {
            base.OnActorSpawned();
            AutoResize();
        }
    }
}
