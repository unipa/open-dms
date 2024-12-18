namespace A3Synch.Models
{
        public class StrutturaInput
        {
            public Results results { get; set; }
        }

        public class Results
        {
            public Units units { get; set; }
        }

        public class Units
    {
            public int current_page { get; set; }
            public int last_page { get; set; }
            public string next_page_url { get; set; }
            public List<Struttura> data { get; set; }
        }
}
