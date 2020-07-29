﻿using System.Collections.Generic;
using Newtonsoft.Json;

namespace FirebaseStoragePathManager
{
    public class FirebaseStoragePathManager
    {
        public enum EAbstractFileType
        {
            File = 1,
            Directory = 2
        };

        public class Node
        {
            public struct InputPack_Directory
            {
                private Node mupperNode;
                private string mfileName;

                public InputPack_Directory(in Node upperNode, string fileName)
                {
                    mupperNode = upperNode;
                    mfileName = fileName;
                }

                public Node UpperNode
                {
                    get => mupperNode;
                }
                public string FileName
                {
                    get => mfileName;
                }
            };
            public struct InputPack_File
            {
                private Node mupperNode;
                private string mfileName;
                private List<string> mfileExtension;

                public InputPack_File(in Node upperNode, string fileName, in List<string> fileExtension)
                {
                    mupperNode = upperNode;
                    mfileName = fileName;
                    mfileExtension = fileExtension;
                }

                public Node UpperNode
                {
                    get => mupperNode;
                }
                public string FIleName
                {
                    get => mfileName;
                }
                public List<string> FileExtension
                {
                    get => mfileExtension;
                }
            };



            private string mfileName;
            private string mfullPath;
            private List<string> mfileExtension;

            private EAbstractFileType mabstractFileType;

            private Dictionary<string, Node> mbranchNodes;
            private List<string> mbranchNodeNames;



            /// <summary>
            /// Create new directory node
            /// </summary>
            /// <param name="upperNode"></param>
            /// <param name="fileName"></param>
            public Node(in InputPack_Directory inputPack)
            {
                mfileName = inputPack.FileName;
                mfullPath = CombinePath(inputPack.UpperNode.FullPath, inputPack.FileName);
                mfileExtension = null;

                mabstractFileType = EAbstractFileType.Directory;

                mbranchNodes = new Dictionary<string, Node>();
                mbranchNodeNames = new List<string>();

                inputPack.UpperNode.BranchNodes.Add(inputPack.FileName, this);
                inputPack.UpperNode.BranchNodeNames.Add(inputPack.FileName);
            }
            /// <summary>
            /// Create new file node
            /// </summary>
            /// <param name="upperNode"></param>
            /// <param name="fileName"></param>
            /// <param name="fileExtension"></param>
            public Node(in InputPack_File inputPack)
            {
                mfileName = inputPack.FIleName;
                mfullPath = CombinePath(inputPack.UpperNode.FullPath, inputPack.FIleName);
                mfileExtension = inputPack.FileExtension;

                mabstractFileType = EAbstractFileType.File;

                mbranchNodes = null;
                mbranchNodeNames = null;

                inputPack.UpperNode.BranchNodes.Add(inputPack.FIleName, this);
                inputPack.UpperNode.BranchNodeNames.Add(inputPack.FIleName);
            }

            public string FileName
            {
                get => mfileName;
            }
            public string FullPath
            {
                get => mfullPath;
            }
            public List<string> FileExtension
            {
                get => mfileExtension;
            }
            public EAbstractFileType AbstractFileType
            {
                get => mabstractFileType;
            }
            public Dictionary<string, Node> BranchNodes
            {
                get => mbranchNodes;
            }
            public List<string> BranchNodeNames
            {
                get => mbranchNodeNames;
            }

            internal static bool AddNode(in InputPack_Directory inputPack_Directory)
            {
                if(inputPack_Directory.UpperNode.AbstractFileType == EAbstractFileType.File)
                {
                    return false;
                }

                new Node(inputPack_Directory);
                return true;
            }
            internal static bool AddNode(in InputPack_File inputPack_File)
            {
                if(inputPack_File.UpperNode.AbstractFileType == EAbstractFileType.File)
                {
                    return false;
                }

                new Node(inputPack_File);
                return true;
            }
            internal static void RemoveNode(in Node targetNode)
            {
                if(targetNode.BranchNodeNames.Count != 0)
                {
                    for(int i = 0; i < targetNode.BranchNodeNames.Count; i++)
                    {
                        RemoveNode(targetNode.BranchNodes[targetNode.BranchNodeNames[i]]);
                    }
                }

                targetNode.mfullPath = null;
                targetNode.mfileName = null;
                targetNode.mfileExtension.Clear();
                targetNode.mbranchNodeNames.Clear();
                targetNode.mbranchNodes.Clear();
            }
            public static string GetCompletePath(in Node targetNode)
            {
                switch(targetNode.AbstractFileType)
                {
                    case EAbstractFileType.Directory:
                        return targetNode.FullPath;

                    case EAbstractFileType.File:
                        string tempPath = targetNode.FullPath;
                        for(int i = 0; i < targetNode.FileExtension.Count; i++)
                        {
                            tempPath += "." + targetNode.FileExtension[i];
                        }
                        return tempPath;
                }

                return default(string);
            }

            private static string CombinePath(string upperFullPath, string fileName)
            {
                return upperFullPath + "/" + fileName;
            }
        };



        private Node mfundementalNode;
        private string mpathManagerRoll;



        public Node FundementalNode
        {
            get => mfundementalNode;
        }
        public string PathManagerRoll
        {
            get => mpathManagerRoll;
            set => mpathManagerRoll = value;
        }

        public bool AddNode(Node.InputPack_Directory inputPack_Directory)
        {
            return Node.AddNode(inputPack_Directory);
        }
        public bool AddNode(in Node.InputPack_File inputPack_File)
        {
            return Node.AddNode(inputPack_File);
        }
        public void RemoveNode(in Node targetNode)
        {
            Node.RemoveNode(targetNode);
        }
        public byte[] GetJsonData()
        {
            return System.Text.Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(this));
        }
    }
}