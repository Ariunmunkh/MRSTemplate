using DevExpress.Web;
using DevExpress.Web.ASPxHtmlEditor;
using DevExpress.Web.Mvc;
using DXWebMRCS.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Web;

namespace DXWebMRCS
{
    public class HtmlEditorFeaturesDemosHelper
    {
        public const string ImagesDirectory = "~/Content/elearn/";
        public const string ThumbnailsDirectory = "~/Content/elearn/";
        public const string UploadDirectory = "~/Content/elearn/";
        public const string HtmlLocation = "~/Content/elearn/";

        public static readonly UploadControlValidationSettings ImageUploadValidationSettings = new UploadControlValidationSettings {
            AllowedFileExtensions = new string[] { ".jpg", ".jpeg", ".jpe", ".gif", ".png" },
            MaxFileSize = 4000000
        };

        static HtmlEditorFileSaveSettings fileSaveSettings;
        public static HtmlEditorFileSaveSettings FileSaveSettings {
            get {
                if(fileSaveSettings == null) {
                    fileSaveSettings = new HtmlEditorFileSaveSettings();
                    fileSaveSettings.FileSystemSettings.UploadFolder = ImagesDirectory + "Upload/";
                }
                return fileSaveSettings;
            }
        }

        static MVCxHtmlEditorImageSelectorSettings imageSelectorSettings;
        public static HtmlEditorImageSelectorSettings ImageSelectorSettings {
            get {
                if(imageSelectorSettings == null) {
                    imageSelectorSettings = new MVCxHtmlEditorImageSelectorSettings();
                    SetHtmlEditorImageSelectorSettings(imageSelectorSettings);
                }
                return imageSelectorSettings;
            }
        }
        public static MVCxHtmlEditorImageSelectorSettings SetHtmlEditorImageSelectorSettings(MVCxHtmlEditorImageSelectorSettings settingsImageSelector) {
            settingsImageSelector.UploadCallbackRouteValues = new { Controller = "Elearn", Action = "InsertMediaPartialImageSelectorUpload" };

            settingsImageSelector.Enabled = true;
            settingsImageSelector.CommonSettings.RootFolder = ImagesDirectory;
            settingsImageSelector.CommonSettings.ThumbnailFolder = ThumbnailsDirectory;
            settingsImageSelector.CommonSettings.AllowedFileExtensions = new string[] { ".jpg", ".jpeg", ".jpe", ".gif", ".png" };
            settingsImageSelector.EditingSettings.AllowCreate = true;
            settingsImageSelector.EditingSettings.AllowDelete = true;
            settingsImageSelector.EditingSettings.AllowMove = true;
            settingsImageSelector.EditingSettings.AllowRename = true;
            settingsImageSelector.UploadSettings.Enabled = true;
            settingsImageSelector.FoldersSettings.ShowLockedFolderIcons = true;

            settingsImageSelector.PermissionSettings.AccessRules.Add(
                new FileManagerFolderAccessRule {
                    Path = "",
                    Upload = Rights.Deny
                });
            return settingsImageSelector;
        }

        public static string GeHtmlContentByFileName(string fileName) {
            return System.IO.File.ReadAllText(System.Web.HttpContext.Current.Request.MapPath(string.Format("{0}{1}", HtmlLocation, fileName)));
        }
        public static string GeHtmlContentByFileName(string fileName, bool demoPageIsInRoot) {
            string result = GeHtmlContentByFileName(fileName);
            return demoPageIsInRoot ? result : result.Replace("Content/", "../Content/");
        }

