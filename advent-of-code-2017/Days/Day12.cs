﻿using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode2017.Days
{
    internal class Day12 : IDay
    {
        public void Part1(string input)
        {
            var nodes = Parse(input);
            Visit(nodes[0]);

            var result = nodes.Values.Count(n => n.Visited);
            Console.WriteLine("Result: " + result);
        }

        public void Part2(string input)
        {
            var nodes = Parse(input);
            int nGroups = 0;

            while (true)
            {
                var node = nodes.Values.FirstOrDefault(n => !n.Visited);
                if (node == null)
                    break;

                nGroups++;

                Visit(node);
            }

            Console.WriteLine("Result: " + nGroups);
        }

        void Visit(Node n)
        {
            if (n.Visited)
                return;

            n.Visited = true;

            foreach (var neigh in n.Neighbours)
                Visit(neigh);
        }

        private static Dictionary<int, Node> Parse(string input)
        {
            var nodes = new Dictionary<int, Node>();
            var lines = input.Split('\n').Select(x => x.Trim());

            foreach (var line in lines)
            {
                var spl = line.Split("<->");
                var node = new Node
                {
                    Id = int.Parse(spl[0].Trim()),
                    NeighboursInts = spl[1].Split(',').Select(x => int.Parse(x.Trim())).ToList()
                };
                nodes[node.Id] = node;
            }

            foreach (var node in nodes.Values)
                node.Neighbours = node.NeighboursInts.Select(i => nodes[i]).ToList();

            return nodes;
        }

        class Node
        {
            public int Id { get; set; }

            public List<int> NeighboursInts { get; set; }

            public List<Node> Neighbours { get; set; }

            public bool Visited { get; set; }
        }
    }
}
