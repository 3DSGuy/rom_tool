using System;
using System.IO;
using System.Windows.Forms;

namespace _3DSExplorer.Modules
{
    public class TreeViewContextTag
    {
        public IContext Context;
        public int Type;
        public object[] Values;
        public string ActivationString;

        public static TreeViewContextTag Create(IContext context)
        {
            return new TreeViewContextTag { Context = context};
        }

        public static TreeViewContextTag Create(IContext context, int type)
        {
            return new TreeViewContextTag { Context = context, Type = type };
        }

        public static TreeViewContextTag Create(IContext context, int type, string activation)
        {
            return new TreeViewContextTag { Context = context, Type = type, ActivationString = activation};
        }

        public static TreeViewContextTag Create(IContext context, int type, object[] values)
        {
            return new TreeViewContextTag {Context = context, Type = type, Values = values};
        }

        public static TreeViewContextTag Create(IContext context, int type, string activation, object[] values)
        {
            return new TreeViewContextTag { Context = context, Type = type, ActivationString = activation, Values = values };
        }
    }

    public interface IContext
    {
        bool Open(Stream fs);
        string GetErrorMessage();
        void Create(FileStream fs, FileStream src);
        void View(frmExplorer f, int view, object[] values);
        bool CanCreate();
        void Activate(string filePath, int type, object[] values);

        string GetFileFilter();
        TreeNode GetExplorerTopNode();
        TreeNode GetFileSystemTopNode();
    }
}
