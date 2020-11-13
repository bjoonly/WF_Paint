using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Security.Cryptography.X509Certificates;
using System.Windows.Forms;
namespace Paint
{

    public partial class Form1 : Form
    {
        private List<Shape> shapes = new List<Shape>();
        private List<Bitmap> operatios = new List<Bitmap>();
        private Shape currentFigure;
        private Graphics graphics;
        private bool isPencil = false;
        private bool isEraser = false;
        private Point mousePoint;
        private Pen Pen;
        private SolidBrush SBrush;
        private bool isDrawing = false;
        private int selectedFigure = 0;
        private bool isHasOutline = true;
        private Image backImage;
        private Bitmap bp;
        private Bitmap curImage;
        private Point mousePos;
        private bool isFillFigure = true;
        public Form1()
        {
            InitializeComponent();
            graphics = pictureBox1.CreateGraphics();

            bp = new Bitmap(pictureBox1.Width, pictureBox1.Height);
            operatios.Add(bp);
            pictureBox1.Image = bp;
            currentFigure = new StraightLineInfo();
            fillColorDialog.Color = fillColorDialog.Color;
            fillColorToolStripButton1.BackColor = fillColorDialog.Color;

            outlineColorDialog1.Color = Color.White;

            SBrush = new SolidBrush(Color.Black);
            currentFigure.Pen = new Pen(fillColorDialog.Color, 1);
            Pen = new Pen(outlineColorDialog1.Color, 1);
            Pen.DashStyle = DashStyle.Solid;

            outlineColorToolStripButton1.BackColor = outlineColorDialog1.Color;




            figuresToolStripSplitButton1.Image = figuresToolStripSplitButton1.DropDownItems[0].Image;
            penStyleToolStripSplitButton1.Image = penStyleToolStripSplitButton1.DropDown.Items[0].Image;
            backgroundColorToolStripButton1.BackColor = backgroundColorDialog.Color;
        }
        private void outlineColorToolStripButton1_Click(object sender, EventArgs e)
        {
            if (outlineColorDialog1.ShowDialog() == DialogResult.OK)
            {
                Pen.Color = outlineColorDialog1.Color;
                outlineColorToolStripButton1.BackColor = Color.FromArgb(254, outlineColorDialog1.Color);
            }
        }
        private void colorToolStripButton1_Click(object sender, EventArgs e)
        {
            if (fillColorDialog.ShowDialog() == DialogResult.OK)
            {


                SBrush.Color = fillColorDialog.Color;
                fillColorToolStripButton1.BackColor = Color.FromArgb(254, fillColorDialog.Color);


            }
        }

        private void PrintAll(Graphics graphics)
        {

            foreach (var s in shapes)
            {
                s.Print(graphics);
            }

        }
        private void Form1_MouseUp(object sender, MouseEventArgs e)
        {

            isDrawing = false;


            shapes.Add(currentFigure);


            bp = new Bitmap(pictureBox1.Image);
            graphics = Graphics.FromImage(bp);
            currentFigure.Print(graphics);

            operatios.Add(bp);
            pictureBox1.Image = bp;
            mousePoint.X = -1;
            mousePoint.Y = -1;
        }

        private void Form1_MouseMove(object sender, MouseEventArgs e)
        {
            mousePos = e.Location;

            if (isDrawing)
            {
                if (isPencil || isEraser)
                {
                    ((LineInfo)currentFigure).Points.Add(e.Location);
                    if (((LineInfo)currentFigure).Points.Count > 1)
                        currentFigure.Print(graphics);

                }

                else if (selectedFigure == 0)
                {


                    currentFigure.X = mousePoint.X;
                    ((StraightLineInfo)currentFigure).EndX = e.X;

                    currentFigure.Y = mousePoint.Y;
                    ((StraightLineInfo)currentFigure).EndY = e.Y;
                }

                else if (selectedFigure == 1)
                {

                    if (e.X > mousePoint.X)
                    {
                        ((RectangleInfo)currentFigure).Width = e.X - mousePoint.X;
                        currentFigure.X = mousePoint.X;
                    }
                    else
                    {
                        ((RectangleInfo)currentFigure).Width = mousePoint.X - e.X;
                        currentFigure.X = e.X;
                    }

                    if (e.Y > mousePoint.Y)
                    {
                        ((RectangleInfo)currentFigure).Height = e.Y - mousePoint.Y;
                        currentFigure.Y = mousePoint.Y;
                    }
                    else
                    {
                        ((RectangleInfo)currentFigure).Height = mousePoint.Y - e.Y;
                        currentFigure.Y = e.Y;
                    }

                }
                else
                {
                    if (e.X > mousePoint.X)
                    {
                        ((EllipseInfo)currentFigure).Width = e.X - mousePoint.X;
                        currentFigure.X = mousePoint.X;
                    }
                    else
                    {
                        ((EllipseInfo)currentFigure).Width = mousePoint.X - e.X;
                        currentFigure.X = e.X;
                    }

                    if (e.Y > mousePoint.Y)
                    {
                        ((EllipseInfo)currentFigure).Height = e.Y - mousePoint.Y;
                        currentFigure.Y = mousePoint.Y;
                    }
                    else
                    {
                        ((EllipseInfo)currentFigure).Height = mousePoint.Y - e.Y;
                        currentFigure.Y = e.Y;
                    }
                }
            }
        }

