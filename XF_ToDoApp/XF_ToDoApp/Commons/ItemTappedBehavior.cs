using Prism.Behaviors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace XF_ToDoApp.Commons
{
    public class ItemTappedBehavior : BehaviorBase<ListView>
    {
        public static readonly BindableProperty CommandProperty = BindableProperty.Create("Command", typeof(ICommand), typeof(EventToCommandBehavior));

        public ICommand Command
        {
            get { return (ICommand)GetValue(CommandProperty); }
            set { SetValue(CommandProperty, value); }
        }

        protected override void OnAttachedTo(ListView bindable)
        {
            base.OnAttachedTo(bindable);
            bindable.ItemSelected += (s, e) =>
            {
                //選択されていなければ実行しない
                if (e.SelectedItem != null)
                {
                    //実行するコマンドが無ければ実行しない
                    if (Command != null)
                    {
                        //選択時にイベントの発火
                        Bindable_ItemSelected(s, e);
                    }
                }
            };
        }

        protected override void OnDetachingFrom(ListView bindable)
        {
            base.OnDetachingFrom(bindable);
            bindable.ItemSelected -= Bindable_ItemSelected;
        }


        protected virtual void Bindable_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            if (Command.CanExecute(e.SelectedItem))
            {
                Command.Execute(e.SelectedItem);
            }
        }
    }
}
