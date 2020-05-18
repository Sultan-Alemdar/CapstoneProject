using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq.Expressions;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;

namespace DesktopApp.Core.Models
{
    public sealed class File : ModelBase
    {
        private string format;
        private int fileSize;
        private string where;
        private string state;
        public string Format { get => format; set => SetProperty<string>(ref this.format, value, "Format"); }
        public int FileSize { get => fileSize; set => SetProperty<int>(ref this.fileSize, value, "FileSize"); }
        public string Where { get => where; set => SetProperty<string>(ref this.where, value, "Where"); }
        public string State { get => state; set => SetProperty<string>(ref this.state, value, "State"); }

        public File(string name, string directory, string format, int fileSize, string where, string state)
        {
            Name = name;
            Directory = directory;
            Format = format;
            FileSize = fileSize;
            Where = where;
            State = state;

        }

    }
}


