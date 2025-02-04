using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Xml.Linq;


namespace _4_1
{
    internal class Program
    {
        public enum CharacterClass//직업
        {
            Warrior = 1,
            Magician,
            Archer,
            Priest,
            Paladin,
            Rogue
        }

        public interface ICharacter//캐릭터 인터페이스
        {
            public string name { get; }
            public int health { get; set; }
            public float attack { get; set; }
            public int defence { get; set; }
        }

        public interface IItem//아이템 인터페이스
        {
            public string name { get; }
            public string explanation { get; }
            public int gold { get; set; }
            public bool isSold { get; set; }
            public bool isEquip { get; set; }
            public void Equip(Player player);
        }

        public class Player : ICharacter
        {
            public string name { get; }//플레이어명
            public CharacterClass characterClass { get; set; }//플레이어 직업
            public int health { get; set; }//플레이어 체력
            public float attack { get; set; }//플레이어 공격력
            public int defence { get; set; }//플레이어 방어력
            public int level { get; set; }//플레이어 레벨
            public int gold { get; set; }//소지 골드

            public int[] clear_dungeon = { 0, 1, 2, 3, 4 };//레벨업에 필요한 던전 수
            public int clear_num { get; set; }//클리어한 던전 수
            public Weapon? weapon { get; set; }//착용한 무기
            public List<Weapon> weapons { get; set; }//소지중인 무기
            public float add_attack { get; set; }//추가 공격력
            public Armor? armor { get; set; }//착용한 갑옷
            public List<Armor> armors { get; set; }//소지중인 갑옷
            public int add_defence { get; set; }//추가 방어력

            public Player(string _name)//생성자 함수
            {
                this.name = _name;
                characterClass = CharacterClass.Warrior;
                attack = 10f;
                add_attack = 0;
                defence = 5;
                add_defence = 0;
                health = 100;
                gold = 1500;
                level = 1;
                clear_num = 0;
                weapons = new List<Weapon>();
                armors = new List<Armor>();
            }
            public void BuyItem(IItem item)//아이템 구매
            {
                if (item.isSold)//아이템이 팔렸을 시
                {
                    Console.WriteLine("이미 구매한 아이템입니다.");
                    Thread.Sleep(500);
                    return;
                }
                if (gold >= item.gold)//골드가 충분할 때
                {
                    item.isSold = true;
                    Console.WriteLine("구매를 완료했습니다.");
                    gold -= item.gold;
                    if (item is Armor && item != null)//구매한 아이템이 갑옷일 때
                    {
                        armors.Add(item as Armor);//갑옷 추가
                    }
                    else if (item is Weapon && item != null)//구매한 아이템이 무기일 때
                    {
                        weapons.Add(item as Weapon);//무기 추가
                    }
                }
                else//골드가 부족할 때
                {
                    Console.WriteLine("보유 골드가 부족합니다.");
                }
                Thread.Sleep(500);
            }
            public void DungeonClear()//던전 클리어
            {
                clear_num++;
                if (level == 5)//maximum level
                {
                    return;
                }
                if (clear_dungeon[level] <= clear_num)//레벨업
                {
                    clear_num -= clear_dungeon[level];
                    level++;
                    attack += 0.5f;
                    defence++;
                }
            }

            public void PlayerDie()//플레이어 사망
            {
                Console.Clear();
                Console.WriteLine("Game Over");
                Console.WriteLine($"플레이어 {this.name}이(가) 사망했습니다.");
            }
        }

        public class Armor : IItem//갑옷 클래스
        {
            public string name { get; set; }//이름
            public string explanation { get; set; }//설명
            public int defence { get; set; }//방어력
            public int gold { get; set; }//가격
            public bool isSold { get; set; }//판매됬는지 여부
            public bool isEquip { get; set; }//착용됬는지 여부

