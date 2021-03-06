using IVisio = Microsoft.Office.Interop.Visio;
using VisioAutomation.Extensions;

namespace VisioScripting.Commands
{
    public class ViewCommands : CommandSet
    {
        private readonly double ZoomIncrement;

        internal ViewCommands(Client client) :
            base(client)
        {
            this.ZoomIncrement = 1.20;
        }

        public IVisio.Window GetActiveWindow()
        {
            this._client.Application.AssertApplicationAvailable();

            var application = this._client.Application.Get();
            var active_window = application.ActiveWindow;
            return active_window;
        }

        public double GetActiveZoom()
        {
            this._client.Application.AssertApplicationAvailable();

            var active_window = this.GetActiveWindow();
            return active_window.Zoom;
        }

        private static void SetViewRectToSelection(IVisio.Window window,
                                                   IVisio.VisBoundingBoxArgs bbargs, 
                                                   double padding_scale)
        {
            if (padding_scale < 0.0)
            {
                throw new System.ArgumentOutOfRangeException(nameof(padding_scale));
            }

            if (padding_scale > 1.0)
            {
                throw new System.ArgumentOutOfRangeException(nameof(padding_scale));
            }

            var app = window.Application;
            var active_window = app.ActiveWindow;
            var sel = active_window.Selection;
            var sel_bb = sel.GetBoundingBox(bbargs);

            var delta = sel_bb.Size*padding_scale;
            var view_rect = new VisioAutomation.Drawing.Rectangle(sel_bb.Left - delta.Width, sel_bb.Bottom - delta.Height,
                                                          sel_bb.Right + delta.Height, sel_bb.Top + delta.Height);
            window.SetViewRect(view_rect);
        }

        public void ZoomToPercentage(double amount)
        {
            this._client.Application.AssertApplicationAvailable();
            this._client.Document.AssertDocumentAvailable();

            if (amount <= 0)
            {
                return;
            }

            var active_window = this.GetActiveWindow();
            active_window.Zoom = amount;
        }

        public void Zoom(VisioScripting.Models.Zoom zoom)
        {
            this._client.Application.AssertApplicationAvailable();

            var active_window = this.GetActiveWindow();

            if (zoom == Models.Zoom.Out)
            {
                var cur = active_window.Zoom;
                this.ZoomToPercentage(cur / this.ZoomIncrement);                
            }
            else if (zoom == Models.Zoom.In)
            {
                var cur = active_window.Zoom;
                this.ZoomToPercentage(cur * this.ZoomIncrement);
            }
            else if (zoom == Models.Zoom.ToPage)
            {
                active_window.ViewFit = (short)IVisio.VisWindowFit.visFitPage;
            }
            else if (zoom == Models.Zoom.ToWidth)
            {
                active_window.ViewFit = (short)IVisio.VisWindowFit.visFitWidth;
            }
            else if (zoom == Models.Zoom.ToSelection)
            {
                if (!this._client.Selection.HasShapes())
                {
                    return;
                }

                double padding_scale = 0.1;
                ViewCommands.SetViewRectToSelection(active_window, IVisio.VisBoundingBoxArgs.visBBoxExtents, padding_scale);

            }
            else
            {
                throw new System.ArgumentOutOfRangeException(nameof(zoom));
            }            
        }
    }
}