using VisioScripting;
using SMA = System.Management.Automation;

namespace VisioPowerShell.Models
{
    public class VisioPsClientContext : ClientContext
    {
        private readonly SMA.Cmdlet cmdlet;
        
        public VisioPsClientContext(SMA.Cmdlet cmdlet)
        {
            this.cmdlet = cmdlet;
        }

        public override void WriteDebug(string s)
        {
            this.cmdlet.WriteDebug(s);
        }

        public override void WriteError(string s)
        {
            this.cmdlet.WriteObject(s);
        }

        public override void WriteUser(string s)
        {
            this.cmdlet.WriteObject(s);
        }

        public override void WriteVerbose(string s)
        {
            this.cmdlet.WriteVerbose(s);
        }

        public override void WriteWarning(string s)
        {
            this.cmdlet.WriteWarning(s);
        }

    }
}