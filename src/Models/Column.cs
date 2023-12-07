namespace Models
{
    public class Column
    {
        public string Title { get; set; }

        public Column(string title)
        {
            Title = title;
        }

        public override string ToString()
        {
            return $"Column {{ Title: \"{Title}\" }}";
        }
    }
}
