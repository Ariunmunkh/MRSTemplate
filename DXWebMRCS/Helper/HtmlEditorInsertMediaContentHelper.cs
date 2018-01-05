using DevExpress.Web;
using DevExpress.Web.ASPxHtmlEditor;
using DevExpress.Web.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DXWebMRCS
{
    public class HtmlEditorInsertMediaContentHelper
    {
        public const string FlashFilesDirectory = "~/Content/elearn/";
        public const string VideoFilesDirectory = "~/Content/elearn/";
        public const string AudioFilesDirectory = "~/Content/elearn/";

        public static readonly UploadControlValidationSettings FlashUploadValidationSettings = new UploadControlValidationSettings
        {
            AllowedFileExtensions = new string[] { ".html" },
            MaxFileSize = 5000000
        };
        public static readonly UploadControlValidationSettings VideoUploadValidationSettings = new UploadControlValidationSettings
        {
            AllowedFileExtensions = new string[] { ".mp4", ".ogg" },
            MaxFileSize = 10000000
        };
        public static readonly UploadControlValidationSettings AudioUploadValidationSettings = new UploadControlValidationSettings
        {
            AllowedFileExtensions = new string[] { ".mp3", ".ogg" },
            MaxFileSize = 5000000
        };

        

        static MVCxHtmlEditorFlashSelectorSettings flashSelectorSettings;
        public static HtmlEditorFlashSelectorSettings FlashSelectorSettings
        {
            get
            {
                if (flashSelectorSettings == null)
                {
                    flashSelectorSettings = new DevExpress.Web.Mvc.MVCxHtmlEditorFlashSelectorSettings(null);
                    flashSelectorSettings.Enabled = true;
                    flashSelectorSettings.UploadCallbackRouteValues = new { Controller = "Elearn", Action = "InsertMediaPartialFlashSelectorUpload" };
                    flashSelectorSettings.CommonSettings.RootFolder = FlashFilesDirectory;
                    flashSelectorSettings.CommonSettings.ThumbnailFolder = FlashFilesDirectory;
                    flashSelectorSettings.CommonSettings.AllowedFileExtensions = new string[] { ".html" };
                    flashSelectorSettings.UploadSettings.Enabled = true;
                }
                return flashSelectorSettings;
            }
            //get
            //{
            //    if (flashSelectorSettings == null)
            //    {
            //        flashSelectorSettings = new MVCxHtmlEditorFlashSelectorSettings();
            //        SetHtmlEditorFlashSelectorSettings(flashSelectorSettings);
            //    }
            //    return flashSelectorSettings;
            //}
        }
        public static MVCxHtmlEditorFlashSelectorSettings SetHtmlEditorFlashSelectorSettings(MVCxHtmlEditorFlashSelectorSettings settingsFlashSelector)
        {
            settingsFlashSelector.Enabled = true;
            settingsFlashSelector.CommonSettings.RootFolder = FlashFilesDirectory;
            settingsFlashSelector.CommonSettings.ThumbnailFolder = HtmlEditorFeaturesDemosHelper.ThumbnailsDirectory;
            settingsFlashSelector.CommonSettings.AllowedFileExtensions = new string[] { ".html" };
            return settingsFlashSelector;
        }

        static MVCxHtmlEditorVideoSelectorSettings videoSelectorSettings;
        public static HtmlEditorVideoSelectorSettings VideoSelectorSettings
        {
            get
            {
                if (videoSelectorSettings == null)
                {
                    videoSelectorSettings = new MVCxHtmlEditorVideoSelectorSettings();
                    SetHtmlEditorVideoSelectorSettings(videoSelectorSettings);
                }
                return videoSelectorSettings;
            }
        }
        public static MVCxHtmlEditorVideoSelectorSettings SetHtmlEditorVideoSelectorSettings(MVCxHtmlEditorVideoSelectorSettings settingsVideoSelector)
        {
            settingsVideoSelector.Enabled = true;
            settingsVideoSelector.CommonSettings.RootFolder = VideoFilesDirectory;
            settingsVideoSelector.CommonSettings.ThumbnailFolder = HtmlEditorFeaturesDemosHelper.ThumbnailsDirectory;
            settingsVideoSelector.CommonSettings.AllowedFileExtensions = new string[] { ".mp4", ".ogg" };
            return settingsVideoSelector;
        }

        static MVCxHtmlEditorAudioSelectorSettings audioSelectorSettings;
        public static HtmlEditorAudioSelectorSettings AudioSelectorSettings
        {
            get
            {
                if (audioSelectorSettings == null)
                {
                    audioSelectorSettings = new MVCxHtmlEditorAudioSelectorSettings();
                    SetHtmlEditorAudioSelectorSettings(audioSelectorSettings);
                }
                return audioSelectorSettings;
            }
        }
        public static MVCxHtmlEditorAudioSelectorSettings SetHtmlEditorAudioSelectorSettings(MVCxHtmlEditorAudioSelectorSettings settingsAudioSelector)
        {
            settingsAudioSelector.Enabled = true;
            settingsAudioSelector.CommonSettings.RootFolder = AudioFilesDirectory;
            settingsAudioSelector.CommonSettings.ThumbnailFolder = HtmlEditorFeaturesDemosHelper.ThumbnailsDirectory;
            settingsAudioSelector.CommonSettings.AllowedFileExtensions = new string[] { ".mp3", ".ogg" };
            return settingsAudioSelector;
        }

    }
}
