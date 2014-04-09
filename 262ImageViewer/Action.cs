using System;
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
        public void run(StudySession study)
        {
            study.addAction(this);
            if (this.nextAction != null)
                this.nextAction.run(study);
        }

        /**
         * Undo the action
         */
        public abstract void undo(StudySession study);

        /**
         * Get the next acction down the chain, returns null if at the end
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
            ImageView iv;
            
            /**
             * Next action constructor, takes the Image view that the action will interact with.
             */
            public Next(ImageView view)
            {
                this.iv = view;
            }

            /**
             * Next behavior
             */
            public void run(StudySession study)
            {
                this.iv.nextImage();
                // Call base
                base.run(study);
            }

            public override void undo(StudySession study)
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
            ImageView iv;

            /**
             * Previous action constructor, takes the Image view that the action will interact with.
             */
            public Previous(ImageView view)
            {
                this.iv = view;
            }

            /**
             * Previous behavior
             */
            public void run(StudySession study)
            {
                this.iv.prevImage();
                // Call base
                base.run(study);
            }

            public override void undo(StudySession study)
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
            ImageView iv;

            /**
             * Previous action constructor, takes the Image view that the action will interact with.
             */
            public Toggle(ImageView view)
            {
                this.iv = view;
            }

            /**
             * Previous behavior
             */
            public void run(StudySession study)
            {
                this.iv.switchMode();
                // Call base
                base.run(study);
            }

            public override void undo(StudySession study)
            {
                this.iv.switchMode();
            }

            public override string ToString()
            {
                return "toggleLayout -> " + (this.nextAction != null ? this.nextAction.ToString() : "end");
            }

        }
    }
}
 