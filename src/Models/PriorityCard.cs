namespace Models
{
    public class PriorityCard : Card
    {
        public int Priority { get; set; }

        public PriorityCard(int id, string title, string description, Column column, int priority)
            : base(id, title, description, column)
        {
            Priority = priority;
        }

        public PriorityCard(string title, string description, Column column, int priority)
            : base(title, description, column)
        {
            Priority = priority;
        }

        public PriorityCard(Card card, int priority)
            : base(card.Title, card.Description, card.Column)
        {
            if (card.Id is not null)
            {
                Id = card.Id;
            }

            Priority = priority;
        }

        public override string ToString()
        {
            return $"Card {{ Id: {Id}, Title: \"{Title}\", Description: \"{Description}\", Column: {Column}, Priority: {Priority} }}";
        }
    }
}
