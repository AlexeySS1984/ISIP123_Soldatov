using System;

namespace RoguelikeGame
{
    class Program
    {
        static void Main(string[] args)
        {
            Game game = new Game();
            game.Run();
        }
    }

    class Game
    {
        private Player player;
        private Random rnd;
        private int turn;

        // Базовые характеристики врагов
        private const int BaseGoblinHP = 20;
        private const int BaseGoblinAttack = 8;
        private const int BaseGoblinDefense = 2;
        private const double BaseGoblinCritChance = 0.2;

        private const int BaseSkeletonHP = 30;
        private const int BaseSkeletonAttack = 10;
        private const int BaseSkeletonDefense = 3;

        private const int BaseMageHP = 10;
        private const int BaseMageAttack = 12;
        private const int BaseMageDefense = 0;
        private const double BaseMageFreezeChance = 0.25;

        public Game()
        {
            rnd = new Random();
            player = new Player(1000, new Weapon("Ржавый меч", 8), new Armor("Кожаная броня", 6));
            turn = 1;
        }

        public void Run()
        {
            Console.WriteLine("Добро пожаловать в текстовую игру-рогалик!");
            while (player.HP > 0)
            {
                Console.WriteLine($"\nХод {turn}. Ваше здоровье: {player.HP}/{player.MaxHP}");
                if (turn % 10 == 0)
                {
                    Enemy boss = GetRandomBoss();
                    Console.WriteLine($"Вы встретили босса: {boss.Name}!");
                    Fight(boss);
                }
                else
                {
                    if (rnd.Next(2) == 0)
                    {
                        HandleChest();
                    }
                    else
                    {
                        Enemy enemy = GetRandomEnemy();
                        Console.WriteLine($"Вы встретили врага: {enemy.Name}!");
                        Fight(enemy);
                    }
                }
                turn++;
            }
            Console.WriteLine("Вы погибли. Игра окончена.");
        }

        private Enemy GetRandomEnemy()
        {
            int type = rnd.Next(3);
            switch (type)
            {
                case 0:
                    return new Enemy("Гоблин", BaseGoblinHP, BaseGoblinAttack, BaseGoblinDefense, false, BaseGoblinCritChance, 0.0);
                case 1:
                    return new Enemy("Скелет", BaseSkeletonHP, BaseSkeletonAttack, BaseSkeletonDefense, true, 0.0, 0.0);
                case 2:
                    return new Enemy("Маг", BaseMageHP, BaseMageAttack, BaseMageDefense, false, 0.0, BaseMageFreezeChance);
                default:
                    throw new Exception("Неверный тип врага");
            }
        }

        private Enemy GetRandomBoss()
        {
            int type = rnd.Next(4);
            switch (type)
            {
                case 0: // ВВГ (Гоблин)
                    return new Enemy("ВВГ", (int)(BaseGoblinHP * 2.0), (int)(BaseGoblinAttack * 1.5), (int)(BaseGoblinDefense * 1.2), false, BaseGoblinCritChance + 0.1, 0.0);
                case 1: // Ковальский (Скелет)
                    return new Enemy("Ковальский", (int)(BaseSkeletonHP * 2.5), (int)(BaseSkeletonAttack * 1.3), (int)(BaseSkeletonDefense * 1.4), true, 0.0, 0.0);
                case 2: // Архимаг C++ (Маг)
                    return new Enemy("Архимаг C++", (int)(BaseMageHP * 1.8), (int)(BaseMageAttack * 1.6), (int)(BaseMageDefense * 1.1), false, 0.0, BaseMageFreezeChance + 0.1);
                case 3: // Пестов C-- (Скелет с заморозкой)
                    return new Enemy("Пестов C--", (int)(BaseSkeletonHP * 1.3), (int)(BaseSkeletonAttack * 1.8), (int)(BaseSkeletonDefense * 0.6), true, 0.0, BaseMageFreezeChance + 0.15);
                default:
                    throw new Exception("Неверный тип босса");
            }
        }

        private void HandleChest()
        {
            Console.WriteLine("Вы нашли сундук!");
            int itemType = rnd.Next(3);
            if (itemType == 0)
            {
                Console.WriteLine("Вы нашли лечебное зелье!");
                player.HP = player.MaxHP;
                Console.WriteLine("Ваше здоровье полностью восстановлено.");
            }
            else if (itemType == 1)
            {
                int newAttack = rnd.Next(5, 21);
                Weapon newWeapon = new Weapon($"Меч силы {newAttack}", newAttack);
                Console.WriteLine($"Вы нашли оружие: {newWeapon.Name} (Атака: {newWeapon.Attack})");
                Console.WriteLine($"Текущее оружие: {player.Weapon.Name} (Атака: {player.Weapon.Attack})");
                Console.Write("Хотите экипировать новое оружие? (y/n): ");
                string choice = Console.ReadLine().ToLower();
                if (choice == "y")
                {
                    player.Weapon = newWeapon;
                    Console.WriteLine("Оружие экипировано.");
                }
                else
                {
                    Console.WriteLine("Оружие выброшено.");
                }
            }
            else
            {
                int newDefense = rnd.Next(3, 16);
                Armor newArmor = new Armor($"Броня защиты {newDefense}", newDefense);
                Console.WriteLine($"Вы нашли броню: {newArmor.Name} (Защита: {newArmor.Defense})");
                Console.WriteLine($"Текущая броня: {player.Armor.Name} (Защита: {player.Armor.Defense})");
                Console.Write("Хотите экипировать новую броню? (y/n): ");
                string choice = Console.ReadLine().ToLower();
                if (choice == "y")
                {
                    player.Armor = newArmor;
                    Console.WriteLine("Броня экипирована.");
                }
                else
                {
                    Console.WriteLine("Броня выброшена.");
                }
            }
        }

