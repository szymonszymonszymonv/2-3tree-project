using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Reflection.Metadata.Ecma335;
using System.Transactions;

namespace ASD_PROJEKT_3B {
    // 2-3 drzewa (M = 1 w b-drzewie)
    // 1-2 klucze w każdym węźle
    class Address : IComparable, IEquatable<Address> {
        public int[] Addr { get; set; }
        public string StrAddr { get; set; } // zeby ladny ToString() byl :)
        public Address(string addr) { 
            StrAddr = addr;
            Addr = Array.ConvertAll(addr.Split("."), x => int.Parse(x)); // "192.168.0.1" -> [192, 168, 0, 1]
        }
        public void ConvertToNetwork() {
            int[] network = { Addr[0], Addr[1], Addr[2] };
            Addr = network;
            StrAddr = string.Join('.', network);
        }
        public int CompareTo(object obj) {
            if(obj == null) {
                return 1;
            }
            Address other = obj as Address;
            for(int i = 0; i < Addr.Length; i++) {
                if(this.Addr[i] == other.Addr[i]) {
                    continue;
                }
                return this.Addr[i].CompareTo(other.Addr[i]);
            }
            return 0;
        }
        public int CompareNetwork(object obj) {
            if (obj == null) {
                return 1;
            }
            Address other = obj as Address;
            for(int i = 0; i < 3; i++) {
                if(this.Addr[i] == other.Addr[i]) {
                    continue;
                }
                return this.Addr[i].CompareTo(other.Addr[i]);
            }
            return 0;
        }
        public bool Equals(Address other) {
            if(other == null) {
                return false;
            }
            if(this.StrAddr == other.StrAddr) {
                return true;
            }
            else {
                return false;
            }
        }
        public override bool Equals(object obj) {
            if(obj == null) {
                return false;
            }
            Address other = obj as Address;
            if(other == null) {
                return false;
            }
            else {
                return Equals(other);
            }
        }
        public override int GetHashCode() {
            return StrAddr.GetHashCode();
        }
        public override string ToString() {
            return StrAddr;
        }
    }

    class Program {
        static void Main(string[] args) {

            Node drzewko_test = new Node();
            Graph grafik = new Graph(drzewko_test);
            Test test = new Test(drzewko_test, "projekt3_in5.txt", grafik);
            test.Launch();
            drzewko_test.PrintTree();
            Console.WriteLine(":)");

        }
    }
}
