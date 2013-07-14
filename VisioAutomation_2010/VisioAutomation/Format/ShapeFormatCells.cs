﻿using System.Collections.Generic;
using VisioAutomation.ShapeSheet;
using VisioAutomation.ShapeSheet.Query;
using IVisio = Microsoft.Office.Interop.Visio;
using VA = VisioAutomation;
using VisioAutomation.Extensions;

namespace VisioAutomation.Format
{
    public class ShapeFormatCells : VA.ShapeSheet.CellGroups.CellGroup
    {
        public VA.ShapeSheet.CellData<int> FillBkgnd { get; set; }
        public VA.ShapeSheet.CellData<double> FillBkgndTrans { get; set; }
        public VA.ShapeSheet.CellData<int> FillForegnd { get; set; }
        public VA.ShapeSheet.CellData<double> FillForegndTrans { get; set; }
        public VA.ShapeSheet.CellData<int> FillPattern { get; set; }
        public VA.ShapeSheet.CellData<double> ShapeShdwObliqueAngle { get; set; }
        public VA.ShapeSheet.CellData<double> ShapeShdwOffsetX { get; set; }
        public VA.ShapeSheet.CellData<double> ShapeShdwOffsetY { get; set; }
        public VA.ShapeSheet.CellData<double> ShapeShdwScaleFactor { get; set; }
        public VA.ShapeSheet.CellData<int> ShapeShdwType { get; set; }
        public VA.ShapeSheet.CellData<int> ShdwBkgnd { get; set; }
        public VA.ShapeSheet.CellData<double> ShdwBkgndTrans { get; set; }
        public VA.ShapeSheet.CellData<int> ShdwForegnd { get; set; }
        public VA.ShapeSheet.CellData<double> ShdwForegndTrans { get; set; }
        public VA.ShapeSheet.CellData<int> ShdwPattern { get; set; }
        public VA.ShapeSheet.CellData<int> BeginArrow { get; set; }
        public VA.ShapeSheet.CellData<double> BeginArrowSize { get; set; }
        public VA.ShapeSheet.CellData<int> EndArrow { get; set; }
        public VA.ShapeSheet.CellData<double> EndArrowSize { get; set; }
        public VA.ShapeSheet.CellData<int> LineCap { get; set; }
        public VA.ShapeSheet.CellData<int> LineColor { get; set; }
        public VA.ShapeSheet.CellData<double> LineColorTrans { get; set; }
        public VA.ShapeSheet.CellData<int> LinePattern { get; set; }
        public VA.ShapeSheet.CellData<double> LineWeight { get; set; }
        public VA.ShapeSheet.CellData<double> Rounding { get; set; }

        public override void ApplyFormulas(ApplyFormula func)
        {
            func(ShapeSheet.SRCConstants.FillBkgnd, this.FillBkgnd.Formula);
            func(ShapeSheet.SRCConstants.FillBkgndTrans, this.FillBkgndTrans.Formula);
            func(ShapeSheet.SRCConstants.FillForegnd, this.FillForegnd.Formula);
            func(ShapeSheet.SRCConstants.FillForegndTrans, this.FillForegndTrans.Formula);
            func(ShapeSheet.SRCConstants.FillPattern, this.FillPattern.Formula);
            func(ShapeSheet.SRCConstants.ShapeShdwObliqueAngle, this.ShapeShdwObliqueAngle.Formula);
            func(ShapeSheet.SRCConstants.ShapeShdwOffsetX, this.ShapeShdwOffsetX.Formula);
            func(ShapeSheet.SRCConstants.ShapeShdwOffsetY, this.ShapeShdwOffsetY.Formula);
            func(ShapeSheet.SRCConstants.ShapeShdwScaleFactor, this.ShapeShdwScaleFactor.Formula);
            func(ShapeSheet.SRCConstants.ShapeShdwType, this.ShapeShdwType.Formula);
            func(ShapeSheet.SRCConstants.ShdwBkgnd, this.ShdwBkgnd.Formula);
            func(ShapeSheet.SRCConstants.ShdwBkgndTrans, this.ShdwBkgndTrans.Formula);
            func(ShapeSheet.SRCConstants.ShdwForegnd, this.ShdwForegnd.Formula);
            func(ShapeSheet.SRCConstants.ShdwForegndTrans, this.ShdwForegndTrans.Formula);
            func(ShapeSheet.SRCConstants.ShdwPattern, this.ShdwPattern.Formula);
            func(ShapeSheet.SRCConstants.BeginArrow, this.BeginArrow.Formula);
            func(ShapeSheet.SRCConstants.BeginArrowSize, this.BeginArrowSize.Formula);
            func(ShapeSheet.SRCConstants.EndArrow, this.EndArrow.Formula);
            func(ShapeSheet.SRCConstants.EndArrowSize, this.EndArrowSize.Formula);
            func(ShapeSheet.SRCConstants.LineCap, this.LineCap.Formula);
            func(ShapeSheet.SRCConstants.LineColor, this.LineColor.Formula);
            func(ShapeSheet.SRCConstants.LineColorTrans, this.LineColorTrans.Formula);
            func(ShapeSheet.SRCConstants.LinePattern, this.LinePattern.Formula);
            func(ShapeSheet.SRCConstants.LineWeight, this.LineWeight.Formula);
            func(ShapeSheet.SRCConstants.Rounding, this.Rounding.Formula);
        }


