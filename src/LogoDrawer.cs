using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Drawing.Text;
using System.Windows.Forms;

// Классы
namespace RD_AAOW
	{
	/// <summary>
	/// Класс обеспечивает отображение логотипа мода
	/// </summary>
	public partial class LogoDrawer:Form
		{
		// Переменные
		private int mainScale, logoScale;       // Главный масштабный множитель
		private int drawerSize;                 // Размер кисти
		private int tailsSize;                  // Длина "хвостов" лого за границами кругов

		private const int frameSpeed = 6;       // Частота смены кадров / отступ кисти в пикселях

		private int logoFontSize;               // Размер шрифта лого
		private int headerFontSize;             // Размер шрифта заголовков расширенного режима
		private int textFontSize;               // Размер шрифта текста расширенного режима
		private const string logoString1 = "Free development lab",      // Тексты лого
			logoString2 = "RD AAOW";

		private uint phase1 = 1, phase2 = 1;    // Текущие фазы отрисовки
		private Point point1, point2,           // Текущие позиции отрисовки
			point3, point4;
		private double arc1;                    // Переменные для расчёта позиций элементов в полярных координатах

		private Graphics g, g2;                 // Объекты-отрисовщики
		private SolidBrush foreBrush, backBrush, backHidingBrush1, backHidingBrush2;
		private Pen backPen;
		private Bitmap logo1;
#if LDDEXPORT
		private Bitmap logo4b;
#endif
		private Font logo1Font, logo2Font, headerFont, textFont;
		private SizeF logo1Size, logo2Size;     // Графические размеры текста для текущего экрана

		private uint extended = 0;              // Тип расширенного режима

		private uint steps = 0, moves = 0;      // Счётчик шагов

		private const int lineFeed = 40;        // Высота строки текста расширенного режима
		private const int lineLeft = 250;       // Начало строки текста расширенного режима

		private Random rnd = new Random ();     // ГПСЧ

		// Строки текста расширенного режима
		private List<List<LogoDrawerString>> extendedStrings1 = new List<List<LogoDrawerString>> ();

		/// <summary>
		/// Доступные режимы отрисовки основного лого
		/// </summary>
		public enum DrawModes
			{
			/// <summary>
			/// Режим 1
			/// </summary>
			Mode1 = 0,

			/// <summary>
			/// Режим 2
			/// </summary>
			Mode2 = 1
			}
		private DrawModes mode;

		/// <summary>
		/// Конструктор. Инициализирует экземпляр отрисовщика
		/// </summary>
		public LogoDrawer ()
			{
			// Инициализация
#if LDDEBUG
			extended = 1;
#else
			extended = (uint)((rnd.Next (5) == 0) ? 1 : 0);
#endif
			mode = DrawModes.Mode1;

			InitializeComponent ();
			}

		private void LogoDrawer_Shown (object sender, EventArgs e)
			{
			// Если запрос границ экрана завершается ошибкой, отменяем отображение
			this.Left = this.Top = 0;
			try
				{
				this.Width = Screen.PrimaryScreen.Bounds.Width;
				this.Height = Screen.PrimaryScreen.Bounds.Height;
				}
			catch
				{
				this.Close ();
				return;
				}

			// Настройка окна
			this.BackColor = ProgramDescription.MasterBackColor;    // Color.FromArgb (192, 255, 224);
			this.ForeColor = ProgramDescription.MasterTextColor;    // Color.FromArgb (32, 32, 32);

			// Инициализация
			mainScale = this.Width / 10;
			logoScale = 85 * mainScale / 100;
			drawerSize = 10 * mainScale / 95;
			tailsSize = 3 * mainScale / 10;
			logoFontSize = 43 * mainScale / 100;
			headerFontSize = 20 * mainScale / 100;
			textFontSize = 13 * mainScale / 100;

			backBrush = new SolidBrush (this.BackColor);
			backPen = new Pen (this.BackColor, mainScale / 7);  // More than drawerSize
			backHidingBrush1 = new SolidBrush (Color.FromArgb (15, this.BackColor.R, this.BackColor.G, this.BackColor.B));
			backHidingBrush2 = new SolidBrush (Color.FromArgb (50, this.BackColor.R, this.BackColor.G, this.BackColor.B));

#if LDDEXPORT
			logo4b = new Bitmap (this.Width, this.Height);
			g = Graphics.FromImage (logo4b);
			g.FillRectangle (backBrush, 0, 0, this.Width, this.Height);
#else
			g = Graphics.FromHwnd (this.Handle);
#endif
			g.TextRenderingHint = TextRenderingHint.SingleBitPerPixelGridFit;   // Убирает ауру на буквах в Win8

			logo1Font = new Font ("Lucida Sans Unicode", logoFontSize);
			logo1Size = g.MeasureString (logoString1, logo1Font);
			logo2Font = new Font ("Lucida Sans Unicode", logoFontSize * 0.7f);
			logo2Size = g.MeasureString (logoString2, logo2Font);

			headerFont = new Font ("Lucida Console", headerFontSize, FontStyle.Bold);
			textFont = new Font ("Lucida Console", textFontSize);

			#region Установка начальных позиций и методов отрисовки

			switch (mode)
				{
				default:
				case DrawModes.Mode1:
					point1 = new Point (this.Width / 6, this.Height / 2);
					point2 = new Point (this.Width / 6, this.Height / 2);
					point3 = new Point (this.Width / 6, this.Height / 2);
					point4 = new Point (this.Width / 6, this.Height / 2);
					DrawingTimer.Tick += DrawingTimer_Mode1;
					break;

				case DrawModes.Mode2:
					point1 = new Point (this.Width / 2, this.Height / 2);
					point2 = new Point (this.Width / 2, this.Height / 2);
					point3 = new Point (this.Width / 2, this.Height / 2);
					point4 = new Point (this.Width / 2, this.Height / 2);
					DrawingTimer.Tick += DrawingTimer_Mode2;
					break;
				}

			arc1 = 0.0;
			foreBrush = new SolidBrush (Color.FromArgb (3, this.ForeColor.R, this.ForeColor.G, this.ForeColor.B));

			if (extended == 1)
				ExtendedTimer.Tick += ExtendedTimer1_Tick;

			#endregion

			#region Тексты расширенного режима

			// Текст расширенного режима, вариант 1
			uint headerLetterWidth = (uint)(g.MeasureString ("A", headerFont).Width * 0.8f);
			uint textLetterWidth = (uint)(g.MeasureString ("A", textFont).Width * 0.8f);

			extendedStrings1.Add (new List<LogoDrawerString> ());
			extendedStrings1[extendedStrings1.Count - 1].Add (new LogoDrawerString (
				"What does our logo mean?", headerFont, 50, headerLetterWidth));
			extendedStrings1[extendedStrings1.Count - 1].Add (new LogoDrawerString (
				"Some explanations from " + RDGenerics.AssemblyCopyright, textFont, 80, textLetterWidth));

			extendedStrings1.Add (new List<LogoDrawerString> ());
			extendedStrings1[extendedStrings1.Count - 1].Add (new LogoDrawerString (" ", textFont, 0, textLetterWidth));
			extendedStrings1[extendedStrings1.Count - 1].Add (new LogoDrawerString (
				"- What does “ESHQ” mean?", textFont, 50, textLetterWidth));
			extendedStrings1[extendedStrings1.Count - 1].Add (new LogoDrawerString (
				"- How d’ya think?", textFont, 80, textLetterWidth));
			extendedStrings1[extendedStrings1.Count - 1].Add (new LogoDrawerString (
				"- Oh, from arabic it means “love”. May be...", textFont, 30, textLetterWidth));
			extendedStrings1[extendedStrings1.Count - 1].Add (new LogoDrawerString (
				"- Hell no! Oh God!  Have you ever seen Half-Life?" +
				"  What kind of love do you assumed to see here?", textFont, 80, textLetterWidth));
			extendedStrings1[extendedStrings1.Count - 1].Add (new LogoDrawerString (
				"- Some  love  to machines,  I think.  Technocracy" +
				"  madness or something...", textFont, 80, textLetterWidth));
			extendedStrings1[extendedStrings1.Count - 1].Add (new LogoDrawerString (
				"- ...I’m shocked.  It’s actually an accident. But" +
				"  you’re completely right", textFont, 50, textLetterWidth));
			extendedStrings1[extendedStrings1.Count - 1].Add (new LogoDrawerString (
				"- OK, keep going", textFont, 100, textLetterWidth));

			extendedStrings1.Add (new List<LogoDrawerString> ());
			extendedStrings1[extendedStrings1.Count - 1].Add (new LogoDrawerString (
				"ESHQ:", headerFont, 20, headerLetterWidth));
			extendedStrings1[extendedStrings1.Count - 1].Add (new LogoDrawerString (
				"• Evil", headerFont, 20, headerLetterWidth));
			extendedStrings1[extendedStrings1.Count - 1].Add (new LogoDrawerString (
				"• Scientists", headerFont, 20, headerLetterWidth));
			extendedStrings1[extendedStrings1.Count - 1].Add (new LogoDrawerString (
				"• Head", headerFont, 20, headerLetterWidth));
			extendedStrings1[extendedStrings1.Count - 1].Add (new LogoDrawerString (
				"• Quarters", headerFont, 40, headerLetterWidth));
			extendedStrings1[extendedStrings1.Count - 1].Add (new LogoDrawerString (" ", textFont, 0, textLetterWidth));
			extendedStrings1[extendedStrings1.Count - 1].Add (new LogoDrawerString (
				"- It’s a  fake name  for infrastructure  facility" +
				"  where  our plot  begins.  Our project  is about" +
				"  tech.  And about  people’s inability  to use it" +
				"  properly.  We just  tried  to create  something" +
				"  that we haven’t in reality. We got it, I think." +
				"  Surrealistic disasters, crushes, mad AI, etc...", textFont, 50, textLetterWidth));
			extendedStrings1[extendedStrings1.Count - 1].Add (new LogoDrawerString (" ", textFont, 0, textLetterWidth));
			extendedStrings1[extendedStrings1.Count - 1].Add (new LogoDrawerString (
				"  But... Hell yeah! We’re sharing “love”! ♥♥♥", textFont, 100, textLetterWidth));

			extendedStrings1.Add (new List<LogoDrawerString> ());
			extendedStrings1[extendedStrings1.Count - 1].Add (new LogoDrawerString (
				"- ...OK. And what about this thing?", textFont, 20, textLetterWidth));

			extendedStrings1.Add (new List<LogoDrawerString> ());
			extendedStrings1[extendedStrings1.Count - 1].Add (new LogoDrawerString (
				"- Offers?", textFont, 80, textLetterWidth));
			extendedStrings1[extendedStrings1.Count - 1].Add (new LogoDrawerString (" ", textFont, 0, textLetterWidth));
			extendedStrings1[extendedStrings1.Count - 1].Add (new LogoDrawerString (
				"- An infinity sign, I think", textFont, 50, textLetterWidth));
			extendedStrings1[extendedStrings1.Count - 1].Add (new LogoDrawerString (
				"- Yes, it is.  But it’s  “interrupted”  infinity." +
				"  Like a life.  It may  be eternal.  But it tends" +
				"  to break off suddenly... What else?", textFont, 80, textLetterWidth));
			extendedStrings1[extendedStrings1.Count - 1].Add (new LogoDrawerString (" ", textFont, 0, textLetterWidth));
			extendedStrings1[extendedStrings1.Count - 1].Add (new LogoDrawerString (
				"- DNA, may be?", textFont, 50, textLetterWidth));
			extendedStrings1[extendedStrings1.Count - 1].Add (new LogoDrawerString (
				"- Yes,  of course.  The  code  of life.  And  the" +
				"  source code  that we using  just like a painter" +
				"  uses his brush. It’s context dependent", textFont, 80, textLetterWidth));
			extendedStrings1[extendedStrings1.Count - 1].Add (new LogoDrawerString (" ", textFont, 0, textLetterWidth));
			extendedStrings1[extendedStrings1.Count - 1].Add (new LogoDrawerString (
				"- Is Large Hadron Collider also here?  Logo looks" +
				"  like its rays dispersing from center", textFont, 50, textLetterWidth));
			extendedStrings1[extendedStrings1.Count - 1].Add (new LogoDrawerString (
				"- It’s  obvious.  LHC is  about life  origin too." +
				"  And about tech too", textFont, 100, textLetterWidth));

			extendedStrings1.Add (new List<LogoDrawerString> ());
			extendedStrings1[extendedStrings1.Count - 1].Add (new LogoDrawerString (
				"- And how about crossing roads?", textFont, 50, textLetterWidth));
			extendedStrings1[extendedStrings1.Count - 1].Add (new LogoDrawerString (
				"- Of course, it is.  An intersection of  ways and" +
				"  fates is the life itself. It’s ESHQ itself", textFont, 80, textLetterWidth));
			extendedStrings1[extendedStrings1.Count - 1].Add (new LogoDrawerString (" ", textFont, 0, textLetterWidth));
			extendedStrings1[extendedStrings1.Count - 1].Add (new LogoDrawerString (
				"- Did you invented new Ankh?", textFont, 50, textLetterWidth));
			extendedStrings1[extendedStrings1.Count - 1].Add (new LogoDrawerString (
				"- No no no.  Don’t be  so pretentious.  It’s just" +
				"  possible descriptions.  And we just need unique" +
				"  logo. Here it is", textFont, 80, textLetterWidth));
			extendedStrings1[extendedStrings1.Count - 1].Add (new LogoDrawerString (" ", textFont, 0, textLetterWidth));
			extendedStrings1[extendedStrings1.Count - 1].Add (new LogoDrawerString (
				"- Oh, well done!", textFont, 120, textLetterWidth));

			#endregion

			// Сброс настроек
			phase1 = phase2 = 1;

			// Запуск таймера
			DrawingTimer.Interval = MovingTimer.Interval = 1;
			PauseTimer.Interval = 2500;
			ExtendedTimer.Interval = 20;

			DrawingTimer.Enabled = true;
			}

		// Таймеры отрисовки

		// Основное лого, вариант 1
		private void DrawingTimer_Mode1 (object sender, EventArgs e)
			{
			// Определение следующей позиции
			switch (phase1)
				{
				// Пауза в центре
				case 1:
					arc1 += 1.0;

					if (arc1 >= 50.0)
						{
						arc1 = 0.0;
						foreBrush = new SolidBrush (this.ForeColor);
						phase1++;
						}

					break;

				// Во всех направлениях от центра
				case 2:
					arc1 += 1.3;

					point1.X = this.Width / 6 + 707 * logoScale / 2000 + (int)(LogoDrawerSupport.Cosinus (135.0 - arc1) *
						logoScale / 2.0);        // Нижняя правая
					point1.Y = this.Height / 2 - 707 * logoScale / 2000 + (int)(LogoDrawerSupport.Sinus (135.0 - arc1) *
						logoScale / 2.0);
					point2.X = this.Width / 6 + 707 * logoScale / 2000 + (int)(LogoDrawerSupport.Cosinus (135.0 + arc1 * 2.0) *
						logoScale / 2.0);       // Верхняя правая
					point2.Y = this.Height / 2 - 707 * logoScale / 2000 + (int)(LogoDrawerSupport.Sinus (135.0 + arc1 * 2.0) *
						logoScale / 2.0);

					point3.X = this.Width / 6 - 707 * logoScale / 2000 + (int)(LogoDrawerSupport.Cosinus (315.0 - arc1) *
						logoScale / 2.0);       // Верхняя левая
					point3.Y = this.Height / 2 + 707 * logoScale / 2000 + (int)(LogoDrawerSupport.Sinus (315.0 - arc1) *
						logoScale / 2.0);
					point4.X = this.Width / 6 - 707 * logoScale / 2000 + (int)(LogoDrawerSupport.Cosinus (315.0 + arc1 * 2.0) *
						logoScale / 2.0);       // Нижняя левая
					point4.Y = this.Height / 2 + 707 * logoScale / 2000 + (int)(LogoDrawerSupport.Sinus (315.0 + arc1 * 2.0) *
						logoScale / 2.0);

					if (arc1 >= 90.0)
						{
						arc1 = 0.0;
						phase1++;
						}
					break;

				// К краям экрана
				case 3:
					int delta = (int)(2.3 - arc1 / 50.0);
					point1.X += delta;
					point1.Y -= delta;
					point2.X += delta;
					point2.Y += delta;
					point3.X -= delta;
					point3.Y += delta;
					point4.X -= delta;
					point4.Y -= delta;

					arc1 += 1.0;
					if (arc1 >= 2 * logoScale / 5)
						phase1++;
					break;

				case 4:
					arc1 = 0.0;
					DrawingTimer.Enabled = false;
					MovingTimer.Enabled = true;
					break;
				}

			// Отрисовка
			g.FillEllipse (foreBrush, point1.X - drawerSize / 2, point1.Y - drawerSize / 2, drawerSize, drawerSize);
			g.FillEllipse (foreBrush, point2.X - drawerSize / 2, point2.Y - drawerSize / 2, drawerSize, drawerSize);
			g.FillEllipse (foreBrush, point3.X - drawerSize / 2, point3.Y - drawerSize / 2, drawerSize, drawerSize);
			g.FillEllipse (foreBrush, point4.X - drawerSize / 2, point4.Y - drawerSize / 2, drawerSize, drawerSize);

			// Вместо CheckTransition
#if LDDEXPORT
			logo4b.Save ("C:\\1\\" + (moves++).ToString ("D8") + ".png", ImageFormat.Png);
#endif
			}

		// Основное лого, вариант 2
		private void DrawingTimer_Mode2 (object sender, EventArgs e)
			{
			/*#if LDDEXPORT
			foreBrush.Color = Color.FromArgb (160, rnd.Next (64, 256), rnd.Next (64, 256), rnd.Next (64, 256));
			#endif*/

			// Определение следующей позиции
			switch (phase1)
				{
				// Пауза в центре
				case 1:
					arc1 += 1.0;

					if (arc1 >= 70.0)
						{
						arc1 = 0.0;
						foreBrush = new SolidBrush (this.ForeColor);
						phase1++;
						}

					break;

				// Во всех направлениях от центра
				case 2:
					arc1 += 1.0;

					point1.X = (this.Width + logoScale) / 2 + (int)(LogoDrawerSupport.Cosinus (180.0 - arc1) *
						logoScale / 2.0);        // Нижняя правая
					point1.Y = this.Height / 2 + (int)(LogoDrawerSupport.Sinus (180.0 - arc1) * logoScale / 2.0);
					point2.X = (this.Width + logoScale) / 2 + (int)(LogoDrawerSupport.Cosinus (180.0 + arc1 * 2.0) *
						logoScale / 2.0);  // Верхняя правая
					point2.Y = this.Height / 2 + (int)(LogoDrawerSupport.Sinus (180.0 + arc1 * 2.0) * logoScale / 2.0);

					point3.X = (this.Width - logoScale) / 2 + (int)(LogoDrawerSupport.Cosinus (-arc1) *
						logoScale / 2.0);               // Верхняя левая
					point3.Y = this.Height / 2 + (int)(LogoDrawerSupport.Sinus (-arc1) * logoScale / 2.0);
					point4.X = (this.Width - logoScale) / 2 + (int)(LogoDrawerSupport.Cosinus (arc1 * 2.0) *
						logoScale / 2.0);          // Нижняя левая
					point4.Y = this.Height / 2 + (int)(LogoDrawerSupport.Sinus (arc1 * 2.0) * logoScale / 2.0);

					if (arc1 >= 90.0)
						{
						phase1++;
						}
					break;

				// К краям экрана
				case 3:
					point1.X += frameSpeed;
					point2.Y += frameSpeed;
					point3.X -= frameSpeed;
					point4.Y -= frameSpeed;
					break;
				}

			// Отрисовка
			if (phase1 == 3)
				{
				g.FillRectangle (foreBrush, point1.X - drawerSize / 2, point1.Y - drawerSize / 2, drawerSize, drawerSize);
				g.FillRectangle (foreBrush, point2.X - drawerSize / 2, point2.Y - drawerSize / 2, drawerSize, drawerSize);
				g.FillRectangle (foreBrush, point3.X - drawerSize / 2, point3.Y - drawerSize / 2, drawerSize, drawerSize);
				g.FillRectangle (foreBrush, point4.X - drawerSize / 2, point4.Y - drawerSize / 2, drawerSize, drawerSize);
				}
			else
				{
				g.FillEllipse (foreBrush, point1.X - drawerSize / 2, point1.Y - drawerSize / 2, drawerSize, drawerSize);
				g.FillEllipse (foreBrush, point2.X - drawerSize / 2, point2.Y - drawerSize / 2, drawerSize, drawerSize);
				g.FillEllipse (foreBrush, point3.X - drawerSize / 2, point3.Y - drawerSize / 2, drawerSize, drawerSize);
				g.FillEllipse (foreBrush, point4.X - drawerSize / 2, point4.Y - drawerSize / 2, drawerSize, drawerSize);
				}

			// Отрисовка и финальный контроль
			CheckTransition ();
			}

		// Завершение отрисовки основного лого, фиксация изображения
		private void CheckTransition ()
			{
			// Остановка таймера по завершению, запуск следующей фазы
			if ((point1.X > this.Width + logoScale / 10) && (point2.Y > this.Height + logoScale / 10) ||
				(point3.X < -logoScale / 10) && (point4.Y < -logoScale / 10))
				{
				// Остановка
				DrawingTimer.Enabled = false;

				// "Фото" экрана
#if !LDDEXPORT
				Bitmap logo1tmp = new Bitmap (this.Width, this.Height);
				g2 = Graphics.FromImage (logo1tmp);
				g2.CopyFromScreen (0, 0, 0, 0, new Size (this.Width, this.Height), CopyPixelOperation.SourceCopy);
				g2.Dispose ();
#else
				Bitmap logo1tmp = new Bitmap (logo4b);
#endif

				// "Фото" лого
				logo1 = logo1tmp.Clone (new Rectangle (this.Width / 2 - (logoScale + tailsSize),
					this.Height / 2 - (int)(logoScale / 2 + tailsSize),
					(logoScale + tailsSize) * 2, logoScale + tailsSize * 2), PixelFormat.Format32bppArgb);
				logo1tmp.Dispose ();

				// В рамочке
				g2 = Graphics.FromImage (logo1);
				g2.DrawRectangle (backPen, 0, 0, logo1.Width, logo1.Height);
				g2.Dispose ();

				// Подготовка следующего таймера
				arc1 = 180.0;

				// Продолжение
				MovingTimer.Enabled = true;
				}

#if LDDEXPORT
			logo4b.Save ("C:\\1\\" + (moves++).ToString ("D8") + ".png", ImageFormat.Png);
#endif
			}

		// Смещение лого и отрисовка подписи
		private void MovingTimer_Tick (object sender, EventArgs e)
			{
			arc1 -= 2.0;

			if ((arc1 >= 0.0) && (mode == DrawModes.Mode2))
				{
				// Расходящаяся от центра рамка, стирающая лишние линии
				g.DrawRectangle (backPen,
					(int)((this.Width / 2 - (logoScale + tailsSize)) *
					((1 + LogoDrawerSupport.Cosinus (arc1 + 180.0)) / 2.0)),

					(int)((this.Height / 2 - (int)(logoScale / 2 + tailsSize)) *
					((1 + LogoDrawerSupport.Cosinus (arc1 + 180.0)) / 2.0)),

					(int)(logo1.Width + (this.Width - logo1.Width) * ((1 + LogoDrawerSupport.Cosinus (arc1)) / 2.0)),

					(int)(logo1.Height + (this.Height - logo1.Height) * ((1 + LogoDrawerSupport.Cosinus (arc1)) / 2.0)));

				// Смещающееся влево лого
				g.DrawImage (logo1,

					(this.Width - logo1.Width) / 2 - (int)(((1 + LogoDrawerSupport.Cosinus (arc1)) / 2.0) *
					(9 * this.Width / 10 - logo1.Width) / 2),

					(int)((this.Height - (logoScale + 2 * tailsSize)) / 2));
				}
			else if (arc1 >= -90.0)
				{
				/*#if LDDEXPORT
				foreBrush.Color = Color.FromArgb (0, -3 * (int)arc1 / 2, -3 * (int)arc1 / 4);
				#endif*/

				// Отображение текста
				g.DrawString (logoString1.Substring (0, (int)(logoString1.Length * LogoDrawerSupport.Sinus (-arc1))),
					logo1Font, foreBrush, 94 * this.Width / 100 - logo1Size.Width, this.Height / 2);
				g.DrawString (logoString2.Substring (0, (int)(logoString2.Length * LogoDrawerSupport.Sinus (-arc1))),
					logo2Font, foreBrush, 93 * this.Width / 100 - logo2Size.Width, this.Height / 2 -
					logo1Size.Height * 0.4f);
				}
			else
				{
				if (mode == DrawModes.Mode1)
					{
					Bitmap logo1tmp = new Bitmap (this.Width, this.Height);
					g2 = Graphics.FromImage (logo1tmp);
					g2.CopyFromScreen (0, 0, 0, 0, new Size (this.Width, this.Height), CopyPixelOperation.SourceCopy);
					g2.Dispose ();

					logo1 = logo1tmp.Clone (new Rectangle (this.Width / 6 - 7 * logoScale / 5,
						this.Height / 2 - 7 * logoScale / 5,
						14 * logoScale / 5, 14 * logoScale / 5), PixelFormat.Format32bppArgb);
					logo1tmp.Dispose ();
					}

				MovingTimer.Enabled = false;
				PauseTimer.Enabled = true;
				}

#if LDDEXPORT
			logo4b.Save ("C:\\1\\" + (moves++).ToString ("D8") + ".png", ImageFormat.Png);
#endif
			}

		// Таймер задержки лого на экране
		private void PauseTimer_Tick (object sender, EventArgs e)
			{
			// Остановка основного режима
			PauseTimer.Enabled = false;

#if LDDEXPORT
			for (int i = 0; i < 120; i++)
				logo4b.Save ("C:\\1\\" + (moves++).ToString ("D8") + ".png", ImageFormat.Png);
#endif

			if (extended == 0)
				{
				// Выход
				this.Close ();
				return;
				}

			// Инициализация расширенного режима
			phase1 = 1;
			steps = 0;

			// Запуск
			ExtendedTimer.Enabled = true;
			}

		// Таймер расширенного режима отображения, вариант 1
		private void ExtendedTimer1_Tick (object sender, EventArgs e)
			{
			switch (phase1)
				{
				// Начальное затенение
				case 1:
					HideScreen (false);
					break;

				// Отрисовка начальных элементов
				case 2:
					DrawMarker (1);
					break;

				case 3:
				case 14:
					DrawMarker (2);
					break;

				case 4:
				case 15:
					if (steps++ == 0)
						{
						g.DrawImage (new Bitmap (logo1, new Size ((int)((double)logo1.Width / ((double)logo1.Height / 24)),
							(int)((double)logo1.Height / ((double)logo1.Height / 24)))), 110, lineFeed * 2);
						}
					else if (steps > 20)
						{
						steps = 0;
						phase1++;
						}
					break;

				case 5:
					if (steps++ == 0)
						g.DrawString ("ESHQ", headerFont, foreBrush, 80, lineFeed);
					else if (steps > 20)
						{
						steps = 0;
						point1.X = lineLeft;
						point1.Y = lineFeed;
						phase1++;
						}
					break;

				case 6:
					DrawSplitter ();
					break;

				// Отрисовка текста
				case 7:
				case 9:
				case 11:
				case 13:
				case 17:
				case 19:
					DrawText (extendedStrings1);
					break;

				// Интермиссии
				case 8:
					HideMarker (2);
					break;

				case 16:
					HideMarker (1);
					break;

				// Обновление экрана
				case 10:
				case 12:
				case 18:
					point1.Y = lineFeed;
					phase1 = 100;
					break;

				case 100:
					HideText ();
					break;

				// Завершение
				case 20:
					HideScreen (true);
					break;

				case 21:
					ExtendedTimer.Enabled = false;
					extended = 0;
					mode = DrawModes.Mode2;
					DrawingTimer.Tick -= DrawingTimer_Mode1;
					LogoDrawer_Shown (null, null);
					break;
				}

#if LDDEXPORT
			logo4b.Save ("C:\\1\\" + (moves++).ToString ("D8") + ".png", ImageFormat.Png);
#endif
			}

		// Обслуживающий функционал

		// Закрытие окна
		private void LogoDrawer_FormClosing (object sender, FormClosingEventArgs e)
			{
			// Остановка всех отрисовок
			DrawingTimer.Enabled = MovingTimer.Enabled = PauseTimer.Enabled = ExtendedTimer.Enabled = false;

			// Сброс всех ресурсов
			foreBrush.Dispose ();
			backBrush.Dispose ();
			backHidingBrush1.Dispose ();
			backHidingBrush2.Dispose ();
			backPen.Dispose ();
			logo1Font.Dispose ();
			logo2Font.Dispose ();
			headerFont.Dispose ();
			textFont.Dispose ();
			g.Dispose ();

			/*if (extended == 2)
				{
				logo2Green.Dispose ();
				logo2Grey.Dispose ();
				logo2BackPart.Dispose ();
				logo2GreenPart.Dispose ();
				logo2GreyPart.Dispose ();
				}
#if LDDEXPORT
			else if (extended == 4)
				{
				if (logo4b != null)
					logo4b.Dispose ();
				}
#endif*/

			if (logo1 != null)
				logo1.Dispose ();
			/*if (logo2a != null)
				logo2a.Dispose ();
			if (logo2b != null)
				logo2b.Dispose ();*/
			}

		// Принудительный выход (по любой клавише)
		private void LogoDrawer_KeyDown (object sender, KeyEventArgs e)
			{
			this.Close ();
			}

		private void LogoDrawer_MouseMove (object sender, MouseEventArgs e)
			{
#if !LDDEBUG
			moves++;

			if (moves > 2)
				this.Close ();
#endif
			}

		// Метод рисует числовой маркер
		private void DrawMarker (uint Number)
			{
			if (steps++ == 0)
				g.DrawString (Number.ToString () + ".", headerFont, foreBrush, 30, lineFeed * Number);
			else if (steps > 20)
				{
				steps = 0;
				phase1++;
				}
			}

		// Метод рисует вертикальный разделитель
		private void DrawSplitter ()
			{
			steps++;
			g.FillRectangle (foreBrush, 220, this.Height * 0.05f, drawerSize / 2, this.Height * 0.9f *
				(float)LogoDrawerSupport.Sinus (steps * 4));

			if (steps > 45)
				{
				steps = 0;
				phase1++;
				}
			}

		// Метод отрисовывает текст
		private void DrawText (List<List<LogoDrawerString>> StringsSet)
			{
			// Последняя строка кончилась
			if (StringsSet[0].Count == 0)
				{
				StringsSet.RemoveAt (0);
				phase1++;
				phase2 = phase1 + 1;
				return;
				}

			// Движение по строке
			if (steps < StringsSet[0][0].StringLength)
				{
				// Одна буква
				g.DrawString (StringsSet[0][0].StringText.Substring ((int)steps++, 1), StringsSet[0][0].StringFont,
					foreBrush, point1);

				// Смещение "каретки"
				point1.X += (int)StringsSet[0][0].LetterSize;

				// Конец строки, перевод "каретки"
				if (point1.X > this.Width * 0.79)
					{
					point1.X = lineLeft;
					point1.Y += lineFeed;

					// Обработка смены экрана
					if (point1.Y > this.Height * 0.95)
						{
						point1.Y = lineFeed;
						phase2 = phase1;
						phase1 = 100;
						}
					}
				}

			// Кончился текст строки и задержка отображения
			else if (steps > StringsSet[0][0].StringLength + StringsSet[0][0].Pause)
				{
				// Переход к следующей текстовой строке
				StringsSet[0].RemoveAt (0);
				steps = 0;
				point1.X = lineLeft;
				point1.Y += lineFeed;

				// Обработка смены экрана
				if (point1.Y > this.Height * 0.95)
					{
					point1.Y = lineFeed;
					phase2 = phase1;
					phase1 = 100;
					}
				}

			// Кончился только текст строки, пауза
			else
				{
				steps++;
				}
			}

		// Метод затеняет указанный маркер
		private void HideMarker (uint Number)
			{
			steps++;
			g.FillRectangle (backHidingBrush1, 0, Number * lineFeed, 220, lineFeed);

			if (steps > 35)
				{
				steps = 0;
				phase1++;   // К отрисовке текста
				}
			}

		// Метод затеняет текстовое поле
		private void HideText ()
			{
			steps++;
			g.FillRectangle (backHidingBrush1, lineLeft, 0, this.Width - lineLeft, this.Height);

			if (steps > 80)
				{
				steps = 0;
				phase1 = phase2;    // К отрисовке текста
				}
			}

		// Метод затеняет экран
		private void HideScreen (bool Full)
			{
			steps++;
			g.FillRectangle (Full ? backHidingBrush2 : backHidingBrush1, 0, 0, this.Width, this.Height);

			if (steps > 65)
				{
				steps = 0;
				phase1++;
				}
			}
		}
	}
