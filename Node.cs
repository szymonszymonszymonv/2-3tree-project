using System;
using System.Collections.Generic;
using System.Text;

namespace ASD_PROJEKT_3B {
    class Node {
        private List<Node> Children;
        private List<Address> Keys;
        public Node(string key) {
            Children = new List<Node>();
            Keys = new List<Address>();
            Keys.Add(new Address(key));
        }
        public Node(Address device) {
            Children = new List<Node>();
            Keys = new List<Address>();
            Keys.Add(device);
        }
        public Node() {
            Children = new List<Node>();
            Keys = new List<Address>();
        }
        public object FindDevice(string key, bool stack = false) {
            // znajdz urzadzenie o danym adresie (+ zwroc stosik opcjonalnie)
            Address device = new Address(key);
            Node current = this;
            Stack<Node> elements = new Stack<Node>(); // sciezka wglab drzewa
            elements.Push(current);
            bool moved;
            if (current.Children.Count == 0) {
                for (int i = 0; i < Keys.Count; i++) {
                    if (device.CompareTo(current.Keys[i]) == 0 && stack == true) {
                        return new object[] { current, stack };
                    }
                    else if (device.CompareTo(current.Keys[i]) == 0) {
                        return current;
                    }
                }
            }
            while (current.Keys.Count != 0) {
                moved = false;
                for (int i = 0; i < current.Keys.Count; i++) {
                    if (device.CompareTo(current.Keys[i]) == 0) {
                        if (stack == true) {
                            return new object[] { current, elements };
                        }
                        else {
                            return current;
                        }
                    }
                }
                if(current.Children.Count != 0) {
                    for (int i = 0; i < current.Keys.Count; i++) {
                        if (device.CompareTo(current.Keys[i]) == -1) {
                            current = current.Children[i];
                            moved = true;
                            elements.Push(current);
                            break;
                        }
                    }
                    if (!moved) {
                        current = current.Children[current.Children.Count - 1];
                        elements.Push(current);
                    }
                }
                else {
                    break;
                }
            }
            return null;
        }

        public Node FindNetwork(string key) {
            // znajdz urzadzenie o danym adresie sieci

            Address network = new Address(key);
            Node current = this;
            bool moved;
            if (current.Children.Count == 0) {
                for (int i = 0; i < Keys.Count; i++) {
                    if (network.CompareNetwork(current.Keys[i]) == 0 ) {
                        return current;
                    }
                }
            }
            while (current.Keys.Count != 0) {
                moved = false;
                for (int i = 0; i < current.Keys.Count; i++) {
                    if (network.CompareNetwork(current.Keys[i]) == 0) {
                        return current;
                    }
                }
                if (current.Children.Count != 0) {
                    for (int i = 0; i < current.Keys.Count; i++) {
                        if (network.CompareNetwork(current.Keys[i]) == -1) {
                            current = current.Children[i];
                            moved = true;
                            break;
                        }
                    }
                    if (!moved) {
                        current = current.Children[current.Children.Count - 1];
                    }
                }
                else {
                    break;
                }
            }
            return null;
        }
        public int Insert(string value) {
            Address device = new Address(value);
            if (FindDevice(value) != null) {
                return -1;
            }
            if (Keys.Count == 0) {
                Keys.Add(device);
                return 1;
            }
            Node current = this;
            Stack<Node> elements = new Stack<Node>();
            bool moved;
            elements.Push(current);
            while (current.Children.Count != 0) {
                moved = false;
                for (int i = 0; i < current.Keys.Count; i++) {
                    if (device.CompareTo(current.Keys[i]) == -1) {
                        current = current.Children[i];
                        moved = true;
                        break;
                    }
                }
                if (!moved) {
                    current = current.Children[current.Children.Count - 1];
                }
                elements.Push(current);
            }
            current.Keys.Add(device);
            current.Keys.Sort();
            if (current.Keys.Count == 3) { // SPLIT
                Split(elements);
            }
            return 1;

        }
        public void InsertNode(Node node) {
            // wstawic wezel, posortowac dzieci po kluczu
            this.Children.Add(node);
            this.Children.Sort((x, y) => x.Keys[0].CompareTo(y.Keys[0]));
        }

