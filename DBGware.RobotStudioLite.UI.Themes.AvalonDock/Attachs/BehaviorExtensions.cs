﻿using Microsoft.Xaml.Behaviors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace DBGware.RobotStudioLite.UI.Themes.AvalonDock.Attachs
{
    internal static class BehaviorExtensions
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T? GetBehavior<T>(this BehaviorCollection behaviors) where T : Behavior
        => (T?)behaviors.FirstOrDefault(a => a.GetType() == typeof(T));

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T GetOrAddBehavior<T>(this DependencyObject dependencyObject, Func<T> defaultValue) where T : Behavior
        => GetOrAddBehavior<T>(Interaction.GetBehaviors(dependencyObject), defaultValue);

        public static T GetOrAddBehavior<T>(this BehaviorCollection behaviors, Func<T> defaultValue) where T : Behavior
        {
            var behavior = GetBehavior<T>(behaviors);
            if (behavior is null)
            {
                behavior = defaultValue();
                behaviors.Add(behavior);
            }

            return behavior;
        }
    }
}