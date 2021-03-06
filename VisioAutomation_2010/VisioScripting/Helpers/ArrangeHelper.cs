﻿using System.Collections.Generic;
using System.Linq;
using VisioAutomation.ShapeSheet.Writers;
using IVisio = Microsoft.Office.Interop.Visio;

namespace VisioScripting.Helpers
{
    internal static class ArrangeHelper
    {
        private static double GetPositionOnShape(VisioScripting.Models.ShapeXFormData xform, VisioScripting.Models.ShapeRelativePosition pos)
        {
            var r = xform.GetRectangle();

            switch (pos)
            {
                case VisioScripting.Models.ShapeRelativePosition.PinY:
                    return xform.PinY;
                case VisioScripting.Models.ShapeRelativePosition.PinX:
                    return xform.PinX;
                case VisioScripting.Models.ShapeRelativePosition.Left:
                    return r.Left;
                case VisioScripting.Models.ShapeRelativePosition.Right:
                    return r.Right;
                case VisioScripting.Models.ShapeRelativePosition.Top:
                    return r.Top;
                case VisioScripting.Models.ShapeRelativePosition.Bottom:
                    return r.Bottom;
            }

            throw new System.ArgumentOutOfRangeException(nameof(pos));
        }

        internal static List<int> SortShapesByPosition(IVisio.Page page, VisioScripting.Models.TargetShapeIDs targets, VisioScripting.Models.ShapeRelativePosition pos)
        {
            // First get the transforms of the shapes on the given axis
            var xforms = VisioScripting.Models.ShapeXFormData.Get(page, targets);

            // Then, sort the shapeids pased on the corresponding value in the results

            var sorted_shape_ids = Enumerable.Range(0, targets.ShapeIDs.Count)
                .Select(i => new { index = i, shapeid = targets.ShapeIDs[i], pos = ArrangeHelper.GetPositionOnShape(xforms[i], pos) })
                .OrderBy(i => i.pos)
                .Select(i => i.shapeid)
                .ToList();

            return sorted_shape_ids;
        }

        public static void DistributeWithSpacing(IVisio.Page page, VisioScripting.Models.TargetShapeIDs target, VisioScripting.Models.Axis axis, double spacing)
        {
            if (spacing < 0.0)
            {
                throw new System.ArgumentOutOfRangeException(nameof(spacing));
            }

            if (target.ShapeIDs.Count < 2)
            {
                return;
            }

            // Calculate the new Xfrms
            var sortpos = axis == VisioScripting.Models.Axis.XAxis
                ? VisioScripting.Models.ShapeRelativePosition.PinX
                : VisioScripting.Models.ShapeRelativePosition.PinY;

            var delta = axis == VisioScripting.Models.Axis.XAxis
                ? new VisioAutomation.Drawing.Size(spacing, 0)
                : new VisioAutomation.Drawing.Size(0, spacing);


            var input_xfrms = VisioScripting.Models.ShapeXFormData.Get(page, target);
            var bb = VisioScripting.Models.ShapeXFormData.GetBoundingBox(input_xfrms);
            var cur_pos = new VisioAutomation.Drawing.Point(bb.Left, bb.Bottom);

            var newpositions = new List<VisioAutomation.Drawing.Point>(target.ShapeIDs.Count);
            foreach (var input_xfrm in input_xfrms)
            {
                var new_pinpos = axis == VisioScripting.Models.Axis.XAxis
                    ? new VisioAutomation.Drawing.Point(cur_pos.X + input_xfrm.LocPinX, input_xfrm.PinY)
                    : new VisioAutomation.Drawing.Point(input_xfrm.PinX, cur_pos.Y + input_xfrm.LocPinY);

                newpositions.Add(new_pinpos);
                cur_pos = cur_pos.Add(input_xfrm.Width, input_xfrm.Height).Add(delta);
            }

            // Apply the changes
            var sorted_shape_ids = ArrangeHelper.SortShapesByPosition(page, target, sortpos);

            ModifyPinPositions(page, sorted_shape_ids, newpositions);
        }

        private static void ModifyPinPositions(IVisio.Page page, IList<int> sorted_shape_ids, List<VisioAutomation.Drawing.Point> newpositions)
        {
            var writer = new SidSrcWriter();
            for (int i = 0; i < newpositions.Count; i++)
            {
                writer.SetFormula((short)sorted_shape_ids[i], VisioAutomation.ShapeSheet.SrcConstants.XFormPinX, newpositions[i].X);
                writer.SetFormula((short)sorted_shape_ids[i], VisioAutomation.ShapeSheet.SrcConstants.XFormPinY, newpositions[i].Y);
            }

            writer.Commit(page);
        }

