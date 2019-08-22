using System.Collections.Generic;

namespace Plugins.TestPlugin2.Models
{
    public class SampleModel
    {
        public int Id { get; set; }
        public string Title { get; set; }
    }

    public class SampleModelList : List<SampleModel>
    {
        public SampleModelList()
        {
            Add(new SampleModel() { Id = 1, Title = "Title 1" });
            Add(new SampleModel() { Id = 2, Title = "Title 2" });
            Add(new SampleModel() { Id = 3, Title = "Title 3" });
            Add(new SampleModel() { Id = 4, Title = "Title 4" });
            Add(new SampleModel() { Id = 5, Title = "Title 5" });
        }
    }

}