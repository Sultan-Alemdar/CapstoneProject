using DesktopApp.Core.Models.Base;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace DesktopApp.Core.Models
{
    public class FileModel : BindableBase
    {
        public enum EnumEvent
        {
            Upload = 0,
            Download = 1,
        }
        public enum EnumProccesStatus
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
            Shared = 3,

        }
        private readonly string _fileId; //<PID>546</PID><MID>53</MID>
        private readonly string _fileName = "Deneme Dokümanı";      //deneme
        private readonly string _fileType = "pdf";       //pdf ...

        private EnumFileState _fileState = EnumFileState.Offered;       //offered, accepted, declined
        private ulong _actionSpeed = 0;       //kbp
        private ulong _percent = 0;       //total/proggresedSİze
        private ulong _progressedSize = 0;         //1024kb
        private readonly ulong _totalSize = 0;      //1024kb
        private EnumProccesStatus _proccesStatus = EnumProccesStatus.Waiting;     //cancelled, progressing, completed
        private EnumEvent _event = EnumEvent.Upload;       //upload,download

        public string FileId { get => _fileId; }
        public string FileName { get => _fileName; }
        public string FileType { get => _fileType; }



        public EnumFileState FileState { get => _fileState; set => SetProperty<EnumFileState>(ref this._fileState, value, "FileState"); }
        public ulong ActionSpeed { get => _actionSpeed; set => SetProperty<ulong>(ref this._actionSpeed, value, "ActionSpeed"); }
        public ulong Percent
        {
            get => (this._progressedSize / this._totalSize) * 100; private set
            {
                ulong val = 0;
                if (TotalSize == 0)
                    throw new ArgumentNullException();
                else if (_progressedSize == 0)
                {
                    Debug.WriteLine("[FileMOdel] Progressed size is 0:");
                    val = 0;
                }
                else
                    val = (this._progressedSize / this._totalSize) * 100;
                SetProperty<ulong>(ref this._percent, val, "Percent");
            }
        }
        public ulong TotalSize { get => _totalSize; }
        public ulong ProgressedSize { get => _progressedSize; set => SetProperty<ulong>(ref this._progressedSize, value, "ProgresedSize"); }
        public EnumProccesStatus ProccesStatus { get => _proccesStatus; private set => SetProperty<EnumProccesStatus>(ref this._proccesStatus, value, "ProccesStatus"); }
        public EnumEvent Event { get => _event; set => SetProperty<EnumEvent>(ref this._event, value, "Event"); }



        /// <summary>
        /// FileModel constructor. It gets readonly paremeters only. Instance will be set as offred  state config.
        /// Offred state config:  offred, waiting, upload.
        /// </summary>
        /// <param name="fileId"></param>
        /// <param name="fileName"></param>
        /// <param name="fileType"></param>
        /// <param name="totalSize"></param>
        public FileModel(string fileId, string fileName, string fileType, ulong totalSize)
        {
            _fileId = fileId;
            _fileName = fileName;
            _fileType = fileType;
            _totalSize = totalSize;
            SetOfferedStateConfig();
        }



        public void ShowPercent()
        {
            this.Percent = 0;//sadece tetikleme.
        }
        private void SetOfferedStateConfig()
        {
            this._fileState = FileModel.EnumFileState.Offered;
            this._proccesStatus = FileModel.EnumProccesStatus.Waiting;
            this._event = EnumEvent.Upload;
        }
        public void SetDeclinedStateConfig()
        {
            this.FileState = FileModel.EnumFileState.Declined;
            this.ProccesStatus = FileModel.EnumProccesStatus.Cancelled;
        }
        public void SetAcceptedStateConfig()
        {
            this.FileState = FileModel.EnumFileState.Accepted;
            this.ProccesStatus = FileModel.EnumProccesStatus.Progressing;
        }
        public void SetSharedStateConfig()
        {
            this.FileState = FileModel.EnumFileState.Shared;
            this.ProccesStatus = FileModel.EnumProccesStatus.Completed;
        }

        public bool IsDeclined => (_fileState == EnumFileState.Declined && _proccesStatus == EnumProccesStatus.Cancelled);
        public bool IsAccepted => (_fileState == EnumFileState.Accepted && _proccesStatus == EnumProccesStatus.Progressing);
        public bool IsShared => (_fileState == EnumFileState.Shared && _proccesStatus == EnumProccesStatus.Completed);

  
    }
}
