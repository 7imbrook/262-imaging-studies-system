using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using _262ImageViewer;
using System.Drawing;

namespace Action
{
    [Serializable]
    public abstract class Action
    {
        // The next action down the chain
        protected Action nextAction;

        /**
         * Run the action on the particular study.
         */
        public abstract void run(MainWindow app);
       
        protected void runNext(MainWindow app)
        {
            var study = app.studySession;
            if (this.nextAction != null)
                this.nextAction.run(app);
        }

        /**
         * Undo the action
         */
        public abstract void undo(MainWindow study);

        /**
         * Get the next action down the chain, returns null if at the end
         */
        public Action next()
        {
            return this.nextAction;
        }

        /**
         * Set the next action of this action
         */
        public void setNext(Action action)
        {
            this.nextAction = action;
        }

        /**
         * Remove the next action in the chain
         */
        public void removeNext()
        {
            this.nextAction = null;
        }

    }

    namespace Grid
    {
        [Serializable]
        public class Next : Action
        {
            /**
             * Next behavior
             */
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
        [Serializable]
        public class Previous : Action
        {
            /**
             * Previous behavior
             */
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
        [Serializable]
        public class Toggle : Action
        {
            /**
             * Previous behavior
             */
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
    namespace Analysis
    {
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
                main.setFrameImageView(analysis);
                base.runNext(main);
            }
            public override void undo(MainWindow app) { }
            public override string ToString() { return "Analysis.Create -> " + (this.nextAction != null ? this.nextAction.ToString() : "end"); }

        }
        [Serializable]
        public class Close : Action
        {
            public override void run(MainWindow app) { }
            public override void undo(MainWindow app) { }
            public override string ToString() { return "Analysis.Close -> " + (this.nextAction != null ? this.nextAction.ToString() : "end"); }
        }
    }

    namespace Reconstruction
    {
        [Serializable]
        public class Create : Action
        {
            MainWindow window;
            ReconstructionView reconstructionView;
            public Create(MainWindow w, Study session)
            {
                window = w;
                ImageLoader imageLoader = new LocalImages(session.imagePath);
                reconstructionView = new ReconstructionView(imageLoader, session.imageIndex, session.imageMode, session);
            }
            public void run(MainWindow app)
            {
                window.setFrameImageView(reconstructionView);
                base.run(app);
            }
            public override void undo(Study study) { }
            public override string ToString() { return "Reconstruction.Create -> " + (this.nextAction != null ? this.nextAction.ToString() : "end"); }

        }
        [Serializable]
        public class Close : Action
        {
            public override void run(MainWindow app) { }
            public override void undo(MainWindow app) { }
            public override string ToString() { return "Reconstruction.Close -> " + (this.nextAction != null ? this.nextAction.ToString() : "end"); }
        }
        [Serializable]
        public class NextReconstruction : Action
        {
            public override void run(MainWindow app) { }
            public override void undo(MainWindow app) { }
            public override string ToString() { return "NextReconstruction -> " + (this.nextAction != null ? this.nextAction.ToString() : "end"); }
        }
        [Serializable]
        public class PreviousReconstruction : Action
        {
            public override void run(MainWindow app) { }
            public override void undo(MainWindow app) { }
            public override string ToString() { return "PreviousReconstruction -> " + (this.nextAction != null ? this.nextAction.ToString() : "end"); }
        }
        [Serializable]
        public class Next : Action
        {
            public override void run(MainWindow app) { }
            public override void undo(MainWindow app) { }
            public override string ToString() { return "Reconstruction.Next -> " + (this.nextAction != null ? this.nextAction.ToString() : "end"); }
        }
        [Serializable]
        public class Previous : Action
        {
            public override void run(MainWindow app) { }
            public override void undo(MainWindow app) { }
            public override string ToString() { return "Reconstruction.Previous -> " + (this.nextAction != null ? this.nextAction.ToString() : "end"); }
        }
    }

    namespace Windowing
    {
        [Serializable]
        public class Create : Action
        {
            public override void run(MainWindow app) { }
            public override void undo(MainWindow app) { }
            public override string ToString() { return null; }

        }
        [Serializable]
        public class Close : Action
        {
            public override void run(MainWindow app) { }
            public override void undo(MainWindow app) { }
            public override string ToString() { return null; }
        }
        [Serializable]
        public class Next : Action
        {
            public override void run(MainWindow app) { }
            public override void undo(MainWindow app) { }
            public override string ToString() { return null; }
        }
        [Serializable]
        public class Previous : Action
        {
            public override void run(MainWindow app) { }
            public override void undo(MainWindow app) { }
            public override string ToString() { return null; }
        }
    }
}
 