            public void Equip(Player player)//장비 착용
            {
                if (isEquip)//착용되어있을시 착용해제
                {
                    isEquip = false;
                    player.add_defence = 0;
                    return;
                }

                if (player.armor != null && player.armor.GetType() != this.GetType())//착용된 장비가 지금 장비와 다를 시, 착용된 장비 해제
                {
                    player.armor.Equip(player);
                }
                //장비 착용
                player.armor = this;
                player.add_defence = defence;
                isEquip = true;
            }
        }

        public class OldArmor : Armor
        {
            public OldArmor()
            {
                name = "낡은 갑옷";
                explanation = "언제 만들어진지 모르는 갑옷";
                defence = 3;
                gold = 800;
                isSold = false;
                isEquip = false;
            }
        }

        public class CommonArmor : Armor
        {
            public CommonArmor()
            {
                name = "평범한 갑옷";
                explanation = "흔하게 볼수 있는 갑옷";
                defence = 8;
                gold = 2500;
                isSold = false;
                isEquip = false;
            }
        }

        public class KnightArmor : Armor
        {
            public KnightArmor()
            {
                name = "기사용 갑옷";
                explanation = "기사들을 위해 만들어진 갑옷";
                defence = 15;
                gold = 3500;
                isSold = false;
                isEquip = false;
            }
        }

        public class LegendArmor : Armor
        {
            public LegendArmor()
            {
                name = "전설 갑옷";
                explanation = "전설으로만 전해지는 갑옷";
                defence = 30;
                gold = 10000;
                isSold = false;
                isEquip = false;
            }
        }

        public class Weapon : IItem //무기 클래스
        {
            public string name { get; set; }//이름
            public string explanation { get; set; }//설명
            public float attack { get; set; }//공격력
            public int gold { get; set; }//가격
            public bool isSold { get; set; }//판매됬는지 여부
            public bool isEquip { get; set; }//착용됬는지 여부
            public void Equip(Player player)//장비 착용
            {
                if (isEquip)//착용되어있을시 착용해제
                {
                    isEquip = false;
                    player.add_attack = 0;
                    return;
                }

                if (player.weapon != null && player.weapon.GetType() != this.GetType())//착용된 장비가 지금 장비와 다를 시, 착용된 장비 해제
                {
                    player.weapon.Equip(player);
                }
                //장비 착용
                player.weapon = this;
                player.add_attack = attack;
                isEquip = true;
            }
        }

        public class WoodenSword : Weapon
        {
            public WoodenSword()
            {
                name = "목검";
                explanation = "나무로 만들어진 검";
                attack = 3;
                gold = 500;
                isSold = false;
                isEquip = false;
            }
        }

        public class CommonSword : Weapon
        {
            public CommonSword()
            {
                name = "평범한 검";
                explanation = "흔히 볼 수 있는 검";
                attack = 5;
                gold = 1500;
                isSold = false;
                isEquip = false;
            }
        }

        public class NamedSword : Weapon
        {
            public NamedSword()
            {
                name = "명검";
                explanation = "이름이 알려진 검";
                attack = 7;
                gold = 2500;
                isSold = false;
                isEquip = false;
            }
        }

