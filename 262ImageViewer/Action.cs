using _262ImageViewer;
using System;
using System.Diagnostics;
using System.Drawing;
using System.Windows;

namespace Action
{
    /*
     * The Action system. Actions are in a chain, and are undoable.
     */
    [Serializable]
    public abstract class Action
    {
        /*
         * The next action down the chain
         */
        protected Action nextAction;

        /*
         * Run the action on the particular study.
         */
        public abstract void run(MainWindow app);
       
        /*
         * Run the next action.
         */
        protected void runNext(MainWindow app)
        {
            Study study = app.studySession;
            if (this.nextAction != null)
                this.nextAction.run(app);
        }

        /*
         * Undo the action
         */
        public abstract void undo(MainWindow study);

        /*
         * Get the next action down the chain, returns null if at the end
         */
        public Action next()
        {
            return this.nextAction;
        }

        /*
         * Set the next action of this action
         */
        public void setNext(Action action)
        {
            this.nextAction = action;
        }

        /*
         * Remove the next action in the chain
         */
        public void removeNext()
        {
            this.nextAction = null;
        }

    }

    /*
     * Actions related to the GridView
     */
    namespace Grid
    {
        /*
         * Next behavior
         */
        [Serializable]
        public class Next : Action
        {
            public override void run(MainWindow app)
            {
                app.imageView.nextImage();
                // Call base
                base.runNext(app);
            }

            public override void undo(MainWindow app)
            {
                app.imageView.prevImage();
            }

            public override string ToString()
            {
                return "nextImage -> " + (this.nextAction != null ? this.nextAction.ToString() : "end");
            }
        }

        /*
         * Previous
         */
        [Serializable]
        public class Previous : Action
        {
            public override void run(MainWindow app)
            {
                app.imageView.prevImage();
                // Call base
                base.runNext(app);
            }

            public override void undo(MainWindow app)
            {
                app.imageView.nextImage();
            }

            public override string ToString()
            {
                return "prevImage -> " + (this.nextAction != null ? this.nextAction.ToString() : "end");
            }
        }

        /*
         * Toggle between 4- and 1-up display modes.
         */
        [Serializable]
        public class Toggle : Action
        {
            public override void run(MainWindow app)
            {
                app.imageView.switchMode();
                // Call base
                base.runNext(app);
            }

            public override void undo(MainWindow app)
            {
                app.imageView.switchMode();
            }

            public override string ToString()
            {
                return "toggleLayout -> " + (this.nextAction != null ? this.nextAction.ToString() : "end");
            }
        }
    }

    /*
     * Actions related to Statistical Analysis
     */
    namespace Analysis
    {
        /*
         * Creation
         */
        [Serializable]
        public class Create : Action
        {
            AnalysisView analysis;
            public Create(Bitmap bi) 
            {
                analysis = new AnalysisView(bi);
            }

            public override void run(MainWindow main) 
            {
                // Create and display a window with the analysis.
                Window win = new Window();
                win.Content = analysis;
                win.SizeToContent = SizeToContent.WidthAndHeight;
                win.Title = "Histogram";
                win.Show();
                base.runNext(main);
            }
            public override void undo(MainWindow app) 
            {
                // Analysis creation is not undo-able.
            }
            public override string ToString() { return "Analysis.Create -> " + (this.nextAction != null ? this.nextAction.ToString() : "end"); }
        }

        /*
         * Close action
         */
        [Serializable]
        public class Close : Action
        {
            public Close() {}
            public override void run(MainWindow app)
            {
                app.setFrameImageView(app.imageView);
            }
            public override void undo(MainWindow app)
            {
                // Analysis close is not undo-able
            }
            public override string ToString() { return "Analysis.Close -> " + (this.nextAction != null ? this.nextAction.ToString() : "end"); }
        }
    }

    /*
     * Actions related to Reconstruction
     */
    namespace Reconstruction
    {
        /*
         * Creation
         */
        [Serializable]
        public class Create : Action
        {
            public override void run(MainWindow app)
            {
                ReconstructionView rec = new ReconstructionView(app.studySession.imageCollection, app.imageView.index, false, app.studySession);
                app.reconstructionView = rec;
                app.setFrameImageView(rec);
                base.runNext(app);
            }

            public override void undo(MainWindow app) 
            {
                Action a = new Close();
                a.run(app);
            }

            public override string ToString() { return "Reconstruction.Create -> " + (this.nextAction != null ? this.nextAction.ToString() : "end"); }
        }

