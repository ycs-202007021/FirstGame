﻿using PlasticGui.WorkspaceWindow;
using Unity.PlasticSCM.Editor.UI.StatusBar;

namespace Unity.PlasticSCM.Editor.Developer
{
    internal class IncomingChangesNotifier :
        IIncomingChangesNotifier,
        CheckIncomingChanges.IUpdateIncomingChanges
    {
        bool IIncomingChangesNotifier.HasNotification
        {
            get { return mHasNotification; }
        }

        IncomingChangesNotification IIncomingChangesNotifier.Notification
        {
            get { return mNotification; }
        }

        internal IncomingChangesNotifier(
            PlasticWindow plasticWindow)
        {
            mPlasticWindow = plasticWindow;
        }

        void CheckIncomingChanges.IUpdateIncomingChanges.Hide()
        {
            PlasticPlugin.SetNotificationStatus(
                mPlasticWindow,
                PlasticNotification.Status.None);

            mNotification.Clear();

            mHasNotification = false;

            mPlasticWindow.Repaint();
        }

        void CheckIncomingChanges.IUpdateIncomingChanges.Show(
            string infoText,
            string actionText,
            string tooltipText,
            CheckIncomingChanges.Severity severity,
            CheckIncomingChanges.Action action)
        {
            PlasticNotification.Status status = PlasticNotification.Status.None;
            if (severity == CheckIncomingChanges.Severity.Info)
                status = PlasticNotification.Status.IncomingChanges;
            else if (severity == CheckIncomingChanges.Severity.Warning)
                status = PlasticNotification.Status.Conflicts;

            PlasticPlugin.SetNotificationStatus(
                mPlasticWindow, 
                status);

            UpdateData(
                mNotification,
                infoText,
                actionText,
                tooltipText,
                status,
                action);

            mHasNotification = true;

            mPlasticWindow.Repaint();
        }

        static void UpdateData(
            IncomingChangesNotification data,
            string infoText,
            string actionText,
            string tooltipText,
            PlasticNotification.Status status,
            CheckIncomingChanges.Action action)
        {
            data.InfoText = infoText;
            data.ActionText = actionText;
            data.TooltipText = tooltipText;
            data.HasUpdateAction = action == CheckIncomingChanges.Action.Update;
            data.Status = status;
        }

        bool mHasNotification;
        IncomingChangesNotification mNotification = new IncomingChangesNotification();

        PlasticWindow mPlasticWindow;
    }
}