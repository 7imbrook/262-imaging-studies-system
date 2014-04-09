﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

using _262ImageViewer;

namespace Action
{
    public abstract class Action
    {
        // The next action down the chain
        protected Action nextAction;

        /**
         * Run the action on the particular study.
         */
        public void run(Study study)
        {
            study.addAction(this);
            if (this.nextAction != null)
                this.nextAction.run(study);
        }

        /**
         * Undo the action
         */
        public abstract void undo(Study study);

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
        public class Next : Action
        {
            // Image view that next interacts with
            GridView iv;
            
            /**
             * Next action constructor, takes the Image view that the action will interact with.
             */
            public Next(GridView view)
            {
                this.iv = view;
            }

            /**
             * Next behavior
             */
            public void run(Study study)
            {
                this.iv.nextImage();
                // Call base
                base.run(study);
            }

            public override void undo(Study study)
            {
                this.iv.prevImage();
            }

            public override string ToString()
            {
                return "nextImage -> " + (this.nextAction != null ? this.nextAction.ToString() : "end");
            }

        }

        public class Previous : Action
        {
            // Image view that next interacts with
            GridView iv;

            /**
             * Previous action constructor, takes the Image view that the action will interact with.
             */
            public Previous(GridView view)
            {
                this.iv = view;
            }

            /**
             * Previous behavior
             */
            public void run(Study study)
            {
                this.iv.prevImage();
                // Call base
                base.run(study);
            }

            public override void undo(Study study)
            {
                this.iv.nextImage();
            }

            public override string ToString()
            {
                return "prevImage -> " + (this.nextAction != null ? this.nextAction.ToString() : "end");
            }

        }

        public class Toggle : Action
        {
            // Image view that next interacts with
            GridView iv;

            /**
             * Previous action constructor, takes the Image view that the action will interact with.
             */
            public Toggle(GridView view)
            {
                this.iv = view;
            }

            /**
             * Previous behavior
             */
            public void run(Study study)
            {
                this.iv.switchMode();
                // Call base
                base.run(study);
            }

            public override void undo(Study study)
            {
                this.iv.switchMode();
            }

            public override string ToString()
            {
                return "toggleLayout -> " + (this.nextAction != null ? this.nextAction.ToString() : "end");
            }

        }
    }
    namespace Analysis
    {
        public class Create : Action
        {
            //public Create(AnalysisView view) { }
            public void run(Study study) { }
            public override void undo(Study study) { }
            public override string ToString() { return null; }

        }

        public class Close : Action
        {
            //public Close(AnalysisView view) { }
            public void run(Study study) { }
            public override void undo(Study study) { }
            public override string ToString() { return null; }
        }
    }

    namespace Reconstruction
    {

        public class Create : Action
        {
            //public Create(ReconstructionView view) { }
            public void run(Study study) { }
            public override void undo(Study study) { }
            public override string ToString() { return null; }

        }

        public class Close : Action
        {
            //public Close(ReconstructionView view) { }
            public void run(Study study) { }
            public override void undo(Study study) { }
            public override string ToString() { return null; }
        }

        public class NextReconstruction : Action
        {
            //public NextReconstruction(ReconstructionView view) { }
            public void run(Study study) { }
            public override void undo(Study study) { }
            public override string ToString() { return null; }
        }

        public class PreviousReconstruction : Action
        {
            //public PreviousReconstruction(ReconstructionView view) { }
            public void run(Study study) { }
            public override void undo(Study study) { }
            public override string ToString() { return null; }
        }

        public class Next : Action
        {
            //public Next(ReconstructionView view) { }
            public void run(Study study) { }
            public override void undo(Study study) { }
            public override string ToString() { return null; }
        }

        public class Previous : Action
        {
            //public Previous(ReconstructionView view) { }
            public void run(Study study) { }
            public override void undo(Study study) { }
            public override string ToString() { return null; }
        }

    }

    namespace Windowing
    {

        public class Create : Action
        {
            //public Create(WindowingView view) { }
            public void run(Study study) { }
            public override void undo(Study study) { }
            public override string ToString() { return null; }

        }

        public class Close : Action
        {
            //public Close(WindowingView view) { }
            public void run(Study study) { }
            public override void undo(Study study) { }
            public override string ToString() { return null; }
        }

        public class Next : Action
        {
            //public Next(WindowingView view) { }
            public void run(Study study) { }
            public override void undo(Study study) { }
            public override string ToString() { return null; }
        }

        public class Previous : Action
        {
            //public Previous(WindowingView view) { }
            public void run(Study study) { }
            public override void undo(Study study) { }
            public override string ToString() { return null; }
        }
    }
}
