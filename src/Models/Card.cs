namespace Models
{
    public class Card
    {
        public int? Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public Column Column { get; set; }

        public Card(int id, string title, string description, Column column)
        {
            Id = id;
            Title = title;
            Description = description;
            Column = column;
        }

        public Card(string title, string description, Column column)
        {
            Id = null;
            Title = title;
            Description = description;
            Column = column;
        }

        public override string ToString()
        {
            return $"Card {{ Id: {Id}, Title: \"{Title}\", Description: \"{Description}\", Column: {Column} }}";
        }
    }
}

