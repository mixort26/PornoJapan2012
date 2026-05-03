using UnityEngine;

public static class GameData
{
    //Ингредиенты
    public static int SizeOfMenuIngredients = 1;
    public static int[] Ingredients = new int[4]{25,0,0,0};
    
    //Бюджет
    public static int Money = 0;
    
    //Уровень кафе
    public static int Level = 0;

    //Меню
    public static int SizeOfMenu = 1;
    public static int[] Coffees = new int[5];
    public static readonly int[] CostCoffees = new int[] { 2, 4, 5, 7, 10 };

    public static readonly int[] CostLevelUp = new int[] { 50, 70, 100 };
    
    public static void LevelUp() {
        if (Level >= CostLevelUp.Length) return;
        switch (Level) {
            case 0 when Money >= CostLevelUp[Level]:
                Money -= CostLevelUp[Level];
                Level++;
                SizeOfMenu = 3;
                SizeOfMenuIngredients = 2;
                break;
            case 1 when Money >= CostLevelUp[Level]:
                Money -= CostLevelUp[Level];
                Level++;
                SizeOfMenu = 4;
                SizeOfMenuIngredients = 3;
                break;
            case 2 when Money >= CostLevelUp[Level]:
                Money -= CostLevelUp[Level];
                Level++;
                SizeOfMenu = 5;
                SizeOfMenuIngredients = 4;
                break;
        }
    }

    //Время
    public static int Minutes = 480;
    
    //Ночь
    public static int Score;
}