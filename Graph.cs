using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ASD_PROJEKT_3B {
    class Graph {
        private Node tree { get; set; }
        // klucz - adres, wartosc: lista tablic z para (adres, waga)
        public Dictionary <Address, List<Info>> connections { get; set; }
        public Graph(Node t) {
            tree = t;
            connections = new Dictionary<Address, List<Info>>();
        }

        public int AddConnection(string str_network_a, string str_network_b, string str_bandwidth) {
            if(tree.FindNetwork(str_network_a) == null || tree.FindNetwork(str_network_b) == null){
                return -1;
            }
            char str_prefix = str_bandwidth[str_bandwidth.Length - 1];
            long max_bandwidth = 100000000000; // maksymalna przepustowosc - 100G
            long prefix = str_prefix == 'G' ? 1000000000 : 1000000;
            long bandwidth = long.Parse(str_bandwidth.Substring(0, str_bandwidth.Length - 1)) * prefix;
            long weight = max_bandwidth / bandwidth;

            Address network_a = new Address(str_network_a);
            Address network_b = new Address(str_network_b);
            if (connections.ContainsKey(network_a)) {
                List<Info> list = connections[network_a];
                if(!list.Any(item => item.Vertice == network_b)) {
                    connections[network_a].Add(new Info(network_b, weight));
                }
            }
            else {
                List<Info> list = new List<Info>();
                list.Add(new Info(network_b, weight));
                connections.Add(network_a, list);
            }
            if (connections.ContainsKey(network_b)) {
                List<Info> list = connections[network_b];
                if (!list.Any(item => item.Vertice as Address == network_a)) {
                    connections[network_b].Add(new Info(network_a, weight));
                }
            }
            else {
                List<Info> list = new List<Info>();
                list.Add(new Info(network_a, weight));
                connections.Add(network_b, list);
            }
            return 1;
        }
        public void RemoveConnection(string str_network_a, string str_network_b) {
            Address network_a = new Address(str_network_a);
            Address network_b = new Address(str_network_b);
            if (connections.ContainsKey(network_a)) {
                List<Info> list = connections[network_a];
                list.RemoveAll(item => item.Vertice == network_b);
            }
            if (connections.ContainsKey(network_b)) {
                List<Info> list = connections[network_b];
                list.RemoveAll(item => item.Vertice == network_a);
            }
        }

    }
}