        private void Form1_MouseDown(object sender, MouseEventArgs e)
        {

            isDrawing = true;
            mousePoint = e.Location;

            if (pictureBox1.Image != null)
            {
                graphics = pictureBox1.CreateGraphics();

            }

            if (isPencil || isEraser)
            {
                currentFigure = new LineInfo();

            }

            else if (selectedFigure == 0)
            {
                currentFigure = new StraightLineInfo();
            }
            else if (selectedFigure == 1)
            {

                currentFigure = new RectangleInfo();
                ((RectangleInfo)currentFigure).Brush = SBrush;

                ((RectangleInfo)currentFigure).IsHasOutline = isHasOutline;
                ((RectangleInfo)currentFigure).IsFill = isFillFigure;
            }
            else if (selectedFigure == 2)
            {
                currentFigure = new EllipseInfo();
                ((EllipseInfo)currentFigure).Brush = SBrush;

                ((EllipseInfo)currentFigure).IsHasOutline = isHasOutline;
                ((EllipseInfo)currentFigure).IsFill = isFillFigure;
            }

            currentFigure.Pen = Pen;
            currentFigure.Pen.Color = outlineColorDialog1.Color;
            currentFigure.DashStyle = Pen.DashStyle;


            if (isEraser)
            {
                currentFigure.Pen.Color = fillColorDialog.Color;
                currentFigure.DashStyle = DashStyle.Solid;
            }
        }



        private void saveToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            saveFileDialog1.Filter = "Image files (*.jpg, *.jpeg, *.jpe, *.jfif, *.png, *.bmp, *.gif) | *.jpg; *.jpeg; *.jpe; *.jfif; *.png";
            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                string fileName = saveFileDialog1.FileName;
                string strFilExtn = fileName.Remove(0, fileName.Length - 3);

