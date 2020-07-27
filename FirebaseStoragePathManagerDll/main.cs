using System;
using System.Collections.Generic;

namespace FirebaseStoragePathManager
{
    #region Defines
    
    #endregion

    #region MainClass
    public class FirebaseStoragePathManager_Formal<DetailFileType>
    {
        public enum AbstractFileType
        {
            File = 1,
            Directory = 2
        };

        public struct Node
        {
            public string mname;
            public string mpath;
            public AbstractFileType mabstractFileType;
            public DetailFileType mdetailFileType;

            public Dictionary<string, Node> childBranch;

            public Node DefaultInstance
            {
                get => new Node();
            }
        }
    }
    #endregion
}