        public static void SetupGlobalUploadBehaviour(HtmlEditorSettings settings)
        {
            settings.SettingsDialogs.InsertImageDialog.SettingsImageUpload.FileSystemSettings.Assign(HtmlEditorFeaturesDemosHelper.FileSaveSettings.FileSystemSettings);
            settings.SettingsDialogs.InsertImageDialog.SettingsImageUpload.ValidationSettings.Assign(HtmlEditorFeaturesDemosHelper.ImageUploadValidationSettings);

            long maxFileSize = 5 * 1024 * 1024;
            settings.SettingsDialogs.InsertImageDialog.SettingsImageUpload.ValidationSettings.MaxFileSize = maxFileSize;
            settings.SettingsDialogs.InsertImageDialog.SettingsImageSelector.UploadSettings.ValidationSettings.MaxFileSize = maxFileSize;
            settings.SettingsDialogs.InsertAudioDialog.SettingsAudioUpload.ValidationSettings.MaxFileSize = maxFileSize;
            settings.SettingsDialogs.InsertAudioDialog.SettingsAudioSelector.UploadSettings.ValidationSettings.MaxFileSize = maxFileSize;
            settings.SettingsDialogs.InsertFlashDialog.SettingsFlashUpload.ValidationSettings.MaxFileSize = maxFileSize;
            settings.SettingsDialogs.InsertFlashDialog.SettingsFlashSelector.UploadSettings.ValidationSettings.MaxFileSize = maxFileSize;
            settings.SettingsDialogs.InsertVideoDialog.SettingsVideoUpload.ValidationSettings.MaxFileSize = maxFileSize;
            settings.SettingsDialogs.InsertVideoDialog.SettingsVideoSelector.UploadSettings.ValidationSettings.MaxFileSize = maxFileSize;
            settings.SettingsDialogs.InsertLinkDialog.SettingsDocumentSelector.UploadSettings.ValidationSettings.MaxFileSize = maxFileSize;
            settings.Init = (s, e) =>
            {
                var htmlEditor = s as DevExpress.Web.ASPxHtmlEditor.ASPxHtmlEditor;
                htmlEditor.ImageFileSaving += OnImageFileSaving;
                htmlEditor.AudioFileSaving += OnAudioFileSaving;
                htmlEditor.FlashFileSaving += OnFlashFileSaving;
                htmlEditor.VideoFileSaving += OnVideoFileSaving;

                htmlEditor.AudioSelectorFileUploading += OnFileSelectorAction;
                htmlEditor.AudioSelectorItemRenaming += OnFileSelectorAction;
                htmlEditor.AudioSelectorItemMoving += OnFileSelectorAction;
                htmlEditor.AudioSelectorItemDeleting += OnFileSelectorAction;
                htmlEditor.AudioSelectorItemCopying += OnFileSelectorAction;

                htmlEditor.FlashSelectorFileUploading += OnFileSelectorAction;
                htmlEditor.FlashSelectorItemRenaming += OnFileSelectorAction;
                htmlEditor.FlashSelectorItemMoving += OnFileSelectorAction;
                htmlEditor.FlashSelectorItemDeleting += OnFileSelectorAction;
                htmlEditor.FlashSelectorItemCopying += OnFileSelectorAction;

                htmlEditor.ImageSelectorFileUploading += OnFileSelectorAction;
                htmlEditor.ImageSelectorItemRenaming += OnFileSelectorAction;
                htmlEditor.ImageSelectorItemMoving += OnFileSelectorAction;
                htmlEditor.ImageSelectorItemDeleting += OnFileSelectorAction;
                htmlEditor.ImageSelectorItemCopying += OnFileSelectorAction;

                htmlEditor.VideoSelectorFileUploading += OnFileSelectorAction;
                htmlEditor.VideoSelectorItemRenaming += OnFileSelectorAction;
                htmlEditor.VideoSelectorItemMoving += OnFileSelectorAction;
                htmlEditor.VideoSelectorItemDeleting += OnFileSelectorAction;
                htmlEditor.VideoSelectorItemCopying += OnFileSelectorAction;

                htmlEditor.DocumentSelectorFileUploading += OnFileSelectorAction;
                htmlEditor.DocumentSelectorItemRenaming += OnFileSelectorAction;
                htmlEditor.DocumentSelectorItemMoving += OnFileSelectorAction;
                htmlEditor.DocumentSelectorItemDeleting += OnFileSelectorAction;
                htmlEditor.DocumentSelectorItemCopying += OnFileSelectorAction;
            };
        }

        static bool? _isSiteMode;
        public static bool IsSiteMode
        {
            get
            {
                if (!_isSiteMode.HasValue)
                {
                    _isSiteMode = ConfigurationManager.AppSettings["SiteMode"].Equals("true", StringComparison.InvariantCultureIgnoreCase);
                }
                return _isSiteMode.Value;
            }
        }
        public static string GetReadOnlyMessageText()
        {
            return String.Format(
                "Data modifications are not allowed in the online demo.\n" +
                "If you need to test data editing functionality, please install \n" +
                "{0} on your machine and run the demo locally.");
        }
        static void OnFileSelectorAction(object source, FileManagerActionEventArgsBase e)
        {
            e.Cancel = IsSiteMode;
            e.ErrorText = GetReadOnlyMessageText();
        }
        public static void OnImageFileSaving(object s, FileSavingEventArgs e)
        {
            PrepareFileToDelayedRemove(s as DevExpress.Web.ASPxHtmlEditor.ASPxHtmlEditor, he => he.SettingsDialogs.InsertImageDialog.SettingsImageUpload, e);
        }
        static void OnAudioFileSaving(object s, FileSavingEventArgs e)
        {
            PrepareFileToDelayedRemove(s as DevExpress.Web.ASPxHtmlEditor.ASPxHtmlEditor, he => he.SettingsDialogs.InsertAudioDialog.SettingsAudioUpload, e);
        }
        static void OnFlashFileSaving(object s, FileSavingEventArgs e)
        {
            PrepareFileToDelayedRemove(s as DevExpress.Web.ASPxHtmlEditor.ASPxHtmlEditor, he => he.SettingsDialogs.InsertFlashDialog.SettingsFlashUpload, e);
        }
        static void OnVideoFileSaving(object s, FileSavingEventArgs e)
        {
            PrepareFileToDelayedRemove(s as DevExpress.Web.ASPxHtmlEditor.ASPxHtmlEditor, he => he.SettingsDialogs.InsertVideoDialog.SettingsVideoUpload, e);
        }

        static void PrepareFileToDelayedRemove(DevExpress.Web.ASPxHtmlEditor.ASPxHtmlEditor htmlEditor,
            Func<DevExpress.Web.ASPxHtmlEditor.ASPxHtmlEditor, ASPxHtmlEditorUploadSettingsBase> uploadSettingsGetter,
            FileSavingEventArgs e)
        {

            e.FileName = string.Format("DEMOTMPFILE_{0}{1}", Guid.NewGuid(), Path.GetExtension(e.FileName));
            string filePath = Path.Combine(htmlEditor.Page.MapPath(uploadSettingsGetter(htmlEditor).UploadFolder), e.FileName);
        }
    }
    }
