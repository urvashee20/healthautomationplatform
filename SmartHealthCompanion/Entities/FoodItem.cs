namespace SmartHealthCompanion.Entities
{
    public class FoodItem
    {
        public long Id { get; set; }
        public string Name { get; set; } // Poha, Roti, Rice
        public int Calories { get; set; }
        public string Category { get; set; } // Breakfast/Lunch/Dinner
    }
}
