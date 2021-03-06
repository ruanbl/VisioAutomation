using System.Management.Automation;
using IVisio = Microsoft.Office.Interop.Visio;

namespace VisioPowerShell.Commands
{
    [Cmdlet(VerbsCommon.Copy, VisioPowerShell.Nouns.VisioPage)]
    public class Copy_VisioPage : VisioCmdlet
    {
        [Parameter(Mandatory = false)]
        public IVisio.Document ToDocument=null;

        protected override void ProcessRecord()
        {
            IVisio.Page newpage;
            if (this.ToDocument == null)
            {
                newpage = this.Client.Page.Duplicate();
            }
            else
            {
                newpage = this.Client.Page.Duplicate(this.ToDocument);
            }

            this.WriteObject(newpage);            
        }
    }
}