        private void Fight(Enemy enemy)
        {
            Console.WriteLine($"Бой начинается! Здоровье врага: {enemy.HP}");
            bool defending = false;
            while (player.HP > 0 && enemy.HP > 0)
            {
                if (player.IsFrozen)
                {
                    Console.WriteLine("Вы заморожены и пропускаете ход!");
                    player.IsFrozen = false;
                }
                else
                {
                    Console.Write("Выберите действие: (a) Атака или (d) Защита: ");
                    string choice = Console.ReadLine().ToLower();
                    if (choice == "a")
                    {
                        int damage = Math.Max(0, player.Weapon.Attack - enemy.Defense); // Локальная переменная
                        enemy.HP -= damage;
                        Console.WriteLine($"Вы атакуете, нанося {damage} урона. Здоровье врага: {enemy.HP}");
                    }
                    else if (choice == "d")
                    {
                        defending = true;
                        Console.WriteLine("Вы защищаетесь.");
                    }
                    else
                    {
                        Console.WriteLine("Неверный выбор. Вы ничего не делаете.");
                    }
                }

                if (enemy.HP <= 0)
                {
                    Console.WriteLine("Враг повержен!");
                    break;
                }

                // Атака врага
                int enemyDamage = enemy.GetDamage(rnd); // Используем другое имя для ясности
                if (defending)
                {
                    defending = false;
                    if (rnd.Next(100) < 40)
                    {
                        enemyDamage = 0; // Переопределяем damage как 0 при уклонении
                        Console.WriteLine("Вы уклонились от атаки!");
                    }
                    else
                    {
                        double blockPercent = rnd.Next(70, 101) / 100.0;
                        int block = (int)(blockPercent * player.Armor.Defense);
                        if (!enemy.IgnoreDefense)
                        {
                            enemyDamage = Math.Max(0, enemyDamage - block);
                        }
                        Console.WriteLine($"Вы заблокировали часть урона.");
                    }
                }

                if (enemyDamage > 0)
                {
                    player.HP -= enemyDamage;
                    Console.WriteLine($"Враг атакует, нанося {enemyDamage} урона. Ваше здоровье: {player.HP}");
                }

                // Применение заморозки, если есть
                if (enemy.FreezeChance > 0 && rnd.NextDouble() < enemy.FreezeChance)
                {
                    player.IsFrozen = true;
                    Console.WriteLine("Вы заморожены!");
                }
            }

            if (player.HP <= 0)
            {
                Console.WriteLine("Вы были побеждены.");
            }
        }
    }

    class Player
    {
        public int HP { get; set; }
        public int MaxHP { get; set; }
        public Weapon Weapon { get; set; }
        public Armor Armor { get; set; }
        public bool IsFrozen { get; set; }

        public Player(int maxHP, Weapon weapon, Armor armor)
        {
            MaxHP = maxHP;
            HP = maxHP;
            Weapon = weapon;
            Armor = armor;
            IsFrozen = false;
        }
    }

    class Weapon
    {
        public string Name { get; }
        public int Attack { get; }

        public Weapon(string name, int attack)
        {
            Name = name;
            Attack = attack;
        }
    }

    class Armor
    {
        public string Name { get; }
        public int Defense { get; }

        public Armor(string name, int defense)
        {
            Name = name;
            Defense = defense;
        }
    }

    class Enemy
    {
        public string Name { get; }
        public int HP { get; set; }
        public int Attack { get; }
        public int Defense { get; }
        public bool IgnoreDefense { get; }
        public double CritChance { get; }
        public double FreezeChance { get; }

        public Enemy(string name, int hp, int attack, int defense, bool ignoreDefense, double critChance, double freezeChance)
        {
            Name = name;
            HP = hp;
            Attack = attack;
            Defense = defense;
            IgnoreDefense = ignoreDefense;
            CritChance = critChance;
            FreezeChance = freezeChance;
        }

        public int GetDamage(Random rnd)
        {
            int damage = Attack;
            if (CritChance > 0 && rnd.NextDouble() < CritChance)
            {
                damage *= 2;
                Console.WriteLine("Критический удар!");
            }
            return damage;
        }
    }
}