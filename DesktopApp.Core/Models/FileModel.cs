using DesktopApp.Core.Models.Base;
using System;
using System.Collections.Generic;
using System.Text;

namespace DesktopApp.Core.Models
{
    public class FileModel : BindableBase
    {
        private string _fileName; //deneme
        private string _fileType; //pdf ...
        private string _fileState;//offered, accepted, declined
        private string _actionSpeed; //kbp
        private string _percent;//total/proggresedSİze
        private string _progresedSize;//1024kb
        private string _totalSize;//1024kb
        private string _status;//cancelled, progressing, completed
        private string _event;

        public string FileName { get => _fileName; set => SetProperty<string>(ref this._fileName, value, "FileName"); }
        public string FileType { get => _fileType; set => SetProperty<string>(ref this._fileType, value, "FileType"); }
        public string FileState { get => _fileState; set => SetProperty<string>(ref this._fileState, value, "FileState"); }
        public string ActionSpeed { get => _actionSpeed; set => SetProperty<string>(ref this._actionSpeed, value, "ActionSpeed"); }
        public string Percent { get => _percent; set => SetProperty<string>(ref this._percent, value, "Percent"); }
        public string TotalSize { get => _totalSize; set => SetProperty<string>(ref this._totalSize, value, "TotalSize"); }
        public string ProgresedSize { get => _progresedSize; set => SetProperty<string>(ref this._progresedSize, value, "ProgresedSize"); }
        public string Status { get => _status; set => SetProperty<string>(ref this._status, value, "Status"); }
        public string Event { get => _event; set => SetProperty<string>(ref this._event, value, "Event"); }
    }
}
