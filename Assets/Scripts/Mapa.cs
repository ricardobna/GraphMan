using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Collections.Generic;
using System;

public class Mapa : MonoBehaviour
{
    Grafo grafoMapa;
    GameObject pacman;
    GameObject blinky;
    Pacman_Movement pacmanScript;
    class Vertice
    {
        public float coordenadaX;
        public float coordenadaY;

        public List<Vertice> vizinhos;
        public Vertice parent;
        public enum Color
        {
            BRANCO,
            CINZA,
            PRETO
        }
        public Color color;
        public Vertice(float x, float y)
        {
            this.coordenadaX = x;
            this.coordenadaY = y;
            this.vizinhos = new List<Vertice>();
        }

    }

    class Grafo
    {
        public List<Vertice> vertices;
        Queue<Vertice> verticesCinza;

        public Grafo()
        {
            vertices = new List<Vertice>();
            verticesCinza = new Queue<Vertice>();
        }

       public void fillArestas()
        {
            foreach(Vertice vertice in vertices)
            {
                //fixes x
                foreach(Vertice vertice2 in vertices)
                {
                    if(vertice.coordenadaX - vertice2.coordenadaX == 1 || vertice.coordenadaX - vertice2.coordenadaX == -1)
                    {
                        if (vertice.coordenadaY - vertice2.coordenadaY == 0)
                        {
                            vertice.vizinhos.Add(vertice2);
                        }
                    }
                }
                //fixes y
                foreach (Vertice vertice2 in vertices)
                {
                    if (vertice.coordenadaY - vertice2.coordenadaY == 1 || vertice.coordenadaY - vertice2.coordenadaY == -1)
                    {
                        if (vertice.coordenadaX - vertice2.coordenadaX == 0)
                        {
                            vertice.vizinhos.Add(vertice2);
                        }
                    }
                }
            }

        }

        public void findPacman(Vertice start, Vertice finish)
        {
            List<Vertice> verticesCinza = new List<Vertice>();
            foreach(Vertice vertice in vertices)
            {
                vertice.color = Vertice.Color.BRANCO;
            }
            start.color = Vertice.Color.CINZA;
            this.verticesCinza.Enqueue(start);
            while(true)
            {
            try {
                    Vertice u = this.verticesCinza.Dequeue();
                    foreach(Vertice v in u.vizinhos)
                    {
                        if(v.color == Vertice.Color.BRANCO)
                        {
                            v.color = Vertice.Color.CINZA;
                            v.parent = u;
                            this.verticesCinza.Enqueue(v);
                        }
                    }
                    u.color = Vertice.Color.PRETO;
                } catch(InvalidOperationException e)
                {
                    break;
                }
            }
            GameObject blinky = GameObject.Find("Blinky");
            Vector2 blinkyDestination = new Vector2(finish.parent.coordenadaX, finish.parent.coordenadaY);
            Vector2 position = Vector2.MoveTowards(blinky.transform.position, blinkyDestination, 0.1f);
            blinky.GetComponent<Rigidbody2D>().MovePosition(position);
            Vector2 dir = blinkyDestination - (Vector2)blinky.transform.position;
            blinky.GetComponent<Animator>().SetFloat("DirX", dir.x);
            blinky.GetComponent<Animator>().SetFloat("DirY", dir.y);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        grafoMapa = new Grafo();
        pacman = GameObject.Find("Pacman");
        blinky = GameObject.Find("Blinky");
        pacmanScript = pacman.GetComponent<Pacman_Movement>();
        //Lê as comidas
        foreach (Transform child in transform)
        {
            //Cria o vérticeComida
            Vertice auxiliarComida = new Vertice(child.transform.position.x, child.transform.position.y);

            //Adicionar o vérticeComida no GrafoMapa
            grafoMapa.vertices.Add(auxiliarComida);
        }
        grafoMapa.fillArestas();
    }

    void FixedUpdate()
    {
        Vertice verticePacman = null;
        Vertice verticeBlinky = null;
        if (pacman != null)
        {
            foreach (Vertice vertice in grafoMapa.vertices)
            {
                if (Math.Round(pacman.transform.position.x) == Math.Round(vertice.coordenadaX) && Math.Round(pacman.transform.position.y) == Math.Round(vertice.coordenadaY))
                {
                    verticePacman = vertice;
                }
                if (Math.Round(blinky.transform.position.x) == Math.Round(vertice.coordenadaX) && Math.Round(blinky.transform.position.y) == Math.Round(vertice.coordenadaY))
                {
                    verticeBlinky = vertice;
                }
                if (verticeBlinky != null && verticePacman != null)
                {
                    break;
                }
            }
        }
        if(verticePacman != null && verticeBlinky != null)
        {
            if(verticeBlinky.coordenadaX == verticePacman.coordenadaX && verticeBlinky.coordenadaY == verticePacman.coordenadaY)
            {
                pacmanScript.destroyPacman();
            } else
            {
                grafoMapa.findPacman(verticePacman, verticeBlinky);
            }
        }
    }   
}
