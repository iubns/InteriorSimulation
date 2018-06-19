﻿using Newtonsoft.Json;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace selfInteriorSimulation
{
    

    class InteriorObject : BaseObject
    {
        public Point Center
        {
            get { return new Point(Margin.Left + Width / 2, Margin.Top + Height / 2); }
            set { Margin = new Thickness(value.X - Width / 2, value.Y - Height / 2, 0, 0); }
        }
        public Image Image
        {
            get { return border.Child as Image; }
            set { border.Child = value; }
        }
        public double Rotate
        {
            get
            {
                var rotate = this.Image.RenderTransform as RotateTransform;
                return rotate.Angle;
            }
            set
            {
                const double magic_num = 3;
                this.Image.RenderTransform = new RotateTransform(value, Width / 2 - magic_num, Height / 2 - magic_num);
            }
        }
        

        public InteriorObject()
        {
            this.Image = new Image() { Stretch = System.Windows.Media.Stretch.Fill };

            this.MouseDown += Mouse_Down;
            this.MouseMove += Mouse_Move;
            this.MouseUp += Mouse_Up;

            Canvas.SetZIndex(this, 3);
        }

        public void Mouse_Move(object sender, MouseEventArgs e)
        {
            if (this.IsMouseCaptured)
            {
                Point clickPoint = e.GetPosition(canvas);
                clickPoint = Algorithm.Adjust_to_fit_std(clickPoint);

                this.Center = new Point(clickPoint.X, clickPoint.Y);
            }
        }

        public void Mouse_Down(object sender, MouseButtonEventArgs e)
        {
            active_notify(this); this.CaptureMouse();
        }
        
        public void Mouse_Up(object sender, MouseButtonEventArgs e)
        {
            if (this.IsMouseCaptured)
            {
                foreach (var each in gRooms)
                {
                    if (Algorithm.Is_collesion(each, this) == true) return;
                }
                change_notify("Moved", this.Name);
                this.ReleaseMouseCapture();
            }
        }


        public override object Clone()
        {
            var copy = Activator.CreateInstance(this.GetType()) as InteriorObject;
            
            copy.Name = this.Name;
            copy.Width = this.Width;
            copy.Height = this.Height;
            copy.Center = this.Center;
            copy.Image = new Image() { Source = this.Image.Source.Clone(), Stretch = this.Image.Stretch };
            copy.Rotate = 0;

            return copy;
        }




        static public InteriorObject Temporary(InteriorObject interiorObject)
        {
            interiorObject.MouseDown -= interiorObject.Mouse_Down;
            interiorObject.MouseMove -= interiorObject.Mouse_Move;
            interiorObject.MouseUp -= interiorObject.Mouse_Up;
            return interiorObject;
        }
    }
}