        private void Split(Stack<Node> elements) {
            // podzielic obecny, srodkowy do rodzica, prawy element to nowe dziecko rodzica
            Node current = elements.Pop();
            while (elements.Count > 0) {
                Node previous = elements.Pop();
                Address left = current.Keys[0];
                Address mid = current.Keys[1];
                Address right = current.Keys[2];
                Node n_right = new Node(right);
                current.Keys.RemoveAt(1);
                // przeniesc dzieci do prawego wezelka
                for(int i = 2; i < current.Children.Count; i++) {
                    n_right.InsertNode(current.Children[i]);
                }
                // usunac przeniesione dzieci
                for(int i = current.Children.Count - 1; i > 1; i--) {
                    current.Children.RemoveAt(i);
                }
                previous.Keys.Add(mid);
                previous.Keys.Sort();
                previous.InsertNode(n_right);
                current.Keys.RemoveAt(1);
                current = previous;
                if (current.Keys.Count < 3) {
                    break;
                }
            }
            // przepelnienie w korzeniu - rozbijanko korzenia
            if (current.Keys.Count == 3) {
                Node n_left = new Node(current.Keys[0]);
                Node n_right = new Node(current.Keys[2]);
                Address left = current.Keys[0];
                Address mid = current.Keys[1];
                Address right = current.Keys[2];
                current.Keys.RemoveAt(0);
                current.Keys.RemoveAt(1);
                foreach (var child in current.Children) {
                    if (child.Keys[0].CompareTo(mid) == -1) {
                        n_left.InsertNode(child);
                    }
                    else {
                        n_right.InsertNode(child);
                    }
                }
                current.Children.Clear();
                current.InsertNode(n_left);
                current.InsertNode(n_right);

            }

        }
        public int Remove(string key) {
            // usun urzadzenie o danym adresie
            Address device = new Address(key);
            object[] data = FindDevice(key, true) as object[];
            Node found = null;
            Stack<Node> elements = null;

            if (data != null) {
                found = data[0] as Node;
                elements = data[1] as Stack<Node>;
            }

            else {
                return -1;
            }

            if (found == null) {
                return -1;
            }

            int keyIdx = -1;
            for(int i = 0; i < found.Keys.Count; i++) {
                if(found.Keys[i].StrAddr == key) {
                    found.Keys.RemoveAt(i);
                    keyIdx = i;
                    break;
                }
            }
            // USUWANIE W NIE-LISCIU
            if (found.Children.Count > 0) {
                GetSuccessor(found, keyIdx, elements);
            }
            // NIEDOMIAR W LISCIU
            else if (found.Keys.Count == 0) {   
                LeafUnderflow(found, elements);
            }
            if(found.Keys.Count == 0) {
                Console.WriteLine("CO JEST");
            }
            return 1;
        }

        private void RemoveLeaf(string key, Node leaf, Node parent, Stack<Node> elements) {
            for (int i = 0; i < leaf.Keys.Count; i++) {
                if (leaf.Keys[i].StrAddr == key) {
                    leaf.Keys.RemoveAt(i);
                    break;
                }
            }
            if(leaf.Keys.Count == 0) {
                LeafUnderflow(leaf, elements);
            }
        }
        private void GetSuccessor(Node target, int keyIdx, Stack<Node> elements) {
            Node current = target;
            Node previous = target;
            if (current.Children.Count == 1) {
                current = current.Children[0];
            }
            else if (current.Children.Count == 2 && keyIdx == 1) {
                current = current.Children[1];
            }
            else {
                current = current.Children[keyIdx + 1];
            }
            elements.Push(current);
            while (current.Children.Count != 0) {
                previous = current;
                current = current.Children[0];
                elements.Push(current);
            }
            Address tmp = current.Keys[0];
            target.Keys.Add(tmp);
            target.Keys.Sort();
            RemoveLeaf(tmp.StrAddr, current, previous, elements);
        }
        private int LeafUnderflow(Node leaf, Stack<Node> elements) {
            elements.Pop();
            while(leaf.Keys.Count == 0) {
                Node parent = elements.Pop();
                List<Node> siblings = new List<Node>();
                for (int i = 0; i < parent.Children.Count; i++) {
                    if (parent.Children[i] == leaf) {
                        if (i == 0 && parent.Children.Count > 1) {
                            siblings.Add    (parent.Children[1]);
                        }
                        else if (i == 1 && parent.Children.Count == 2) {
                            siblings.Add(parent.Children[0]);
                        }
                        else if (i == 1 && parent.Children.Count == 3) {
                            siblings.Add(parent.Children[0]);
                            siblings.Add(parent.Children[2]);
                        }
                        else if (i == 2) {
                            siblings.Add(parent.Children[1]);
                        }
                    }
                }
                for (int i = 0; i < siblings.Count; i++) {
                    // sprawdzenie, czy mozliwa jest pozyczka klucza
                    if (siblings[i].Keys.Count == 2) {
                        Rotation(leaf, siblings[i], parent);
                        return 1;
                    }
                }
                // w przeciwnym wypadku: merge
                if (elements.Count == 0) {
                    Merge(leaf, siblings[0], parent, true);
                }
                else {
                    Merge(leaf, siblings[0], parent);
                }
                leaf = parent;
            }
            if (leaf.Keys.Count == 0) {
                Console.WriteLine("CO JEST");
            }
            return 1;
        }

