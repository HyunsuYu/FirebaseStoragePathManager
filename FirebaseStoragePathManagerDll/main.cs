﻿using System.Collections.Generic;
using System.Dynamic;
using System.IO;
using Newtonsoft.Json;

namespace FirebaseStoragePathManager
{
    public class EditorPathManager
    {
        public enum EDetailType
        {
            AxisFullPack = 1,
            AxisPalette = 2,
            AxisOrderedCube = 3,
            AxisOrderedType = 4,
            DirectPalette = 5,
            DirectPack = 6
        };
        public enum EFileType
        {
            Json = 1,
            PNG = 2
        };

        public class Node
        {
            public struct InputPack
            {
                private string mfileName;
                private EDetailType mdetailType;
                private EFileType mfileType;

                public InputPack(string fileName, EDetailType detailType, EFileType fileType)
                {
                    mfileName = fileName;
                    mdetailType = detailType;
                    mfileType = fileType;
                }
                public InputPack(InputPack inputPack)
                {
                    mfileName = inputPack.FileName;
                    mdetailType = inputPack.DetailType;
                    mfileType = inputPack.FileType;
                }

                public string FileName
                {
                    get => mfileName;
                }
                public EDetailType DetailType
                {
                    get => mdetailType;
                }
                public EFileType FileType
                {
                    get => mfileType;
                }
            }

            private string mfileName;
            private EDetailType mdetailType;
            private EFileType mfileType;

            public Node(InputPack inputPack)
            {
                mfileName = inputPack.FileName;
                mdetailType = inputPack.DetailType;
                mfileType = inputPack.FileType;
            }

            public string FileName
            {
                get => mfileName;
            }
            public EDetailType DetailType
            {
                get => mdetailType;
            }
            public EFileType FileType
            {
                get => mfileType;
            }

            public string GetFullPath()
            {
                string tempPath = "";

                switch(DetailType)
                {
                    case EDetailType.AxisFullPack:
                        tempPath = Axis_FullPackURL + FileName + Axis_FullPackExtension;
                        break;

                    case EDetailType.AxisPalette:
                        tempPath = Axis_PaletteURL + FileName + Axis_PaletteExtension;
                        break;

                    case EDetailType.AxisOrderedCube:
                        tempPath = Axis_OrderedCubeURL + FileName + Axis_OrderedCubeExtension;
                        break;

                    case EDetailType.AxisOrderedType:
                        tempPath = Axis_OrderedTypeURL + FileName + Axis_OrderedTypeExtension;
                        break;
                }

                switch(FileType)
                {
                    case EFileType.Json:
                        tempPath += ".json";
                        break;

                    case EFileType.PNG:
                        tempPath += ".png";
                        break;
                }

                return tempPath;
            }
        }



        private Dictionary<string, Node> mnodeTable;
        private List<string> mnodeNames;
        private string mstorageURL;



        public EditorPathManager(string storageURL)
        {
            mnodeTable = new Dictionary<string, Node>();
            mnodeNames = new List<string>();
            mstorageURL = storageURL;
        }
        public EditorPathManager(EditorPathManager editorPathManager)
        {
            mnodeTable = editorPathManager.NodeTable;
            mnodeNames = editorPathManager.NodeNames;
            mstorageURL = editorPathManager.StorageURL;
        }

        public Dictionary<string, Node> NodeTable
        {
            get => mnodeTable;
        }
        public List<string> NodeNames
        {
            get => mnodeNames;
        }
        public string StorageURL
        {
            get => mstorageURL;
        }
        public static string EditorURL
        {
            get => "Editor/";
        }
        public static string AxisURL
        {
            get => EditorURL + "Axis/";
        }
        public static string Axis_FullPackURL
        {
            get => AxisURL + "AxisCubeTable/";
        }
        public static string Axis_PaletteURL
        {
            get => AxisURL + "Palette/";
        }
        public static string Axis_OrderedCubeURL
        {
            get => AxisURL + "OrderedCube/";
        }
        public static string Axis_OrderedTypeURL
        {
            get => AxisURL + "OrderedType";
        }
        public static string Axis_FullPackExtension
        {
            get => ".AxisFullPack";
        }
        public static string Axis_PaletteExtension
        {
            get => ".AxisPalette";
        }
        public static string Axis_OrderedCubeExtension
        {
            get => ".AxisOrderedCube";
        }
        public static string Axis_OrderedTypeExtension
        {
            get => ".AxisOrderedType";
        }
        public static string PathManagerURL
        {
            get => EditorURL + "EditorPathManager.PathManager.json";
        }

        public bool TryAddNode(Node.InputPack nodeData)
        {
            if(mnodeTable.ContainsKey(nodeData.FileName))
            {
                return false;
            }

            mnodeTable.Add(nodeData.FileName, new Node(new Node.InputPack(nodeData)));
            mnodeNames.Add(nodeData.FileName);
            return true;
        }
        public bool TryRemoveNode(string nodeName)
        {
            if(mnodeTable.Remove(nodeName))
            {
                mnodeNames.Remove(nodeName);
                return true;
            }
            return false;
        }
        public List<Node> SearchSpecificNodes(EDetailType detailType)
        {
            List<Node> tempList = new List<Node>();

            for(int index = 0; index < mnodeNames.Count; index++)
            {
                if(mnodeTable[mnodeNames[index]].DetailType == detailType)
                {
                    tempList.Add(mnodeTable[mnodeNames[index]]);
                }
            }

            return tempList;
        }
        public byte[] GetJsonData()
        {
            return System.Text.Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(this));
        }
    }
}