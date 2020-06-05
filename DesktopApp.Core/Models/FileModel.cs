using DesktopApp.Core.Models.Base;
using Newtonsoft.Json;
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
            InQueue = 4,
            Progressing = 1,
            Completed = 2,
            Declined = 3,
            WritingToStorageError = 5
        }
        public enum EnumFileState
        {
            Offered = 0,
            Accepted = 1,
            Canceled = 2,
            Endded = 3,
            Started = 4,
            Failure = 5
        }
        [JsonProperty] private readonly string _id; //<PID>546</PID><MID>53</MID>
        [JsonProperty] private readonly string _fileName = "Deneme Dokümanı";      //deneme
        [JsonProperty] private readonly string _fileType = "pdf";       //pdf ...
        [JsonProperty] private readonly string _fileDisplayName;
        [JsonProperty] private readonly string _fileDisplayType;
        [JsonProperty] private EnumFileState _fileState = EnumFileState.Offered;       //offered, accepted, declined
        [JsonProperty] private long _actionSpeed = 0;       //kbp
        [JsonProperty] private float _percent = 0;       //total/proggresedSİze
        [JsonProperty] private long _progressedSize = 0;         //1024kb
        [JsonProperty] private readonly ulong _totalSize = 0;      //1024kb
        [JsonProperty] private EnumProccesStatus _proccesStatus = EnumProccesStatus.Waiting;     //cancelled, progressing, completed
        [JsonProperty] private EnumEvent _event = EnumEvent.Download;       //upload,download

        public string Id { get => _id; }
        public string FileName { get => _fileName; }
        public string FileType { get => _fileType; }



        public EnumFileState FileState { get => _fileState; set => SetProperty<EnumFileState>(ref this._fileState, value, "FileState"); }
        public long ActionSpeed { get => _actionSpeed; set => SetProperty<long>(ref this._actionSpeed, value, "ActionSpeed"); }
        public float Percent
        {
            get => _percent; private set
            {
                float val = 0;
                if (TotalSize == 0)
                    throw new ArgumentNullException();
                else if (_progressedSize == 0)
                {
                    Debug.WriteLine("[FileMOdel] Progressed size is 0:");
                    val = 0;
                }
                else
                {
                    val = (float)((_progressedSize * 100) / (long)_totalSize);


                }
                SetProperty<float>(ref this._percent, val, "Percent");
            }
        }
        public long TotalSize { get => (long)_totalSize; }
        public long ProgressedSize { get => _progressedSize; set => SetProperty<long>(ref this._progressedSize, value, "ProgressedSize"); }
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
        public FileModel(string fileId, string fileName, string fileType, long totalSize, string fileDisplayName, string fileDisplayType)
        {
            _id = fileId;
            _fileName = fileName;
            _fileType = fileType;
            _totalSize = (ulong)totalSize;
            _fileDisplayName = fileDisplayName;
            _fileDisplayType = fileDisplayType;
        }



        public void ShowPercent()
        {
            this.Percent = 0;//sadece tetikleme.
        }
        public void SetOfferedStateConfig()
        {
            this._fileState = FileModel.EnumFileState.Offered;
            this._proccesStatus = FileModel.EnumProccesStatus.Waiting;
            this._event = EnumEvent.Upload;
        }
        public void SetCanceledStateConfig()
        {
            this.FileState = FileModel.EnumFileState.Canceled;
            this.ProccesStatus = FileModel.EnumProccesStatus.Declined;
        }
        public void SetAcceptedStateConfig()
        {
            this.FileState = FileModel.EnumFileState.Accepted;
            this.ProccesStatus = FileModel.EnumProccesStatus.InQueue;
        }
        public void SetEndedStateConfig()
        {
            this.FileState = FileModel.EnumFileState.Endded;
            this.ProccesStatus = FileModel.EnumProccesStatus.Completed;
        }
        public void SetStartedStateConfig()
        {
            this.FileState = FileModel.EnumFileState.Started;
            this.ProccesStatus = FileModel.EnumProccesStatus.Progressing;
        }
        public void SetFailureStateConfig()
        {
            this.FileState = FileModel.EnumFileState.Failure;
            this.ProccesStatus = FileModel.EnumProccesStatus.WritingToStorageError;
        }

        public bool IsCanceled => (_fileState == EnumFileState.Canceled && _proccesStatus == EnumProccesStatus.Declined);
        public bool IsAccepted => (_fileState == EnumFileState.Accepted && _proccesStatus == EnumProccesStatus.InQueue);
        public bool IsStarted => (_fileState == EnumFileState.Started && _proccesStatus == EnumProccesStatus.Progressing);
        public bool IsEnded => (_fileState == EnumFileState.Endded && _proccesStatus == EnumProccesStatus.Completed);
        public bool IsDownload => (_event == EnumEvent.Download);

        public string FileDisplayName => _fileDisplayName;

        public string FileDisplayType => _fileDisplayType;
    }
}
