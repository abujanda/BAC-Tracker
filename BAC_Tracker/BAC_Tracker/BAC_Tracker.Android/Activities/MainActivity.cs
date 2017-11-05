﻿using System;
using System.Collections.Generic;
using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Toolbar = Android.Widget.Toolbar;
using Android.OS;
using Android.Support.V7.Widget;
using com.refractored.fab;
using Microsoft.WindowsAzure.MobileServices;
using BAC_Tracker.Droid.Classes;

namespace BAC_Tracker.Droid
{
	[Activity (Label = "BAC_Tracker.Android", MainLauncher = true, Icon = "@drawable/icon")]
	public class MainActivity : Activity
    {
        string[] mData;
        RecyclerView mRecyclerView;
        RecyclerViewAdapter mAdapter;
        RecyclerView.LayoutManager mLayoutManager;
        FloatingActionButton mFAB;


        public static MobileServiceClient MobileService = new MobileServiceClient("https://bac-tracker.azurewebsites.net");
        private IMobileServiceTable<AlcoholTest> alcoholTable;
        List<AlcoholTest> myBooze;

        protected override void OnCreate(Bundle bundle)
        {
            #region Other Stuff
            base.OnCreate(bundle);

            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.Main);

            //Set our toolbar
            var mToolbar = FindViewById<Toolbar>(Resource.Id.toolbar);
            SetActionBar(mToolbar);
            ActionBar.Title = "BAC Tracker";

            mData = new string[] { "Item 1", "Item 2", "Item 3", "Item 4", "Item 5", "Item 6", "Item 7", "Item 8", "Item 9", "Item 10" };

            mRecyclerView = FindViewById<RecyclerView>(Resource.Id.recyclerView);
            mLayoutManager = new LinearLayoutManager(this);
            mRecyclerView.SetLayoutManager(mLayoutManager);

            mAdapter = new RecyclerViewAdapter(mData);
            mRecyclerView.SetAdapter(mAdapter);
            mAdapter.ItemClick += OnItemClick;

            mFAB = FindViewById<FloatingActionButton>(Resource.Id.fab);
            mFAB.AttachToRecyclerView(mRecyclerView);
            mFAB.Click += (sender, args) =>
            {
                Toast.MakeText(this, "FAB Clicked", ToastLength.Short).Show();

                string output = "Items in " + alcoholTable.TableName + ":\n";
                foreach(AlcoholTest item in myBooze)
                {
                    output += item.Name + "| Volume: " + item.Volume + " | Finished: " + item.Finished + "\n";
                }
                FindViewById<TextView>(Resource.Id.textView1).Text = output;
            };
            #endregion

            CurrentPlatform.Init();
            myBooze = new List<AlcoholTest>();
            try
            {
                GetTable();
            }
            catch(Exception e)
            {
                FindViewById<TextView>(Resource.Id.textView1).Text = e.Message;
            }
            AddAlcohol(new AlcoholTest("Test Booze", 1.2f, false));
        }

        public async void GetTable()
        {
            try
            {
                alcoholTable = MobileService.GetTable<AlcoholTest>();
            }
            catch(Exception e)
            {
                FindViewById<TextView>(Resource.Id.textView1).Text = e.Message;
            }
            myBooze = await alcoholTable.ToListAsync();
        }

        public async void AddAlcohol(AlcoholTest item)
        {
            if (MobileService == null)
                FindViewById<TextView>(Resource.Id.textView1).Text = "The fuck";
            //return;

            try
            {
                await alcoholTable.InsertAsync(item);
            }
            catch (Exception e)
            {
                FindViewById<TextView>(Resource.Id.textView1).Text = e.Message;
            }
        }




        protected override void OnSaveInstanceState(Bundle outState)
        {
            base.OnSaveInstanceState(outState);
        }

        public override bool OnCreateOptionsMenu(IMenu menu)
        {
            MenuInflater.Inflate(Resource.Menu.top_menus, menu);
            return base.OnCreateOptionsMenu(menu);
        }

        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            switch(item.ItemId){
                case Resource.Id.menu_settings:
                    StartActivity(typeof(SettingsActivity));
                    return true;
                default:
                    return base.OnOptionsItemSelected(item);
            }
        }

        void OnItemClick(object sender, int position)
        {
            int itemNum = position + 1;
            Toast.MakeText(this, "This is item " + itemNum, ToastLength.Short).Show();
        }
    }
}


