using Microsoft.Win32;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace _20230103Ex_metodes_Hotel
{
    internal class Aplicacio
    {


        public void Inici()
        {
            String[,] hotel = DadesHotel();
            String[,] clients = DadesClient();

            bool salir = false;
            string opcio;

            do
            {
                mostrarMenu();
                opcio = DemanarOpcioMenu();
                salir = ExecutarMenu(hotel, clients, opcio);

            } while (!salir);
            Console.WriteLine();
        }

        string DemanarOpcioMenu()
        {
            string opcio;
            do
            {
                Console.Write("Sel.lecciona opció: ");
                opcio = Console.ReadLine();
            } while (!"0123456789".Contains(opcio));
            return opcio;
        }

        bool ExecutarMenu(String[,] hotel, String[,] clients, String opcio)
        {
            bool salir = false;
            switch (opcio)
            {
                case "1":
                    MostrarClients(clients);
                    break;
                case "2":
                    AltaClients(clients);
                    break;
                case "3":
                    //Mostrar  habitacions Hotel
                    MostrarHotel(hotel, clients, false);//pasem els dos arrays i un false
                    break;
                case "4":
                    MostrarLliures(hotel);
                    break;
                case "5":
                    MostrarOcupades(hotel, clients, false);
                    break;
                case "6":
                    EntrarReserva(hotel, clients);
                    break;
                case "7":
                    LlistaReserves(hotel);
                    break;
                case "8":
                    Factura(hotel, clients);
                    break;
                case "9":
                    AnularReserva(hotel, clients, false);
                    break;

                case "0":
                    salir = true;
                    break;

            }
            return salir;
        }



        void AltaClients(String[,] clients)
        {
            string dniClient;
            dniClient = DemanaClient();
            int fila;
            fila = existsNif(dniClient, clients);
            string nomCLient;
            string mailCLient;
            int filaLliure = getNewFilaClientes(clients);

            if (fila != REGISTRE_INEXISTENT)
            {
                Console.WriteLine("El client ja existeix");
            }
            else
            {
                clients[filaLliure, CLI_NIF] = dniClient;
                Console.WriteLine("Introdueix el nom del client: ");
                nomCLient = Console.ReadLine();
                clients[filaLliure, CLI_NOM] = nomCLient;
                Console.WriteLine("Introdueix el e-mail del client: ");
                mailCLient = Console.ReadLine();
                clients[filaLliure, CLI_MAIL] = mailCLient;
            }
        }
        int getNewFilaClientes(String[,] clientes)
        {
            bool encontrado = false;
            int filaLliure = 0;
            while (filaLliure < clientes.GetLength(0) & !encontrado)
            {
                if (clientes[filaLliure, ID_CLIENT].Equals(""))
                {
                    encontrado = true;
                }
                else
                {
                    filaLliure++;
                }
            }
            if (encontrado)
            {
                return filaLliure;
            }
            else
            {
                return REGISTRE_INEXISTENT;
            }
        }

        int existsNif(String user, String[,] clients)
        {
            bool encontrado = false;
            int fila = 0;
            while (fila < clients.GetLength(0) & !encontrado)
            {
                if (user.Equals(clients[fila, CLI_NIF]))
                {
                    encontrado = true;

                }
                else
                {
                    fila++;
                }
            }
            if (encontrado)
            {
                return fila;
            }
            else
            {
                return REGISTRE_INEXISTENT;
            }

        }

        public string DemanaClient()
        {
            string dniClient;
            Console.Write("Introdueix Dni del CLient: ");
            dniClient = Console.ReadLine();
            return dniClient;
        }

        //rep dos arrays i un boleano. En la opció 3 false en la segona true.
        void MostrarHotel(String[,] hotel, string[,] clients, bool mostrarnombre)
        {
            Console.WriteLine("\nNum Hab\tPis\tN Llits\tPreu\tDni Client");
            //recorrem l'array hotel
            for (int i = 0; i < hotel.GetLength(0); i++)
            {
                for (int j = 0; j < hotel.GetLength(1); j++)
                {

                    //si a l'array hotel, el que troba no es null i el boolean es fals (no mostrarnom)
                    if (hotel[i, j] != null & !mostrarnombre)
                    {
                        //escriu el que surti
                        Console.Write(hotel[i, j] + "\t");
                    }
                    //si no si a l'array hotel, el que troba no es null i el boolean es true (hem de  mostrarnom)
                    else if (hotel[i, j] != null & mostrarnombre)
                    {
                        //si la columna jo no es la columna 4(hot_nif)
                        if (j != HOT_NIF)
                        //escriu el que hi hagi
                        {
                            Console.Write(hotel[i, j] + "\t");
                            //si no, es a dir, a la columna 4, fem una búsqueda a l'array clients
                        }
                        else
                        {
                            bool encontrado = false;
                            int fila = 0;

                            //si a la búsqueda trobem que els valors de HOT_NIF I CLI_NIF COINCIDEIXEN
                            while (fila < clients.GetLength(0) & !encontrado)
                            {
                                if (hotel[i, HOT_NIF] == clients[fila, CLI_NIF])
                                {
                                    encontrado = true;//ENCONTRADO TRUE
                                }
                                else// SI NO ES ENCONTRADO ES QUE NO L'HA TROBAT I HA DE SEGUIR RECORRENT
                                {
                                    fila++;
                                }
                            }
                            if (encontrado)//SI L'HA TROBAT
                            {//ESCRIU A L'ARRAY CLIENTS A LA POSICIÓ 4(CLI_NOM) EL QUE HAGIS TROBAT
                                Console.Write(clients[fila, CLI_NOM]);
                            }

                        }
                    }
                    else
                    {//SI ES NULL MOSTRA LIBRE
                        Console.Write("Libre");
                    }

                }
                Console.WriteLine();
            }
            Console.WriteLine();
        }

        void MostrarClients(String[,] clients)
        {
            Console.WriteLine("\nNif\tNom\te-mail");
            for (int i = 0; i < clients.GetLength(0); i++)
            {
                for (int j = 0; j < clients.GetLength(1); j++)
                {

                    if (clients[i, j] != "")
                    {
                        Console.Write(clients[i, j] + "\t");
                    }

                }
                if (clients[i, 0] != "")
                {
                    Console.WriteLine();
                }

            }
            Console.WriteLine();
        }

        void MostrarLliures(String[,] hotel)
        {
            Console.WriteLine("\nNum Hab\tPis\tN Llits");
            //recorrem l'array hotel
            for (int i = 0; i < hotel.GetLength(0); i++)
            {


                if (hotel[i, HOT_NIF] == null)
                {
                    Console.WriteLine(hotel[i, 0] + "\t" + hotel[i, 1] + "\t" + hotel[i, 2] + "\t");

                }

            }

        }
        void MostrarOcupades(String[,] hotel, String[,] clients, bool encontrado)
        {
            Console.WriteLine("\n Num Hab\tPis \t N Llits\t Preu \t Nom Client");
            //recorrem l'array hotel
            for (int i = 0; i < hotel.GetLength(0); i++)
            {
                if (hotel[i, HOT_NIF] != null)
                {
                    Console.Write("   " + hotel[i, HOT_HAB] + " \t\t " + hotel[i, HOT_PIS] + " \t " + hotel[i, HOT_LLIT] + " \t\t " + hotel[i, HOT_PREU] + " \t ");

                    int fila = 0;
                    //si a la búsqueda trobem que els valors de HOT_NIF I CLI_NIF COINCIDEIXEN
                    while (fila < clients.GetLength(0) & !encontrado)
                    {
                        if (hotel[i, HOT_NIF] == clients[fila, CLI_NIF])
                        {

                            encontrado = true;//ENCONTRADO TRUE
                        }
                        else// SI NO ES ENCONTRADO ES QUE NO L'HA TROBAT I HA DE SEGUIR RECORRENT
                        {
                            fila++;
                        }
                    }
                    if (encontrado)//SI L'HA TROBAT
                    {//ESCRIU A L'ARRAY CLIENTS A LA POSICIÓ 4(CLI_NOM) EL QUE HAGIS TROBAT
                        Console.WriteLine(clients[fila, CLI_NOM]);
                    }


                }

            }
        }

        void EntrarReserva(String[,] hotel, String[,] clients)
        {
            string nHab;
            string tipusHab;
            string nifClient;
            int filaClient;
            int count = 0;


            Console.WriteLine("Sel.leccioni el nº d'habitacions que desitja: ");
            nHab = Console.ReadLine();
            Console.WriteLine("Sel.leccioni el tipus d'habitació: ");
            Console.Write("-1 Simple\t" + "2-doble\t" + "3-Triple\t");
            tipusHab = Console.ReadLine();

            //Mètode recórrer l'array.
            //Busquem si hi ha habitacions lliures
            for (int fila = 0; fila < hotel.GetLength(0); fila++)
            {
                //si a la búsqueda trobem que els valors de HOT_HAB I tipusHab COINCIDEIXEN
                if ((hotel[fila, HOT_LLIT] == tipusHab) & (hotel[fila, HOT_NIF] == null))
                {
                    count++;//el count ens diu cuantes hab. lliures hi ha
                }
            }
            if (count < int.Parse(nHab))//si hi ha menys hab lliures de les sol.licitades
            {
                Console.WriteLine("No hi ha suficients habitacions disponibles del tipus escollit");
            }
            else//si hi ha suficients hab lliures
            {
                nifClient = DemanaClient();//demana el nif
                filaClient = existsNif(nifClient, clients);//mira si existeix al array clients i en quina fila
                if (filaClient != REGISTRE_INEXISTENT)//si existeix,
                {
                    Console.WriteLine("Benvingut senyor" + clients[filaClient, CLI_NOM]);
                }
                else//si el client no existeix
                {

                    int filaLliure;
                    filaLliure = getNewFilaClientes(clients);//busca una fila lliure al array clients

                    if (filaLliure == REGISTRE_INEXISTENT) //si no hi ha files lliures
                    {
                        Console.WriteLine("No hi ha espai a la memòria. Avisa al tècnic");
                    }
                    else //si hi ha files llliures
                    {
                        Console.WriteLine("Introdueix el nom del client: ");
                        string nomClient = Console.ReadLine();
                        Console.WriteLine("Introdueix el email del client: ");
                        string mailClient = Console.ReadLine();
                        //omplim l'array clients amb les dades obtingudes i el mostrem
                        clients[filaLliure, CLI_NOM] = nomClient;
                        clients[filaLliure, CLI_NIF] = nifClient;
                        clients[filaLliure, CLI_MAIL] = mailClient;
                        MostrarClients(clients);
                    }

                }
                int countHab = 0;
                int fila = 0;

                //busquem les hab lliures i hi introduim les dades de client
                while (fila < hotel.GetLength(0) & countHab < int.Parse(nHab))
                {
                    if (hotel[fila, HOT_NIF] == null & hotel[fila, HOT_LLIT] == tipusHab)
                    {
                        hotel[fila, HOT_NIF] = nifClient;
                        countHab++;

                    }
                    fila++;
                }
                MostrarHotel(hotel, clients, false);
            }
        }

        void LlistaReserves(String[,] hotel)
        {
            string client;

            client = DemanaClient();
            int count = 0;

            Console.WriteLine("\nNum Hab\tPis\tN Llits\tPreu");
            //recorrem l'array hotel
            for (int i = 0; i < hotel.GetLength(0); i++)
            {
                if (hotel[i, HOT_NIF] == client)
                {
                    Console.WriteLine(hotel[i, HOT_HAB] + "\t" + hotel[i, HOT_PIS] + "\t" + hotel[i, HOT_LLIT] + "\t" + hotel[i, HOT_PREU] + "\t");
                    count++;
                }

            }
            if (count == 0)
            {
                Console.WriteLine("No hi ha cap client amb aquestes dades.");
            }

        }

        void Factura(String[,] hotel, String[,] clients)
        {
            string client;
            int fila;

            client = DemanaClient();//Demanem Dni
            fila = existsNif(client, clients);//busquem si existeix al array clients

            if (fila == REGISTRE_INEXISTENT)//si el client no existeix mostrem missatge
            {
                Console.WriteLine("No hi ha cap client amb aquest DNI registrat");
            }
            //Si el client existeix                     
            else
            {
                Console.WriteLine("\nNum Hab\tPis\tN Llits\tPreu");
                int suma = 0;
                int count = 0;
               
                //recorrem l'array hotel
                for (int i = 0; i < hotel.GetLength(0); i++)
                {
                    if (hotel[i, HOT_NIF] == client)
                    {
                        Console.WriteLine(hotel[i, HOT_HAB] + "\t" + hotel[i, HOT_PIS] + "\t" + hotel[i, HOT_LLIT] + "\t" + hotel[i, HOT_PREU] + "\t");
                        count++;
                        suma += int.Parse(hotel[i, HOT_PREU]);
                    }
                }
                if (count == 0)
                {
                    Console.WriteLine("No hi ha cap client amb aquestes dades.");
                }
                //anem a fer factura
                Console.WriteLine();
                Console.WriteLine();
                Console.WriteLine("Hotel Ilerna Nif:65478912B");
                Console.WriteLine("Sr/Sra. " + clients[fila, CLI_NOM]);
                Console.WriteLine("NIF: " + clients[fila, CLI_NIF]);
                //Console.WriteLine("Nº Factura" + rand);
                Console.WriteLine("PREU TOTAL: " + suma);
                Console.WriteLine();
            }
        }

        void AnularReserva(String[,] hotel, String[,] clients, bool encontrado)
        {
            string client;
            int fila;

            client = DemanaClient();//Demanem Dni
            fila = existsNif(client, clients);//busquem si existeix al array clients

            if (fila == REGISTRE_INEXISTENT)//si el client no existeix mostrem missatge
            {
                Console.WriteLine("No hi ha cap client amb aquest DNI registrat");
            }
            //Si el client existeix                     
            else
            {
                clients[fila, CLI_NIF] = "";
                clients[fila, CLI_NOM] = "";
                clients[fila, CLI_MAIL] = "";

                //recorrem l'array hotel
                for (int i = 0; i < hotel.GetLength(0); i++)
                {
                    if (hotel[i, HOT_NIF] == client)
                    {
                        hotel[i, HOT_NIF] = null;
                        
                    }
                }
            }
            MostrarClients(clients);
            MostrarHotel(hotel, clients, encontrado);
        }



        string[,] DadesHotel()
        {


            String[,] hotel = {
            //num hab/ pis/ n llits/ preu/ null o dni pers
            {"101", "1", "2", "100", "11111111A"},
            {"102", "1", "2", "100", "11111111A"},
            {"103", "1", "2", "100", "22222222B"},
            {"104", "1", "3", "130", "22222222B"},
            {"105", "1", "3", "130", "33333333C"},
            {"106", "1", "1", "75",  "44444444D"},
            {"201", "2", "2", "100", "44444444D"},
            {"202", "2", "2", "100", "44444444D"},
            {"203", "2", "2", "100", "55555555E"},
            {"204", "2", "3", "130", null},
            {"205", "2", "3", "130", null},
            {"206", "2", "1", "75",  null},
            {"301", "3", "2", "100", null},
            {"302", "3", "2", "100", null},
            {"303", "3", "2", "100", null},
            {"304", "3", "3", "130", null},
            {"305", "3", "3", "130", null},
            {"306", "3", "1", "75",  null}};
            return hotel;
        }

        string[,] DadesClient()
        {
            //Nif/nom_CLient/email
            string[,] clients = {
            {"11111111A", "Josep Busquets", "jbusquets@gmail.com", },
            {"", "", "", },
            {"", "", "", },
            {"", "", "", },
            {"", "", "", },
            {"", "", "", },
            {"", "", "", },
            {"44444444D", "Hector Puig", "hp@gmail.com", },
            {"", "", "", },
            {"44444444D", "Hector Puig", "hp@gmail.com", },
            {"22222222B", "Maria Garcia", "mgarcia@hotmail.com", },
            {"22222222B", "Maria Garcia", "mgarcia@hotmail.com", },
            {"55555555E", "Isabel Casas", "isacasas@hotmail.com", },
            {"33333333C", "Albert Gonzalez", "gonzalbert@icloud.com", },
            {"55555555E", "Isabel Casas", "isacasas@hotmail.com", } };

            return clients;
        }

        void mostrarMenu()
        {
            Console.WriteLine("1. Mostrar clients ");
            Console.WriteLine("2. Alta clients");
            Console.WriteLine("3. Mostrar  habitacions Hotel");
            Console.WriteLine("4. Mostrar habitacions lliures");
            Console.WriteLine("5. Mostrar habitacions ocupades amb el nom");
            Console.WriteLine("6. Entrar reserva");
            Console.WriteLine("7. Llista de reserves a partir d'un Nif");
            Console.WriteLine("8. Factura de la  reserva a partir d'un Nif");
            Console.WriteLine("9. Anul.lar reserva");
            Console.WriteLine("0. Sortir");
        }


        const int CLI_NIF = 0;
        const int CLI_NOM = 1;
        const int CLI_MAIL = 2;
        const int REGISTRE_INEXISTENT = -1;
        const int ID_CLIENT = 0;

        //num hab/ pis/ n llits/ preu/ null o dni pers
        const int HOT_HAB = 0;
        const int HOT_PIS = 1;
        const int HOT_LLIT = 2;
        const int HOT_PREU = 3;
        const int HOT_NIF = 4;
    }
}