        public class PracticeSpear : Weapon
        {
            public PracticeSpear()
            {
                name = "연습용 창";
                explanation = "연습용으로 알맞은 창";
                attack = 4;
                gold = 600;
                isSold = false;
                isEquip = false;
            }
        }
        public class CommonSpear : Weapon
        {
            public CommonSpear()
            {
                name = "평범한 창";
                explanation = "흔하게 볼 수 있는 창";
                attack = 5;
                gold = 2000;
                isSold = false;
                isEquip = false;
            }
        }
        public class UI
        {
            public Player player { get; set; } //플레이어
            public List<Weapon> weapons { get; set; }//무기들
            public List<Armor> armors { get; set; }//갑옷들
            public UI(Player _player, List<Weapon> _weapons, List<Armor> _armors)//생성자 함수
            {
                player = _player;
                this.weapons = _weapons;
                this.armors = _armors;
            }
            public void SetClass()//직업 선택
            {
                Console.Clear();
                Console.WriteLine("스파르타 던전에 오신 여러분 환영합니다.");
                Console.WriteLine("원하시는 직업을 입력해주세요.\n");
                int num = 1;
                foreach (var character_class in Enum.GetValues(typeof(CharacterClass)))
                {
                    Console.WriteLine($"{num++}. {character_class}");
                }
                Console.WriteLine("\n원하시는 행동을 입력해주세요.");

                while (true)
                {
                    string input = Console.ReadLine();
                    if (int.TryParse(input, out int con) && con > 0 && con < 7)//알맞은 입력이 들어올 시
                    {
                        player.characterClass = (CharacterClass)con;
                        break;
                    }
                    else//잘못된 입력이 들어올 시
                    {
                        Console.WriteLine("잘못된 입력입니다.");
                    }
                }
            }
            public void PrintStart()//시작화면
            {
                Console.Clear();
                Console.WriteLine("스타르타 마을에 오신 여러분 환영합니다.");
                Console.WriteLine("이곳에서 던전으로 들어가기전 활동을 할 수 있습니다.\n");
                Console.WriteLine("1. 상태 보기");
                Console.WriteLine("2. 인벤토리");
                Console.WriteLine("3. 상점");
                Console.WriteLine("4. 던전입장");
                Console.WriteLine("5. 휴식하기");
                Console.WriteLine();
                Console.WriteLine("원하시는 행동을 입력해주세요.");

                while (true)
                {
                    string input = Console.ReadLine();
                    if (int.TryParse(input, out int con) && con > 0 && con < 6)
                    {
                        switch (con)
                        {
                            case 1:
                                PrintPlayerInfor();//상태 보기
                                break;
                            case 2:
                                PrintInventory();//인벤토리
                                break;
                            case 3:
                                PrintStore();//상점
                                break;
                            case 4:
                                PrintDungeon();//던전 입장
                                break;
                            case 5:
                                Rest();//휴식
                                break;
                        }
                        break;
                    }
                    else
                    {
                        Console.WriteLine("잘못된 입력입니다.");
                    }
                }
            }
            public void PrintPlayerInfor()//상태 보기
            {
                string level = player.level.ToString("D2");//두자리 숫자로 출력 ex)02
                Console.Clear();
                Console.WriteLine("상태 보기");
                Console.WriteLine("캐릭터의 정보가 표시됩니다.\n");
                Console.WriteLine($"Lv. {level}");
                Console.WriteLine($"Chad ({(CharacterClass)player.characterClass})");
                Console.Write($"공격력 : {player.attack}");
                if (player.add_attack != 0)//추가 공격력이 있을 때 출력
                {
                    Console.Write($" (+{player.add_attack})");
                }
                Console.Write($"\n방어력 : {player.defence}");
                if (player.add_defence != 0)//추가 방어력이 있을 때 출력
                {
                    Console.Write($" (+{player.add_defence})");
                }
                Console.WriteLine($"\n체력 : {player.health}");
                Console.WriteLine($"Gold : {player.gold} G");
                Console.WriteLine("\n0. 나가기\n");
                Console.WriteLine("원하시는 행동을 입력해주세요");
                while (true)
                {
                    string input = Console.ReadLine();
                    if (int.TryParse(input, out int con) && con == 0)
                    {
                        break;
                    }
                    else
                    {
                        Console.WriteLine("잘못된 입력입니다.");
                    }
                }
                PrintStart();//시작화면
                return;
            }
            public void PrintInventory()//인벤토리
            {
                Console.Clear();
                Console.WriteLine("인벤토리");
                Console.WriteLine("보유 중인 아이템을 관리 할 수 있습니다.\n");
                Console.WriteLine("[아이템 목록]");
                if (weapons.Count == 0 && armors.Count == 0)//보유 중인 장비가 없을 때
                {
                    Console.WriteLine();
                }
                else//보유 중인 장비가 있을 때
                {
                    int num = 1;

                    foreach (var weapon in player.weapons)//무기 출력
                    {
                        Console.Write($"- {num++} ");
                        if (weapon.isEquip)//착용중일 때
                        {
                            Console.Write("[E]");
                        }
                        Console.WriteLine($"{weapon.name,-10}|  공격력 +{weapon.attack} | {weapon.explanation,-20}| ");

                    }


                    foreach (var armor in player.armors) // 방어구 출력
                    {
                        Console.Write($"- {num++} ");
                        if (armor.isEquip)//착용중일 때
                        {
                            Console.Write("[E]");
                        }
                        Console.WriteLine($"{armor.name,-10}|  방어력 +{armor.defence} | {armor.explanation,-20}|");

                    }

                    Console.WriteLine($"\n0.나가기\n");
                    Console.WriteLine("원하시는 행동을 입력해주세요.");
                    int tmp;
                    while (true)
                    {
                        string input = Console.ReadLine();
                        if (int.TryParse(input, out tmp) && tmp >= 0 && tmp < num)
                        {
                            break;
                        }
                        else
                        {
                            Console.WriteLine("잘못된 입력입니다.");
                        }
                    }
                    if (tmp == 0)
                    {
                        PrintStart();//시작화면
                        return;
                    }
                    else
                    {
                        if (tmp <= player.weapons.Count)
                        {
                            player.weapons[tmp - 1].Equip(player);//무기 착용
                        }
                        else
                        {
                            player.armors[tmp - player.weapons.Count - 1].Equip(player);//갑옷 착용
                        }
                        PrintInventory();//인벤토리
                        return;
                    }
                }
            }
            public void PrintStore()//상점
            {
                Console.Clear();
                Console.WriteLine("상점");
                Console.WriteLine("필요한 아이템을 얻을 수 있는 상점입니다.");
                Console.WriteLine("\n[보유골드]");
                Console.WriteLine($"{player.gold} G");
                Console.WriteLine("\n[아이템 목록]");

                foreach (var weapon in weapons)//무기 출력
                {
                    Console.Write($"- ");
                    Console.Write($"{weapon.name,-10}|  공격력 +{weapon.attack} | {weapon.explanation,-20}| ");
                    if (weapon.isSold)//무기가 팔렸을 시
                    {
                        Console.WriteLine(" 구매완료");
                    }
                    else//무기가 안 팔렸을 때
                    {
                        Console.WriteLine($" {weapon.gold} G");
                    }
                }
                foreach (var armor in armors)//갑옷 출력
                {
                    Console.Write($"- ");
                    Console.Write($"{armor.name,-10}|  방어력 +{armor.defence} | {armor.explanation,-20} |");
                    if (armor.isSold)//갑옷이 팔렸을 시
                    {
                        Console.WriteLine(" 구매완료");
                    }
                    else//갑옷이 안 팔렸을 때
                    {
                        Console.WriteLine($" {armor.gold} G");
                    }
                }
                Console.WriteLine("\n1. 아이템 구매");
                Console.WriteLine("2. 아이템 판매");
                Console.WriteLine("0. 나가기");
                Console.WriteLine("원하시는 행동을 입력해주세요.");
                int con;

                while (true)
                {
                    string input = Console.ReadLine();
                    if (int.TryParse(input, out con) && con >= 0 && con <= 2)
                    {
                        break;
                    }
                    else
                    {
                        Console.WriteLine("잘못된 입력입니다.");
                    }
                }
                if (con == 0)
                {
                    PrintStart();//시작화면
                    return;
                }
                else if (con == 1)
                {
                    PrintBuyItem();//아이템 구매
                    return;
                }
                else if (con == 2)
                {
                    PrintSellItem();//아이템 판매
                    return;
                }
            }
            public void PrintBuyItem()//아이템 구매
            {
                Console.Clear();
                Console.WriteLine("상점");
                Console.WriteLine("필요한 아이템을 얻을 수 있는 상점입니다.");
                Console.WriteLine("\n[보유골드]");
                Console.WriteLine($"{player.gold} G");
                Console.WriteLine("\n[아이템 목록]");
                int num = 1;
                foreach (var weapon in weapons)//무기 출력
                {
                    Console.Write($"- {num++} ");
                    Console.Write($"{weapon.name,-10}|  공격력 +{weapon.attack} | {weapon.explanation,-20}| ");
                    if (weapon.isSold)//무기가 팔렸을 시
                    {
                        Console.WriteLine(" 구매완료");
                    }
                    else//무기가 안 팔렸을 시
                    {
                        Console.WriteLine($" {weapon.gold} G");
                    }
                }
                foreach (var armor in armors)//갑옷 출력
                {
                    Console.Write($"- {num++} ");
                    Console.Write($"{armor.name,-10}|  방어력 +{armor.defence} | {armor.explanation,-20} |");

                    if (armor.isSold)//갑옷이 팔렸을 시
                    {
                        Console.WriteLine(" 구매완료");
                    }
                    else//갑옷 안 팔렸을 시
                    {
                        Console.WriteLine($" {armor.gold} G");
                    }
                }
                Console.WriteLine("\n0. 나가기\n");
                Console.WriteLine("원하시는 행동을 입력해주세요.");
                int con;

                while (true)
                {
                    string input = Console.ReadLine();
                    if (int.TryParse(input, out con) && con >= 0 && con < num)
                    {
                        break;
                    }
                    else
                    {
                        Console.WriteLine("잘못된 입력입니다.");
                    }
                }
                if (con == 0)
                {
                    PrintStore();//상점
                    return;
                }

                if (con <= weapons.Count)
                {
                    player.BuyItem(weapons[con - 1]);//무기 구매
                }
                else
                {
                    player.BuyItem(armors[con - weapons.Count - 1]);//갑옷 구매
                }
                PrintBuyItem();//아이템 구매
                return;
            }
            public void PrintSellItem()//아이템 판매
            {
                Console.Clear();
                Console.WriteLine("상점 - 아이템 판매");
                Console.WriteLine("필요한 아이템을 얻을 수 있는 상점입니다.\n");
                Console.WriteLine("[보유 골드]");
                Console.WriteLine($"{player.gold} G");
                Console.WriteLine("\n[아이템 목록]");
                if (weapons.Count == 0 && armors.Count == 0)
                {
                    Console.WriteLine();
                }
                else
                {
                    int num = 1;

                    foreach (var weapon in player.weapons)//보유중인 무기 출력
                    {
                        Console.Write($"- {num++} ");
                        if (weapon.isEquip)//착용중일 때
                        {
                            Console.Write("[E]");
                        }
                        Console.WriteLine($"{weapon.name,-10}|  공격력 +{weapon.attack} | {weapon.explanation,-20}| {(weapon.gold) / 100 * 85}");//판매가격 출력

                    }


                    foreach (var armor in player.armors)//보유중인 갑옷 출력
                    {
                        Console.Write($"- {num++} ");
                        if (armor.isEquip)//착용중일 때
                        {
                            Console.Write("[E]");
                        }
                        Console.WriteLine($"{armor.name,-10}|  공격력 +{armor.defence} | {armor.explanation,-20}| {(armor.gold) / 100 * 85}");//판매가격 출력

                    }
                    Console.WriteLine($"\n0.나가기\n");
                    Console.WriteLine("원하시는 행동을 입력해주세요.");
                    int tmp;

                    while (true)
                    {
                        string input = Console.ReadLine();
                        if (int.TryParse(input, out tmp) && tmp >= 0 && tmp < num)
                        {
                            break;
                        }
                        else
                        {
                            Console.WriteLine("잘못된 입력입니다.");
                        }
                    }
                    if (tmp == 0)
                    {
                        PrintStore();//상점
                        return;
                    }
                    else
                    {
                        if (tmp < weapons.Count)
                        {
                            player.weapons[tmp - 1].isEquip = false;//착용 해제
                            player.weapons[tmp - 1].isSold = false;//재구매 가능
                            player.gold += weapons[tmp - 1].gold / 100 * 85;//판매가
                            player.weapons.RemoveAt(tmp - 1);//보유중인 무기 제거
                        }
                        else
                        {
                            player.armors[tmp - weapons.Count - 1].isEquip = false;//착용 해제
                            player.armors[tmp - weapons.Count - 1].isSold = false;//재구매 가능
                            player.gold += weapons[tmp - weapons.Count - 1].gold / 100 * 85;//판매가
                            player.armors.RemoveAt(tmp - weapons.Count - 1);//보유중인 갑옷 제거
                        }
                        PrintSellItem();
                        return;
                    }
                }
            }
            public void PrintDungeon()//던전 입장
            {
                Console.Clear();
                Console.WriteLine("던전입장");
                Console.WriteLine("이곳에서 던전으로 들어가기전 활동을 할 수 있습니다.\n");
                Console.WriteLine("1. 쉬운 던전     | 방어력 5 이상 권장");
                Console.WriteLine("2. 일반 던전     | 방어력 11 이상 권장");
                Console.WriteLine("3. 어려운 던전   | 방어력 17 이상 권장");
                Console.WriteLine("0. 나가기\n");
                Console.WriteLine("\n원하시는 행동을 입력해주세요.");
                int con;

                while (true)
                {
                    string input = Console.ReadLine();
                    if (int.TryParse(input, out con) && con >= 0 && con <= 3)
                    {
                        break;
                    }
                    else
                    {
                        Console.WriteLine("잘못된 입력입니다.");
                    }
                }
                if (con == 0)
                {
                    PrintStart();//시작화면
                    return;
                }
                else
                {
                    DungeonEnter(con);//던전 도전
                    return;
                }
            }
            public void DungeonEnter(int con)//던전 도전
            {
                int clear_gold = 0; // 클리어 골드
                int wanted_defence = 0;//권장 방어력
                string dungeon_name = "";//던전 이름
                switch (con)
                {
                    case 1:
                        clear_gold = 1000;
                        wanted_defence = 5;
                        dungeon_name = "쉬운 던전";
                        break;
                    case 2:
                        clear_gold = 1700;
                        wanted_defence = 11;
                        dungeon_name = "일반 던전";
                        break;
                    case 3:
                        clear_gold = 2500;
                        wanted_defence = 17;
                        dungeon_name = "어려운 던전";
                        break;
                }
                int player_defence = player.defence + player.add_defence;//플레이어 최종 방어력
                bool isClear = true;//클리어 여부
                if (player_defence < wanted_defence)//권장 방어력보다 낮다면
                {
                    int clear_rate = new Random().Next(1, 11);
                    if (clear_rate <= 4)//40프로 확률로 실패
                    {
                        isClear = false;
                    }


                }
                if (isClear == false)//클리어 실패
                {
                    Console.Clear();
                    Console.WriteLine("던전 클리어 실패");
                    Console.WriteLine("체력이 50 감소합니다.");
                    player.health -= 50;//체력감소
                    if (player.health <= 0)//플레이어 사망
                    {
                        player.PlayerDie();
                        return;
                    }
                    Thread.Sleep(1000);
                    PrintDungeon();//던전 입장
                }
                else//클리어 성공
                {
                    float player_attack = player.attack + player.add_attack;//최종 공격력
                    float step = 1f;//추가 보상 단위 1%
                    int health_decrese = new Random().Next(20, 36) - player_defence + wanted_defence;//감소될 체력
                    double random = new Random().NextDouble();
                    int rate = (int)Math.Round(random * player_attack + player_attack);//추가 보상 비율
                    clear_gold += clear_gold * rate / 100;//클리어 골드
                    if (player.health - health_decrese <= 0)//플레이어 사망
                    {
                        player.PlayerDie();
                        return;
                    }
                    else
                    {
                        Console.Clear();
                        Console.WriteLine("던전 클리어");
                        Console.WriteLine("축하합니다!");
                        Console.WriteLine($"{dungeon_name}을 클리어 하였습니다.\n");
                        Console.WriteLine("[탐험 결과]");
                        Console.WriteLine($"체력 {player.health} -> {player.health -= health_decrese}");//체력 변화 출력
                        Console.WriteLine($"Gold {player.gold} -> {player.gold += clear_gold}");//소지 골드 변화 출력
                        Console.WriteLine("\n0. 나가기\n");
                        Console.WriteLine("원하시는 행동을 입력해주세요.");
                        player.DungeonClear();//던전클리어함수 실행
                        int tmp;
                        while (true)
                        {
                            string input = Console.ReadLine();
                            if (int.TryParse(input, out tmp) && tmp == 0)
                            {
                                break;
                            }
                            else
                            {
                                Console.WriteLine("잘못된 입력입니다.");
                            }
                        }
                        if (tmp == 0)
                        {
                            PrintDungeon();//던전입장
                            return;
                        }
                    }
                }
            }
            public void Rest()//휴식하기
            {
                Console.Clear();
                Console.WriteLine("휴식하기");
                Console.WriteLine($"500 G 를 내면 체력을 회복할 수 있습니다. (보유 골드 : {player.gold} G)");
                Console.WriteLine("\n1. 휴식하기");
                Console.WriteLine("0. 나가기");
                Console.WriteLine("\n원하시는 행동을 입력해주세요.");
                int con;

                while (true)
                {
                    string input = Console.ReadLine();
                    if (int.TryParse(input, out con) && con >= 0 && con <= 1)
                    {
                        break;
                    }
                    else
                    {
                        Console.WriteLine("잘못된 입력입니다.");
                    }
                }
                if (con == 0)
                {
                    PrintStart();//시작화면
                    return;
                }
                if (con == 1)
                {
                    if (player.gold >= 500)//골드가 충분할 때
                    {
                        Console.WriteLine("휴식을 완료했습니다.");
                        player.gold -= 500;//소지 골드 감소
                        player.health = 100;//체력 회복
                    }
                    else//골드 부족
                    {
                        Console.WriteLine("Gold가 부족합니다.");

                    }
                    Thread.Sleep(1000);
                    PrintStart();//시작화면
                    return;
                }

            }
        }


