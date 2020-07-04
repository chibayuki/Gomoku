/* * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * *
Copyright © 2020 chibayuki@foxmail.com

五子棋 (Gomoku)
Version 7.1.17000.7840.R19.200704-1620

This file is part of "五子棋" (Gomoku)

"五子棋" (Gomoku) is released under the GPLv3 license
* * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using System.Diagnostics;
using System.Drawing.Drawing2D;
using System.Drawing.Text;
using System.IO;
using System.Text.RegularExpressions;

namespace WinFormApp
{
    public partial class Form_Main : Form
    {
        #region 版本信息

        private static readonly string ApplicationName = Application.ProductName; // 程序名。
        private static readonly string ApplicationEdition = "7.1.19"; // 程序版本。

        private static readonly Int32 MajorVersion = new Version(Application.ProductVersion).Major; // 主版本。
        private static readonly Int32 MinorVersion = new Version(Application.ProductVersion).Minor; // 副版本。
        private static readonly Int32 BuildNumber = new Version(Application.ProductVersion).Build; // 版本号。
        private static readonly Int32 BuildRevision = new Version(Application.ProductVersion).Revision; // 修订版本。
        private static readonly string LabString = "R19"; // 分支名。
        private static readonly string BuildTime = "200704-1620"; // 编译时间。

        //

        private static readonly string RootDir_Product = Environment.SystemDirectory.Substring(0, 1) + @":\ProgramData\AppConfig\gomoku"; // 根目录：此产品。
        private static readonly string RootDir_CurrentVersion = RootDir_Product + "\\" + BuildNumber + "." + BuildRevision; // 根目录：当前版本。

        private static readonly string ConfigFileDir = RootDir_CurrentVersion + @"\Config"; // 配置文件所在目录。
        private static readonly string ConfigFilePath = ConfigFileDir + @"\settings.cfg"; // 配置文件路径。

        private static readonly string LogFileDir = RootDir_CurrentVersion + @"\Log"; // 存档文件所在目录。
        private static readonly string DataFilePath = LogFileDir + @"\userdata.cfg"; // 用户数据文件路径（包含最佳成绩与游戏时长）。
        private static readonly string RecordFilePath = LogFileDir + @"\lastgame.cfg"; // 上次游戏文件路径（包含最后一次游戏记录）。

        //

        private static readonly List<Version> OldVersionList = new List<Version> // 兼容的版本列表，用于从最新的兼容版本迁移配置设置。
        {
            new Version(7, 1, 17000, 0),
            new Version(7, 1, 17000, 149),
            new Version(7, 1, 17000, 696),
            new Version(7, 1, 17000, 730),
            new Version(7, 1, 17000, 903),
            new Version(7, 1, 17000, 1416),
            new Version(7, 1, 17000, 1690),
            new Version(7, 1, 17000, 4087),
            new Version(7, 1, 17000, 4867),
            new Version(7, 1, 17000, 5136),
            new Version(7, 1, 17000, 5378),
            new Version(7, 1, 17000, 5417),
            new Version(7, 1, 17000, 5443),
            new Version(7, 1, 17000, 5602),
            new Version(7, 1, 17000, 7785),
            new Version(7, 1, 17000, 7813),
            new Version(7, 1, 17000, 7822)
        };

        //

        private static readonly string URL_GitHub_Base = @"https://github.com/chibayuki/Gomoku"; // 此项目在 GitHub 的 URL。
        private static readonly string URL_GitHub_Release = URL_GitHub_Base + @"/releases/latest"; // 此项目的最新发布版本在 GitHub 的 URL。

        #endregion

        #region 配置设置变量

        private Int32 ElementSize = 40; // 元素边长。

        //

        private enum ChessboardTypes { NULL = -1, Gomoku_15x15, Go_19x19, COUNT } // 棋盘类型枚举。
        private ChessboardTypes ChessboardType = ChessboardTypes.Gomoku_15x15; // 当前棋盘类型。

        private enum ChessboardStyles { NULL = -1, Intersection, Checker, COUNT } // 棋盘样式枚举。
        private ChessboardStyles ChessboardStyle = ChessboardStyles.Intersection; // 当前棋盘样式。

        //

        private bool UseBalanceBreaker = true; // 是否使用禁手规则。
        private bool ShowBanPoint = false; // 当使用禁手规则时，是否标在棋盘上标注黑方禁手点。
        private bool SaveRecord => (UseBalanceBreaker && !ShowBanPoint); // 是否保存记录。

        //

        private const Int32 StepTagNum_MIN = 0; // 最小落子序号数量。
        private Int32 StepTagNum_MAX => Range.Width * Range.Height; // 最大落子序号数量。
        private Int32 StepTagNum = 2; // 当前落子序号数量。正整数表示在最后多少手落子处显示序号；0 表示不显示序号。

        //

        private const Com.WinForm.Theme Theme_DEFAULT = Com.WinForm.Theme.Colorful; // 主题的默认值。

        private bool UseRandomThemeColor = true; // 是否使用随机的主题颜色。

        private static readonly Color ThemeColor_DEFAULT = Color.Gray; // 主题颜色的默认值。

        private const bool ShowFormTitleColor_DEFAULT = true; // 是否显示窗体标题栏的颜色的默认值。

        private const double Opacity_MIN = 0.05; // 总体不透明度的最小值。
        private const double Opacity_MAX = 1.0; // 总体不透明度的最大值。

        //

        private bool AntiAlias = true; // 是否使用抗锯齿模式绘图。

        #endregion

        #region 元素矩阵变量

        private const Int32 CAPACITY = 19; // 元素矩阵容量的平方根。

        private Size Range // 当前界面布局（以元素数为单位）。
        {
            get
            {
                switch (ChessboardType)
                {
                    case ChessboardTypes.Gomoku_15x15: return new Size(15, 15);
                    case ChessboardTypes.Go_19x19: return new Size(19, 19);
                    default: return new Size(15, 15);
                }
            }
        }

        private Int32[,] ElementMatrix = new Int32[CAPACITY, CAPACITY]; // 元素矩阵。

        private List<Point> ElementIndexList = new List<Point>(CAPACITY * CAPACITY); // 元素索引列表。

        #endregion

        #region 棋局变量

        private Point Center // 天元坐标。
        {
            get
            {
                switch (ChessboardType)
                {
                    case ChessboardTypes.Gomoku_15x15: return new Point(7, 7);
                    case ChessboardTypes.Go_19x19: return new Point(9, 9);
                    default: return new Point(7, 7);
                }
            }
        }

        private Point[] Star // 星位坐标。
        {
            get
            {
                switch (ChessboardType)
                {
                    case ChessboardTypes.Gomoku_15x15: return new Point[] { new Point(3, 3), new Point(3, 11), new Point(7, 7), new Point(11, 3), new Point(11, 11) };
                    case ChessboardTypes.Go_19x19: return new Point[] { new Point(3, 3), new Point(3, 9), new Point(3, 15), new Point(9, 3), new Point(9, 9), new Point(9, 15), new Point(15, 3), new Point(15, 9), new Point(15, 15) };
                    default: return new Point[] { new Point(3, 3), new Point(3, 11), new Point(7, 7), new Point(11, 3), new Point(11, 11) };
                }
            }
        }

        private const Int32 BLACK = 1; // 黑棋的元素值。
        private const Int32 WHITE = 2; // 白棋的元素值。

        //

        private bool AIFirst = true; // 是否由 AI 执黑先行。

        private Int32 ChessColor_AI => (AIFirst ? BLACK : WHITE);  // AI 执棋棋色。
        private Int32 ChessColor_User => (AIFirst ? WHITE : BLACK); // 用户执棋棋色。

        //

        private static readonly TimeSpan TotalFreeTime = TimeSpan.FromHours(2); // 对局每方初始自由时长。
        private static readonly TimeSpan TotalCountdownTime = TimeSpan.FromMinutes(3); // 对局每方初始读秒时长。

        private TimeSpan FreeTimeRemaining_Black = TimeSpan.Zero; // 黑方剩余自由时长。
        private TimeSpan CountdownTimeRemaining_Black = TimeSpan.Zero;  // 黑方剩余读秒时长。

        private TimeSpan FreeTimeRemaining_White = TimeSpan.Zero;  // 白方剩余自由时长。
        private TimeSpan CountdownTimeRemaining_White = TimeSpan.Zero;  // 白方剩余读秒时长。

        private TimeSpan FreeTimeRemaining_AI => (AIFirst ? FreeTimeRemaining_Black : FreeTimeRemaining_White); // AI 剩余自由时长。
        private TimeSpan CountdownTimeRemaining_AI => (AIFirst ? CountdownTimeRemaining_Black : CountdownTimeRemaining_White); // AI 剩余读秒时长。

        private TimeSpan FreeTimeRemaining_User => (AIFirst ? FreeTimeRemaining_White : FreeTimeRemaining_Black); // 用户剩余自由时长。
        private TimeSpan CountdownTimeRemaining_User => (AIFirst ? CountdownTimeRemaining_White : CountdownTimeRemaining_Black); // 用户剩余读秒时长。

        private TimeSpan FreeTimeRemaining_Current => (CurrentColor == Roles.Black ? FreeTimeRemaining_Black : FreeTimeRemaining_White); // 当前行棋方剩余自由时长。
        private TimeSpan CountdownTimeRemaining_Current => (CurrentColor == Roles.Black ? CountdownTimeRemaining_Black : CountdownTimeRemaining_White); // 当前行棋方剩余读秒时长。

        //

        private enum GameStates { NULL = -1, BlackWin, WhiteWin, Draw, Ongoing, COUNT } // 棋局状态枚举。
        private GameStates GameState = GameStates.Ongoing; // 当前棋局状态。

        private List<Point> JdgBasisIDList = new List<Point>(CAPACITY * CAPACITY); // 表示作为棋局状态判定依据的索引列表。【注意】仅当终局时此列表不为空。

        private List<Point> BanIDList = new List<Point>(CAPACITY * CAPACITY); // 黑方禁手点索引列表。【注意】仅当正在使用相关功能时此列表不为空。

        //

        private enum Roles { NULL = -1, AI, User, Black, White, Both, COUNT }; // 角色枚举。

        private Roles CurrentPlayer => (AIFirst == (ElementIndexList.Count % 2 == 0) ? Roles.AI : Roles.User); // 当前行棋方棋手。
        private Roles CurrentColor => (ElementIndexList.Count % 2 == 0 ? Roles.Black : Roles.White); // 当前行棋方棋色。

        private Roles WinningPlayer // 当前胜方棋手。
        {
            get
            {
                switch (GameState)
                {
                    case GameStates.BlackWin: return (AIFirst ? Roles.AI : Roles.User);
                    case GameStates.WhiteWin: return (AIFirst ? Roles.User : Roles.AI);
                    case GameStates.Draw: return Roles.Both;
                    default: return Roles.NULL;
                }
            }
        }
        private Roles WinningColor // 当前胜方棋色。
        {
            get
            {
                switch (GameState)
                {
                    case GameStates.BlackWin: return Roles.Black;
                    case GameStates.WhiteWin: return Roles.White;
                    case GameStates.Draw: return Roles.Both;
                    default: return Roles.NULL;
                }
            }
        }

        #endregion

        #region 游戏变量

        private static readonly Size FormClientInitialSize = new Size(585, 420); // 窗体工作区初始大小。

        //

        private Color GameUIBackColor_DEC => Me.RecommendColors.Background_DEC.ToColor(); // 游戏 UI 背景颜色（浅色）。
        private Color GameUIBackColor_INC => Me.RecommendColors.Background_INC.ToColor(); // 游戏 UI 背景颜色（深色）。

        private Color ChessboardBackColor => Me.ThemeColor.AtLightness_HSL(84).ToColor(); // 棋盘背景颜色。
        private Color ChessboardLineColor => Me.ThemeColor.AtLightness_HSL(48).ToColor(); // 棋盘阳线颜色。
        private Color ChessboardStarColor => Me.ThemeColor.AtLightness_HSL(24).ToColor(); // 棋盘星位颜色。

        private static class GlobalColors // 全局固定颜色。
        {
            public static readonly Color BlackChess = Color.FromArgb(16, 16, 16); // 黑棋颜色。
            public static readonly Color WhiteChess = Color.FromArgb(240, 240, 240); // 白棋颜色。

            public static readonly Color Win = Color.FromArgb(235, 71, 99); // 胜方颜色。
            public static readonly Color Lose = Color.FromArgb(36, 143, 89); // 负方颜色。
            public static readonly Color Draw = Color.FromArgb(242, 147, 13); // 和局颜色。

            public static readonly Color Ban_Self = Color.FromArgb(235, 71, 99); // 己方禁手点颜色。
            public static readonly Color Ban_Opp = Color.FromArgb(36, 143, 89); // 对方禁手点颜色。
        }

        //

        private static readonly string AIName = "电脑"; // AI 名。
        private static readonly string UserName = Environment.UserName; // 用户名。

        //

        private TimeSpan ThisGameTime = TimeSpan.Zero; // 本次游戏时长。
        private TimeSpan TotalGameTime = TimeSpan.Zero; // 累计游戏时长。

        //

        private Int32 UserWinGames = 0; // 用户胜局数。
        private Int32 UserLoseGames = 0; // 用户负局数。
        private Int32 DrawGames = 0; // 和局数。
        private Int32 TotalGames => (UserWinGames + UserLoseGames + DrawGames); // 总局数。

        //

        private ChessboardTypes ChessboardType_Last = ChessboardTypes.Gomoku_15x15; // 上次游戏使用的棋盘大小。

        private bool UseBalanceBreaker_Last = true; // 上次游戏是否使用禁手规则。
        private bool ShowBanPoint_Last = false; // 当上次游戏使用禁手规则时，是否标在棋盘上标注黑方禁手点。
        private bool SaveRecord_Last => (UseBalanceBreaker_Last && !ShowBanPoint_Last); // 上次游戏是否保存记录。

        private bool AIFirst_Last = true; // 上次游戏是否由 AI 执黑先行。

        //

        private TimeSpan FreeTimeRemaining_Black_Last = TimeSpan.Zero; // 上次游戏的黑方剩余时长。
        private TimeSpan CountdownTimeRemaining_Black_Last = TimeSpan.Zero; // 上次游戏的黑方剩余时长。

        private TimeSpan FreeTimeRemaining_White_Last = TimeSpan.Zero; // 上次游戏的白方剩余时长。
        private TimeSpan CountdownTimeRemaining_White_Last = TimeSpan.Zero; // 上次游戏的白方剩余时长。

        private Int32[,] ElementMatrix_Last = new Int32[CAPACITY, CAPACITY]; // 上次游戏的元素矩阵。

        private List<Point> ElementIndexList_Last = new List<Point>(CAPACITY * CAPACITY); // 上次游戏的元素索引列表。

        #endregion

        #region 计时器数据

        private struct CycData // 计时周期数据。
        {
            private Int32 _Tick0;
            private Int32 _Tick1;

            public double DeltaMS // 当前周期的毫秒数。
            {
                get
                {
                    return Math.Abs(_Tick0 - _Tick1);
                }
            }

            private Int32 _Cnt; // 周期计数。
            public Int32 Cnt
            {
                get
                {
                    return _Cnt;
                }
            }

            private double _Avg_Am; // 周期毫秒数的算数平均值。
            public double Avg_Am
            {
                get
                {
                    return _Avg_Am;
                }
            }

            private double _Avg_St; // 周期毫秒数的统计平均值。
            public double Avg_St
            {
                get
                {
                    return _Avg_St;
                }
            }

            public void Reset() // 重置此结构。
            {
                _Tick0 = _Tick1 = Environment.TickCount;
                _Cnt = 0;
                _Avg_Am = _Avg_St = 0;
            }

            public void Update() // 更新此结构。
            {
                if (_Tick0 <= _Tick1)
                {
                    _Tick0 = Environment.TickCount;
                }
                else
                {
                    _Tick1 = Environment.TickCount;
                }

                _Cnt = Math.Min(1048576, _Cnt + 1);

                _Avg_Am = (_Avg_Am + DeltaMS) / 2;
                _Avg_St = _Avg_St * (_Cnt - 1) / _Cnt + DeltaMS / _Cnt;
            }
        }

        #endregion

        #region 窗体构造

        private Com.WinForm.FormManager Me;

        public Com.WinForm.FormManager FormManager
        {
            get
            {
                return Me;
            }
        }

        private void _Ctor(Com.WinForm.FormManager owner)
        {
            InitializeComponent();

            //

            if (owner != null)
            {
                Me = new Com.WinForm.FormManager(this, owner);
            }
            else
            {
                Me = new Com.WinForm.FormManager(this);
            }

            //

            FormDefine();
        }

        public Form_Main()
        {
            _Ctor(null);
        }

        public Form_Main(Com.WinForm.FormManager owner)
        {
            _Ctor(owner);
        }

        private void FormDefine()
        {
            Me.Caption = ApplicationName;
            Me.FormStyle = Com.WinForm.FormStyle.Sizable;
            Me.EnableFullScreen = true;
            Me.ClientSize = FormClientInitialSize;
            Me.Theme = Theme_DEFAULT;
            Me.ThemeColor = new Com.ColorX(ThemeColor_DEFAULT);
            Me.ShowCaptionBarColor = ShowFormTitleColor_DEFAULT;

            Me.Loading += LoadingEvents;
            Me.Loaded += LoadedEvents;
            Me.Closed += ClosedEvents;
            Me.Resize += ResizeEvents;
            Me.SizeChanged += SizeChangedEvents;
            Me.ThemeChanged += ThemeColorChangedEvents;
            Me.ThemeColorChanged += ThemeColorChangedEvents;
        }

        #endregion

        #region 窗体事件

        private void LoadingEvents(object sender, EventArgs e)
        {
            //
            // 在窗体加载时发生。
            //

            TransConfig();

            DelOldConfig();

            LoadConfig();

            LoadUserData();

            LoadLastGame();

            //

            if (UseRandomThemeColor)
            {
                Me.ThemeColor = Com.ColorManipulation.GetRandomColorX();
            }
        }

        private void LoadedEvents(object sender, EventArgs e)
        {
            //
            // 在窗体加载后发生。
            //

            Me.OnSizeChanged();
            Me.OnThemeChanged();

            //

            Int32 ThreadCount = Environment.ProcessorCount;

            AsyncWorkerList = new List<BackgroundWorker>(ThreadCount);

            for (int i = 0; i < ThreadCount; i++)
            {
                BackgroundWorker AsyncWorker = new BackgroundWorker();

                AsyncWorker.WorkerSupportsCancellation = true;

                AsyncWorker.DoWork += AsyncWorkerAnalyze;
                AsyncWorker.RunWorkerCompleted += AsyncWorkerDecide;

                AsyncWorkerList.Add(AsyncWorker);
            }

            SrchRsltListAry = new List<SrchRslt>[ThreadCount];

            //

            RadioButton_ChessboardType_Gomoku_15x15.CheckedChanged -= RadioButton_ChessboardType_Gomoku_15x15_CheckedChanged;
            RadioButton_ChessboardType_Go_19x19.CheckedChanged -= RadioButton_ChessboardType_Go_19x19_CheckedChanged;

            switch (ChessboardType)
            {
                case ChessboardTypes.Gomoku_15x15: RadioButton_ChessboardType_Gomoku_15x15.Checked = true; break;
                case ChessboardTypes.Go_19x19: RadioButton_ChessboardType_Go_19x19.Checked = true; break;
            }

            RadioButton_ChessboardType_Gomoku_15x15.CheckedChanged += RadioButton_ChessboardType_Gomoku_15x15_CheckedChanged;
            RadioButton_ChessboardType_Go_19x19.CheckedChanged += RadioButton_ChessboardType_Go_19x19_CheckedChanged;

            RadioButton_ChessboardStyle_Intersection.CheckedChanged -= RadioButton_ChessboardStyle_Intersection_CheckedChanged;
            RadioButton_ChessboardStyle_Checker.CheckedChanged -= RadioButton_ChessboardStyle_Checker_CheckedChanged;

            switch (ChessboardStyle)
            {
                case ChessboardStyles.Intersection: RadioButton_ChessboardStyle_Intersection.Checked = true; break;
                case ChessboardStyles.Checker: RadioButton_ChessboardStyle_Checker.Checked = true; break;
            }

            RadioButton_ChessboardStyle_Intersection.CheckedChanged += RadioButton_ChessboardStyle_Intersection_CheckedChanged;
            RadioButton_ChessboardStyle_Checker.CheckedChanged += RadioButton_ChessboardStyle_Checker_CheckedChanged;

            //

            CheckBox_UseBalanceBreaker.CheckedChanged -= CheckBox_UseBalanceBreaker_CheckedChanged;

            CheckBox_UseBalanceBreaker.Checked = UseBalanceBreaker;

            CheckBox_UseBalanceBreaker.CheckedChanged += CheckBox_UseBalanceBreaker_CheckedChanged;

            ResetBalanceBreakerControls();

            //

            RadioButton_UseRandomThemeColor.CheckedChanged -= RadioButton_UseRandomThemeColor_CheckedChanged;
            RadioButton_UseCustomColor.CheckedChanged -= RadioButton_UseCustomColor_CheckedChanged;

            if (UseRandomThemeColor)
            {
                RadioButton_UseRandomThemeColor.Checked = true;
            }
            else
            {
                RadioButton_UseCustomColor.Checked = true;
            }

            RadioButton_UseRandomThemeColor.CheckedChanged += RadioButton_UseRandomThemeColor_CheckedChanged;
            RadioButton_UseCustomColor.CheckedChanged += RadioButton_UseCustomColor_CheckedChanged;

            Label_ThemeColorName.Enabled = !UseRandomThemeColor;

            //

            CheckBox_AntiAlias.CheckedChanged -= CheckBox_AntiAlias_CheckedChanged;

            CheckBox_AntiAlias.Checked = AntiAlias;

            CheckBox_AntiAlias.CheckedChanged += CheckBox_AntiAlias_CheckedChanged;

            //

            Label_ApplicationName.Text = ApplicationName;
            Label_ApplicationEdition.Text = ApplicationEdition;
            Label_Version.Text = "版本: " + MajorVersion + "." + MinorVersion + "." + BuildNumber + "." + BuildRevision;

            //

            Com.WinForm.ControlSubstitution.LabelAsButton(Label_StartNewGame, Label_StartNewGame_Click);
            Com.WinForm.ControlSubstitution.LabelAsButton(Label_ContinueLastGame, Label_ContinueLastGame_Click);

            //

            FunctionAreaTab = FunctionAreaTabs.Start;
        }

        private void ClosedEvents(object sender, EventArgs e)
        {
            //
            // 在窗体关闭后发生。
            //

            SaveConfig();

            if (GameUINow)
            {
                Interrupt(InterruptActions.CloseApp);
            }
        }

        private void ResizeEvents(object sender, EventArgs e)
        {
            //
            // 在窗体的大小调整时发生。
            //

            Panel_FunctionArea.Size = Panel_GameUI.Size = Panel_Client.Size = Panel_Main.Size;

            Panel_FunctionAreaOptionsBar.Size = new Size(Panel_FunctionArea.Width / 3, Panel_FunctionArea.Height);
            Label_Tab_Start.Size = Label_Tab_Record.Size = Label_Tab_Options.Size = Label_Tab_About.Size = new Size(Panel_FunctionAreaOptionsBar.Width, Panel_FunctionAreaOptionsBar.Height / 4);
            Label_Tab_Record.Top = Label_Tab_Start.Bottom;
            Label_Tab_Options.Top = Label_Tab_Record.Bottom;
            Label_Tab_About.Top = Label_Tab_Options.Bottom;

            Panel_FunctionAreaTab.Left = Panel_FunctionAreaOptionsBar.Right;
            Panel_FunctionAreaTab.Size = new Size(Panel_FunctionArea.Width - Panel_FunctionAreaOptionsBar.Width, Panel_FunctionArea.Height);

            Func<Control, Control, Size> GetTabSize = (Tab, Container) => new Size(Container.Width - (Container.Height < Tab.MinimumSize.Height ? 25 : 0), Container.Height - (Container.Width < Tab.MinimumSize.Width ? 25 : 0));

            Panel_Tab_Start.Size = GetTabSize(Panel_Tab_Start, Panel_FunctionAreaTab);
            Panel_Tab_Record.Size = GetTabSize(Panel_Tab_Record, Panel_FunctionAreaTab);
            Panel_Tab_Options.Size = GetTabSize(Panel_Tab_Options, Panel_FunctionAreaTab);
            Panel_Tab_About.Size = GetTabSize(Panel_Tab_About, Panel_FunctionAreaTab);

            //

            Panel_EnterGameSelection.Location = new Point((Panel_Tab_Start.Width - Panel_EnterGameSelection.Width) / 2, (Panel_Tab_Start.Height - Panel_EnterGameSelection.Height) / 2);

            Panel_Score.Width = Panel_Tab_Record.Width - Panel_Score.Left * 2;
            Panel_GameTime.Width = Panel_Tab_Record.Width - Panel_GameTime.Left * 2;

            Panel_Chessboard.Width = Panel_Tab_Options.Width - Panel_Chessboard.Left * 2;
            Panel_BalanceBreaker.Width = Panel_Tab_Options.Width - Panel_BalanceBreaker.Left * 2;
            Panel_StepTag.Width = Panel_Tab_Options.Width - Panel_StepTag.Left * 2;
            Panel_ThemeColor.Width = Panel_Tab_Options.Width - Panel_ThemeColor.Left * 2;
            Panel_AntiAlias.Width = Panel_Tab_Options.Width - Panel_AntiAlias.Left * 2;

            //

            Panel_Current.Width = Panel_GameUI.Width;

            Panel_Interrupt.Left = Panel_Current.Width - Panel_Interrupt.Width;

            Panel_Environment.Size = new Size(Panel_GameUI.Width, Panel_GameUI.Height - Panel_Environment.Top);
        }

        private void SizeChangedEvents(object sender, EventArgs e)
        {
            //
            // 在窗体的大小更改时发生。
            //

            if (Panel_GameUI.Visible)
            {
                ElementSize = Math.Max(1, Math.Min(Panel_Environment.Width / Range.Width, Panel_Environment.Height / Range.Height));

                EMatBmpRect.Size = new Size(Math.Max(1, ElementSize * Range.Width), Math.Max(1, ElementSize * Range.Height));
                EMatBmpRect.Location = new Point((Panel_Environment.Width - EMatBmpRect.Width) / 2, (Panel_Environment.Height - EMatBmpRect.Height) / 2);

                RepaintCurBmp();

                ElementMatrix_RepresentAll();
            }

            if (Panel_FunctionArea.Visible && FunctionAreaTab == FunctionAreaTabs.Record)
            {
                Panel_Tab_Record.Refresh();
            }
        }

        private void ThemeColorChangedEvents(object sender, EventArgs e)
        {
            //
            // 在窗体的主题色更改时发生。
            //

            // 功能区选项卡

            Panel_FunctionArea.BackColor = Me.RecommendColors.Background_DEC.ToColor();
            Panel_FunctionAreaOptionsBar.BackColor = Me.RecommendColors.Main.ToColor();

            FunctionAreaTab = _FunctionAreaTab;

            // "记录"区域

            Label_TotalGames.ForeColor = Label_UserWinGames.ForeColor = Label_UserLoseGames.ForeColor = Label_DrawGames.ForeColor = Me.RecommendColors.Text.ToColor();
            Label_TotalGamesVal.ForeColor = Label_UserWinGamesVal.ForeColor = Label_UserLoseGamesVal.ForeColor = Label_DrawGamesVal.ForeColor = Me.RecommendColors.Text_INC.ToColor();

            Label_ThisTime.ForeColor = Label_TotalTime.ForeColor = Me.RecommendColors.Text.ToColor();
            Label_ThisTimeVal.ForeColor = Label_TotalTimeVal.ForeColor = Me.RecommendColors.Text_INC.ToColor();

            // "选项"区域

            Label_Chessboard.ForeColor = Label_BalanceBreaker.ForeColor = Label_StepTag.ForeColor = Label_ThemeColor.ForeColor = Label_AntiAlias.ForeColor = Me.RecommendColors.Text_INC.ToColor();

            Label_ChessboardType.ForeColor = Label_ChessboardStyle.ForeColor = Me.RecommendColors.Text.ToColor();

            RadioButton_ChessboardType_Gomoku_15x15.ForeColor = RadioButton_ChessboardType_Go_19x19.ForeColor = RadioButton_ChessboardStyle_Intersection.ForeColor = RadioButton_ChessboardStyle_Checker.ForeColor = Me.RecommendColors.Text.ToColor();

            CheckBox_UseBalanceBreaker.ForeColor = CheckBox_ShowBanPoint.ForeColor = Me.RecommendColors.Text.ToColor();

            Label_BalanceBreaker_Info.ForeColor = Me.RecommendColors.Text.ToColor();

            Label_StepTagNum_Val.ForeColor = Me.RecommendColors.Text.ToColor();

            Panel_StepTagNumAdjustment.BackColor = Panel_FunctionArea.BackColor;

            RadioButton_UseRandomThemeColor.ForeColor = RadioButton_UseCustomColor.ForeColor = Me.RecommendColors.Text.ToColor();

            Label_ThemeColorName.Text = Com.ColorManipulation.GetColorName(Me.ThemeColor.ToColor());
            Label_ThemeColorName.ForeColor = Me.RecommendColors.Text.ToColor();

            CheckBox_AntiAlias.ForeColor = Me.RecommendColors.Text.ToColor();

            // "关于"区域

            Label_ApplicationName.ForeColor = Me.RecommendColors.Text_INC.ToColor();
            Label_ApplicationEdition.ForeColor = Label_Version.ForeColor = Label_Copyright.ForeColor = Me.RecommendColors.Text.ToColor();
            Label_GitHub_Part1.ForeColor = Label_GitHub_Base.ForeColor = Label_GitHub_Part2.ForeColor = Label_GitHub_Release.ForeColor = Me.RecommendColors.Text.ToColor();

            // 控件替代

            Com.WinForm.ControlSubstitution.PictureBoxAsButton(PictureBox_Restart, PictureBox_Restart_Click, null, PictureBox_Restart_MouseEnter, null, Color.Transparent, Me.RecommendColors.Button_INC.AtOpacity(50).ToColor(), Me.RecommendColors.Button_INC.AtOpacity(70).ToColor());
            Com.WinForm.ControlSubstitution.PictureBoxAsButton(PictureBox_ExitGame, PictureBox_ExitGame_Click, null, PictureBox_ExitGame_MouseEnter, null, Color.Transparent, Me.RecommendColors.Button_INC.AtOpacity(50).ToColor(), Me.RecommendColors.Button_INC.AtOpacity(70).ToColor());

            Com.WinForm.ControlSubstitution.LabelAsButton(Label_ThemeColorName, Label_ThemeColorName_Click, Color.Transparent, Me.RecommendColors.Button_DEC.ToColor(), Me.RecommendColors.Button_INC.ToColor(), new Font("微软雅黑", 9.75F, FontStyle.Underline, GraphicsUnit.Point, 134), new Font("微软雅黑", 9.75F, FontStyle.Regular, GraphicsUnit.Point, 134), new Font("微软雅黑", 9.75F, FontStyle.Regular, GraphicsUnit.Point, 134));

            Com.WinForm.ControlSubstitution.LabelAsButton(Label_GitHub_Base, Label_GitHub_Base_Click, Color.Transparent, Me.RecommendColors.Button_DEC.ToColor(), Me.RecommendColors.Button_INC.ToColor(), new Font("微软雅黑", 9.75F, FontStyle.Underline, GraphicsUnit.Point, 134), new Font("微软雅黑", 9.75F, FontStyle.Regular, GraphicsUnit.Point, 134), new Font("微软雅黑", 9.75F, FontStyle.Regular, GraphicsUnit.Point, 134));
            Com.WinForm.ControlSubstitution.LabelAsButton(Label_GitHub_Release, Label_GitHub_Release_Click, Color.Transparent, Me.RecommendColors.Button_DEC.ToColor(), Me.RecommendColors.Button_INC.ToColor(), new Font("微软雅黑", 9.75F, FontStyle.Underline, GraphicsUnit.Point, 134), new Font("微软雅黑", 9.75F, FontStyle.Regular, GraphicsUnit.Point, 134), new Font("微软雅黑", 9.75F, FontStyle.Regular, GraphicsUnit.Point, 134));

            // 中断按钮图像

            InterruptImages.Update(Me.RecommendColors.Text.ToColor());

            PictureBox_Restart.Image = InterruptImages.Restart;
            PictureBox_ExitGame.Image = InterruptImages.ExitGame;
        }

        #endregion

        #region 背景绘图

        private void Panel_FunctionAreaOptionsBar_Paint(object sender, PaintEventArgs e)
        {
            //
            // Panel_FunctionAreaOptionsBar 绘图。
            //

            Graphics Grap = e.Graphics;
            Grap.SmoothingMode = SmoothingMode.AntiAlias;

            //

            Control[] TabCtrl = new Control[(Int32)FunctionAreaTabs.COUNT] { Label_Tab_Start, Label_Tab_Record, Label_Tab_Options, Label_Tab_About };

            List<bool> TabBtnPointed = new List<bool>(TabCtrl.Length);
            List<bool> TabBtnSeld = new List<bool>(TabCtrl.Length);

            for (int i = 0; i < TabCtrl.Length; i++)
            {
                TabBtnPointed.Add(Com.Geometry.CursorIsInControl(TabCtrl[i]));
                TabBtnSeld.Add(FunctionAreaTab == (FunctionAreaTabs)i);
            }

            Color TabBtnCr_Bk_Pointed = Color.FromArgb(128, Color.White), TabBtnCr_Bk_Seld = Color.FromArgb(192, Color.White), TabBtnCr_Bk_Uns = Color.FromArgb(64, Color.White);

            for (int i = 0; i < TabCtrl.Length; i++)
            {
                Color TabBtnCr_Bk = (TabBtnSeld[i] ? TabBtnCr_Bk_Seld : (TabBtnPointed[i] ? TabBtnCr_Bk_Pointed : TabBtnCr_Bk_Uns));

                GraphicsPath Path_TabBtn = new GraphicsPath();
                Path_TabBtn.AddRectangle(TabCtrl[i].Bounds);
                PathGradientBrush PGB_TabBtn = new PathGradientBrush(Path_TabBtn)
                {
                    CenterColor = Color.FromArgb(TabBtnCr_Bk.A / 2, TabBtnCr_Bk),
                    SurroundColors = new Color[] { TabBtnCr_Bk },
                    FocusScales = new PointF(1F, 0F)
                };
                Grap.FillPath(PGB_TabBtn, Path_TabBtn);
                Path_TabBtn.Dispose();
                PGB_TabBtn.Dispose();

                if (TabBtnSeld[i])
                {
                    PointF[] Polygon = new PointF[] { new PointF(TabCtrl[i].Right, TabCtrl[i].Top + TabCtrl[i].Height / 4), new PointF(TabCtrl[i].Right - TabCtrl[i].Height / 4, TabCtrl[i].Top + TabCtrl[i].Height / 2), new PointF(TabCtrl[i].Right, TabCtrl[i].Bottom - TabCtrl[i].Height / 4) };

                    Grap.FillPolygon(new SolidBrush(Panel_FunctionArea.BackColor), Polygon);
                }
            }
        }

        private void Panel_Score_Paint(object sender, PaintEventArgs e)
        {
            //
            // Panel_Score 绘图。
            //

            Control Cntr = sender as Control;

            if (Cntr != null)
            {
                Pen P = new Pen(Me.RecommendColors.Border_DEC.ToColor(), 1);
                Control Ctrl = PictureBox_Overview;
                e.Graphics.DrawLine(P, new Point(Ctrl.Right, Ctrl.Top + Ctrl.Height / 2), new Point(Cntr.Width - Ctrl.Left, Ctrl.Top + Ctrl.Height / 2));
                P.Dispose();
            }

            //

            PaintScore(e);
        }

        private void Panel_GameTime_Paint(object sender, PaintEventArgs e)
        {
            //
            // Panel_GameTime 绘图。
            //

            Control Cntr = sender as Control;

            if (Cntr != null)
            {
                Pen P = new Pen(Me.RecommendColors.Border_DEC.ToColor(), 1);
                Control Ctrl = PictureBox_GameTime;
                e.Graphics.DrawLine(P, new Point(Ctrl.Right, Ctrl.Top + Ctrl.Height / 2), new Point(Cntr.Width - Ctrl.Left, Ctrl.Top + Ctrl.Height / 2));
                P.Dispose();
            }
        }

        private void Panel_Chessboard_Paint(object sender, PaintEventArgs e)
        {
            //
            // Panel_Chessboard 绘图。
            //

            Control Cntr = sender as Control;

            if (Cntr != null)
            {
                Pen P = new Pen(Me.RecommendColors.Border_DEC.ToColor(), 1);
                Control Ctrl = Label_Chessboard;
                e.Graphics.DrawLine(P, new Point(Ctrl.Right, Ctrl.Top + Ctrl.Height / 2), new Point(Cntr.Width - Ctrl.Left, Ctrl.Top + Ctrl.Height / 2));
                P.Dispose();
            }
        }

        private void Panel_BalanceBreaker_Paint(object sender, PaintEventArgs e)
        {
            //
            // Panel_BalanceBreaker 绘图。
            //

            Control Cntr = sender as Control;

            if (Cntr != null)
            {
                Pen P = new Pen(Me.RecommendColors.Border_DEC.ToColor(), 1);
                Control Ctrl = Label_BalanceBreaker;
                e.Graphics.DrawLine(P, new Point(Ctrl.Right, Ctrl.Top + Ctrl.Height / 2), new Point(Cntr.Width - Ctrl.Left, Ctrl.Top + Ctrl.Height / 2));
                P.Dispose();
            }
        }

        private void Panel_StepTag_Paint(object sender, PaintEventArgs e)
        {
            //
            // Panel_StepTag 绘图。
            //

            Control Cntr = sender as Control;

            if (Cntr != null)
            {
                Pen P = new Pen(Me.RecommendColors.Border_DEC.ToColor(), 1);
                Control Ctrl = Label_StepTag;
                e.Graphics.DrawLine(P, new Point(Ctrl.Right, Ctrl.Top + Ctrl.Height / 2), new Point(Cntr.Width - Ctrl.Left, Ctrl.Top + Ctrl.Height / 2));
                P.Dispose();
            }
        }

        private void Panel_ThemeColor_Paint(object sender, PaintEventArgs e)
        {
            //
            // Panel_ThemeColor 绘图。
            //

            Control Cntr = sender as Control;

            if (Cntr != null)
            {
                Pen P = new Pen(Me.RecommendColors.Border_DEC.ToColor(), 1);
                Control Ctrl = Label_ThemeColor;
                e.Graphics.DrawLine(P, new Point(Ctrl.Right, Ctrl.Top + Ctrl.Height / 2), new Point(Cntr.Width - Ctrl.Left, Ctrl.Top + Ctrl.Height / 2));
                P.Dispose();
            }
        }

        private void Panel_AntiAlias_Paint(object sender, PaintEventArgs e)
        {
            //
            // Panel_AntiAlias 绘图。
            //

            Control Cntr = sender as Control;

            if (Cntr != null)
            {
                Pen P = new Pen(Me.RecommendColors.Border_DEC.ToColor(), 1);
                Control Ctrl = Label_AntiAlias;
                e.Graphics.DrawLine(P, new Point(Ctrl.Right, Ctrl.Top + Ctrl.Height / 2), new Point(Cntr.Width - Ctrl.Left, Ctrl.Top + Ctrl.Height / 2));
                P.Dispose();
            }
        }

        #endregion

        #region 配置设置

        private void TransConfig()
        {
            //
            // 从当前内部版本号下最近的旧版本迁移配置文件。
            //

            try
            {
                if (!Directory.Exists(RootDir_CurrentVersion))
                {
                    if (OldVersionList.Count > 0)
                    {
                        List<Version> OldVersionList_Copy = new List<Version>(OldVersionList);
                        List<Version> OldVersionList_Sorted = new List<Version>(OldVersionList_Copy.Count);

                        while (OldVersionList_Copy.Count > 0)
                        {
                            Version LatestVersion = OldVersionList_Copy[0];

                            foreach (Version Ver in OldVersionList_Copy)
                            {
                                if (LatestVersion <= Ver)
                                {
                                    LatestVersion = Ver;
                                }
                            }

                            OldVersionList_Sorted.Add(LatestVersion);
                            OldVersionList_Copy.Remove(LatestVersion);
                        }

                        for (int i = 0; i < OldVersionList_Sorted.Count; i++)
                        {
                            string Dir = RootDir_Product + "\\" + OldVersionList_Sorted[i].Build + "." + OldVersionList_Sorted[i].Revision;

                            if (Directory.Exists(Dir))
                            {
                                try
                                {
                                    Com.IO.CopyFolder(Dir, RootDir_CurrentVersion, true, true, true);

                                    break;
                                }
                                catch
                                {
                                    continue;
                                }
                            }
                        }
                    }
                }
            }
            catch { }
        }

        private void DelOldConfig()
        {
            //
            // 删除当前内部版本号下所有旧版本的配置文件。
            //

            try
            {
                if (OldVersionList.Count > 0)
                {
                    foreach (Version Ver in OldVersionList)
                    {
                        string Dir = RootDir_Product + "\\" + Ver.Build + "." + Ver.Revision;

                        if (Directory.Exists(Dir))
                        {
                            try
                            {
                                Directory.Delete(Dir, true);
                            }
                            catch
                            {
                                continue;
                            }
                        }
                    }
                }
            }
            catch { }
        }

        private void LoadConfig()
        {
            //
            // 加载配置文件。
            //

            if (File.Exists(ConfigFilePath))
            {
                if (new FileInfo(ConfigFilePath).Length > 0)
                {
                    StreamReader Read = new StreamReader(ConfigFilePath, false);
                    string Cfg = Read.ReadLine();
                    Read.Close();

                    Regex RegexUint = new Regex(@"[^0-9]");
                    Regex RegexFloat = new Regex(@"[^0-9\-\.]");

                    //

                    try
                    {
                        string SubStr = RegexUint.Replace(Com.Text.GetIntervalString(Cfg, "<ElementSize>", "</ElementSize>", false, false), string.Empty);

                        ElementSize = Convert.ToInt32(SubStr);
                    }
                    catch { }

                    //

                    try
                    {
                        string SubStr = Com.Text.GetIntervalString(Cfg, "<ChessboardType>", "</ChessboardType>", false, false);

                        foreach (object Obj in Enum.GetValues(typeof(ChessboardTypes)))
                        {
                            if (SubStr.Trim().ToUpper() == Obj.ToString().ToUpper())
                            {
                                ChessboardType = (ChessboardTypes)Obj;

                                break;
                            }
                        }
                    }
                    catch { }

                    try
                    {
                        string SubStr = Com.Text.GetIntervalString(Cfg, "<ChessboardStyle>", "</ChessboardStyle>", false, false);

                        foreach (object Obj in Enum.GetValues(typeof(ChessboardStyles)))
                        {
                            if (SubStr.Trim().ToUpper() == Obj.ToString().ToUpper())
                            {
                                ChessboardStyle = (ChessboardStyles)Obj;

                                break;
                            }
                        }
                    }
                    catch { }

                    //

                    if (Com.Text.GetIntervalString(Cfg, "<UseBalanceBreaker>", "</UseBalanceBreaker>", false, false).Contains((!UseBalanceBreaker).ToString()))
                    {
                        UseBalanceBreaker = !UseBalanceBreaker;
                    }

                    if (Com.Text.GetIntervalString(Cfg, "<ShowBanPoint>", "</ShowBanPoint>", false, false).Contains((!ShowBanPoint).ToString()))
                    {
                        ShowBanPoint = !ShowBanPoint;
                    }

                    //

                    try
                    {
                        string SubStr = RegexUint.Replace(Com.Text.GetIntervalString(Cfg, "<StepTagNum>", "</StepTagNum>", false, false), string.Empty);

                        Int32 SST = Convert.ToInt32(SubStr);

                        if (SST >= StepTagNum_MIN && SST <= StepTagNum_MAX)
                        {
                            StepTagNum = SST;
                        }
                    }
                    catch { }

                    //

                    try
                    {
                        string SubStr = Com.Text.GetIntervalString(Cfg, "<Theme>", "</Theme>", false, false);

                        foreach (object Obj in Enum.GetValues(typeof(Com.WinForm.Theme)))
                        {
                            if (SubStr.Trim().ToUpper() == Obj.ToString().ToUpper())
                            {
                                Me.Theme = (Com.WinForm.Theme)Obj;

                                break;
                            }
                        }
                    }
                    catch { }

                    //

                    if (Com.Text.GetIntervalString(Cfg, "<UseRandomThemeColor>", "</UseRandomThemeColor>", false, false).Contains((!UseRandomThemeColor).ToString()))
                    {
                        UseRandomThemeColor = !UseRandomThemeColor;
                    }

                    if (!UseRandomThemeColor)
                    {
                        try
                        {
                            string SubStr = Com.Text.GetIntervalString(Cfg, "<ThemeColor>", "</ThemeColor>", false, false);

                            string[] Fields = SubStr.Split(',');

                            if (Fields.Length == 3)
                            {
                                int i = 0;

                                string StrR = RegexUint.Replace(Fields[i++], string.Empty);
                                Int32 TC_R = Convert.ToInt32(StrR);

                                string StrG = RegexUint.Replace(Fields[i++], string.Empty);
                                Int32 TC_G = Convert.ToInt32(StrG);

                                string StrB = RegexUint.Replace(Fields[i++], string.Empty);
                                Int32 TC_B = Convert.ToInt32(StrB);

                                Me.ThemeColor = Com.ColorX.FromRGB(TC_R, TC_G, TC_B);
                            }
                        }
                        catch { }
                    }

                    //

                    if (Com.Text.GetIntervalString(Cfg, "<ShowFormTitleColor>", "</ShowFormTitleColor>", false, false).Contains((!Me.ShowCaptionBarColor).ToString()))
                    {
                        Me.ShowCaptionBarColor = !Me.ShowCaptionBarColor;
                    }

                    //

                    try
                    {
                        string SubStr = RegexFloat.Replace(Com.Text.GetIntervalString(Cfg, "<Opacity>", "</Opacity>", false, false), string.Empty);

                        double Op = Convert.ToDouble(SubStr);

                        if (Op >= Opacity_MIN && Op <= Opacity_MAX)
                        {
                            Me.Opacity = Op;
                        }
                    }
                    catch { }

                    //

                    if (Com.Text.GetIntervalString(Cfg, "<AntiAlias>", "</AntiAlias>", false, false).Contains((!AntiAlias).ToString()))
                    {
                        AntiAlias = !AntiAlias;
                    }
                }
            }
        }

        private void SaveConfig()
        {
            //
            // 保存配置文件。
            //

            string Cfg = string.Empty;

            Cfg += "<Config>";

            Cfg += "<ElementSize>" + ElementSize + "</ElementSize>";
            Cfg += "<ChessboardType>" + ChessboardType + "</ChessboardType>";
            Cfg += "<ChessboardStyle>" + ChessboardStyle + "</ChessboardStyle>";
            Cfg += "<UseBalanceBreaker>" + UseBalanceBreaker + "</UseBalanceBreaker>";
            Cfg += "<ShowBanPoint>" + ShowBanPoint + "</ShowBanPoint>";
            Cfg += "<StepTagNum>" + StepTagNum + "</StepTagNum>";

            Cfg += "<Theme>" + Me.Theme.ToString() + "</Theme>";
            Cfg += "<UseRandomThemeColor>" + UseRandomThemeColor + "</UseRandomThemeColor>";
            Cfg += "<ThemeColor>(" + Me.ThemeColor.ToColor().R + ", " + Me.ThemeColor.ToColor().G + ", " + Me.ThemeColor.ToColor().B + ")</ThemeColor>";
            Cfg += "<ShowFormTitleColor>" + Me.ShowCaptionBarColor + "</ShowFormTitleColor>";
            Cfg += "<Opacity>" + Me.Opacity + "</Opacity>";

            Cfg += "<AntiAlias>" + AntiAlias + "</AntiAlias>";

            Cfg += "</Config>";

            //

            try
            {
                if (!Directory.Exists(ConfigFileDir))
                {
                    Directory.CreateDirectory(ConfigFileDir);
                }

                StreamWriter Write = new StreamWriter(ConfigFilePath, false);
                Write.WriteLine(Cfg);
                Write.Close();
            }
            catch { }
        }

        #endregion

        #region 存档管理

        // 用户数据。

        private void LoadUserData()
        {
            //
            // 加载用户数据。
            //

            if (File.Exists(DataFilePath))
            {
                FileInfo FInfo = new FileInfo(DataFilePath);

                if (FInfo.Length > 0)
                {
                    StreamReader SR = new StreamReader(DataFilePath, false);
                    string Str = SR.ReadLine();
                    SR.Close();

                    Regex RegexUint = new Regex(@"[^0-9]");

                    //

                    try
                    {
                        string SubStr = RegexUint.Replace(Com.Text.GetIntervalString(Str, "<TotalGameTime>", "</TotalGameTime>", false, false), string.Empty);

                        TotalGameTime = TimeSpan.FromMilliseconds(Convert.ToInt64(SubStr));
                    }
                    catch { }

                    //

                    try
                    {
                        string SubStr = RegexUint.Replace(Com.Text.GetIntervalString(Str, "<UserWinGames>", "</UserWinGames>", false, false), string.Empty);

                        UserWinGames = Convert.ToInt32(SubStr);
                    }
                    catch { }

                    try
                    {
                        string SubStr = RegexUint.Replace(Com.Text.GetIntervalString(Str, "<UserLoseGames>", "</UserLoseGames>", false, false), string.Empty);

                        UserLoseGames = Convert.ToInt32(SubStr);
                    }
                    catch { }

                    try
                    {
                        string SubStr = RegexUint.Replace(Com.Text.GetIntervalString(Str, "<DrawGames>", "</DrawGames>", false, false), string.Empty);

                        DrawGames = Convert.ToInt32(SubStr);
                    }
                    catch { }
                }
            }
        }

        private void SaveUserData()
        {
            //
            // 保存用户数据。
            //

            string Str = string.Empty;

            Str += "<Log>";

            Str += "<TotalGameTime>" + (Int64)TotalGameTime.TotalMilliseconds + "</TotalGameTime>";

            Str += "<UserWinGames>" + UserWinGames + "</UserWinGames>";
            Str += "<UserLoseGames>" + UserLoseGames + "</UserLoseGames>";
            Str += "<DrawGames>" + DrawGames + "</DrawGames>";

            Str += "</Log>";

            //

            try
            {
                if (!Directory.Exists(LogFileDir))
                {
                    Directory.CreateDirectory(LogFileDir);
                }

                StreamWriter SW = new StreamWriter(DataFilePath, false);
                SW.WriteLine(Str);
                SW.Close();
            }
            catch { }
        }

        // 上次游戏。

        private void LoadLastGame()
        {
            //
            // 加载上次游戏。
            //

            if (File.Exists(RecordFilePath))
            {
                FileInfo FInfo = new FileInfo(RecordFilePath);

                if (FInfo.Length > 0)
                {
                    StreamReader SR = new StreamReader(RecordFilePath, false);
                    string Str = SR.ReadLine();
                    SR.Close();

                    Regex RegexUint = new Regex(@"[^0-9]");

                    //

                    try
                    {
                        string SubStr = Com.Text.GetIntervalString(Str, "<ChessboardType>", "</ChessboardType>", false, false);

                        foreach (object Obj in Enum.GetValues(typeof(ChessboardTypes)))
                        {
                            if (SubStr.Trim().ToUpper() == Obj.ToString().ToUpper())
                            {
                                ChessboardType_Last = (ChessboardTypes)Obj;

                                break;
                            }
                        }
                    }
                    catch { }

                    //

                    if (Com.Text.GetIntervalString(Str, "<UseBalanceBreaker>", "</UseBalanceBreaker>", false, false).Contains((!UseBalanceBreaker_Last).ToString()))
                    {
                        UseBalanceBreaker_Last = !UseBalanceBreaker_Last;
                    }

                    if (Com.Text.GetIntervalString(Str, "<ShowBanPoint>", "</ShowBanPoint>", false, false).Contains((!ShowBanPoint_Last).ToString()))
                    {
                        ShowBanPoint_Last = !ShowBanPoint_Last;
                    }

                    //

                    if (Com.Text.GetIntervalString(Str, "<AIFirst>", "</AIFirst>", false, false).Contains((!AIFirst_Last).ToString()))
                    {
                        AIFirst_Last = !AIFirst_Last;
                    }

                    //

                    try
                    {
                        string SubStr = RegexUint.Replace(Com.Text.GetIntervalString(Str, "<FreeTimeRemaining_Black>", "</FreeTimeRemaining_Black>", false, false), string.Empty);

                        Int64 MS = Convert.ToInt64(SubStr);

                        if (MS >= 0 && MS <= TotalFreeTime.TotalMilliseconds)
                        {
                            FreeTimeRemaining_Black_Last = TimeSpan.FromMilliseconds(MS);
                        }
                    }
                    catch { }

                    try
                    {
                        string SubStr = RegexUint.Replace(Com.Text.GetIntervalString(Str, "<CountdownTimeRemaining_Black>", "</CountdownTimeRemaining_Black>", false, false), string.Empty);

                        Int64 MS = Convert.ToInt64(SubStr);

                        if (MS >= 0 && MS <= TotalCountdownTime.TotalMilliseconds)
                        {
                            CountdownTimeRemaining_Black_Last = TimeSpan.FromMilliseconds(MS);
                        }
                    }
                    catch { }

                    //

                    try
                    {
                        string SubStr = RegexUint.Replace(Com.Text.GetIntervalString(Str, "<FreeTimeRemaining_White>", "</FreeTimeRemaining_White>", false, false), string.Empty);

                        Int64 MS = Convert.ToInt64(SubStr);

                        if (MS >= 0 && MS <= TotalFreeTime.TotalMilliseconds)
                        {
                            FreeTimeRemaining_White_Last = TimeSpan.FromMilliseconds(MS);
                        }
                    }
                    catch { }

                    try
                    {
                        string SubStr = RegexUint.Replace(Com.Text.GetIntervalString(Str, "<CountdownTimeRemaining_White>", "</CountdownTimeRemaining_White>", false, false), string.Empty);

                        Int64 MS = Convert.ToInt64(SubStr);

                        if (MS >= 0 && MS <= TotalCountdownTime.TotalMilliseconds)
                        {
                            CountdownTimeRemaining_White_Last = TimeSpan.FromMilliseconds(MS);
                        }
                    }
                    catch { }

                    //

                    try
                    {
                        Int32 LastChessboardSize = ChessboardType_Last == ChessboardTypes.Go_19x19 ? 19 : 15;

                        string SubStr = Com.Text.GetIntervalString(Str, "<Element>", "</Element>", false, false);

                        while (SubStr.Contains("(") && SubStr.Contains(")"))
                        {
                            try
                            {
                                string StrE = Com.Text.GetIntervalString(SubStr, "(", ")", false, false);

                                string[] Fields = StrE.Split(',');

                                if (Fields.Length == 3)
                                {
                                    int i = 0;

                                    Point Index = new Point();
                                    Int32 E = 0;

                                    string StrIDX = RegexUint.Replace(Fields[i++], string.Empty);
                                    Index.X = Convert.ToInt32(StrIDX);

                                    string StrIDY = RegexUint.Replace(Fields[i++], string.Empty);
                                    Index.Y = Convert.ToInt32(StrIDY);

                                    string StrVal = RegexUint.Replace(Fields[i++], string.Empty);
                                    E = Convert.ToInt32(StrVal);

                                    if ((Index.X >= 0 && Index.X < LastChessboardSize && Index.Y >= 0 && Index.Y < LastChessboardSize) && (E == BLACK || E == WHITE))
                                    {
                                        ElementMatrix_Last[Index.X, Index.Y] = E;
                                        ElementIndexList_Last.Add(Index);
                                    }
                                }
                            }
                            catch { }

                            SubStr = SubStr.Substring(SubStr.IndexOf(")") + (")").Length);
                        }
                    }
                    catch { }
                }
            }
        }

        private void SaveLastGame()
        {
            //
            // 保存上次游戏。
            //

            ChessboardType_Last = ChessboardType;

            UseBalanceBreaker_Last = UseBalanceBreaker;
            ShowBanPoint_Last = ShowBanPoint;

            AIFirst_Last = AIFirst;

            FreeTimeRemaining_Black_Last = FreeTimeRemaining_Black;
            CountdownTimeRemaining_Black_Last = CountdownTimeRemaining_Black;

            FreeTimeRemaining_White_Last = FreeTimeRemaining_White;
            CountdownTimeRemaining_White_Last = CountdownTimeRemaining_White;

            foreach (Point A in ElementIndexList_Last)
            {
                ElementMatrix_Last[A.X, A.Y] = 0;
            }

            ElementIndexList_Last.Clear();

            foreach (Point A in ElementIndexList)
            {
                ElementMatrix_Last[A.X, A.Y] = ElementMatrix_GetValue(A);

                ElementIndexList_Last.Add(A);
            }

            //

            string Str = string.Empty;

            Str += "<Log>";

            Str += "<ChessboardType>" + ChessboardType + "</ChessboardType>";

            Str += "<UseBalanceBreaker>" + UseBalanceBreaker + "</UseBalanceBreaker>";
            Str += "<ShowBanPoint>" + ShowBanPoint + "</ShowBanPoint>";

            Str += "<AIFirst>" + AIFirst + "</AIFirst>";

            Str += "<FreeTimeRemaining_Black>" + (Int64)FreeTimeRemaining_Black.TotalMilliseconds + "</FreeTimeRemaining_Black>";
            Str += "<CountdownTimeRemaining_Black>" + (Int64)CountdownTimeRemaining_Black.TotalMilliseconds + "</CountdownTimeRemaining_Black>";

            Str += "<FreeTimeRemaining_White>" + (Int64)FreeTimeRemaining_White.TotalMilliseconds + "</FreeTimeRemaining_White>";
            Str += "<CountdownTimeRemaining_White>" + (Int64)CountdownTimeRemaining_White.TotalMilliseconds + "</CountdownTimeRemaining_White>";

            Str += "<Element>[";
            for (int i = 0; i < ElementIndexList.Count; i++)
            {
                Point A = ElementIndexList[i];

                Str += "(" + A.X + "," + A.Y + "," + ElementMatrix_GetValue(A) + ")";
            }
            Str += "]</Element>";

            Str += "</Log>";

            //

            try
            {
                if (!Directory.Exists(LogFileDir))
                {
                    Directory.CreateDirectory(LogFileDir);
                }

                StreamWriter SW = new StreamWriter(RecordFilePath, false);
                SW.WriteLine(Str);
                SW.Close();
            }
            catch { }
        }

        private void EraseLastGame()
        {
            //
            // 擦除上次游戏。
            //

            foreach (Point A in ElementIndexList_Last)
            {
                ElementMatrix_Last[A.X, A.Y] = 0;
            }

            ElementIndexList_Last.Clear();

            //

            try
            {
                if (!Directory.Exists(LogFileDir))
                {
                    Directory.CreateDirectory(LogFileDir);
                }

                StreamWriter SW = new StreamWriter(RecordFilePath, false);
                SW.WriteLine(string.Empty);
                SW.Close();
            }
            catch { }
        }

        #endregion

        #region 数组功能

        private static Int32[,] GetCopyOfArray(Int32[,] Array)
        {
            //
            // 返回二维矩阵的浅表副本。Array：矩阵。
            //

            try
            {
                if (Array != null)
                {
                    return (Int32[,])Array.Clone();
                }

                return null;
            }
            catch
            {
                return null;
            }
        }

        private static Int32 GetZeroCountOfArray(Int32[,] Array, Size Cap)
        {
            //
            // 计算二维矩阵值为 0 的元素的数量。Array：矩阵，索引为 [x, y]；Cap：矩阵的大小，分量 (Width, Height) 分别表示沿 x 方向和沿 y 方向的元素数量。
            //

            try
            {
                if (Array != null)
                {
                    Int32 ZeroCount = 0;

                    for (int X = 0; X < Cap.Width; X++)
                    {
                        for (int Y = 0; Y < Cap.Height; Y++)
                        {
                            if (Array[X, Y] == 0)
                            {
                                ZeroCount++;
                            }
                        }
                    }

                    return ZeroCount;
                }

                return 0;
            }
            catch
            {
                return 0;
            }
        }

        private static List<Point> GetCertainIndexListOfArray(Int32[,] Array, Size Cap, Int32 Value)
        {
            //
            // 返回二维矩阵中所有值为指定值的元素的索引的列表。Array：矩阵，索引为 [x, y]；Cap：矩阵的大小，分量 (Width, Height) 分别表示沿 x 方向和沿 y 方向的元素数量；Value：指定值。
            //

            try
            {
                if (Array != null)
                {
                    List<Point> L = new List<Point>(Cap.Width * Cap.Height);

                    for (int X = 0; X < Cap.Width; X++)
                    {
                        for (int Y = 0; Y < Cap.Height; Y++)
                        {
                            if (Array[X, Y] == Value)
                            {
                                L.Add(new Point(X, Y));
                            }
                        }
                    }

                    return L;
                }

                return new List<Point>(0);
            }
            catch
            {
                return new List<Point>(0);
            }
        }

        private static void ArrayLogicalAppend(Int32[,] Array, Size Cap, Point A, Int32 Value)
        {
            //
            // 向二维矩阵逻辑添加一个元素。Array：矩阵，索引为 [x, y]；Cap：矩阵的大小，分量 (Width, Height) 分别表示沿 x 方向和沿 y 方向的元素数量；A：索引；Value：元素的值。
            //

            try
            {
                if (Array != null)
                {
                    if (Value != 0 && (A.X >= 0 && A.X < Cap.Width && A.Y >= 0 && A.Y < Cap.Height))
                    {
                        if (Array[A.X, A.Y] == 0)
                        {
                            Array[A.X, A.Y] = Value;
                        }
                    }
                }
            }
            catch { }
        }

        #endregion

        #region 元素矩阵基本功能

        // 初始化。

        private void ElementMatrix_Initialize()
        {
            //
            // 初始化。
            //

            for (int i = 0; i < ElementIndexList.Count; i++)
            {
                ElementMatrix[ElementIndexList[i].X, ElementIndexList[i].Y] = 0;
            }

            ElementIndexList.Clear();
        }

        // 索引。

        private bool ElementMatrix_IndexValid(Point A)
        {
            //
            // 检查指定的索引是否有效。A：索引。
            //

            try
            {
                return (A.X >= 0 && A.X < Range.Width && A.Y >= 0 && A.Y < Range.Height);
            }
            catch
            {
                return false;
            }
        }

        private Int32 ElementMatrix_GetValue(Point A)
        {
            //
            // 获取元素矩阵指定的索引的元素的值。A：索引。
            //

            try
            {
                if (ElementMatrix_IndexValid(A))
                {
                    return ElementMatrix[A.X, A.Y];
                }

                return Int32.MinValue;
            }
            catch
            {
                return Int32.MinValue;
            }
        }

        private Int32 ElementMatrix_GetValue(Int32 X, Int32 Y)
        {
            //
            // 获取元素矩阵指定的索引的元素的值。X，Y：索引。
            //

            try
            {
                if (ElementMatrix_IndexValid(new Point(X, Y)))
                {
                    return ElementMatrix[X, Y];
                }

                return Int32.MinValue;
            }
            catch
            {
                return Int32.MinValue;
            }
        }

        private Point ElementMatrix_GetIndex(Point P)
        {
            //
            // 获取绘图容器中的指定坐标所在元素的索引。P：坐标。
            //

            try
            {
                Point dP = new Point(P.X - EMatBmpRect.X, P.Y - EMatBmpRect.Y);
                Point A = new Point((Int32)Math.Floor((double)dP.X / ElementSize), (Int32)Math.Floor((double)dP.Y / ElementSize));

                if (ElementMatrix_IndexValid(A))
                {
                    return A;
                }

                return new Point(-1, -1);
            }
            catch
            {
                return new Point(-1, -1);
            }
        }

        // 添加与移除。

        private void ElementMatrix_Add(Point A, Int32 E)
        {
            //
            // 向元素矩阵添加一个元素。A：索引；E：元素的值。
            //

            if (E != 0 && ElementMatrix_IndexValid(A))
            {
                if (!ElementIndexList.Contains(A))
                {
                    ElementMatrix[A.X, A.Y] = E;

                    ElementIndexList.Add(A);
                }
            }
        }

        private void ElementMatrix_RemoveAt(Point A)
        {
            //
            // 从元素矩阵移除一个元素。A：索引。
            //

            if (ElementMatrix_IndexValid(A))
            {
                ElementMatrix[A.X, A.Y] = 0;

                if (ElementIndexList.Contains(A))
                {
                    ElementIndexList.Remove(A);
                }
            }
        }

        // 颜色。

        private Color ElementMatrix_GetColor(Int32 E)
        {
            //
            // 获取元素颜色。E：元素的值。
            //

            try
            {
                if (E == 0)
                {
                    return Me.RecommendColors.Background.ToColor();
                }
                else if (E > 0)
                {
                    if (E == BLACK)
                    {
                        return GlobalColors.BlackChess;
                    }
                    else if (E == WHITE)
                    {
                        return GlobalColors.WhiteChess;
                    }
                }

                return Color.Empty;
            }
            catch
            {
                return Color.Empty;
            }
        }

        // 绘图与呈现。

        private Rectangle EMatBmpRect = new Rectangle(); // 元素矩阵位图区域（相对于绘图容器）。

        private Bitmap EMatBmp; // 元素矩阵位图。

        private Graphics EMatBmpGrap; // 元素矩阵位图绘图。

        private void ElementMatrix_DrawAtPoint(Point A, bool PresentNow)
        {
            //
            // 在元素矩阵位图的指定索引处绘制一个元素。A：索引；PresentNow：是否立即呈现此元素，如果为 true，那么将在位图中绘制此元素，并在不重绘整个位图的情况下在容器中绘制此元素，如果为 false，那么将仅在位图中绘制此元素。
            //

            if (ElementMatrix_IndexValid(A))
            {
                Rectangle BmpRect = new Rectangle(new Point(A.X * ElementSize, A.Y * ElementSize), new Size(ElementSize, ElementSize));

                Bitmap Bmp = new Bitmap(BmpRect.Width, BmpRect.Height);

                Graphics BmpGrap = Graphics.FromImage(Bmp);

                if (AntiAlias)
                {
                    BmpGrap.SmoothingMode = SmoothingMode.AntiAlias;
                    BmpGrap.TextRenderingHint = TextRenderingHint.AntiAlias;
                }

                BmpGrap.Clear(ChessboardBackColor);

                // 棋盘：

                switch (ChessboardStyle)
                {
                    case ChessboardStyles.Intersection:
                        {
                            Pen ChessboardLine = new Pen(Color.FromArgb(192, ChessboardLineColor), 1);

                            if (A.X == 0 && A.Y == 0)
                            {
                                BmpGrap.FillRectangle(new LinearGradientBrush(new Point(-1, -1), new Point(BmpRect.Width, BmpRect.Height), GameUIBackColor_DEC, ChessboardBackColor), new Rectangle(new Point(0, 0), BmpRect.Size));

                                BmpGrap.DrawLine(ChessboardLine, new Point(BmpRect.Width / 2, BmpRect.Height / 2), new Point(BmpRect.Width, BmpRect.Height / 2));
                                BmpGrap.DrawLine(ChessboardLine, new Point(BmpRect.Width / 2, BmpRect.Height / 2), new Point(BmpRect.Width / 2, BmpRect.Height));
                            }
                            else if (A.X == 0 && A.Y == Range.Height - 1)
                            {
                                BmpGrap.FillRectangle(new LinearGradientBrush(new Point(-1, BmpRect.Height), new Point(BmpRect.Width, -1), GameUIBackColor_DEC, ChessboardBackColor), new Rectangle(new Point(0, 0), BmpRect.Size));

                                BmpGrap.DrawLine(ChessboardLine, new Point(BmpRect.Width / 2, BmpRect.Height / 2), new Point(BmpRect.Width, BmpRect.Height / 2));
                                BmpGrap.DrawLine(ChessboardLine, new Point(BmpRect.Width / 2, 0), new Point(BmpRect.Width / 2, BmpRect.Height / 2));
                            }
                            else if (A.X == Range.Width - 1 && A.Y == 0)
                            {
                                BmpGrap.FillRectangle(new LinearGradientBrush(new Point(BmpRect.Width, -1), new Point(-1, BmpRect.Height), GameUIBackColor_DEC, ChessboardBackColor), new Rectangle(new Point(0, 0), BmpRect.Size));

                                BmpGrap.DrawLine(ChessboardLine, new Point(0, BmpRect.Height / 2), new Point(BmpRect.Width / 2, BmpRect.Height / 2));
                                BmpGrap.DrawLine(ChessboardLine, new Point(BmpRect.Width / 2, BmpRect.Height / 2), new Point(BmpRect.Width / 2, BmpRect.Height));
                            }
                            else if (A.X == Range.Width - 1 && A.Y == Range.Height - 1)
                            {
                                BmpGrap.FillRectangle(new LinearGradientBrush(new Point(BmpRect.Width, BmpRect.Height), new Point(-1, -1), GameUIBackColor_DEC, ChessboardBackColor), new Rectangle(new Point(0, 0), BmpRect.Size));

                                BmpGrap.DrawLine(ChessboardLine, new Point(0, BmpRect.Height / 2), new Point(BmpRect.Width / 2, BmpRect.Height / 2));
                                BmpGrap.DrawLine(ChessboardLine, new Point(BmpRect.Width / 2, 0), new Point(BmpRect.Width / 2, BmpRect.Height / 2));
                            }
                            else if ((A.X > 0 && A.X < Range.Width - 1) && A.Y == 0)
                            {
                                BmpGrap.FillRectangle(new LinearGradientBrush(new Point(0, -1), new Point(0, BmpRect.Height), Com.ColorManipulation.BlendByRGB(GameUIBackColor_DEC, ChessboardBackColor, 0.5), ChessboardBackColor), new Rectangle(new Point(0, 0), BmpRect.Size));

                                BmpGrap.DrawLine(ChessboardLine, new Point(0, BmpRect.Height / 2), new Point(BmpRect.Width, BmpRect.Height / 2));
                                BmpGrap.DrawLine(ChessboardLine, new Point(BmpRect.Width / 2, BmpRect.Height / 2), new Point(BmpRect.Width / 2, BmpRect.Height));
                            }
                            else if ((A.X > 0 && A.X < Range.Width - 1) && A.Y == Range.Height - 1)
                            {
                                BmpGrap.FillRectangle(new LinearGradientBrush(new Point(0, BmpRect.Height), new Point(0, -1), Com.ColorManipulation.BlendByRGB(GameUIBackColor_DEC, ChessboardBackColor, 0.5), ChessboardBackColor), new Rectangle(new Point(0, 0), BmpRect.Size));

                                BmpGrap.DrawLine(ChessboardLine, new Point(0, BmpRect.Height / 2), new Point(BmpRect.Width, BmpRect.Height / 2));
                                BmpGrap.DrawLine(ChessboardLine, new Point(BmpRect.Width / 2, 0), new Point(BmpRect.Width / 2, BmpRect.Height / 2));
                            }
                            else if (A.X == 0 && (A.Y > 0 && A.Y < Range.Height - 1))
                            {
                                BmpGrap.FillRectangle(new LinearGradientBrush(new Point(-1, 0), new Point(BmpRect.Width, 0), Com.ColorManipulation.BlendByRGB(GameUIBackColor_DEC, ChessboardBackColor, 0.5), ChessboardBackColor), new Rectangle(new Point(0, 0), BmpRect.Size));

                                BmpGrap.DrawLine(ChessboardLine, new Point(BmpRect.Width / 2, BmpRect.Height / 2), new Point(BmpRect.Width, BmpRect.Height / 2));
                                BmpGrap.DrawLine(ChessboardLine, new Point(BmpRect.Width / 2, 0), new Point(BmpRect.Width / 2, BmpRect.Height));
                            }
                            else if (A.X == Range.Width - 1 && (A.Y > 0 && A.Y < Range.Height - 1))
                            {
                                BmpGrap.FillRectangle(new LinearGradientBrush(new Point(BmpRect.Width, 0), new Point(-1, 0), Com.ColorManipulation.BlendByRGB(GameUIBackColor_DEC, ChessboardBackColor, 0.5), ChessboardBackColor), new Rectangle(new Point(0, 0), BmpRect.Size));

                                BmpGrap.DrawLine(ChessboardLine, new Point(0, BmpRect.Height / 2), new Point(BmpRect.Width / 2, BmpRect.Height / 2));
                                BmpGrap.DrawLine(ChessboardLine, new Point(BmpRect.Width / 2, 0), new Point(BmpRect.Width / 2, BmpRect.Height));
                            }
                            else
                            {
                                BmpGrap.DrawLine(ChessboardLine, new Point(0, BmpRect.Height / 2), new Point(BmpRect.Width, BmpRect.Height / 2));
                                BmpGrap.DrawLine(ChessboardLine, new Point(BmpRect.Width / 2, 0), new Point(BmpRect.Width / 2, BmpRect.Height));
                            }
                        }
                        break;

                    case ChessboardStyles.Checker:
                        {
                            Color _ChessboardBackColor = ((A.X + A.Y) % 2 == 0 ? Com.ColorManipulation.ShiftLightnessByHSL(ChessboardBackColor, 0.5) : ChessboardBackColor);

                            if (A.X == 0 && A.Y == 0)
                            {
                                BmpGrap.FillRectangle(new LinearGradientBrush(new Point(-1, -1), new Point(BmpRect.Width, BmpRect.Height), GameUIBackColor_DEC, _ChessboardBackColor), new Rectangle(new Point(0, 0), BmpRect.Size));
                            }
                            else if (A.X == 0 && A.Y == Range.Height - 1)
                            {
                                BmpGrap.FillRectangle(new LinearGradientBrush(new Point(-1, BmpRect.Height), new Point(BmpRect.Width, -1), GameUIBackColor_DEC, _ChessboardBackColor), new Rectangle(new Point(0, 0), BmpRect.Size));
                            }
                            else if (A.X == Range.Width - 1 && A.Y == 0)
                            {
                                BmpGrap.FillRectangle(new LinearGradientBrush(new Point(BmpRect.Width, -1), new Point(-1, BmpRect.Height), GameUIBackColor_DEC, _ChessboardBackColor), new Rectangle(new Point(0, 0), BmpRect.Size));
                            }
                            else if (A.X == Range.Width - 1 && A.Y == Range.Height - 1)
                            {
                                BmpGrap.FillRectangle(new LinearGradientBrush(new Point(BmpRect.Width, BmpRect.Height), new Point(-1, -1), GameUIBackColor_DEC, _ChessboardBackColor), new Rectangle(new Point(0, 0), BmpRect.Size));
                            }
                            else if ((A.X > 0 && A.X < Range.Width - 1) && A.Y == 0)
                            {
                                BmpGrap.FillRectangle(new LinearGradientBrush(new Point(0, -1), new Point(0, BmpRect.Height), Com.ColorManipulation.BlendByRGB(GameUIBackColor_DEC, _ChessboardBackColor, 0.5), _ChessboardBackColor), new Rectangle(new Point(0, 0), BmpRect.Size));
                            }
                            else if ((A.X > 0 && A.X < Range.Width - 1) && A.Y == Range.Height - 1)
                            {
                                BmpGrap.FillRectangle(new LinearGradientBrush(new Point(0, BmpRect.Height), new Point(0, -1), Com.ColorManipulation.BlendByRGB(GameUIBackColor_DEC, _ChessboardBackColor, 0.5), _ChessboardBackColor), new Rectangle(new Point(0, 0), BmpRect.Size));
                            }
                            else if (A.X == 0 && (A.Y > 0 && A.Y < Range.Height - 1))
                            {
                                BmpGrap.FillRectangle(new LinearGradientBrush(new Point(-1, 0), new Point(BmpRect.Width, 0), Com.ColorManipulation.BlendByRGB(GameUIBackColor_DEC, _ChessboardBackColor, 0.5), _ChessboardBackColor), new Rectangle(new Point(0, 0), BmpRect.Size));
                            }
                            else if (A.X == Range.Width - 1 && (A.Y > 0 && A.Y < Range.Height - 1))
                            {
                                BmpGrap.FillRectangle(new LinearGradientBrush(new Point(BmpRect.Width, 0), new Point(-1, 0), Com.ColorManipulation.BlendByRGB(GameUIBackColor_DEC, _ChessboardBackColor, 0.5), _ChessboardBackColor), new Rectangle(new Point(0, 0), BmpRect.Size));
                            }
                            else
                            {
                                BmpGrap.FillRectangle(new SolidBrush(_ChessboardBackColor), new Rectangle(new Point(0, 0), BmpRect.Size));
                            }
                        }
                        break;
                }

                // 坐标：

                if (A.Y == 0)
                {
                    string StringText = ((char)('A' + A.X)).ToString();

                    Color StringColor = Me.RecommendColors.Text_INC.ToColor();
                    Font StringFont = Com.Text.GetSuitableFont(StringText, new Font("微软雅黑", 9F, FontStyle.Regular, GraphicsUnit.Point, 134), new SizeF(Bmp.Width * 0.5F, Bmp.Height * 0.4F));
                    RectangleF StringRect = new RectangleF();
                    StringRect.Size = BmpGrap.MeasureString(StringText, StringFont);
                    StringRect.Location = new PointF((Bmp.Width - StringRect.Width) / 2, (Bmp.Height / 2 - StringRect.Height) / 2);

                    Com.Painting2D.PaintTextWithShadow(Bmp, StringText, StringFont, StringColor, StringColor, StringRect.Location, 0.1F, AntiAlias);
                }

                if (A.X == 0)
                {
                    string StringText = (1 + A.Y).ToString();

                    Color StringColor = Me.RecommendColors.Text_INC.ToColor();
                    Font StringFont = Com.Text.GetSuitableFont(StringText, new Font("微软雅黑", 9F, FontStyle.Regular, GraphicsUnit.Point, 134), new SizeF(Bmp.Width * 0.5F, Bmp.Height * 0.4F));
                    RectangleF StringRect = new RectangleF();
                    StringRect.Size = BmpGrap.MeasureString(StringText, StringFont);
                    StringRect.Location = new PointF((Bmp.Width / 2 - StringRect.Width) / 2, (Bmp.Height - StringRect.Height) / 2);

                    Com.Painting2D.PaintTextWithShadow(Bmp, StringText, StringFont, StringColor, StringColor, StringRect.Location, 0.1F, AntiAlias);
                }

                // 星位、禁手点、棋子：

                Int32 E = ElementMatrix_GetValue(A);

                if (E == 0)
                {
                    if (Star.Contains(A))
                    {
                        Rectangle Rect_Star = new Rectangle(new Point((Int32)((BmpRect.Width - BmpRect.Width * 0.15) / 2 + 0.5), (Int32)((BmpRect.Height - BmpRect.Height * 0.15) / 2 + 0.5)), new Size(Math.Max(1, (Int32)(BmpRect.Width * 0.15)), Math.Max(1, (Int32)(BmpRect.Height * 0.15))));

                        BmpGrap.FillEllipse(new SolidBrush(ChessboardStarColor), Rect_Star);
                    }

                    //

                    bool IsShownBanPoint = false, IsSelfBanPoint = false;

                    if (UseBalanceBreaker && ShowBanPoint)
                    {
                        if (BanIDList.Contains(A))
                        {
                            IsShownBanPoint = true;
                            IsSelfBanPoint = !AIFirst;
                        }
                        else
                        {
                            bool Flag;

                            switch (ElementIndexList.Count)
                            {
                                case 0: Flag = (A == Center); break;
                                case 1: Flag = (Math.Abs(A.X - Center.X) <= 1 && Math.Abs(A.Y - Center.Y) <= 1); break;
                                case 2: Flag = (Math.Abs(A.X - Center.X) <= 2 && Math.Abs(A.Y - Center.Y) <= 2); break;
                                default: Flag = true; break;
                            }

                            IsShownBanPoint = (!Flag && A == GameUIPointedIndex);
                            IsSelfBanPoint = true;
                        }
                    }

                    Action<bool> DrawBanMark = (IsHighlight) =>
                    {
                        Rectangle Rect_Ban = new Rectangle(new Point((Int32)((BmpRect.Width - BmpRect.Width * 0.4) / 2 + 0.5), (Int32)((BmpRect.Height - BmpRect.Height * 0.4) / 2 + 0.5)), new Size(Math.Max(1, (Int32)(BmpRect.Width * 0.4)), Math.Max(1, (Int32)(BmpRect.Height * 0.4))));

                        Color ColorBan = (IsSelfBanPoint ? GlobalColors.Ban_Self : GlobalColors.Ban_Opp);

                        Int32 PenWidth = (IsHighlight ? 4 : 2);

                        BmpGrap.DrawLine(new Pen(ColorBan, PenWidth), new Point(Rect_Ban.X, Rect_Ban.Y), new Point(Rect_Ban.Right, Rect_Ban.Bottom));
                        BmpGrap.DrawLine(new Pen(ColorBan, PenWidth), new Point(Rect_Ban.Right, Rect_Ban.Y), new Point(Rect_Ban.X, Rect_Ban.Bottom));
                    };

                    Action<bool> DrawChess = (IsHighlight) =>
                    {
                        Rectangle Rect_Cen = new Rectangle(new Point(BmpRect.Width / 24, BmpRect.Height / 24), new Size(Math.Max(1, BmpRect.Width * 11 / 12), Math.Max(1, BmpRect.Height * 11 / 12)));

                        Color Cr_Cen, Cr_LT, Cr_RB;

                        Cr_Cen = (CurrentColor == Roles.Black ? GlobalColors.BlackChess : GlobalColors.WhiteChess);

                        if (IsHighlight)
                        {
                            Cr_Cen = Com.ColorManipulation.BlendByRGB((CurrentPlayer == Roles.User ? GlobalColors.Ban_Self : GlobalColors.Ban_Opp), Cr_Cen, 0.3);
                        }

                        if (CurrentColor == Roles.Black)
                        {
                            Cr_LT = Color.FromArgb(192, Com.ColorManipulation.ShiftLightnessByHSL(Cr_Cen, 0.3));
                            Cr_RB = Color.FromArgb(192, Cr_Cen);
                        }
                        else
                        {
                            Cr_LT = Color.FromArgb(192, Cr_Cen);
                            Cr_RB = Color.FromArgb(192, Com.ColorManipulation.ShiftLightnessByHSL(Cr_Cen, -0.3));
                        }

                        BmpGrap.FillEllipse(new LinearGradientBrush(new Point(Rect_Cen.X - 1, Rect_Cen.Y - 1), new Point(Rect_Cen.Right, Rect_Cen.Bottom), Cr_LT, Cr_RB), Rect_Cen);
                    };

                    if (A == GameUIPointedIndex)
                    {
                        if (IsShownBanPoint)
                        {
                            if (IsSelfBanPoint)
                            {
                                DrawChess(true);
                                DrawBanMark(true);
                            }
                            else
                            {
                                DrawBanMark(false);
                                DrawChess(false);
                            }
                        }
                        else
                        {
                            DrawChess(false);
                        }
                    }
                    else
                    {
                        if (IsShownBanPoint)
                        {
                            DrawBanMark(false);
                        }
                    }
                }
                else
                {
                    Rectangle Rect_Cen = new Rectangle(new Point(BmpRect.Width / 24, BmpRect.Height / 24), new Size(Math.Max(1, BmpRect.Width * 11 / 12), Math.Max(1, BmpRect.Height * 11 / 12)));

                    Color Cr_Cen = ElementMatrix_GetColor(E);

                    if (E == BLACK)
                    {
                        BmpGrap.FillEllipse(new LinearGradientBrush(new Point(Rect_Cen.X - 1, Rect_Cen.Y - 1), new Point(Rect_Cen.Right, Rect_Cen.Bottom), Com.ColorManipulation.ShiftLightnessByHSL(Cr_Cen, 0.3), Cr_Cen), Rect_Cen);
                    }
                    else
                    {
                        BmpGrap.FillEllipse(new LinearGradientBrush(new Point(Rect_Cen.X - 1, Rect_Cen.Y - 1), new Point(Rect_Cen.Right, Rect_Cen.Bottom), Cr_Cen, Com.ColorManipulation.ShiftLightnessByHSL(Cr_Cen, -0.3)), Rect_Cen);
                    }

                    if (JdgBasisIDList.Contains(A))
                    {
                        Color Cr = Com.ColorManipulation.GetComplementaryColor(Cr_Cen);

                        Rectangle R = new Rectangle(new Point((Int32)(Rect_Cen.X + (Rect_Cen.Width - Rect_Cen.Width * 0.8) / 2 + 0.5), (Int32)(Rect_Cen.Y + (Rect_Cen.Height - Rect_Cen.Height * 0.8) / 2 + 0.5)), new Size(Math.Max(1, (Int32)(Rect_Cen.Width * 0.8)), Math.Max(1, (Int32)(Rect_Cen.Height * 0.8))));

                        GraphicsPath Path = new GraphicsPath();
                        Path.AddEllipse(R);
                        PathGradientBrush PGB = new PathGradientBrush(Path)
                        {
                            CenterColor = Color.FromArgb(64, Cr),
                            SurroundColors = new Color[] { Color.Transparent },
                            FocusScales = new PointF(0.7F, 0.7F)
                        };
                        BmpGrap.FillPath(PGB, Path);
                        Path.Dispose();
                        PGB.Dispose();
                    }

                    string StringText = (ElementIndexList.IndexOf(A) >= ElementIndexList.Count - StepTagNum ? (ElementIndexList.IndexOf(A) + 1).ToString() : string.Empty);

                    if (StringText.Length > 0)
                    {
                        Color StringColor = ((Com.ColorX)Cr_Cen).Invert.ToColor();
                        Font StringFont = Com.Text.GetSuitableFont(StringText, new Font("微软雅黑", 9F, FontStyle.Regular, GraphicsUnit.Point, 134), new SizeF(Rect_Cen.Width * 0.75F, Rect_Cen.Height * 0.6F));
                        RectangleF StringRect = new RectangleF();
                        StringRect.Size = BmpGrap.MeasureString(StringText, StringFont);
                        StringRect.Location = new PointF(Rect_Cen.X + (Rect_Cen.Width - StringRect.Width) / 2, Rect_Cen.Y + (Rect_Cen.Height - StringRect.Height) / 2);

                        Com.Painting2D.PaintTextWithShadow(Bmp, StringText, StringFont, StringColor, StringColor, StringRect.Location, 0.05F, AntiAlias);
                    }
                }

                //

                if (Bmp != null)
                {
                    EMatBmpGrap.DrawImage(Bmp, BmpRect.Location);

                    if (PresentNow)
                    {
                        Panel_Environment.CreateGraphics().DrawImage(Bmp, new Point(EMatBmpRect.X + BmpRect.X, EMatBmpRect.Y + BmpRect.Y));
                    }
                }
            }
        }

        private void ElementMatrix_RepresentAll()
        {
            //
            // 更新并呈现元素矩阵包含的所有元素。
            //

            if (Panel_Environment.Visible && (Panel_Environment.Width > 0 && Panel_Environment.Height > 0))
            {
                if (EMatBmp != null)
                {
                    EMatBmp.Dispose();
                }

                EMatBmp = new Bitmap(Math.Max(1, EMatBmpRect.Width), Math.Max(1, EMatBmpRect.Height));

                EMatBmpGrap = Graphics.FromImage(EMatBmp);

                if (AntiAlias)
                {
                    EMatBmpGrap.SmoothingMode = SmoothingMode.AntiAlias;
                    EMatBmpGrap.TextRenderingHint = TextRenderingHint.AntiAlias;
                }

                EMatBmpGrap.Clear(GameUIBackColor_INC);

                //

                for (int X = 0; X < Range.Width; X++)
                {
                    for (int Y = 0; Y < Range.Height; Y++)
                    {
                        ElementMatrix_DrawAtPoint(new Point(X, Y), false);
                    }
                }

                //

                if (GameState != GameStates.Ongoing)
                {
                    EMatBmpGrap.FillRectangle(new SolidBrush(Color.FromArgb(128, Color.White)), new Rectangle(new Point(0, 0), EMatBmp.Size));
                }

                //

                RepaintEMatBmp();
            }
        }

        private void ElementMatrix_PresentAt(Point A)
        {
            //
            // 呈现元素矩阵中指定的索引处的一个元素。A：索引。
            //

            if (Panel_Environment.Visible && (Panel_Environment.Width > 0 && Panel_Environment.Height > 0))
            {
                if (ElementMatrix_IndexValid(A))
                {
                    ElementMatrix_DrawAtPoint(A, true);
                }
            }
        }

        private void RepaintEMatBmp()
        {
            //
            // 重绘元素矩阵位图。
            //

            if (EMatBmp != null)
            {
                if (Panel_Environment.Width > EMatBmp.Width)
                {
                    Panel_Environment.CreateGraphics().FillRectangles(new SolidBrush(GameUIBackColor_DEC), new Rectangle[] { new Rectangle(new Point(0, 0), new Size((Panel_Environment.Width - EMatBmp.Width) / 2, Panel_Environment.Height)), new Rectangle(new Point(Panel_Environment.Width - (Panel_Environment.Width - EMatBmp.Width) / 2, 0), new Size((Panel_Environment.Width - EMatBmp.Width) / 2, Panel_Environment.Height)) });
                }

                if (Panel_Environment.Height > EMatBmp.Height)
                {
                    Panel_Environment.CreateGraphics().FillRectangles(new SolidBrush(GameUIBackColor_DEC), new Rectangle[] { new Rectangle(new Point(0, 0), new Size(Panel_Environment.Width, (Panel_Environment.Height - EMatBmp.Height) / 2)), new Rectangle(new Point(0, Panel_Environment.Height - (Panel_Environment.Height - EMatBmp.Height) / 2), new Size(Panel_Environment.Width, (Panel_Environment.Height - EMatBmp.Height) / 2)) });
                }

                Panel_Environment.CreateGraphics().DrawImage(EMatBmp, EMatBmpRect);
            }
        }

        private void Panel_Environment_Paint(object sender, PaintEventArgs e)
        {
            //
            // Panel_Environment 绘图。
            //

            if (EMatBmp != null)
            {
                if (Panel_Environment.Width > EMatBmp.Width)
                {
                    e.Graphics.FillRectangles(new SolidBrush(GameUIBackColor_DEC), new Rectangle[] { new Rectangle(new Point(0, 0), new Size((Panel_Environment.Width - EMatBmp.Width) / 2, Panel_Environment.Height)), new Rectangle(new Point(Panel_Environment.Width - (Panel_Environment.Width - EMatBmp.Width) / 2, 0), new Size((Panel_Environment.Width - EMatBmp.Width) / 2, Panel_Environment.Height)) });
                }

                if (Panel_Environment.Height > EMatBmp.Height)
                {
                    e.Graphics.FillRectangles(new SolidBrush(GameUIBackColor_DEC), new Rectangle[] { new Rectangle(new Point(0, 0), new Size(Panel_Environment.Width, (Panel_Environment.Height - EMatBmp.Height) / 2)), new Rectangle(new Point(0, Panel_Environment.Height - (Panel_Environment.Height - EMatBmp.Height) / 2), new Size(Panel_Environment.Width, (Panel_Environment.Height - EMatBmp.Height) / 2)) });
                }

                e.Graphics.DrawImage(EMatBmp, EMatBmpRect);
            }
        }

        #endregion

        #region 元素矩阵扩展功能

        // 逻辑添加。

        private void ElementMatrix_LogicalAppend(Point A, Int32 E)
        {
            //
            // 向元素矩阵逻辑添加一个元素，并进行判定。A：索引；E：元素的值。
            //

            if (E != 0 && ElementMatrix_IndexValid(A))
            {
                if (!ElementIndexList.Contains(A))
                {
                    ElementMatrix_Add(A, E);

                    ElementMatrix_PresentAt(A);

                    if (StepTagNum > 0 && ElementIndexList.Count > StepTagNum)
                    {
                        ElementMatrix_PresentAt(ElementIndexList[ElementIndexList.Count - StepTagNum - 1]);
                    }

                    Judgement();
                }
            }
        }

        #endregion

        #region 棋局分析

        // 统计。

        private class StatsInfo // 统计信息。
        {
            public List<Point[]> Chain_FIR; // 成五链列表。
            public List<Point[]> Chain_Long; // 长连链列表。
            public List<Point[]> Chain_5; // 五连链列表。
            public List<Point[]> Chain_4A; // 活四链列表。
            public List<Point[]> Chain_4S; // 冲四链列表。
            public List<Point[]> Chain_3A; // 活三链列表。
            public List<Point[]> Chain_3S; // 眠三链列表。
            public List<Point[]> Chain_2A; // 活二链列表。
            public List<Point[]> Chain_2S; // 眠二链列表。

            public List<Point> CrossIDList_4_4; // 四-四链的交点索引列表。
            public List<Point> CrossIDList_4A_Any; // 活四-任意链的交点索引列表。
            public List<Point> CrossIDList_4A_4A; // 活四-活四链的交点索引列表。
            public List<Point> CrossIDList_4A_4S; // 活四-冲四链的交点索引列表。
            public List<Point> CrossIDList_4A_3A; // 活四-活三链的交点索引列表。
            public List<Point> CrossIDList_4A_3S; // 活四-眠三链的交点索引列表。
            public List<Point> CrossIDList_4A_2A; // 活四-活二链的交点索引列表。
            public List<Point> CrossIDList_4A_2S; // 活四-眠二链的交点索引列表。
            public List<Point> CrossIDList_4S_4S; // 冲四-冲四链的交点索引列表。
            public List<Point> CrossIDList_4S_3A; // 冲四-活三链的交点索引列表。
            public List<Point> CrossIDList_4S_3S; // 冲四-眠三链的交点索引列表。
            public List<Point> CrossIDList_4S_2A; // 冲四-活二链的交点索引列表。
            public List<Point> CrossIDList_4S_2S; // 冲四-眠二链的交点索引列表。
            public List<Point> CrossIDList_3A_3A; // 活三-活三链的交点索引列表。
            public List<Point> CrossIDList_3A_3S; // 活三-眠三链的交点索引列表。
            public List<Point> CrossIDList_3A_2A; // 活三-活二链的交点索引列表。
            public List<Point> CrossIDList_3A_2S; // 活三-眠二链的交点索引列表。
            public List<Point> CrossIDList_3S_3S; // 眠三-眠三链的交点索引列表。
            public List<Point> CrossIDList_3S_2A; // 眠三-活二链的交点索引列表。
            public List<Point> CrossIDList_3S_2S; // 眠三-眠二链的交点索引列表。
            public List<Point> CrossIDList_2A_2A; // 活二-活二链的交点索引列表。
            public List<Point> CrossIDList_2A_2S; // 活二-眠二链的交点索引列表。
            public List<Point> CrossIDList_2S_2S; // 眠二-眠二链的交点索引列表。
        }

        private StatsInfo GetStatisticalInformationOfArray(Int32[,] Array, Size Cap, Int32 Value)
        {
            //
            // 获取棋局以指定值为参数的统计信息。Array：矩阵，索引为 [x, y]；Cap：矩阵的大小，分量 (Width, Height) 分别表示沿 x 方向和沿 y 方向的元素数量；Value：指定值。
            //

            StatsInfo SI = new StatsInfo();

            SI.Chain_FIR = new List<Point[]>(0);
            SI.Chain_Long = new List<Point[]>(0);
            SI.Chain_5 = new List<Point[]>(0);
            SI.Chain_4A = new List<Point[]>(0);
            SI.Chain_4S = new List<Point[]>(0);
            SI.Chain_3A = new List<Point[]>(0);
            SI.Chain_3S = new List<Point[]>(0);
            SI.Chain_2A = new List<Point[]>(0);
            SI.Chain_2S = new List<Point[]>(0);

            SI.CrossIDList_4_4 = new List<Point>(0);
            SI.CrossIDList_4A_Any = new List<Point>(0);
            SI.CrossIDList_4A_4A = new List<Point>(0);
            SI.CrossIDList_4A_4S = new List<Point>(0);
            SI.CrossIDList_4A_3A = new List<Point>(0);
            SI.CrossIDList_4A_3S = new List<Point>(0);
            SI.CrossIDList_4A_2A = new List<Point>(0);
            SI.CrossIDList_4A_2S = new List<Point>(0);
            SI.CrossIDList_4S_4S = new List<Point>(0);
            SI.CrossIDList_4S_3A = new List<Point>(0);
            SI.CrossIDList_4S_3S = new List<Point>(0);
            SI.CrossIDList_4S_2A = new List<Point>(0);
            SI.CrossIDList_4S_2S = new List<Point>(0);
            SI.CrossIDList_3A_3A = new List<Point>(0);
            SI.CrossIDList_3A_3S = new List<Point>(0);
            SI.CrossIDList_3A_2A = new List<Point>(0);
            SI.CrossIDList_3A_2S = new List<Point>(0);
            SI.CrossIDList_3S_3S = new List<Point>(0);
            SI.CrossIDList_3S_2A = new List<Point>(0);
            SI.CrossIDList_3S_2S = new List<Point>(0);
            SI.CrossIDList_2A_2A = new List<Point>(0);
            SI.CrossIDList_2A_2S = new List<Point>(0);
            SI.CrossIDList_2S_2S = new List<Point>(0);

            try
            {
                // 长连、五连：

                for (int X = 0; X < Cap.Width; X++)
                {
                    List<Point> AL = new List<Point>(Cap.Height);

                    for (int Y = 0; Y < Cap.Height; Y++)
                    {
                        Point A = new Point(X, Y);
                        Int32 E = Array[A.X, A.Y];

                        if (E == Value)
                        {
                            AL.Add(A);
                        }
                        else
                        {
                            if (AL.Count > 5)
                            {
                                SI.Chain_FIR.Add(AL.ToArray());
                                SI.Chain_Long.Add(AL.ToArray());
                            }
                            else if (AL.Count == 5)
                            {
                                SI.Chain_FIR.Add(AL.ToArray());
                                SI.Chain_5.Add(AL.ToArray());
                            }

                            AL.Clear();
                        }
                    }

                    if (AL.Count > 5)
                    {
                        SI.Chain_FIR.Add(AL.ToArray());
                        SI.Chain_Long.Add(AL.ToArray());
                    }
                    else if (AL.Count == 5)
                    {
                        SI.Chain_FIR.Add(AL.ToArray());
                        SI.Chain_5.Add(AL.ToArray());
                    }
                }

                for (int Y = 0; Y < Cap.Height; Y++)
                {
                    List<Point> AL = new List<Point>(Cap.Width);

                    for (int X = 0; X < Cap.Width; X++)
                    {
                        Point A = new Point(X, Y);
                        Int32 E = Array[A.X, A.Y];

                        if (E == Value)
                        {
                            AL.Add(A);
                        }
                        else
                        {
                            if (AL.Count > 5)
                            {
                                SI.Chain_FIR.Add(AL.ToArray());
                                SI.Chain_Long.Add(AL.ToArray());
                            }
                            else if (AL.Count == 5)
                            {
                                SI.Chain_FIR.Add(AL.ToArray());
                                SI.Chain_5.Add(AL.ToArray());
                            }

                            AL.Clear();
                        }
                    }

                    if (AL.Count > 5)
                    {
                        SI.Chain_FIR.Add(AL.ToArray());
                        SI.Chain_Long.Add(AL.ToArray());
                    }
                    else if (AL.Count == 5)
                    {
                        SI.Chain_FIR.Add(AL.ToArray());
                        SI.Chain_5.Add(AL.ToArray());
                    }
                }

                for (int i = 0; i <= 2 * (Cap.Width - 1); i++)
                {
                    List<Point> AL = new List<Point>(2 * Cap.Width);

                    for (int X = (i < Cap.Width ? 0 : i - (Cap.Width - 1)); X <= (i < Cap.Width ? i : Cap.Width - 1); X++)
                    {
                        int Y = i - X;

                        Point A = new Point(X, Y);
                        Int32 E = Array[A.X, A.Y];

                        if (E == Value)
                        {
                            AL.Add(A);
                        }
                        else
                        {
                            if (AL.Count > 5)
                            {
                                SI.Chain_FIR.Add(AL.ToArray());
                                SI.Chain_Long.Add(AL.ToArray());
                            }
                            else if (AL.Count == 5)
                            {
                                SI.Chain_FIR.Add(AL.ToArray());
                                SI.Chain_5.Add(AL.ToArray());
                            }

                            AL.Clear();
                        }
                    }

                    if (AL.Count > 5)
                    {
                        SI.Chain_FIR.Add(AL.ToArray());
                        SI.Chain_Long.Add(AL.ToArray());
                    }
                    else if (AL.Count == 5)
                    {
                        SI.Chain_FIR.Add(AL.ToArray());
                        SI.Chain_5.Add(AL.ToArray());
                    }
                }

                for (int i = 0; i <= 2 * (Cap.Width - 1); i++)
                {
                    List<Point> AL = new List<Point>(2 * Cap.Width);

                    for (int X = (i < Cap.Width ? 0 : i - (Cap.Width - 1)); X <= (i < Cap.Width ? i : Cap.Width - 1); X++)
                    {
                        int Y = X + (Cap.Width - 1 - i);

                        Point A = new Point(X, Y);
                        Int32 E = Array[A.X, A.Y];

                        if (E == Value)
                        {
                            AL.Add(A);
                        }
                        else
                        {
                            if (AL.Count > 5)
                            {
                                SI.Chain_FIR.Add(AL.ToArray());
                                SI.Chain_Long.Add(AL.ToArray());
                            }
                            else if (AL.Count == 5)
                            {
                                SI.Chain_FIR.Add(AL.ToArray());
                                SI.Chain_5.Add(AL.ToArray());
                            }

                            AL.Clear();
                        }
                    }

                    if (AL.Count > 5)
                    {
                        SI.Chain_FIR.Add(AL.ToArray());
                        SI.Chain_Long.Add(AL.ToArray());
                    }
                    else if (AL.Count == 5)
                    {
                        SI.Chain_FIR.Add(AL.ToArray());
                        SI.Chain_5.Add(AL.ToArray());
                    }
                }

                // 活四、冲四、活三、眠三、活二、眠二：

                for (int X = 0; X < Cap.Width; X++)
                {
                    for (int Y = 4; Y < Cap.Height; Y++)
                    {
                        Point[] IDAry = new Point[] { new Point(X, Y), new Point(X, Y - 1), new Point(X, Y - 2), new Point(X, Y - 3), new Point(X, Y - 4) };
                        Point ArrayHead = new Point(X, Y + 1), ArrayTail = new Point(X, Y - 5);
                        Point ChainHead = new Point(X, Y + 1), ChainTail = new Point(X, Y - 5);

                        Int32 ZeroCntOut = 0, ZeroCntIn = 0, ValueCnt = 0, OtherCnt = 0;

                        for (int n = 0; n < IDAry.Length; n++)
                        {
                            Int32 E = Array[IDAry[n].X, IDAry[n].Y];

                            if (E == 0)
                            {
                                Int32 Wall = 0;

                                for (int p = n - 1; p >= 0; p--)
                                {
                                    if (Array[IDAry[p].X, IDAry[p].Y] != 0)
                                    {
                                        Wall++;

                                        break;
                                    }
                                }

                                for (int q = n + 1; q < IDAry.Length; q++)
                                {
                                    if (Array[IDAry[q].X, IDAry[q].Y] != 0)
                                    {
                                        Wall++;

                                        break;
                                    }
                                }

                                if (Wall <= 1)
                                {
                                    ZeroCntOut++;
                                }
                                else
                                {
                                    ZeroCntIn++;
                                }
                            }
                            else if (E == Value)
                            {
                                ValueCnt++;
                            }
                            else
                            {
                                OtherCnt++;
                            }
                        }

                        if (ZeroCntIn <= 1 && OtherCnt == 0)
                        {
                            List<Point> AL = new List<Point>(IDAry.Length);

                            foreach (Point A in IDAry)
                            {
                                if (Array[A.X, A.Y] == Value)
                                {
                                    AL.Add(A);
                                }
                            }

                            for (int p = 0; p < IDAry.Length; p++)
                            {
                                if (Array[IDAry[p].X, IDAry[p].Y] == Value)
                                {
                                    if (p > 0)
                                    {
                                        ChainHead = IDAry[p - 1];
                                    }

                                    break;
                                }
                            }

                            for (int q = IDAry.Length - 1; q >= 1; q--)
                            {
                                if (Array[IDAry[q].X, IDAry[q].Y] == Value)
                                {
                                    if (q < IDAry.Length - 1)
                                    {
                                        ChainTail = IDAry[q + 1];
                                    }

                                    break;
                                }
                            }

                            Int32 ArrayAliveValue = 0, ChainAliveValue = 0;

                            if ((ArrayHead.X >= 0 && ArrayHead.X < Cap.Width && ArrayHead.Y >= 0 && ArrayHead.Y < Cap.Height) && Array[ArrayHead.X, ArrayHead.Y] == 0)
                            {
                                ArrayAliveValue++;
                            }

                            if ((ArrayTail.X >= 0 && ArrayTail.X < Cap.Width && ArrayTail.Y >= 0 && ArrayTail.Y < Cap.Height) && Array[ArrayTail.X, ArrayTail.Y] == 0)
                            {
                                ArrayAliveValue++;
                            }

                            if ((ChainHead.X >= 0 && ChainHead.X < Cap.Width && ChainHead.Y >= 0 && ChainHead.Y < Cap.Height) && Array[ChainHead.X, ChainHead.Y] == 0)
                            {
                                ChainAliveValue++;
                            }

                            if ((ChainTail.X >= 0 && ChainTail.X < Cap.Width && ChainTail.Y >= 0 && ChainTail.Y < Cap.Height) && Array[ChainTail.X, ChainTail.Y] == 0)
                            {
                                ChainAliveValue++;
                            }

                            if (ValueCnt == 4)
                            {
                                if (ZeroCntIn == 0 && ChainAliveValue >= 2)
                                {
                                    SI.Chain_4A.Add(AL.ToArray());
                                }
                                else
                                {
                                    SI.Chain_4S.Add(AL.ToArray());
                                }
                            }
                            else if (ValueCnt == 3)
                            {
                                if (ArrayAliveValue >= 1 && ChainAliveValue == 2)
                                {
                                    SI.Chain_3A.Add(AL.ToArray());
                                }
                                else
                                {
                                    SI.Chain_3S.Add(AL.ToArray());
                                }
                            }
                            else if (ValueCnt == 2)
                            {
                                if (ArrayAliveValue >= 1 && ChainAliveValue == 2)
                                {
                                    SI.Chain_2A.Add(AL.ToArray());
                                }
                                else
                                {
                                    SI.Chain_2S.Add(AL.ToArray());
                                }
                            }
                        }
                    }
                }

                for (int Y = 0; Y < Cap.Height; Y++)
                {
                    for (int X = 4; X < Cap.Width; X++)
                    {
                        Point[] IDAry = new Point[] { new Point(X, Y), new Point(X - 1, Y), new Point(X - 2, Y), new Point(X - 3, Y), new Point(X - 4, Y) };
                        Point ArrayHead = new Point(X + 1, Y), ArrayTail = new Point(X - 5, Y);
                        Point ChainHead = new Point(X + 1, Y), ChainTail = new Point(X - 5, Y);

                        Int32 ZeroCntOut = 0, ZeroCntIn = 0, ValueCnt = 0, OtherCnt = 0;

                        for (int n = 0; n < IDAry.Length; n++)
                        {
                            Int32 E = Array[IDAry[n].X, IDAry[n].Y];

                            if (E == 0)
                            {
                                Int32 Wall = 0;

                                for (int p = n - 1; p >= 0; p--)
                                {
                                    if (Array[IDAry[p].X, IDAry[p].Y] != 0)
                                    {
                                        Wall++;

                                        break;
                                    }
                                }

                                for (int q = n + 1; q < IDAry.Length; q++)
                                {
                                    if (Array[IDAry[q].X, IDAry[q].Y] != 0)
                                    {
                                        Wall++;

                                        break;
                                    }
                                }

                                if (Wall <= 1)
                                {
                                    ZeroCntOut++;
                                }
                                else
                                {
                                    ZeroCntIn++;
                                }
                            }
                            else if (E == Value)
                            {
                                ValueCnt++;
                            }
                            else
                            {
                                OtherCnt++;
                            }
                        }

                        if (ZeroCntIn <= 1 && OtherCnt == 0)
                        {
                            List<Point> AL = new List<Point>(IDAry.Length);

                            foreach (Point A in IDAry)
                            {
                                if (Array[A.X, A.Y] == Value)
                                {
                                    AL.Add(A);
                                }
                            }

                            for (int p = 0; p < IDAry.Length; p++)
                            {
                                if (Array[IDAry[p].X, IDAry[p].Y] == Value)
                                {
                                    if (p > 0)
                                    {
                                        ChainHead = IDAry[p - 1];
                                    }

                                    break;
                                }
                            }

                            for (int q = IDAry.Length - 1; q >= 1; q--)
                            {
                                if (Array[IDAry[q].X, IDAry[q].Y] == Value)
                                {
                                    if (q < IDAry.Length - 1)
                                    {
                                        ChainTail = IDAry[q + 1];
                                    }

                                    break;
                                }
                            }

                            Int32 ArrayAliveValue = 0, ChainAliveValue = 0;

                            if ((ArrayHead.X >= 0 && ArrayHead.X < Cap.Width && ArrayHead.Y >= 0 && ArrayHead.Y < Cap.Height) && Array[ArrayHead.X, ArrayHead.Y] == 0)
                            {
                                ArrayAliveValue++;
                            }

                            if ((ArrayTail.X >= 0 && ArrayTail.X < Cap.Width && ArrayTail.Y >= 0 && ArrayTail.Y < Cap.Height) && Array[ArrayTail.X, ArrayTail.Y] == 0)
                            {
                                ArrayAliveValue++;
                            }

                            if ((ChainHead.X >= 0 && ChainHead.X < Cap.Width && ChainHead.Y >= 0 && ChainHead.Y < Cap.Height) && Array[ChainHead.X, ChainHead.Y] == 0)
                            {
                                ChainAliveValue++;
                            }

                            if ((ChainTail.X >= 0 && ChainTail.X < Cap.Width && ChainTail.Y >= 0 && ChainTail.Y < Cap.Height) && Array[ChainTail.X, ChainTail.Y] == 0)
                            {
                                ChainAliveValue++;
                            }

                            if (ValueCnt == 4)
                            {
                                if (ZeroCntIn == 0 && ChainAliveValue >= 2)
                                {
                                    SI.Chain_4A.Add(AL.ToArray());
                                }
                                else
                                {
                                    SI.Chain_4S.Add(AL.ToArray());
                                }
                            }
                            else if (ValueCnt == 3)
                            {
                                if (ArrayAliveValue >= 1 && ChainAliveValue == 2)
                                {
                                    SI.Chain_3A.Add(AL.ToArray());
                                }
                                else
                                {
                                    SI.Chain_3S.Add(AL.ToArray());
                                }
                            }
                            else if (ValueCnt == 2)
                            {
                                if (ArrayAliveValue >= 1 && ChainAliveValue == 2)
                                {
                                    SI.Chain_2A.Add(AL.ToArray());
                                }
                                else
                                {
                                    SI.Chain_2S.Add(AL.ToArray());
                                }
                            }
                        }
                    }
                }

                for (int i = 0; i <= 2 * (Cap.Width - 1); i++)
                {
                    for (int X = (i < Cap.Width ? 4 : i + 4 - (Cap.Width - 1)); X <= (i < Cap.Width ? i : Cap.Width - 1); X++)
                    {
                        int Y = i - X;

                        Point[] IDAry = new Point[] { new Point(X, Y), new Point(X - 1, Y + 1), new Point(X - 2, Y + 2), new Point(X - 3, Y + 3), new Point(X - 4, Y + 4) };
                        Point ArrayHead = new Point(X + 1, Y - 1), ArrayTail = new Point(X - 5, Y + 5);
                        Point ChainHead = new Point(X + 1, Y - 1), ChainTail = new Point(X - 5, Y + 5);

                        Int32 ZeroCntOut = 0, ZeroCntIn = 0, ValueCnt = 0, OtherCnt = 0;

                        for (int n = 0; n < IDAry.Length; n++)
                        {
                            Int32 E = Array[IDAry[n].X, IDAry[n].Y];

                            if (E == 0)
                            {
                                Int32 Wall = 0;

                                for (int p = n - 1; p >= 0; p--)
                                {
                                    if (Array[IDAry[p].X, IDAry[p].Y] != 0)
                                    {
                                        Wall++;

                                        break;
                                    }
                                }

                                for (int q = n + 1; q < IDAry.Length; q++)
                                {
                                    if (Array[IDAry[q].X, IDAry[q].Y] != 0)
                                    {
                                        Wall++;

                                        break;
                                    }
                                }

                                if (Wall <= 1)
                                {
                                    ZeroCntOut++;
                                }
                                else
                                {
                                    ZeroCntIn++;
                                }
                            }
                            else if (E == Value)
                            {
                                ValueCnt++;
                            }
                            else
                            {
                                OtherCnt++;
                            }
                        }

                        if (ZeroCntIn <= 1 && OtherCnt == 0)
                        {
                            List<Point> AL = new List<Point>(IDAry.Length);

                            foreach (Point A in IDAry)
                            {
                                if (Array[A.X, A.Y] == Value)
                                {
                                    AL.Add(A);
                                }
                            }

                            for (int p = 0; p < IDAry.Length; p++)
                            {
                                if (Array[IDAry[p].X, IDAry[p].Y] == Value)
                                {
                                    if (p > 0)
                                    {
                                        ChainHead = IDAry[p - 1];
                                    }

                                    break;
                                }
                            }

                            for (int q = IDAry.Length - 1; q >= 1; q--)
                            {
                                if (Array[IDAry[q].X, IDAry[q].Y] == Value)
                                {
                                    if (q < IDAry.Length - 1)
                                    {
                                        ChainTail = IDAry[q + 1];
                                    }

                                    break;
                                }
                            }

                            Int32 ArrayAliveValue = 0, ChainAliveValue = 0;

                            if ((ArrayHead.X >= 0 && ArrayHead.X < Cap.Width && ArrayHead.Y >= 0 && ArrayHead.Y < Cap.Height) && Array[ArrayHead.X, ArrayHead.Y] == 0)
                            {
                                ArrayAliveValue++;
                            }

                            if ((ArrayTail.X >= 0 && ArrayTail.X < Cap.Width && ArrayTail.Y >= 0 && ArrayTail.Y < Cap.Height) && Array[ArrayTail.X, ArrayTail.Y] == 0)
                            {
                                ArrayAliveValue++;
                            }

                            if ((ChainHead.X >= 0 && ChainHead.X < Cap.Width && ChainHead.Y >= 0 && ChainHead.Y < Cap.Height) && Array[ChainHead.X, ChainHead.Y] == 0)
                            {
                                ChainAliveValue++;
                            }

                            if ((ChainTail.X >= 0 && ChainTail.X < Cap.Width && ChainTail.Y >= 0 && ChainTail.Y < Cap.Height) && Array[ChainTail.X, ChainTail.Y] == 0)
                            {
                                ChainAliveValue++;
                            }

                            if (ValueCnt == 4)
                            {
                                if (ZeroCntIn == 0 && ChainAliveValue >= 2)
                                {
                                    SI.Chain_4A.Add(AL.ToArray());
                                }
                                else
                                {
                                    SI.Chain_4S.Add(AL.ToArray());
                                }
                            }
                            else if (ValueCnt == 3)
                            {
                                if (ArrayAliveValue >= 1 && ChainAliveValue == 2)
                                {
                                    SI.Chain_3A.Add(AL.ToArray());
                                }
                                else
                                {
                                    SI.Chain_3S.Add(AL.ToArray());
                                }
                            }
                            else if (ValueCnt == 2)
                            {
                                if (ArrayAliveValue >= 1 && ChainAliveValue == 2)
                                {
                                    SI.Chain_2A.Add(AL.ToArray());
                                }
                                else
                                {
                                    SI.Chain_2S.Add(AL.ToArray());
                                }
                            }
                        }
                    }
                }

                for (int i = 0; i <= 2 * (Cap.Width - 1); i++)
                {
                    for (int X = (i < Cap.Width ? 4 : i + 4 - (Cap.Width - 1)); X <= (i < Cap.Width ? i : Cap.Width - 1); X++)
                    {
                        int Y = X + (Cap.Width - 1 - i);

                        Point[] IDAry = new Point[] { new Point(X, Y), new Point(X - 1, Y - 1), new Point(X - 2, Y - 2), new Point(X - 3, Y - 3), new Point(X - 4, Y - 4) };
                        Point ArrayHead = new Point(X + 1, Y + 1), ArrayTail = new Point(X - 5, Y - 5);
                        Point ChainHead = new Point(X + 1, Y + 1), ChainTail = new Point(X - 5, Y - 5);

                        Int32 ZeroCntOut = 0, ZeroCntIn = 0, ValueCnt = 0, OtherCnt = 0;

                        for (int n = 0; n < IDAry.Length; n++)
                        {
                            Int32 E = Array[IDAry[n].X, IDAry[n].Y];

                            if (E == 0)
                            {
                                Int32 Wall = 0;

                                for (int p = n - 1; p >= 0; p--)
                                {
                                    if (Array[IDAry[p].X, IDAry[p].Y] != 0)
                                    {
                                        Wall++;

                                        break;
                                    }
                                }

                                for (int q = n + 1; q < IDAry.Length; q++)
                                {
                                    if (Array[IDAry[q].X, IDAry[q].Y] != 0)
                                    {
                                        Wall++;

                                        break;
                                    }
                                }

                                if (Wall <= 1)
                                {
                                    ZeroCntOut++;
                                }
                                else
                                {
                                    ZeroCntIn++;
                                }
                            }
                            else if (E == Value)
                            {
                                ValueCnt++;
                            }
                            else
                            {
                                OtherCnt++;
                            }
                        }

                        if (ZeroCntIn <= 1 && OtherCnt == 0)
                        {
                            List<Point> AL = new List<Point>(IDAry.Length);

                            foreach (Point A in IDAry)
                            {
                                if (Array[A.X, A.Y] == Value)
                                {
                                    AL.Add(A);
                                }
                            }

                            for (int p = 0; p < IDAry.Length; p++)
                            {
                                if (Array[IDAry[p].X, IDAry[p].Y] == Value)
                                {
                                    if (p > 0)
                                    {
                                        ChainHead = IDAry[p - 1];
                                    }

                                    break;
                                }
                            }

                            for (int q = IDAry.Length - 1; q >= 1; q--)
                            {
                                if (Array[IDAry[q].X, IDAry[q].Y] == Value)
                                {
                                    if (q < IDAry.Length - 1)
                                    {
                                        ChainTail = IDAry[q + 1];
                                    }

                                    break;
                                }
                            }

                            Int32 ArrayAliveValue = 0, ChainAliveValue = 0;

                            if ((ArrayHead.X >= 0 && ArrayHead.X < Cap.Width && ArrayHead.Y >= 0 && ArrayHead.Y < Cap.Height) && Array[ArrayHead.X, ArrayHead.Y] == 0)
                            {
                                ArrayAliveValue++;
                            }

                            if ((ArrayTail.X >= 0 && ArrayTail.X < Cap.Width && ArrayTail.Y >= 0 && ArrayTail.Y < Cap.Height) && Array[ArrayTail.X, ArrayTail.Y] == 0)
                            {
                                ArrayAliveValue++;
                            }

                            if ((ChainHead.X >= 0 && ChainHead.X < Cap.Width && ChainHead.Y >= 0 && ChainHead.Y < Cap.Height) && Array[ChainHead.X, ChainHead.Y] == 0)
                            {
                                ChainAliveValue++;
                            }

                            if ((ChainTail.X >= 0 && ChainTail.X < Cap.Width && ChainTail.Y >= 0 && ChainTail.Y < Cap.Height) && Array[ChainTail.X, ChainTail.Y] == 0)
                            {
                                ChainAliveValue++;
                            }

                            if (ValueCnt == 4)
                            {
                                if (ZeroCntIn == 0 && ChainAliveValue >= 2)
                                {
                                    SI.Chain_4A.Add(AL.ToArray());
                                }
                                else
                                {
                                    SI.Chain_4S.Add(AL.ToArray());
                                }
                            }
                            else if (ValueCnt == 3)
                            {
                                if (ArrayAliveValue >= 1 && ChainAliveValue == 2)
                                {
                                    SI.Chain_3A.Add(AL.ToArray());
                                }
                                else
                                {
                                    SI.Chain_3S.Add(AL.ToArray());
                                }
                            }
                            else if (ValueCnt == 2)
                            {
                                if (ArrayAliveValue >= 1 && ChainAliveValue == 2)
                                {
                                    SI.Chain_2A.Add(AL.ToArray());
                                }
                                else
                                {
                                    SI.Chain_2S.Add(AL.ToArray());
                                }
                            }
                        }
                    }
                }

                // 去重：

                for (int i = 0; i < SI.Chain_4A.Count - 1; i++)
                {
                    for (int j = i + 1; j < SI.Chain_4A.Count; j++)
                    {
                        Int32 Cnt = 0;

                        foreach (Point A in SI.Chain_4A[i])
                        {
                            if (SI.Chain_4A[j].Contains(A))
                            {
                                Cnt++;
                            }
                        }

                        if (Cnt == SI.Chain_4A[i].Length)
                        {
                            SI.Chain_4A.RemoveAt(i);

                            i--;

                            break;
                        }
                    }
                }

                //

                for (int i = 0; i < SI.Chain_4S.Count - 1; i++)
                {
                    for (int j = i + 1; j < SI.Chain_4S.Count; j++)
                    {
                        Int32 Cnt = 0;

                        foreach (Point A in SI.Chain_4S[i])
                        {
                            if (SI.Chain_4S[j].Contains(A))
                            {
                                Cnt++;
                            }
                        }

                        if (Cnt == SI.Chain_4S[i].Length)
                        {
                            SI.Chain_4S.RemoveAt(i);

                            i--;

                            break;
                        }
                    }
                }

                for (int i = 0; i < SI.Chain_4S.Count; i++)
                {
                    for (int j = 0; j < SI.Chain_Long.Count; j++)
                    {
                        Int32 Cnt = 0;

                        foreach (Point A in SI.Chain_4S[i])
                        {
                            if (SI.Chain_Long[j].Contains(A))
                            {
                                Cnt++;
                            }
                        }

                        if (Cnt == SI.Chain_4S[i].Length)
                        {
                            SI.Chain_4S.RemoveAt(i);

                            i--;

                            break;
                        }
                    }
                }

                for (int i = 0; i < SI.Chain_4S.Count; i++)
                {
                    for (int j = 0; j < SI.Chain_5.Count; j++)
                    {
                        Int32 Cnt = 0;

                        foreach (Point A in SI.Chain_4S[i])
                        {
                            if (SI.Chain_5[j].Contains(A))
                            {
                                Cnt++;
                            }
                        }

                        if (Cnt == SI.Chain_4S[i].Length)
                        {
                            SI.Chain_4S.RemoveAt(i);

                            i--;

                            break;
                        }
                    }
                }

                //

                for (int i = 0; i < SI.Chain_3A.Count - 1; i++)
                {
                    for (int j = i + 1; j < SI.Chain_3A.Count; j++)
                    {
                        Int32 Cnt = 0;

                        foreach (Point A in SI.Chain_3A[i])
                        {
                            if (SI.Chain_3A[j].Contains(A))
                            {
                                Cnt++;
                            }
                        }

                        if (Cnt == SI.Chain_3A[i].Length)
                        {
                            SI.Chain_3A.RemoveAt(i);

                            i--;

                            break;
                        }
                    }
                }

                for (int i = 0; i < SI.Chain_3A.Count; i++)
                {
                    for (int j = 0; j < SI.Chain_4S.Count; j++)
                    {
                        Int32 Cnt = 0;

                        foreach (Point A in SI.Chain_3A[i])
                        {
                            if (SI.Chain_4S[j].Contains(A))
                            {
                                Cnt++;
                            }
                        }

                        if (Cnt == SI.Chain_3A[i].Length)
                        {
                            SI.Chain_3A.RemoveAt(i);

                            i--;

                            break;
                        }
                    }
                }

                //

                for (int i = 0; i < SI.Chain_3S.Count - 1; i++)
                {
                    for (int j = i + 1; j < SI.Chain_3S.Count; j++)
                    {
                        Int32 Cnt = 0;

                        foreach (Point A in SI.Chain_3S[i])
                        {
                            if (SI.Chain_3S[j].Contains(A))
                            {
                                Cnt++;
                            }
                        }

                        if (Cnt == SI.Chain_3S[i].Length)
                        {
                            SI.Chain_3S.RemoveAt(i);

                            i--;

                            break;
                        }
                    }
                }

                for (int i = 0; i < SI.Chain_3S.Count; i++)
                {
                    for (int j = 0; j < SI.Chain_Long.Count; j++)
                    {
                        Int32 Cnt = 0;

                        foreach (Point A in SI.Chain_3S[i])
                        {
                            if (SI.Chain_Long[j].Contains(A))
                            {
                                Cnt++;
                            }
                        }

                        if (Cnt == SI.Chain_3S[i].Length)
                        {
                            SI.Chain_3S.RemoveAt(i);

                            i--;

                            break;
                        }
                    }
                }

                for (int i = 0; i < SI.Chain_3S.Count; i++)
                {
                    for (int j = 0; j < SI.Chain_5.Count; j++)
                    {
                        Int32 Cnt = 0;

                        foreach (Point A in SI.Chain_3S[i])
                        {
                            if (SI.Chain_5[j].Contains(A))
                            {
                                Cnt++;
                            }
                        }

                        if (Cnt == SI.Chain_3S[i].Length)
                        {
                            SI.Chain_3S.RemoveAt(i);

                            i--;

                            break;
                        }
                    }
                }

                for (int i = 0; i < SI.Chain_3S.Count; i++)
                {
                    for (int j = 0; j < SI.Chain_4A.Count; j++)
                    {
                        Int32 Cnt = 0;

                        foreach (Point A in SI.Chain_3S[i])
                        {
                            if (SI.Chain_4A[j].Contains(A))
                            {
                                Cnt++;
                            }
                        }

                        if (Cnt == SI.Chain_3S[i].Length)
                        {
                            SI.Chain_3S.RemoveAt(i);

                            i--;

                            break;
                        }
                    }
                }

                for (int i = 0; i < SI.Chain_3S.Count; i++)
                {
                    for (int j = 0; j < SI.Chain_4S.Count; j++)
                    {
                        Int32 Cnt = 0;

                        foreach (Point A in SI.Chain_3S[i])
                        {
                            if (SI.Chain_4S[j].Contains(A))
                            {
                                Cnt++;
                            }
                        }

                        if (Cnt == SI.Chain_3S[i].Length)
                        {
                            SI.Chain_3S.RemoveAt(i);

                            i--;

                            break;
                        }
                    }
                }

                //

                for (int i = 0; i < SI.Chain_2A.Count - 1; i++)
                {
                    for (int j = i + 1; j < SI.Chain_2A.Count; j++)
                    {
                        Int32 Cnt = 0;

                        foreach (Point A in SI.Chain_2A[i])
                        {
                            if (SI.Chain_2A[j].Contains(A))
                            {
                                Cnt++;
                            }
                        }

                        if (Cnt == SI.Chain_2A[i].Length)
                        {
                            SI.Chain_2A.RemoveAt(i);

                            i--;

                            break;
                        }
                    }
                }

                for (int i = 0; i < SI.Chain_2A.Count; i++)
                {
                    for (int j = 0; j < SI.Chain_4S.Count; j++)
                    {
                        Int32 Cnt = 0;

                        foreach (Point A in SI.Chain_2A[i])
                        {
                            if (SI.Chain_4S[j].Contains(A))
                            {
                                Cnt++;
                            }
                        }

                        if (Cnt == SI.Chain_2A[i].Length)
                        {
                            SI.Chain_2A.RemoveAt(i);

                            i--;

                            break;
                        }
                    }
                }

                for (int i = 0; i < SI.Chain_2A.Count; i++)
                {
                    for (int j = 0; j < SI.Chain_3A.Count; j++)
                    {
                        Int32 Cnt = 0;

                        foreach (Point A in SI.Chain_2A[i])
                        {
                            if (SI.Chain_3A[j].Contains(A))
                            {
                                Cnt++;
                            }
                        }

                        if (Cnt == SI.Chain_2A[i].Length)
                        {
                            SI.Chain_2A.RemoveAt(i);

                            i--;

                            break;
                        }
                    }
                }

                for (int i = 0; i < SI.Chain_2A.Count; i++)
                {
                    for (int j = 0; j < SI.Chain_3S.Count; j++)
                    {
                        Int32 Cnt = 0;

                        foreach (Point A in SI.Chain_2A[i])
                        {
                            if (SI.Chain_3S[j].Contains(A))
                            {
                                Cnt++;
                            }
                        }

                        if (Cnt == SI.Chain_2A[i].Length)
                        {
                            SI.Chain_2A.RemoveAt(i);

                            i--;

                            break;
                        }
                    }
                }

                //

                for (int i = 0; i < SI.Chain_2S.Count - 1; i++)
                {
                    for (int j = i + 1; j < SI.Chain_2S.Count; j++)
                    {
                        Int32 Cnt = 0;

                        foreach (Point A in SI.Chain_2S[i])
                        {
                            if (SI.Chain_2S[j].Contains(A))
                            {
                                Cnt++;
                            }
                        }

                        if (Cnt == SI.Chain_2S[i].Length)
                        {
                            SI.Chain_2S.RemoveAt(i);

                            i--;

                            break;
                        }
                    }
                }

                for (int i = 0; i < SI.Chain_2S.Count; i++)
                {
                    for (int j = 0; j < SI.Chain_Long.Count; j++)
                    {
                        Int32 Cnt = 0;

                        foreach (Point A in SI.Chain_2S[i])
                        {
                            if (SI.Chain_Long[j].Contains(A))
                            {
                                Cnt++;
                            }
                        }

                        if (Cnt == SI.Chain_2S[i].Length)
                        {
                            SI.Chain_2S.RemoveAt(i);

                            i--;

                            break;
                        }
                    }
                }

                for (int i = 0; i < SI.Chain_2S.Count; i++)
                {
                    for (int j = 0; j < SI.Chain_5.Count; j++)
                    {
                        Int32 Cnt = 0;

                        foreach (Point A in SI.Chain_2S[i])
                        {
                            if (SI.Chain_5[j].Contains(A))
                            {
                                Cnt++;
                            }
                        }

                        if (Cnt == SI.Chain_2S[i].Length)
                        {
                            SI.Chain_2S.RemoveAt(i);

                            i--;

                            break;
                        }
                    }
                }

                for (int i = 0; i < SI.Chain_2S.Count; i++)
                {
                    for (int j = 0; j < SI.Chain_4A.Count; j++)
                    {
                        Int32 Cnt = 0;

                        foreach (Point A in SI.Chain_2S[i])
                        {
                            if (SI.Chain_4A[j].Contains(A))
                            {
                                Cnt++;
                            }
                        }

                        if (Cnt == SI.Chain_2S[i].Length)
                        {
                            SI.Chain_2S.RemoveAt(i);

                            i--;

                            break;
                        }
                    }
                }

                for (int i = 0; i < SI.Chain_2S.Count; i++)
                {
                    for (int j = 0; j < SI.Chain_4S.Count; j++)
                    {
                        Int32 Cnt = 0;

                        foreach (Point A in SI.Chain_2S[i])
                        {
                            if (SI.Chain_4S[j].Contains(A))
                            {
                                Cnt++;
                            }
                        }

                        if (Cnt == SI.Chain_2S[i].Length)
                        {
                            SI.Chain_2S.RemoveAt(i);

                            i--;

                            break;
                        }
                    }
                }

                for (int i = 0; i < SI.Chain_2S.Count; i++)
                {
                    for (int j = 0; j < SI.Chain_3A.Count; j++)
                    {
                        Int32 Cnt = 0;

                        foreach (Point A in SI.Chain_2S[i])
                        {
                            if (SI.Chain_3A[j].Contains(A))
                            {
                                Cnt++;
                            }
                        }

                        if (Cnt == SI.Chain_2S[i].Length)
                        {
                            SI.Chain_2S.RemoveAt(i);

                            i--;

                            break;
                        }
                    }
                }

                for (int i = 0; i < SI.Chain_2S.Count; i++)
                {
                    for (int j = 0; j < SI.Chain_3S.Count; j++)
                    {
                        Int32 Cnt = 0;

                        foreach (Point A in SI.Chain_2S[i])
                        {
                            if (SI.Chain_3S[j].Contains(A))
                            {
                                Cnt++;
                            }
                        }

                        if (Cnt == SI.Chain_2S[i].Length)
                        {
                            SI.Chain_2S.RemoveAt(i);

                            i--;

                            break;
                        }
                    }
                }

                // 交点：

                for (int i = 0; i < SI.Chain_4A.Count - 1; i++)
                {
                    for (int j = i + 1; j < SI.Chain_4A.Count; j++)
                    {
                        foreach (Point A in SI.Chain_4A[i])
                        {
                            if (SI.Chain_4A[j].Contains(A))
                            {
                                SI.CrossIDList_4_4.Add(A);
                                SI.CrossIDList_4A_Any.Add(A);
                                SI.CrossIDList_4A_4A.Add(A);
                            }
                        }
                    }
                }

                for (int i = 0; i < SI.Chain_4A.Count; i++)
                {
                    for (int j = 0; j < SI.Chain_4S.Count; j++)
                    {
                        foreach (Point A in SI.Chain_4A[i])
                        {
                            if (SI.Chain_4S[j].Contains(A))
                            {
                                SI.CrossIDList_4_4.Add(A);
                                SI.CrossIDList_4A_Any.Add(A);
                                SI.CrossIDList_4A_4S.Add(A);
                            }
                        }
                    }
                }

                for (int i = 0; i < SI.Chain_4A.Count; i++)
                {
                    for (int j = 0; j < SI.Chain_3A.Count; j++)
                    {
                        foreach (Point A in SI.Chain_4A[i])
                        {
                            if (SI.Chain_3A[j].Contains(A))
                            {
                                SI.CrossIDList_4A_Any.Add(A);
                                SI.CrossIDList_4A_3A.Add(A);
                            }
                        }
                    }
                }

                for (int i = 0; i < SI.Chain_4A.Count; i++)
                {
                    for (int j = 0; j < SI.Chain_3S.Count; j++)
                    {
                        foreach (Point A in SI.Chain_4A[i])
                        {
                            if (SI.Chain_3S[j].Contains(A))
                            {
                                SI.CrossIDList_4A_Any.Add(A);
                                SI.CrossIDList_4A_3S.Add(A);
                            }
                        }
                    }
                }

                for (int i = 0; i < SI.Chain_4A.Count; i++)
                {
                    for (int j = 0; j < SI.Chain_2A.Count; j++)
                    {
                        foreach (Point A in SI.Chain_4A[i])
                        {
                            if (SI.Chain_2A[j].Contains(A))
                            {
                                SI.CrossIDList_4A_Any.Add(A);
                                SI.CrossIDList_4A_2A.Add(A);
                            }
                        }
                    }
                }

                for (int i = 0; i < SI.Chain_4A.Count; i++)
                {
                    for (int j = 0; j < SI.Chain_2S.Count; j++)
                    {
                        foreach (Point A in SI.Chain_4A[i])
                        {
                            if (SI.Chain_2S[j].Contains(A))
                            {
                                SI.CrossIDList_4A_Any.Add(A);
                                SI.CrossIDList_4A_2S.Add(A);
                            }
                        }
                    }
                }

                for (int i = 0; i < SI.Chain_4S.Count - 1; i++)
                {
                    for (int j = i + 1; j < SI.Chain_4S.Count; j++)
                    {
                        foreach (Point A in SI.Chain_4S[i])
                        {
                            if (SI.Chain_4S[j].Contains(A))
                            {
                                SI.CrossIDList_4_4.Add(A);
                                SI.CrossIDList_4S_4S.Add(A);
                            }
                        }
                    }
                }

                for (int i = 0; i < SI.Chain_4S.Count; i++)
                {
                    for (int j = 0; j < SI.Chain_3A.Count; j++)
                    {
                        foreach (Point A in SI.Chain_4S[i])
                        {
                            if (SI.Chain_3A[j].Contains(A))
                            {
                                SI.CrossIDList_4S_3A.Add(A);
                            }
                        }
                    }
                }

                for (int i = 0; i < SI.Chain_4S.Count; i++)
                {
                    for (int j = 0; j < SI.Chain_3S.Count; j++)
                    {
                        foreach (Point A in SI.Chain_4S[i])
                        {
                            if (SI.Chain_3S[j].Contains(A))
                            {
                                SI.CrossIDList_4S_3S.Add(A);
                            }
                        }
                    }
                }

                for (int i = 0; i < SI.Chain_4S.Count; i++)
                {
                    for (int j = 0; j < SI.Chain_2A.Count; j++)
                    {
                        foreach (Point A in SI.Chain_4S[i])
                        {
                            if (SI.Chain_2A[j].Contains(A))
                            {
                                SI.CrossIDList_4S_2A.Add(A);
                            }
                        }
                    }
                }

                for (int i = 0; i < SI.Chain_4S.Count; i++)
                {
                    for (int j = 0; j < SI.Chain_2S.Count; j++)
                    {
                        foreach (Point A in SI.Chain_4S[i])
                        {
                            if (SI.Chain_2S[j].Contains(A))
                            {
                                SI.CrossIDList_4S_2S.Add(A);
                            }
                        }
                    }
                }

                for (int i = 0; i < SI.Chain_3A.Count - 1; i++)
                {
                    for (int j = i + 1; j < SI.Chain_3A.Count; j++)
                    {
                        foreach (Point A in SI.Chain_3A[i])
                        {
                            if (SI.Chain_3A[j].Contains(A))
                            {
                                SI.CrossIDList_3A_3A.Add(A);
                            }
                        }
                    }
                }

                for (int i = 0; i < SI.Chain_3A.Count; i++)
                {
                    for (int j = 0; j < SI.Chain_3S.Count; j++)
                    {
                        foreach (Point A in SI.Chain_3A[i])
                        {
                            if (SI.Chain_3S[j].Contains(A))
                            {
                                SI.CrossIDList_3A_3S.Add(A);
                            }
                        }
                    }
                }

                for (int i = 0; i < SI.Chain_3A.Count; i++)
                {
                    for (int j = 0; j < SI.Chain_2A.Count; j++)
                    {
                        foreach (Point A in SI.Chain_3A[i])
                        {
                            if (SI.Chain_2A[j].Contains(A))
                            {
                                SI.CrossIDList_3A_2A.Add(A);
                            }
                        }
                    }
                }

                for (int i = 0; i < SI.Chain_3A.Count; i++)
                {
                    for (int j = 0; j < SI.Chain_2S.Count; j++)
                    {
                        foreach (Point A in SI.Chain_3A[i])
                        {
                            if (SI.Chain_2S[j].Contains(A))
                            {
                                SI.CrossIDList_3A_2S.Add(A);
                            }
                        }
                    }
                }

                for (int i = 0; i < SI.Chain_3S.Count - 1; i++)
                {
                    for (int j = i + 1; j < SI.Chain_3S.Count; j++)
                    {
                        foreach (Point A in SI.Chain_3S[i])
                        {
                            if (SI.Chain_3S[j].Contains(A))
                            {
                                SI.CrossIDList_3S_3S.Add(A);
                            }
                        }
                    }
                }

                for (int i = 0; i < SI.Chain_3S.Count; i++)
                {
                    for (int j = 0; j < SI.Chain_2A.Count; j++)
                    {
                        foreach (Point A in SI.Chain_3S[i])
                        {
                            if (SI.Chain_2A[j].Contains(A))
                            {
                                SI.CrossIDList_3S_2A.Add(A);
                            }
                        }
                    }
                }

                for (int i = 0; i < SI.Chain_3S.Count; i++)
                {
                    for (int j = 0; j < SI.Chain_2S.Count; j++)
                    {
                        foreach (Point A in SI.Chain_3S[i])
                        {
                            if (SI.Chain_2S[j].Contains(A))
                            {
                                SI.CrossIDList_3S_2S.Add(A);
                            }
                        }
                    }
                }

                for (int i = 0; i < SI.Chain_2A.Count - 1; i++)
                {
                    for (int j = i + 1; j < SI.Chain_2A.Count; j++)
                    {
                        foreach (Point A in SI.Chain_2A[i])
                        {
                            if (SI.Chain_2A[j].Contains(A))
                            {
                                SI.CrossIDList_2A_2A.Add(A);
                            }
                        }
                    }
                }

                for (int i = 0; i < SI.Chain_2A.Count; i++)
                {
                    for (int j = 0; j < SI.Chain_2S.Count; j++)
                    {
                        foreach (Point A in SI.Chain_2A[i])
                        {
                            if (SI.Chain_2S[j].Contains(A))
                            {
                                SI.CrossIDList_2A_2S.Add(A);
                            }
                        }
                    }
                }

                for (int i = 0; i < SI.Chain_2S.Count - 1; i++)
                {
                    for (int j = i + 1; j < SI.Chain_2S.Count; j++)
                    {
                        foreach (Point A in SI.Chain_2S[i])
                        {
                            if (SI.Chain_2S[j].Contains(A))
                            {
                                SI.CrossIDList_2S_2S.Add(A);
                            }
                        }
                    }
                }

                //

                return SI;
            }
            catch
            {
                return SI;
            }
        }

        // 检索。

        private bool IndexIsInChainList(Point A, List<Point[]> ChainList)
        {
            //
            // 判断一个索引是否在指定链列表中。A：索引；ChainList：链列表。
            //

            try
            {
                foreach (Point[] IDAry in ChainList)
                {
                    if (IDAry.Contains(A))
                    {
                        return true;
                    }
                }

                return false;
            }
            catch
            {
                return false;
            }
        }

        // 合法性与禁手。

        private bool Legality(Int32 ChessColor, StatsInfo StatsInfo, Point LastIndex)
        {
            //
            // 检查行棋的合法性。ChessColor：执棋棋色；StatsInfo：统计信息；LastIndex：最后落子索引。
            //

            try
            {
                if (UseBalanceBreaker)
                {
                    if (ChessColor == BLACK)
                    {
                        if (IndexIsInChainList(LastIndex, StatsInfo.Chain_5))
                        {
                            if (StatsInfo.Chain_Long.Count > 0)
                            {
                                return false;
                            }
                            else
                            {
                                return true;
                            }
                        }
                        else
                        {
                            if (StatsInfo.Chain_Long.Count > 0 || StatsInfo.CrossIDList_4_4.Contains(LastIndex) || StatsInfo.CrossIDList_3A_3A.Contains(LastIndex))
                            {
                                return false;
                            }
                            else
                            {
                                return true;
                            }
                        }
                    }
                    else
                    {
                        return true;
                    }
                }
                else
                {
                    return true;
                }
            }
            catch
            {
                return false;
            }
        }

        private List<Point> GetBanIndexList(Int32[,] Array, Size Cap)
        {
            //
            // 获取指定棋局的黑方禁手点索引列表。Array：矩阵，索引为 [x, y]；Cap：矩阵的大小，分量 (Width, Height) 分别表示沿 x 方向和沿 y 方向的元素数量。
            //

            try
            {
                if (UseBalanceBreaker)
                {
                    List<Point> ZL = GetCertainIndexListOfArray(Array, Cap, 0);

                    List<Point> BIL = new List<Point>(ZL.Count);

                    foreach (Point A in ZL)
                    {
                        Int32[,] Ary = GetCopyOfArray(Array);

                        ArrayLogicalAppend(Ary, Cap, A, BLACK);

                        StatsInfo SI_Bk = GetStatisticalInformationOfArray(Ary, Cap, BLACK);

                        if (!Legality(BLACK, SI_Bk, A))
                        {
                            BIL.Add(A);
                        }
                    }

                    return BIL;
                }

                return new List<Point>(0);
            }
            catch
            {
                return new List<Point>(0);
            }
        }

        // 局面评价。

        private double GetPositionValueFromStatsInfo(StatsInfo SI_Self, StatsInfo SI_Opp)
        {
            //
            // 根据双方统计信息计算棋局对于己方的局面评价指数。SI_Self：己方统计信息；SI_Opp：对方统计信息。
            //

            try
            {
                double PV = 0;

                PV += Math.Pow(2, 12) * SI_Self.Chain_FIR.Count;
                PV -= 0.75 * Math.Pow(2, 12) * SI_Opp.Chain_FIR.Count;

                PV += Math.Pow(2, 11) * SI_Self.Chain_4A.Count;
                PV -= 0.75 * Math.Pow(2, 11) * SI_Opp.Chain_4A.Count;

                PV += Math.Pow(2, 10) * SI_Self.CrossIDList_4S_4S.Count;
                PV -= 0.75 * Math.Pow(2, 10) * SI_Opp.CrossIDList_4S_4S.Count;

                PV += Math.Pow(2, 9) * SI_Self.CrossIDList_4S_3A.Count;
                PV -= 0.75 * Math.Pow(2, 9) * SI_Opp.CrossIDList_4S_3A.Count;

                PV += Math.Pow(2, 8) * SI_Self.CrossIDList_3A_3A.Count;
                PV -= 0.75 * Math.Pow(2, 8) * SI_Opp.CrossIDList_3A_3A.Count;

                PV += Math.Pow(2, 7) * SI_Self.CrossIDList_3A_2A.Count;
                PV -= 0.75 * Math.Pow(2, 7) * SI_Opp.CrossIDList_3A_2A.Count;

                PV += Math.Pow(2, 6) * (SI_Self.CrossIDList_3A_3S.Count + SI_Self.CrossIDList_4S_2A.Count);
                PV -= 0.75 * Math.Pow(2, 6) * (SI_Opp.CrossIDList_3A_3S.Count + SI_Opp.CrossIDList_4S_2A.Count);

                PV += Math.Pow(2, 5) * (SI_Self.CrossIDList_3A_2S.Count + SI_Self.CrossIDList_4S_3S.Count + SI_Self.CrossIDList_2A_2A.Count);
                PV -= 0.75 * Math.Pow(2, 5) * (SI_Opp.CrossIDList_3A_2S.Count + SI_Opp.CrossIDList_4S_3S.Count + SI_Opp.CrossIDList_2A_2A.Count);

                PV += Math.Pow(2, 4) * (SI_Self.Chain_3A.Count + SI_Self.CrossIDList_4S_2S.Count + SI_Self.CrossIDList_3S_2A.Count);
                PV -= 0.75 * Math.Pow(2, 4) * (SI_Opp.Chain_3A.Count + SI_Opp.CrossIDList_4S_2S.Count + SI_Opp.CrossIDList_3S_2A.Count);

                PV += Math.Pow(2, 3) * (SI_Self.Chain_4S.Count + SI_Self.CrossIDList_2A_2S.Count + SI_Self.CrossIDList_3S_3S.Count);
                PV -= 0.75 * Math.Pow(2, 3) * (SI_Opp.Chain_4S.Count + SI_Opp.CrossIDList_2A_2S.Count + SI_Opp.CrossIDList_3S_3S.Count);

                PV += Math.Pow(2, 2) * (SI_Self.Chain_2A.Count + SI_Self.CrossIDList_3S_2S.Count);
                PV -= 0.75 * Math.Pow(2, 2) * (SI_Opp.Chain_2A.Count + SI_Opp.CrossIDList_3S_2S.Count);

                PV += Math.Pow(2, 1) * (SI_Self.Chain_3S.Count + SI_Self.CrossIDList_2S_2S.Count);
                PV -= 0.75 * Math.Pow(2, 1) * (SI_Opp.Chain_3S.Count + SI_Opp.CrossIDList_2S_2S.Count);

                PV += Math.Pow(2, 0) * SI_Self.Chain_2S.Count;
                PV -= 0.75 * Math.Pow(2, 0) * SI_Opp.Chain_2S.Count;

                return PV;
            }
            catch
            {
                return 0;
            }
        }

        // 棋局状态判定。

        private void GetGameState()
        {
            //
            // 计算棋局状态。
            //

            GameStates GS = GameStates.Ongoing;

            JdgBasisIDList.Clear();

            try
            {
                if (ElementIndexList.Count > 0)
                {
                    StatsInfo SI_Black = GetStatisticalInformationOfArray(ElementMatrix, Range, BLACK);
                    StatsInfo SI_White = GetStatisticalInformationOfArray(ElementMatrix, Range, WHITE);

                    // 开局禁手判定：

                    if (UseBalanceBreaker)
                    {
                        if (GS == GameStates.Ongoing)
                        {
                            Point A = ElementIndexList[ElementIndexList.Count - 1];

                            if (ElementIndexList.Count == 1 && A != Center)
                            {
                                GS = GameStates.WhiteWin;

                                JdgBasisIDList.Add(A);
                            }
                            else if (ElementIndexList.Count == 2 && (Math.Abs(A.X - Center.X) > 1 || Math.Abs(A.Y - Center.Y) > 1))
                            {
                                GS = GameStates.BlackWin;

                                JdgBasisIDList.Add(A);
                            }
                            else if (ElementIndexList.Count == 3 && (Math.Abs(A.X - Center.X) > 2 || Math.Abs(A.Y - Center.Y) > 2))
                            {
                                GS = GameStates.WhiteWin;

                                JdgBasisIDList.Add(A);
                            }
                        }
                    }

                    // 长连禁手判定：

                    if (UseBalanceBreaker)
                    {
                        if (GS == GameStates.Ongoing)
                        {
                            if (SI_Black.Chain_Long.Count > 0)
                            {
                                GS = GameStates.WhiteWin;

                                foreach (Point[] IDAry in SI_Black.Chain_Long)
                                {
                                    foreach (Point A in IDAry)
                                    {
                                        JdgBasisIDList.Add(A);
                                    }
                                }
                            }
                        }
                    }

                    // 成五判定：

                    if (GS == GameStates.Ongoing)
                    {
                        if (SI_White.Chain_FIR.Count > 0)
                        {
                            GS = GameStates.WhiteWin;

                            foreach (Point[] IDAry in SI_White.Chain_FIR)
                            {
                                foreach (Point A in IDAry)
                                {
                                    JdgBasisIDList.Add(A);
                                }
                            }
                        }
                        else
                        {
                            if (UseBalanceBreaker)
                            {
                                if (SI_Black.Chain_5.Count > 0)
                                {
                                    GS = GameStates.BlackWin;

                                    foreach (Point[] IDAry in SI_Black.Chain_5)
                                    {
                                        foreach (Point A in IDAry)
                                        {
                                            JdgBasisIDList.Add(A);
                                        }
                                    }
                                }
                            }
                            else
                            {
                                if (SI_Black.Chain_FIR.Count > 0)
                                {
                                    GS = GameStates.BlackWin;

                                    foreach (Point[] IDAry in SI_Black.Chain_FIR)
                                    {
                                        foreach (Point A in IDAry)
                                        {
                                            JdgBasisIDList.Add(A);
                                        }
                                    }
                                }
                            }
                        }
                    }

                    // 四四禁手、三三禁手判定：

                    if (UseBalanceBreaker)
                    {
                        if (GS == GameStates.Ongoing)
                        {
                            Point ALast = ElementIndexList[ElementIndexList.Count - 1];
                            Int32 ELast = ElementMatrix_GetValue(ALast);

                            if (ELast == BLACK)
                            {
                                if (SI_Black.CrossIDList_4_4.Contains(ALast))
                                {
                                    GS = GameStates.WhiteWin;

                                    foreach (Point[] IDAry in SI_Black.Chain_4A)
                                    {
                                        if (IDAry.Contains(ALast))
                                        {
                                            foreach (Point A in IDAry)
                                            {
                                                JdgBasisIDList.Add(A);
                                            }
                                        }
                                    }

                                    foreach (Point[] IDAry in SI_Black.Chain_4S)
                                    {
                                        if (IDAry.Contains(ALast))
                                        {
                                            foreach (Point A in IDAry)
                                            {
                                                JdgBasisIDList.Add(A);
                                            }
                                        }
                                    }
                                }

                                if (SI_Black.CrossIDList_3A_3A.Contains(ALast))
                                {
                                    GS = GameStates.WhiteWin;

                                    foreach (Point[] IDAry in SI_Black.Chain_3A)
                                    {
                                        if (IDAry.Contains(ALast))
                                        {
                                            foreach (Point A in IDAry)
                                            {
                                                JdgBasisIDList.Add(A);
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }

                    // 和局判定：

                    if (GS == GameStates.Ongoing)
                    {
                        if (GetZeroCountOfArray(ElementMatrix, Range) == 0)
                        {
                            GS = GameStates.Draw;
                        }
                        else
                        {
                            List<Point> ZL = GetCertainIndexListOfArray(ElementMatrix, Range, 0);

                            Int32[,] Ary_Blk = GetCopyOfArray(ElementMatrix);
                            Int32[,] Ary_Whi = GetCopyOfArray(ElementMatrix);

                            foreach (Point A in ZL)
                            {
                                ArrayLogicalAppend(Ary_Blk, Range, A, BLACK);
                                ArrayLogicalAppend(Ary_Whi, Range, A, WHITE);
                            }

                            StatsInfo SI_Blk = GetStatisticalInformationOfArray(Ary_Blk, Range, BLACK);
                            StatsInfo SI_Whi = GetStatisticalInformationOfArray(Ary_Whi, Range, WHITE);

                            if (SI_Blk.Chain_FIR.Count == 0 && SI_Whi.Chain_FIR.Count == 0)
                            {
                                GS = GameStates.Draw;
                            }
                        }
                    }

                    // 禁手点查找：

                    if (UseBalanceBreaker && ShowBanPoint)
                    {
                        List<Point> OldBanIDList = new List<Point>(BanIDList);

                        BanIDList.Clear();

                        if (GS == GameStates.Ongoing)
                        {
                            BanIDList = GetBanIndexList(ElementMatrix, Range);
                        }

                        foreach (Point A in OldBanIDList)
                        {
                            if (!BanIDList.Contains(A))
                            {
                                ElementMatrix_PresentAt(A);
                            }
                        }

                        foreach (Point A in BanIDList)
                        {
                            if (!OldBanIDList.Contains(A))
                            {
                                ElementMatrix_PresentAt(A);
                            }
                        }
                    }
                }
                else
                {
                    GS = GameStates.Ongoing;
                }

                // 超时判定：

                if (GS == GameStates.Ongoing)
                {
                    if (FreeTimeRemaining_Black.TotalMilliseconds <= 0 && CountdownTimeRemaining_Black.TotalMilliseconds <= 0)
                    {
                        GS = GameStates.WhiteWin;
                    }
                    else if (FreeTimeRemaining_White.TotalMilliseconds <= 0 && CountdownTimeRemaining_White.TotalMilliseconds <= 0)
                    {
                        GS = GameStates.BlackWin;
                    }
                }

                //

                GameState = GS;
            }
            catch
            {
                GameState = GS;
            }
        }

        #endregion

        #region 猜先，棋钟，落子

        // 猜先与换棋。

        private void Toss()
        {
            //
            // 猜先。随机确定是否由 AI 执黑先行。
            //

            AIFirst = (Com.Statistics.RandomInteger() % 2 == 0);

            RepaintCurBmp();
        }

        private void ExchangeChessColor()
        {
            //
            // 交换棋色。
            //

            AIFirst = !AIFirst;

            RepaintCurBmp();
        }

        // 棋钟。

        private CycData CD_ChessClock = new CycData(); // Timer_ChessClock 计时周期数据。

        private void Timer_ChessClock_Tick(object sender, EventArgs e)
        {
            //
            // Timer_ChessClock。
            //

            ChessClockWorkOnce();
        }

        private void ChessClockStart()
        {
            //
            // 棋钟开始。
            //

            CD_ChessClock.Reset();

            //

            ChessClockUpdateInterval();

            //

            Timer_ChessClock.Enabled = true;
        }

        private void ChessClockWorkOnce()
        {
            //
            // 棋钟进行一次工作。
            //

            CD_ChessClock.Update();

            //

            ThisGameTime += TimeSpan.FromMilliseconds(CD_ChessClock.DeltaMS);
            TotalGameTime += TimeSpan.FromMilliseconds(CD_ChessClock.DeltaMS);

            if (ElementIndexList.Count > 0)
            {
                if (CurrentColor == Roles.Black)
                {
                    if (FreeTimeRemaining_Black.TotalMilliseconds > 0)
                    {
                        FreeTimeRemaining_Black -= TimeSpan.FromMilliseconds(CD_ChessClock.DeltaMS);

                        CountdownTimeRemaining_Black = TotalCountdownTime;
                    }
                    else
                    {
                        FreeTimeRemaining_Black = TimeSpan.Zero;

                        if (CountdownTimeRemaining_Black.TotalMilliseconds > 0)
                        {
                            CountdownTimeRemaining_Black -= TimeSpan.FromMilliseconds(CD_ChessClock.DeltaMS);
                        }
                        else
                        {
                            CountdownTimeRemaining_Black = TimeSpan.Zero;

                            Timer_ChessClock.Enabled = false;

                            Judgement();
                        }
                    }
                }
                else
                {
                    if (FreeTimeRemaining_White.TotalMilliseconds > 0)
                    {
                        FreeTimeRemaining_White -= TimeSpan.FromMilliseconds(CD_ChessClock.DeltaMS);

                        CountdownTimeRemaining_White = TotalCountdownTime;
                    }
                    else
                    {
                        FreeTimeRemaining_White = TimeSpan.Zero;

                        if (CountdownTimeRemaining_White.TotalMilliseconds > 0)
                        {
                            CountdownTimeRemaining_White -= TimeSpan.FromMilliseconds(CD_ChessClock.DeltaMS);
                        }
                        else
                        {
                            CountdownTimeRemaining_White = TimeSpan.Zero;

                            Timer_ChessClock.Enabled = false;

                            Judgement();
                        }
                    }
                }
            }

            RepaintCurBmp();

            //

            ChessClockUpdateInterval();
        }

        private void ChessClockStop()
        {
            //
            // 棋钟停止。
            //

            Timer_ChessClock.Enabled = false;
        }

        private void ChessClockReset()
        {
            //
            // 棋钟重置。
            //

            FreeTimeRemaining_Black = FreeTimeRemaining_White = TotalFreeTime;
            CountdownTimeRemaining_Black = CountdownTimeRemaining_White = TotalCountdownTime;

            RepaintCurBmp();
        }

        private void ChessClockUpdateInterval()
        {
            //
            // 棋钟更新工作周期。
            //

            if ((CurrentColor == Roles.Black && FreeTimeRemaining_Black.TotalSeconds < 60) || (CurrentColor == Roles.White && FreeTimeRemaining_White.TotalSeconds < 60))
            {
                Timer_ChessClock.Interval = 10;
            }
            else
            {
                Timer_ChessClock.Interval = 100;
            }
        }

        // 落子。

        private void Lazi(Point A, bool AsAI)
        {
            //
            // 落子。A：索引；AsAI：是否以 AI 身份落子。
            //

            if (ElementMatrix_IndexValid(A))
            {
                if (ElementMatrix_GetValue(A) == 0)
                {
                    if (AsAI)
                    {
                        ElementMatrix_LogicalAppend(A, ChessColor_AI);
                    }
                    else
                    {
                        ElementMatrix_LogicalAppend(A, ChessColor_User);

                        if (GameState == GameStates.Ongoing)
                        {
                            AsyncWorkerStart();
                        }
                    }

                    if (FreeTimeRemaining_Black.TotalMilliseconds <= 0)
                    {
                        CountdownTimeRemaining_Black = TimeSpan.FromMinutes(Math.Ceiling(CountdownTimeRemaining_Black.TotalSeconds / 60));
                    }

                    if (FreeTimeRemaining_White.TotalMilliseconds <= 0)
                    {
                        CountdownTimeRemaining_White = TimeSpan.FromMinutes(Math.Ceiling(CountdownTimeRemaining_White.TotalSeconds / 60));
                    }
                }
            }
        }

        #endregion

        #region 异步分析器

        private List<BackgroundWorker> AsyncWorkerList = null; // 异步分析器列表。

        private struct SrchRslt // 搜索分支。
        {
            public Point Index; // 索引。
            public Int32 PRI; // 优先级。数值越小，优先级越高。
            public double Value; // 落子价值。在同一索引处己方落子得到的己方局面评价与对方落子得到的对方局面评价的和。
        }

        private List<SrchRslt>[] SrchRsltListAry = null; // 搜索分支列表数组。

        private Int32 PRIMin = Int32.MaxValue; // 当前所有搜索分支中的最高优先级。

        private List<Point> ZeroIndexList = new List<Point>(CAPACITY * CAPACITY); // 零值元素索引列表。

        private void AsyncWorkerStart()
        {
            //
            // 异步分析器开始工作。
            //

            try
            {
                for (int i = 0; i < SrchRsltListAry.Length; i++)
                {
                    SrchRsltListAry[i] = new List<SrchRslt>(0);
                }

                PRIMin = Int32.MaxValue;

                //

                ZeroIndexList.Clear();

                List<Point> _ZeroIndexList = GetCertainIndexListOfArray(ElementMatrix, Range, 0);

                while (_ZeroIndexList.Count > 0)
                {
                    int i = Com.Statistics.RandomInteger(_ZeroIndexList.Count);

                    ZeroIndexList.Add(_ZeroIndexList[i]);
                    _ZeroIndexList.RemoveAt(i);
                }

                //

                for (int i = 0; i < AsyncWorkerList.Count; i++)
                {
                    if (!AsyncWorkerList[i].IsBusy)
                    {
                        AsyncWorkerList[i].RunWorkerAsync();
                    }
                }
            }
            catch { }
        }

        private void AsyncWorkerStop()
        {
            //
            // 异步分析器停止工作。
            //

            try
            {
                for (int i = 0; i < AsyncWorkerList.Count; i++)
                {
                    if (AsyncWorkerList[i].IsBusy)
                    {
                        AsyncWorkerList[i].CancelAsync();
                    }
                }
            }
            catch { }
        }

        private void AsyncWorkerAnalyze(object sender, DoWorkEventArgs e)
        {
            //
            // 异步分析器执行分析。
            //

            try
            {
                Int32 ThreadId = -1;

                for (int i = 0; i < AsyncWorkerList.Count; i++)
                {
                    if (object.ReferenceEquals(sender, AsyncWorkerList[i]))
                    {
                        ThreadId = i;

                        break;
                    }
                }

                //

                List<Point> AL = new List<Point>(ZeroIndexList.Count);

                for (int i = 0; i < ZeroIndexList.Count; i++)
                {
                    if (i % SrchRsltListAry.Length == ThreadId)
                    {
                        AL.Add(ZeroIndexList[i]);
                    }
                }

                for (int i = 0; i < AL.Count; i++)
                {
                    if (((BackgroundWorker)sender).CancellationPending)
                    {
                        break;
                    }

                    //

                    Point A = AL[i];

                    bool F_Start;

                    if (UseBalanceBreaker)
                    {
                        switch (ElementIndexList.Count)
                        {
                            case 0: F_Start = (A == Center); break;
                            case 1: F_Start = (Math.Abs(A.X - Center.X) <= 1 && Math.Abs(A.Y - Center.Y) <= 1); break;
                            case 2: F_Start = (Math.Abs(A.X - Center.X) <= 2 && Math.Abs(A.Y - Center.Y) <= 2); break;
                            default: F_Start = true; break;
                        }
                    }
                    else
                    {
                        F_Start = true;
                    }

                    if (F_Start)
                    {
                        SrchRslt SR = new SrchRslt();
                        SR.Index = A;

                        Int32[,] Ary_AI = GetCopyOfArray(ElementMatrix);
                        Int32[,] Ary_User = GetCopyOfArray(ElementMatrix);

                        ArrayLogicalAppend(Ary_AI, Range, A, ChessColor_AI);
                        ArrayLogicalAppend(Ary_User, Range, A, ChessColor_User);

                        StatsInfo SI_AI = GetStatisticalInformationOfArray(Ary_AI, Range, ChessColor_AI);
                        StatsInfo SI_User = GetStatisticalInformationOfArray(Ary_User, Range, ChessColor_User);

                        StatsInfo SI_AI_Self = SI_AI;
                        StatsInfo SI_AI_Opp = GetStatisticalInformationOfArray(Ary_AI, Range, ChessColor_User);

                        StatsInfo SI_User_Self = SI_User;
                        StatsInfo SI_User_Opp = GetStatisticalInformationOfArray(Ary_User, Range, ChessColor_AI);

                        bool F_AI = Legality(ChessColor_AI, SI_AI, A);
                        bool F_User = Legality(ChessColor_User, SI_User, A);

                        bool Flag = false;

                        Int32 PRI = -1;

                        while (!Flag)
                        {
                            if (((BackgroundWorker)sender).CancellationPending)
                            {
                                break;
                            }

                            //

                            PRI++;

                            if (PRI > PRIMin)
                            {
                                break;
                            }

                            switch (PRI)
                            {
                                case 0: // 攻：成五
                                    Flag = (F_AI && IndexIsInChainList(A, SI_AI.Chain_FIR));
                                    break;

                                case 1: // 防：成五
                                    Flag = (F_User && IndexIsInChainList(A, SI_User.Chain_FIR));
                                    break;

                                //

                                case 2: // 攻：活四-任意
                                    Flag = (F_AI && SI_AI.CrossIDList_4A_Any.Contains(A));
                                    break;

                                case 3: // 攻：活四
                                    Flag = (F_AI && IndexIsInChainList(A, SI_AI.Chain_4A));
                                    break;

                                case 4: // 攻：冲四-冲四
                                    Flag = (F_AI && SI_AI.CrossIDList_4S_4S.Contains(A));
                                    break;

                                case 5: // 攻：冲四-活三
                                    Flag = (F_AI && SI_AI.CrossIDList_4S_3A.Contains(A));
                                    break;

                                case 6: // 防：活四-任意
                                    Flag = ((F_AI && F_User) && SI_User.CrossIDList_4A_Any.Contains(A));
                                    break;

                                case 7: // 防：活四
                                    Flag = ((F_AI && F_User) && IndexIsInChainList(A, SI_User.Chain_4A));
                                    break;

                                case 8: // 防：冲四-冲四
                                    Flag = ((F_AI && F_User) && SI_User.CrossIDList_4S_4S.Contains(A));
                                    break;

                                case 9: // 防：冲四-活三
                                    Flag = ((F_AI && F_User) && SI_User.CrossIDList_4S_3A.Contains(A));
                                    break;

                                //

                                case 10: // 攻：此落子形成冲四，并且成五点是黑方的禁手点
                                    if (UseBalanceBreaker && !AIFirst)
                                    {
                                        if (F_AI && IndexIsInChainList(A, SI_AI.Chain_4S))
                                        {
                                            List<Point> NextIDList = new List<Point>(80);

                                            for (int X = Math.Max(0, A.X - 4); X < Math.Min(Range.Width, A.X + 5); X++)
                                            {
                                                Point A0 = new Point(X, A.Y);

                                                if (Ary_AI[A0.X, A0.Y] == 0)
                                                {
                                                    NextIDList.Add(A0);
                                                }

                                                Point A1 = new Point(X, A.Y + (X - A.X));

                                                if (A1.Y >= 0 && A1.Y < Range.Height)
                                                {
                                                    if (Ary_AI[A1.X, A1.Y] == 0)
                                                    {
                                                        NextIDList.Add(A1);
                                                    }
                                                }
                                            }

                                            for (int Y = Math.Max(0, A.Y - 4); Y < Math.Min(Range.Height, A.Y + 5); Y++)
                                            {
                                                Point A0 = new Point(A.X, Y);

                                                if (Ary_AI[A0.X, A0.Y] == 0)
                                                {
                                                    NextIDList.Add(A0);
                                                }

                                                Point A1 = new Point(A.X - (Y - A.Y), Y);

                                                if (A1.X >= 0 && A1.X < Range.Width)
                                                {
                                                    if (Ary_AI[A1.X, A1.Y] == 0)
                                                    {
                                                        NextIDList.Add(A1);
                                                    }
                                                }
                                            }

                                            bool _F_AI = false;

                                            Point ToFIRID = new Point();

                                            foreach (Point _A in NextIDList)
                                            {
                                                Int32[,] _Ary_AI = GetCopyOfArray(Ary_AI);

                                                ArrayLogicalAppend(_Ary_AI, Range, _A, ChessColor_AI);

                                                StatsInfo _SI_AI = GetStatisticalInformationOfArray(_Ary_AI, Range, ChessColor_AI);

                                                _F_AI = Legality(ChessColor_AI, _SI_AI, _A);

                                                if (_F_AI && IndexIsInChainList(_A, _SI_AI.Chain_FIR))
                                                {
                                                    ToFIRID = _A;

                                                    break;
                                                }
                                            }

                                            if (_F_AI)
                                            {
                                                Int32[,] _Ary_User = GetCopyOfArray(Ary_AI);

                                                ArrayLogicalAppend(_Ary_User, Range, ToFIRID, ChessColor_User);

                                                StatsInfo _SI_User = GetStatisticalInformationOfArray(_Ary_User, Range, ChessColor_User);

                                                bool _F_User = Legality(ChessColor_User, _SI_User, ToFIRID);

                                                Flag = !_F_User;
                                            }
                                        }
                                    }
                                    break;

                                case 11: // 防：此落子形成冲四，并且成五点是黑方的禁手点
                                    if (UseBalanceBreaker && AIFirst)
                                    {
                                        if (F_User && IndexIsInChainList(A, SI_User.Chain_4S))
                                        {
                                            List<Point> NextIDList = new List<Point>(80);

                                            for (int X = Math.Max(0, A.X - 4); X < Math.Min(Range.Width, A.X + 5); X++)
                                            {
                                                Point A0 = new Point(X, A.Y);

                                                if (Ary_User[A0.X, A0.Y] == 0)
                                                {
                                                    NextIDList.Add(A0);
                                                }

                                                Point A1 = new Point(X, A.Y + (X - A.X));

                                                if (A1.Y >= 0 && A1.Y < Range.Height)
                                                {
                                                    if (Ary_User[A1.X, A1.Y] == 0)
                                                    {
                                                        NextIDList.Add(A1);
                                                    }
                                                }
                                            }

                                            for (int Y = Math.Max(0, A.Y - 4); Y < Math.Min(Range.Height, A.Y + 5); Y++)
                                            {
                                                Point A0 = new Point(A.X, Y);

                                                if (Ary_User[A0.X, A0.Y] == 0)
                                                {
                                                    NextIDList.Add(A0);
                                                }

                                                Point A1 = new Point(A.X - (Y - A.Y), Y);

                                                if (A1.X >= 0 && A1.X < Range.Width)
                                                {
                                                    if (Ary_User[A1.X, A1.Y] == 0)
                                                    {
                                                        NextIDList.Add(A1);
                                                    }
                                                }
                                            }

                                            bool _F_User = false;

                                            Point ToFIRID = new Point();

                                            foreach (Point _A in NextIDList)
                                            {
                                                Int32[,] _Ary_User = GetCopyOfArray(Ary_User);

                                                ArrayLogicalAppend(_Ary_User, Range, _A, ChessColor_User);

                                                StatsInfo _SI_User = GetStatisticalInformationOfArray(_Ary_User, Range, ChessColor_User);

                                                _F_User = Legality(ChessColor_User, _SI_User, _A);

                                                if (_F_User && IndexIsInChainList(_A, _SI_User.Chain_FIR))
                                                {
                                                    ToFIRID = _A;

                                                    break;
                                                }
                                            }

                                            if (_F_User)
                                            {
                                                Int32[,] _Ary_AI = GetCopyOfArray(Ary_User);

                                                ArrayLogicalAppend(_Ary_AI, Range, ToFIRID, ChessColor_AI);

                                                StatsInfo _SI_AI = GetStatisticalInformationOfArray(_Ary_AI, Range, ChessColor_AI);

                                                bool _F_AI = Legality(ChessColor_AI, _SI_AI, ToFIRID);

                                                Flag = !_F_AI;
                                            }
                                        }
                                    }
                                    break;

                                //

                                case 12: // 攻：此落子形成活三，并且成四点之一是黑方的禁手点
                                    if (UseBalanceBreaker && !AIFirst)
                                    {
                                        if (F_AI && IndexIsInChainList(A, SI_AI.Chain_3A))
                                        {
                                            List<Point> NextIDList = new List<Point>(80);

                                            for (int X = Math.Max(0, A.X - 4); X < Math.Min(Range.Width, A.X + 5); X++)
                                            {
                                                Point A0 = new Point(X, A.Y);

                                                if (Ary_AI[A0.X, A0.Y] == 0)
                                                {
                                                    NextIDList.Add(A0);
                                                }

                                                Point A1 = new Point(X, A.Y + (X - A.X));

                                                if (A1.Y >= 0 && A1.Y < Range.Height)
                                                {
                                                    if (Ary_AI[A1.X, A1.Y] == 0)
                                                    {
                                                        NextIDList.Add(A1);
                                                    }
                                                }
                                            }

                                            for (int Y = Math.Max(0, A.Y - 4); Y < Math.Min(Range.Height, A.Y + 5); Y++)
                                            {
                                                Point A0 = new Point(A.X, Y);

                                                if (Ary_AI[A0.X, A0.Y] == 0)
                                                {
                                                    NextIDList.Add(A0);
                                                }

                                                Point A1 = new Point(A.X - (Y - A.Y), Y);

                                                if (A1.X >= 0 && A1.X < Range.Width)
                                                {
                                                    if (Ary_AI[A1.X, A1.Y] == 0)
                                                    {
                                                        NextIDList.Add(A1);
                                                    }
                                                }
                                            }

                                            List<Point> To4IDList = new List<Point>(NextIDList.Count);

                                            foreach (Point _A in NextIDList)
                                            {
                                                Int32[,] _Ary_AI = GetCopyOfArray(Ary_AI);

                                                ArrayLogicalAppend(_Ary_AI, Range, _A, ChessColor_AI);

                                                StatsInfo _SI_AI = GetStatisticalInformationOfArray(_Ary_AI, Range, ChessColor_AI);

                                                bool _F_AI = Legality(ChessColor_AI, _SI_AI, _A);

                                                if (_F_AI && (IndexIsInChainList(_A, _SI_AI.Chain_4A) || IndexIsInChainList(_A, _SI_AI.Chain_4S)))
                                                {
                                                    To4IDList.Add(_A);
                                                }
                                            }

                                            if (To4IDList.Count > 0)
                                            {
                                                foreach (Point _A in To4IDList)
                                                {
                                                    Int32[,] _Ary_User = GetCopyOfArray(Ary_AI);

                                                    ArrayLogicalAppend(_Ary_User, Range, _A, ChessColor_User);

                                                    StatsInfo _SI_User = GetStatisticalInformationOfArray(_Ary_User, Range, ChessColor_User);

                                                    bool _F_User = Legality(ChessColor_User, _SI_User, _A);

                                                    if (!_F_User)
                                                    {
                                                        Flag = true;

                                                        break;
                                                    }
                                                }
                                            }
                                        }
                                    }
                                    break;

                                case 13: // 防：此落子形成活三，并且成四点之一是黑方的禁手点
                                    if (UseBalanceBreaker && AIFirst)
                                    {
                                        if (F_User && IndexIsInChainList(A, SI_User.Chain_3A))
                                        {
                                            List<Point> NextIDList = new List<Point>(80);

                                            for (int X = Math.Max(0, A.X - 4); X < Math.Min(Range.Width, A.X + 5); X++)
                                            {
                                                Point A0 = new Point(X, A.Y);

                                                if (Ary_User[A0.X, A0.Y] == 0)
                                                {
                                                    NextIDList.Add(A0);
                                                }

                                                Point A1 = new Point(X, A.Y + (X - A.X));

                                                if (A1.Y >= 0 && A1.Y < Range.Height)
                                                {
                                                    if (Ary_User[A1.X, A1.Y] == 0)
                                                    {
                                                        NextIDList.Add(A1);
                                                    }
                                                }
                                            }

                                            for (int Y = Math.Max(0, A.Y - 4); Y < Math.Min(Range.Height, A.Y + 5); Y++)
                                            {
                                                Point A0 = new Point(A.X, Y);

                                                if (Ary_User[A0.X, A0.Y] == 0)
                                                {
                                                    NextIDList.Add(A0);
                                                }

                                                Point A1 = new Point(A.X - (Y - A.Y), Y);

                                                if (A1.X >= 0 && A1.X < Range.Width)
                                                {
                                                    if (Ary_User[A1.X, A1.Y] == 0)
                                                    {
                                                        NextIDList.Add(A1);
                                                    }
                                                }
                                            }

                                            List<Point> To4IDList = new List<Point>(NextIDList.Count);

                                            foreach (Point _A in NextIDList)
                                            {
                                                Int32[,] _Ary_User = GetCopyOfArray(Ary_User);

                                                ArrayLogicalAppend(_Ary_User, Range, _A, ChessColor_User);

                                                StatsInfo _SI_User = GetStatisticalInformationOfArray(_Ary_User, Range, ChessColor_User);

                                                bool _F_User = Legality(ChessColor_User, _SI_User, _A);

                                                if (_F_User && (IndexIsInChainList(_A, _SI_User.Chain_4A) || IndexIsInChainList(_A, _SI_User.Chain_4S)))
                                                {
                                                    To4IDList.Add(_A);
                                                }
                                            }

                                            if (To4IDList.Count > 0)
                                            {
                                                foreach (Point _A in To4IDList)
                                                {
                                                    Int32[,] _Ary_AI = GetCopyOfArray(Ary_User);

                                                    ArrayLogicalAppend(_Ary_AI, Range, _A, ChessColor_AI);

                                                    StatsInfo _SI_AI = GetStatisticalInformationOfArray(_Ary_AI, Range, ChessColor_AI);

                                                    bool _F_AI = Legality(ChessColor_AI, _SI_AI, _A);

                                                    if (!_F_AI)
                                                    {
                                                        Flag = true;

                                                        break;
                                                    }
                                                }
                                            }
                                        }
                                    }
                                    break;

                                //

                                case 14: // 攻：活三-活三
                                    Flag = (F_AI && SI_AI.CrossIDList_3A_3A.Contains(A));
                                    break;

                                case 15: // 防：活三-活三
                                    Flag = ((F_AI && F_User) && SI_User.CrossIDList_3A_3A.Contains(A));
                                    break;

                                //

                                case 16: // 攻：此落子形成冲四或活三，并且在被防守后仍是接下来可能形成活四-任意、冲四-冲四、冲四-活三、活三-活三的构成子
                                    if (F_AI && (IndexIsInChainList(A, SI_AI.Chain_4S) || IndexIsInChainList(A, SI_AI.Chain_3A)))
                                    {
                                        List<Point> AntiIDList = new List<Point>(80);

                                        for (int X = Math.Max(0, A.X - 4); X < Math.Min(Range.Width, A.X + 5); X++)
                                        {
                                            Point A0 = new Point(X, A.Y);

                                            if (Ary_User[A0.X, A0.Y] == 0)
                                            {
                                                AntiIDList.Add(A0);
                                            }

                                            Point A1 = new Point(X, A.Y + (X - A.X));

                                            if (A1.Y >= 0 && A1.Y < Range.Height)
                                            {
                                                if (Ary_User[A1.X, A1.Y] == 0)
                                                {
                                                    AntiIDList.Add(A1);
                                                }
                                            }
                                        }

                                        for (int Y = Math.Max(0, A.Y - 4); Y < Math.Min(Range.Height, A.Y + 5); Y++)
                                        {
                                            Point A0 = new Point(A.X, Y);

                                            if (Ary_User[A0.X, A0.Y] == 0)
                                            {
                                                AntiIDList.Add(A0);
                                            }

                                            Point A1 = new Point(A.X - (Y - A.Y), Y);

                                            if (A1.X >= 0 && A1.X < Range.Width)
                                            {
                                                if (Ary_User[A1.X, A1.Y] == 0)
                                                {
                                                    AntiIDList.Add(A1);
                                                }
                                            }
                                        }

                                        List<Point> UserNextIDList = new List<Point>(AntiIDList.Count);

                                        foreach (Point _A in AntiIDList)
                                        {
                                            Int32[,] _Ary_User = GetCopyOfArray(Ary_AI);

                                            ArrayLogicalAppend(_Ary_User, Range, _A, ChessColor_User);

                                            StatsInfo _SI_User = GetStatisticalInformationOfArray(_Ary_User, Range, ChessColor_User);
                                            StatsInfo _SI_AI = GetStatisticalInformationOfArray(_Ary_User, Range, ChessColor_AI);

                                            bool _F_User = Legality(ChessColor_User, _SI_User, _A);

                                            if (_F_User && (!IndexIsInChainList(A, _SI_AI.Chain_4S) && !IndexIsInChainList(A, _SI_AI.Chain_3A)))
                                            {
                                                UserNextIDList.Add(_A);
                                            }
                                        }

                                        if (UserNextIDList.Count > 0)
                                        {
                                            bool F_N = true;

                                            foreach (Point A_N in UserNextIDList)
                                            {
                                                Int32[,] _Ary_User = GetCopyOfArray(Ary_AI);

                                                ArrayLogicalAppend(_Ary_User, Range, A_N, ChessColor_User);

                                                List<Point> ZeroIDList = new List<Point>(80);

                                                for (int X = Math.Max(0, A.X - 4); X < Math.Min(Range.Width, A.X + 5); X++)
                                                {
                                                    for (int Y = Math.Max(0, A.Y - 4); Y < Math.Min(Range.Height, A.Y + 5); Y++)
                                                    {
                                                        if (_Ary_User[X, Y] == 0)
                                                        {
                                                            ZeroIDList.Add(new Point(X, Y));
                                                        }
                                                    }
                                                }

                                                bool F_Z = false;

                                                foreach (Point A_Z in ZeroIDList)
                                                {
                                                    Int32[,] _Ary_AI = GetCopyOfArray(_Ary_User);

                                                    ArrayLogicalAppend(_Ary_AI, Range, A_Z, ChessColor_AI);

                                                    StatsInfo _SI_AI = GetStatisticalInformationOfArray(_Ary_AI, Range, ChessColor_AI);

                                                    bool _F_AI = Legality(ChessColor_AI, _SI_AI, A_Z);

                                                    if (_F_AI && (_SI_AI.CrossIDList_4A_Any.Contains(A_Z) || _SI_AI.CrossIDList_4S_4S.Contains(A_Z) || _SI_AI.CrossIDList_4S_3A.Contains(A_Z) || _SI_AI.CrossIDList_3A_3A.Contains(A_Z)))
                                                    {
                                                        F_Z = true;

                                                        break;
                                                    }
                                                }

                                                if (!F_Z)
                                                {
                                                    F_N = false;

                                                    break;
                                                }
                                            }

                                            Flag = F_N;
                                        }
                                    }
                                    break;

                                case 17: // 防：此落子形成冲四或活三，并且在被防守后仍是接下来可能形成活四-任意、冲四-冲四、冲四-活三、活三-活三的构成子
                                    if (F_User && (IndexIsInChainList(A, SI_User.Chain_4S) || IndexIsInChainList(A, SI_User.Chain_3A)))
                                    {
                                        List<Point> AntiIDList = new List<Point>(80);

                                        for (int X = Math.Max(0, A.X - 4); X < Math.Min(Range.Width, A.X + 5); X++)
                                        {
                                            Point A0 = new Point(X, A.Y);

                                            if (Ary_User[A0.X, A0.Y] == 0)
                                            {
                                                AntiIDList.Add(A0);
                                            }

                                            Point A1 = new Point(X, A.Y + (X - A.X));

                                            if (A1.Y >= 0 && A1.Y < Range.Height)
                                            {
                                                if (Ary_User[A1.X, A1.Y] == 0)
                                                {
                                                    AntiIDList.Add(A1);
                                                }
                                            }
                                        }

                                        for (int Y = Math.Max(0, A.Y - 4); Y < Math.Min(Range.Height, A.Y + 5); Y++)
                                        {
                                            Point A0 = new Point(A.X, Y);

                                            if (Ary_User[A0.X, A0.Y] == 0)
                                            {
                                                AntiIDList.Add(A0);
                                            }

                                            Point A1 = new Point(A.X - (Y - A.Y), Y);

                                            if (A1.X >= 0 && A1.X < Range.Width)
                                            {
                                                if (Ary_User[A1.X, A1.Y] == 0)
                                                {
                                                    AntiIDList.Add(A1);
                                                }
                                            }
                                        }

                                        List<Point> AINextIDList = new List<Point>(AntiIDList.Count);

                                        foreach (Point _A in AntiIDList)
                                        {
                                            Int32[,] _Ary_AI = GetCopyOfArray(Ary_User);

                                            ArrayLogicalAppend(_Ary_AI, Range, _A, ChessColor_AI);

                                            StatsInfo _SI_AI = GetStatisticalInformationOfArray(_Ary_AI, Range, ChessColor_AI);
                                            StatsInfo _SI_User = GetStatisticalInformationOfArray(_Ary_AI, Range, ChessColor_User);

                                            bool _F_AI = Legality(ChessColor_AI, _SI_AI, _A);

                                            if (_F_AI && (!IndexIsInChainList(A, _SI_User.Chain_4S) && !IndexIsInChainList(A, _SI_User.Chain_3A)))
                                            {
                                                AINextIDList.Add(_A);
                                            }
                                        }

                                        if (AINextIDList.Count > 0)
                                        {
                                            bool F_N = true;

                                            foreach (Point A_N in AINextIDList)
                                            {
                                                Int32[,] _Ary_AI = GetCopyOfArray(Ary_User);

                                                ArrayLogicalAppend(_Ary_AI, Range, A_N, ChessColor_AI);

                                                List<Point> ZeroIDList = new List<Point>(80);

                                                for (int X = Math.Max(0, A.X - 4); X < Math.Min(Range.Width, A.X + 5); X++)
                                                {
                                                    for (int Y = Math.Max(0, A.Y - 4); Y < Math.Min(Range.Height, A.Y + 5); Y++)
                                                    {
                                                        if (_Ary_AI[X, Y] == 0)
                                                        {
                                                            ZeroIDList.Add(new Point(X, Y));
                                                        }
                                                    }
                                                }

                                                bool F_Z = false;

                                                foreach (Point A_Z in ZeroIDList)
                                                {
                                                    Int32[,] _Ary_User = GetCopyOfArray(_Ary_AI);

                                                    ArrayLogicalAppend(_Ary_User, Range, A_Z, ChessColor_User);

                                                    StatsInfo _SI_User = GetStatisticalInformationOfArray(_Ary_User, Range, ChessColor_User);

                                                    bool _F_User = Legality(ChessColor_User, _SI_User, A_Z);

                                                    if (_F_User && (_SI_User.CrossIDList_4A_Any.Contains(A_Z) || _SI_User.CrossIDList_4S_4S.Contains(A_Z) || _SI_User.CrossIDList_4S_3A.Contains(A_Z) || _SI_User.CrossIDList_3A_3A.Contains(A_Z)))
                                                    {
                                                        F_Z = true;

                                                        break;
                                                    }
                                                }

                                                if (!F_Z)
                                                {
                                                    F_N = false;

                                                    break;
                                                }
                                            }

                                            Flag = F_N;
                                        }
                                    }
                                    break;

                                //

                                case 18: // 攻：活三-活二
                                    Flag = (F_AI && SI_AI.CrossIDList_3A_2A.Contains(A));
                                    break;

                                case 19: // 攻：活三-眠三、冲四-活二
                                    Flag = (F_AI && (SI_AI.CrossIDList_3A_3S.Contains(A) || SI_AI.CrossIDList_4S_2A.Contains(A)));
                                    break;

                                case 20: // 防：活三-活二
                                    Flag = ((F_AI && F_User) && SI_User.CrossIDList_3A_2A.Contains(A));
                                    break;

                                case 21: // 防：活三-眠三、冲四-活二
                                    Flag = ((F_AI && F_User) && (SI_User.CrossIDList_3A_3S.Contains(A) || SI_User.CrossIDList_4S_2A.Contains(A)));
                                    break;

                                //

                                case 22: // 攻：活三-眠二、冲四-眠三、活二-活二
                                    Flag = (F_AI && (SI_AI.CrossIDList_3A_2S.Contains(A) || SI_AI.CrossIDList_4S_3S.Contains(A) || SI_AI.CrossIDList_2A_2A.Contains(A)));
                                    break;

                                case 23: // 攻：活三、冲四-眠二、眠三-活二
                                    Flag = (F_AI && (IndexIsInChainList(A, SI_AI.Chain_3A) || SI_AI.CrossIDList_4S_2S.Contains(A) || SI_AI.CrossIDList_3S_2A.Contains(A)));
                                    break;

                                case 24: // 防：活三-眠二、冲四-眠三、活二-活二
                                    Flag = ((F_AI && F_User) && (SI_User.CrossIDList_3A_2S.Contains(A) || SI_User.CrossIDList_4S_3S.Contains(A) || SI_User.CrossIDList_2A_2A.Contains(A)));
                                    break;

                                case 25: // 防：活三、冲四-眠二、眠三-活二
                                    Flag = ((F_AI && F_User) && (IndexIsInChainList(A, SI_User.Chain_3A) || SI_User.CrossIDList_4S_2S.Contains(A) || SI_User.CrossIDList_3S_2A.Contains(A)));
                                    break;

                                //

                                case 26: // 攻：冲四、活二-眠二、眠三-眠三
                                    Flag = (F_AI && (IndexIsInChainList(A, SI_AI.Chain_4S) || SI_AI.CrossIDList_2A_2S.Contains(A) || SI_AI.CrossIDList_3S_3S.Contains(A)));
                                    break;

                                case 27: // 防：冲四、活二-眠二、眠三-眠三
                                    Flag = ((F_AI && F_User) && (IndexIsInChainList(A, SI_User.Chain_4S) || SI_User.CrossIDList_2A_2S.Contains(A) || SI_User.CrossIDList_3S_3S.Contains(A)));
                                    break;

                                case 28: // 攻：活二、眠三-眠二
                                    Flag = (F_AI && (IndexIsInChainList(A, SI_AI.Chain_2A) || SI_AI.CrossIDList_3S_2S.Contains(A)));
                                    break;

                                case 29: // 防：活二、眠三-眠二
                                    Flag = ((F_AI && F_User) && (IndexIsInChainList(A, SI_User.Chain_2A) || SI_User.CrossIDList_3S_2S.Contains(A)));
                                    break;

                                case 30: // 攻：眠三、眠二-眠二
                                    Flag = (F_AI && (IndexIsInChainList(A, SI_AI.Chain_3S) || SI_AI.CrossIDList_2S_2S.Contains(A)));
                                    break;

                                case 31: // 防：眠三、眠二-眠二
                                    Flag = ((F_AI && F_User) && (IndexIsInChainList(A, SI_User.Chain_3S) || SI_User.CrossIDList_2S_2S.Contains(A)));
                                    break;

                                case 32: // 攻：眠二
                                    Flag = (F_AI && IndexIsInChainList(A, SI_AI.Chain_2S));
                                    break;

                                case 33: // 防：眠二
                                    Flag = ((F_AI && F_User) && IndexIsInChainList(A, SI_User.Chain_2S));
                                    break;

                                //

                                default:
                                    Flag = true;
                                    break;
                            }
                        }

                        if (Flag)
                        {
                            PRIMin = Math.Min(PRIMin, PRI);

                            SR.PRI = PRI;
                            SR.Value = (F_AI ? (GetPositionValueFromStatsInfo(SI_AI_Self, SI_AI_Opp) + GetPositionValueFromStatsInfo(SI_User_Self, SI_User_Opp)) : double.NegativeInfinity);

                            SrchRsltListAry[ThreadId].Add(SR);
                        }
                    }
                }
            }
            catch { }
        }

        private void AsyncWorkerDecide(object sender, RunWorkerCompletedEventArgs e)
        {
            //
            // 异步分析器分析完成，执行决策与落子。
            //

            try
            {
                bool Flag = true;

                for (int i = 0; i < AsyncWorkerList.Count; i++)
                {
                    if (AsyncWorkerList[i].IsBusy)
                    {
                        Flag = false;

                        break;
                    }
                }

                if (Flag)
                {
                    List<SrchRslt> SrchRsltList = new List<SrchRslt>(0);

                    for (int i = 0; i < SrchRsltListAry.Length; i++)
                    {
                        SrchRsltList.AddRange(SrchRsltListAry[i]);
                    }

                    if (SrchRsltList.Count > 0)
                    {
                        List<Int32> PRIMinIDL = new List<Int32>(SrchRsltList.Count);

                        for (int i = 0; i < SrchRsltList.Count; i++)
                        {
                            if (SrchRsltList[i].PRI == PRIMin)
                            {
                                PRIMinIDL.Add(i);
                            }
                        }

                        Int32 ValueMaxID = PRIMinIDL[0];

                        for (int i = 0; i < PRIMinIDL.Count; i++)
                        {
                            if (SrchRsltList[ValueMaxID].Value < SrchRsltList[PRIMinIDL[i]].Value)
                            {
                                ValueMaxID = PRIMinIDL[i];
                            }
                        }

                        if (Panel_GameUI.Visible && GameState == GameStates.Ongoing)
                        {
                            Lazi(SrchRsltList[ValueMaxID].Index, true);
                        }
                    }
                }
            }
            catch { }
        }

        #endregion

        #region 中断管理

        // 判定。

        private void Judgement()
        {
            //
            // 终局判定。
            //

            if (GameState == GameStates.Ongoing)
            {
                GetGameState();

                if (GameState != GameStates.Ongoing)
                {
                    ChessClockStop();

                    ElementMatrix_RepresentAll();

                    if (SaveRecord)
                    {
                        if (WinningPlayer == Roles.AI)
                        {
                            UserLoseGames += 1;
                        }
                        else if (WinningPlayer == Roles.User)
                        {
                            UserWinGames += 1;
                        }
                        else if (WinningPlayer == Roles.Both)
                        {
                            DrawGames += 1;
                        }
                    }

                    SaveUserData();

                    EraseLastGame();
                }
            }

            //

            RepaintCurBmp();
        }

        // 中断。

        private enum InterruptActions { NULL = -1, StartNew, Continue, Restart, ExitGame, CloseApp, COUNT }; // 中断动作枚举。

        private void Interrupt(InterruptActions IA)
        {
            //
            // 中断。
            //

            switch (IA)
            {
                case InterruptActions.StartNew: // 开始新游戏。
                    {
                        if (SaveRecord_Last && ElementIndexList_Last.Count >= 2)
                        {
                            UserLoseGames += 1;
                        }

                        //

                        EraseLastGame();

                        //

                        GameUIPointedIndex = new Point(-1, -1);

                        //

                        EnterGameUI();

                        //

                        Toss();

                        ChessClockReset();

                        ChessClockStart();

                        if (AIFirst)
                        {
                            AsyncWorkerStart();
                        }
                    }
                    break;

                case InterruptActions.Continue: // 继续上次的游戏。
                    {
                        ChessboardType = ChessboardType_Last;

                        RadioButton_ChessboardType_Gomoku_15x15.CheckedChanged -= RadioButton_ChessboardType_Gomoku_15x15_CheckedChanged;
                        RadioButton_ChessboardType_Go_19x19.CheckedChanged -= RadioButton_ChessboardType_Go_19x19_CheckedChanged;

                        switch (ChessboardType)
                        {
                            case ChessboardTypes.Gomoku_15x15: RadioButton_ChessboardType_Gomoku_15x15.Checked = true; break;
                            case ChessboardTypes.Go_19x19: RadioButton_ChessboardType_Go_19x19.Checked = true; break;
                        }

                        RadioButton_ChessboardType_Gomoku_15x15.CheckedChanged += RadioButton_ChessboardType_Gomoku_15x15_CheckedChanged;
                        RadioButton_ChessboardType_Go_19x19.CheckedChanged += RadioButton_ChessboardType_Go_19x19_CheckedChanged;

                        //

                        GameUIPointedIndex = new Point(-1, -1);

                        //

                        EnterGameUI();

                        //

                        UseBalanceBreaker = UseBalanceBreaker_Last;
                        ShowBanPoint = ShowBanPoint_Last;

                        CheckBox_UseBalanceBreaker.CheckedChanged -= CheckBox_UseBalanceBreaker_CheckedChanged;

                        CheckBox_UseBalanceBreaker.Checked = UseBalanceBreaker;

                        CheckBox_UseBalanceBreaker.CheckedChanged += CheckBox_UseBalanceBreaker_CheckedChanged;

                        ResetBalanceBreakerControls();

                        //

                        AIFirst = AIFirst_Last;

                        FreeTimeRemaining_Black = FreeTimeRemaining_Black_Last;
                        CountdownTimeRemaining_Black = CountdownTimeRemaining_Black_Last;

                        FreeTimeRemaining_White = FreeTimeRemaining_White_Last;
                        CountdownTimeRemaining_White = CountdownTimeRemaining_White_Last;

                        ElementMatrix_Initialize();

                        foreach (Point A in ElementIndexList_Last)
                        {
                            ElementMatrix_Add(A, ElementMatrix_Last[A.X, A.Y]);
                        }

                        ElementMatrix_RepresentAll();

                        Judgement();

                        ChessClockStart();

                        if (CurrentPlayer == Roles.AI)
                        {
                            AsyncWorkerStart();
                        }
                    }
                    break;

                case InterruptActions.Restart: // 重新开始。
                    {
                        EraseLastGame();

                        //

                        GameUIPointedIndex = new Point(-1, -1);

                        //

                        if (SaveRecord && GameState == GameStates.Ongoing && ElementIndexList.Count >= 2)
                        {
                            UserLoseGames += 1;
                        }

                        //

                        SaveUserData();

                        //

                        GameState = GameStates.Ongoing;

                        JdgBasisIDList.Clear();

                        BanIDList.Clear();

                        ThisGameTime = TimeSpan.Zero;

                        ElementMatrix_Initialize();

                        ElementMatrix_RepresentAll();

                        ExchangeChessColor();

                        ChessClockReset();

                        ChessClockStart();

                        if (AIFirst)
                        {
                            AsyncWorkerStart();
                        }

                        //

                        Panel_Environment.Focus();
                    }
                    break;

                case InterruptActions.ExitGame: // 退出游戏。
                    {
                        AsyncWorkerStop();

                        //

                        SaveUserData();

                        //

                        Panel_Environment.Focus();

                        //

                        ChessClockStop();

                        if (GameState == GameStates.Ongoing && ElementIndexList.Count >= 2)
                        {
                            SaveLastGame();
                        }

                        ExitGameUI();
                    }
                    break;

                case InterruptActions.CloseApp: // 关闭程序。
                    {
                        AsyncWorkerStop();

                        //

                        SaveUserData();

                        //

                        ChessClockStop();

                        if (GameState == GameStates.Ongoing && ElementIndexList.Count >= 2)
                        {
                            SaveLastGame();
                        }
                    }
                    break;
            }
        }

        // 中断按钮。

        private static class InterruptImages // 包含表示中断的图像的静态类。
        {
            private static readonly Size _Size = new Size(25, 25);

            private static Bitmap _Restart = null;
            private static Bitmap _ExitGame = null;

            //

            public static Bitmap Restart => _Restart; // 重新开始。
            public static Bitmap ExitGame => _ExitGame; // 退出游戏。

            //

            public static void Update(Color color) // 使用指定的颜色更新所有图像。
            {
                _Restart = new Bitmap(_Size.Width, _Size.Height);

                using (Graphics Grap = Graphics.FromImage(_Restart))
                {
                    Grap.SmoothingMode = SmoothingMode.AntiAlias;

                    Grap.DrawArc(new Pen(color, 2F), new Rectangle(new Point(5, 5), new Size(15, 15)), -150F, 300F);
                    Grap.DrawLines(new Pen(color, 2F), new Point[] { new Point(5, 5), new Point(5, 10), new Point(10, 10) });
                }

                //

                _ExitGame = new Bitmap(_Size.Width, _Size.Height);

                using (Graphics Grap = Graphics.FromImage(_ExitGame))
                {
                    Grap.SmoothingMode = SmoothingMode.AntiAlias;

                    Grap.DrawLine(new Pen(color, 2F), new Point(5, 5), new Point(19, 19));
                    Grap.DrawLine(new Pen(color, 2F), new Point(19, 5), new Point(5, 19));
                }
            }
        }

        private void Label_StartNewGame_Click(object sender, EventArgs e)
        {
            //
            // 单击 Label_StartNewGame。
            //

            Interrupt(InterruptActions.StartNew);
        }

        private void Label_ContinueLastGame_Click(object sender, EventArgs e)
        {
            //
            // 单击 Label_ContinueLastGame。
            //

            Interrupt(InterruptActions.Continue);
        }

        private void PictureBox_Restart_MouseEnter(object sender, EventArgs e)
        {
            //
            // 鼠标进入 PictureBox_Restart。
            //

            ToolTip_InterruptPrompt.RemoveAll();

            ToolTip_InterruptPrompt.SetToolTip(PictureBox_Restart, GameState == GameStates.Ongoing ? (ElementIndexList.Count >= 2 ? (SaveRecord ? "认负并重新开始" : "重新开始") : "交换棋色") : "重新开始");
        }

        private void PictureBox_Restart_Click(object sender, EventArgs e)
        {
            //
            // 单击 PictureBox_Restart。
            //

            Interrupt(InterruptActions.Restart);
        }

        private void PictureBox_ExitGame_MouseEnter(object sender, EventArgs e)
        {
            //
            // 鼠标进入 PictureBox_ExitGame。
            //

            ToolTip_InterruptPrompt.RemoveAll();

            ToolTip_InterruptPrompt.SetToolTip(PictureBox_ExitGame, ((GameState == GameStates.Ongoing && ElementIndexList.Count >= 2) ? "保存并退出" : "退出"));
        }

        private void PictureBox_ExitGame_Click(object sender, EventArgs e)
        {
            //
            // 单击 PictureBox_ExitGame。
            //

            Interrupt(InterruptActions.ExitGame);
        }

        #endregion

        #region UI 切换

        private bool GameUINow = false; // 当前 UI 是否为游戏 UI。

        private void EnterGameUI()
        {
            //
            // 进入游戏 UI。
            //

            GameUINow = true;

            //

            ElementMatrix_Initialize();

            //

            GameState = GameStates.Ongoing;

            JdgBasisIDList.Clear();

            BanIDList.Clear();

            ThisGameTime = TimeSpan.Zero;

            //

            Panel_FunctionArea.Visible = false;
            Panel_GameUI.Visible = true;

            //

            Panel_Environment.Focus();

            //

            while (ElementSize * Range.Width > Screen.PrimaryScreen.WorkingArea.Width || Me.CaptionBarHeight + Panel_Current.Height + ElementSize * Range.Height > Screen.PrimaryScreen.WorkingArea.Height)
            {
                ElementSize = ElementSize * 9 / 10;
            }

            Rectangle NewBounds = new Rectangle();
            NewBounds.Size = new Size(ElementSize * Range.Width, Me.CaptionBarHeight + Panel_Current.Height + ElementSize * Range.Height);
            NewBounds.Location = new Point((Screen.PrimaryScreen.WorkingArea.Width - NewBounds.Width) / 2, (Screen.PrimaryScreen.WorkingArea.Height - NewBounds.Height) / 2);
            Me.Bounds = NewBounds;

            ElementSize = Math.Max(1, Math.Min(Panel_Environment.Width / Range.Width, Panel_Environment.Height / Range.Height));

            EMatBmpRect.Size = new Size(Math.Max(1, ElementSize * Range.Width), Math.Max(1, ElementSize * Range.Height));
            EMatBmpRect.Location = new Point((Panel_Environment.Width - EMatBmpRect.Width) / 2, (Panel_Environment.Height - EMatBmpRect.Height) / 2);

            //

            RepaintCurBmp();

            ElementMatrix_RepresentAll();
        }

        private void ExitGameUI()
        {
            //
            // 退出游戏 UI。
            //

            GameUINow = false;

            //

            Panel_FunctionArea.Visible = true;
            Panel_GameUI.Visible = false;

            //

            ElementMatrix_Initialize();

            //

            Rectangle NewBounds = new Rectangle();
            NewBounds.Size = new Size(FormClientInitialSize.Width, Me.CaptionBarHeight + FormClientInitialSize.Height);
            NewBounds.Location = new Point((Screen.PrimaryScreen.WorkingArea.Width - NewBounds.Width) / 2, (Screen.PrimaryScreen.WorkingArea.Height - NewBounds.Height) / 2);
            Me.Bounds = NewBounds;

            //

            FunctionAreaTab = FunctionAreaTabs.Start;
        }

        #endregion

        #region 游戏 UI 交互

        private Point GameUIPointedIndex = new Point(-1, -1); // 鼠标指向的索引。

        private void Panel_Environment_MouseMove(object sender, MouseEventArgs e)
        {
            //
            // 鼠标经过 Panel_Environment。
            //

            if (Me.IsActive)
            {
                Panel_Environment.Focus();
            }

            //

            if (GameState == GameStates.Ongoing)
            {
                if (CurrentPlayer == Roles.User)
                {
                    Point A = ElementMatrix_GetIndex(Com.Geometry.GetCursorPositionOfControl(Panel_Environment));

                    if (UseBalanceBreaker && ElementIndexList.Count == 0)
                    {
                        if (ElementMatrix_IndexValid(A))
                        {
                            A = Center;
                        }
                        else
                        {
                            A = new Point(-1, -1);
                        }
                    }

                    if (GameUIPointedIndex != A)
                    {
                        Point LastPointedIndex = GameUIPointedIndex;

                        if (ElementMatrix_IndexValid(A) && ElementMatrix_GetValue(A) == 0)
                        {
                            GameUIPointedIndex = A;
                        }
                        else
                        {
                            GameUIPointedIndex = new Point(-1, -1);
                        }

                        if (ElementMatrix_IndexValid(LastPointedIndex) && ElementMatrix_GetValue(LastPointedIndex) == 0)
                        {
                            ElementMatrix_PresentAt(LastPointedIndex);
                        }

                        if (ElementMatrix_IndexValid(GameUIPointedIndex) && ElementMatrix_GetValue(GameUIPointedIndex) == 0)
                        {
                            ElementMatrix_PresentAt(GameUIPointedIndex);
                        }
                    }
                }
            }
        }

        private void Panel_Environment_MouseLeave(object sender, EventArgs e)
        {
            //
            // 鼠标离开 Panel_Environment。
            //

            if (GameState == GameStates.Ongoing)
            {
                if (CurrentPlayer == Roles.User)
                {
                    Point LastPointedIndex = GameUIPointedIndex;
                    GameUIPointedIndex = new Point(-1, -1);

                    if (ElementMatrix_IndexValid(LastPointedIndex) && ElementMatrix_GetValue(LastPointedIndex) == 0)
                    {
                        ElementMatrix_PresentAt(LastPointedIndex);
                    }
                }
            }
        }

        private void Panel_Environment_MouseDown(object sender, MouseEventArgs e)
        {
            //
            // 鼠标按下 Panel_Environment。
            //

            if (GameState == GameStates.Ongoing && e.Button == MouseButtons.Left)
            {
                if (CurrentPlayer == Roles.User)
                {
                    Point A = ElementMatrix_GetIndex(Com.Geometry.GetCursorPositionOfControl(Panel_Environment));

                    if (UseBalanceBreaker && ElementIndexList.Count == 0)
                    {
                        A = Center;
                    }

                    Lazi(A, false);

                    GameUIPointedIndex = new Point(-1, -1);
                }
            }
        }

        private void Panel_Environment_KeyDown(object sender, KeyEventArgs e)
        {
            //
            // 在 Panel_Environment 按下键。
            //

            switch (e.KeyCode)
            {
                case Keys.Home: Interrupt(InterruptActions.Restart); break;
                case Keys.End:
                case Keys.Escape: Interrupt(InterruptActions.ExitGame); break;
            }
        }

        #endregion

        #region 鼠标滚轮功能

        private void Panel_FunctionAreaOptionsBar_MouseWheel(object sender, MouseEventArgs e)
        {
            //
            // 鼠标滚轮在 Panel_FunctionAreaOptionsBar 滚动。
            //

            if (e.Delta < 0 && (Int32)FunctionAreaTab < (Int32)FunctionAreaTabs.COUNT - 1)
            {
                FunctionAreaTab++;
            }
            else if (e.Delta > 0 && (Int32)FunctionAreaTab > 0)
            {
                FunctionAreaTab--;
            }
        }

        private void Panel_Environment_MouseWheel(object sender, MouseEventArgs e)
        {
            //
            // 鼠标滚轮在 Panel_Environment 滚动。
            //

            Rectangle NewBounds = Me.Bounds;

            if (Range.Width <= Range.Height)
            {
                if (e.Delta > 0)
                {
                    NewBounds.Location = new Point(NewBounds.X - NewBounds.Width / 20, NewBounds.Y - NewBounds.Width / 20 * Range.Height / Range.Width);
                    NewBounds.Size = new Size(NewBounds.Width + NewBounds.Width / 20 * 2, NewBounds.Height + NewBounds.Width / 20 * Range.Height / Range.Width * 2);
                }
                else if (e.Delta < 0)
                {
                    NewBounds.Location = new Point(NewBounds.X + NewBounds.Width / 20, NewBounds.Y + NewBounds.Width / 20 * Range.Height / Range.Width);
                    NewBounds.Size = new Size(NewBounds.Width - NewBounds.Width / 20 * 2, NewBounds.Height - NewBounds.Width / 20 * Range.Height / Range.Width * 2);
                }
            }
            else
            {
                if (e.Delta > 0)
                {
                    NewBounds.Location = new Point(NewBounds.X - NewBounds.Height / 20 * Range.Width / Range.Height, NewBounds.Y - NewBounds.Height / 20);
                    NewBounds.Size = new Size(NewBounds.Width + NewBounds.Height / 20 * Range.Width / Range.Height * 2, NewBounds.Height + NewBounds.Height / 20 * 2);
                }
                else if (e.Delta < 0)
                {
                    NewBounds.Location = new Point(NewBounds.X + NewBounds.Height / 20 * Range.Width / Range.Height, NewBounds.Y + NewBounds.Height / 20);
                    NewBounds.Size = new Size(NewBounds.Width - NewBounds.Height / 20 * Range.Width / Range.Height * 2, NewBounds.Height - NewBounds.Height / 20 * 2);
                }
            }

            NewBounds.Location = new Point(Math.Max(0, Math.Min(Screen.PrimaryScreen.WorkingArea.Width - NewBounds.Width, NewBounds.X)), Math.Max(0, Math.Min(Screen.PrimaryScreen.WorkingArea.Height - NewBounds.Height, NewBounds.Y)));

            Me.Bounds = NewBounds;
        }

        #endregion

        #region 计分栏

        private Bitmap CurBmp; // 计分栏位图。

        private Graphics CurBmpGrap; // 计分栏位图绘图。

        private void UpdateCurBmp()
        {
            //
            // 更新计分栏位图。
            //

            if (CurBmp != null)
            {
                CurBmp.Dispose();
            }

            CurBmp = new Bitmap(Math.Max(1, Panel_Current.Width), Math.Max(1, Panel_Current.Height));

            CurBmpGrap = Graphics.FromImage(CurBmp);

            if (AntiAlias)
            {
                CurBmpGrap.SmoothingMode = SmoothingMode.AntiAlias;
                CurBmpGrap.TextRenderingHint = TextRenderingHint.AntiAlias;
            }

            CurBmpGrap.Clear(GameUIBackColor_DEC);

            //

            double TimeRemainingPct = 0;

            if (FreeTimeRemaining_Current.TotalMilliseconds > 0)
            {
                TimeRemainingPct = FreeTimeRemaining_Current.TotalMilliseconds / TotalFreeTime.TotalMilliseconds;
            }
            else
            {
                TimeRemainingPct = TimeSpan.FromMilliseconds((Int32)CountdownTimeRemaining_Current.TotalMilliseconds % 60001).TotalMilliseconds / TimeSpan.FromMinutes(1).TotalMilliseconds;
            }

            Rectangle Rect_Total = new Rectangle(new Point(0, 0), new Size(Math.Max(1, Panel_Current.Width), Math.Max(1, Panel_Current.Height)));
            Rectangle Rect_Current = new Rectangle(Rect_Total.Location, new Size((Int32)Math.Max(2, TimeRemainingPct * Rect_Total.Width), Rect_Total.Height));

            Color RectCr_Total = Me.RecommendColors.Background.ToColor(), RectCr_Current = Me.RecommendColors.Border.ToColor();

            GraphicsPath Path_Total = new GraphicsPath();
            Path_Total.AddRectangle(Rect_Total);
            PathGradientBrush PGB_Total = new PathGradientBrush(Path_Total)
            {
                CenterColor = RectCr_Total,
                SurroundColors = new Color[] { Com.ColorManipulation.ShiftLightnessByHSL(RectCr_Total, 0.3) },
                FocusScales = new PointF(1F, 0F)
            };
            CurBmpGrap.FillPath(PGB_Total, Path_Total);
            Path_Total.Dispose();
            PGB_Total.Dispose();

            GraphicsPath Path_Current = new GraphicsPath();
            Path_Current.AddRectangle(Rect_Current);
            PathGradientBrush PGB_Current = new PathGradientBrush(Path_Current)
            {
                CenterColor = RectCr_Current,
                SurroundColors = new Color[] { Com.ColorManipulation.ShiftLightnessByHSL(RectCr_Current, 0.3) },
                FocusScales = new PointF(1F, 0F)
            };
            CurBmpGrap.FillPath(PGB_Current, Path_Current);
            Path_Current.Dispose();
            PGB_Current.Dispose();

            //

            SizeF RegionSize_L = new SizeF(), RegionSize_R = new SizeF();
            RectangleF RegionRect = new RectangleF();

            const Int32 Margin_Chess_L = 5, Margin_Name_L = 5, Margin_Time_L = 5, Margin_Result_L = 5;

            Color Cr_Black = GlobalColors.BlackChess;
            Color Cr_White = GlobalColors.WhiteChess;
            Color Cr_Win = GlobalColors.Win;
            Color Cr_Draw = GlobalColors.Draw;
            Color Cr_Lose = GlobalColors.Lose;

            RectangleF Rect_Black_Polygon = new RectangleF(), Rect_Black_Chess = new RectangleF(), Rect_White_Polygon = new RectangleF(), Rect_White_Chess = new RectangleF();
            Rect_Black_Polygon.Size = Rect_White_Polygon.Size = new SizeF(14, 27);
            Rect_Black_Chess.Size = Rect_White_Chess.Size = new SizeF(35, 35);

            string StringText_UserName_Black = (AIFirst ? AIName : UserName);
            Color StringColor_UserName_Black = Me.RecommendColors.Text.ToColor();
            Font StringFont_UserName_Black = new Font("微软雅黑", 12F, FontStyle.Regular, GraphicsUnit.Point, 134);
            RectangleF StringRect_UserName_Black = new RectangleF();
            StringRect_UserName_Black.Size = CurBmpGrap.MeasureString(StringText_UserName_Black, StringFont_UserName_Black);

            TimeSpan TS_Black = (FreeTimeRemaining_Black.TotalMilliseconds > 0 ? FreeTimeRemaining_Black : TimeSpan.FromMilliseconds((Int32)CountdownTimeRemaining_Black.TotalMilliseconds % 60001));
            string StringText_TimeRemaining_Black = TS_Black.Hours.ToString("D2") + ":" + TS_Black.Minutes.ToString("D2") + ":" + TS_Black.Seconds.ToString("D2");
            Color StringColor_TimeRemaining_Black = Me.RecommendColors.Text_INC.ToColor();
            Font StringFont_TimeRemaining_Black = new Font("微软雅黑", 12F, FontStyle.Bold, GraphicsUnit.Point, 134);
            RectangleF StringRect_TimeRemaining_Black = new RectangleF();
            StringRect_TimeRemaining_Black.Size = CurBmpGrap.MeasureString(StringText_TimeRemaining_Black, StringFont_TimeRemaining_Black);

            string StringText_WinningState_Black = string.Empty;
            Color StringColor_WinningState_Black = Color.Empty;
            Font StringFont_WinningState_Black = null;
            RectangleF StringRect_WinningState_Black = new RectangleF();

            if (WinningColor != Roles.NULL)
            {
                StringText_WinningState_Black = (WinningColor == Roles.Black ? "胜" : (WinningColor == Roles.White ? "负" : "和"));
                StringColor_WinningState_Black = (WinningColor == Roles.Black ? Cr_Win : (WinningColor == Roles.White ? Cr_Lose : Cr_Draw));
                StringFont_WinningState_Black = new Font("微软雅黑", 21.75F, FontStyle.Regular, GraphicsUnit.Point, 134);
                StringRect_WinningState_Black.Size = CurBmpGrap.MeasureString(StringText_WinningState_Black, StringFont_WinningState_Black);
            }

            string StringText_UserName_White = (AIFirst ? UserName : AIName);
            Color StringColor_UserName_White = Me.RecommendColors.Text.ToColor();
            Font StringFont_UserName_White = new Font("微软雅黑", 12F, FontStyle.Regular, GraphicsUnit.Point, 134);
            RectangleF StringRect_UserName_White = new RectangleF();
            StringRect_UserName_White.Size = CurBmpGrap.MeasureString(StringText_UserName_White, StringFont_UserName_White);

            TimeSpan TS_White = (FreeTimeRemaining_White.TotalMilliseconds > 0 ? FreeTimeRemaining_White : TimeSpan.FromMilliseconds((Int32)CountdownTimeRemaining_White.TotalMilliseconds % 60001));
            string StringText_TimeRemaining_White = TS_White.Hours.ToString("D2") + ":" + TS_White.Minutes.ToString("D2") + ":" + TS_White.Seconds.ToString("D2");
            Color StringColor_TimeRemaining_White = Me.RecommendColors.Text_INC.ToColor();
            Font StringFont_TimeRemaining_White = new Font("微软雅黑", 12F, FontStyle.Bold, GraphicsUnit.Point, 134);
            RectangleF StringRect_TimeRemaining_White = new RectangleF();
            StringRect_TimeRemaining_White.Size = CurBmpGrap.MeasureString(StringText_TimeRemaining_White, StringFont_TimeRemaining_White);

            string StringText_WinningState_White = string.Empty;
            Color StringColor_WinningState_White = Color.Empty;
            Font StringFont_WinningState_White = null;
            RectangleF StringRect_WinningState_White = new RectangleF();

            if (WinningColor != Roles.NULL)
            {
                StringText_WinningState_White = (WinningColor == Roles.White ? "胜" : (WinningColor == Roles.Black ? "负" : "和"));
                StringColor_WinningState_White = (WinningColor == Roles.White ? Cr_Win : (WinningColor == Roles.Black ? Cr_Lose : Cr_Draw));
                StringFont_WinningState_White = new Font("微软雅黑", 21.75F, FontStyle.Regular, GraphicsUnit.Point, 134);
                StringRect_WinningState_White.Size = CurBmpGrap.MeasureString(StringText_WinningState_White, StringFont_WinningState_White);
            }

            RegionSize_L = new SizeF(Rect_Black_Polygon.Width + Margin_Chess_L + Rect_Black_Chess.Width + Margin_Name_L + Math.Max(StringRect_UserName_Black.Width, StringRect_TimeRemaining_Black.Width) + Margin_Result_L + StringRect_WinningState_Black.Width, 0);
            RegionSize_R = new SizeF(Rect_White_Polygon.Width + Margin_Chess_L + Rect_White_Chess.Width + Margin_Name_L + Math.Max(StringRect_UserName_White.Width, StringRect_TimeRemaining_White.Width) + Margin_Result_L + StringRect_WinningState_White.Width, 0);

            RegionRect.Size = new SizeF(Math.Max(RegionSize_L.Width + RegionSize_R.Width, Math.Min(EMatBmpRect.Width, Panel_Interrupt.Left - EMatBmpRect.X)), Panel_Current.Height);
            RegionRect.Location = new PointF(Math.Max(0, Math.Min(EMatBmpRect.X + (EMatBmpRect.Width - RegionRect.Width) / 2, Panel_Interrupt.Left - RegionRect.Width)), 0);

            Rect_Black_Polygon.Location = new PointF(RegionRect.X, 9);
            Rect_Black_Chess.Location = new PointF(Rect_Black_Polygon.Right + Margin_Chess_L, 5);

            CurBmpGrap.FillEllipse(new LinearGradientBrush(new PointF(Rect_Black_Chess.X - 1, Rect_Black_Chess.Y - 1), new PointF(Rect_Black_Chess.Right, Rect_Black_Chess.Bottom), Com.ColorManipulation.ShiftLightnessByHSL(Cr_Black, 0.3), Cr_Black), Rect_Black_Chess);

            if (FreeTimeRemaining_Black.TotalMilliseconds <= 0)
            {
                RectangleF[] Rect_Black_Countdown = new RectangleF[] { new RectangleF(new PointF(Rect_Black_Chess.X + 5, Rect_Black_Chess.Y + Rect_Black_Chess.Height + 2), new SizeF(5, 5)), new RectangleF(new PointF(Rect_Black_Chess.X + 15, Rect_Black_Chess.Y + Rect_Black_Chess.Height + 2), new SizeF(5, 5)), new RectangleF(new PointF(Rect_Black_Chess.X + 25, Rect_Black_Chess.Y + Rect_Black_Chess.Height + 2), new SizeF(5, 5)) };

                Int32 MinutesRemaining_Black = (Int32)Math.Ceiling(CountdownTimeRemaining_Black.TotalSeconds / 60);

                for (int i = 0; i < Math.Min(Rect_Black_Countdown.Length, MinutesRemaining_Black); i++)
                {
                    RectangleF R = Rect_Black_Countdown[i];

                    Color Cr_Countdown = Com.ColorManipulation.ShiftLightnessByHSL(Me.RecommendColors.Text, 0.25).ToColor();

                    CurBmpGrap.FillEllipse(new SolidBrush(Cr_Countdown), R);
                }
            }

            StringRect_UserName_Black.Location = new PointF(Rect_Black_Chess.Right + Margin_Name_L, (RegionRect.Height / 2 - StringRect_UserName_Black.Height) / 2);
            StringRect_TimeRemaining_Black.Location = new PointF(Rect_Black_Chess.Right + Margin_Time_L, RegionRect.Height / 2 + (RegionRect.Height / 2 - StringRect_TimeRemaining_Black.Height) / 2);

            Com.Painting2D.PaintTextWithShadow(CurBmp, StringText_UserName_Black, StringFont_UserName_Black, StringColor_UserName_Black, StringColor_UserName_Black, StringRect_UserName_Black.Location, 0.1F, AntiAlias);
            Com.Painting2D.PaintTextWithShadow(CurBmp, StringText_TimeRemaining_Black, StringFont_TimeRemaining_Black, StringColor_TimeRemaining_Black, StringColor_TimeRemaining_Black, StringRect_TimeRemaining_Black.Location, 0.1F, AntiAlias);

            Rect_White_Polygon.Location = new PointF(RegionRect.X + RegionRect.Width / 2, 9);
            Rect_White_Chess.Location = new PointF(Rect_White_Polygon.Right + Margin_Chess_L, 5);

            CurBmpGrap.FillEllipse(new LinearGradientBrush(new PointF(Rect_White_Chess.X - 1, Rect_White_Chess.Y - 1), new PointF(Rect_White_Chess.Right, Rect_White_Chess.Bottom), Cr_White, Com.ColorManipulation.ShiftLightnessByHSL(Cr_White, -0.3)), Rect_White_Chess);

            if (FreeTimeRemaining_White.TotalMilliseconds <= 0)
            {
                RectangleF[] Rect_White_Countdown = new RectangleF[] { new RectangleF(new PointF(Rect_White_Chess.X + 5, Rect_White_Chess.Y + Rect_White_Chess.Height + 2), new SizeF(5, 5)), new RectangleF(new PointF(Rect_White_Chess.X + 15, Rect_White_Chess.Bottom + 2), new SizeF(5, 5)), new RectangleF(new PointF(Rect_White_Chess.X + 25, Rect_White_Chess.Bottom + 2), new SizeF(5, 5)) };

                Int32 MinutesRemaining_White = (Int32)Math.Ceiling(CountdownTimeRemaining_White.TotalSeconds / 60);

                for (int i = 0; i < Math.Min(Rect_White_Countdown.Length, MinutesRemaining_White); i++)
                {
                    RectangleF R = Rect_White_Countdown[i];

                    Color Cr_Countdown = Com.ColorManipulation.ShiftLightnessByHSL(Me.RecommendColors.Text, 0.25).ToColor();

                    CurBmpGrap.FillEllipse(new SolidBrush(Cr_Countdown), R);
                }
            }

            StringRect_UserName_White.Location = new PointF(Rect_White_Chess.Right + Margin_Name_L, (RegionRect.Height / 2 - StringRect_UserName_White.Height) / 2);
            StringRect_TimeRemaining_White.Location = new PointF(Rect_White_Chess.Right + Margin_Time_L, RegionRect.Height / 2 + (RegionRect.Height / 2 - StringRect_TimeRemaining_White.Height) / 2);

            Com.Painting2D.PaintTextWithShadow(CurBmp, StringText_UserName_White, StringFont_UserName_White, StringColor_UserName_White, StringColor_UserName_White, StringRect_UserName_White.Location, 0.1F, AntiAlias);
            Com.Painting2D.PaintTextWithShadow(CurBmp, StringText_TimeRemaining_White, StringFont_TimeRemaining_White, StringColor_TimeRemaining_White, StringColor_TimeRemaining_White, StringRect_TimeRemaining_White.Location, 0.1F, AntiAlias);

            if (GameState == GameStates.Ongoing)
            {
                RectangleF Rect_Polygon = (CurrentColor == Roles.Black ? Rect_Black_Polygon : Rect_White_Polygon);

                CurBmpGrap.FillPolygon(new LinearGradientBrush(new PointF(Rect_Polygon.X - 1, Rect_Polygon.Y), new PointF(Rect_Polygon.Right, Rect_Polygon.Y), Color.Transparent, Com.ColorManipulation.BlendByRGB(Me.RecommendColors.Text_DEC, Me.RecommendColors.Text_INC, Math.Abs(Environment.TickCount * 0.001 - ((Int32)Math.Ceiling(Environment.TickCount * 0.001) / 2) * 2)).ToColor()), new PointF[] { new PointF(Rect_Polygon.X, Rect_Polygon.Y), new PointF(Rect_Polygon.Right, Rect_Polygon.Y + Rect_Polygon.Height / 2), new PointF(Rect_Polygon.X, Rect_Polygon.Bottom) });
            }

            if (WinningColor != Roles.NULL)
            {
                StringRect_WinningState_Black.Location = new PointF(Math.Max(StringRect_UserName_Black.Right, StringRect_TimeRemaining_Black.Right) + Margin_Result_L, (RegionRect.Height - StringRect_WinningState_Black.Height) / 2);
                StringRect_WinningState_White.Location = new PointF(Math.Max(StringRect_UserName_White.Right, StringRect_TimeRemaining_White.Right) + Margin_Result_L, (RegionRect.Height - StringRect_WinningState_White.Height) / 2);

                Com.Painting2D.PaintTextWithShadow(CurBmp, StringText_WinningState_Black, StringFont_WinningState_Black, StringColor_WinningState_Black, StringColor_WinningState_Black, StringRect_WinningState_Black.Location, 0.05F, AntiAlias);
                Com.Painting2D.PaintTextWithShadow(CurBmp, StringText_WinningState_White, StringFont_WinningState_White, StringColor_WinningState_White, StringColor_WinningState_White, StringRect_WinningState_White.Location, 0.05F, AntiAlias);
            }
        }

        private void RepaintCurBmp()
        {
            //
            // 更新并重绘计分栏位图。
            //

            UpdateCurBmp();

            if (CurBmp != null)
            {
                Panel_Current.CreateGraphics().DrawImage(CurBmp, new Point(0, 0));

                foreach (object Obj in Panel_Current.Controls)
                {
                    ((Control)Obj).Refresh();
                }
            }
        }

        private void Panel_Current_Paint(object sender, PaintEventArgs e)
        {
            //
            // Panel_Current 绘图。
            //

            UpdateCurBmp();

            if (CurBmp != null)
            {
                e.Graphics.DrawImage(CurBmp, new Point(0, 0));
            }
        }

        #endregion

        #region 功能区

        private enum FunctionAreaTabs { NULL = -1, Start, Record, Options, About, COUNT } // 功能区选项卡枚举。

        private FunctionAreaTabs _FunctionAreaTab = FunctionAreaTabs.NULL; // 当前打开的功能区选项卡。
        private FunctionAreaTabs FunctionAreaTab
        {
            get
            {
                return _FunctionAreaTab;
            }

            set
            {
                _FunctionAreaTab = value;

                Color TabBtnCr_Fr_Seld = Me.RecommendColors.Main_INC.ToColor(), TabBtnCr_Fr_Uns = Color.White;
                Color TabBtnCr_Bk_Seld = Color.Transparent, TabBtnCr_Bk_Uns = Color.Transparent;
                Font TabBtnFt_Seld = new Font("微软雅黑", 11.25F, FontStyle.Bold, GraphicsUnit.Point, 134), TabBtnFt_Uns = new Font("微软雅黑", 11.25F, FontStyle.Regular, GraphicsUnit.Point, 134);

                Label_Tab_Start.ForeColor = (_FunctionAreaTab == FunctionAreaTabs.Start ? TabBtnCr_Fr_Seld : TabBtnCr_Fr_Uns);
                Label_Tab_Start.BackColor = (_FunctionAreaTab == FunctionAreaTabs.Start ? TabBtnCr_Bk_Seld : TabBtnCr_Bk_Uns);
                Label_Tab_Start.Font = (_FunctionAreaTab == FunctionAreaTabs.Start ? TabBtnFt_Seld : TabBtnFt_Uns);

                Label_Tab_Record.ForeColor = (_FunctionAreaTab == FunctionAreaTabs.Record ? TabBtnCr_Fr_Seld : TabBtnCr_Fr_Uns);
                Label_Tab_Record.BackColor = (_FunctionAreaTab == FunctionAreaTabs.Record ? TabBtnCr_Bk_Seld : TabBtnCr_Bk_Uns);
                Label_Tab_Record.Font = (_FunctionAreaTab == FunctionAreaTabs.Record ? TabBtnFt_Seld : TabBtnFt_Uns);

                Label_Tab_Options.ForeColor = (_FunctionAreaTab == FunctionAreaTabs.Options ? TabBtnCr_Fr_Seld : TabBtnCr_Fr_Uns);
                Label_Tab_Options.BackColor = (_FunctionAreaTab == FunctionAreaTabs.Options ? TabBtnCr_Bk_Seld : TabBtnCr_Bk_Uns);
                Label_Tab_Options.Font = (_FunctionAreaTab == FunctionAreaTabs.Options ? TabBtnFt_Seld : TabBtnFt_Uns);

                Label_Tab_About.ForeColor = (_FunctionAreaTab == FunctionAreaTabs.About ? TabBtnCr_Fr_Seld : TabBtnCr_Fr_Uns);
                Label_Tab_About.BackColor = (_FunctionAreaTab == FunctionAreaTabs.About ? TabBtnCr_Bk_Seld : TabBtnCr_Bk_Uns);
                Label_Tab_About.Font = (_FunctionAreaTab == FunctionAreaTabs.About ? TabBtnFt_Seld : TabBtnFt_Uns);

                switch (_FunctionAreaTab)
                {
                    case FunctionAreaTabs.Start:
                        {
                            if (ElementIndexList_Last.Count >= 2)
                            {
                                Label_ContinueLastGame.Visible = true;

                                Label_ContinueLastGame.Focus();

                                Label_StartNewGame.Text = (SaveRecord_Last ? "认负并开始新游戏" : "开始新游戏");
                            }
                            else
                            {
                                Label_ContinueLastGame.Visible = false;

                                Label_StartNewGame.Focus();

                                Label_StartNewGame.Text = "开始新游戏";
                            }
                        }
                        break;

                    case FunctionAreaTabs.Record:
                        {
                            if (TotalGames == 0)
                            {
                                Label_TotalGamesVal.Text = "无记录";
                                Label_UserWinGamesVal.Text = "无";
                                Label_UserLoseGamesVal.Text = "无";
                                Label_DrawGamesVal.Text = "无";
                            }
                            else
                            {
                                Label_TotalGamesVal.Text = TotalGames.ToString();
                                Label_UserWinGamesVal.Text = UserWinGames + " (" + ((double)UserWinGames / TotalGames * 100).ToString("N1") + "%)";
                                Label_UserLoseGamesVal.Text = UserLoseGames + " (" + ((double)UserLoseGames / TotalGames * 100).ToString("N1") + "%)";
                                Label_DrawGamesVal.Text = DrawGames + " (" + ((double)DrawGames / TotalGames * 100).ToString("N1") + "%)";
                            }

                            Label_ThisTimeVal.Text = Com.Text.GetTimeStringFromTimeSpan(ThisGameTime);
                            Label_TotalTimeVal.Text = Com.Text.GetTimeStringFromTimeSpan(TotalGameTime);
                        }
                        break;

                    case FunctionAreaTabs.Options:
                        {

                        }
                        break;

                    case FunctionAreaTabs.About:
                        {

                        }
                        break;
                }

                Timer_EnterPrompt.Enabled = (_FunctionAreaTab == FunctionAreaTabs.Start);

                if (Panel_FunctionAreaTab.AutoScroll)
                {
                    // Panel 的 AutoScroll 功能似乎存在 bug，下面的代码可以规避某些显示问题

                    Panel_FunctionAreaTab.AutoScroll = false;

                    foreach (object Obj in Panel_FunctionAreaTab.Controls)
                    {
                        if (Obj is Panel)
                        {
                            Panel Pnl = Obj as Panel;

                            Pnl.Location = new Point(0, 0);
                        }
                    }

                    Panel_FunctionAreaTab.AutoScroll = true;
                }

                Panel_Tab_Start.Visible = (_FunctionAreaTab == FunctionAreaTabs.Start);
                Panel_Tab_Record.Visible = (_FunctionAreaTab == FunctionAreaTabs.Record);
                Panel_Tab_Options.Visible = (_FunctionAreaTab == FunctionAreaTabs.Options);
                Panel_Tab_About.Visible = (_FunctionAreaTab == FunctionAreaTabs.About);
            }
        }

        private void Label_Tab_MouseEnter(object sender, EventArgs e)
        {
            //
            // 鼠标进入 Label_Tab。
            //

            Panel_FunctionAreaOptionsBar.Refresh();
        }

        private void Label_Tab_MouseLeave(object sender, EventArgs e)
        {
            //
            // 鼠标离开 Label_Tab。
            //

            Panel_FunctionAreaOptionsBar.Refresh();
        }

        private void Label_Tab_Start_MouseDown(object sender, MouseEventArgs e)
        {
            //
            // 鼠标按下 Label_Tab_Start。
            //

            if (e.Button == MouseButtons.Left)
            {
                if (FunctionAreaTab != FunctionAreaTabs.Start)
                {
                    FunctionAreaTab = FunctionAreaTabs.Start;
                }
            }
        }

        private void Label_Tab_Record_MouseDown(object sender, MouseEventArgs e)
        {
            //
            // 鼠标按下 Label_Tab_Record。
            //

            if (e.Button == MouseButtons.Left)
            {
                if (FunctionAreaTab != FunctionAreaTabs.Record)
                {
                    FunctionAreaTab = FunctionAreaTabs.Record;
                }
            }
        }

        private void Label_Tab_Options_MouseDown(object sender, MouseEventArgs e)
        {
            //
            // 鼠标按下 Label_Tab_Options。
            //

            if (e.Button == MouseButtons.Left)
            {
                if (FunctionAreaTab != FunctionAreaTabs.Options)
                {
                    FunctionAreaTab = FunctionAreaTabs.Options;
                }
            }
        }

        private void Label_Tab_About_MouseDown(object sender, MouseEventArgs e)
        {
            //
            // 鼠标按下 Label_Tab_About。
            //

            if (e.Button == MouseButtons.Left)
            {
                if (FunctionAreaTab != FunctionAreaTabs.About)
                {
                    FunctionAreaTab = FunctionAreaTabs.About;
                }
            }
        }

        #endregion

        #region "开始"区域

        private const Int32 EnterGameButtonHeight_Min = 30, EnterGameButtonHeight_Max = 50; // 进入游戏按钮高度的取值范围。

        private Color EnterGameBackColor_INC = Color.Empty; // Panel_EnterGameSelection 绘图使用的颜色（深色）。
        private Color EnterGameBackColor_DEC => Panel_FunctionArea.BackColor; // Panel_EnterGameSelection 绘图使用的颜色（浅色）。

        private void Panel_EnterGameSelection_Paint(object sender, PaintEventArgs e)
        {
            //
            // Panel_EnterGameSelection 绘图。
            //

            Rectangle Rect_StartNew = new Rectangle(Label_StartNewGame.Location, Label_StartNewGame.Size);

            Color Cr_StartNew = Com.ColorManipulation.BlendByRGB(EnterGameBackColor_INC, EnterGameBackColor_DEC, Math.Sqrt((double)(Label_StartNewGame.Height - EnterGameButtonHeight_Min) / (EnterGameButtonHeight_Max - EnterGameButtonHeight_Min)));

            GraphicsPath Path_StartNew = new GraphicsPath();
            Path_StartNew.AddRectangle(Rect_StartNew);
            PathGradientBrush PGB_StartNew = new PathGradientBrush(Path_StartNew)
            {
                CenterColor = Cr_StartNew,
                SurroundColors = new Color[] { Com.ColorManipulation.BlendByRGB(Cr_StartNew, EnterGameBackColor_DEC, 0.7) },
                FocusScales = new PointF(1F, 0F)
            };
            e.Graphics.FillPath(PGB_StartNew, Path_StartNew);
            Path_StartNew.Dispose();
            PGB_StartNew.Dispose();

            //

            if (Label_ContinueLastGame.Visible)
            {
                Rectangle Rect_Continue = new Rectangle(Label_ContinueLastGame.Location, Label_ContinueLastGame.Size);

                Color Cr_Continue = Com.ColorManipulation.BlendByRGB(EnterGameBackColor_INC, EnterGameBackColor_DEC, Math.Sqrt((double)(Label_ContinueLastGame.Height - EnterGameButtonHeight_Min) / (EnterGameButtonHeight_Max - EnterGameButtonHeight_Min)));

                GraphicsPath Path_Continue = new GraphicsPath();
                Path_Continue.AddRectangle(Rect_Continue);
                PathGradientBrush PGB_Continue = new PathGradientBrush(Path_Continue)
                {
                    CenterColor = Cr_Continue,
                    SurroundColors = new Color[] { Com.ColorManipulation.BlendByRGB(Cr_Continue, EnterGameBackColor_DEC, 0.7) },
                    FocusScales = new PointF(1F, 0F)
                };
                e.Graphics.FillPath(PGB_Continue, Path_Continue);
                Path_Continue.Dispose();
                PGB_Continue.Dispose();
            }
        }

        private double EnterPrompt_Val = 0; // 闪烁相位。
        private double EnterPrompt_Step = 0.025; // 闪烁步长。

        private void Timer_EnterPrompt_Tick(object sender, EventArgs e)
        {
            //
            // Timer_EnterPrompt。
            //

            if (EnterPrompt_Val >= 0 && EnterPrompt_Val <= 1)
            {
                EnterPrompt_Val += EnterPrompt_Step;
            }

            if (EnterPrompt_Val < 0 || EnterPrompt_Val > 1)
            {
                EnterPrompt_Val = Math.Max(0, Math.Min(EnterPrompt_Val, 1));

                EnterPrompt_Step = -EnterPrompt_Step;
            }

            EnterGameBackColor_INC = Com.ColorManipulation.BlendByRGB(Me.RecommendColors.Border_INC, Me.RecommendColors.Border, EnterPrompt_Val).ToColor();

            //

            if (Label_ContinueLastGame.Visible)
            {
                Label_StartNewGame.Top = 0;

                if (Com.Geometry.CursorIsInControl(Label_StartNewGame))
                {
                    Label_StartNewGame.Height = Math.Max(EnterGameButtonHeight_Min, Math.Min(EnterGameButtonHeight_Max, Label_StartNewGame.Height + Math.Max(1, (EnterGameButtonHeight_Max - Label_StartNewGame.Height) / 4)));
                }
                else
                {
                    Label_StartNewGame.Height = Math.Max(EnterGameButtonHeight_Min, Math.Min(EnterGameButtonHeight_Max, Label_StartNewGame.Height - Math.Max(1, (Label_StartNewGame.Height - EnterGameButtonHeight_Min) / 4)));
                }

                Label_ContinueLastGame.Top = Label_StartNewGame.Bottom;
                Label_ContinueLastGame.Height = Panel_EnterGameSelection.Height - Label_ContinueLastGame.Top;
            }
            else
            {
                Label_StartNewGame.Height = EnterGameButtonHeight_Max;

                Label_StartNewGame.Top = (Panel_EnterGameSelection.Height - Label_StartNewGame.Height) / 2;
            }

            Label_StartNewGame.Width = (Int32)(Math.Sqrt((double)Label_StartNewGame.Height / EnterGameButtonHeight_Max) * Panel_EnterGameSelection.Width);
            Label_StartNewGame.Left = (Panel_EnterGameSelection.Width - Label_StartNewGame.Width) / 2;

            Label_ContinueLastGame.Width = (Int32)(Math.Sqrt((double)Label_ContinueLastGame.Height / EnterGameButtonHeight_Max) * Panel_EnterGameSelection.Width);
            Label_ContinueLastGame.Left = (Panel_EnterGameSelection.Width - Label_ContinueLastGame.Width) / 2;

            Label_StartNewGame.Font = new Font("微软雅黑", Math.Max(1F, (Label_StartNewGame.Height - 4) / 3F), FontStyle.Regular, GraphicsUnit.Point, 134);
            Label_ContinueLastGame.Font = new Font("微软雅黑", Math.Max(1F, (Label_ContinueLastGame.Height - 4) / 3F), FontStyle.Regular, GraphicsUnit.Point, 134);

            Label_StartNewGame.ForeColor = Com.ColorManipulation.BlendByRGB(Me.RecommendColors.Text_INC, Me.RecommendColors.Text, Math.Sqrt((double)(Label_StartNewGame.Height - EnterGameButtonHeight_Min) / (EnterGameButtonHeight_Max - EnterGameButtonHeight_Min))).ToColor();
            Label_ContinueLastGame.ForeColor = Com.ColorManipulation.BlendByRGB(Me.RecommendColors.Text_INC, Me.RecommendColors.Text, Math.Sqrt((double)(Label_ContinueLastGame.Height - EnterGameButtonHeight_Min) / (EnterGameButtonHeight_Max - EnterGameButtonHeight_Min))).ToColor();

            //

            Panel_EnterGameSelection.Refresh();
        }

        #endregion

        #region "记录"区域

        private void PaintScore(PaintEventArgs e)
        {
            //
            // 绘制成绩。
            //

            Graphics Grap = e.Graphics;
            Grap.SmoothingMode = SmoothingMode.AntiAlias;

            //

            Color Cr_UserWinGames = GlobalColors.Win;
            Color Cr_DrawGames = GlobalColors.Draw;
            Color Cr_UserLoseGames = GlobalColors.Lose;

            Rectangle Rect_Pie = new Rectangle();
            Rect_Pie.Location = new Point(Label_TotalGames.Left, Label_TotalGames.Bottom + 20);
            Rect_Pie.Size = new Size(Panel_Score.Height - Rect_Pie.Y - Label_TotalGames.Top, Panel_Score.Height - Rect_Pie.Y - Label_TotalGames.Top);

            //

            double Angle_UserWinGames = 6;
            double Angle_DrawGames = 6;
            double Angle_UserLoseGames = 6;

            if (TotalGames > 0)
            {
                Angle_UserWinGames = Math.Max(6, (double)UserWinGames / TotalGames * 360);
                Angle_DrawGames = Math.Max(6, (double)DrawGames / TotalGames * 360);
                Angle_UserLoseGames = Math.Max(6, (double)UserLoseGames / TotalGames * 360);

                double Angle_Ratio = 360 / (Angle_UserWinGames + Angle_DrawGames + Angle_UserLoseGames);

                Angle_UserWinGames *= Angle_Ratio;
                Angle_DrawGames *= Angle_Ratio;
                Angle_UserLoseGames *= Angle_Ratio;
            }
            else
            {
                Angle_UserWinGames = Angle_UserLoseGames = 6;
                Angle_DrawGames = 360 - (Angle_UserWinGames + Angle_UserLoseGames);
            }

            Grap.FillPie(new SolidBrush(Cr_UserWinGames), Rect_Pie, -90F, (float)Angle_UserWinGames);
            Grap.FillPie(new SolidBrush(Cr_DrawGames), Rect_Pie, -90F + (float)Angle_UserWinGames, (float)Angle_DrawGames);
            Grap.FillPie(new SolidBrush(Cr_UserLoseGames), Rect_Pie, -90F + (float)(Angle_UserWinGames + Angle_DrawGames), (float)Angle_UserLoseGames);

            Grap.DrawPie(new Pen(Me.RecommendColors.Background_DEC.ToColor(), 1F), Rect_Pie, -90F, (float)Angle_UserWinGames);
            Grap.DrawPie(new Pen(Me.RecommendColors.Background_DEC.ToColor(), 1F), Rect_Pie, -90F + (float)Angle_UserWinGames, (float)Angle_DrawGames);
            Grap.DrawPie(new Pen(Me.RecommendColors.Background_DEC.ToColor(), 1F), Rect_Pie, -90F + (float)(Angle_UserWinGames + Angle_DrawGames), (float)Angle_UserLoseGames);

            Grap.FillEllipse(new SolidBrush(Me.RecommendColors.Background_DEC.ToColor()), new Rectangle(new Point(Rect_Pie.X + Rect_Pie.Width / 4, Rect_Pie.Y + Rect_Pie.Height / 4), new Size(Rect_Pie.Width / 2, Rect_Pie.Height / 2)));

            //

            Size RectSize_LE = new Size(12, 12);

            Rectangle Rect_DrawGames_LE = new Rectangle(new Point(Rect_Pie.Right + 20, Rect_Pie.Y + (Rect_Pie.Height - RectSize_LE.Height) / 2), RectSize_LE);
            Rectangle Rect_UserWinGames_LE = new Rectangle(new Point(Rect_DrawGames_LE.X, Rect_DrawGames_LE.Top - 20 - RectSize_LE.Height), RectSize_LE);
            Rectangle Rect_UserLoseGames_LE = new Rectangle(new Point(Rect_DrawGames_LE.X, Rect_DrawGames_LE.Bottom + 20), RectSize_LE);

            Label_UserWinGames.Location = new Point(Rect_UserWinGames_LE.Right + 10, Rect_UserWinGames_LE.Y + (Rect_UserWinGames_LE.Height - Label_UserWinGames.Height) / 2);
            Label_UserWinGamesVal.Location = new Point(Label_UserWinGames.Right + 5, Label_UserWinGames.Top);

            Label_DrawGames.Location = new Point(Rect_DrawGames_LE.Right + 10, Rect_DrawGames_LE.Y + (Rect_DrawGames_LE.Height - Label_DrawGames.Height) / 2);
            Label_DrawGamesVal.Location = new Point(Label_DrawGames.Right + 5, Label_DrawGames.Top);

            Label_UserLoseGames.Location = new Point(Rect_UserLoseGames_LE.Right + 10, Rect_UserLoseGames_LE.Y + (Rect_UserLoseGames_LE.Height - Label_UserLoseGames.Height) / 2);
            Label_UserLoseGamesVal.Location = new Point(Label_UserLoseGames.Right + 5, Label_UserLoseGames.Top);

            GraphicsPath Path_UserWinGames_LE = new GraphicsPath();
            Path_UserWinGames_LE.AddRectangle(Rect_UserWinGames_LE);
            PathGradientBrush PGB_UserWinGames_LE = new PathGradientBrush(Path_UserWinGames_LE)
            {
                CenterColor = Cr_UserWinGames,
                SurroundColors = new Color[] { Com.ColorManipulation.ShiftLightnessByHSL(Cr_UserWinGames, 0.3) },
                FocusScales = new PointF(1F, 0F)
            };
            Grap.FillPath(PGB_UserWinGames_LE, Path_UserWinGames_LE);
            Path_UserWinGames_LE.Dispose();
            PGB_UserWinGames_LE.Dispose();

            GraphicsPath Path_DrawGames_LE = new GraphicsPath();
            Path_DrawGames_LE.AddRectangle(Rect_DrawGames_LE);
            PathGradientBrush PGB_DrawGames_LE = new PathGradientBrush(Path_DrawGames_LE)
            {
                CenterColor = Cr_DrawGames,
                SurroundColors = new Color[] { Com.ColorManipulation.ShiftLightnessByHSL(Cr_DrawGames, 0.3) },
                FocusScales = new PointF(1F, 0F)
            };
            Grap.FillPath(PGB_DrawGames_LE, Path_DrawGames_LE);
            Path_DrawGames_LE.Dispose();
            PGB_DrawGames_LE.Dispose();

            GraphicsPath Path_UserLoseGames_LE = new GraphicsPath();
            Path_UserLoseGames_LE.AddRectangle(Rect_UserLoseGames_LE);
            PathGradientBrush PGB_UserLoseGames_LE = new PathGradientBrush(Path_UserLoseGames_LE)
            {
                CenterColor = Cr_UserLoseGames,
                SurroundColors = new Color[] { Com.ColorManipulation.ShiftLightnessByHSL(Cr_UserLoseGames, 0.3) },
                FocusScales = new PointF(1F, 0F)
            };
            Grap.FillPath(PGB_UserLoseGames_LE, Path_UserLoseGames_LE);
            Path_UserLoseGames_LE.Dispose();
            PGB_UserLoseGames_LE.Dispose();
        }

        #endregion

        #region "选项"区域

        // 棋盘：棋盘类型。

        private void RadioButton_ChessboardType_Gomoku_15x15_CheckedChanged(object sender, EventArgs e)
        {
            //
            // RadioButton_ChessboardType_Gomoku_15x15 选中状态改变。
            //

            if (RadioButton_ChessboardType_Gomoku_15x15.Checked)
            {
                ChessboardType = ChessboardTypes.Gomoku_15x15;

                StepTagNum = Math.Max(StepTagNum_MIN, Math.Min(StepTagNum_MAX, StepTagNum));

                RepaintSTNTrb();
            }
        }

        private void RadioButton_ChessboardType_Go_19x19_CheckedChanged(object sender, EventArgs e)
        {
            //
            // RadioButton_ChessboardType_Go_19x19 选中状态改变。
            //

            if (RadioButton_ChessboardType_Go_19x19.Checked)
            {
                ChessboardType = ChessboardTypes.Go_19x19;

                StepTagNum = Math.Max(StepTagNum_MIN, Math.Min(StepTagNum_MAX, StepTagNum));

                RepaintSTNTrb();
            }
        }

        // 棋盘：棋盘样式。

        private void RadioButton_ChessboardStyle_Intersection_CheckedChanged(object sender, EventArgs e)
        {
            //
            // RadioButton_ChessboardStyle_Intersection 选中状态改变。
            //

            if (RadioButton_ChessboardStyle_Intersection.Checked)
            {
                ChessboardStyle = ChessboardStyles.Intersection;
            }
        }

        private void RadioButton_ChessboardStyle_Checker_CheckedChanged(object sender, EventArgs e)
        {
            //
            // RadioButton_ChessboardStyle_Checker 选中状态改变。
            //

            if (RadioButton_ChessboardStyle_Checker.Checked)
            {
                ChessboardStyle = ChessboardStyles.Checker;
            }
        }

        // 禁手。

        private void ResetBalanceBreakerControls()
        {
            //
            // 重置禁手选项控件。
            //

            CheckBox_ShowBanPoint.Enabled = UseBalanceBreaker;

            CheckBox_ShowBanPoint.CheckedChanged -= CheckBox_ShowBanPoint_CheckedChanged;

            CheckBox_ShowBanPoint.Checked = (UseBalanceBreaker && ShowBanPoint);

            CheckBox_ShowBanPoint.CheckedChanged += CheckBox_ShowBanPoint_CheckedChanged;

            Label_BalanceBreaker_Info.Enabled = !SaveRecord;
        }

        private void CheckBox_UseBalanceBreaker_CheckedChanged(object sender, EventArgs e)
        {
            //
            // CheckBox_UseBalanceBreaker 选中状态改变。
            //

            UseBalanceBreaker = CheckBox_UseBalanceBreaker.Checked;

            ResetBalanceBreakerControls();
        }

        private void CheckBox_ShowBanPoint_CheckedChanged(object sender, EventArgs e)
        {
            //
            // CheckBox_ShowBanPoint 选中状态改变。
            //

            ShowBanPoint = CheckBox_ShowBanPoint.Checked;

            Label_BalanceBreaker_Info.Enabled = !SaveRecord;
        }

        // 落子序号。

        private const Int32 StepTagNumMouseWheelStep = 2; // 显示序号数量的鼠标滚轮调节步长。

        private bool StepTagNumIsAdjusting = false; // 是否正在调整显示序号数量。

        private Bitmap STNTrbBmp; // 显示序号数量调节器位图。

        private Size STNTrbSliderSize => new Size(2, Panel_StepTagNumAdjustment.Height); // 显示序号数量调节器滑块大小。

        private void UpdateSTNTrb()
        {
            //
            // 更新显示序号数量调节器位图。
            //

            if (STNTrbBmp != null)
            {
                STNTrbBmp.Dispose();
            }

            STNTrbBmp = new Bitmap(Math.Max(1, Panel_StepTagNumAdjustment.Width), Math.Max(1, Panel_StepTagNumAdjustment.Height));

            using (Graphics STNTrbBmpGrap = Graphics.FromImage(STNTrbBmp))
            {
                STNTrbBmpGrap.Clear(Panel_StepTagNumAdjustment.BackColor);

                //

                Color Color_Slider, Color_ScrollBar_Current, Color_ScrollBar_Unavailable;

                if (Com.Geometry.CursorIsInControl(Panel_StepTagNumAdjustment) || StepTagNumIsAdjusting)
                {
                    Color_Slider = Com.ColorManipulation.ShiftLightnessByHSL(Me.RecommendColors.Border_INC, 0.3).ToColor();
                    Color_ScrollBar_Current = Com.ColorManipulation.ShiftLightnessByHSL(Me.RecommendColors.Border_INC, 0.3).ToColor();
                    Color_ScrollBar_Unavailable = Com.ColorManipulation.ShiftLightnessByHSL(Me.RecommendColors.Border_DEC, 0.3).ToColor();
                }
                else
                {
                    Color_Slider = Me.RecommendColors.Border_INC.ToColor();
                    Color_ScrollBar_Current = Me.RecommendColors.Border_INC.ToColor();
                    Color_ScrollBar_Unavailable = Me.RecommendColors.Border_DEC.ToColor();
                }

                Rectangle Rect_Slider = new Rectangle(new Point((Panel_StepTagNumAdjustment.Width - STNTrbSliderSize.Width) * (StepTagNum - StepTagNum_MIN) / (StepTagNum_MAX - StepTagNum_MIN), 0), STNTrbSliderSize);
                Rectangle Rect_ScrollBar_Current = new Rectangle(new Point(0, 0), new Size(Rect_Slider.X, Panel_StepTagNumAdjustment.Height));
                Rectangle Rect_ScrollBar_Unavailable = new Rectangle(new Point(Rect_Slider.Right, 0), new Size(Panel_StepTagNumAdjustment.Width - Rect_Slider.Right, Panel_StepTagNumAdjustment.Height));

                Rect_Slider.Width = Math.Max(1, Rect_Slider.Width);
                Rect_ScrollBar_Current.Width = Math.Max(1, Rect_ScrollBar_Current.Width);
                Rect_ScrollBar_Unavailable.Width = Math.Max(1, Rect_ScrollBar_Unavailable.Width);

                GraphicsPath Path_ScrollBar_Unavailable = new GraphicsPath();
                Path_ScrollBar_Unavailable.AddRectangle(Rect_ScrollBar_Unavailable);
                PathGradientBrush PGB_ScrollBar_Unavailable = new PathGradientBrush(Path_ScrollBar_Unavailable)
                {
                    CenterColor = Color_ScrollBar_Unavailable,
                    SurroundColors = new Color[] { Com.ColorManipulation.ShiftLightnessByHSL(Color_ScrollBar_Unavailable, 0.3) },
                    FocusScales = new PointF(1F, 0F)
                };
                STNTrbBmpGrap.FillPath(PGB_ScrollBar_Unavailable, Path_ScrollBar_Unavailable);
                Path_ScrollBar_Unavailable.Dispose();
                PGB_ScrollBar_Unavailable.Dispose();

                GraphicsPath Path_ScrollBar_Current = new GraphicsPath();
                Path_ScrollBar_Current.AddRectangle(Rect_ScrollBar_Current);
                PathGradientBrush PGB_ScrollBar_Current = new PathGradientBrush(Path_ScrollBar_Current)
                {
                    CenterColor = Color_ScrollBar_Current,
                    SurroundColors = new Color[] { Com.ColorManipulation.ShiftLightnessByHSL(Color_ScrollBar_Current, 0.3) },
                    FocusScales = new PointF(1F, 0F)
                };
                STNTrbBmpGrap.FillPath(PGB_ScrollBar_Current, Path_ScrollBar_Current);
                Path_ScrollBar_Current.Dispose();
                PGB_ScrollBar_Current.Dispose();

                GraphicsPath Path_Slider = new GraphicsPath();
                Path_Slider.AddRectangle(Rect_Slider);
                PathGradientBrush PGB_Slider = new PathGradientBrush(Path_Slider)
                {
                    CenterColor = Color_Slider,
                    SurroundColors = new Color[] { Com.ColorManipulation.ShiftLightnessByHSL(Color_Slider, 0.3) },
                    FocusScales = new PointF(1F, 0F)
                };
                STNTrbBmpGrap.FillPath(PGB_Slider, Path_Slider);
                Path_Slider.Dispose();
                PGB_Slider.Dispose();

                //

                Label_StepTagNum_Val.Text = (StepTagNum <= StepTagNum_MIN ? "从不显示" : (StepTagNum >= StepTagNum_MAX ? "显示所有" : "显示最后" + StepTagNum + "手"));
                Label_StepTagNum_Val.Left = Math.Max(Panel_StepTagNumAdjustment.Left, Math.Min(Panel_StepTagNumAdjustment.Left + Panel_StepTagNumAdjustment.Width - Label_StepTagNum_Val.Width, Panel_StepTagNumAdjustment.Left + Rect_Slider.Left + (Rect_Slider.Width - Label_StepTagNum_Val.Width) / 2));
            }
        }

        private void RepaintSTNTrb()
        {
            //
            // 更新并重绘显示序号数量调节器位图。
            //

            UpdateSTNTrb();

            if (STNTrbBmp != null)
            {
                Panel_StepTagNumAdjustment.CreateGraphics().DrawImage(STNTrbBmp, new Point(0, 0));
            }
        }

        private void StepTagNumAdjustment()
        {
            //
            // 调整显示序号数量。
            //

            Int32 CurPosXOfCtrl = Math.Max(-STNTrbSliderSize.Width, Math.Min(Com.Geometry.GetCursorPositionOfControl(Panel_StepTagNumAdjustment).X, Panel_StepTagNumAdjustment.Width + STNTrbSliderSize.Width));

            double DivisionWidth = (double)(Panel_StepTagNumAdjustment.Width - STNTrbSliderSize.Width) / (StepTagNum_MAX - StepTagNum_MIN);

            StepTagNum = (Int32)Math.Max(StepTagNum_MIN, Math.Min(StepTagNum_MIN + (CurPosXOfCtrl - (STNTrbSliderSize.Width - DivisionWidth) / 2) / DivisionWidth, StepTagNum_MAX));

            RepaintSTNTrb();
        }

        private void Panel_StepTagNumAdjustment_Paint(object sender, PaintEventArgs e)
        {
            //
            // Panel_StepTagNumAdjustment 绘图。
            //

            UpdateSTNTrb();

            if (STNTrbBmp != null)
            {
                e.Graphics.DrawImage(STNTrbBmp, new Point(0, 0));
            }
        }

        private void Panel_StepTagNumAdjustment_MouseEnter(object sender, EventArgs e)
        {
            //
            // 鼠标进入 Panel_StepTagNumAdjustment。
            //

            RepaintSTNTrb();
        }

        private void Panel_StepTagNumAdjustment_MouseLeave(object sender, EventArgs e)
        {
            //
            // 鼠标离开 Panel_StepTagNumAdjustment。
            //

            RepaintSTNTrb();
        }

        private void Panel_StepTagNumAdjustment_MouseDown(object sender, MouseEventArgs e)
        {
            //
            // 鼠标按下 Panel_StepTagNumAdjustment。
            //

            if (e.Button == MouseButtons.Left)
            {
                StepTagNumIsAdjusting = true;

                StepTagNumAdjustment();
            }
        }

        private void Panel_StepTagNumAdjustment_MouseUp(object sender, MouseEventArgs e)
        {
            //
            // 鼠标释放 Panel_StepTagNumAdjustment。
            //

            StepTagNumIsAdjusting = false;
        }

        private void Panel_StepTagNumAdjustment_MouseMove(object sender, MouseEventArgs e)
        {
            //
            // 鼠标经过 Panel_StepTagNumAdjustment。
            //

            if (StepTagNumIsAdjusting)
            {
                StepTagNumAdjustment();
            }
        }

        private void Panel_StepTagNumAdjustment_MouseWheel(object sender, MouseEventArgs e)
        {
            //
            // 鼠标滚轮在 Panel_StepTagNumAdjustment 滚动。
            //

            if (e.Delta > 0)
            {
                if (StepTagNum % StepTagNumMouseWheelStep == 0)
                {
                    StepTagNum = Math.Min(StepTagNum_MAX, StepTagNum + StepTagNumMouseWheelStep);
                }
                else
                {
                    if (StepTagNum > 0)
                    {
                        StepTagNum = Math.Min(StepTagNum_MAX, StepTagNum - StepTagNum % StepTagNumMouseWheelStep + StepTagNumMouseWheelStep);
                    }
                    else
                    {
                        StepTagNum = Math.Min(StepTagNum_MAX, StepTagNum - StepTagNum % StepTagNumMouseWheelStep);
                    }
                }
            }
            else if (e.Delta < 0)
            {
                if (StepTagNum % StepTagNumMouseWheelStep == 0)
                {
                    StepTagNum = Math.Max(StepTagNum_MIN, StepTagNum - StepTagNumMouseWheelStep);
                }
                else
                {
                    if (StepTagNum > 0)
                    {
                        StepTagNum = Math.Max(StepTagNum_MIN, StepTagNum - StepTagNum % StepTagNumMouseWheelStep);
                    }
                    else
                    {
                        StepTagNum = Math.Max(StepTagNum_MIN, StepTagNum - StepTagNum % StepTagNumMouseWheelStep - StepTagNumMouseWheelStep);
                    }
                }
            }

            RepaintSTNTrb();
        }

        // 主题颜色。

        private void RadioButton_UseRandomThemeColor_CheckedChanged(object sender, EventArgs e)
        {
            //
            // RadioButton_UseRandomThemeColor 选中状态改变。
            //

            if (RadioButton_UseRandomThemeColor.Checked)
            {
                UseRandomThemeColor = true;
            }

            Label_ThemeColorName.Enabled = !UseRandomThemeColor;
        }

        private void RadioButton_UseCustomColor_CheckedChanged(object sender, EventArgs e)
        {
            //
            // RadioButton_UseCustomColor 选中状态改变。
            //

            if (RadioButton_UseCustomColor.Checked)
            {
                UseRandomThemeColor = false;
            }

            Label_ThemeColorName.Enabled = !UseRandomThemeColor;
        }

        private void Label_ThemeColorName_Click(object sender, EventArgs e)
        {
            //
            // 单击 Label_ThemeColorName。
            //

            ColorDialog_ThemeColor.Color = Me.ThemeColor.ToColor();

            Me.Enabled = false;

            DialogResult DR = ColorDialog_ThemeColor.ShowDialog();

            if (DR == DialogResult.OK)
            {
                Me.ThemeColor = new Com.ColorX(ColorDialog_ThemeColor.Color);
            }

            Me.Enabled = true;
        }

        // 抗锯齿。

        private void CheckBox_AntiAlias_CheckedChanged(object sender, EventArgs e)
        {
            //
            // CheckBox_AntiAlias 选中状态改变。
            //

            AntiAlias = CheckBox_AntiAlias.Checked;
        }

        #endregion

        #region "关于"区域

        private void Label_GitHub_Base_Click(object sender, EventArgs e)
        {
            //
            // 单击 Label_GitHub_Base。
            //

            Process.Start(URL_GitHub_Base);
        }

        private void Label_GitHub_Release_Click(object sender, EventArgs e)
        {
            //
            // 单击 Label_GitHub_Release。
            //

            Process.Start(URL_GitHub_Release);
        }

        #endregion

    }
}