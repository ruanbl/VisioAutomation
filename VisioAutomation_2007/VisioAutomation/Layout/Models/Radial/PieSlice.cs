﻿using System;
using System.Collections.Generic;
using System.Linq;
using VisioAutomation.Drawing;
using VA = VisioAutomation;
using IVisio = Microsoft.Office.Interop.Visio;
using VisioAutomation.Extensions;

namespace VisioAutomation.Layout.Models.Radial
{
    public class PieSlice : RadialSlice
    {
        public double Radius { get; private set; }

        public PieSlice(Point center, double start, double end, double radius) :
            base(center,start,end)
        {
            if (radius < 0.0)
            {
                throw new System.ArgumentException("radius","must be non-negative");
            }

            this.Radius = radius;
        }


        internal List<Point> GetShapeBezier(out int degree)
        {
            this.check_normal_angle();

            var arc_bez = this.GetArcBez(this.Radius, out degree);

            // Create one big bezier that accounts for the entire pie shape. This includes the arc
            // calculated above and the sides of the pie slice
            var pie_bez = new List<VA.Drawing.Point>(3 + arc_bez.Count + 3);

            var point_first = arc_bez[0];
            var point_last = arc_bez[arc_bez.Count - 1];

            pie_bez.Add(this.Center);
            pie_bez.Add(this.Center);
            pie_bez.Add(point_first);
            pie_bez.AddRange(arc_bez);
            pie_bez.Add(point_last);
            pie_bez.Add(this.Center);
            pie_bez.Add(this.Center);
            return pie_bez;
        }

        public IVisio.Shape Render( IVisio.Page page)
        {

            if (this.Sector.Angle == 0.0)
            {
                var p1 = this.GetPointAtRadius(this.Center, this.Sector.StartAngle, this.Radius);
                return page.DrawLine(this.Center, p1);
            }
            else if (this.Sector.Angle >= System.Math.PI)
            {
                var A = this.Center.Add(-this.Radius, -this.Radius);
                var B = this.Center.Add(this.Radius, this.Radius);
                var rect = new VA.Drawing.Rectangle(A, B);
                var shape = page.DrawOval(rect);
                return shape;
            }
            else
            {
                int degree;
                var pie_bez = this.GetShapeBezier(out degree);

                // Render the bezier
                var doubles_array = VA.Drawing.Point.ToDoubles(pie_bez).ToArray();
                var pie_slice = page.DrawBezier(doubles_array, (short)degree, 0);
                return pie_slice;
            }
        }

        public static List<PieSlice> GetSlicesFromValues(Point center, double radius, IList<double> values)
        {
            var slices = RadialSlice.GetSlicesFromValues(center, values,
                                                         sector =>
                                                         new PieSlice(center, sector.StartAngle, sector.EndAngle, radius));
            return slices;
        }
    }
}