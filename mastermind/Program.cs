using System;
using System.Linq;

namespace mastermind
{
    public class Program
    {

        public enum color
        {
            ROUGE,
            BLEU,
            VERT,
            JAUNE,
            VIOLET,
            ORANGE
        }

        public struct last_round
        {
            public color[] player_combi;
            public int combi_bp;
            public int combi_bv;
            public bool past_round;
        }

        static void Main()
        {
            last_round[] all_round = new last_round[1];

            color[] secret_combi;
            color[] player_combi = new color [4];

            bool correct_combi = false;

            int max_round = 0;
            int max_game = 0;
            int current_round = 0;
            int current_game = 0;
            int total_game_win = 0;
            int combi_white = 0;
            int combi_red = 0;

            Console.WriteLine(",--.   ,--.                ,--.                ,--.   ,--.,--.           ,--.");
            Console.WriteLine("|   `.'   | ,--,--. ,---.,-'  '-. ,---. ,--.--.|   `.'   |`--',--,--,  ,-|  |");
            Console.WriteLine("|  |'.'|  |' ,-.  |(  .-''-.  .-'| .-. :|  .--'|  |'.'|  |,--.|      \' .-. |");
            Console.WriteLine("|  |   |  |\\ '-'  |.-'  `) |  |  \\   --.|  |   |  |   |  ||  ||  ||  |\\ `-' |");
            Console.WriteLine("`--'   `--' `--`--'`----'  `--'   `----'`--'   `--'   `--'`--'`--''--' `---' ");
            Console.WriteLine(" ");
            Console.WriteLine("------------------------------------------------------------------------------\n\r\n\r\n\r");

            set_rules(ref max_game, ref max_round, ref all_round);

            rules(max_game,  max_round);
            random_secret_combi(out secret_combi);
            Show_secret_cobmbi(secret_combi); //only to debug
            

            start_game(ref current_game);
            start_round(max_round, ref current_round);
            Get_player_combi(ref player_combi);
            Get_player_combi_red(secret_combi, player_combi, ref correct_combi, ref combi_red);
            Get_player_combi_white(secret_combi, player_combi, ref combi_white);
            end_round(max_round, max_game, secret_combi, ref current_game, ref current_round, ref correct_combi, ref total_game_win, ref combi_red, ref combi_white, all_round, player_combi);

            Console.ReadLine();
        }

        static void start_game(ref int current_game)
        {
            current_game = current_game + 1;

            Console.WriteLine("----------------------------------------------------------------\n\r");
            Console.WriteLine($"--------------------------Partie(s) : {current_game}-------------------------");
            Console.WriteLine("\n\r----------------------------------------------------------------\n\r\n\r");


        }

        static void set_rules(ref int max_game, ref int max_round, ref last_round[] all_round)
        {

            const int d_max_round = 12;
            const int d_max_game = 3;

            bool correct = false;
            bool color_limit = false;

            string custom_settings = "";

            Console.WriteLine("Voulez vous personnaliser les paramètres ?");
            custom_settings = Console.ReadLine();
            custom_settings = custom_settings.ToUpper();

            if (custom_settings == "OUI")
            {
                while (correct == false && color_limit == false)
                {
                    try
                    {
                        Console.WriteLine("\n\r\n\rEntrez le nombre maximum de manches pour trouver la combinaison secrete ");
                        Console.WriteLine("________________________________________________________________________\n\r");
                        max_round = Int32.Parse(Console.ReadLine());
                        Console.WriteLine("Entrez le nombre maximum de parties à remporter");
                        Console.WriteLine("________________________________________________________________________");
                        max_game = Int32.Parse(Console.ReadLine());
                        Console.WriteLine("");
                        correct = true;
                        all_round = new last_round [max_round];
                    }
                    catch
                    {
                        Console.WriteLine("Vous avez entrer une valeur incorrecte ! \n\r\n\r");
                        continue;
                    }
                }
            }
            else
            {
                
                max_round = d_max_round;
                max_game = d_max_game;
                all_round = new last_round[max_round];
                
            }
            all_round[0].player_combi = new color[4];
        }

        static void rules(int max_game,  int max_round)
        {
            Console.WriteLine("__________________paramètres de la partie__________________\n\r");
            Console.WriteLine($"   Nombre de manches à remporter          : {max_round}");
            Console.WriteLine($"   Nombre de couleurS dans la combinaison : 4");
            Console.WriteLine($"   Nombre de parties à remporter          : {max_game}");
            Console.WriteLine($"   Voici toutes les couleurs disponible   : \n\r                         -ROUGE,\n\r                         -BLEU,\n\r                         -VERT,\n\r                         -JAUNE,\n\r                         -VIOLET,\n\r                         -ORANGE\n\r\n\r");
        }

        static void random_secret_combi(out color[] secret_combi)
        {
            secret_combi = new color[4];
            Random rnd = new Random();
            for(int i = 0; i <= 3; i++){
                color valeur  = (color)rnd.Next(0, 6);
                secret_combi[i] = valeur;
            }
        }

        static void Show_secret_cobmbi(color[] secret_combi) 
        {
            for(int i = 0; i <= 3; i++)
            {
                Console.WriteLine(secret_combi[i]);
            }
        }

