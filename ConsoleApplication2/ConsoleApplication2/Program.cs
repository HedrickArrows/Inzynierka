using System;
using System.Collections.Generic;
using System.Linq;

namespace ConsoleApplication2
{
    public class Tree {
        
        public List<string> attrList;
        public Branch root;
        public List<Branch> leafs;
        public List<List<string>> l;
        public List<string> grpres;
        public double poss;
        public Random r;
        public int groups, limit;

        public Tree(int a, int g, int m) {
            attrList = new List<string>();
            grpres = new List<string>();
            limit = m;
            for (int i = 0; i < a; i++)
                attrList.Add(i.ToString());
            groups = g;
            poss = Math.Pow(2, limit);
            r = new Random();
            leafs = new List<Branch>();
            root = new Branch("", null, "", this);
            root.DoTree(attrList, limit);
            l = new List<List<string>>();

            foreach (Branch b in leafs)
            {
                b.grp = r.Next(groups).ToString();
                if (b.sibling().leaf)
                    while (b.grp.Equals(b.sibling().grp))
                       b.grp = r.Next(groups).ToString();
                Console.Write("Group " + b.grp + " ");
                grpres.Add(b.grp);
                List<string> tmp = new List<string>();
                l.Add(tmp);
                foreach (string s in attrList)
                    tmp.Add("-");
                Branch temp = b;
                while (temp.parent != null) {
                    tmp[Int32.Parse(temp.attr)] = temp.val;
                    temp = temp.parent;
                }
                foreach (string s in tmp)
                    Console.Write(s);
                Console.Write("\n");
            }
        }
        public class Branch {
            public Branch parent, childL, childR;
            public string attr, chs, val, grp;
            public bool leaf;
            public Tree g;
            public Branch(string a, Branch p, string v, Tree t)
            {
                attr = a;
                parent = p;
                val = v;
                g = t;
            }

            public Branch sibling() {
                return parent.childL == this ? parent.childR : parent.childL;
            }

            public void DoTree(List<string> attrs, int lim) {
                
                chs = ((lim > 0) ? attrs.ElementAt(g.r.Next(attrs.Count())) : "0");
                Console.WriteLine("Attr no " + chs);
                List<string> _attrs = new List<string>(attrs);
                _attrs.Remove(chs);
                childL = new Branch(chs, this, "0", g);
                Console.WriteLine("Left");
                Decide(_attrs, childL, lim -1);
                childR = new Branch(chs, this, "1", g);
                Console.WriteLine("Right");
                Decide(_attrs, childR, lim -1);
            }

            private void Decide(List<string> attrs, Branch b, int lim) {
                double h = Math.Pow(2, lim);
                if (((g.groups - g.leafs.Count() - 1 < g.poss - h)
                    && (g.r.NextDouble() > h * 1.5 / g.poss)) 
                    || lim == 0) 
                {
                    b.leaf = true;
                    g.leafs.Add(b);
                    g.poss -= h;
                    Console.WriteLine("Done here, possibilities left: " + g.poss);
                }
                else { 
                    Console.WriteLine("Go, attrs: " + (attrs.Count()));
                     b.DoTree(attrs, lim);

                }
            }
        }
    }
    class Program
    {
        static void Main(string[] args)
        {
            int input, ex, limit, results;
            Random r = new Random();
            Console.WriteLine("Examples: ");
            ex = Convert.ToInt32(Console.ReadLine());
            Console.WriteLine("Attrs: ");
            input = Convert.ToInt32(Console.ReadLine());
            Console.WriteLine("Limit: ");
            limit = Convert.ToInt32(Console.ReadLine());
            if (limit > input || limit <= 0) limit = input;
            Console.WriteLine("Results: ");
            results = Convert.ToInt32(Console.ReadLine());
            Tree t = new Tree( input, ex, limit);

            List<string> grps = new List<string>();

            System.IO.StreamWriter resData = new System.IO.StreamWriter("D:\\resData.txt");
            System.IO.StreamWriter resGrps = new System.IO.StreamWriter("D:\\resGrps.txt");

            List<int> tempGrps = new List<int>();
            for (int i = 0; i < results; i++)
            
               tempGrps.Add(t.r.Next(t.l.Count()));

            using (resData)
            {
                foreach (int i in tempGrps)
                {
                    string temp = "";
                    foreach (string s in t.l[i])
                    {
                        if (s.Equals("-"))
                            temp += (t.r.Next(1).ToString());
                        else
                            temp += (s);
                        temp += ",";
                    }
                    temp = temp.Remove(temp.Length - 1, 1) + "\n";
                    resData.WriteLine(temp);
                }
            }
            using (resGrps)
            {
                foreach (int i in tempGrps)
                {
                    resGrps.WriteLine(t.grpres[i]);
                }
            }
            Console.ReadKey();
        }
    }
}
