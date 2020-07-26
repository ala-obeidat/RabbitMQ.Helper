using System; 

namespace RabbitCaller
{
    [Serializable]
    public class PersonModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}