        /*
         * Close
         */
        [Serializable]
        public class Close : Action
        {
            public override void run(MainWindow app) 
            {
                app.setFrameImageView(app.imageView);
                base.runNext(app);            
            }

            public override void undo(MainWindow app) 
            {
                Action a = new Create();
                a.run(app);
            }

            public override string ToString() { return "Reconstruction.Close -> " + (this.nextAction != null ? this.nextAction.ToString() : "end"); }
        }

        /*
         * Next Reconstruction. Move to the next "slice" of the rebuilt image.
         */
        [Serializable]
        public class NextReconstruction : Action
        {
            public override void run(MainWindow app) 
            {
                app.reconstructionView.nextReconstruction();
                // Call base
                base.runNext(app);
            }

            public override void undo(MainWindow app) 
            {
                Action a = new PreviousReconstruction();
                a.run(app);
            }

            public override string ToString() { return "NextReconstruction -> " + (this.nextAction != null ? this.nextAction.ToString() : "end"); }
        }

        /*
         * Previous Reconstruction. Move to the previous "slice" of the rebuilt image.
         */
        [Serializable]
        public class PreviousReconstruction : Action
        {
            public override void run(MainWindow app) 
            {
                app.reconstructionView.previousReconstruction();
                // Call base
                base.runNext(app);
            }

            public override void undo(MainWindow app)
            {
                Action a = new NextReconstruction();
                a.run(app);
            }

            public override string ToString() { return "PreviousReconstruction -> " + (this.nextAction != null ? this.nextAction.ToString() : "end"); }
        }

        /*
         * Next. Move to the next image in the study.
         */
        [Serializable]
        public class Next : Action
        {
            public override void run(MainWindow app) 
            {
                app.reconstructionView.nextImage();
                // Call base
                base.runNext(app);
            }

            public override void undo(MainWindow app)
            {
                Action a = new Previous();
                a.run(app);
            }

            public override string ToString() { return "Reconstruction.Next -> " + (this.nextAction != null ? this.nextAction.ToString() : "end"); }
        }

        /*
         * Previous. Move to the previous image in the study.
         */
        [Serializable]
        public class Previous : Action
        {
            public override void run(MainWindow app) 
            {
                app.reconstructionView.prevImage();
                // Call base
                base.runNext(app);
            }

            public override void undo(MainWindow app)
            {
                Action a = new Next();
                a.run(app);
            }

            public override string ToString() { return "Reconstruction.Previous -> " + (this.nextAction != null ? this.nextAction.ToString() : "end"); }
        }
    }

    /*
     * Actions related to Windowing
     */
    namespace Windowing
    {
        /*
         * Creation
         */
        [Serializable]
        public class Create : Action
        {
            float high = -1;
            float low = -1;

            public override void run(MainWindow app)
            {
                /**
                WindowingPrompt w = new WindowingPrompt(app);
                w.Title = "Windowing";
                w.Show();
                high = w.high_cut;
                low = w.low_cut;
                base.runNext(app);
                 **/
                if (high == -1 || low == -1)
                {
                    high = app.winPrompt.high_cut;
                    low = app.winPrompt.low_cut;
                }
                WindowingView winView = new WindowingView(high, low, app);
                app.setFrameImageView(winView);
                app.windowingView = winView;
            }

            public override void undo(MainWindow app)
            {
                Action a = new Close();
                a.run(app);
            }

            public override string ToString() { return null; }
        }

        /*
         * Close a WindowingView
         */
        [Serializable]
        public class Close : Action
        {
            public override void run(MainWindow app)
            {
                app.setFrameImageView(app.imageView);
                base.runNext(app);
            }

            public override void undo(MainWindow app)
            {
                app.setFrameImageView(app.windowingView);
            }

            public override string ToString() { return null; }
        }

        /*
         * Next.
         */
        [Serializable]
        public class Next : Action
        {
            public override void run(MainWindow app)
            {
                app.windowingView.nextImage();
                base.runNext(app);
            }
            public override void undo(MainWindow app)
            {
                var a = new Previous();
                a.run(app);
            }

            public override string ToString() { return null; }
        }

        /*
         * Previous.
         */
        [Serializable]
        public class Previous : Action
        {

            public override void run(MainWindow app)
            {
                app.windowingView.prevImage();
                base.runNext(app);
            }

            public override void undo(MainWindow app) 
            {
                Action a = new Next();
                a.run(app);
            }

            public override string ToString() { return null; }
        }
    }
}
 
