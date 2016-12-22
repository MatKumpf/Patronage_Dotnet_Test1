using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Patronage_Test1
{
    public enum ObjectType
    {
        File, Directory
    };

    public class Tree
    {
        private List<Tree> _children = new List<Tree>();
        private static StringBuilder treeStructure = new StringBuilder("");
        private static StringBuilder typeStructure = new StringBuilder("");
        private static StringBuilder timeStructure = new StringBuilder("");

        public FileSystemInfo Object { get; set; }
        public ObjectType Type { get; set; }

        public List<Tree> Children
        {
            get
            {
                return _children;
            }
        }

        public Tree(FileSystemInfo obj, ObjectType type)
        {
            Object = obj;
            Type = type;
        }

        public Tree AddChild(FileSystemInfo obj, ObjectType type)
        {
            Tree nodeItem = new Tree(obj, type);
            _children.Add(nodeItem);
            return nodeItem;
        }

        public void PrintTreeData()
        {
            InitializeData("", true);
            int measureName = MeasureName();
            string[] newLineChar = { Environment.NewLine, "\n" };
            string[] splitTree = treeStructure.ToString().Split(newLineChar, StringSplitOptions.RemoveEmptyEntries);
            string[] splitType = typeStructure.ToString().Split(newLineChar, StringSplitOptions.RemoveEmptyEntries);
            string[] splitTime = timeStructure.ToString().Split(newLineChar, StringSplitOptions.RemoveEmptyEntries);

            if (measureName <= 16)
            {
                ReplayCharInConsole("=", 54);
                Console.WriteLine();
                Console.Write("| Folder/File Name |    Type   |    Creation Time    |");
                Console.WriteLine();
                ReplayCharInConsole("=", 54);
                Console.WriteLine();
                for (int i = 0; i < splitTime.Length; i++)
                {
                    Console.Write("| " + splitTree[i]);
                    ReplayCharInConsole(" ", 16 - splitTree[i].Length);
                    Console.Write(" | ");
                    if (splitType[i] == "File")
                        Console.Write("  " + splitType[i] + "    | " + splitTime[i] + " |");
                    else
                        Console.Write(splitType[i] + " | " + splitTime[i] + " |");
                    Console.WriteLine();
                }

            }
            else
            {
                ReplayCharInConsole("=", measureName + 38);
                Console.WriteLine();
                Console.Write("|");
                ;
                ReplayCharInConsole(" ", Math.Ceiling(((double)measureName - 16) / 2));
                Console.Write(" Folder/File Name ");
                ReplayCharInConsole(" ", (measureName - 16) / 2);
                Console.Write("|    Type   |    Creation Time    |");
                Console.WriteLine();
                ReplayCharInConsole("=", measureName + 38);
                Console.WriteLine();
                for (int i = 0; i < splitTime.Length; i++)
                {
                    Console.Write("| " + splitTree[i]);
                    ReplayCharInConsole(" ", measureName - splitTree[i].Length);
                    Console.Write(" | ");
                    if (splitType[i] == "File")
                        Console.Write("  " + splitType[i] + "    | " + splitTime[i] + " |");
                    else
                        Console.Write(splitType[i] + " | " + splitTime[i] + " |");
                    Console.WriteLine();
                }
            }
        }

        private void InitializeData(string indent, bool last)
        {
            treeStructure.Append(indent);
            if (last)
            {
                treeStructure.Append("\\-");
                indent += "  ";
            }
            else
            {
                treeStructure.Append("|-");
                indent += "| ";
            }
            treeStructure.AppendLine(Object.Name);
            typeStructure.AppendLine(Type.ToString());
            timeStructure.AppendLine(Object.CreationTime.ToString());

            for (int i = 0; i < Children.Count; i++)
            {
                Children[i].InitializeData(indent, i == Children.Count - 1);
            }
        }

        private int MeasureName()
        {
            string[] newLineChar = { Environment.NewLine, "\n" };
            string[] temp = treeStructure.ToString().Split(newLineChar, StringSplitOptions.None);
            int measure = 0;
            foreach (string text in temp)
            {
                if (text.Length > measure)
                    measure = text.Length;
            }
            return measure;
        }

        private void ReplayCharInConsole(string character, double replay)
        {
            for (int i = 0; i < (int)replay; i++)
                Console.Write(character);
        }
    }
}
