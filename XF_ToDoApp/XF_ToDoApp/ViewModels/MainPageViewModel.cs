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
    public class MainPageViewModel : BindableBase, INavigationAware
    {
        private string _title;
        public string Title
        {
            get { return _title; }
            set { SetProperty(ref _title, value); }
        }

        private readonly INavigationService _navigationService;
        public ICommand AddTaskCommand { get; }
        public ICommand ItemTappedCommand { get; }
        public ICommand DeleteTodoCommand { get; }
        public MainPageViewModel(INavigationService navigationService)
        {
            _navigationService = navigationService;

            this.SelectAll();

            AddTaskCommand = new DelegateCommand(() =>
            {
                System.Diagnostics.Debug.WriteLine("AddTaskCommand");

                var navigationParameters = new NavigationParameters();
                navigationParameters.Add("id", "");
                _navigationService.NavigateAsync("SecondPage");
            });

            ItemTappedCommand = new DelegateCommand<object>((param) =>
            {
                System.Diagnostics.Debug.WriteLine("ItemTappedCommand");

                TodoItem todoItem = (TodoItem)param;
                var navigationParameters = new NavigationParameters();
                navigationParameters.Add("id", todoItem.Id);
                _navigationService.NavigateAsync("SecondPage", navigationParameters);
            });

            DeleteTodoCommand = new DelegateCommand<object>((x) =>
            {
                System.Diagnostics.Debug.WriteLine("DeleteTodoCommand");

                TodoItem delItem = (TodoItem)x;

                var realm = Realm.GetInstance();
                using (var trans = realm.BeginWrite())
                {
                    realm.Remove(delItem);
                    trans.Commit();
                }
            });
        }

        public void OnNavigatedFrom(NavigationParameters parameters)
        {

        }

        public void OnNavigatingTo(NavigationParameters parameters)
        {

        }

        public void OnNavigatedTo(NavigationParameters parameters)
        {
            if (parameters.ContainsKey("title"))
                Title = (string)parameters["title"] + " and Prism";
        }

        private void SelectAll()
        {
            Realm _realm = Realm.GetInstance();
            this.TodoItemsData = _realm.All<TodoItem>();
        }

        private IEnumerable<TodoItem> _todoItemsData;
        public IEnumerable<TodoItem> TodoItemsData
        {
            get
            {
                return this._todoItemsData;
            }
            set
            {
                this.SetProperty(ref this._todoItemsData, value);
            }
        }
    }
}
