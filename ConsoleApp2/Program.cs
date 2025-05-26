#region inits

using ConsoleApp2;
using System.Diagnostics.Tracing;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;


#region variables
int cities = 5;// InputGetter("how many cities? :");
int travelerstime = 180;// InputGetter("how long should be the traveling period? :");

int startingpoint = 0;//InputGetter("Where is your starting city? :");
while (startingpoint >= cities)
{
    Console.WriteLine("your start must be with in the cities!");
    startingpoint = InputGetter("Where is your starting city? :");
}
int[,] mat = new int[cities, cities];
int[] visitingtime = new int[cities];
Random random = new Random();
Dictionary<int, bool> visited = new Dictionary<int, bool>();

#endregion
int InputGetter(string text)
{

    while (true)
    {
        Console.Write(text);
        string input = Console.ReadLine();

        if (int.TryParse(input, out int result))
        {
            return result;
        }
        else
        {
            Console.WriteLine("Invalid,Only Enter numbers");
        }
    }
}
for (int i = 0; i < cities; i++)
{
    visited[i] = false;
    visitingtime[i] = random.Next(30, 100);
    for (int j = 0; j < cities; j++)
    {
        if (i == j)
        {
            mat[i, j] = 0;
        }
        else
        {
            mat[i, j] = random.Next(10, 50);
        }
    }
}
Console.WriteLine("The distances :  ");
for (int i = 0; i < cities; i++)
{
    for (int j = 0; j < cities; j++)
    {
        Console.Write(mat[i, j] + "  ");

    }
    Console.WriteLine();
}
#endregion


static (int cost, List<int> path) TotalCost(
    int mask,
    int curr,
    int n,
    int[,] cost,
    int sp,
    int[] visittime,
    int travlertime,
    Dictionary<(int, int), (int, List<int>)> memo)
{
    (int cost, List<int> path) best = (0, new List<int> { curr });
    if (mask == (1 << n) - 1)
    {
        int returnCost = cost[curr, sp];
        if (returnCost <= travlertime)
        {
            var candidate = (cost: returnCost, path: new List<int> { curr, sp });
            if (candidate.path.Count > best.path.Count ||
               (candidate.path.Count == best.path.Count && candidate.cost < best.cost))
            {
                best = candidate;
            }
        }
        memo[(mask, curr)] = best;
        return best;
    }

    if (memo.TryGetValue((mask, curr), out var memoResult))
    {
        return memoResult;
    }

    for (int i = 0; i < n; i++)
    {
        if ((mask & (1 << i)) == 0)
        {
            int newMask = mask | (1 << i);
            var (subCost, subPath) = TotalCost(newMask, i, n, cost, sp, visittime, travlertime, memo);

            if (subCost == int.MaxValue)
                continue;

            int candidateCost = cost[curr, i] + visittime[i] + subCost;

            if (candidateCost <= travlertime)
            {
                var candidatePath = new List<int> { curr };
                candidatePath.AddRange(subPath);
                var candidate = (cost: candidateCost, path: candidatePath);

                if (candidate.path.Count > best.path.Count ||
                   (candidate.path.Count == best.path.Count && candidate.cost < best.cost))
                {
                    best = candidate;
                }
            }
        }
    }

    memo[(mask, curr)] = best;
    return best;
}


static (int,List<int>) tsp(int[,] cost, int sp, int[] visittime, int travlertime)
{
    int n = cost.GetLength(0);
    var memo = new Dictionary<(int, int), (int, List<int>)>();

    int initialMask = 1 << sp;
    var (minCost, path) = TotalCost(initialMask, sp, n, cost, sp, visittime, travlertime, memo);
    path.Add(sp);

return (minCost, path);
}
(int res,List<int> cool) = tsp(mat, startingpoint, visitingtime, travelerstime);
Console.WriteLine(res);
foreach (var i in cool)
{
    Console.Write(i + ",");
}