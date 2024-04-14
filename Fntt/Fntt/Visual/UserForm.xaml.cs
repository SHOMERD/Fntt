using Fntt.Logics;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using Xamarin.Forms;

namespace Fntt.Visual
{
    public partial class UserForm : ContentPage
    {
        SheetsOperator sheetsOperator;
        public bool DataExsist;


        public UserForm(SheetsOperator sheetsOperator)
        {
            InitializeComponent();
            this.sheetsOperator = sheetsOperator;
            DataExsist = false;
            TrySetData();





        }

        public async Task TrySetData()
        {
            CoursePicker.Items.Clear();
            List<string> courses = await sheetsOperator.GetСourseNames();
            List<string> TeacherNames = await sheetsOperator.GetTeacherNames();
            for (int i = 0; i < courses.Count; i++)
            {
                CoursePicker.Items.Add(courses[i]);
            }
            for (int i = 0; i < TeacherNames.Count; i++)
            {
                TeacherNamePicker.Items.Add(TeacherNames[i]);
            }
            DataExsist = true;

        }

        public async Task TrySetGroup()
        {
            bool Flag = false;
            while (!Flag)
            {
                List<string> Group = await sheetsOperator.GetGrupsNames((string)CoursePicker.SelectedItem);
                if (Group == null) { Flag = false; continue; }
                for (int i = 0; i < Group.Count; i++)
                {
                    GroupPicker.Items.Add(Group[i]);
                }

                Flag = true;
            }
        }

        private void SaveData(object sender, EventArgs e)
        {
            sheetsOperator.SetUser(UserTypePicker.SelectedIndex, (string)TeacherNamePicker.SelectedItem, (string)CoursePicker.SelectedItem, (string)GroupPicker.SelectedItem);
            sheetsOperator.SetData();
            new CarouselCreater(sheetsOperator, DateTime.Now);
        }

        private void UserTypePicked(object sender, EventArgs e)
        {
            if (!DataExsist)
            {
                DisplayAlert("Данных нет", "Данных еще не получены или обрадбатываются", "Подождать");
            }
            else
            {
                if (UserTypePicker.SelectedIndex == 0)
                {
                    TeacherName.IsVisible = false;
                    Course.IsVisible = true;
                }
                else if (UserTypePicker.SelectedIndex == 1)
                {
                    Course.IsVisible = false;
                    Group.IsVisible = false;
                    TeacherName.IsVisible = true;
                }
            }
        }

        private async void CoursePicked(object sender, EventArgs e)
        {

            sheetsOperator.SetAktiveСourse((string)CoursePicker.SelectedItem);
            TrySetGroup();

            Group.IsVisible = true;
        }

        private void GroupPicked(object sender, EventArgs e)
        {
            SaveButton.IsVisible = true;
        }

        private void TeacherNamePicked(object sender, EventArgs e)
        {
            SaveButton.IsVisible = true;
        }


    }
}