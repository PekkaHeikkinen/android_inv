using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace Epic_text_adventure___android_invasion
{
    class Program
    {
        static bool quit = false;
        static int currentPage = 1;                                    //nykyinen sivu
        static bool checkQuit(ConsoleKey key)
        {
            if (key == ConsoleKey.Q)
                return true;
            else
                return false;
        }
        static void valinta(ref string[] choicesByPage)
        {
            if(choicesByPage.Length == 0)
            {
                Console.WriteLine("no choices on the choicesbypage file :( quitting...");
                quit = true;
            }

            string[] choicesOnThisPage = choicesByPage[currentPage].Split('\n');
            if(choicesOnThisPage.Length == 0)
            {
                //Game over ei vaihtoehtoja
                Console.WriteLine("no more choices on this page, quitting game...");
                Console.ReadKey();
                quit = false;
            }
            if(choicesOnThisPage.Length-1 == 1)
            {
                //on vain yksi valinta, ei näytetä sitä vaan vaihdetaan sivua
                int num = 0;
                //onko stringissä inttiä
                if(int.TryParse(choicesOnThisPage[0], out num)){ //jos string muuttujasta löytyy int, tallenna se num
                    currentPage = num;                           //aseta currentpageksi num
                }
            }
            if(choicesOnThisPage.Length-1 > 1)
            {
                for(int i = 0; i < choicesOnThisPage.Length; i++)
                {
                    if (choicesOnThisPage[i].Length-1 > 0)
                    {
                        Console.WriteLine("Choice[" + i + "] = Go to page " + choicesOnThisPage[i]);
                    }
                }

                bool foundChoice = false;                                       //onko löydetty validi vaihtoehto inputista
                while(foundChoice == false){                                    //kysytään input niinkauan kunnes antaa oikean numeron
                    ConsoleKeyInfo key = Console.ReadKey();                     //haetaan keyinfo readkeyllä
                    int choice = 0;                                             //meidän sivu valinta muuttuja
                   
                    for(int i = 0; i < choicesOnThisPage.Length; i++)           //käydään läpi kaikki vaihtoehdot tällä sivulla
                    {
                        if (char.IsDigit(key.KeyChar))                          //katsotaan oliko näppäin numero, tarkistus
                        {
                            int keyasNumber  = int.Parse(key.KeyChar.ToString());   //jos oli niin muunnetaan string intiksi keyasnumber muuttujaan
                            if (int.TryParse(choicesOnThisPage[i], out choice))     //katsotaan onko valinnat.txt tiedoston sen rivin arvo oikeasti numero vai ei
                            {
                                //Console.WriteLine("checking if... " + keyasNumber + " == " + (i));    //debug viesti
                                if ((keyasNumber) == i)                      // onko readkeyn keyasnumber validi valinta
                                {
                                    //Console.WriteLine("yes " + keyasNumber + " == "  +i);
                                    foundChoice = true;                         //oli
                                    currentPage = choice;                       //asetaan sivuksi choice
                                    break;
                                }
                            }                           
                        }
                        else
                        {
                            Console.WriteLine("error, couldnt make keyinfo into number");
                        }
                    }
                     if(foundChoice == false)
                     {
                        Console.WriteLine("valinta.txtssa ei ole lukua " + choicesOnThisPage[0]);
                     }
                }
            }
        }

        static void print_page(ref string[] pages, int i) //pass by reference, ei kopioi uutta string[] vaan lähettää aikaisemmasta osoitteen mistä löytyy
        {
    
            //TODO: bufferointi jos sivu on liian pitkä näytettäväksi kerralla

            string[] lines = pages[i].Split('\n');                           // jaa nykyinen sivu riveihin, käyttäen rivin vaihto merkkiä eroittimena
            int currentLine = 0;                                             //nykyinen rivi joka printataan
            Console.Clear();                                                 //tyhjennetään konsoli
                                                  //ei olla lopetettu peliä
            while(currentLine < lines.Length && quit == false)                                //printataan niin kauan kunnes rivit loppuu
            {
                if (currentLine != 0)                                        //ei printata ekaa riviä, sitä jossa on @ merkki
                {
                    if (lines[currentLine].Length > 0)                       //katsotaan että rivin teksti ei ole tyhjä, jos haluat tyhjiä rivejä niin kommentoi tämä pois
                    {
                        Console.WriteLine(lines[currentLine]);               //printtataan nykyinen rivi
                    }
                    if (currentLine % 10 == 0)                               // katsotaan onko rivinumero jaettuna 10 jakojäännös 0, eli onko luku jaollinen 10:llä
                    {
                        Console.WriteLine("\nPressKeyToNextPage");          //sanotaan että paina jotain
                        ConsoleKey key = Console.ReadKey().Key;             //luetaan jotain näppäintä
                        quit = checkQuit(key);                              //katso oliko näppäin Q, jos on niin lopetataan ohjelma
                        Console.Clear();                                    //tyhjennetään sivu, jotta seuraava page näyttää sen 10 omaa riviä
                    }
                }
                currentLine++;                                              //kasvatetaan rivinumeroa

            }
            if (quit == false)
            {
           //     Console.WriteLine("\nPress left arrow to go to the previous chapter,\nright arrow to go to the next chapter");  //rivit on printattu, vaihdetaan kirjan sivua
            }
      
        }

        static void Main(string[] args)
        {
            string[] choicesByPage = new string[30];
            using (StreamReader sr1 = new StreamReader("valinnat.txt"))      //haetaan valinnat
            {
                String choicesfile = sr1.ReadToEnd();
                string[] temp = choicesfile.Split('@');                      //jaetaan valinnat sivuttain, erottaen @merkillä
                string[] temp2;
                int rivi = 0;

                //laske rivien määrä, jotka eivät ole tyhjiä
                for (int i = 0; i < temp.Length; i++)
                {
                    if (temp[i].Length > 0)
                    {
                        rivi++;
                    }
                }
                temp2 = new string[rivi];                           //alusta oikein kokoiseksi
                rivi = 0;                                                   //resetoi rivi

                for (int i = 0; i < temp.Length; i++)                       //putsataan tyhjät rivit pois 
                {
                    if (temp[i].Length > 0)                                  //jos rivi ei ole tyhjä
                    {
                        temp2[rivi] += temp[i];                     //lisää tämä rivi
                        temp2[rivi] += '\n';                        //lisää rivin vaihto
                        rivi++;
                    }
                }
                //poistetaan kommentit eli ei lukuja olevat rivit
                rivi = 0;
           
                for (int i = 0; i < temp2.Length; i++)
                {
                    string[] temp3 = temp2[i].Split('\n');
                    for (int j = 0; j < temp3.Length; j++)
                    {
                        int num;
                        if (int.TryParse(temp3[j], out num))
                        {
                            choicesByPage[rivi] = temp3[j];
                            rivi++;
                        }
                    }
                  
                }


            }
            

            try                                                              // jos tämä koodi lohko epäonnistuu niin catch kohdassa napataan virheilmoitus ja voidaan käsitellä se
            {                                                                // Open the text file using a stream reader.
                using (StreamReader sr = new StreamReader("TestFile.txt"))
                {
                    
                    String line = sr.ReadToEnd();                            //lue koko tiedosto
                    String[] pages = line.Split('@');                        //jaa tiedosto sivuihin @ merkeillä

                   
                    print_page(ref pages, currentPage);                     //kun ohjelma alkaa printataan eka sivu
                    valinta(ref choicesByPage);
                    while (quit == false)                                   //toistetaan kunnes on painettu q
                    {
                                   //luetaan jotain näppäintä
                      //  quit = checkQuit(key);                              //katso oliko näppäin Q, jos on niin lopetataan ohjelma
                        print_page(ref pages, currentPage);
                        valinta(ref choicesByPage);
                        /*ConsoleKey key = Console.ReadKey().Key;
                        //debug mene eteen taakse sivuilla
                        if (key == ConsoleKey.LeftArrow)                    // jos näppäin on vasen nuoli
                        {
                            currentPage--;                                  //mennään edelliselle sivulle
                            if (currentPage <= 1)                           //ei mennä ulos sivujen mini määrästä, eka sivu on 1, ei 0 hox!
                                currentPage = 1;                            //laitetaan pienin sivu. jos vähennettiin liikaa
                            print_page(ref pages, currentPage);
                        }
                        else if(key == ConsoleKey.RightArrow)           // jos painetaan oikea nuolinäppäin
                        {
                            currentPage++;                                  //kasvatetaan sivuja
                            if(currentPage >= pages.Length)                 //jos on viimeinen sivu
                            {
                                currentPage = pages.Length - 1;             //laitetaan sivu viimeiseksi sivuksi
                            }
                            print_page(ref pages, currentPage);             //printataan sivu
                        }*/
                     
                    }
                }
            }

            catch (Exception e) // jos tulee virheitä niin tulostetaan virhe ruudulle
            {
                Console.Clear();    //tyhjentää ruudun
                Console.WriteLine("The file could not be read:");
                Console.WriteLine(e.Message);
            }

            // PELI LOPPUI
            Console.Clear();
            Console.WriteLine("Quitting..");
            Console.ReadKey();
        }
    }
}
