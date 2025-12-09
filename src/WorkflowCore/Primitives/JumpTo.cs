using WorkflowCore.Interface;
using WorkflowCore.Models;

namespace WorkflowCore.Primitives
{
    public sealed class JumpTo : StepBody
    {
        public override ExecutionResult Run(IStepExecutionContext context)
        {
            foreach (var item in context.Workflow.ExecutionPointers)
            {
                if (context.ExecutionPointer.Scope.Contains(item.Id))
                {
                    item.Active = false;
                    item.Status = PointerStatus.Complete;
                    item.EndTime = DateTime.Now;
                }
            }

            // 清空作用域
            context.ExecutionPointer.Scope = [];
            return ExecutionResult.Next();
        }
    }
}
