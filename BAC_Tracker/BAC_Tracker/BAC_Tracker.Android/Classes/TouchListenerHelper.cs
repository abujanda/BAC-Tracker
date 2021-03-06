﻿using System;
using Android.Views;
using Android.Support.V4.View;
using Android.Support.V7.Widget;
using BAC_Tracker.Droid.Interfaces;

namespace BAC_Tracker.Droid.Classes
{
    public class TouchListenerHelper : Java.Lang.Object, View.IOnTouchListener
    {
        private RecyclerView.ViewHolder _itemHolder;
        private IOnStartDragListener _mDragStartListener;

        public TouchListenerHelper(RecyclerView.ViewHolder holder, IOnStartDragListener mDragStartListener)
        {
            _itemHolder = holder;
            _mDragStartListener = mDragStartListener;
        }

        public bool OnTouch(View v, MotionEvent e)
        {
            if (e.Action == MotionEventActions.Down)
            {
                _mDragStartListener.OnStartDrag(_itemHolder);
            }
            return false;
        }
    }
}