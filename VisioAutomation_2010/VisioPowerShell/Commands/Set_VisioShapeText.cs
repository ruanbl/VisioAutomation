using System.Management.Automation;
using IVisio = Microsoft.Office.Interop.Visio;

namespace VisioPowerShell.Commands
{
    [Cmdlet(VerbsCommon.Set, VisioPowerShell.Nouns.VisioShapeText)]
    public class Set_VisioShapeText : VisioCmdlet
    {
        [Parameter(Position = 0, Mandatory = true)]
        public string[] Text { get; set; }

        [Parameter(Mandatory = false)]
        public IVisio.Shape[] Shapes;

        protected override void ProcessRecord()
        {
            var targets = new VisioScripting.Models.TargetShapes(this.Shapes);
            this.Client.Text.Set(targets, this.Text);
        }
    }
}
