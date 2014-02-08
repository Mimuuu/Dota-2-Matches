using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.ComponentModel;
using System.Windows.Data;
using System.Net.NetworkInformation;
using Microsoft.Phone.Controls;
using Newtonsoft.Json;

namespace Dota_2_Matches
{
    public partial class MainPage : PhoneApplicationPage
    {
        RootObject matches = new RootObject();
        List<Upcoming> upMatches = new List<Upcoming>();
        List<Ended> endMatches = new List<Ended>();
        WebClient webClient = new WebClient();

        // Constructor
        public MainPage()
        {
            InitializeComponent();
            this.loadJSON();
        }

        // Load the JSON data
        private void loadJSON()
        {
            //webClient = new WebClient();
            webClient.DownloadStringCompleted += new DownloadStringCompletedEventHandler(webClient_DownloadStringCompleted);
            webClient.DownloadStringAsync(new Uri("http://api.dotaprj.me/v2/jd/"));
        }

        // Displaying the upcoming matches sorted by date
        private void webClient_DownloadStringCompleted(object sender, DownloadStringCompletedEventArgs e)
        {
            // Check if the download finished correctly
            if (e.Error != null)
            {
                MessageBox.Show("Can't access the data right now. Try again later.");
                return;
            }
            else
            {

                matches = JsonConvert.DeserializeObject<RootObject>(e.Result);
                loadTxtBlock.Visibility = System.Windows.Visibility.Collapsed;
                loadTxtBlock2.Visibility = System.Windows.Visibility.Collapsed;
                upMatches.Clear();
                endMatches.Clear();

                // Setting images
                if (matches.started != null)
                {
                    foreach (var item in matches.started)
                    {
                        //Team 1
                        item.img1 = getImgPath(item.img1tit);
                        item.img2 = getImgPath(item.img2tit);

                        // Save live matches into an Upcoming object to add it to the upcoming list)
                        upMatches.Add(new Upcoming()
                        {
                            id = item.id,
                            img1 = item.img1,
                            img1tit = item.img1tit,
                            team1 = item.team1,
                            img2 = item.img2,
                            img2tit = item.img2tit,
                            team2 = item.team2,
                            linkID = item.linkID,
                            eventName = item.eventName,
                            fullDate = item.fullDate,
                            timeStamp = item.timeStamp,
                            liveIn = item.liveIn,
                            teamWidth = item.teamWidth,
                            eventWidth = item.eventWidth,
                        });
                    }
                }


                if (matches.upcoming != null)
                {
                    foreach (var item in matches.upcoming)
                    {
                        //Team 1
                        item.img1 = getImgPath(item.img1tit);
                        item.img2 = getImgPath(item.img2tit);
                    }
                }

                if (matches.ended != null)
                {
                    foreach (var item in matches.ended)
                    {
                        //Team 1
                        item.img1 = getImgPath(item.img1tit);
                        item.img2 = getImgPath(item.img2tit);

                        // Winner color
                        if (item.score1 > item.score2)
                        {
                            item.color1 = "#FFEEE1A8";
                            item.color2 = "#FFF1F1F1";
                        }
                        else if (item.score1 < item.score2)
                        {
                            item.color1 = "#FFF1F1F1";
                            item.color2 = "#FFEEE1A8";
                        }
                        else
                        {
                            item.color1 = "#FFF1F1F1";
                            item.color2 = "#FFF1F1F1";
                        }
                    }
                }

                // Merge live and upcoming matches
                if (matches.upcoming != null)
                {
                    upMatches.AddRange(matches.upcoming);
                }
                if (matches.ended != null)
                {
                    endMatches.AddRange(matches.ended);
                }

                // Set the size of the textblocks
                if (Orientation == PageOrientation.LandscapeRight || Orientation == PageOrientation.LandscapeLeft)
                {
                    setLandscapeProperties();
                }
                else
                {
                    setPortraitProperties();
                }

                ListUpMatches.ItemsSource = upMatches;
                ListEndMatches.ItemsSource = endMatches;
            }
        }

        // Returning the right image
        private string getImgPath(string o)
        {
            string s;
            switch (o)
            {
                case "USA": s = "img/us.jpg";
                    break;
                case "Sweden": s = "img/se.jpg";
                    break;
                case "China": s = "img/cn.jpg";
                    break;
                case "Europe": s = "img/eu.jpg";
                    break;
                case "Ukraine": s = "img/ua.jpg";
                    break;
                case "World": s = "img/world.jpg";
                    break;
                case "Singapore": s = "img/sg.jpg";
                    break;
                case "Philippines": s = "img/ph.jpg";
                    break;
                case "Thailand": s = "img/th.jpg";
                    break;
                case "Russia": s = "img/ru.jpg";
                    break;
                case "Belarus": s = "img/by.jpg";
                    break;
                case "Denmark": s = "img/dk.jpg";
                    break;
                case "Australia": s = "img/au.jpg";
                    break;
                case "Korea": s = "img/kr.jpg";
                    break;
                case "Malaysia": s = "img/my.jpg";
                    break;
                case "Vietnam": s = "img/vn.jpg";
                    break;
                case "Bhutan": s = "img/bt.jpg";
                    break;
                case "Brazil": s = "img/br.jpg";
                    break;
                case "Peru": s = "img/pe.jpg";
                    break;
                default: s = "img/world.jpg";
                    break;
            }

            return s;
        }

        private void PhoneApplicationPage_OrientationChanged(object sender, OrientationChangedEventArgs e)
        {
            // Portrait
            if ((e.Orientation & PageOrientation.Portrait) == (PageOrientation.Portrait))
            {
                setPortraitProperties();
            }
            // Landscape
            else
            {
                setLandscapeProperties();
            }
            ListUpMatches.ItemsSource = null;
            ListEndMatches.ItemsSource = null;
            ListUpMatches.ItemsSource = upMatches;
            ListEndMatches.ItemsSource = endMatches;
        }

        private void setLandscapeProperties()
        {
            foreach (var item in upMatches)
            {
                item.eventWidth = 390;
                item.teamWidth = 172;
            }
            foreach (var item in endMatches)
            {
                item.eventWidth = 390;
                item.teamWidth = 188;
            }
        }

        private void setPortraitProperties()
        {
            foreach (var item in upMatches)
            {
                item.eventWidth = 205;
                item.teamWidth = 80;
            }
            foreach (var item in endMatches)
            {
                item.eventWidth = 205;
                item.teamWidth = 95;
            }
        }

        // Load again the data
        private void reloadData(object sender, EventArgs e)
        {
            this.loadJSON();
            ListUpMatches.ItemsSource = null;
            ListEndMatches.ItemsSource = null;
            loadTxtBlock.Visibility = System.Windows.Visibility.Visible;
            loadTxtBlock2.Visibility = System.Windows.Visibility.Visible;
        }
        
    }
}