        public static IList<ShapeFormatCells> GetCells(IVisio.Page page, IList<int> shapeids)
        {
            var query = get_query();
            return VA.ShapeSheet.CellGroups.CellGroup._GetCells(page, shapeids, query, query.GetCells);
        }

        public static ShapeFormatCells GetCells(IVisio.Shape shape)
        {
            var query = get_query();
            return VA.ShapeSheet.CellGroups.CellGroup._GetCells(shape, query, query.GetCells);
        }

        private static ShapeFormatCellQuery _mCellQuery;
        private static ShapeFormatCellQuery get_query()
        {
            _mCellQuery = _mCellQuery ?? new ShapeFormatCellQuery();
            return _mCellQuery;
        }

        class ShapeFormatCellQuery : VA.ShapeSheet.Query.CellQuery
        {
            public Column FillBkgnd { get; set; }
            public Column FillBkgndTrans { get; set; }
            public Column FillForegnd { get; set; }
            public Column FillForegndTrans { get; set; }
            public Column FillPattern { get; set; }
            public Column ShapeShdwObliqueAngle { get; set; }
            public Column ShapeShdwOffsetX { get; set; }
            public Column ShapeShdwOffsetY { get; set; }
            public Column ShapeShdwScaleFactor { get; set; }
            public Column ShapeShdwType { get; set; }
            public Column ShdwBkgnd { get; set; }
            public Column ShdwBkgndTrans { get; set; }
            public Column ShdwForegnd { get; set; }
            public Column ShdwForegndTrans { get; set; }
            public Column ShdwPattern { get; set; }
            public Column BeginArrow { get; set; }
            public Column BeginArrowSize { get; set; }
            public Column EndArrow { get; set; }
            public Column EndArrowSize { get; set; }
            public Column LineColor { get; set; }
            public Column LineCap { get; set; }
            public Column LineColorTrans { get; set; }
            public Column LinePattern { get; set; }
            public Column LineWeight { get; set; }
            public Column Rounding { get; set; }

