namespace WinFormApp
{
    partial class Form_Main
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form_Main));
            this.Panel_Main = new System.Windows.Forms.Panel();
            this.Panel_Client = new System.Windows.Forms.Panel();
            this.Panel_FunctionArea = new System.Windows.Forms.Panel();
            this.Panel_FunctionAreaOptionsBar = new System.Windows.Forms.Panel();
            this.Label_Tab_Start = new System.Windows.Forms.Label();
            this.Label_Tab_Record = new System.Windows.Forms.Label();
            this.Label_Tab_Options = new System.Windows.Forms.Label();
            this.Label_Tab_About = new System.Windows.Forms.Label();
            this.Panel_FunctionAreaTab = new System.Windows.Forms.Panel();
            this.Panel_Tab_Start = new System.Windows.Forms.Panel();
            this.Panel_EnterGameSelection = new System.Windows.Forms.Panel();
            this.Label_StartNewGame = new System.Windows.Forms.Label();
            this.Label_ContinueLastGame = new System.Windows.Forms.Label();
            this.Panel_Tab_Record = new System.Windows.Forms.Panel();
            this.Panel_Score = new System.Windows.Forms.Panel();
            this.PictureBox_Overview = new System.Windows.Forms.PictureBox();
            this.Label_TotalGames = new System.Windows.Forms.Label();
            this.Label_TotalGamesVal = new System.Windows.Forms.Label();
            this.Label_UserWinGames = new System.Windows.Forms.Label();
            this.Label_UserWinGamesVal = new System.Windows.Forms.Label();
            this.Label_DrawGames = new System.Windows.Forms.Label();
            this.Label_DrawGamesVal = new System.Windows.Forms.Label();
            this.Label_UserLoseGames = new System.Windows.Forms.Label();
            this.Label_UserLoseGamesVal = new System.Windows.Forms.Label();
            this.Panel_GameTime = new System.Windows.Forms.Panel();
            this.PictureBox_GameTime = new System.Windows.Forms.PictureBox();
            this.Label_TotalTimeVal = new System.Windows.Forms.Label();
            this.Label_ThisTimeVal = new System.Windows.Forms.Label();
            this.Label_TotalTime = new System.Windows.Forms.Label();
            this.Label_ThisTime = new System.Windows.Forms.Label();
            this.Panel_Tab_Options = new System.Windows.Forms.Panel();
            this.Panel_Chessboard = new System.Windows.Forms.Panel();
            this.Label_Chessboard = new System.Windows.Forms.Label();
            this.Panel_ChessboardType = new System.Windows.Forms.Panel();
            this.Label_ChessboardType = new System.Windows.Forms.Label();
            this.RadioButton_ChessboardType_Gomoku_15x15 = new System.Windows.Forms.RadioButton();
            this.RadioButton_ChessboardType_Go_19x19 = new System.Windows.Forms.RadioButton();
            this.Panel_ChessboardStyle = new System.Windows.Forms.Panel();
            this.Label_ChessboardStyle = new System.Windows.Forms.Label();
            this.RadioButton_ChessboardStyle_Intersection = new System.Windows.Forms.RadioButton();
            this.RadioButton_ChessboardStyle_Checker = new System.Windows.Forms.RadioButton();
            this.Panel_BalanceBreaker = new System.Windows.Forms.Panel();
            this.Label_BalanceBreaker = new System.Windows.Forms.Label();
            this.CheckBox_UseBalanceBreaker = new System.Windows.Forms.CheckBox();
            this.CheckBox_ShowBanPoint = new System.Windows.Forms.CheckBox();
            this.Label_BalanceBreaker_Info = new System.Windows.Forms.Label();
            this.Panel_StepTag = new System.Windows.Forms.Panel();
            this.Label_StepTag = new System.Windows.Forms.Label();
            this.Label_StepTagNum_Val = new System.Windows.Forms.Label();
            this.Panel_StepTagNumAdjustment = new System.Windows.Forms.Panel();
            this.Panel_ThemeColor = new System.Windows.Forms.Panel();
            this.Label_ThemeColor = new System.Windows.Forms.Label();
            this.RadioButton_UseRandomThemeColor = new System.Windows.Forms.RadioButton();
            this.RadioButton_UseCustomColor = new System.Windows.Forms.RadioButton();
            this.Label_ThemeColorName = new System.Windows.Forms.Label();
            this.Panel_AntiAlias = new System.Windows.Forms.Panel();
            this.CheckBox_AntiAlias = new System.Windows.Forms.CheckBox();
            this.Label_AntiAlias = new System.Windows.Forms.Label();
            this.Panel_Tab_About = new System.Windows.Forms.Panel();
            this.PictureBox_ApplicationLogo = new System.Windows.Forms.PictureBox();
            this.Label_ApplicationName = new System.Windows.Forms.Label();
            this.Label_ApplicationEdition = new System.Windows.Forms.Label();
            this.Label_Version = new System.Windows.Forms.Label();
            this.Label_Copyright = new System.Windows.Forms.Label();
            this.Panel_GitHub = new System.Windows.Forms.Panel();
            this.Label_GitHub_Part1 = new System.Windows.Forms.Label();
            this.Label_GitHub_Base = new System.Windows.Forms.Label();
            this.Label_GitHub_Part2 = new System.Windows.Forms.Label();
            this.Label_GitHub_Release = new System.Windows.Forms.Label();
            this.Panel_GameUI = new System.Windows.Forms.Panel();
            this.Panel_Current = new System.Windows.Forms.Panel();
            this.Panel_Interrupt = new System.Windows.Forms.Panel();
            this.PictureBox_Restart = new System.Windows.Forms.PictureBox();
            this.PictureBox_ExitGame = new System.Windows.Forms.PictureBox();
            this.Panel_Environment = new System.Windows.Forms.Panel();
            this.ImageList_Interrupt = new System.Windows.Forms.ImageList(this.components);
            this.Timer_EnterPrompt = new System.Windows.Forms.Timer(this.components);
            this.Timer_ChessClock = new System.Windows.Forms.Timer(this.components);
            this.ToolTip_InterruptPrompt = new System.Windows.Forms.ToolTip(this.components);
            this.ColorDialog_ThemeColor = new System.Windows.Forms.ColorDialog();
            this.Panel_Main.SuspendLayout();
            this.Panel_Client.SuspendLayout();
            this.Panel_FunctionArea.SuspendLayout();
            this.Panel_FunctionAreaOptionsBar.SuspendLayout();
            this.Panel_FunctionAreaTab.SuspendLayout();
            this.Panel_Tab_Start.SuspendLayout();
            this.Panel_EnterGameSelection.SuspendLayout();
            this.Panel_Tab_Record.SuspendLayout();
            this.Panel_Score.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.PictureBox_Overview)).BeginInit();
            this.Panel_GameTime.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.PictureBox_GameTime)).BeginInit();
            this.Panel_Tab_Options.SuspendLayout();
            this.Panel_Chessboard.SuspendLayout();
            this.Panel_ChessboardType.SuspendLayout();
            this.Panel_ChessboardStyle.SuspendLayout();
            this.Panel_BalanceBreaker.SuspendLayout();
            this.Panel_StepTag.SuspendLayout();
            this.Panel_ThemeColor.SuspendLayout();
            this.Panel_AntiAlias.SuspendLayout();
            this.Panel_Tab_About.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.PictureBox_ApplicationLogo)).BeginInit();
            this.Panel_GitHub.SuspendLayout();
            this.Panel_GameUI.SuspendLayout();
            this.Panel_Current.SuspendLayout();
            this.Panel_Interrupt.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.PictureBox_Restart)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.PictureBox_ExitGame)).BeginInit();
            this.SuspendLayout();
            // 
            // Panel_Main
            // 
            this.Panel_Main.BackColor = System.Drawing.Color.Transparent;
            this.Panel_Main.Controls.Add(this.Panel_Client);
            this.Panel_Main.Dock = System.Windows.Forms.DockStyle.Fill;
            this.Panel_Main.Location = new System.Drawing.Point(0, 0);
            this.Panel_Main.Name = "Panel_Main";
            this.Panel_Main.Size = new System.Drawing.Size(585, 420);
            this.Panel_Main.TabIndex = 0;
            // 
            // Panel_Client
            // 
            this.Panel_Client.BackColor = System.Drawing.Color.Transparent;
            this.Panel_Client.Controls.Add(this.Panel_FunctionArea);
            this.Panel_Client.Controls.Add(this.Panel_GameUI);
            this.Panel_Client.Location = new System.Drawing.Point(0, 0);
            this.Panel_Client.Name = "Panel_Client";
            this.Panel_Client.Size = new System.Drawing.Size(585, 420);
            this.Panel_Client.TabIndex = 0;
            // 
            // Panel_FunctionArea
            // 
            this.Panel_FunctionArea.BackColor = System.Drawing.Color.Transparent;
            this.Panel_FunctionArea.Controls.Add(this.Panel_FunctionAreaOptionsBar);
            this.Panel_FunctionArea.Controls.Add(this.Panel_FunctionAreaTab);
            this.Panel_FunctionArea.Location = new System.Drawing.Point(0, 0);
            this.Panel_FunctionArea.Name = "Panel_FunctionArea";
            this.Panel_FunctionArea.Size = new System.Drawing.Size(585, 420);
            this.Panel_FunctionArea.TabIndex = 0;
            // 
            // Panel_FunctionAreaOptionsBar
            // 
            this.Panel_FunctionAreaOptionsBar.BackColor = System.Drawing.Color.Transparent;
            this.Panel_FunctionAreaOptionsBar.Controls.Add(this.Label_Tab_Start);
            this.Panel_FunctionAreaOptionsBar.Controls.Add(this.Label_Tab_Record);
            this.Panel_FunctionAreaOptionsBar.Controls.Add(this.Label_Tab_Options);
            this.Panel_FunctionAreaOptionsBar.Controls.Add(this.Label_Tab_About);
            this.Panel_FunctionAreaOptionsBar.Location = new System.Drawing.Point(0, 0);
            this.Panel_FunctionAreaOptionsBar.MaximumSize = new System.Drawing.Size(150, 65535);
            this.Panel_FunctionAreaOptionsBar.MinimumSize = new System.Drawing.Size(40, 40);
            this.Panel_FunctionAreaOptionsBar.Name = "Panel_FunctionAreaOptionsBar";
            this.Panel_FunctionAreaOptionsBar.Size = new System.Drawing.Size(150, 420);
            this.Panel_FunctionAreaOptionsBar.TabIndex = 0;
            this.Panel_FunctionAreaOptionsBar.Paint += new System.Windows.Forms.PaintEventHandler(this.Panel_FunctionAreaOptionsBar_Paint);
            this.Panel_FunctionAreaOptionsBar.MouseWheel += new System.Windows.Forms.MouseEventHandler(this.Panel_FunctionAreaOptionsBar_MouseWheel);
            // 
            // Label_Tab_Start
            // 
            this.Label_Tab_Start.AutoEllipsis = true;
            this.Label_Tab_Start.BackColor = System.Drawing.Color.Transparent;
            this.Label_Tab_Start.Font = new System.Drawing.Font("微软雅黑", 11.25F);
            this.Label_Tab_Start.ForeColor = System.Drawing.Color.White;
            this.Label_Tab_Start.Location = new System.Drawing.Point(0, 0);
            this.Label_Tab_Start.MaximumSize = new System.Drawing.Size(150, 50);
            this.Label_Tab_Start.MinimumSize = new System.Drawing.Size(40, 10);
            this.Label_Tab_Start.Name = "Label_Tab_Start";
            this.Label_Tab_Start.Padding = new System.Windows.Forms.Padding(10, 0, 10, 0);
            this.Label_Tab_Start.Size = new System.Drawing.Size(150, 50);
            this.Label_Tab_Start.TabIndex = 0;
            this.Label_Tab_Start.Text = "开始";
            this.Label_Tab_Start.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.Label_Tab_Start.MouseDown += new System.Windows.Forms.MouseEventHandler(this.Label_Tab_Start_MouseDown);
            this.Label_Tab_Start.MouseEnter += new System.EventHandler(this.Label_Tab_MouseEnter);
            this.Label_Tab_Start.MouseLeave += new System.EventHandler(this.Label_Tab_MouseLeave);
            // 
            // Label_Tab_Record
            // 
            this.Label_Tab_Record.AutoEllipsis = true;
            this.Label_Tab_Record.BackColor = System.Drawing.Color.Transparent;
            this.Label_Tab_Record.Font = new System.Drawing.Font("微软雅黑", 11.25F);
            this.Label_Tab_Record.ForeColor = System.Drawing.Color.White;
            this.Label_Tab_Record.Location = new System.Drawing.Point(0, 50);
            this.Label_Tab_Record.MaximumSize = new System.Drawing.Size(150, 50);
            this.Label_Tab_Record.MinimumSize = new System.Drawing.Size(40, 10);
            this.Label_Tab_Record.Name = "Label_Tab_Record";
            this.Label_Tab_Record.Padding = new System.Windows.Forms.Padding(10, 0, 10, 0);
            this.Label_Tab_Record.Size = new System.Drawing.Size(150, 50);
            this.Label_Tab_Record.TabIndex = 0;
            this.Label_Tab_Record.Text = "记录";
            this.Label_Tab_Record.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.Label_Tab_Record.MouseDown += new System.Windows.Forms.MouseEventHandler(this.Label_Tab_Record_MouseDown);
            this.Label_Tab_Record.MouseEnter += new System.EventHandler(this.Label_Tab_MouseEnter);
            this.Label_Tab_Record.MouseLeave += new System.EventHandler(this.Label_Tab_MouseLeave);
            // 
            // Label_Tab_Options
            // 
            this.Label_Tab_Options.AutoEllipsis = true;
            this.Label_Tab_Options.BackColor = System.Drawing.Color.Transparent;
            this.Label_Tab_Options.Font = new System.Drawing.Font("微软雅黑", 11.25F);
            this.Label_Tab_Options.ForeColor = System.Drawing.Color.White;
            this.Label_Tab_Options.Location = new System.Drawing.Point(0, 100);
            this.Label_Tab_Options.MaximumSize = new System.Drawing.Size(150, 50);
            this.Label_Tab_Options.MinimumSize = new System.Drawing.Size(40, 10);
            this.Label_Tab_Options.Name = "Label_Tab_Options";
            this.Label_Tab_Options.Padding = new System.Windows.Forms.Padding(10, 0, 10, 0);
            this.Label_Tab_Options.Size = new System.Drawing.Size(150, 50);
            this.Label_Tab_Options.TabIndex = 0;
            this.Label_Tab_Options.Text = "选项";
            this.Label_Tab_Options.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.Label_Tab_Options.MouseDown += new System.Windows.Forms.MouseEventHandler(this.Label_Tab_Options_MouseDown);
            this.Label_Tab_Options.MouseEnter += new System.EventHandler(this.Label_Tab_MouseEnter);
            this.Label_Tab_Options.MouseLeave += new System.EventHandler(this.Label_Tab_MouseLeave);
            // 
            // Label_Tab_About
            // 
            this.Label_Tab_About.AutoEllipsis = true;
            this.Label_Tab_About.BackColor = System.Drawing.Color.Transparent;
            this.Label_Tab_About.Font = new System.Drawing.Font("微软雅黑", 11.25F);
            this.Label_Tab_About.ForeColor = System.Drawing.Color.White;
            this.Label_Tab_About.Location = new System.Drawing.Point(0, 150);
            this.Label_Tab_About.MaximumSize = new System.Drawing.Size(150, 50);
            this.Label_Tab_About.MinimumSize = new System.Drawing.Size(40, 10);
            this.Label_Tab_About.Name = "Label_Tab_About";
            this.Label_Tab_About.Padding = new System.Windows.Forms.Padding(10, 0, 10, 0);
            this.Label_Tab_About.Size = new System.Drawing.Size(150, 50);
            this.Label_Tab_About.TabIndex = 4;
            this.Label_Tab_About.Text = "关于";
            this.Label_Tab_About.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.Label_Tab_About.MouseDown += new System.Windows.Forms.MouseEventHandler(this.Label_Tab_About_MouseDown);
            this.Label_Tab_About.MouseEnter += new System.EventHandler(this.Label_Tab_MouseEnter);
            this.Label_Tab_About.MouseLeave += new System.EventHandler(this.Label_Tab_MouseLeave);
            // 
            // Panel_FunctionAreaTab
            // 
            this.Panel_FunctionAreaTab.AutoScroll = true;
            this.Panel_FunctionAreaTab.BackColor = System.Drawing.Color.Transparent;
            this.Panel_FunctionAreaTab.Controls.Add(this.Panel_Tab_Start);
            this.Panel_FunctionAreaTab.Controls.Add(this.Panel_Tab_Record);
            this.Panel_FunctionAreaTab.Controls.Add(this.Panel_Tab_Options);
            this.Panel_FunctionAreaTab.Controls.Add(this.Panel_Tab_About);
            this.Panel_FunctionAreaTab.Location = new System.Drawing.Point(150, 0);
            this.Panel_FunctionAreaTab.Name = "Panel_FunctionAreaTab";
            this.Panel_FunctionAreaTab.Size = new System.Drawing.Size(435, 420);
            this.Panel_FunctionAreaTab.TabIndex = 0;
            // 
            // Panel_Tab_Start
            // 
            this.Panel_Tab_Start.BackColor = System.Drawing.Color.Transparent;
            this.Panel_Tab_Start.Controls.Add(this.Panel_EnterGameSelection);
            this.Panel_Tab_Start.Location = new System.Drawing.Point(0, 0);
            this.Panel_Tab_Start.MinimumSize = new System.Drawing.Size(260, 80);
            this.Panel_Tab_Start.Name = "Panel_Tab_Start";
            this.Panel_Tab_Start.Size = new System.Drawing.Size(435, 420);
            this.Panel_Tab_Start.TabIndex = 0;
            // 
            // Panel_EnterGameSelection
            // 
            this.Panel_EnterGameSelection.BackColor = System.Drawing.Color.Transparent;
            this.Panel_EnterGameSelection.Controls.Add(this.Label_StartNewGame);
            this.Panel_EnterGameSelection.Controls.Add(this.Label_ContinueLastGame);
            this.Panel_EnterGameSelection.Location = new System.Drawing.Point(75, 170);
            this.Panel_EnterGameSelection.Name = "Panel_EnterGameSelection";
            this.Panel_EnterGameSelection.Size = new System.Drawing.Size(260, 80);
            this.Panel_EnterGameSelection.TabIndex = 0;
            this.Panel_EnterGameSelection.Paint += new System.Windows.Forms.PaintEventHandler(this.Panel_EnterGameSelection_Paint);
            // 
            // Label_StartNewGame
            // 
            this.Label_StartNewGame.AutoEllipsis = true;
            this.Label_StartNewGame.BackColor = System.Drawing.Color.Transparent;
            this.Label_StartNewGame.Font = new System.Drawing.Font("微软雅黑", 12F);
            this.Label_StartNewGame.ForeColor = System.Drawing.Color.White;
            this.Label_StartNewGame.Location = new System.Drawing.Point(0, 0);
            this.Label_StartNewGame.Name = "Label_StartNewGame";
            this.Label_StartNewGame.Size = new System.Drawing.Size(260, 40);
            this.Label_StartNewGame.TabIndex = 0;
            this.Label_StartNewGame.Text = "开始新游戏";
            this.Label_StartNewGame.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // Label_ContinueLastGame
            // 
            this.Label_ContinueLastGame.AutoEllipsis = true;
            this.Label_ContinueLastGame.BackColor = System.Drawing.Color.Transparent;
            this.Label_ContinueLastGame.Font = new System.Drawing.Font("微软雅黑", 12F);
            this.Label_ContinueLastGame.ForeColor = System.Drawing.Color.White;
            this.Label_ContinueLastGame.Location = new System.Drawing.Point(0, 40);
            this.Label_ContinueLastGame.Name = "Label_ContinueLastGame";
            this.Label_ContinueLastGame.Size = new System.Drawing.Size(260, 40);
            this.Label_ContinueLastGame.TabIndex = 0;
            this.Label_ContinueLastGame.Text = "继续上次的游戏";
            this.Label_ContinueLastGame.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // Panel_Tab_Record
            // 
            this.Panel_Tab_Record.BackColor = System.Drawing.Color.Transparent;
            this.Panel_Tab_Record.Controls.Add(this.Panel_Score);
            this.Panel_Tab_Record.Controls.Add(this.Panel_GameTime);
            this.Panel_Tab_Record.Location = new System.Drawing.Point(0, 0);
            this.Panel_Tab_Record.MinimumSize = new System.Drawing.Size(400, 390);
            this.Panel_Tab_Record.Name = "Panel_Tab_Record";
            this.Panel_Tab_Record.Size = new System.Drawing.Size(435, 420);
            this.Panel_Tab_Record.TabIndex = 0;
            this.Panel_Tab_Record.Visible = false;
            // 
            // Panel_Score
            // 
            this.Panel_Score.BackColor = System.Drawing.Color.Transparent;
            this.Panel_Score.Controls.Add(this.PictureBox_Overview);
            this.Panel_Score.Controls.Add(this.Label_TotalGames);
            this.Panel_Score.Controls.Add(this.Label_TotalGamesVal);
            this.Panel_Score.Controls.Add(this.Label_UserWinGames);
            this.Panel_Score.Controls.Add(this.Label_UserWinGamesVal);
            this.Panel_Score.Controls.Add(this.Label_DrawGames);
            this.Panel_Score.Controls.Add(this.Label_DrawGamesVal);
            this.Panel_Score.Controls.Add(this.Label_UserLoseGames);
            this.Panel_Score.Controls.Add(this.Label_UserLoseGamesVal);
            this.Panel_Score.Location = new System.Drawing.Point(30, 30);
            this.Panel_Score.Name = "Panel_Score";
            this.Panel_Score.Size = new System.Drawing.Size(360, 210);
            this.Panel_Score.TabIndex = 0;
            this.Panel_Score.Paint += new System.Windows.Forms.PaintEventHandler(this.Panel_Score_Paint);
            // 
            // PictureBox_Overview
            // 
            this.PictureBox_Overview.BackColor = System.Drawing.Color.Transparent;
            this.PictureBox_Overview.ErrorImage = null;
            this.PictureBox_Overview.Image = ((System.Drawing.Image)(resources.GetObject("PictureBox_Overview.Image")));
            this.PictureBox_Overview.InitialImage = null;
            this.PictureBox_Overview.Location = new System.Drawing.Point(0, 0);
            this.PictureBox_Overview.Name = "PictureBox_Overview";
            this.PictureBox_Overview.Size = new System.Drawing.Size(20, 20);
            this.PictureBox_Overview.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
            this.PictureBox_Overview.TabIndex = 0;
            this.PictureBox_Overview.TabStop = false;
            // 
            // Label_TotalGames
            // 
            this.Label_TotalGames.AutoSize = true;
            this.Label_TotalGames.BackColor = System.Drawing.Color.Transparent;
            this.Label_TotalGames.Font = new System.Drawing.Font("微软雅黑", 11.25F);
            this.Label_TotalGames.ForeColor = System.Drawing.Color.White;
            this.Label_TotalGames.Location = new System.Drawing.Point(25, 30);
            this.Label_TotalGames.Name = "Label_TotalGames";
            this.Label_TotalGames.Size = new System.Drawing.Size(58, 20);
            this.Label_TotalGames.TabIndex = 0;
            this.Label_TotalGames.Text = "总局数:";
            // 
            // Label_TotalGamesVal
            // 
            this.Label_TotalGamesVal.AutoSize = true;
            this.Label_TotalGamesVal.BackColor = System.Drawing.Color.Transparent;
            this.Label_TotalGamesVal.Font = new System.Drawing.Font("微软雅黑", 11.25F, System.Drawing.FontStyle.Bold);
            this.Label_TotalGamesVal.ForeColor = System.Drawing.Color.White;
            this.Label_TotalGamesVal.Location = new System.Drawing.Point(90, 30);
            this.Label_TotalGamesVal.Name = "Label_TotalGamesVal";
            this.Label_TotalGamesVal.Size = new System.Drawing.Size(98, 19);
            this.Label_TotalGamesVal.TabIndex = 0;
            this.Label_TotalGamesVal.Text = "TotalGames";
            // 
            // Label_UserWinGames
            // 
            this.Label_UserWinGames.AutoSize = true;
            this.Label_UserWinGames.BackColor = System.Drawing.Color.Transparent;
            this.Label_UserWinGames.Font = new System.Drawing.Font("微软雅黑", 11.25F);
            this.Label_UserWinGames.ForeColor = System.Drawing.Color.White;
            this.Label_UserWinGames.Location = new System.Drawing.Point(155, 70);
            this.Label_UserWinGames.Name = "Label_UserWinGames";
            this.Label_UserWinGames.Size = new System.Drawing.Size(28, 20);
            this.Label_UserWinGames.TabIndex = 0;
            this.Label_UserWinGames.Text = "胜:";
            // 
            // Label_UserWinGamesVal
            // 
            this.Label_UserWinGamesVal.AutoSize = true;
            this.Label_UserWinGamesVal.BackColor = System.Drawing.Color.Transparent;
            this.Label_UserWinGamesVal.Font = new System.Drawing.Font("微软雅黑", 11.25F, System.Drawing.FontStyle.Bold);
            this.Label_UserWinGamesVal.ForeColor = System.Drawing.Color.White;
            this.Label_UserWinGamesVal.Location = new System.Drawing.Point(190, 70);
            this.Label_UserWinGamesVal.Name = "Label_UserWinGamesVal";
            this.Label_UserWinGamesVal.Size = new System.Drawing.Size(124, 19);
            this.Label_UserWinGamesVal.TabIndex = 0;
            this.Label_UserWinGamesVal.Text = "UserWinGames";
            // 
            // Label_DrawGames
            // 
            this.Label_DrawGames.AutoSize = true;
            this.Label_DrawGames.BackColor = System.Drawing.Color.Transparent;
            this.Label_DrawGames.Font = new System.Drawing.Font("微软雅黑", 11.25F);
            this.Label_DrawGames.ForeColor = System.Drawing.Color.White;
            this.Label_DrawGames.Location = new System.Drawing.Point(155, 105);
            this.Label_DrawGames.Name = "Label_DrawGames";
            this.Label_DrawGames.Size = new System.Drawing.Size(28, 20);
            this.Label_DrawGames.TabIndex = 0;
            this.Label_DrawGames.Text = "和:";
            // 
            // Label_DrawGamesVal
            // 
            this.Label_DrawGamesVal.AutoSize = true;
            this.Label_DrawGamesVal.BackColor = System.Drawing.Color.Transparent;
            this.Label_DrawGamesVal.Font = new System.Drawing.Font("微软雅黑", 11.25F, System.Drawing.FontStyle.Bold);
            this.Label_DrawGamesVal.ForeColor = System.Drawing.Color.White;
            this.Label_DrawGamesVal.Location = new System.Drawing.Point(190, 105);
            this.Label_DrawGamesVal.Name = "Label_DrawGamesVal";
            this.Label_DrawGamesVal.Size = new System.Drawing.Size(100, 19);
            this.Label_DrawGamesVal.TabIndex = 0;
            this.Label_DrawGamesVal.Text = "DrawGames";
            // 
            // Label_UserLoseGames
            // 
            this.Label_UserLoseGames.AutoSize = true;
            this.Label_UserLoseGames.BackColor = System.Drawing.Color.Transparent;
            this.Label_UserLoseGames.Font = new System.Drawing.Font("微软雅黑", 11.25F);
            this.Label_UserLoseGames.ForeColor = System.Drawing.Color.White;
            this.Label_UserLoseGames.Location = new System.Drawing.Point(155, 140);
            this.Label_UserLoseGames.Name = "Label_UserLoseGames";
            this.Label_UserLoseGames.Size = new System.Drawing.Size(28, 20);
            this.Label_UserLoseGames.TabIndex = 0;
            this.Label_UserLoseGames.Text = "负:";
            // 
            // Label_UserLoseGamesVal
            // 
            this.Label_UserLoseGamesVal.AutoSize = true;
            this.Label_UserLoseGamesVal.BackColor = System.Drawing.Color.Transparent;
            this.Label_UserLoseGamesVal.Font = new System.Drawing.Font("微软雅黑", 11.25F, System.Drawing.FontStyle.Bold);
            this.Label_UserLoseGamesVal.ForeColor = System.Drawing.Color.White;
            this.Label_UserLoseGamesVal.Location = new System.Drawing.Point(190, 140);
            this.Label_UserLoseGamesVal.Name = "Label_UserLoseGamesVal";
            this.Label_UserLoseGamesVal.Size = new System.Drawing.Size(128, 19);
            this.Label_UserLoseGamesVal.TabIndex = 0;
            this.Label_UserLoseGamesVal.Text = "UserLoseGames";
            // 
            // Panel_GameTime
            // 
            this.Panel_GameTime.BackColor = System.Drawing.Color.Transparent;
            this.Panel_GameTime.Controls.Add(this.PictureBox_GameTime);
            this.Panel_GameTime.Controls.Add(this.Label_TotalTimeVal);
            this.Panel_GameTime.Controls.Add(this.Label_ThisTimeVal);
            this.Panel_GameTime.Controls.Add(this.Label_TotalTime);
            this.Panel_GameTime.Controls.Add(this.Label_ThisTime);
            this.Panel_GameTime.Location = new System.Drawing.Point(30, 240);
            this.Panel_GameTime.Name = "Panel_GameTime";
            this.Panel_GameTime.Size = new System.Drawing.Size(360, 120);
            this.Panel_GameTime.TabIndex = 0;
            this.Panel_GameTime.Paint += new System.Windows.Forms.PaintEventHandler(this.Panel_GameTime_Paint);
            // 
            // PictureBox_GameTime
            // 
            this.PictureBox_GameTime.BackColor = System.Drawing.Color.Transparent;
            this.PictureBox_GameTime.ErrorImage = null;
            this.PictureBox_GameTime.Image = ((System.Drawing.Image)(resources.GetObject("PictureBox_GameTime.Image")));
            this.PictureBox_GameTime.InitialImage = null;
            this.PictureBox_GameTime.Location = new System.Drawing.Point(0, 0);
            this.PictureBox_GameTime.Name = "PictureBox_GameTime";
            this.PictureBox_GameTime.Size = new System.Drawing.Size(20, 20);
            this.PictureBox_GameTime.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
            this.PictureBox_GameTime.TabIndex = 0;
            this.PictureBox_GameTime.TabStop = false;
            // 
            // Label_TotalTimeVal
            // 
            this.Label_TotalTimeVal.AutoSize = true;
            this.Label_TotalTimeVal.BackColor = System.Drawing.Color.Transparent;
            this.Label_TotalTimeVal.Font = new System.Drawing.Font("微软雅黑", 11.25F, System.Drawing.FontStyle.Bold);
            this.Label_TotalTimeVal.ForeColor = System.Drawing.Color.White;
            this.Label_TotalTimeVal.Location = new System.Drawing.Point(135, 70);
            this.Label_TotalTimeVal.Name = "Label_TotalTimeVal";
            this.Label_TotalTimeVal.Size = new System.Drawing.Size(128, 19);
            this.Label_TotalTimeVal.TabIndex = 0;
            this.Label_TotalTimeVal.Text = "TotalGameTime";
            // 
            // Label_ThisTimeVal
            // 
            this.Label_ThisTimeVal.AutoSize = true;
            this.Label_ThisTimeVal.BackColor = System.Drawing.Color.Transparent;
            this.Label_ThisTimeVal.Font = new System.Drawing.Font("微软雅黑", 11.25F, System.Drawing.FontStyle.Bold);
            this.Label_ThisTimeVal.ForeColor = System.Drawing.Color.White;
            this.Label_ThisTimeVal.Location = new System.Drawing.Point(135, 30);
            this.Label_ThisTimeVal.Name = "Label_ThisTimeVal";
            this.Label_ThisTimeVal.Size = new System.Drawing.Size(120, 19);
            this.Label_ThisTimeVal.TabIndex = 0;
            this.Label_ThisTimeVal.Text = "ThisGameTime";
            // 
            // Label_TotalTime
            // 
            this.Label_TotalTime.AutoSize = true;
            this.Label_TotalTime.BackColor = System.Drawing.Color.Transparent;
            this.Label_TotalTime.Font = new System.Drawing.Font("微软雅黑", 11.25F);
            this.Label_TotalTime.ForeColor = System.Drawing.Color.White;
            this.Label_TotalTime.Location = new System.Drawing.Point(25, 70);
            this.Label_TotalTime.Name = "Label_TotalTime";
            this.Label_TotalTime.Size = new System.Drawing.Size(103, 20);
            this.Label_TotalTime.TabIndex = 0;
            this.Label_TotalTime.Text = "累计游戏时长:";
            // 
            // Label_ThisTime
            // 
            this.Label_ThisTime.AutoSize = true;
            this.Label_ThisTime.BackColor = System.Drawing.Color.Transparent;
            this.Label_ThisTime.Font = new System.Drawing.Font("微软雅黑", 11.25F);
            this.Label_ThisTime.ForeColor = System.Drawing.Color.White;
            this.Label_ThisTime.Location = new System.Drawing.Point(25, 30);
            this.Label_ThisTime.Name = "Label_ThisTime";
            this.Label_ThisTime.Size = new System.Drawing.Size(103, 20);
            this.Label_ThisTime.TabIndex = 0;
            this.Label_ThisTime.Text = "本次游戏时长:";
            // 
            // Panel_Tab_Options
            // 
            this.Panel_Tab_Options.BackColor = System.Drawing.Color.Transparent;
            this.Panel_Tab_Options.Controls.Add(this.Panel_Chessboard);
            this.Panel_Tab_Options.Controls.Add(this.Panel_BalanceBreaker);
            this.Panel_Tab_Options.Controls.Add(this.Panel_StepTag);
            this.Panel_Tab_Options.Controls.Add(this.Panel_ThemeColor);
            this.Panel_Tab_Options.Controls.Add(this.Panel_AntiAlias);
            this.Panel_Tab_Options.Location = new System.Drawing.Point(0, 0);
            this.Panel_Tab_Options.MinimumSize = new System.Drawing.Size(410, 610);
            this.Panel_Tab_Options.Name = "Panel_Tab_Options";
            this.Panel_Tab_Options.Size = new System.Drawing.Size(435, 610);
            this.Panel_Tab_Options.TabIndex = 0;
            this.Panel_Tab_Options.Visible = false;
            // 
            // Panel_Chessboard
            // 
            this.Panel_Chessboard.BackColor = System.Drawing.Color.Transparent;
            this.Panel_Chessboard.Controls.Add(this.Label_Chessboard);
            this.Panel_Chessboard.Controls.Add(this.Panel_ChessboardType);
            this.Panel_Chessboard.Controls.Add(this.Panel_ChessboardStyle);
            this.Panel_Chessboard.Location = new System.Drawing.Point(30, 30);
            this.Panel_Chessboard.Name = "Panel_Chessboard";
            this.Panel_Chessboard.Size = new System.Drawing.Size(350, 145);
            this.Panel_Chessboard.TabIndex = 0;
            this.Panel_Chessboard.Paint += new System.Windows.Forms.PaintEventHandler(this.Panel_Chessboard_Paint);
            // 
            // Label_Chessboard
            // 
            this.Label_Chessboard.AutoSize = true;
            this.Label_Chessboard.BackColor = System.Drawing.Color.Transparent;
            this.Label_Chessboard.Dock = System.Windows.Forms.DockStyle.Top;
            this.Label_Chessboard.Font = new System.Drawing.Font("微软雅黑", 11.25F);
            this.Label_Chessboard.ForeColor = System.Drawing.Color.White;
            this.Label_Chessboard.Location = new System.Drawing.Point(0, 0);
            this.Label_Chessboard.Name = "Label_Chessboard";
            this.Label_Chessboard.Size = new System.Drawing.Size(39, 20);
            this.Label_Chessboard.TabIndex = 0;
            this.Label_Chessboard.Text = "棋盘";
            this.Label_Chessboard.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // Panel_ChessboardType
            // 
            this.Panel_ChessboardType.BackColor = System.Drawing.Color.Transparent;
            this.Panel_ChessboardType.Controls.Add(this.Label_ChessboardType);
            this.Panel_ChessboardType.Controls.Add(this.RadioButton_ChessboardType_Gomoku_15x15);
            this.Panel_ChessboardType.Controls.Add(this.RadioButton_ChessboardType_Go_19x19);
            this.Panel_ChessboardType.Location = new System.Drawing.Point(25, 30);
            this.Panel_ChessboardType.Name = "Panel_ChessboardType";
            this.Panel_ChessboardType.Size = new System.Drawing.Size(300, 50);
            this.Panel_ChessboardType.TabIndex = 0;
            // 
            // Label_ChessboardType
            // 
            this.Label_ChessboardType.AutoSize = true;
            this.Label_ChessboardType.BackColor = System.Drawing.Color.Transparent;
            this.Label_ChessboardType.Font = new System.Drawing.Font("微软雅黑", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.Label_ChessboardType.ForeColor = System.Drawing.Color.White;
            this.Label_ChessboardType.Location = new System.Drawing.Point(0, 0);
            this.Label_ChessboardType.Name = "Label_ChessboardType";
            this.Label_ChessboardType.Size = new System.Drawing.Size(64, 19);
            this.Label_ChessboardType.TabIndex = 0;
            this.Label_ChessboardType.Text = "棋盘类型:";
            this.Label_ChessboardType.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // RadioButton_ChessboardType_Gomoku_15x15
            // 
            this.RadioButton_ChessboardType_Gomoku_15x15.AutoSize = true;
            this.RadioButton_ChessboardType_Gomoku_15x15.BackColor = System.Drawing.Color.Transparent;
            this.RadioButton_ChessboardType_Gomoku_15x15.Font = new System.Drawing.Font("微软雅黑", 9.75F);
            this.RadioButton_ChessboardType_Gomoku_15x15.ForeColor = System.Drawing.Color.White;
            this.RadioButton_ChessboardType_Gomoku_15x15.Location = new System.Drawing.Point(20, 25);
            this.RadioButton_ChessboardType_Gomoku_15x15.Name = "RadioButton_ChessboardType_Gomoku_15x15";
            this.RadioButton_ChessboardType_Gomoku_15x15.Size = new System.Drawing.Size(146, 23);
            this.RadioButton_ChessboardType_Gomoku_15x15.TabIndex = 0;
            this.RadioButton_ChessboardType_Gomoku_15x15.Text = "五子棋棋盘 (15×15)";
            this.RadioButton_ChessboardType_Gomoku_15x15.UseVisualStyleBackColor = false;
            this.RadioButton_ChessboardType_Gomoku_15x15.CheckedChanged += new System.EventHandler(this.RadioButton_ChessboardType_Gomoku_15x15_CheckedChanged);
            // 
            // RadioButton_ChessboardType_Go_19x19
            // 
            this.RadioButton_ChessboardType_Go_19x19.AutoSize = true;
            this.RadioButton_ChessboardType_Go_19x19.BackColor = System.Drawing.Color.Transparent;
            this.RadioButton_ChessboardType_Go_19x19.Font = new System.Drawing.Font("微软雅黑", 9.75F);
            this.RadioButton_ChessboardType_Go_19x19.ForeColor = System.Drawing.Color.White;
            this.RadioButton_ChessboardType_Go_19x19.Location = new System.Drawing.Point(170, 25);
            this.RadioButton_ChessboardType_Go_19x19.Name = "RadioButton_ChessboardType_Go_19x19";
            this.RadioButton_ChessboardType_Go_19x19.Size = new System.Drawing.Size(133, 23);
            this.RadioButton_ChessboardType_Go_19x19.TabIndex = 0;
            this.RadioButton_ChessboardType_Go_19x19.Text = "围棋棋盘 (19×19)";
            this.RadioButton_ChessboardType_Go_19x19.UseVisualStyleBackColor = false;
            this.RadioButton_ChessboardType_Go_19x19.CheckedChanged += new System.EventHandler(this.RadioButton_ChessboardType_Go_19x19_CheckedChanged);
            // 
            // Panel_ChessboardStyle
            // 
            this.Panel_ChessboardStyle.BackColor = System.Drawing.Color.Transparent;
            this.Panel_ChessboardStyle.Controls.Add(this.Label_ChessboardStyle);
            this.Panel_ChessboardStyle.Controls.Add(this.RadioButton_ChessboardStyle_Intersection);
            this.Panel_ChessboardStyle.Controls.Add(this.RadioButton_ChessboardStyle_Checker);
            this.Panel_ChessboardStyle.Location = new System.Drawing.Point(25, 85);
            this.Panel_ChessboardStyle.Name = "Panel_ChessboardStyle";
            this.Panel_ChessboardStyle.Size = new System.Drawing.Size(300, 50);
            this.Panel_ChessboardStyle.TabIndex = 0;
            // 
            // Label_ChessboardStyle
            // 
            this.Label_ChessboardStyle.AutoSize = true;
            this.Label_ChessboardStyle.BackColor = System.Drawing.Color.Transparent;
            this.Label_ChessboardStyle.Font = new System.Drawing.Font("微软雅黑", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.Label_ChessboardStyle.ForeColor = System.Drawing.Color.White;
            this.Label_ChessboardStyle.Location = new System.Drawing.Point(0, 0);
            this.Label_ChessboardStyle.Name = "Label_ChessboardStyle";
            this.Label_ChessboardStyle.Size = new System.Drawing.Size(64, 19);
            this.Label_ChessboardStyle.TabIndex = 0;
            this.Label_ChessboardStyle.Text = "棋盘样式:";
            this.Label_ChessboardStyle.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // RadioButton_ChessboardStyle_Intersection
            // 
            this.RadioButton_ChessboardStyle_Intersection.AutoSize = true;
            this.RadioButton_ChessboardStyle_Intersection.BackColor = System.Drawing.Color.Transparent;
            this.RadioButton_ChessboardStyle_Intersection.Font = new System.Drawing.Font("微软雅黑", 9.75F);
            this.RadioButton_ChessboardStyle_Intersection.ForeColor = System.Drawing.Color.White;
            this.RadioButton_ChessboardStyle_Intersection.Location = new System.Drawing.Point(20, 25);
            this.RadioButton_ChessboardStyle_Intersection.Name = "RadioButton_ChessboardStyle_Intersection";
            this.RadioButton_ChessboardStyle_Intersection.Size = new System.Drawing.Size(92, 23);
            this.RadioButton_ChessboardStyle_Intersection.TabIndex = 0;
            this.RadioButton_ChessboardStyle_Intersection.Text = "交点式棋位";
            this.RadioButton_ChessboardStyle_Intersection.UseVisualStyleBackColor = false;
            this.RadioButton_ChessboardStyle_Intersection.CheckedChanged += new System.EventHandler(this.RadioButton_ChessboardStyle_Intersection_CheckedChanged);
            // 
            // RadioButton_ChessboardStyle_Checker
            // 
            this.RadioButton_ChessboardStyle_Checker.AutoSize = true;
            this.RadioButton_ChessboardStyle_Checker.BackColor = System.Drawing.Color.Transparent;
            this.RadioButton_ChessboardStyle_Checker.Font = new System.Drawing.Font("微软雅黑", 9.75F);
            this.RadioButton_ChessboardStyle_Checker.ForeColor = System.Drawing.Color.White;
            this.RadioButton_ChessboardStyle_Checker.Location = new System.Drawing.Point(170, 25);
            this.RadioButton_ChessboardStyle_Checker.Name = "RadioButton_ChessboardStyle_Checker";
            this.RadioButton_ChessboardStyle_Checker.Size = new System.Drawing.Size(92, 23);
            this.RadioButton_ChessboardStyle_Checker.TabIndex = 0;
            this.RadioButton_ChessboardStyle_Checker.Text = "方格式棋位";
            this.RadioButton_ChessboardStyle_Checker.UseVisualStyleBackColor = false;
            this.RadioButton_ChessboardStyle_Checker.CheckedChanged += new System.EventHandler(this.RadioButton_ChessboardStyle_Checker_CheckedChanged);
            // 
            // Panel_BalanceBreaker
            // 
            this.Panel_BalanceBreaker.BackColor = System.Drawing.Color.Transparent;
            this.Panel_BalanceBreaker.Controls.Add(this.Label_BalanceBreaker);
            this.Panel_BalanceBreaker.Controls.Add(this.CheckBox_UseBalanceBreaker);
            this.Panel_BalanceBreaker.Controls.Add(this.CheckBox_ShowBanPoint);
            this.Panel_BalanceBreaker.Controls.Add(this.Label_BalanceBreaker_Info);
            this.Panel_BalanceBreaker.Location = new System.Drawing.Point(30, 175);
            this.Panel_BalanceBreaker.Name = "Panel_BalanceBreaker";
            this.Panel_BalanceBreaker.Size = new System.Drawing.Size(350, 135);
            this.Panel_BalanceBreaker.TabIndex = 0;
            this.Panel_BalanceBreaker.Paint += new System.Windows.Forms.PaintEventHandler(this.Panel_BalanceBreaker_Paint);
            // 
            // Label_BalanceBreaker
            // 
            this.Label_BalanceBreaker.AutoSize = true;
            this.Label_BalanceBreaker.BackColor = System.Drawing.Color.Transparent;
            this.Label_BalanceBreaker.Dock = System.Windows.Forms.DockStyle.Top;
            this.Label_BalanceBreaker.Font = new System.Drawing.Font("微软雅黑", 11.25F);
            this.Label_BalanceBreaker.ForeColor = System.Drawing.Color.White;
            this.Label_BalanceBreaker.Location = new System.Drawing.Point(0, 0);
            this.Label_BalanceBreaker.Name = "Label_BalanceBreaker";
            this.Label_BalanceBreaker.Size = new System.Drawing.Size(39, 20);
            this.Label_BalanceBreaker.TabIndex = 0;
            this.Label_BalanceBreaker.Text = "禁手";
            this.Label_BalanceBreaker.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // CheckBox_UseBalanceBreaker
            // 
            this.CheckBox_UseBalanceBreaker.AutoSize = true;
            this.CheckBox_UseBalanceBreaker.BackColor = System.Drawing.Color.Transparent;
            this.CheckBox_UseBalanceBreaker.Font = new System.Drawing.Font("微软雅黑", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.CheckBox_UseBalanceBreaker.ForeColor = System.Drawing.Color.White;
            this.CheckBox_UseBalanceBreaker.Location = new System.Drawing.Point(25, 30);
            this.CheckBox_UseBalanceBreaker.Name = "CheckBox_UseBalanceBreaker";
            this.CheckBox_UseBalanceBreaker.Size = new System.Drawing.Size(106, 23);
            this.CheckBox_UseBalanceBreaker.TabIndex = 0;
            this.CheckBox_UseBalanceBreaker.TabStop = false;
            this.CheckBox_UseBalanceBreaker.Text = "使用禁手规则";
            this.CheckBox_UseBalanceBreaker.UseVisualStyleBackColor = false;
            this.CheckBox_UseBalanceBreaker.CheckedChanged += new System.EventHandler(this.CheckBox_UseBalanceBreaker_CheckedChanged);
            // 
            // CheckBox_ShowBanPoint
            // 
            this.CheckBox_ShowBanPoint.AutoSize = true;
            this.CheckBox_ShowBanPoint.BackColor = System.Drawing.Color.Transparent;
            this.CheckBox_ShowBanPoint.Font = new System.Drawing.Font("微软雅黑", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.CheckBox_ShowBanPoint.ForeColor = System.Drawing.Color.White;
            this.CheckBox_ShowBanPoint.Location = new System.Drawing.Point(45, 55);
            this.CheckBox_ShowBanPoint.Name = "CheckBox_ShowBanPoint";
            this.CheckBox_ShowBanPoint.Size = new System.Drawing.Size(145, 23);
            this.CheckBox_ShowBanPoint.TabIndex = 0;
            this.CheckBox_ShowBanPoint.TabStop = false;
            this.CheckBox_ShowBanPoint.Text = "在棋盘上标注禁手点";
            this.CheckBox_ShowBanPoint.UseVisualStyleBackColor = false;
            this.CheckBox_ShowBanPoint.CheckedChanged += new System.EventHandler(this.CheckBox_ShowBanPoint_CheckedChanged);
            // 
            // Label_BalanceBreaker_Info
            // 
            this.Label_BalanceBreaker_Info.AutoSize = true;
            this.Label_BalanceBreaker_Info.BackColor = System.Drawing.Color.Transparent;
            this.Label_BalanceBreaker_Info.Font = new System.Drawing.Font("微软雅黑", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.Label_BalanceBreaker_Info.ForeColor = System.Drawing.Color.White;
            this.Label_BalanceBreaker_Info.Location = new System.Drawing.Point(41, 80);
            this.Label_BalanceBreaker_Info.Name = "Label_BalanceBreaker_Info";
            this.Label_BalanceBreaker_Info.Size = new System.Drawing.Size(282, 38);
            this.Label_BalanceBreaker_Info.TabIndex = 0;
            this.Label_BalanceBreaker_Info.Text = "不使用禁手规则，或者使用禁手规则并且在棋盘\r\n上标注禁手点，将不会保存你的记录";
            this.Label_BalanceBreaker_Info.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // Panel_StepTag
            // 
            this.Panel_StepTag.BackColor = System.Drawing.Color.Transparent;
            this.Panel_StepTag.Controls.Add(this.Label_StepTag);
            this.Panel_StepTag.Controls.Add(this.Label_StepTagNum_Val);
            this.Panel_StepTag.Controls.Add(this.Panel_StepTagNumAdjustment);
            this.Panel_StepTag.Location = new System.Drawing.Point(30, 310);
            this.Panel_StepTag.Name = "Panel_StepTag";
            this.Panel_StepTag.Size = new System.Drawing.Size(350, 90);
            this.Panel_StepTag.TabIndex = 0;
            this.Panel_StepTag.Paint += new System.Windows.Forms.PaintEventHandler(this.Panel_StepTag_Paint);
            // 
            // Label_StepTag
            // 
            this.Label_StepTag.AutoSize = true;
            this.Label_StepTag.BackColor = System.Drawing.Color.Transparent;
            this.Label_StepTag.Dock = System.Windows.Forms.DockStyle.Top;
            this.Label_StepTag.Font = new System.Drawing.Font("微软雅黑", 11.25F);
            this.Label_StepTag.ForeColor = System.Drawing.Color.White;
            this.Label_StepTag.Location = new System.Drawing.Point(0, 0);
            this.Label_StepTag.Name = "Label_StepTag";
            this.Label_StepTag.Size = new System.Drawing.Size(69, 20);
            this.Label_StepTag.TabIndex = 0;
            this.Label_StepTag.Text = "落子序号";
            this.Label_StepTag.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // Label_StepTagNum_Val
            // 
            this.Label_StepTagNum_Val.AutoSize = true;
            this.Label_StepTagNum_Val.BackColor = System.Drawing.Color.Transparent;
            this.Label_StepTagNum_Val.Font = new System.Drawing.Font("微软雅黑", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.Label_StepTagNum_Val.ForeColor = System.Drawing.Color.White;
            this.Label_StepTagNum_Val.Location = new System.Drawing.Point(25, 30);
            this.Label_StepTagNum_Val.Name = "Label_StepTagNum_Val";
            this.Label_StepTagNum_Val.Size = new System.Drawing.Size(98, 19);
            this.Label_StepTagNum_Val.TabIndex = 0;
            this.Label_StepTagNum_Val.Text = "显示最后224手";
            this.Label_StepTagNum_Val.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // Panel_StepTagNumAdjustment
            // 
            this.Panel_StepTagNumAdjustment.BackColor = System.Drawing.Color.Transparent;
            this.Panel_StepTagNumAdjustment.Location = new System.Drawing.Point(25, 55);
            this.Panel_StepTagNumAdjustment.Name = "Panel_StepTagNumAdjustment";
            this.Panel_StepTagNumAdjustment.Size = new System.Drawing.Size(300, 24);
            this.Panel_StepTagNumAdjustment.TabIndex = 0;
            this.Panel_StepTagNumAdjustment.Paint += new System.Windows.Forms.PaintEventHandler(this.Panel_StepTagNumAdjustment_Paint);
            this.Panel_StepTagNumAdjustment.MouseDown += new System.Windows.Forms.MouseEventHandler(this.Panel_StepTagNumAdjustment_MouseDown);
            this.Panel_StepTagNumAdjustment.MouseEnter += new System.EventHandler(this.Panel_StepTagNumAdjustment_MouseEnter);
            this.Panel_StepTagNumAdjustment.MouseLeave += new System.EventHandler(this.Panel_StepTagNumAdjustment_MouseLeave);
            this.Panel_StepTagNumAdjustment.MouseMove += new System.Windows.Forms.MouseEventHandler(this.Panel_StepTagNumAdjustment_MouseMove);
            this.Panel_StepTagNumAdjustment.MouseUp += new System.Windows.Forms.MouseEventHandler(this.Panel_StepTagNumAdjustment_MouseUp);
            this.Panel_StepTagNumAdjustment.MouseWheel += new System.Windows.Forms.MouseEventHandler(this.Panel_StepTagNumAdjustment_MouseWheel);
            // 
            // Panel_ThemeColor
            // 
            this.Panel_ThemeColor.BackColor = System.Drawing.Color.Transparent;
            this.Panel_ThemeColor.Controls.Add(this.Label_ThemeColor);
            this.Panel_ThemeColor.Controls.Add(this.RadioButton_UseRandomThemeColor);
            this.Panel_ThemeColor.Controls.Add(this.RadioButton_UseCustomColor);
            this.Panel_ThemeColor.Controls.Add(this.Label_ThemeColorName);
            this.Panel_ThemeColor.Location = new System.Drawing.Point(30, 400);
            this.Panel_ThemeColor.Name = "Panel_ThemeColor";
            this.Panel_ThemeColor.Size = new System.Drawing.Size(350, 115);
            this.Panel_ThemeColor.TabIndex = 0;
            this.Panel_ThemeColor.Paint += new System.Windows.Forms.PaintEventHandler(this.Panel_ThemeColor_Paint);
            // 
            // Label_ThemeColor
            // 
            this.Label_ThemeColor.AutoSize = true;
            this.Label_ThemeColor.BackColor = System.Drawing.Color.Transparent;
            this.Label_ThemeColor.Dock = System.Windows.Forms.DockStyle.Top;
            this.Label_ThemeColor.Font = new System.Drawing.Font("微软雅黑", 11.25F);
            this.Label_ThemeColor.ForeColor = System.Drawing.Color.White;
            this.Label_ThemeColor.Location = new System.Drawing.Point(0, 0);
            this.Label_ThemeColor.Name = "Label_ThemeColor";
            this.Label_ThemeColor.Size = new System.Drawing.Size(54, 20);
            this.Label_ThemeColor.TabIndex = 0;
            this.Label_ThemeColor.Text = "主题色";
            // 
            // RadioButton_UseRandomThemeColor
            // 
            this.RadioButton_UseRandomThemeColor.AutoSize = true;
            this.RadioButton_UseRandomThemeColor.BackColor = System.Drawing.Color.Transparent;
            this.RadioButton_UseRandomThemeColor.Font = new System.Drawing.Font("微软雅黑", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.RadioButton_UseRandomThemeColor.ForeColor = System.Drawing.Color.White;
            this.RadioButton_UseRandomThemeColor.Location = new System.Drawing.Point(25, 30);
            this.RadioButton_UseRandomThemeColor.Name = "RadioButton_UseRandomThemeColor";
            this.RadioButton_UseRandomThemeColor.Size = new System.Drawing.Size(53, 23);
            this.RadioButton_UseRandomThemeColor.TabIndex = 0;
            this.RadioButton_UseRandomThemeColor.Text = "随机";
            this.RadioButton_UseRandomThemeColor.UseVisualStyleBackColor = false;
            this.RadioButton_UseRandomThemeColor.CheckedChanged += new System.EventHandler(this.RadioButton_UseRandomThemeColor_CheckedChanged);
            // 
            // RadioButton_UseCustomColor
            // 
            this.RadioButton_UseCustomColor.AutoSize = true;
            this.RadioButton_UseCustomColor.BackColor = System.Drawing.Color.Transparent;
            this.RadioButton_UseCustomColor.Font = new System.Drawing.Font("微软雅黑", 9.75F);
            this.RadioButton_UseCustomColor.ForeColor = System.Drawing.Color.White;
            this.RadioButton_UseCustomColor.Location = new System.Drawing.Point(25, 55);
            this.RadioButton_UseCustomColor.Name = "RadioButton_UseCustomColor";
            this.RadioButton_UseCustomColor.Size = new System.Drawing.Size(69, 23);
            this.RadioButton_UseCustomColor.TabIndex = 0;
            this.RadioButton_UseCustomColor.Text = "自定义:";
            this.RadioButton_UseCustomColor.UseVisualStyleBackColor = false;
            this.RadioButton_UseCustomColor.CheckedChanged += new System.EventHandler(this.RadioButton_UseCustomColor_CheckedChanged);
            // 
            // Label_ThemeColorName
            // 
            this.Label_ThemeColorName.AutoSize = true;
            this.Label_ThemeColorName.BackColor = System.Drawing.Color.Transparent;
            this.Label_ThemeColorName.Font = new System.Drawing.Font("微软雅黑", 9.75F, System.Drawing.FontStyle.Underline, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.Label_ThemeColorName.ForeColor = System.Drawing.Color.White;
            this.Label_ThemeColorName.Location = new System.Drawing.Point(41, 80);
            this.Label_ThemeColorName.Name = "Label_ThemeColorName";
            this.Label_ThemeColorName.Size = new System.Drawing.Size(83, 19);
            this.Label_ThemeColorName.TabIndex = 0;
            this.Label_ThemeColorName.Text = "ThemeColor";
            // 
            // Panel_AntiAlias
            // 
            this.Panel_AntiAlias.BackColor = System.Drawing.Color.Transparent;
            this.Panel_AntiAlias.Controls.Add(this.CheckBox_AntiAlias);
            this.Panel_AntiAlias.Controls.Add(this.Label_AntiAlias);
            this.Panel_AntiAlias.Location = new System.Drawing.Point(30, 515);
            this.Panel_AntiAlias.Name = "Panel_AntiAlias";
            this.Panel_AntiAlias.Size = new System.Drawing.Size(350, 65);
            this.Panel_AntiAlias.TabIndex = 0;
            this.Panel_AntiAlias.Paint += new System.Windows.Forms.PaintEventHandler(this.Panel_AntiAlias_Paint);
            // 
            // CheckBox_AntiAlias
            // 
            this.CheckBox_AntiAlias.AutoSize = true;
            this.CheckBox_AntiAlias.BackColor = System.Drawing.Color.Transparent;
            this.CheckBox_AntiAlias.Font = new System.Drawing.Font("微软雅黑", 9.75F);
            this.CheckBox_AntiAlias.ForeColor = System.Drawing.Color.White;
            this.CheckBox_AntiAlias.Location = new System.Drawing.Point(25, 30);
            this.CheckBox_AntiAlias.Name = "CheckBox_AntiAlias";
            this.CheckBox_AntiAlias.Size = new System.Drawing.Size(145, 23);
            this.CheckBox_AntiAlias.TabIndex = 0;
            this.CheckBox_AntiAlias.TabStop = false;
            this.CheckBox_AntiAlias.Text = "使用抗锯齿模式绘图";
            this.CheckBox_AntiAlias.UseVisualStyleBackColor = false;
            this.CheckBox_AntiAlias.CheckedChanged += new System.EventHandler(this.CheckBox_AntiAlias_CheckedChanged);
            // 
            // Label_AntiAlias
            // 
            this.Label_AntiAlias.AutoSize = true;
            this.Label_AntiAlias.BackColor = System.Drawing.Color.Transparent;
            this.Label_AntiAlias.Dock = System.Windows.Forms.DockStyle.Top;
            this.Label_AntiAlias.Font = new System.Drawing.Font("微软雅黑", 11.25F);
            this.Label_AntiAlias.ForeColor = System.Drawing.Color.White;
            this.Label_AntiAlias.Location = new System.Drawing.Point(0, 0);
            this.Label_AntiAlias.Name = "Label_AntiAlias";
            this.Label_AntiAlias.Size = new System.Drawing.Size(54, 20);
            this.Label_AntiAlias.TabIndex = 0;
            this.Label_AntiAlias.Text = "抗锯齿";
            // 
            // Panel_Tab_About
            // 
            this.Panel_Tab_About.BackColor = System.Drawing.Color.Transparent;
            this.Panel_Tab_About.Controls.Add(this.PictureBox_ApplicationLogo);
            this.Panel_Tab_About.Controls.Add(this.Label_ApplicationName);
            this.Panel_Tab_About.Controls.Add(this.Label_ApplicationEdition);
            this.Panel_Tab_About.Controls.Add(this.Label_Version);
            this.Panel_Tab_About.Controls.Add(this.Label_Copyright);
            this.Panel_Tab_About.Controls.Add(this.Panel_GitHub);
            this.Panel_Tab_About.Location = new System.Drawing.Point(0, 0);
            this.Panel_Tab_About.MinimumSize = new System.Drawing.Size(395, 315);
            this.Panel_Tab_About.Name = "Panel_Tab_About";
            this.Panel_Tab_About.Size = new System.Drawing.Size(435, 420);
            this.Panel_Tab_About.TabIndex = 0;
            this.Panel_Tab_About.Visible = false;
            // 
            // PictureBox_ApplicationLogo
            // 
            this.PictureBox_ApplicationLogo.BackColor = System.Drawing.Color.Transparent;
            this.PictureBox_ApplicationLogo.ErrorImage = null;
            this.PictureBox_ApplicationLogo.Image = ((System.Drawing.Image)(resources.GetObject("PictureBox_ApplicationLogo.Image")));
            this.PictureBox_ApplicationLogo.InitialImage = null;
            this.PictureBox_ApplicationLogo.Location = new System.Drawing.Point(60, 60);
            this.PictureBox_ApplicationLogo.Name = "PictureBox_ApplicationLogo";
            this.PictureBox_ApplicationLogo.Size = new System.Drawing.Size(64, 64);
            this.PictureBox_ApplicationLogo.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.PictureBox_ApplicationLogo.TabIndex = 0;
            this.PictureBox_ApplicationLogo.TabStop = false;
            // 
            // Label_ApplicationName
            // 
            this.Label_ApplicationName.AutoSize = true;
            this.Label_ApplicationName.BackColor = System.Drawing.Color.Transparent;
            this.Label_ApplicationName.Font = new System.Drawing.Font("微软雅黑", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.Label_ApplicationName.ForeColor = System.Drawing.Color.White;
            this.Label_ApplicationName.Location = new System.Drawing.Point(155, 65);
            this.Label_ApplicationName.Name = "Label_ApplicationName";
            this.Label_ApplicationName.Size = new System.Drawing.Size(212, 31);
            this.Label_ApplicationName.TabIndex = 0;
            this.Label_ApplicationName.Text = "ApplicationName";
            // 
            // Label_ApplicationEdition
            // 
            this.Label_ApplicationEdition.AutoSize = true;
            this.Label_ApplicationEdition.BackColor = System.Drawing.Color.Transparent;
            this.Label_ApplicationEdition.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.Label_ApplicationEdition.ForeColor = System.Drawing.Color.White;
            this.Label_ApplicationEdition.Location = new System.Drawing.Point(157, 100);
            this.Label_ApplicationEdition.Name = "Label_ApplicationEdition";
            this.Label_ApplicationEdition.Size = new System.Drawing.Size(149, 21);
            this.Label_ApplicationEdition.TabIndex = 0;
            this.Label_ApplicationEdition.Text = "ApplicationEdition";
            // 
            // Label_Version
            // 
            this.Label_Version.AutoSize = true;
            this.Label_Version.BackColor = System.Drawing.Color.Transparent;
            this.Label_Version.Font = new System.Drawing.Font("微软雅黑", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.Label_Version.ForeColor = System.Drawing.Color.White;
            this.Label_Version.Location = new System.Drawing.Point(60, 185);
            this.Label_Version.Name = "Label_Version";
            this.Label_Version.Size = new System.Drawing.Size(88, 19);
            this.Label_Version.TabIndex = 0;
            this.Label_Version.Text = "版本: Version";
            // 
            // Label_Copyright
            // 
            this.Label_Copyright.AutoSize = true;
            this.Label_Copyright.BackColor = System.Drawing.Color.Transparent;
            this.Label_Copyright.Font = new System.Drawing.Font("微软雅黑", 9.75F);
            this.Label_Copyright.ForeColor = System.Drawing.Color.White;
            this.Label_Copyright.Location = new System.Drawing.Point(60, 210);
            this.Label_Copyright.Name = "Label_Copyright";
            this.Label_Copyright.Size = new System.Drawing.Size(272, 19);
            this.Label_Copyright.TabIndex = 0;
            this.Label_Copyright.Text = "Copyright © 2020 chibayuki@foxmail.com";
            // 
            // Panel_GitHub
            // 
            this.Panel_GitHub.BackColor = System.Drawing.Color.Transparent;
            this.Panel_GitHub.Controls.Add(this.Label_GitHub_Part1);
            this.Panel_GitHub.Controls.Add(this.Label_GitHub_Base);
            this.Panel_GitHub.Controls.Add(this.Label_GitHub_Part2);
            this.Panel_GitHub.Controls.Add(this.Label_GitHub_Release);
            this.Panel_GitHub.Location = new System.Drawing.Point(60, 235);
            this.Panel_GitHub.Name = "Panel_GitHub";
            this.Panel_GitHub.Size = new System.Drawing.Size(270, 19);
            this.Panel_GitHub.TabIndex = 0;
            // 
            // Label_GitHub_Part1
            // 
            this.Label_GitHub_Part1.AutoSize = true;
            this.Label_GitHub_Part1.BackColor = System.Drawing.Color.Transparent;
            this.Label_GitHub_Part1.Font = new System.Drawing.Font("微软雅黑", 9.75F);
            this.Label_GitHub_Part1.ForeColor = System.Drawing.Color.White;
            this.Label_GitHub_Part1.Location = new System.Drawing.Point(0, 0);
            this.Label_GitHub_Part1.Name = "Label_GitHub_Part1";
            this.Label_GitHub_Part1.Size = new System.Drawing.Size(113, 19);
            this.Label_GitHub_Part1.TabIndex = 0;
            this.Label_GitHub_Part1.Text = "访问 GitHub 查看";
            // 
            // Label_GitHub_Base
            // 
            this.Label_GitHub_Base.AutoSize = true;
            this.Label_GitHub_Base.BackColor = System.Drawing.Color.Transparent;
            this.Label_GitHub_Base.Font = new System.Drawing.Font("微软雅黑", 9.75F, System.Drawing.FontStyle.Underline);
            this.Label_GitHub_Base.ForeColor = System.Drawing.Color.White;
            this.Label_GitHub_Base.Location = new System.Drawing.Point(113, 0);
            this.Label_GitHub_Base.Name = "Label_GitHub_Base";
            this.Label_GitHub_Base.Size = new System.Drawing.Size(48, 19);
            this.Label_GitHub_Base.TabIndex = 0;
            this.Label_GitHub_Base.Text = "源代码";
            // 
            // Label_GitHub_Part2
            // 
            this.Label_GitHub_Part2.AutoSize = true;
            this.Label_GitHub_Part2.BackColor = System.Drawing.Color.Transparent;
            this.Label_GitHub_Part2.Font = new System.Drawing.Font("微软雅黑", 9.75F);
            this.Label_GitHub_Part2.ForeColor = System.Drawing.Color.White;
            this.Label_GitHub_Part2.Location = new System.Drawing.Point(161, 0);
            this.Label_GitHub_Part2.Name = "Label_GitHub_Part2";
            this.Label_GitHub_Part2.Size = new System.Drawing.Size(22, 19);
            this.Label_GitHub_Part2.TabIndex = 0;
            this.Label_GitHub_Part2.Text = "或";
            // 
            // Label_GitHub_Release
            // 
            this.Label_GitHub_Release.AutoSize = true;
            this.Label_GitHub_Release.BackColor = System.Drawing.Color.Transparent;
            this.Label_GitHub_Release.Font = new System.Drawing.Font("微软雅黑", 9.75F, System.Drawing.FontStyle.Underline);
            this.Label_GitHub_Release.ForeColor = System.Drawing.Color.White;
            this.Label_GitHub_Release.Location = new System.Drawing.Point(183, 0);
            this.Label_GitHub_Release.Name = "Label_GitHub_Release";
            this.Label_GitHub_Release.Size = new System.Drawing.Size(87, 19);
            this.Label_GitHub_Release.TabIndex = 0;
            this.Label_GitHub_Release.Text = "最新发布版本";
            // 
            // Panel_GameUI
            // 
            this.Panel_GameUI.BackColor = System.Drawing.Color.Transparent;
            this.Panel_GameUI.Controls.Add(this.Panel_Current);
            this.Panel_GameUI.Controls.Add(this.Panel_Environment);
            this.Panel_GameUI.Location = new System.Drawing.Point(0, 0);
            this.Panel_GameUI.Name = "Panel_GameUI";
            this.Panel_GameUI.Size = new System.Drawing.Size(585, 420);
            this.Panel_GameUI.TabIndex = 0;
            this.Panel_GameUI.Visible = false;
            // 
            // Panel_Current
            // 
            this.Panel_Current.BackColor = System.Drawing.Color.Transparent;
            this.Panel_Current.Controls.Add(this.Panel_Interrupt);
            this.Panel_Current.Location = new System.Drawing.Point(0, 0);
            this.Panel_Current.Name = "Panel_Current";
            this.Panel_Current.Size = new System.Drawing.Size(585, 50);
            this.Panel_Current.TabIndex = 0;
            this.Panel_Current.Paint += new System.Windows.Forms.PaintEventHandler(this.Panel_Current_Paint);
            // 
            // Panel_Interrupt
            // 
            this.Panel_Interrupt.Controls.Add(this.PictureBox_Restart);
            this.Panel_Interrupt.Controls.Add(this.PictureBox_ExitGame);
            this.Panel_Interrupt.Location = new System.Drawing.Point(485, 0);
            this.Panel_Interrupt.Name = "Panel_Interrupt";
            this.Panel_Interrupt.Size = new System.Drawing.Size(100, 50);
            this.Panel_Interrupt.TabIndex = 0;
            // 
            // PictureBox_Restart
            // 
            this.PictureBox_Restart.BackColor = System.Drawing.Color.Transparent;
            this.PictureBox_Restart.ErrorImage = null;
            this.PictureBox_Restart.InitialImage = null;
            this.PictureBox_Restart.Location = new System.Drawing.Point(0, 0);
            this.PictureBox_Restart.Name = "PictureBox_Restart";
            this.PictureBox_Restart.Size = new System.Drawing.Size(50, 50);
            this.PictureBox_Restart.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
            this.PictureBox_Restart.TabIndex = 0;
            this.PictureBox_Restart.TabStop = false;
            // 
            // PictureBox_ExitGame
            // 
            this.PictureBox_ExitGame.BackColor = System.Drawing.Color.Transparent;
            this.PictureBox_ExitGame.ErrorImage = null;
            this.PictureBox_ExitGame.InitialImage = null;
            this.PictureBox_ExitGame.Location = new System.Drawing.Point(50, 0);
            this.PictureBox_ExitGame.Name = "PictureBox_ExitGame";
            this.PictureBox_ExitGame.Size = new System.Drawing.Size(50, 50);
            this.PictureBox_ExitGame.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
            this.PictureBox_ExitGame.TabIndex = 0;
            this.PictureBox_ExitGame.TabStop = false;
            // 
            // Panel_Environment
            // 
            this.Panel_Environment.BackColor = System.Drawing.Color.Transparent;
            this.Panel_Environment.Location = new System.Drawing.Point(0, 50);
            this.Panel_Environment.Name = "Panel_Environment";
            this.Panel_Environment.Size = new System.Drawing.Size(585, 370);
            this.Panel_Environment.TabIndex = 0;
            this.Panel_Environment.KeyDown += new System.Windows.Forms.KeyEventHandler(this.Panel_Environment_KeyDown);
            this.Panel_Environment.Paint += new System.Windows.Forms.PaintEventHandler(this.Panel_Environment_Paint);
            this.Panel_Environment.MouseDown += new System.Windows.Forms.MouseEventHandler(this.Panel_Environment_MouseDown);
            this.Panel_Environment.MouseLeave += new System.EventHandler(this.Panel_Environment_MouseLeave);
            this.Panel_Environment.MouseMove += new System.Windows.Forms.MouseEventHandler(this.Panel_Environment_MouseMove);
            this.Panel_Environment.MouseWheel += new System.Windows.Forms.MouseEventHandler(this.Panel_Environment_MouseWheel);
            // 
            // ImageList_Interrupt
            // 
            this.ImageList_Interrupt.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("ImageList_Interrupt.ImageStream")));
            this.ImageList_Interrupt.TransparentColor = System.Drawing.Color.Transparent;
            this.ImageList_Interrupt.Images.SetKeyName(0, "Breakall.png");
            this.ImageList_Interrupt.Images.SetKeyName(1, "Run.png");
            // 
            // Timer_EnterPrompt
            // 
            this.Timer_EnterPrompt.Interval = 10;
            this.Timer_EnterPrompt.Tick += new System.EventHandler(this.Timer_EnterPrompt_Tick);
            // 
            // Timer_ChessClock
            // 
            this.Timer_ChessClock.Interval = 10;
            this.Timer_ChessClock.Tick += new System.EventHandler(this.Timer_ChessClock_Tick);
            // 
            // ToolTip_InterruptPrompt
            // 
            this.ToolTip_InterruptPrompt.ShowAlways = true;
            // 
            // ColorDialog_ThemeColor
            // 
            this.ColorDialog_ThemeColor.Color = System.Drawing.Color.White;
            this.ColorDialog_ThemeColor.FullOpen = true;
            // 
            // Form_Main
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Black;
            this.ClientSize = new System.Drawing.Size(585, 420);
            this.Controls.Add(this.Panel_Main);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "Form_Main";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Panel_Main.ResumeLayout(false);
            this.Panel_Client.ResumeLayout(false);
            this.Panel_FunctionArea.ResumeLayout(false);
            this.Panel_FunctionAreaOptionsBar.ResumeLayout(false);
            this.Panel_FunctionAreaTab.ResumeLayout(false);
            this.Panel_Tab_Start.ResumeLayout(false);
            this.Panel_EnterGameSelection.ResumeLayout(false);
            this.Panel_Tab_Record.ResumeLayout(false);
            this.Panel_Score.ResumeLayout(false);
            this.Panel_Score.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.PictureBox_Overview)).EndInit();
            this.Panel_GameTime.ResumeLayout(false);
            this.Panel_GameTime.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.PictureBox_GameTime)).EndInit();
            this.Panel_Tab_Options.ResumeLayout(false);
            this.Panel_Chessboard.ResumeLayout(false);
            this.Panel_Chessboard.PerformLayout();
            this.Panel_ChessboardType.ResumeLayout(false);
            this.Panel_ChessboardType.PerformLayout();
            this.Panel_ChessboardStyle.ResumeLayout(false);
            this.Panel_ChessboardStyle.PerformLayout();
            this.Panel_BalanceBreaker.ResumeLayout(false);
            this.Panel_BalanceBreaker.PerformLayout();
            this.Panel_StepTag.ResumeLayout(false);
            this.Panel_StepTag.PerformLayout();
            this.Panel_ThemeColor.ResumeLayout(false);
            this.Panel_ThemeColor.PerformLayout();
            this.Panel_AntiAlias.ResumeLayout(false);
            this.Panel_AntiAlias.PerformLayout();
            this.Panel_Tab_About.ResumeLayout(false);
            this.Panel_Tab_About.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.PictureBox_ApplicationLogo)).EndInit();
            this.Panel_GitHub.ResumeLayout(false);
            this.Panel_GitHub.PerformLayout();
            this.Panel_GameUI.ResumeLayout(false);
            this.Panel_Current.ResumeLayout(false);
            this.Panel_Interrupt.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.PictureBox_Restart)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.PictureBox_ExitGame)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel Panel_Main;
        private System.Windows.Forms.Panel Panel_Environment;
        private System.Windows.Forms.Panel Panel_Client;
        private System.Windows.Forms.Panel Panel_Current;
        private System.Windows.Forms.Panel Panel_GameUI;
        private System.Windows.Forms.Panel Panel_FunctionArea;
        private System.Windows.Forms.Panel Panel_FunctionAreaOptionsBar;
        private System.Windows.Forms.Panel Panel_Tab_Record;
        private System.Windows.Forms.Label Label_TotalTime;
        private System.Windows.Forms.Label Label_ThisTime;
        private System.Windows.Forms.Label Label_TotalTimeVal;
        private System.Windows.Forms.Label Label_ThisTimeVal;
        private System.Windows.Forms.Panel Panel_Tab_About;
        private System.Windows.Forms.Label Label_ApplicationEdition;
        private System.Windows.Forms.Label Label_Copyright;
        private System.Windows.Forms.Label Label_Version;
        private System.Windows.Forms.Label Label_ApplicationName;
        private System.Windows.Forms.PictureBox PictureBox_ApplicationLogo;
        private System.Windows.Forms.Panel Panel_Tab_Options;
        private System.Windows.Forms.Panel Panel_Tab_Start;
        private System.Windows.Forms.Panel Panel_EnterGameSelection;
        private System.Windows.Forms.Timer Timer_EnterPrompt;
        private System.Windows.Forms.ImageList ImageList_Interrupt;
        private System.Windows.Forms.Panel Panel_Score;
        private System.Windows.Forms.Panel Panel_GameTime;
        private System.Windows.Forms.Timer Timer_ChessClock;
        private System.Windows.Forms.Label Label_TotalGames;
        private System.Windows.Forms.Label Label_DrawGamesVal;
        private System.Windows.Forms.Label Label_DrawGames;
        private System.Windows.Forms.Label Label_UserLoseGamesVal;
        private System.Windows.Forms.Label Label_UserLoseGames;
        private System.Windows.Forms.Label Label_UserWinGamesVal;
        private System.Windows.Forms.Label Label_UserWinGames;
        private System.Windows.Forms.Label Label_TotalGamesVal;
        private System.Windows.Forms.Panel Panel_BalanceBreaker;
        private System.Windows.Forms.Label Label_BalanceBreaker;
        private System.Windows.Forms.CheckBox CheckBox_UseBalanceBreaker;
        private System.Windows.Forms.Panel Panel_StepTag;
        private System.Windows.Forms.Label Label_StepTag;
        private System.Windows.Forms.Panel Panel_StepTagNumAdjustment;
        private System.Windows.Forms.Label Label_StepTagNum_Val;
        private System.Windows.Forms.CheckBox CheckBox_ShowBanPoint;
        private System.Windows.Forms.ToolTip ToolTip_InterruptPrompt;
        private System.Windows.Forms.Panel Panel_FunctionAreaTab;
        private System.Windows.Forms.Panel Panel_Interrupt;
        private System.Windows.Forms.PictureBox PictureBox_Overview;
        private System.Windows.Forms.PictureBox PictureBox_GameTime;
        private System.Windows.Forms.Panel Panel_AntiAlias;
        private System.Windows.Forms.CheckBox CheckBox_AntiAlias;
        private System.Windows.Forms.Label Label_AntiAlias;
        private System.Windows.Forms.Panel Panel_Chessboard;
        private System.Windows.Forms.Label Label_Chessboard;
        private System.Windows.Forms.RadioButton RadioButton_ChessboardType_Gomoku_15x15;
        private System.Windows.Forms.RadioButton RadioButton_ChessboardType_Go_19x19;
        private System.Windows.Forms.Label Label_ChessboardType;
        private System.Windows.Forms.Label Label_ChessboardStyle;
        private System.Windows.Forms.RadioButton RadioButton_ChessboardStyle_Intersection;
        private System.Windows.Forms.RadioButton RadioButton_ChessboardStyle_Checker;
        private System.Windows.Forms.Panel Panel_ChessboardStyle;
        private System.Windows.Forms.Panel Panel_ChessboardType;
        private System.Windows.Forms.ColorDialog ColorDialog_ThemeColor;
        private System.Windows.Forms.Panel Panel_ThemeColor;
        private System.Windows.Forms.Label Label_ThemeColor;
        private System.Windows.Forms.RadioButton RadioButton_UseRandomThemeColor;
        private System.Windows.Forms.RadioButton RadioButton_UseCustomColor;
        private System.Windows.Forms.Label Label_ThemeColorName;
        private System.Windows.Forms.Label Label_BalanceBreaker_Info;
        private System.Windows.Forms.Label Label_Tab_Start;
        private System.Windows.Forms.Label Label_Tab_Record;
        private System.Windows.Forms.Label Label_Tab_Options;
        private System.Windows.Forms.Label Label_Tab_About;
        private System.Windows.Forms.PictureBox PictureBox_Restart;
        private System.Windows.Forms.PictureBox PictureBox_ExitGame;
        private System.Windows.Forms.Label Label_StartNewGame;
        private System.Windows.Forms.Label Label_ContinueLastGame;
        private System.Windows.Forms.Panel Panel_GitHub;
        private System.Windows.Forms.Label Label_GitHub_Part1;
        private System.Windows.Forms.Label Label_GitHub_Base;
        private System.Windows.Forms.Label Label_GitHub_Part2;
        private System.Windows.Forms.Label Label_GitHub_Release;
    }
}