        static void Main(string[] args)
        {
            string name = "";
            bool isFlag = true;

            List<Weapon> weapons = new List<Weapon>()//무기 종류
            {
                new PracticeSpear(), new CommonSpear(), new WoodenSword(), new CommonSword(), new NamedSword()
            };
            List<Armor> armors = new List<Armor>()//갑옷 종류
            {
                new OldArmor(), new CommonArmor(), new LegendArmor()
            };

            Console.WriteLine("스파르타 던전에 오신 여러분 환영합니다.");
            while (isFlag)//이름 입력
            {
                Console.WriteLine("원하시는 이름을 설정해주세요.\n");
                name = Console.ReadLine();
                Console.WriteLine($"입력하신 이름은 {name}입니다.\n");
                Console.WriteLine("1.저장");
                Console.WriteLine("2.취소\n");
                Console.WriteLine("원하시는 행동을 입력해주세요.");
                while (true)
                {
                    string input = Console.ReadLine();
                    if (int.TryParse(input, out int con) && con == 1)
                    {
                        isFlag = false;
                        break;
                    }
                    else
                    {
                        Console.WriteLine("잘못된 입력입니다.");
                    }
                }
            }
            Player player = new Player(name);//플레이어 생성
            UI ui = new UI(player, weapons, armors);
            ui.SetClass();//직업 선택
            ui.PrintStart();//시작화면
        }
    }
}
