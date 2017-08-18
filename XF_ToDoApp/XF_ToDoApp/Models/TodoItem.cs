using Realms;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XF_ToDoApp.Models
{
    public class TodoItem : RealmObject, INotifyPropertyChanged
    {
        public string Id { get; set; }

        public DateTimeOffset InDate { get; set; }

        public String Todo { get; set; }
    }
}
