public static class GameData
{
    //Ингредиенты
    public static int SizeOfMenuIngredients = 1;
    public static int[] Ingredients = new int[4];

    //Бюджет
    public static int Money = 0;

    //Уровень кафе
    public static int Level = 1;

    //Меню
    public static int SizeOfMenu = 1;
    public static int[] Coffees = new int[5];

public static void LevelUp2() {
        Level++;
        SizeOfMenu = 3;
        SizeOfMenuIngredients = 2;
    }

    public static void LevelUp3() {
        Level++;
        SizeOfMenu = 4;
        SizeOfMenuIngredients = 3;
    }
    
    public static void LevelUp4() {
        Level++;
        SizeOfMenu = 5;
        SizeOfMenuIngredients = 4;
    }
    
    //Время
    public static int Minutes = 480;
    
}
