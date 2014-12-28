using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Core.Buildings;
using Core.Technologies;
using Core.Military;

namespace Core
{
    public class RequirementGraph : IEnumerable<IGrouping<int, int>>
    {
        private ILookup<int, int> graph;
        public RequirementGraph(ILookup<int, int> graph)
        {
            this.graph = graph;
            if (IsCircular())
            {
                throw new AggregateException("Votre liste est circulaire");
            }
        }
        public RequirementGraph(IEnumerable<BuildableEntity> entities)
        {
            if (HaveDuplicatedId(entities))
            {
                throw new DuplicateIdException("Vos id sont dupliqué");
            }
            else
            {
                graph = entities.SelectMany(x => x.Requirement.Buildings.Select(parent => new KeyValuePair<int, int>(parent, x.ID))).ToLookup(x => x.Key, x => x.Value);
                if (IsCircular())
                {
                    throw new AggregateException("Votre liste est circulaire");
                }
            }
        }
        public RequirementGraph Simplify()
        {
            var matrix = LookUpToMatrix(graph);
            for (int x = 0; x <  matrix.GetUpperBound(0); x++)
            {
                for (int y = 0; y <  matrix.GetUpperBound(0); y++)
                {
                    for (int z = 0; z <  matrix.GetUpperBound(0); z++)
                    {
                        if(matrix[x,z]&&matrix[x,y]&&matrix[y,z])
                        {
                            matrix[x, z] = false;
                        }
                    }
                }
            }
            return new RequirementGraph(MatrixToLookUp(matrix));
        }
        private static bool[,] LookUpToMatrix(ILookup<int,int> lookUp)
        {
            int maxIndex = BuildingFactory.GetInstance().Buildings().Count();
            var matrix = new bool[maxIndex, maxIndex];
            foreach (var relation in lookUp)
            {
                foreach (var node in relation)
                {
                    matrix[relation.Key, node] = true;
                }
            }
            return matrix;
        }
        private static ILookup<int,int> MatrixToLookUp(bool[,] matrix)
        {
            var list = new List<KeyValuePair<int, int>>();
            for (int i = 0; i < matrix.GetUpperBound(0); i++)
            {
                for (int j = 0; j < matrix.GetUpperBound(1); j++)
                {
                    if(matrix[i,j])
                    {
                        list.Add(new KeyValuePair<int, int>(i, j));
                    }
                }
            }
            return list.ToLookup(x => x.Key, x => x.Value);
        }
        
        private IEnumerable<KeyValuePair<int,int>> GetAllRelations()
        {
            return graph.SelectMany(entity => entity.Select(x=> new KeyValuePair<int, int>(entity.Key, x)));
        }

        private bool RelationBetween(int id, IEnumerable<int> targets)
        {
            return graph[id].Any(x => targets.Any(y=>x==y) || RelationBetween(x,targets));
        }
        public bool IsCircular()
        {
            var visited = graph.ToDictionary(x => x.Key, x => DFSType.NotVisited);
            return graph.Any(x => !BranchIsCorrect(x.Key, visited));
        }
        private static bool HaveDuplicatedId(IEnumerable<BuildableEntity> entities)
        {
            return entities.GroupBy(x => x.ID).All(x => x.Count() > 1);
        }
        enum DFSType
        {
            NotVisited,
            AlreadyVisited,
            Correct
        }
        private bool BranchIsCorrect(int id, Dictionary<int, DFSType> visited)
        {
            visited[id] = DFSType.AlreadyVisited;
            graph[id].All(
                adj =>
                {
                    if (!visited.ContainsKey(adj))
                    {
                        return true;
                    }
                    switch (visited[adj])
                    {
                        case DFSType.NotVisited:
                            return BranchIsCorrect(adj, visited);
                        case DFSType.AlreadyVisited:
                            return false;
                        case DFSType.Correct:
                            return true;
                        default:
                            return true;
                    }
                });
            visited[id] = DFSType.Correct;
            return true;
        }

        public IEnumerator<IGrouping<int, int>> GetEnumerator()
        {
            return graph.GetEnumerator();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return graph.GetEnumerator();
        }
    }

    [Serializable]
    public class DuplicateIdException : Exception
    {
        public DuplicateIdException() { }
        public DuplicateIdException(string message) : base(message) { }
        public DuplicateIdException(string message, Exception inner) : base(message, inner) { }
        protected DuplicateIdException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context)
            : base(info, context) { }
    }
}
