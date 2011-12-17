﻿using VA = VisioAutomation;
using System.Collections.Generic;
using System.Linq;
using IVisio = Microsoft.Office.Interop.Visio;

namespace VisioAutomation.ShapeSheet.Query
{
    public class CellQuery : QueryBase<CellQueryColumn>
    {
        public CellQuery() :
            base()
        {
        }

        public CellQueryColumn AddColumn(SRC src)
        {
            var col = new CellQueryColumn(this.Columns.Count, src, null);
            this.AddColumn(col);
            return col;
        }

        public CellQueryColumn AddColumn(SRC src, string name)
        {
            var col = new CellQueryColumn(this.Columns.Count, src, name);
            this.AddColumn(col);
            return col;
        }

        public VA.ShapeSheet.Data.QueryDataSet<T> GetFormulasAndResults<T>(IVisio.Shape shape)
        {
            var qds = this._Execute<T>(shape, true, true);
            return qds;
        }

        public VA.ShapeSheet.Data.Table<string> GetFormulas(IVisio.Shape shape)
        {
            var qds = this._Execute<double>(shape, true, false);
            return qds.Formulas;
        }

        public VA.ShapeSheet.Data.Table<T> GetResults<T>(IVisio.Shape shape)
        {
            var qds = this._Execute<T>(shape, false, true);
            return qds.Results;
        }

        private VA.ShapeSheet.Data.QueryDataSet<T> _Execute<T>(IVisio.Shape shape, bool getformulas, bool getresults)
        {
            if (shape == null)
            {
                throw new System.ArgumentNullException("shape");
            }


            int total_cells = Columns.Count;
            var unitcodes = CreateUnitCodeArray();

            if (getresults)
            {
                validate_unitcodes(unitcodes, total_cells);
            }

            var shapeids = new[] { shape.ID };
            var group_counts = new[] { 1 };
            int rowcount = shapeids.Count();
            
            // Build the Stream
            var srcs = this.Columns.Items.Select(col => col.SRC).ToList();

            var stream = VA.ShapeSheet.SRC.ToStream(srcs);
            var formulas = getformulas ? VA.ShapeSheet.Query.QueryUtil.GetFormulasU(shape, stream, srcs.Count) : null;
            var results = getresults ? VA.ShapeSheet.Query.QueryUtil.GetResults<T>(shape, stream, unitcodes, srcs.Count) : null;

            var qds = new VA.ShapeSheet.Data.QueryDataSet<T>(formulas, results, shapeids, this.Columns.Count, rowcount, group_counts);

            return qds;
        }

        public VA.ShapeSheet.Data.QueryDataSet<T> GetFormulasAndResults<T>(
                IVisio.Page page,
                IList<int> shapeids)
        {
            var qds = this._Execute<T>(page, shapeids, true, true);
            return qds;
        }

        public VA.ShapeSheet.Data.Table<string> GetFormulas(
            IVisio.Page page,
            IList<int> shapeids)
        {
            var qds = this._Execute<double>(page, shapeids, true, false);
            return qds.Formulas;
        }

        public VA.ShapeSheet.Data.Table<T> GetResults<T>(
            IVisio.Page page,
            IList<int> shapeids)
        {
            var qds = this._Execute<T>(page, shapeids, false, true);
            return qds.Results;
        }

        private VA.ShapeSheet.Data.QueryDataSet<T> _Execute<T>(
            IVisio.Page page,
            IList<int> shapeids, bool getformulas, bool getresults)
        {
            if (page == null)
            {
                throw new System.ArgumentNullException("page");
            }

            if (shapeids == null)
            {
                throw new System.ArgumentNullException("shapeids");
            }

            var srcs = Columns.Items.Select(i => i.SRC).ToList();
            var unitcodes = CreateUnitCodeArray();
            
            if (getresults)
            {
                validate_unitcodes(unitcodes, srcs.Count);
            }

            var groupcounts = new int[shapeids.Count];
            for (int i = 0; i < shapeids.Count; i++)
            {
                groupcounts[i] = 1;
            }
            int rowcount = shapeids.Count;
            int total_cells = rowcount * this.Columns.Count;

            // Build the Stream
            var sidsrcs = new List<VA.ShapeSheet.SIDSRC>(total_cells);
            foreach (var id in shapeids)
            {
                foreach (var src in srcs)
                {
                    var sidsrc = new VA.ShapeSheet.SIDSRC((short) id, src);
                    sidsrcs.Add(sidsrc);
                }
            }
            var stream = VA.ShapeSheet.SIDSRC.ToStream(sidsrcs);

            var formulas = getformulas ? VA.ShapeSheet.Query.QueryUtil.GetFormulasU(page, stream, sidsrcs.Count) : null;
            var results = getresults ? VA.ShapeSheet.Query.QueryUtil.GetResults<T>(page, stream, unitcodes, sidsrcs.Count) : null;

            var qds = new VA.ShapeSheet.Data.QueryDataSet<T>(formulas, results, shapeids, this.Columns.Count, rowcount, groupcounts);

            return qds;
        }
    }
}