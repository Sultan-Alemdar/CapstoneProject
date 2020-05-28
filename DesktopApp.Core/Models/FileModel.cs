using DesktopApp.Core.Models.Base;
using System;
using System.Collections.Generic;
using System.Text;

namespace DesktopApp.Core.Models
{
    public class FileModel : BindableBase
    {
        public enum EnumEvent
        {
            Update = 0,
            Download = 1,
        }
        public enum EnumStatus
        {
            Waiting = 0,
            Progressing = 1,
            Completed = 2,
            Cancelled = 3, 
        }
        public enum EnumFileState
        {
            Offered = 0,
            Accepted = 1,
            Declined = 2,
        }

        private string _fileName = "Deneme Dokümanı";      //deneme
        private string _fileType = "pdf";       //pdf ...
        private EnumFileState _fileState = EnumFileState.Offered;       //offered, accepted, declined
        private string _actionSpeed = "1020kbps";       //kbp
        private string _percent = "20";       //total/proggresedSİze
        private string _progressedSize = "1GB";         //1024kb
        private string _totalSize = "10GB";      //1024kb
        private EnumStatus _status = EnumStatus.Waiting;     //cancelled, progressing, completed
        private EnumEvent _event = EnumEvent.Update;       //update,download

        public string FileName { get => _fileName; set => SetProperty<string>(ref this._fileName, value, "FileName"); }
        public string FileType { get => _fileType; set => SetProperty<string>(ref this._fileType, value, "FileType"); }
        public EnumFileState FileState { get => _fileState; set => SetProperty<EnumFileState>(ref this._fileState, value, "FileState"); }
        public string ActionSpeed { get => _actionSpeed; set => SetProperty<string>(ref this._actionSpeed, value, "ActionSpeed"); }
        public string Percent { get => _percent; set => SetProperty<string>(ref this._percent, value, "Percent"); }
        public string TotalSize { get => _totalSize; set => SetProperty<string>(ref this._totalSize, value, "TotalSize"); }
        public string ProgressedSize { get => _progressedSize; set => SetProperty<string>(ref this._progressedSize, value, "ProgresedSize"); }
        public EnumStatus Status { get => _status; set => SetProperty<EnumStatus>(ref this._status, value, "Status"); }
        public EnumEvent Event { get => _event; set => SetProperty<EnumEvent>(ref this._event, value, "Event"); }
    }
}
