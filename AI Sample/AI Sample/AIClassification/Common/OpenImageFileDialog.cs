//using System;
//using System.Windows.Forms;

//namespace AI_Sample.AIClassification.Common
//{
//    internal class OpenImageFileDialog
//    {
//        internal void OpenImage(Action<string> onImageSelected)
//        {
//            using (OpenFileDialog ofd = new OpenFileDialog())
//            {
//                ofd.Filter = "Image files(*.jpg, *.jpeg, *.jpe, *.jfif, *.png) | *.jpg; *.jpeg; *.jpe; *.jfif; *.png";
//                if (ofd.ShowDialog() == DialogResult.OK)
//                {
//                    onImageSelected(ofd.FileName);
//                }
//            }
//        }
//    }
//}