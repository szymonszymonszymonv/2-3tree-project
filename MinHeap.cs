using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;

namespace ASD_PROJEKT_3B {

    struct Info {
        public Address Vertice;
        public long Distance;
        public Info(Address v, long d) {
            Vertice = v;
            Distance = d;
        }
    }
    class MinHeap {
        // kopiec zawiera tylko funkcje niezbedne do algorytmu dijkstry
        // pop, updatekey i heapify
        public Info[] DistToVertice { get; set; }
        public Dictionary<Address, long> IndexOfVertice;
        public long MaxSize { get; set; }
        public long CurrentSize { get; set; }
        public MinHeap(Dictionary<Address, List<Info>> connections) {
            IndexOfVertice = new Dictionary<Address, long>();
            MaxSize = connections.Count;
            DistToVertice = new Info[MaxSize];
            CurrentSize = 0;
            foreach(var key in connections.Keys) {
                DistToVertice[CurrentSize] = new Info(key, long.MaxValue);
                IndexOfVertice.Add(key, CurrentSize);
                CurrentSize++;
            }
        }

        public long Parent(long idx) {
            return (idx - 1) / 2;
        }

        public long LeftChild(long idx) {
            return (idx * 2 + 1);
        }
        public long RightChild(long idx) {
            return (idx * 2 + 2);
        }
        public Info PopTop() { 
            if(CurrentSize == 0) {
                return new Info(null, long.MaxValue);
            }
            if (CurrentSize == 1) {
                CurrentSize--;
                return DistToVertice[0];
            }
            Info root = DistToVertice[0];
            Info last_item = DistToVertice[CurrentSize - 1];
            IndexOfVertice[last_item.Vertice] = 0;
            IndexOfVertice[root.Vertice] = -1; // root usuniety
            DistToVertice[0] = last_item;
            CurrentSize--;
            Heapify(0);
            return root;
        }
        public void Heapify(long idx) {
            long left = LeftChild(idx);
            long right = RightChild(idx);
            long min = idx;
            if((left < CurrentSize) && (DistToVertice[left].Distance < DistToVertice[min].Distance)) {
                min = left;
            }
            if((right < CurrentSize) && (DistToVertice[right].Distance < DistToVertice[min].Distance)) {
                min = right;
            }
            if(min != idx) {
                Info tmp = DistToVertice[idx];
                Info tmp2 = DistToVertice[min];
                DistToVertice[idx] = DistToVertice[min];
                DistToVertice[min] = tmp;
                IndexOfVertice[tmp.Vertice] = min;
                IndexOfVertice[tmp2.Vertice] = idx;
                Heapify(min);
            }
        }
        public int DecreaseKey(Address key, long value) {
            long idx = IndexOfVertice[key];
            if (idx == -1) { // element nie istnieje
                return -1;
            }
            if(DistToVertice[idx].Distance <= value) { // nie da sie zmniejszyc
                return -1;
            }
            DistToVertice[idx].Distance = value;
            while(idx != 0 && DistToVertice[Parent(idx)].Distance > DistToVertice[idx].Distance) {
                // zamiana rodzica z obecnym
                Info tmp = DistToVertice[Parent(idx)];
                Info tmp2 = DistToVertice[idx];
                DistToVertice[Parent(idx)] = DistToVertice[idx];
                DistToVertice[idx] = tmp;
                IndexOfVertice[tmp.Vertice] = idx;
                IndexOfVertice[tmp2.Vertice] = Parent(idx);
                idx = Parent(idx);
            }
            return 1;
        }
    }
}
