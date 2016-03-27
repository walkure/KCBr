using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;
using System.Diagnostics;


namespace KCB2
{
    /*
     * Mixer APIを叩いてプロセスの音量を調整する
     * XPまでだとシステムグローバルの音量を調整してしまう
     * 
     * Vista以降だと、mixer APIの値をプロセスごとに覚える変更が入ったので
     * プロセスの音量を調整できる
     * 
     * http://hp.vector.co.jp/authors/VA016117/mixer1.html
     * http://hp.vector.co.jp/authors/VA016117/mixer2.html
     * http://www11.ocn.ne.jp/~ikalu/csharp/0005.html
     * http://www.pinvoke.net/ ...
     */
    public class MixerAPI : IDisposable
    {

        #region mixerOpen
        public enum MIXER_OBJECTF : uint
        {
            HANDLE = 0x80000000u,
            MIXER = 0x00000000u,
            HMIXER = (HANDLE | MIXER),
            WAVEOUT = 0x10000000u,
            HWAVEOUT = (HANDLE | WAVEOUT),
            WAVEIN = 0x20000000u,
            HWAVEIN = (HANDLE | WAVEIN),
            MIDIOUT = 0x30000000u,
            HMIDIOUT = (HANDLE | MIDIOUT),
            MIDIIN = 0x40000000u,
            HMIDIIN = (HANDLE | MIDIIN),
            AUX = 0x50000000u,
        }
        public enum MMRESULT : uint
        {
            MMSYSERR_NOERROR = 0,
            MMSYSERR_ERROR = 1,
            MMSYSERR_BADDEVICEID = 2,
            MMSYSERR_NOTENABLED = 3,
            MMSYSERR_ALLOCATED = 4,
            MMSYSERR_INVALHANDLE = 5,
            MMSYSERR_NODRIVER = 6,
            MMSYSERR_NOMEM = 7,
            MMSYSERR_NOTSUPPORTED = 8,
            MMSYSERR_BADERRNUM = 9,
            MMSYSERR_INVALFLAG = 10,
            MMSYSERR_INVALPARAM = 11,
            MMSYSERR_HANDLEBUSY = 12,
            MMSYSERR_INVALIDALIAS = 13,
            MMSYSERR_BADDB = 14,
            MMSYSERR_KEYNOTFOUND = 15,
            MMSYSERR_READERROR = 16,
            MMSYSERR_WRITEERROR = 17,
            MMSYSERR_DELETEERROR = 18,
            MMSYSERR_VALNOTFOUND = 19,
            MMSYSERR_NODRIVERCB = 20,
            WAVERR_BADFORMAT = 32,
            WAVERR_STILLPLAYING = 33,
            WAVERR_UNPREPARED = 34
        }

        [DllImport("winmm.dll")]
        static extern uint mixerOpen(ref IntPtr phmx, uint pMxId,
                    IntPtr dwCallback, IntPtr dwInstance, MIXER_OBJECTF fdwOpen);
        #endregion

        #region mixerGetLineInfo
        public enum MIXERLINE_TARGETTYPE : uint
        {
            UNDEFINED = 0,
            WAVEOUT = 1,
            WAVEIN = 2,
            MIDIOUT = 3,
            MIDIIN = 4,
            AUX = 5,
        }
        public enum MIXERLINE_LINEF : uint
        {
            ACTIVE = 0x00000001u,
            DISCONNECTED = 0x00008000u,
            SOURCE = 0x80000000u,
        }
        public enum MIXERLINE_COMPONENTTYPE : uint
        {
            DST_FIRST = 0x00000000u,
            DST_UNDEFINED = (DST_FIRST + 0),
            DST_DIGITAL = (DST_FIRST + 1),
            DST_LINE = (DST_FIRST + 2),
            DST_MONITOR = (DST_FIRST + 3),
            DST_SPEAKERS = (DST_FIRST + 4),
            DST_HEADPHONES = (DST_FIRST + 5),
            DST_TELEPHONE = (DST_FIRST + 6),
            DST_WAVEIN = (DST_FIRST + 7),
            DST_VOICEIN = (DST_FIRST + 8),
            DST_LAST = (DST_FIRST + 8),
            SRC_FIRST = 0x00001000u,
            SRC_UNDEFINED = (SRC_FIRST + 0),
            SRC_DIGITAL = (SRC_FIRST + 1),
            SRC_LINE = (SRC_FIRST + 2),
            SRC_MICROPHONE = (SRC_FIRST + 3),
            SRC_SYNTHESIZER = (SRC_FIRST + 4),
            SRC_COMPACTDISC = (SRC_FIRST + 5),
            SRC_TELEPHONE = (SRC_FIRST + 6),
            SRC_PCSPEAKER = (SRC_FIRST + 7),
            SRC_WAVEOUT = (SRC_FIRST + 8),
            SRC_AUXILIARY = (SRC_FIRST + 9),
            SRC_ANALOG = (SRC_FIRST + 10),
            SRC_LAST = (SRC_FIRST + 10),
        }

