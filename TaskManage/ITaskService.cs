using System.Collections.Concurrent;

namespace TaskManage
{
    public interface ITaskService
    {
        void AddTask(int id, Task task, CancellationTokenSource cts);

        Task RemoveTask(int id);
    }

    public class TaskService : ITaskService
    {
        private readonly ConcurrentDictionary<string, (Task task, CancellationTokenSource token)> _runningTasks = new();

        public void AddTask(int id, Task task, CancellationTokenSource cts)
        {
            if (_runningTasks.ContainsKey(id.ToString()))
            {
                return;
            }

            _runningTasks[id.ToString()] = (task, cts);
        }


        public async Task RemoveTask(int id)
        {
            if (_runningTasks.TryRemove(id.ToString(), out var taskInfo))
            {
                taskInfo.token.Cancel();

                await Task.WhenAll(taskInfo.task);

                taskInfo.token.Dispose();
            }
        }
    }

}
