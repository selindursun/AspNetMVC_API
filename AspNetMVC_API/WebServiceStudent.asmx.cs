using AspNetMVC_API_BLL.Repository;
using AspNetMVC_API_Entity.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Services;

namespace AspNetMVC_API
{
    /// <summary>
    /// Summary description for WebServiceStudent
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    // [System.Web.Script.Services.ScriptService]
    public class WebServiceStudent : System.Web.Services.WebService
    {

        [WebMethod]
        public string HelloWorld()
        {
            return "Hello World";
        }
        //Global alan
        StudentRepo myStudentRepo = new StudentRepo();
        private bool IsAuthenticated
        {
            get
            {
                bool result = false;
                try
                {
                    string authorization = "";
                    authorization = HttpContext.Current.Request.Headers["Authorization"];
                    if (authorization!=null)
                    {
                        authorization=authorization.Replace("Basic","");
                        byte[] byteArray = Convert.FromBase64String(authorization);
                        string usernamepassword = System.Text.Encoding.UTF8.GetString(byteArray);
                        bool usernameResult = usernamepassword.Split(':').First().Equals(ConfigurationManager.AppSettings["USERNAME"].ToString());

                        bool passwordResult = usernamepassword.Split(':').Last().Equals(ConfigurationManager.AppSettings["PASSWORD"].ToString());
                        //username ve parola doğru geldiyse kontrol etsin result'a true atasın.Doğru değilse false atasın
                        result = (usernameResult && passwordResult) ? true : false;
                    }
                    return result;
                }
                catch (Exception)
                {
                    result = false;
                    return result;
                }
            }
        }
        private void CheckCredentials()
        {
            if (!IsAuthenticated)
            {
                throw new Exception("Kullanıcı adı veya şifre hatalıdır.Tekrar deneyiniz!");
            }
        }
        [WebMethod]
        public List<Student> GetAll()
        {
            try
            {
                CheckCredentials();
                List<Student> list = myStudentRepo.GetAll();
                return list;
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        [WebMethod]
        public string Insert(string name, string surname)
        {
            try
            {
                CheckCredentials();
                if (string.IsNullOrEmpty(name) || string.IsNullOrEmpty(surname))
                {
                    throw new Exception("name surname alanları boş geçilemez...");
                }
                Student newStudent = new Student()
                {
                    Name = name,
                    Surname = surname
                };
                int insertResult = myStudentRepo.Insert(newStudent);
                if (insertResult > 0)
                {
                    //1. yöntem
                    return "Kayıt eklendi.id=" + newStudent.Id;
                    //2.yöntem
                }
                else
                {
                    throw new Exception("Kayıt ekleme işleminde beklenmedik bir hata oluştu!");
                }
            }
            catch (Exception ex)
            {

                return ex.Message;
            }
        }
        [WebMethod]
        public string Delete(int id)
        {
            try
            {
                CheckCredentials();
                if (id > 0)
                {
                    Student student = myStudentRepo.GetById(id);
                    if (student == null)
                    {
                        throw new Exception("öğrenci silme işlemi başarısızdır");
                    }
                    int deleteResult = myStudentRepo.Delete(student);
                    if (deleteResult > 0)
                    {
                        return "Kayıt silme başarılı";
                    }
                    else
                    {
                        throw new Exception("beklenmedik bir hata oluştu");
                    }
                }
                else
                {
                    throw new Exception("gönderilen id değeri sıfırdan büyük olmalıdır.");
                }
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }
        [WebMethod]
        public string Update(int currentid, string newname, string newsurname)
        {
            try
            {
                CheckCredentials();
                if (currentid <= 0)
                {
                    throw new Exception("Gönderilen id değeri sıfırdan büyük olmalıdır!");
                }
                if (string.IsNullOrEmpty(newname) && string.IsNullOrEmpty(newsurname))
                {
                    throw new Exception("Gönderilen verilerden newname ve newsurname boş geçilemez!");
                }
                Student currentStudent = myStudentRepo.GetById(currentid);
                if (currentStudent == null)
                {
                    throw new Exception("Öğrenci bulunamadığı için güncelleme işlemi başarısızdır!");
                }

                //eğer newname parametresi dolu ise isim güncellenecek
                if (!string.IsNullOrEmpty(newname))
                {
                    currentStudent.Name = newname;
                }

                //eğer newsurname parametresi dolu ise soyisim güncellenecek
                if (!string.IsNullOrEmpty(newsurname))
                {
                    currentStudent.Surname = newsurname;
                }

                int updateResult = myStudentRepo.Update();
                if (updateResult > 0)
                {
                    return "Kayıt başarılı bir şekilde güncellendi.";
                }
                else
                {
                    throw new Exception("Beklenmedik bir hata nedeniyle kayıt günceleme başarısız!");
                }
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }
    }
}
