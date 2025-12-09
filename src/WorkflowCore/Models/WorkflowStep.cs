using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using WorkflowCore.Interface;

namespace WorkflowCore.Models
{
    public abstract class WorkflowStep
    {
        public abstract Type BodyType { get; }

        public virtual int Id { get; set; }

        public virtual string Name { get; set; }

        public virtual string ExternalId { get; set; }

        public virtual List<int> Children { get; set; } = new List<int>();

        public virtual List<IStepOutcome> Outcomes { get; set; } = new List<IStepOutcome>();

        public virtual List<IStepParameter> Inputs { get; set; } = new List<IStepParameter>();

        public virtual List<IStepParameter> Outputs { get; set; } = new List<IStepParameter>();

        public virtual WorkflowErrorHandling? ErrorBehavior { get; set; }

        public virtual TimeSpan? RetryInterval { get; set; }

        public virtual LambdaExpression CancelCondition { get; set; }

        public bool ProceedOnCancel { get; set; } = false;

        public virtual IStepBody ConstructBody(IServiceProvider serviceProvider)
        {
            IStepBody body = (serviceProvider.GetService(BodyType) as IStepBody);
            if (body == null)
            {
                var stepCtor = BodyType.GetConstructor(new Type[] { });
                if (stepCtor != null)
                    body = (stepCtor.Invoke(null) as IStepBody);
            }
            return body;
        }
    }

    public class WorkflowStep<TStepBody> : WorkflowStep
        where TStepBody : IStepBody 
    {
        public override Type BodyType => typeof(TStepBody);
    }

	public enum ExecutionPipelineDirective 
    { 
        Next = 0, 
        Defer = 1, 
        EndWorkflow = 2 
    }
}
