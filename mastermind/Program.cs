using System;
using System.Collections.Generic;

namespace mastermind
{
    public class Program
    {
        // Enumeration de toutes les couleurs disponible
        public enum color
        {
            ROUGE,
            BLEU,
            VERT,
            JAUNE,
            VIOLET,
            ORANGE
        }

        // Structure de la derniere manche
        public struct last_round
        {
            public color[] player_combi;
            public int combi_bp;
            public int combi_bv;
            public bool past_round;
        }

        static void Main()
        {
            last_round[] all_round = new last_round[1]; // tous les rounds jouer 

            color[] secret_combi; // combinaison secrete 
            color[] player_combi = new color [4]; // combinaison du joueur

            bool correct_combi = false; // la combinaison est correcte

            int max_round = 0; // nombre maximum de manche 
            int max_game = 0; // nombre maximum de partie
            int current_round = 0; // manche actuelle
            int current_game = 0; // partie actuelle 
            int total_game_win = 0; // nombre total de victoire du joueur
            int combi_white = 0; // nombre de pion blanc du joueur
            int combi_red = 0; // nombre de pion rouge du joueur 

            // Afficher le logo MasterMind
            Console.WriteLine(",--.   ,--.                ,--.                ,--.   ,--.,--.           ,--.");
            Console.WriteLine("|   `.'   | ,--,--. ,---.,-'  '-. ,---. ,--.--.|   `.'   |`--',--,--,  ,-|  |");
            Console.WriteLine("|  |'.'|  |' ,-.  |(  .-''-.  .-'| .-. :|  .--'|  |'.'|  |,--.|      \' .-.  |");
            Console.WriteLine("|  |   |  |\\ '-'  |.-'  `) |  |  \\   --.|  |   |  |   |  ||  ||  ||  |\\ `-' |");
            Console.WriteLine("`--'   `--' `--`--'`----'  `--'   `----'`--'   `--'   `--'`--'`--''--' `---' ");
            Console.WriteLine(" ");
            Console.WriteLine("------------------------------------------------------------------------------\n\r\n\r\n\r");

            set_rules(ref max_game, ref max_round, ref all_round); // personnaliser les règles du jeu
            rules(max_game,  max_round); // afficher les règles

            random_secret_combi(out secret_combi); // choisir une combinaison aléatoire
            Show_secret_cobmbi(secret_combi); // Afficher la combinaison secrète
            
            start_game(ref current_game); // lancer la partie
            start_round(max_round, ref current_round); // lancer la manche

            Get_player_combi(ref player_combi); // récuperer la combinaison du joueur
            Get_player_combi_red(secret_combi, player_combi, ref correct_combi, ref combi_red); // récuperer les pions rouges du joueur
            Get_player_combi_white(secret_combi, player_combi, ref combi_white); // récuperer les pions blancs du joueur

            end_round(max_round, max_game, secret_combi, ref current_game, ref current_round, ref correct_combi, ref total_game_win, ref combi_red, ref combi_white, all_round, player_combi); // fin de manche 

            Console.ReadLine();
        }

        // Lancer la partie
        static void start_game(ref int current_game)
        {
            current_game = current_game + 1; // ajouter 1 au nombre de partie actuelle

            // aficher la partie actuelle 
            Console.WriteLine("----------------------------------------------------------------\n\r");
            Console.WriteLine($"--------------------------Partie(s) : {current_game}-------------------------");
            Console.WriteLine("\n\r----------------------------------------------------------------\n\r\n\r");

        }

        // Personnaliser les règles du jeu
        static void set_rules(ref int max_game, ref int max_round, ref last_round[] all_round)
        {

            const int d_max_round = 12; // nombre par défault de manche
            const int d_max_game = 3; // nombre par défault de partie

            bool correct = false; // les régles du jeu sont correctes ( par default = false )

            string custom_settings = "NON"; // Le joueur veut personnaliser les règles ( par défault = false ) 

            // Demander si le joueur veut personnaliser les règles du jeu
            Console.WriteLine("Voulez vous personnaliser les paramètres ?");
            custom_settings = Console.ReadLine();
            custom_settings = custom_settings.ToUpper();

            // Personnaliser les règles si le joueur le veut 
            if (custom_settings == "OUI")
            {
                while (correct == false)
                {
                    try
                    {
                        // Demander le nombre maximum de manches 
                        Console.WriteLine("\n\r\n\rEntrez le nombre maximum de manches pour trouver la combinaison secrete ");
                        Console.WriteLine("________________________________________________________________________\n\r");
                        max_round = Int32.Parse(Console.ReadLine());

                        // Demander le nombre maximum de parties
                        Console.WriteLine("Entrez le nombre maximum de parties à jouer");
                        Console.WriteLine("________________________________________________________________________");
                        max_game = Int32.Parse(Console.ReadLine());

                        Console.WriteLine("");
                        correct = true;
                        all_round = new last_round [max_round];
                    }
                    catch
                    {
                        // Afficher que le joueur s'est trompé en écrivant les règles personnalisées
                        Console.WriteLine("Vous avez entrer une valeur incorrecte ! \n\r\n\r");
                        continue;
                    }
                }
            }
            else
            {
                // valeur par défault 
                max_round = d_max_round;
                max_game = d_max_game;
                all_round = new last_round[max_round];
                
            }
            all_round[0].player_combi = new color[4];
        }

