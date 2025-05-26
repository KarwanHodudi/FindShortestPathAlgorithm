using System;
using System.Collections.Generic;
using System.Linq;

namespace ConsoleApp2
{
    public class PriorityQ
    {
        private PriorityQueue<int, int> queue = new PriorityQueue<int, int>();
        private Dictionary<int, int> entryFinder = new Dictionary<int, int>();

        public void AddTask(int priority, int node)
        {
            if (entryFinder.ContainsKey(node))
            {
                UpdatePriority(priority, node);
                return;
            }

            entryFinder[node] = priority;
            queue.Enqueue(node, priority);
        }

        public int CheckQueueCount()
        {
            Console.WriteLine(queue.Count);
            return queue.Count;
        }

        public void UpdatePriority(int priority, int node)
        {
            if (entryFinder.ContainsKey(node))
            {
                entryFinder.Remove(node);
            }
            queue.TryDequeue(out priority, out node);
            AddTask(priority, node); // Re-enqueue with new priority
        }

        public (int, int) PopTask()
        {
            if (queue.Count == 0)
                throw new InvalidOperationException("No nodes remaining.");

            int node;
            int distance;
            queue.TryDequeue(out node, out distance); // Efficiently dequeue

            entryFinder.Remove(node);
            return (node, distance);
        }
    }
}
