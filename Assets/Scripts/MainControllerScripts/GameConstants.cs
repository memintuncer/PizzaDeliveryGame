using UnityEngine;

public class GameConstants
{
    
    public struct LEVEL_EVENTS
    {
       
        public static string OBJECTIVE_FAILED = "OBJECTIVE_FAILED";
        public static string LEVEL_FAILED = "LEVEL_FAILED";
        public static string LEVEL_SUCCESSED = "LEVEL_SUCCESSED";
        public static string LEVEL_STARTED = "LEVEL_STARTED";
        
        public static string LevelFinished = "LevelFinished";

        public static string REACHED_FINISH = "REACHED_FINISH";
    }

   
    public struct GameEvents
    {
        public static string SEND_PIZZABOX_TO_PLAYER = "SEND_PIZZABOX_TO_PLAYER";
        public static string GAME_STARTED = "GAME_STARTED";
        public static string HITTED_TO_PIZZABOX = "HITTED_TO_PIZZABOX";
        public static string SWIPE_LEFT = "SWIPE_LEFT";
        public static string SWIPE_RIGHT = "SWIPE_RIGHT";
        public static string PIZZA_BOX_COLLECTED = "PIZZA_BOX_COLLECTED";
        public static string FINISHED_PIZZA_BOXES = "FINISHED_PIZZA_BOXES";
        public static string STICK_DANGER_TIMEOUT = "STICK_DANGER_TIMEOUT";

    }


}