        private void Rotation(Node target, Node sibling, Node parent) {
            int targetIdx = -1, siblingIdx = -1, parentKey = 0;
            for (int i = 0; i < parent.Children.Count; i++) {
                if (parent.Children[i] == target) {
                    targetIdx = i;
                }
                if (parent.Children[i] == sibling) {
                    siblingIdx = i;
                }
            }
            if (targetIdx == 2 || siblingIdx == 2) {
                // drugi klucz u rodzica
                parentKey = 1;
            }
            
            // rotacja w lewo gdy target jest na lewo od brata
            if (targetIdx < siblingIdx) {
                target.Keys.Add(parent.Keys[parentKey]);
                parent.Keys.RemoveAt(parentKey);
                parent.Keys.Add(sibling.Keys[0]);
                if (sibling.Children.Count > 0) {
                    target.InsertNode(sibling.Children[0]);
                    sibling.Children.RemoveAt(0);
                }
                sibling.Keys.RemoveAt(0);
            }
            // rotacja w prawo gdy target jest na prawo od brata
            else {
                target.Keys.Add(parent.Keys[parentKey]);
                parent.Keys.RemoveAt(parentKey);
                parent.Keys.Add(sibling.Keys[sibling.Keys.Count - 1]);
                sibling.Keys.RemoveAt(sibling.Keys.Count - 1);
                if (sibling.Children.Count > 0) {
                    target.InsertNode(sibling.Children[sibling.Children.Count - 1]);
                    sibling.Children.RemoveAt(sibling.Children.Count - 1);
                }

            }
            target.Keys.Sort();
            parent.Keys.Sort();
        }
        private void Merge(Node target, Node sibling, Node parent, bool root = false) {
            // bool root = true - rodzic to korzen
            int targetIdx = -1, siblingIdx = -1, parentKey = 0;
            for (int i = 0; i < parent.Children.Count; i++) {
                if (parent.Children[i] == target) {
                    targetIdx = i;
                }
                if (parent.Children[i] == sibling) {
                    siblingIdx = i;
                }
            }

            for (int i = 0; i < sibling.Keys.Count; i++) {
                target.Keys.Add(sibling.Keys[i]);
            }
            for (int i = 0; i < sibling.Children.Count; i++) {
                target.InsertNode(sibling.Children[i]);
            }
            sibling.Children.Clear();
            if (targetIdx == 2 || siblingIdx == 2) {
                parentKey = 1;
            }
            parent.Children.RemoveAt(siblingIdx);
            target.Keys.Add(parent.Keys[parentKey]);
            target.Keys.Sort();
            if (root == true && parent.Keys.Count == 1) {
                parent.Keys.RemoveAt(0);
                for (int i = 0; i < target.Keys.Count; i++) {
                    parent.Keys.Add(target.Keys[i]);
                }
                parent.Children.Clear();
                parent.Keys.Sort();
                for (int i = 0; i < target.Children.Count; i++) {
                    parent.InsertNode(target.Children[i]);
                }
            }
            else {
                parent.Keys.RemoveAt(parentKey);
            }

        }

        public int DeviceCount(string key) {
            // liczba urzadzen w danej podsieci X.X.X.*
            Node node = FindNetwork(key);
            Address network = new Address(key);
            int count = 0;
            if (node == null) {
                return 0;
            }
            for (int i = 0; i < node.Keys.Count; i++) {
                if (network.CompareNetwork(node.Keys[i]) == 0) {
                    count++;
                }
            }
            for (int i = 0; i < node.Children.Count; i++) {
                Node child = node.Children[i];
                count += child.DeviceCount(key);
            }
            return count;
        }
        public void PrintTree(string indent = "", bool last = true) {
            Console.Write(indent);
            if (last) {
                Console.Write("\\-");
                indent += "    ";
            }
            else {
                Console.Write("|-");
                indent += "|   ";
            }
            if (this.Keys.Count == 2) {
                Console.WriteLine($"{this.Keys[0]}, {this.Keys[1]}");
            }
            else {
                Console.WriteLine($"{this.Keys[0]}");
            }
            for (int i = 0; i < this.Children.Count; i++) {
                Children[i].PrintTree(indent, i == this.Children.Count - 1);
            }
        }
    }
}