                if (pictureBox1.Image != null)
                {
                    bp = new Bitmap(bp);
                    graphics = Graphics.FromImage(bp);
                    pictureBox1.Image = bp;
                }
                else
                {
                    bp = new Bitmap(pictureBox1.Width, pictureBox1.Height);
                    graphics = Graphics.FromImage(bp);
                    if (pictureBox1.Image == null)
                    {
                        graphics.FillRectangle(new SolidBrush(pictureBox1.BackColor), new RectangleF(new Point(0, 0), new Size(Width, Height)));
                    }

                }
                PrintAll(graphics);
                switch (strFilExtn)
                {
                    case "bmp":
                        bp.Save(fileName, ImageFormat.Bmp);
                        break;
                    case "jpg":
                        bp.Save(fileName, ImageFormat.Jpeg);
                        break;
                    case "gif":
                        bp.Save(fileName, ImageFormat.Gif);
                        break;
                    case "png":
                        bp.Save(fileName, ImageFormat.Png);
                        break;
                    default:
                        break;
                }
            }



        }

        private void pencilToolStripButton1_Click(object sender, EventArgs e)
        {

            isPencil = true;
            isEraser = false;

            eraserToolStripButton1.Checked = false;
        }



        private void figuresToolStripComboBox1_Click(object sender, EventArgs e)
        {
            isPencil = false;
            isEraser = false;
            pencilToolStripButton1.Checked = false;
            eraserToolStripButton1.Checked = false;

        }

        private void eraserToolStripButton1_Click(object sender, EventArgs e)
        {

            isPencil = false;
            isEraser = true;
            pencilToolStripButton1.Checked = false;

        }

        private void widthToolStripMenuItem2_Click(object sender, EventArgs e)
        {
            Pen.Width = Convert.ToInt32(((ToolStripMenuItem)sender).Tag);
            foreach (ToolStripMenuItem item in windthToolStripDropDownButton1.DropDownItems)
            {
                item.CheckState = CheckState.Unchecked;

            }
            ((ToolStripMenuItem)sender).CheckState = CheckState.Checked;
        }

        private void toolStripMenuItem6_Click(object sender, EventArgs e)
        {
            selectedFigure = Convert.ToInt32(((ToolStripDropDownItem)sender).Tag);
            figuresToolStripSplitButton1.Image = figuresToolStripSplitButton1.DropDownItems[selectedFigure].Image;
            foreach (ToolStripMenuItem item in figuresToolStripSplitButton1.DropDownItems)
            {
                item.CheckState = CheckState.Unchecked;
            }
            ((ToolStripMenuItem)sender).CheckState = CheckState.Checked;

        }

        private void backgroundToolStripButton1_Click(object sender, EventArgs e)
        {
            if (backgroundColorDialog.ShowDialog() == DialogResult.OK)
            {

                backgroundColorToolStripButton1.BackColor = Color.FromArgb(254, backgroundColorDialog.Color);

                pictureBox1.BackColor = backgroundColorDialog.Color;




            }
        }

        private void openToolStripMenuItem9_Click(object sender, EventArgs e)
        {
            openFileDialog1.Filter = "Image files (*.jpg, *.jpeg, *.jpe, *.jfif, *.png, *.bmp, *.gif) | *.jpg; *.jpeg; *.jpe; *.jfif; *.png";
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                bp = new Bitmap(Image.FromFile(openFileDialog1.FileName), pictureBox1.Width, pictureBox1.Height);
                backImage = bp;
                pictureBox1.Image = bp;
                graphics = Graphics.FromImage(pictureBox1.Image);
                PrintAll(graphics);
                operatios.Add(bp);
            }
        }

        private void pictureBox1_Paint(object sender, PaintEventArgs e)
        {


            if (pictureBox1.Image == null) graphics.Clear(backgroundColorDialog.Color);
            graphics.SmoothingMode = SmoothingMode.HighQuality;

            graphics = Graphics.FromImage(bp);
            PrintAll(graphics);


        }

        private void toolStripMenuItem11_Click(object sender, EventArgs e)
        {

            int tag = Convert.ToInt32(((ToolStripMenuItem)sender).Tag);

            if (tag == 0)
                Pen.DashStyle = DashStyle.Solid;
            else if (tag == 1)
                Pen.DashStyle = DashStyle.Dash;
            else if (tag == 2)
                Pen.DashStyle = DashStyle.Dot;
            foreach (ToolStripMenuItem item in penStyleToolStripSplitButton1.DropDownItems)
            {
                item.CheckState = CheckState.Unchecked;

            }
            ((ToolStripMenuItem)sender).CheckState = CheckState.Checked;
            penStyleToolStripSplitButton1.Image = penStyleToolStripSplitButton1.DropDown.Items[tag].Image;
        }

        private void toolStripMenuItem13_Click(object sender, EventArgs e)
        {
            int tag = Convert.ToInt32(((ToolStripMenuItem)sender).Tag);
            if (tag == 0)
                isFillFigure = true;
            else
            {
                isFillFigure = false;
                isHasOutline = true;
                toolStripMenuItem14.CheckState = CheckState.Checked;
                toolStripMenuItem15.CheckState = CheckState.Unchecked;
            }

            foreach (ToolStripMenuItem item in fillToolStripSplitButton1.DropDownItems)
            {
                item.CheckState = CheckState.Unchecked;

            }
             ((ToolStripMenuItem)sender).CheckState = CheckState.Checked;
        }

        private void toolStripMenuItem14_Click(object sender, EventArgs e)
        {
            int tag = Convert.ToInt32(((ToolStripMenuItem)sender).Tag);
            if (tag == 0)
                isHasOutline = true;
            else
            {
                isFillFigure = true;
                isHasOutline = false;
                toolStripMenuItem13.CheckState = CheckState.Checked;
                toolStripMenuItem12.CheckState = CheckState.Unchecked;
            }

            foreach (ToolStripMenuItem item in outlineToolStripSplitButton1.DropDownItems)
            {
                item.CheckState = CheckState.Unchecked;

            }
             ((ToolStripMenuItem)sender).CheckState = CheckState.Checked;
        }

        private void clearToolStripButton1_Click(object sender, EventArgs e)
        {
            graphics.Clear(backgroundColorDialog.Color);
            if (pictureBox1.Image != null)
            {
                bp = new Bitmap(pictureBox1.Width, pictureBox1.Height);
                pictureBox1.Image = bp;
            }
            shapes.Clear();

            operatios.Add(bp);
        }

        private void toolStripMenuItem17_Click(object sender, EventArgs e)
        {

            if (curImage != null)
            {
                if (pictureBox1.Image == null)
                {
                    bp = new Bitmap(pictureBox1.Width, pictureBox1.Height);
                    graphics = Graphics.FromImage(bp);
                }

                else
                {
                    bp = new Bitmap(pictureBox1.Image);
                    graphics = Graphics.FromImage(bp);
                }
               
                graphics.DrawImage(curImage, mousePos);

                pictureBox1.Image = bp;
                graphics = pictureBox1.CreateGraphics();
                operatios.Add(bp);
            }


        }

        private void toolStripMenuItem16_Click(object sender, EventArgs e)
        {
            openFileDialog2.Filter = "Image files (*.jpg, *.jpeg, *.jpe, *.jfif, *.png, *.bmp, *.gif) | *.jpg; *.jpeg; *.jpe; *.jfif; *.png";
            if (openFileDialog2.ShowDialog() == DialogResult.OK)
            {
                curImage = new Bitmap(Image.FromFile(openFileDialog2.FileName));
            }
        }

        private void UndotoolStripMenuItem16_Click_1(object sender, EventArgs e)
        {
            if (operatios.Count > 1)
            {
                bp = (operatios[operatios.Count - 2]);
                graphics = Graphics.FromImage(bp);
                graphics.DrawImage(bp, this.Width, this.Height);
                pictureBox1.Image = bp;
                graphics = pictureBox1.CreateGraphics();
                operatios.RemoveAt(operatios.Count - 1);
                if (shapes.Count > 0)
                    shapes.RemoveAt(shapes.Count - 1);
            }
        }
    }
}