        // Afficher les règles
        static void rules(int max_game,  int max_round)
        {
            Console.WriteLine("__________________paramètres de la partie__________________\n\r");
            Console.WriteLine($"   Nombre de manches à remporter          : {max_round}");
            Console.WriteLine($"   Nombre de couleurS dans la combinaison : 4");
            Console.WriteLine($"   Nombre de parties à remporter          : {max_game}");
            Console.WriteLine($"   Voici toutes les couleurs disponible   : \n\r                         -ROUGE,\n\r                         -BLEU,\n\r                         -VERT,\n\r                         -JAUNE,\n\r                         -VIOLET,\n\r                         -ORANGE\n\r\n\r");
        }

        // Choisir aléatoirement une combinaison secrete 
        static void random_secret_combi(out color[] secret_combi)
        {
            secret_combi = new color[4];
            Random rnd = new Random();
            for(int i = 0; i <= 3; i++){
                color valeur  = (color)rnd.Next(0, 6); // couleur aléatoire
                secret_combi[i] = valeur; // ajout de la couleur aléatoire à la combinaison secréte 
            }
        }

        // Afficher la combinaison secrete 
        static void Show_secret_cobmbi(color[] secret_combi) 
        {
            for(int i = 0; i <= 3; i++)
            {
                Console.WriteLine(secret_combi[i]);
            }
        }

        // Récuperer la combinaison du joueur
        static void Get_player_combi(ref color[] player_combi)
        {
            for (int i = 0; i != 4; i++)
            {
                try
                {
                    // Demander la couleur au joueur 
                    Console.WriteLine($"entre la couleur numéro : {i + 1}\n\r ");
                    player_combi[i] = (color)Enum.Parse(typeof(color), Console.ReadLine());
                    Console.WriteLine("------------------------------\n\r");
                    
                }
                catch
                {
                    // Mauvaise combinaison entrée par le joueur 
                    i--;
                    Console.WriteLine("");
                    Console.WriteLine("Vous n'avez pas entrer une couleur valide \n\r Merci de rentrez une nouvelle couleur \n\r");
                    continue;
                }
            }


        }

        // Récuperer les pions rouges du joueur 
        static void Get_player_combi_red(color[] player_combi, color[] secret_combi, ref bool correct_combi, ref int combi_red)
        {
            
            for(int i = 0; i < 4; i++)
            {
                // Si la couleur du joueur actuelle du joueur est égale à celle de la combinaison secrete 
                if (player_combi[i] == secret_combi[i])
                {
                    combi_red++; // Ajouter 1 au nombre de pions rouge du joueur 
                    
                }
            }
            // Si le joueur à 4 pions rouge
            if (combi_red == 4)
            {
                correct_combi = true; // la combinaison est correcte 

            }

            // Afficher le nombre de pion rouge
            Console.WriteLine($"{combi_red} : rouge(s)");
            Console.WriteLine("-------------");

        }

        static void Get_player_combi_white(color[] player_combi, color[] secret_combi, ref int combi_white)
        {

           List<color> All_ready_in = new List<color>(); // liste des couleurs qui peuvent être pion blanc

            // ajouter toutes les couleurs dans All_ready_in
           for(int a = 0; a < 4; a++)
            {
                All_ready_in.Add(secret_combi[a]);
            } 


            for (int i = 0; i < 4; i++)
            {
                if ((player_combi[i] != secret_combi[i]) & (All_ready_in.Contains(player_combi[i])))
                {
                    combi_white++;
                    All_ready_in.Remove(player_combi[i]);
                }
                
            }
            All_ready_in.Clear();
            Console.WriteLine($"{combi_white} : blanc(s)");
            
        }

        // Commencer une manche 
        static void start_round(int max_round, ref int current_round)
        {
            current_round++; // ajouter 1 au nombre actuelle de manche

            // Afficher la manche
            Console.WriteLine($"         Manche : {current_round}         ");
            Console.WriteLine($"         {current_round}/{max_round} essai(s)");
            Console.WriteLine("___________________________________\n\r");
            Console.WriteLine("Entrez une combinaison de couleur");

        }