        private const uint MIXER_SHORT_NAME_CHARS = 16U; //ソースライン名（ショート）の制限文字数
        private const uint MIXER_LONG_NAME_CHARS = 64U; //ソースライン名（ロング）  の制限文字数
        public const uint MAXPNAMELEN = 32U; //製品名を格納する最大文字数

        [StructLayout(LayoutKind.Sequential)]
        public struct MIXERLINE
        {
            public uint StructSize;          /* MIXERLINE 構造体のサイズを入れる   */
            public uint dwDestination;     /* ディストネーション索引番号の指定   */
            public int dwSource;          /* ソースライン索引番号の指定         */
            public uint dwLineID;          /* ソースラインのローカル番号   ID    */
            public uint fdwLine;           /* ソースラインの状態を表すフラグ     */
            public uint dwUser;            /* 特例情報（通常は無視）             */
            public uint ComponentType;   /* ライン端子の種別を表すフラグ       */
            public uint cChannels;         /* 指定ソースラインの最大チャンネル数 */
            public uint cConnections;      /* 指定ディスト~  のソースライン数    */
            public uint cControls;         /* 指定ソースラインのコントロール数   */

            [MarshalAs(UnmanagedType.ByValTStr,
                 SizeConst = (int)MIXER_SHORT_NAME_CHARS)]
            public string szShortName;         /* ソースライン名（ショート）         */

            [MarshalAs(UnmanagedType.ByValTStr,
                 SizeConst = (int)MIXER_LONG_NAME_CHARS)]
            public string szName;             /* ソースライン名（ロング）           */

            //Struct Target
            //
            //  構造体にメンバとして他の構造体を指定すると、構造体メンバのメンバが
            //　ドット演算子で参照できないので、構造体メンバのメンバを親構造体の直
            //　接のメンバとして指定してしまう
            //
            public uint Target_dwType;     /* MIXERLINE_TARGETTYPE_xxxx          */
            // 以下は MIXERCAPS 構造体のメンバと同一
            public uint Target_dwDeviceID; /* 対象装置のデバイス（番号） ID      */
            public ushort Target_wMid;         /* 対象装置のメーカー（番号） id      */
            public ushort Target_wPid;         /* 対象装置の製造（番号） id          */
            public uint Target_vDriverVersion;
            /* 対象装置のドライババージョン       */
            [MarshalAs(UnmanagedType.ByValTStr,
                    SizeConst = (int)MAXPNAMELEN)]
            public string Target_szPname;         /* 対象装置の製品名                   */
        }

