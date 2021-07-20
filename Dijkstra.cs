using System;
using System.Collections.Generic;
using System.Text;

namespace ASD_PROJEKT_3B {
    class Dijkstra {
        public MinHeap heap { get; set; }
        public Dictionary<Address, Address> Previous { get; set; } // { adres: poprzedni }
        public Dictionary<Address, long> Distance { get; set; }
        public Graph graph { get; set; }

        public Dijkstra(Graph g) {
            heap = new MinHeap(g.connections);
            Previous = new Dictionary<Address, Address>();
            Distance = new Dictionary<Address, long>();
            graph = g;
        }

        public long FindShortestPath(Address source, Address dest) {
            heap.DecreaseKey(source, 0);
            Previous.Add(source, null);
            while(heap.CurrentSize != 0) {
                Info info = heap.PopTop(); // obecnie przetwarzany wierzcholek
                Distance.Add(info.Vertice, info.Distance); 
                foreach(var connection in graph.connections[info.Vertice]) {
                    if(heap.DecreaseKey(connection.Vertice, connection.Distance + info.Distance) == 1) {
                        // jesli mozna zmniejszyc dystans do wierzcholka
                        if (Previous.ContainsKey(connection.Vertice)) {
                            Previous[connection.Vertice] = info.Vertice;
                        }
                        else {
                            Previous.Add(connection.Vertice, info.Vertice);
                        }
                    }
                }
            }
            if (Distance.ContainsKey(dest)) {
                return Distance[dest];
            }
            return -1;
        }
        //public long FindShortestPath(Address source, Address dest, long k) {

        //}
    }
}
