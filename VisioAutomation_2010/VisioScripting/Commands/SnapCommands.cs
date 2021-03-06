namespace VisioScripting.Commands
{
    public class SnapCommands : CommandSet
    {
        internal SnapCommands(Client client) :
            base(client)
        {
        }

        public void SnapSize(VisioScripting.Models.TargetShapes targets, double w, double h)
        {
            this._client.Application.AssertApplicationAvailable();
            this._client.Document.AssertDocumentAvailable();

            var shapes = targets.ResolveShapes2D(this._client);
            if (shapes.Shapes.Count < 1)
            {
                return;
            }

            var application = this._client.Application.Get();
            var page = application.ActivePage;
            var target_ids = targets.ToShapeIDs();
            using (var undoscope = this._client.Application.NewUndoScope("Snape Shape Sizes"))
            {
                var snapsize = new VisioAutomation.Drawing.Size(w, h);
                var minsize = new VisioAutomation.Drawing.Size(w, h);
                VisioScripting.Helpers.ArrangeHelper.SnapSize(page, target_ids, snapsize, minsize);
            }
        }

        public void SnapCorner(VisioScripting.Models.TargetShapes targets, double w, double h, VisioScripting.Models.SnapCornerPosition corner)
        {
            this._client.Application.AssertApplicationAvailable();
            this._client.Document.AssertDocumentAvailable();

            var shapes = targets.ResolveShapes2D(this._client);

            if (shapes.Shapes.Count < 1)
            {
                return;
            }

            var application = this._client.Application.Get();
            var page = application.ActivePage;
            var target_ids = targets.ToShapeIDs();
            using (var undoscope = this._client.Application.NewUndoScope("Snap Shape Corner"))
            {
                VisioScripting.Helpers.ArrangeHelper.SnapCorner(page, target_ids, new VisioAutomation.Drawing.Size(w, h), corner);
            }
        }

        public void SnapSize(VisioScripting.Models.TargetShapes targets, VisioAutomation.Drawing.Size snapsize, VisioAutomation.Drawing.Size minsize)
        {
            this._client.Application.AssertApplicationAvailable();
            this._client.Document.AssertDocumentAvailable();

            var shapes = targets.ResolveShapes2D(this._client);

            if (shapes.Shapes.Count < 1)
            {
                return;
            }

            var application = this._client.Application.Get();
            var page = application.ActivePage;
            var target_ids = targets.ToShapeIDs();
            using (var undoscope = this._client.Application.NewUndoScope("Snap Shape Size"))
            {
                VisioScripting.Helpers.ArrangeHelper.SnapSize(page, target_ids, snapsize, minsize);
            }
        }
    }
}