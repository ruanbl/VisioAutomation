﻿using VA = VisioAutomation;
using System.Collections.Generic;
using System.Linq;
using IVisio = Microsoft.Office.Interop.Visio;

namespace VisioAutomation.ShapeSheet.Query
{
   public partial class CellQuery
    {

       public class SectionSubQuery
       {
           public short SectionIndex { get; private set; }
           public List<Column> Columns { get; private set; }
           public int Ordinal { get; private set; }

           public SectionSubQuery(int ordinal, short section)
           {
               this.Ordinal = ordinal;
               this.SectionIndex = section;
               this.Columns = new List<Column>();
           }

           public static implicit operator int(SectionSubQuery m)
           {
               return m.Ordinal;
           }

           public Column AddColumn(SRC src, string name)
           {
               int ordinal = this.Columns.Count;
               if (src.Section != this.SectionIndex)
               {
                   throw new VA.AutomationException("SRC's Section does not match");
               }
               var col = new Column(ordinal, src, name);
               this.Columns.Add(col);
               return col;
           }

           public Column AddColumn(short cell, string name)
           {
               int ordinal = this.Columns.Count;
               var col = new Column(ordinal, cell, name);
               this.Columns.Add(col);
               return col;
           }
       }
    }
}