        [Flags]
        public enum MIXER_GETLINEINFOF : uint
        {
            DESTINATION = 0x00000000u,
            SOURCE = 0x00000001u,
            LINEID = 0x00000002u,
            COMPONENTTYPE = 0x00000003u,
            TARGETTYPE = 0x00000004u,
            QUERYMASK = 0x0000000Fu,
        }

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 4)]
        public struct MIXERLINECONTROLS
        {
            private UInt32 cbStruct;       /* size in bytes of MIXERLINECONTROLS */
            private UInt32 dwLineID;       /* line id (from MIXERLINE.dwLineID) */
            private UInt32 dwControlID;    /* MIXER_GETLINECONTROLSF_ONEBYID */
            //    private UInt32 dwControlType;  //UNIONED with dwControlID /* MIXER_GETLINECONTROLSF_ONEBYTYPE */
            private UInt32 cControls;      /* count of controls pmxctrl points to */
            private UInt32 cbmxctrl;       /* size in bytes of _one_ MIXERCONTROL */
            private IntPtr pamxctrl;       /* pointer to first MIXERCONTROL array */

            #region Properties

            /// <summary>size in bytes of MIXERLINECONTROLS</summary>
            public UInt32 StructSize
            {
                get { return this.cbStruct; }
                set { this.cbStruct = value; }
            }
            /// <summary>line id (from MIXERLINE.dwLineID)</summary>
            public UInt32 LineID
            {
                get { return this.dwLineID; }
                set { this.dwLineID = value; }
            }
            /// <summary>MIXER_GETLINECONTROLSF_ONEBYID</summary>
            public UInt32 ControlID
            {
                get { return this.dwControlID; }
                set { this.dwControlID = value; }
            }
            /// <summary>MIXER_GETLINECONTROLSF_ONEBYTYPE</summary>
            public MIXERCONTROL_CONTROLTYPE ControlType
            {
                get { return (MIXERCONTROL_CONTROLTYPE)this.dwControlID; }
                set { this.dwControlID = (uint)value; }
            }
            /// <summary>count of controls pmxctrl points to</summary>
            public UInt32 Controls
            {
                get { return this.cControls; }
                set { this.cControls = value; }
            }
            /// <summary>size in bytes of _one_ MIXERCONTROL</summary>
            public UInt32 MixerControlItemSize
            {
                get { return this.cbmxctrl; }
                set { this.cbmxctrl = value; }
            }
            /// <summary>pointer to first MIXERCONTROL array</summary>
            public IntPtr MixerControlArray
            {
                get { return this.pamxctrl; }
                set { this.pamxctrl = value; }
            }

            #endregion
        }

        private const uint MIXERCONTROL_CT_CLASS_MASK = 0xF0000000u;
        private const uint MIXERCONTROL_CT_CLASS_CUSTOM = 0x00000000u;
        private const uint MIXERCONTROL_CT_CLASS_METER = 0x10000000u;
        private const uint MIXERCONTROL_CT_CLASS_SWITCH = 0x20000000u;
        private const uint MIXERCONTROL_CT_CLASS_NUMBER = 0x30000000u;
        private const uint MIXERCONTROL_CT_CLASS_SLIDER = 0x40000000u;
        private const uint MIXERCONTROL_CT_CLASS_FADER = 0x50000000u;
        private const uint MIXERCONTROL_CT_CLASS_TIME = 0x60000000u;
        private const uint MIXERCONTROL_CT_CLASS_LIST = 0x70000000u;

        private const uint MIXERCONTROL_CT_SUBCLASS_MASK = 0x0F000000u;

        private const uint MIXERCONTROL_CT_SC_SWITCH_BOOLEAN = 0x00000000u;
        private const uint MIXERCONTROL_CT_SC_SWITCH_BUTTON = 0x01000000u;

        private const uint MIXERCONTROL_CT_SC_METER_POLLED = 0x00000000u;

        private const uint MIXERCONTROL_CT_SC_TIME_MICROSECS = 0x00000000u;
        private const uint MIXERCONTROL_CT_SC_TIME_MILLISECS = 0x01000000u;

        private const uint MIXERCONTROL_CT_SC_LIST_SINGLE = 0x00000000u;
        private const uint MIXERCONTROL_CT_SC_LIST_MULTIPLE = 0x01000000u;

        private const uint MIXERCONTROL_CT_UNITS_MASK = 0x00FF0000u;
        private const uint MIXERCONTROL_CT_UNITS_CUSTOM = 0x00000000u;
        private const uint MIXERCONTROL_CT_UNITS_BOOLEAN = 0x00010000u;
        private const uint MIXERCONTROL_CT_UNITS_SIGNED = 0x00020000u;
        private const uint MIXERCONTROL_CT_UNITS_UNSIGNED = 0x00030000u;
        private const uint MIXERCONTROL_CT_UNITS_DECIBELS = 0x00040000u; /* in 10ths */
        private const uint MIXERCONTROL_CT_UNITS_PERCENT = 0x00050000u; /* in 10ths */

        /// <summary>Commonly used control types for specifying MIXERCONTROL.dwControlType</summary>
        public enum MIXERCONTROL_CONTROLTYPE : uint
        {
            CUSTOM = (MIXERCONTROL_CT_CLASS_CUSTOM | MIXERCONTROL_CT_UNITS_CUSTOM),
            BOOLEANMETER = (MIXERCONTROL_CT_CLASS_METER | MIXERCONTROL_CT_SC_METER_POLLED | MIXERCONTROL_CT_UNITS_BOOLEAN),
            SIGNEDMETER = (MIXERCONTROL_CT_CLASS_METER | MIXERCONTROL_CT_SC_METER_POLLED | MIXERCONTROL_CT_UNITS_SIGNED),
            PEAKMETER = (SIGNEDMETER + 1),
            UNSIGNEDMETER = (MIXERCONTROL_CT_CLASS_METER | MIXERCONTROL_CT_SC_METER_POLLED | MIXERCONTROL_CT_UNITS_UNSIGNED),
            BOOLEAN = (MIXERCONTROL_CT_CLASS_SWITCH | MIXERCONTROL_CT_SC_SWITCH_BOOLEAN | MIXERCONTROL_CT_UNITS_BOOLEAN),
            ONOFF = (BOOLEAN + 1),
            MUTE = (BOOLEAN + 2),
            MONO = (BOOLEAN + 3),
            LOUDNESS = (BOOLEAN + 4),
            STEREOENH = (BOOLEAN + 5),
            BASS_BOOST = (BOOLEAN + 0x00002277),
            BUTTON = (MIXERCONTROL_CT_CLASS_SWITCH | MIXERCONTROL_CT_SC_SWITCH_BUTTON | MIXERCONTROL_CT_UNITS_BOOLEAN),
            DECIBELS = (MIXERCONTROL_CT_CLASS_NUMBER | MIXERCONTROL_CT_UNITS_DECIBELS),
            SIGNED = (MIXERCONTROL_CT_CLASS_NUMBER | MIXERCONTROL_CT_UNITS_SIGNED),
            UNSIGNED = (MIXERCONTROL_CT_CLASS_NUMBER | MIXERCONTROL_CT_UNITS_UNSIGNED),
            PERCENT = (MIXERCONTROL_CT_CLASS_NUMBER | MIXERCONTROL_CT_UNITS_PERCENT),
            SLIDER = (MIXERCONTROL_CT_CLASS_SLIDER | MIXERCONTROL_CT_UNITS_SIGNED),
            PAN = (SLIDER + 1),
            QSOUNDPAN = (SLIDER + 2),
            FADER = (MIXERCONTROL_CT_CLASS_FADER | MIXERCONTROL_CT_UNITS_UNSIGNED),
            VOLUME = (FADER + 1),
            BASS = (FADER + 2),
            TREBLE = (FADER + 3),
            EQUALIZER = (FADER + 4),
            SINGLESELECT = (MIXERCONTROL_CT_CLASS_LIST | MIXERCONTROL_CT_SC_LIST_SINGLE | MIXERCONTROL_CT_UNITS_BOOLEAN),
            MUX = (SINGLESELECT + 1),
            MULTIPLESELECT = (MIXERCONTROL_CT_CLASS_LIST | MIXERCONTROL_CT_SC_LIST_MULTIPLE | MIXERCONTROL_CT_UNITS_BOOLEAN),
            MIXER = (MULTIPLESELECT + 1),
            MICROTIME = (MIXERCONTROL_CT_CLASS_TIME | MIXERCONTROL_CT_SC_TIME_MICROSECS | MIXERCONTROL_CT_UNITS_UNSIGNED),
            MILLITIME = (MIXERCONTROL_CT_CLASS_TIME | MIXERCONTROL_CT_SC_TIME_MILLISECS | MIXERCONTROL_CT_UNITS_UNSIGNED),
        }

        [Flags]
        public enum MIXERCONTROL_CONTROLF
        {
            None = 0,
            Uniform = 0x00000001,
            Multiple = 0x00000002,
            Disabled = unchecked((int)0x80000000)
        }

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 4)]
        public struct MIXERCONTROL
        {
            private UInt32 cbStruct;
            private UInt32 dwControlID;
            private UInt32 dwControlType;
            private UInt32 fdwControl;
            private UInt32 cMultipleItems;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = (int)MIXER_SHORT_NAME_CHARS)]
            private string szShortName;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = (int)MIXER_LONG_NAME_CHARS)]
            private string szName;
            public MIXERCONTROL_BOUNDS Bounds;
            public MIXERCONTROL_METRICS Metrics;

            #region Properties

            /// <summary>size in bytes of <see cref="MIXERCONTROL">MIXERCONTROL</see></summary>
            public UInt32 StructSize
            {
                get { return this.cbStruct; }
                set { this.cbStruct = value; }
            }
            /// <summary></summary>
            public UInt32 ControlID
            {
                get { return this.dwControlID; }
                set { this.dwControlID = value; }
            }
            /// <summary>MIXERCONTROL_CONTROLTYPE_xxx</summary>
            public MIXERCONTROL_CONTROLTYPE ControlType
            {
                get { return (MIXERCONTROL_CONTROLTYPE)this.dwControlType; }
                set { this.dwControlType = (UInt32)value; }
            }
            /// <summary>MIXERCONTROL_CONTROLF_xxx</summary>
            public MIXERCONTROL_CONTROLF Control
            {
                get { return (MIXERCONTROL_CONTROLF)this.fdwControl; }
                set { this.fdwControl = (uint)value; }
            }
            /// <summary>if MIXERCONTROL_CONTROLF_MULTIPLE set</summary>
            public UInt32 MultipleItems
            {
                get { return this.cMultipleItems; }
                set { this.cMultipleItems = value; }
            }
            /// <summary></summary>
            public String ShortName
            {
                get { return this.szShortName; }
                set { this.szShortName = value; }
            }
            /// <summary></summary>
            public String Name
            {
                get { return this.szName; }
                set { this.szName = value; }
            }

            #endregion
        }

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 4)]
        public struct MIXERCONTROL_BOUNDS
        {
            private Int32 lMinimum;
            private Int32 lMaximum;
            //public uint dwMinimum; // Unioned with lMinimum
            //public uint dwMaximum; // Unioned with lMaximum
            //public uint dwReserved1; // Unioned with lMinimum
            //public uint dwReserved2; // Unioned with lMaximum
            private UInt32 dwReserved3;
            private UInt32 dwReserved4;
            private UInt32 dwReserved5;
            private UInt32 dwReserved6;

            #region Properties

            /// <summary>signed minimum for this control</summary>
            public Int32 Minimum
            {
                get { return this.lMinimum; }
                set { this.lMinimum = value; }
            }
            /// <summary>signed maximum for this control</summary>
            public Int32 Maximum
            {
                get { return this.lMaximum; }
                set { this.lMaximum = value; }
            }
            /// <summary>unsigned minimum for this control</summary>
            public UInt32 UnsignedMinimum
            {
                get { return (uint)this.lMinimum; }//TODO: something different
                set { this.lMinimum = (int)value; }
            }
            /// <summary>unsigned maximum for this control</summary>
            public UInt32 UnsignedMaximum
            {
                get { return (uint)this.lMaximum; }//TODO: something different
                set { this.lMaximum = (int)value; }
            }
            /// <summary></summary>
            public UInt32 Reserved1
            {
                get { return (uint)this.lMinimum; }//TODO: something different
                set { this.lMinimum = (int)value; }
            }
            /// <summary></summary>
            public UInt32 Reserved2
            {
                get { return (uint)this.lMaximum; }//TODO: something different
                set { this.lMaximum = (int)value; }
            }
            /// <summary></summary>
            public UInt32 Reserved3
            {
                get { return this.dwReserved3; }
                set { this.dwReserved3 = value; }
            }
            /// <summary></summary>
            public UInt32 Reserved4
            {
                get { return this.dwReserved4; }
                set { this.dwReserved4 = value; }
            }
            /// <summary></summary>
            public UInt32 Reserved5
            {
                get { return this.dwReserved5; }
                set { this.dwReserved5 = value; }
            }
            /// <summary></summary>
            public UInt32 Reserved6
            {
                get { return this.dwReserved6; }
                set { this.dwReserved6 = value; }
            }

            #endregion
        }

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 4)]
        public struct MIXERCONTROL_METRICS
        {
            private UInt32 cSteps;
            //private UInt32 cbCustomData; //Unioned with cSteps
            //private UInt32 dwReserved1; //Unioned with cSteps
            private UInt32 dwReserved2;
            private UInt32 dwReserved3;
            private UInt32 dwReserved4;
            private UInt32 dwReserved5;
            private UInt32 dwReserved6;

            #region Properties

            /// <summary># of steps between min & max</summary>
            public UInt32 Steps
            {
                get { return this.cSteps; }
                set { this.cSteps = value; }
            }
            /// <summary>size in bytes of custom data</summary>
            public UInt32 CustomData
            {
                get { return this.cSteps; }
                set { this.cSteps = value; }
            }
            /// <summary>!!! needed? we have cbStruct....</summary>
            public UInt32 Reserved1
            {
                get { return this.cSteps; }//TODO: something different
                set { this.cSteps = value; }
            }
            /// <summary>!!! needed? we have cbStruct....</summary>
            public UInt32 Reserved2
            {
                get { return this.dwReserved2; }
                set { this.dwReserved2 = value; }
            }
            /// <summary>!!! needed? we have cbStruct....</summary>
            public UInt32 Reserved3
            {
                get { return this.dwReserved3; }
                set { this.dwReserved3 = value; }
            }
            /// <summary>!!! needed? we have cbStruct....</summary>
            public UInt32 Reserved4
            {
                get { return this.dwReserved4; }
                set { this.dwReserved4 = value; }
            }
            /// <summary>!!! needed? we have cbStruct....</summary>
            public UInt32 Reserved5
            {
                get { return this.dwReserved5; }
                set { this.dwReserved5 = value; }
            }
            /// <summary>!!! needed? we have cbStruct....</summary>
            public UInt32 Reserved6
            {
                get { return this.dwReserved6; }
                set { this.dwReserved6 = value; }
            }

            #endregion
        }

        public enum MIXER_GETLINECONTROLSF : uint
        {
            ALL = 0x00000000u,
            ONEBYID = 0x00000001u,
            ONEBYTYPE = 0x00000002u,

            QUERYMASK = 0x0000000Fu,
        }
        [DllImport("winmm.dll")]
        static extern uint mixerGetLineInfo(IntPtr hmxobj,
           ref MIXERLINE pmxl, uint fdwInfo);

        uint mixerGetLineInfo(IntPtr hmxobj,
           ref MIXERLINE pmxl,MIXER_OBJECTF objF , MIXER_GETLINEINFOF lineF )
        {
            uint flag = (uint)objF | (uint)lineF;
            return mixerGetLineInfo(hmxobj, ref pmxl, flag);
        }

        #endregion

        #region mixerGetLineControls
        [DllImport("winmm.dll")]
        static extern uint mixerGetLineControls(IntPtr hmxobj, 
            ref MIXERLINECONTROLS pmxlc, uint fdwControls);

        uint mixerGetLineControls(IntPtr hmxobj,
            ref MIXERLINECONTROLS pmxlc, MIXER_OBJECTF objFlag, MIXER_GETLINECONTROLSF ctlFlag)
        {
            uint flag = (uint)objFlag | (uint)ctlFlag;
            return mixerGetLineControls(hmxobj, ref pmxlc, flag);
        }


        #endregion 

        #region mixerClose
        [DllImport("winmm.dll")]
            static extern uint mixerClose(IntPtr hmx);
        #endregion

        uint _volumeID;
        uint _muteID;
        uint _volMax;
        uint _volMin;
        IntPtr _hMixer = IntPtr.Zero;

        public class MixerException : Exception
        {
            public MixerException(string msg, uint err)
                :base( string.Format("MixerException msg:{0},code:{1}",msg,err))
            {
                Debug.WriteLine("MixerException:"+Message);
            }
        }

        public MixerAPI()
        {
            _hMixer = new IntPtr();
            uint mmresult = mixerOpen(ref _hMixer, 0, IntPtr.Zero, IntPtr.Zero, MIXER_OBJECTF.MIXER);
            if (mmresult != (uint)MMRESULT.MMSYSERR_NOERROR)
            {
                throw new MixerException("mixerOpen:",mmresult);
            }

            MIXERLINE lineInfo = new MIXERLINE();
            lineInfo.StructSize = (uint)Marshal.SizeOf(new MIXERLINE());
            lineInfo.ComponentType =(uint)MIXERLINE_COMPONENTTYPE.SRC_WAVEOUT;
            //volInfo.ComponentType =(uint)MIXERLINE_COMPONENTTYPE.DST_SPEAKERS;
            mmresult = mixerGetLineInfo(_hMixer, ref lineInfo, MIXER_OBJECTF.HMIXER 
                ,MIXER_GETLINEINFOF.COMPONENTTYPE);

            IntPtr ctlPtr = Marshal.AllocCoTaskMem(Marshal.SizeOf(new MIXERCONTROL()));

            MIXERLINECONTROLS ctlParam = new MIXERLINECONTROLS();
            ctlParam.StructSize = (uint)Marshal.SizeOf(new MIXERLINECONTROLS());
            ctlParam.LineID = lineInfo.dwLineID;
            ctlParam.ControlType = MIXERCONTROL_CONTROLTYPE.MUTE;
            ctlParam.Controls = 1;
            ctlParam.MixerControlItemSize=  (uint)Marshal.SizeOf(new MIXERCONTROL());
            ctlParam.MixerControlArray = ctlPtr;

            mmresult = mixerGetLineControls(_hMixer, ref ctlParam,MIXER_OBJECTF.HMIXER
                ,MIXER_GETLINECONTROLSF.ONEBYTYPE);
            if (mmresult != (uint)MMRESULT.MMSYSERR_NOERROR)
            {
                Marshal.FreeCoTaskMem(ctlPtr);
                throw new MixerException("mixerGetLineControls", mmresult);
            }

            MIXERCONTROL muteCtrl = (MIXERCONTROL)Marshal.PtrToStructure(ctlPtr, typeof(MIXERCONTROL));

            _muteID = muteCtrl.ControlID;

            ctlParam.ControlType = MIXERCONTROL_CONTROLTYPE.VOLUME;
            //他の値は再利用。まだMarshal.FreeCoTaskMemを呼んではいけない

            mmresult = mixerGetLineControls(_hMixer, ref ctlParam,MIXER_OBJECTF.HMIXER
                ,MIXER_GETLINECONTROLSF.ONEBYTYPE);
            if (mmresult != (uint)MMRESULT.MMSYSERR_NOERROR)
            {
                Marshal.FreeCoTaskMem(ctlPtr);
                throw new MixerException("mixerGetLineControls" , mmresult);
            }

            MIXERCONTROL volCtrl = (MIXERCONTROL)Marshal.PtrToStructure(ctlPtr, typeof(MIXERCONTROL));
            Marshal.FreeCoTaskMem(ctlPtr);

            _volumeID = volCtrl.ControlID;
            _volMax = volCtrl.Bounds.UnsignedMaximum;
            _volMin = volCtrl.Bounds.UnsignedMinimum;

            Debug.WriteLine(string.Format("MixerAPI constructor muteID:{0} volID:{1} volMan:{2} volMin:{3}",
                _muteID, _volumeID, _volMax, _volMin));


        }

        /// <summary>
        /// IDisposeの実装
        /// </summary>
        public void Dispose()
        {
            if (_hMixer != IntPtr.Zero)
            {
                Debug.WriteLine("MixerObject Dispose");
                mixerClose(_hMixer);
                _hMixer = IntPtr.Zero;
            }
        }

        #region mixerGetControlDetails

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 4)]
        public struct MIXERCONTROLDETAILS
        {
             private UInt32 cbStruct;
             private UInt32 dwControlID;
             private UInt32 cChannels;
             private IntPtr hwndOwner;
        //     private UInt32 cMultipleItems; //Unioned with hwndOwner /* if _MULTIPLE, the number of items per channel */
             private UInt32 cbDetails;
             private IntPtr paDetails;

                                                                                                                                                                                                                                                                                                                                                                     #region Properties

     /// <summary>size in bytes of MIXERCONTROLDETAILS</summary>
     public UInt32 StructSize
     {
        get { return this.cbStruct; }
        set { this.cbStruct = value; }
     }
     /// <summary>control id to get/set details on</summary>
     public UInt32 ControlID
     {
        get { return this.dwControlID; }
        set { this.dwControlID = value; }
     }
     /// <summary>number of channels in paDetails array</summary>
     public UInt32 Channels
     {
        get { return this.cChannels; }
        set { this.cChannels = value; }
     }
     /// <summary>for MIXER_SETCONTROLDETAILSF_CUSTOM</summary>
     public IntPtr OwnerHandle
     {
        get { return this.hwndOwner; }
        set { this.hwndOwner = value; }
     }
     /// <summary>if _MULTIPLE, the number of items per channel</summary>
     public UInt32 MultipleItems
     {
        get { return (UInt32)this.hwndOwner; }
        set { this.hwndOwner = (IntPtr)value; }
     }
     /// <summary>size of _one_ details_XX struct</summary>
     public UInt32 DetailsItemSize
     {
        get { return this.cbDetails; }
        set { this.cbDetails = value; }
     }
     /// <summary>pointer to array of details_XX structs</summary>
     public IntPtr DetailsPointer
     {
        get { return this.paDetails; }
        set { this.paDetails = value; }
     }

     #endregion
        }
        [DllImport("winmm.dll", CharSet = CharSet.Unicode)]
        static extern uint mixerGetControlDetails(IntPtr hmxobj, ref MIXERCONTROLDETAILS pmxcd, uint fdwDetails);

        static uint mixerGetControlDetails(IntPtr hmxobj, ref MIXERCONTROLDETAILS pmxcd, MIXER_OBJECTF fdwDetails, MIXER_GETCONTROLDETAILSF getControlDetails)
        {
            uint flags = ((uint)fdwDetails | (uint)getControlDetails);
            return mixerGetControlDetails(hmxobj, ref pmxcd, flags);
        }

        public enum MIXER_GETCONTROLDETAILSF : uint
        {
            VALUE = 0x00000000u,
            LISTTEXT = 0x00000001u,

            QUERYMASK = 0x0000000Fu,
        }

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 4)]
        public struct MIXERCONTROLDETAILS_UNSIGNED
        {
            private UInt32 dwValue;

            #region Properties

            public UInt32 Value
            {
                get { return this.dwValue; }
                set { this.dwValue = value; }
            }

            #endregion
        }

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode, Pack = 2)]
        public struct MIXERCONTROLDETAILS_BOOLEAN
        {
            private int fValue;

            public int Value
            {
                get { return fValue; }
                set { fValue = value; }
            }
        }

        #endregion

        /// <summary>
        /// 音量の設定取得(0-100)
        /// </summary>
        public uint Volume { get { return GetCurrentVolume(); } set { SetVolume(value); } }

        uint GetCurrentVolume()
        {
            MIXERCONTROLDETAILS volCtl = new MIXERCONTROLDETAILS();
            IntPtr volPtr = Marshal.AllocCoTaskMem(Marshal.SizeOf(new MIXERCONTROLDETAILS_UNSIGNED()));
            volCtl.StructSize = (uint)Marshal.SizeOf(new MIXERCONTROLDETAILS());
            volCtl.ControlID = _volumeID;
            volCtl.Channels = 1;
            volCtl.MultipleItems = 0;
            volCtl.DetailsItemSize = (uint)Marshal.SizeOf(new MIXERCONTROLDETAILS_UNSIGNED());
            volCtl.DetailsPointer = volPtr;

            uint mmresult = mixerGetControlDetails(_hMixer, ref volCtl,
                MIXER_OBJECTF.HMIXER , MIXER_GETCONTROLDETAILSF.VALUE);
            if (mmresult != (uint)MMRESULT.MMSYSERR_NOERROR)
            {
                Marshal.FreeCoTaskMem(volPtr);
                throw new MixerException("mixerGetControlDetails" , mmresult);
            }

            MIXERCONTROLDETAILS_UNSIGNED volInfo
                = (MIXERCONTROLDETAILS_UNSIGNED)Marshal.PtrToStructure(volPtr,
                typeof(MIXERCONTROLDETAILS_UNSIGNED));
            Marshal.FreeCoTaskMem(volPtr);

            double ret = ((double)(volInfo.Value - _volMin) * 100 + (double)(_volMax - _volMin) / 2)
                                                        / (double)(_volMax - _volMin);

            Debug.WriteLine(string.Format("MixerAPI.GetCurrentVolume vol:{0} volMax:{1} volMin:{2} ret:{3}",
                volInfo.Value,_volMax,_volMin,ret));

            return (uint)ret;
        }

        #region mixerSetControlDetails
        [DllImport("winmm.dll")]
        static extern uint mixerSetControlDetails(IntPtr hmxobj,
           ref MIXERCONTROLDETAILS pmxcd, UInt32 fdwDetails);

        static uint mixerSetControlDetails(IntPtr hmxobj, ref MIXERCONTROLDETAILS pmxcd, MIXER_OBJECTF fdwDetails, MIXER_GETCONTROLDETAILSF getControlDetails)
        {
            uint flags = ((uint)fdwDetails | (uint)getControlDetails);
            return mixerSetControlDetails(hmxobj, ref pmxcd, flags);
        }

        #endregion

        void SetVolume(uint value)
        {
            MIXERCONTROLDETAILS volCtl = new MIXERCONTROLDETAILS();
            MIXERCONTROLDETAILS_UNSIGNED volDat = new MIXERCONTROLDETAILS_UNSIGNED();
            double volValue = (double)value * (double)(_volMax - _volMin) / 100D + _volMin;
            volDat.Value = (uint)volValue;

            Debug.WriteLine(string.Format("MixerAPI.SetCurrentVolume vol:{0} volMax:{1} volMin:{2} param:{3}",
                value, _volMax, _volMin, volDat.Value));


            IntPtr volPtr = Marshal.AllocCoTaskMem(Marshal.SizeOf(new MIXERCONTROLDETAILS_UNSIGNED()));
            Marshal.StructureToPtr(volDat, volPtr, false);

            volCtl.StructSize = (uint)Marshal.SizeOf(new MIXERCONTROLDETAILS());
            volCtl.ControlID = _volumeID;
            volCtl.Channels = 1;
            volCtl.MultipleItems = 0;
            volCtl.DetailsItemSize = (uint)Marshal.SizeOf(new MIXERCONTROLDETAILS_UNSIGNED());
            volCtl.DetailsPointer = volPtr;

            uint mmresult = mixerSetControlDetails(_hMixer, ref volCtl,
                MIXER_OBJECTF.HMIXER, MIXER_GETCONTROLDETAILSF.VALUE);
            Marshal.FreeCoTaskMem(volPtr);

            if (mmresult != (uint)MMRESULT.MMSYSERR_NOERROR)
            {
                throw new MixerException("mixerSetControlDetails" , mmresult);
            }


        }

        /// <summary>
        /// ミュート(true/false)
        /// </summary>
        public bool Mute { get { return IsMute(); } set { SetMute(value); } }

        bool IsMute()
        {
            MIXERCONTROLDETAILS muteCtl = new MIXERCONTROLDETAILS();
            IntPtr mutePtr = Marshal.AllocCoTaskMem(Marshal.SizeOf(new MIXERCONTROLDETAILS_BOOLEAN()));
            muteCtl.StructSize = (uint)Marshal.SizeOf(new MIXERCONTROLDETAILS());
            muteCtl.ControlID = _muteID;
            muteCtl.Channels = 1;
            muteCtl.MultipleItems = 0;
            muteCtl.DetailsItemSize = (uint)Marshal.SizeOf(new MIXERCONTROLDETAILS_BOOLEAN());
            muteCtl.DetailsPointer = mutePtr;

            uint mmresult = mixerGetControlDetails(_hMixer, ref muteCtl,
                MIXER_OBJECTF.HMIXER, MIXER_GETCONTROLDETAILSF.VALUE);
            if (mmresult != (uint)MMRESULT.MMSYSERR_NOERROR)
            {
                Marshal.FreeCoTaskMem(mutePtr);
                throw new MixerException("mixerGetControlDetails" , mmresult);
            }

            MIXERCONTROLDETAILS_BOOLEAN muteInfo
                = (MIXERCONTROLDETAILS_BOOLEAN)Marshal.PtrToStructure(mutePtr,
                typeof(MIXERCONTROLDETAILS_BOOLEAN));
            Marshal.FreeCoTaskMem(mutePtr);

            Debug.WriteLine("MixerAPI.IsMuteret:" + muteInfo.Value.ToString());

            return muteInfo.Value > 0;

        }

        void SetMute(bool flag)
        {
            MIXERCONTROLDETAILS muteCtl = new MIXERCONTROLDETAILS();
            MIXERCONTROLDETAILS_BOOLEAN muteVal = new MIXERCONTROLDETAILS_BOOLEAN();
            muteVal.Value = flag ? 1 : 0;

            Debug.WriteLine("MixerAPI.SetMute:" + flag.ToString());

            IntPtr mutePtr = Marshal.AllocCoTaskMem(Marshal.SizeOf(new MIXERCONTROLDETAILS_BOOLEAN()));
            Marshal.StructureToPtr(muteVal, mutePtr, false);

            muteCtl.StructSize = (uint)Marshal.SizeOf(new MIXERCONTROLDETAILS());
            muteCtl.ControlID = _muteID;
            muteCtl.Channels = 1;
            muteCtl.MultipleItems = 0;
            muteCtl.DetailsItemSize = (uint)Marshal.SizeOf(new MIXERCONTROLDETAILS_BOOLEAN());
            muteCtl.DetailsPointer = mutePtr;

            uint mmresult = mixerSetControlDetails(_hMixer, ref muteCtl,
                MIXER_OBJECTF.HMIXER, MIXER_GETCONTROLDETAILSF.VALUE);
            Marshal.FreeCoTaskMem(mutePtr);

            if (mmresult != (uint)MMRESULT.MMSYSERR_NOERROR)
            {
                throw new Exception("mixerSetControlDetails" + mmresult.ToString());
            }
        }


    }
}