        private static void ModifySizes(IVisio.Page page, IList<int> sorted_shape_ids, List<VisioAutomation.Drawing.Size> newsizes)
        {
            var writer = new SidSrcWriter();
            for (int i = 0; i < newsizes.Count; i++)
            {
                writer.SetFormula((short)sorted_shape_ids[i], VisioAutomation.ShapeSheet.SrcConstants.XFormWidth, newsizes[i].Width);
                writer.SetFormula((short)sorted_shape_ids[i], VisioAutomation.ShapeSheet.SrcConstants.XFormHeight, newsizes[i].Height);
            }

            writer.Commit(page);
        }

        public static void SnapCorner(IVisio.Page page, VisioScripting.Models.TargetShapeIDs target, VisioAutomation.Drawing.Size snapsize, VisioScripting.Models.SnapCornerPosition corner)
        {
            // First caculate the new transforms
            var snap_grid = new VisioScripting.Models.SnappingGrid(snapsize);
            var input_xfrms = VisioScripting.Models.ShapeXFormData.Get(page, target);
            var output_xfrms = new List<VisioAutomation.Drawing.Point>(input_xfrms.Count);

            foreach (var input_xfrm in input_xfrms)
            {
                var old_rect = input_xfrm.GetRectangle();
                var old_lower_left = old_rect.LowerLeft;
                var new_lower_left = snap_grid.Snap(old_lower_left);
                var new_pin_position = ArrangeHelper.GetPinPositionForCorner(input_xfrm, new_lower_left, corner);
                var output_xfrm = new VisioAutomation.Drawing.Point(new_pin_position.X, new_pin_position.Y);
                output_xfrms.Add(output_xfrm);
            }

            ModifyPinPositions(page, target.ShapeIDs, output_xfrms);
        }


        private static VisioAutomation.Drawing.Point GetPinPositionForCorner(VisioScripting.Models.ShapeXFormData input_xfrm, VisioAutomation.Drawing.Point new_lower_left, VisioScripting.Models.SnapCornerPosition corner)
        {
            var size = new VisioAutomation.Drawing.Size(input_xfrm.Width, input_xfrm.Height);
            var locpin = new VisioAutomation.Drawing.Point(input_xfrm.LocPinX, input_xfrm.LocPinY);

            switch (corner)
            {
                case VisioScripting.Models.SnapCornerPosition.LowerLeft:
                    {
                        return new_lower_left.Add(locpin.X, locpin.Y);
                    }
                case VisioScripting.Models.SnapCornerPosition.UpperRight:
                    {
                        return new_lower_left.Subtract(size.Width, size.Height).Add(locpin.X, locpin.Y);
                    }
                case VisioScripting.Models.SnapCornerPosition.LowerRight:
                    {
                        return new_lower_left.Subtract(size.Width, 0).Add(locpin.X, locpin.Y);
                    }
                case VisioScripting.Models.SnapCornerPosition.UpperLeft:
                    {
                        return new_lower_left.Subtract(0, size.Height).Add(locpin.X, locpin.Y);
                    }
                default:
                    {
                        throw new System.ArgumentOutOfRangeException(nameof(corner), "Unsupported corner");
                    }
            }
        }

        public static void SnapSize(IVisio.Page page, VisioScripting.Models.TargetShapeIDs target, VisioAutomation.Drawing.Size snapsize, VisioAutomation.Drawing.Size minsize)
        {
            var input_xfrms = VisioScripting.Models.ShapeXFormData.Get(page, target);
            var sizes = new List<VisioAutomation.Drawing.Size>(input_xfrms.Count);

            var grid = new VisioScripting.Models.SnappingGrid(snapsize);
            foreach (var input_xfrm in input_xfrms)
            {
                // First snap the size to the grid
                double old_w = input_xfrm.Width;
                double old_h = input_xfrm.Height;
                var input_size = new VisioAutomation.Drawing.Size(old_w, old_h);
                var snapped_size = grid.Snap(input_size);

                // then account for any minum size requirements
                double new_w = System.Math.Max(snapped_size.Width, minsize.Width);
                double new_h = System.Math.Max(snapped_size.Height, minsize.Height);

                sizes.Add(new VisioAutomation.Drawing.Size(new_w, new_h));
            }

            // Now apply the updates to the sizes
            ModifySizes(page, target.ShapeIDs, sizes);
        }
    }
}