        static void Get_player_combi(ref color[] player_combi)
        {
            for (int i = 0; i != 4; i++)
            {
                try
                {
                    Console.WriteLine($"entre la couleur numéro : {i + 1}\n\r ");
                    player_combi[i] = (color)Enum.Parse(typeof(color), Console.ReadLine());
                    Console.WriteLine("------------------------------\n\r");
                    
                }
                catch
                {
                    i--;
                    Console.WriteLine("");
                    Console.WriteLine("Vous n'avez pas entrer une couleur valide \n\r Merci de rentrez une nouvelle couleur \n\r");
                    continue;
                }
            }


        }

        static void Get_player_combi_red(color[] player_combi, color[] secret_combi, ref bool correct_combi, ref int combi_red)
        {
            
            for(int i = 0; i < 4; i++)
            {
                if (player_combi[i] == secret_combi[i])
                {
                    combi_red++;
                    
                }
            }
            if (combi_red == 4)
            {
                correct_combi = true;

            }
            Console.WriteLine($"{combi_red} : rouge(s)");
            Console.WriteLine("-------------");

        }

        static void Get_player_combi_white(color[] player_combi, color[] secret_combi, ref int combi_white)
        {

            color[] used_color = new color[4];

            for (int i = 0; i < 4; i++)
            {
                if (player_combi.Contains(secret_combi[i]) == true != (player_combi[i] == secret_combi[i]))
                {
                    combi_white++;
                } 
            }
            Console.WriteLine($"{combi_white} : blanc(s)");
            
        }

        static void start_round(int max_round, ref int current_round)
        {
            current_round++;

            Console.WriteLine($"         Manche : {current_round}         ");
            Console.WriteLine($"         {current_round}/{max_round} essai(s)");
            Console.WriteLine("___________________________________\n\r");
            Console.WriteLine("Entrez une combinaison de couleur");

        }

        static void end_round(int max_round, int max_game, color[] secret_combi, ref int current_game, ref int current_round, ref bool correct_combi, ref int total_game_win, ref int combi_red, ref int combi_white, last_round[] all_round, color[] player_combi)
        {
            string rounds_summary = ""; 
            string restart = "";

            // 
            all_round[current_round - 1].player_combi = new color[4];
            all_round[current_round - 1].combi_bp = combi_red;
            all_round[current_round - 1].combi_bv = combi_white;
            all_round[current_round - 1].past_round = true;
            for(int i = 0; i< 4; i++)
            {
                all_round[current_round - 1].player_combi[i] = player_combi[i];
            }

            combi_red = 0;
            combi_white = 0;

            // show last rounds
            Console.WriteLine("\n\r\n\rVoici vos essaies précedents : ");
            Console.WriteLine("--------------------------------------\n\r");
            for (int i = 0; i < all_round.Length; i++)
            {
                if (all_round[i].past_round == true)
                {
                    rounds_summary = $"manche : {i + 1}   | { all_round[i].combi_bp} : rouge(s) |  ";
                    for (int o = 0; o <4; o++)
                    {
                        rounds_summary = rounds_summary + (all_round[i].player_combi[o]) + "   |   " ;
                    }

                    rounds_summary = rounds_summary + $"{ all_round[i].combi_bv} : blanc(s)";
                    Console.WriteLine(value: $"{rounds_summary}");
                    Console.WriteLine("-----------------------------------------------------------------------------------------");
                }

            }

            if (current_round == max_round || correct_combi == true)
            {
                current_round = 0;

                if (correct_combi == true)
                {
                    //the player has found the secret combisaison 
                    Console.WriteLine("\n\rBravo vous avez réussi à trouver la combinaison secrete !\n\r");
                    total_game_win++;
                    correct_combi = false; 
                    
                }
                else
                {
                    //End of current game 
                    Console.WriteLine($"\n\r\n\rVous n'avez plus d'essais \n\rVous aviez {max_round} essai(s)");
                    Console.WriteLine("-------------------------------------------------------------\n\r\n\r");
                }

                if (current_game < max_game)
                {
                    //start new game
                    random_secret_combi(out secret_combi);
                    start_game(ref current_game);
                }
                else
                {
                    //end game 
                    Console.WriteLine($"Vous avez joué {max_game} parties\n\rVous avez gagné {total_game_win} parties\n\r");
                    Console.WriteLine("------------------------------------------------------------------------------");
                    Console.WriteLine("Voulez vous rejouer ? ");
                    restart = Console.ReadLine();
                    restart = restart.ToUpper();

                    //restard game 
                    if (restart == "OUI")
                    {
                        //restart
                        Console.Clear();
                        Main();
                    }
                    else
                    {
                        //quit
                        Console.WriteLine("Merci d'avoir joué !");
                        System.Environment.Exit(0);
                    }
                }

            }
            //start new round
            start_round(max_round, ref current_round);
            Get_player_combi(ref player_combi);
            Get_player_combi_red(secret_combi, player_combi, ref correct_combi, ref combi_red);
            Get_player_combi_white(secret_combi, player_combi, ref combi_white);
            end_round(max_round, max_game, secret_combi, ref current_game, ref current_round, ref correct_combi, ref total_game_win, ref combi_red, ref combi_white, all_round, player_combi);
            
        }

    }
}
