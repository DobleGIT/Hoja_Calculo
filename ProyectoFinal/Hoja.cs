using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProyectoFinal
{
    class Hoja
    {
        private String nombreHoja;
        List<Puntos> listaPuntos = new List<Puntos>(); //lista en la que se guardan los puntos

        public string Nombre
        {
            get { return nombreHoja; }
            set { nombreHoja = value; }
        }
        public List<Puntos> RecuperarListaPuntos
        {
            get { return listaPuntos; }
            
        }
 

        public Hoja(string nombreHoja)
        {
            this.nombreHoja = nombreHoja;

        }
        public void añadirPuntos(float coorX, float coorY)
        {
            Puntos pt = new Puntos(coorX, coorY);
            listaPuntos.Add(pt);


        }

        internal bool comprobarPuntosEnLista(float coorX, float coorY)
        {
            for(int i=0;i< listaPuntos.Count;i++)
            {
                if(listaPuntos[i].CoorX == coorX && listaPuntos[i].CoorY==coorY )
                {
                    return true;
                    
                }
            }
            return false;
        }
        public void borrarPuntos(Puntos punto)
        {
            listaPuntos.Remove(punto);
        }

        internal void ordenarPuntosX()
        {
            listaPuntos = listaPuntos.OrderBy(y => y.CoorY).OrderBy(x => x.CoorX).ToList();
        }

        internal void ordenarPuntosY()
        {
            listaPuntos = listaPuntos.OrderBy(x => x.CoorX).OrderBy(y => y.CoorY).ToList();
        }
    }
    
}
