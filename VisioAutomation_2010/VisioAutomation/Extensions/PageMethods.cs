using System.Collections.Generic;
using IVisio = Microsoft.Office.Interop.Visio;
using System.Linq;
using VA = VisioAutomation;

namespace VisioAutomation.Extensions
{
    public static class PageMethods
    {
        public static void ResizeToFitContents(this IVisio.Page page, VA.Drawing.Size padding)
        {
            VA.Pages.PageHelper.ResizeToFitContents(page, padding);
        }

        public static IVisio.Shape DrawLine(this IVisio.Page page, VA.Drawing.Point p1, VA.Drawing.Point p2)
        {
            var shape = page.DrawLine(p1.X, p1.Y, p2.X, p2.Y);
            return shape;
        }

        public static IVisio.Shape DrawOval(this IVisio.Page page, VA.Drawing.Rectangle rect)
        {
            var shape = page.DrawOval(rect.Left, rect.Bottom, rect.Right, rect.Top);
            return shape;
        }

        public static IVisio.Shape DrawOval(this IVisio.Master master, VA.Drawing.Rectangle rect)
        {
            var shape = master.DrawOval(rect.Left, rect.Bottom, rect.Right, rect.Top);
            return shape;
        }

        public static IVisio.Shape DrawRectangle(this IVisio.Page page, VA.Drawing.Rectangle rect)
        {
            var shape = page.DrawRectangle(rect.Left, rect.Bottom, rect.Right, rect.Top);
            return shape;
        }

        public static IVisio.Shape DrawBezier(this IVisio.Page page, IList<VA.Drawing.Point> points)
        {
            short degree = 3;
            short flags = 0;
            var shape = page.DrawBezier(points, degree, flags);
            return shape;
        }

        public static IVisio.Shape DrawBezier(this IVisio.Master master, IList<VA.Drawing.Point> points)
        {
            short degree = 3;
            short flags = 0;
            var shape = master.DrawBezier(points, degree, flags);
            return shape;
        }


        public static IVisio.Shape DrawBezier(this IVisio.Page page, IList<VA.Drawing.Point> points, short degree, short flags)
        {
            // DrawBezier method: http://msdn.microsoft.com/en-us/library/office/ff766273.aspx
            var doubles_array = VA.Drawing.Point.ToDoubles(points).ToArray();
            var shape = page.DrawBezier(doubles_array, degree, flags);
            return shape;
        }

        public static IVisio.Shape DrawBezier(this IVisio.Master master, IList<VA.Drawing.Point> points, short degree, short flags)
        {
            // DrawBezier method: http://msdn.microsoft.com/en-us/library/office/ff766273.aspx
            var doubles_array = VA.Drawing.Point.ToDoubles(points).ToArray();
            var shape = master.DrawBezier(doubles_array, degree, flags);
            return shape;
        }


        public static IVisio.Shape DrawPolyline(this IVisio.Page page, IList<VA.Drawing.Point> points)
        {
            var doubles_array = VA.Drawing.Point.ToDoubles(points).ToArray();
            var shape = page.DrawPolyline(doubles_array, 0);
            return shape;
        }

        public static IVisio.Shape DrawPolyline(this IVisio.Master master, IList<VA.Drawing.Point> points)
        {
            var doubles_array = VA.Drawing.Point.ToDoubles(points).ToArray();
            var shape = master.DrawPolyline(doubles_array, 0);
            return shape;
        }


        public static IVisio.Shape DrawNURBS(this IVisio.Page page, IList<VA.Drawing.Point> controlpoints,
                                             IList<double> knots,
                                             IList<double> weights, int degree)
        {
            // flags:
            // None = 0,
            // IVisio.VisDrawSplineFlags.visSpline1D

            var flags = 0;
            double[] pts_dbl_a = VA.Drawing.Point.ToDoubles(controlpoints).ToArray();
            double[] kts_dbl_a = knots.ToArray();
            double[] weights_dbl_a = weights.ToArray();

            var shape = page.DrawNURBS((short) degree, (short) flags, pts_dbl_a, kts_dbl_a, weights_dbl_a);
            return shape;
        }

        public static IVisio.Shape Drop(
            this IVisio.Page page,
            IVisio.Master master,
            VA.Drawing.Point point)
        {
            if (master == null)
            {
                throw new System.ArgumentNullException("master");
            }

            return page.Drop(master, point.X, point.Y);
        }

        public static short[] DropManyU(
            this IVisio.Page page,
            IList<IVisio.Master> masters,
            IEnumerable<VA.Drawing.Point> points)
        {
            short[] shapeids = VA.Pages.PageHelper.DropManyU(page, masters, points);
            return shapeids;
        }

        public static IEnumerable<IVisio.Page> AsEnumerable(this IVisio.Pages pages)
        {
            short count = pages.Count;
            for (int i = 0; i < count; i++)
            {
                yield return pages[i + 1];
            }
        }

        public static string[] GetNamesU(this IVisio.Pages pages)
        {
            System.Array names_sa;
            pages.GetNamesU(out names_sa);
            string[] names = (string[])names_sa;
            return names;
        }
    }
}