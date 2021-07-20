using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace ASD_PROJEKT_3B {
    class Test {
        public int LinesNumber { get; set; }
        public Node Tree { get; set; }
        public Graph graph { get; set; }
        public Dijkstra dijkstra { get; set; }
        public string FileNameInput { get; set; }
        public string FileNameOutput { get; set; }
        public string[][] Commands;
        public Test(Node node, string fname, Graph g) {
            Tree = node;
            graph = g;
            FileNameInput = fname;
            FileNameOutput = FileNameInput.Substring(0, FileNameInput.Length - 8) + "_OUTPUT.txt";
        }

        private void LoadCommands() {
            using(var sr = new StreamReader(FileNameInput)) {
                LinesNumber = int.Parse(sr.ReadLine());
                Commands = new string[LinesNumber][];
                for (int i = 0; i < LinesNumber; i++) {
                    Commands[i] = sr.ReadLine().Split();
                }
            }
        }

        public void Launch() {
            LoadCommands();
            var sw = new StreamWriter(FileNameOutput);
            foreach(string[] command in Commands) {
                switch (command[0]) {
                    case "DK":
                        Tree.Insert(command[1]);
                        break;
                    case "UK":
                        Tree.Remove(command[1]);
                        break;
                    case "WK":
                        object found = Tree.FindDevice(command[1]);
                        if (found != null) {
                            sw.WriteLine("TAK");
                        }
                        else {
                            sw.WriteLine("NIE");
                        }
                        break;
                    case "LK":
                        int n_devices = Tree.DeviceCount(command[1]);
                        sw.WriteLine(n_devices);
                        break;
                    case "DP":
                        graph.AddConnection(command[1], command[2], command[3]);
                        break;
                    case "UP":
                        graph.RemoveConnection(command[1], command[2]);
                        break;
                    case "NP":
                        if(Tree.FindDevice(command[1]) != null && Tree.FindDevice(command[2]) != null) {
                            Address net1 = new Address(command[1]);
                            net1.ConvertToNetwork();
                            Address net2 = new Address(command[2]);
                            net2.ConvertToNetwork();
                            dijkstra = new Dijkstra(graph);
                            long distance = dijkstra.FindShortestPath(net1, net2);
                            sw.WriteLine(distance);
                        }
                        else {
                            sw.WriteLine("NIE");
                        }
                        break;
                        
                    default:
                        sw.WriteLine("BRAK NP2");
                        break;
                }
            }
            sw.Close();
        }
    }
}