        // Fin de manche
        static void end_round(int max_round, int max_game, color[] secret_combi, ref int current_game, ref int current_round, ref bool correct_combi, ref int total_game_win, ref int combi_red, ref int combi_white, last_round[] all_round, color[] player_combi)
        {
            string rounds_summary = " "; // somaire de toutes les manches
            string restart = "NON"; // relancer le jeu ( par défault = False ) 

            //  ajouter les pions blancs / rouges et que le round est passé dans all_round
            all_round[current_round - 1].player_combi = new color[4];
            all_round[current_round - 1].combi_bp = combi_red;
            all_round[current_round - 1].combi_bv = combi_white;
            all_round[current_round - 1].past_round = true;

            // ajouter la combinaison du joueur all_round
            for(int i = 0; i< 4; i++)
            {
                all_round[current_round - 1].player_combi[i] = player_combi[i];
            }

            combi_red = 0; // mettre à 0 le nombre de pions rouges
            combi_white = 0; // mettre à 0 le nombre de pions blancs

            // afficher tous les derniers rounds
            Console.WriteLine("\n\r\n\rVoici vos essaies précedents : ");
            Console.WriteLine("--------------------------------------\n\r");
            for (int i = 0; i < all_round.Length; i++)
            {
                // Si la manche est passée 
                if (all_round[i].past_round == true)
                {
                    // Ajouter le numéro de la manche et le nombre de pion rouge
                    rounds_summary = $"manche : {i + 1}   | { all_round[i].combi_bp} : rouge(s) |  ";
                    for (int o = 0; o <4; o++)
                    {
                        // Ajouter toutes les couleurs du joueur pendant la manche
                        rounds_summary = rounds_summary + (all_round[i].player_combi[o]) + "   |   " ;
                    }
                    // Ajouter le nombre de pion blanc
                    rounds_summary = rounds_summary + $"{ all_round[i].combi_bv} : blanc(s)";

                    // Afficher toutes les manches précédentes 
                    Console.WriteLine(value: $"{rounds_summary}");
                    Console.WriteLine("-----------------------------------------------------------------------------------------");
                }

            }

            // Si le nombre actuelle de manche est égale au nombre maxium de manche OU que la combinaison du joueur est correcte
            if (current_round == max_round || correct_combi == true)
            {
                current_round = 0; // mettre à 0 le nombre actuelle 

                // Si la combinaison est correcte 
                if (correct_combi == true)
                {
                    // Le joueur à trouvé la combinaison secrete 
                    // Afficher que le joueur à trouvé la combinaison secrete 
                    Console.WriteLine("\n\rBravo vous avez réussi à trouver la combinaison secrete !\n\r");
                    total_game_win++; // ajouter 1 au nombre total de victoire
                    correct_combi = false; // la combinaion n'est plus correcte 
                    
                }
                else
                {
                    // Fin de la partie actuelle
                    // Afficher que le joueur n'a plus d'essaie et afficher son nombre de manche maximum
                    Console.WriteLine($"\n\r\n\rVous n'avez plus d'essais \n\rVous aviez {max_round} essai(s)");
                    // Afficher la combinaison secrete 
                    Console.WriteLine("Voici la combinaison secrete ! \n\r\n\r");
                    Show_secret_cobmbi(secret_combi);
                    Console.WriteLine("-------------------------------------------------------------\n\r\n\r");
                }

                if (current_game < max_game)
                {
                    //commencer une nouvelle partie
                    random_secret_combi(out secret_combi);
                    start_game(ref current_game);
                }
                else
                {
                    // Fin du jeu
                    // Afficher le nombre de partie que le joueur à joué et le nombre qu'il a gagné
                    Console.WriteLine($"Vous avez joué {max_game} parties\n\rVous avez gagné {total_game_win} parties\n\r");
                    Console.WriteLine("------------------------------------------------------------------------------");
                    // Demander au joueur s'il veut rejouer 
                    Console.WriteLine("Voulez vous rejouer ? ");
                    restart = Console.ReadLine();
                    restart = restart.ToUpper();

                    // Si le joueur veut rejouer le jeu
                    if (restart == "OUI")
                    {
                        // Relancer le jeu
                        Console.Clear();
                        Main();
                    }
                    else
                    {
                        // Quitter le jeu
                        Console.WriteLine("Merci d'avoir joué !");
                        System.Environment.Exit(0);
                    }
                }

            }
            // Commencer une nouvelle manche
            start_round(max_round, ref current_round);
            Get_player_combi(ref player_combi);
            Get_player_combi_red(secret_combi, player_combi, ref correct_combi, ref combi_red);
            Get_player_combi_white(secret_combi, player_combi, ref combi_white);
            end_round(max_round, max_game, secret_combi, ref current_game, ref current_round, ref correct_combi, ref total_game_win, ref combi_red, ref combi_white, all_round, player_combi);
            
        }

    }
}
