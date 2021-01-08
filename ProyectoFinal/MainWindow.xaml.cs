using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ProyectoFinal
{
    /// <summary>
    /// Lógica de interacción para MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }
        List<Puntos> listaDePuntos = new List<Puntos>();

        bool click_presionado = false;
        private Point puntoClick;
        private Polyline graficaPolilinea = new Polyline();
        private List<Rectangle> listaDeRectangulos = new List<Rectangle>();
        private Polyline ejeX;
        private Polyline ejeY;
        bool rectangulo_activo = false;
        bool graficaBarrasAktivo = false;
        bool graficaPolilineaAktivo = false;




        private void Abrir_las_hojas(object sender, RoutedEventArgs e)
        {
            VentanaHojas vh = new VentanaHojas();
            vh.graficaPoli += Representar_Polilinea1; //representar_Polilinea es el controlador que maneja el evento que ha sucedido, en dicho evento estan los puntos
            vh.graficaBarras += Representar_Barras;
            vh.Owner = this;
            vh.Show();
        }

        private void Representar_Barras(object sender, graficaDeBarrasEventArgs eventoConPuntos)
        {
            if (graficaBarrasAktivo == true)
            {
                for (int i = 0; i < listaDeRectangulos.Count; i++)
                {
                    ElCanvas.Children.Remove(listaDeRectangulos[i]);
                    graficaBarrasAktivo = false;
                }
            }
            if (graficaPolilineaAktivo == true)
            {
                ElCanvas.Children.Remove(graficaPolilinea);
                graficaPolilineaAktivo = false;
            }
            ElCanvas.Children.Remove(ejeY);
            ElCanvas.Children.Remove(ejeX);
            representarLosEjes();

            this.listaDePuntos = eventoConPuntos.ListaPuntosBarras;

            representarGraficaDeBarras(listaDePuntos);
            graficaBarrasAktivo = true;
        }

        private void representarGraficaDeBarras(List<Puntos> listaDePuntos)
        {
            double xDeLista, yDeLista;
            double ancho = ElCanvas.ActualWidth;
            double alto = ElCanvas.ActualHeight;
            double xEnLaGrafica, yEnLaGrafica;
            listaDeRectangulos.Clear();

            for (int i=0;i<listaDePuntos.Count;i++)
            {
                xDeLista = listaDePuntos[i].CoorX;
                yDeLista = listaDePuntos[i].CoorY;

                xEnLaGrafica = xDeLista + (ancho / 2);
                yEnLaGrafica = (alto / 2) - yDeLista;

                double alturaRectangulo = Math.Abs(yDeLista); //tiene que ser absoluto sino da error

                Rectangle rectangulito = new Rectangle()
                {
                    Width = 3,
                    Height = alturaRectangulo,
                    Fill = new SolidColorBrush(Colors.Red),
                    Stroke = new SolidColorBrush(Colors.Red),
                    StrokeThickness = 1
                };

                Canvas.SetLeft(rectangulito, xEnLaGrafica);
                
                if (yDeLista < 0)
                {
                    Canvas.SetTop(rectangulito, alto/2);
                }
                else
                {
                    Canvas.SetTop(rectangulito, (alto / 2) - alturaRectangulo);
                }
                listaDeRectangulos.Add(rectangulito);
                
            }
            for(int j=0;j<listaDeRectangulos.Count;j++)
            {
                ElCanvas.Children.Add(listaDeRectangulos[j]);
            }

            areaSeleccionada.Visibility = Visibility.Collapsed;


        }
        private void representarGraficaDeBarrasConAreaSeleccionada(List<Puntos> listaDePuntos, Point puntoClickUp)
        {
            double xDeLista, yDeLista;
            double ancho = ElCanvas.ActualWidth;
            double alto = ElCanvas.ActualHeight;
            double xEnLaGrafica, yEnLaGrafica;

            for (int i = 0; i < listaDePuntos.Count; i++)
            {
                xDeLista = listaDePuntos[i].CoorX;
                yDeLista = listaDePuntos[i].CoorY;

                xEnLaGrafica = xDeLista + (ancho / 2);
                yEnLaGrafica = (alto / 2) - yDeLista;

                double alturaRectangulo = Math.Abs((yEnLaGrafica - (alto / 2))); //tiene que ser absoluto sino da error

                Rectangle rectangulito = new Rectangle()
                {
                    Width = 3,
                    Height = alturaRectangulo,
                    Fill = new SolidColorBrush(Colors.Red),
                    Stroke = new SolidColorBrush(Colors.Red),
                    StrokeThickness = 1
                };
                if (puntoClick.X < xEnLaGrafica && puntoClickUp.X > xEnLaGrafica && puntoClick.Y < yEnLaGrafica && puntoClickUp.Y > yEnLaGrafica)
                {
                    Canvas.SetLeft(rectangulito, xEnLaGrafica);

                    if (yDeLista < 0)
                    {
                        Canvas.SetTop(rectangulito, alto / 2);
                    }
                    else
                    {
                        Canvas.SetTop(rectangulito, (alto / 2) - alturaRectangulo);
                    }
                    listaDeRectangulos.Add(rectangulito);
                }
                else if (puntoClick.X > xEnLaGrafica && puntoClickUp.X < xEnLaGrafica && puntoClick.Y > yEnLaGrafica && puntoClickUp.Y < yEnLaGrafica)
                {
                    Canvas.SetLeft(rectangulito, xEnLaGrafica);

                    if (yDeLista < 0)
                    {
                        Canvas.SetTop(rectangulito, alto / 2);
                    }
                    else
                    {
                        Canvas.SetTop(rectangulito, (alto / 2) - alturaRectangulo);
                    }
                    listaDeRectangulos.Add(rectangulito);
                }


            }
            for (int j = 0; j < listaDeRectangulos.Count; j++)
            {
                ElCanvas.Children.Add(listaDeRectangulos[j]);
            }
            graficaBarrasAktivo = true;
        }
        private void Representar_Polilinea1(object sender, graficaConPolilineasEventArgs eventoConPuntos)
        {

            if (graficaBarrasAktivo == true)
            {
                for (int i = 0; i < listaDeRectangulos.Count; i++)
                {
                    ElCanvas.Children.Remove(listaDeRectangulos[i]);
                }
                graficaBarrasAktivo = false;
            }
            if (graficaPolilineaAktivo == true)
            {
                ElCanvas.Children.Remove(graficaPolilinea);
                graficaPolilineaAktivo = false;
            }
            ElCanvas.Children.Remove(ejeY);
            ElCanvas.Children.Remove(ejeX);
            representarLosEjes();

            this.listaDePuntos = eventoConPuntos.ListaPuntosPoli;

            representarGrafica(listaDePuntos);

            areaSeleccionada.Visibility = Visibility.Collapsed;

            graficaPolilineaAktivo = true;

        }

        private void representarGrafica(List<Puntos> listaDePuntos)
        {
            double xDeLista, yDeLista;
            double ancho = ElCanvas.ActualWidth;
            double alto = ElCanvas.ActualHeight;
            double xEnLaGrafica, yEnLaGrafica;


            PointCollection puntosPolilinea = new PointCollection();

            graficaPolilinea.StrokeThickness = 2;
            graficaPolilinea.Stroke = Brushes.Red;



            for (int i = 0; i < listaDePuntos.Count; i++)
            {
                xDeLista = listaDePuntos[i].CoorX;
                yDeLista = listaDePuntos[i].CoorY;

                xEnLaGrafica = xDeLista + (ancho / 2);
                yEnLaGrafica = (alto / 2) - yDeLista;

                puntosPolilinea.Add(new Point(xEnLaGrafica, yEnLaGrafica));

            }

            graficaPolilinea.Points = puntosPolilinea;
            ElCanvas.Children.Add(graficaPolilinea);


        }

        private void representarLosEjes()
        {

            double ancho = ElCanvas.ActualWidth;
            double alto = ElCanvas.ActualHeight;


            ejeX = new Polyline();
            ejeX.Stroke = Brushes.Black;
            Point[] puntosX = new Point[2];
            puntosX[0].X = 0;
            puntosX[0].Y = alto / 2;
            puntosX[1].X = ancho;
            puntosX[1].Y = alto / 2;
            ejeX.Points = new PointCollection(puntosX);
            ElCanvas.Children.Add(ejeX);

            ejeY = new Polyline();
            ejeY.Stroke = Brushes.Black;
            Point[] puntosY = new Point[2];
            puntosY[0].X = ancho / 2;
            puntosY[0].Y = 0;
            puntosY[1].X = ancho / 2;
            puntosY[1].Y = alto;
            ejeY.Points = new PointCollection(puntosY);
            ElCanvas.Children.Add(ejeY);


        }
        private void representarGraficaConAreaSeleccionada(List<Puntos> listaDePuntos, Point puntoClickUp)
        {
            double xDeLista, yDeLista;
            double ancho = ElCanvas.ActualWidth;
            double alto = ElCanvas.ActualHeight;
            double xEnLaGrafica, yEnLaGrafica;


            PointCollection puntosPolilinea = new PointCollection();

            graficaPolilinea.StrokeThickness = 2;
            graficaPolilinea.Stroke = Brushes.Red;



            for (int i = 0; i < listaDePuntos.Count; i++)
            {
                xDeLista = listaDePuntos[i].CoorX;
                yDeLista = listaDePuntos[i].CoorY;

                xEnLaGrafica = xDeLista + (ancho / 2);
                yEnLaGrafica = (alto / 2) - yDeLista;

                if (puntoClick.X < xEnLaGrafica && puntoClickUp.X > xEnLaGrafica && puntoClick.Y < yEnLaGrafica && puntoClickUp.Y > yEnLaGrafica)
                {
                    puntosPolilinea.Add(new Point(xEnLaGrafica, yEnLaGrafica));
                }
                else if (puntoClick.X > xEnLaGrafica && puntoClickUp.X < xEnLaGrafica && puntoClick.Y > yEnLaGrafica && puntoClickUp.Y < yEnLaGrafica)
                {
                    puntosPolilinea.Add(new Point(xEnLaGrafica, yEnLaGrafica));
                }


            }

            graficaPolilinea.Points = puntosPolilinea;
            ElCanvas.Children.Add(graficaPolilinea);
        }

        private void Borrar_Grafica(object sender, RoutedEventArgs e)
        {

            if(graficaBarrasAktivo == true)
            {
                for(int i=0;i<listaDeRectangulos.Count;i++)
                {
                    ElCanvas.Children.Remove(listaDeRectangulos[i]);
                }
                graficaBarrasAktivo = false;
            }
            if(graficaPolilineaAktivo == true)
            {
                ElCanvas.Children.Remove(graficaPolilinea);
                graficaPolilineaAktivo = false;
            }
            areaSeleccionada.Visibility = Visibility.Collapsed;

        }
        

        private void ElCanvas_Size_Changed(object sender, SizeChangedEventArgs e)
        {

            if (graficaBarrasAktivo == true)
            {
                for (int i = 0; i < listaDeRectangulos.Count; i++)
                {
                    ElCanvas.Children.Remove(listaDeRectangulos[i]); 
                }
                listaDeRectangulos.Clear();
                representarGraficaDeBarras(listaDePuntos);
            }
            else if(graficaPolilineaAktivo == true)
            {
                ElCanvas.Children.Remove(graficaPolilinea);
                representarGrafica(listaDePuntos);
            }

            ElCanvas.Children.Remove(ejeY);
            ElCanvas.Children.Remove(ejeX);
            representarLosEjes();
            areaSeleccionada.Visibility = Visibility.Collapsed;

        }

        private void ElCanvas_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
         
            
            if(graficaBarrasAktivo == true)
            {
                Rectangle areaSeleccionada = new Rectangle();
                click_presionado = true;
                rectangulo_activo = true;
                puntoClick = e.GetPosition(ElCanvas);

                Canvas.SetTop(areaSeleccionada, puntoClick.Y);
                Canvas.SetLeft(areaSeleccionada, puntoClick.X);
                areaSeleccionada.Visibility = Visibility.Visible;
            }
            if (graficaPolilineaAktivo == true)
            {
                Rectangle areaSeleccionada = new Rectangle();
                click_presionado = true;
                rectangulo_activo = true;
                puntoClick = e.GetPosition(ElCanvas);

                Canvas.SetTop(areaSeleccionada, puntoClick.Y);
                Canvas.SetLeft(areaSeleccionada, puntoClick.X);
                areaSeleccionada.Visibility = Visibility.Visible;
            }


        }

        private void ElCanvas_MouseMove(object sender, MouseEventArgs e)
        {
            if (click_presionado==true)
            {
                Point mousePos = e.GetPosition(ElCanvas);
                areaSeleccionada.Visibility = Visibility.Visible;


                if (puntoClick.X < mousePos.X)
                {
                    Canvas.SetLeft(areaSeleccionada, puntoClick.X);
                    areaSeleccionada.Width = mousePos.X - puntoClick.X;
                }
                else
                {
                    Canvas.SetLeft(areaSeleccionada, mousePos.X);
                    areaSeleccionada.Width = puntoClick.X - mousePos.X;
                }

                if (puntoClick.Y < mousePos.Y)
                {
                    Canvas.SetTop(areaSeleccionada, puntoClick.Y);
                    areaSeleccionada.Height = mousePos.Y - puntoClick.Y;
                }
                else
                {
                    Canvas.SetTop(areaSeleccionada, mousePos.Y);
                    areaSeleccionada.Height = puntoClick.Y - mousePos.Y;
                }
            }
        }

        private void ElCanvas_MouseUp(object sender, MouseButtonEventArgs e)
        {

            if(rectangulo_activo == true)
            {
                Point mouseUpPos = e.GetPosition(ElCanvas);

                if(mouseUpPos.X == puntoClick.X && mouseUpPos.Y == puntoClick.Y) //si haces click y no arrastras
                {
                    rectangulo_activo = false;
                    areaSeleccionada.Visibility = Visibility.Collapsed;

                    if (graficaPolilineaAktivo == true)
                    {
                        ElCanvas.Children.Remove(graficaPolilinea);
                        representarGrafica(listaDePuntos);
                    }
                    if (graficaBarrasAktivo == true)
                    {
                        for (int i = 0; i < listaDeRectangulos.Count; i++)
                        {
                            ElCanvas.Children.Remove(listaDeRectangulos[i]);
                        }
                        representarGraficaDeBarras(listaDePuntos);
                    }
                    click_presionado = false;
                }
                else
                {
                    ElCanvas.ReleaseMouseCapture();
                    areaSeleccionada.Visibility = Visibility.Visible;


                    if (graficaPolilineaAktivo == true)
                    {
                        ElCanvas.Children.Remove(graficaPolilinea);
                        representarGraficaConAreaSeleccionada(listaDePuntos, mouseUpPos);
                        click_presionado = false;
                    }
                    if (graficaBarrasAktivo == true)
                    {
                        for (int i = 0; i < listaDeRectangulos.Count; i++)
                        {
                            ElCanvas.Children.Remove(listaDeRectangulos[i]);
                        }
                        listaDeRectangulos.Clear();
                        representarGraficaDeBarrasConAreaSeleccionada(listaDePuntos, mouseUpPos);
                        click_presionado = false;
                    }
                }
            }         
        }

        

        

        private void ElCanvas_MouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
            if(rectangulo_activo == true)
            {
                rectangulo_activo = false;
                areaSeleccionada.Visibility = Visibility.Collapsed;

                if(graficaPolilineaAktivo == true)
                {
                    ElCanvas.Children.Remove(graficaPolilinea);
                    representarGrafica(listaDePuntos);
                }
                if(graficaBarrasAktivo == true)
                {
                    for (int i = 0; i < listaDeRectangulos.Count; i++)
                    {
                        ElCanvas.Children.Remove(listaDeRectangulos[i]);
                    }
                    representarGraficaDeBarras(listaDePuntos);
                }
                
            }
            
        }
    }
}