            public ShapeFormatCellQuery()
            {
                this.FillBkgnd = this.AddColumn(VA.ShapeSheet.SRCConstants.FillBkgnd, "FillBkgnd");
                this.FillBkgndTrans = this.AddColumn(VA.ShapeSheet.SRCConstants.FillBkgndTrans, "FillBkgndTrans");
                this.FillForegnd = this.AddColumn(VA.ShapeSheet.SRCConstants.FillForegnd, "FillForegnd");
                this.FillForegndTrans = this.AddColumn(VA.ShapeSheet.SRCConstants.FillForegndTrans, "FillForegndTrans");
                this.FillPattern = this.AddColumn(VA.ShapeSheet.SRCConstants.FillPattern, "FillPattern");
                this.ShapeShdwObliqueAngle = this.AddColumn(VA.ShapeSheet.SRCConstants.ShapeShdwObliqueAngle, "ShapeShdwObliqueAngle");
                this.ShapeShdwOffsetX = this.AddColumn(VA.ShapeSheet.SRCConstants.ShapeShdwOffsetX, "ShapeShdwOffsetX");
                this.ShapeShdwOffsetY = this.AddColumn(VA.ShapeSheet.SRCConstants.ShapeShdwOffsetY, "ShapeShdwOffsetY");
                this.ShapeShdwScaleFactor = this.AddColumn(VA.ShapeSheet.SRCConstants.ShapeShdwScaleFactor, "ShapeShdwScaleFactor");
                this.ShapeShdwType = this.AddColumn(VA.ShapeSheet.SRCConstants.ShapeShdwType, "ShapeShdwType");
                this.ShdwBkgnd = this.AddColumn(VA.ShapeSheet.SRCConstants.ShdwBkgnd, "ShdwBkgnd ");
                this.ShdwBkgndTrans = this.AddColumn(VA.ShapeSheet.SRCConstants.ShdwBkgndTrans, "ShdwBkgndTrans");
                this.ShdwForegnd = this.AddColumn(VA.ShapeSheet.SRCConstants.ShdwForegnd, "ShdwForegnd ");
                this.ShdwForegndTrans = this.AddColumn(VA.ShapeSheet.SRCConstants.ShdwForegndTrans, "ShdwForegndTrans");
                this.ShdwPattern = this.AddColumn(VA.ShapeSheet.SRCConstants.ShdwPattern, "ShdwPattern");

                this.BeginArrow = this.AddColumn(VA.ShapeSheet.SRCConstants.BeginArrow, "BeginArrow");
                this.BeginArrowSize = this.AddColumn(VA.ShapeSheet.SRCConstants.BeginArrowSize, "BeginArrowSize");
                this.EndArrow = this.AddColumn(VA.ShapeSheet.SRCConstants.EndArrow, "EndArrow ");
                this.EndArrowSize = this.AddColumn(VA.ShapeSheet.SRCConstants.EndArrowSize, "EndArrowSize");
                this.LineColor = this.AddColumn(VA.ShapeSheet.SRCConstants.LineColor, "LineColor");
                this.LineCap = this.AddColumn(VA.ShapeSheet.SRCConstants.LineCap, "LineCap");
                this.LineColorTrans = this.AddColumn(VA.ShapeSheet.SRCConstants.LineColorTrans, "LineColorTrans");
                this.LinePattern = this.AddColumn(VA.ShapeSheet.SRCConstants.LinePattern, "LinePattern");
                this.LineWeight = this.AddColumn(VA.ShapeSheet.SRCConstants.LineWeight, "LineWeight");
                this.Rounding = this.AddColumn(VA.ShapeSheet.SRCConstants.Rounding, "Rounding");
            }

            public ShapeFormatCells GetCells(QueryResult<CellData<double>> row)
            {
                var cells = new ShapeFormatCells();
                cells.FillBkgnd = row[ this.FillBkgnd.Ordinal].ToInt();
                cells.FillBkgndTrans = row[ this.FillBkgndTrans.Ordinal];
                cells.FillForegnd = row[ this.FillForegnd.Ordinal].ToInt();
                cells.FillForegndTrans = row[ this.FillForegndTrans.Ordinal];
                cells.FillPattern = row[ this.FillPattern.Ordinal].ToInt();
                cells.ShapeShdwObliqueAngle = row[ this.ShapeShdwObliqueAngle.Ordinal];
                cells.ShapeShdwOffsetX = row[ this.ShapeShdwOffsetX.Ordinal];
                cells.ShapeShdwOffsetY = row[ this.ShapeShdwOffsetY.Ordinal];
                cells.ShapeShdwScaleFactor = row[ this.ShapeShdwScaleFactor.Ordinal];
                cells.ShapeShdwType = row[ this.ShapeShdwType.Ordinal].ToInt();
                cells.ShdwBkgnd = row[ this.ShdwBkgnd.Ordinal].ToInt();
                cells.ShdwBkgndTrans = row[ this.ShdwBkgndTrans.Ordinal];
                cells.ShdwForegnd = row[ this.ShdwForegnd.Ordinal].ToInt();
                cells.ShdwForegndTrans = row[ this.ShdwForegndTrans.Ordinal];
                cells.ShdwPattern = row[ this.ShdwPattern.Ordinal].ToInt();
                cells.BeginArrow = row[ this.BeginArrow.Ordinal].ToInt();
                cells.BeginArrowSize = row[ this.BeginArrowSize.Ordinal];
                cells.EndArrow = row[ this.EndArrow.Ordinal].ToInt();
                cells.EndArrowSize = row[ this.EndArrowSize.Ordinal];
                cells.LineCap = row[ this.LineCap.Ordinal].ToInt();
                cells.LineColor = row[ this.LineColor.Ordinal].ToInt();
                cells.LineColorTrans = row[ this.LineColorTrans.Ordinal];
                cells.LinePattern = row[ this.LinePattern.Ordinal].ToInt();
                cells.LineWeight = row[ this.LineWeight.Ordinal];
                cells.Rounding = row[ this.Rounding.Ordinal];
                return cells;
            }

        }
    }
}