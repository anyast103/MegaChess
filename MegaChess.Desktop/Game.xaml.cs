﻿using MegaChess.Logic;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Media;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;

namespace MegaChess.Desktop
{
    public partial class Game : Window
    {
        public Game()
        {
            new IDrawer(); // Добавляем конструктор в Code Behind, чтобы дальше работать с доской.
            Cell.isFinished = true;
            DirectoryInfo rateInfo = new DirectoryInfo("Rating");
            foreach(var rates in rateInfo.GetFiles())
            {
                if (rates.Name.Trim(rates.Extension.ToCharArray()) == $"{FirstPlayer.Name} & {SecondPlayer.Name}")
                {
                    string[] rate = File.ReadAllLines($"Rating/{FirstPlayer.Name} & {SecondPlayer.Name} rating.txt");
                    FirstPlayer.Wins = int.Parse(rate[1]);
                    SecondPlayer.Wins = int.Parse(rate[3]);
                }
                else
                {
                    File.WriteAllLines($"Rating/{FirstPlayer.Name} & {SecondPlayer.Name} rating.txt", new string[] { FirstPlayer.Name, 0.ToString(), SecondPlayer.Name, 0.ToString() });
                    FirstPlayer.Wins = 0;
                    SecondPlayer.Wins = 0;
                }
            }
            InitializeComponent();
        }
        
        private void Field_SizeChanged(object sender, SizeChangedEventArgs e) // Создаём обработчик события для Canvas, чтобы отрисовать доску.
        {
            SoundPlayer sp = new SoundPlayer
            {
                SoundLocation = "fon-sound.wav"
            };
            sp.Load();
            sp.PlayLooping();

            field.Children.Clear();

            for(int i = 0; i < 8; i ++)
            {
                for(int j = 0; j < 8; j++)
                {
                    field.Children.Add(IDrawer.Board[i, j].Square); // Дoбавляем на Canvas элементы массива с клетками (уже закрашенными и с установленным размером для каждой).

                    Canvas.SetTop(IDrawer.Board[i, j].Square, 75 * i); // Устанавливаем позицию на Canvas.
                    Canvas.SetLeft(IDrawer.Board[i, j].Square, 75 * j);
                }
            }
        }
        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
                var save = JsonConvert.SerializeObject(Placement.field, Formatting.Indented);
                File.WriteAllText($"Saves/{FirstPlayer.Name} and {SecondPlayer.Name}.txt", save);
                IDrawer.WhiteOrBlack = true;
                MainWindow main = new MainWindow();
                main.Show();
            
        }
    }
}