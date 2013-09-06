﻿using System.Collections.Generic;
using System.Linq;
using VisioAutomation.Shapes;
using VA=VisioAutomation;
using IVisio = Microsoft.Office.Interop.Visio;

namespace VisioAutomation.Scripting.Commands
{
    public class FormatCommands : CommandSet
    {
        public FormatCommands(Session session) :
            base(session)
        {

        }

        public void Set(IList<IVisio.Shape> target_shapes, ShapeFormatCells format)
        {
            this.CheckVisioApplicationAvailable();
            this.CheckActiveDrawingAvailable();

            var shapes = this.GetTargetShapes(target_shapes);

            if (shapes.Count<1)
            {
                return;
            }

            var update = new VA.ShapeSheet.Update();
            var shapeids = shapes.Select(s => s.ID).ToList();

            foreach (int shapeid in shapeids)
            {
                update.SetFormulas((short)shapeid, format);
            }

            update.Execute(this.Session.VisioApplication.ActivePage);            
        }

        public IList<ShapeFormatCells> Get(IList<IVisio.Shape> target_shapes)
        {
            this.CheckVisioApplicationAvailable();
            this.CheckActiveDrawingAvailable();

            var shapes = this.GetTargetShapes(target_shapes);

            if (shapes.Count < 1)
            {
                return new List<ShapeFormatCells>(0);
            }

            var shapeids = shapes.Select(s => s.ID).ToList();
            var fmts = ShapeFormatCells.GetCells(this.Session.VisioApplication.ActivePage, shapeids);
            return fmts;
        }

        private double? cached_size_width;
        private double? cached_size_height;

        /// <summary>
        /// Caches the resize (the results, not formulas) of a the first currently selected shape
        /// </summary>
        public void CopySize()
        {
            this.CheckVisioApplicationAvailable();
            this.CheckActiveDrawingAvailable();
            
            if (!this.Session.HasSelectedShapes())
            {
                return;
            }

            var application = this.Session.VisioApplication;
            var active_window = application.ActiveWindow;
            var selection = active_window.Selection;
            var shape = selection[1];

            var query = new VA.ShapeSheet.Query.CellQuery();
            var width_col = query.Columns.Add(VA.ShapeSheet.SRCConstants.Width, "Width");
            var height_col = query.Columns.Add(VA.ShapeSheet.SRCConstants.Height, "Height");
            var queryresults = query.GetResults<double>(shape);

            cached_size_width = queryresults[width_col.Ordinal];
            cached_size_height = queryresults[height_col.Ordinal];
        }

        public void PasteSize(IList<IVisio.Shape> target_shapes, bool paste_width, bool paste_height)
        {
            this.CheckVisioApplicationAvailable();
            this.CheckActiveDrawingAvailable();
            
            var shapes = this.GetTargetShapes(target_shapes);

            if (shapes.Count < 1)
            {
                return;
            }

            if ((!cached_size_width.HasValue) && (!cached_size_height.HasValue))
            {
                return;
            }

            var update = new VA.ShapeSheet.Update();
            var shapeids = shapes.Select(s => s.ID).ToList();

            foreach (var shapeid in shapeids)
            {
                if (paste_width)
                {
                    update.SetFormula((short)shapeid, VA.ShapeSheet.SRCConstants.Width, cached_size_width.Value);
                }

                if (paste_height)
                {
                    update.SetFormula((short)shapeid, VA.ShapeSheet.SRCConstants.Height, cached_size_height.Value);
                }
            }

            var application = this.Session.VisioApplication;
            var active_page = application.ActivePage;
            update.Execute(active_page);
        }

        private readonly FormatPaintCache cache = new FormatPaintCache();

        public void Copy()
        {
            this.CheckVisioApplicationAvailable();
            this.CheckActiveDrawingAvailable();

            var allflags = this.cache.GetAllFormatPaintFlags();
            this.Copy(null, allflags);
        }

        public void Copy(IVisio.Shape target_shape, FormatCategory category)
        {
            this.CheckVisioApplicationAvailable();
            this.CheckActiveDrawingAvailable();

            var shape = GetTargetShape(target_shape);
            if (shape == null)
            {
                return;
            }

            this.cache.CopyFormat(shape, category);
        }

        public void ClearFormatCache()
        {
            this.cache.Clear();
        }

        public void Paste(IList<IVisio.Shape> target_shapes, FormatCategory category, bool apply_formulas)
        {
            this.CheckVisioApplicationAvailable();
            this.CheckActiveDrawingAvailable();

            var shapes = GetTargetShapes(target_shapes);
            if (shapes.Count < 1)
            {
                return;
            }
 
            var shapeids = target_shapes.Select(s=>s.ID).ToList();
            var application = this.Session.VisioApplication;
            var active_page = application.ActivePage;

            this.cache.PasteFormat(active_page, shapeids, category, apply_formulas);
        }
    }
}