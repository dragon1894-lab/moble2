using System;
using System.Linq;
using System.Windows.Forms;

namespace DJing
{
    // 로그인 이후 화면에서 공통으로 사용하는 뒤로가기와 로그아웃 기능
    public static class AppNavigation
    {
        // 이전 화면이 있으면 돌아가고, 없으면 로그인 화면으로 이동
        public static void GoBack(Form currentForm)
        {
            Form? previousForm = currentForm.Owner;

            if (previousForm != null && !previousForm.IsDisposed)
            {
                previousForm.Show();
                previousForm.BringToFront();
                previousForm.Activate();
                currentForm.Close();
                return;
            }

            Choice? choiceForm = Application.OpenForms.OfType<Choice>().FirstOrDefault();

            if (choiceForm != null && !choiceForm.IsDisposed)
            {
                choiceForm.Show();
                choiceForm.BringToFront();
                choiceForm.Activate();
                currentForm.Close();
                return;
            }

            Logout(currentForm);
        }

        // 로그인 정보를 지우고 열려 있는 로그인 이후 화면을 닫은 다음 로그인 창으로 이동
        public static void Logout(Form currentForm)
        {
            UserSession.UserId = "";
            UserSession.IsGuest = false;

            Login? loginForm = Application.OpenForms.OfType<Login>().FirstOrDefault();

            // Choice와 soundcloud가 닫힐 때 프로그램 전체가 종료되지 않도록 알림
            foreach (Choice choice in Application.OpenForms.OfType<Choice>().ToArray())
            {
                choice.PrepareForNavigation();
            }

            foreach (soundcloud cloud in Application.OpenForms.OfType<soundcloud>().ToArray())
            {
                cloud.PrepareForNavigation();
            }

            // 로그인 창을 제외한 나머지 화면을 모두 닫음
            Form[] openedForms = Application.OpenForms.Cast<Form>().ToArray();
            foreach (Form form in openedForms)
            {
                if (form != loginForm && !form.IsDisposed)
                {
                    form.Close();
                }
            }

            if (loginForm == null || loginForm.IsDisposed)
            {
                loginForm = new Login();
            }

            loginForm.ResetForLogout();
            loginForm.Show();
            loginForm.BringToFront();
            loginForm.Activate();
        }
    }
}

