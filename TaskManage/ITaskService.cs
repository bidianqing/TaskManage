using System.Collections.Concurrent;

namespace TaskManage
{
    public interface ITaskService
    {
        Task AddTask(int id, Task task, CancellationTokenSource cts);

        Task RemoveTask(int id);
    }

    public class TaskService : ITaskService
    {
        private ConcurrentDictionary<string, (Task task, CancellationTokenSource token)> _runningTasks = new();

        public async Task AddTask(int id, Task task, CancellationTokenSource cts)
        {
            if (_runningTasks.ContainsKey(id.ToString()))
            {
                var taskInfo = _runningTasks[id.ToString()];

                taskInfo.token.Cancel();

                if (!taskInfo.task.IsCompleted)
                {
                    await Task.WhenAny(taskInfo.task);
                }

                taskInfo.task.Dispose();

                taskInfo.token.Dispose();
            }

            _runningTasks[id.ToString()] = (task, cts);
        }


        public async Task RemoveTask(int id)
        {
            if (_runningTasks.TryRemove(id.ToString(), out var taskInfo))
            {
                taskInfo.token.Cancel();

                if (!taskInfo.task.IsCompleted)
                {
                    await Task.WhenAny(taskInfo.task);
                }

                taskInfo.task.Dispose();

                taskInfo.token.Dispose();
            }
        }
    }

}
