using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using XF_ToDoApp.Models;
using Realms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;

namespace XF_ToDoApp.ViewModels
{
    public class SecondPageViewModel : BindableBase, INavigatedAware
    {
        private string _id;
        private readonly INavigationService _navigationService;
        public ICommand TodoCreationCommand { get; }
        public ICommand CancelTaskCommand { get; }
        public SecondPageViewModel(INavigationService navigationService)
        {
            _navigationService = navigationService;

            TodoCreationCommand = new DelegateCommand(() =>
            {
                System.Diagnostics.Debug.WriteLine("TodoCreationCommand");

                using (var realm = Realm.GetInstance())
                {
                    if (string.IsNullOrEmpty(_id))
                    {
                        realm.Write(() =>
                        {
                            var addTask = realm.CreateObject("TodoItem");
                            addTask.Id = Guid.NewGuid().ToString();
                            System.Diagnostics.Debug.WriteLine(Guid.NewGuid().ToString());
                            addTask.InDate = DateTime.SpecifyKind(InDate, DateTimeKind.Utc);
                            addTask.Todo = Todo;
                            realm.Add(addTask);
                        });
                    }
                    else
                    {
                        var updateTask = realm.All<TodoItem>().Where(s => s.Id == _id).First();
                        realm.Write(() =>
                        {
                            updateTask.InDate = DateTime.SpecifyKind(InDate, DateTimeKind.Utc);
                            updateTask.Todo = Todo;
                        });
                    }
                }

                _navigationService.GoBackAsync();
            });

            CancelTaskCommand = new DelegateCommand(() =>
            {
                System.Diagnostics.Debug.WriteLine("CancelTaskCommand");
            });
        }

        public void OnNavigatedFrom(NavigationParameters parameters)
        {

        }

        public void OnNavigatedTo(NavigationParameters parameters)
        {
            if (parameters.ContainsKey("id"))
            {
                _id = (string)parameters["id"];
            }

            if (string.IsNullOrEmpty(_id))
            {
                this.InDate = DateTime.Today;
            }
            else
            {
                Realm _realm = Realm.GetInstance();
                TodoItem item = _realm.All<TodoItem>().Where(s => s.Id == _id).First();
                this.InDate = DateTime.SpecifyKind(item.InDate.DateTime, DateTimeKind.Utc);
                this.Todo = item.Todo;
            }
        }

        private DateTime _inDate;
        public DateTime InDate
        {
            get
            {
                return this._inDate;
            }
            set
            {
                this.SetProperty(ref this._inDate, value);
            }
        }

        private string _todo;
        public string Todo
        {
            get
            {
                return this._todo;
            }
            set
            {
                this.SetProperty(ref this._todo, value);
            }
        }
    }
}
